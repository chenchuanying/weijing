﻿
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{ 
    public static class FubenHelp
	{

		public static M2C_SyncChatInfo m2C_SyncChatInfo = new M2C_SyncChatInfo();



		/// <summary>
		/// 触发BUFF
		/// </summary>
		/// <param name="self"></param>
		/// <param name="sceneType"></param>
        public static void TriggerTeamBuff(this Unit self, int sceneType)
        {
            if (sceneType == SceneTypeEnum.MainCityScene)
            {
                return;
            }

            List<Unit> entities = self.DomainScene().GetComponent<UnitComponent>().GetAll();
            for (int i = entities.Count - 1; i >= 0; i--)
            {
                if (entities[i].Type != UnitType.Player)
                {
                    continue;
                }
                if (self.IsSameTeam(entities[i]))
                {
                    entities[i].GetComponent<SkillPassiveComponent>().OnTrigegerPassiveSkill(SkillPassiveTypeEnum.TeamerEnter_12);
					entities[i].GetComponent<SkillManagerComponent>().TriggerTeamBuff();
                }
            }
        }

        /// <summary>
        /// 寻找一个可通行的随机位置
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="from"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static bool GetCanReachPath(Scene scene, int navMeshId, Vector3 from, Vector3 target)
		{
			using var list = ListComponent<Vector3>.Create();
			//scene.GetComponent<PathfindingComponent>().Find(from, target, list);
			List<Vector3> path = list;
			if (path.Count >= 2)
				return true;

			return false;
		}

		public static void CreateMonsterList(Scene scene, int[] monsterPos)
		{
			if (monsterPos == null || monsterPos.Length == 0)
			{
				return;
			}
			for (int i = 0; i < monsterPos.Length;i++)
			{
				int monsterId = monsterPos[i];

				int whileNumber = 0;

                while (monsterId != 0)
				{
                    whileNumber++;
                    if (whileNumber >= 100)
                    {
                        Log.Error("whileNumber >= 100");
                        break;
                    }

					try
					{
						monsterId = CreateMonsterByPos(scene, monsterId);
					}
					catch (Exception ex)
					{
						Log.Error(ex.ToString());
					}
				}
			}
		}

		public static int CreateMonsterByPos(Scene scene, int monsterPos)
		{
			if (monsterPos == 0)
			{
				return 0;
			}
			//Id      NextID  Type Position             MonsterID CreateRange CreateNum Create    Par(3代表刷新时间)
			//10001   10002   2    - 71.46,0.34,-5.35   81000002       0           1       90    30,60
			MonsterPositionConfig monsterPosition = MonsterPositionConfigCategory.Instance.Get(monsterPos);
			int mtype = monsterPosition.Type;
			int monsterid = monsterPosition.MonsterID;
			string[] position = monsterPosition.Position.Split(',');
			if (mtype == 1)    //固定位置刷怪
			{
				if (monsterPosition.CreateNum > 100)
				{
					Log.Error($"monsterPosition.CreateNum:  {monsterPos}");
                    return 0;
                }

				for (int c = 0; c < monsterPosition.CreateNum; c++)
				{
					MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterid);
					Vector3 vector3 = new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2]));

					//51 场景怪 有AI 不显示名称
					//52 能量台子 无AI
					//54 场景怪 有AI 显示名称
					//55 宝箱类(一次) 无AI
					//56 宝箱类(无限) 无AI
					if (monsterConfig.MonsterSonType != 52)
					{
						UnitFactory.CreateMonster(scene, monsterConfig.Id, vector3, new CreateMonsterInfo()
						{
							Camp = monsterConfig.MonsterCamp,
							Rotation = monsterPosition.Create,
						});
					}
				}
			}
			if (mtype == 2)
			{
                if (monsterPosition.CreateNum > 100)
                {
                    Log.Error($"monsterPosition.CreateNum:  {monsterPos}");
                    return 0;
                }

                for (int c = 0; c < monsterPosition.CreateNum; c++)
				{
					float range = (float)monsterPosition.CreateRange;
					MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterPosition.MonsterID);
					Vector3 vector3 = new Vector3(float.Parse(position[0]) + RandomHelper.RandomNumberFloat(-1 * range, range), float.Parse(position[1]), float.Parse(position[2]) + RandomHelper.RandomNumberFloat(-1 * range, range));
					UnitFactory.CreateMonster(scene, monsterPosition.MonsterID, vector3, new CreateMonsterInfo()
					{
						Camp = monsterConfig.MonsterCamp,
						Rotation = monsterPosition.Create,
					});
				}
			}
			if (mtype == 3)
			{
				//定时刷新  YeWaiRefreshComponent
				scene.GetComponent<YeWaiRefreshComponent>().CreateMonsterByPos(monsterPosition.Id);
			}
			if (mtype == 4)
			{
				//4; 0,0,0; 71020001; 2,2; 2, 2
				int playerLv = 1;
				if (scene.GetComponent<MapComponent>().SceneTypeEnum == SceneTypeEnum.Tower)
				{
					Unit mainUnit = scene.GetComponent<TowerComponent>().MainUnit;
					playerLv = mainUnit.GetComponent<UserInfoComponent>().UserInfo.Lv;
				}
                if (monsterPosition.CreateNum > 100)
                {
                    Log.Error($"monsterPosition.CreateNum:  {monsterPos}");
                    return 0;
                }

                for (int c = 0; c < monsterPosition.CreateNum; c++)
				{
					float range = (float)monsterPosition.CreateRange;
					MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterPosition.MonsterID);
					Vector3 vector3 = new Vector3(float.Parse(position[0]) + RandomHelper.RandomNumberFloat(-1 * range, range), float.Parse(position[1]), float.Parse(position[2]) + RandomHelper.RandomNumberFloat(-1 * range, range));
					UnitFactory.CreateMonster(scene, monsterPosition.MonsterID, vector3, new CreateMonsterInfo()
					{
						PlayerLevel = playerLv,
						AttributeParams = monsterPosition.Par,
						Camp = monsterConfig.MonsterCamp,
						Rotation = monsterPosition.Create,
					});
				}
			}
			if (mtype == 5 || mtype == 6)
			{
				//固定时间刷新  YeWaiRefreshComponent
				scene.GetComponent<YeWaiRefreshComponent>().CreateMonsterByPos_2(monsterPosition.Id);
			}

			return monsterPosition.NextID;
		}

		public static List<KeyValuePairInt> GetRandomMonster(Scene scene, int fubenid, string createMonster)
		{
            List<KeyValuePairInt> randomMonsterList = new List<KeyValuePairInt>();	

            MapComponent mapComponent = scene.GetComponent<MapComponent>();
			int sceneType = mapComponent.SceneTypeEnum;
			if (sceneType != SceneTypeEnum.LocalDungeon)
			{ 
				return randomMonsterList;
			}

			LocalDungeonComponent localDungeonComponent = scene.GetComponent<LocalDungeonComponent>();
			Unit mainUnit = localDungeonComponent.MainUnit;

            UserInfoComponent userInfoComponent = mainUnit.GetComponent<UserInfoComponent>();
			NumericComponent numericComponent = mainUnit.GetComponent<NumericComponent>();
			
			
			TaskPro taskPro = mainUnit.GetComponent<TaskComponent>().GetTreasureMonster(mapComponent.SceneId);
			if (taskPro!=null)
			{
				TaskConfig taskConfig = TaskConfigCategory.Instance.Get(taskPro.taskID);
                KeyValuePairInt keyValuePairInt = new KeyValuePairInt();
                keyValuePairInt.KeyId = taskPro.WaveId;
				keyValuePairInt.Value = taskConfig.Target[0];
                randomMonsterList.Add( keyValuePairInt );
			}

            string[] monsters = createMonster.Split('@');
            if (SeasonHelper.GetOpenSeason(userInfoComponent.UserInfo.Lv)!= null)
			{
				//赛季boss
				long serverNow = TimeHelper.ServerNow();
				long seasonBossTime = numericComponent.GetAsLong(NumericType.SeasonBossRefreshTime);
				int sessonBossFuben = numericComponent.GetAsInt(NumericType.SeasonBossFuben);
                if (seasonBossTime > 0 && serverNow > seasonBossTime && fubenid == sessonBossFuben)
				{
                    KeyValuePairInt keyValuePairInt = new KeyValuePairInt();
					keyValuePairInt.KeyId = RandomHelper.RandomNumber(0, monsters.Length);
                    keyValuePairInt.Value = SeasonHelper.SeasonBossId;
                    randomMonsterList.Add(keyValuePairInt);
                }
            }
			
			for (int i = 0; i < monsters.Length; i++)
			{
				if (ComHelp.IfNull(monsters[i]))
				{
					continue;
				}

				string[] mondels = monsters[i].Split(';');
				int monsterid = int.Parse(mondels[2]);
				MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterid);
				if (monsterConfig.MonsterType != MonsterTypeEnum.Normal && monsterConfig.MonsterSonType != 55)
				{
					continue;
				}
				if (numericComponent.GetAsInt(NumericType.LocalDungeonTime) >= 30)
				{
					break;
				}

				int randomid = userInfoComponent.GetRandomMonsterId();
				if (randomid > 0)
				{
					localDungeonComponent.RandomMonster = randomid;
                    KeyValuePairInt keyValuePairInt = new KeyValuePairInt();
                    keyValuePairInt.KeyId = i;
					keyValuePairInt.Value = randomid;

                    randomMonsterList.Add( keyValuePairInt );	
                    break;
				}

				randomid = userInfoComponent.GetRandomJingLingId();
				if (randomid > 0)
				{
					localDungeonComponent.RandomJingLing = randomid;
                    KeyValuePairInt keyValuePairInt = new KeyValuePairInt();
                    keyValuePairInt.KeyId = i;
					keyValuePairInt.Value = randomid;

                    randomMonsterList.Add(keyValuePairInt);
                    break;
				}
			}

			return randomMonsterList;
		}

		public static  void CreateMonsterList(Scene scene, string createMonster)
		{
			if (ComHelp.IfNull(createMonster))
            {
				return;
            }

			MapComponent mapComponent = scene.GetComponent<MapComponent>();
			int sceneType = mapComponent.SceneTypeEnum;
			string[] monsters = createMonster.Split('@');
			//1;37.65,0,3.2;70005005;1@138.43,0,0.06;70005010;1

			List<KeyValuePairInt> randomMonsterList = GetRandomMonster(scene, mapComponent.SceneId, createMonster);

			for (int i = 0; i < monsters.Length; i++)
			{
				if (ComHelp.IfNull(monsters[i]))
				{
					continue;
				}
				//2;37.65,0,3.2;70005005;1,2
				string[] mondels = monsters[i].Split(';');
				string[] mtype = mondels[0].Split(',');
				string[] position = mondels[1].Split(',');
				int monsterid =  int.Parse(mondels[2]);
				string[] mcount = mondels[3].Split(',');
				if (!MonsterConfigCategory.Instance.Contain(monsterid))
				{
					Log.Error($"monsterid==null {monsterid}");
					continue;
				}

				bool haveotherMonster = false;
				MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(monsterid);
				for (int kk = 0; kk < randomMonsterList.Count; kk++)
				{
					if (position.Length < 3)
					{
						Log.Warning($"生成随机怪错误： {mapComponent.SceneId} {i} {(int)randomMonsterList[kk].Value}  {position}");
					}

                    if (randomMonsterList[kk].KeyId == i && (int)randomMonsterList[kk].Value > 0 && position.Length >= 3)
					{
						monsterid = (int)randomMonsterList[kk].Value;
						monsterConfig = MonsterConfigCategory.Instance.Get(monsterid);

						int skinId = 0;
                        if (monsterConfig.MonsterSonType == 58) //奇遇宠物
                        {
							int itemid = monsterConfig.Parameter[1];
							ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemid);
							int petId = int.Parse(itemConfig.ItemUsePar);
							PetConfig petConfig = PetConfigCategory.Instance.Get(petId );

                            List<int> weight = new List<int>(petConfig.SkinPro);
                            int index = RandomHelper.RandomByWeight(weight);
                            skinId = petConfig.Skin[index];
                        }
                        Vector3 vector3 = new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2]));
                        Unit unitmonster = UnitFactory.CreateMonster(scene, monsterid, vector3, new CreateMonsterInfo()
                        {
                            Camp = monsterConfig.MonsterCamp,
							SkinId = skinId,	
                        });
                       
                        haveotherMonster = true;
                    }
				}
				if (haveotherMonster)
				{
					continue;
				}
				
				if (sceneType == SceneTypeEnum.LocalDungeon && monsterConfig.MonsterSonType == 55)
				{
					LocalDungeonComponent localDungeonComponent = scene.GetComponent<LocalDungeonComponent>();
					UserInfoComponent userInfoComponent = localDungeonComponent.MainUnit.GetComponent<UserInfoComponent>();
					TaskComponent taskComponent = localDungeonComponent.MainUnit.GetComponent<TaskComponent>();
					if (userInfoComponent.IsCheskOpen(mapComponent.SceneId, monsterid)
					&& !taskComponent.IsItemTask(monsterid))
					{
						continue;
					}
				}

				if (mtype[0] == "1")//固定位置刷怪
				{
                    int cmcount = int.Parse(mcount[0]);
                    if (cmcount > 100)
					{
						Log.Error($"int.Parse(mcount[0]) > 100； {createMonster}");
                        return;
                    }

					for (int c = 0; c < cmcount; c++)
					{
						Vector3 vector3 = new Vector3(float.Parse(position[0]), float.Parse(position[1]), float.Parse(position[2]));

						//51 场景怪 有AI 不显示名称
						//52 能量台子 无AI
						//54 场景怪 有AI 显示名称
						//55 宝箱类 无AI
						if (monsterConfig.MonsterSonType == 52)
						{
							CellDungeonComponent cellDungeonComponent = scene.GetComponent<CellDungeonComponent>();
							if (cellDungeonComponent!=null)
							{
								List<int> EnergySkills = cellDungeonComponent.EnergySkills;
								int skillId = EnergySkills[RandomHelper.RandomNumber(0, EnergySkills.Count)];
								EnergySkills.Remove(skillId);
								UnitFactory.CreateMonster(scene, monsterConfig.Id, vector3, new CreateMonsterInfo()
								{
									SkillId = skillId,
									Camp = monsterConfig.MonsterCamp
								});
							}
						}
						else
						{
							UnitFactory.CreateMonster(scene, monsterid, vector3,  new CreateMonsterInfo()
							{
								Camp = monsterConfig.MonsterCamp
							});
						}
					}
				}
				if (mtype[0] == "2") //随机位置
				{
					int cmcount = int.Parse(mcount[0]);
                    if (cmcount > 100)
                    {
                        Log.Error($"int.Parse(mcount[0]) > 100； {createMonster}");
                        return;
                    }

                    for (int c = 0; c < cmcount; c++)
					{
						float range = float.Parse(mcount[1]);
						Vector3 vector3 = new Vector3(float.Parse(position[0]) + RandomHelper.RandomNumberFloat(-1 * range, range), float.Parse(position[1]), float.Parse(position[2]) + RandomHelper.RandomNumberFloat(-1 * range, range));
						UnitFactory.CreateMonster(scene, monsterid, vector3, new CreateMonsterInfo()
						{ 
							Camp = monsterConfig.MonsterCamp
						});
					}
				}
				if (mtype[0] == "3")
				{
					//野外场景定时刷新
					//scene.GetComponent<YeWaiRefreshComponent>().CreateMonsterList(createMonster);
					scene.GetComponent<YeWaiRefreshComponent>().CreateMonsterList(monsters[i]);
				}
				if (mtype[0] == "4")
				{
					//4; 0,0,0; 71020001; 2,2; 2, 2  //是随机塔附加属性
					int playerLv = 1;
					if (scene.GetComponent<MapComponent>().SceneTypeEnum == SceneTypeEnum.Tower)
					{
						Unit mainUnit = scene.GetComponent<TowerComponent>().MainUnit;
						playerLv = mainUnit.GetComponent<UserInfoComponent>().UserInfo.Lv;
					}
					int cmcount = int.Parse(mcount[0]);
                    if (cmcount > 100)
                    {
                        Log.Error($"int.Parse(mcount[0]) > 100； {createMonster}");
						return;
                    }

                    for (int c = 0; c < cmcount; c++)
					{
						float range = float.Parse(mcount[1]);
						Vector3 vector3 = new Vector3(float.Parse(position[0]) + RandomHelper.RandomNumberFloat(-1 * range, range), float.Parse(position[1]), float.Parse(position[2]) + RandomHelper.RandomNumberFloat(-1 * range, range));
						UnitFactory.CreateMonster(scene, monsterid, vector3,  new CreateMonsterInfo() {
							PlayerLevel = playerLv, AttributeParams = mondels[4] + ";" + mondels[5],
							Camp = monsterConfig.MonsterCamp
						});
					}
				}
				//固定时间刷新
				if (mtype[0] == "5" || mtype[0] == "6")
				{
					//scene.GetComponent<YeWaiRefreshComponent>().CreateMonsterList_2(createMonster);
					scene.GetComponent<YeWaiRefreshComponent>().CreateMonsterList_2(monsters[i]);
				}
			}
		}

		public static void CreateNpc(Scene scene, int sceneId)
		{
			SceneConfig sceneConfig = SceneConfigCategory.Instance.Get(sceneId);
			int[] npcs = sceneConfig.NpcList;
			if (npcs == null)
			{
				return;
			}
			for (int i = 0; i < npcs.Length; i++)
			{
				if (npcs[i] == 0)
				{
					continue;
				}
				UnitFactory.CreateNpc(scene, npcs[i]);
			}
		}

		public static bool IsAllMonsterDead(Scene scene, Unit main)
		{
			List<Unit> units = scene.GetComponent<UnitComponent>().GetAll();
			for(int i = 0; i < units.Count; i++)
			{
				if (units[i].Type == UnitType.Monster && main.IsCanAttackUnit(units[i]))
				{
					return false;
				}
			}

			return true;
		}

		public static int GetAlivePetNumber(Scene scene)
		{
			int petNumber = 0;
			List<Unit> units = scene.GetComponent<UnitComponent>().GetAll();
			for(int i = 0; i < units.Count; i++)
			{
				if (units[i].Type == UnitType.Pet && units[i].GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Dead) == 0)
				{
					petNumber++;
				}
			}

			return petNumber;
		}

		public static void SendTeamPickMessage(Unit unit, DropInfo dropInfo,List<long> ids,  List<int> points)
		{
			m2C_SyncChatInfo.ChatInfo = new ChatInfo();
			m2C_SyncChatInfo.ChatInfo.PlayerLevel = unit.GetComponent<UserInfoComponent>().UserInfo.Lv;
			m2C_SyncChatInfo.ChatInfo.Occ = unit.GetComponent<UserInfoComponent>().UserInfo.Occ;
			m2C_SyncChatInfo.ChatInfo.ChannelId = (int)ChannelEnum.Pick;

			ItemConfig itemConfig = ItemConfigCategory.Instance.Get(dropInfo.ItemID);
			string numShow = "";
			if (itemConfig.Id == 1)
			{
				numShow = dropInfo.ItemNum.ToString();
			}
			string colorValue = ComHelp.QualityReturnColor(itemConfig.ItemQuality);
			m2C_SyncChatInfo.ChatInfo.ChatMsg = $"<color=#FDD376>{unit.GetComponent<UserInfoComponent>().UserInfo.Name}</color>拾取<color=#{colorValue}>{numShow}{itemConfig.ItemName}</color>";

			for (int p = 0; p < points.Count; p++)
			{
				Unit player = unit.GetParent<UnitComponent>().Get(ids[p]);
				if (player == null)
				{
					continue;
				}
				
				m2C_SyncChatInfo.ChatInfo.ChatMsg += $"{player.GetComponent<UserInfoComponent>().UserInfo.Name}:{points[p]}点";
				m2C_SyncChatInfo.ChatInfo.ChatMsg += (p == points.Count - 1 ? "" : "  ");
			}

			MessageHelper.SendToClient(UnitHelper.GetUnitList(unit.DomainScene(), UnitType.Player), m2C_SyncChatInfo);
		}

		public static void SendFubenPickMessage(Unit unit, DropInfo dropInfo)
		{
			UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
			m2C_SyncChatInfo.ChatInfo = new ChatInfo();
			m2C_SyncChatInfo.ChatInfo.PlayerLevel = userInfoComponent.UserInfo.Lv;
			m2C_SyncChatInfo.ChatInfo.Occ = userInfoComponent.UserInfo.Occ;
			m2C_SyncChatInfo.ChatInfo.ChannelId = (int)ChannelEnum.Pick;

			ItemConfig itemConfig = ItemConfigCategory.Instance.Get(dropInfo.ItemID);
			string numShow = "";
			if (itemConfig.Id == 1)
			{
				numShow = dropInfo.ItemNum.ToString();
			}
			string colorValue = ComHelp.QualityReturnColor(itemConfig.ItemQuality);
			m2C_SyncChatInfo.ChatInfo.ChatMsg = $"<color=#FDD376>{unit.GetComponent<UserInfoComponent>().UserInfo.Name}</color>拾取<color=#{colorValue}>{numShow}{itemConfig.ItemName}</color>";
			//MessageHelper.SendToClient(GetUnitList(unit.DomainScene(), UnitType.Player), m2C_SyncChatInfo);
			//Log.Warning($"SendFubenPickMessage: {unit.Id} {dropInfo.ItemID}");
			MessageHelper.SendToClient(unit, m2C_SyncChatInfo);
		}
	}
}
