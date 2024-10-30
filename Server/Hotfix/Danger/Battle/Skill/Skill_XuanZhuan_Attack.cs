using UnityEngine;

namespace ET
{
    //旋转攻击, 不能控制方向
    public class Skill_XuanZhuan_Attack : SkillHandler
    {

        public override void OnInit(SkillInfo skillId, Unit theUnitFrom)
        {
            this.BaseOnInit(skillId, theUnitFrom);

            this.ICheckShape.Clear();
            string[] paraminfos = this.SkillConf.GameObjectParameter.Split(';');
            int angle = this.SkillInfo.TargetAngle;
            int range = paraminfos.Length > 1 ? int.Parse(paraminfos[0]) : 0;
            int number = paraminfos.Length > 1 ? int.Parse(paraminfos[1]) : 1;
            int delta = number > 1 ? range / (number - 1) : 0;
            int starAngle = angle - (int)(range * 0.5f);
            /// 写死3 错误做法 
            for (int i = 0; i < number; i++)
            {
                this.ICheckShape.Add(this.CreateCheckShape(starAngle + i * delta));
            }
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
            string[] paraminfos = this.SkillConf.GameObjectParameter.Split(';');
            int angle = this.SkillInfo.TargetAngle;
            int speed = paraminfos.Length > 1 ? int.Parse(paraminfos[0]) : 0;   //每秒转多少角度
            int number = paraminfos.Length > 1 ? int.Parse(paraminfos[1]) : 1;

            //两条线的间隔
            int delta = 0;
            int starAngle = angle;
            if (number > 1)
            {
                delta = Mathf.FloorToInt(360f / number);
                starAngle = angle - 180;
            }
            long passTime = serverNow - this.SkillBeginTime;
            int addrangle = (int)(passTime * speed * 0.001f );
            for (int i = 0; i < this.ICheckShape.Count; i++)
            {
                int anglea_1 = starAngle + i * delta + addrangle;
                (this.ICheckShape[i] as Rectangle).s_forward = (Quaternion.Euler(0, anglea_1, 0) * Vector3.forward).normalized; ;
            }
            this.TheUnitFrom.Rotation = Quaternion.Euler(0, angle + addrangle, 0);

            this.ExcuteSkillAction();
            this.CheckChiXuHurt();
        }

        public override void OnFinished()
        {
            this.Clear();
        }
    }
}
