using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{

    [Timer(TimerType.ShouLieUpLoadTimer)]
    public class ShouLieUpLoadTimer : ATimer<UserInfoComponent>
    {
        public override void Run(UserInfoComponent self)
        {
            try
            {
                self.UpdateShowLie().Coroutine();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }


    [ObjectSystem]
    public class UserInfoComponentAwake : AwakeSystem<UserInfoComponent>
    {
        public override void Awake(UserInfoComponent self)
        {

        }
    }

    [ObjectSystem]
    public class UserInfoComponentDestroy : DestroySystem<UserInfoComponent>
    {
        public override void Destroy(UserInfoComponent self)
        {
            TimerComponent.Instance.Remove(ref self.ShouLieUpLoadTimer);
        }
    }

    public static class UserInfoComponentSystem
    {
        public static void Check(this UserInfoComponent self)
        {
            self.TodayOnLine++;
            self.LingDiOnLine++;

            //领地和家园都是一小时刷新一次经验
            if (self.LingDiOnLine > 60)
            {
                self.LingDiOnLine = 0;
                //self.OnRongyuChanChu(1, true);
                self.OnJiaYuanExp(1f);
            }

            if (self.UpdateRankTime > 0 )
            {
                self.UpdateRankTime = 0;
                self.UploadCombat().Coroutine();
            }
        }

        public static void OnJiaYuanExp(this UserInfoComponent self, float hour)
        {
            JiaYuanConfig jiaYuanConfig = JiaYuanConfigCategory.Instance.Get(self.UserInfo.JiaYuanLv);
            //self.UserInfo.JiaYuanExp += jiaYuanConfig.JiaYuanAddExp;
            int addexp = Mathf.FloorToInt(hour * jiaYuanConfig.JiaYuanAddExp);
            self.UpdateRoleMoneyAdd(UserDataType.JiaYuanExp, $"{addexp}", true, ItemGetWay.JiaYuanExchange);
        }

        public static void OnRongyuChanChu(this UserInfoComponent self, int coefficient, bool notice)
        {
            if (coefficient == 0)
            {
                return;
            }
            Unit unit = self.GetParent<Unit>();
            int lingdiLv = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Ling_DiLv);
            LingDiConfig lingDiConfig = LingDiConfigCategory.Instance.Get(lingdiLv);

            //unit.GetComponent<UserInfoComponent>().UpdateRoleData(UserDataType.Exp, (coefficient *lingDiConfig.HoureExp).ToString(), notice).Coroutine();
            self.UpdateRoleData(UserDataType.FangRong, (coefficient * lingDiConfig.HoureExp).ToString(), notice);
            self.UpdateRoleData(UserDataType.RongYu, (coefficient * lingDiConfig.HoureHonor).ToString(), notice);
        }

        public static void OpenAll(this UserInfoComponent self)
        {
            self.UserInfo.FubenPassList.Clear();

            Dictionary<int, ChapterConfig> keyValuePairs = ChapterConfigCategory.Instance.GetAll();
            foreach (var item in keyValuePairs)
            {
                self.UserInfo.FubenPassList.Add(new FubenPassInfo()
                {
                    FubenId = item.Key,
                    Difficulty = (int)FubenDifficulty.DiYu
                });
            }
        }


        public static int GetTiLiTimes(this UserInfoComponent self, int hour_1, int hour_2)
        {
            int index_1 = self.GetTiLiIndex(hour_1);
            int index_2 = self.GetTiLiIndex(hour_2);
            if (index_1 > index_2)
            {
                return 0;
            }
            return index_2 - index_1;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <param name="self"></param>
        /// <param name="hour_1"></param>
        /// <param name="hour_2"></param>  0 6 12 20
        /// <returns></returns>
        public static List<int> GetTiLiIndexsNew(this UserInfoComponent self, int hour_1, int hour_2)
        {
            List<int> indexs = new  List<int>();    
            if (hour_1 >= hour_2)
            {

                return indexs;
            }
            if (hour_1 < 6 && hour_2 >= 6)
            {
                indexs.Add(6);
            }
            if (hour_1 < 12 && hour_2 >= 12)
            {
                indexs.Add(12);
            }
            if (hour_1 < 20 && hour_2 >= 20)
            {
                indexs.Add(20);
            }

            return indexs;
        }

        public static int GetTiliRecover(this UserInfoComponent self, List<int> indexids)
        {
            int totalTili = 0;
            int totalindex = indexids.Count;
            if (totalindex >= 1 && indexids.Contains(6))
            {
                totalTili += 50;
                totalindex--;
            }
            if (totalindex >= 1 && indexids.Contains(20))
            {
                totalTili += 50;
                totalindex--;
            }
            if (totalindex >= 1)
            {
                totalTili = totalTili + totalindex * 30;
                totalindex = 0;
            }
            return totalTili;
        }

        public static int GetTiLiIndex(this UserInfoComponent self, int hour_1)
        {
            if (hour_1 < 6)
            {
                return 1;
            }
            if (hour_1 < 12)
            {
                return 2;
            }
            if (hour_1 < 20)
            {
                return 3;
            }
            if (hour_1 < 24)
            {
                return 4;
            }
            return 5;
        }

        public static void CheckData(this UserInfoComponent self)
        {

            if (self.UserInfo.JiaYuanLv <= 0)
            {
                self.UserInfo.JiaYuanLv = 10001;
            }
            if (self.UserInfo.SeasonLevel == 0)
            {
                self.UserInfo.SeasonLevel = 1;
            }
            if (self.UserInfo.CreateTime == 0)
            {
                self.UserInfo.CreateTime = TimeHelper.ServerNow();
            }
            if (self.UserInfo.Lv < 20 && self.UserInfo.BaoShiDu < 100)
            {
                self.UserInfo.BaoShiDu = 100;
            }

            int maxTowerId = 0;
            if (self.UserInfo.TowerRewardIds.Count > 0)
            {
                maxTowerId = self.UserInfo.TowerRewardIds[self.UserInfo.TowerRewardIds.Count - 1];
            }
            NumericComponent numericComponent = self.GetParent<Unit>().GetComponent<NumericComponent>();
            if (numericComponent.GetAsInt(NumericType.TrialDungeonId) < maxTowerId)
            {
                numericComponent.Set(NumericType.TrialDungeonId, maxTowerId, false);
            }
            if (numericComponent.GetAsInt(NumericType.UpdateActivty) == 0)
            {
                self.GetParent<Unit>().GetComponent<ActivityComponent>().ClearJieRiActivty();
                numericComponent.Set(NumericType.UpdateActivty, 1, false);
            }
            DataCollationComponent dataCollationComponent = self.GetParent<Unit>().GetComponent<DataCollationComponent>();
            int recharge = numericComponent.GetAsInt(NumericType.RechargeNumber);
            if (recharge!=0 && dataCollationComponent.ChouKaTimes > (recharge * 2) && dataCollationComponent.ChouKaTimes > 100)
            {
                Log.Warning($"抽卡次数异常:{self.DomainZone()} {self.UserInfo.Name}   充值:{numericComponent.GetAsInt(NumericType.RechargeNumber)}  抽卡:{dataCollationComponent.ChouKaTimes}");
            }

            // 烟雨楼Id: 2466222808943362373   烟雨楼 寸断De法殇 ID: 2466171477355986944
            if (self.UserInfo.UserId == 2466171477355986944)
            {
                //self.UserInfo.UnionName = "烟雨楼";
                //self.GetParent<Unit>().GetComponent<NumericComponent>().ApplyValue(NumericType.UnionLeader, 0, false);
                //self.GetParent<Unit>().GetComponent<NumericComponent>().ApplyValue(NumericType.UnionId_0, 2466222808943362373, false);
            }

            if (self.UserInfo.UnionKeJiList.Count < UnionKeJiConfigCategory.Instance.UnionQiangHuaList.Count)
            {
                int curNumber = self.UserInfo.UnionKeJiList.Count;
                int maxNumber = UnionKeJiConfigCategory.Instance.UnionQiangHuaList.Count;
                for (int keji = curNumber; keji < maxNumber; keji++)
                {
                    self.UserInfo.UnionKeJiList.Add(UnionKeJiConfigCategory.Instance.GetFristId( keji ) );
                }
            }
        }

        public static void OnOffLine(this UserInfoComponent self)
        {
            //self.LastLoginTime = TimeHelper.ServerNow();
        }

        public static void OnLogin(this UserInfoComponent self, string remoteIp, string deviceName)
        {
            self.CheckData();
            self.RemoteAddress = remoteIp;
            self.DeviceName = deviceName;
            Unit unit = self.GetParent<Unit>();
            long currentTime = TimeHelper.ServerNow();

            DateTime dateTime = TimeInfo.Instance.ToDateTime(currentTime);
            long lastLoginTime = self.LastLoginTime;
            if (lastLoginTime != 0)
            {
                DateTime lastdateTime = TimeInfo.Instance.ToDateTime(lastLoginTime);
                if (dateTime.Day != lastdateTime.Day)
                {
                    Log.Debug($"OnZeroClockUpdate [登录刷新]: {unit.Id}");
                    float passhour = ((currentTime - lastLoginTime) *1f / TimeHelper.Hour);
                    if (passhour >= 24f)
                    {
                        self.RecoverPiLao(120, false);
                    }
                    else
                    {

                        List<int> indexids_1 = self.GetTiLiIndexsNew(lastdateTime.Hour, 23);
                        List<int> indexids_2 = self.GetTiLiIndexsNew(0, dateTime.Hour);
                        List<int> indexids = new List<int>();
                        indexids.Add(0);
                        indexids.AddRange(indexids_1);
                        indexids.AddRange(indexids_2);
                        if (indexids.Count > 0)
                        {
                            int recoverTili = self.GetTiliRecover(indexids);
                            self.RecoverPiLao(recoverTili, false);
                            string indexstr = $"{unit.Id}  two day : hour_1: {lastdateTime.Hour}  hour_2:{dateTime.Hour}   indexs: ";
                            for (int index = 0; index < indexids.Count; index++)
                            {
                                indexstr = indexstr + indexids[index].ToString() + "   ";
                            }
                            indexstr = indexstr + $"recover: {recoverTili}";
                            Log.Debug(indexstr);
                        }

                    }
                    self.OnZeroClockUpdate(false);
                    unit.GetComponent<TaskComponent>().CheckWeeklyUpdate(lastLoginTime, currentTime);
                    unit.GetComponent<TaskComponent>().OnZeroClockUpdate(false);
                    unit.GetComponent<EnergyComponent>().OnResetEnergyInfo();
                    unit.GetComponent<HeroDataComponent>().OnZeroClockUpdate(false);
                    unit.GetComponent<ActivityComponent>().OnZeroClockUpdate(self.UserInfo.Lv);
                    unit.GetComponent<ChengJiuComponent>().OnZeroClockUpdate();
                    unit.GetComponent<JiaYuanComponent>().OnZeroClockUpdate(false);
                    unit.GetComponent<DataCollationComponent>().OnZeroClockUpdate(false);
                    self.OnJiaYuanExp(Math.Min(passhour, 12f));
                }
                else
                {
                    int hour_1, hour_2 = 0;
                    hour_1 = lastdateTime.Hour;
                    hour_2 = dateTime.Hour;

                    List<int> indexids = self.GetTiLiIndexsNew(hour_1, hour_2);
                    if (indexids.Count > 0)
                    { 
                        int recoverTili = self.GetTiliRecover(indexids);
                        self.RecoverPiLao(recoverTili, false);
                        string indexstr = $"{unit.Id}  one day  hour_1: {hour_1}  hour_2:{hour_2}   indexs: ";
                        for (int index = 0; index < indexids.Count; index++)
                        {
                            indexstr = indexstr + indexids[index].ToString() + "   ";
                        }
                        indexstr = indexstr + $"recover: {recoverTili}";
                        Log.Debug(indexstr);
                    }
  
                    unit.GetComponent<JiaYuanComponent>().OnLoginCheck(hour_1, hour_2);
                    float passhour = ((currentTime - lastLoginTime) * 1f / TimeHelper.Hour);
                    self.OnJiaYuanExp(Math.Min(passhour, 12f));
                }
            }
            else
            {
                Log.Debug($"OnZeroClockUpdate [数据初始化]: {unit.Id}");
                unit.GetComponent<TaskComponent>().OnZeroClockUpdate(false);
            }

            unit.GetComponent<BagComponent>().OnLogin(self.UserInfo.RobotId);
            unit.GetComponent<TaskComponent>().OnLogin();
            unit.GetComponent<HeroDataComponent>().OnLogin(self.UserInfo.RobotId);
            unit.GetComponent<DBSaveComponent>().OnLogin();
            unit.GetComponent<RechargeComponent>().OnLogin();
            unit.GetComponent<PetComponent>().OnLogin();
            unit.GetComponent<ActivityComponent>().OnLogin(self.UserInfo.Lv);
            unit.GetComponent<TitleComponent>().OnCheckTitle(false);
            unit.GetComponent<ChengJiuComponent>().OnLogin();
            unit.GetComponent<JiaYuanComponent>().OnLogin();
            unit.GetComponent<SkillSetComponent>().OnLogin(self.UserInfo.Occ);

            self.LastLoginTime = currentTime;
            self.UserName = self.UserInfo.Name;
            self.ShouLieSendTime = 0;
        }

        /// <summary>
        /// 体力
        /// </summary>
        /// <param name="self"></param>
        /// <param name="skillNumber"></param>
        /// <returns></returns>
        public static int GetAddPiLao(this UserInfoComponent self, int skillNumber)
        {
            return 0;
        }

        /// <summary>
        /// 0 6 12 20点各刷新30点体力
        /// </summary>
        /// <param name="self"></param>
        /// <param name="notice"></param>
        public static void OnHourUpdate(this UserInfoComponent self, int hour, bool notice)
        {
            if (self.LastLoginTime > 0)
            {
                DateTime lastdateTime = TimeInfo.Instance.ToDateTime(self.LastLoginTime);
                DateTime nowdateTime = TimeInfo.Instance.ToDateTime(TimeHelper.ServerNow());
                if ( lastdateTime.Hour == nowdateTime.Hour)
                {
                    if (self.Id == 2341487098982367232)
                    {
                        Console.WriteLine($"刀：lastdateTime.Hour == nowdateTime.Hour  {hour}");
                    }
                    return;
                }
            }

            if (hour == 0 )
            {
                self.RecoverPiLao(30 + self.GetAddPiLao(self.UserInfo.MakeList.Count), notice);
            }
            if (hour == 12)
            {
                self.RecoverPiLao(30, notice);
            }

            if (hour == 6 ||  hour == 20)
            {
                self.RecoverPiLao(50, notice);
            }

            self.GetParent<Unit>().GetComponent<JiaYuanComponent>().OnHourUpdate(hour, notice);
            LogHelper.CheckZuoBi(self.GetParent<Unit>());
            //LogHelper.CheckBlackRoom(self.GetParent<Unit>());
        }

        public static void RecoverPiLao(this UserInfoComponent self, int addValue, bool notice)
        {
            Unit unit = self.GetParent<Unit>();
            long recoverPiLao = self.GetParent<Unit>().GetMaxPiLao() - self.UserInfo.PiLao;
            recoverPiLao = Math.Min(recoverPiLao, addValue);

            Log.Warning($"[增加体力] {unit.DomainZone()}    {unit.Id}    {recoverPiLao}");
            self.UpdateRoleData(UserDataType.PiLao, recoverPiLao.ToString(), notice);
            self.LastLoginTime = TimeHelper.ServerNow();
        }



        public static void OnZeroClockUpdate(this UserInfoComponent self, bool notice)
        {
            Unit unit = self.GetParent<Unit>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            int skillNumber = 1 + numericComponent.GetAsInt(NumericType.MakeType_2) > 0 ? 1 : 0;
            int updatevalue = unit.GetMaxHuoLi(skillNumber) - self.UserInfo.Vitality;
            self.UpdateRoleData(UserDataType.Vitality, updatevalue.ToString(), notice);
            //updatevalue = ComHelp.GetMaxBaoShiDu() - self.UserInfo.BaoShiDu;
            //self.UpdateRoleData(UserDataType.BaoShiDu, updatevalue.ToString(), notice);
            numericComponent.ApplyValue(NumericType.ZeroClock, 1, notice);
            self.ClearDayData();
            self.LastLoginTime = TimeHelper.ServerNow();
            self.TodayOnLine = 0;
            self.ShouLieKill = 0;
        }

        public static UserInfo GetUserInfo(this UserInfoComponent self)
        {
            return self.UserInfo;
        }

        public static void OnShowLieKill(this UserInfoComponent self)
        {
            self.ShouLieKill++;

            if (self.ShouLieUpLoadTimer == 0)
            {
                self.ShouLieUpLoadTimer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 5 * TimeHelper.Second, TimerType.ShouLieUpLoadTimer, self);
            }
            else
            {
                self.UpdateShowLie().Coroutine();
            }
        }

        public static async ETTask UpdateShowLie(this UserInfoComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            if (!ActivityHelper.IsShowLieOpen() || unit.IsRobot())
            {
                return;
            }
            self.ShouLieSendTime = TimeHelper.ServerNow();
            TimerComponent.Instance.Remove(ref self.ShouLieUpLoadTimer);
            RankShouLieInfo rankPetInfo = new RankShouLieInfo();
            UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
            rankPetInfo.UnitID = userInfoComponent.UserInfo.UserId;
            rankPetInfo.PlayerName = userInfoComponent.UserInfo.Name;
            rankPetInfo.Occ = userInfoComponent.UserInfo.Occ;
            rankPetInfo.KillNumber = self.ShouLieKill;
            long mapInstanceId = DBHelper.GetRankServerId(self.DomainZone());
            R2M_RankShowLieResponse Response = (R2M_RankShowLieResponse)await ActorMessageSenderComponent.Instance.Call
                     (mapInstanceId, new M2R_RankShowLieRequest()
                     {
                         RankingInfo = rankPetInfo
                     });
        }


        /// <summary>
        /// 杀怪经验
        /// </summary>
        /// <param name="self"></param>
        /// <param name="beKill"></param>
        public static void OnKillUnit(this UserInfoComponent self, Unit beKill, int sceneType, int sceneId)
        {
            Unit main = self.GetParent<Unit>();
            if (beKill.Type != UnitType.Monster)
            {
                return;
            }

            bool showlieopen = ActivityHelper.IsShowLieOpen();
            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(beKill.ConfigId);
            if (showlieopen && ( monsterConfig.Lv >= 60 || Mathf.Abs(self.UserInfo.Lv - monsterConfig.Lv) <= 9) )
            {
                self.OnShowLieKill();
                main.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.ShowLieMonster_1201, 0, 1);
            }

            if (sceneType == SceneTypeEnum.LocalDungeon && monsterConfig.MonsterSonType == 55)
            {
                self.OnAddChests(sceneId, beKill.ConfigId);
            }

            if (SeasonHelper.GetOpenSeason(self.UserInfo.Lv)!=null && beKill.IsBoss() && monsterConfig.Lv >= 40)
            {
                int seasonExp = RandomHelper.RandomNumber(1, 6);
                self.UpdateRoleData(UserDataType.SeasonExp, seasonExp.ToString());
            }

            NumericComponent numericComponent = main.GetComponent<NumericComponent>();
            numericComponent.ApplyChange(null, NumericType.KillMonsterNumber, 1, 0);

            int tiliKillNumber = numericComponent.GetAsInt(NumericType.TiLiKillNumber);
            if (sceneType == SceneTypeEnum.LocalDungeon && !showlieopen && self.UserInfo.PiLao > 0)
            {
                if (tiliKillNumber >= 4)
                {
                    numericComponent.ApplyValue(NumericType.TiLiKillNumber, 0, false);

                    numericComponent.ApplyChange(null, NumericType.CostTiLi, 1, 0);
                    if ( ComHelp.IsZhuBoZone(self.DomainZone()) && self.UserInfo.PiLao < 2)
                    {
                        self.UpdateRoleData(UserDataType.PiLao, "100", true);
                    }
                    else
                    {
                        self.UpdateRoleData(UserDataType.PiLao, "-1", true);
                    }
                }
                else
                {
                    numericComponent.ApplyChange(null, NumericType.TiLiKillNumber,  1, 0);
                }
            }

            bool drop = true;
            if (SceneConfigHelper.IsSingleFuben(sceneType))
            {
                drop = self.UserInfo.PiLao > 0 || beKill.IsBoss() || showlieopen;
            }
            if (drop)
            {
                MonsterConfig mCof = MonsterConfigCategory.Instance.Get(beKill.ConfigId);
                float expcoefficient = 1f;
                if (sceneType == SceneTypeEnum.LocalDungeon && beKill.IsBoss())
                {
                    int killNumber = main.GetComponent<UserInfoComponent>().GetMonsterKillNumber(mCof.Id);
                    int chpaterid = DungeonConfigCategory.Instance.GetChapterByDungeon(sceneId);
                    BossDevelopment bossDevelopment = ConfigHelper.GetBossDevelopmentByKill(chpaterid, killNumber);
                    expcoefficient *= bossDevelopment.ExpAdd;
                }

                if ((sceneType == SceneTypeEnum.LocalDungeon && self.UserInfo.PiLao > 0)
                  || sceneType != SceneTypeEnum.LocalDungeon)
                {
                    if (numericComponent.GetAsInt(NumericType.JueXingExp) < 5000)
                    {
                        numericComponent.ApplyChange(null, NumericType.JueXingExp, 1, 0);
                    }
                }

                int addexp = (int)(expcoefficient * mCof.Exp);
                self.UpdateRoleData(UserDataType.Exp, addexp.ToString());
            }

            // 纪录击败的Boss
            if (beKill.IsBoss() && ConfigHelper.DefeatedBossIds.ContainsKey(beKill.ConfigId))
            {
                if (!self.UserInfo.DefeatedBossIds.Contains(beKill.ConfigId))
                {
                    self.UserInfo.DefeatedBossIds.Add(beKill.ConfigId);
                }
            }
        }

        public static void UpdateRoleDataBroadcast(this UserInfoComponent self, int Type, string value)
        {
            Unit unit = self.GetParent<Unit>();
            M2C_RoleDataBroadcast m2C_BroadcastRoleData = self.m2C_RoleDataBroadcast;
            m2C_BroadcastRoleData.UnitId = unit.Id;
            m2C_BroadcastRoleData.UpdateType = (int)Type;
            m2C_BroadcastRoleData.UpdateTypeValue = value;
            MessageHelper.Broadcast(unit, m2C_BroadcastRoleData);
        }

        public static int GetMysteryBuy(this UserInfoComponent self, int mysteryId)
        {
            for (int i = 0; i < self.UserInfo.MysteryItems.Count; i++)
            {
                if (self.UserInfo.MysteryItems[i].KeyId == mysteryId)
                {
                    return (int)self.UserInfo.MysteryItems[i].Value;
                }
            }
            return 0;
        }

        public static void OnMysteryBuy(this UserInfoComponent self, int mysteryId)
        {
            for (int i = 0; i < self.UserInfo.MysteryItems.Count; i++)
            {
                if (self.UserInfo.MysteryItems[i].KeyId == mysteryId)
                {
                    self.UserInfo.MysteryItems[i].Value += 1;
                    return;
                }
            }
            self.UserInfo.MysteryItems.Add(new KeyValuePairInt() { KeyId = mysteryId, Value = 1 });
        }

        public static int GetStoreBuy(this UserInfoComponent self, int mysteryId)
        {
            for (int i = 0; i < self.UserInfo.BuyStoreItems.Count; i++)
            {
                if (self.UserInfo.BuyStoreItems[i].KeyId == mysteryId)
                {
                    return (int)self.UserInfo.BuyStoreItems[i].Value;
                }
            }
            return 0;
        }

        public static void OnStoreBuy(this UserInfoComponent self, int mysteryId)
        {
            for (int i = 0; i < self.UserInfo.BuyStoreItems.Count; i++)
            {
                if (self.UserInfo.BuyStoreItems[i].KeyId == mysteryId)
                {
                    self.UserInfo.BuyStoreItems[i].Value += 1;
                    return;
                }
            }
            self.UserInfo.BuyStoreItems.Add(new KeyValuePairInt() { KeyId = mysteryId, Value = 1 });
        }

        //加金币
        public static void UpdateRoleMoneyAdd(this UserInfoComponent self, int Type, string value, bool notice, int getWay, string paramsifo = "")
        {
            Unit unit = self.GetParent<Unit>();
            long gold = long.Parse(value);
            if (gold < 0)
            {
                Log.Warning($"增加货币出错:{Type}  {unit.Id} {getWay} {self.UserInfo.Name}  {value}", true);
            }
            else
            {
                if (getWay != ItemGetWay.PickItem || gold > 1000)
                {
                    LogHelper.LogWarning($"增加货币:{Type} {unit.Id} {getWay} {self.UserInfo.Name}  {value}", true);
                }
            }
            if (gold > 100000 || gold < -100000)
            {
                Log.Warning($"增加货币[大额]:{Type} {unit.Id} {getWay} {self.UserInfo.Name} {value}  {paramsifo}", true);
            }
            else if (gold > 1000000 || gold < -1000000)
            {
                Log.Warning($"增加货币[超额]:{Type} {unit.Id} {getWay} {self.UserInfo.Name} {value}", true);
            }

            if (gold > 0 && getWay == ItemGetWay.PaiMaiSell)
            {
                unit.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.PaiMaiGetGoldNumber_217, 0, (int)gold);
            }

            if (Type == UserDataType.Diamond)
            {
                self.UserInfo.DiamondGetWay.Add(getWay);
                if (self.UserInfo.DiamondGetWay.Count > 200)
                {
                    self.UserInfo.DiamondGetWay.RemoveAt(0);    
                }
            }

            if (Type == UserDataType.Gold)
            {
                self.UserInfo.GoldGetWay.Add(getWay);
                if (self.UserInfo.GoldGetWay.Count > 200)
                {
                    self.UserInfo.GoldGetWay.RemoveAt(0);
                }
            }

            if (Type == UserDataType.Exp)
            {
                self.UserInfo.ExpGetWay.Add(getWay);
                if (self.UserInfo.ExpGetWay.Count > 200)
                {
                    self.UserInfo.ExpGetWay.RemoveAt(0);
                }
            }

            if (Type == UserDataType.Diamond)
            {
                Log.Warning($"增加钻石: {Type} {unit.Id} {getWay} {self.UserInfo.Name} {value}");
            }

            if (Type == UserDataType.UnionExp || Type == UserDataType.UnionGold)
            {
                self.SendUnionOperate(getWay, Type, gold).Coroutine();
            }
            unit.GetComponent<DataCollationComponent>().UpdateRoleMoneyAdd(Type, getWay, gold);
            self.UpdateRoleData(Type, value, notice);
        }

        //扣金币
        public static void UpdateRoleMoneySub(this UserInfoComponent self, int Type, string value, bool notice = true, int getWay = ItemGetWay.System, string paramsifo = "")
        {
            Unit unit = self.GetParent<Unit>();
            long gold = long.Parse(value);
            if (gold > 0)
            {
                LogHelper.LogWarning($"扣除货币出错:{Type} {unit.Id} {getWay} {self.UserInfo.Name}  {value}", true);
            }
            else
            {
                LogHelper.LogWarning($"扣除货币:{Type} {unit.Id} {getWay} {self.UserInfo.Name} {value}", true);
            }
            if (gold > 100000 || gold < -100000)
            {
                Log.Warning($"扣除货币[大额]:{Type} {unit.Id} {getWay} {self.UserInfo.Name} {value}");
            }
            if (Type == UserDataType.Diamond)
            {
                Log.Warning($"扣除钻石: {Type} {unit.Id} {getWay} {self.UserInfo.Name} {value}");
            }

            unit.GetComponent<DataCollationComponent>().UpdateRoleMoneySub(Type, getWay, gold);
            self.UpdateRoleData(Type, value, notice);
        }

        public static async ETTask SendUnionOperate(this UserInfoComponent self, int getWay, int dataType,  long dataValue)
        {
            Unit unit = self.GetParent<Unit>();
            long unionid = unit.GetUnionId();
            if (unionid == 0)
            {
                return;
            }
            string playerName = self.UserInfo.Name;
            long serverod = DBHelper.GetUnionServerId(self.DomainZone() );
            U2M_UnionOperationResponse responseUnionEnter = (U2M_UnionOperationResponse)await ActorMessageSenderComponent.Instance.Call(
                            serverod, new M2U_UnionOperationRequest() { OperateType = 1, UnionId = unionid, Par = $"{playerName}_{getWay}_{dataType}_{dataValue}" });
        }

        public static async ETTask BroadcastLevel(this UserInfoComponent self, int level)
        {
            Unit unit = self.GetParent<Unit>();
            long chatServerId = StartSceneConfigCategory.Instance.GetBySceneName(unit.DomainZone(), Enum.GetName(SceneType.Chat)).InstanceId;
            Chat2M_UpdateLevel chat2G_EnterChat = (Chat2M_UpdateLevel)await MessageHelper.CallActor(chatServerId, new M2Chat_UpdateLevel()
            {
                UnitId = unit.Id,
                Level = level,
            });
        }

        //需要通知客户端
        public static void UpdateRoleData(this UserInfoComponent self, int Type, string value, bool notice = true)
        {
            Unit unit = self.GetParent<Unit>();
            string saveValue = "";
            long longValue = 0;
            switch (Type)
            {
                //case UserDataType.UnionExp:
                //    int addexp = int.Parse(value);
                //    self.SendUnionOperate(1, addexp).Coroutine();
                //    return;
                //case UserDataType.UnionGold:
                //    self.SendUnionOperate(5, int.Parse(value)).Coroutine();
                //    return;
                case UserDataType.JiaYuanExp:
                    self.UserInfo.JiaYuanExp += int.Parse(value);
                    saveValue = self.UserInfo.JiaYuanExp.ToString();
                    break;
                case UserDataType.JiaYuanFund:
                    self.UserInfo.JiaYuanFund += int.Parse(value);
                    saveValue = self.UserInfo.JiaYuanFund.ToString();
                    break;
                case UserDataType.UnionContri:
                    self.UserInfo.UnionZiJin += int.Parse(value);
                    saveValue = self.UserInfo.UnionZiJin.ToString();
                    break;
                case UserDataType.SeasonCoin:
                    self.UserInfo.SeasonCoin += int.Parse(value);
                    saveValue = self.UserInfo.SeasonCoin.ToString();
                    break;
                case UserDataType.SeasonExp:
                    self.UserInfo.SeasonExp += int.Parse(value);
                    SeasonLevelConfig seasonLevelConfig = SeasonLevelConfigCategory.Instance.Get(self.UserInfo.SeasonLevel);
                    if (self.UserInfo.SeasonExp >= seasonLevelConfig.UpExp && SeasonLevelConfigCategory.Instance.Contain(self.UserInfo.SeasonLevel + 1))
                    {
                        self.UserInfo.SeasonExp -= seasonLevelConfig.UpExp;
                        self.UpdateRoleData( UserDataType.SeasonLevel, "1" );
                    }
                    saveValue = self.UserInfo.SeasonExp.ToString();
                    longValue = self.UserInfo.SeasonExp;
                    break;
                case UserDataType.SeasonLevel:
                    self.UserInfo.SeasonLevel += int.Parse(value);
                    saveValue = self.UserInfo.SeasonLevel.ToString();
                    break;
                case UserDataType.JiaYuanLv:
                    self.UserInfo.JiaYuanLv += int.Parse(value);
                    saveValue = self.UserInfo.JiaYuanLv.ToString();
                    unit.GetComponent<TaskComponent>().TriggerTaskEvent(TaskTargetType.JiaYuanLevel_22, 0, self.UserInfo.JiaYuanLv - 10000);
                    unit.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.JiaYuanLevel_22, 0, self.UserInfo.JiaYuanLv - 10000);
                    unit.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.JiaYuanLevel_404, 0, self.UserInfo.JiaYuanLv - 10000);
                    break;
                case UserDataType.FangRong:
                    LingDiHelp.OnAddLingDiExp(unit, int.Parse(value), notice);
                    break;
                //名字应该在改名的协议处理
                case UserDataType.Name:
                    self.UserInfo.Name = value;
                    saveValue = self.UserInfo.Name;
                    break;
                case UserDataType.Exp:
                    self.Role_AddExp(long.Parse(value), notice);
                    //saveValue = self.UserInfo.Exp.ToString();
                    longValue = self.UserInfo.Exp;
                    break;
                case UserDataType.Lv:
                    self.UserInfo.Lv += int.Parse(value);
                    saveValue = self.UserInfo.Lv.ToString();
                    long maxHp = unit.GetComponent<NumericComponent>().GetAsLong((int)NumericType.Now_MaxHp);
                    unit.GetComponent<NumericComponent>().ApplyValue(NumericType.Now_Hp, maxHp, false);
                    unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.PointRemain, int.Parse(value) * 10, 0);
                    unit.GetComponent<TaskComponent>().OnUpdateLevel(self.UserInfo.Lv);
                    unit.GetComponent<ChengJiuComponent>().OnUpdateLevel(self.UserInfo.Lv);
                    unit.GetComponent<HeroDataComponent>().CheckSeasonOpen(true);
                    self.UpdateRoleData(UserDataType.Sp, value, notice);
                    self.BroadcastLevel(self.UserInfo.Lv).Coroutine();
                    Function_Fight.GetInstance().UnitUpdateProperty_Base(unit, true,true );
                    break;
                case UserDataType.Sp:
                    self.UserInfo.Sp += int.Parse(value);
                    saveValue = self.UserInfo.Sp.ToString();
                    break;
                case UserDataType.Gold:
                    self.UserInfo.Gold += long.Parse(value);
                    saveValue = self.UserInfo.Gold.ToString();
                    unit.GetComponent<ChengJiuComponent>().OnGetGold(int.Parse(value));
                    unit.GetComponent<TaskComponent>().OnCostCoin(int.Parse(value));
                    break;
                case UserDataType.WeiJingGold:
                    self.UserInfo.WeiJingGold += long.Parse(value);
                    saveValue = self.UserInfo.WeiJingGold.ToString();
                    break;
                case UserDataType.RongYu:
                    self.UserInfo.RongYu += long.Parse(value);
                    saveValue = self.UserInfo.RongYu.ToString();
                    break;
                case UserDataType.Diamond:
                    long addDiamond = long.Parse(value);
                    self.UserInfo.Diamond += addDiamond;
                    self.UserInfo.Diamond = Math.Max(self.UserInfo.Diamond, 0);
                    saveValue = self.UserInfo.Diamond.ToString();
                    if (addDiamond < 0)
                    {
                        unit.GetComponent<ChengJiuComponent>().OnCostDiamond(addDiamond);
                        unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.V1DayCostDiamond, addDiamond * - 1, 0);
                    }
                    break;
                case UserDataType.Occ:
                    break;
                case UserDataType.InvestMent:
                    unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.InvestMent, long.Parse(value), 0);
                    unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.InvestTotal, long.Parse(value), 0);
                    break;
                case UserDataType.JueXingExp:
                    unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.JueXingExp, long.Parse(value), 0);
                    break;
                case UserDataType.MaoXianExp:
                    unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.MaoXianExp, long.Parse(value), 0);
                    break;
                case UserDataType.Recharge:
                    RechargeHelp.SendDiamondToUnit(unit, int.Parse(value), "道具");
                    break;
                case UserDataType.PiLao:
                    if (value == "0")
                    {
                        return;
                    }
                    int maxValue = unit.IsYueKaStates() ? int.Parse(GlobalValueConfigCategory.Instance.Get(26).Value) : int.Parse(GlobalValueConfigCategory.Instance.Get(10).Value);
                    long newValue = long.Parse(value) + self.UserInfo.PiLao;
                    newValue = Math.Min(Math.Max(0, newValue), maxValue);
                    self.UserInfo.PiLao = newValue;
                    saveValue = self.UserInfo.PiLao.ToString();
                    break;
                case UserDataType.BaoShiDu:
                    long addValue = long.Parse(value);
                    newValue = self.UserInfo.BaoShiDu + (int)addValue;
                    newValue = Math.Min(Math.Max(0, newValue), ComHelp.GetMaxBaoShiDu());
                    self.UserInfo.BaoShiDu = (int)newValue;
                    saveValue = self.UserInfo.BaoShiDu.ToString();
                    unit.GetComponent<BuffManagerComponent>()?.InitBaoShiBuff();
                    break;
                case UserDataType.HuoYue:
                    break;
                case UserDataType.DungeonTimes:
                    unit.GetComponent<NumericComponent>().ApplyValue(NumericType.TeamDungeonTimes, unit.GetTeamDungeonTimes() - 1);
                    unit.GetComponent<NumericComponent>().ApplyValue(NumericType.TeamDungeonXieZhu, unit.GetTeamDungeonXieZhu() - 1);
                    self.UserInfo.DayFubenTimes.Clear();
                    break;
                case UserDataType.UnionName:
                    self.UserInfo.UnionName = value;
                    saveValue = self.UserInfo.UnionName;
                    break;
                case UserDataType.DemonName:
                    self.UserInfo.DemonName = value;
                    saveValue = self.UserInfo.DemonName;
                    break;
                case UserDataType.StallName:
                    self.UserInfo.StallName = value;
                    saveValue = self.UserInfo.StallName;
                    break;
                case UserDataType.Combat:
                    self.UserInfo.Combat = int.Parse(value);
                    saveValue = self.UserInfo.Combat.ToString();
                    unit.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.CombatToValue_211, 0, self.UserInfo.Combat);
                    unit.GetComponent<TaskComponent>().TriggerTaskEvent(TaskTargetType.CombatToValue_133, 0, self.UserInfo.Combat);
                    break;
                case UserDataType.Vitality:
                    NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
                    int skillNumber = 1 + numericComponent.GetAsInt(NumericType.MakeType_2) > 0 ? 1 : 0;
                    maxValue = unit.GetMaxHuoLi(skillNumber);
                    addValue = long.Parse(value);
                    newValue = self.UserInfo.Vitality + (int)addValue;
                    newValue = Math.Min(Math.Max(0, newValue), maxValue);
                    self.UserInfo.Vitality = (int)newValue;
                    saveValue = self.UserInfo.Vitality.ToString();
                    break;
                case UserDataType.BuffSkill:
                    longValue = long.Parse(value);
                    break;
                default:
                    saveValue = value;
                    break;
            }

            //发送更新值
            if (notice)
            {
                M2C_RoleDataUpdate m2C_RoleDataUpdate1 = self.m2C_RoleDataUpdate;
                m2C_RoleDataUpdate1.UpdateType = (int)Type;
                m2C_RoleDataUpdate1.UpdateTypeValue = saveValue;
                m2C_RoleDataUpdate1.UpdateValueLong = longValue;
                MessageHelper.SendToClient(self.GetParent<Unit>(), m2C_RoleDataUpdate1);
            }
        }

        public static async ETTask UploadCombat(this UserInfoComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            if (unit.IsRobot())
            {
                return;
            }

            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            long mapInstanceId = StartSceneConfigCategory.Instance.GetBySceneName(self.DomainZone(), Enum.GetName(SceneType.Rank)).InstanceId;
            RankingInfo rankPetInfo = new RankingInfo();
            UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
            rankPetInfo.UserId = userInfoComponent.UserInfo.UserId;
            rankPetInfo.PlayerName = userInfoComponent.UserInfo.Name;
            rankPetInfo.PlayerLv = userInfoComponent.UserInfo.Lv;
            rankPetInfo.Combat = userInfoComponent.UserInfo.Combat;
            rankPetInfo.Occ = userInfoComponent.UserInfo.Occ;
            R2M_RankUpdateResponse Response = (R2M_RankUpdateResponse)await ActorMessageSenderComponent.Instance.Call
                     (mapInstanceId, new M2R_RankUpdateRequest()
                     {
                         CampId = numericComponent.GetAsInt(NumericType.AcvitiyCamp),
                         RankingInfo = rankPetInfo
                     });
            if (unit.IsDisposed)
            {
                return;
            }
            numericComponent.ApplyValue(NumericType.CombatRankID, Response.RankId);
            numericComponent.ApplyValue(NumericType.OccCombatRankID, Response.OccRankId);
            numericComponent.ApplyValue(NumericType.PetTianTiRankID, Response.PetRankId);
            numericComponent.ApplyValue(NumericType.SoloRankId, Response.SoloRankId);
        }

        public static void  UpdateRankInfo(this UserInfoComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            if (unit.IsRobot())
            {
                return;
            }
            self.UpdateRankTime = TimeHelper.ServerNow();
        }

        //增加经验
        public static void Role_AddExp(this UserInfoComponent self, long addValue, bool notice)
        {
            Scene scene = self.DomainScene();
            ServerInfoComponent serverInfoComponent = scene.GetComponent<ServerInfoComponent>();
            if (serverInfoComponent == null)
            {
                Log.Warning($"ServerInfo==null: {scene.GetComponent<MapComponent>().SceneTypeEnum} {self.Id}");
                return;
            }
            if (serverInfoComponent.ServerInfo == null)
            {
                Log.Warning($"ServerInfo==null: {scene.GetComponent<MapComponent>().SceneTypeEnum} {self.Id}");
                return;
            }
            ServerInfo serverInfo = serverInfoComponent.ServerInfo;

            float expAdd = ComHelp.GetExpAdd(self.UserInfo.Lv, serverInfo);

            ExpConfig xiulianconf1 = ExpConfigCategory.Instance.Get(self.UserInfo.Lv);
            long upNeedExp = xiulianconf1.UpExp;

            //等级达到上限,则无法获得经验. 经验最多200%
            if (addValue > 0 &&self.UserInfo.Lv >= GlobalValueConfigCategory.Instance.MaxLevel)
            {
                long maxExp = upNeedExp * 2;
                if (self.UserInfo.Exp > maxExp) 
                {
                    self.UpdateRoleData(UserDataType.Message, "当前经验超过200%，请前往主城经验老头处用多余的经验兑换奖励喔!");
                    return;
                }
            }

            self.UserInfo.Exp = self.UserInfo.Exp + (int)(addValue * (1.0f + expAdd));

            //判定是否升级
            if (self.UserInfo.Lv >= serverInfo.WorldLv)
            {
                return;
            }

            if (self.UserInfo.Exp >= upNeedExp)
            {
                self.UserInfo.Exp -= upNeedExp;
                self.UpdateRoleData(UserDataType.Lv, "1", notice);
            }
        }

        public static int GetRandomMonsterId(this UserInfoComponent self)
        {
            List<KeyValuePairInt> dayMonster = self.UserInfo.DayMonsters;
            List<DayMonsters> dayMonsterConfig = GlobalValueConfigCategory.Instance.DayMonsterList;

            for (int i = 0; i < dayMonsterConfig.Count; i++)
            {
                if (RandomHelper.RandFloat01() > dayMonsterConfig[i].GaiLv)
                {
                    continue;
                }

                KeyValuePairInt keyValuePairInt = null;
                for (int d = 0; d < dayMonster.Count; d++)
                {
                    if (dayMonster[d].KeyId != dayMonsterConfig[i].MonsterId)
                    {
                        continue;
                    }
                    keyValuePairInt = dayMonster[d];
                }
                if (keyValuePairInt == null)
                {
                    keyValuePairInt = new KeyValuePairInt() { KeyId = dayMonsterConfig[i].MonsterId, Value = 0 };
                    dayMonster.Add(keyValuePairInt);
                }
                if (keyValuePairInt.Value < dayMonsterConfig[i].TotalNumber)
                {
                    keyValuePairInt.Value++;
                    return dayMonsterConfig[i].MonsterId;
                }
            }

            return 0;
        }

        public static int GetRandomJingLingId(this UserInfoComponent self)
        {
            List<DayJingLing> dayMonsterConfig = GlobalValueConfigCategory.Instance.DayJingLingList;
            List<int> dayMonster = self.UserInfo.DayJingLing;
            for(int i = 0; i < dayMonsterConfig.Count; i++)
            {
                if (RandomHelper.RandFloat01() > dayMonsterConfig[i].GaiLv)
                {
                    continue;
                }
                if (dayMonster.Count <= i)
                {
                    for (int d = dayMonster.Count; d < i+1; d++)
                    {
                        dayMonster.Add(0);
                    }
                }
                if (dayMonster[i] >= dayMonsterConfig[i].TotalNumber)
                {
                    continue;
                }

                dayMonster[i]++;
                int randomIndex = RandomHelper.RandomByWeight(dayMonsterConfig[i].Weights);
                return dayMonsterConfig[i].MonsterId[randomIndex];
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

        public static void OnAddChests(this UserInfoComponent self, int fubenId, int monsterId)
        {
            bool have = false;
            List<KeyValuePair> chestList = self.UserInfo.OpenChestList;
            for (int i = 0; i < chestList.Count; i++)
            {
                if (chestList[i].KeyId == fubenId)
                {
                    chestList[i].Value += ($"_{monsterId}");
                    have = true;
                }
            }
            if (!have)
            {
                self.UserInfo.OpenChestList.Add(new KeyValuePair() { KeyId = fubenId, Value = monsterId.ToString() });
            }
        }

        public static bool IsCheskOpen(this UserInfoComponent self, int fubenId, int monsterId)
        {
            List<KeyValuePair> chestList = self.UserInfo.OpenChestList;
            for (int i = 0; i < chestList.Count; i++)
            {
                if (chestList[i].KeyId == fubenId)
                {
                    return chestList[i].Value.Contains(monsterId.ToString());
                }
            }
            return false;
        }

        public static int OnGetFirstWinSelf(this UserInfoComponent self, int firstwinid, int difficulty)
        {
            KeyValuePair keyValuePair1 = null;
            for (int i = 0; i < self.UserInfo.FirstWinSelf.Count; i++)
            {
                if (self.UserInfo.FirstWinSelf[i].KeyId != firstwinid)
                {
                    continue;
                }
                keyValuePair1 = self.UserInfo.FirstWinSelf[i];
                break;
            }
            if (keyValuePair1 == null)
            {
                return ErrorCode.ERR_NetWorkError;
            }
            if (keyValuePair1.Value2.Contains(difficulty.ToString()))
            {
                return ErrorCode.ERR_AlreadyReceived;
            }
            if (string.IsNullOrEmpty(keyValuePair1.Value2))
            {
                keyValuePair1.Value2 = difficulty.ToString();
            }
            else
            {
                keyValuePair1.Value2 += $"_{difficulty}";
            }
            return ErrorCode.ERR_Success;
        }

        public static void OnAddFirstWinSelf(this UserInfoComponent self, Unit boss, int difficulty)
        {
            if (difficulty == 0)
            {
                difficulty = 1;
            }
            int firstwinid = FirstWinHelper.GetFirstWinID(boss.ConfigId, difficulty);
            if (firstwinid == 0)
            {
                return;
            }

            bool have = false;
            for (int i = 0; i < self.UserInfo.FirstWinSelf.Count; i++)
            {
                KeyValuePair keyValuePair = self.UserInfo.FirstWinSelf[i];
                if (keyValuePair.KeyId != firstwinid)
                {
                    continue;
                }
                //keyValuePair.Value  击杀难度
                //keyValuePair.Value2 领取难度
                if (keyValuePair.Value.Contains(difficulty.ToString()))
                {
                    return;
                }

                keyValuePair.Value += $"_{difficulty}";
                have = true;
                break;
            }
            if (!have)
            {
                self.UserInfo.FirstWinSelf.Add( new KeyValuePair() {  KeyId = firstwinid, Value = difficulty.ToString(), Value2 = "" } );
            }

            M2C_FirstWinSelfUpdateMessage m2C_FirstWinSelfUpdateMessage = new M2C_FirstWinSelfUpdateMessage() { FirstWinInfos = self.UserInfo.FirstWinSelf  };
            MessageHelper.SendToClient( self.GetParent<Unit>(), m2C_FirstWinSelfUpdateMessage);
        }

        public static void OnCleanBossCD(this UserInfoComponent self)
        {
            for (int i = 0; i < self.UserInfo.MonsterRevives.Count; i++)
            {
                self.UserInfo.MonsterRevives[i].Value = "0";
            }
        }

        public static void OnAddRevive(this UserInfoComponent self, int monsterId, long reviveTime)
        {
            bool have = false;  
            for (int i = 0; i < self.UserInfo.MonsterRevives.Count; i++)
            {
                KeyValuePair keyValuePair = self.UserInfo.MonsterRevives[i];
                if (keyValuePair.KeyId != monsterId)
                {
                    continue;
                }
                if (string.IsNullOrEmpty(keyValuePair.Value2))
                {
                    keyValuePair.Value2 = "1";
                }

                keyValuePair.Value = reviveTime.ToString();
                keyValuePair.Value2 = (int.Parse(keyValuePair.Value2) + 1).ToString();  
                have = true;
                break;
            }
            if (!have)
            {
                self.UserInfo.MonsterRevives.Add(new KeyValuePair() { KeyId = monsterId, Value = reviveTime.ToString(), Value2 = "1" });
            }

            M2C_UpdateUserInfoMessage m2C_UpdateUserInfo = new M2C_UpdateUserInfoMessage();
            m2C_UpdateUserInfo.UserInfo = self.UserInfo;
            MessageHelper.SendToClient( self.GetParent<Unit>(), m2C_UpdateUserInfo );
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
                // 0 固定 1移动
                case GameSettingEnum.YanGan:
                    return "0";
                case GameSettingEnum.FenBianlLv:
                    return "1";
                default:
                    return "0";
            }
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

        public static int GetCrateDay(this UserInfoComponent self)
        {
            return  ServerHelper.DateDiff_Time(TimeHelper.ServerNow(), self.UserInfo.CreateTime);
        }

        /// <summary>
        /// 高级是1  中级是2
        /// </summary>
        /// <param name="self"></param>
        /// <param name="level"></param>
        public static void OnGmGaoJi(this UserInfoComponent self, int level)
        {
            int lv = level == 1 ? 70 - self.UserInfo.Lv : 40 - self.UserInfo.Lv;
            self.UpdateRoleData(UserDataType.Lv, lv.ToString());

            self.UserInfo.HorseIds.Clear();
            Dictionary<int, ZuoQiShowConfig> allzuoqi = ZuoQiShowConfigCategory.Instance.GetAll();
            foreach (( int zuoqiid, ZuoQiShowConfig zuoQiShowConfig ) in allzuoqi)
            {
                self.UserInfo.HorseIds.Add(zuoqiid);
            }
            self.GetParent<Unit>().GetComponent<NumericComponent>().ApplyValue(NumericType.HorseRide, self.UserInfo.HorseIds[0]);
            self.GetParent<Unit>().GetComponent<NumericComponent>().ApplyValue(NumericType.HorseFightID, self.UserInfo.HorseIds[0]);

            JiaYuanConfig maxjiayuan = null;
            Dictionary<int, JiaYuanConfig> allJiayuan = JiaYuanConfigCategory.Instance.GetAll();
            foreach ((int jiayualv, JiaYuanConfig jiaYuanConfig) in allJiayuan)
            {
                maxjiayuan = jiaYuanConfig;
            }
            self.UserInfo.JiaYuanLv = maxjiayuan.Id;


            SeasonLevelConfig maxseason = null;
            Dictionary<int, SeasonLevelConfig> allseason = SeasonLevelConfigCategory.Instance.GetAll(); 
            foreach ((int seasonid, SeasonLevelConfig seasonLevelConfig) in allseason )
            {
                maxseason = seasonLevelConfig;
            }
            self.UserInfo.SeasonLevel = maxseason.Id;

            if (self.UserInfo.OccTwo == 0)
            {
                OccupationConfig occupationConfig = OccupationConfigCategory.Instance.Get(self.UserInfo.Occ);
                self.UserInfo.OccTwo = occupationConfig.OccTwoID[ RandomHelper.RandomNumber(0, occupationConfig.OccTwoID.Length) ];
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
            self.UserInfo.PetExploreRewardIds.Clear();  
            self.UserInfo.PetHeXinExploreRewardIds.Clear();
            self.UserInfo.ItemXiLianNumRewardIds.Clear();
        }

    }

}
