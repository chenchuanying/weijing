using System;
using System.Collections.Generic;
using System.Reflection;

namespace ET
{

    public static class ComHelp
    {

        public const int TapHttpOuter = 20008;
        public const int TapHttpIneer = 30300;


        public const int MaxZone = 1024;
        //版号专区
        public static bool IsBanHaoZone(int zone)
        {
            return zone == 3;
        }

        //内部专区
        public static bool IsAlphaZone(int zone)
        {
            return zone == 3;
        }

        public static bool IsZhuBoZone(int zone)
        {
            return zone == 128;
        }

        /// <summary>
        /// 3内部区 3版号区 202中心区 203机器人
        /// </summary>
        public static List<int> InnerZoneList = new List<int>() { 202, 203 };

        public const string RobotPassWord = "et@#robot";

        //public static string LocalIp = "192.168.1.16"; 
        public static string LocalIp = "127.0.0.1";

        //宠物魔法技能
        public static List<int> PetMagicSkill = new List<int>(2) { };
        //public static List<int> PetMagicSkill = new List<int>(2) { 80001003, 80002003 };
     
        public const int RankNumber = 30;
        public const int CampRankNumber = 50;
        public const int PetRankNumber = 100;
        public const int PetHeXinMax = 200;

        //熔炼获得道具
        public const int MeltingItemId = 1;

        public const int ShenYuanCostId = 10000150;


        public static int MaxShuLianDu()
        {
            return GlobalValueConfigCategory.Instance.Get(45).Value2;
        }

        public static int GetSkillCdRate(int sceneType)
        {
            int cdRate =  (sceneType == SceneTypeEnum.Arena || sceneType == SceneTypeEnum.Solo) ? 2 : 1;
            return cdRate;
        }

        public static int GetMaxBaoShiDu()
        {
            return 120;
        }

        public static int MainCityID()
        {
            return GlobalValueConfigCategory.Instance.Get(47).Value2;
        }

        /// <summary>
        /// 经验加成
        /// </summary>
        /// <returns></returns>
        public static float GetExpAdd(int userLv, ServerInfo serverInfo)
        {
            int worldLv = serverInfo.WorldLv;                   //世界等级
            RankingInfo rankingInfo = serverInfo.RankingInfo;   //肝帝[可能为空]

            int lvCha = worldLv - userLv;

            int LvCha_0 = 0;
            int LvCha_5 = 0;
            int LvCha_10 = 0;

            if (lvCha >= 5)
            {
                LvCha_0 = 5;
                lvCha = lvCha - LvCha_0;
            }
            else {
                LvCha_0 = lvCha;
                lvCha = 0;
            }

            if (lvCha >= 5)
            {
                LvCha_5 = 5;
                lvCha = lvCha - LvCha_5;
                LvCha_10 = lvCha;
            }
            else
            {
                LvCha_5 = lvCha;
                lvCha = 0;
            }

            //每级是2%
            //float pro = (worldLv - userLv) * 0.02f;
            //如果等级差是10级以上,那么超过10级的部分是5%  5级以上是3%
            float pro = LvCha_0 * 0.02f + LvCha_5 * 0.03f + LvCha_10 * 0.05f;

            if (pro > 1f) {
                pro = 1f;
            }

            if (pro < 0)
            {
                pro = 0f;
            }

            return pro;
        }

#if NOT_UNITY
        public static bool IsInnerNet()
        {
            if (StartMachineConfigCategory.Instance.Get(1).OuterIP.Contains("127.0.0.1")
               || StartMachineConfigCategory.Instance.Get(1).OuterIP.Contains("192.168"))
            {
                return true;
            }
            return false;
        }
#endif

        public static int GetPlayerLimit(int sceneId)
        {
            return SceneConfigCategory.Instance.Get(sceneId).PlayerLimit;
        }

        public static bool IfNull(string value) {

            if (value == "" || value == "0" || value == null) {
                return true;
            }
            else {
                return false;
            }
        }

        public static int GetTodayCreateRoleNumber(List<KeyValuePairLong> CreateRoleList)
        {
            int createNumber = 0;
            long serverTime = TimeHelper.ServerNow();
            int today = GetDayByTime(serverTime);
            for (int i = 0; i < CreateRoleList.Count; i++)
            {
                if (today == GetDayByTime(CreateRoleList[i].Value))
                { 
                    createNumber++; 
                }
            }
            return createNumber;
        }


