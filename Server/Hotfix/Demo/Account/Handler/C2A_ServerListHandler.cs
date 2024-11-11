using System;
using System.Collections.Generic;

namespace ET
{
    //游戏服务器处理
    [MessageHandler]
    public class C2A_ServerListHandler : AMRpcHandler<C2A_ServerList, A2C_ServerList>
    {
        protected override async ETTask Run(Session session, C2A_ServerList request, A2C_ServerList response, Action reply)
        {
            try
            {
                if (session.GetComponent<SessionLockingComponent>() != null)
                {
                    response.Error = ErrorCode.ERR_RequestRepeatedly;
                    reply();
                    session.Disconnect().Coroutine();
                    return;
                }

                using (session.AddComponent<SessionLockingComponent>())
                {
                    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.GetServerList, 0))
                    {
                        long serverTime = TimeHelper.ServerNow();
                        List<ServerItem> serverItems = ServerHelper.GetServerList(ComHelp.IsInnerNet());
                        response.ServerItems.Clear();
                        for (int i = 0; i < serverItems.Count; i++)
                        {
                            //128服只有主播账号才显示。。
                            if (serverItems[i].ServerId == 128 && !GMHelp.ZhuBoURBossAccount.Contains(request.Account))
                            {
                                continue;
                            }

                            if (serverItems[i].Show != 0 && serverItems[i].ServerOpenTime <= serverTime)
                            {
                                response.ServerItems.Add(serverItems[i]);
                            }
                        }
                        response.Message = session.DomainScene().GetComponent<AccountCenterComponent>().TianQiValue.ToString();
                        string[] stringxxx = LogHelper.GetNoticeNew().Split('@');
                        response.NoticeVersion = stringxxx[0];
                        response.NoticeText = stringxxx[1];
                        int accountcenter = StartSceneConfigCategory.Instance.AccountCenterConfig.OuterPort;
                        string outeIp = StartMachineConfigCategory.Instance.Get(1).OuterIP;
                        response.AccountCenterIp = $"{outeIp}:{accountcenter}";

                        StartSceneConfig realmStartSceneConfig = RealmGateAddressHelper.GetRealm(5);
                        string real = realmStartSceneConfig.OuterIPPort.ToString();
                        StartSceneConfig gateconfig = RealmGateAddressHelper.GetGate(5, 1);
                        string gate = gateconfig.OuterIPPort.ToString();
                        response.RealAndGate = real + "_" + gate;
                        reply();
                        await ETTask.CompletedTask;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}
