using UnityEngine;

namespace ET
{

    //旋转攻击, 可以控制方向
    public class Skill_XuanZhuan_Attack_2 : SkillHandler
    {

        public override void OnInit(SkillInfo skillId, Unit theUnitFrom)
        {
            this.BaseOnInit(skillId, theUnitFrom);
            this.SkillTriggerInvelTime = 1000;
            this.SkillTriggerLastTime = TimeHelper.ServerNow();
            OnExecute();
        }

        public override void OnExecute()
        {
            this.InitSelfBuff();
            this.OnUpdate();
        }

        public override void OnUpdate()
        {
            long serverNow = TimeHelper.ServerNow();
            //根据技能效果延迟触发伤害
            if (serverNow < this.SkillExcuteHurtTime)
            {
                return;
            }

            //根据技能存在时间设置其结束状态
            if (serverNow > this.SkillEndTime)
            {
                this.SetSkillState(SkillState.Finished);
                return;
            }


            //每间隔一段时间触发一次伤害
            if (serverNow - this.SkillTriggerLastTime >= this.SkillTriggerInvelTime)
            {
                this.SkillTriggerLastTime = TimeHelper.ServerNow();
                this.HurtIds.Clear();

                for (int i = 0; i < this.ICheckShape.Count; i++)
                {
                    // (int)Quaternion.QuaternionToEuler().y;
                    // (this.ICheckShape[i] as Rectangle).s_forward = (Quaternion.Euler(0, anglea_1, 0) * Vector3.forward).normalized; 
                    (this.ICheckShape[i] as Rectangle).s_forward = (this.TheUnitFrom.Rotation * Vector3.forward).normalized;
                }
                this.ExcuteSkillAction();
            }

            this.CheckChiXuHurt();
        }

        public override void OnFinished()
        {
            this.Clear();
        }

    }
}
