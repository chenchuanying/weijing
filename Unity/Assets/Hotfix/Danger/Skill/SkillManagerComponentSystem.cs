using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{

    [Timer(TimerType.SkillTimer)]
    public class SkillViewTimer : ATimer<SkillManagerComponent>
    {
        public override void Run(SkillManagerComponent self)
        {
            try
            {
                self.OnUpdate();
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
            self.t_Skills.Clear();
            self.Skills.Clear();
            self.SkillCDs.Clear();
            self.SkillPublicCDTime = 0;
            self.FangunSkillId = int.Parse(GlobalValueConfigCategory.Instance.Get(2).Value);

            Unit unit = self.GetParent<Unit>();
            self.MainHero = unit.MainHero;
        }
    }

    [ObjectSystem]
    public class SkillManagerComponentDestroySystem : DestroySystem<SkillManagerComponent>
    {
        public override void Destroy(SkillManagerComponent self)
        {
            self.SkillCDs.Clear();
            self.OnFinish();
        }
    }

    /// <summary>
    /// 技能管理
    /// </summary>

    public static class SkillManagerComponentSystem
    {
        public static bool IsSkillMoveTime(this SkillManagerComponent self)
        {
            return TimeHelper.ClientNow() < self.SkillMoveTime;
        }

        public static bool IsSkillSingTime(this SkillManagerComponent self)
        {
            return TimeHelper.ClientNow() < self.SkillSingTime;
        }

        public static SkillCDItem GetSkillCD(this SkillManagerComponent self, int skillId)
        {
            for (int i = 0; i < self.SkillCDs.Count; i++)
            {
                if (self.SkillCDs[i].SkillID == skillId)
                {
                    return self.SkillCDs[i];
                }
            }
            return null;
        }

        public static long GetCdTime(this SkillManagerComponent self, int skillId, long nowTime)
        {
            SkillCDItem skillCD = self.GetSkillCD(skillId);
            if (skillCD!=null)
            {
                return skillCD.CDEndTime - nowTime;
            }
            return 0;
        }

        /// <summary>
        /// 清除所有技能和Cd
        /// </summary>
        /// <param name="self"></param>
        public static void ClearSkillAndCd(this SkillManagerComponent self)
        {
            self.SkillCDs.Clear();
            self.OnFinish();
        }

        public static void ClearSkillCD(this SkillManagerComponent self,  int skillId)
        {
            for (int i = 0; i < self.SkillCDs.Count; i++)
            {
                if (self.SkillCDs[i].SkillID == skillId)
                {
                    self.SkillCDs[i].CDEndTime = 0;
                }
            }
        }

        public static void OnUpdate(this SkillManagerComponent self)
        {
            long nowTime = TimeHelper.ServerNow();
            int skillcdcnt = self.SkillCDs.Count;
            for (int i = skillcdcnt - 1; i >= 0; i--)
            {
                if (self.SkillCDs[i].CDEndTime < nowTime)
                {
                    self.SkillCDs.RemoveAt(i);
                }
            }

            int skillcnt = self.Skills.Count;
            for (int i = skillcnt - 1; i >= 0; i--)
            {
                ASkillHandler skillHandler = self.Skills[i];
                if (skillHandler.IsSkillFinied())
                {
                    skillHandler.OnFinished();
                    self.Skills.RemoveAt(i);
                    ObjectPool.Instance.Recycle(skillHandler);
                    continue;
                }
                self.Skills[i].OnUpdate();
            }

            if (self.MainHero)
            {
                EventType.DataUpdate.Instance.DataType = DataType.SkillCDUpdate;
                EventSystem.Instance.PublishClass(EventType.DataUpdate.Instance);
            }
            if (self.Skills.Count == 0 && self.SkillCDs.Count == 0 && self.SkillPublicCDTime < nowTime)
            {
                TimerComponent.Instance?.Remove(ref self.Timer);
            }
        }

        public static void OnFinish(this SkillManagerComponent self)
        {
            int skillcnt = self.Skills.Count;
            for (int i = skillcnt - 1; i >= 0; i--)
            {
                ASkillHandler skillHandler = self.Skills[i];
                skillHandler.OnFinished();
                self.Skills.RemoveAt(i);
                ObjectPool.Instance.Recycle(skillHandler);
            }
            if (self.Skills.Count == 0 && self.SkillCDs.Count == 0)
            {
                TimerComponent.Instance?.Remove(ref self.Timer);
            }
        }

        //发送使用消息的技能
        public static async ETTask<int> SendUseSkill(this SkillManagerComponent self, int skillid, int itemId, int angle, long targetId, float distance, bool checksing = true)
        {
            try
            {
                Unit unit = self.GetParent<Unit>();
                if (self.SkillCmd == null)
                {
                    self.SkillCmd = new C2M_SkillCmd();
                }
                C2M_SkillCmd skillCmd = self.SkillCmd;
                skillCmd.SkillID = skillid;
                skillCmd.TargetAngle = angle;
                skillCmd.TargetID = targetId;
                skillCmd.ItemId = itemId;
                skillCmd.TargetDistance = distance;
                int errorCode = self.CanUseSkill(itemId, skillid);
                if (errorCode != ErrorCode.ERR_Success)
                {
                    HintHelp.GetInstance().ShowHintError(errorCode, self.ZoneScene());
                    return errorCode;       
                }
                if (unit.GetComponent<SingingComponent>().IsSkillSinging(skillid))
                {
                    return ErrorCode.ERR_Success;
                }

                unit.GetComponent<SingingComponent>().BeginSkill();
                SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillid);
                SkillSetComponent skillSetComponent = unit.ZoneScene().GetComponent<SkillSetComponent>();
                if (checksing && skillConfig.SkillFrontSingTime > 0 && !skillSetComponent.IsSkillSingingCancel(skillConfig.Id))
                {
                    unit.GetComponent<SingingComponent>().BeforeSkillSing(skillCmd);
                    unit.ZoneScene().GetComponent<AttackComponent>().RemoveTimer();
                    errorCode =  ErrorCode.ERR_Success;
                }
                else
                {
                    errorCode = await self.ImmediateUseSkill(skillCmd);
                }
                if (errorCode == ErrorCode.ERR_Success)
                {
                    unit.GetComponent<SingingComponent>().AfterSkillSing(skillConfig);
                }
                return errorCode;
            }
            catch (Exception e)
            {
                Log.Error(e);
                return ErrorCode.ERR_NetWorkError;
            }
        }

        public static async ETTask<int> ImmediateUseSkill(this SkillManagerComponent self, C2M_SkillCmd skillCmd)
        {
            long time_1 = TimeHelper.ClientNow();
            Unit unit = self.GetParent<Unit>();
            try
            {
                unit.GetComponent<StateComponent>().SetNetWaitEndTime(time_1 + 200);
                M2C_SkillCmd m2C_SkillCmd = await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(skillCmd) as M2C_SkillCmd;
                if (unit.IsDisposed)
                {
                    return ErrorCode.ERR_NetWorkError;
                }
                unit.GetComponent<StateComponent>().SetNetWaitEndTime(0);
                if (m2C_SkillCmd.Error == 0)
                {
                    BagComponent bagComponent = self.ZoneScene().GetComponent<BagComponent>();
                    SkillSetComponent skillSetComponent = self.ZoneScene().GetComponent<SkillSetComponent>();   
                    int weaponSkill = SkillHelp.GetWeaponSkill(skillCmd.SkillID, UnitHelper.GetEquipType(self.ZoneScene()), skillSetComponent.SkillList);
                    SkillConfig skillWeaponConfig = SkillConfigCategory.Instance.Get(weaponSkill);

                    long addTime = (long)(skillWeaponConfig.SkillRigidity * 1000);
                    long time_2 = TimeHelper.ClientNow();
                    long rigidity = addTime - (time_2 - time_1);
                    rigidity = Math.Max(rigidity, 0);

                    unit.GetComponent<StateComponent>().SetRigidityEndTime(rigidity + time_2);
                }
                else
                {
                    unit.GetComponent<StateComponent>().SetRigidityEndTime(0);
                }
                if (!string.IsNullOrEmpty(m2C_SkillCmd.Message))
                {
                    HintHelp.GetInstance().ShowHint(m2C_SkillCmd.Message);
                }
                return m2C_SkillCmd.Error;
            }
            catch (Exception ex)
            {
                Log.Debug(ex.ToString());
                self.ZoneScene()?.GetComponent<AttackComponent>()?.RemoveTimer();
                return ErrorCode.ERR_NetWorkError;
            }
        }

        /// <summary>
        /// 释放二段技能的时候
        /// </summary>
        /// <param name="self"></param>
        /// <param name="skillId"></param>
        /// <param name="secondId"></param>
        public static void AddSkillSecond(this SkillManagerComponent self, int skillId, int secondId)
        {
            if (self.SkillSecond.ContainsKey(secondId))
            {
                return;
            }
            self.SkillSecond.Add(secondId, skillId); //skillId302  secondId702
        }

        public static void AddSkillCD(this SkillManagerComponent self, int skillId, long cdEndTime, long pulicCD)
        {
            if (self.SkillSecond.ContainsKey(skillId))
            {
                skillId = self.SkillSecond[skillId];    
            }

            //添加技能CD列表
            SkillCDItem skillcd = self.GetSkillCD(skillId);
            if (skillcd == null)
            {
                skillcd = new SkillCDItem();
                self.SkillCDs.Add(skillcd);
            }
            skillcd.SkillID = skillId;
            skillcd.CDEndTime = cdEndTime; ;// + 100;
            self.SkillPublicCDTime = pulicCD; ;// + 100;
            self.AddSkillTimer();
        }

        public static void AddSkillTimer(this SkillManagerComponent self)
        {
            if (self.Timer == 0)
            {
                self.Timer = TimerComponent.Instance.NewFrameTimer(TimerType.SkillTimer, self);
            }
        }

        public static void InitSkill(this SkillManagerComponent self)
        {
            List<SkillInfo> skillInfos = self.t_Skills;
            for (int i = 0; i < skillInfos.Count; i++)
            {
                if (skillInfos[i].SkillEndTime < TimeHelper.ServerNow())
                {
                    continue;
                }
                M2C_UnitUseSkill m2C_UnitUseSkill = new M2C_UnitUseSkill();
                m2C_UnitUseSkill.SkillInfos.Add(skillInfos[i]);
                self.OnUseSkill(m2C_UnitUseSkill);
            }
            self.t_Skills.Clear();
        }

        /// <summary>
        /// CD异常 换客户端时间111111111111111
        /// </summary>
        /// <param name="self"></param>
        /// <param name="skillcmd"></param>
        public static void OnUseSkill(this SkillManagerComponent self, M2C_UnitUseSkill skillcmd )
        {
            Unit unit = self.GetParent<Unit>();
            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillcmd.SkillInfos[0].WeaponSkillID);
          
            if (moveComponent != null && !moveComponent.IsArrived() && skillConfig.IfStopMove == 0)
            {
                moveComponent.Stop();
            }
           
            if (unit.MainHero && !unit.IsSelfRobot())
            {
                self.AddSkillCD( skillcmd.SkillID, skillcmd.CDEndTime, skillcmd.PublicCDTime);

                EventType.DataUpdate.Instance.DataType = DataType.OnSkillUse;
                EventType.DataUpdate.Instance.UpdateValue = skillcmd.SkillID;
                EventSystem.Instance.PublishClass(EventType.DataUpdate.Instance);
            }

            if (unit.Type != UnitType.Player && skillcmd.SkillInfos.Count > 0)
            {
                Unit target = unit.GetParent<UnitComponent>().Get(skillcmd.SkillInfos[0].TargetID);
                if (target != null)
                {
#if !SERVER && NOT_UNITY
                    Vector3 direction = target.Position - unit.Position;
                    float ange = Mathf.Rad2Deg(Mathf.Atan2(direction.x, direction.z));
                    unit.Rotation = Quaternion.Euler(0, ange, 0);
#else
                    Vector3 direction = target.Position - unit.Position;
                    float ange = Mathf.Rad2Deg * Mathf.Atan2(direction.x, direction.z);
                    unit.Rotation = Quaternion.Euler(0, ange, 0);
#endif
                }
                else
                {
                    unit.Rotation = Quaternion.Euler(0, skillcmd.TargetAngle, 0);
                }
            }
            else
            {
                unit.Rotation = Quaternion.Euler(0, skillcmd.TargetAngle, 0);
            }


            //播放对应攻击动作
            if (skillConfig.IfStopMove == 1)
            {
                EventType.PlayAnimator.Instance.Unit = unit;
                EventType.PlayAnimator.Instance.Animator = skillConfig.SkillAnimation;
                Game.EventSystem.PublishClass(EventType.PlayAnimator.Instance);
            }
            else
            {
                bool noMoveSkill = skillConfig.GameObjectName.Equals("Skill_Other_XuanFengZhan_1");
                if (noMoveSkill && skillConfig.SkillType == 1)
                {
                    self.SkillMoveTime = noMoveSkill ? skillConfig.SkillLiveTime + TimeHelper.ClientNow() : 0; 
                }
                
                double singTime = skillConfig.SkillSingTime;
                self.SkillSingTime = singTime == 0f ? 0 : TimeHelper.ClientNow() + (int)(1000f * singTime);

                double rigibTime = skillConfig.SkillRigidity;
                long skillRigibTime = TimeHelper.ClientNow() + (int)(1000f * rigibTime);
                //光之能量 保持在动作的最后一帧
                if (!noMoveSkill && skillConfig.Id >= 61022301 && skillConfig.Id <= 61022306 )
                {
                    self.SkillMoveTime = skillRigibTime;
                }

                if (!ComHelp.IfNull(skillConfig.SkillAnimation))
                {
                    int fsmType = skillConfig.ComboSkillID > 0 ? 5 : 4;
                    if (SkillHelp.NotCombatSkill.Contains(skillConfig.SkillAnimation))
                    {
                        fsmType = 4;
                    }

                    EventType.FsmChange.Instance.FsmHandlerType = fsmType;
                    EventType.FsmChange.Instance.SkillId = skillcmd.SkillInfos[0].WeaponSkillID;
                    EventType.FsmChange.Instance.Unit = unit;
                    Game.EventSystem.PublishClass(EventType.FsmChange.Instance);
                }
            }

            //播放对应技能特效
            for (int i = 0; i < skillcmd.SkillInfos.Count; i++)
            {
                SkillConfig skillConfig1 = SkillConfigCategory.Instance.Get(skillcmd.SkillInfos[i].WeaponSkillID);
                int effctId = skillConfig1.SkillEffectID[0];
                if (effctId != 0 && !EffectConfigCategory.Instance.Contain(effctId))
                {
                    Log.Error($"无效的effectid {effctId}");
                    continue;
                }

                ASkillHandler skillHandler = (ASkillHandler)ObjectPool.Instance.Fetch(SkillDispatcherComponent.Instance.SkillTypes[skillConfig1.GameObjectName]);
                self.Skills.Add(skillHandler);
                skillHandler.OnInit(skillcmd.SkillInfos[i], unit);
            }
            self.AddSkillTimer();
        }

        public static void InterruptSkill(this SkillManagerComponent self, int skillId)
        {
            int skillcnt = self.Skills.Count;
            for (int i = skillcnt - 1; i >= 0; i--)
            {
                ASkillHandler skillHandler = self.Skills[i];
                if (skillHandler.SkillConf.Id != skillId)
                {
                    continue;
                }
                skillHandler.SetSkillState(SkillState.Finished);
            }
        }

        public static void OnSkillSecondResult(this SkillManagerComponent self, M2C_SkillSecondResult message)
        {
            if (message.HurtIds.Count == 0)
            { 
                
            }
        }

        public static bool HaveSkillType(this SkillManagerComponent self, string skilltype)
        {
            int skillcnt = self.Skills.Count;
            for (int i = skillcnt - 1; i >= 0; i--)
            {
                if (self.Skills[i].SkillConf.GameObjectName.Equals(skilltype))
                {
                    return true;
                }
            }
            return false;
        }

        public static int CanUseSkill(this SkillManagerComponent self,int itemId, int skillId)
        {
            Unit unit = self.GetParent<Unit>();

            //检查魔法值
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);
            int skillMp = unit.GetComponent<NumericComponent>().GetAsInt( NumericType.SkillUseMP );
            if (skillConfig.SkillUseMP > 0 && skillMp < skillConfig.SkillUseMP)
            {
                return ErrorCode.ERR_CanNotUseSkill_MP;
            }

            //普攻不检测CD
            if (skillConfig.SkillActType != 0)
            {
                SkillCDItem skillCDList = self.GetSkillCD(skillId);
                //技能冷却中
                if (skillCDList != null)
                {
                    return ErrorCode.ERR_UseSkillInCD5;
                }
                //公共技能冷却
                long leftPublicCD = self.SkillPublicCDTime - TimeHelper.ServerNow();
                if (leftPublicCD > 1500)
                {
                    Log.Error($"leftPublicCD > 1000 {leftPublicCD}");
                    self.SkillPublicCDTime = TimeHelper.ServerNow();
                    leftPublicCD = 0;
                }
                if (itemId==0 && leftPublicCD > 0)
                {
                    return ErrorCode.ERR_UseSkillInCD6;
                }
            }

            StateComponent stateComponent = unit.GetComponent<StateComponent>();
            int errorCode = stateComponent.CanUseSkill(skillConfig, true);
            if (errorCode!=ErrorCode.ERR_Success)
            {
                stateComponent.CheckSilence();
                return errorCode;
            }
            if (self.IsSkillMoveTime())
            {
                return ErrorCode.ERR_SkillMoveTime;
            }
            MapComponent mapComponent = self.ZoneScene().GetComponent<MapComponent>();
            if (itemId > 0 && SceneConfigHelper.UseSceneConfig(mapComponent.SceneTypeEnum))
            {
                SceneConfig sceneConfig = SceneConfigCategory.Instance.Get(mapComponent.SceneId);
                if (sceneConfig.IfUseSkillItem == 1)
                {
                    return ErrorCode.ERR_NoUseItemSkill;
                }
            }

            return 0;
        }
    }

}
