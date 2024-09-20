using UnityEngine;
using System;
using System.Collections.Generic;

namespace ET
{

    [Timer(TimerType.FsmTimer)]
    public class FsmTimer : ATimer<FsmComponent>
    {
        public override void Run(FsmComponent self)
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

    public class FsmComponentAwakeSystem: AwakeSystem<FsmComponent>
    {
        public override void Awake(FsmComponent self)
        {
            Unit unit = self.GetParent<Unit>();
            self.Animator = unit.GetComponent<AnimatorComponent>();
            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            bool idle = moveComponent == null || moveComponent.IsArrived();
            self.ChangeState(idle ? FsmStateEnum.FsmIdleState : FsmStateEnum.FsmRunState);
            self.WaitIdleTime = 0;
        }
    }

    public class FsmComponentDestroySystem : DestroySystem<FsmComponent>
    {
        public override void Destroy(FsmComponent self)
        {
            self.EndTimer();
        }
    }

    /// <summary>
    /// 适用于动画切换的栈式状态机
    /// </summary>
    public static class FsmComponentSystem
    {
        public static void Check(this FsmComponent self)
        {
            if (self.Animator == null)
            {
                int monsterId = self.GetParent<Unit>().ConfigId;
                Log.Error($"{self.Animator == null} { monsterId}");
                self.EndTimer();
                return;
            }

            SkillManagerComponent skillManagerComponent = self.GetParent<Unit>().GetComponent<SkillManagerComponent>(); 
            if (skillManagerComponent.SkillMoveTime != 0 && TimeHelper.ClientNow() >= skillManagerComponent.SkillMoveTime)
            {
                skillManagerComponent.SkillMoveTime = 0;
                if (self.CurrentFsm == FsmStateEnum.FsmIdleState)
                {
                    self.SetIdleState();
                }
                if (self.CurrentFsm == FsmStateEnum.FsmSkillState)  //光之能量 保持在动作的最后一帧
                {
                    self.SetIdleState();
                }
                if (self.CurrentFsm == FsmStateEnum.FsmRunState)
                {
                    self.SetRunState();
                }
                self.EndTimer();
            }
            if (skillManagerComponent.SkillSingTime > 0 && TimeHelper.ClientNow() >= skillManagerComponent.SkillSingTime)
            {
                skillManagerComponent.SkillSingTime = 0;
                self.SetIdleState();
                self.EndTimer();
            }
            if (self.WaitIdleTime > 0 && TimeHelper.ClientNow() >= self.WaitIdleTime)   //连击回Idle
            {
                self.WaitIdleTime = 0;
                self.SetIdleState();
                self.EndTimer();
            }
        }

        public static void EndTimer(this FsmComponent self)
        {
            TimerComponent.Instance?.Remove(ref self.Timer);
        }

        public static void BeginTimer(this FsmComponent self)
        {
            if (self.Timer == 0)
            {
                self.Timer = TimerComponent.Instance.NewFrameTimer(TimerType.FsmTimer, self);
            }
        }

        public static void ChangeState(this FsmComponent self, int targetFsm, int skillid = 0)
        {
            if (self.Animator == null)
            {
                return;
            }
            switch (self.CurrentFsm)
            {
                case FsmStateEnum.FsmRunState:
                    if (targetFsm == FsmStateEnum.FsmRunState)
                    {
                        return;
                    }
                    break;
                case FsmStateEnum.FsmComboState:
                    self.WaitIdleTime =0;
                    TimerComponent.Instance.Remove(ref self.Timer);
                    break;
                case FsmStateEnum.FsmDeathState:
                    self.Animator.SetBoolValue("Death", false);
                    break;
                case FsmStateEnum.FsmNpcSpeak:
                    self.Animator.SetBoolValue("Idle", true);
                    break;
                case FsmStateEnum.FsmSinging:
                    self.Animator.SetBoolValue("Run", false);
                    self.Animator.SetBoolValue("Idle", true);
                    break;
                case FsmStateEnum.FsmSkillState:
                    self.Animator.SetBoolValue("Run", false);
                    self.Animator.SetBoolValue("Idle", true);
                    break;
                default:
                    break;
            }

            switch (targetFsm)
            {
                case FsmStateEnum.FsmComboState:
                    //this.ClearnAnimator();
                    self.Animator.SetBoolValue("Idle", false);
                    self.Animator.SetBoolValue("Run", false);
                    self.OnEnterFsmComboState(skillid);
                    break;
                case FsmStateEnum.FsmDeathState:
                    self.Animator.SetBoolValue("Idle", false);
                    self.Animator.SetBoolValue("Death", true);
                    self.Animator.Play("Death");
                    break;
                case FsmStateEnum.FsmHui:
                    self.Animator.SetBoolValue("Idle", false);
                    self.Animator.Play("Hui");
                    break;
                case FsmStateEnum.FsmIdleState:
                    self.OnEnterIdleState();
                    break;
                case FsmStateEnum.FsmNpcSpeak:
                    self.Animator.SetBoolValue("Idle", false);
                    self.Animator.SetBoolValue("Run", false);
                    self.Animator.Play("Speak");
                    break;
                case FsmStateEnum.FsmRunState:
                    self.OnEnterFsmRunState();
                    break;
                case FsmStateEnum.FsmShiQuItem:
                    self.Animator.SetBoolValue("Idle", false);
                    self.Animator.SetBoolValue("Run", false);
                    self.Animator.Play("ShiQu");
                    break;
                case FsmStateEnum.FsmSinging:
                    self.Animator.SetBoolValue("Idle", false);
                    self.Animator.SetBoolValue("Run", false);
                    self.Animator.Play("YinChang");
                    break;
                case FsmStateEnum.FsmSkillState:
                    self.OnEnterFsmSkillState(skillid);
                    break;
                case FsmStateEnum.FsmHorse:
                    self.Animator.SetBoolValue("Idle", false);
                    self.Animator.SetBoolValue("Run", false);
                    self.Animator.Play("Horse");
                    break;
                default:
                    break;
            }
            self.CurrentFsm = targetFsm;
        }

