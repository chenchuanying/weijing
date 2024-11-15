using System;
using UnityEngine;

namespace ET
{

    /// <summary>
    /// 置空Buff
    /// </summary>
    [BuffHandler]
    public class RoleBuff_Bounce : RoleBuff_Base
    {

        public override void OnExecute()
        {
            this.EffectInstanceId = this.PlayBuffEffects();
            this.BuffState = BuffState.Running;
           
            EventType.BuffBounce.Instance.Unit = this.TheUnitBelongto;
            EventType.BuffBounce.Instance.ZoneScene = this.TheUnitBelongto.ZoneScene();
            EventType.BuffBounce.Instance.OperateType = 1;
            EventSystem.Instance.PublishClass(EventType.BuffBounce.Instance);

            this.StartPosition = this.TheUnitBelongto.Position;
            this.BuffData.TargetPostion = this.StartPosition + Vector3.up * (float)this.mSkillBuffConf.buffParameterValue;

            ChangePosition().Coroutine();
        }

        public async ETTask ChangePosition()
        {
            //上升 0.3 下降0.2  置空0.5
            float shangsheng = 0.3f;
            float zhikong = 0.5f;
            //float xiajiang = 0.2f;
            while (this.BuffState == BuffState.Running)
            {
                this.BaseOnUpdate();
                long leftTime =  this.BuffEndTime - TimeHelper.ServerNow() ;
                if (leftTime <= 0)
                {
                    this.TheUnitBelongto.Position = this.StartPosition;
                    break;
                }

                float progress = 1f  - (float)(leftTime * 1f / this.mSkillBuffConf.BuffTime);
                if (progress <= shangsheng)
                {
                    //上升
                    this.TheUnitBelongto.Position = this.StartPosition + (this.BuffData.TargetPostion - this.StartPosition) * (progress / shangsheng);
                }
                else if (progress < (shangsheng + zhikong))
                {
                    //do nothing
                }
                else
                {
                    //下降
                    this.TheUnitBelongto.Position = this.StartPosition + (this.BuffData.TargetPostion - this.StartPosition) * ((1f - progress) / (shangsheng + zhikong));
                }
                await TimerComponent.Instance.WaitFrameAsync();
                if (this.BuffState != BuffState.Running)
                {
                    break;
                }
            }
        }


        public override void OnFinished()
        {
            EventType.BuffBounce.Instance.Unit = this.TheUnitBelongto;
            EventType.BuffBounce.Instance.ZoneScene = this.TheUnitBelongto.ZoneScene();
            EventType.BuffBounce.Instance.OperateType = 2;
            EventSystem.Instance.PublishClass(EventType.BuffBounce.Instance);

            base.OnFinished();
        }
    }
}
