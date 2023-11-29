﻿namespace ET
{

    [SkillHandler]
    public class Skill_Action_Common : ASkillHandler
    {
     
        public override void OnInit(SkillInfo skillId, Unit theUnitFrom)
        {
            this.BaseOnInit(skillId, theUnitFrom);
            this.OnExecute();

            if (theUnitFrom.MainHero && this.SkillConf.SkillType == 1 && SkillHelp.havePassiveSkillType(this.SkillConf.PassiveSkillType, 1))
            {
                EventType.DataUpdate.Instance.DataType = DataType.SkillBeging;
                EventType.DataUpdate.Instance.DataParamString = this.SkillConf.Id.ToString();
                EventSystem.Instance.PublishClass(EventType.DataUpdate.Instance);
            }
        }

        public override void OnExecute()
        {
            this.PlaySkillEffects(this.TargetPosition);
            this.OnShowSkillIndicator(this.SkillInfo);
            this.OnUpdate();
        }

        public override void OnUpdate()
        {
            this.BaseOnUpdate();
        }

        public override void OnFinished()
        {
            if (this.TheUnitFrom.MainHero && this.SkillConf.SkillType == 1 && SkillHelp.havePassiveSkillType(this.SkillConf.PassiveSkillType, 1))
            {
                EventType.DataUpdate.Instance.DataType = DataType.SkillFinish;
                EventType.DataUpdate.Instance.DataParamString = this.SkillConf.Id.ToString();
                EventSystem.Instance.PublishClass(EventType.DataUpdate.Instance);
            }

            this.EndSkillEffect();
            this.SkillInfo = null;
        }
    }
}
