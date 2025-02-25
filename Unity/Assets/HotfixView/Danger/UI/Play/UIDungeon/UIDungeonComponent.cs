﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIDungeonComponent: Entity, IAwake, IDestroy
    {
        public Dictionary<int, Text> BossRefreshObjs = new Dictionary<int, Text>();
        public Dictionary<int, long> BossRefreshTime = new Dictionary<int, long>();
        public long Timer;
        public GameObject BossRefreshTimeList;
        public GameObject UIBossRefreshTimeItem;
        public GameObject BossRefreshSettingBtn;
        public GameObject BossRefreshSettingPanel;
        public GameObject BossRefreshSettingList;
        public GameObject UIBossRefreshSettingItem;
        public GameObject CloseBossRefreshSettingBtn;
        public GameObject ScrollView;
        public GameObject ChapterList;
        public GameObject ButtonClose;
        public List<string> AssetPath = new List<string>();
    }

    [Timer(TimerType.UIDungenBossRefreshTimer)]
    public class UIDungenBossRefreshTimer: ATimer<UIDungeonComponent>
    {
        public override void Run(UIDungeonComponent self)
        {
            self.UpdateBossRefreshTimer();
        }
    }

    public class UIDungeonComponentAwakeSystem: AwakeSystem<UIDungeonComponent>
    {
        public override void Awake(UIDungeonComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.BossRefreshTimeList = rc.Get<GameObject>("BossRefreshTimeList");
            self.UIBossRefreshTimeItem = rc.Get<GameObject>("UIBossRefreshTimeItem");
            self.BossRefreshSettingBtn = rc.Get<GameObject>("BossRefreshSettingBtn");
            self.BossRefreshSettingPanel = rc.Get<GameObject>("BossRefreshSettingPanel");
            self.BossRefreshSettingList = rc.Get<GameObject>("BossRefreshSettingList");
            self.UIBossRefreshSettingItem = rc.Get<GameObject>("UIBossRefreshSettingItem");
            self.CloseBossRefreshSettingBtn = rc.Get<GameObject>("CloseBossRefreshSettingBtn");
            self.ButtonClose = rc.Get<GameObject>("ButtonClose");
            self.ChapterList = rc.Get<GameObject>("ChapterList");
            self.ScrollView = rc.Get<GameObject>("ScrollView");

            self.ButtonClose.GetComponent<Button>().onClick.AddListener(() => { self.OnCloseChapter(); });
            self.BossRefreshSettingBtn.GetComponent<Button>().onClick.AddListener(self.OnBossRefreshSetting);
            self.CloseBossRefreshSettingBtn.GetComponent<Button>().onClick.AddListener(self.OnCloseBossRefreshSetting);

            self.UIBossRefreshTimeItem.SetActive(false);
            self.UIBossRefreshSettingItem.SetActive(false);
            self.BossRefreshSettingPanel.SetActive(false);
        }
    }

    public class UIDungeonComponentDestoySystem: DestroySystem<UIDungeonComponent>
    {
        public override void Destroy(UIDungeonComponent self)
        {
            for(int i = 0; i < self.AssetPath.Count; i++)
            {
                if (!string.IsNullOrEmpty(self.AssetPath[i]))
                {
                    ResourcesComponent.Instance.UnLoadAsset(self.AssetPath[i]); 
                }
            }
            self.AssetPath = null;

            TimerComponent.Instance?.Remove(ref self.Timer);
        }
    }

    public static class UIDungeonComponentSystem
    {
        public static async ETTask UpdateChapterList(this UIDungeonComponent self)
        {
            long instanceid = self.InstanceId;
            var path = ABPathHelper.GetUGUIPath("Dungeon/UIDungeonItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            self.AssetPath.Add(path);
            if (instanceid != self.InstanceId)
            {
                return;
            }

            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            List<DungeonSectionConfig> dungeonConfigs = DungeonSectionConfigCategory.Instance.GetAll().Values.ToList();
            for (int i = 0; i < dungeonConfigs.Count; i++)
            {
                int chapterid = dungeonConfigs[i].Id;
                GameObject go = GameObject.Instantiate(bundleGameObject);

                UICommonHelper.SetParent(go, self.ChapterList);
                UIDungeonItemComponent uIChapterItemComponent = self.AddChild<UIDungeonItemComponent, GameObject>(go);
                uIChapterItemComponent.OnInitData(i, chapterid).Coroutine();
            }

            if (userInfoComponent.UserInfo.Lv > 50)
            {
                await TimerComponent.Instance.WaitAsync(10);
                self.ScrollView.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
            }

            await TimerComponent.Instance.WaitAsync(10);
            self.ZoneScene().GetComponent<GuideComponent>().OnTrigger(GuideTriggerType.OpenUI, UIType.UIDungeon);
        }

        /// <summary>
        /// Boss刷新倒计时显示
        /// </summary>
        /// <param name="self"></param>
        public static async ETTask UpdateBossRefreshTimeList(this UIDungeonComponent self)
        {
            // 重新从服务器同步Booss刷新数据
            long instance = self.InstanceId;
            await NetHelper.RequestUserInfo(self.ZoneScene());
            if (instance != self.InstanceId)
            {
                return;
            }

            // 获取所有Boss的刷新数据
            Unit myUnit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            var bossRevivesTime = myUnit.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.MonsterRevives;

            // 根据时间排序,从小到大
            bossRevivesTime.Sort((s1, s2) => long.Parse(s1.Value).CompareTo(long.Parse(s2.Value)));

            // 为Boss刷新列表添加子物体
            self.BossRefreshObjs.Clear();
            self.BossRefreshTime.Clear();
            for (int i = 0; i < bossRevivesTime.Count; i++)
            {
                var bossConfId = bossRevivesTime[i].KeyId;

                // 判断该boss是否需要显示,暂时从本地获取设置
                if (PlayerPrefs.HasKey(bossConfId.ToString()))
                {
                    if (PlayerPrefs.GetString(bossConfId.ToString()) == "0")
                    {
                        continue;
                    }
                }
                else
                {
                    PlayerPrefs.SetString(bossConfId.ToString(), "1");
                }

                MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(bossConfId);

                GameObject go = GameObject.Instantiate(self.UIBossRefreshTimeItem);
                go.SetActive(true);
                ReferenceCollector boosTimeItemRc = go.gameObject.GetComponent<ReferenceCollector>();

                // Boss头像
                string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.MonsterIcon, monsterConfig.MonsterHeadIcon);
                Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
                if (!self.AssetPath.Contains(path))
                {
                    self.AssetPath.Add(path);
                }
                boosTimeItemRc.Get<GameObject>("Photo").GetComponent<Image>().sprite = sp;

                // Boss名字
                boosTimeItemRc.Get<GameObject>("Name").GetComponent<Text>().text = monsterConfig.MonsterName;


                int dungeonid = SceneConfigHelper.GetFubenByMonster(monsterConfig.Id);
                if (dungeonid > 0)
                {
                    // Boss出生地
                    boosTimeItemRc.Get<GameObject>("Map").GetComponent<Text>().text =
                            $"({DungeonConfigCategory.Instance.Get(dungeonid).ChapterName})";

                }
                if (self.BossRefreshTime.ContainsKey(bossRevivesTime[i].KeyId))
                {
                    continue;
                }

                // Boss刷新时间
                self.BossRefreshTime.Add(bossRevivesTime[i].KeyId, long.Parse(bossRevivesTime[i].Value));
                self.BossRefreshObjs.Add(bossRevivesTime[i].KeyId, go.GetComponent<ReferenceCollector>().Get<GameObject>("Time").GetComponent<Text>());
                // 先提前刷新一下
                self.UpdateBossRefreshTimer();

                UICommonHelper.SetParent(go, self.BossRefreshTimeList);
            }

            // 开启定时器
            if (self.Timer != 0)
            {
                TimerComponent.Instance.Remove(ref self.Timer);
            }

            self.Timer = TimerComponent.Instance.NewRepeatedTimer(1000, TimerType.UIDungenBossRefreshTimer, self);
        }

        /// <summary>
        /// UI刷新Bosss复活时间
        /// </summary>
        /// <param name="self"></param>
        public static void UpdateBossRefreshTimer(this UIDungeonComponent self)
        {
            foreach (KeyValuePair<int, long> it in self.BossRefreshTime)
            {
                long time = it.Value;
                if (time > TimeHelper.ServerNow())
                {
                    time -= TimeHelper.ClientNow();
                    time /= 1000;
                    int hour = (int)time / 3600;
                    int min = (int)((time - (hour * 3600)) / 60);
                    int sec = (int)(time - (hour * 3600) - (min * 60));
                    string showStr = hour + "时" + min + "分" + sec + "秒";
                    self.BossRefreshObjs[it.Key].text = $"刷新时间:{showStr}";
                }
                else
                {
                    self.BossRefreshObjs[it.Key].text = "已刷新";
                }
            }
        }

        public static void OnCloseChapter(this UIDungeonComponent self)
        {
            UIHelper.Remove(self.DomainScene(), UIType.UIDungeon);
        }

        /// <summary>
        /// 打开Boss刷新设置界面
        /// </summary>
        /// <param name="self"></param>
        public static void OnBossRefreshSetting(this UIDungeonComponent self)
        {
            self.BossRefreshSettingPanel.SetActive(true);
            // 为Boss设置列表添加子物体
            foreach (int bossConfigId in ConfigHelper.BossShowTimeList)
            {
                MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(bossConfigId);

                GameObject go = GameObject.Instantiate(self.UIBossRefreshSettingItem);
                go.SetActive(true);
                // Boss名字
                ReferenceCollector rc = go.GetComponent<ReferenceCollector>();
                go.Get<GameObject>("NameText").GetComponent<Text>().text = monsterConfig.MonsterName;

                // 按钮
                if (!PlayerPrefs.HasKey(bossConfigId.ToString()))
                {
                    PlayerPrefs.SetString(bossConfigId.ToString(), "1");
                }

                go.Get<GameObject>("ShowText").SetActive(PlayerPrefs.GetString(bossConfigId.ToString()) == "1");

                go.Get<GameObject>("ToggleBtn").GetComponent<Button>().onClick.AddListener(() =>
                {
                    self.OnSettingChanged(bossConfigId.ToString(), go.Get<GameObject>("ShowText"));
                });

                UICommonHelper.SetParent(go, self.BossRefreshSettingList);
            }
        }

        /// <summary>
        /// 点击Boss显示按钮
        /// </summary>
        /// <param name="self"></param>
        /// <param name="key"></param>
        /// <param name="obj"></param>
        public static void OnSettingChanged(this UIDungeonComponent self, string key, GameObject obj)
        {
            if (PlayerPrefs.GetString(key) == "0")
            {
                PlayerPrefs.SetString(key, "1");
                obj.SetActive(true);
            }
            else
            {
                PlayerPrefs.SetString(key, "0");
                obj.SetActive(false);
            }
        }

        /// <summary>
        /// 关闭Boss刷新列表界面
        /// </summary>
        /// <param name="self"></param>
        public static void OnCloseBossRefreshSetting(this UIDungeonComponent self)
        {
            TimerComponent.Instance.Remove(ref self.Timer);

            int childCount = self.BossRefreshTimeList.transform.childCount;
            for (int i = 1; i < childCount; i++)
            {
                GameObject.Destroy(self.BossRefreshTimeList.transform.GetChild(i).gameObject);
            }

            self.UpdateBossRefreshTimeList().Coroutine();
            self.BossRefreshSettingPanel.SetActive(false);
        }
    }
}