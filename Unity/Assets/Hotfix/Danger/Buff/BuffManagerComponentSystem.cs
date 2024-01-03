using System;
using System.Collections.Generic;

namespace ET
{

    [Timer(TimerType.BuffTimer)]
    public class BuffViewTimer : ATimer<BuffManagerComponent>
    {
        public override void Run(BuffManagerComponent self)
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

    [ObjectSystem]
    public class BuffManagerCompoenntAwakeSystem : AwakeSystem<BuffManagerComponent>
    {
        public override void Awake(BuffManagerComponent self)
        {
            self.t_Buffs.Clear();
            self.m_Buffs.Clear();
        }
    }

    [ObjectSystem]
    public class BuffManagerCompoenntDestroySystem : DestroySystem<BuffManagerComponent>
    {
        public override void Destroy(BuffManagerComponent self)
        {
            self.OnFinish();
        }
    }

    public static class BuffManagerComponetSystem
    {
        public static void OnFinish(this BuffManagerComponent self)
        {
            for (int i = self.m_Buffs.Count - 1; i >= 0; i--)
            {
                ABuffHandler skillHandler = self.m_Buffs[i];
                skillHandler.OnFinished();
                skillHandler.Clear();
                self.m_Buffs.RemoveAt(i);
                ObjectPool.Instance.Recycle(skillHandler);
            }
            TimerComponent.Instance?.Remove(ref self.Timer);
        }

        public static void OnUpdate(this BuffManagerComponent self)
        {
            int buffcnt = self.m_Buffs.Count;
            for (int i = buffcnt - 1; i >= 0; i--)
            {
                ABuffHandler skillHandler = self.m_Buffs[i];
                skillHandler.OnUpdate();
                if (skillHandler.BuffState == BuffState.Finished)
                {
                    skillHandler.OnFinished();
                    skillHandler.Clear();
                    self.m_Buffs.RemoveAt(i);
                    ObjectPool.Instance.Recycle(skillHandler);
                    continue;
                }

            }

            if (self.m_Buffs.Count == 0)
            {
                TimerComponent.Instance?.Remove(ref self.Timer);
            }
        }

        #region 添加，移除Buff
        public static void InitBuff(this BuffManagerComponent self)
        {
            List<KeyValuePair> buffs = self.t_Buffs;
            long timeNow = TimeHelper.ClientNow();
            for (int i = 0; i < buffs.Count; i++)
            {
                long buffEndTime = long.Parse(buffs[i].Value2);
                if (buffEndTime <= timeNow)
                {
                    continue;
                }
                // $"{buffHandler.BuffData.SkillId}_{buffHandler.BuffData.Spellcaster}"
                BuffData buffData = new BuffData();
                string[] buffinfo = buffs[i].Value.Split('_');
                buffData.TargetAngle = 0;
                buffData.BuffId = buffs[i].KeyId;
                buffData.SkillId = int.Parse(buffinfo[0]);
                buffData.Spellcaster = buffinfo[1];
                buffData.BuffEndTime = buffEndTime;
                self.BuffFactory(buffData);
            }
            self.t_Buffs.Clear();
        }

        public  static void BuffFactory(this BuffManagerComponent self, BuffData buffData)
        {
            SkillBuffConfig skillBuffConfig = SkillBuffConfigCategory.Instance.Get(buffData.BuffId);
            string BuffClassScript = skillBuffConfig.BuffScript;
            ABuffHandler resultBuff = (ABuffHandler)ObjectPool.Instance.Fetch(BuffDispatcherComponent.Instance.BuffTypes[BuffClassScript]);
            self.m_Buffs.Add(resultBuff);       //给buff目标添加buff管理器
            resultBuff.OnInit(buffData, self.GetParent<Unit>());

            if (self.Timer == 0)
            {
                //self.Timer = TimerComponent.Instance.NewFrameTimer(TimerType.BuffTimer, self);
                self.Timer = TimerComponent.Instance.NewRepeatedTimer(500, TimerType.BuffTimer, self);
            }
        }

        /// <summary>
        /// 移除Buff(下一帧才真正移除)
        /// </summary>
        /// <param name="buffId">要移除的BuffId</param>
        public static void RemoveBuff(this BuffManagerComponent self, long buffId)
        {
            int buffcnt = self.m_Buffs.Count;
            for (int i = buffcnt - 1; i >= 0; i--)
            {
                ABuffHandler aBuffHandler = self.m_Buffs[i];
                if (aBuffHandler.mSkillBuffConf == null)
                {
                    continue;
                }
                if (aBuffHandler.mSkillBuffConf.Id == buffId)
                {
                    aBuffHandler.OnFinished();
                    aBuffHandler.Clear();
                    self.m_Buffs.RemoveAt(i);
                    ObjectPool.Instance.Recycle(aBuffHandler);
                }
            }
        }

        #endregion

        #region 获取BuffSystem

        public static int GetBuffNumber(this BuffManagerComponent self, int buffId)
        {
            int number = 0;
            int buffcnt = self.m_Buffs.Count;
            for (int i = buffcnt - 1; i >= 0; i--)
            {
                if (self.m_Buffs[i].BuffData.BuffId == buffId)
                {
                    number ++;
                }
            }
            return number;
        }

        /// <summary>
        /// 通过标识ID获得Buff
        /// </summary>
        /// <param name="buffId">BuffData的标识ID</param>
        public static ABuffHandler GetBuffById(this BuffManagerComponent self, long buffId)
        {
            int buffcnt = self.m_Buffs.Count;
            for (int i = buffcnt - 1; i >= 0; i--)
            {
                if (self.m_Buffs[i].mSkillBuffConf == null)
                {
                    continue;
                }
                if (self.m_Buffs[i].mSkillBuffConf.Id == buffId)
                {
                    return self.m_Buffs[i];
                }
            }
            return null;
        }
        #endregion
    }
}