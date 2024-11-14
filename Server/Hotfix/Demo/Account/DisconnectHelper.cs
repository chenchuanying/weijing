using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public static class DisconnectHelper
    {
        public static async ETTask Disconnect(this Session self)
        {
            if (self == null || self.IsDisposed)
            {
                return;
            }

            long instanceId = self.InstanceId;

            await TimerComponent.Instance.WaitAsync(1000);

            if (self.InstanceId != instanceId)
            {
                return;
            }
            self.Dispose();

        }

        public static async ETTask KickPlayer(Player player, bool isException = false)
        {
            if (player == null || player.IsDisposed)
            {
                return;
            }
            long instanceId = player.InstanceId;
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, player.AccountId.GetHashCode()))
            {
                if (player.IsDisposed || instanceId != player.InstanceId)
                {
                    return;
                }
                Log.Debug($"KickPlayerBegin playerId: {player.Id} InstanceId:{player.InstanceId}: state {player.PlayerState} {isException}");

                if (!isException)   //异常下线不会走正常下线的流程。
                {
                    switch (player.PlayerState)
                    {
                        case PlayerState.Disconnect:
                            break;
                        case PlayerState.Gate:
                            break;
                        case PlayerState.Game:
                            //通知游戏逻辑服下线Unit角色逻辑，并将数据存入数据库
                            var m2GRequestExitGame = (M2G_RequestExitGame)await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestExitGame());

                            //通知组队服
                            await ServerMessageHelper.SendServerMessage(player.TeamServerID, NoticeType.PlayerExit, player.UnitId.ToString());

                            //通知Solo服
                            await ServerMessageHelper.SendServerMessage(player.SoloServerID, NoticeType.PlayerExit, player.UnitId.ToString());

                            //通知聊天服下线聊天Unit
                            var chat2GRequestExitChat = (Chat2G_RequestExitChat)await MessageHelper.CallActor(player.ChatInfoInstanceId, new G2Chat_RequestExitChat());
                            //通知移除账号角色登录信息
                            long LoginCenterConfigSceneId = StartSceneConfigCategory.Instance.LoginCenterConfig.InstanceId;
                            var L2G_RemoveLoginRecord = (L2G_RemoveLoginRecord)await MessageHelper.CallActor(LoginCenterConfigSceneId, new G2L_RemoveLoginRecord()
                            {
                                AccountId = player.AccountId,
                                ServerId = player.DomainZone()
                            });
                            break;
                    }
                }

               
                //通知账号服
                long accountSceneId = DBHelper.GetAccountServerId(player.DomainZone());
                var a2G_ExitGame = (A2G_ExitGame)await MessageHelper.CallActor(accountSceneId, new G2A_ExitGame()
                {
                    AccountId = player.AccountId,
                });

                //通知排队服
                long queueSceneId = DBHelper.GetQueueServerId(player.DomainZone());
                var q2G_ExitGame = (Q2G_ExitGame)await MessageHelper.CallActor(queueSceneId, new G2Q_ExitGame()
                {
                    AccountId = player.AccountId,
                });

                Log.Info($"KickPlayerEnd playerId: {player.Id} InstanceId:{player.InstanceId}: {isException}");
                player.PlayerState = PlayerState.Disconnect;
                player.DomainScene().GetComponent<PlayerComponent>()?.Remove(player.AccountId);
                player?.Dispose();
                await TimerComponent.Instance.WaitAsync(300);
            }
        }
        
        public static async ETTask KickPlayer(int zone, long unitid)
        {
            Log.Info("KickPlayer_2");
            long fubencenterId = DBHelper.GetFubenCenterId(zone);
            M2F_FubenCenterListRequest request = new M2F_FubenCenterListRequest() { };
            F2M_FubenCenterListResponse response = (F2M_FubenCenterListResponse)await ActorMessageSenderComponent.Instance.Call(fubencenterId, request);

            List<long> mapIdList = new List<long>()
            {
                StartSceneConfigCategory.Instance.GetBySceneName(zone, $"Map{ComHelp.MainCityID()}").InstanceId
            };
            mapIdList.AddRange(response.FubenInstanceList);

            for (int i = mapIdList.Count - 1; i >= 0; i--)
            {
                ActorMessageSenderComponent.Instance.Send(mapIdList[i], new G2M_KickPlayerRequest() { UnitId = unitid });
            }

            await ETTask.CompletedTask;
        }

        public static async ETTask<int> ArchiveKickOutPlayer(int zone, long unitid)
        {
            M2A_KickOutPlayerResponse m2G_RechargeResponse = (M2A_KickOutPlayerResponse)await ActorLocationSenderComponent.Instance.Call(unitid, new A2M_KickOutPlayerRequest
            {
                UnitID = unitid 
            });
            if (m2G_RechargeResponse.Error == ErrorCode.ERR_Success)
            {
                Console.WriteLine("踢玩家下线成功！！");
            }
            else
            {
                Console.WriteLine($"踢玩家下线返回： {m2G_RechargeResponse.Error}！！");
            }
            return m2G_RechargeResponse.Error;
        }
    }
}
