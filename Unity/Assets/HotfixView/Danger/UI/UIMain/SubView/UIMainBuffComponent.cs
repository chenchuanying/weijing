﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [Timer(TimerType.MainBuffTimer)]
    public class MainBuffTimer : ATimer<UIMainBuffComponent>
    {
        public override void Run(UIMainBuffComponent self)
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

    public class UIMainBuffComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public long Timer;
        public GameObject UIMainBuffItem;
        public List<UIMainBuffItemComponent> MainBuffUIList = new List<UIMainBuffItemComponent>();
        public List<UIMainBuffItemComponent> CacheUIList = new List<UIMainBuffItemComponent>();
        public GameObject GameObject;
    }


    public class UIMainBuffComponentAwakeSystem : AwakeSystem<UIMainBuffComponent, GameObject>
    {
        public override void Awake(UIMainBuffComponent self, GameObject gameObject)
        {
            self.MainBuffUIList.Clear();
            self.CacheUIList.Clear();
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            self.UIMainBuffItem = rc.Get<GameObject>("UIMainBuffItem");
            self.UIMainBuffItem.SetActive(false);
        }
    }


    public class UIMainBuffComponentDestroySystem : DestroySystem<UIMainBuffComponent>
    {
        public override void Destroy(UIMainBuffComponent self)
        {
            TimerComponent.Instance?.Remove(ref self.Timer);
        }
    }

    public static class UIMainBuffComponentSystem
    {

        public static void OnUpdate(this UIMainBuffComponent self)
        {
            int buffcnt = self.MainBuffUIList.Count;
            for (int i = buffcnt - 1; i >= 0; i--)
            {
                UIMainBuffItemComponent uIMainBuffItemComponent = self.MainBuffUIList[i];
                bool update = uIMainBuffItemComponent.OnUpdate();
                if (!update)
                {
                    uIMainBuffItemComponent.BeforeRemove();
                    uIMainBuffItemComponent.BuffID = 0;
                    self.MainBuffUIList[i].GameObject.SetActive(false);
                    self.CacheUIList.Add(self.MainBuffUIList[i]);
                    self.MainBuffUIList.RemoveAt(i);
                }
            }
            if (self.MainBuffUIList.Count == 0)
            {
                TimerComponent.Instance.Remove(ref self.Timer);
            }
        }

        public static void OnBuffUpdate(this UIMainBuffComponent self, ABuffHandler aBuffHandler, int operatetype)
        {
            //1添加  2移除 3重置

            switch (operatetype)
            {
                case 1:
                    self.OnAddBuff(aBuffHandler);
                    break;
                case 2:
                    self.OnRemoveBuff(aBuffHandler);
                    break;
                case 3:
                    self.OnResetBuff(aBuffHandler);
                    break;
            }
        }

        public static void ResetUI(this UIMainBuffComponent self)
        {
            for (int i = 0; i < self.MainBuffUIList.Count; i++)
            {
                self.MainBuffUIList[i].BuffID = 0;
                self.MainBuffUIList[i].GameObject.SetActive(false);
                self.CacheUIList.Add(self.MainBuffUIList[i]);
            }
            self.MainBuffUIList.Clear();
            if (self.MainBuffUIList.Count == 0)
            {
                TimerComponent.Instance.Remove(ref self.Timer);
            }
        }

        public static void OnAddBuff(this UIMainBuffComponent self, ABuffHandler buffHandler)
        {
            if (self.MainBuffUIList.Count >= 8 || buffHandler.mSkillBuffConf.IfShowIconTips == 0)
            {
                return;
            }

            if (!self.AddSameBuff(buffHandler))
            {
                UIMainBuffItemComponent ui_buff = self.CacheUIList.Count > 0 ? self.CacheUIList[0] : null;
                if (ui_buff == null)
                {
                    ui_buff = self.AddChild<UIMainBuffItemComponent, GameObject>(GameObject.Instantiate(self.UIMainBuffItem));
                }
                else
                {
                    self.CacheUIList.RemoveAt(0);
                }
                self.MainBuffUIList.Add(ui_buff);
                ui_buff.GameObject.SetActive(true);
                ui_buff.OnAddBuff(buffHandler);
                UICommonHelper.SetParent(ui_buff.GameObject, self.GameObject);
            }

            if (self.Timer == 0)
            {
                self.Timer = TimerComponent.Instance.NewRepeatedTimer(500, TimerType.MainBuffTimer, self);
            }
        }

        public static bool AddSameBuff(this UIMainBuffComponent self, ABuffHandler buffHandler)
        {
            for (int i = 0; i < self.MainBuffUIList.Count; i++)
            {
                UIMainBuffItemComponent uIMainBuffItemComponent = self.MainBuffUIList[i];
                if (uIMainBuffItemComponent.BuffID == buffHandler.BuffData.BuffId && uIMainBuffItemComponent.UnitId == buffHandler.TheUnitBelongto.Id)
                {
                    uIMainBuffItemComponent.UpdateBuffNumber(buffHandler, 0);
                    uIMainBuffItemComponent.EndTime = buffHandler.BuffEndTime;
                    return true;
                }
            }
            return false;
        }

        public static bool HaveSameBuff(this UIMainBuffComponent self, int buffID)
        {
            for (int i = 0; i < self.MainBuffUIList.Count; i++)
            {
                UIMainBuffItemComponent uIMainBuffItemComponent = self.MainBuffUIList[i];
                if (uIMainBuffItemComponent.BuffID == buffID)
                {
                    return true;
                }
            }
            return false;
        }

        public static void OnResetBuff(this UIMainBuffComponent self, ABuffHandler buffHandler)
        {
            for (int i = 0; i < self.MainBuffUIList.Count; i++)
            {
                UIMainBuffItemComponent uIMainBuffItemComponent = self.MainBuffUIList[i];
                if (uIMainBuffItemComponent.BuffID == buffHandler.BuffData.BuffId)
                {
                    uIMainBuffItemComponent.OnResetBuff(buffHandler);
                }
            }
        }

        public static void OnRemoveBuff(this UIMainBuffComponent self, ABuffHandler buffHandler)
        {
            for (int i = self.MainBuffUIList.Count - 1; i >= 0; i--)
            {
                UIMainBuffItemComponent uIMainBuffItemComponent = self.MainBuffUIList[i];
                if (uIMainBuffItemComponent.BuffID == buffHandler.BuffData.BuffId)
                {
                    uIMainBuffItemComponent.UpdateBuffNumber(buffHandler, -1);
                }
            }
            self.OnUpdate() ;
        }
    }

}
