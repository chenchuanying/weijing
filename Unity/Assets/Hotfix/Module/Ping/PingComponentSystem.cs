using System;

namespace ET
{
    [ObjectSystem]
    public class PingComponentAwakeSystem: AwakeSystem<PingComponent>
    {
        public override void Awake(PingComponent self)
        {
            self.DisconnectType = 0;
            PingAsync(self).Coroutine();
        }

        private static async ETTask PingAsync(PingComponent self)
        {
            Session session = self.GetParent<Session>();
            long instanceId = self.InstanceId;
            
            while (true)
            {
                if (self.InstanceId != instanceId)
                {
                    return;
                }

                long time1 = TimeHelper.ClientNow();
                try
                {
                    G2C_Ping response = await session.Call(self.C2G_Ping) as G2C_Ping;

                    if (self.InstanceId != instanceId)
                    {
                        return;
                    }

                    long time2 = TimeHelper.ClientNow();
                    self.Ping = time2 - time1;
                    
                    Game.TimeInfo.ServerMinusClientTime = response.Time + (time2 - time1) / 2 - time2;

                    await TimerComponent.Instance.WaitAsync(5000);
                }
                catch (RpcException e)
                {
                    // session断开导致ping rpc报错，记录一下即可，不需要打成error
                    Log.Info($"ping error: {self.Id} {e.Error}");
                    return;
                }
                catch (Exception e)
                {
                    Log.Error($"ping error: \n{e}");
                }
            }
        }
    }

   [ObjectSystem]
    public class PingComponentDestroySystem : DestroySystem<PingComponent>
    {
        public override void Destroy(PingComponent self)
        {
            self.Ping = default;
            //self.DisconnectType == -1 主动断开不处理}
            if (self.DisconnectType == -1)
            {
                return;
            }

            Scene zonescene = self.DomainScene();
            AttackComponent AttackComponent = zonescene.GetComponent<AttackComponent>();
            AttackComponent?.RemoveTimer();
            MapComponent mapComponent = self.DomainScene().GetComponent<MapComponent>();
            if (self.DisconnectType == 0 && mapComponent.SceneTypeEnum >= SceneTypeEnum.MainCityScene)
            {
                Log.ILog.Debug($"PingComponent: {self.DisconnectType} Destroy:BeginRelink");
                EventType.BeginRelink.Instance.ZoneScene = self.DomainScene();
                Game.EventSystem.PublishClass(EventType.BeginRelink.Instance);
            }
            else
            {
                Log.ILog.Debug($"PingComponent: {self.DisconnectType}  Destroy:ReturnLogin");
                EventType.ReturnLogin.Instance.ZoneScene = self.DomainScene();
                EventType.ReturnLogin.Instance.ErrorCode = self.DisconnectType;
                Game.EventSystem.PublishClass(EventType.ReturnLogin.Instance);
            }
        }
    }
}