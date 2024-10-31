using ET;
using UnityEngine;
using System.Collections.Generic;

namespace ET
{

    //旋风斩
    public class Skill_Other_XuanFengZhan_1 : SkillHandler
    {
      
        public override void OnInit(SkillInfo skillId, Unit theUnitFrom)
        {
            this.BaseOnInit(skillId, theUnitFrom);
            this.SkillTriggerLastTime = TimeHelper.ServerNow();
        }

        public override void OnExecute()
        {
            this.InitSelfBuff();

            this.OnUpdate();
        }

        public override void OnUpdate()
        {
            //每间隔一段时间触发一次伤害
            long serverNow = TimeHelper.ServerNow();
            if (serverNow - this.SkillTriggerLastTime >= 250)
            {
                this.SkillTriggerLastTime = TimeHelper.ServerNow();
                this.HurtIds.Clear();
                this.UpdateCheckPoint(this.TheUnitFrom.Position);
                this.ExcuteSkillAction();
            }

            //技能存在时间
            if (serverNow > this.SkillEndTime)
            {
                this.SetSkillState(SkillState.Finished);
                return;
            }

            this.CheckChiXuHurt();
        }

        public override void OnFinished()
        {
            this.Clear();
        }
    }
}
