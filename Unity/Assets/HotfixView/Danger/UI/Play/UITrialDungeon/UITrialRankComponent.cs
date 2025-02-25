using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UITrialRankComponent : Entity, IAwake
    {
        public GameObject BtnItemTypeSet;
        public GameObject Button_Reward;
        public GameObject Text_MyRank;
        public GameObject RankListNode;
        public GameObject UISet;
        public GameObject CloseButton;
        public GameObject UITrialRankItem;
        public GameObject Text_RewardTime;

        public int Page;
    }

    public class UITrialRankComponentAwake : AwakeSystem<UITrialRankComponent>
    {
        public override void Awake(UITrialRankComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.RankListNode = rc.Get<GameObject>("RankListNode");
            self.Text_MyRank = rc.Get<GameObject>("Text_MyRank");
            self.UISet = rc.Get<GameObject>("UISet");
            self.UITrialRankItem = rc.Get<GameObject>("UITrialRankItem");
            self.UITrialRankItem.SetActive(false);

            self.Text_RewardTime = rc.Get<GameObject>("Text_RewardTime");

            self.Button_Reward = rc.Get<GameObject>("Button_Reward");
            ButtonHelp.AddListenerEx(self.Button_Reward, () => { self.Button_Reward().Coroutine(); });

            self.BtnItemTypeSet = rc.Get<GameObject>("BtnItemTypeSet");
            for (int i = 0; i < self.BtnItemTypeSet.transform.childCount; i++)
            {
                Transform transform = self.BtnItemTypeSet.transform.GetChild(i);
                int i1 = i;
                transform.Find("Button").GetComponent<Button>().onClick.AddListener(() => { self.OnClickPageButton(i1 + 1); });
            }

            UICommonHelper.ShowOccIcon(rc.Get<GameObject>("HeadIcomImage1"), 1);
            UICommonHelper.ShowOccIcon(rc.Get<GameObject>("HeadIcomImage2"), 2);
            UICommonHelper.ShowOccIcon(rc.Get<GameObject>("HeadIcomImage3"), 3);
            
            self.ShowRewardTime().Coroutine();
            self.OnUpdateUI().Coroutine();
        }
    }

    public static class UITrialRankComponentSystem
    {
        
        public static void OnClickPageButton(this UITrialRankComponent self, int page)
        {
            if (self.Page == page)
            {
                self.Page = 0;
                for (int i = 0; i < self.BtnItemTypeSet.transform.childCount; i++)
                {
                    Transform transform = self.BtnItemTypeSet.transform.GetChild(i);
                    transform.Find("XuanZhong").gameObject.SetActive(false);
                    transform.Find("WeiXuanZhong").gameObject.SetActive(true);
                }
            }
            else
            {
                self.Page = page;
                for (int i = 0; i < self.BtnItemTypeSet.transform.childCount; i++)
                {
                    Transform transform = self.BtnItemTypeSet.transform.GetChild(i);
                    transform.Find("XuanZhong").gameObject.SetActive(page - 1 == i);
                    transform.Find("WeiXuanZhong").gameObject.SetActive(page - 1 != i);
                }
            }

            self.OnUpdateUI(self.Page).Coroutine();
        }

        public static async ETTask ShowRewardTime(this UITrialRankComponent self)
        {
            long instanceid = self.InstanceId;
            self.OnTimer();

            while (true)
            {
                await TimerComponent.Instance.WaitAsync(1000);
                if (instanceid != self.InstanceId)
                {
                    break;
                }
                self.OnTimer();
            }
        }

        public static void OnTimer(this UITrialRankComponent self)
        {
            long serverTime = TimeHelper.ServerNow();
            DateTime dateTime = TimeInfo.Instance.ToDateTime(serverTime);

            int today = (int)dateTime.DayOfWeek == 0 ? 7 : (int)dateTime.DayOfWeek;
            long opentime = 7 * TimeHelper.OneDay;
            long curTime = today * TimeHelper.OneDay + dateTime.Hour * TimeHelper.Hour + dateTime.Minute * TimeHelper.Minute + dateTime.Second * TimeHelper.Second;

            long leftTime = opentime - curTime;
            if (leftTime < 0)
            {
                self.Text_RewardTime.GetComponent<Text>().text = string.Empty;
            }
            else
            {
                self.Text_RewardTime.GetComponent<Text>().text = $"奖励发放时间: {TimeHelper.ShowLeftTime(leftTime)}"; 
            }
        }

        public static async ETTask Button_Reward(this UITrialRankComponent self)
        {
            self.UISet.SetActive(false);
            UI uiHelp = await UIHelper.Create(self.ZoneScene(), UIType.UITrialReward);
            if (!self.IsDisposed && uiHelp != null)
            {
                uiHelp.GetComponent<UITrialRewardComponent>().OnInitUI(6);
                uiHelp.GetComponent<UITrialRewardComponent>().ClickOnClose = self.OpenShow;
            }
        }

        public static void OpenShow(this UITrialRankComponent self)
        {
            self.UISet.SetActive(true);
        }

        public static async ETTask OnUpdateUI(this UITrialRankComponent self,int type = 0)
        {
            long instanceid = self.InstanceId;
            C2R_RankTrialListRequest c2M_RankListRequest = new C2R_RankTrialListRequest();
            R2C_RankTrialListResponse r2C_Response = (R2C_RankTrialListResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_RankListRequest);
            if (instanceid != self.InstanceId)
            {
                return;
            }

            long selfId = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.UserId;
            int myRank = -1;
            int rank = 1;
            UICommonHelper.DestoryChild(self.RankListNode);
            for (int i = 0; i < r2C_Response.RankList.Count; i++)
            {
                if (i % 5 == 0)
                {
                    await TimerComponent.Instance.WaitAsync(1);
                }
                if (instanceid != self.InstanceId)
                {
                    return;
                }
                if (type != 0 && r2C_Response.RankList[i].Occ != type)
                {
                    continue;
                }
                GameObject skillItem = GameObject.Instantiate(self.UITrialRankItem);
                skillItem.SetActive(true);
                UICommonHelper.SetParent(skillItem, self.RankListNode);
                UITrialRankShowItemComponent uIItemComponent = self.AddChild<UITrialRankShowItemComponent, GameObject>(skillItem);
                uIItemComponent.OnInitData(rank, r2C_Response.RankList[i]);

                if (selfId == r2C_Response.RankList[i].UserId)
                {
                    myRank = rank;
                }
                rank++;
            }

            if (myRank == -1)
            {
                self.Text_MyRank.GetComponent<Text>().text = "我的排名: 未上榜";
            }
            else
            {
                self.Text_MyRank.GetComponent<Text>().text = $"我的排名: {myRank}";
            }
        }

    }
}