        public static void OnEnterFsmSkillState(this FsmComponent self, int skillid)
        {
            SkillManagerComponent skillManagerComponent = self.GetParent<Unit>().GetComponent<SkillManagerComponent>();
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillid);
            if (skillManagerComponent.SkillMoveTime > TimeHelper.ClientNow()
               || skillManagerComponent.SkillSingTime > TimeHelper.ClientNow())
            {
                self.Animator.SetBoolValue("Idle", false);
                self.Animator.SetBoolValue("Run", false);
                self.BeginTimer();
            }
            else
            {
                self.Animator.SetBoolValue("Run", false);
                self.Animator.SetBoolValue("Idle", true);
            }
            self.Animator.Play(skillConfig.SkillAnimation, skillConfig.Id, 1f, true);
        }

        public static void OnEnterFsmRunState(this FsmComponent self)
        {
            self.LastAnimator = string.Empty;
            Unit unit = self.GetParent<Unit>();
            SkillManagerComponent skillManagerComponent = unit.GetComponent<SkillManagerComponent>();
            if (TimeHelper.ClientNow() > skillManagerComponent.SkillMoveTime)
            {
                self.SetRunState();
            }
        }

        public static void OnEnterIdleState(this FsmComponent self)
        {
            self.LastAnimator = string.Empty;
            SkillManagerComponent skillManagerComponent = self.GetParent<Unit>().GetComponent<SkillManagerComponent>();
            if (skillManagerComponent == null || TimeHelper.ClientNow() > skillManagerComponent.SkillMoveTime)
            {
                self.SetIdleState();
            }
        }

        public static void SetHorseState(this FsmComponent self)
        {
            self.Animator.SetBoolValue("Run", false);
            self.Animator.SetBoolValue("Idle", false);
            self.Animator.Animator.Play("ZuoQi");
        }

        public static void SetIdleState(this FsmComponent self)
        {
            self.Animator.SetBoolValue("Run", false);
            self.Animator.SetBoolValue("Idle", true);
            self.Animator.Play("Idle");
        }

        public static void SetRunState(this FsmComponent self)
        {
            self.Animator.SetBoolValue("Idle", false);
            self.Animator.SetBoolValue("Run", true);
            self.Animator.Play("Run");
        }

        public static void  OnEnterFsmComboState(this FsmComponent self, int skillid)
        {
            Unit unit = self.GetParent<Unit>();
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillid);
            //int EquipType = (int)ItemEquipType.Common;
            //int itemId = (int)unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Weapon);
            //if (itemId != 0)
            //{
            //    ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemId);
            //    EquipType = itemConfig.EquipType;
            //}

            //1:剑 2:刀 11 默认11  //(EquipType == 2)
            if (unit.ConfigId == 1) 
            {
                string boolAnimation = skillConfig.SkillAnimation;
                if (boolAnimation == "Act_11")
                {
                    boolAnimation = "Act_1";
                }
                if (boolAnimation == "Act_12")
                {
                    boolAnimation = "Act_2";
                }
                if (boolAnimation == "Act_13")
                {
                    boolAnimation = "Act_3";
                }

                string curAckAnimation = String.Empty;
                AnimatorStateInfo animatorStateInfo = self.Animator.Animator.GetCurrentAnimatorStateInfo(0);
                foreach (var item in SkillHelp.AckExitTime)
                {
                    if (animatorStateInfo.IsName(item.Key))
                    {
                        curAckAnimation = item.Key;
                        break;
                    }
                }
                if (self.LastAnimator == skillConfig.SkillAnimation)
                {
                    self.Animator.SetBoolValue("Act_1", false);
                    self.Animator.SetBoolValue("Act_2", false);
                    self.Animator.SetBoolValue("Act_3", false);
                    self.Animator.Play(skillConfig.SkillAnimation, skillConfig.Id);
                }
                else if (curAckAnimation == String.Empty)
                {
                    self.Animator.SetBoolValue("Act_1", false);
                    self.Animator.SetBoolValue("Act_2", false);
                    self.Animator.SetBoolValue("Act_3", false);
                    self.Animator.Play(skillConfig.SkillAnimation, skillConfig.Id);
                }
                else
                {
                    self.Animator.SetBoolValue("Act_1", false);
                    self.Animator.SetBoolValue("Act_2", false);
                    self.Animator.SetBoolValue("Act_3", false);
                    self.Animator.SetBoolValue(boolAnimation, true);
                }
                
                self.LastAnimator = skillConfig.SkillAnimation;
                self.WaitIdleTime = TimeHelper.ClientNow() + SkillHelp.AckExitTime[skillConfig.SkillAnimation];
                TimerComponent.Instance.Remove(ref self.Timer);
                self.BeginTimer();
            }
            else
            {
                self.Animator.Play(skillConfig.SkillAnimation, skillConfig.Id);
            }
        }

        public static void ClearnAnimator(this FsmComponent self)
        {
            //重置参数
            self.Animator.SetBoolValue("Idle", false);
            self.Animator.SetBoolValue("Run", false);
            self.Animator.SetBoolValue("Death", false);
            self.Animator.SetBoolValue("CriAct", false);
            self.Animator.SetBoolValue("Act_1", false);
            self.Animator.SetBoolValue("Act_2", false);
            self.Animator.SetBoolValue("Act_3", false);
        }
    }
}