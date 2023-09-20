﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIOccTwoComponent : Entity, IAwake
    {
        public GameObject Image_ZhiYe;
        public GameObject Image_ZhiYe_4;

        public GameObject Image_WuQi_Zhuan;
        public GameObject Image_WuQi_Type;

        public GameObject ButtonOccTwo;
        public GameObject Button_ZhiYe_Select;
        public GameObject Button_ZhiYe_3;
        public GameObject Button_ZhiYe_2;
        public GameObject Button_ZhiYe_1;
        public List<GameObject> Button_ZhiYe_List = new List<GameObject>();

        public GameObject OccNengLi_1;
        public GameObject OccNengLi_2;
        public GameObject OccNengLi_3;

        public GameObject OccLine_1;
        public GameObject OccLine_2;
        public GameObject OccLine_3;

        public GameObject Button_ZhiYeSelect_1;
        public GameObject Button_ZhiYeSelect_2;
        public GameObject Button_ZhiYeSelect_3;

        public GameObject SkillContainer;
        public GameObject Text_ZhiYe_4;
        public GameObject Text_ZhiYe_3;
        public GameObject Text_ZhiYe_2;
        public GameObject Text_ZhiYe_1;
        public GameObject Text_ZhiYe;

        public GameObject closeButton;
        public GameObject ButtonOccReset;

        public GameObject Lab_HuJia;
        public GameObject Lab_WuQi;

        public int OccTwoId;

        public Dictionary<int, string> showType = new Dictionary<int, string>()
        {
            {  1,  GameSettingLanguge.LoadLocalization("剑类") },
            {  2,  GameSettingLanguge.LoadLocalization("刀类") },
            {  3,  GameSettingLanguge.LoadLocalization("法杖") },
            {  4,  GameSettingLanguge.LoadLocalization("魔法书") },
             {  5,  GameSettingLanguge.LoadLocalization("弓箭") },
            {  11,  GameSettingLanguge.LoadLocalization("布甲") },
            {  12,  GameSettingLanguge.LoadLocalization("轻甲") },
            {  13,  GameSettingLanguge.LoadLocalization("重甲") },
        };
    }


    public class UIOccTwoComponentAwakeSystem : AwakeSystem<UIOccTwoComponent>
    {
        public override void Awake(UIOccTwoComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.Button_ZhiYe_List.Clear();

            self.Image_ZhiYe_4 = rc.Get<GameObject>("Image_ZhiYe_4");
            self.Image_ZhiYe = rc.Get<GameObject>("Image_ZhiYe");

            self.Image_WuQi_Zhuan = rc.Get<GameObject>("Image_WuQi_Zhuan");
            self.Image_WuQi_Type = rc.Get<GameObject>("Image_WuQi_Type");

            self.OccNengLi_1 = rc.Get<GameObject>("OccNengLi_1");
            self.OccNengLi_2 = rc.Get<GameObject>("OccNengLi_2");
            self.OccNengLi_3 = rc.Get<GameObject>("OccNengLi_3");

            self.OccLine_1 = rc.Get<GameObject>("OccLine_1");
            self.OccLine_2 = rc.Get<GameObject>("OccLine_2");
            self.OccLine_3 = rc.Get<GameObject>("OccLine_3");

            self.Button_ZhiYeSelect_1 = rc.Get<GameObject>("Button_ZhiYeSelect_1");
            self.Button_ZhiYeSelect_2 = rc.Get<GameObject>("Button_ZhiYeSelect_2");
            self.Button_ZhiYeSelect_3 = rc.Get<GameObject>("Button_ZhiYeSelect_3");

            self.Button_ZhiYe_Select = rc.Get<GameObject>("Button_ZhiYe_Select");
            self.Button_ZhiYe_3 = rc.Get<GameObject>("Button_ZhiYe_3");
            self.Button_ZhiYe_3.GetComponent<Button>().onClick.AddListener(() => { self.OnButton_ZhiYe(2); });
            self.Button_ZhiYe_2 = rc.Get<GameObject>("Button_ZhiYe_2");
            self.Button_ZhiYe_2.GetComponent<Button>().onClick.AddListener(() => { self.OnButton_ZhiYe(1); });
            self.Button_ZhiYe_1 = rc.Get<GameObject>("Button_ZhiYe_1");
            self.Button_ZhiYe_1.GetComponent<Button>().onClick.AddListener(() => { self.OnButton_ZhiYe(0); });
            self.SkillContainer = rc.Get<GameObject>("SkillContainer");
            self.Button_ZhiYe_List.Add( self.Button_ZhiYe_1);
            self.Button_ZhiYe_List.Add( self.Button_ZhiYe_2);
            self.Button_ZhiYe_List.Add( self.Button_ZhiYe_3);

            self.ButtonOccTwo = rc.Get<GameObject>("ButtonOccTwo");
            ButtonHelp.AddListenerEx(self.ButtonOccTwo, () => { self.OnClickOccTwo();   });

            self.Text_ZhiYe_4 = rc.Get<GameObject>("Text_ZhiYe_4");
            self.Text_ZhiYe_3 = rc.Get<GameObject>("Text_ZhiYe_3");
            self.Text_ZhiYe_2 = rc.Get<GameObject>("Text_ZhiYe_2");
            self.Text_ZhiYe_1 = rc.Get<GameObject>("Text_ZhiYe_1");
            self.Text_ZhiYe = rc.Get<GameObject>("Text_ZhiYe");

            self.closeButton = rc.Get<GameObject>("closeButton");
            self.closeButton.GetComponent<Button>().onClick.AddListener(() => { self.OnClickOccTwoui(); });

            self.ButtonOccReset = rc.Get<GameObject>("ButtonOccReset");
            self.ButtonOccReset.GetComponent<Button>().onClick.AddListener(() => { self.OnButtonOccReset().Coroutine(); });

            self.Lab_HuJia = rc.Get<GameObject>("Lab_HuJia");
            self.Lab_WuQi = rc.Get<GameObject>("Lab_WuQi");

            self.OnInitUI();
        }
    }

    public static class UIOccTwoComponentSystem
    {

        public static void OnClickOccTwo(this UIOccTwoComponent self)
        {
            AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
            //if (!GMHelp.GmAccount.Contains(accountInfoComponent.Account))
            //{
            //    return;
            //}

            OccupationTwoConfig occupationTwoConfig = OccupationTwoConfigCategory.Instance.Get(self.OccTwoId);
            PopupTipHelp.OpenPopupTip(self.ZoneScene(), "转职", $"是否转职为：{occupationTwoConfig.OccupationName}", () =>
            {
                self.RequestChangeOcc().Coroutine();
            }).Coroutine();
        }

        public static async ETTask RequestChangeOcc(this UIOccTwoComponent self)
        {
            bool ifChange = await self.ZoneScene().GetComponent<SkillSetComponent>().ChangeOccTwoRequest(self.OccTwoId);
            if (ifChange)
            {
                UIHelper.Create(self.DomainScene(), UIType.UIOccTwoShow).Coroutine();
                UIHelper.Remove(self.DomainScene(), UIType.UIOccTwo);
            }
        }

        public static void OnInitUI(this UIOccTwoComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            int occ = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Occ;
            OccupationConfig occupationConfig = OccupationConfigCategory.Instance.Get(occ);
            self.Text_ZhiYe.GetComponent<Text>().text = occupationConfig.OccupationName;

            int[] OccTwoID = occupationConfig.OccTwoID;
            self.Text_ZhiYe_1.GetComponent<Text>().text = OccupationTwoConfigCategory.Instance.Get(OccTwoID[0]).OccupationName;
            self.Text_ZhiYe_2.GetComponent<Text>().text = OccupationTwoConfigCategory.Instance.Get(OccTwoID[1]).OccupationName;
            self.Text_ZhiYe_3.GetComponent<Text>().text = OccupationTwoConfigCategory.Instance.Get(OccTwoID[2]).OccupationName;

            int occTwo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.OccTwo;
            List<int> occTwoList = new List<int>(OccTwoID);
            int index = occTwoList.IndexOf(occTwo);
            index = index < 0 ? 0 : index;
            self.OnButton_ZhiYe(index);

            self.Button_ZhiYe_1.GetComponent<Image>().sprite = ABAtlasHelp.GetIconSprite(ABAtlasTypes.OtherIcon, $"OccTwo_{OccTwoID[0]}");
            self.Button_ZhiYe_2.GetComponent<Image>().sprite = ABAtlasHelp.GetIconSprite(ABAtlasTypes.OtherIcon, $"OccTwo_{OccTwoID[1]}");
            self.Button_ZhiYe_3.GetComponent<Image>().sprite = ABAtlasHelp.GetIconSprite(ABAtlasTypes.OtherIcon, $"OccTwo_{OccTwoID[2]}");

            self.Image_ZhiYe.GetComponent<Image>().sprite = ABAtlasHelp.GetIconSprite(ABAtlasTypes.OtherIcon, $"Occ_{occ}");
        }

        public static void OnButton_ZhiYe(this UIOccTwoComponent self, int index)
        {
            //UICommonHelper.SetParent(self.Button_ZhiYe_Select, self.Button_ZhiYe_List[index]);

            UICommonHelper.SetImageGray(self.Button_ZhiYe_List[0], true);
            UICommonHelper.SetImageGray(self.Button_ZhiYe_List[1], true);
            UICommonHelper.SetImageGray(self.Button_ZhiYe_List[2], true);

            UICommonHelper.SetImageGray(self.Button_ZhiYe_List[index], false);

            int occ = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Occ;
            OccupationConfig occupationConfig = OccupationConfigCategory.Instance.Get(occ);
            self.OnSelectZhiYe(occupationConfig.OccTwoID[index]);

            //显示路线
            self.OccLine_1.SetActive(false);
            self.OccLine_2.SetActive(false);
            self.OccLine_3.SetActive(false);

            self.Button_ZhiYeSelect_1.SetActive(false);
            self.Button_ZhiYeSelect_2.SetActive(false);
            self.Button_ZhiYeSelect_3.SetActive(false);

            switch (index)
            {
                case 0:
                    self.OccLine_1.SetActive(true);
                    self.Button_ZhiYeSelect_1.SetActive(true);
                    break;
                case 1:
                    self.OccLine_2.SetActive(true);
                    self.Button_ZhiYeSelect_2.SetActive(true);
                    break;
                case 2:
                    self.OccLine_3.SetActive(true);
                    self.Button_ZhiYeSelect_3.SetActive(true);
                    break;
                    
            }
    }

        public static void OnSelectZhiYe(this UIOccTwoComponent self, int occTwoId)
        {
            self.OccTwoId = occTwoId;
            OccupationTwoConfig occupationTwoConfig = OccupationTwoConfigCategory.Instance.Get(occTwoId);
            self.Text_ZhiYe_4.GetComponent<Text>().text = occupationTwoConfig.OccupationName;
            UICommonHelper.DestoryChild(self.SkillContainer);

            self.Image_WuQi_Zhuan.GetComponent<Image>().sprite = ABAtlasHelp.GetIconSprite( ABAtlasTypes.OtherIcon, $"HuJia_{occupationTwoConfig.ArmorMastery}" );
            self.Image_WuQi_Type.GetComponent<Image>().sprite = ABAtlasHelp.GetIconSprite(ABAtlasTypes.OtherIcon, $"WuQi_{occupationTwoConfig.WeaponType}");

            self.Lab_HuJia.GetComponent<Text>().text = self.showType[occupationTwoConfig.ArmorMastery]+"专精";
            self.Lab_WuQi.GetComponent<Text>().text = self.showType[occupationTwoConfig.WeaponType] + "专精";

            self.Image_ZhiYe_4.GetComponent<Image>().sprite = ABAtlasHelp.GetIconSprite(ABAtlasTypes.OtherIcon, $"OccTwo_{occupationTwoConfig.Id}");

            self.OccNengLi_1.transform.Find("Text_NengLiValue").GetComponent<Text>().text = occupationTwoConfig.Capacitys[0].ToString();
            self.OccNengLi_2.transform.Find("Text_NengLiValue").GetComponent<Text>().text = occupationTwoConfig.Capacitys[1].ToString();
            self.OccNengLi_3.transform.Find("Text_NengLiValue").GetComponent<Text>().text = occupationTwoConfig.Capacitys[2].ToString();

            Log.Info("(float)occupationTwoConfig.Capacitys[0] * 1f / 100f = " + (float)occupationTwoConfig.Capacitys[0] * 1f / 100f);
            self.OccNengLi_1.transform.Find("ImageProgress").GetComponent<Image>().fillAmount = (float)occupationTwoConfig.Capacitys[0] * 1f / 100f;
            self.OccNengLi_2.transform.Find("ImageProgress").GetComponent<Image>().fillAmount = occupationTwoConfig.Capacitys[1] * 1f / 100f;
            self.OccNengLi_3.transform.Find("ImageProgress").GetComponent<Image>().fillAmount = occupationTwoConfig.Capacitys[2] * 1f / 100f;

            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonSkillItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            int[] skills = occupationTwoConfig.ShowTalentSkill;
            for (int i = 0; i < skills.Length; i++)
            {
                GameObject skillItem = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(skillItem, self.SkillContainer);
                skillItem.SetActive(true);
                skillItem.transform.localScale = Vector3.one * 1f;

                UICommonSkillItemComponent ui_item = self.AddChild<UICommonSkillItemComponent, GameObject>( skillItem);
                ui_item.OnUpdateUI(skills[i]);
            }
        }

        public static async ETTask OnButtonOccReset(this UIOccTwoComponent self)
        {
            GlobalValueConfig globalValueConfig = GlobalValueConfigCategory.Instance.Get(29);
            int needGold = int.Parse(globalValueConfig.Value);
            PopupTipHelp.OpenPopupTip(self.ZoneScene(), "技能点重置",
                $"是否花费{needGold}钻石重置技能点",
                () =>
                {
                    self.RequestReset(2).Coroutine();
                }).Coroutine();

            await ETTask.CompletedTask;
        }

        public static async ETTask RequestReset(this UIOccTwoComponent self, int operation)
        {
            GlobalValueConfig globalValueConfig = GlobalValueConfigCategory.Instance.Get(29);
            int needGold = int.Parse(globalValueConfig.Value);
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            if (userInfoComponent.UserInfo.Diamond < needGold)
            {
                ErrorHelp.Instance.ErrorHint(ErrorCode.ERR_DiamondNotEnoughError);
                return;
            }

            C2M_SkillOperation c2M_SkillSet = new C2M_SkillOperation() { OperationType = operation };
            M2C_SkillOperation m2C_SkillSet = (M2C_SkillOperation)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_SkillSet);
            if (m2C_SkillSet.Error != 0)
            {
                return;
            }
            userInfoComponent.UserInfo.OccTwo = 0;
            HintHelp.GetInstance().DataUpdate(DataType.SkillReset);
        }

        public static void OnClickOccTwoui(this UIOccTwoComponent self)
        {
            UIHelper.Remove(self.DomainScene(), UIType.UIOccTwo);
        }
    }
}
