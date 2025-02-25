using System;
using UnityEngine;

namespace ET
{
     
    [EffectHandler]
    public class RoleSkillEffect: AEffectHandler
    {
        public override void OnInit(EffectData effectData, Unit theUnitBelongto)
        {
            this.EffectPath = string.Empty;
            this.EffectObj = null;
            this.EffectData = effectData;
            this.EffectState = BuffState.Running;
            this.TheUnitBelongto = theUnitBelongto;
            this.EffectConfig = EffectConfigCategory.Instance.Get(effectData.EffectId);
            this.EffectData.EffectBeginTime = effectData.EffectBeginTime > 0 ? effectData.EffectBeginTime : TimeHelper.ServerNow();
            this.EffectEndTime = TimeHelper.ServerNow() +  this.EffectConfig.SkillEffectLiveTime ;

            this.OnUpdate();
        }

        public void OnLoadGameObject(GameObject gameObject, long instanceId)
        {
            try
            {
                if (this.EffectState != BuffState.Running || instanceId != this.InstanceId)
                {
                    if (gameObject != null)
                    {
                        GameObject.DestroyImmediate(gameObject);
                    }
                    return;
                }
                this.EffectObj = gameObject;
                this.EffectObj.name = this.EffectConfig.EffectName;
                if (this.EffectData.InstanceId == 0  || gameObject == null)
                {
                    this.EffectState = BuffState.Finished;
                }
                if (this.TheUnitBelongto == null || this.TheUnitBelongto.IsDisposed)
                {
                    this.EffectState = BuffState.Finished;
                }
                if (this.EffectState == BuffState.Finished)
                {
                    return;
                }

                int skillParentID = this.EffectConfig.SkillParent;
                //显示特效大小。
                //if (this.EffectData.SkillId != 0)
                //{
                //    SkillConfig skillConfig = SkillConfigCategory.Instance.Get(this.EffectData.SkillId);
                //    int rangeType = skillConfig.DamgeRangeType;       //技能范围类型
                //    float[] rangeValue = FunctionHelp.DoubleArrToFloatArr(skillConfig.DamgeRange);          //技能范围
                //    this.AddCollider(this.EffectObj, rangeType, rangeValue);            //添加客户端碰撞显示
                //}
                switch (skillParentID)
                {
                    //跟随玩家
                    case 0:
                        HeroTransformComponent heroTransformComponent = this.TheUnitBelongto.GetComponent<HeroTransformComponent>();
                        if (heroTransformComponent == null)
                        {
                            this.EffectState = BuffState.Finished;
                            return;
                        }
                        Transform tParent = heroTransformComponent.GetTranform(this.EffectConfig.SkillParentPosition);
                        if (tParent == null)
                        {
                            this.EffectState = BuffState.Finished;
                            return;
                        }

                        this.EffectObj.transform.SetParent(tParent);
                        this.EffectObj.transform.localPosition = Vector3.zero;
                        this.EffectObj.transform.localScale = Vector3.one;
                        float angle = this.EffectData.TargetAngle != 0 ? this.EffectData.TargetAngle - this.TheUnitBelongto.Rotation.eulerAngles.y    : 0f;
                        this.EffectObj.transform.localRotation = Quaternion.Euler(0, angle, 0);
                        break;
                    //不跟随玩家
                    case 1:
                        angle = this.EffectData.EffectAngle != 0 ? this.EffectData.EffectAngle : this.EffectData.TargetAngle;
                        this.EffectObj.transform.SetParent(GlobalComponent.Instance.UnitEffect);
                        this.EffectObj.transform.position = this.EffectData.EffectPosition;
                        this.EffectObj.transform.localScale = Vector3.one;
                        this.EffectObj.transform.localRotation = Quaternion.Euler(0, angle, 0);
                        break;
                    //实时跟随玩家位置,但是不跟随旋转
                    case 2:
                        this.EffectObj.transform.SetParent(GlobalComponent.Instance.UnitEffect);
                        this.EffectObj.transform.position = this.TheUnitBelongto.Position;
                        this.EffectObj.transform.localScale = Vector3.one;
                        this.EffectObj.transform.localRotation = Quaternion.Euler(0, this.EffectData.TargetAngle, 0);
                        break;
                    //实时跟随位置,无指定绑点
                    case 3:
                        this.EffectObj.transform.SetParent(GlobalComponent.Instance.UnitEffect);
                        this.EffectObj.transform.position = this.TheUnitBelongto.Position;
                        this.EffectObj.transform.localScale = Vector3.one;
                        this.EffectObj.transform.localRotation = Quaternion.Euler(0, this.EffectData.TargetAngle, 0);
                        break;
                    //闪电链特效
                    case 4:
                        Unit unitTarget = null;
                        ChainLightningComponent chainLightningComponent = this.AddComponent<ChainLightningComponent, GameObject>(EffectObj);
                        heroTransformComponent = this.TheUnitBelongto.GetComponent<HeroTransformComponent>();
                        if (heroTransformComponent == null)
                        {
                            this.EffectState = BuffState.Finished;
                            return;
                        }
                        chainLightningComponent.Start = heroTransformComponent.GetTranform(PosType.Center);
                        if (this.EffectData.TargetID != 0)
                        {
                            unitTarget = this.TheUnitBelongto.GetParent<UnitComponent>().Get(this.EffectData.TargetID);
                            if (unitTarget == null)
                            {
                                this.EffectState = BuffState.Finished;
                                return;
                            }
                            chainLightningComponent.UsePosition = false;
                            chainLightningComponent.End = unitTarget.GetComponent<HeroTransformComponent>().GetTranform(PosType.Center);
                            chainLightningComponent.OnUpdate();
                        }
                        else
                        {
                            chainLightningComponent.UsePosition = true;
                            chainLightningComponent.EndPosition = this.EffectData.EffectPosition;
                            chainLightningComponent.OnUpdate();
                        }
                        break;
                }

                this.EffectObj.SetActive(true);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        /// <summary>
        /// 实例化特效
        /// </summary>
        public  void PlayEffect()
        {
            if (this.EffectData.InstanceId == 0)
            {
                return;
            }
            string effectFileName = "";
            switch (this.EffectConfig.EffectType) 
            {
                //技能特效
                case 1:
                    effectFileName = "SkillEffect/";
                    break;
                //受击特效
                case 2:
                    effectFileName = "SkillHitEffect/";
                    break;
                //技能特效
                case 3:
                    effectFileName = "SkillEffect/";
                    break;
            }

            string effectNamePath  = effectFileName + this.EffectConfig.EffectName;
            this.EffectPath = StringBuilderHelper.GetEffetPath(effectNamePath);
            GameObjectPoolComponent.Instance.AddLoadQueue(EffectPath, this.InstanceId, this.OnLoadGameObject);
        }

        public override void OnUpdate()
        {
            if (this.EffectState == BuffState.Finished)
            {
                return;
            }
            long serverTime = TimeHelper.ServerNow();
            float passTime = (serverTime - this.EffectData.EffectBeginTime) * 0.001f;
            if (passTime < this.EffectConfig.SkillEffectDelayTime)
            {
                return;
            }
            if (string.IsNullOrEmpty(this.EffectPath))
            {
                this.PlayEffect();
            }
            if (serverTime > this.EffectEndTime)
            {
                this.EffectState = BuffState.Finished;
                return;
            }
            if (this.TheUnitBelongto == null || this.TheUnitBelongto.IsDisposed || this.EffectData.InstanceId == 0)
            {
                this.EffectState = BuffState.Finished;
                return;
            }
            int skillParentID = this.EffectConfig.SkillParent;
            if (skillParentID == 4)//闪电链
            {
                if (this.EffectData.TargetID != 0 && null == this.TheUnitBelongto.GetParent<UnitComponent>().Get(EffectData.TargetID))
                {
                    this.EffectState = BuffState.Finished;
                    return;
                }
            }

            if (this.EffectConfig.HideTime > 0 && this.EffectObj!=null)
            {
                this.HideObjTime += Time.deltaTime;
                if (this.HideObjTime >= this.EffectConfig.HideTime)
                {
                    this.HideObjTime = 0;
                    this.EffectObj.SetActive(false);
                    this.EffectObj.SetActive(true);
                }
            }
        }

        public override void OnFinished()
        {
            GameObjectPoolComponent.Instance.RecoverGameObject(this.EffectPath, this.EffectObj);
            this.EffectState = BuffState.Finished;
            this.EffectData.InstanceId = 0;
            this.TheUnitBelongto = null; 
            this.EffectObj = null;
            this.EffectPath = String.Empty;
            this.EffectEndTime = 0;
        }

    }
}