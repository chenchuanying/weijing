﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public enum UnionXiuLianEnum: int
    {
        UnionRoleXiuLian = 0,
        UnionPetXiuLian = 1,
        UnionBloodStone = 2,
        
        Number,
    }

    public class UIUnionXiuLianComponent: Entity, IAwake
    {

        public GameObject Btn_3;

        public GameObject SubViewNode;
        public GameObject FunctionSetBtn;

        public UIPageViewComponent UIPageView;
        public UIPageButtonComponent UIPageButton;
    }

    public class UIUnionXiuLianComponentAwakeSystem: AwakeSystem<UIUnionXiuLianComponent>
    {
        public override void Awake(UIUnionXiuLianComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            GameObject pageView = rc.Get<GameObject>("SubViewNode");
            UI uiPageView = self.AddChild<UI, string, GameObject>("FunctionBtnSet", pageView);
            UIPageViewComponent pageViewComponent = uiPageView.AddComponent<UIPageViewComponent>();
            pageViewComponent.UISubViewList = new UI[(int)PaiMaiPageEnum.Number];
            pageViewComponent.UISubViewPath = new string[(int)PaiMaiPageEnum.Number];
            pageViewComponent.UISubViewType = new Type[(int)PaiMaiPageEnum.Number];

            pageViewComponent.UISubViewPath[(int)UnionXiuLianEnum.UnionRoleXiuLian] = ABPathHelper.GetUGUIPath("Main/Union/UIUnionRoleXiuLian");
            pageViewComponent.UISubViewPath[(int)UnionXiuLianEnum.UnionPetXiuLian] = ABPathHelper.GetUGUIPath("Main/Union/UIUnionPetXiuLian");
            pageViewComponent.UISubViewPath[(int)UnionXiuLianEnum.UnionBloodStone] = ABPathHelper.GetUGUIPath("Main/Union/UIUnionBloodStone");

            pageViewComponent.UISubViewType[(int)UnionXiuLianEnum.UnionRoleXiuLian] = typeof (UIUnionRoleXiuLianComponent);
            pageViewComponent.UISubViewType[(int)UnionXiuLianEnum.UnionPetXiuLian] = typeof (UIUnionPetXiuLianComponent);
            pageViewComponent.UISubViewType[(int)UnionXiuLianEnum.UnionBloodStone] = typeof (UIUnionBloodStoneComponent);
            self.UIPageView = pageViewComponent;

            self.Btn_3 = rc.Get<GameObject>("Btn_3");
            self.Btn_3.SetActive(true);

            self.FunctionSetBtn = rc.Get<GameObject>("FunctionSetBtn");
            UI ui = self.AddChild<UI, string, GameObject>("FunctionSetBtn", self.FunctionSetBtn);

            //IOS适配
            IPHoneHelper.SetPosition(self.FunctionSetBtn, new Vector2(300f, 316f));

            UIPageButtonComponent uIPageButtonComponent = ui.AddComponent<UIPageButtonComponent>();
            uIPageButtonComponent.SetClickHandler((int page) => { self.OnClickPageButton(page); });
            uIPageButtonComponent.OnSelectIndex(0);
            self.UIPageButton = uIPageButtonComponent;

            uIPageButtonComponent.CheckHandler = (int page) => { return self.CheckPageButton_1(page); };
        }
    }

    public static class UIUnionXiuLianComponentSystem
    {

        public static bool CheckPageButton_1(this UIUnionXiuLianComponent self, int page)
        {
            if (page == (int)UnionXiuLianEnum.UnionBloodStone)
            {
                UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();   
                if (userInfoComponent.UserInfo.Lv < 60)
                {
                    FloatTipManager.Instance.ShowFloatTip("60级开启血石系统！");
                    return false;
                }
            }
            return true;
        }

        public static void OnClickPageButton(this UIUnionXiuLianComponent self, int page)
        {
            self.UIPageView.OnSelectIndex(page).Coroutine();
        }
    }
}