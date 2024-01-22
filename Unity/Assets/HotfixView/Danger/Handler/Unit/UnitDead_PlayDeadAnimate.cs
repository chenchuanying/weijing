﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{

    [Event]
    public class UnitDead_PlayDeadAnimate : AEventClass<EventType.UnitDead>
    {
        
        protected override void Run(object cls)
        {
            try
            {
                EventType.UnitDead args = cls as EventType.UnitDead;
                Unit unit = args.Unit;
                if (unit == null || unit.IsDisposed)
                {
                    Log.Error("unitplaydead  unit.InstanceId == 0 || unit.IsDisposed");
                    return;
                }
                if (unit.ZoneScene().CurrentScene() == null)
                {
                    Log.Error("unitplaydead  unit.ZoneScene().CurrentScene() == null");
                    return;
                }

                unit.ZoneScene().GetComponent<LockTargetComponent>().OnUnitDead(unit);

                MapComponent mapComponent = unit.ZoneScene().GetComponent<MapComponent>();
                UnitInfoComponent unitInfoComponent = unit.GetComponent<UnitInfoComponent>();
                if (unit.Type == UnitType.Player)
                {
                    unit.GetComponent<EffectViewComponent>()?.OnDispose();

                    if (mapComponent.SceneTypeEnum != SceneTypeEnum.Demon)
                    {
                        unit.GetComponent<FsmComponent>()?.ChangeState(FsmStateEnum.FsmDeathState);
                        ShowRevive(unit, mapComponent).Coroutine();
                    }
                }
                else
                {
                    unit.GetComponent<EffectViewComponent>()?.OnDispose();
                }

                //播放开启宝箱特效
                if (unit.IsChest())
                {
                    unit.GetComponent<GameObjectComponent>().GameObject.SetActive(false);   //隐藏宝箱
                    unit.AddComponent<EffectViewComponent>();
                    int monsterid = unit.ConfigId;
                    if (monsterid == 80000101 || monsterid == 80000201 
                        || monsterid == 80000301 || monsterid == 80000401
                        || monsterid == 80000501 || monsterid == 80002003 
                        || monsterid == 80002004 || monsterid == 80003001 
                        || monsterid == 80003002)
                    {
                        FunctionEffect.GetInstance().PlaySelfEffect(unit, 91000108);
                    }
                }

                if (unit.GetComponent<NumericComponent>().GetAsLong(NumericType.ReviveTime) > 0)
                {
                    OnBossDead(unit).Coroutine();
                }
                else
                {
                    unit.GetComponent<FsmComponent>()?.ChangeState(FsmStateEnum.FsmDeathState);
                }

                if (unit.Type == UnitType.Monster && unit.GetMonsterType() == (int)MonsterTypeEnum.Boss)
                {
                    unit.GetComponent<MonsterActRangeComponent>()?.OnDead();
                    UI uimain = UIHelper.GetUI(unit.ZoneScene(), UIType.UIMain);
                    uimain.GetComponent<UIMainComponent>().UIMainHpBar.OnUnitDead(unit.Id);
                }

                if (unit.Type == UnitType.Monster && mapComponent.SceneTypeEnum == (int)SceneTypeEnum.TeamDungeon)
                {
                    GameObject Obstruct = GameObject.Find("Obstruct");
                    if (Obstruct == null)
                    {
                        return;
                    }
                    Obstruct.transform.Find(unit.ConfigId.ToString())?.gameObject.SetActive(false);
                }

                //如果死亡的是怪物,判断当前是否在挂机
                if (unit.Type == UnitType.Monster && mapComponent.SceneTypeEnum == SceneTypeEnum.LocalDungeon)
                {
                    //执行下一次攻击怪物指令
                    unit.ZoneScene().GetComponent<UnitGuaJiComponen>()?.KillMonster().Coroutine();
                }
                if (unit.Type == UnitType.Player || unit.IsBoss())
                {
                    unit.ZoneScene().GetComponent<BattleMessageComponent>().CancelRideTargetUnit(unit.Id);
                }

                //死亡的是怪物， 则清空一下
                if (unit.Type == UnitType.Pet && unit.GetMasterId() == UnitHelper.GetMyUnitId(unit.ZoneScene()))
                {
                     Dictionary<long, long> PetFightTime = unit.ZoneScene().GetComponent<BattleMessageComponent>().PetFightCD;
                    if (PetFightTime.ContainsKey(unit.Id))
                    {
                        PetFightTime[unit.Id] = 0;
                    }
                }

                //记录tap数据
                if (unit.Type == UnitType.Player)
                {
                    AccountInfoComponent accountInfoComponent = unit.ZoneScene().GetComponent<AccountInfoComponent>();
                    string serverName = ServerHelper.GetGetServerItem(!GlobalHelp.IsOutNetMode, accountInfoComponent.ServerId).ServerName;
                    UserInfo userInfo = unit.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
#if UNITY_ANDROID
                    TapSDKHelper.UpLoadPlayEvent(userInfo.Name, serverName, userInfo.Lv, 6, 1);
#endif
                }

                if (unit.Type == UnitType.Monster)
                {
                    OnMonsterDead(unit).Coroutine();
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        private async ETTask ShowRevive(Unit unit, MapComponent mapComponent)
        {
            if (!unit.MainHero)
            {
                return;
            }
          
            if (!SceneConfigHelper.IfCanRevive(mapComponent.SceneTypeEnum, mapComponent.SceneId))
            {
                long instanceId = unit.InstanceId;
                FloatTipManager.Instance.ShowFloatTip(GameSettingLanguge.LoadLocalization("该地图不支持复活"));
                await TimerComponent.Instance.WaitAsync(3000);
                if (instanceId != unit.InstanceId)
                {
                    return;
                }
                EnterFubenHelp.RequestQuitFuben(unit.ZoneScene());
                return;
            }

            unit.ZoneScene().GetComponent<SkillIndicatorComponent>().OnSelfDead();
            unit.ZoneScene().GetComponent<LockTargetComponent>().LastLockId = 0;
            UI uimain = UIHelper.GetUI(unit.ZoneScene(), UIType.UIMain);
            uimain.GetComponent<UIMainComponent>().OnStopAction();
            if (UIHelper.GetUI(unit.ZoneScene(), UIType.UICellDungeonRevive) == null)
            {
                UI uI = await UIHelper.Create(unit.ZoneScene(), UIType.UICellDungeonRevive);
                uI.GetComponent<UICellDungeonReviveComponent>().OnInitUI(mapComponent.SceneTypeEnum);
            }
        }

        private async ETTask OnMonsterDead(Unit unit)
        {
            long instanceId = unit.InstanceId;
            await TimerComponent.Instance.WaitAsync(1000);
            if (instanceId != unit.InstanceId)
            {
                return;
            }

            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(unit.ConfigId);
            if (monsterConfig.DeathSkillId != 0)
            {
                SkillConfig skillConfigCategory = SkillConfigCategory.Instance.Get(monsterConfig.DeathSkillId);
                long waitType = 1000 + (long)(skillConfigCategory.SkillDelayTime * 1000) + skillConfigCategory.SkillLiveTime;
                if (waitType >= 5000)
                {
                    unit.GetComponent<GameObjectComponent>().EnterHide();
                }
            }
        }

        private async ETTask OnBossDead(Unit unit)
        {
            long instanceId = unit.InstanceId;
            await TimerComponent.Instance.WaitAsync(1000);
            if (instanceId != unit.InstanceId)
            {
                return;
            }

            unit.GetComponent<FsmComponent>().ChangeState(FsmStateEnum.FsmHui);
            unit.GetComponent<UIUnitHpComponent>()?.OnDead();
            unit.GetComponent<GameObjectComponent>().OnHui();
        }
    }
}
