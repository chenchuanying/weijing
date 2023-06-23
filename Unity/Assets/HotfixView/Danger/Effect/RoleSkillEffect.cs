using System;
using UnityEngine;

namespace ET
{
     
    [EffectHandler]
    public class RoleSkillEffect: AEffectHandler
    {
        public override void OnInit(EffectData effectData, Unit theUnitBelongto)
        {
            this.EffectPath = "";
            this.EffectObj = null;
            this.PassTime = 0f;
            this.EffectData = effectData;
            this.EffectState = BuffState.Running;
            this.TheUnitBelongto = theUnitBelongto;
            this.EffectConfig = EffectConfigCategory.Instance.Get(effectData.EffectId);
            this.EffectEndTime = this.EffectConfig.SkillEffectLiveTime * 0.001f;
            this.EffectDelayTime = (float)this.EffectConfig.SkillEffectDelayTime;
            this.OnUpdate();
        }

        public void OnLoadGameObject(GameObject gameObject, long instanceId)
        {
            try
            {
                this.EffectObj = gameObject;
                this.EffectObj.name = $"{this.EffectConfig.EffectName}_{instanceId}";
                if (this.EffectData.InstanceId == 0 || instanceId != this.InstanceId || gameObject == null)
                {
                    this.EffectState = BuffState.Finished;
                }
                if (this.TheUnitBelongto == null || this.TheUnitBelongto.IsDisposed)
                {
                    this.EffectState = BuffState.Finished;
                }
                if (this.EffectState == BuffState.Finished)
                {
                    GameObject.Destroy(gameObject);
                    return;
                }
                int skillParentID = this.EffectConfig.SkillParent;
                if (this.EffectData.SkillId != 0)
                {
                    SkillConfig skillConfig = SkillConfigCategory.Instance.Get(this.EffectData.SkillId);
                    int rangeType = skillConfig.DamgeRangeType;       //技能范围类型
                    float[] rangeValue = FunctionHelp.DoubleArrToFloatArr(skillConfig.DamgeRange);          //技能范围
                    this.AddCollider(this.EffectObj, rangeType, rangeValue);            //添加客户端碰撞显示
                }
                switch (skillParentID)
                {
                    //跟随玩家
                    case 0:
                        HeroTransformComponent heroTransformComponent = this.TheUnitBelongto.GetComponent<HeroTransformComponent>();
                        if (heroTransformComponent == null)
                        {
                            GameObject.Destroy(gameObject);
                            return;
                        }
                        Transform tParent = heroTransformComponent.GetTranform((PosType)Enum.Parse(typeof(PosType), this.EffectConfig.SkillParentPosition));
                        if (tParent == null)
                        {
                            GameObject.Destroy(gameObject);
                            return;
                        }
                        this.EffectObj.transform.SetParent(tParent);
                        this.EffectObj.transform.localPosition = Vector3.zero;
                        this.EffectObj.transform.localScale = Vector3.one;
                        this.EffectObj.transform.localRotation = Quaternion.Euler(Vector3.zero);
                        break;
                    //不跟随玩家
                    case 1:
                        float angle = this.EffectData.EffectAngle != 0 ? this.EffectData.EffectAngle : this.EffectData.TargetAngle;
                        this.EffectObj.transform.SetParent(GlobalComponent.Instance.UnitEffect);
                        this.EffectObj.transform.position = EffectData.EffectPosition;
                        this.EffectObj.transform.localScale = Vector3.one;
                        this.EffectObj.transform.localRotation = Quaternion.Euler(0, angle, 0);
                        break;
                    //实时跟随玩家位置,但是不跟随旋转
                    case 2:
                        this.EffectObj.transform.SetParent(GlobalComponent.Instance.UnitEffect);
                        this.EffectObj.transform.position = this.TheUnitBelongto.Position;
                        this.EffectObj.transform.localScale = Vector3.one;
                        this.EffectObj.transform.localRotation = Quaternion.Euler(0, EffectData.TargetAngle, 0);
                        break;
                    //实时跟随位置,无指定绑点
                    case 3:
                        this.EffectObj.transform.SetParent(GlobalComponent.Instance.UnitEffect);
                        this.EffectObj.transform.position = this.TheUnitBelongto.Position;
                        this.EffectObj.transform.localScale = Vector3.one;
                        this.EffectObj.transform.localRotation = Quaternion.Euler(0, EffectData.TargetAngle, 0);
                        break;
                    //闪电链特效
                    case 4:
                        Unit unitTarget = null;
                        ChainLightningComponent chainLightningComponent = this.AddComponent<ChainLightningComponent, GameObject>(EffectObj);
                        heroTransformComponent = this.TheUnitBelongto.GetComponent<HeroTransformComponent>();
                        if (heroTransformComponent == null)
                        {
                            GameObject.Destroy(gameObject);
                            return;
                        }
                        chainLightningComponent.Start = heroTransformComponent.GetTranform(PosType.Center);
                        if (this.EffectData.TargetID != 0)
                        {
                            unitTarget = this.TheUnitBelongto.GetParent<UnitComponent>().Get(EffectData.TargetID);
                            if (unitTarget == null)
                            {
                                GameObject.Destroy(gameObject);
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
            Log.Debug("PlayEffect实例化碰撞特效:" + this.EffectConfig.EffectName);
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
            Log.Debug("实例化碰撞特效:" + effectNamePath);
            EffectPath = ABPathHelper.GetEffetPath(effectNamePath);
            GameObjectPoolComponent.Instance.AddLoadQueue(EffectPath, this.InstanceId, this.OnLoadGameObject);
        }

        public override void OnUpdate()
        {
            //只有不是永久Buff的情况下才会执行Update判断
            base.OnUpdate();

            if (this.EffectState == BuffState.Finished)
            {
                this.OnFinished();
                return;
            }
            if (this.EffectDelayTime >= 0f && this.PassTime > this.EffectDelayTime)
            {
                this.EffectDelayTime = -1f;
               
                this.PlayEffect();
            }
            if (this.PassTime > this.EffectEndTime)
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
            this.EffectState = BuffState.Waiting;
            this.EffectData.InstanceId = 0;
            this.TheUnitBelongto = null;
            this.EffectPath = String.Empty;
            this.EffectEndTime = 0;
            this.PassTime = 0;
        }

    }
}