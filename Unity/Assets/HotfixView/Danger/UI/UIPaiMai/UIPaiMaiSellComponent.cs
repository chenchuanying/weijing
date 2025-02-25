﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace ET
{
    public class UIPaiMaiSellComponent : Entity, IAwake
    {
        public GameObject SellTypeButton;
        public GameObject Btn_Stall;
        public GameObject Lab_ShengYuTime;
        public GameObject Text_SellTime;
        public GameObject Btn_XiaJia;
        public GameObject Btn_ShangJia;
        public GameObject ItemListNode;
        public GameObject PaiMaiListNode;
        public GameObject UIPaiMaiSellItem;
        public Text PaiMaiGoldText;

        public List<UIItemComponent> BagItemUILIist = new List<UIItemComponent>();
        public List<UIPaiMaiSellItemComponent> SellItemUILIist = new List<UIPaiMaiSellItemComponent>();

        public UIPageButtonComponent UIPageButton;

        public List<PaiMaiItemInfo> PaiMaiItemInfos = new List<PaiMaiItemInfo>();
        public long PaiMaiItemInfoId;

        public BagComponent BagComponent;
        public BagInfo BagInfo;

        public bool IsHoldDown;
    }


    public class UIPaiMaiSellComponentAwakeSystem : AwakeSystem<UIPaiMaiSellComponent>
    {
        public override void Awake(UIPaiMaiSellComponent self)
        {
            self.BagItemUILIist.Clear();
            self.SellItemUILIist.Clear();
            self.PaiMaiItemInfos.Clear();
            self.PaiMaiItemInfoId = 0;
            self.BagInfo = null;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Lab_ShengYuTime = rc.Get<GameObject>("Lab_ShengYuTime");
            self.Text_SellTime = rc.Get<GameObject>("Text_SellTime");

            self.Btn_XiaJia = rc.Get<GameObject>("Btn_XiaJia");
            self.Btn_XiaJia.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_XiaJia().Coroutine(); });

            self.Btn_ShangJia = rc.Get<GameObject>("Btn_ShangJia");
            self.Btn_ShangJia.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_ShangJia().Coroutine(); });

            self.ItemListNode = rc.Get<GameObject>("ItemListNode");
            self.PaiMaiListNode = rc.Get<GameObject>("PaiMaiListNode");
            self.UIPaiMaiSellItem = rc.Get<GameObject>("UIPaiMaiSellItem");
            self.UIPaiMaiSellItem.SetActive(false);

            self.PaiMaiGoldText = rc.Get<GameObject>("PaiMaiGoldText").GetComponent<Text>();

            self.Btn_Stall = rc.Get<GameObject>("Btn_Stall");
            self.Btn_Stall.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_Stall(); });

            self.GetParent<UI>().OnUpdateUI = () => { self.OnUpdateUI(); };

            self.BagComponent = self.ZoneScene().GetComponent<BagComponent>();

            self.SellTypeButton = rc.Get<GameObject>("SellTypeButton");
            UI uIPageButton = self.AddChild<UI, string, GameObject>("FunctionSetBtn", self.SellTypeButton);
            UIPageButtonComponent uIPageButtonComponent = uIPageButton.AddComponent<UIPageButtonComponent>();
            uIPageButtonComponent.SetClickHandler((int page) => {
                self.OnClickPageButton(page);
            });
            self.UIPageButton = uIPageButtonComponent;
        }
    }

    public static class UIPaiMaiSellComponentSystem
    {


        public static async ETTask RequestSelfPaiMaiList(this UIPaiMaiSellComponent self)
        {
            long instanceid = self.InstanceId;
            C2P_PaiMaiListRequest c2M_PaiMaiBuyRequest = new C2P_PaiMaiListRequest()
            {
                PaiMaiType = 0,
                UserId = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.UserId
            };
            P2C_PaiMaiListResponse m2C_PaiMaiBuyResponse = (P2C_PaiMaiListResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_PaiMaiBuyRequest);
            if (self.IsDisposed || instanceid != self.InstanceId)
            {
                return;
            }
            if (m2C_PaiMaiBuyResponse.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            int zone = self.ZoneScene().GetComponent<AccountInfoComponent>().ServerId;
            int openday = ServerHelper.GetOpenServerDay(false, zone);

            self.PaiMaiItemInfos = m2C_PaiMaiBuyResponse.PaiMaiItemInfos;
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());    
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            long sellgold = numericComponent.GetAsLong(NumericType.PaiMaiTodayGold);
            long todayGold = ConfigHelper.GetPaiMaiTodayGold(openday);

            float sellgold_1 = sellgold * 0.0001f;
            float todayGold_1 = todayGold * 0.0001f;

            self.PaiMaiGoldText.text = $"今日获利:{UICommonHelper.ShowFloatValue_1(sellgold_1)}万/{UICommonHelper.ShowFloatValue_1(todayGold_1)}万";
            self.UpdateSellItemUILIist(self.UIPageButton.CurrentIndex);
        }

        public static void OnUpdateUI(this UIPaiMaiSellComponent self)
        {
            self.UpdateBagItemUIList().Coroutine();
            self.UIPageButton.OnSelectIndex(0);
            self.RequestSelfPaiMaiList().Coroutine();
        }

        public static void OnClickPageButton(this UIPaiMaiSellComponent self, int page)
        {
            self.UpdateSellItemUILIist(page);
        }

        public static void OnBtn_Stall(this UIPaiMaiSellComponent self)
        {
            
        }

        public static async ETTask OnBtn_XiaJia(this UIPaiMaiSellComponent self)
        {
            if (self.PaiMaiItemInfoId == 0)
            {
                FloatTipManager.Instance.ShowFloatTip("请选中道具");
                return;
            }

            int itemType = 0;
            for (int i = self.PaiMaiItemInfos.Count - 1; i >= 0; i--)
            {
                if (self.PaiMaiItemInfos[i].Id == self.PaiMaiItemInfoId)
                {
                    if (self.PaiMaiItemInfos[i].UserId != self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.UserId)
                    {
                        FloatTipManager.Instance.ShowFloatTip("数据错误!");
                        return;
                    }
                    itemType = ItemConfigCategory.Instance.Get(self.PaiMaiItemInfos[i].BagInfo.ItemID).ItemType;
                }
            }


            C2M_PaiMaiXiaJiaRequest c2M_PaiMaiBuyRequest = new C2M_PaiMaiXiaJiaRequest() { ItemType = itemType,  PaiMaiItemInfoId = self.PaiMaiItemInfoId };
            M2C_PaiMaiXiaJiaResponse m2C_PaiMaiBuyResponse = (M2C_PaiMaiXiaJiaResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_PaiMaiBuyRequest);
            if (self.IsDisposed)
            {
                return;
            }

            for (int i = self.PaiMaiItemInfos.Count - 1; i >= 0; i--)
            {
                if (self.PaiMaiItemInfos[i].Id == self.PaiMaiItemInfoId)
                {
                    self.PaiMaiItemInfos.RemoveAt(i);
                }
            }
            self.PaiMaiItemInfoId = 0;

            self.UpdateBagItemUIList().Coroutine();
            self.UpdateSellItemUILIist(self.UIPageButton.CurrentIndex);
        }

        public static async ETTask OnBtn_ShangJia(this UIPaiMaiSellComponent self)
        {
            if (self.BagInfo == null)
            {
                FloatTipManager.Instance.ShowFloatTip("请选中道具！");
                return;
            }

            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.BagInfo.ItemID);
            if (itemConfig.IfStopPaiMai  == 1)
            {
                FloatTipManager.Instance.ShowFloatTip("此道具禁止上架！");
                return;
            }
            if (!ComHelp.IsShowPaiMai(itemConfig.ItemType, itemConfig.ItemSubType))
            {
                FloatTipManager.Instance.ShowFloatTip("此道具不能上架！");
                return;
            }
            if (self.PaiMaiItemInfos.Count >= GlobalValueConfigCategory.Instance.Get(50).Value2)
            {
                FloatTipManager.Instance.ShowFloatTip("已经达到最大上架数量！");
                return;
            }

            UI uI = await UIHelper.Create(self.DomainScene(), UIType.UIPaiMaiSellPrice);
            uI.GetComponent<UIPaiMaiSellPriceComponent>().InitPriceUI(self.BagInfo);
        }

        public static async ETTask UpdateBagItemUIList(this UIPaiMaiSellComponent self)
        {
            long instanceid = self.InstanceId;
            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            if (instanceid != self.InstanceId)
            {
                return;
            }

            int number = 0;
            List<BagInfo> equipInfos = self.BagComponent.GetBagList();
            for (int i = 0; i < equipInfos.Count; i++)
            {
                if (equipInfos[i].isBinging || equipInfos[i].IsProtect)
                {
                    continue;
                }
                UIItemComponent uI = null;
                if (number < self.BagItemUILIist.Count)
                {
                    uI = self.BagItemUILIist[number];
                    uI.GameObject.SetActive(true);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(bundleGameObject);
                    UICommonHelper.SetParent(go, self.ItemListNode);
                    go.transform.localScale = Vector3.one * 1f;
                    uI = self.AddChild<UIItemComponent, GameObject>( go);
                    uI.HideItemName();
                    uI.SetClickHandler((BagInfo baginfo) => { self.OnSelectItem(baginfo); });
                    //uIItemComponent.SetEventTrigger(true);
                    //uIItemComponent.PointerDownHandler = (BagInfo binfo, PointerEventData pdata) => { self.OnPointerDown(binfo, pdata).Coroutine(); };
                    //uIItemComponent.PointerUpHandler = (BagInfo binfo, PointerEventData pdata) => { self.OnPointerUp(binfo, pdata); };
                    self.BagItemUILIist.Add(uI);
                }
                number++;
                uI.UpdateItem(equipInfos[i], ItemOperateEnum.PaiMaiSell);
            }
            for (int i = number; i < self.BagItemUILIist.Count; i++)
            {
                self.BagItemUILIist[i].GameObject.SetActive(false);
            }
        }

        public static void OnPaiBuyShangJia(this UIPaiMaiSellComponent self, PaiMaiItemInfo paiMaiItemInfo)
        {
            self.BagInfo = null;                            //选中置空
            self.PaiMaiItemInfos.Add(paiMaiItemInfo);       //增加拍卖行出售的列表

            self.UpdateBagItemUIList().Coroutine();
            self.UpdateSellItemUILIist(self.UIPageButton.CurrentIndex);
        }

        public static void OnSelectItem(this UIPaiMaiSellComponent self, BagInfo bagInfo)
        {
            self.BagInfo = bagInfo;

            //增加选中状态
            for (int i = 0; i < self.BagItemUILIist.Count; i++)
            {
                self.BagItemUILIist[i].SetSelected(bagInfo);
            }
        }

        public static void OnSelectSellItem(this UIPaiMaiSellComponent self, PaiMaiItemInfo paiMaiItemInfo)
        {
            self.PaiMaiItemInfoId = paiMaiItemInfo.Id;

            for (int i = 0; i < self.SellItemUILIist.Count; i++)
            {
                self.SellItemUILIist[i].SetSelected(paiMaiItemInfo.Id);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="subType">0 装备   1其他</param>
        /// <returns></returns>
        public static  void UpdateSellItemUILIist(this UIPaiMaiSellComponent self, int subType)
        {
           
            int number = 0;
            for (int i = 0; i < self.PaiMaiItemInfos.Count; i++)
            {
                PaiMaiItemInfo paiMaiItemInfo =  self.PaiMaiItemInfos[i];
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(paiMaiItemInfo.BagInfo.ItemID);
                if (subType == 1 && itemConfig.ItemType != ItemTypeEnum.Equipment)
                {
                    continue;
                }
                if (subType == 2 &&  itemConfig.ItemType == ItemTypeEnum.Equipment)
                {
                    continue;
                }

                UIPaiMaiSellItemComponent uI = null;
                if (number < self.SellItemUILIist.Count)
                {
                    uI = self.SellItemUILIist[number];
                    uI.GameObject.SetActive(true);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(self.UIPaiMaiSellItem);
                    go.SetActive(true);
                    UICommonHelper.SetParent(go, self.PaiMaiListNode);
                    go.transform.localScale = Vector3.one * 1f;
                    uI = self.AddChild<UIPaiMaiSellItemComponent, GameObject>(go);
                    uI.SetClickHandler((PaiMaiItemInfo bagInfo) => { self.OnSelectSellItem(bagInfo); });
                    self.SellItemUILIist.Add(uI);
                }
                uI.OnUpdateUI(paiMaiItemInfo);
                number++;
            }
            for (int i = number; i < self.SellItemUILIist.Count; i++)
            {
                self.SellItemUILIist[i].GameObject.SetActive(false);
            }

            //显示上架数量
            int maxNum =  GlobalValueConfigCategory.Instance.Get(50).Value2;
            self.Text_SellTime.GetComponent<Text>().text = "已上架:"  + $"{self.PaiMaiItemInfos.Count}/{maxNum}";
        }
        
        public static async ETTask OnPointerDown(this UIPaiMaiSellComponent self, BagInfo binfo, PointerEventData pdata)
        {
            self.IsHoldDown = true;
            self.OnSelectItem(binfo);

            long instanceId = self.InstanceId;
            await TimerComponent.Instance.WaitAsync(100);
            if (instanceId != self.InstanceId || !self.IsHoldDown)
                return;
            EventType.ShowItemTips.Instance.ZoneScene = self.DomainScene();
            EventType.ShowItemTips.Instance.bagInfo = binfo;
            EventType.ShowItemTips.Instance.itemOperateEnum = ItemOperateEnum.None;
            EventType.ShowItemTips.Instance.inputPoint = Input.mousePosition;
            EventType.ShowItemTips.Instance.Occ = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Occ;
            Game.EventSystem.PublishClass(EventType.ShowItemTips.Instance);
        }

        public static void OnPointerUp(this UIPaiMaiSellComponent self, BagInfo binfo, PointerEventData pdata)
        {
            self.IsHoldDown = false;
            UIHelper.Remove(self.DomainScene(), UIType.UIEquipDuiBiTips);
        }
        
    }

}
