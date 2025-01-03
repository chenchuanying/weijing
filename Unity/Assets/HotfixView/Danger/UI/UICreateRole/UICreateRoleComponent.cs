using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace ET
{
    public class UICreateRoleComponent : Entity, IAwake
    {


        public GameObject Btn_Occ4;
        public GameObject Icon_4_2;
        public GameObject Icon_4_1;
        public GameObject Icon_3_2;
        public GameObject Icon_3_1;
        public GameObject Icon_2_2;
        public GameObject Icon_2_1;
        public GameObject Icon_1_2;
        public GameObject Icon_1_1;
        public GameObject ImageButton;
        public GameObject ButtonRight;
        public GameObject ButtonLeft;
        public GameObject Text_Desc;
        public GameObject BtnRandomName;
        public GameObject SkillListNode;
        public GameObject FunctionSetBtn;
        public GameObject BtnCreateRole;
        public GameObject InputCreateRoleName;
        public GameObject OccShow_ZhanShi;
        public GameObject OccShow_FaShi;
        public GameObject UICreateRoleSkillItem;

        public GameObject RawImage;
        public UIModelShowComponent uIModelShowComponent;
        public int Occ;
        public UI uIPageView;
        public List<int> OccList;
        public float LastCrateRoleTime;

        public UI UIModelShowComponent;
        public ETCancellationToken eTCancellation = null;
        public List<UICommonSkillItemComponent> SkillUIList = new List<UICommonSkillItemComponent>();

        public int PageIndex = 0;
    }


    public class UICreateRoleComponentAwakeSystem : AwakeSystem<UICreateRoleComponent>
    {

        public override void Awake(UICreateRoleComponent self)
        {
            self.LastCrateRoleTime = 0;
            self.OccList = new List<int>() { 1 , 2, 3, 4 };
            self.SkillUIList.Clear();
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.RawImage = rc.Get<GameObject>("RawImage");
            self.ImageButton = rc.Get<GameObject>("ImageButton");
            self.ImageButton.GetComponent<Button>().onClick.AddListener(() => { self.OnClickImageButton().Coroutine(); });
            self.ButtonRight = rc.Get<GameObject>("ButtonRight");
            self.ButtonLeft = rc.Get<GameObject>("ButtonLeft");
            self.ButtonRight.GetComponent<Button>().onClick.AddListener(() => { self.OnClickSelectButton(1); });
            self.ButtonLeft.GetComponent<Button>().onClick.AddListener(() => { self.OnClickSelectButton(-1); });

            self.OccShow_ZhanShi = rc.Get<GameObject>("OccShow_ZhanShi");
            self.OccShow_FaShi = rc.Get<GameObject>("OccShow_FaShi");
            self.UICreateRoleSkillItem = rc.Get<GameObject>("UICreateRoleSkillItem");
            self.UICreateRoleSkillItem.SetActive(false);

            self.Text_Desc = rc.Get<GameObject>("Text_Desc");
            self.BtnRandomName = rc.Get<GameObject>("BtnRandomName");
            self.BtnRandomName.GetComponent<Button>().onClick.AddListener(() => { self.OnClickRandomName(); });

            self.SkillListNode = rc.Get<GameObject>("SkillListNode");
            self.InputCreateRoleName = rc.Get<GameObject>("InputCreateRoleName");
            self.InputCreateRoleName.GetComponent<InputField>().onValueChanged.AddListener((string text) => { self.CheckSensitiveWords(); });


            self.Btn_Occ4 = rc.Get<GameObject>("Btn_Occ4");
            self.Btn_Occ4.SetActive( true );

            self.Icon_4_2 = rc.Get<GameObject>("Icon_4_2");
            self.Icon_4_1 = rc.Get<GameObject>("Icon_4_1");
            self.Icon_3_2 = rc.Get<GameObject>("Icon_3_2");
            self.Icon_3_1 = rc.Get<GameObject>("Icon_3_1");
            self.Icon_2_2 = rc.Get<GameObject>("Icon_2_2");
            self.Icon_2_1 = rc.Get<GameObject>("Icon_2_1");
            self.Icon_1_2 = rc.Get<GameObject>("Icon_1_2");
            self.Icon_1_1 = rc.Get<GameObject>("Icon_1_1");

            UICommonHelper.ShowOccIcon(self.Icon_4_2, 4);
            UICommonHelper.ShowOccIcon(self.Icon_4_1, 4);
            UICommonHelper.ShowOccIcon(self.Icon_3_2, 3);
            UICommonHelper.ShowOccIcon(self.Icon_3_1, 3);
            UICommonHelper.ShowOccIcon(self.Icon_2_2, 2);
            UICommonHelper.ShowOccIcon(self.Icon_2_1, 2);
            UICommonHelper.ShowOccIcon(self.Icon_1_2, 1);
            UICommonHelper.ShowOccIcon(self.Icon_1_1, 1);

            self.BtnCreateRole = rc.Get<GameObject>("BtnCreateRole");
            ButtonHelp.AddListenerEx(self.BtnCreateRole, () => { self.OnBtnCreateRole().Coroutine(); });

            GameObject BtnItemTypeSet = rc.Get<GameObject>("FunctionSetBtn");
            UI uiJoystick = self.AddChild<UI, string, GameObject>( "FunctionBtnSet", BtnItemTypeSet);


            //ios适配
            IPHoneHelper.SetPosition(BtnItemTypeSet, new Vector2(200f, 298f));


            self.uIPageView = uiJoystick;
            UIPageButtonComponent uIPageViewComponent = uiJoystick.AddComponent<UIPageButtonComponent>();
            uIPageViewComponent.SetClickHandler((int page) => {
                self.OnClickPageButton(page);
            });
            uIPageViewComponent.OnSelectIndex(0);

            self.OnClickRandomName();
        }
    }

    public static class UICreateRoleComponentSystem
    {
        public static void InitModelShowView(this UICreateRoleComponent self)
        {
            //模型展示界面
            var path = ABPathHelper.GetUGUIPath("Common/UIModelShow1");
            GameObject bundleGameObject =  ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UICommonHelper.SetParent(gameObject, self.RawImage);
            gameObject.transform.localPosition = new Vector3(0, 0, 0);
            gameObject.transform.Find("Camera").localPosition = new Vector3(0f, 70f, 150f);

            UI ui = self.AddChild<UI, string, GameObject>( "UIModelShow", gameObject);
            self.uIModelShowComponent = ui.AddComponent<UIModelShowComponent, GameObject>(self.RawImage);
            self.RawImage.SetActive(true);
        }
        
        public static void ShowHeroSelect(this UICreateRoleComponent self, int occ)
        {
            self.eTCancellation?.Cancel();
            self.eTCancellation = new ETCancellationToken();
            if (self.uIModelShowComponent == null)
            {
                self.InitModelShowView();
            }
            self.uIModelShowComponent.ShowPlayerModel(new BagInfo(), occ, 0, new List<int>() { });
            self.ShowSelectEff().Coroutine();
        }

        public static async ETTask ShowSelectEff(this UICreateRoleComponent self) {
            GameObject child = GameObject.Find("Effect_CreateSelect") ;
            for (int c = 0; c < child.transform.childCount; c++)
            {
                child.transform.GetChild(c).gameObject.SetActive(true);
            }
            await TimerComponent.Instance.WaitAsync(500);
            for (int c = 0; c < child.transform.childCount; c++)
            {
                child.transform.GetChild(c).gameObject.SetActive(false);
            }
        }

        public static void CheckSensitiveWords(this UICreateRoleComponent self)
        {
            string text_new = "";
            string text_old = self.InputCreateRoleName.GetComponent<InputField>().text;
            MaskWordHelper.Instance.IsContainSensitiveWords(ref text_old, out text_new);
            text_old = text_old.Replace("*", "");
            self.InputCreateRoleName.GetComponent<InputField>().text = text_old;
        }

        public static async ETTask OnClickImageButton(this UICreateRoleComponent self)
        {
            await UIHelper.Create(self.DomainScene(), UIType.UILobby);
            UIHelper.Remove(  self.DomainScene(), UIType.UICreateRole );
        }

        public static async ETTask OnBtnCreateRole(this UICreateRoleComponent self)
        {
            string createName = self.InputCreateRoleName.GetComponent<InputField>().text;

            if (Time.time - self.LastCrateRoleTime < 3f)
            {
                return;
            }
            if (createName.Contains("*") 
                || !StringHelper.IsSpecialChar(createName))
            {
                FloatTipManager.Instance.ShowFloatTip("名字不合法!");
                return;
            }
            Session session = self.ZoneScene().GetComponent<SessionComponent>().Session;
            if (session == null || session.IsDisposed)
            {
                FloatTipManager.Instance.ShowFloatTip(GameSettingLanguge.LoadLocalization("已掉线，请重新连接!"));
                return;
            }


            int TodayCreateRole = self.ZoneScene().GetComponent<AccountInfoComponent>().TodayCreateRole;
            if (TodayCreateRole < 5)
            {
                self.RequestCreateRole(createName).Coroutine();
            }
            else
            {
                PopupTipHelp.OpenPopupTip(self.ZoneScene(), "系统提示", $"每天只能建立8个角色,当前已经创建了{TodayCreateRole}个角色", () =>
                {
                    self.RequestCreateRole(createName).Coroutine();
                }, null).Coroutine();

            }
        }

        public static async ETTask RequestCreateRole(this UICreateRoleComponent self, string createName)
        {
            long instanceid = self.InstanceId;
            A2C_CreateRoleData g2cCreateRole = await LoginHelper.CreateRole(self.DomainScene(), self.Occ, createName);
            if (g2cCreateRole == null || g2cCreateRole.Error != 0 || instanceid != self.InstanceId)
            {
                return;
            }

            self.DomainScene().GetComponent<AccountInfoComponent>().CreateRoleList.Add(g2cCreateRole.createRoleInfo);
            self.ZoneScene().GetComponent<AccountInfoComponent>().TodayCreateRole++;
            UI uI = await UIHelper.Create(self.DomainScene(), UIType.UILobby);
            uI.GetComponent<UILobbyComponent>().OnCreateRoleData(g2cCreateRole.createRoleInfo, self.PageIndex);

            UIHelper.Remove(self.DomainScene(), UIType.UICreateRole);
        }

        public static void OnClickRandomName(this UICreateRoleComponent self)
        {
            string randomNameStr = "";
            int xingXuHaoMax = GameSettingLanguge.Instance.randomName_xing.Length - 1;
            int nameXuHaoMax = GameSettingLanguge.Instance.randomName_name.Length - 1;
            int xingXuHao = FunctionUI.GetInstance().ReturnRamdomValueInt(0, xingXuHaoMax);
            int nameXuHao = FunctionUI.GetInstance().ReturnRamdomValueInt(0, nameXuHaoMax);
            randomNameStr = GameSettingLanguge.Instance.randomName_xing[xingXuHao] + GameSettingLanguge.Instance.randomName_name[nameXuHao];

            if (randomNameStr != "")
            {
                randomNameStr = randomNameStr.Replace("*", "");
                self.InputCreateRoleName.GetComponent<InputField>().text = randomNameStr;
            }
        }

        public static void OnClickPageButton(this UICreateRoleComponent self, int page)
        {
            self.Occ = page + 1;
            self.ShowHeroSelect(self.Occ);
            self.OnUpdateOccInfo();
        }

        public static void  OnUpdateOccInfo(this UICreateRoleComponent self)
        {
            long instanceid = self.InstanceId;
            self.eTCancellation?.Cancel();
            self.eTCancellation = new ETCancellationToken();
            //if (self.uIModelShowComponent == null)
            //{
            //    self.InitModelShowView();
            //}
            //self.uIModelShowComponent.ShowPlayerModel(new BagInfo(), self.Occ);
            OccupationConfig occupationConfig = OccupationConfigCategory.Instance.Get(self.Occ);
            if (instanceid != self.InstanceId)
            {
                return;
            }

            for (int i = 0; i < occupationConfig.InitSkillID.Length; i++)
            {
                UICommonSkillItemComponent ui_1;
                if (occupationConfig.InitSkillID[i] == 0)
                {
                    continue;
                }
                if (i < self.SkillUIList.Count)
                {
                    ui_1 = self.SkillUIList[i];
                    ui_1.GameObject.SetActive(true);
                }
                else
                {
                    GameObject taskTypeItem = GameObject.Instantiate(self.UICreateRoleSkillItem);
                    taskTypeItem.SetActive(true);
                    UICommonHelper.SetParent(taskTypeItem, self.SkillListNode);
                    ui_1 = self.AddChild<UICommonSkillItemComponent, GameObject>(taskTypeItem);
                    self.SkillUIList.Add(ui_1);
                }
                ui_1.OnUpdateUI(occupationConfig.InitSkillID[i]);
            }
            for (int i = occupationConfig.InitSkillID.Length; i < self.SkillUIList.Count; i++)
            {
                self.SkillUIList[i].GameObject.SetActive(false);
            }

            //显示职业介绍
            self.OccShow_ZhanShi.SetActive(false);
            self.OccShow_FaShi.SetActive(false);
            Log.Info("self.Occ = " + self.Occ);
            if (self.Occ == 1) 
            {
                self.OccShow_ZhanShi.SetActive(true);
            }

            if (self.Occ == 2)
            {
                self.OccShow_FaShi.SetActive(true);
            }
        }

        public static void OnClickSelectButton(this UICreateRoleComponent self, int direction)
        {
            //int occ = self.Occ + direction;

            //if (occ > self.OccList.Count)
            //    occ = occ % self.OccList.Count;
            //if (occ <= 0)
            //    occ = self.OccList.Count;

            //self.uIPageView.GetComponent<UIPageButtonComponent>().OnSelectIndex(occ - 1);

            //self.ShowSelectEff().Coroutine();
        }

    }
}
