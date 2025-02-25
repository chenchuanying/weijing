﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIFriendChatComponent : Entity, IAwake
    {

        public GameObject InputFieldTMP;
        public GameObject ButtonSend;
        public GameObject ChatContent;
        public GameObject UIFriendChatItem;

        public FriendInfo FriendInfo;
        public List<UIFriendChatItemComponent> ChatUIList = new List<UIFriendChatItemComponent>();
    }


    public class UIFriendChatComponentAwakeSystem : AwakeSystem<UIFriendChatComponent>
    {
        public override void Awake(UIFriendChatComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.InputFieldTMP = rc.Get<GameObject>("InputFieldTMP");
            self.InputFieldTMP.GetComponent<InputField>().onValueChanged.AddListener((string text) => { self.CheckSensitiveWords(); });

            self.ButtonSend = rc.Get<GameObject>("ButtonSend");
            self.ButtonSend.GetComponent<Button>().onClick.AddListener(() => { self.OnSendChat(); });

            self.ChatContent = rc.Get<GameObject>("ChatContent");
            self.UIFriendChatItem = rc.Get<GameObject>("UIFriendChatItem");
            self.UIFriendChatItem.SetActive(false);
            self.ChatUIList.Clear();
        }
    }

    public static class UIFriendChatComponentSystem
    {

        public static void CheckSensitiveWords(this UIFriendChatComponent self)
        {
            string text_new = "";
            string text_old = self.InputFieldTMP.GetComponent<InputField>().text;
            MaskWordHelper.Instance.IsContainSensitiveWords(ref text_old, out text_new);
            self.InputFieldTMP.GetComponent<InputField>().text = text_old;
        }

        public static void OnUpdateUI(this UIFriendChatComponent self, FriendInfo friendInfo)
        {
            self.FriendInfo = friendInfo;
            self.OnFriendChat();
        }

        public static  void OnFriendChat(this UIFriendChatComponent self)
        {
            long friendId = self.FriendInfo.UserId;
            List<ChatInfo> chatInfos = null;
            FriendComponent friendComponent = self.ZoneScene().GetComponent<FriendComponent>();
            friendComponent.ChatMsgList.TryGetValue(friendId, out chatInfos);
            for (int i =0; i < self.ChatUIList.Count; i++)
            {
                self.ChatUIList[i].GameObject.SetActive(false);
            }
            if (chatInfos == null)
            {
                return;
            }

            for (int i = 0; i < chatInfos.Count; i++)
            {
                UIFriendChatItemComponent ui_2 = null;
                if (i < self.ChatUIList.Count)
                {
                    ui_2 = self.ChatUIList[i];
                    ui_2.GameObject.SetActive(true);
                }
                else
                {
                    GameObject itemSpace = GameObject.Instantiate(self.UIFriendChatItem);
                    itemSpace.SetActive(true);
                    UICommonHelper.SetParent(itemSpace, self.ChatContent);
                    ui_2 = self.AddChild<UIFriendChatItemComponent, GameObject>( itemSpace);
                    self.ChatUIList.Add(ui_2);
                }

                ui_2.OnUpdateUI(chatInfos[i]);
            }
           

            self.UpdatePosition().Coroutine();
        }

        public static async ETTask UpdatePosition(this UIFriendChatComponent self)
        {
            await TimerComponent.Instance.WaitAsync(100);
            if (self.ChatContent == null)
                return;
            RectTransform rectTransform = self.ChatContent.GetComponent<RectTransform>();
            if (rectTransform.sizeDelta.y > 600)
            {
                rectTransform.anchoredPosition = new Vector2(0, rectTransform.sizeDelta.y - 600);
            }
        }

        public static void OnSendChat(this UIFriendChatComponent self)
        {
            string text = self.InputFieldTMP.GetComponent<InputField>().text;
            if (string.IsNullOrEmpty(text) || text.Length == 0)
            {
                FloatTipManager.Instance.ShowFloatTip("请输入聊天内容！");
                return;
            }

            bool mask = MaskWordHelper.Instance.IsContainSensitiveWords(text);
            if (mask)
            {
                FloatTipManager.Instance.ShowFloatTip("请重新输入！");
                return;
            }

            self.ZoneScene().GetComponent<ChatComponent>().SendChat(ChannelEnum.Friend, text, self.FriendInfo.UserId).Coroutine();
            self.InputFieldTMP.GetComponent<InputField>().text = "";
        }

    }
}