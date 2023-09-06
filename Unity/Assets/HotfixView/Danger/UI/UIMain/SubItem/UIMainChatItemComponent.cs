﻿using UnityEngine;
using System;
using UnityEngine.UI;

namespace ET
{
    public class UIMainChatItemComponent : Entity, IAwake<GameObject>
    {
        public bool UpdateHeight;
        public GameObject Lab_ChatText;
        public GameObject ImageButton;
        public GameObject[] TitleList = new GameObject[ChannelEnum.Number];

        public ChatInfo m2C_SyncChatInfo;
        public Action ClickHanlder;
        public GameObject GameObject;
    }

    public class UIMainChatItemComponentAwakeSystem : AwakeSystem<UIMainChatItemComponent, GameObject>
    {
        public override void Awake(UIMainChatItemComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;   
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            self.Lab_ChatText = rc.Get<GameObject>("Lab_ChatText");

            for (int i = 0; i < ChannelEnum.Number; i++)
            {
                self.TitleList[i] = rc.Get<GameObject>(i.ToString());
                self.TitleList[i].SetActive(false);
            }

            self.ImageButton = rc.Get<GameObject>("ImageButton");
            self.ImageButton.GetComponent<Button>().onClick.AddListener(() => { self.ClickHanlder(); });
        }
    }

    public static class UIMainChatItemComponentSystem
    {

        public static void SetClickHandler(this UIMainChatItemComponent self, Action action)
        {
            self.ClickHanlder = action;
        }

        public static  void UpdateHeight(this UIMainChatItemComponent self)
        {
            if (!self.GameObject.activeSelf || !self.UpdateHeight)
            {
                return;
            }
            self.UpdateHeight = false;
            Text textMeshProUGUI = self.Lab_ChatText.GetComponent<Text>();
            if (textMeshProUGUI.GetComponent<Text>().preferredHeight > 40)
            {
                self.GameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(400, textMeshProUGUI.GetComponent<Text>().preferredHeight);
            }
            self.GameObject.SetActive(false);
            self.GameObject.SetActive(true);
        }

        //<link="ID">my link</link>
        //<sprite=0>
        public static  void OnUpdateUI(this UIMainChatItemComponent self, ChatInfo chatInfo)
        {
            self.UpdateHeight = true;
            self.m2C_SyncChatInfo = chatInfo;
            Text textMeshProUGUI = self.Lab_ChatText.GetComponent<Text>();

            if (chatInfo.ChannelId == (int)ChannelEnum.System)
            {
                textMeshProUGUI.text = chatInfo.ChatMsg;
            }
            else
            {
                //<color=#FFFF00>白泪伊1</color>: 12112
                //textMeshProUGUI.text = $"<color=#FFFF00>{chatInfo.PlayerName}</color>: {chatInfo.ChatMsg}";
                textMeshProUGUI.text = $"{chatInfo.PlayerName} : {chatInfo.ChatMsg}";
            }
            self.TitleList[chatInfo.ChannelId].SetActive(true);
        }
    }

}
