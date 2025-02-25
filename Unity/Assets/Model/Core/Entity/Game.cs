﻿using System;
using System.Collections.Generic;

namespace ET
{
    public static class Game
    {
        public static ThreadSynchronizationContext ThreadSynchronizationContext => ThreadSynchronizationContext.Instance;

        public static TimeInfo TimeInfo => TimeInfo.Instance;
        
        public static EventSystem EventSystem => EventSystem.Instance;

        private static Scene scene;
        public static Scene Scene
        {
            get
            {
                if (scene != null)
                {
                    return scene;
                }
                scene = EntitySceneFactory.CreateScene(IdGenerater.Instance.GenerateInstanceId(), 0, SceneType.Process, "Process");
                return scene;
            }
        }

        public static Scene GetZoneScene(int zone)
        {
            Dictionary<long, Entity> childs = Scene.Children;
            foreach (var item in childs)
            {
                bool isscene = item.Value is Scene;
                if (!isscene)
                {
                    continue;
                }

                Scene zonescene = item.Value as Scene;
                if (zonescene == null)
                {
                    continue;
                }

                if (zonescene.Zone == 1)
                {
                    return zonescene;
                }
            }
            return null;
        }

        public static ObjectPool ObjectPool => ObjectPool.Instance;

        public static IdGenerater IdGenerater => IdGenerater.Instance;

        public static Options Options => Options.Instance;

        public static List<Action> FrameFinishCallback = new List<Action>();

        public static void Update()
        {
            long lastTime = TimeInfo.FrameTime;
            ThreadSynchronizationContext.Update();
            TimeInfo.Update();
            EventSystem.Update();

#if SERVER
            if (TimeInfo.FrameTime - lastTime > 500)
            {
                Log.Console($"TimeInfo.FrameTime - lastTime: {TimeInfo.FrameTime - lastTime}:  { TimeHelper.DateTimeNow().ToString()}");
            }
#endif
        }
        
        public static void LateUpdate()
        {
            EventSystem.LateUpdate();
        }

        public static void FrameFinish()
        {
            foreach (Action action in FrameFinishCallback)
            {
                action.Invoke();
            }
            FrameFinishCallback.Clear();
        }

        public static void Close()
        {
            scene?.Dispose();
            scene = null;
            MonoPool.Instance.Dispose();
            EventSystem.Instance.Dispose();
            IdGenerater.Instance.Dispose();
        }
    }
}