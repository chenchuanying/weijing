using System.Collections.Generic;

namespace ET
{

    [ObjectSystem]
    public class ChatComponentAwakeSystem : AwakeSystem<ChatComponent>
    {
        public override void Awake(ChatComponent self)
        {
            self.ChatTypeList.Clear();
            self.ChatTypeList.Add(ChannelEnum.Word, new List<ChatInfo>() );
            self.ChatTypeList.Add(ChannelEnum.Team, new List<ChatInfo>());
            self.ChatTypeList.Add(ChannelEnum.System, new List<ChatInfo>());
            self.ChatTypeList.Add(ChannelEnum.Union, new List<ChatInfo>());
            self.ChatTypeList.Add(ChannelEnum.Friend, new List<ChatInfo>());
            self.ChatTypeList.Add(ChannelEnum.Pick, new List<ChatInfo>());
            self.ChatTypeList.Add(ChannelEnum.PaiMai, new List<ChatInfo>());
        }
    }

    public static class ChatComponentSystem
    {

        public async static ETTask SendChat(this ChatComponent self, int channelEnum, string content, long paramId = 0)
        {
            if (channelEnum == ChannelEnum.Word && TimeHelper.ClientNow() - self.LastSendWord < 60 * TimeHelper.Second)
            {
                EventType.CommonHintError.Instance.ZoneScene = self.ZoneScene();
                EventType.CommonHintError.Instance.errorValue = ErrorCode.ERR_WordChat;
                EventSystem.Instance.PublishClass(EventType.CommonHintError.Instance);
                return;
            }
            if (channelEnum != ChannelEnum.Word  && TimeHelper.ClientNow() - self.LastSendTeam < 10 * TimeHelper.Second)
            {
                EventType.CommonHintError.Instance.ZoneScene = self.ZoneScene();
                EventType.CommonHintError.Instance.errorValue = ErrorCode.ERR_UnionChatLimit;
                EventSystem.Instance.PublishClass(EventType.CommonHintError.Instance);
                return;
            }

            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
            C2C_SendChatRequest c2S_SendChatRequest = new C2C_SendChatRequest() {  };
            c2S_SendChatRequest.ChatInfo = new ChatInfo();
            c2S_SendChatRequest.ChatInfo.PlayerLevel = userInfo.Lv;
            c2S_SendChatRequest.ChatInfo.Occ = userInfo.Occ;
            switch (channelEnum)
            {
                case ChannelEnum.Word:
                    self.LastSendWord = TimeHelper.ClientNow();
                    break;
                case ChannelEnum.Team:
                    self.LastSendTeam = TimeHelper.ClientNow();
                    Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
                    NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
                    c2S_SendChatRequest.ChatInfo.ParamId = numericComponent.GetAsLong(NumericType.TeamId);
                    break;
                case ChannelEnum.Union:
                    self.LastSendTeam = TimeHelper.ClientNow();
                    unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
                    numericComponent = unit.GetComponent<NumericComponent>();
                    c2S_SendChatRequest.ChatInfo.ParamId = numericComponent.GetAsLong(NumericType.UnionId_0);
                    break;
                case ChannelEnum.Friend:
                    self.LastSendTeam = TimeHelper.ClientNow();
                    c2S_SendChatRequest.ChatInfo.ParamId = paramId;
                    break;
                default:
                    self.LastSendTeam = TimeHelper.ClientNow();
                    break;
            }

            c2S_SendChatRequest.ChatInfo.UserId = userInfo.UserId;
            c2S_SendChatRequest.ChatInfo.ChannelId = (int)channelEnum;
            c2S_SendChatRequest.ChatInfo.ChatMsg = content;
            c2S_SendChatRequest.ChatInfo.PlayerName = userInfo.Name;
            C2C_SendChatResponse sendChatResponse = (C2C_SendChatResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2S_SendChatRequest);

            if (sendChatResponse.Error != ErrorCode.ERR_Success && !string.IsNullOrEmpty(sendChatResponse.Message))
            {
                HintHelp.GetInstance().ShowHint(sendChatResponse.Message);
            }
        }

        public static List<ChatInfo> GetChatListByType(this ChatComponent self, int channelEnum)
        {
            if (channelEnum == ChannelEnum.Word)
            {
                List<ChatInfo> chatInfos = new List<ChatInfo>();
                chatInfos.AddRange(self.ChatTypeList[ChannelEnum.Word]);
                chatInfos.AddRange(self.ChatTypeList[ChannelEnum.System]);
                chatInfos.AddRange(self.ChatTypeList[ChannelEnum.Union]);
                chatInfos.Sort(delegate (ChatInfo a, ChatInfo b)
                {
                    return (int)(a.Time - b.Time);
                });

                return chatInfos;
            }
            if (channelEnum == ChannelEnum.System)
            {
                List<ChatInfo> chatInfos = new List<ChatInfo>();
                chatInfos.AddRange(self.ChatTypeList[ChannelEnum.System]);
                chatInfos.AddRange(self.ChatTypeList[ChannelEnum.Pick]);
                chatInfos.Sort(delegate (ChatInfo a, ChatInfo b)
                {
                    return (int)(a.Time - b.Time);
                });

                return chatInfos;
            }
            return self.ChatTypeList[channelEnum];
        }

        public static  void OnRecvChat(this ChatComponent self, ChatInfo chatInfo)
        {
            // 在黑名单的玩家发言不显示
            if (chatInfo.ChannelId == (int)ChannelEnum.Word)
            {
                foreach (FriendInfo friendInfo in self.ZoneScene().GetComponent<FriendComponent>().Blacklist)
                {
                    if (friendInfo.UserId == chatInfo.UserId)
                    {
                        return;
                    }
                }
            }

            if (UnitHelper.IsBackRoom(self.ZoneScene()))
            {
                return;
            }

            self.LastChatInfo = chatInfo;
            List<ChatInfo> chatInfos = self.ChatTypeList[chatInfo.ChannelId];
            if (chatInfos.Count >= 20)
            {
                chatInfos.RemoveAt(0);
            }
            chatInfos.Add(chatInfo);
            HintHelp.GetInstance().DataUpdate(DataType.OnRecvChat);
        }
    }
}