        public static int GetDayByTime(long time)
        {
            DateTime dateTime = TimeInfo.Instance.ToDateTime(time);
            return dateTime.Year * 10000 + dateTime.Month * 100 + dateTime.Day;
        }

        //字符串转换整形List
        public static List<int> StringArrToIntList(string[] stringArr) {

            List<int> listValue = new List<int>();

            for (int i = 0; i < stringArr.Length; i++) {
                listValue.Add(int.Parse(stringArr[i]));
            }

            return listValue;
        }

        //根据品质返回一个Color
        public static string QualityReturnColor(int ItenQuality)
        {
            string color = "FFFFFF";
            switch (ItenQuality)
            {
                case 1:
                    //color = new Color(1, 1, 1);
                    break;

                case 2:
                    color = "00FF00";
                    break;
                case 3:
                    color = "0CC2D8";
                    break;

                case 4:
                    color = "EF7FFF";
                    break;
                case 5:
                    color = "FF7F00";
                    break;
                case 6:
                    color = "CC7F30";
                    break;
            }
            return color;
        }

        //浅拷贝即可
        public static T DeepCopy<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj == null || obj is string || obj.GetType().IsValueType) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            System.Reflection.FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (System.Reflection.FieldInfo field in fields)
            {
                //try { field.SetValue(retval, DeepCopy(field.GetValue(obj))); }
                try { field.SetValue(retval, (field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }

        public static T DeepCopy_2<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj == null || obj is string || obj.GetType().IsValueType) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            System.Reflection.FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (System.Reflection.FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopy(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }

        public static bool IsShowPaiMai(int itemType, int subType)
        {
            if (itemType == 1)
            {
                if (subType == 101 || subType == 106 || subType == 114 || subType == 121 || subType == 15 || subType == 102|| subType == 127)
                {
                    return true;
                }
            }

            if (itemType == 2)
            {
                
                if (subType == 1 || subType == 101 || subType == 121 || subType == 122)
                {
                    return true;
                }
                
            }

            if (itemType == 3 || itemType == 4)
            {
                return true;
            }
            return  false;
        }

        //根据时间蛋计算剩余消耗钻石
        public static int ReturnPetOpenTimeDiamond(int itemID,long endTime) {

            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemID);
            string[] itemUseinfo = itemConfig.ItemUsePar.Split('@');
            float costValue = float.Parse(itemUseinfo[1]);

            long timeNow = endTime - TimeHelper.ServerNow();
            if (timeNow <= 0)
            {
                return 0;
            }

            float proValue = (float)timeNow / 86400000f;
            int renturnInt = (int)(proValue * costValue);

            if (renturnInt < 10) {
                renturnInt = 10;
            }

            return renturnInt;
        }


        //暴击等级等属性转换成实际暴击率的方法
        public static float LvProChange(long value, int lv)
        {
            float proValue = (float)value / (float)(7500 + lv * 250);
            if (proValue < 0)
            {
                proValue = 0;
            }
            if (proValue > 0.75f)
            {
                proValue = 0.75f;
            }
            return proValue;
        }


        //藏宝图等级对应掉落
        public static int TreasureToDropID(int dungeonID,int roleLv,int type) {

            DungeonConfig dungCof = DungeonConfigCategory.Instance.Get(dungeonID);
            int lv = dungCof.EnterLv;
            if (RandomHelper.RandFloat01() <= 0.5f)
            {
                lv = roleLv;
            }

            //低级藏宝图
            if (type == 1) {

                if (lv <= 18)
                {
                    return 60801101;
                }

                if (lv <= 29)
                {
                    return 60801201;
                }

                if (lv <= 39)
                {
                    return 60801301;
                }

                if (lv <= 49)
                {
                    return 60801401;
                }

                if (lv <= 100)
                {
                    return 60801501;
                }
            }

            //高级藏宝图
            if (type == 2)
            {

                if (lv <= 18)
                {
                    return 60801151;
                }

                if (lv <= 29)
                {
                    return 60801251;
                }

                if (lv <= 39)
                {
                    return 60801351;
                }

                if (lv <= 49)
                {
                    return 60801451;
                }

                if (lv <= 100)
                {
                    return 60801551;
                }
            }



            return 0;

        }


        public static int GetZhuPuGaiLv(int monsterid, int itemid, int jiacheng)
        {
            if (monsterid == 0)
            {
                return 0;
            }
            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterid);
            int[] Parameter = monsterConfig.Parameter;
            if (Parameter == null)
            {
                return 0;
            }
            int gailv = Parameter[0];
            if (itemid > 0)
            {
                int add = GlobalValueConfigCategory.Instance.ZhuaPuItem[itemid];

                //抓捕怪物加成
                if (monsterConfig.MonsterSonType == 58) {
                    add = add * 2;
                }

                gailv += add;

            }
            //触点加成
            if (jiacheng == 2)
            {
                gailv += 50;
            }
            return gailv;
        }

        public static int GetJiaYuanPetExp(int petLv, int xinqingValue) {

            ExpConfig expCof = ExpConfigCategory.Instance.Get(petLv);
            float ProValue = 1;
            switch (xinqingValue) {
                case 0:
                    ProValue = 0.3f;
                    break;

                case 1:
                    ProValue = 0.5f;
                    break;

                case 2:
                    ProValue = 0.65f;
                    break;

                case 3:
                    ProValue = 0.8f;
                    break;

                case 4:
                    ProValue = 0.9f;
                    break;

                case 5:
                    ProValue = 1f;
                    break;
            }
            return (int)(expCof.PetItemUpExp* ProValue);
        }

        public static bool IsHexinMine(int mineType, int position, List<KeyValuePairInt> extends)
        {
            for (int  i= 0;i < extends.Count; i++)
            {
                if (extends[i].KeyId == mineType && extends[i].Value == position )
                {
                    return true;
                }
            
            }
            return false;
        }

        /// <summary>
        /// 矿场产出系数
        /// </summary>
        /// <param name="openDay"></param>
        /// <returns></returns>
        public static float GetMineCoefficient(int openDay, int mineType, int position, List<KeyValuePairInt> extends)
        {
            bool hexin = IsHexinMine(mineType, position, extends);

            float addValue = (float)openDay / 30f;
            if (addValue > 1)
            {
                addValue = 1;
            }
            //float proValue = ( hexin ? 1.5f : 1f) + addValue;

            float proValue = 1 + addValue;

            if (hexin) {
                proValue = proValue * 1.5f;
            }
            return proValue;
        }

        /// <summary>
        /// 投资利润总额
        /// </summary>
        /// <param name="touru"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int GetWelfareTotalLiRun(int total, int touzi)
        {
            return total - touzi;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="openDay"></param>
        /// <returns></returns>
        public static int GetWelfareDrawIndex(int openDay)
        {
            if (openDay == 1) //开服第1天
            {
                return 1;
            }

            if (openDay == 3) //开服第3天
            {
                return 3;
            }
            if (openDay == 5) //开服第5天
            {
                return 5;
            }
            if (openDay == 7) //开服第7天
            {
                return 7;
            }
            return -1;         //权重随机
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="occ">第一职业</param>
        /// <param name="occTwo">第二职业</param>
        /// <returns></returns>
        public static int GetWelfareWeapon(int occ, int occTwo)
        {
            int itemID = 0;
            switch (occTwo) {
                case 101:
                    itemID = 14000001;
                    break;
                case 102:
                    itemID = 14000001;
                    break;
                case 103:
                    itemID = 14000002;
                    break;
                case 201:
                    itemID = 14000003;
                    break;
                case 202:
                    itemID = 14000004;
                    break;
                case 203:
                    itemID = 14000004;
                    break;
                case 301:
                    itemID = 14000005;
                    break;
                case 302:
                    itemID = 14000005;
                    break;
                case 303:
                    itemID = 14000005;
                    break;
                case 401:
                    itemID = 14000003;
                    break;
                case 402:
                    itemID = 14000004;
                    break;
                case 403:
                    itemID = 14000004;
                    break;
            }

            if (itemID == 0) {
                switch (occ) {

                    case 1:
                        itemID = 14000001;
                        break;

                    case 2:
                        itemID = 14000003;
                        break;

                    case 3:
                        itemID = 14000005;
                        break;
                }
            }

            return itemID;
        }

        public static float GetTaskExpRewardCof(int playerlv)
        {
            float pro = 1 + (playerlv * 2 / 100);
            if (pro >= 3) {
                pro = 3;
            }
            return pro;
        }

        public static float GetTaskCoinRewardCof(int playerlv)
        {
            float pro = 1 + (playerlv * 2 / 100);
            if (pro >= 3)
            {
                pro = 3;
            }
            return pro;
        }

        //传入随机范围,生成一个随机数,越到后面的随机数获取概率越低
        public static int ReturnEquipRamdomValue(int randomMinValue, int randomMaxValue, int hideID = 0)
        {

            int randomChaValue = randomMaxValue - randomMinValue;
            //随机4次,获得取值范围
            /*
            0-25%       0.5
            26%-50%     0.3
            51%-75%     0.15
            76%-100%    0.05
            */
            float randomRangeValue = RandomHelper.RandFloat01();
            float randomRangeValue_Now = 0;
            if (randomRangeValue <= 0.5f)
            {
                //0-0.25f
                randomRangeValue_Now = randomRangeValue / 4;

            }
            if (randomRangeValue > 0.5f && randomRangeValue <= 0.8f)
            {
                //0.25-0.5
                randomRangeValue_Now = randomRangeValue / 4 + 0.25f;
            }
            if (randomRangeValue > 0.8f && randomRangeValue <= 0.95f)
            {
                //0.5-0.75
                randomRangeValue_Now = randomRangeValue / 4 + 0.5f;
            }
            if (randomRangeValue > 0.95f && randomRangeValue <= 1f)
            {
                //0.75-1
                randomRangeValue_Now = randomRangeValue / 4 + 0.75f;
            }

            //极品大师
            if (hideID != 0)
            {
                float hintSkillProValue = 0f;
                if (hintSkillProValue != 0f)
                {
                    randomRangeValue_Now = randomRangeValue_Now * (1 + hintSkillProValue);
                }
            }
            if (randomRangeValue_Now > 1)
            {
                randomRangeValue_Now = 1;
            }

            //计算最终值
            int retunrnValue = (int)(randomMinValue + randomChaValue * randomRangeValue_Now);
            return retunrnValue;
        }

        //传入随机范围,生成一个随机数,越到后面的随机数获取概率越低
        public static int ReturnEquipRamdomValue(int randomMinValue, int randomMaxValue)
        {

            int randomChaValue = randomMaxValue - randomMinValue;
            //随机4次,获得取值范围
            /*
            0-25%       0.5
            26%-50%     0.3
            51%-75%     0.15
            76%-100%    0.05
            */
            float randomRangeValue = RandomHelper.RandFloat();
            float randomRangeValue_Now = 0;
            if (randomRangeValue <= 0.5f)
            {
                //0-0.25f
                randomRangeValue_Now = RandomHelper.RandFloat() / 4;

            }
            if (randomRangeValue > 0.5f && randomRangeValue <= 0.8f)
            {
                //0.25-0.5
                randomRangeValue_Now = RandomHelper.RandFloat() / 4 + 0.25f;
            }
            if (randomRangeValue > 0.8f && randomRangeValue <= 0.95f)
            {
                //0.5-0.75
                randomRangeValue_Now = RandomHelper.RandFloat() / 4 + 0.5f;
            }
            if (randomRangeValue > 0.95f && randomRangeValue <= 1f)
            {
                //0.75-1
                randomRangeValue_Now = RandomHelper.RandFloat() / 4 + 0.75f;
            }

            if (randomRangeValue_Now > 1)
            {
                randomRangeValue_Now = 1;
            }

            //计算最终值
            int retunrnValue = (int)(randomMinValue + randomChaValue * randomRangeValue_Now);
            return retunrnValue;
        }

        //传入随机范围,生成一个随机数,越到后面的随机数获取概率越低
        public static float ReturnEquipRamdomValue_float(float randomMinValue, float randomMaxValue)
        {

            float randomChaValue = randomMaxValue - randomMinValue;
            //随机4次,获得取值范围
            /*
            0-25%       0.5
            26%-50%     0.3
            51%-75%     0.15
            76%-100%    0.05
            */
            float randomRangeValue = RandomHelper.RandFloat();
            float randomRangeValue_Now = 0;
            if (randomRangeValue <= 0.5f)
            {
                //0-0.25f
                randomRangeValue_Now = RandomHelper.RandFloat() / 4;

            }
            if (randomRangeValue > 0.5f && randomRangeValue <= 0.8f)
            {
                //0.25-0.5
                randomRangeValue_Now = RandomHelper.RandFloat() / 4 + 0.25f;
            }
            if (randomRangeValue > 0.8f && randomRangeValue <= 0.95f)
            {
                //0.5-0.75
                randomRangeValue_Now = RandomHelper.RandFloat() / 4 + 0.5f;
            }
            if (randomRangeValue > 0.95f && randomRangeValue <= 1f)
            {
                //0.75-1
                randomRangeValue_Now = RandomHelper.RandFloat() / 4 + 0.75f;
            }

            if (randomRangeValue_Now > 1)
            {
                randomRangeValue_Now = 1;
            }

            //计算最终值
            float retunrnValue = (float)(randomMinValue + randomChaValue * randomRangeValue_Now);
            retunrnValue = (float)(System.Math.Round(retunrnValue, 3));
            return retunrnValue;
        }

        //第一天达到10级以上或第二天进行等级限制
        //1-10级默认开启（只有第一天,确保开始的时候是开启的）
        //以下三个条件满足任意一个
        //拍卖行购买功能限制
        //1. 充值任意金额
        //2. 或者击败2个以上的同级别（等级不低于玩家等级5级）的BOSS(狼王不算)
        //3.
        //1. 14  （24小时内)
        //2. 16
        //3. 18
        //4. 19
        //5. 20
        //6. 21
        //7. 22
        //以上开启一次就不会进行二次限制。
        //未开启统一提示:等级需达到X级或赞助任意金额开启拍卖行购买功能！
        public static int IsCanPaiMai_Level(int createDay, int lv)
        {
            if(createDay == 1)
            {
                if(lv <= 20)
                {
                    return 20;
                }
                return 0;
            }
            if (createDay <= 2)
            {
                if (lv <= 22)
                {
                    return 22;
                }
                return 0;
            }
            if (createDay <= 3)
            {
                if (lv <= 24)
                {
                    return 24;
                }
                return 0;
            }
            if (createDay <= 4)
            {
                if (lv <= 26)
                {
                    return 26;
                }
                return 0;
            }
            if (createDay <= 5)
            {
                if (lv <= 28)
                {
                    return 28;
                }
                return 0;
            }
            if (createDay <= 6)
            {
                if (lv <= 28)
                {
                    return 28;
                }
                return 0;
            }
            if (createDay <= 7)
            {
                if (lv <= 28)
                {
                    return 28;
                }
                return 0;
            }
            return 0;    
        }

        public static bool IsCanPaiMai_Recharge( Unit unit )
        {
            return unit.GetComponent<NumericComponent>().GetAsInt(NumericType.RechargeNumber) > 0;  
        }

        public static bool IsCanPaiMai_KillBoss(List<KeyValuePair> monsterlist, int lv)
        {
            int number = 0;

            for (int i = 0; i < monsterlist.Count; i++)
            {
                if (monsterlist[i].KeyId == 70001004)
                {
                    continue;
                }

                MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterlist[i].KeyId);
                if (monsterConfig.Lv >= (lv - 5))
                {
                    number++;
                }
            }
            return number >= 3;
        }

        public static int KillBoss_Lv_Number(List<KeyValuePair> monsterlist, int lv)
        {
            int number = 0;

            for (int i = 0; i < monsterlist.Count; i++)
            {
                if (monsterlist[i].KeyId == 70001004)
                {
                    continue;
                }

                MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterlist[i].KeyId);
                if (monsterConfig.Lv >= (lv - 5))
                {
                    number++;
                }
            }
            return number;
        }

        public static bool IsCanChat_KillBoss(List<KeyValuePair> monsterlist, int lv)
        {
            if (lv >= 30)
            {
                return true;
            }

            int number = 0;
            for (int i = 0; i < monsterlist.Count; i++)
            {
                if (monsterlist[i].KeyId == 70001004)
                {
                    continue;
                }

                MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterlist[i].KeyId);
                if (monsterConfig.Lv >= (lv - 5))
                {
                    number++;
                }
            }
            return number >= 3;
        }


        //宠物守护
        public static float GetPetShouHuPro(int mainValue, int fightValue)
        {

            mainValue = mainValue - 2000;
            if (mainValue < 2000)
            {
                mainValue = 0;
            }

            fightValue = fightValue - 8000;
            if (fightValue < 0)
            {
                fightValue = 0;
            }

            float mainPro = LvProChange((long)(mainValue * 0.1f), 70);
            float otherPro = LvProChange((long)(fightValue / 4 * 0.1f), 70);

            if (mainPro < 0)
            {
                mainPro = 0;
            }

            if (otherPro < 0)
            {
                otherPro = 0;
            }

            float pro = mainPro + otherPro;

            return pro;
        }

    }
}
