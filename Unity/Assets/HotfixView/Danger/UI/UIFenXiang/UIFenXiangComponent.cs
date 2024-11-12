using System;
using UnityEngine;
using cn.sharesdk.unity3d;

namespace ET 
{
    public enum FenXiangPageEnum : int
    {
        Set = 0,
        Popularize = 1,
        Serial = 2,
        LunTan = 3,
        QQGroup = 4,

        Number,
    }

    public class UIFenXiangComponent : Entity, IAwake, IDestroy
    {
        public GameObject Btn_Type1;
        public GameObject Btn_Type2;
        public GameObject Btn_Type3;
        public GameObject Btn_Type4;
        public GameObject Btn_Type5;
        public GameObject SubViewNode;
        public GameObject FunctionSetBtn;

        public UIPageViewComponent UIPageView;
        public UIPageButtonComponent UIPageButtonComponent;
    }


    public class UIFenXiangComponentAwake : AwakeSystem<UIFenXiangComponent>
    {
        public override void Awake(UIFenXiangComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.SubViewNode = rc.Get<GameObject>("SubViewNode");

            GameObject pageView = rc.Get<GameObject>("SubViewNode");
            UI uiPageView = self.AddChild<UI, string, GameObject>("FunctionBtnSet", pageView);
            UIPageViewComponent pageViewComponent = uiPageView.AddComponent<UIPageViewComponent>();
            pageViewComponent.UISubViewList = new UI[(int)FenXiangPageEnum.Number];
            pageViewComponent.UISubViewPath = new string[(int)FenXiangPageEnum.Number];
            pageViewComponent.UISubViewType = new Type[(int)FenXiangPageEnum.Number];

            pageViewComponent.UISubViewPath[(int)FenXiangPageEnum.Set] = ABPathHelper.GetUGUIPath("Main/FenXiang/UIFenXiangSet");
            pageViewComponent.UISubViewPath[(int)FenXiangPageEnum.Popularize] = ABPathHelper.GetUGUIPath("Main/FenXiang/UIPopularize");
            pageViewComponent.UISubViewPath[(int)FenXiangPageEnum.Serial] = ABPathHelper.GetUGUIPath("Main/FenXiang/UISerial");
            pageViewComponent.UISubViewPath[(int)FenXiangPageEnum.LunTan] = ABPathHelper.GetUGUIPath("Main/FenXiang/UILunTan");
            pageViewComponent.UISubViewPath[(int)FenXiangPageEnum.QQGroup] = ABPathHelper.GetUGUIPath("Main/FenXiang/UIFenXiangQQAddSet");

            pageViewComponent.UISubViewType[(int)FenXiangPageEnum.Set] = typeof(UIFenXiangSetComponent);
            pageViewComponent.UISubViewType[(int)FenXiangPageEnum.Popularize] = typeof(UIPopularizeComponent);
            pageViewComponent.UISubViewType[(int)FenXiangPageEnum.Serial] = typeof(UISerialComponent);
            pageViewComponent.UISubViewType[(int)FenXiangPageEnum.LunTan] = typeof(UILunTanComponent);
            pageViewComponent.UISubViewType[(int)FenXiangPageEnum.QQGroup] = typeof(UIQQAddSetComponent);

            self.UIPageView = pageViewComponent;

            self.FunctionSetBtn = rc.Get<GameObject>("FunctionSetBtn");
            //IOS适配
            IPHoneHelper.SetPosition(self.FunctionSetBtn, new Vector2(300f, 316f));


            self.Btn_Type1 = rc.Get<GameObject>("Btn_Type1");
            self.Btn_Type2 = rc.Get<GameObject>("Btn_Type2");
            self.Btn_Type3 = rc.Get<GameObject>("Btn_Type3");
            self.Btn_Type4 = rc.Get<GameObject>("Btn_Type4");
            self.Btn_Type5 = rc.Get<GameObject>("Btn_Type5");
            if (GlobalHelp.GetPlatform() == 5 || GlobalHelp.GetPlatform() == 6)
            {
                self.Btn_Type4.SetActive(false);
            }

            if (GlobalHelp.IsBanHaoMode)
            {
                self.Btn_Type2.SetActive(false);
                self.Btn_Type3.SetActive(false);
                self.Btn_Type4.SetActive(false);
                self.Btn_Type5.SetActive(false);
            }

            UI uiPageButton = self.AddChild<UI, string, GameObject>("FunctionSetBtn", self.FunctionSetBtn);
            UIPageButtonComponent uIPageButtonComponent = uiPageButton.AddComponent<UIPageButtonComponent>();
            uIPageButtonComponent.SetClickHandler((int page) =>
            {
                self.OnClickPageButton(page);
            });

            uIPageButtonComponent.CheckHandler = (int page) => { return self.CheckPageButton_1(page); };
            self.UIPageButtonComponent = uIPageButtonComponent;
            
            // 主播模式隐藏部分内容
            if (PlayerPrefsHelp.GetInt(PlayerPrefsHelp.ZhuBo) == 1)
            {
                rc.Get<GameObject>("Btn_Type1").SetActive(false);
                rc.Get<GameObject>("Btn_Type2").SetActive(false);
                rc.Get<GameObject>("Btn_Type4").SetActive(false);
                rc.Get<GameObject>("Btn_Type5").SetActive(false);

                //Vector3 vector3 = rc.Get<GameObject>("Btn_Type1").transform.position;
                //rc.Get<GameObject>("Btn_Type3").transform.position = vector3;
                // vector3.y -= 160;
                // rc.Get<GameObject>("Btn_Type4").transform.position = vector3;
                
                self.UIPageButtonComponent.OnSelectIndex(2);
            }
            else
            {
                self.UIPageButtonComponent.OnSelectIndex(0);
            }
        }
    }

    public static class UIFenXiangComponentSystem
    {

        public static bool CheckPageButton_1(this UIFenXiangComponent self, int page)
        {
            if (page == (int)FenXiangPageEnum.Popularize)
            {
                Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
                int openPaiMai = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.PaiMaiOpen);
                if (openPaiMai == 0)
                {
                    return true;
                }

                UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
                int createDay = userInfoComponent.GetCrateDay();
                //if (createDay <= 1 && userInfoComponent.UserInfo.Lv <= 10)
                //{
                //    return true;
                //}

                if (ComHelp.IsCanPaiMai_Recharge(unit))
                {
                    return true;
                }

                if (ComHelp.IsCanPaiMai_KillBoss(userInfoComponent.UserInfo.MonsterRevives, userInfoComponent.UserInfo.Lv))
                {
                    return true;
                }

                int needLv = ComHelp.IsCanPaiMai_Level(createDay, userInfoComponent.UserInfo.Lv);
                if (needLv == 0)
                {
                    return true;
                }
                FloatTipManager.Instance.ShowFloatTip($"需要等级达到{needLv}");
                return false;
            }
            return true;
        }

        public static void OnClickPageButton(this UIFenXiangComponent self, int page)
        {
            self.UIPageView.OnSelectIndex(page).Coroutine();
        }
    }
}
