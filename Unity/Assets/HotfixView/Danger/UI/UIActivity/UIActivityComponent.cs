﻿using System;
using UnityEngine;

namespace ET
{

    public enum ActivityPageEnum : int
	{
		YueKa = 0,
		MaoXian = 1,
		Token = 2,
		TeHui = 3,
		Recharge = 4,
		Number,
	}

	public class UIActivityComponent : Entity, IAwake
	{
		public UIPageViewComponent UIPageView;
		public UIPageButtonComponent UIPageButton;

		public ActivityComponent ActivityComponent;
	}


	public class UIActivityComponentAwakeSystem : AwakeSystem<UIActivityComponent>
	{
		public override void Awake(UIActivityComponent self)
		{
			ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

			GameObject pageView = rc.Get<GameObject>("SubViewNode");
			UI uiPageView = self.AddChild<UI, string, GameObject>("FunctionBtnSet", pageView);

			//IOS适配
			IPHoneHelper.SetPosition(rc.Get<GameObject>("FunctionSetBtn"), new Vector2(300f, 316f));

			UIPageViewComponent pageViewComponent = uiPageView.AddComponent<UIPageViewComponent>();
			pageViewComponent.UISubViewList = new UI[(int)ActivityPageEnum.Number];
			pageViewComponent.UISubViewPath = new string[(int)ActivityPageEnum.Number];
			pageViewComponent.UISubViewType = new Type[(int)ActivityPageEnum.Number];

			pageViewComponent.UISubViewPath[(int)ActivityPageEnum.YueKa] = ABPathHelper.GetUGUIPath("Main/Activity/UIActivityYueKa");
			pageViewComponent.UISubViewPath[(int)ActivityPageEnum.MaoXian] = ABPathHelper.GetUGUIPath("Main/Activity/UIActivityMaoXian");
			pageViewComponent.UISubViewPath[(int)ActivityPageEnum.Token] = ABPathHelper.GetUGUIPath("Main/Activity/UIActivityToken");
			pageViewComponent.UISubViewPath[(int)ActivityPageEnum.TeHui] = ABPathHelper.GetUGUIPath("Main/Activity/UIActivityTeHui");
			pageViewComponent.UISubViewPath[(int)ActivityPageEnum.Recharge] = ABPathHelper.GetUGUIPath("Main/Recharge/UIRecharge");

			pageViewComponent.UISubViewType[(int)ActivityPageEnum.YueKa] = typeof(UIActivityYueKaComponent);
			pageViewComponent.UISubViewType[(int)ActivityPageEnum.MaoXian] = typeof(UIActivityMaoXianComponent);
			pageViewComponent.UISubViewType[(int)ActivityPageEnum.Token] = typeof(UIActivityTokenComponent );
			pageViewComponent.UISubViewType[(int)ActivityPageEnum.TeHui] = typeof(UIActivityTeHuiComponent);
			pageViewComponent.UISubViewType[(int)ActivityPageEnum.Recharge] = typeof(UIRechargeComponent);
			self.UIPageView = pageViewComponent;

			//单选组件
			GameObject BtnItemTypeSet = rc.Get<GameObject>("FunctionSetBtn");
			UI uiPage = self.AddChild<UI, string, GameObject>("FunctionSetBtn", BtnItemTypeSet);
			UIPageButtonComponent uIPageViewComponent = uiPage.AddComponent<UIPageButtonComponent>();
			uIPageViewComponent.SetClickHandler((int page) => {
				self.OnClickPageButton(page);
			});

			self.UIPageButton = uIPageViewComponent;
			uIPageViewComponent.ClickEnabled = false;
			self.ActivityComponent = self.ZoneScene().GetComponent<ActivityComponent>();
			self.RequeatActivityInfo().Coroutine();
		}
	}

	public static class UIActivityComponentSystem
	{

		//点击回调
		public static void OnClickPageButton(this UIActivityComponent self, int page)
		{
			self.UIPageView.OnSelectIndex(page).Coroutine();
		}

		public static async ETTask RequeatActivityInfo(this UIActivityComponent self)
		{
			await NetHelper.RequestActivityInfo(self.ZoneScene());
			self.UIPageButton.ClickEnabled = true;
			self.UIPageButton.OnSelectIndex(0);
		}
	}

}
