﻿using System;
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

            //IOS适配
            IPHoneHelper.SetPosition(self.FunctionSetBtn, new Vector2(300f, 316f));

            self.FunctionSetBtn = rc.Get<GameObject>("FunctionSetBtn");
            UI uiPageButton = self.AddChild<UI, string, GameObject>("FunctionSetBtn", self.FunctionSetBtn);
            UIPageButtonComponent uIPageButtonComponent = uiPageButton.AddComponent<UIPageButtonComponent>();
            uIPageButtonComponent.SetClickHandler((int page) =>
            {
                self.OnClickPageButton(page);
            });
           
            self.UIPageButtonComponent = uIPageButtonComponent;
            self.UIPageButtonComponent.OnSelectIndex(0);
        }
    }

    public static class UIFenXiangComponentSystem
    {
        public static void OnClickPageButton(this UIFenXiangComponent self, int page)
        {
            self.UIPageView.OnSelectIndex(page).Coroutine();
        }
    }
}
