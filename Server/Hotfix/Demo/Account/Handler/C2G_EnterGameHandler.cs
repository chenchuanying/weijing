﻿using System;

namespace ET
{
	public class C2G_EnterGameHandler : AMRpcHandler<C2G_EnterGame, G2C_EnterGame>
	{
		protected override async ETTask Run(Session session, C2G_EnterGame request, G2C_EnterGame response, Action reply)
		{
			if (session.DomainScene().SceneType != SceneType.Gate)
			{
				Log.Error($"LoginTest C2G_EnterGame请求的Scene错误，当前Scene为：{session.DomainScene().SceneType}");
				session.Dispose();
				return;
			}
            if (ComHelp.Version != request.Version)
            {
				response.Error = ErrorCore.ERR_VersionNoMatch;
                reply();
                return;
            }
            if (session.GetComponent<SessionLockingComponent>() != null)
			{
				response.Error = ErrorCore.ERR_RequestRepeatedly;
				reply();
				return;
			}
			//没有loginGate
			SessionPlayerComponent sessionPlayerComponent = session.GetComponent<SessionPlayerComponent>();
			if (null == sessionPlayerComponent)
			{
				response.Error = ErrorCore.ERR_SessionPlayerError;
				reply();
				return;
			}
			//连接gate的时候会做记录
			Player player = Game.EventSystem.Get(sessionPlayerComponent.PlayerInstanceId) as Player;
			if (player == null || player.IsDisposed)
			{
				response.Error = ErrorCore.ERR_NonePlayerError;
				reply();
				return;
			}

			long instanceId = session.InstanceId;
			using (session.AddComponent<SessionLockingComponent>())
			{
				using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginGate, player.AccountId.GetHashCode()))
				{
					if (instanceId != session.InstanceId || player.IsDisposed)
					{
						LogHelper.LogDebug($"LoginTest C2G_EnterGameHandler: instanceId： {instanceId}  session.InstanceId： {session.InstanceId} {player.IsDisposed} ");
						response.Error = ErrorCore.ERR_PlayerSessionError;
						reply();
						return;
					}

					//同一个session不能重复进
					if (session.GetComponent<SessionStateComponent>() != null
						&& session.GetComponent<SessionStateComponent>().State == SessionState.Game)
					{
						LogHelper.LogDebug("LoginTest C2G_EnterGameHandler: SessionStateComponent.State == SessionState.Game");
						response.Error = ErrorCore.ERR_SessionStateError;
						reply();
						return;
					}

					//LogHelper.LogDebug($"LoginTest  C2G_EnterGame_1 player.Id： {player.Id} request.UserID: {request.UserID} player.PlayerState：{player.PlayerState} request.Relink：{request.Relink}");
                    //player可以映射任意一个seesion。 session是唯一的
                    if (player.PlayerState == PlayerState.Game && !request.Relink)
                    {
                        //快速重启客户端而非重连
                        //通知游戏逻辑服下线Unit角色逻辑，并将数据存入数据库
                        IActorResponse reqEnter = (M2G_RequestEnterGameState)await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestEnterGameState()
                        {
                            GateSessionActorId = 0
                        });
                        player.RemoveComponent<GateMapComponent>();
                        player.PlayerState = PlayerState.None;
                    }

