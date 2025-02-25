﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIHuntRankingComponent: Entity, IAwake
    {
        public GameObject HuntingTimeText;
        public GameObject HeadImage_No1;
        public GameObject NameText_No1;
        public GameObject HuntNumText_No1;
        public GameObject HuntRankingListNode;
        public GameObject UIHuntRankingPlayerInfoItem;
        public GameObject HuntRewardsListNode1;
        public GameObject HuntRewardsListNode2;
        public GameObject HuntRewardsListNode3;

        public long EndTime;
    }

    public class UIHuntRankingComponentAwakesystem: AwakeSystem<UIHuntRankingComponent>
    {
        public override void Awake(UIHuntRankingComponent self)
        {
            self.Awake();
        }
    }

    public static class UIHuntRankingComponentSystem
    {
        public static void Awake(this UIHuntRankingComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.HuntingTimeText = rc.Get<GameObject>("HuntingTimeText");
            self.HeadImage_No1 = rc.Get<GameObject>("HeadImage_No1");
            self.NameText_No1 = rc.Get<GameObject>("NameText_No1");
            self.HuntNumText_No1 = rc.Get<GameObject>("HuntNumText_No1");
            self.HuntRankingListNode = rc.Get<GameObject>("HuntRankingListNode");
            self.UIHuntRankingPlayerInfoItem = rc.Get<GameObject>("UIHuntRankingPlayerInfoItem");
            self.HuntRewardsListNode1 = rc.Get<GameObject>("HuntRewardsListNode1");
            self.HuntRewardsListNode2 = rc.Get<GameObject>("HuntRewardsListNode2");
            self.HuntRewardsListNode3 = rc.Get<GameObject>("HuntRewardsListNode3");

            self.HeadImage_No1.SetActive(false);
            self.NameText_No1.SetActive(false);
            self.HuntNumText_No1.SetActive(false);
            self.UIHuntRankingPlayerInfoItem.SetActive(false);
            
            self.EndTime = FunctionHelp.GetCloseTime(1052);

            self.ShowHuntingTime().Coroutine();
            self.ShowHuntRewards();
            self.UpdataRanking().Coroutine();
        }

        /// <summary>
        /// 更新玩家排名信息
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask UpdataRanking(this UIHuntRankingComponent self)
        {
            long instacnid = self.InstanceId;
            C2R_RankShowLieRequest request = new C2R_RankShowLieRequest();
            R2C_RankShowLieResponse response =
                    await self.DomainScene().GetComponent<SessionComponent>().Session.Call(request) as R2C_RankShowLieResponse;
            if (response.RankList == null || response.RankList.Count < 1)
            {
                return;
            }

            if (instacnid != self.InstanceId)
            {
                return;
            }

            // 排序
            response.RankList.Sort((x, y) => (int)(y.KillNumber - x.KillNumber));

            // 第一名
            self.HeadImage_No1.SetActive(true);
            self.NameText_No1.SetActive(true);
            self.HuntNumText_No1.SetActive(true);
            self.NameText_No1.GetComponent<Text>().text = response.RankList[0].PlayerName;
            self.HuntNumText_No1.GetComponent<Text>().text = $"狩猎数量:{response.RankList[0].KillNumber}";
            UICommonHelper.ShowOccIcon(self.HeadImage_No1, response.RankList[0].Occ);

            // 其余
            for (int i = 1; i < response.RankList.Count; i++)
            {
                GameObject go = GameObject.Instantiate(self.UIHuntRankingPlayerInfoItem);
                go.SetActive(true);
                UICommonHelper.SetParent(go, self.HuntRankingListNode);
                self.AddChild<UIHuntRankingPlayerInfoItemComponent, GameObject>(go).OnUpdate(response.RankList[i], i + 1);
            }
        }

        /// <summary>
        /// 显示狩猎剩余时间
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask ShowHuntingTime(this UIHuntRankingComponent self)
        {
            while (!self.IsDisposed)
            {
                DateTime dateTime = TimeInfo.Instance.ToDateTime(TimeHelper.ServerNow());
                long curTime = (dateTime.Hour * 60 + dateTime.Minute ) * 60 + dateTime.Second;
                long endTime = self.EndTime - curTime;
                if (endTime > 0)
                {
                    self.HuntingTimeText.GetComponent<Text>().text = $"{endTime / 60}分{endTime % 60}秒";
                }
                else
                {
                    self.HuntingTimeText.GetComponent<Text>().text = "未到活动时间";
                }

                await TimerComponent.Instance.WaitAsync(1000);
                if (self.IsDisposed)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 展示排名奖励
        /// </summary>
        /// <param name="self"></param>
        public static void  ShowHuntRewards(this UIHuntRankingComponent self)
        {
            List<RankRewardConfig> rankRewardConfigs = RankHelper.GetTypeRankRewards(3);
            for (int i = 0; i < rankRewardConfigs.Count; i++)
            {
                if (rankRewardConfigs[i].NeedPoint[0] == 1 && rankRewardConfigs[i].NeedPoint[1] == 1)
                {
                    UICommonHelper.ShowItemList(rankRewardConfigs[i].RewardItems, self.HuntRewardsListNode1, self, 0.9f);
                }
                else if (rankRewardConfigs[i].NeedPoint[0] == 2 && rankRewardConfigs[i].NeedPoint[1] == 3)
                {
                    UICommonHelper.ShowItemList(rankRewardConfigs[i].RewardItems, self.HuntRewardsListNode2, self, 0.9f);
                }
                else if (rankRewardConfigs[i].NeedPoint[0] == 4 && rankRewardConfigs[i].NeedPoint[1] == 10)
                {
                    UICommonHelper.ShowItemList(rankRewardConfigs[i].RewardItems, self.HuntRewardsListNode3, self, 0.9f);
                }
            }
        }
    }
}