using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UISeasonHomeComponent: Entity, IAwake, IDestroy
    {
        public GameObject SeasonText;
        public GameObject SeasonTimeText;
        public GameObject SeasonExperienceText;
        public GameObject SeasonExperienceImg;
        public GameObject SeasonLvText;
        public GameObject MonsterHeadImg;
        public GameObject MonsterNameText;
        public GameObject MonsterPositionText;
        public Text MonsterRefreshTimeText;
        public GameObject ShowBtn;
        public GameObject SeasonRewardText;
        public GameObject RewardsListNode;
        public GameObject GetBtn;
        public GameObject AcvityedImg;

        public List<string> AssetPath = new List<string>();
    }

    public class UISeasonHomeComponentAwakeSystem: AwakeSystem<UISeasonHomeComponent>
    {
        public override void Awake(UISeasonHomeComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.SeasonText = rc.Get<GameObject>("SeasonText");
            self.SeasonTimeText = rc.Get<GameObject>("SeasonTimeText");
            self.SeasonExperienceText = rc.Get<GameObject>("SeasonExperienceText");
            self.SeasonExperienceImg = rc.Get<GameObject>("SeasonExperienceImg");
            self.SeasonLvText = rc.Get<GameObject>("SeasonLvText");
            self.MonsterHeadImg = rc.Get<GameObject>("MonsterHeadImg");
            self.MonsterNameText = rc.Get<GameObject>("MonsterNameText");
            self.MonsterPositionText = rc.Get<GameObject>("MonsterPositionText");
            self.MonsterRefreshTimeText = rc.Get<GameObject>("MonsterRefreshTimeText").GetComponent<Text>();
            self.ShowBtn = rc.Get<GameObject>("ShowBtn");
            self.SeasonRewardText = rc.Get<GameObject>("SeasonRewardText");
            self.RewardsListNode = rc.Get<GameObject>("RewardsListNode");
            self.GetBtn = rc.Get<GameObject>("GetBtn");
            self.AcvityedImg = rc.Get<GameObject>("AcvityedImg");

            self.GetBtn.GetComponent<Button>().onClick.AddListener(() => { self.OnGetBtn().Coroutine(); });
            self.ShowBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                UIHelper.Create(self.DomainScene(), UIType.UISeasonLordDetail).Coroutine();
            });

            self.UpdateInfo();
            self.UpdateTime().Coroutine();
        }
    }

    public class UISeasonHomeComponentDestroy: DestroySystem<UISeasonHomeComponent>
    {
        public override void Destroy(UISeasonHomeComponent self)
        {
            for (int i = 0; i < self.AssetPath.Count; i++)
            {
                if (!string.IsNullOrEmpty(self.AssetPath[i]))
                {
                    ResourcesComponent.Instance.UnLoadAsset(self.AssetPath[i]);
                }
            }

            self.AssetPath = null;
        }
    }

    public static class UISeasonHomeComponentSystem
    {
        public static void UpdateInfo(this UISeasonHomeComponent self)
        {
            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
            KeyValuePairLong seasonOpenTime = SeasonHelper.GetOpenSeason(userInfo.Lv);
            if(seasonOpenTime == null)
            {
                return;
            }

            DateTime startTime = TimeInfo.Instance.ToDateTime(seasonOpenTime.Value);
            DateTime endTime = TimeInfo.Instance.ToDateTime(seasonOpenTime.Value2);

            self.SeasonTimeText.GetComponent<Text>().text =
                    $"赛季时间:{startTime.Year}.{startTime.Month}.{startTime.Day}-{endTime.Year}.{endTime.Month}.{endTime.Day}";


            if (seasonOpenTime.KeyId == 3)
            {
                self.SeasonText.GetComponent<Text>().text = "2024第四赛季";
            }
            if (seasonOpenTime.KeyId == 4)
            {
                self.SeasonText.GetComponent<Text>().text = "2024第五赛季";
            }

            int seasonExp = userInfo.SeasonExp;
            SeasonLevelConfig seasonLevelConfig = SeasonLevelConfigCategory.Instance.Get(userInfo.SeasonLevel);

            if (seasonExp > seasonLevelConfig.UpExp)
            {
                seasonExp = seasonLevelConfig.UpExp;
            }

            self.SeasonExperienceText.GetComponent<Text>().text = $"赛季经验:{seasonExp}/{seasonLevelConfig.UpExp}";
            self.SeasonExperienceImg.GetComponent<Image>().fillAmount = 1f * seasonExp / seasonLevelConfig.UpExp;

            self.SeasonLvText.GetComponent<Text>().text = userInfo.SeasonLevel.ToString();

            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

            int bossId = SeasonHelper.SeasonBossId;
            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(bossId);
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.MonsterIcon, monsterConfig.MonsterHeadIcon);
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }

            self.MonsterHeadImg.GetComponent<Image>().sprite = sp;

            int fubenid = numericComponent.GetAsInt(NumericType.SeasonBossFuben);
            if (fubenid == 0)
            {
                self.MonsterPositionText.GetComponent<Text>().text = "未刷新";
            }
            else
            {
                DungeonConfig dungeonConfig = DungeonConfigCategory.Instance.Get(fubenid);
                self.MonsterPositionText.GetComponent<Text>().text = $"出现位置:{dungeonConfig.ChapterName}";
            }
           

            self.UpdateSeasonReward();
        }

        public static void UpdateSeasonReward(this UISeasonHomeComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;

            int oldReward = numericComponent.GetAsInt(NumericType.SeasonReward);
            int nowReward = oldReward + 1;
            if (nowReward > SeasonLevelConfigCategory.Instance.GetAll().Count)
            {
                nowReward -= 1;
                self.AcvityedImg.SetActive(true);
                self.GetBtn.SetActive(false);
            }
            else
            {
                self.AcvityedImg.SetActive(false);
                self.GetBtn.SetActive(true);
            }

            self.SeasonRewardText.GetComponent<Text>().text = $"{nowReward}级赛季奖励";
            UICommonHelper.DestoryChild(self.RewardsListNode);
            UICommonHelper.ShowItemList(SeasonLevelConfigCategory.Instance.Get(nowReward).Reward, self.RewardsListNode, self, 0.9f);
        }

        public static async ETTask UpdateTime(this UISeasonHomeComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

            while (!self.IsDisposed)
            {
                long now = TimeHelper.ServerNow();
                long end = numericComponent.GetAsLong(NumericType.SeasonBossRefreshTime);
                if (end - now > 0)
                {
                    DateTime nowTime = TimeInfo.Instance.ToDateTime(now);
                    DateTime endTime = TimeInfo.Instance.ToDateTime(end);
                    TimeSpan ts = endTime - nowTime;
                    self.MonsterRefreshTimeText.text = $"刷新时间:{ts.Days}天{ts.Hours}小时{ts.Minutes}分";
                }
                else
                {
                    self.MonsterRefreshTimeText.text = "赛季领主已刷新!!";
                }

                await TimerComponent.Instance.WaitAsync(1000);
                if (self.IsDisposed)
                {
                    break;
                }
            }
        }

        public static async ETTask OnGetBtn(this UISeasonHomeComponent self)
        {
            if (self.ZoneScene().GetComponent<BagComponent>().GetBagLeftCell() < 5)
            {
                FloatTipManager.Instance.ShowFloatTip("背包空间不足！");
                return;
            }

            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;

            int oldReward = numericComponent.GetAsInt(NumericType.SeasonReward);
            int nowReward = oldReward + 1;
            if (nowReward <= SeasonLevelConfigCategory.Instance.GetAll().Count)
            {
                if (nowReward > userInfo.SeasonLevel)
                {
                    FloatTipManager.Instance.ShowFloatTip("未到领取等级！");
                    return;
                }

                C2M_SeasonLevelRewardRequest request = new C2M_SeasonLevelRewardRequest() { SeasonLevel = nowReward };
                M2C_SeasonLevelRewardResponse response =
                        (M2C_SeasonLevelRewardResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
                self.UpdateSeasonReward();
            }
        }
    }
}