                    if (player.PlayerState == PlayerState.Game)
					{
						try
						{
							//重连 [二次登录不成功可能导致Unit没移除]
							LogHelper.LogDebug($"LoginTest C2G_EnterGame 二次登录开始; player.Id： {player.Id} request.UserID{request.UserID}  player.UnitId: {player.UnitId}");
							//主要判断unit还在不在
							IActorResponse reqEnter =(M2G_RequestEnterGameState) await MessageHelper.CallLocationActor(player.UnitId, new G2M_RequestEnterGameState()
							{
								GateSessionActorId = session.InstanceId
							});
                            if (reqEnter.Error == ErrorCode.ERR_Success)
                            {
								LogHelper.LogDebug($"LoginTest C2G_EnterGame 二次登录成功; player.Id： {player.Id} request.UserID:{request.UserID}");
                                reply();
                                return;
                            }

                            Log.Error($"LoginTest C2G_EnterGame 二次登录失败1 player.Id： {player.Id} request.UserID{request.UserID}  player.UnitId: {player.UnitId}");
							response.Error = ErrorCore.ERR_ReEnterGameError;
							await DisconnectHelper.KickPlayer(player, true);
							await DisconnectHelper.KickPlayer(session.DomainZone(), request.UserID);

							reply();
							session?.Disconnect().Coroutine();
							return;
						}
						catch (Exception e)
						{
							Log.Error($"LoginTest C2G_EnterGame 二次登录失败2 player.Id： {player.Id}  request.UserID{request.UserID}  player.UnitId: {player.UnitId}" + e.ToString());
							response.Error = ErrorCore.ERR_ReEnterGameError2;
							await DisconnectHelper.KickPlayer(player, true);
							await DisconnectHelper.KickPlayer(session.DomainZone(), request.UserID);
							reply();
							session?.Disconnect().Coroutine();
							throw;
						}
					}
					try
					{
						if (player.GetComponent<GateMapComponent>()!=null)
						{
							player.RemoveComponent<GateMapComponent>();
						}
						GateMapComponent gateMapComponent = player.AddComponent<GateMapComponent>();
						gateMapComponent.Scene = SceneFactory.Create(gateMapComponent, "GateMap", SceneType.GateMap);

						Unit unit = UnitFactory.Create(gateMapComponent.Scene, request.UserID, UnitType.Player);
						await DBHelper.AddDataComponent<UserInfoComponent>(unit, request.UserID, DBHelper.UserInfoComponent);
						await DBHelper.AddDataComponent<NumericComponent>(unit, request.UserID, DBHelper.NumericComponent);
						await DBHelper.AddDataComponent<TaskComponent>(unit, request.UserID, DBHelper.TaskComponent);
						await DBHelper.AddDataComponent<ShoujiComponent>(unit, request.UserID, DBHelper.ShoujiComponent);
						await DBHelper.AddDataComponent<ChengJiuComponent>(unit, request.UserID, DBHelper.ChengJiuComponent);
						await DBHelper.AddDataComponent<BagComponent>(unit, request.UserID, DBHelper.BagComponent);
						await DBHelper.AddDataComponent<PetComponent>(unit, request.UserID, DBHelper.PetComponent);
						await DBHelper.AddDataComponent<SkillSetComponent>(unit, request.UserID, DBHelper.SkillSetComponent);
						await DBHelper.AddDataComponent<EnergyComponent>(unit, request.UserID, DBHelper.EnergyComponent);
						await DBHelper.AddDataComponent<ActivityComponent>(unit, request.UserID, DBHelper.ActivityComponent);
						await DBHelper.AddDataComponent<RechargeComponent>(unit, request.UserID, DBHelper.RechargeComponent);
						await DBHelper.AddDataComponent<ReddotComponent>(unit, request.UserID, DBHelper.ReddotComponent);
						await DBHelper.AddDataComponent<TitleComponent>(unit, request.UserID, DBHelper.TitleComponent);
						await DBHelper.AddDataComponent<JiaYuanComponent>(unit, request.UserID, DBHelper.JiaYuanComponent);
						
						unit.AddComponent<UnitGateComponent, long>(player.InstanceId);
						unit.AddComponent<MailComponent>();
						unit.AddComponent<StateComponent>();
						unit.AddComponent<HeroDataComponent>();
						unit.AddComponent<DBSaveComponent>();
						unit.AddComponent<SkillPassiveComponent>().UpdatePassiveSkill();
						unit.GetComponent<UserInfoComponent>().OnLogin(session.RemoteAddress.ToString(), request.DeviceName);
						unit.GetComponent<UnitInfoComponent>().UnitName = unit.GetComponent<UserInfoComponent>().UserName;

						//Function_Fight.GetInstance().UnitUpdateProperty_Base(unit, false, false);
						unit.GetComponent<DBSaveComponent>().UpdateCacheDB();
						if (session.DomainZone() == 0)
						{
							LogHelper.LogDebug($"LoginTest C2G_EnterGame session.DomainZone() == 0 player.Id： {player.Id} request.UserID{request.UserID}  player.UnitId: {player.UnitId}");
							response.Error = ErrorCore.ERR_SessionStateError;
							reply();
							return;
						}
						Log.Debug($"LoginTest C2G_EnterGame TransferHelper.Transfer; unitid: {request.UserID} player.Id {player.Id} player.InstanceId: {player.InstanceId} {session.DomainZone()}");
						long unitId = unit.Id;

						await EnterRankServer(unit);
                        await EnterMailServer(unit);
                        player.ChatInfoInstanceId = await EnterWorldChatServer(unit);	//登录聊天服

						player.DBCacheId = DBHelper.GetDbCacheId(session.DomainZone());
						player.MailServerID = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), Enum.GetName(SceneType.EMail)).InstanceId;
						player.RankServerID = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), Enum.GetName(SceneType.Rank)).InstanceId;
						player.PaiMaiServerID = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), Enum.GetName(SceneType.PaiMai)).InstanceId;
						player.ActivityServerID = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), Enum.GetName(SceneType.Activity)).InstanceId;
						player.TeamServerID = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), Enum.GetName(SceneType.Team)).InstanceId;
						player.FriendServerID = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), Enum.GetName(SceneType.Friend)).InstanceId;
						player.UnionServerID = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), Enum.GetName(SceneType.Union)).InstanceId;
						player.SoloServerID = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), Enum.GetName(SceneType.Solo)).InstanceId;
						player.PopularizeServerID = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), Enum.GetName(SceneType.Popularize)).InstanceId;
						player.ReChargeServerID = StartSceneConfigCategory.Instance.RechargeConfig.InstanceId;
						player.CenterServerID = StartSceneConfigCategory.Instance.CenterConfig.InstanceId;
						response.MyId = unitId;

						reply();
						StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), $"Map{ComHelp.MainCityID()}");
						await TransferHelper.Transfer(unit, startSceneConfig.InstanceId, (int)SceneTypeEnum.MainCityScene, ComHelp.MainCityID(), 0, "0");

						player.PlayerState = PlayerState.Game;
						player.UnitId = request.UserID;
						SessionStateComponent SessionStateComponent = session.GetComponent<SessionStateComponent>();
						if (SessionStateComponent != null)
						{
							SessionStateComponent.State = SessionState.Game;
						}
						else
						{
							Log.Error($"切场景掉线了 {request.UserID}");
						}
					}
					catch (Exception e)
					{
						Log.Error($"LoginTest 角色进入游戏逻辑服出现问题 区 账号Id: {session.DomainZone()}  player.UnitId: {player.UnitId}   异常信息： {e.ToString()}");
						response.Error = ErrorCore.ERR_EnterGameError;
						reply();
						await DisconnectHelper.KickPlayer(player, true);
						session.Disconnect().Coroutine();
					}
				}
			}
		}

		private async ETTask<long> EnterWorldChatServer(Unit unit)
		{
			long chatServerId = StartSceneConfigCategory.Instance.GetBySceneName(unit.DomainZone(), Enum.GetName(SceneType.Chat)).InstanceId;
			Chat2G_EnterChat chat2G_EnterChat = (Chat2G_EnterChat)await MessageHelper.CallActor(chatServerId, new G2Chat_EnterChat()
			{ 
				UnitId = unit.Id,
				Name = unit.GetComponent<UserInfoComponent>().UserInfo.Name,
				GateSessionActorId = unit.GetComponent<UnitGateComponent>().GateSessionActorId
			});
			return chat2G_EnterChat.ChatInfoUnitInstanceId;
		}

		private async ETTask EnterMailServer(Unit unit)
		{
            long mailServerId = StartSceneConfigCategory.Instance.GetBySceneName(unit.DomainZone(), Enum.GetName(SceneType.EMail)).InstanceId;
            Mail2G_EnterMail chat2G_EnterChat = (Mail2G_EnterMail)await MessageHelper.CallActor(mailServerId, new G2Mail_EnterMail()
            {
                UnitId = unit.Id,
            });
			if (chat2G_EnterChat.Error == ErrorCore.ERR_Success)
			{
                unit.GetComponent<UserInfoComponent>().UserInfo.ServerMailIdCur  = chat2G_EnterChat.ServerMailIdMax;
            }
		}

		private async ETTask EnterRankServer(Unit unit)
		{
			long rankServerId = StartSceneConfigCategory.Instance.GetBySceneName(unit.DomainZone(), Enum.GetName(SceneType.Rank)).InstanceId;
			Rank2G_EnterRank chat2G_EnterChat = (Rank2G_EnterRank)await MessageHelper.CallActor(rankServerId, new G2Rank_EnterRank()
			{
				UnitId = unit.Id,
			});

			NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
			long unionid = unit.GetUnionId();
			numericComponent.ApplyValue(NumericType.RankID, chat2G_EnterChat.RankId, false,false);
			numericComponent.ApplyValue(NumericType.PetRankID, chat2G_EnterChat.PetRankId, false, false);
            numericComponent.ApplyValue(NumericType.SoloRankId, chat2G_EnterChat.SoloRankId, false, false);

            long unionsceneid = StartSceneConfigCategory.Instance.GetBySceneName(unit.DomainZone(), Enum.GetName(SceneType.Union)).InstanceId;
			Union2G_EnterUnion union2G_EnterChat = (Union2G_EnterUnion)await MessageHelper.CallActor(unionsceneid, new G2Union_EnterUnion()
			{
				UnitId = unit.Id,
			});

			numericComponent.ApplyValue(NumericType.RaceDonationRankID, union2G_EnterChat.DonationRankId, false, false);
			if (unionid != 0 && union2G_EnterChat.WinUnionId == unionid)
			{
				numericComponent.ApplyValue(NumericType.UnionRaceWin, 1, false, false);
			}
			else
			{
				numericComponent.ApplyValue(NumericType.UnionRaceWin, 0, false, false);
			}
		}
	}
}