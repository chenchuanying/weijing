using System;
using System.Collections.Generic;
using static UnityEngine.UI.CanvasScaler;

namespace ET
{
    [Timer(TimerType.Effectimer)]
    public class EffectViewTimer : ATimer<EffectViewComponent>
    {
        public override void Run(EffectViewComponent self)
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


    public class EffectViewCompoenntAwakeSystem : AwakeSystem<EffectViewComponent>
    {
        public override void Awake(EffectViewComponent self)
        {
            self.Effects.Clear();
            self.InitEffect();
        }
    }

    public class EffectViewCompoenntDestroySystem : DestroySystem<EffectViewComponent>
    {
        public override void Destroy(EffectViewComponent self)
        {
            self.OnDispose();
        }
    }

    public static class EffectViewComponentSystem
    {

        public static void OnDispose(this EffectViewComponent self)
        {
            for (int i = self.Effects.Count - 1; i >= 0; i--)
            {
                AEffectHandler aEffectHandler = self.Effects[i];
                aEffectHandler.OnFinished();
                aEffectHandler.Dispose();
                self.Effects.RemoveAt(i);
            }
            TimerComponent.Instance?.Remove(ref self.Timer);
        }

        public static void RemoveEffect(this EffectViewComponent self, int effectTypeEnum)
        {
            for (int i = self.Effects.Count - 1; i >= 0; i--)
            {
                if (self.Effects[i].EffectData.EffectTypeEnum == effectTypeEnum)
                {
                    self.Effects[i].EffectState = BuffState.Finished;
                }
            }
        }

        public static AEffectHandler GetEffect(this EffectViewComponent self, long instanceId)
        {
            for (int i = self.Effects.Count - 1; i >= 0; i--)
            {
                if (self.Effects[i].EffectData.InstanceId == instanceId)
                {
                    return self.Effects[i];
                }
            }
            return null;
        }

        public static void UpdatePositon(this EffectViewComponent self)
        {
            for (int i = self.Effects.Count - 1; i >= 0; i--)
            {
                AEffectHandler aEffectHandler = self.Effects[i];
                if (aEffectHandler.EffectData.InstanceId == 0)
                {
                    continue;
                }
                if (aEffectHandler.EffectConfig.SkillParent!=2)
                {
                    continue;
                }
                aEffectHandler.UpdateEffectPosition(self.GetParent<Unit>().Position, -1);
            }
        }

        public static void OnUpdate(this EffectViewComponent self)
        {
            for (int i = self.Effects.Count - 1; i >= 0; i--)
            {
                AEffectHandler aEffectHandler = self.Effects[i];
                if (aEffectHandler.EffectState == BuffState.Finished)
                {
                    aEffectHandler.OnFinished();
                    aEffectHandler.Dispose();
                    self.Effects.RemoveAt(i);
                    continue;
                }
                self.Effects[i].OnUpdate();
            }

            if (self.Effects.Count == 0)
            {
                TimerComponent.Instance?.Remove(ref self.Timer);
            }
        }

        public static void InitEffect(this EffectViewComponent self)
        {
            BuffManagerComponent buffManager = self.GetParent<Unit>().GetComponent<BuffManagerComponent>();
            if (buffManager == null)
            {
                return;
            }
            List<ABuffHandler> buffList =  buffManager.m_Buffs;
            for (int i = 0; i < buffList.Count; i++)
            {
                ABuffHandler aBuffHandler = buffList[i];
                if (self.GetEffect(aBuffHandler.EffectInstanceId)!=null)
                {
                    continue;
                }
                if (aBuffHandler.mSkillConf ==null || aBuffHandler.EffectData.InstanceId == 0)
                {
                    continue;
                }
                int skillParentID = aBuffHandler.mEffectConf.SkillParent;
                if (skillParentID == 0 || skillParentID == 2 || skillParentID == 3)
                {
                    self.EffectFactory(aBuffHandler.EffectData);
                }
            }
        }

        public static AEffectHandler EffectFactory(this EffectViewComponent self, EffectData effectData)
        {
            Unit unit = self.GetParent<Unit>();
            //if (!unit.MainHero && !SettingHelper.ShowEffect)
            //{
            //    return null;
            //}
            if (!SettingHelper.ShowEffect ||  UnitHelper.GetUnitList(unit.DomainScene(), UnitType.Player).Count > SettingHelper.NotGuangHuan)
            {
                EffectConfig effectConfig = EffectConfigCategory.Instance.Get(effectData.EffectId);
                if (effectConfig.EffectName.Contains(StringBuilderHelper.GuangHuan))
                {
                    return null;
                }
            }
            AEffectHandler resultEffect = self.AddChild<RoleSkillEffect>();
            resultEffect.OnInit(effectData, unit);
            self.AddEffect(resultEffect);       //给buff目标添加buff管理器
            return resultEffect;
        }

        /// <summary>
        /// 添加effect到真实链表，禁止外部调用
        /// </summary>
        /// <param name="aBuff"></param>
        public static void AddEffect(this EffectViewComponent self, AEffectHandler aBuff)
        {
            self.Effects.Add(aBuff);

            if (self.Timer == 0)
            {
                self.Timer = TimerComponent.Instance.NewFrameTimer(TimerType.Effectimer, self);
            }
        }
    }
}