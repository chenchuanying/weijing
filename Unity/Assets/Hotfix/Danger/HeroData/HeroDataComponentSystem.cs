using System;
using System.Collections.Generic;

namespace ET
{

    [ObjectSystem]
    public class HeroDataComponentAwakeSystem : AwakeSystem<HeroDataComponent>
    {
        public override void Awake(HeroDataComponent self)
        {

        }
    }

    /// <summary>
    /// 英雄数据组件，负责管理英雄数据
    /// </summary>
    public static class HeroDataComponentSystem
    {

#if SERVER
        public static void CheckNumeric(this HeroDataComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            //重置所有属性
            long max = (int)NumericType.Max;
            foreach (int key in numericComponent.NumericDic.Keys)
            {
                //这个范围内的属性为特殊属性不进行重置
                if (key < max)
                {
                    continue;
                }
                numericComponent.NumericDic[key] = 0;
            }

            if (numericComponent.GetAsInt(NumericType.Ling_DiLv) == 0)
            {
                numericComponent.Set(NumericType.Ling_DiLv, 1, false);
            }
            if (numericComponent.GetAsInt(NumericType.CangKuNumber) == 0)
            {
                numericComponent.Set(NumericType.CangKuNumber, 1, false);
            }
            if (numericComponent.GetAsInt(NumericType.JianYuanCangKu) == 0)
            {
                numericComponent.Set(NumericType.JianYuanCangKu, 1, false);
            }


            if (numericComponent.GetAsFloat(NumericType.ChouKaTenTime) == 0)
            {
                numericComponent.Set(NumericType.ChouKaTenTime, TimeHelper.ServerNow());
            }
            if (numericComponent.GetAsFloat(NumericType.ChouKaOneTime) == 0)
            {
                numericComponent.Set(NumericType.ChouKaOneTime, TimeHelper.ServerNow());
            }
            if (numericComponent.GetAsInt(NumericType.HorseRide) == 1)
            {
                numericComponent.Set(NumericType.HorseRide, numericComponent.GetAsInt(NumericType.HorseFightID));
            }
            if (numericComponent.GetAsInt(NumericType.UnionXiuLian_0) == 0)
            {
                Dictionary<int, List<UnionQiangHuaConfig>> keyValuePairs = UnionQiangHuaConfigCategory.Instance.UnionQiangHuaList;
                numericComponent.Set(NumericType.UnionXiuLian_0, keyValuePairs[0][0].Id);
                numericComponent.Set(NumericType.UnionXiuLian_1, keyValuePairs[1][0].Id);
                numericComponent.Set(NumericType.UnionXiuLian_2, keyValuePairs[2][0].Id);
                numericComponent.Set(NumericType.UnionXiuLian_3, keyValuePairs[3][0].Id);
            }
            if (numericComponent.GetAsInt(NumericType.UnionPetXiuLian_0) == 0)
            {
                Dictionary<int, List<UnionQiangHuaConfig>> keyValuePairs = UnionQiangHuaConfigCategory.Instance.UnionQiangHuaList;
                numericComponent.Set(NumericType.UnionPetXiuLian_0, keyValuePairs[4][0].Id);
                numericComponent.Set(NumericType.UnionPetXiuLian_1, keyValuePairs[5][0].Id);
                numericComponent.Set(NumericType.UnionPetXiuLian_2, keyValuePairs[6][0].Id);
                numericComponent.Set(NumericType.UnionPetXiuLian_3, keyValuePairs[7][0].Id);
            }
            if (numericComponent.GetAsInt(NumericType.Bloodstone) == 0)
            {
                numericComponent.Set(NumericType.Bloodstone, 10100);
            }
            if (numericComponent.GetAsInt(NumericType.UnionAttribute_1) == 0)
            {
                numericComponent.Set(NumericType.UnionAttribute_1, 20100);
            }
            if (numericComponent.GetAsInt(NumericType.UnionAttribute_2) == 0)
            {
                numericComponent.Set(NumericType.UnionAttribute_2, 30100);
            }

            UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
            int PointLiLiang = numericComponent.GetAsInt(NumericType.PointLiLiang);
            int PointZhiLi = numericComponent.GetAsInt(NumericType.PointZhiLi);
            int PointTiZhi = numericComponent.GetAsInt(NumericType.PointTiZhi);
            int PointNaiLi = numericComponent.GetAsInt(NumericType.PointNaiLi);
            int PointMinJie = numericComponent.GetAsInt(NumericType.PointMinJie);
            int PointRemain = numericComponent.GetAsInt(NumericType.PointRemain);
            int totalPoint = (userInfoComponent.UserInfo.Lv - 1) * 10;
            
            //检测属性点
            if (unit.IsRobot())
            {
                //机器人属性点
            }
            else
            {
                long addvalue = PointLiLiang + PointZhiLi + PointTiZhi + PointNaiLi + PointMinJie + PointRemain;
                if (addvalue != totalPoint || addvalue > totalPoint || PointLiLiang > totalPoint || PointZhiLi > totalPoint
                    || PointTiZhi > totalPoint || PointNaiLi > totalPoint || PointMinJie > totalPoint
                    || PointLiLiang < 0 || PointZhiLi < 0 || PointTiZhi < 0 || PointNaiLi < 0 || PointMinJie < 0)
                {

                    Log.Debug($"{PointLiLiang} {PointZhiLi} {PointTiZhi} {PointNaiLi} {PointMinJie}  {PointRemain}  totalPoint: {totalPoint}");
                    numericComponent.Set(NumericType.PointLiLiang, 0);
                    numericComponent.Set(NumericType.PointZhiLi, 0);
                    numericComponent.Set(NumericType.PointTiZhi, 0);
                    numericComponent.Set(NumericType.PointNaiLi, 0);
                    numericComponent.Set(NumericType.PointMinJie, 0);
                    numericComponent.Set(NumericType.PointRemain, totalPoint);
                }
            }
        }

        public static void SendSeasonReward(this HeroDataComponent self, int seasonLevel)
        {
            string rewardItem = SeasonHelper.GetSeasonOverReward(seasonLevel);
            if (string.IsNullOrEmpty(rewardItem))
            {
                return;
            }

            MailInfo mailInfo = new MailInfo();
            mailInfo.Status = 0;
            mailInfo.Context = "赛季结束奖励";
            mailInfo.Title = "赛季结束奖励";
            mailInfo.MailId = IdGenerater.Instance.GenerateId();
            mailInfo.ItemList.AddRange(ItemHelper.GetRewardItems_2(rewardItem));
            MailHelp.SendUserMail( self.DomainZone(), self.Id, mailInfo ).Coroutine();
        }

