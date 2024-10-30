using UnityEngine;

namespace ET
{

    [SkillHandler]
    public class Skill_XuanZhuan_Attack_2 : Skill_Action_Common
    {
        public override void OnInit(SkillInfo skillId, Unit theUnitFrom)
        {
            this.BaseOnInit(skillId, theUnitFrom);

            this.OnExecute();
        }

        public override void OnExecute()
        {
            int angle = this.SkillInfo.TargetAngle;
            this.PlaySkillEffects(this.TheUnitFrom.Position ,angle);
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

#if !SERVER && NOT_UNITY
            int angle = (int)Quaternion.QuaternionToEuler(this.TheUnitFrom.Rotation).y;
#else
            int angle = (int)(this.TheUnitFrom.Rotation.eulerAngles).y;
#endif
            for (int i = 0; i < this.EffectInstanceId.Count; i++)
            {
                EventType.SkillEffectMove.Instance.Postion = this.TargetPosition;
                EventType.SkillEffectMove.Instance.Unit = this.TheUnitFrom;
                EventType.SkillEffectMove.Instance.Angle = angle;
                EventType.SkillEffectMove.Instance.EffectInstanceId = this.EffectInstanceId[i];
                EventSystem.Instance.PublishClass(EventType.SkillEffectMove.Instance);
            }
            this.BaseOnUpdate();
        }

        public override void OnFinished()
        {
            this.EndSkillEffect();
        }
    }
}
