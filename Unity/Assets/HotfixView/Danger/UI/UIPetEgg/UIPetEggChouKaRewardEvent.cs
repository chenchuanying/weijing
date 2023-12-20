﻿using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UIPetEggChouKaReward)]
    public class UIPetEggChouKaRewardEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIPetEggChouKaReward);
            await ETTask.CompletedTask;
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UI ui = uiComponent.AddChild<UI, string, GameObject>( UIType.UIPetEggChouKaReward, gameObject);
            ui.AddComponent<UIPetEggChouKaRewardComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIPetEggChouKaReward);
            ResourcesComponent.Instance.UnLoadAsset(path);
        }
    }
}