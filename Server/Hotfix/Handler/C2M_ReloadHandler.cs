﻿using System;
using System.Linq;
using System.Collections.Generic;

namespace ET
{
	[MessageHandler]
	public class C2M_ReloadHandler: AMRpcHandler<C2M_Reload, M2C_Reload>
	{
		protected override async ETTask Run(Session session, C2M_Reload request, M2C_Reload response, Action reply)
		{
			if (request.Account != "tcg" && request.Password != "tcg")
			{
				Log.Error($"error reload account and password: {MongoHelper.ToJson(request)}");
				return;
			}

			List<StartProcessConfig> listprogress = StartProcessConfigCategory.Instance.GetAll().Values.ToList();
			Log.Info("C2M_Reload_a: listprogress " + listprogress.Count);
			for (int i = 0; i < listprogress.Count; i++)
			{
				List<StartSceneConfig> processScenes = StartSceneConfigCategory.Instance.GetByProcess(listprogress[i].Id);
				if (processScenes.Count == 0 || listprogress[i].Id == 203)  //机器人进程
				{
					continue;
				}

				StartSceneConfig startSceneConfig = processScenes[0];
				Log.Info("C2M_Reload_a: processScenes " + startSceneConfig);
				long mapInstanceId = StartSceneConfigCategory.Instance.GetBySceneName(startSceneConfig.Zone, startSceneConfig.Name).InstanceId;
				A2M_Reload createUnit = (A2M_Reload)await ActorMessageSenderComponent.Instance.Call(
					mapInstanceId, new M2A_Reload() { LoadType =request.LoadType, LoadValue = request.LoadValue });
			}

			reply();
			await ETTask.CompletedTask;
		}
	}
}