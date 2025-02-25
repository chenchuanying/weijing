﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UISettlementRewardComponent : Entity, IAwake<GameObject>
    {
        public GameObject Image_bg;
        public GameObject ImageButton;
        public GameObject Image_bgOpen;
        public GameObject TextName;

        public RewardItem RewardItem;
        public Action<int> ClickHandler;
        public int Index = -1;
        public bool IsSelect;
        public UIItemComponent UiItem;

        public GameObject ItemNode;
        public GameObject GameObject;
    }


    public class UISettlementRewardComponentAwakeSystem : AwakeSystem<UISettlementRewardComponent, GameObject>
    {
        public override void Awake(UISettlementRewardComponent self, GameObject rc)
        {
            self.IsSelect = false;
            self.UiItem = null;
            self.RewardItem = null;
            self.GameObject = rc;
            self.Image_bgOpen = rc.transform.Find("Image_bgOpen").gameObject;
            self.Image_bgOpen.SetActive(false);

            self.TextName = rc.transform.Find("TextName").gameObject;

            self.Image_bg = rc.transform.Find("Image_bg").gameObject;

            self.ImageButton = rc.transform.Find("ImageButton").gameObject;
            ButtonHelp.AddListenerEx(self.ImageButton, self.OnClickItem);

            self.ItemNode = rc.transform.Find("UIItem").gameObject;
            self.ItemNode.SetActive(false);
            self.OnInitUI();
        }
    }

    public static class UISettlementRewardComponentSystem
    {
        public static  void OnInitUI(this UISettlementRewardComponent self)
        {
            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject UIItem = GameObject.Instantiate(bundleGameObject);
            UIItemComponent ui_1 = self.AddChild<UIItemComponent, GameObject>(UIItem);
            UICommonHelper.SetParent(UIItem, self.ItemNode);
            self.UiItem = ui_1;
            if (self.RewardItem != null)
            {
                self.UiItem.UpdateItem(new BagInfo() { ItemID = self.RewardItem.ItemID, ItemNum = self.RewardItem.ItemNum }, ItemOperateEnum.None);
            }
        }

        public static  void OnUpdateData(this UISettlementRewardComponent self, RewardItem rewardItem)
        {
            self.RewardItem = rewardItem;
            if (self.UiItem != null)
            {
                self.UiItem.UpdateItem(new BagInfo() { ItemID = self.RewardItem.ItemID, ItemNum = self.RewardItem.ItemNum }, ItemOperateEnum.None);
            }
        }

        public static void SetClickHandler(this UISettlementRewardComponent self, Action<int> action, int index)
        {
            self.Index = index;
            self.ClickHandler = action;
        }

        public static void ShowRewardItem(this UISettlementRewardComponent self, string name)
        {
            self.TextName.GetComponent<Text>().text = name;
            if (self.ItemNode.activeSelf)
            {
                return;
            }

            self.ItemNode.SetActive(true);
            self.Image_bgOpen.SetActive(true);
            self.Image_bg.SetActive(false);
            self.DisableClick();
        }

        public static bool IsCanClicked(this UISettlementRewardComponent self)
        {
            return self.ImageButton.activeSelf;
        }

        public static void DisableClick(this UISettlementRewardComponent self)
        {
            self.ImageButton.SetActive(false);
        }

        public static void OnClickItem(this UISettlementRewardComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            if (self.Index >= 3 && !unit.IsYueKaEndStates())
            {
                FloatTipManager.Instance.ShowFloatTip("周卡用户才能开启！");
                return;
            }

            if (self.ItemNode.activeSelf)
            {
                return;
            };

            //self.ShowRewardItem(string.Empty);
            self.ClickHandler(self.Index);
        }
    }
}
