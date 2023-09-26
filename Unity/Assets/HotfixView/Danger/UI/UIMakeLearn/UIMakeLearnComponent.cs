﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIMakeLearnComponent : Entity, IAwake
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
        public GameObject Image_ItemIcon;
        public GameObject Image_ItemQuality;
        public GameObject CostListNode;
        public GameObject LearnListNode;
        public GameObject Obj_Lab_LearnItemName;
        public GameObject Obj_Lab_LearnItemCost;
        public GameObject LabNeedShuLian;

        public int MakeId;
        public List<UIMakeLearnItemComponent> LearnUIList = new List<UIMakeLearnItemComponent>();
        public List<UIItemComponent> CostUIList = new List<UIItemComponent>();
        public UserInfoComponent userInfoComponent;
        public int MakeType;
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
         
            self.Image_ItemIcon = rc.Get<GameObject>("Image_ItemIcon");
            self.Image_ItemQuality = rc.Get<GameObject>("Image_ItemQuality");
            self.CostListNode = rc.Get<GameObject>("CostListNode");
            self.LearnListNode = rc.Get<GameObject>("LearnListNode");
            self.Obj_Lab_LearnItemName = rc.Get<GameObject>("Lab_LearnItemName");
            self.Obj_Lab_LearnItemCost = rc.Get<GameObject>("Lab_LearnItemCost");
            self.LabNeedShuLian = rc.Get<GameObject>("LabNeedShuLian");

            self.userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();

            self.ButtonLearn.GetComponent<Button>().onClick.AddListener(() =>
            {
                self.OnButtonLearn().Coroutine();
            });

            self.ImageButton = rc.Get<GameObject>("ImageButton");
            self.ImageButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                self.OnImageButton();
            });

            self.CheckMakeType();
            self.UpdateShuLianDu();
        }
    }

    public static class UIMakeLearnComponentSystem
    {

        public static void UpdateShuLianDu(this UIMakeLearnComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int maxValue = ComHelp.MaxShuLianDu();
            int curValue = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeShuLianDu);

            self.Lab_ShuLianDu.GetComponent<Text>().text = $"{curValue}/{maxValue}";
            self.Img_ShuLianPro.GetComponent<Image>().fillAmount = curValue * 1f / maxValue;
        }

        public static void On_Button_Select(this UIMakeLearnComponent self, int makeId)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int makeType = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeType);
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
            C2M_MakeSelectRequest request = new C2M_MakeSelectRequest() { MakeType = makeId };
            M2C_MakeSelectResponse response = (M2C_MakeSelectResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.MakeList.Clear();
            self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.MakeList = response.MakeList;
            self.CheckMakeType();
        }

        public static void CheckMakeType(this UIMakeLearnComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int makeType = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeType);
            int showValue = NpcConfigCategory.Instance.Get(UIHelper.CurrentNpcId).ShopValue;
            self.MakeType = makeType;

            self.Right.SetActive(makeType == showValue);
            self.Left.SetActive(makeType == showValue);
            self.Select.SetActive(makeType != showValue);
            self.Select_1.SetActive(showValue == 1);
            self.Select_2.SetActive(showValue == 2);
            self.Select_3.SetActive(showValue == 3);
            self.Select_3.SetActive(showValue == 6);

            self.InitData(makeType).Coroutine();
        }

        public static void OnImageButton(this UIMakeLearnComponent self)
        {
            UIHelper.Remove(self.DomainScene(), UIType.UIMakeLearn);
        }

        public static async ETTask InitData(this UIMakeLearnComponent self, int makeType)
        {
            if (self.MakeType != makeType)
            {
                return;
            }
            var path = ABPathHelper.GetUGUIPath("Main/MakeLearn/UIMakeLearnItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);

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
                    GameObject itemSpace = GameObject.Instantiate(bundleGameObject);
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

            self.Obj_Lab_LearnItemName.GetComponent<Text>().text = itemConfig.ItemName;
            self.Obj_Lab_LearnItemName.GetComponent<Text>().color = FunctionUI.GetInstance().QualityReturnColor(itemConfig.ItemQuality);

            Sprite sp = ABAtlasHelp.GetIconSprite(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            self.Image_ItemIcon.GetComponent<Image>().sprite = sp;

            string ItemQuality = FunctionUI.GetInstance().ItemQualiytoPath(itemConfig.ItemQuality);
            self.Image_ItemQuality.GetComponent<Image>().sprite = ABAtlasHelp.GetIconSprite(ABAtlasTypes.ItemQualityIcon, ItemQuality);

            //显示需要熟练度
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int nowShuLianDu = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeShuLianDu);
            self.LabNeedShuLian.GetComponent<Text>().text = $"{nowShuLianDu}/{equipMakeConfig.NeedProficiencyValue}";
            if (unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeShuLianDu) < equipMakeConfig.NeedProficiencyValue)
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

            C2M_MakeLearnRequest m_Learn = new C2M_MakeLearnRequest() { MakeId = self.MakeId };
            M2C_MakeLearnResponse r2c_roleEquip = (M2C_MakeLearnResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(m_Learn);
            if (r2c_roleEquip.Error == 0)
            {
                self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.MakeList.Add(self.MakeId);
                self.InitData(self.MakeType).Coroutine();
                FloatTipManager.Instance.ShowFloatTip(GameSettingLanguge.LoadLocalization("学习配方成功!"));
            }
        }
    }

}


