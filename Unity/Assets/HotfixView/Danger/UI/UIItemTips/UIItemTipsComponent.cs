using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIItemTipsComponent : Entity, IAwake,IDestroy
    {
        public GameObject Lab_FuLingDes;
        public GameObject Lab_FuLing;
        public GameObject Btn_Split;
        public GameObject ImageQualityLine;
        public GameObject ImageQualityBg;
        public GameObject TextBtn_Use;
        public GameObject Obj_ItemQuality;
        public GameObject Obj_ItemIcon;
        public GameObject Lab_ItemName;
        public GameObject ItemDes;
        public GameObject ItemItemLv;
        public GameObject ItemType;
        public GameObject Img_back;
        public GameObject Obj_BagOpenSet;
        public GameObject Obj_Diu;
        public GameObject Obj_Btn_GemHoleText;
        public GameObject Obj_Btn_HuiShou;
        public GameObject Obj_Btn_HuiShouCancle;
        public GameObject Obj_Btn_XieXiaGemSet;
        public GameObject selPastureItemUICommonHint;
        public GameObject Obj_Lab_EquipBangDing;
        public GameObject Obj_Img_EquipBangDing;

        public GameObject Imagebg;

        public ItemOperateEnum ItemOpetateType;               //操作类型
        public BagInfo BagInfo;

        //按钮相关
        public GameObject Btn_Sell;
        public GameObject Btn_Use;

        public GameObject Btn_StoreHouse;    //放入仓库
        public GameObject Btn_PutBag;           //放入背包
        public BagComponent BagComponent;

        public Vector2 Img_backVector2;
        public float Lab_ItemNameWidth;
        public List<string> AssetPath = new List<string>();

        public long LastGemTime = 0;
    }

    public class UIItemTipsComponentAwakeSystem : AwakeSystem<UIItemTipsComponent>
    {
        public override void Awake(UIItemTipsComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Lab_FuLingDes = rc.Get<GameObject>("Lab_FuLingDes");
            self.Lab_FuLing = rc.Get<GameObject>("Lab_FuLing");
            self.ImageQualityLine = rc.Get<GameObject>("ImageQualityLine");
            self.ImageQualityBg = rc.Get<GameObject>("ImageQulityBg");
            self.TextBtn_Use = rc.Get<GameObject>("TextBtn_Use");
            self.Obj_Btn_XieXiaGemSet = rc.Get<GameObject>("Btn_XieXiaGem");
            self.Obj_Btn_HuiShouCancle = rc.Get<GameObject>("Btn_HuiShouCancle");
            self.Obj_Btn_HuiShou = rc.Get<GameObject>("Btn_HuiShou");
            self.Obj_Diu = rc.Get<GameObject>("Btn_Diu");
            self.ItemType = rc.Get<GameObject>("Lab_ItemType");
            self.ItemItemLv = rc.Get<GameObject>("Lab_ItemLv");
            self.ItemDes = rc.Get<GameObject>("Lab_ItemDes");
            self.Lab_ItemName = rc.Get<GameObject>("Lab_ItemName");
            self.Lab_ItemNameWidth = self.Lab_ItemName.GetComponent<RectTransform>().sizeDelta.x;
            self.Obj_ItemIcon = rc.Get<GameObject>("Image_ItemIcon");
            self.Obj_ItemQuality = rc.Get<GameObject>("Image_ItemQuality");
            self.Imagebg = rc.Get<GameObject>("Imagebg");
            self.Obj_BagOpenSet = rc.Get<GameObject>("BagOpenSet");
            self.Img_back = rc.Get<GameObject>("Img_back");
            self.Img_backVector2 = self.Img_back.GetComponent<RectTransform>().sizeDelta;

            self.Btn_Use = rc.Get<GameObject>("Btn_Use");
            self.Btn_Sell = rc.Get<GameObject>("Btn_Sell");
            self.Btn_Split = rc.Get<GameObject>("Btn_Split");
            self.Obj_Lab_EquipBangDing = rc.Get<GameObject>("Lab_BangDing");
            self.Obj_Img_EquipBangDing = rc.Get<GameObject>("Img_BangDing");

            ButtonHelp.AddListenerEx(self.Imagebg, self.OnCloseTips);
            ButtonHelp.AddListenerEx(self.Btn_Sell, () => { self.OnClickSell().Coroutine(); });
            ButtonHelp.AddListenerEx(self.Btn_Use, () => { self.OnClickUse().Coroutine(); });     
            ButtonHelp.AddListenerEx(self.Btn_Split, () => { self.OnBtn_Split().Coroutine(); });
           
            self.Btn_StoreHouse = rc.Get<GameObject>("Btn_StoreHouse");
            ButtonHelp.AddListenerEx(self.Btn_StoreHouse, self.OnBtn_StoreHouse);
            self.Btn_StoreHouse.SetActive(false);

            self.Btn_PutBag = rc.Get<GameObject>("Btn_PutBag");
            self.Btn_PutBag.GetComponent<Button>().onClick.AddListener(self.OnBtn_PutBag);
            self.Btn_PutBag.SetActive(false);

            ButtonHelp.AddListenerEx(self.Obj_Btn_HuiShouCancle, () => { self.OnBtn_HuiShouCancle(); });
            ButtonHelp.AddListenerEx(self.Obj_Btn_HuiShou, () => { self.On_Btn_HuiShou(); });
            ButtonHelp.AddListenerEx(self.Obj_Btn_XieXiaGemSet, () => { self.On_Btn_XieXiaGemSet(); });
            self.BagComponent = self.ZoneScene().GetComponent<BagComponent>();
        }
    }
    public class UIItemTipsComponentDestroy: DestroySystem<UIItemTipsComponent>
    {
        public override void Destroy(UIItemTipsComponent self)
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
    public static class UIItemTipsComponentSystem
    {
        public static void On_Btn_HuiShou(this UIItemTipsComponent self)
        {
            HintHelp.GetInstance().DataUpdate(DataType.HuiShouSelect, $"1_{self.BagInfo.BagInfoID}");
            self.OnCloseTips();
        }

        public static void OnBtn_HuiShouCancle(this UIItemTipsComponent self)
        {
            HintHelp.GetInstance().DataUpdate(DataType.HuiShouSelect, $"0_{self.BagInfo.BagInfoID}");
            self.OnCloseTips();
        }

        //卸下宝石
        public static void On_Btn_XieXiaGemSet(this UIItemTipsComponent self)
        {
            UI uiRole = UIHelper.GetUI(self.ZoneScene(), UIType.UIRole);
            if (uiRole == null)
            {
                return;
            }
            UIRoleComponent uIRoleComponent = uiRole.GetComponent<UIRoleComponent>();
            UIRoleGemComponent uIRoleGemComponent = uIRoleComponent.UIPageView.UISubViewList[(int)RolePageEnum.RoleGem].GetComponent<UIRoleGemComponent>();
            if (uIRoleGemComponent.XiangQianItem == null)
            {
                return;
            }

            self.BagComponent.SendXieXiaGem(uIRoleGemComponent.XiangQianItem, uIRoleGemComponent.XiangQianIndex.ToString()).Coroutine();

            self.OnCloseTips();
        }

        //放入仓库
        public static void OnBtn_StoreHouse(this UIItemTipsComponent self)
        {
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.BagInfo.ItemID);
            if (self.ItemOpetateType == ItemOperateEnum.GemBag && itemConfig.ItemType!= ItemTypeEnum.Gemstone)
            {
                FloatTipManager.Instance.ShowFloatTip("只能放入宝石！");
                return;
            }

            if (self.ItemOpetateType == ItemOperateEnum.AccountBag)
            {
                //ItemViewHelp.AccountCangkuPutIn( self.ZoneScene(), self.BagInfo );
            }
            else
            {
                self.BagComponent.SendPutStoreHouse(self.BagInfo).Coroutine();
            }

            self.OnCloseTips();
        }

        //放入背包
        public static void OnBtn_PutBag(this UIItemTipsComponent self)
        {
            if (self.ItemOpetateType == ItemOperateEnum.AccountCangku)
            {
                NetHelper.RequestAccountWarehousOperate(self.ZoneScene(), 2, self.BagInfo.BagInfoID).Coroutine();
            }
            else
            {
                self.BagComponent.SendPutBag(self.BagInfo).Coroutine();
            }

            self.OnCloseTips();
        }


        public static void Awake(this UIItemTipsComponent self)
        {

        }

        //点击Tips
        public static void OnCloseTips(this UIItemTipsComponent self)
        {
            if (self.IsDisposed)
            {
                return;
            }

            UIHelper.Remove(self.DomainScene(), UIType.UIItemTips);
        }

        //出售道具
        public static async ETTask OnClickSell(this UIItemTipsComponent self)
        {
            //发送消息
            //if (ItemConfigCategory.Instance.Get(self.BagInfo.ItemID).ItemQuality >= 4)
            //{
            //    ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.BagInfo.ItemID);
            //    PopupTipHelp.OpenPopupTip(self.ZoneScene(), "出售道具", "是否出售道具:" + itemConfig.ItemName + "\n 出售总价:" + itemConfig.SellMoneyValue * self.BagInfo.ItemNum, () =>
            //    {
            //        self.ZoneScene().GetComponent<BagComponent>().SendSellItem(self.BagInfo).Coroutine();
            //        //播放音效
            //        UIHelper.PlayUIMusic("10004");
            //        self.OnCloseTips();
            //    }).Coroutine();
            //}
            //else
            //{
            //    self.ZoneScene().GetComponent<BagComponent>().SendSellItem(self.BagInfo).Coroutine();
            //    //播放音效
            //    UIHelper.PlayUIMusic("10004");
            //    self.OnCloseTips();
            //}
            long intanceId = self.InstanceId;
            UI uI = await UIHelper.Create( self.ZoneScene(), UIType.UIItemSellTip );
            if (intanceId != self.InstanceId)
            {
                return;
            }
            uI.GetComponent<UIItemSellTipComponent>().OnInitUI( self.BagInfo );
            self.OnCloseTips();
        }

        public static async ETTask RequestXiangQianGem(this UIItemTipsComponent self, string usrPar)
        {
            if (TimeHelper.ServerNow() - self.LastGemTime < 2000)
            {
                return;
            }

            self.LastGemTime = TimeHelper.ServerNow();
            long intanceId = self.InstanceId;   
            await self.ZoneScene().GetComponent<BagComponent>().SendXiangQianGem(self.BagInfo, usrPar);
            if (intanceId != self.InstanceId)
            {
                return;
            }
            //注销Tips
            self.OnCloseTips();
        }

        public static async ETTask OnBtn_Split(this UIItemTipsComponent self)
        {
            UI uI = await UIHelper.Create(self.ZoneScene(), UIType.UIRoleBagSplit);
            uI.GetComponent<UIRoleBagSplitComponent>().OnInitUI(self.BagInfo);

            self.OnCloseTips();
        }

        public static async ETTask OnItemFumoUse(this UIItemTipsComponent self, ItemConfig itemConfig)
        {
            await ETTask.CompletedTask;
            string[] itemparams = itemConfig.ItemUsePar.Split('@');
            int weizhi = int.Parse(itemparams[0]);
            BagInfo equipinfo = self.ZoneScene().GetComponent<BagComponent>().GetEquipBySubType(ItemLocType.ItemLocEquip, weizhi);
            if (equipinfo == null)
            {
                FloatTipManager.Instance.ShowFloatTip("对应的位置没有装备！");
                return;
            }
            if (weizhi == (int)ItemSubTypeEnum.Shiping)
            {
                UI uI = await UIHelper.Create( self.ZoneScene(), UIType.UIItemFumoSelect );
                uI.GetComponent<UIItemFumoSelectComponent>().OnInitUI( self.BagInfo );
                self.OnCloseTips();
                return;
            }

            List<HideProList> hideProLists = XiLianHelper.GetItemFumoPro(itemConfig.Id);
            string itemfumo = ItemViewHelp.GetFumpProDesc(hideProLists);

            if (equipinfo.FumoProLists.Count > 0)
            {
                string equipfumo = ItemViewHelp.GetFumpProDesc(equipinfo.FumoProLists);
                string fumopro = $"当前附魔属性<color=#BEFF34>{equipfumo}</color> \n是否覆盖已有属性\n{itemfumo}\n此附魔道具已消耗";

                self.ZoneScene().GetComponent<BagComponent>().SendFumoUse(self.BagInfo, hideProLists).Coroutine();
                PopupTipHelp.OpenPopupTip(self.ZoneScene(), "装备附魔", fumopro, () =>
                {
                    self.ZoneScene().GetComponent<BagComponent>().SendFumoPro(0).Coroutine();
                    FloatTipManager.Instance.ShowFloatTip($"附魔属性 {itemfumo}");
                    self.OnCloseTips();
                }, () =>
                {
                    self.OnCloseTips();
                }).Coroutine();
            }
            else
            {
                await self.ZoneScene().GetComponent<BagComponent>().SendFumoUse(self.BagInfo, hideProLists);
                await self.ZoneScene().GetComponent<BagComponent>().SendFumoPro(0);
                FloatTipManager.Instance.ShowFloatTip($"附魔属性 {itemfumo}");

                self.OnCloseTips();
            }
            return;
        }

        //使用道具
        public static async ETTask OnClickUse(this UIItemTipsComponent self)
        {
            //发送消息
            //判断当前技能是否再CD状态
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.BagInfo.ItemID);
          
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int errorCode = ErrorCode.ERR_Success;
            string usrPar = "";

            //int addTime = 0;
            //if (itemConfig.Id == 10000134)
            //{
            //    NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            //    addTime = numericComponent.GetAsInt(NumericType.PetExtendNumber);
            //}

            if (itemConfig.DayUseNum > 0 && userInfoComponent.GetDayItemUse(itemConfig.Id) >= itemConfig.DayUseNum)
            {
                FloatTipManager.Instance.ShowFloatTip(ErrorHelp.Instance.ErrorHintList[ErrorCode.ERR_ItemNoUseTime]);
                return;
            }
            if (itemConfig.SumUseNum > 0 && userInfoComponent.GetTotalUseTimes(itemConfig.Id) >= itemConfig.SumUseNum)
            {
                FloatTipManager.Instance.ShowFloatTip(ErrorHelp.Instance.ErrorHintList[ErrorCode.ERR_ItemNoUseTime]);
                return;
            }

            // 增幅卷轴
            if (itemConfig.ItemSubType == 17)
            {
                FloatTipManager.Instance.ShowFloatTip("请前往家园装备增幅系统");
                return;
            }

            //材料
            if (itemConfig.ItemType == (int)ItemTypeEnum.Material)
            {
                return;
            }
            //镶嵌宝石
            if (itemConfig.ItemType == (int)ItemTypeEnum.Gemstone)
            {
                UI uiRole = UIHelper.GetUI(self.ZoneScene(), UIType.UIRole);
                if (uiRole == null)
                {
                    return;
                }
                UIRoleComponent uIRoleComponent = uiRole.GetComponent<UIRoleComponent>();
                if (uIRoleComponent.UIPageButton.CurrentIndex != (int)RolePageEnum.RoleGem)
                {
                    uIRoleComponent.UIPageButton.OnSelectIndex((int)RolePageEnum.RoleGem);
                    return;
                }
                UIRoleGemComponent uIRoleGemComponent = uIRoleComponent.UIPageView.UISubViewList[(int)RolePageEnum.RoleGem].GetComponent<UIRoleGemComponent>();
                if (uIRoleGemComponent.XiangQianItem == null)
                {
                    FloatTipManager.Instance.ShowFloatTip("请选择装备！");
                    return;
                }
                string gemHole = uIRoleGemComponent.XiangQianItem.GemHole;
                if (uIRoleGemComponent.XiangQianIndex == -1)
                {
                    FloatTipManager.Instance.ShowFloatTip("请选择孔位！");
                    return;
                }
                string[] gemHolelist = gemHole.Split('_');
                if (gemHolelist.Length <= uIRoleGemComponent.XiangQianIndex)
                {
                    FloatTipManager.Instance.ShowFloatTip("请选择孔位！");
                    return;
                }
                string itemgem = gemHolelist[uIRoleGemComponent.XiangQianIndex];
                if (itemgem != itemConfig.ItemSubType.ToString() && itemConfig.ItemSubType != 110 && itemConfig.ItemSubType != 111)
                {
                    FloatTipManager.Instance.ShowFloatTip("宝石与孔位不符！");
                    return;
                }
                string[] getIdNew = uIRoleGemComponent.XiangQianItem.GemIDNew.Split('_');
                usrPar = $"{uIRoleGemComponent.XiangQianItem.BagInfoID}_{uIRoleGemComponent.XiangQianIndex}";
                if (getIdNew[uIRoleGemComponent.XiangQianIndex] != "0")
                {
                    PopupTipHelp.OpenPopupTip(self.ZoneScene(), "镶嵌宝石", "是否需要覆盖宝石?\n覆盖后原有位置得宝石会自动销毁哦!", () =>
                    {
                        self.RequestXiangQianGem(usrPar).Coroutine();
                    }).Coroutine();
                }
                else
                {
                    self.RequestXiangQianGem(usrPar).Coroutine();
                }
                return;
            }
            if (itemConfig.ItemType == (int)ItemTypeEnum.PetHeXin)
            {
                UI uI = UIHelper.GetUI(self.ZoneScene(), UIType.UIPet);
                errorCode = await uI.GetComponent<UIPetComponent>().RequestPetHeXinSelect();
                //注销Tips
                if (errorCode == ErrorCode.ERR_Success)
                {
                    self.OnCloseTips();
                }
                return;
            }

            if (itemConfig.ItemSubType == 4 || itemConfig.ItemSubType == 14)
            {
                if (self.ZoneScene().GetComponent<MapComponent>().SceneTypeEnum != (int)SceneTypeEnum.LocalDungeon)
                {
                    FloatTipManager.Instance.ShowFloatTip("副本中才能使用!");
                    return;
                }
            }
            if (itemConfig.ItemSubType == 5)
            {
                UI uI = await UIHelper.Create(self.ZoneScene(), UIType.UITuZhiMake);
                uI.GetComponent<UITuZhiMakeComponent>().OnInitUI(self.BagInfo).Coroutine();
                self.OnCloseTips();
                return;
            }
            if (itemConfig.ItemSubType == 15)   //附魔
            {
                self.OnItemFumoUse( itemConfig ).Coroutine();
                return;
            }
            if (itemConfig.ItemSubType == 16)   //锻造精灵
            {
                int makeNew = int.Parse(itemConfig.ItemUsePar);
                EquipMakeConfig equipMakeConfig = EquipMakeConfigCategory.Instance.Get(makeNew);

                int makeType_1 = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeType_1);
                int makeType_2 = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeType_2);
                if (makeType_1 != equipMakeConfig.ProficiencyType && makeType_2 != equipMakeConfig.ProficiencyType)
                {
                    ErrorHelp.Instance.ErrorHint(ErrorCode.ERR_MakeTypeError);
                    return;
                }
                if (self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.MakeList.Contains(makeNew))
                {
                    FloatTipManager.Instance.ShowFloatTip("已经学习过该技能！");
                    return;
                }
            }
            if (itemConfig.ItemSubType == 101)
            {
                unit.GetComponent<SkillManagerComponent>().SendUseSkill(int.Parse(itemConfig.ItemUsePar), itemConfig.Id,
                    Mathf.FloorToInt(unit.Rotation.eulerAngles.y), 0, 0).Coroutine();
                self.OnCloseTips();
                return;
            }
            if (itemConfig.ItemSubType == 102)
            {
                FloatTipManager.Instance.ShowFloatTip("请前往主城的宠物蛋孵化处!");
                return;
            }
            if (itemConfig.ItemSubType == 112)
            {
                AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
                long openserverTime = ServerHelper.GetOpenServerTime(!GlobalHelp.IsOutNetMode, accountInfoComponent.ServerId);
                if (TimeHelper.ServerNow() - openserverTime < TimeHelper.Hour * 4)
                {
                    FloatTipManager.Instance.ShowFloatTip("开区4小时后开启!");
                    return;
                }
                
                UI uI = await UIHelper.Create(self.ZoneScene(), UIType.UIItemExpBox);
                uI.GetComponent<UIItemExpBoxComponent>().OnInitUI(self.BagInfo);
                self.OnCloseTips();
                return;
            }

            // 弹出道具批量使用
            if (self.BagInfo.ItemNum >= 2 && ConfigHelper.BatchUseItemList.Contains(itemConfig.Id))
            {
                UI uI = await UIHelper.Create(self.ZoneScene(), UIType.UIItemBatchUse);
                uI.GetComponent<UIItemBatchUseComponent>().OnInitUI(self.BagInfo);
                self.OnCloseTips();
                return;
            }

            if (itemConfig.ItemSubType == 113 || itemConfig.ItemSubType == 127)
            {
                if (self.BagComponent.GetBagLeftCell() < 1)
                {
                    FloatTipManager.Instance.ShowFloatTip("背包格子不够！");
                    return;
                }

                int curSceneId = 0;
                int needSceneId = int.Parse(self.BagInfo.ItemPar.Split('@')[0]);
                MapComponent mapComponent = self.ZoneScene().GetComponent<MapComponent>();
                if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.LocalDungeon)
                {
                    curSceneId = mapComponent.SceneId;
                }
                if (curSceneId != needSceneId)
                {
                    string fubenName = DungeonConfigCategory.Instance.Get(needSceneId).ChapterName;
                    FloatTipManager.Instance.ShowFloatTip($"请前往{fubenName}");
                    return;
                }
                else
                {
                    // $"{dungeonid}@{"TaskMove_6"}@{dropId}";
                    Scene zoneScene = self.ZoneScene();
                    EventType.DigForTreasure.Instance.BagInfo = self.BagInfo;
                    EventType.DigForTreasure.Instance.ZoneScene = self.ZoneScene();
                    Game.EventSystem.PublishClass(EventType.DigForTreasure.Instance);
                    //UIHelper.Remove(zoneScene, UIType.UIRole);
                    self.OnCloseTips();
                    FloatTipManager.Instance.ShowFloatTip($"消耗道具:{itemConfig.ItemName}");
                    return;
                }
            }
            if (itemConfig.ItemSubType == 108
                || itemConfig.ItemSubType == 109
                || itemConfig.ItemSubType == 117
                || itemConfig.ItemSubType == 118
                || itemConfig.ItemSubType == 119
                || itemConfig.ItemSubType == 122)
            {
                PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
                RolePetInfo petInfo = petComponent.GetFightPet();
                if ((itemConfig.ItemSubType == 108
                || itemConfig.ItemSubType == 109) && petInfo != null)
                {
                    petComponent.RequestXiLian(self.BagInfo.BagInfoID, petInfo.Id).Coroutine();
                    self.OnCloseTips();
                    return;
                }
                FloatTipManager.Instance.ShowFloatTip("请前往宠物重塑界面使用！");
                return;
            }
            if (itemConfig.ItemSubType == 132)
            {
                FloatTipManager.Instance.ShowFloatTip("请前往赛季界面使用");
                return;
            }
            if (itemConfig.ItemSubType == 137)
            {
                UI ui = await UIHelper.Create(self.ZoneScene(), UIType.UIPetEggFuLing);
                ui.GetComponent<UIPetEggFuLingComponent>().UpdateItemList(self.BagInfo).Coroutine();
                UIHelper.PlayUIMusic("10010");
                self.OnCloseTips();
                return;
            }
            if (itemConfig.ItemSubType == 139)
            {
                if (self.ZoneScene().GetComponent<BagComponent>().AdditionalCellNum[0] >= 10)
                {
                    FloatTipManager.Instance.ShowFloatTipDi(GameSettingLanguge.LoadLocalization("已达到最大购买格子数量!"));
                    return;
                }
            }
            if (itemConfig.ItemSubType == 140)
            {
                if (self.ZoneScene().GetComponent<BagComponent>().AdditionalCellNum[5] >= 10)
                {
                    FloatTipManager.Instance.ShowFloatTipDi(GameSettingLanguge.LoadLocalization("已达到最大购买格子数量!"));
                    return;
                }
            }
            if (itemConfig.ItemSubType == 141)
            {
                UI uichangeOcc =  await UIHelper.Create(self.ZoneScene(), UIType.UIItemChangeOcc);
                uichangeOcc.GetComponent<UIItemChangeOccComponent>().BagInfoID = self.BagInfo.BagInfoID;
                //注销Tips
                self.OnCloseTips();
                return;
            }

            long instanceid = self.InstanceId;
            errorCode = await self.ZoneScene().GetComponent<BagComponent>().SendUseItem(self.BagInfo, usrPar);

            if (errorCode == ErrorCode.ERR_Success)
            {
                FloatTipManager.Instance.ShowFloatTipDi(GameSettingLanguge.LoadLocalization("道具使用成功!"));
            }
            if (errorCode == ErrorCode.ERR_ItemOnlyUseOcc)
            {
                OccupationConfig occupationConfig = OccupationConfigCategory.Instance.Get(itemConfig.UseOcc);
                string tip = string.Format(ErrorHelp.Instance.GetHint(ErrorCode.ERR_ItemOnlyUseOcc), occupationConfig.OccupationName);
                FloatTipManager.Instance.ShowFloatTipDi(GameSettingLanguge.LoadLocalization(tip));
            }

            //播放音效
            UIHelper.PlayUIMusic("10010");

            if (instanceid == self.InstanceId)
            {
                if (itemConfig.ItemSubType == 110)
                {
                    UIHelper.Remove(self.ZoneScene(), UIType.UIRole);
                }

                //注销Tips
                self.OnCloseTips();
            }
        }

        public static void InitData(this UIItemTipsComponent self, BagInfo baginfo, ItemOperateEnum equipTipsType, Action handler = null)
        {
            Log.ILog.Debug($"getway:  {baginfo.GetWay}");

            self.BagInfo = baginfo;
            ItemConfig itemconf = ItemConfigCategory.Instance.Get(baginfo.ItemID);

            int itemType = itemconf.ItemType;
            int itemSubType = itemconf.ItemSubType;

            string qualityiconLine = FunctionUI.GetInstance().ItemQualityLine(itemconf.ItemQuality);
            string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, qualityiconLine);
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.ImageQualityLine.GetComponent<Image>().sprite = sp;
            
            string qualityiconBack = FunctionUI.GetInstance().ItemQualityBack(itemconf.ItemQuality); 
            path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, qualityiconBack);
            sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.ImageQualityBg.GetComponent<Image>().sprite = sp;

            //类型描述
            string itemTypename = "消耗品";
            ItemViewHelp.ItemTypeName.TryGetValue(itemType, out itemTypename);

            //烹饪道具显示
            if (itemType == 1 && itemSubType == 131) {
                itemTypename = "家园烹饪";
            }

            self.ItemType.GetComponent<Text>().text = "类型:" + itemTypename;

            string Text_ItemDes = itemconf.ItemDes;
            //获取道具描述的分隔符
            string[] itemDesArray = Text_ItemDes.Split(';');
            string itemMiaoShu = "";
            for (int i = 0; i <= itemDesArray.Length - 1; i++)
            {
                if (itemMiaoShu == "")
                {
                    itemMiaoShu = itemDesArray[i];
                }
                else
                {
                    itemMiaoShu = itemMiaoShu + "\n" + itemDesArray[i];
                }
            }

            //显示是否绑定
            if (baginfo.isBinging)
            {
                self.Obj_Lab_EquipBangDing.GetComponent<Text>().text = GameSettingLanguge.LoadLocalization("已绑定");
                self.Obj_Lab_EquipBangDing.GetComponent<Text>().color = new Color(175f / 255f, 1, 6f / 255f);
                self.Obj_Img_EquipBangDing.SetActive(true);
            }
            else
            {
                self.Obj_Lab_EquipBangDing.GetComponent<Text>().text = GameSettingLanguge.LoadLocalization("未绑定");
                self.Obj_Lab_EquipBangDing.GetComponent<Text>().color = new Color(255f / 255f, 240f / 255f, 200f / 255f);
                self.Obj_Img_EquipBangDing.SetActive(false);
            }

            //数组大于2表示有换行符,否则显示原来的描述
            if (itemDesArray.Length >= 2)
            {
                Text_ItemDes = itemMiaoShu;
            }

            //根据Tips描述长度缩放底的大小
            int i1 = 0;
            Text_ItemDes = ItemViewHelp.GetItemDesc(baginfo, ref i1);
           
            //显示图标
            //显示道具Icon
            string ItemIcon = itemconf.Icon;
            string path1 =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, ItemIcon);
            Sprite sp1 = ResourcesComponent.Instance.LoadAsset<Sprite>(path1);
            if (!self.AssetPath.Contains(path1))
            {
                self.AssetPath.Add(path1);
            }
            self.Obj_ItemIcon.GetComponent<Image>().sprite = sp1;

            string ItemQuality = FunctionUI.GetInstance().ItemQualiytoPath(itemconf.ItemQuality);
            path1 =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, ItemQuality);
            sp1 = ResourcesComponent.Instance.LoadAsset<Sprite>(path1);
            if (!self.AssetPath.Contains(path1))
            {
                self.AssetPath.Add(path1);
            }
            self.Obj_ItemQuality.GetComponent<Image>().sprite = sp1;

            string Text_ItemStory = itemconf.ItemDes;
            //显示道具描述
            int i2 = (int)((Text_ItemStory.Length) / 20) + 1;
            //float ItemBottomTextNum = 30.0f;
            self.TextBtn_Use.GetComponent<Text>().text = itemconf.ItemSubType == 114 ? "镶嵌" : "使用";
            self.Obj_BagOpenSet.SetActive(false);
            self.Obj_Btn_HuiShou.SetActive(false);
            self.Obj_Btn_HuiShouCancle.SetActive(false);
            self.Obj_Btn_XieXiaGemSet.SetActive(false);
            self.Btn_Use.SetActive(false);
            self.Btn_Split.SetActive(false);
            self.Lab_FuLing.SetActive(false);
            self.Lab_FuLingDes.SetActive(false);
            self.Btn_Sell.SetActive(false);
            self.Btn_StoreHouse.SetActive(false);

            //显示按钮
            switch ((ItemOperateEnum)equipTipsType)
            {
                //不显示任何按钮
                case ItemOperateEnum.None:
                case ItemOperateEnum.PaiMaiSell:
                case ItemOperateEnum.PaiMaiBuy:
                    //ItemBottomTextNum = 0;
                    break;
                case ItemOperateEnum.JianYuanBag:
                    self.Obj_BagOpenSet.SetActive(false);
                    self.Btn_Split.SetActive(false);
                    break;
                //背包打开显示对应功能按钮
                case ItemOperateEnum.Bag:
                    self.Obj_BagOpenSet.SetActive(true);
                    //判定当前是否打开仓库
                    self.Obj_Diu.SetActive(true);
                    self.Btn_Use.SetActive(itemType != (int)ItemTypeEnum.Material);
                    self.Btn_Split.SetActive(itemType == (int)ItemTypeEnum.Material);
                    break;
                //角色栏打开显示对应功能按钮
                case ItemOperateEnum.Juese:
                    self.Obj_BagOpenSet.SetActive(true);
                    self.Btn_Use.SetActive(true);
                    break;
                //商店查看属性
                case ItemOperateEnum.Shop:
                    self.Obj_BagOpenSet.SetActive(false);
                    //ItemBottomTextNum = 0;
                    break;
                //仓库查看属性
                case ItemOperateEnum.Cangku:
                    self.Obj_BagOpenSet.SetActive(false);
                    self.Btn_PutBag.SetActive(true);
                    //ItemBottomTextNum = 0;
                    break;
                case ItemOperateEnum.GemCangku:
                    self.Obj_BagOpenSet.SetActive(false);
                    self.Btn_PutBag.SetActive(true);
                    break;
                case ItemOperateEnum.GemBag:
                    self.Obj_BagOpenSet.SetActive(true);
                    self.Btn_StoreHouse.SetActive(true);
                    self.Btn_Use.SetActive(false);
                    break;
                case ItemOperateEnum.CangkuBag:
                    self.Obj_BagOpenSet.SetActive(true);
                    self.Btn_StoreHouse.SetActive(true);
                    self.Btn_Use.SetActive( false );
                    break;
                //回收背包打开
                case ItemOperateEnum.HuishouBag:
                    self.Obj_BagOpenSet.SetActive(true);
                    self.Obj_Btn_HuiShou.SetActive(true);
                    break;
                //回收功能显示
                case ItemOperateEnum.HuishouShow:
                    self.Obj_BagOpenSet.SetActive(false);
                    self.Obj_Btn_HuiShouCancle.SetActive(true);
                    break;
                //牧场  不显示任何按钮
                case ItemOperateEnum.Muchang:
                    //ItemBottomTextNum = 0;
                    break;
                //牧场仓库  显示出售按钮
                case ItemOperateEnum.MuchangCangku:
                    self.Obj_BagOpenSet.SetActive(true);
                    self.Obj_Diu.SetActive(true);
                    break;
                //镶嵌背包切页
                case ItemOperateEnum.XiangQianBag:
                    self.Obj_BagOpenSet.SetActive(true);
                    self.Btn_Use.SetActive(true);
                    break;
                //镶嵌在装备上的宝石
                case ItemOperateEnum.XiangQianGem:
                    self.Obj_BagOpenSet.SetActive(false);
                    self.Obj_Btn_XieXiaGemSet.SetActive(true);
                    break;
                case ItemOperateEnum.PetHeXinBag:
                    self.Obj_BagOpenSet.SetActive(true);
                    self.Btn_Use.SetActive(true);
                    self.Btn_Use.transform.Find("TextBtn_Use").GetComponent<Text>().text = "装备";
                    break;
                default:
                    //ItemBottomTextNum = 0;
                    break;
            }

            // 图纸类型需要的按钮
            if (itemType == 1 && itemSubType == 5)
            {
                self.Btn_Sell.SetActive(false);
                self.Btn_Use.SetActive(true);
                self.Btn_Split.SetActive(true);
            }

            // 宠物蛋附灵
            if (itemType == 1 && itemSubType == 102 && self.BagInfo.FuLing == 1)
            {
                self.Lab_FuLing.SetActive(true);
                // self.Lab_FuLing.GetComponent<Text>().color = new Color(175f / 255f, 1, 6f / 255f);
                self.Lab_FuLingDes.SetActive(true);
                // self.Lab_FuLingDes.GetComponent<Text>().color = new Color(175f / 255f, 1, 6f / 255f);
            }

            //判定道具为宝石时显示使用变为镶嵌字样
            if (itemType == 4)
            {
                string langStr_A = GameSettingLanguge.LoadLocalization("镶 嵌");
                //self.Obj_Btn_GemHoleText.GetComponent<Text>().text = langStr_A;
            }

            //设置底的长度
            //self.ItemDi.GetComponent<RectTransform>().sizeDelta = new Vector2(301.0f, 180.0f + i1 * 20.0f + i2 * 16.0f + ItemBottomTextNum);
            //显示道具信息
            self.Lab_ItemName.GetComponent<Text>().text = itemconf.ItemName;
            self.Lab_ItemName.GetComponent<Text>().color = FunctionUI.GetInstance().QualityReturnColor(itemconf.ItemQuality);
            self.ItemDes.GetComponent<Text>().text = Text_ItemDes;
            //赞助宝箱设置描述为绿色
            //if (itemSubType == 9)
            //{
            //    self.ItemDes.GetComponent<Text>().color = Color.green;
            //}
            float exceedWidth = self.Lab_ItemName.GetComponent<Text>().preferredWidth - self.Lab_ItemNameWidth;
            if (exceedWidth > -20)
            {
                self.Img_back.GetComponent<RectTransform>().sizeDelta = new Vector2(self.Img_backVector2.x + exceedWidth + 30, self.Img_backVector2.y);
            }

            //鉴定品质符
            if (itemconf.ItemSubType == 121)
            {
                self.ItemDes.GetComponent<Text>().text = Text_ItemDes + "\n" + "\n" + $"鉴定符品质:{baginfo.ItemPar}" + "\n" + "品质越高,鉴定出极品的概率越高。" + "\n" + "鉴定符品质与制造者熟练度相关。";
            }

            //鉴定品质符
            if (itemconf.ItemType == 1 && itemconf.ItemSubType == 131)
            {
                string[] addList = itemconf.ItemUsePar.Split(';')[0].Split(',');
                self.ItemDes.GetComponent<Text>().text = Text_ItemDes + "\n" + "\n" + "烹饪品质:" + baginfo.ItemPar;
            }

            //宠物技能
            if (itemconf.ItemType == 2 && itemconf.ItemSubType == 122)
            {
                SkillConfig skillCof = SkillConfigCategory.Instance.Get(int.Parse(itemconf.ItemUsePar));
                self.ItemDes.GetComponent<Text>().text = Text_ItemDes + "\n" + "\n" + $"技能描述:{skillCof.SkillDescribe}";
            }

            //藏宝图
            if (itemconf.ItemSubType == 127 && !string.IsNullOrEmpty(self.BagInfo.ItemPar)) 
            {
                int sceneID = int.Parse(self.BagInfo.ItemPar.Split('@')[0]);
                self.ItemDes.GetComponent<Text>().text =  $"{itemconf.ItemDes}\n前往地图:{DungeonConfigCategory.Instance.Get(sceneID).ChapterName}开启藏宝图!";
            }

            string langStr = GameSettingLanguge.LoadLocalization("使用等级");
            if (itemconf.UseLv > 0)
            {
                self.ItemItemLv.GetComponent<Text>().text = langStr + ":" + itemconf.UseLv;
            }
            else
            {
                self.ItemItemLv.GetComponent<Text>().text = langStr + ":1";
            }

            //监测UI是否超过底部显示
            /*
            float DiHight = self.ItemDi.GetComponent<RectTransform>().sizeDelta.y;
            Debug.Log("DiHight = " + DiHight);
            float screen_higeValue = 768 * FunctionUI.GetInstance().ReturnScreenScalePro();
            float UIHeadValue = screen_higeValue - self.GetParent<UI>().GameObject.transform.localPosition.y - DiHight / 2;            //UI�Ͷ����ľ���
            float UIHightValue = UIHeadValue + DiHight + 50.0f;

            if (UIHightValue >= screen_higeValue)
            {
                ////////////this.transform.localPosition = new Vector3(this.transform.localPosition.x, 50 + DiHight / 2, 0);
            }
            //���UI�Ƿ񳬹��˶�����ʾ
            if (UIHeadValue <= 30)
            {
                ////////////this.transform.localPosition = new Vector3(this.transform.localPosition.x, screen_higeValue - DiHight / 2 - 50, 0);
            }
            */
        }
    }
}