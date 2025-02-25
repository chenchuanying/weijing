﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIActivityTeHuiItemComponent : Entity, IAwake<GameObject>,IDestroy
    {
        public GameObject ImageReceived;
        public GameObject TextPrice;
        public GameObject TextType;
        public GameObject ButtonBuy;
        public GameObject ItemListNode;
        public GameObject ImageTitle;
        public GameObject ImageBox;

        public ActivityConfig ActivityConfig;

        public List<string> AssetPath = new List<string>();
    }


    public class UIActivityTeHuiItemComponentAwakeSystem : AwakeSystem<UIActivityTeHuiItemComponent, GameObject>
    {
        public override void Awake(UIActivityTeHuiItemComponent self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            
            self.ImageReceived = rc.Get<GameObject>("ImageReceived");
            self.TextPrice = rc.Get<GameObject>("TextPrice");
            self.TextType = rc.Get<GameObject>("TextType");

            self.ButtonBuy = rc.Get<GameObject>("ButtonBuy");
            ButtonHelp.AddListenerEx( self.ButtonBuy, () => { self.OnButtonBuy().Coroutine();  } );

            self.ItemListNode = rc.Get<GameObject>("ItemListNode");
            self.ImageTitle = rc.Get<GameObject>("ImageTitle");
            self.ImageBox = rc.Get<GameObject>("ImageBox");
        }
    }

    public class UIActivityTeHuiItemComponentDestroy: DestroySystem<UIActivityTeHuiItemComponent>
    {
        public override void Destroy(UIActivityTeHuiItemComponent self)
        {
            for (int i = 0; i < self.AssetPath.Count; i++)
            {
                if (!string.IsNullOrEmpty(self.AssetPath[i]))
                {
                    ResourcesComponent.Instance.UnLoadAsset(self.AssetPath[i]);
                }
            }

            self.AssetPath = null;
        }
    }

    public static class UIActivityTeHuiItemComponentSystem
    {
        public static async ETTask OnButtonBuy(this UIActivityTeHuiItemComponent self)
        {
            ActivityComponent activityComponent = self.ZoneScene().GetComponent<ActivityComponent>();
            if (activityComponent.ActivityReceiveIds.Contains(self.ActivityConfig.Id))
            {
                FloatTipManager.Instance.ShowFloatTip("已经购买过该礼包！");
                return;
            }

            int errorCode = await activityComponent.GetActivityReward(self.ActivityConfig.ActivityType, self.ActivityConfig.Id);
            if (errorCode == ErrorCode.ERR_Success)
            {
                self.ImageReceived.SetActive(true);
                self.ButtonBuy.SetActive(false);
            }
        }

        public static void OnUpdateUI(this UIActivityTeHuiItemComponent self, int activityId, bool received)
        {
            ActivityConfig activityConfig = ActivityConfigCategory.Instance.Get(activityId);
            self.ActivityConfig = activityConfig;
            self.ImageReceived.SetActive(received);
            self.ButtonBuy.SetActive(!received);
            self.TextPrice.GetComponent<Text>().text = activityConfig.Par_2.Split(';')[1];
            self.TextType.GetComponent<Text>().text = activityConfig.Par_4;

            UICommonHelper.DestoryChild(self.ItemListNode);
            UICommonHelper.ShowItemList(activityConfig.Par_3, self.ItemListNode, self, 1f, true, ItemGetWay.Activity_DayTeHui);

            //显示图标
            string ItemIcon = activityConfig.Icon;
            if (ItemIcon != "" && ItemIcon != null)
            {
                string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, ItemIcon);
                Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
                if (!self.AssetPath.Contains(path))
                {
                    self.AssetPath.Add(path);
                }
                self.ImageBox.GetComponent<Image>().sprite = sp;
            }
        }
    }
}
