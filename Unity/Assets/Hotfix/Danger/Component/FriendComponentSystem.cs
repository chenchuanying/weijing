﻿using System.Collections.Generic;

namespace ET
{

    [ObjectSystem]
    public class FriendComponentAwakeSystem : AwakeSystem<FriendComponent>
    {
        public override void Awake(FriendComponent self)
        {
            self.FriendList.Clear();
            self.ApplyList.Clear();
        }
    }

    public static  class FriendComponentSystem
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="friendInfo"></param>
        /// <param name="code">1 同意 2 拒绝</param>
        /// <returns></returns>
        public static async ETTask FriendApplyReply(this FriendComponent self,FriendInfo friendInfo, int code)
        {
            C2F_FriendApplyReplyRequest c2M_ItemHuiShouRequest = new C2F_FriendApplyReplyRequest() {
                FriendID = friendInfo.UserId,
                ReplyCode = code,
                UserID = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.UserId,
            };
            F2C_FriendApplyReplyResponse F2C_FriendInfoResponse = (F2C_FriendApplyReplyResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_ItemHuiShouRequest);

            if (F2C_FriendInfoResponse.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            if (code == 1)
            {
                self.FriendList.Add(friendInfo);
            }
            for (int i = self.ApplyList.Count - 1; i >= 0; i--)
            {
                if (self.ApplyList[i].UserId == friendInfo.UserId)
                {
                    self.ApplyList.RemoveAt(i);
                    break;
                }
            }
            HintHelp.GetInstance().DataUpdate(DataType.FriendUpdate);
        }

        public static void  OnRecvFriendApplyResult(this FriendComponent self, M2C_FriendApplyResult message)
        {
            bool have = false;
            for (int i = 0; i < self.ApplyList.Count; i++)
            {
                if (self.ApplyList[i].UserId == message.FriendInfo.UserId)
                {
                    have = true;
                    break;
                }
            }

            if (!have)
            {
                self.ApplyList.Add(message.FriendInfo);
            }
            self.ZoneScene().GetComponent<ReddotComponent>().AddReddont(ReddotType.FriendApply);
        }

        public static void InitFrindChat(this FriendComponent self, List<ChatInfo> chatlist)
        {
            for (int i = 0; i < chatlist.Count; i++)
            {
                self.SetChatData(chatlist[i]);
            }
        }

        public static void SetChatData(this FriendComponent self, ChatInfo chatInfo)
        {
            long friendId = 0;
            long myUserId = UnitHelper.GetMyUnitId(self.ZoneScene());
            bool othertoself = false;
            if (chatInfo.UserId == myUserId)
            {
                friendId = chatInfo.ParamId;   //我对别人的私聊
            }
            else
            {
                friendId = chatInfo.UserId;    //别人对我的私聊
                othertoself = true;
            }

            if (othertoself)
            {
                self.ZoneScene().GetComponent<ReddotComponent>().AddReddont(ReddotType.FriendChat);
            }

            if (!self.ChatMsgList.ContainsKey(friendId))
            {
                self.ChatMsgList.Add(friendId, new List<ChatInfo>());
            }
            if (!self.FriendChatId.Contains(chatInfo.UserId))
            {
                self.FriendChatId.Add(chatInfo.UserId);
            }
            List<ChatInfo> chatInfoList = self.ChatMsgList[friendId];
            for (int i = 0; i < chatInfoList.Count; i++)
            {
                if (chatInfoList[i].Time > 0 && chatInfoList[i].Time == chatInfo.Time)
                {
                    return;
                }
            }
            chatInfoList.Add(chatInfo);
        }

        public static void OnRecvChat(this FriendComponent self, ChatInfo chatInfo)
        {
            self.SetChatData(chatInfo);
            HintHelp.GetInstance().DataUpdate(DataType.FriendChat);
        }

        public static void OnFriendDelelte(this FriendComponent self, long friendId)
        {
            for (int i = self.FriendList.Count - 1; i >= 0; i--)
            {
                if (self.FriendList[i].UserId == friendId)
                {
                    self.FriendList.RemoveAt(i);    
                }
            }
        }

        /// <summary>
        /// 1 好友  2黑名单
        /// </summary>
        /// <param name="self"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetFriendType(this FriendComponent self, long userId)
        {
            for (int i = 0; i < self.FriendList.Count; i++)
            {
                if (self.FriendList[i].UserId == userId)
                {
                    return 1;
                }
            }
            for (int i = 0; i < self.Blacklist.Count; i++)
            {
                if (self.Blacklist[i].UserId == userId)
                {
                    return 2;
                }
            }
            return 0;
        }

    }
}
