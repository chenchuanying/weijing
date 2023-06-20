﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ET
{

    [Timer(TimerType.FallingFont)]
    public class FallingFontTimer : ATimer<FallingFontComponent>
    {
        public override void Run(FallingFontComponent self)
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

    /// <summary>
    /// 飘字组件，用于处理诸如伤害飘字。治疗飘字这种效果
    /// </summary>
    public class FallingFontComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
        public List<FallingFontShowComponent> FallingFontShows = new List<FallingFontShowComponent>();
    }

    public class FallingFontComponentAwakeSystem : AwakeSystem<FallingFontComponent>
    {
        public override void Awake(FallingFontComponent self)
        {
            self.OnAwake();
        }
    }

    public class FallingFontComponentDestroySystem : DestroySystem<FallingFontComponent>
    {
        public override void Destroy(FallingFontComponent self)
        {
            for (int i = self.FallingFontShows.Count - 1; i >= 0; i--)
            {
                FallingFontShowComponent fallingFontShowComponent = self.FallingFontShows[i];
                self.FallingFontShows.RemoveAt(i);
                fallingFontShowComponent.Dispose();
            }
            TimerComponent.Instance?.Remove(ref self.Timer);
        }
    }

    public static class FallingFontComponentSystem
    {
        public static void OnAwake(this FallingFontComponent self)
        {
            self.FallingFontShows.Clear();
        }

        /// <summary>
        /// 播放飘字特效
        /// </summary>
        /// <param name="targetValue">目标值</param>
        public static void  Play(this FallingFontComponent self, GameObject HeadBar, long targetValue, Unit unit, int type)
        {
            //判断目标是否已经死亡

            FallingFontShowComponent fallingFont = self.AddChild<FallingFontShowComponent>();
            fallingFont.OnInitData(HeadBar, targetValue, unit, type);
            self.FallingFontShows.Add(fallingFont);

            if (self.Timer == 0)
            {
                self.Timer = TimerComponent.Instance.NewFrameTimer(TimerType.FallingFont, self);
            }
        }

        public static void OnUpdate(this FallingFontComponent self)
        {
            for (int i = self.FallingFontShows.Count - 1; i >= 0; i--)
            {
                FallingFontShowComponent fallingFontShowComponent = self.FallingFontShows[i];
                bool remove = fallingFontShowComponent.LateUpdate();
                if (remove)
                {
                    self.FallingFontShows.RemoveAt(i);
                    fallingFontShowComponent.Dispose();
                }
            }

            if (self.FallingFontShows.Count == 0 && self.Timer!=0)
            {
                TimerComponent.Instance?.Remove(ref self.Timer);
            }
        }
    }

}