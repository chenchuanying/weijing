﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{

    [Timer(TimerType.SkillTimer)]
    public class SkillTimer : ATimer<SkillManagerComponent>
    {
        public override void Run(SkillManagerComponent self)
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
    public class SkillManagerComponentAwakeSystem : AwakeSystem<SkillManagerComponent>
    {
        public override void Awake(SkillManagerComponent self)
        {
            self.Skills.Clear();
            self.DelaySkillList.Clear();
            self.SkillCDs.Clear();
            self.FangunSkillId = int.Parse(GlobalValueConfigCategory.Instance.Get(2).Value);
            self.SelfUnitComponent = self.DomainScene().GetComponent<UnitComponent>();
            self.SelfUnit = self.GetParent<Unit>();
        }
    }

    [ObjectSystem]
    public class SkillManagerComponentDestroySystem : DestroySystem<SkillManagerComponent>
    {
        public override void Destroy(SkillManagerComponent self)
        {
            self.OnDispose();
        }
    }

    /// <summary>
    /// 技能管理
    /// </summary>
    public static class SkillManagerComponentSystem
    {
        public static List<SkillInfo> GetRandomSkills(this SkillManagerComponent self, C2M_SkillCmd skillcmd, int weaponSkill)
        {
            Unit unit = self.GetParent<Unit>();
            List<SkillInfo> skillInfos = new List<SkillInfo>();

            SkillInfo skillInfo = new SkillInfo();
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(weaponSkill);

            Unit target = self.DomainScene().GetComponent<UnitComponent>().Get(skillcmd.TargetID);
            string[] randomInfos = skillConfig.GameObjectParameter.Split(';');

            switch (skillConfig.SkillTargetType)
            {
                case (int)SkillTargetType.SelfPosition:
                case (int)SkillTargetType.SelfFollow:
                    skillInfo = new SkillInfo();
                    skillInfo.WeaponSkillID = weaponSkill;
                    skillInfo.PosX = unit.Position.x;
                    skillInfo.PosY = unit.Position.y;
                    skillInfo.PosZ = unit.Position.z;
                    skillInfo.TargetID = skillcmd.TargetID;
                    skillInfo.TargetAngle = skillcmd.TargetAngle;
                    skillInfos.Add(skillInfo);
                    break;
                case (int)SkillTargetType.TargetPositon:
                    skillInfo = new SkillInfo();
                    skillInfo.WeaponSkillID = weaponSkill;
                    skillInfo.PosX = target != null ? target.Position.x : unit.Position.x;
                    skillInfo.PosY = target != null ? target.Position.y : unit.Position.y;
                    skillInfo.PosZ = target != null ? target.Position.z : unit.Position.z;
                    skillInfo.TargetID = skillcmd.TargetID;
                    skillInfo.TargetAngle = skillcmd.TargetAngle;
                    skillInfos.Add(skillInfo);
                    break;
                case (int)SkillTargetType.FixedPosition:            //定点位置
                    Vector3 sourcePoint = unit.Position;
                    Quaternion rotation = Quaternion.Euler(0, skillcmd.TargetAngle, 0);
                    Vector3 targetPoint = sourcePoint + rotation * Vector3.forward * skillcmd.TargetDistance;
                    skillInfo.WeaponSkillID = weaponSkill;
                    skillInfo.PosX = targetPoint.x;
                    skillInfo.PosY = targetPoint.y;
                    skillInfo.PosZ = targetPoint.z;
                    skillInfo.TargetID = skillcmd.TargetID;
                    skillInfo.TargetAngle = skillcmd.TargetAngle;
                    skillInfos.Add(skillInfo);
                    break;
                case (int)SkillTargetType.SelfRandom:                   //自身中心点随机
                    int randomSkillId = int.Parse(randomInfos[0]);
                    int randomNumber = int.Parse(randomInfos[1]);
                    int randomRange = int.Parse(randomInfos[2]);
                    int skillNumber = RandomHelper.RandomNumber(1, randomNumber);
                    for (int i = 0; i < skillNumber; i++)
                    {
                        skillInfo = new SkillInfo();
                        skillInfo.WeaponSkillID = randomSkillId;
                        skillInfo.TargetID = skillcmd.TargetID;
                        skillInfo.PosX = unit.Position.x + RandomHelper.RandomNumberFloat(-1 * randomRange, randomRange);
                        skillInfo.PosY = unit.Position.y;
                        skillInfo.PosZ = unit.Position.z + RandomHelper.RandomNumberFloat(-1 * randomRange, randomRange);
                        skillInfo.TargetID = skillcmd.TargetID;
                        skillInfo.TargetAngle = skillcmd.TargetAngle;
                        skillInfos.Add(skillInfo);
                    }
                    break;
                case (int)SkillTargetType.TargetRandom:                 //目标中心点随机
                    randomSkillId = int.Parse(randomInfos[0]);
                    randomNumber = int.Parse(randomInfos[1]);
                    randomRange = int.Parse(randomInfos[2]);
                    skillNumber = RandomHelper.RandomNumber(1, randomNumber);

                    for (int i = 0; i < skillNumber; i++)
                    {
                        skillInfo = new SkillInfo();
                        skillInfo.WeaponSkillID = randomSkillId;
                        skillInfo.PosX = target == null ? unit.Position.x : target.Position.x + RandomHelper.RandomNumberFloat(-1 * randomRange, randomRange);
                        skillInfo.PosY = target == null ? unit.Position.y : target.Position.y;
                        skillInfo.PosZ = target == null ? unit.Position.z : target.Position.z + RandomHelper.RandomNumberFloat(-1 * randomRange, randomRange);
                        skillInfo.TargetID = skillcmd.TargetID;
                        skillInfo.TargetAngle = skillcmd.TargetAngle;
                        skillInfos.Add(skillInfo);
                    }
                    break;
                case (int)SkillTargetType.PositionRandom:       //定点位置随机
                    randomSkillId = int.Parse(randomInfos[0]);
                    randomNumber = int.Parse(randomInfos[1]);
                    randomRange = int.Parse(randomInfos[2]);
                    skillNumber = RandomHelper.RandomNumber(1, randomNumber);
                    sourcePoint = unit.Position;
                    rotation = Quaternion.Euler(0, skillcmd.TargetAngle, 0);
                    targetPoint = sourcePoint + rotation * Vector3.forward * skillcmd.TargetDistance;
                    for (int i = 0; i < skillNumber; i++)
                    {
                        skillInfo = new SkillInfo();
                        skillInfo.WeaponSkillID = randomSkillId;
                        skillInfo.PosX = targetPoint.x + RandomHelper.RandomNumberFloat(-1 * randomRange, randomRange);
                        skillInfo.PosY = targetPoint.y;
                        skillInfo.PosZ = targetPoint.z + RandomHelper.RandomNumberFloat(-1 * randomRange, randomRange);
                        skillInfo.TargetID = skillcmd.TargetID;
                        skillInfo.TargetAngle = skillcmd.TargetAngle;
                        skillInfos.Add(skillInfo);
                    }
                    break;
                case (int)SkillTargetType.TargetFollow:         //跟随目标随机
                    randomSkillId = int.Parse(randomInfos[0]);
                    float intervalTime = float.Parse(randomInfos[1]);
                    skillNumber = Mathf.FloorToInt(float.Parse(randomInfos[2]) / intervalTime);
                    for (int i = 0; i < skillNumber; i++)
                    {
                        skillInfo = new SkillInfo();
                        skillInfo.WeaponSkillID = randomSkillId;
                        skillInfo.PosX = target.Position.x;
                        skillInfo.PosY = target.Position.y;
                        skillInfo.PosZ = target.Position.z;
                        skillInfo.TargetID = skillcmd.TargetID;
                        skillInfo.TargetAngle = skillcmd.TargetAngle;
                        skillInfo.SkillBeginTime = TimeHelper.ServerNow() + (long)(i * intervalTime * 1000);

                        if (i == 0)
                        {
                            skillInfos.Add(skillInfo);
                            continue;
                        }
                        self.DelaySkillList.Add(skillInfo);
                    }
                    break;
                case (int)SkillTargetType.SelfOnly:
                    skillInfo = new SkillInfo();
                    skillInfo.WeaponSkillID = weaponSkill;
                    skillInfo.PosX = unit.Position.x;
                    skillInfo.PosY = unit.Position.y;
                    skillInfo.PosZ = unit.Position.z;
                    skillInfos.Add(skillInfo);
                    break;
                case (int)SkillTargetType.TargetOnly:
                    if (target != null)
                    {
                        skillInfo = new SkillInfo();
                        skillInfo.WeaponSkillID = weaponSkill;
                        skillInfo.PosX = target.Position.x;
                        skillInfo.PosY = target.Position.y;
                        skillInfo.PosZ = target.Position.z;
                        skillInfo.TargetID = skillcmd.TargetID;
                        skillInfo.TargetAngle = skillcmd.TargetAngle;
                        skillInfos.Add(skillInfo);
                    }
                    else if (target == null && skillConfig.SkillActType == 0)
                    {
                        skillInfo = new SkillInfo();
                        skillInfo.TargetAngle = (int)Quaternion.QuaternionToEuler(unit.Rotation).y;
                        SkillConfig skillConfig1 = SkillConfigCategory.Instance.Get(weaponSkill);
                        Vector3 targetPosition = unit.Position + unit.Rotation * Vector3.forward * (float)skillConfig1.SkillRangeSize;
                        skillInfo.WeaponSkillID = weaponSkill;
                        skillInfo.PosX = targetPosition.x;
                        skillInfo.PosY = targetPosition.y;
                        skillInfo.PosZ = targetPosition.z;
                        skillInfo.TargetID = skillcmd.TargetID;

                        skillInfos.Add(skillInfo);
                    }
                    else
                    {
                        Log.Debug($"SkillManagerComponent: target == null:  {weaponSkill}");
                    }
                    break;
            }
            //如果是闪现技能，并且目标点不能到达
            if (skillConfig.GameObjectName == "Skill_ShanXian_1" && skillInfos.Count > 0)
            {
                Vector3 vector3 = new Vector3(skillInfos[0].PosX, skillInfos[0].PosY, skillInfos[0].PosZ);
                Vector3 target3 = self.DomainScene().GetComponent<MapComponent>().GetCanReachPath(unit.Position, vector3);
                skillInfos[0].PosX = target3.x;
                skillInfos[0].PosY = target3.y;
                skillInfos[0].PosZ = target3.z;
            }
            return skillInfos;
        }

        public static void OnDispose(this SkillManagerComponent self)
        {
            int skillcnt = self.Skills.Count;
            for (int i = skillcnt - 1; i >= 0; i--)
            {
                SkillHandler skillHandler = self.Skills[i];
                self.Skills.RemoveAt(i);
                ObjectPool.Instance.Recycle(skillHandler);
            }
            self.SkillCDs.Clear();
            TimerComponent.Instance?.Remove(ref self.Timer);
        }

        public static void OnFinish(this SkillManagerComponent self, bool notice)
        {
            Unit unit = self.GetParent<Unit>();
            int skillcnt = self.Skills.Count;
            for (int i = skillcnt - 1; i >= 0; i--)
            {
                SkillHandler skillHandler = self.Skills[i];
                self.Skills.RemoveAt(i);
                skillHandler.OnFinished();
                ObjectPool.Instance.Recycle(skillHandler);
            }

            if (notice && unit!=null && !unit.IsDisposed)
            {
                self.M2C_UnitFinishSkill.UnitId = unit.Id;
                MessageHelper.SendToClient(UnitHelper.GetUnitList(unit.DomainScene(), UnitType.Player), self.M2C_UnitFinishSkill);
            }
        }

        public static async ETTask OnContinueSkill(this SkillManagerComponent self, C2M_SkillCmd skillcmd)
        {
            long instanceid = self.InstanceId;
            await TimerComponent.Instance.WaitAsync(1000);
            if (instanceid != self.InstanceId)
            {
                return;
            }
            self.OnUseSkill(skillcmd, false);
        }

        public static void InterruptSkill(this SkillManagerComponent self, int skillId)
        {
            int skillcnt = self.Skills.Count;
            for (int i = skillcnt - 1; i >= 0; i--)
            {
                SkillHandler skillHandler = self.Skills[i];
                if (skillHandler.SkillConf.Id != skillId)
                {
                    continue;
                }
                skillHandler.SetSkillState(SkillState.Finished);
            }
        }

        /// <summary>
        /// 不能重复释放冲锋技能
        /// </summary>
        /// <param name="self"></param>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public static bool CheckChongJi(this SkillManagerComponent self, int skillId)
        {
            if (!SkillConfigCategory.Instance.Contain(skillId))
            {
                return false;
            }
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);
            if (!SkillHelp.IsChongJi(skillConfig.GameObjectName))
            {
                return false;
            }
            int skillcnt = self.Skills.Count;
            for (int i = skillcnt - 1; i >= 0; i--)
            {
                if (self.Skills[i].SkillConf.GameObjectName == skillConfig.GameObjectName)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 打断吟唱中， 吟唱前客户端处理
        /// </summary>
        /// <param name="self"></param>
        /// <param name="skillId"></param>
        public static void InterruptSing(this SkillManagerComponent self,int skillId,bool ifStop)
        {
            Unit unit =self.GetParent<Unit>();
            for (int i = self.Skills.Count - 1; i >= 0; i--)
            {
                SkillHandler skillHandler = self.Skills[i];
                if (skillHandler.SkillConf.SkillSingTime == 0)
                {
                    continue;
                }
                //打断
                if (ifStop)
                {
                    skillHandler.SetSkillState(SkillState.Finished);
                    M2C_SkillInterruptResult m2C_SkillInterruptResult = new M2C_SkillInterruptResult() { UnitId = unit.Id, SkillId = skillHandler.SkillConf.Id };
                    //MessageHelper.Broadcast(unit, m2C_SkillInterruptResult);
                    self.BroadcastSkill(unit, m2C_SkillInterruptResult);
                }
            }
        }
        
        /// <summary>
        /// 服务器释放技能的点
        /// </summary>
        /// <param name="self"></param>
        /// <param name="skillcmd"></param>
        /// <param name="zhudong">被动触发</param>
        /// <returns></returns>
        public static M2C_SkillCmd OnUseSkill(this SkillManagerComponent self, C2M_SkillCmd skillcmd, bool zhudong = true)
        {
            Unit unit = self.GetParent<Unit>();
            M2C_SkillCmd m2C_Skill = self.M2C_SkillCmd;
            m2C_Skill.Message = String.Empty;

            //判断技能是否可以释放
            int errorCode = self.IsCanUseSkill(skillcmd.SkillID, zhudong);
            if (zhudong && errorCode != ErrorCore.ERR_Success)
            {
                m2C_Skill.Error = errorCode;
                return m2C_Skill;
            }

            int weaponSkill = unit.GetWeaponSkill(skillcmd.SkillID);
            SkillSetComponent skillSetComponent = unit.GetComponent<SkillSetComponent>();
            int tianfuSkill = skillSetComponent != null ? skillSetComponent.GetReplaceSkillId(skillcmd.SkillID) : 0;
            if (tianfuSkill != 0)
            {
                weaponSkill = tianfuSkill;
            }
            SkillConfig weaponSkillConfig = SkillConfigCategory.Instance.Get(weaponSkill);
            List<SkillInfo> skillList = self.GetRandomSkills(skillcmd, weaponSkill);
            if (skillList.Count == 0)
            {
                m2C_Skill.Error = ErrorCore.ERR_UseSkillError;
                return m2C_Skill;
            }

            unit.Rotation = Quaternion.Euler(0, skillcmd.TargetAngle, 0);
            if (unit.Type == UnitType.Player && weaponSkillConfig.IfStopMove == 0 && !unit.GetComponent<MoveComponent>().IsArrived())
            {
                unit.Stop(skillcmd.SkillID);
            }
            float now_ZhuanZhuPro = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Now_ZhuanZhuPro);
            if (zhudong && RandomHelper.RandFloat01() < now_ZhuanZhuPro
                && TimeHelper.ServerFrameTime() - self.LastLianJiTime >= 4000
                && !SkillHelp.IsChongJi(weaponSkillConfig.GameObjectName))
            {
                if (unit.Type == UnitType.Player)
                {
                    m2C_Skill.Message = "双重施法,触发法术连击!";
                }
                self.LastLianJiTime = TimeHelper.ServerFrameTime();
                self.OnContinueSkill(skillcmd).Coroutine();
            }
            self.InterruptSing(skillcmd.SkillID,false);

            for (int i = 0; i < skillList.Count; i++)
            {
                SkillHandler skillAction = self.SkillFactory(skillList[i], unit);
                skillList[i].SkillBeginTime = skillAction.SkillBeginTime;
                skillList[i].SkillEndTime = skillAction.SkillEndTime;
                self.Skills.Add(skillAction);
            }

            //添加技能CD列表  给客户端发送消失 我创建了一个技能,客户端创建特效等相关功能
            SkillCDItem skillCd = self.AddSkillCD(skillcmd.SkillID, weaponSkillConfig, zhudong);
            m2C_Skill.Error = ErrorCore.ERR_Success;
            m2C_Skill.CDEndTime = skillCd != null ? skillCd.CDEndTime : 0;
            m2C_Skill.PublicCDTime = self.SkillPublicCDTime;
            M2C_UnitUseSkill useSkill = new M2C_UnitUseSkill()
            {
                UnitId = unit.Id,
                ItemId = skillcmd.ItemId,
                SkillID = skillcmd.SkillID,
                TargetAngle = skillcmd.TargetAngle,
                SkillInfos = skillList,
                CDEndTime = skillCd != null ? skillCd.CDEndTime : 0,
                PublicCDTime = self.SkillPublicCDTime
            };
            //MessageHelper.Broadcast(unit, useSkill);
            self.BroadcastSkill(unit, useSkill);

            if (zhudong)
            {
                SkillPassiveComponent skillPassiveComponent = unit.GetComponent<SkillPassiveComponent>();
                if (skillPassiveComponent == null)
                {
                    Log.Debug($"skillPassiveComponent == null: {unit.Type}");
                }
                skillPassiveComponent?.OnTrigegerPassiveSkill(weaponSkillConfig.SkillActType == 0 ? SkillPassiveTypeEnum.AckGaiLv_1 : SkillPassiveTypeEnum.SkillGaiLv_7, skillcmd.TargetID, skillcmd.SkillID);
                skillPassiveComponent?.OnTrigegerPassiveSkill(weaponSkillConfig.SkillRangeSize <= 4 ? SkillPassiveTypeEnum.AckDistance_9 : SkillPassiveTypeEnum.AckDistance_10, skillcmd.TargetID, skillcmd.SkillID);
            }
            self.TriggerAddSkill(skillcmd, skillList[0].WeaponSkillID);

            TimerComponent.Instance.Remove(ref self.Timer);
            long repeatertime =  unit.Type == UnitType.Monster && MonsterConfigCategory.Instance.NoSkillMonsterList.Contains(unit.ConfigId) ? 200 : 200;
            self.Timer = TimerComponent.Instance.NewRepeatedTimer(repeatertime, TimerType.SkillTimer, self);
            return m2C_Skill;
        }

        public static SkillCDItem AddSkillCD(this SkillManagerComponent self, int skillid, SkillConfig weaponConfig, bool zhudong)
        {
            SkillCDItem skillCd = null;
            if (skillid == self.FangunSkillId)
            {
                skillCd = self.UpdateFangunSkillCD();
            }
            else if (weaponConfig.SkillActType == 0)
            {
                Unit unit = self.GetParent<Unit>();
                if (unit.Type != UnitType.Player)
                {
                    skillCd = self.UpdateSkillCD(skillid, weaponConfig.Id, zhudong);
                }
            }
            else
            {
                if (weaponConfig.SkillType == 1 && weaponConfig.PassiveSkillType == 1)
                {
                    return null;
                }
                skillCd = self.UpdateSkillCD(skillid, weaponConfig.Id, zhudong);
            }
            return skillCd;
        }

        public static void TriggerAddSkill(this SkillManagerComponent self, C2M_SkillCmd c2M_SkillCmd, int skillId)
        {
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);

            int addSkillId = skillConfig.AddSkillID;
            if (addSkillId!= 0 && !SkillConfigCategory.Instance.Contain(addSkillId))
            {
                Log.Debug($"skillConfig.AddSkillID无效：  {skillId} {addSkillId}");
            }
            if (addSkillId!=0 && SkillConfigCategory.Instance.Contain(addSkillId))
            {
                c2M_SkillCmd.SkillID = addSkillId;
                self.OnUseSkill(c2M_SkillCmd, false);
            }
            int[] selfSkillList = skillConfig.TriggerSelfSkillID;
            if (selfSkillList == null || selfSkillList.Length == 0 || selfSkillList[0] == 0)
            {
                return;
            }
            SkillSetComponent skillset = self.GetParent<Unit>().GetComponent<SkillSetComponent>();
            if (skillset == null)
            {
                return;
            }
            for (int i = 0; i < selfSkillList.Length; i++)
            {
                int selfSkillId = selfSkillList[i];
                if (skillset.GetBySkillID(selfSkillId) == null)
                {
                    continue;
                }
                c2M_SkillCmd.SkillID = selfSkillId;
                self.OnUseSkill(c2M_SkillCmd, false);
            }
        }

        public static SkillCDItem UpdateSkillCD(this SkillManagerComponent self, int skillId, int weaponSkill, bool zhudong)
        {
            Unit unit = self.GetParent<Unit>();
            SkillCDItem skillcd = null;
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(weaponSkill);
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            double skillcdTime = skillConfig.SkillCD;
            float nocdPro = numericComponent.GetAsFloat(NumericType.Now_SkillNoCDPro);
            if (nocdPro > RandomHelper.RandFloat01())
            {
                //return skillcd;
                skillcdTime = 1;  //1秒冷却CD
            }
            else
            {
                float now_cdpro= numericComponent.GetAsFloat(NumericType.Now_SkillCDTimeCostPro);
                skillcdTime *= ( 1f - now_cdpro);
            }

            float reduceCD = 0f;
            SkillSetComponent skillSetComponent = unit.GetComponent<SkillSetComponent>();
            
            Dictionary<int, float> keyValuePairs = skillSetComponent != null ? skillSetComponent.GetSkillPropertyAdd(weaponSkill) : null;
            if (keyValuePairs != null)
            {
                keyValuePairs.TryGetValue((int)SkillAttributeEnum.ReduceSkillCD, out reduceCD);
            }
            self.SkillCDs.TryGetValue(skillId, out skillcd);
            if (skillcd == null)
            {
                skillcd = new SkillCDItem();
                self.SkillCDs.Add(skillId, skillcd);
            }
            if (zhudong)
            {
                skillcd.SkillID = skillId;
                skillcd.CDEndTime = TimeHelper.ServerNow() +  (int)(1000 * ( (float)skillcdTime - reduceCD) );
            }
            else
            {
                skillcd.SkillID = skillId;
                skillcd.CDPassive = TimeHelper.ServerNow() + (int)(1000 * ((float)skillcdTime - reduceCD));
            }

            if (skillConfig.IfPublicSkillCD == 0 )
            {
                //添加技能公共CD
                self.SkillPublicCDTime = TimeHelper.ServerNow() + 800;  //公共1秒CD  
            }
            return skillcd;
        }

        //冲锋逻辑
        //1.连续释放3次技能,进入冷却状态
        //2.每次释放之间有5秒间隔时间,未超过间隔时间触发连击，如果超过时间重置为初始状态
        //初始状态 最开始的0次连击
        //冷却状态 10秒钟
        public static SkillCDItem UpdateFangunSkillCD(this SkillManagerComponent self)
        {
            SkillCDItem skillcd = null;
            long newTime = TimeHelper.ServerNow();
            if (newTime - self.FangunLastTime <= 5000)
            {
                self.FangunComboNumber++;
            }
            else
            {
                self.FangunComboNumber = 1;
            }

            if (self.FangunComboNumber >= 3)
            {
                int fangunskill = self.FangunSkillId;
                if (self.SkillCDs.ContainsKey(fangunskill))
                {
                    self.SkillCDs.Remove(fangunskill);  
                }
                self.FangunComboNumber = 0;
                skillcd = new SkillCDItem();
                skillcd.SkillID = fangunskill;
                skillcd.CDEndTime = newTime + 10000;
                self.SkillCDs.Add(fangunskill, skillcd);

                //Unit unit = self.GetParent<Unit>();
                //BuffData buffData_2 = new BuffData();
                //buffData_2.BuffConfig = SkillBuffConfigCategory.Instance.Get(90106003);
                //buffData_2.BuffClassScript = buffData_2.BuffConfig.BuffScript;
                //unit.GetComponent<BuffManagerComponent>().BuffFactory(buffData_2, unit, null);
            }
            self.FangunLastTime = newTime;
            return skillcd;
        }

        //技能是否可以使用
        public static int IsCanUseSkill(this SkillManagerComponent self, int nowSkillID, bool zhudong = true)
        {
            if (self.CheckChongJi(nowSkillID))
            { 
                return ErrorCore.ERR_SkillMoveTime;
            }

            Unit unit = self.GetParent<Unit>();
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(nowSkillID);
            StateComponent stateComponent = unit.GetComponent<StateComponent>();

            //判断技能是否再冷却中
            long serverNow = TimeHelper.ServerNow();
            SkillCDItem skillCDItem = null;
            self.SkillCDs.TryGetValue(nowSkillID, out skillCDItem);
            //被动技能触发冷却CD
            if (!zhudong && skillCDItem != null && serverNow < skillCDItem.CDPassive)
            {
                return ErrorCore.ERR_UseSkillInCD4;
            }
            //主动技能触发冷却CD
            if (zhudong && skillCDItem != null && serverNow < skillCDItem.CDEndTime)
            {
                return ErrorCore.ERR_UseSkillInCD3;
            }

            if (unit.Type == UnitType.Monster)
            {
                if (stateComponent.IsRigidity())  // && skillConfig.SkillActType == 0)
                {
                    return ErrorCore.ERR_CanNotUseSkill_Rigidity;
                }
            }
            if (unit.Type != UnitType.Player)
            {
                //判断当前眩晕状态
                int errorCode = stateComponent.CanUseSkill();
                if (ErrorCore.ERR_Success!= errorCode)
                {
                    return errorCode;
                }
                //判定是否再公共冷却时间
                if (serverNow < self.SkillPublicCDTime && skillConfig.SkillActType != 0)
                {
                    return ErrorCore.ERR_UseSkillInCD2;
                }
            }
            return ErrorCore.ERR_Success;
        }
        
        public static SkillHandler SkillFactory(this SkillManagerComponent self, SkillInfo skillcmd, Unit from)
        {
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillcmd.WeaponSkillID);
            SkillHandler skillHandler = (SkillHandler)ObjectPool.Instance.Fetch(SkillDispatcherComponent.Instance.SkillTypes[skillConfig.GameObjectName]);
            skillHandler.OnInit(skillcmd, from);
            return skillHandler;
        }

        public static List<SkillInfo> GetMessageSkill(this SkillManagerComponent self)
        {
            List<SkillInfo> skillinfos = new List<SkillInfo>();
            for (int i = 0; i < self.Skills.Count; i++)
            {
                skillinfos.Add(self.Skills[i].SkillInfo);
            }
            return skillinfos;
        }

        /// <summary>
        /// 队友进入地图
        /// </summary>
        /// <param name="self"></param>
        public static void OnEnterMap(this SkillManagerComponent self)
        {
            for (int i = self.Skills.Count - 1; i >= 0; i--)
            {
                SkillHandler skillHandler = self.Skills[i];
                self.Skills[i].OnUpdate();
                if (skillHandler.SkillConf.GameObjectName.Equals(StringBuilderHelper.Skill_Halo_2))
                {
                    (self.Skills[i] as Skill_Halo_2).Check_Map();
                } 
            }
        }

        public static void Check(this SkillManagerComponent self)
        {
            int skillcnt = self.Skills.Count;
            for (int i = skillcnt - 1; i >= 0; i-- )
            {
                if (self.Skills.Count == 0)
                {
                    Unit unit = self.GetParent<Unit>();
                    Log.Debug($"SkillManagerComponent582:  {unit.Type} {unit.ConfigId} {unit.InstanceId}");
                    break;
                }

                if (self.Skills[i].GetSkillState() == SkillState.Finished)
                {
                    SkillHandler skillHandler = self.Skills[i];
                    ObjectPool.Instance.Recycle(skillHandler);
                    skillHandler.OnFinished();
                    self.Skills.RemoveAt(i);
                    continue;
                }
                self.Skills[i].OnUpdate();
            }

            int dalaycnt = self.DelaySkillList.Count;
            for (int i = dalaycnt - 1; i >= 0; i--)
            {
                SkillInfo skillInfo = self.DelaySkillList[i];
                
                Unit target = self.SelfUnitComponent.Get(skillInfo.TargetID);
                if (target != null && !target.IsDisposed)
                {
                    skillInfo.PosX = target.Position.x;
                    skillInfo.PosY = target.Position.y;
                    skillInfo.PosZ = target.Position.z;
                }
                if (TimeHelper.ServerNow() < skillInfo.SkillBeginTime)
                {
                    continue;
                }
                
                //Unit from = self.GetParent<Unit>();
                SkillHandler skillAction = self.SkillFactory(skillInfo, self.SelfUnit);
                skillInfo.SkillBeginTime = skillAction.SkillBeginTime;
                skillInfo.SkillEndTime = skillAction.SkillEndTime;
                self.Skills.Add(skillAction);

                M2C_UnitUseSkill useSkill = new M2C_UnitUseSkill()
                {
                    UnitId = self.SelfUnit.Id,
                    SkillID = 0,
                    TargetAngle = 0,
                    SkillInfos = new List<SkillInfo>() { skillInfo }
                };

                //MessageHelper.Broadcast(self.SelfUnit, useSkill);
                self.BroadcastSkill(self.SelfUnit, useSkill);
                self.DelaySkillList.RemoveAt(i);
            }

            //循环检查冷却CD的技能
            /*
            if (self.SkillCDs.Count >= 1)
            {
                long nowTime = TimeHelper.ServerNow();
                List<int> removeList = new List<int>();
                foreach (SkillCDItem skillcd in self.SkillCDs.Values)
                {
                    if (nowTime >= skillcd.CDEndTime
                     && nowTime >= skillcd.CDPassive)
                    {
                        removeList.Add(skillcd.SkillID);
                    }
                }

                //移除技能cd结束的技能
                foreach (int removeID in removeList)
                {
                    self.SkillCDs.Remove(removeID);
                }
            }
            */
            
            if (self.Skills.Count == 0 && self.DelaySkillList.Count == 0)
            {
                TimerComponent.Instance.Remove( ref self.Timer );
            }
        }


        //技能广播
        public static void BroadcastSkill(this SkillManagerComponent self, Unit unit, IActorMessage message)
        {
            //主城不广播技能
            if (unit.DomainScene().GetComponent<MapComponent>().SceneTypeEnum != SceneTypeEnum.MainCityScene)
            {
                MessageHelper.Broadcast(unit, message);
            }
        }
    }
}
