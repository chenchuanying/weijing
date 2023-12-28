﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIWarehouseRoleComponent: Entity, IAwake, IDestroy
    {
        public GameObject BtnItemTypeSet;
        public GameObject BuildingList1;
        public GameObject BuildingList2;
        public GameObject ButtonPack;
        public GameObject ButtonQuick;

        public BagComponent BagComponent;
        public UIPageButtonComponent UIPageComponent;

        public List<UIItemComponent> BagList = new List<UIItemComponent>();
        public List<UIItemComponent> HouseList = new List<UIItemComponent>();

        public List<GameObject> LockList = new List<GameObject>();
        public List<GameObject> NoLockList = new List<GameObject>();
    }

    public class UIWarehouseRoleComponentAwakeSystem: AwakeSystem<UIWarehouseRoleComponent>
    {
        public override void Awake(UIWarehouseRoleComponent self)
        {
            self.BagList.Clear();
            self.HouseList.Clear();
            self.LockList.Clear();
            self.NoLockList.Clear();

            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.ButtonPack = rc.Get<GameObject>("ButtonPack");
            self.ButtonPack.GetComponent<Button>().onClick.AddListener(self.OnBtn_ZhengLi);

            self.ButtonQuick = rc.Get<GameObject>("ButtonQuick");
            self.ButtonQuick.GetComponent<Button>().onClick.AddListener(() => { self.OnButtonQuick().Coroutine(); });

            self.BuildingList1 = rc.Get<GameObject>("BuildingList1");
            self.BuildingList2 = rc.Get<GameObject>("BuildingList2");

            self.LockList.Add(rc.Get<GameObject>("Lock_1"));
            self.LockList.Add(rc.Get<GameObject>("Lock_2"));
            self.LockList.Add(rc.Get<GameObject>("Lock_3"));
            self.LockList.Add(rc.Get<GameObject>("Lock_4"));
            self.NoLockList.Add(rc.Get<GameObject>("NoLock_1"));
            self.NoLockList.Add(rc.Get<GameObject>("NoLock_2"));
            self.NoLockList.Add(rc.Get<GameObject>("NoLock_3"));
            self.NoLockList.Add(rc.Get<GameObject>("NoLock_4"));

            self.BagComponent = self.ZoneScene().GetComponent<BagComponent>();
            self.GetParent<UI>().OnUpdateUI = self.OnUpdateUI;

            //单选组件
            GameObject BtnItemTypeSet = rc.Get<GameObject>("BtnItemTypeSet");
            UI uiPage = self.AddChild<UI, string, GameObject>("BtnItemTypeSet", BtnItemTypeSet);
            UIPageButtonComponent pageButton = uiPage.AddComponent<UIPageButtonComponent>();
            self.UIPageComponent = pageButton;
            pageButton.CheckHandler = (int page) => { return self.CheckPageButton_1(page); };
            pageButton.SetClickHandler((int page) => { self.OnClickPageButton(page); });
            self.UIPageComponent.ClickEnabled = false;
            self.InitBagCell().Coroutine();
            self.UpdateLockList(0);

            DataUpdateComponent.Instance.AddListener(DataType.BagItemUpdate, self);
            DataUpdateComponent.Instance.AddListener(DataType.BuyBagCell, self);
        }
    }

    public class UIWarehouseRoleComponentDestroySystem: DestroySystem<UIWarehouseRoleComponent>
    {
        public override void Destroy(UIWarehouseRoleComponent self)
        {
            DataUpdateComponent.Instance.RemoveListener(DataType.BagItemUpdate, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.BuyBagCell, self);
        }
    }

    public static class UIWarehouseRoleComponentSystem
    {
        public static bool CheckPageButton_1(this UIWarehouseRoleComponent self, int page)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int cangkuNumber = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.CangKuNumber);
            if (cangkuNumber <= page)
            {
                string costItems = GlobalValueConfigCategory.Instance.Get(38).Value;
                PopupTipHelp.OpenPopupTip(self.ZoneScene(), "开启仓库",
                    $"是否消耗{UICommonHelper.GetNeedItemDesc(costItems)}开启一个仓库",
                    () => { self.RequestOpenCangKu().Coroutine(); }, null).Coroutine();
                return false;
            }

            return true;
        }

        public static void UpdateLockList(this UIWarehouseRoleComponent self, int page)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int cangkuNumber = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.CangKuNumber);
            for (int i = 0; i < self.LockList.Count; i++)
            {
                self.LockList[i].SetActive(cangkuNumber - 1 < i);
                self.NoLockList[i].SetActive(cangkuNumber - 1 >= i && i != page);
            }
        }

        public static async ETTask RequestOpenCangKu(this UIWarehouseRoleComponent self)
        {
            C2M_RoleOpenCangKuRequest request = new C2M_RoleOpenCangKuRequest();
            M2C_RoleOpenCangKuResponse response =
                    (M2C_RoleOpenCangKuResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            if (response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int cangkuNumber = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.CangKuNumber);
            self.UpdateLockList(cangkuNumber - 1);
            self.UIPageComponent.OnSelectIndex(cangkuNumber - 1);
        }

        public static async ETTask OnButtonQuick(this UIWarehouseRoleComponent self)
        {
            int itemType = self.UIPageComponent.GetCurrentIndex();
            int currentHouse = itemType + (int)ItemLocType.ItemWareHouse1;
            C2M_ItemQuickPutRequest request = new C2M_ItemQuickPutRequest() { HorseId = currentHouse };
            M2C_ItemQuickPutResponse response =
                    (M2C_ItemQuickPutResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
        }

        public static void OnBtn_ZhengLi(this UIWarehouseRoleComponent self)
        {
            int itemType = self.UIPageComponent.GetCurrentIndex();
            int currentHouse = itemType + (int)ItemLocType.ItemWareHouse1;
            self.ZoneScene().GetComponent<BagComponent>().SendSortByLoc((ItemLocType)currentHouse).Coroutine();
        }

        public static async ETTask InitBagCell(this UIWarehouseRoleComponent self)
        {
            var path = ABPathHelper.GetUGUIPath("Main/Role/UIItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            int bagcellNumber = self.BagComponent.GetTotalSpace();

            for (int i = 0; i < bagcellNumber; i++)
            {
                GameObject go = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(go, self.BuildingList2);

                UIItemComponent uiitem = self.AddChild<UIItemComponent, GameObject>(go);
                self.BagList.Add(uiitem);
            }

            int hourseNumber = GlobalValueConfigCategory.Instance.StoreMaxCell;
            for (int i = 0; i < hourseNumber; i++)
            {
                GameObject go = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(go, self.BuildingList1);

                UIItemComponent uiitem = self.AddChild<UIItemComponent, GameObject>(go);
                uiitem.Image_Lock.GetComponent<Button>().onClick.AddListener(self.OnClickImage_Lock);
                self.HouseList.Add(uiitem);
            }

            self.UIPageComponent.ClickEnabled = true;
            self.UIPageComponent.OnSelectIndex(0);

            self.UpdateBagList();
        }

        public static void OnBuyBagCell(this UIWarehouseRoleComponent self, string dataparams)
        {
            //int openell = self.BagComponent.WarehouseAddedCell[self.OpenIndex] + GlobalValueConfigCategory.Instance.StoreCapacity;
            //for (int i = 0; i < self.HouseList.Count; i++)
            //{
            //    self.HouseList[i].UpdateUnLock(i < openell);
            //}
            self.UpdateWareHouse();

            FloatTipManager.Instance.ShowFloatTip($"获得道具: {UICommonHelper.GetNeedItemDesc(dataparams)}");
        }

        public static void OnClickImage_Lock(this UIWarehouseRoleComponent self)
        {
            int curindex = self.UIPageComponent.GetCurrentIndex();
            int addcell = self.BagComponent.WarehouseAddedCell[curindex];
            BuyCellCost buyCellCost = ConfigHelper.BuyStoreCellCosts[curindex * 10 + addcell];
            //string costitems = GlobalValueConfigCategory.Instance.Get(83).Value;
            PopupTipHelp.OpenPopupTip(self.ZoneScene(), "购买格子",
                $"是否花费{UICommonHelper.GetNeedItemDesc(buyCellCost.Cost)}购买一个背包格子?",
                () => { self.ZoneScene().GetComponent<BagComponent>().SendBuyBagCell(curindex + 5).Coroutine(); }, null).Coroutine();
            return;
        }

        public static void OnClickPageButton(this UIWarehouseRoleComponent self, int page)
        {
            int itemType = self.UIPageComponent.GetCurrentIndex();
            self.BagComponent.CurrentHouse = itemType + (int)ItemLocType.ItemWareHouse1;

            self.UpdateWareHouse();
            self.UpdateLockList(itemType);
        }

        /// <summary>
        /// 刷新仓库
        /// </summary>
        /// <param name="self"></param>
        public static void UpdateWareHouse(this UIWarehouseRoleComponent self)
        {
            int curindex = self.UIPageComponent.GetCurrentIndex();

            int openell = self.BagComponent.WarehouseAddedCell[curindex] + GlobalValueConfigCategory.Instance.StoreCapacity;
            List<BagInfo> bagInfos = self.BagComponent.GetItemsByLoc((ItemLocType)self.BagComponent.CurrentHouse);

            for (int i = 0; i < self.HouseList.Count; i++)
            {
                if (i < bagInfos.Count)
                {
                    self.HouseList[i].UpdateItem(bagInfos[i], ItemOperateEnum.Cangku);
                }
                else
                {
                    self.HouseList[i].UpdateItem(null, ItemOperateEnum.None);
                }

                if (i < openell)
                {
                    self.HouseList[i].UpdateUnLock(true);
                }
                else
                {
                    self.HouseList[i].UpdateUnLock(false);
                    int addcell = self.BagComponent.WarehouseAddedCell[curindex] + (i - openell);
                    BuyCellCost buyCellCost = ConfigHelper.BuyStoreCellCosts[curindex * 10 + addcell];
                    int itemid = int.Parse(buyCellCost.Get.Split(';')[0]);
                    int itemnum = int.Parse(buyCellCost.Get.Split(';')[1]);
                    self.HouseList[i].UpdateItem(new BagInfo() { ItemID = itemid, ItemNum = itemnum }, ItemOperateEnum.None);
                }
            }
        }

        public static void OnUpdateUI(this UIWarehouseRoleComponent self)
        {
            self.UpdateBagList();
        }

        /// <summary>
        /// 刷新背包
        /// </summary>
        /// <param name="self"></param>
        public static void UpdateBagList(this UIWarehouseRoleComponent self)
        {
            List<BagInfo> bagInfos = self.BagComponent.GetItemsByLoc(ItemLocType.ItemLocBag);
            for (int i = 0; i < self.BagList.Count; i++)
            {
                if (i < bagInfos.Count)
                {
                    self.BagList[i].UpdateItem(bagInfos[i], ItemOperateEnum.CangkuBag);
                }
                else
                {
                    self.BagList[i].UpdateItem(null, ItemOperateEnum.None);
                }
            }
        }

        public static void UpdateBagUI(this UIWarehouseRoleComponent self)
        {
            if (self.HouseList.Count < 20)
            {
                return;
            }

            self.UpdateWareHouse();
            self.UpdateBagList();
        }
    }
}