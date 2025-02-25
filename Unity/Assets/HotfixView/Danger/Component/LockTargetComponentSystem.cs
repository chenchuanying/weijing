﻿using System.Collections.Generic;
using UnityEngine;

namespace ET
{


    public class LockTargetComponentAwakeSystem : AwakeSystem<LockTargetComponent>
    {
        public override void Awake(LockTargetComponent self)
        {
            Camera camera = UIComponent.Instance.MainCamera;
            self.MyCamera_1 =  camera.GetComponent<MyCamera_1>();
        }
    }


    public class LockTargetComponentDestroySystem : DestroySystem<LockTargetComponent>
    {
        public override void Destroy(LockTargetComponent self)
        {
            if (self.LockUnitEffect != null)
            {
                GameObject.Destroy(self.LockUnitEffect);
                self.LockUnitEffect = null;
            }
        }
    }

    public static class LockTargetComponentSystem
    {
        public static void OnMainHeroMove(this LockTargetComponent self)
        {
            Unit haveBoss = null;
            Unit main = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            MapComponent mapComponent = self.ZoneScene().GetComponent<MapComponent>();
            if (mapComponent.SceneTypeEnum != SceneTypeEnum.MainCityScene)
            {
                List<Unit> allUnit = main.GetParent<UnitComponent>().GetAll();
                for (int i = 0; i < allUnit.Count; i++)
                {
                    Unit unit = allUnit[i];
                    if (unit.Type != UnitType.Monster)
                    {
                        continue;
                    }
                    MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(unit.ConfigId);
                    if (monsterConfig.MonsterType == (int)MonsterTypeEnum.Boss && PositionHelper.Distance2D(unit, main) < 20f)
                    {
                        haveBoss = unit;
                        break;
                    }
                }
                UI uimain = UIHelper.GetUI(self.ZoneScene(), UIType.UIMain);
                uimain.GetComponent<UIMainComponent>().UIMainHpBar.ShowBossHPBar(haveBoss);
            }
            else
            {
                self.MyCamera_1.OnUpdate();
            }
        }

        public static void BeginEnterScene(this LockTargetComponent self)
        {
            self.HideLockEffect();
        }

        public static void HideLockEffect(this LockTargetComponent self)
        {
            if (self.LockUnitEffect != null)
            {
                self.LockUnitEffect.SetActive(false);
            }
            self.LastLockId = 0;

            UI uimain = UIHelper.GetUI(self.ZoneScene(), UIType.UIMain);
            uimain.GetComponent<UIMainComponent>().UIMainHpBar.OnCancelLock();
        }

        public static void OnSelfDead(this LockTargetComponent self)
        {
            self.HideLockEffect();
        }

        public static void OnUnitDead(this LockTargetComponent self, Unit unit)
        {
            if (unit.Id == self.LastLockId)
            {
                self.LastLockId = 0;
                self.HideLockEffect();
            }
        }

        public static void OnUnitRemove(this LockTargetComponent self, List<long> ids)
        {
            if (ids.Contains(self.LastLockId))
            {
                self.LastLockId = 0;
                self.HideLockEffect();
            }
        }

        public static void  CheckLockEffect(this LockTargetComponent self)
        {
            if (self.LockUnitEffect == null)
            {
                GameObject prefab = ResourcesComponent.Instance.LoadAsset<GameObject>(ABPathHelper.GetEffetPath("SkillZhishi/RoseSelectTarget"));
                self.LockUnitEffect = GameObject.Instantiate(prefab);
            }
        }

        public static void LockTargetUnitId(this LockTargetComponent self, long unitId)
        {
            Unit unitTarget = self.ZoneScene().CurrentScene().GetComponent<UnitComponent>().Get(unitId);
            if (unitTarget == null)
            {
                return;
            }
            self.LastLockId = unitId;
            self.CheckLockEffect();
            UICommonHelper.SetParent(self.LockUnitEffect, unitTarget.GetComponent<GameObjectComponent>().GameObject);
            self.LockUnitEffect.SetActive(true);
            
            if (unitTarget.Type == UnitType.Monster)
            {
                MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(unitTarget.ConfigId);
                UI uimain = UIHelper.GetUI(self.ZoneScene(), UIType.UIMain);
                uimain.GetComponent<UIMainComponent>().UIMainHpBar.OnLockUnit(unitTarget);
                uimain.GetComponent<UIMainComponent>().UIMainSkillComponent.OnLockUnit(unitTarget);
                self.SetEffectSize((float)monsterConfig.SelectSize);
            }
        }

