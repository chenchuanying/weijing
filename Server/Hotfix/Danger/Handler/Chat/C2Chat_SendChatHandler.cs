using System;

namespace ET
{

    [ActorMessageHandler]
    public class C2Chat_SendChatHandler : AMActorRpcHandler<ChatInfoUnit, C2C_SendChatRequest, C2C_SendChatResponse>
    {

        protected override async ETTask Run(ChatInfoUnit chatInfoUnit, C2C_SendChatRequest request, C2C_SendChatResponse response, Action reply)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Chat, chatInfoUnit.Id))
            {
                if (string.IsNullOrEmpty(request.ChatInfo.ChatMsg))
                {
                    Log.Error($"C2Chat_SendChatHandler.1");
                    response.Error = ErrorCode.ERR_ModifyData;
                    reply();
                    return;
                }

                long serverTime = TimeHelper.ServerNow();
                if (serverTime - chatInfoUnit.LastSendChat < TimeHelper.Second * 10)
                {
                    response.Error = ErrorCode.ERR_WordChat;
                    reply();
                    return;
                }

                if (!ComHelp.IsBanHaoZone(chatInfoUnit.DomainZone()) && chatInfoUnit.Level < 20)
                {
                    response.Error = ErrorCode.ERR_LevelIsNot;
                    reply();
                    return;
                }

                chatInfoUnit.LastSendChat = serverTime;
                M2C_SyncChatInfo m2C_SyncChatInfo = new M2C_SyncChatInfo();
                request.ChatInfo.Time = TimeHelper.ServerNow();
                request.ChatInfo.PlayerName = chatInfoUnit.Name;
                m2C_SyncChatInfo.ChatInfo = request.ChatInfo;
                switch (request.ChatInfo.ChannelId)
                {
                    case (int)ChannelEnum.PaiMai:
                    case (int)ChannelEnum.Word:
                        ChatSceneComponent chatInfoUnitsComponent = chatInfoUnit.DomainScene().GetComponent<ChatSceneComponent>();

                        if (request.ChatInfo.ChannelId == ChannelEnum.Word)
                        {
                            BeReportedInfo bePortedNumber = null;
                            chatInfoUnitsComponent.BeReportedNumber.TryGetValue(request.ChatInfo.UserId, out bePortedNumber);
                            if (bePortedNumber != null && bePortedNumber.JinYanTime > TimeHelper.ServerNow())
                            {
                                long leftTime = bePortedNumber.JinYanTime - TimeHelper.ServerNow();
                                int hour = (int)(leftTime * 1f / TimeHelper.Hour);
                                hour = Math.Max(hour, 1);
                                response.Error = ErrorCode.ERR_Chat_JinYan_3;
                                response.Message = $"你当前已经被禁言，{hour}小时后解封！";
                                reply();
                                return;
                            }
                            if (bePortedNumber != null && bePortedNumber.JinYanTime != 0 && bePortedNumber.JinYanTime <= TimeHelper.ServerNow())
                            {
                                chatInfoUnitsComponent.BeReportedNumber.Remove(request.ChatInfo.UserId);
                            }

                            LogHelper.ChatInfo( $"区:{chatInfoUnit.DomainZone()}    {request.ChatInfo.PlayerName}:  {request.ChatInfo.ChatMsg} ");
                        }

                        foreach (var otherUnit in chatInfoUnitsComponent.ChatInfoUnitsDict.Values)
                        {
                            MessageHelper.SendActor(otherUnit.GateSessionActorId, m2C_SyncChatInfo);
                        }

                        if (request.ChatInfo.ChannelId == (int)ChannelEnum.Word)
                        {
                            chatInfoUnitsComponent.WordChatInfos.Add(request.ChatInfo);
                            if (chatInfoUnitsComponent.WordChatInfos.Count > 10)
                            {
                                chatInfoUnitsComponent.WordChatInfos.RemoveAt(chatInfoUnitsComponent.WordChatInfos.Count - 1);
                            }
                        }

                        //if (chatInfoUnit.DomainZone() == 5)
                        //{
                        //    bool havegm = false;
                        //    for (int i = 0; i < chatInfoUnitsComponent.WordChatInfos.Count; i++)
                        //    {
                        //        if (chatInfoUnitsComponent.WordChatInfos[i].ChatMsg.Contains("mail"))
                        //        {
                        //            havegm = true; 
                        //            break;
                        //        }
                        //    }
                        //    if (havegm)
                        //    {
                        //        chatInfoUnitsComponent.WordChatInfos.Clear();   
                        //    }
                        //}
                        break;
                    case (int)ChannelEnum.Team:
                        long teamServerId = StartSceneConfigCategory.Instance.GetBySceneName(chatInfoUnit.DomainZone(), Enum.GetName(SceneType.Team)).InstanceId;
                        T2C_GetTeamInfoResponse g_SendChatRequest1 = (T2C_GetTeamInfoResponse)await ActorMessageSenderComponent.Instance.Call
                            (teamServerId, new C2T_GetTeamInfoRequest() { UserID = request.ChatInfo.UserId });

                        long gateServerId = StartSceneConfigCategory.Instance.GetBySceneName(chatInfoUnit.DomainZone(), "Gate1").InstanceId;
                        G2T_GateUnitInfoResponse g2M_UpdateUnitResponse = null;
                        if (g_SendChatRequest1.Error == 0 && g_SendChatRequest1.TeamInfo != null)
                        {
                            for (int i = 0; i < g_SendChatRequest1.TeamInfo.PlayerList.Count; i++)
                            {
                                g2M_UpdateUnitResponse = (G2T_GateUnitInfoResponse)await ActorMessageSenderComponent.Instance.Call
                                (gateServerId, new T2G_GateUnitInfoRequest()
                                {
                                    UserID = g_SendChatRequest1.TeamInfo.PlayerList[i].UserID
                                });

                                if (g2M_UpdateUnitResponse.PlayerState == (int)PlayerState.Game && g2M_UpdateUnitResponse.SessionInstanceId > 0)
                                {
                                    MessageHelper.SendActor(g2M_UpdateUnitResponse.SessionInstanceId, m2C_SyncChatInfo);
                                }
                            }
                        }
                        break;
                    case (int)ChannelEnum.Union:
                        long unionid = request.ChatInfo.ParamId;
                        if (unionid == 0)
                        {
                            response.Error = ErrorCode.ERR_Union_Not_Exist;
                            reply();
                            return;
                        }
                        chatInfoUnitsComponent = chatInfoUnit.DomainScene().GetComponent<ChatSceneComponent>();
                        foreach (var otherUnit in chatInfoUnitsComponent.ChatInfoUnitsDict.Values)
                        {
                            if (otherUnit.UnionId == unionid)
                            {
                                MessageHelper.SendActor(otherUnit.GateSessionActorId, m2C_SyncChatInfo);
                            }
                        }
                        break;

                    case (int)ChannelEnum.Friend:
                        gateServerId = StartSceneConfigCategory.Instance.GetBySceneName(chatInfoUnit.DomainZone(), "Gate1").InstanceId;
                        g2M_UpdateUnitResponse = (G2T_GateUnitInfoResponse)await ActorMessageSenderComponent.Instance.Call
                              (gateServerId, new T2G_GateUnitInfoRequest()
                              {
                                  UserID = request.ChatInfo.ParamId
                              });

                        //发给好友()
                        if (g2M_UpdateUnitResponse.PlayerState == (int)PlayerState.Game && g2M_UpdateUnitResponse.SessionInstanceId > 0)
                        {
                            MessageHelper.SendActor(g2M_UpdateUnitResponse.SessionInstanceId, m2C_SyncChatInfo);
                        }
                        else
                        {
                            //存入到离线消息
                            long dbCacheId = DBHelper.GetDbCacheId(chatInfoUnit.DomainZone());
                            DBFriendInfo dBFriendInfo = await DBHelper.GetComponentCache<DBFriendInfo>(chatInfoUnit.DomainZone(), request.ChatInfo.ParamId);
                            if (dBFriendInfo != null && dBFriendInfo.FriendChats.Count < 10)
                            {
                                dBFriendInfo.FriendChats.Add(request.ChatInfo);
                                DBHelper.SaveComponentCache(chatInfoUnit.DomainZone(), request.ChatInfo.ParamId, dBFriendInfo).Coroutine();
                            }
                        }

                        //发给自己
                        g2M_UpdateUnitResponse = (G2T_GateUnitInfoResponse)await ActorMessageSenderComponent.Instance.Call
                              (gateServerId, new T2G_GateUnitInfoRequest()
                              {
                                  UserID = request.ChatInfo.UserId
                              });
                        MessageHelper.SendActor(g2M_UpdateUnitResponse.SessionInstanceId, m2C_SyncChatInfo);
                        break;
                }

                reply();
            }
        }
    }
}
