﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UISeasonJingHeZhuruComponent: Entity, IAwake
    {
        public GameObject CloseBtn;
        public GameObject UICommonItem;
        public GameObject ItemNameText;
        public GameObject NowQualityText;
        public GameObject ItemListNode;
        public GameObject UIItem;
        public GameObject AddQualityText;
        public GameObject ZhuRuBtn;

        public BagInfo MainBagInfo;
        public UIItemComponent UIItemComponent;
        public List<long> CostIds = new List<long>();
        public int MaxAdd;
        public int MinAdd;
        public List<UIItemComponent> ItemList = new List<UIItemComponent>();
        public List<string> AssetPath = new List<string>();
    }

    public class UISeasonJingHeZhuruComponentAwakeSystem: AwakeSystem<UISeasonJingHeZhuruComponent>
    {
        public override void Awake(UISeasonJingHeZhuruComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.CloseBtn = rc.Get<GameObject>("CloseBtn");
            self.UICommonItem = rc.Get<GameObject>("UICommonItem");
            self.ItemNameText = rc.Get<GameObject>("ItemNameText");
            self.NowQualityText = rc.Get<GameObject>("NowQualityText");
            self.ItemListNode = rc.Get<GameObject>("ItemListNode");
            self.UIItem = rc.Get<GameObject>("UIItem");
            self.AddQualityText = rc.Get<GameObject>("AddQualityText");
            self.ZhuRuBtn = rc.Get<GameObject>("ZhuRuBtn");

            self.UIItem.SetActive(false);
            self.UIItemComponent = self.AddChild<UIItemComponent, GameObject>(self.UICommonItem);
            self.ZhuRuBtn.GetComponent<Button>().onClick.AddListener(() => { self.OnZhuRuBtn().Coroutine(); });
            self.CloseBtn.GetComponent<Button>().onClick.AddListener(() => { UIHelper.Remove(self.DomainScene(), UIType.UISeasonJingHeZhuru); });
        }
    }

    public static class ISeasonJingHeZhuruComponentSystem
    {
        public static async ETTask OnZhuRuBtn(this UISeasonJingHeZhuruComponent self)
        {
            if (self.CostIds.Count <= 0)
            {
                FloatTipManager.Instance.ShowFloatTip("未选择道具！");
                return;
            }

            C2M_JingHeZhuruRequest request = new C2M_JingHeZhuruRequest() { BagInfoId = self.MainBagInfo.BagInfoID, OperateBagID = self.CostIds };
            M2C_JingHeZhuruResponse response = (M2C_JingHeZhuruResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);

            self.MainBagInfo = self.ZoneScene().GetComponent<BagComponent>().GetBagInfo(self.MainBagInfo.BagInfoID);
            self.AddQualityText.GetComponent<Text>().text = "";
            self.MinAdd = 0;
            self.MaxAdd = 0;
            self.CostIds.Clear();
            self.UpdateItemList();
            self.NowQualityText.GetComponent<Text>().text = $"当前品质:{self.MainBagInfo.ItemPar}";
        }

        public static void InitInfo(this UISeasonJingHeZhuruComponent self, BagInfo bagInfo)
        {
            self.MainBagInfo = bagInfo;
            self.UIItemComponent.UpdateItem(bagInfo, ItemOperateEnum.None);
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(bagInfo.ItemID);
            self.ItemNameText.GetComponent<Text>().text = itemConfig.ItemName;
            self.NowQualityText.GetComponent<Text>().text = $"当前品质:{bagInfo.ItemPar}";
            self.AddQualityText.GetComponent<Text>().text = "";

            self.UpdateItemList();
        }

        public static void UpdateItemList(this UISeasonJingHeZhuruComponent self)
        {
            BagComponent bagComponent = self.ZoneScene().GetComponent<BagComponent>();

            int number = 0;
            if (self.ItemList.Count <= 12)
            {
                // 先生成12个格子
                for (int i = 0; i < 12; i++)
                {
                    UIItemComponent uI = null;
                    GameObject go = GameObject.Instantiate(self.UIItem);
                    go.SetActive(true);
                    UICommonHelper.SetParent(go, self.ItemListNode);
                    go.SetActive(true);
                    uI = self.AddChild<UIItemComponent, GameObject>(go);
                    uI.SetClickHandler((bagInfo) => { self.OnSelect(bagInfo); });
                    self.ItemList.Add(uI);
                    uI.UpdateItem(null, ItemOperateEnum.None);
                }
            }

            List<BagInfo> bagInfos = bagComponent.GetItemsByLoc(ItemLocType.ItemLocBag);
            for (int i = 0; i < bagInfos.Count; i++)
            {
                if (bagInfos[i].BagInfoID == self.MainBagInfo.BagInfoID)
                {
                    continue;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(bagInfos[i].ItemID);
                if (itemConfig.ItemType == ItemTypeEnum.Equipment && itemConfig.EquipType == 201)
                {
                    UIItemComponent uI = null;
                    if (number < self.ItemList.Count)
                    {
                        uI = self.ItemList[number];
                        uI.GameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject go = GameObject.Instantiate(self.UIItem);
                        UICommonHelper.SetParent(go, self.ItemListNode);
                        go.SetActive(true);
                        uI = self.AddChild<UIItemComponent, GameObject>(go);
                        uI.SetClickHandler((bagInfo) => { self.OnSelect(bagInfo); });
                        self.ItemList.Add(uI);
                    }

                    number++;
                    uI.UpdateItem(bagInfos[i], ItemOperateEnum.None);
                }
            }

            for (int i = number; i < self.ItemList.Count; i++)
            {
                self.ItemList[i].UpdateItem(null, ItemOperateEnum.None);
            }
        }

        public static void OnSelect(this UISeasonJingHeZhuruComponent self, BagInfo bagInfo)
        {
            bool selected = false;
            for (int i = 0; i < self.ItemList.Count; i++)
            {
                if (self.ItemList[i].Baginfo != null && self.ItemList[i].Baginfo.BagInfoID == bagInfo.BagInfoID)
                {
                    selected = !self.ItemList[i].Image_XuanZhong.activeSelf;
                    self.ItemList[i].Image_XuanZhong.SetActive(selected);

                    List<int> valuerange = ItemHelper.GetJingHeAddQulity(new List<int>() { int.Parse(bagInfo.ItemPar) });
                    if (selected)
                    {
                        if (!self.CostIds.Contains(bagInfo.BagInfoID))
                        {
                            self.CostIds.Add(bagInfo.BagInfoID);
                        }

                        self.MinAdd += valuerange[0];
                        self.MaxAdd += valuerange[1];
                    }
                    else
                    {
                        self.CostIds.Remove(bagInfo.BagInfoID);
                        self.MinAdd -= valuerange[0];
                        self.MaxAdd -= valuerange[1];
                    }

                    break;
                }
            }

            self.AddQualityText.GetComponent<Text>().text = $"{self.MinAdd}~{self.MaxAdd}";
        }
    }
}