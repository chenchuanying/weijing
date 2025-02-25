﻿using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIActivitySingleRechargeItemComponent: Entity, IAwake<GameObject>
    {
        public GameObject GameObject;
        public GameObject ConsumeNumText;
        public GameObject RewardListNode;
        public GameObject ReceiveBtn;
        public GameObject ReceivedImg;
        public GameObject Reddot;
        public int Key;
    }

    public class UIActivitySingleRechargeItemComponentAwakeSystem: AwakeSystem<UIActivitySingleRechargeItemComponent, GameObject>
    {
        public override void Awake(UIActivitySingleRechargeItemComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.ConsumeNumText = rc.Get<GameObject>("ConsumeNumText");
            self.RewardListNode = rc.Get<GameObject>("RewardListNode");
            self.ReceiveBtn = rc.Get<GameObject>("ReceiveBtn");
            self.ReceivedImg = rc.Get<GameObject>("ReceivedImg");
            self.Reddot = rc.Get<GameObject>("Reddot");

            self.ReceivedImg.SetActive(false);
            self.ReceiveBtn.GetComponent<Button>().onClick.AddListener(() => { self.OnReceiveBtn().Coroutine(); });
        }
    }

    public static class UIActivitySingleRechargeItemComponentSystem
    {
        public static void OnUpdateData(this UIActivitySingleRechargeItemComponent self, int key)
        {
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();

            self.Key = key;
            self.ConsumeNumText.GetComponent<Text>().text = $"单笔充值{key}元";
            self.Reddot.SetActive( userInfoComponent.UserInfo.SingleRechargeIds.Contains(key) && !userInfoComponent.UserInfo.SingleRewardIds.Contains(key));
            self.ReceivedImg.SetActive(userInfoComponent.UserInfo.SingleRewardIds.Contains(key));
            self.ReceiveBtn.SetActive(!userInfoComponent.UserInfo.SingleRewardIds.Contains(key));
            UICommonHelper.DestoryChild(self.RewardListNode);
            UICommonHelper.ShowItemList(ConfigHelper.SingleRechargeReward[key], self.RewardListNode, self, 1f);

            // UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
            // if (userInfo.SingleRechargeRewardIds.Contains(self.Key))
            // {
            //     self.ReceiveBtn.SetActive(true);
            // }
        }

        public static async ETTask OnReceiveBtn(this UIActivitySingleRechargeItemComponent self)
        {
            if (!ConfigHelper.SingleRechargeReward.ContainsKey(self.Key))
            {
                return;
            }

            string[] rewarditemlist = ConfigHelper.SingleRechargeReward[self.Key].Split('@');
            if (self.ZoneScene().GetComponent<BagComponent>().GetBagLeftCell() < rewarditemlist.Length)
            {
                FloatTipManager.Instance.ShowFloatTip("背包空间不足");
                return;
            }

            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
            if (!userInfo.SingleRechargeIds.Contains(self.Key))
            {
                FloatTipManager.Instance.ShowFloatTip("未达条件");
                return;
            }
            if (userInfo.SingleRewardIds.Contains(self.Key))
            {
                FloatTipManager.Instance.ShowFloatTip("已经领取");
                return;
            }

            C2M_SingleRechargeRewardRequest request = new C2M_SingleRechargeRewardRequest() { RewardId = self.Key };
            M2C_SingleRechargeRewardResponse response =
                    (M2C_SingleRechargeRewardResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);

            if (response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            userInfo.SingleRewardIds = response.RewardIds;
            ReddotComponent redPointComponent = self.ZoneScene().GetComponent<ReddotComponent>();
            redPointComponent.UpdateReddont(ReddotType.SingleRecharge);

            self.Reddot.SetActive(false);
            self.ReceiveBtn.SetActive(false);
            self.ReceivedImg.SetActive(true);
        }
    }
}