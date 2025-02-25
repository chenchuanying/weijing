﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIJiaYuanWarehouseComponent:Entity, IAwake, IDestroy
    {
        public GameObject ButtonTakeOutAll;
        public GameObject ButtonOneKey;
        public GameObject BtnItemTypeSet;
        public GameObject BuildingList1;
        public GameObject BuildingList2;
        public GameObject ButtonPack;

        public BagComponent BagComponent;
        public JiaYuanComponent JiaYuanComponent;   
        public UIPageButtonComponent UIPageComponent;

        public List<UIItemComponent> BagList = new List<UIItemComponent>();
        public List<UIItemComponent> HouseList = new List<UIItemComponent>();

        public List<GameObject> LockList = new List<GameObject>();
        public List<GameObject> NoLockList = new List<GameObject>();
    }


    public class UIJiaYuanWarehouseComponentDestroy : DestroySystem<UIJiaYuanWarehouseComponent>
    {
        public override void Destroy(UIJiaYuanWarehouseComponent self)
        {
            DataUpdateComponent.Instance.RemoveListener(DataType.BagItemUpdate, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.BuyBagCell, self);
        }
    }


    public class UIJiaYuanWarehouseComponentAwake : AwakeSystem<UIJiaYuanWarehouseComponent>
    {
        public override void Awake(UIJiaYuanWarehouseComponent self)
        {
            self.BagList.Clear();
            self.HouseList.Clear();
            self.LockList.Clear();
            self.NoLockList.Clear();

            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.ButtonPack = rc.Get<GameObject>("ButtonPack");
            self.ButtonPack.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_ZhengLi().Coroutine(); });

            self.ButtonTakeOutAll = rc.Get<GameObject>("ButtonTakeOutAll");
            self.ButtonTakeOutAll.GetComponent<Button>().onClick.AddListener(() => { self.OnButtonTakeOutAll().Coroutine(); });
            
            self.ButtonOneKey = rc.Get<GameObject>("ButtonOneKey");
            self.ButtonOneKey.GetComponent<Button>().onClick.AddListener(() => { self.OnButtonOneKey().Coroutine(); });

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
            self.JiaYuanComponent = self.ZoneScene().GetComponent<JiaYuanComponent>();

            //单选组件
            GameObject BtnItemTypeSet = rc.Get<GameObject>("BtnItemTypeSet");
            UI uiPage = self.AddChild<UI, string, GameObject>("BtnItemTypeSet", BtnItemTypeSet);
            UIPageButtonComponent pageButton = uiPage.AddComponent<UIPageButtonComponent>();
            self.UIPageComponent = pageButton;
            pageButton.CheckHandler = (int page) => { return self.CheckPageButton_1(page); };
            pageButton.SetClickHandler((int page) => {
                self.OnClickPageButton(page);
            });
            self.UIPageComponent.ClickEnabled = false;
            self.InitBagCell().Coroutine();
            self.UpdateLockList(0);

            DataUpdateComponent.Instance.AddListener(DataType.BagItemUpdate, self);
            DataUpdateComponent.Instance.AddListener(DataType.BuyBagCell, self);
        }
    }

    public static class UIJiaYuanWarehouseComponentSystem
    {

        public static bool CheckPageButton_1(this UIJiaYuanWarehouseComponent self, int page)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int cangkuNumber = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.JianYuanCangKu);
            if (cangkuNumber <= page)
            {
                string costItems = JiaYuanHelper.GetOpenJiaYuanWarehouse(cangkuNumber);
                PopupTipHelp.OpenPopupTip(self.ZoneScene(), "开启仓库",
                    $"是否消耗{UICommonHelper.GetNeedItemDesc(costItems)}开启一个仓库",
                    () =>
                    {
                        self.RequestOpenCangKu().Coroutine();
                    }, null).Coroutine();
                return false;
            }
            return true;
        }

        public static void UpdateLockList(this UIJiaYuanWarehouseComponent self, int page)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int cangkuNumber = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.JianYuanCangKu);
            for (int i = 0; i < self.LockList.Count; i++)
            {
                self.LockList[i].SetActive(cangkuNumber - 1 < i);
                self.NoLockList[i].SetActive(cangkuNumber - 1 >= i && i != page);
            }
        }

        public static async ETTask RequestOpenCangKu(this UIJiaYuanWarehouseComponent self)
        {
            C2M_JiaYuanCangKuOpenRequest request = new C2M_JiaYuanCangKuOpenRequest();
            M2C_JiaYuanCangKuOpenResponse response = (M2C_JiaYuanCangKuOpenResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            if (response.Error != ErrorCode.ERR_Success)
            {
                return;
            }
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int cangkuNumber = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.JianYuanCangKu);
            self.UpdateLockList(cangkuNumber - 1);
            self.UIPageComponent.OnSelectIndex(cangkuNumber - 1);
        }

        public static async ETTask OnBtn_ZhengLi(this UIJiaYuanWarehouseComponent self)
        {
            int itemType = self.UIPageComponent.GetCurrentIndex();
            int currentHouse = itemType + (int)ItemLocType.JianYuanWareHouse1;
            await  self.ZoneScene().GetComponent<BagComponent>().SendSortByLoc((ItemLocType)currentHouse);
            self.UpdateWareHouse();
        }

        /// <summary>
        /// 一键取出
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask OnButtonTakeOutAll(this UIJiaYuanWarehouseComponent self)
        {
            C2M_TakeOutAllRequest request = new C2M_TakeOutAllRequest() { HorseId = self.BagComponent.CurrentHouse };
            M2C_TakeOutAllResponse response = await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request) as M2C_TakeOutAllResponse;
        }

        public static async ETTask OnButtonOneKey(this UIJiaYuanWarehouseComponent self)
        {
            C2M_JiaYuanStoreRequest  request    = new C2M_JiaYuanStoreRequest() { HorseId = self.BagComponent.CurrentHouse };
            M2C_JiaYuanStoreResponse response = (M2C_JiaYuanStoreResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
        }

        public static async ETTask InitBagCell(this UIJiaYuanWarehouseComponent self)
        {
            var path = ABPathHelper.GetUGUIPath("Main/Role/UIItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            int bagcellNumber = self.BagComponent.GetBagTotalCell();

            for (int i = 0; i < bagcellNumber; i++)
            {
                GameObject go = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(go, self.BuildingList2);

                UIItemComponent uiitem = self.AddChild<UIItemComponent, GameObject>(go);
                self.BagList.Add(uiitem);
            }

            int hourseNumber = GlobalValueConfigCategory.Instance.HourseInitCapacity;
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

        public static void OnBuyBagCell(this UIJiaYuanWarehouseComponent self, string dataparams)
        {
            self.UpdateWareHouse();

            FloatTipManager.Instance.ShowFloatTip($"获得道具: {UICommonHelper.GetNeedItemDesc(dataparams)}");
        }


        public static void OnClickImage_Lock(this UIJiaYuanWarehouseComponent self)
        {
            //int curindex = self.UIPageComponent.GetCurrentIndex();
            string costitems = GlobalValueConfigCategory.Instance.Get(83).Value;
            PopupTipHelp.OpenPopupTip(self.ZoneScene(), "购买格子",
                $"是否花费{UICommonHelper.GetNeedItemDesc(costitems)}购买一个背包格子?", () =>
                {

                }, null).Coroutine();
            return;
        }

        public static void OnClickPageButton(this UIJiaYuanWarehouseComponent self, int page)
        {
            int itemType = self.UIPageComponent.GetCurrentIndex();
            self.BagComponent.CurrentHouse = itemType + (int)ItemLocType.JianYuanWareHouse1;

            self.UpdateWareHouse();
            self.UpdateLockList(itemType);
        }

        /// <summary>
        /// 刷新仓库
        /// </summary>
        /// <param name="self"></param>
        public static void UpdateWareHouse(this UIJiaYuanWarehouseComponent self)
        {
            int curindex = self.UIPageComponent.GetCurrentIndex();

            List<BagInfo> bagInfos = self.BagComponent.GetItemsByLoc((curindex + ItemLocType.JianYuanWareHouse1));
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
            }
        }

        /// <summary>
        /// 刷新背包
        /// </summary>
        /// <param name="self"></param>
        public static void UpdateBagList(this UIJiaYuanWarehouseComponent self)
        {
            List<BagInfo> bagInfos = self.BagComponent.GetItemsByLoc(ItemLocType.ItemLocBag);
            List<BagInfo> seedlist = ItemHelper.GetSeedList(bagInfos);

            for (int i = 0; i < self.BagList.Count; i++)
            {
                if (i >= seedlist.Count)
                {
                    continue;
                }

                self.BagList[i].UpdateItem(seedlist[i], ItemOperateEnum.CangkuBag);
            }
            for (int i = seedlist.Count; i < self.BagList.Count; i++)
            {
                self.BagList[i].UpdateItem(null, ItemOperateEnum.None);
            }
        }

        public static void OnUpdateUI(this UIJiaYuanWarehouseComponent self)
        {
            if (self.HouseList.Count < GlobalValueConfigCategory.Instance.HourseInitCapacity)
            {
                return;
            }

            self.UpdateWareHouse();
            self.UpdateBagList();
        }

        public static void OnCloseWarehouse(this UIJiaYuanWarehouseComponent self)
        {
            UIHelper.Remove(self.DomainScene(), UIType.UIJiaYuanWarehouse);
        }

    }
}
