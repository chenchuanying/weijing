﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIJiaYuanDaShiProComponent : Entity, IAwake, IDestroy
    {
        public string AssetPath;
        public GameObject ButtonEat;
        public GameObject BuildingList1;
        public GameObject BuildingList2;
        public GameObject Label_Tips;

        public UIItemComponent UIItemCost;
        public List<UIItemComponent> ItemList = new List<UIItemComponent>();
        public List<UIJiaYuanDaShiProItemComponent> ProList = new List<UIJiaYuanDaShiProItemComponent>();
    }

    public class UIJiaYuanDaShiProComponentDestroy : DestroySystem<UIJiaYuanDaShiProComponent>
    {
        public override void Destroy(UIJiaYuanDaShiProComponent self)
        {
            if (!string.IsNullOrEmpty(self.AssetPath))
            {
                ResourcesComponent.Instance.UnLoadAsset(self.AssetPath);
            }

            self.AssetPath = string.Empty;
        }
    }

    public class UIJiaYuanDaShiProComponentAwake : AwakeSystem<UIJiaYuanDaShiProComponent>
    {
        public override void Awake(UIJiaYuanDaShiProComponent self)
        {
            self.ItemList.Clear();
            self.ProList.Clear();
            self.AssetPath = string.Empty;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.BuildingList1 = rc.Get<GameObject>("BuildingList1");
            self.BuildingList2 = rc.Get<GameObject>("BuildingList2");
            self.Label_Tips = rc.Get<GameObject>("Label_Tips");

            GameObject gameObject = rc.Get<GameObject>("UICommonItem");
            self.UIItemCost = self.AddChild<UIItemComponent, GameObject>(gameObject);
            self.UIItemCost.Label_ItemNum.SetActive(false);
            self.UIItemCost.Label_ItemName.SetActive(false);

            self.ButtonEat = rc.Get<GameObject>("ButtonEat");
            ButtonHelp.AddListenerEx( self.ButtonEat, () => { self.OnButtonEat().Coroutine(); } );

            self.GetParent<UI>().OnUpdateUI = self.OnUpdateUI;
        }
    }

    public static class UIJiaYuanDaShiProComponentSystem
    {

        public static void OnUpdateUI(this UIJiaYuanDaShiProComponent self)
        {
            self.OnUpdateProList();
            self.OnUpdateItemList();
        }

        public static void OnUpdateProList(this UIJiaYuanDaShiProComponent self)
        {
            var path = ABPathHelper.GetUGUIPath("JiaYuan/UIJiaYuanDaShiProItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            JiaYuanComponent jiaYuanComponent = self.ZoneScene().GetComponent<JiaYuanComponent>();
            self.AssetPath = path;
            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
            JiaYuanConfig jiaYuanConfig = JiaYuanConfigCategory.Instance.Get(userInfo.JiaYuanLv);

            string proMax = jiaYuanConfig.ProMax;
            string[] prolist = proMax.Split(';');
            for (int i = 0; i < prolist.Length; i++)
            { 
                if (ComHelp.IfNull(prolist[i]))
                {
                    continue;
                }
                string[] proinfo = prolist[i].Split(',');
                if (proinfo.Length < 2)
                {
                    continue;
                }

                UIJiaYuanDaShiProItemComponent ui_1 = null;
                if (i < self.ProList.Count)
                {
                    ui_1 = self.ProList[i];
                    ui_1.GameObject.SetActive(true);
                }
                else
                {
                    GameObject gameObject = GameObject.Instantiate(bundleGameObject);
                    ui_1 = self.AddChild<UIJiaYuanDaShiProItemComponent, GameObject>(gameObject);
                    UICommonHelper.SetParent(gameObject, self.BuildingList1);
                    self.ProList.Add(ui_1);
                }

                ui_1.OnUpdateUI( jiaYuanComponent.GetDaShiProInfo(int.Parse(proinfo[0])), prolist[i]);
            }
            for (int i = prolist.Length;i < self.ProList.Count; i++ )
            {
                self.ProList[i].GameObject.SetActive(false);
            }
        }

        public static void OnUpdateItemList(this UIJiaYuanDaShiProComponent self)
        {
            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            BagComponent bagComponent = self.ZoneScene().GetComponent<BagComponent>();
            List<BagInfo> bagInfos = bagComponent.GetBagList();

            int number = 0;
            for (int i = 0; i < bagInfos.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(bagInfos[i].ItemID);
                if (itemConfig.ItemType!= 1 || itemConfig.ItemSubType!= 131)
                {
                    continue;
                }
                if (itemConfig.ItemQuality == 1)
                {
                    continue;
                }

                UIItemComponent ui_1 = null;
                if (number < self.ItemList.Count)
                {
                    ui_1 = self.ItemList[number];
                    ui_1.GameObject.SetActive(true);
                }
                else
                {
                    GameObject gameObject = GameObject.Instantiate(bundleGameObject);
                    ui_1 = self.AddChild<UIItemComponent, GameObject>(gameObject);
                    UICommonHelper.SetParent(gameObject, self.BuildingList2);
                    ui_1.SetClickHandler((BagInfo bagInfo) => { self.OnSelectItem(bagInfo); });
                    self.ItemList.Add(ui_1);
                }
                ui_1.UpdateItem(bagInfos[i], ItemOperateEnum.None);

                number++;
            }
            for (int i = number; i < self.ItemList.Count; i++)
            {
                self.ItemList[i].GameObject.SetActive(false);
            }
        }

        public static void OnSelectItem(this UIJiaYuanDaShiProComponent self, BagInfo bagInfo)
        {
            foreach (UIItemComponent uiItemComponent in self.ItemList)
            {
                uiItemComponent.Image_XuanZhong.SetActive(uiItemComponent.Baginfo.BagInfoID == bagInfo.BagInfoID);
            }

            self.UIItemCost.UpdateItem(bagInfo, ItemOperateEnum.None);
            self.UIItemCost.Label_ItemNum.GetComponent<Text>().text = "1";
            self.UIItemCost.Label_ItemName.SetActive(true);

            ItemConfig itemCof = ItemConfigCategory.Instance.Get(bagInfo.ItemID);
            self.Label_Tips.GetComponent<Text>().text = itemCof.ItemBlackDes;

        }

        public static async ETTask OnButtonEat(this UIJiaYuanDaShiProComponent self)
        {
            BagInfo bagInfo = self.UIItemCost.Baginfo;
            if (bagInfo == null)
            {
                return;
            }
            BagComponent bagComponent = self.ZoneScene().GetComponent<BagComponent>();
            if (bagComponent.GetItemNumber(bagInfo.ItemID) < 1)
            {
                FloatTipManager.Instance.ShowFloatTip("道具数量不足！");
                return;
            }

            int index = 0;
            for (int i = 0; i < self.ItemList.Count; i++)
            {
                if (self.ItemList[i].Baginfo.BagInfoID != self.UIItemCost.Baginfo.BagInfoID)
                {
                    continue;
                }

                index = i;
                break;
            }

            List<long> ids = new List<long>() { bagInfo.BagInfoID };
            C2M_JiaYuanDaShiRequest  request = new C2M_JiaYuanDaShiRequest() { BagInfoIDs = ids };
            M2C_JiaYuanDaShiResponse response = (M2C_JiaYuanDaShiResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);

            if (self.IsDisposed || response.Error != ErrorCode.ERR_Success)
            {
                return;
            }
            string asstips = "增加属性： ";
            for (int i = 0; i < response.JiaYuanProAdd.Count; i++)
            {
                string pname = ItemViewHelp.GetAttributeName(response.JiaYuanProAdd[i].KeyId);
                asstips += $"{pname}: +{response.JiaYuanProAdd[i].Value} ";
            }
            FloatTipManager.Instance.ShowFloatTip(asstips);

            JiaYuanComponent jiaYuanComponent = self.ZoneScene().GetComponent<JiaYuanComponent>();
            jiaYuanComponent.JiaYuanProList_7 = response.JiaYuanProList;
            jiaYuanComponent.JiaYuanDaShiTime_1 = response.JiaYuanDaShiTime;

            if (bagComponent.GetItemNumber(self.UIItemCost.Baginfo.ItemID) < 1)
            {
                self.UIItemCost.UpdateItem(null, ItemOperateEnum.None);
                self.UIItemCost.Image_ItemQuality.SetActive(false);
                self.UIItemCost.Label_ItemName.SetActive(false);
            }

            self.OnUpdateUI();

            self.Label_Tips.GetComponent<Text>().text = "";
            
            if (self.ItemList[index].GameObject.activeSelf)
            {
                self.OnSelectItem(self.ItemList[index].Baginfo);
            }
            else if (index > 0 && self.ItemList[index - 1].GameObject.activeSelf)
            {
                self.OnSelectItem(self.ItemList[index - 1].Baginfo);
            }
        }
    }
}
