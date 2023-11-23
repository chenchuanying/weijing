﻿using System;
using System.Collections.Generic;

namespace ET
{

    [ObjectSystem]
    public class UserInfoComponentAwakeSystem : AwakeSystem<UserInfoComponent>
    {
        public override void Awake(UserInfoComponent self)
        {

        }
    }

    public static class UserInfoComponentSystem
    {

        public static bool IsHaveFristWinReward(this UserInfoComponent self, int firstwinid, int difficulty)
        {
            for (int i = 0; i < self.UserInfo.FirstWinSelf.Count; i++)
            {
                KeyValuePair keyValuePair = self.UserInfo.FirstWinSelf[i];
                if (keyValuePair.KeyId != firstwinid)
                {
                    continue;
                }

                return keyValuePair.Value.Contains(difficulty.ToString()) && !keyValuePair.Value2.Contains(difficulty.ToString());
            }
            return false;
        }

        public static bool IsReceivedFristWinReward(this UserInfoComponent self, int firstwinid, int difficulty)
        {
            for (int i = 0; i < self.UserInfo.FirstWinSelf.Count; i++)
            {
                if (self.UserInfo.FirstWinSelf[i].KeyId != firstwinid)
                {
                    continue;
                }
                return self.UserInfo.FirstWinSelf[i].Value2.Contains(difficulty.ToString());
            }
            return false;
        }

        public static long GetMakeTime(this UserInfoComponent self, int makeId)
        {
            List<KeyValuePairInt> makeList = self.UserInfo.MakeIdList;
            for (int i = 0; i < makeList.Count; i++)
            {
                if (makeList[i].KeyId == makeId)
                {
                    return makeList[i].Value;
                }
            }
            return 0;
        }

        public static void OnMakeItem(this UserInfoComponent self, int makeId)
        {
            EquipMakeConfig equipMakeConfig = EquipMakeConfigCategory.Instance.Get(makeId);
            List<KeyValuePairInt> makeList = self.UserInfo.MakeIdList;

            bool have = false;
            long endTime = TimeHelper.ServerNow() + equipMakeConfig.MakeTime * 1000; 
            for (int i = 0; i < makeList.Count; i++)
            {
                if (makeList[i].KeyId == makeId)
                {
                    makeList[i].Value = endTime;
                    have = true;
                }
            }
            if (!have)
            {
                self.UserInfo.MakeIdList.Add(new KeyValuePairInt() { KeyId = makeId, Value = endTime });
            }
        }
        public static string GetGameSettingValue(this UserInfoComponent self, GameSettingEnum gameSettingEnum)
        {
            for (int i = 0; i < self.UserInfo.GameSettingInfos.Count; i++)
            {
                if (self.UserInfo.GameSettingInfos[i].KeyId == (int)gameSettingEnum)
                    return self.UserInfo.GameSettingInfos[i].Value;
            }
            switch (gameSettingEnum)
            {
                case GameSettingEnum.Music:
                    return "1";
                case GameSettingEnum.Sound:
                    return "0";
                case GameSettingEnum.YanGan:  //0 固定 1移动
                    return "0";
                case GameSettingEnum.FenBianlLv:
                    return "1";
                case GameSettingEnum.OneSellSet:
                    return "1@0@0";
                case GameSettingEnum.OneSellSet2:
                    return "0@0@0@0@0";
                case GameSettingEnum.HighFps:
                    return "1";
                case GameSettingEnum.AutoAttack:
                    return "1";
                default:
                    return "0";
            }
        }

        public static void UpdateGameSetting(this UserInfoComponent self, List<KeyValuePair> gameSettingInfos)
        {
            for (int i = 0; i < gameSettingInfos.Count; i++)
            {
                bool exist = false;
                for (int k = 0; k < self.UserInfo.GameSettingInfos.Count; k++)
                {
                    if (self.UserInfo.GameSettingInfos[k].KeyId == gameSettingInfos[i].KeyId)
                    {
                        exist = true;
                        self.UserInfo.GameSettingInfos[k].Value = gameSettingInfos[i].Value;
                        break;
                    }
                }
                if (!exist)
                {
                    self.UserInfo.GameSettingInfos.Add(gameSettingInfos[i]);
                }
            }
        }

        public static FubenPassInfo GetPassInfoByID(this UserInfoComponent self, int levelid)
        {
            for (int i = 0; i < self.UserInfo.FubenPassList.Count; i++)
            {
                if (self.UserInfo.FubenPassList[i].FubenId == levelid)
                {
                    return self.UserInfo.FubenPassList[i];
                }
            }
            return null;
        }

        public static void OnFubenSettlement(this UserInfoComponent self, int levelid, int difficulty)
        {
            FubenPassInfo fubenPassInfo = null;
            for (int i = 0; i < self.UserInfo.FubenPassList.Count; i++)
            {
                if (self.UserInfo.FubenPassList[i].FubenId == levelid)
                {
                    fubenPassInfo = self.UserInfo.FubenPassList[i];
                }
            }
            if (fubenPassInfo == null)
            {
                fubenPassInfo = new FubenPassInfo();
                fubenPassInfo.FubenId = levelid;
                self.UserInfo.FubenPassList.Add(fubenPassInfo);
            }
            fubenPassInfo.Difficulty = (difficulty > fubenPassInfo.Difficulty) ? difficulty : fubenPassInfo.Difficulty;
        }

        public static bool IsLevelPassed(this UserInfoComponent self, int levelid)
        {
            for (int i = 0; i < self.UserInfo.FubenPassList.Count; i++)
            {
                if (self.UserInfo.FubenPassList[i].FubenId == levelid)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsChapterOpen(this UserInfoComponent self, int chapterid)
        {
            if (chapterid == 1)
            {
                return true;
            }
            if (!ChapterSectionConfigCategory.Instance.Contain(chapterid))
            {
                return false;
            }

            ChapterSectionConfig chapterSectionConfig = ChapterSectionConfigCategory.Instance.Get(chapterid - 1);
            int[] RandomArea = chapterSectionConfig.RandomArea;

            for (int i = 0; i < RandomArea.Length; i++)
            {
                if (!self.IsLevelPassed(RandomArea[i]))
                {
                    return false;
                }
            }
            return true;
        }

        public static void OnHorseActive(this UserInfoComponent self, int horseId, bool active)
        {
            if (active && !self.UserInfo.HorseIds.Contains(horseId))
            {
                self.UserInfo.HorseIds.Add(horseId);
            }
            if(!active && self.UserInfo.HorseIds.Contains(horseId))
            {
                self.UserInfo.HorseIds.Remove(horseId);
            }
        }

        public static void ClearDayData(this UserInfoComponent self)
        {
            self.UserInfo.DayFubenTimes.Clear();
            self.UserInfo.ChouKaRewardIds.Clear();
            self.UserInfo.MysteryItems.Clear();
            self.UserInfo.DayItemUse.Clear();
            self.UserInfo.DayMonsters.Clear();
            self.UserInfo.DayJingLing.Clear();
        }

        /// <summary>
        /// 角色创建天数  从1 开始
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int GetCrateDay(this UserInfoComponent self)
        {
            return ServerHelper.DateDiff_Time(TimeHelper.ServerNow(), self.UserInfo.CreateTime);
        }

    }

}
