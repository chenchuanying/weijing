﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIChengJiuActiviteComponent : Entity,IAwake,IDestroy
    {
        public GameObject Text_ChengJiuDesc;
        public GameObject Text_ChengJiuPoint;
        public GameObject Text_ChengJiuName;
        public GameObject ChengJiuIcon;
        
        public List<string> AssetPath = new List<string>();
    }


    public class UIChengJiuActiviteComponentAwake : AwakeSystem<UIChengJiuActiviteComponent>
    {
        public override void Awake(UIChengJiuActiviteComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_ChengJiuDesc = rc.Get<GameObject>("Text_ChengJiuDesc");
            self.Text_ChengJiuPoint = rc.Get<GameObject>("Text_ChengJiuPoint");
            self.Text_ChengJiuName = rc.Get<GameObject>("Text_ChengJiuName");
            self.ChengJiuIcon = rc.Get<GameObject>("ChengJiuIcon");
        }
    }
    public class UIChengJiuActiviteComponentDestroy : DestroySystem<UIChengJiuActiviteComponent>
    {
        public override void Destroy(UIChengJiuActiviteComponent self)
        {
            for(int i = 0; i < self.AssetPath.Count; i++)
            {
                if (!string.IsNullOrEmpty(self.AssetPath[i]))
                {
                    ResourcesComponent.Instance.UnLoadAsset(self.AssetPath[i]); 
                }
            }
            self.AssetPath = null;
        }
    }
    public static class UIChengJiuActiviteComponentSystem
    {
        public static async ETTask OnInitUI(this UIChengJiuActiviteComponent self, int chengjiuId)
        {
            ChengJiuConfig chengJiuConfig = ChengJiuConfigCategory.Instance.Get(chengjiuId);
            self.Text_ChengJiuDesc.GetComponent<Text>().text = chengJiuConfig.Des;
            self.Text_ChengJiuPoint.GetComponent<Text>().text = chengJiuConfig.RewardNum.ToString();
            self.Text_ChengJiuName.GetComponent<Text>().text = chengJiuConfig.Name;
            string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ChengJiuIcon, chengJiuConfig.Icon.ToString());
            Sprite sprite = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.ChengJiuIcon.GetComponent<Image>().sprite = sprite;

            long instanceId = self.InstanceId;
            await TimerComponent.Instance.WaitAsync(3000);
            if (instanceId != self.InstanceId)
            {
                return;
            }
            UIHelper.Remove(self.ZoneScene(), UIType.UIChengJiuActivite);
        }
    }
}