        public static void OnLogin(this HeroDataComponent self, int robotId)
        {
            Unit unit = self.GetParent<Unit>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            numericComponent.Set((int)NumericType.Now_Dead , 0, false);
            numericComponent.Set((int)NumericType.Now_Damage, 0, false);
            numericComponent.Set((int)NumericType.Now_Stall, 0, false);
            numericComponent.Set((int)NumericType.TeamId, 0, false);
            numericComponent.Set((int)NumericType.Now_Hp, numericComponent.GetAsLong((int)NumericType.Now_MaxHp), false);
            numericComponent.Set((int)NumericType.Now_Weapon, unit.GetComponent<BagComponent>().GetWuqiItemId(), false);
            numericComponent.Set(NumericType.JueXingAnger, 0, false);
            numericComponent.Set(NumericType.RunRaceRankId, 0, false);
            numericComponent.Set(NumericType.ZeroClock, 0, false);

            if (robotId != 0)
            {
                RobotConfig robotConfig = RobotConfigCategory.Instance.Get(robotId);
                numericComponent.ApplyValue(NumericType.PointLiLiang, robotConfig.PointList[0], false);
                numericComponent.ApplyValue(NumericType.PointZhiLi, robotConfig.PointList[1], false);
                numericComponent.ApplyValue(NumericType.PointTiZhi, robotConfig.PointList[2], false);
                numericComponent.ApplyValue(NumericType.PointNaiLi, robotConfig.PointList[3], false);
                numericComponent.ApplyValue(NumericType.PointMinJie, robotConfig.PointList[4], false);
                numericComponent.ApplyValue(NumericType.CombatRankID, 0, false);
                numericComponent.ApplyValue(NumericType.OccCombatRankID, 0, false);
            }

            if (numericComponent.GetAsInt(NumericType.CostTiLi) > 600)
            {
                UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
                Log.Warning($"体力消耗异常: {self.DomainZone()}  {userInfoComponent.UserInfo.Name} {numericComponent.GetAsInt(NumericType.CostTiLi)}");
            }

            if (numericComponent.UpdateNumber < 1)
            {
                numericComponent.UpdateNumber = 1;
                numericComponent.ApplyValue(NumericType.PetExploreLuckly, 100, false);
            }

            if (unit.Type == UnitType.Player && numericComponent.GetAsInt(NumericType.PetExtendNumber) > 0)
            {
                if (unit.GetComponent<UserInfoComponent>().GetTotalUseTimes(10000134) <= 0)
                {
                    unit.GetComponent<UserInfoComponent>().OnTotalUseTimes(10000134, numericComponent.GetAsInt(NumericType.PetExtendNumber));
                }
            }
           
            if (numericComponent.GetAsInt(NumericType.SkillMakePlan2) == 0)
            {
                numericComponent.ApplyValue(NumericType.MakeType_2, 0, false);
            }

            //月卡次数用完，则清空标志
            int yuekatimes = numericComponent.GetAsInt(NumericType.YueKaRemainTimes);
            if (yuekatimes > 0)
            {
                numericComponent.ApplyValue(NumericType.YueKaEndTime, yuekatimes, false);
            }

            self.CheckSeasonOver(false);
            self.CheckSeasonOpen(false);
        }

        public static void CheckSeasonOver(this HeroDataComponent self, bool notice)
        {
            ///赛季数据[赛季开始]
            Unit unit = self.GetParent<Unit>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
            long seasonopenTime = numericComponent.GetAsLong(NumericType.SeasonOpenTime);
            KeyValuePairLong keyValuePairLong = SeasonHelper.GetOpenSeason(userInfoComponent.UserInfo.Lv);

            if (seasonopenTime != 0 &&  (keyValuePairLong== null  || seasonopenTime != keyValuePairLong.Value) )
            {
                //清空赛季相关数据. 赛季任务 晶核
                Log.Warning($"清空赛季数据！:{unit.Id}");
                Console.WriteLine($"清空赛季数据！: {unit.DomainZone()}  {unit.Id}  {seasonopenTime} ");
                self.SendSeasonReward(unit.GetComponent<UserInfoComponent>().UserInfo.SeasonLevel);

                numericComponent.ApplyValue(NumericType.SeasonOpenTime, 0, notice);
                numericComponent.ApplyValue(NumericType.SeasonReward, 0, notice);
                numericComponent.ApplyValue(NumericType.SeasonBossFuben, 0, notice);
                numericComponent.ApplyValue(NumericType.SeasonBossRefreshTime, 0, notice);
                numericComponent.ApplyValue(NumericType.SeasonTowerId, 0, notice);
                numericComponent.ApplyValue(NumericType.SeasonTask, 0, notice);

                unit.GetComponent<UserInfoComponent>().OnResetSeason(notice);
                unit.GetComponent<BagComponent>().OnResetSeason(notice);
                unit.GetComponent<TaskComponent>().OnResetSeason(notice);
            }
        }

        public static void CheckSeasonOpen(this HeroDataComponent self, bool notice)
        {
            Unit unit = self.GetParent<Unit>();
            UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

            if (numericComponent.GetAsInt(NumericType.SeasonBossFuben) >= 100000)
            {
                numericComponent.ApplyValue(NumericType.SeasonBossFuben, SeasonHelper.GetFubenId(userInfoComponent.UserInfo.Lv));
            }

            if (numericComponent.GetAsInt(NumericType.SeasonBossFuben) >= ConfigHelper.GMDungeonId)
            {
                numericComponent.ApplyValue(NumericType.SeasonBossFuben, SeasonHelper.GetFubenId(userInfoComponent.UserInfo.Lv));
            }

            KeyValuePairLong seasonOpenTime = SeasonHelper.GetOpenSeason(userInfoComponent.UserInfo.Lv);
            if (numericComponent.GetAsLong(NumericType.SeasonOpenTime) == 0 && seasonOpenTime != null)
            {
                //Console.WriteLine($"赛季开启: {unit.DomainZone()}  {unit.Id}  {seasonOpenTime.KeyId}");

                //刷新boss
                numericComponent.ApplyValue(NumericType.SeasonBossFuben, SeasonHelper.GetFubenId(userInfoComponent.UserInfo.Lv), notice);
                numericComponent.ApplyValue(NumericType.SeasonBossRefreshTime, TimeHelper.ServerNow() + TimeHelper.Minute, notice);
                numericComponent.ApplyValue(NumericType.SeasonOpenTime, seasonOpenTime.Value, notice);

                //刷新任务
                TaskComponent taskComponent = unit.GetComponent<TaskComponent>();
                taskComponent.InitSeasonMainTask(notice);
                taskComponent.UpdateSeasonWeekTask(notice); 
            }
        }

