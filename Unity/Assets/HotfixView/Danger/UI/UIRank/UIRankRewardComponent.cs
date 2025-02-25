﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{

    public class UIRankRewardComponent : Entity, IAwake
    {
        public GameObject CloseButton;
        public GameObject RewardListNode;
        public Action ClickOnClose;
    }


    public class UIRankRewardComponentAwakeSystem : AwakeSystem<UIRankRewardComponent>
    {
        public override void Awake(UIRankRewardComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.CloseButton = rc.Get<GameObject>("CloseButton");
            self.CloseButton.GetComponent<Button>().onClick.AddListener(() => { self.OnCloseButton(); });

            self.RewardListNode = rc.Get<GameObject>("RewardListNode");
            self.OnInitUI(1);
        }
    }

    public static class UIRankRewardComponentSytstem
    {
        public static void OnCloseButton(this UIRankRewardComponent self)
        {
            self.ClickOnClose?.Invoke();
        }

        public static  void OnInitUI(this UIRankRewardComponent self, int rankType)
        {
            long instanceid = self.InstanceId;
            var path = ABPathHelper.GetUGUIPath("Main/Rank/UIRankRewardItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
         
            List<RankRewardConfig> rankRewardConfigs = RankHelper.GetTypeRankRewards(1);
            for (int i = 0; i < rankRewardConfigs.Count; i++ )
            {
                GameObject go = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(go, self.RewardListNode);
                self.AddChild<UIRankRewardItemComponent, GameObject>(go, true).OnUpdateUI(rankRewardConfigs[i]);
            }
        }
    }
}
