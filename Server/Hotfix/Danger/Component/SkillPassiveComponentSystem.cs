﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{

    [Timer(TimerType.SkillPassive)]
    public class SkillPassiveTimer : ATimer<SkillPassiveComponent>
    {
        public override void Run(SkillPassiveComponent self)
        {
            try
            {
                self.Check();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }


    [ObjectSystem]
    public class SkillPassiveComponentAwakeSystem : AwakeSystem<SkillPassiveComponent>
    {
        public override void Awake(SkillPassiveComponent self)
        {

        }
    }

    [ObjectSystem]
    public class SkillPassiveComponentDestroySystem : DestroySystem<SkillPassiveComponent>
    {
        public override void Destroy(SkillPassiveComponent self)
        {
            TimerComponent.Instance?.Remove(ref self.Timer);
        }
    }

    public static class SkillPassiveComponentSystem
    {

        public static void Stop(this SkillPassiveComponent self)
        {
            TimerComponent.Instance?.Remove(ref self.Timer);
        }

        public static void Activeted(this SkillPassiveComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            if (unit.Type == UnitType.Player)
            {
                int equipId = unit.GetComponent<BagComponent>().GetWuqiItemId();
                self.OnTrigegerPassiveSkill(SkillPassiveTypeEnum.WandBuff_8, equipId);
            }

            TimerComponent.Instance?.Remove(ref self.Timer);
            if (self.SkillPassiveInfos.Count > 0)
            {
                self.Timer = TimerComponent.Instance.NewRepeatedTimer(1000, TimerType.SkillPassive, self);
            }
        }

        public static void CheckHuiXue(this SkillPassiveComponent self)
        {
            NumericComponent numericComponent = self.GetParent<Unit>().GetComponent<NumericComponent>();
            if (numericComponent.GetAsLong((int)NumericType.Now_Hp) >= numericComponent.GetAsLong((int)NumericType.Now_MaxHp))
                return;

            if (self.GetParent<Unit>().Type == UnitType.Pet)
            {
                numericComponent.ApplyChange(null, NumericType.Now_Hp, (long)(numericComponent.GetAsLong((int)NumericType.Now_MaxHp) * 0.1f), 0, true);
            }
            float now_SecHpAddPro = numericComponent.GetAsFloat(NumericType.Now_SecHpAddPro);
            if (now_SecHpAddPro > 0f)
            {
                numericComponent.ApplyChange(null, NumericType.Now_Hp, (long)(numericComponent.GetAsLong((int)NumericType.Now_MaxHp) * now_SecHpAddPro), 0, true);
            }
            float now_HuiXue = numericComponent.GetAsFloat(NumericType.Now_HuiXue);
            if (now_HuiXue > 0f)
            {
                numericComponent.ApplyChange(null, NumericType.Now_Hp, (long)(now_HuiXue * 10000),0, true);
            }
        }

        public static void Check(this SkillPassiveComponent self)
        {
            self.CheckHuiXue();
            self.OnTrigegerPassiveSkill( SkillPassiveTypeEnum.XueLiang_2 , self.GetParent<Unit>().Id);
        }

        public static void AddRolePassiveSkill(this SkillPassiveComponent self, int skillId)
        {
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);
            self.AddPassiveSkillByType(skillConfig);
            self.CheckSkillTianFu(skillId, true);
        }

        public static void CheckSkillTianFu(this SkillPassiveComponent self, int skillId,  bool active)
        {
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);
            if (skillConfig.SkillType == 1 || skillConfig.PassiveSkillType != 11)
            {
                return;
            }
            int tianfuid = int.Parse(skillConfig.ComObjParameter);
            self.GetParent<Unit>().GetComponent<SkillSetComponent>().AddiontTianFu(tianfuid, active);
        }

        public static void RemoveRolePassiveSkill(this SkillPassiveComponent self, int skillId)
        {
            for (int i = self.SkillPassiveInfos.Count - 1; i >= 0; i--)
            {
                if (self.SkillPassiveInfos[i].SkillId != skillId)
                {
                    continue;
                }
                self.SkillPassiveInfos.RemoveAt(i);
                break;
            }
            self.CheckSkillTianFu(skillId, false);
        }

        /// <summary>
        /// 更新角色被动技能
        /// </summary>
        /// <param name="self"></param>
        public static void UpdatePassiveSkill(this SkillPassiveComponent self)
        {
            self.SkillPassiveInfos.Clear();

            List<SkillPro> skillList = self.GetParent<Unit>().GetComponent<SkillSetComponent>().SkillList;
            for (int i = 0; i < skillList.Count; i++)
            {
                if (skillList[i].SkillSetType == (int)SkillSetEnum.Item)
                {
                    continue;
                }
                SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillList[i].SkillID);
                self.AddPassiveSkillByType(skillConfig);
            }
        }

        /// <summary>
        /// 更新怪物被动技能
        /// </summary>
        /// <param name="self"></param>
        public static void UpdateMonsterPassiveSkill(this SkillPassiveComponent self)
        {
            self.SkillPassiveInfos.Clear();
            int configId = self.GetParent<Unit>().ConfigId;
            MonsterConfig MonsterCof = MonsterConfigCategory.Instance.Get(configId);
            int[] aiSkillIDList = MonsterCof.SkillID;
            if (aiSkillIDList == null)
            {
                return;
            }
            for (int i = 0; i < aiSkillIDList.Length; i++)
            {
                if (aiSkillIDList[i] == 0)
                {
                    continue;
                }
                if (!SkillConfigCategory.Instance.Contain(aiSkillIDList[i]))
                {
                    continue;
                }
                SkillConfig skillConfig = SkillConfigCategory.Instance.Get(aiSkillIDList[i]);
                self.AddPassiveSkillByType(skillConfig);
            }
        }

        public static void UpdatePastureSkill(this SkillPassiveComponent self)
        { 
            
        }

        public static void UpdateJingLingSkill(this SkillPassiveComponent self,  int jinglingid)
        {
            JingLingConfig jingLingConfig = JingLingConfigCategory.Instance.Get(jinglingid);
            if (jingLingConfig.FunctionType == JingLingFunctionType.AddSkill)
            {
                self.AddRolePassiveSkill(int.Parse(jingLingConfig.FunctionValue));
            }
        }

        public static void UpdatePetPassiveSkill(this SkillPassiveComponent self, RolePetInfo rolePetInfo)
        {
            self.SkillPassiveInfos.Clear();
            int configId = self.GetParent<Unit>().ConfigId;
            PetConfig MonsterCof = PetConfigCategory.Instance.Get(configId);
            int zhuanZhuSkillID = int.Parse(MonsterCof.ZhuanZhuSkillID);

            SkillConfig skillConfig = null;
            if (zhuanZhuSkillID != 0)
            {
                skillConfig = SkillConfigCategory.Instance.Get(zhuanZhuSkillID);
                self.AddPassiveSkillByType(skillConfig);
            }

            string[] baseSkillID = MonsterCof.BaseSkillID.Split(';');
            for (int i = 0; i < baseSkillID.Length; i++ )
            {
                int baseSkillId = int.Parse(baseSkillID[i]);
                if (baseSkillId == 0)
                {
                    continue;
                }

                skillConfig = SkillConfigCategory.Instance.Get(baseSkillId);
                self.AddPassiveSkillByType(skillConfig);
            }

            for (int i = 0; i < rolePetInfo.PetSkill.Count; i++)
            {
                int baseSkillId = rolePetInfo.PetSkill[i];
                if (baseSkillId == 0)
                {
                    continue;
                }

                skillConfig = SkillConfigCategory.Instance.Get(baseSkillId);
                self.AddPassiveSkillByType(skillConfig);
            }
        }

        public static void AddPassiveSkillByType(this SkillPassiveComponent self,SkillConfig  skillConfig)
        {
            if (skillConfig.SkillType == 1 || skillConfig.PassiveSkillType == 0)
            {
                return;
            }
            for (int i = 0; i < self.SkillPassiveInfos.Count; i++)
            {
                if (self.SkillPassiveInfos[i].SkillId == skillConfig.Id)
                {
                    return;
                }
            }

            SkillPassiveInfo skillPassiveInfo = new SkillPassiveInfo(skillConfig.PassiveSkillType, skillConfig.Id, 
                (float)skillConfig.PassiveSkillPro, skillConfig.PassiveSkillTriggerOnce, skillConfig.SkillCD);
            self.SkillPassiveInfos.Add(skillPassiveInfo);
        }

        public static void OnTrigegerPassiveSkill(this SkillPassiveComponent self, SkillPassiveTypeEnum skillPassiveTypeEnum, long targetId = 0, int skillid = 0)
        {
            Unit unit = self.GetParent<Unit>();
            if (unit.Type == UnitType.Player)
            {
                ChengJiuComponent chengJiuComponent = unit.GetComponent<ChengJiuComponent>();
                if (chengJiuComponent.JingLingUnitId != 0 && unit.GetParent<UnitComponent>().Get(chengJiuComponent.JingLingUnitId) != null)
                {
                    Unit jingling = unit.GetParent<UnitComponent>().Get(chengJiuComponent.JingLingUnitId);
                    jingling.GetComponent<SkillPassiveComponent>().OnTrigegerPassiveSkill(skillPassiveTypeEnum, targetId, skillid);
                }
            }

            List<SkillPassiveInfo> skillPassiveInfos = new List<SkillPassiveInfo>();
            for (int i = 0; i < self.SkillPassiveInfos.Count; i++)
            {
                if (self.SkillPassiveInfos[i].SkillPassiveTypeEnum == (int)skillPassiveTypeEnum)
                {
                    skillPassiveInfos.Add(self.SkillPassiveInfos[i]);
                }
            }
            if (skillPassiveInfos.Count == 0)
            {
                return;
            }
            //SkillPassiveInfo skillIfo = skillPassiveInfos[RandomHelper.RandomNumber(0, skillPassiveInfos.Count)];
            for(int s = 0;s < skillPassiveInfos.Count;s ++)
            {
                    SkillPassiveInfo skillIfo = skillPassiveInfos[s];
                if (skillPassiveTypeEnum == SkillPassiveTypeEnum.WandBuff_8)
                {
                    int weapontype = Convert.ToInt32(skillIfo.SkillPro);
                    int buffId = SkillConfigCategory.Instance.Get(skillIfo.SkillId).InitBuffID[0];
                    int weaponType = targetId == 0 ? ItemEquipType.Common : (int)ItemConfigCategory.Instance.Get((int)targetId).EquipType;
                    if (weaponType != weapontype)
                    {
                        self.GetParent<Unit>().GetComponent<BuffManagerComponent>().BuffRemove(buffId);
                    }
                    if (weaponType == weapontype)
                    {
                        BuffData buffData_1 = new BuffData();
                        buffData_1.SkillId = skillIfo.SkillId;
                        buffData_1.BuffId = buffId;
                        unit.GetComponent<BuffManagerComponent>().BuffFactory(buffData_1, unit, null);
                    }
                    continue;
                }
                //只触发一次
                if (skillIfo.LastTriggerTime > 0 && skillIfo.TriggerOnce == 1)
                {
                        continue;
                }
                //触发限制
                if (skillIfo.TriggerInterval > 0 && TimeHelper.ServerNow() - skillIfo.LastTriggerTime < skillIfo.TriggerInterval)
                {
                        continue;
                }
                bool trigger = false;
                switch (skillPassiveTypeEnum)
                {
                    case SkillPassiveTypeEnum.AckGaiLv_1:
                        trigger = skillIfo.SkillPro >= RandomHelper.RandFloat01();
                        break;
                    case SkillPassiveTypeEnum.XueLiang_2:
                        NumericComponent numCom = self.GetParent<Unit>().GetComponent<NumericComponent>();
                        long nowHp = numCom.GetAsLong((int)NumericType.Now_Hp);
                        long maxHp = numCom.GetAsLong((int)NumericType.Now_MaxHp);
                        float hpPro = (float)nowHp / (float)maxHp;
                        trigger = hpPro <= skillIfo.SkillPro;
                        break;
                    case SkillPassiveTypeEnum.BeHurt_3:
                    case SkillPassiveTypeEnum.Critical_4:
                    case SkillPassiveTypeEnum.ShanBi_5:
                    case SkillPassiveTypeEnum.WillDead_6:
                    case SkillPassiveTypeEnum.SkillGaiLv_7:
                    case SkillPassiveTypeEnum.AckDistance_9:
                    case SkillPassiveTypeEnum.AckDistance_10:
                        trigger = skillIfo.SkillPro >= RandomHelper.RandFloat01();
                        break;
                }
                if (!trigger)
                {
                        continue;
                }
                long rigidityEndTime = 0;
                if (skillid == skillIfo.SkillId)
                {
                    Log.Debug($"SkillPassiveComponent: {skillIfo.SkillId}");
                        continue;
                }
                SkillManagerComponent skillManagerComponent = unit.GetComponent<SkillManagerComponent>();
                if (skillManagerComponent.IsCanUseSkill(skillIfo.SkillId) == ErrorCore.ERR_Success)
                {
                    int weaponSkill = unit.GetWeaponSkill(skillIfo.SkillId);
                    SkillConfig skillConfig = SkillConfigCategory.Instance.Get(weaponSkill);
                    List<long> targetIdList = new List<long>();
                    AIComponent aIComponent = unit.GetComponent<AIComponent>();
                    if (aIComponent != null)
                    {
                        targetId = aIComponent.TargetID;
                        Unit aiTarget = unit.DomainScene().GetComponent<UnitComponent>().Get(targetId);
                        if (aiTarget != null && skillConfig.SkillTargetType == (int)SkillTargetType.TargetOnly
                            && PositionHelper.Distance2D(unit.Position, aiTarget.Position) > aIComponent.ActDistance)
                        {
                                continue;
                        }

                        if (skillConfig.SkillTargetType > 0)
                        {
                            targetIdList.AddRange(AIHelp.GetNearestEnemyIds(unit, (float)aIComponent.ActRange, skillConfig.SkillTargetType));
                        }
                        else
                        {
                            targetIdList.Add(targetId);
                        }
                    }
                    if (targetIdList.Count == 0)
                    {
                        if (targetId == 0)
                        {
                            targetId = self.GetParent<Unit>().Id;
                        }
                        targetIdList.Add(targetId);
                    }

                    int targetAngle = (int)Quaternion.QuaternionToEuler(unit.Rotation).y;
                    Unit target = unit.DomainScene().GetComponent<UnitComponent>().Get(targetId);
                    if (target != null && target.Id != targetId)
                    {
                        Vector3 direction = target.Position - unit.Position;
                        targetAngle = (int)Mathf.Rad2Deg(Mathf.Atan2(direction.x, direction.z));
                    }

                    for (int i = 0; i < targetIdList.Count; i++)
                    {
                        C2M_SkillCmd cmd = new C2M_SkillCmd();
                        cmd.TargetAngle = targetAngle;
                        cmd.SkillID = skillIfo.SkillId;
                        cmd.TargetID = targetId;
                        skillManagerComponent.OnUseSkill(cmd, false);
                    }

                    skillIfo.LastTriggerTime = TimeHelper.ServerNow();
                    rigidityEndTime = (long)(skillConfig.SkillRigidity * 1000) + TimeHelper.ServerNow();
                }
                if (unit.IsDisposed)
                {
                    Log.Debug("SkillPassiveComponent :unit.IsDisposed ");
                        continue;
                }
                if (rigidityEndTime > unit.GetComponent<StateComponent>().RigidityEndTime)
                {
                    unit.GetComponent<StateComponent>().SetRigidityEndTime(rigidityEndTime);
                }
            }
        }
    }
}