        /// <summary>
        /// 重置。隔天登录或者零点刷新
        /// </summary>
        /// <param name="self"></param>
        /// <param name="notice"></param>
        public static void OnZeroClockUpdate(this HeroDataComponent self, bool notice = false)
        {
            Unit unit = self.GetParent<Unit>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

            numericComponent.ApplyValue(NumericType.HongBao, 0, notice);
            numericComponent.ApplyValue(NumericType.Now_XiLian, 0, notice);
            numericComponent.ApplyValue(NumericType.PetChouKa, 0, notice);
            numericComponent.ApplyValue(NumericType.YueKaAward, 0, notice);
            numericComponent.ApplyValue(NumericType.XiuLian_ExpNumber, 0, notice);
            numericComponent.ApplyValue(NumericType.XiuLian_CoinNumber, 0, notice);
            numericComponent.ApplyValue(NumericType.XiuLian_ExpTime, 0, notice);
            numericComponent.ApplyValue(NumericType.XiuLian_CoinTime, 0, notice);
            numericComponent.ApplyValue(NumericType.TiLiKillNumber, 0, notice);
            numericComponent.ApplyValue(NumericType.ChouKa, 0, notice);
            numericComponent.ApplyValue(NumericType.ExpToGoldTimes, 0, notice);
            numericComponent.ApplyValue(NumericType.RechargeSign, 0, notice);
            numericComponent.ApplyValue(NumericType.TeamDungeonTimes, 0, notice);
            numericComponent.ApplyValue(NumericType.TeamDungeonXieZhu, 0, notice);
            numericComponent.ApplyValue(NumericType.BattleTodayKill, 0, notice);
            numericComponent.ApplyValue(NumericType.FubenTimesReset, 0, notice);
            numericComponent.ApplyValue(NumericType.FenShangSet, 0, notice);
            numericComponent.ApplyValue(NumericType.ArenaNumber, 0, notice);
            numericComponent.ApplyValue(NumericType.LocalDungeonTime, 0, notice);
            numericComponent.ApplyValue(NumericType.TreasureTask, 0, notice);
            numericComponent.ApplyValue(NumericType.JiaYuanExchangeZiJin, 0, notice);
            numericComponent.ApplyValue(NumericType.JiaYuanExchangeExp, 0, notice);
            numericComponent.ApplyValue(NumericType.JiaYuanVisitRefresh, 0, notice);
            numericComponent.ApplyValue(NumericType.JiaYuanGatherOther, 0, notice);
            numericComponent.ApplyValue(NumericType.JiaYuanPickOther, 0, notice);
            numericComponent.ApplyValue(NumericType.UnionDonationNumber, 0, notice);
            numericComponent.ApplyValue(NumericType.UnionDiamondDonationNumber, 0, notice);
            numericComponent.ApplyValue(NumericType.RaceDonationNumber, 0, notice);
            // 重置封印之塔数据
            numericComponent.ApplyValue(NumericType.JiaYuanPurchaseRefresh, 0, notice);
            numericComponent.ApplyValue(NumericType.TowerOfSealArrived, 0, notice);
            numericComponent.ApplyValue(NumericType.TowerOfSealFinished, 0, notice);

            numericComponent.ApplyValue(NumericType.RunRaceRankId, 0, notice);
            numericComponent.ApplyValue(NumericType.HappyCellIndex, 0, notice);
            numericComponent.ApplyValue(NumericType.HappyMoveNumber, 0, notice);

            numericComponent.ApplyValue(NumericType.PetMineBattle, 0, notice);
            numericComponent.ApplyValue(NumericType.PetMineLogin, 0, notice);

            numericComponent.ApplyValue(NumericType.CostTiLi, 0, notice);
            numericComponent.ApplyValue(NumericType.DrawIndex, 0, notice);
            numericComponent.ApplyValue(NumericType.DrawReward, 0, notice);

            numericComponent.ApplyValue(NumericType.PetMineReset, 0, notice);

            numericComponent.ApplyValue(NumericType.V1DayCostDiamond, 0, notice);
            numericComponent.ApplyValue(NumericType.V1ChouKaNumber, 0, notice);
            numericComponent.ApplyValue(NumericType.V1RechageNumber, 0, notice);
            numericComponent.ApplyValue(NumericType.PetExploreNumber, 0, notice);
            numericComponent.ApplyValue(NumericType.PetHeXinExploreNumber, 0, notice);
            numericComponent.ApplyValue(NumericType.ItemXiLianNumber, 0, notice);
            numericComponent.ApplyValue(NumericType.PaiMaiTodayGold, 0, notice);

            //月卡次数用完，则清空标志
            int yuekatimes = numericComponent.GetAsInt(NumericType.YueKaRemainTimes);
            numericComponent.ApplyValue(NumericType.YueKaEndTime, yuekatimes, notice);

            int lirun =  (int)(numericComponent.GetAsInt(NumericType.InvestTotal) * 0.25f);
            numericComponent.ApplyValue(NumericType.InvestTotal, numericComponent.GetAsInt(NumericType.InvestTotal) + lirun, notice);

            self.CheckSeasonOver(notice);
            self.CheckSeasonOpen(notice);   
        }

        /// <summary>
        /// 返回主城
        /// </summary>
        /// <param name="self"></param>
        public static void OnReturn(this HeroDataComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            numericComponent.NumericDic[NumericType.Now_Dead] = 0;
            numericComponent.NumericDic[NumericType.Now_Damage] = 0;
            numericComponent.NumericDic[NumericType.BossBelongID] = 0;
            numericComponent.NumericDic[NumericType.Now_Shield_HP] = 0;
            numericComponent.NumericDic[NumericType.Now_Shield_MaxHP] = 0;
            numericComponent.NumericDic[NumericType.Now_Shield_DamgeCostPro] = 0;
            if (unit.GetComponent<NumericComponent>().GetAsLong(NumericType.Now_Dead) <= 0)
            {
                long max_hp = self.Parent.GetComponent<NumericComponent>().GetAsLong(NumericType.Now_MaxHp);
                unit.GetComponent<NumericComponent>().NumericDic[NumericType.Now_Hp] = max_hp;
            }
        }

        public static void OnResetPoint(this HeroDataComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
            numericComponent.ApplyValue(NumericType.PointLiLiang, 0);
            numericComponent.ApplyValue(NumericType.PointZhiLi, 0);
            numericComponent.ApplyValue(NumericType.PointTiZhi, 0);
            numericComponent.ApplyValue(NumericType.PointNaiLi, 0);
            numericComponent.ApplyValue(NumericType.PointMinJie,0);
            numericComponent.ApplyValue(NumericType.PointRemain, (userInfoComponent.UserInfo.Lv - 1) * 10);
            Function_Fight.GetInstance().UnitUpdateProperty_Base(unit, true, true); ;
        }