        public static void SkillLock(this LockTargetComponent self, Unit main, SkillConfig skillConfig)
        {
            //没有技能指示器，并且不是强击类型的技能
            if (skillConfig.SkillZhishiType == 0 && skillConfig.SkillTargetType != SkillTargetType.TargetOnly)
            {
                return;
            }

            if (self.AttackTarget == 0)
            {
                //选择最近的
                self.LockTargetUnit();
            }
            else
            {
                //获取当前目标和自身目标的距离
                long targetId = self.LastLockId;
                UnitComponent unitComponent = main.GetParent<UnitComponent>();
                Unit targetUnit = unitComponent.Get(targetId);
                if (targetUnit == null || (PositionHelper.Distance2D(targetUnit, main) + 4) > skillConfig.SkillRangeSize)
                {
                    //获取当前最近的单位
                    Unit enemy = AIHelp.GetNearestEnemy_Client(main, (float)skillConfig.SkillRangeSize + 4);
                    //设置目标
                    if (targetUnit == null && enemy != null)
                    {
                        self.LockTargetUnitId(enemy.Id);
                    }
                }
            }
        }

        /// <summary>
        /// nearest 最近
        /// </summary>
        /// <param name="self"></param>
        /// <param name="random"></param>
        /// <returns></returns>
        public static long LockTargetUnit(this LockTargetComponent self, bool random = false)
        {
            Unit main = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            if (!random && self.AttackTarget == 1)
            {
                Unit unitTarget = main.GetParent<UnitComponent>().Get(self.LastLockId);
                if (unitTarget != null && PositionHelper.Distance2D(main, unitTarget) < 10f && unitTarget.IsCanBeAttack())
                {
                    return self.LastLockId;
                }
            }
            
            float distance = 10f;
            List<Unit> units = main.GetParent<UnitComponent>().GetAll();
            ListComponent<UnitLockRange> UnitLockRanges = new ListComponent<UnitLockRange>();
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];

                if (self.IsGuaJi)
                {
                    if (!main.IsCanAttackUnit(unit))
                    {
                        continue;
                    }
                }
                else
                {
                    ///非挂机状态下可以选中精灵
                    if (!main.IsCanAttackUnit(unit) && !unit.IsJingLingMonster())
                    {
                        continue;
                    }
                }
                

                StateComponent stateComponent = units[i].GetComponent<StateComponent>();
                if (stateComponent.StateTypeGet(StateTypeEnum.Stealth) || stateComponent.StateTypeGet(StateTypeEnum.Hide))
                {
                    continue;
                }
                float dd = PositionHelper.Distance2D(main, unit);
                if (dd < distance)
                {
                    UnitLockRanges.Add(new UnitLockRange() { Id = unit.Id, Range = (int)(dd * 100), Type = unit.Type });
                }
            }

            UnitLockRanges.Sort((a, b) => a.Range - b.Range);

            if (UnitLockRanges.Count == 0)
            {
                //取消锁定
                self.LastLockIndex = -1;
                self.LastLockId = 0;
            }
            else
            {
                bool attackedPlayer = false; // 是否锁定了玩家
                if (self.SkillAttackPlayerFirst == 1)
                {
                    if (self.AttackTarget == 0)
                    {
                        self.LastLockIndex = 0;
                    }
                    else
                    {
                        self.LastLockIndex++;
                    }

                    for (int i = self.LastLockIndex; i < UnitLockRanges.Count; i++)
                    {
                        if (UnitLockRanges[i].Type != UnitType.Player)
                        {
                            continue;
                        }

                        attackedPlayer = true;
                        self.LastLockIndex = i;
                        self.LastLockId = UnitLockRanges[i].Id;
                        break;
                    }
                }

                if (!attackedPlayer)
                {
                    //锁定最近目标
                    if (self.AttackTarget == 0)
                    {
                        self.LastLockIndex = 0;
                        self.LastLockId = UnitLockRanges[self.LastLockIndex].Id;
                    }
                    else
                    {
                        self.LastLockIndex++;
                        if (self.LastLockIndex >= UnitLockRanges.Count)
                        {
                            self.LastLockIndex = 0;
                        }

                        if (self.LastLockId != 0 && UnitLockRanges[self.LastLockIndex].Id == self.LastLockId)
                        {
                            self.LastLockIndex++;
                        }

                        if (self.LastLockIndex >= UnitLockRanges.Count)
                        {
                            self.LastLockIndex = 0;
                        }

                        self.LastLockId = UnitLockRanges[self.LastLockIndex].Id;
                    }
                }
            }
            self.LockTargetUnitId(self.LastLockId);
            return self.LastLockId;
        }

        public static void SetEffectSize(this LockTargetComponent self, float size)
        {
            GameObject RedCircle = self.LockUnitEffect.transform.Find("Efx_Click_Red/RedCircle").gameObject;
            ParticleSystem ps = RedCircle.GetComponent<ParticleSystem>();
            var main = ps.main;
            main.startSize = new ParticleSystem.MinMaxCurve(3 * size);
        }

        public static void OnLockNpc(this LockTargetComponent self, Unit unitTarget)
        {
            self.CheckLockEffect();
            if (unitTarget != null)
            {
                UICommonHelper.SetParent(self.LockUnitEffect, unitTarget.GetComponent<GameObjectComponent>().GameObject);
                self.LockUnitEffect.SetActive(true);
                self.SetEffectSize(1f);
            }
        }
    }

}
