using System.Collections.Generic;

namespace ET
{

    [Event]
    public class Unit_OnNumericUpdate : AEventClass<EventType.UnitNumericUpdate>
    {
        protected override void Run(object numerice)
        {
            EventType.UnitNumericUpdate args = numerice as EventType.UnitNumericUpdate;
            Scene zoneScene = args.Unit.ZoneScene();
            switch (args.NumericType)
            {
                case NumericType.RechargeNumber:
                    int rechargeNumber = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.RechargeNumber);
                    int addNumer = rechargeNumber - (int)args.OldValue;

                    UserInfoComponent userInfoComponent = zoneScene.GetComponent<UserInfoComponent>();
                    if (!userInfoComponent.UserInfo.SingleRechargeIds.Contains(addNumer))
                    {
                        userInfoComponent.UserInfo.SingleRechargeIds.Add(addNumer);
                        args.Unit.ZoneScene().GetComponent<ReddotComponent>().UpdateReddont(ReddotType.SingleRecharge);
                    }
                    UI uI = UIHelper.GetUI(zoneScene, UIType.UIMain);
                    uI.GetComponent<UIMainComponent>().OnRechageSucess(addNumer);
                    break;
                case NumericType.PetExploreLuckly:
                    uI = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIPetEgg);
                    uI?.GetComponent<UIPetEggComponent>().OnUpdateLuckly();
                    break;
                case NumericType.WearWeaponFisrt:
                    UIHelper.Create(args.Unit.ZoneScene(), UIType.UIWearWeapon).Coroutine();
                    break;
                case NumericType.Now_Stall:
                    int stallType = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Stall);
                    args.Unit.GetComponent<GameObjectComponent>().OnUnitStallUpdate(stallType);
                    if (args.Unit.MainHero)
                    {
                        uI = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIMain);
                        uI.GetComponent<UIMainComponent>().ShowUIStall(stallType);
                    }
                    break;
                case NumericType.OccCombatRankID:
                case NumericType.FirstUnionName:
                    args.Unit.GetComponent<UIUnitHpComponent>()?.OnUpdateUnionName();
                    break;
                case NumericType.BattleCamp:
                    Unit unitmain = UnitHelper.GetMyUnitFromZoneScene(zoneScene);
                    UIUnitHpComponentSystem.UpdateBattleCamp(unitmain, args.Unit.Id);
                    break;
                case NumericType.RunRaceTransform:
                    int runraceMonster = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.RunRaceTransform);
                    args.Unit.GetComponent<GameObjectComponent>()?.OnRunRaceTranfer(runraceMonster, true);
                    if (args.Unit.MainHero)
                    {
                        args.Unit.ZoneScene().GetComponent<AttackComponent>().OnTransformId(args.Unit.ConfigId, runraceMonster);    
                    }
                    break;
                case NumericType.CardTransform:
                    int cardMonster = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.CardTransform);
                    args.Unit.GetComponent<GameObjectComponent>().OnCardTranfer(cardMonster, true);
                    if (args.Unit.MainHero)
                    {
                        args.Unit.ZoneScene().GetComponent<AttackComponent>().OnTransformId(args.Unit.ConfigId, cardMonster);
                    }
                    break;
                case NumericType.HappyCellIndex:
                    if (args.Unit.MainHero)
                    {
                        long oldvalue = args.OldValue;
                        long newvalue = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.HappyCellIndex);
                        if (oldvalue == newvalue)
                        {
                            FloatTipManager.Instance.ShowFloatTip("位置没有改变！");
                        }
                    }
                    break;
                case NumericType.JueXingAnger:
                    if (args.Unit.MainHero)
                    {
                        uI = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIMain);
                        uI.GetComponent<UIMainComponent>().UIMainSkillComponent.OnUpdateAngle();
                        args.Unit.GetComponent<UIUnitHpComponent>()?.UptateJueXingAnger();
                    }
                    break;
                case NumericType.UnionRaceWin:
                    if (args.Unit.MainHero)
                    {
                        args.Unit.GetComponent<UIUnitHpComponent>()?.UpdateShow();
                    }
                    break;
                case NumericType.SkillUseMP:
                    if (args.Unit.MainHero)
                    {
                        args.Unit.GetComponent<UIUnitHpComponent>()?.UpdateSkillUseMP();
                    }
                    break;
                case NumericType.UnionId_0:
                    long unionId = args.Unit.GetComponent<NumericComponent>().GetAsLong(NumericType.UnionId_0);
                    UI uifriend = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIFriend);
                    if (args.Unit.MainHero && uifriend != null && unionId > 0)
                    {
                        uifriend.GetComponent<UIFriendComponent>().OnCreateUnion();
                    }
                    if (args.Unit.MainHero && uifriend != null && unionId == 0)
                    {
                        uifriend.GetComponent<UIFriendComponent>().OnLeaveUnion();
                    }
                    if (args.Unit.MainHero)
                    {
                        UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIMain).GetComponent<UIMainComponent>().Btn_Union.SetActive(unionId > 0);
                    }
                    break;
                case NumericType.UnionLeader:
                    if (args.Unit.MainHero)
                    {
                        args.Unit.GetComponent<UIUnitHpComponent>().UpdateShow();
                    }
                    break;
                case NumericType.BossBelongID:
                    long bossbelongid = args.Unit.GetComponent<NumericComponent>().GetAsLong(NumericType.BossBelongID);
                    UI uImain = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIMain);
                    uImain.GetComponent<UIMainComponent>().UIMainHpBar.OnUpdateBelongID(args.Unit.Id, bossbelongid);
                    break;
                case NumericType.PetChouKa:
                    UI uipet = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIPetEgg);
                    uipet?.GetComponent<UIPetEggComponent>().UpdateChouKaTime();
                    break;
                case NumericType.Now_Weapon:
                    int weaponId = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Weapon);
                    args.Unit.GetComponent<ChangeEquipComponent>()?.ChangeWeapon(weaponId);
                    if (args.Unit.MainHero)
                    {
                        args.Unit.ZoneScene().GetComponent<AttackComponent>().OnInitOcc(args.Unit.ConfigId);
                    }
                    break;
                case NumericType.EquipIndex:
                    args.Unit.GetComponent<ChangeEquipComponent>()?.ChangeEquipIndex();
                    if (args.Unit.MainHero)
                    {
                        args.Unit.ZoneScene().GetComponent<AttackComponent>().OnInitOcc(args.Unit.ConfigId);
                        args.Unit.GetComponent<AnimatorComponent>().UpdateController();
                        args.Unit.GetComponent<GameObjectComponent>().CheckRunState();
                    }
                    break;
                case NumericType.Now_XiLian:
                    break;
                case NumericType.Ling_DiLv:
                    break;
                case NumericType.Ling_DiExp:
                    break;
                case NumericType.PetSkin:
                    args.Unit.RemoveComponent<EffectViewComponent>();
                    args.Unit.RemoveComponent<GameObjectComponent>();
                    args.Unit.RemoveComponent<AnimatorComponent>();
                    args.Unit.RemoveComponent<HeroTransformComponent>();
                    args.Unit.RemoveComponent<FsmComponent>();
                    args.Unit.RemoveComponent<UIUnitHpComponent>();
                    args.Unit.AddComponent<GameObjectComponent>();
                    break;
                case NumericType.TitleID:
                    args.Unit.GetComponent<UIUnitHpComponent>()?.UpdateShow();
                    break;
                case NumericType.ZeroClock:
                    zoneScene.GetComponent<UserInfoComponent>().ClearDayData();
                    zoneScene.GetComponent<TaskComponent>().OnZeroClockUpdate();
                    zoneScene.GetComponent<ActivityComponent>().OnZeroClockUpdate();
                    zoneScene.GetComponent<AccountInfoComponent>().TodayCreateRole = 0;
                    UIHelper.GetUI(zoneScene, UIType.UIMain).GetComponent<UIMainComponent>().OnZeroClockUpdate();
                    break;
                case NumericType.HongBao:
                    int hongbao= args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.HongBao);
                    UIHelper.GetUI(zoneScene, UIType.UIMain).GetComponent<UIMainComponent>().OnHongBao(hongbao);
                    break;
                case NumericType.PointRemain:
                    ReddotComponent reddotComponent = args.Unit.ZoneScene().GetComponent<ReddotComponent>();
                    reddotComponent.UpdateReddont(ReddotType.RolePoint);
                    break;
                case NumericType.BossInCombat:
                    int incombat = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.BossInCombat);
                    args.Unit.GetComponent<MonsterActRangeComponent>()?.OnBossInCombat(incombat);
                    break;
                case NumericType.Now_AI:
                    args.Unit.GetComponent<UIUnitHpComponent>()?.UpdateAI();
                    break;
                case NumericType.Now_TurtleAI:
                    //小龟状态改变。 头顶随机说一句话
                    args.Unit.GetComponent<NpcHeadBarComponent>()?.UpdateTurtleAI();
                    break;
                case NumericType.HorseRide:
                    args.Unit.GetComponent<GameObjectComponent>()?.OnUpdateHorse();
                    args.Unit.GetComponent<UIUnitHpComponent>()?.OnUpdateHorse();
                    break;
                case NumericType.HorseFightID:
                    if (args.Unit.MainHero)
                    {
                        uI = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIMain);
                        uI.GetComponent<UIMainComponent>().OnHorseRide();
                    }
                    break;
                case NumericType.JiaYuanPickOther:
                case NumericType.JiaYuanGatherOther:
                    uI = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIJiaYuanMain);
                    if (uI != null)
                    {
                        uI.GetComponent<UIJiaYuanMainComponent>().UIJiaYuaVisitComponent.UpdateGatherTimes();
                    }
                    break;
                case NumericType.BattleTodayKill:
                    UI uI_2 = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIBattleMain);
                    uI_2?.GetComponent<UIBattleMainComponent>().OnUpdateSelfKill();
                    break;
                case NumericType.PetExtendNumber:
                    Log.Debug("NumericType.PetExtendNumber");
                    break;
                case NumericType.TowerId:
                    int towerId = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.TowerId);
                    uI_2 = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UITowerOpen);
                    uI_2?.GetComponent<UITowerOpenComponent>().OnUpdateUI(towerId);
                    break;
                case NumericType.KillMonsterNumber:
                    uI = UIHelper.GetUI(args.Unit.ZoneScene(), UIType.UIMain);
                    uI?.GetComponent<UIMainComponent>().UpdateKillMonsterReward();
                    break;
                case NumericType.RingTaskId:
                    int newvalue1 = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.RingTaskId);
                    if (newvalue1 != 0)
                    {
                        NetHelper.SendGetTask(args.Unit.ZoneScene(),newvalue1).Coroutine();
                    }
                    break;
                case NumericType.UnionTaskId:
                    int newvalue2 = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.UnionTaskId);
                    if (newvalue2 != 0)
                    {
                        NetHelper.SendGetTask(args.Unit.ZoneScene(),newvalue2).Coroutine();
                    }
                    break;
                case NumericType.DailyTaskID:
                    int newvalue3 = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.DailyTaskID);
                    if (newvalue3 != 0)
                    {
                        NetHelper.SendGetTask(args.Unit.ZoneScene(),newvalue3).Coroutine();
                    }  
                    break;
                case NumericType.WeeklyTaskId:
                    int newvalue4 = args.Unit.GetComponent<NumericComponent>().GetAsInt(NumericType.WeeklyTaskId);
                    if (newvalue4 != 0)
                    {
                        NetHelper.SendGetTask(args.Unit.ZoneScene(), newvalue4).Coroutine();
                    }
                    break;
                default:
                    break;

            }
        }

    }
}
