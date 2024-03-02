﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIMainChatComponent : Entity, IAwake<GameObject>
    {
        public GameObject ImageButton;
        public GameObject UIMainChatItem;
        public GameObject ChatUIListNode;
        public ScrollRect ScrollRect;

        public List<ChatInfo> ChatInfoList = new List<ChatInfo>();
        public List<UIMainChatItemComponent> ChatUIList = new List<UIMainChatItemComponent>();
    }



    public class UIMainChatComponentAwakeSystem : AwakeSystem<UIMainChatComponent, GameObject>
    {
        public override void Awake(UIMainChatComponent self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            self.ChatUIListNode = rc.Get<GameObject>("ChatUIListNode");
            self.UIMainChatItem = rc.Get<GameObject>("UIMainChatItem");
            self.UIMainChatItem.SetActive(false);
            self.ChatUIList.Clear();
            self.ChatInfoList.Clear();

            self.ImageButton = rc.Get<GameObject>("ImageButton");
            self.ImageButton.GetComponent<Button>().onClick.AddListener(() => { self.OnOpenChat(); });
            self.ScrollRect = rc.Get<GameObject>("ScrollView").GetComponent<ScrollRect>();
        }
    }

    public static class UIMainChatComponentSystem
    {
        public static void OnOpenChat(this UIMainChatComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIChat).Coroutine();
        }

        public static void  OnRecvChat(this UIMainChatComponent self, ChatInfo chatInfo)
        {
            if (self.ChatInfoList.Count >= 10)
            {
                self.ChatInfoList.RemoveAt(0);

                if (self.ChatUIList.Count > 0)
                {
                    UIMainChatItemComponent ui_1 = self.ChatUIList[0];
                    self.ChatUIList.RemoveAt(0);
                    self.ChatUIList.Add(ui_1);
                    ui_1.GameObject.transform.SetAsLastSibling();       
                }
            }
            self.ChatInfoList.Add(chatInfo);

            for (int i = 0; i < self.ChatInfoList.Count; i++)
            {
                UIMainChatItemComponent ui_2 = null;
                if (i < self.ChatUIList.Count)
                {
                    ui_2 = self.ChatUIList[i];
                    ui_2.GameObject.SetActive(true);
                }
                else
                {
                    GameObject itemSpace = GameObject.Instantiate(self.UIMainChatItem);
                    UICommonHelper.SetParent(itemSpace, self.ChatUIListNode);
                    itemSpace.SetActive(true);
                    ui_2 = self.AddChild<UIMainChatItemComponent,GameObject>(itemSpace);
                    ui_2.SetClickHandler(() => { self.OnOpenChat();  } );
                    self.ChatUIList.Add(ui_2);
                }

                ui_2.OnUpdateUI(self.ChatInfoList[i]);
            }
            for (int i = self.ChatInfoList.Count; i < self.ChatUIList.Count; i++)
            {
                self.ChatUIList[i].GameObject.SetActive(false);
                self.ChatUIList[i].UpdateHeight = false;
            }

            self.UpdatePosition().Coroutine();
            self.ImageButton.SetActive(self.ChatInfoList.Count < 4);
        }

        public static async ETTask UpdatePosition(this UIMainChatComponent self)
        {
            long instanceid = self.InstanceId;
            await TimerComponent.Instance.WaitAsync(100);
            if (instanceid != self.InstanceId)
            {
                return;
            }

            for (int i = 0; i < self.ChatUIList.Count; i++)
            {
                self.ChatUIList[i].UpdateHeight();
            }
            await TimerComponent.Instance.WaitAsync(100);
            if (instanceid != self.InstanceId)
            {
                return;
            }

            self.ScrollRect.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
        }
    }
}
