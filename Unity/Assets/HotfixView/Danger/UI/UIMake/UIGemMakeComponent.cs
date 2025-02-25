﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ET
{
    [Timer(TimerType.MakeCDTimer)]
    public class GemMakeTimer : ATimer<UIGemMakeComponent>
    {
        public override void Run(UIGemMakeComponent self)
        {
            try
            {
                self.OnUpdate();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    public class UIGemMakeComponent : Entity, IAwake,IDestroy
    {
        public GameObject TextVitality;
        public GameObject ImageSelect;
        public GameObject Text_Current;
        public GameObject Lab_MakeNum;
        public GameObject Lab_MakeName;
        public GameObject UIItemMake;
        public GameObject Btn_Make;
        public GameObject NeedListNode;
        public GameObject MakeINeedNode;
        public GameObject MakeListNode;
        public GameObject Lab_MakeCDTime;

        public List<UIMakeItemComponent> MakeListUI = new List<UIMakeItemComponent>();
        public List<UIMakeNeedComponent> NeedListUI = new List<UIMakeNeedComponent>();
        public UIItemComponent MakeItemUI;
        public int MakeId;
        public long Timer;
    }

    public class UIGemMakeComponentDestroySystem : DestroySystem<UIGemMakeComponent>
    {
        public override void Destroy(UIGemMakeComponent self)
        {
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }

    public class UIGemMakeMakeComponentAwakeSystem : AwakeSystem<UIGemMakeComponent>
    {
        public override void Awake(UIGemMakeComponent self)
        {
            self.MakeId = 0;
            self.MakeItemUI = null;
            self.NeedListUI.Clear();
            self.MakeListUI.Clear();

            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.ImageSelect = rc.Get<GameObject>("ImageSelect");
            self.ImageSelect.SetActive(false);
            self.Text_Current = rc.Get<GameObject>("Text_Current");
            self.TextVitality = rc.Get<GameObject>("TextVitality");
            self.Lab_MakeName = rc.Get<GameObject>("Lab_MakeName");
            self.Lab_MakeNum = rc.Get<GameObject>("Lab_MakeNum"); self.Lab_MakeCDTime = rc.Get<GameObject>("Lab_MakeCDTime");
            self.Lab_MakeCDTime = rc.Get<GameObject>("Lab_MakeCDTime");

            self.Btn_Make = rc.Get<GameObject>("Btn_Make");
            ButtonHelp.AddListenerEx(self.Btn_Make, () => { self.OnBtn_Make().Coroutine(); });

            self.NeedListNode = rc.Get<GameObject>("NeedListNode");

            self.UIItemMake = rc.Get<GameObject>("UIItemMake");

            self.MakeINeedNode = rc.Get<GameObject>("MakeINeedNode");
            self.MakeListNode = rc.Get<GameObject>("MakeListNode");

            self.OnInitUI();
            int showValue = NpcConfigCategory.Instance.Get(UIHelper.CurrentNpcId).ShopValue;
            self.UpdateMakeList(showValue).Coroutine();
        }
    }

    public static class UIGemMakeMakeComponentSystem
    {

        public static  void OnInitUI(this UIGemMakeComponent self)
        {
            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
            var bundleGameObject =  ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject bagSpace = GameObject.Instantiate(bundleGameObject);
            UICommonHelper.SetParent(bagSpace, self.UIItemMake);
            UIItemComponent uIItemComponent = self.AddChild<UIItemComponent, GameObject>(bagSpace);
            uIItemComponent.Label_ItemNum.SetActive(false);
            uIItemComponent.Label_ItemName.SetActive(false);
            self.MakeItemUI = uIItemComponent;
            self.UpateMakeItemUI();
        }

        public static async ETTask OnBtn_Make(this UIGemMakeComponent self)
        {
            if (self.MakeId == 0)
            {
                return;
            }
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            long cdEndTime = userInfoComponent.GetMakeTime(self.MakeId);
            if (cdEndTime > TimeHelper.ServerNow())
            {
                FloatTipManager.Instance.ShowFloatTip(ErrorHelp.Instance.ErrorHintList[ErrorCode.ERR_InMakeCD]);
                return;
            }

            EquipMakeConfig equipMakeConfig = EquipMakeConfigCategory.Instance.Get(self.MakeId);
            if (self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Gold < equipMakeConfig.MakeNeedGold)
            {
                FloatTipManager.Instance.ShowFloatTip("金币不足！");
                return;
            }

            bool success = self.ZoneScene().GetComponent<BagComponent>().CheckNeedItem(equipMakeConfig.NeedItems);
            if (!success)
            {
                FloatTipManager.Instance.ShowFloatTip("材料不足！");
                return;
            }

            await NetHelper.RequestEquipMake(self.ZoneScene(), 0, self.MakeId, 1);
            self.OnCostItemUpdate().Coroutine();
        }

        public static void UpateMakeItemUI(this UIGemMakeComponent self)
        {
            if (self.MakeId == 0)
            {
                self.MakeItemUI.GameObject.SetActive(false);
                return;
            }
            self.MakeItemUI.GameObject.SetActive(true);
            EquipMakeConfig equipMakeConfig = EquipMakeConfigCategory.Instance.Get(self.MakeId);
            if (self.MakeItemUI != null)
            {
                self.MakeItemUI.UpdateItem(new BagInfo() { ItemID = equipMakeConfig.MakeItemID }, ItemOperateEnum.MakeItem);
                self.MakeItemUI.Label_ItemNum.SetActive(false);
            }
        }

        public static async ETTask OnCostItemUpdate(this UIGemMakeComponent self)
        {
            EquipMakeConfig equipMakeConfig = EquipMakeConfigCategory.Instance.Get(self.MakeId);
            long instanceid = self.InstanceId;
            var path = ABPathHelper.GetUGUIPath("Main/Make/UIMakeNeed");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            if (instanceid != self.InstanceId)
            {
                return;
            }

            string needItems = equipMakeConfig.NeedItems;
            string[] itemsList = needItems.Split('@');

            //显示名称
            self.Lab_MakeName.GetComponent<Text>().text = ItemConfigCategory.Instance.Get(equipMakeConfig.MakeItemID).ItemName;
            self.Lab_MakeName.GetComponent<Text>().color = UICommonHelper.QualityReturnColor(ItemConfigCategory.Instance.Get(equipMakeConfig.MakeItemID).ItemQuality);
            self.Lab_MakeNum.GetComponent<Text>().text = equipMakeConfig.MakeEquipNum.ToString();

            //显示消耗活力
            self.TextVitality.GetComponent<Text>().text = equipMakeConfig.MakeNeedGold.ToString();
            self.Text_Current.GetComponent<Text>().text = $"当前金币:  {self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Gold}";

            for (int i = 0; i < itemsList.Length; i++)
            {
                UIMakeNeedComponent ui_2 = null;
                if (i < self.NeedListUI.Count)
                {
                    ui_2 = self.NeedListUI[i];
                    ui_2.GameObject.SetActive(true);
                }
                else
                {
                    GameObject itemSpace = GameObject.Instantiate(bundleGameObject);
                    itemSpace.SetActive(true);
                    UICommonHelper.SetParent(itemSpace, self.NeedListNode);
                    ui_2 = self.AddChild<UIMakeNeedComponent, GameObject>(itemSpace);
                    self.NeedListUI.Add(ui_2);
                }

                string[] itemInfo = itemsList[i].Split(';');
                int itemId = int.Parse(itemInfo[0]);
                int itemNum = int.Parse(itemInfo[1]);
                ui_2.UpdateItem(itemId, itemNum);
            }

            for (int k = itemsList.Length; k < self.NeedListUI.Count; k++)
            {
                self.NeedListUI[k].GameObject.SetActive(false);
            }
        }

        public static void OnUpdate(this UIGemMakeComponent self)
        {
            int makeId = self.MakeId;
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            long cdEndTime = userInfoComponent.GetMakeTime(makeId);
            if (cdEndTime <= TimeHelper.ServerNow())
            {
                self.Lab_MakeCDTime.SetActive(false);
                TimerComponent.Instance?.Remove(ref self.Timer);
                return;
            }
            self.Lab_MakeCDTime.GetComponent<Text>().text = TimeHelper.ShowLeftTime(cdEndTime - TimeHelper.ServerNow());
        }

        public static void ShowCDTime(this UIGemMakeComponent self)
        {
            self.Lab_MakeCDTime.SetActive(false);
            TimerComponent.Instance?.Remove(ref self.Timer);
            int makeId = self.MakeId;
            if (makeId == 0)
            {
                return;
            }
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            long cdEndTime = userInfoComponent.GetMakeTime(makeId);
            if (cdEndTime <= TimeHelper.ServerNow())
            {
                return;
            }
            self.Lab_MakeCDTime.SetActive(true);
            self.Timer = TimerComponent.Instance.NewRepeatedTimer(1000, TimerType.MakeCDTimer, self);
            self.OnUpdate();
        }


        public static void OnSelectMakeItem(this UIGemMakeComponent self, int makeid)
        {
            self.MakeId = makeid;
            self.MakeINeedNode.SetActive(makeid!=0);
            self.ShowCDTime();
            self.UpateMakeItemUI();
            self.OnCostItemUpdate().Coroutine();

            //设置选中框
            for (int k = 0; k < self.MakeListUI.Count; k++)
            {
                if (self.MakeListUI[k].MakeID == makeid)
                {
                    self.ImageSelect.SetActive(true);
                    UICommonHelper.SetParent(self.ImageSelect, self.MakeListUI[k].GameObject);
                    self.ImageSelect.transform.localPosition = new Vector3(0f, 12f, 0f);
                    break;
                }
            }
        }

        public static async ETTask UpdateMakeList(this UIGemMakeComponent self, int makeType)
        {
            int number = 0;
            var path = ABPathHelper.GetUGUIPath("Main/Make/UIMakeItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            List<EquipMakeConfig> makeList = EquipMakeConfigCategory.Instance.GetAll().Values.ToList();

            for (int k = 0; k < self.MakeListUI.Count; k++)
            {
                self.MakeListUI[k].GameObject.SetActive(false);
            }
            for (int i = 0; i < makeList.Count; i++)
            {
                EquipMakeConfig equipMakeConfig = makeList[i];
                if (equipMakeConfig.ProficiencyType != makeType)
                {
                    continue;
                }
                UIMakeItemComponent ui_2 = null;
                if (i < self.MakeListUI.Count)
                {
                    ui_2 = self.MakeListUI[number];
                    ui_2.GameObject.SetActive(true);
                }
                else
                {
                    GameObject itemSpace = GameObject.Instantiate(bundleGameObject);
                    itemSpace.SetActive(true);
                    UICommonHelper.SetParent(itemSpace, self.MakeListNode);
                    ui_2 = self.AddChild<UIMakeItemComponent, GameObject>(itemSpace);
                    ui_2.SetClickAction((int itemid) => { self.OnSelectMakeItem(itemid); });
                    self.MakeListUI.Add(ui_2);
                }
                number++;
                ui_2.OnUpdateUI(equipMakeConfig.Id);
            }

            self.OnSelectMakeItem(number == 0 ? 0 : self.MakeListUI[0].MakeID);
        }
    }
}
