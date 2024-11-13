using System;

namespace ET
{

    //服务器之间通用通信协议
    [ActorMessageHandler]
    public class A2A_ServerMessageHandler : AMActorRpcHandler<Scene, A2A_ServerMessageRequest, A2A_ServerMessageRResponse>
    {

        protected override async ETTask Run(Scene scene, A2A_ServerMessageRequest request, A2A_ServerMessageRResponse response, Action reply)
        {
            try
            {
                switch (scene.SceneType)
                {
                    case SceneType.Account:
                        if (request.MessageType == NoticeType.Archive)
                        {
                            // MessageValue = $"{acccout} {unitid} {archive}",
                            string[] sssvalue = request.MessageValue.Split(' ');
                            string account = sssvalue[0];
                            long unitid = long.Parse(sssvalue[1]);
                            int archive = int.Parse(sssvalue[2]);
                            scene.GetComponent<PlayerInfoListComponent>().OnAddArchive(account, unitid, archive);
                        }
                        break;
                    case SceneType.Team:
                        if (request.MessageType == NoticeType.PlayerExit)
                        {
                            scene.GetComponent<TeamSceneComponent>().OnRecvUnitLeave(long.Parse(request.MessageValue), true);
                        }
                        break;
                    case SceneType.Solo:
                        if (request.MessageType == NoticeType.PlayerExit)
                        {
                            scene.GetComponent<SoloSceneComponent>().OnRecvUnitLeave(long.Parse(request.MessageValue));
                        }
                        break;
                    case SceneType.Battle:
                        break;
                    case SceneType.Rank:
                        if (request.MessageType == NoticeType.RankRefresh)
                        {
                            scene.GetComponent<RankSceneComponent>().UpdateCombat().Coroutine();
                        }
                        if (request.MessageType == NoticeType.StopSever)
                        {
                            scene.GetComponent<RankSceneComponent>().SaveDB().Coroutine();
                            Log.Debug($"数据落地:  Rank: {scene.DomainZone()}");
                        }
                        break;
                    case SceneType.PaiMai:
                        if (request.MessageType == NoticeType.StopSever)
                        {
                            scene.GetComponent<PaiMaiSceneComponent>().SaveDB(0).Coroutine();
                            Log.Debug($"数据落地:  PaiMai: {scene.DomainZone()}");
                        }
                        break;
                    case SceneType.Union:
                        if (request.MessageType == NoticeType.StopSever)
                        {
                            scene.GetComponent<UnionSceneComponent>().SaveDB();
                            Log.Debug($"数据落地:  Union: {scene.DomainZone()}");
                        }
                        break;
                    case SceneType.Chat:
                        ChatSceneComponent chatInfoUnitsComponent = scene.GetComponent<ChatSceneComponent>();
                        if (request.MessageType == NoticeType.PlayerExit)
                        {
                            long unitid = long.Parse(request.MessageValue);
                            if (chatInfoUnitsComponent.Get(unitid) != null)
                            {
                                chatInfoUnitsComponent.Remove(unitid);
                            }
                        }
                        else if (request.MessageType == NoticeType.ClearChat)
                        {
                            Console.WriteLine("清空聊天:  " + scene.DomainZone());
                            chatInfoUnitsComponent.WordChatInfos.Clear();
                        }
                        else if (request.MessageType == NoticeType.JinYan)
                        {
                            BeReportedInfo bePortedNumber;
                            string[] jinyaninfo = request.MessageValue.Split(' ');
                            long JinYanId = long.Parse(jinyaninfo[2]);
                            chatInfoUnitsComponent.BeReportedNumber.TryGetValue(JinYanId, out bePortedNumber);

                            if (bePortedNumber == null)
                            {
                                bePortedNumber = new BeReportedInfo();
                                chatInfoUnitsComponent.BeReportedNumber.Add(JinYanId, bePortedNumber);
                            }
                            bePortedNumber.JinYanTime = TimeHelper.ServerNow() + TimeHelper.OneDay * 3;
                        }
                        else if (request.MessageType == NoticeType.PaiMai)
                        {
                            M2C_SyncChatInfo m2C_SyncChatInfo = new M2C_SyncChatInfo();
                            m2C_SyncChatInfo.ChatInfo = new ChatInfo();
                            m2C_SyncChatInfo.ChatInfo.ChannelId = (int)ChannelEnum.PaiMai;
                            m2C_SyncChatInfo.ChatInfo.ChatMsg = request.MessageValue;
                            m2C_SyncChatInfo.ChatInfo.Time = TimeHelper.ServerNow();
                            foreach (var otherUnit in chatInfoUnitsComponent.ChatInfoUnitsDict.Values)
                            {
                                MessageHelper.SendActor(otherUnit.GateSessionActorId, m2C_SyncChatInfo);
                            }
                        }
                        else
                        {
                            M2C_HorseNoticeInfo m2C_HorseNoticeInfo = new M2C_HorseNoticeInfo()
                            {
                                NoticeType = request.MessageType,
                                NoticeText = request.MessageValue
                            };
                            foreach (var otherUnit in chatInfoUnitsComponent.ChatInfoUnitsDict.Values)
                            {
                                MessageHelper.SendActor(otherUnit.GateSessionActorId, m2C_HorseNoticeInfo);
                            }
                        }
                        reply();
                        break;
                    case SceneType.AccountCenter:
                        string[] messagevalue = request.MessageValue.Split('_');
                        if (!messagevalue[1].Equals(DllHelper.Admin))
                        {
                            Log.Warning($"AccountCenter = a: {messagevalue[1]}   b: {DllHelper.Admin}");
                            reply();
                            return;
                        }
                        
                        if (messagevalue[0] == "0")
                        {
                            scene.GetComponent<FangChenMiComponent>().StopServer = true;
                            LogHelper.OnStopServer();
                            Log.Warning("StopServer = true");
                        }
                        if(messagevalue[0] == "1")
                        {
                            scene.GetComponent<FangChenMiComponent>().StopServer = false;
                            Log.Warning("StopServer = false");
                        }
                        if (messagevalue[0] == "2")
                        {
                            Game.EventSystem.Publish(new EventType.GenerateSerials() { AccountCenterScene = scene });
                        }
                        //if (yeardate == 20230412 && hour == 13 && self.DomainZone() == 3)
                        //{
                        //    //通知中心刷新序列号
                        //    LogHelper.LogWarning($"刷新序列号", true);
                        //    long centerid = DBHelper.GetAccountCenter();
                        //    A2A_ActivityUpdateResponse m2m_TrasferUnitResponse = (A2A_ActivityUpdateResponse)await ActorMessageSenderComponent.Instance.Call
                        //                 (centerid, new A2A_ActivityUpdateRequest() { ActivityType = 1 });
                        //}
                        break;
                    default:
                        break;
                }

                reply();
                await ETTask.CompletedTask;
            }
            catch (Exception e)
            { 
                Log.Error(e);
            }
        }
    }
}