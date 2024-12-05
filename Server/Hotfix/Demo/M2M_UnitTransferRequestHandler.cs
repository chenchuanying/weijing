using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{

    [ActorMessageHandler]
	public class M2M_UnitTransferRequestHandler : AMActorRpcHandler<Scene, M2M_UnitTransferRequest, M2M_UnitTransferResponse>
	{
		protected override async ETTask Run(Scene scene, M2M_UnitTransferRequest request, M2M_UnitTransferResponse response, Action reply)
		{
			try
			{
				UnitComponent unitComponent = scene.GetComponent<UnitComponent>();
				if (unitComponent.Get(request.Unit.Id) != null)
				{
					Log.Error($"LoginTest M2M_UnitTransfer   unitComponent.Get(unit.Id)!=null: {scene.DomainZone()} {request.Unit.Id} {request.SceneType}");

					if (request.SceneType == SceneTypeEnum.JiaYuan)
					{
						Log.Error($"JiaYuan: {scene.Id} {scene.InstanceId}");
                    }
					response.Error = ErrorCode.ERR_OperationOften;
					reply();
					return;
				}
				else
				{
					Log.Debug($"LoginTest M2M_UnitTransfer:  {scene.DomainZone()}  {request.Unit.Id}  {request.SceneType}  {request.Difficulty}  {request.ParamInfo}");
				}
                
                Unit unit = request.Unit;
                unitComponent.AddChild(unit);
				unitComponent.Add(unit);
                
                Dictionary<long, List<byte[]>> components = unitComponent.UnitComponents;
				request.EntityBytes.AddRange(components[request.Unit.Id]);
				components[request.Unit.Id].Clear();
                foreach (byte[] bytes in request.EntityBytes)
				{
					Entity entity = MongoHelper.Deserialize<Entity>(bytes);
                    if (bytes.Length > 300000)
                    {
						Log.Warning($"bytes.Length > too large: {unit.Id} {entity.GetType().Name} {bytes.Length}");
                    }
                    unit.AddComponent(entity);
				}
				unit.AddComponent<MoveComponent>();
				unit.AddComponent<MailBoxComponent>();
				unit.AddComponent<ObjectWait>();
				unit.AddComponent<SkillManagerComponent>();
				unit.AddComponent<BuffManagerComponent>();
				unit.AddComponent<AttackRecordComponent>();
				NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
				numericComponent.Set(NumericType.BattleCamp, CampEnum.CampPlayer_1, false);
                numericComponent.Set(NumericType.RunRaceTransform, 0, false);
                numericComponent.Set(NumericType.CardTransform, 0, false);

                unit.Type = UnitType.Player;
                unit.SceneType = request.SceneType;
				unit.ConfigId = unit.GetComponent<UserInfoComponent>().UserInfo.Occ;
				unit.GetComponent<UserInfoComponent>().UserInfo.DemonName = string.Empty;
                unit.GetComponent<HeroDataComponent>().CheckNumeric();
                Function_Fight.GetInstance().UnitUpdateProperty_Base(unit, false, false);
                //添加消息类型, GateSession邮箱在收到消息的时候会立即转发给客户端，MessageDispatcher类型会再次对Actor消息进行分发到具体的Handler处理，默认的MailboxComponent类型是MessageDispatcher。
                //await unit.AddLocation();                     
                //注册消息机制的ID,可以通过消息ID让其他玩家对自己进行消息发送
                //客户端收到创建Unit之后会请求数据。 不用通知
                switch (request.SceneType)
				{
					case (int)SceneTypeEnum.CellDungeon:
						int sonid = scene.GetComponent<CellDungeonComponent>().CurrentFubenCell.sonid;
						ChapterSonConfig chapterSon = ChapterSonConfigCategory.Instance.Get(sonid);
						unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
						Game.Scene.GetComponent<RecastPathComponent>().Update(scene.GetComponent<MapComponent>().NavMeshId);
						scene.GetComponent<CellDungeonComponent>().MainUnit = unit;
						//更新unit坐标
						unit.Position = new Vector3(chapterSon.BornPosLeft[0] * 0.01f, chapterSon.BornPosLeft[1] * 0.01f, chapterSon.BornPosLeft[2] * 0.01f);
						unit.Rotation = Quaternion.identity;

						// 通知客户端创建My Unit
						M2C_CreateMyUnit m2CCreateUnits = new M2C_CreateMyUnit();
						m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
						MessageHelper.SendToClient(unit, m2CCreateUnits);
						// 加入aoi
						unit.AddComponent<AOIEntity, int, Vector3>(4 * 1000, unit.Position);
						scene.GetComponent<CellDungeonComponent>().GenerateFubenScene(false);
						TransferHelper.AfterTransfer(unit);
						break;
					case (int)SceneTypeEnum.PetMing:
					case (int)SceneTypeEnum.PetDungeon:
					case (int)SceneTypeEnum.PetTianTi:
						SceneConfig sceneConfig = SceneConfigCategory.Instance.Get(request.ChapterId);
						scene.GetComponent<MapComponent>().NavMeshId = sceneConfig.MapID;
						unit.AddComponent<PathfindingComponent, int>(sceneConfig.MapID);
						Game.Scene.GetComponent<RecastPathComponent>().Update(sceneConfig.MapID);
						//更新unit坐标
						unit.Position = new Vector3(sceneConfig.InitPos[0] * 0.01f, sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f);
						unit.Rotation = Quaternion.identity;

						m2CCreateUnits = new M2C_CreateMyUnit();
						m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
						MessageHelper.SendToClient(unit, m2CCreateUnits);
						// 加入aoi
						unit.AddComponent<AOIEntity, int, Vector3>(40 * 1000, unit.Position);
						if (request.SceneType == (int)SceneTypeEnum.PetDungeon)
						{
							scene.GetComponent<PetFubenSceneComponent>().MainUnit = unit;
							scene.GetComponent<PetFubenSceneComponent>().GeneratePetFuben(unit, int.Parse(request.ParamInfo));
						}
						if (request.SceneType == (int)SceneTypeEnum.PetTianTi)
						{
							scene.GetComponent<PetTianTiComponent>().MainUnit = unit;
							scene.GetComponent<PetTianTiComponent>().GeneratePetFuben().Coroutine();
							unit.GetComponent<TaskComponent>().TriggerTaskEvent(TaskTargetType.PetTianTiNumber_14,0, 1 );
                            unit.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.PetTianTiNumber_14, 0, 1);
                            unit.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.PetTianTiNumber_310, 0, 1);
						}
						if (request.SceneType == (int)SceneTypeEnum.PetMing)
						{
							scene.GetComponent<PetMingDungeonComponent>().MainUnit = unit;
							scene.GetComponent<PetMingDungeonComponent>().GeneratePetFuben().Coroutine();
                        }
						break;
					case (int)SceneTypeEnum.LocalDungeon:
						numericComponent.ApplyValue(NumericType.TaskDungeonId, request.ChapterId, false);
						DungeonConfig dungeonConfig = DungeonConfigCategory.Instance.Get(request.ChapterId);
						scene.GetComponent<MapComponent>().NavMeshId = dungeonConfig.MapID;
						unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
						Game.Scene.GetComponent<RecastPathComponent>().Update(scene.GetComponent<MapComponent>().NavMeshId);
                        scene.GetComponent<LocalDungeonComponent>().MainUnit = unit;

                        //更新unit坐标
                        int transferId = int.Parse(request.ParamInfo);
						if (transferId != 0)
						{
							DungeonTransferConfig transferConfig = DungeonTransferConfigCategory.Instance.Get(transferId);
							unit.Position = new Vector3(transferConfig.BornPos[0] * 0.01f, transferConfig.BornPos[1] * 0.01f, transferConfig.BornPos[2] * 0.01f);
						}
						else
						{
							unit.Position = new Vector3(dungeonConfig.BornPosLeft[0] * 0.01f, dungeonConfig.BornPosLeft[1] * 0.01f, dungeonConfig.BornPosLeft[2] * 0.01f);
						}

						//神秘之门返回
						if (unit.GetComponent<UnitInfoComponent>().LastDungeonId == request.ChapterId)
						{
							unit.GetComponent<UnitInfoComponent>().LastDungeonId = 0;
						 	unit.Position = unit.GetComponent<UnitInfoComponent>().LastDungeonPosition;
                        }

                        //进入神秘之门（喜从天降玩法）
                        if (dungeonConfig.MapType == SceneSubTypeEnum.LocalDungeon_1)
                        {
                            numericComponent.ApplyValue(NumericType.HappyMoveNumber, 0, false);
                            numericComponent.ApplyValue(NumericType.HappyMoveTime, 0, false);
                            int randomPosition = RandomHelper.RandomNumber(0, HappyHelper.PositionList.Count);
                            numericComponent.Set(NumericType.HappyCellIndex, randomPosition + 1, false);
                            unit.Position = HappyHelper.PositionList[randomPosition];

							scene.AddComponent<DungeonHappyComponent>();
                        }

                        unit.Rotation = Quaternion.identity;
						// 通知客户端创建My Unit
						m2CCreateUnits = new M2C_CreateMyUnit();
						m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
						MessageHelper.SendToClient(unit, m2CCreateUnits);
						// 加入aoi
						unit.AddComponent<AOIEntity, int, Vector3>(10 * 1000, unit.Position);
						TransferHelper.AfterTransfer(unit);
						scene.GetComponent<LocalDungeonComponent>().GenerateFubenScene(request.ChapterId);
						unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.LocalDungeonTime, 1, 0);
						break;
                    case SceneTypeEnum.Happy:
                        unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
                        sceneConfig = SceneConfigCategory.Instance.Get(request.ChapterId);

						int happcellIndex = numericComponent.GetAsInt(NumericType.HappyCellIndex);
						if (happcellIndex > 0)
						{
                            unit.Position = HappyHelper.PositionList[happcellIndex - 1];
                        }
						else
						{
                            int randomPosition = RandomHelper.RandomNumber(0, HappyHelper.PositionList.Count);
                            numericComponent.Set(NumericType.HappyCellIndex, randomPosition + 1, false);
                            unit.Position = HappyHelper.PositionList[randomPosition];
                        }
                        unit.Rotation = Quaternion.identity;
                        // 通知客户端创建My Unit
                        m2CCreateUnits = new M2C_CreateMyUnit();
                        m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
                        MessageHelper.SendToClient(unit, m2CCreateUnits);
                        // 加入aoi
                        unit.AddComponent<AOIEntity, int, Vector3>(2 * 1000, unit.Position);
                        //TransferHelper.AfterTransfer(unit);

                        unit.DomainScene().GetComponent<HappyDungeonComponent>().NoticeRefreshTime(unit);
                        break;
                    case SceneTypeEnum.Battle:
						//int todayCamp = numericComponent.GetAsInt(NumericType.BattleTodayCamp);
						//todayCamp = todayCamp > 0 ? todayCamp : int.Parse(request.ParamInfo);
						int todayCamp = int.Parse(request.ParamInfo);
						numericComponent.Set(NumericType.BattleCamp, todayCamp); //1 2
						//numericComponent.Set(NumericType.BattleTodayCamp, todayCamp); //1 2
						unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
						sceneConfig = SceneConfigCategory.Instance.Get(request.ChapterId);
						int startIndex = todayCamp == 1 ? 0 : 3;
						unit.Position = new Vector3(sceneConfig.InitPos[startIndex+0] * 0.01f, sceneConfig.InitPos[startIndex + 1] * 0.01f, sceneConfig.InitPos[startIndex + 2] * 0.01f);
						unit.Rotation = Quaternion.identity;
						// 通知客户端创建My Unit
						m2CCreateUnits = new M2C_CreateMyUnit();
						m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
						MessageHelper.SendToClient(unit, m2CCreateUnits);
						// 加入aoi
						unit.AddComponent<AOIEntity, int, Vector3>(4 * 1000, unit.Position);

						TransferHelper.AfterTransfer(unit);
                        break;
					case SceneTypeEnum.Arena:
						unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
						sceneConfig = SceneConfigCategory.Instance.Get(request.ChapterId);
						unit.Position = new Vector3(sceneConfig.InitPos[0] * 0.01f, sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f);
						unit.Rotation = Quaternion.identity;

						// 通知客户端创建My Unit
						m2CCreateUnits = new M2C_CreateMyUnit();
						m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
						MessageHelper.SendToClient(unit, m2CCreateUnits);
						// 加入aoi
						unit.AddComponent<AOIEntity, int, Vector3>(4 * 1000, unit.Position);
						TransferHelper.AfterTransfer(unit);
						unit.DomainScene().GetComponent<ArenaDungeonComponent>().OnUpdateRank();
						break;
					case SceneTypeEnum.UnionRace:
						unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
						sceneConfig = SceneConfigCategory.Instance.Get(request.ChapterId);
						unit.Position = new Vector3(sceneConfig.InitPos[0] * 0.01f, sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f);
						unit.Rotation = Quaternion.identity;

						// 通知客户端创建My Unit
						m2CCreateUnits = new M2C_CreateMyUnit();
						m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
						MessageHelper.SendToClient(unit, m2CCreateUnits);
						// 加入aoi
						unit.AddComponent<AOIEntity, int, Vector3>(4 * 1000, unit.Position);
						TransferHelper.AfterTransfer(unit);
						break;
					case SceneTypeEnum.Solo:
						numericComponent.ApplyValue(NumericType.JueXingAnger, 0, false);
                        unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
                        sceneConfig = SceneConfigCategory.Instance.Get(request.ChapterId);

					    List<Unit> units =  UnitHelper.GetUnitList(unit.DomainScene(), UnitType.Player );
						if (units.Count == 1)
						{
							//第1个人
							unit.Position = new Vector3(sceneConfig.InitPos[0] * 0.01f, sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f);
						}

						if (units.Count == 2)
						{
							//第2个人
							unit.Position = new Vector3(10.07f, 0f, 0.27f);
						}

						unit.Rotation = Quaternion.identity;

                        // 通知客户端创建My Unit
                        m2CCreateUnits = new M2C_CreateMyUnit();
                        m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
                        MessageHelper.SendToClient(unit, m2CCreateUnits);
                        // 加入aoi
                        unit.AddComponent<AOIEntity, int, Vector3>(6 * 1000, unit.Position);

                        TransferHelper.AfterTransfer(unit);
                        break;
					case SceneTypeEnum.RunRace:
                        unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
                        sceneConfig = SceneConfigCategory.Instance.Get(request.ChapterId);
                        unit.Position = new Vector3(sceneConfig.InitPos[0] * 0.01f + RandomHelper.RandomNumberFloat(-1f, 1f), sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f + RandomHelper.RandomNumberFloat(-1f, 1f));
                        unit.Rotation = Quaternion.identity;

                        unit.GetComponent<NumericComponent>().ApplyValue(NumericType.HorseRide, 0, false);
						int runracemonster = ConfigHelper.RunRaceMonsterList[RandomHelper.RandomNumber(0, ConfigHelper.RunRaceMonsterList.Count)];
						numericComponent.Set(NumericType.RunRaceTransform, runracemonster, false);
						Function_Fight.GetInstance().UnitUpdateProperty_RunRace(unit, false);

						// 通知客户端创建My Unit
						m2CCreateUnits = new M2C_CreateMyUnit();
                        m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
                        MessageHelper.SendToClient(unit, m2CCreateUnits);
                        // 加入aoi
                        unit.AddComponent<AOIEntity, int, Vector3>(9 * 1000, unit.Position);

                        unit.DomainScene().GetComponent<RunRaceDungeonComponent>().OnEnter(unit);
                        break;
					case SceneTypeEnum.Demon:
                        unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
                        sceneConfig = SceneConfigCategory.Instance.Get(request.ChapterId);
                        unit.Position = new Vector3(sceneConfig.InitPos[0] * 0.01f + RandomHelper.RandomNumberFloat(-1f, 1f), sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f + RandomHelper.RandomNumberFloat(-1f, 1f));
                        unit.Rotation = Quaternion.identity;

                        unit.GetComponent<NumericComponent>().ApplyValue(NumericType.HorseRide, 0, false);
                        // 通知客户端创建My Unit
                        m2CCreateUnits = new M2C_CreateMyUnit();
                        m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
                        MessageHelper.SendToClient(unit, m2CCreateUnits);
                        // 加入aoi
                        unit.AddComponent<AOIEntity, int, Vector3>(9 * 1000, unit.Position);
                        break;
                    case SceneTypeEnum.OneChallenge:
                        unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
                        sceneConfig = SceneConfigCategory.Instance.Get(request.ChapterId);
						if (unit.GetParent<UnitComponent>().GetAll().Count == 1)
                        {
							//第一个玩家坐标
                            unit.Position = new Vector3(sceneConfig.InitPos[0] * 0.01f, sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f);
                            unit.Rotation = Quaternion.identity;
                        }
						else
						{
                            //第二个玩家坐标
                            unit.Position = new Vector3(sceneConfig.InitPos[0] * 0.01f, sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f);
                            unit.Rotation = Quaternion.identity;
                        }

                        // 通知客户端创建My Unit
                        m2CCreateUnits = new M2C_CreateMyUnit();
                        m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
                        MessageHelper.SendToClient(unit, m2CCreateUnits);
                        // 加入aoi
                        unit.AddComponent<AOIEntity, int, Vector3>(9 * 1000, unit.Position);

                        TransferHelper.AfterTransfer(unit);
                        break;
                    case SceneTypeEnum.JiaYuan:
					case SceneTypeEnum.Union:
					case SceneTypeEnum.BaoZang:
					case SceneTypeEnum.MiJing:
					case SceneTypeEnum.Tower:
                    case SceneTypeEnum.TeamDungeon:
                    case SceneTypeEnum.RandomTower:
                    case SceneTypeEnum.TrialDungeon:
                    case SceneTypeEnum.SeasonTower:
                        unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
						sceneConfig = SceneConfigCategory.Instance.Get(request.ChapterId);
						unit.Position = new Vector3(sceneConfig.InitPos[0] * 0.01f, sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f);
						unit.Rotation = Quaternion.identity;

						// 通知客户端创建My Unit
						m2CCreateUnits = new M2C_CreateMyUnit();
						m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
						MessageHelper.SendToClient(unit, m2CCreateUnits);
						// 加入aoi
						unit.AddComponent<AOIEntity, int, Vector3>(9 * 1000, unit.Position);

						if (!unit.IsRobot() && request.SceneType == SceneTypeEnum.TeamDungeon)
						{
							TeamDungeonComponent teamDungeonComponent = unit.DomainScene().GetComponent<TeamDungeonComponent>();
							int fubenType = teamDungeonComponent.FubenType;
							bool firstEnter = !teamDungeonComponent.EnterPlayers.Contains(unit.Id);
							if (firstEnter)
							{
                                teamDungeonComponent.EnterPlayers.Add(unit.Id);
                                if (fubenType == TeamFubenType.XieZhu && unit.Id == teamDungeonComponent.TeamInfo.TeamId)
                                {
                                    int times_2 = unit.GetTeamDungeonXieZhu();
                                    int totalTimes_2 = int.Parse(GlobalValueConfigCategory.Instance.Get(74).Value);
                                    if (totalTimes_2 > times_2)
                                    {
                                        unit.GetComponent<NumericComponent>().ApplyValue(NumericType.TeamDungeonXieZhu, unit.GetTeamDungeonXieZhu() + 1);
                                    }
                                    else
                                    {
                                        unit.GetComponent<NumericComponent>().ApplyValue(NumericType.TeamDungeonTimes, unit.GetTeamDungeonTimes() + 1);
                                    }
                                }
                                else
                                {
                                    unit.GetComponent<NumericComponent>().ApplyValue(NumericType.TeamDungeonTimes, unit.GetTeamDungeonTimes() + 1);
                                }
                                if (fubenType == TeamFubenType.ShenYuan && unit.Id == teamDungeonComponent.TeamInfo.TeamId)
                                {
                                    unit.GetComponent<BagComponent>().OnCostItemData($"{ComHelp.ShenYuanCostId};1", ItemLocType.ItemLocBag, ItemGetWay.FubenGetReward);
                                }
                                if (fubenType == TeamFubenType.ShenYuan)
                                {
                                    unit.GetComponent<TaskComponent>().TriggerTaskEvent(TaskTargetType.ShenYuanNumber_135, 0, 1);
                                    unit.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.ShenYuanNumber_135, 0, 1);
                                }
                            }
                        }
						if (request.SceneType == (int)SceneTypeEnum.Tower)
						{
							Game.Scene.GetComponent<RecastPathComponent>().Update(scene.GetComponent<MapComponent>().NavMeshId);
							unit.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.Tower_1013, 0, 1);
							scene.GetComponent<TowerComponent>().MainUnit = unit;
						}
						if (request.SceneType == SceneTypeEnum.RandomTower)
						{
							Game.Scene.GetComponent<RecastPathComponent>().Update(scene.GetComponent<MapComponent>().NavMeshId);
							scene.GetComponent<RandomTowerComponent>().MainUnit = unit;
						}
						if (request.SceneType == SceneTypeEnum.TrialDungeon)
						{
							Game.Scene.GetComponent<RecastPathComponent>().Update(scene.GetComponent<MapComponent>().NavMeshId);
							scene.GetComponent<TrialDungeonComponent>().GenerateFuben(int.Parse(request.ParamInfo));
							unit.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.TrialFuben_1012, 0, 1);
						}
						if(request.SceneType == SceneTypeEnum.SeasonTower)
						{
                            Game.Scene.GetComponent<RecastPathComponent>().Update(scene.GetComponent<MapComponent>().NavMeshId);
							scene.GetComponent<SeasonTowerComponent>().GenerateFuben(int.Parse(request.ParamInfo));
                        }
						
                        TransferHelper.AfterTransfer(unit);
                        break;
                    case SceneTypeEnum.TowerOfSeal:
	                    unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
	                    sceneConfig = SceneConfigCategory.Instance.Get(request.ChapterId);
	                    unit.Position = new Vector3(sceneConfig.InitPos[0] * 0.01f, sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f);
	                    unit.Rotation = Quaternion.identity;

	                    // 通知客户端创建My Unit
	                    m2CCreateUnits = new M2C_CreateMyUnit();
	                    m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
	                    MessageHelper.SendToClient(unit, m2CCreateUnits);
	                    // 加入aoi
	                    unit.AddComponent<AOIEntity, int, Vector3>(4 * 1000, unit.Position);

	                    Game.Scene.GetComponent<RecastPathComponent>().Update(scene.GetComponent<MapComponent>().NavMeshId);
	                    scene.GetComponent<TowerOfSealComponent>().MyUnit = unit;
	                    scene.GetComponent<TowerOfSealComponent>()
			                    .GenerateFuben(unit.GetComponent<NumericComponent>().GetAsInt(NumericType.TowerOfSealArrived),
				                    unit.GetComponent<NumericComponent>().GetAsInt(NumericType.TowerOfSealFinished));

                        TransferHelper.AfterTransfer(unit);
                        break;
					case (int)SceneTypeEnum.MainCityScene:
						sceneConfig = SceneConfigCategory.Instance.Get(ComHelp.MainCityID());
						numericComponent = unit.GetComponent<NumericComponent>();
						if (numericComponent.GetAsFloat(NumericType.MainCity_X) != 0f)
						{
							unit.Position = new Vector3(numericComponent.GetAsFloat(NumericType.MainCity_X),
								numericComponent.GetAsFloat(NumericType.MainCity_Y),
								numericComponent.GetAsFloat(NumericType.MainCity_Z));
						}
						else
						{
							unit.Position = new Vector3(sceneConfig.InitPos[0] * 0.01f + RandomHelper.RandFloat01(),
								sceneConfig.InitPos[1] * 0.01f, sceneConfig.InitPos[2] * 0.01f + RandomHelper.RandFloat01());
						}
						if (unit.IsRobot())
						{
                            unit.Position = new Vector3(-26f + RandomHelper.RandFloat01() * 2f , -4f, -8f + RandomHelper.RandFloat01() * 2f);
                        }
						unitComponent.AddPlayer(unit);		
						unit.AddComponent<PathfindingComponent, int>(scene.GetComponent<MapComponent>().NavMeshId);
						unit.GetComponent<HeroDataComponent>().OnReturn();
						// 通知客户端创建My Unit
						m2CCreateUnits = new M2C_CreateMyUnit();
						m2CCreateUnits.Unit = UnitHelper.CreateUnitInfo(unit);
						MessageHelper.SendToClient(unit, m2CCreateUnits);

						// 加入aoi
						unit.AddComponent<AOIEntity, int, Vector3>(4 * 1000, unit.Position);
						TransferHelper.AfterTransfer(unit);
						TransferHelper.RemoveStall(unit);

                        break;
				}

                unit.GetComponent<DBSaveComponent>().Check_2();
                unit.GetComponent<DBSaveComponent>().Activeted();
               
                if (request.SceneType != SceneTypeEnum.RunRace)
				{
                    unit.GetComponent<BuffManagerComponent>().InitBuff(request.SceneType);
                    unit.GetComponent<SkillPassiveComponent>().Reset();
                    unit.GetComponent<SkillPassiveComponent>().Activeted();
                    unit.OnUpdateHorseRide(0);
                    unit.TriggerTeamBuff(request.SceneType);
                }
                //Function_Fight.GetInstance().UnitUpdateProperty_Base(unit, false, true);
				response.NewInstanceId = unit.InstanceId;
				reply();
                await ETTask.CompletedTask;
            }
			catch (Exception ex)
			{
				Log.Debug($"LoginTest M2M_UnitTransfer Exception  {request.Unit.Id} {ex.ToString()}");
			}
		}
	}
}