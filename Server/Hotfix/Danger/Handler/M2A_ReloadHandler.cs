﻿using System;

namespace ET
{
    [ActorMessageHandler]
    public class M2A_ReloadHandler : AMActorRpcHandler<Scene, M2A_Reload, A2M_Reload>
    {
        protected override async ETTask Run(Scene session, M2A_Reload request, A2M_Reload response, Action reply)
        {
            Log.Console("C2M_Reload_b: " + session.Name);

            OpcodeHelper.ShowMessage = false;
            OpcodeHelper.OneTotalNumber = 20000;
            OpcodeHelper.OneTotalLength = 20000000;

            Log.Console("C2M_Reload_b: " + ConfigLoader.RemovePlayer);

            switch (request.LoadType)
            {
                case 0://全部
                    //Game.EventSystem.Add(typeof(Game).Assembly);
                    Game.EventSystem.Add(DllHelper.GetHotfixAssembly());
                    Game.EventSystem.Load();

                    ConfigComponent.Instance.LoadAsync().Coroutine();
                    break;
                case 1: //代码
                    Game.EventSystem.Add(DllHelper.GetHotfixAssembly());
                    Game.EventSystem.Load();
                    break;
                case 2: //配置表
                    if (string.IsNullOrEmpty(request.LoadValue))
                    {
                        ConfigComponent.Instance.LoadAsync().Coroutine();
                    }
                    else
                    {
                        string configName = request.LoadValue;
                        string category = $"{configName}Category";
                        Type type = Game.EventSystem.GetType($"ET.{category}");
                        if (type == null)
                        {
                            Log.Console($"reload config but not find {category}");
                            return;
                        }
                        ConfigComponent.Instance.LoadOneConfig(type);
                        Log.Console($"reload config {configName} finish!");
                    }
                    break;
                case 3:
#if SERVER
                    if (string.IsNullOrEmpty(request.LoadValue))
                    {
                        Game.Scene.GetComponent<RecastPathComponent>().OnLoad();
                    }
                    else
                    {
                        Game.Scene.GetComponent<RecastPathComponent>().OnLoadSingle(int.Parse(request.LoadValue));
                    }
#endif
                    break;
            }

            Log.Console("EventSystem.Instance.ToString: 1");
            Log.Console("EventSystem:   "+ EventSystem.Instance.ToString());
            Log.Console("TimerComponent:"+ TimerComponent.Instance.ToString());
            Log.Console("ObjectPool:    "+ ObjectPool.Instance.ToString());
            Log.Console("MonoPool:      "+ MonoPool.Instance.ToString());
            Log.Console("EventSystem.Instance.ToString: 2");

            reply();
            await ETTask.CompletedTask;
        }
    }
}
