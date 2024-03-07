﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIPaiMaiBuyItemComponent: Entity, IAwake<GameObject>
    {
        /// <summary>
        /// 弹出下拉列表按钮
        /// </summary>
        public GameObject PDListBtn;

        public GameObject ButtonBuy;
        public GameObject Text_Owner;
        public GameObject Text_LeftTime;
        public GameObject Text_Price;
        public GameObject ItemNode;
        public GameObject Text_Name;

        public PaiMaiItemInfo PaiMaiItemInfo;
        public GameObject GameObject;

        public UIItemComponent ItemUI;
    }

    public class UIPaiMaiBuyItemComponentAwakeSystem: AwakeSystem<UIPaiMaiBuyItemComponent, GameObject>
    {
        public override void Awake(UIPaiMaiBuyItemComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            self.ItemUI = null;
            self.PaiMaiItemInfo = null;

            self.PDListBtn = rc.Get<GameObject>("PDListBtn");
            ButtonHelp.AddListenerEx(self.PDListBtn, () => { self.OnOpenPDList().Coroutine(); });

            self.ButtonBuy = rc.Get<GameObject>("ButtonBuy");
            self.ButtonBuy.GetComponent<Button>().onClick.AddListener(() => { self.OnClickButtonBuy().Coroutine(); });

            self.Text_Owner = rc.Get<GameObject>("Text_Owner");
            self.Text_LeftTime = rc.Get<GameObject>("Text_LeftTime");
            self.Text_Price = rc.Get<GameObject>("Text_Price");
            self.ItemNode = rc.Get<GameObject>("ItemNode");
            self.Text_Name = rc.Get<GameObject>("Text_Name");

            self.InitItemUI();
        }
    }

    public static class UIPaiMaiBuyItemComponentSystem
    {
        public static void InitItemUI(this UIPaiMaiBuyItemComponent self)
        {
            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);

            GameObject go = GameObject.Instantiate(bundleGameObject);
            UICommonHelper.SetParent(go, self.ItemNode);
            self.ItemUI = self.AddChild<UIItemComponent, GameObject>(go);
            self.ItemUI.Label_ItemName.SetActive(false);
            self.ItemUI.GameObject.transform.localScale = Vector3.one * 0.8f;

            if (self.PaiMaiItemInfo != null)
            {
                self.OnUpdateItem(self.PaiMaiItemInfo);
            }
        }

        /// <summary>
        /// 打开下拉列表
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask OnOpenPDList(this UIPaiMaiBuyItemComponent self)
        {
            UI uI = await UIHelper.Create(self.DomainScene(), UIType.UIWatchPaiMai);
            uI.GetComponent<UIWatchPaiMaiComponent>().OnUpdateUI(self.FastSeach);
        }

        public static void FastSeach(this UIPaiMaiBuyItemComponent self)
        {
            self.GetParent<UIPaiMaiBuyComponent>().InputField.GetComponent<InputField>().text = self.Text_Name.GetComponent<Text>().text;
        }

        public static async ETTask RequestBuy(this UIPaiMaiBuyItemComponent self)
        {
            long instanceId = self.InstanceId;
            C2M_PaiMaiBuyRequest c2M_PaiMaiBuyRequest = new C2M_PaiMaiBuyRequest() { PaiMaiItemInfo = self.PaiMaiItemInfo };
            M2C_PaiMaiBuyResponse m2C_PaiMaiBuyResponse =
                    (M2C_PaiMaiBuyResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_PaiMaiBuyRequest);
            if (instanceId != self.InstanceId)
            {
                return;
            }

            //隐藏显示

            if (m2C_PaiMaiBuyResponse.Error == 0)
            {
                if (self.GameObject != null)
                {
                    self.GameObject.SetActive(false);
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.PaiMaiItemInfo.BagInfo.ItemID);
                self.GetParent<UIPaiMaiBuyComponent>().RemoveItem(itemConfig.ItemType, self.PaiMaiItemInfo);
            }
            else
            {
                FloatTipManager.Instance.ShowFloatTip("道具已经被买走了！");
            }
        }

        public static async ETTask OnClickButtonBuy(this UIPaiMaiBuyItemComponent self)
        {
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.PaiMaiItemInfo.BagInfo.ItemID);
            // 橙色装备不能购买
            if (itemConfig.ItemQuality >= 5 && itemConfig.ItemType == 3)
            {
                FloatTipManager.Instance.ShowFloatTip("橙色品质及以上的装备不能购买！");
                return;
            }

            BagComponent bagComponent = self.ZoneScene().GetComponent<BagComponent>();
            if (bagComponent.GetBagLeftCell() < 1)
            {
                FloatTipManager.Instance.ShowFloatTip("背包空间不足");
                return;
            }

            bool canBuy = false;
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int openPaiMai = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.PaiMaiOpen);
            if (openPaiMai == 1)
            {
                canBuy = true;
            }

            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            int createDay = userInfoComponent.GetCrateDay();
            if (createDay <= 1 && userInfoComponent.UserInfo.Lv <= 10)
            {
                canBuy = true;
            }

            AccountInfoComponent accountInfo = self.ZoneScene().GetComponent<AccountInfoComponent>();
            if (ComHelp.IsCanPaiMai_Recharge(accountInfo.PlayerInfo))
            {
                canBuy = true;
            }

            if (ComHelp.IsCanPaiMai_KillBoss(userInfoComponent.UserInfo.MonsterRevives, userInfoComponent.UserInfo.Lv))
            {
                canBuy = true;
            }

            int needLv = ComHelp.IsCanPaiMai_Level(createDay, userInfoComponent.UserInfo.Lv);
            if (needLv == 0)
            {
                canBuy = true;
            }

            if (!canBuy)
            {
                FloatTipManager.Instance.ShowFloatTip($"等级需达到{needLv}级或赞助任意金额开启拍卖行购买功能！");
                return;
            }


            if (self.PaiMaiItemInfo.BagInfo.ItemNum > 1)
            {
                UI ui = await UIHelper.Create(self.ZoneScene(), UIType.UIPaiMaiBuyTip);
                ui.GetComponent<UIPaiMaiBuyTipComponent>()?.InitInfo(self.PaiMaiItemInfo, (int buyNum) =>
                {
                    if (buyNum < self.PaiMaiItemInfo.BagInfo.ItemNum)
                    {
                        self.PaiMaiItemInfo.BagInfo.ItemNum -= buyNum;
                        self.OnUpdateItem(self.PaiMaiItemInfo);
                    }
                    else
                    {
                        if (self.GameObject != null)
                        {
                            self.GameObject.SetActive(false);
                        }

                        ItemConfig itemConfig1 = ItemConfigCategory.Instance.Get(self.PaiMaiItemInfo.BagInfo.ItemID);
                        self.GetParent<UIPaiMaiBuyComponent>().RemoveItem(itemConfig1.ItemType, self.PaiMaiItemInfo);
                    }
                });
            }
            else
            {
                if (self.PaiMaiItemInfo.Price * self.PaiMaiItemInfo.BagInfo.ItemNum >= 500000)
                {
                    PopupTipHelp.OpenPopupTip(self.ZoneScene(), "购买道具",
                        $"你购买的道具需要花费{self.PaiMaiItemInfo.Price * self.PaiMaiItemInfo.BagInfo.ItemNum}金币，是否购买？",
                        () => { self.RequestBuy().Coroutine(); },
                        null).Coroutine();
                }
                else
                {
                    self.RequestBuy().Coroutine();
                }
            }
        }

        public static void OnUpdateItem(this UIPaiMaiBuyItemComponent self, PaiMaiItemInfo paiMaiItemInfo)
        {
            self.PaiMaiItemInfo = paiMaiItemInfo;
            if (paiMaiItemInfo == null || self.ItemUI == null)
                return;

            self.ItemUI.UpdateItem(paiMaiItemInfo.BagInfo, ItemOperateEnum.PaiMaiBuy);
            self.Text_Owner.GetComponent<Text>().text = paiMaiItemInfo.PlayerName;

            //显示名称
            FunctionUI.GetInstance().ItemObjShowName(self.Text_Name, self.PaiMaiItemInfo.BagInfo.ItemID);

            //显示价格
            int sumPrice = paiMaiItemInfo.Price * paiMaiItemInfo.BagInfo.ItemNum;
            self.Text_Price.GetComponent<Text>().text = sumPrice.ToString();

            //显示时间
            self.Text_LeftTime.GetComponent<Text>().text = TimeHelper.TimeToShowCostTimeStr(paiMaiItemInfo.SellTime, 48);

            //装备显示等级
            ItemConfig itemCof = ItemConfigCategory.Instance.Get(self.PaiMaiItemInfo.BagInfo.ItemID);
            if (itemCof.ItemType == 3)
            {
                self.ItemUI.Label_ItemNum.SetActive(true);
                self.ItemUI.Label_ItemNum.GetComponent<Text>().text = itemCof.UseLv + "级";
            }
        }
    }
}