        /// <summary>
        /// 0 不复活 1等待复活
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int OnWaitRevive(this HeroDataComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            if (unit.Type != UnitType.Monster)
            {
                return 0;
            }

            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(unit.ConfigId);
            int resurrection = (int)monsterConfig.ReviveTime;
            MapComponent mapComponent = unit.DomainScene().GetComponent<MapComponent>();
            if (SeasonHelper.SeasonBossId == unit.ConfigId && mapComponent.SceneTypeEnum == (int)SceneTypeEnum.LocalDungeon)
            {
                LocalDungeonComponent localDungeon = unit.DomainScene().GetComponent<LocalDungeonComponent>();
                UserInfoComponent userInfoComponent = localDungeon.MainUnit.GetComponent<UserInfoComponent>();
                localDungeon.MainUnit.GetComponent<NumericComponent>().ApplyValue(NumericType.SeasonBossFuben, SeasonHelper.GetFubenId(userInfoComponent.UserInfo.Lv));
                localDungeon.MainUnit.GetComponent<NumericComponent>().ApplyValue(NumericType.SeasonBossRefreshTime, TimeHelper.ServerNow() + resurrection * 1000);
                resurrection = 0;
            }
            if (resurrection == 0)
            {
                return 0;
            }

            if (self.DomainZone() == 128)
            {
                resurrection = 10;
            }
         
            if (monsterConfig.MonsterType != (int)MonsterTypeEnum.Boss)
            {
                //unit.DomainScene().GetComponent<YeWaiRefreshComponent>().OnAddRefreshList(unit, resurrection * 1000);
                if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.LocalDungeon
                 || mapComponent.SceneTypeEnum == (int)SceneTypeEnum.MiJing
                 || mapComponent.SceneTypeEnum == (int)SceneTypeEnum.RunRace)
                {
                    unit.DomainScene().GetComponent<YeWaiRefreshComponent>().OnAddRefreshList(unit, resurrection * 1000);
                }
                return 0;
            }
            else
            {
                long resurrectionTime = 0;
                if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.LocalDungeon)
                {
                    LocalDungeonComponent localDungeon = unit.DomainScene().GetComponent<LocalDungeonComponent>();
                    UserInfoComponent userInfoComponent = localDungeon.MainUnit.GetComponent<UserInfoComponent>();
                    int killNumber = userInfoComponent.GetMonsterKillNumber(unit.ConfigId)  +  1;
                    int chpaterid = DungeonConfigCategory.Instance.GetChapterByDungeon(mapComponent.SceneId);
                    BossDevelopment bossDevelopment = ConfigHelper.GetBossDevelopmentByKill(chpaterid , killNumber);
                    resurrection = (int)(resurrection * bossDevelopment.ReviveTimeAdd);

                    resurrectionTime = TimeHelper.ServerNow() + resurrection * 1000;
                    unit.GetComponent<NumericComponent>().ApplyValue(NumericType.ReviveTime, resurrectionTime);
                    userInfoComponent.OnAddRevive(unit.ConfigId, resurrectionTime);
                    unit.RemoveComponent<ReviveTimeComponent>();
                    unit.AddComponent<ReviveTimeComponent, long>(resurrectionTime);

                    userInfoComponent.OnAddFirstWinSelf(unit, localDungeon.FubenDifficulty);
                    FirstWinHelper.SendFirstWinInfo(localDungeon.MainUnit, unit, localDungeon.FubenDifficulty);
                    return 1;
                }
                else
                {
                    if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.MiJing)
                    {
                        resurrectionTime = TimeHelper.ServerNow() + resurrection * 1000;
                        unit.GetComponent<NumericComponent>().ApplyValue(NumericType.ReviveTime, resurrectionTime);

                        unit.RemoveComponent<ReviveTimeComponent>();
                        unit.AddComponent<ReviveTimeComponent, long>(resurrectionTime);
                        return 1;
                    }
                }
                return 0;
            }
        }

        public static void OnKillZhaoHuan(this HeroDataComponent self, Unit attack)
        {
            Unit unit = self.GetParent<Unit>();
            UnitInfoComponent unitInfoComponent = unit.GetComponent<UnitInfoComponent>();
            if (unitInfoComponent == null)
            {
                Log.Debug($"unitInfoComponent == null  {unit.Type } {unit.IsDisposed}");
                return;
            }
            for (int i = unitInfoComponent.ZhaohuanIds.Count - 1; i >= 0; i--)
            {
                Unit zhaohuan = unit.GetParent<UnitComponent>().Get(unitInfoComponent.ZhaohuanIds[i]);
                if (zhaohuan == null)
                {
                    continue;
                }
                //zhaohuan.GetComponent<SkillPassiveComponent>()?.Stop();
                //zhaohuan.GetComponent<NumericComponent>().ApplyChange(args.Attack, NumericType.Now_Hp, -1000000, args.SkillId);
                zhaohuan.GetComponent<HeroDataComponent>().OnDead(attack!=null ? attack : zhaohuan);
            }
            unitInfoComponent.ZhaohuanIds.Clear();
        }

        public static void PlayDeathSkill(this HeroDataComponent self,Unit attack)
        {
            Unit unit = self.GetParent<Unit>();
            if (unit.Type == UnitType.Monster)
            {
                if (unit.ConfigId == 90000202)   //90030005
                {
                    Log.Warning("PlayDeathSkill: 72009045");
                }

                MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(unit.ConfigId);
                if (monsterConfig.DeathSkillId != 0)
                {
                    unit.GetComponent<SkillManagerComponent>().OnUseSkill(new C2M_SkillCmd()
                    {
                        SkillID = monsterConfig.DeathSkillId,
                    }, false);
                }
            }
            if (unit.Type == UnitType.Pet )
            {
                unit.GetComponent<SkillPassiveComponent>()?.OnTrigegerPassiveSkill(SkillPassiveTypeEnum.WillDead_6, attack.Id);
            }
        }

        public static void OnRevive(this HeroDataComponent self, bool bornPostion = false)
        {
            Unit unit = self.GetParent<Unit>();
            NumericComponent numericComponent  = unit.GetComponent<NumericComponent>();
            long max_hp = numericComponent.GetAsLong(NumericType.Now_MaxHp);

            numericComponent.ApplyValue(NumericType.Now_Dead, 0);
            numericComponent.NumericDic[NumericType.Now_Hp] = 0;
            numericComponent.ApplyChange(null, NumericType.Now_Hp, max_hp, 0);
            numericComponent.ApplyValue(NumericType.ReviveTime, 0);
            unit.GetComponent<SkillPassiveComponent>()?.Activeted();
            unit.GetComponent<BuffManagerComponent>()?.OnRevive();
            unit.Position = unit.GetBornPostion();
            if (unit.Type == UnitType.Monster)
            {
                unit.GetComponent<AIComponent>().Begin();
            }
        }

        public static void InitTempFollower(this HeroDataComponent self, Unit matster, int monster)
        {
            Unit nowUnit = self.GetParent<Unit>();
            NumericComponent numericComponent = nowUnit.GetComponent<NumericComponent>();
            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monster);

            //判定是否为成长怪物
            if (monsterConfig.MonsterSonType == 1)
            {
                int nowUserLv = nowUnit.GetComponent<UserInfoComponent>().UserInfo.Lv;
                for (int i = 0; i < monsterConfig.Parameter.Length; i++)
                {
                    MonsterConfig monsterCof = MonsterConfigCategory.Instance.Get(monsterConfig.Parameter[i]);
                    if (nowUserLv >= monsterCof.Lv)
                    {
                        //指定等级对应属性
                        monsterConfig = monsterCof;
                    }
                }
            }

            NumericComponent numericComponentMaster = matster.GetComponent<NumericComponent>();
            //numericComponent.Set((int)NumericType.Base_MaxHp_Base, (int)(monsterConfig.Hp * hpCoefficient), false);
            //numericComponent.Set((int)NumericType.Base_MinAct_Base, (int)(monsterConfig.Act * ackCoefficient), false);
            //numericComponent.Set((int)NumericType.Base_MaxAct_Base, (int)(monsterConfig.Act * ackCoefficient), false);
            //numericComponent.Set((int)NumericType.Base_MinDef_Base, monsterConfig.Def, false);
            //numericComponent.Set((int)NumericType.Base_MaxDef_Base, monsterConfig.Def, false);
            //numericComponent.Set((int)NumericType.Base_MinAdf_Base, monsterConfig.Adf, false);
            //numericComponent.Set((int)NumericType.Base_MaxAdf_Base, monsterConfig.Adf, false);
            numericComponent.Set((int)NumericType.Base_MaxHp_Base, (int)(numericComponentMaster.GetAsInt(NumericType.Base_MaxHp_Base) * 0.5f), false);
            numericComponent.Set((int)NumericType.Base_MinAct_Base, (int)(numericComponentMaster.GetAsInt(NumericType.Base_MinAct_Base) * 0.5f), false);
            numericComponent.Set((int)NumericType.Base_MaxAct_Base, (int)(numericComponentMaster.GetAsInt(NumericType.Base_MaxAct_Base) * 0.5f), false);
            numericComponent.Set((int)NumericType.Base_MinDef_Base, (int)(numericComponentMaster.GetAsInt(NumericType.Base_MinDef_Base) * 0.5f), false);
            numericComponent.Set((int)NumericType.Base_MaxDef_Base, (int)(numericComponentMaster.GetAsInt(NumericType.Base_MaxDef_Base) * 0.5f), false);
            numericComponent.Set((int)NumericType.Base_MinAdf_Base, (int)(numericComponentMaster.GetAsInt(NumericType.Base_MinAdf_Base) * 0.5f), false);
            numericComponent.Set((int)NumericType.Base_MaxAdf_Base, (int)(numericComponentMaster.GetAsInt(NumericType.Base_MaxAdf_Base) * 0.5f), false);

            numericComponent.Set((int)NumericType.Base_Speed_Base, monsterConfig.MoveSpeed, false);
            numericComponent.Set((int)NumericType.Base_Cri_Base, monsterConfig.Cri, false);
            numericComponent.Set((int)NumericType.Base_Res_Base, monsterConfig.Res, false);
            numericComponent.Set((int)NumericType.Base_Hit_Base, monsterConfig.Hit, false);
            numericComponent.Set((int)NumericType.Base_Dodge_Base, monsterConfig.Dodge, false);
            numericComponent.Set((int)NumericType.Base_ActDamgeSubPro_Base, monsterConfig.DefAdd, false);
            numericComponent.Set((int)NumericType.Base_MageDamgeSubPro_Base, monsterConfig.AdfAdd, false);
            numericComponent.Set((int)NumericType.Base_DamgeSubPro_Base, monsterConfig.DamgeAdd, false);

            //设置当前血量
            numericComponent.NumericDic[(int)NumericType.Now_Hp] = numericComponent.NumericDic[(int)NumericType.Now_MaxHp];
        }

        public static void InitJiaYuanPet(this HeroDataComponent self,  bool notice)
        {
            NumericComponent numericComponent = self.GetParent<Unit>().GetComponent<NumericComponent>();
            numericComponent.Set(NumericType.Now_MaxHp, 1, notice);
            numericComponent.Set(NumericType.Now_Hp, 1, notice);
        }

        public static void InitPet(this HeroDataComponent self, RolePetInfo rolePetInfo, bool notice)
        {
            Unit unit = self.GetParent<Unit>();

            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            for (int i = 0; i < rolePetInfo.Ks.Count; i++)
            {
                numericComponent.Set(rolePetInfo.Ks[i], rolePetInfo.Vs[i], notice);
            }
        }

        public static void InitPlan(this HeroDataComponent self, JiaYuanPlant jiaYuanPlant, bool notice)
        {
            NumericComponent numericComponent = self.GetParent<Unit>().GetComponent<NumericComponent>();
            numericComponent.Set(NumericType.StartTime, jiaYuanPlant.StartTime);
            numericComponent.Set(NumericType.GatherNumber, jiaYuanPlant.GatherNumber);
            numericComponent.Set(NumericType.GatherLastTime, jiaYuanPlant.GatherLastTime);
            numericComponent.Set(NumericType.GatherCellIndex, jiaYuanPlant.CellIndex);
        }

        public static void InitPasture(this HeroDataComponent self, JiaYuanPastures jiaYuanPlant, bool notice)
        {
            NumericComponent numericComponent = self.GetParent<Unit>().GetComponent<NumericComponent>();
            numericComponent.Set(NumericType.StartTime, jiaYuanPlant.StartTime);
            numericComponent.Set(NumericType.GatherNumber, jiaYuanPlant.GatherNumber);
            numericComponent.Set(NumericType.GatherLastTime, jiaYuanPlant.GatherLastTime);
        }

        public static void InitJingLing(this HeroDataComponent self, Unit master, int jinglingid, bool notice)
        {
            NumericComponent masterNumericComponent = master.GetComponent<NumericComponent>();

            NumericComponent numericComponent = self.GetParent<Unit>().GetComponent<NumericComponent>();
            foreach ((int ntype, long value) in masterNumericComponent.NumericDic)
            {
                if (ntype == NumericType.RechargeNumber || ntype == NumericType.MaoXianExp)
                {
                    continue;
                }

                numericComponent.Set(ntype, value, false);
            }
        }

        /// <summary>
        /// 角色属性模块初始化
        /// </summary>
        public static void InitMonsterInfo_Summon2(this HeroDataComponent self, MonsterConfig monsterConfig, CreateMonsterInfo createMonsterInfo)
        {
            Unit nowUnit = self.GetParent<Unit>();
            NumericComponent numericComponent = nowUnit.GetComponent<NumericComponent>();

            int monsterlevel = 1;
            Unit masterUnit = nowUnit.GetParent<UnitComponent>().Get(createMonsterInfo.MasterID);
            if (masterUnit.Type == UnitType.Player)
            {
                monsterlevel = masterUnit.GetComponent<UserInfoComponent>().UserInfo.Lv;
            }
            else
            {
                monsterlevel = monsterConfig.Lv;
            }

            //0.8,0.8,0.5,0.5;5000,0,0,0,0
            //血量比例,攻击比例,魔法比例,物防比例，魔防比例；血量固定值,攻击固定值，魔法固定值，物防固定值，魔防固定值
            string[] summonInfo = createMonsterInfo.AttributeParams.Split(';');

            int useMasterModel = int.Parse(summonInfo[0]);
            numericComponent.Set((int)NumericType.UseMasterModel, useMasterModel, false);

            string[] attributeList_1 = summonInfo[1].Split(',');    //比列
            string[] attributeList_2 = summonInfo[2].Split(',');    //固定值

            NumericComponent masterNumberComponent = masterUnit.GetComponent<NumericComponent>();
            numericComponent.Set((int)NumericType.Now_Lv, monsterlevel, false);
            numericComponent.Set((int)NumericType.Base_MaxHp_Base, (int)((float)masterNumberComponent.GetAsInt(NumericType.Now_MaxHp) * float.Parse(attributeList_1[0]) * (1+ masterNumberComponent.GetAsFloat(NumericType.Now_SummonAddPro))) + int.Parse(attributeList_2[0]), false);
            numericComponent.Set((int)NumericType.Base_MinAct_Base, (int)((float)masterNumberComponent.GetAsInt(NumericType.Now_MaxAct) * float.Parse(attributeList_1[1]) * (1 + masterNumberComponent.GetAsFloat(NumericType.Now_SummonAddPro))) + int.Parse(attributeList_2[1]), false);  //召唤怪物继承当前角色最大攻击
            numericComponent.Set((int)NumericType.Base_MaxAct_Base, (int)((float)masterNumberComponent.GetAsInt(NumericType.Now_MaxAct) * float.Parse(attributeList_1[1]) * (1 + masterNumberComponent.GetAsFloat(NumericType.Now_SummonAddPro))) + int.Parse(attributeList_2[1]), false);
            numericComponent.Set((int)NumericType.Base_Mage_Base, (int)((float)masterNumberComponent.GetAsInt(NumericType.Now_Mage) * float.Parse(attributeList_1[2]) * (1 + masterNumberComponent.GetAsFloat(NumericType.Now_SummonAddPro))) + int.Parse(attributeList_2[2]), false);
            numericComponent.Set((int)NumericType.Base_MinDef_Base, (int)((float)masterNumberComponent.GetAsInt(NumericType.Now_MinDef) * float.Parse(attributeList_1[3]) * (1 + masterNumberComponent.GetAsFloat(NumericType.Now_SummonAddPro))) + int.Parse(attributeList_2[3]), false);
            numericComponent.Set((int)NumericType.Base_MaxDef_Base, (int)((float)masterNumberComponent.GetAsInt(NumericType.Now_MaxDef) * float.Parse(attributeList_1[3]) * (1 + masterNumberComponent.GetAsFloat(NumericType.Now_SummonAddPro))) + int.Parse(attributeList_2[3]), false);
            numericComponent.Set((int)NumericType.Base_MinAdf_Base, (int)((float)masterNumberComponent.GetAsInt(NumericType.Now_MinAdf) * float.Parse(attributeList_1[4]) * (1 + masterNumberComponent.GetAsFloat(NumericType.Now_SummonAddPro))) + int.Parse(attributeList_2[4]), false);
            numericComponent.Set((int)NumericType.Base_MaxAdf_Base, (int)((float)masterNumberComponent.GetAsInt(NumericType.Now_MaxAdf) * float.Parse(attributeList_1[4]) * (1 + masterNumberComponent.GetAsFloat(NumericType.Now_SummonAddPro))) + int.Parse(attributeList_2[4]), false);

            numericComponent.Set((int)NumericType.Now_MageBossPro, (int)(masterNumberComponent.GetAsInt(NumericType.Now_MageBossPro) ), false);
            numericComponent.Set((int)NumericType.Now_ActBossSubPro, (int)(masterNumberComponent.GetAsInt(NumericType.Now_ActBossSubPro) ), false);


            //属性
            numericComponent.Set((int)NumericType.Base_Speed_Base, monsterConfig.MoveSpeed, false);
            numericComponent.Set((int)NumericType.Base_Cri_Base, monsterConfig.Cri, false);
            numericComponent.Set((int)NumericType.Base_Res_Base, monsterConfig.Res, false);
            numericComponent.Set((int)NumericType.Base_Hit_Base, monsterConfig.Hit, false);
            numericComponent.Set((int)NumericType.Base_Dodge_Base, monsterConfig.Dodge, false);
            numericComponent.Set((int)NumericType.Base_ActDamgeSubPro_Base, monsterConfig.DefAdd, false);
            numericComponent.Set((int)NumericType.Base_MageDamgeSubPro_Base, monsterConfig.AdfAdd, false);
            numericComponent.Set((int)NumericType.Base_DamgeSubPro_Base, monsterConfig.DamgeAdd, false);
            //设置当前血量
            numericComponent.Set((int)NumericType.Now_Hp, numericComponent.GetAsInt(NumericType.Now_MaxHp));
            //Log.Debug("初始化当前怪物血量:" + numericComponent.GetAsLong(NumericType.Now_Hp));

            //新增的参数
            //血量比例,攻击比例,魔法比例,物防比例，魔防比例,新增比列1，新增比列2....；血量固定值,攻击固定值，魔法固定值，物防固定值，魔防固定值，新增固定值1,新增固定值2..
            if (attributeList_1.Length > 5)
            {
                float a = masterNumberComponent.GetAsFloat(NumericType.Now_Cri);
                //暴击,命中,闪避,韧性
                numericComponent.Set((int)NumericType.Base_Cri_Base, (float.Parse(attributeList_1[5]) * masterNumberComponent.GetAsFloat(NumericType.Now_Cri)), false);
                numericComponent.Set((int)NumericType.Base_Hit_Base, (float.Parse(attributeList_1[6]) * masterNumberComponent.GetAsFloat(NumericType.Now_Hit)), false);
                numericComponent.Set((int)NumericType.Base_Dodge_Base, (float.Parse(attributeList_1[7]) * masterNumberComponent.GetAsFloat(NumericType.Now_Dodge)), false);
                numericComponent.Set((int)NumericType.Base_Res_Base, (float.Parse(attributeList_1[8]) * masterNumberComponent.GetAsFloat(NumericType.Now_Res)), false);
            }

        }

        /// <summary>
        /// 角色属性模块初始化
        /// </summary>
        public static void InitMonsterInfo(this HeroDataComponent self, MonsterConfig monsterConfig, CreateMonsterInfo createMonsterInfo)
        {
            Unit nowUnit = self.GetParent<Unit>();
            NumericComponent numericComponent = nowUnit.GetComponent<NumericComponent>();

            float hpCoefficient = 1f;
            float ackCoefficient = 1f;
            //根据副本难度刷新属性
            //进入 挑战关卡 怪物血量增加 1.5 伤害增加 1.2 低于关卡 血量增加2 伤害增加 1.5
            MapComponent mapComponent = nowUnit.DomainScene().GetComponent<MapComponent>();
            int sceneType = mapComponent.SceneTypeEnum;
            int fubenDifficulty = FubenDifficulty.None;

            float attributeAdd = 1f;

            if (sceneType == SceneTypeEnum.CellDungeon || sceneType == SceneTypeEnum.LocalDungeon)
            {
                switch (sceneType)
                {
                    case SceneTypeEnum.CellDungeon:
                        fubenDifficulty = nowUnit.DomainScene().GetComponent<CellDungeonComponent>().FubenDifficulty;
                        break;
                    case SceneTypeEnum.LocalDungeon:
                        if (monsterConfig.MonsterType == MonsterTypeEnum.Boss)
                        {
                            LocalDungeonComponent localDungeonComponent = nowUnit.DomainScene().GetComponent<LocalDungeonComponent>();
                            Unit mainUnit = localDungeonComponent.MainUnit;
                            int killNumber = mainUnit.GetComponent<UserInfoComponent>().GetMonsterKillNumber(monsterConfig.Id);
                            int chpaterid = DungeonConfigCategory.Instance.GetChapterByDungeon(mapComponent.SceneId);
                            BossDevelopment bossDevelopment = ConfigHelper.GetBossDevelopmentByKill(chpaterid, killNumber);
                            attributeAdd = bossDevelopment.AttributeAdd;
                            fubenDifficulty = localDungeonComponent.FubenDifficulty;
                        }
                        break;
                    default:
                        break;
                }
                if (monsterConfig.MonsterType == MonsterTypeEnum.Boss && monsterConfig.Id!=SeasonHelper.SeasonBossId)
                {
                    switch (fubenDifficulty)
                    {
                        case FubenDifficulty.TiaoZhan:
                            hpCoefficient = 1.75f;
                            ackCoefficient = 1.3f;
                            break;
                        case FubenDifficulty.DiYu:
                            hpCoefficient = 2.5f;
                            ackCoefficient = 1.65f;
                            break;
                    }
                }
            }
            if (sceneType == SceneTypeEnum.TeamDungeon)
            {
                //副本的怪物难度提升（类似不难度的个人副本 给个配置即可）
                int realplayerNumber = nowUnit.DomainScene().GetComponent<TeamDungeonComponent>().InitPlayerNumber();
                fubenDifficulty = mapComponent.FubenDifficulty;
                //深渊BOSSS属性加成
                if (fubenDifficulty == TeamFubenType.ShenYuan && monsterConfig.MonsterType == MonsterTypeEnum.Boss)
                {
                    hpCoefficient = 2.5f;
                    ackCoefficient = 1.3f;
                }

                //人数对应小怪
                if (realplayerNumber == 2 && monsterConfig.MonsterType != MonsterTypeEnum.Boss)
                {
                    hpCoefficient = 1.5f;
                }
                if (realplayerNumber >= 3 && monsterConfig.MonsterType != MonsterTypeEnum.Boss)
                {
                    hpCoefficient = 2f;
                }
            }

            //判定是否为成长怪物
            if (monsterConfig.MonsterSonType == 1)
            {
                int nowUserLv = nowUnit.GetComponent<UserInfoComponent>().UserInfo.Lv;
                for (int i = 0; i < monsterConfig.Parameter.Length; i++)
                {
                    MonsterConfig monsterCof = MonsterConfigCategory.Instance.Get(monsterConfig.Parameter[i]);
                    if (nowUserLv >= monsterCof.Lv)
                    {
                        //指定等级对应属性
                        monsterConfig = monsterCof;
                    }
                }
            }

            //判定是否为成长怪物
            if (monsterConfig.MonsterSonType == 2)
            {
                MonsterConfig monsterCof = MonsterConfigCategory.Instance.Get(nowUnit.ConfigId);
                int nowUserLv = monsterCof.Lv;
                //nowUnit.GetComponent<UserInfoComponent>().UserInfo.Lv;
                int playerLv = createMonsterInfo.PlayerLevel;
                string attribute = createMonsterInfo.AttributeParams;   //2;2
                float hpPro = float.Parse(attribute.Split(";")[0]);
                float otherPro = float.Parse(attribute.Split(";")[1]);
                ExpConfig expCof = ExpConfigCategory.Instance.Get(nowUserLv);
                monsterConfig.Hp = (int)(expCof.BaseHp * hpPro);
                monsterConfig.Act = (int)(expCof.BaseAct * otherPro);
                monsterConfig.Def = (int)(expCof.BaseDef * otherPro);
                monsterConfig.Adf = (int)(expCof.BaseAdf * otherPro);
                monsterConfig.Lv = playerLv;
            }

            //attributeAdd   (boss成长boss加成)
            numericComponent.Set((int)NumericType.Base_MaxHp_Base, (int)(monsterConfig.Hp * hpCoefficient * attributeAdd), false);
            numericComponent.Set((int)NumericType.Base_MinAct_Base, (int)(monsterConfig.Act * ackCoefficient * attributeAdd), false);
            numericComponent.Set((int)NumericType.Base_MaxAct_Base, (int)(monsterConfig.Act * ackCoefficient * attributeAdd), false);
            numericComponent.Set((int)NumericType.Base_MinDef_Base, monsterConfig.Def, false);
            numericComponent.Set((int)NumericType.Base_MaxDef_Base, monsterConfig.Def, false);
            numericComponent.Set((int)NumericType.Base_MinAdf_Base, monsterConfig.Adf, false);
            numericComponent.Set((int)NumericType.Base_MaxAdf_Base, monsterConfig.Adf, false);
            numericComponent.Set((int)NumericType.Base_Speed_Base, monsterConfig.MoveSpeed, false);
            numericComponent.Set((int)NumericType.Base_Cri_Base, monsterConfig.Cri, false);
            numericComponent.Set((int)NumericType.Base_Res_Base, monsterConfig.Res, false);
            numericComponent.Set((int)NumericType.Base_Hit_Base, monsterConfig.Hit, false);
            numericComponent.Set((int)NumericType.Base_Dodge_Base, monsterConfig.Dodge, false);
            numericComponent.Set((int)NumericType.Base_ActDamgeSubPro_Base, monsterConfig.DefAdd, false);
            numericComponent.Set((int)NumericType.Base_MageDamgeSubPro_Base, monsterConfig.AdfAdd, false);
            numericComponent.Set((int)NumericType.Base_DamgeSubPro_Base, monsterConfig.DamgeAdd, false);

            //设置当前血量
            numericComponent.Set((int)NumericType.Now_Hp,  numericComponent.GetAsInt(NumericType.Now_MaxHp));
            //Log.Debug("初始化当前怪物血量:" + numericComponent.GetAsLong(NumericType.Now_Hp));
        }

        /// <summary>
        /// 更新当前角色身上的buff信息, 更新基础属性
        /// </summary>
        public static void BuffPropertyUpdate_Long(this HeroDataComponent self, int numericType, long NumericTypeValue)
        {
            if (numericType== NumericType.RechargeBuChang || numericType == NumericType.RechargeNumber)
            {
                Log.Error($"BuffPropertyUpdate_Long: {self.DomainZone()}  {self.Id}");
            }

            Unit nowUnit = self.GetParent<Unit>();
            NumericComponent numericComponent = nowUnit.GetComponent<NumericComponent>();
            long newvalue = numericComponent.GetAsLong(numericType) + NumericTypeValue;
            numericComponent.Set(numericType, newvalue);

            /*
            //获取是暴击等级等二次属性 需要二次计算
            if ((int)(numericType / 100) == NumericType.Now_CriLv)
            {

                long criLv = numericComponent.GetAsLong(NumericType.Now_CriLv);
                long hitLv = numericComponent.GetAsLong(NumericType.Now_HitLv);
                long dodgeLv = numericComponent.GetAsLong(NumericType.Now_DodgeLv);
                long resLv = numericComponent.GetAsLong(NumericType.Now_ResLv);

                Function_Fight.GetInstance().UnitUpdateProperty_Base(nowUnit);

                //float criProAdd = Function_Fight.LvProChange(criLv, nowUnit.GetComponent<UserInfoComponent>().UserInfo.Lv);
                //numericComponent.Set(NumericType.Now_Cri, (long)(criLv * 10000) + numericComponent.GetAsLong(NumericType.Now_Cri), true);
            }
            */
        }

        public static void BuffPropertyUpdate_Float(this HeroDataComponent self, int numericType, float NumericTypeValue)
        {
            if (numericType == NumericType.RechargeBuChang || numericType == NumericType.RechargeNumber)
            {
                Log.Error($"BuffPropertyUpdate_Long: {self.DomainZone()}  {self.Id}");
            }
            Unit nowUnit = self.GetParent<Unit>();
            NumericComponent numericComponent = nowUnit.GetComponent<NumericComponent>();
            float newvalue = numericComponent.GetAsFloat(numericType) + NumericTypeValue;
            numericComponent.Set(numericType, newvalue);
        }


        public static void OnDead(this HeroDataComponent self, Unit attack)
        {
            Unit unit = self.GetParent<Unit>();
            unit.GetComponent<MoveComponent>()?.Stop();
            //{
            //    unit.Stop(-1);
            //}

            unit.GetComponent<AIComponent>()?.Stop();
            unit.GetComponent<SkillPassiveComponent>()?.Stop();
            unit.GetComponent<SkillManagerComponent>()?.OnFinish(false);
            unit.GetComponent<BuffManagerComponent>()?.OnDead(attack);
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            if (unit.Type == UnitType.Player)
            {
                RolePetInfo rolePetInfo = unit.GetComponent<PetComponent>().GetFightPet();
                if (rolePetInfo != null)
                {
                    unit.GetParent<UnitComponent>().Remove(rolePetInfo.Id);
                    unit.GetComponent<PetComponent>().OnPetDead(rolePetInfo.Id);
                }

                int now_horse = numericComponent.GetAsInt(NumericType.HorseRide);
                if (now_horse > 0)
                {
                    numericComponent.ApplyValue(NumericType.HorseRide, 0);
                }
            }
            //玩家死亡，怪物技能清空
            if (unit.Type == UnitType.Player && attack != null && attack.Type == UnitType.Monster)
            {
                Unit nearest = AIHelp.GetNearestEnemy(attack, attack.GetComponent<AIComponent>().ActRange);
                if (nearest == null)
                {
                    attack.GetComponent<AIComponent>().ChangeTarget(0);
                    attack.GetComponent<SkillManagerComponent>().OnFinish(true);
                }
                List<Unit> units = UnitHelper.GetUnitList(unit.DomainScene(), UnitType.Monster);
                for (int i = 0; i < units.Count; i++)
                {
                    units[i].GetComponent<AttackRecordComponent>()?.OnRemoveAttackByUnit(unit.Id);
                }
            }
            if (unit.Type == UnitType.Pet)
            {
                int sceneTypeEnum = unit.DomainScene().GetComponent<MapComponent>().SceneTypeEnum;
                if (sceneTypeEnum != (int)SceneTypeEnum.PetTianTi
                 && sceneTypeEnum != (int)SceneTypeEnum.PetDungeon
                 && sceneTypeEnum != (int)SceneTypeEnum.PetMing)
                {
                    long manster = numericComponent.GetAsLong(NumericType.MasterId);
                    Unit unit_manster = unit.GetParent<UnitComponent>().Get(manster);
                    //修改宠物出战状态
                    unit_manster.GetComponent<PetComponent>().OnPetDead(unit.Id);
                }
            }
            //怪物死亡， 清除玩家BUFF
            if (unit.Type == UnitType.Monster && MonsterConfigCategory.Instance.Get(unit.ConfigId).RemoveBuff == 0)
            {
                List<Unit> units = UnitHelper.GetUnitList(unit.DomainScene(), UnitType.Player);
                for (int i = 0; i < units.Count; i++)
                {
                    units[i].GetComponent<BuffManagerComponent>().OnDeadRemoveBuffBy(unit.Id);
                }
            }
            int waitRevive = self.OnWaitRevive();
            numericComponent.ApplyValue(NumericType.Now_Dead, 1);
            Game.EventSystem.Publish(new EventType.KillEvent()
            {
                WaitRevive = waitRevive,
                UnitAttack = attack,
                UnitDefend = unit,
            });
        }
#else
        public static void OnDead(this HeroDataComponent self, Unit attack)
        {
            Unit unit = self.GetParent<Unit>();
            unit.GetComponent<StateComponent>().Reset();
            unit.GetComponent<MoveComponent>()?.Stop();
            unit.GetComponent<SkillManagerComponent>()?.OnFinish();
            unit.GetComponent<BuffManagerComponent>()?.OnDead(attack);
            int sceneTypeEnum = unit.ZoneScene().GetComponent<MapComponent>().SceneTypeEnum;
            if (sceneTypeEnum == (int)SceneTypeEnum.CellDungeon)
            {
                unit.ZoneScene().GetComponent<CellDungeonComponent>().CheckChuansongOpen();
            }
        }
#endif



    }
}