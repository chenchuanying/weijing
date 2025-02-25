﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIJiaYuanDaShiShowComponent : Entity, IAwake, IDestroy
    {
        public string AssetPath;
        public GameObject BuildingList1;

        public List<UIJiaYuanDaShiShowItemComponent> uIJiaYuanDaShis = new List<UIJiaYuanDaShiShowItemComponent>();
    }

    public class UIJiaYuanDaShiShowComponentAwake : AwakeSystem<UIJiaYuanDaShiShowComponent>
    {
        public override void Awake(UIJiaYuanDaShiShowComponent self)
        {
            self.uIJiaYuanDaShis.Clear();
            self.AssetPath = string.Empty;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.BuildingList1 = rc.Get<GameObject>("BuildingList1");

            self.GetParent<UI>().OnUpdateUI = self.OnUpdateUI;
        }
    }

    public class UIJiaYuanDaShiShowComponentDestroy : DestroySystem<UIJiaYuanDaShiShowComponent>
    {
        public override void Destroy(UIJiaYuanDaShiShowComponent self)
        {
            if (!string.IsNullOrEmpty(self.AssetPath))
            {
                ResourcesComponent.Instance.UnLoadAsset(self.AssetPath);
            }
            self.AssetPath = string.Empty;
        }
    }


    public static class UIJiaYuanDaShiShowComponentSystem
    {
        public static void OnUpdateUI(this UIJiaYuanDaShiShowComponent self)
        {
            var path = ABPathHelper.GetUGUIPath("JiaYuan/UIJiaYuanDaShiShowItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            self.AssetPath = path;
           JiaYuanComponent jiaYuanComponent = self.ZoneScene().GetComponent<JiaYuanComponent>();
            List<KeyValuePair> jiayuandashi = ConfigHelper.JiaYuanDaShiPro;
            for (int i = 0; i < jiayuandashi.Count / 3; i++)
            {
                UIJiaYuanDaShiShowItemComponent ui_1 = null;
                if (i < self.uIJiaYuanDaShis.Count)
                {
                    ui_1 = self.uIJiaYuanDaShis[i];
                }
                else
                {
                    GameObject gameObject = GameObject.Instantiate(bundleGameObject);
                    UICommonHelper.SetParent(gameObject, self.BuildingList1);
                    ui_1 = self.AddChild<UIJiaYuanDaShiShowItemComponent, GameObject>(gameObject);
                    self.uIJiaYuanDaShis.Add(ui_1);
                }
                ui_1.OnUpdateUI(i, jiaYuanComponent.JiaYuanDaShiTime_1);
            }
        }
    }
}