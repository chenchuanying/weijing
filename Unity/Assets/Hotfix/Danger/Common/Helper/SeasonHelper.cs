
using System.Collections.Generic;

namespace ET
{
    public static class SeasonHelper
    {

        public static bool IsOpenSeason(int userLv)
        {
            if (userLv < 55)
            {
                return false;
            }
            long serverTime = TimeHelper.ServerNow();
            return SeasonOpenTime >0 && serverTime >= SeasonOpenTime && serverTime <= SeasonCloseTime;
        }

        public static int GetFubenId(int lv)
        {
            List<int> canEnterIds = new List<int>();
            Dictionary<int, DungeonConfig> keyValuePairs = DungeonConfigCategory.Instance.GetAll();
            foreach (var item in keyValuePairs)
            {
                if (item.Key >= 100000)
                {
                    continue;
                }
                if (DungeonSectionConfigCategory.Instance.MysteryDungeonList.Contains(item.Key))
                {
                    continue;
                }
                if (item.Value.EnterLv <= lv && item.Key < ConfigHelper.GMDungeonId)
                {
                    canEnterIds.Add(item.Key);
                }
            }

            return canEnterIds[RandomHelper.RandomNumber(0, canEnterIds.Count)];
        }


        /// <summary>
        /// 赛季结束奖励
        /// </summary>
        public static string SeasonOverReward(int seasonLevel)
        {
            if (seasonLevel >= 25)
            {
                return "10010040;1";
            }
            if (seasonLevel >= 10)
            {
                return "10000135;1";
            }

            return "";
        }


        /// <summary>
        /// 赛季开始时间
        /// </summary>
        public static long SeasonOpenTime = 1701360000000;

        //赛季结束时间
        public static long SeasonCloseTime = 1713715200000;

        /// <summary>
        /// 赛季BOSS
        /// </summary>
        public static int SeasonBossId = 90000051;
    }
}

