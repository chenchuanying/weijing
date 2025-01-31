
using System.Collections.Generic;

namespace ET
{
    public static class SeasonHelper
    {

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
        public static string GetSeasonOverReward(int seasonLevel)
        {
            if (seasonLevel >= 20)
            {
                return "1;1000000@10000159;30@10010094;1@10000165;10";
            }
            if (seasonLevel >= 15)
            {
                return "1;500000@10000159;20@10010093;1@10000165;5";
            }
            if (seasonLevel >= 10)
            {
                return "1;300000@10000159;10@10010093;1";
            }
            if (seasonLevel >= 5)
            {
                return "1;100000@10000159;5@10010092;1";
            }
            if (seasonLevel >= 1)
            {
                return "1;10000@10000159;2";
            }

            return string.Empty;
        }
        /// <summary>
        /// 第几赛季
        /// </summary>
        /// <param name="userLv"></param>
        /// <returns></returns>
        public static KeyValuePairLong GetOpenSeason(int userLv)
        {
            if (userLv < 55)
            {
                return null;
            }
            long serverTime = TimeHelper.ServerNow();
            //return SeasonOpenTime > 0 && serverTime >= SeasonOpenTime && serverTime <= SeasonCloseTime;
            for (int i = 0; i < SeasonTimeList.Count; i++)
            {
                if (serverTime > SeasonTimeList[i].Value && serverTime <= SeasonTimeList[i].Value2)
                {
                    return SeasonTimeList[i];
                }
            }

            return null;
        }

        /// <summary>
        /// 赛季开始时间 . 已废弃
        /// </summary>
        //public static long SeasonOpenTime = 1701360000000;

        //赛季结束时间 . 已废弃
        //public static long SeasonCloseTime = 1713715200000;

        /// <summary>
        /// 赛季开时间
        /// </summary>
        public static List<KeyValuePairLong> SeasonTimeList = new List<KeyValuePairLong>()
        {
            new KeyValuePairLong(){ KeyId = 0, Value = 1701360000000, Value2 = 1713715200000 },
            new KeyValuePairLong(){ KeyId = 1, Value = 1713715200001, Value2 = 1721577600000 },
            new KeyValuePairLong(){ KeyId = 2, Value = 1721577600001, Value2 = 1730390400000 },
            new KeyValuePairLong(){ KeyId = 3, Value = 1730390400001, Value2 = 1738339200000 },
            new KeyValuePairLong(){ KeyId = 4, Value = 1738339200001, Value2 = 1746028800000 },
        };

        /// <summary>
        /// 第几赛季
        /// </summary>
        public static long SeasonIndex = 0;

        /// <summary>
        /// 赛季BOSS
        /// </summary>
        public static int SeasonBossId = 90000051;
    }
}

