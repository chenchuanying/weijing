﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;


namespace ET
{
    public class UIZhanQuLevelComponent : Entity, IAwake, IDestroy
    {
        public GameObject ItemNodeList;
        public GameObject Lab_MyLv;
        public string AssetPath = string.Empty;
    }


    public class UIZhanQuLevelComponentAwake : AwakeSystem<UIZhanQuLevelComponent>
    {
        public override void Awake(UIZhanQuLevelComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.AssetPath = string.Empty;
            self.ItemNodeList = rc.Get<GameObject>("ItemNodeList");
            self.Lab_MyLv = rc.Get<GameObject>("Lab_MyLv");

            self.OnInitUI().Coroutine();
        }
    }

    public class UIZhanQuLevelComponentDestroy : DestroySystem<UIZhanQuLevelComponent>
    {
        public override void Destroy(UIZhanQuLevelComponent self)
        {
            if (!string.IsNullOrEmpty(self.AssetPath))
            { 
                ResourcesComponent.Instance.UnLoadAsset(self.AssetPath);    
            }
        }
    }

    public static class UIZhanQuLevelComponentSystem
    {
        public static async ETTask OnInitUI(this UIZhanQuLevelComponent self)
        {
            long instanceid = self.InstanceId;
            var path = ABPathHelper.GetUGUIPath("Main/ZhanQu/UIZhanQuLevelItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            self.AssetPath = path;
            if (instanceid != self.InstanceId)
            {
                return;
            }
            List<ActivityConfig> activityConfigs = ActivityConfigCategory.Instance.GetAll().Values.ToList();
            for (int i = 0; i < activityConfigs.Count; i++)
            {
                if (activityConfigs[i].ActivityType != 21)
                {
                    continue;
                }

                GameObject bagSpace = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(bagSpace, self.ItemNodeList);

                UI ui_item = self.AddChild<UI, string, GameObject>( "UIItem_" + i.ToString(), bagSpace);
                UIZhanQuLevelItemComponent uIItemComponent = ui_item.AddComponent<UIZhanQuLevelItemComponent>();
                uIItemComponent.OnInitUI(activityConfigs[i]);
            }

            self.Lab_MyLv.GetComponent<Text>().text = "我的等级：" + self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Lv.ToString();
        }
        
    }

}
