﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIMakeLearnComponent : Entity, IAwake,IDestroy
    {
        public GameObject Img_ShuLianPro;
        public GameObject Lab_ShuLianDu;
        public GameObject Button_Select_6;
        public GameObject Button_Select_3;
        public GameObject Button_Select_2;
        public GameObject Button_Select_1;
        public GameObject Right;
        public GameObject Left;
        public GameObject Select;
        public GameObject Select_6;
        public GameObject Select_3;
        public GameObject Select_2;
        public GameObject Select_1;

        public GameObject ImageButton;
        public GameObject TextTMP_Name;
        public GameObject TextTMP_CostCoin;
        public GameObject ButtonLearn;
        public GameObject CostListNode;
        public GameObject LearnListNode;
        public GameObject UIMakeLearnItem;
        public GameObject Obj_Lab_LearnItemCost;
        public GameObject LabNeedShuLian;

        public UIItemComponent UILearn = null;

        public List<UIMakeLearnItemComponent> LearnUIList = new List<UIMakeLearnItemComponent>();
        public List<UIItemComponent> CostUIList = new List<UIItemComponent>();
        public UserInfoComponent userInfoComponent;
        public int MakeType;
        public int MakeId;
        public int Plan = 1;
        
        public List<string> AssetPath = new List<string>();
    }


    public class UIMakeLearnComponentAwakeSystem : AwakeSystem<UIMakeLearnComponent>
    {
        public override void Awake(UIMakeLearnComponent self)
        {
            self.MakeId = 0;
            self.MakeType = 1;
            self.LearnUIList.Clear();
            self.CostUIList.Clear();
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Button_Select_6 = rc.Get<GameObject>("Button_Select_6");
            self.Button_Select_3 = rc.Get<GameObject>("Button_Select_3");
            self.Button_Select_2 = rc.Get<GameObject>("Button_Select_2");
            self.Button_Select_1 = rc.Get<GameObject>("Button_Select_1");
            ButtonHelp.AddListenerEx(self.Button_Select_1, () => { self.On_Button_Select(1); });
            ButtonHelp.AddListenerEx(self.Button_Select_2, () => { self.On_Button_Select(2); });
            ButtonHelp.AddListenerEx(self.Button_Select_3, () => { self.On_Button_Select(3); });
            ButtonHelp.AddListenerEx(self.Button_Select_6, () => { self.On_Button_Select(6); });

            self.Right = rc.Get<GameObject>("Right");
            self.Left = rc.Get<GameObject>("Left");
            self.Select_6 = rc.Get<GameObject>("Select_6");
            self.Select_3 = rc.Get<GameObject>("Select_3");
            self.Select_2 = rc.Get<GameObject>("Select_2");
            self.Select_1 = rc.Get<GameObject>("Select_1");
            self.Select = rc.Get<GameObject>("Select");
            self.Lab_ShuLianDu = rc.Get<GameObject>("Lab_ShuLianDu");
            self.Img_ShuLianPro = rc.Get<GameObject>("Img_ShuLianPro");

            self.TextTMP_Name = rc.Get<GameObject>("TextTMP_Name");
            self.TextTMP_CostCoin = rc.Get<GameObject>("TextTMP_CostCoin");

            self.ButtonLearn = rc.Get<GameObject>("ButtonLearn");
         
            GameObject UILearn = rc.Get<GameObject>("UILearn");
            self.UILearn = self.AddComponent<UIItemComponent, GameObject>(UILearn);
           

            self.CostListNode = rc.Get<GameObject>("CostListNode");
            self.LearnListNode = rc.Get<GameObject>("LearnListNode");
            self.UIMakeLearnItem = rc.Get<GameObject>("UIMakeLearnItem");
            self.UIMakeLearnItem.SetActive(false);
            self.Obj_Lab_LearnItemCost = rc.Get<GameObject>("Lab_LearnItemCost");
            self.LabNeedShuLian = rc.Get<GameObject>("LabNeedShuLian");

            self.userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();

            self.ButtonLearn.GetComponent<Button>().onClick.AddListener(() =>
            {
                self.OnButtonLearn().Coroutine();
            });

            self.ImageButton = rc.Get<GameObject>("ImageButton");
            self.ImageButton.GetComponent<Button>().onClick.AddListener(self.OnImageButton);
            self.OnBtn_Plan(1);
        }
    }
    public class UIMakeLearnComponentDestroy: DestroySystem<UIMakeLearnComponent>
    {
        public override void Destroy(UIMakeLearnComponent self)
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
    public static class UIMakeLearnComponentSystem
    {

        public static void OnBtn_Plan(this UIMakeLearnComponent self, int plan)
        {
            self.Plan = plan;

            self.CheckMakeType();
            self.UpdateShuLianDu();
        }

        public static void UpdateShuLianDu(this UIMakeLearnComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int maxValue = ComHelp.MaxShuLianDu();

            int shulianduNumeric = self.Plan == 1 ? NumericType.MakeShuLianDu_1 : NumericType.MakeShuLianDu_2;
            int curValue = unit.GetComponent<NumericComponent>().GetAsInt(shulianduNumeric);

            self.Lab_ShuLianDu.GetComponent<Text>().text = $"{curValue}/{maxValue}";
            self.Img_ShuLianPro.GetComponent<Image>().fillAmount = curValue * 1f / maxValue;
        }

        public static void On_Button_Select(this UIMakeLearnComponent self, int makeId)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int makeType = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeType_1);
            if (makeType != 0)
            {
                int cost = GlobalValueConfigCategory.Instance.Get(46).Value2;
                PopupTipHelp.OpenPopupTip(self.ZoneScene(), "技能重置",
                    $"重置后自身学习的生活技能将全部遗忘,请谨慎选择!", ()=>
                    {
                        self.RequestMakeSelect(makeId).Coroutine();
                    }, null).Coroutine();
                return;
            }
            self.RequestMakeSelect(makeId).Coroutine();
        }

        public static async ETTask RequestMakeSelect(this UIMakeLearnComponent self, int makeId)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int makeType_1 = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeType_1);
            int makeType_2 = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeType_2);
            if (makeType_1 == makeId || makeType_2 == makeId)
            {
                FloatTipManager.Instance.ShowFloatTip("该生活技能已学习！");
                return;
            }
            C2M_MakeSelectRequest request = new C2M_MakeSelectRequest() { MakeType = makeId , Plan = self.Plan == -1 ? 1 : self.Plan };
            M2C_MakeSelectResponse response = (M2C_MakeSelectResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.MakeList.Clear();
            self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.MakeList = response.MakeList;
            self.CheckMakeType();
        }

        //0:图纸制造（需要消耗图纸）
        //1.锻造类型
        //2.裁缝类型
        //3.炼金类型
        //4.宝石类型
        //5.神器类型
        //6.附魔类型
        //8.家园类型
        public static void CheckMakeType(this UIMakeLearnComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int makeType_1 = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeType_1);
            int makeType_2 = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeType_2);
           
            int showValue = NpcConfigCategory.Instance.Get(UIHelper.CurrentNpcId).ShopValue;
            self.MakeType = showValue;
            self.Plan = -1;
            if (makeType_1 == showValue)
            {
                self.Plan = 1;
            }
            if (makeType_2 == showValue)
            {
                self.Plan = 2;
            }

            self.Right.SetActive(self.Plan != -1);
            self.Left.SetActive(self.Plan != -1);
            self.Select.SetActive(self.Plan == -1);
            self.Select_1.SetActive(showValue == 1);
            self.Select_2.SetActive(showValue == 2);
            self.Select_3.SetActive(showValue == 3);
            self.Select_6.SetActive(showValue == 6);

            self.InitData(self.MakeType);
        }

        public static void OnImageButton(this UIMakeLearnComponent self)
        {
            UIHelper.Remove(self.DomainScene(), UIType.UIMakeLearn);
        }

        public static  void InitData(this UIMakeLearnComponent self, int makeType)
        {
            for (int i = 0; i < self.LearnUIList.Count; i++)
            {
                self.LearnUIList[i].GameObject.SetActive(false);
            }
            if (self.MakeType != makeType)
            {
                return;
            }

            int number = 0;
            int playeLv = self.userInfoComponent.UserInfo.Lv;
            Dictionary<int, EquipMakeConfig> keyValuePairs = EquipMakeConfigCategory.Instance.GetAll();
            string initMakeList = GlobalValueConfigCategory.Instance.Get(43).Value;
            foreach(var item in keyValuePairs)
            {
                if (self.userInfoComponent.UserInfo.MakeList.Contains(item.Key))
                {
                    continue;
                }
                if (initMakeList.Contains(item.Key.ToString()))
                {
                    continue;
                }

                if (playeLv < item.Value.LearnLv || item.Value.LearnType != 0)
                {
                    continue;
                }
                if (item.Value.ProficiencyType != self.MakeType)
                {
                    continue;
                }
                UIMakeLearnItemComponent uI_1 = null;
                if (number < self.LearnUIList.Count)
                {
                    uI_1 = self.LearnUIList[number];
                    uI_1.GameObject.SetActive(true);
                }
                else
                {
                    GameObject itemSpace = GameObject.Instantiate(self.UIMakeLearnItem);
                    itemSpace.SetActive(true);
                    UICommonHelper.SetParent(itemSpace, self.LearnListNode);
                    uI_1 = self.AddChild<UIMakeLearnItemComponent, GameObject>(itemSpace);
                    uI_1.SetClickHandler((int itemid) => { self.OnSelectLearnItem(itemid); });
                    self.LearnUIList.Add(uI_1);
                }

                uI_1.OnUpdateUI(item.Key);
                number++;
            }
            for (int i = number; i < self.LearnUIList.Count; i++)
            {
                self.LearnUIList[i].GameObject.SetActive(false);
            }
            if (self.LearnUIList.Count > 0)
            {
                self.LearnUIList[0].OnImageButton();
            }
            if (self.MakeId != 0)
            {
                self.OnSelectLearnItem(self.MakeId);
            }
        }

        public static void OnSelectLearnItem(this UIMakeLearnComponent self, int makeid)
        {
            self.MakeId = makeid;
            for (int i = 0; i < self.LearnUIList.Count; i++)
            {
                self.LearnUIList[i].SetSelected(makeid);
            }
            self.OnUpdateLearn(makeid);
        }

        public static  void OnUpdateLearn(this UIMakeLearnComponent self, int makeid)
        {
            EquipMakeConfig equipMakeConfig = EquipMakeConfigCategory.Instance.Get(makeid);
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(equipMakeConfig.MakeItemID);

            //self.TextTMP_CostCoin.GetComponent<TextMeshProUGUI>().text = equipMakeConfig.LearnGoldValue.ToString();
            self.Obj_Lab_LearnItemCost.GetComponent<Text>().text = equipMakeConfig.LearnGoldValue.ToString();
            //self.TextTMP_Name.GetComponent<TextMeshProUGUI>().text = itemConfig.ItemName;
            //self.TextTMP_Name.GetComponent<TextMeshProUGUI>().color = FunctionUI.GetInstance().QualityReturnColor(itemConfig.ItemQuality);

            self.UILearn.UpdateItem( new BagInfo() { ItemID = itemConfig.Id, ItemNum = 1 } , ItemOperateEnum.None);
            self.UILearn.Label_ItemNum.SetActive(false);

            //显示需要熟练度
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int shulianduNumeric = self.Plan == 1 ? NumericType.MakeShuLianDu_1 : NumericType.MakeShuLianDu_2;
            int nowShuLianDu = unit.GetComponent<NumericComponent>().GetAsInt(shulianduNumeric);
            self.LabNeedShuLian.GetComponent<Text>().text = $"{nowShuLianDu}/{equipMakeConfig.NeedProficiencyValue}";
            if (unit.GetComponent<NumericComponent>().GetAsInt(shulianduNumeric) < equipMakeConfig.NeedProficiencyValue)
            {
                //不满足显示红色,满足显示绿色
                self.LabNeedShuLian.GetComponent<Text>().text += "(熟练度不足)";
                self.LabNeedShuLian.GetComponent<Text>().color = new Color(207f / 255f, 12f / 255f, 0);
            }
            else {
                //满足显示绿色,满足显示绿色
                self.LabNeedShuLian.GetComponent<Text>().text += "(可学习)";
                self.LabNeedShuLian.GetComponent<Text>().color = new Color(86f / 255f, 147f / 255f, 0);
            }

            string[] costItems = equipMakeConfig.NeedItems.Split('@');
            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
            var bundleGameObject =ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            for (int i = 0; i < costItems.Length; i++)
            {
                string[] itemInfo = costItems[i].Split(';');
                if (itemInfo.Length < 2)
                {
                    continue;
                }

                UIItemComponent ui_2 = null;
                if (i < self.CostUIList.Count)
                {
                    ui_2 = self.CostUIList[i];
                    ui_2.GameObject.SetActive(true);
                }
                else
                {
                    GameObject itemSpace = GameObject.Instantiate(bundleGameObject);
                    itemSpace.SetActive(true);
                    UICommonHelper.SetParent(itemSpace, self.CostListNode);
                    ui_2 = self.AddChild<UIItemComponent, GameObject>( itemSpace);
                    self.CostUIList.Add(ui_2);
                }
                ui_2.UpdateItem( new BagInfo() {  ItemID = int.Parse(itemInfo[0]), ItemNum = int.Parse(itemInfo[1]) }, ItemOperateEnum.None );
                ui_2.Label_ItemName.SetActive(true);
            }
            for (int i = costItems.Length; i < self.CostUIList.Count; i++)
            {
                self.CostUIList[i].GameObject.SetActive(false);
            }
        }

        public static async ETTask OnButtonLearn(this UIMakeLearnComponent self)
        {
            if (self.MakeId == 0)
            {
                return;
            }
            if (self.userInfoComponent.UserInfo.MakeList.Contains(self.MakeId))
            {
                FloatTipManager.Instance.ShowFloatTip(GameSettingLanguge.LoadLocalization("已经学习过该道具!"));
                return;
            }

            C2M_MakeLearnRequest m_Learn = new C2M_MakeLearnRequest() { MakeId = self.MakeId, Plan = self.Plan };
            M2C_MakeLearnResponse r2c_roleEquip = (M2C_MakeLearnResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(m_Learn);
            if (r2c_roleEquip.Error == 0)
            {
                self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.MakeList.Add(self.MakeId);
                self.InitData(self.MakeType);
                FloatTipManager.Instance.ShowFloatTip(GameSettingLanguge.LoadLocalization("学习配方成功!"));
            }
        }
    }

}


