using System;
using UnityEngine;

namespace ET
{

    [UIEvent(UIType.UIItemChangeOcc)]
    public class UIItemChangeOccEvent : AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIItemChangeOcc);
            await ETTask.CompletedTask;
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UI ui = uiComponent.AddChild<UI, string, GameObject>(UIType.UIItemChangeOcc, gameObject);
            ui.AddComponent<UIItemChangeOccComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            var path = ABPathHelper.GetUGUIPath(UIType.UIItemChangeOcc);
            ResourcesComponent.Instance.UnLoadAsset(path);

        }

    }
}
