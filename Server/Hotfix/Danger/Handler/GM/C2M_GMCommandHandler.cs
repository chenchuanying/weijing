using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    [ActorMessageHandler]
    public class C2M_GMCommandHandler : AMActorLocationHandler<Unit, C2M_GMCommandRequest>
    {
		protected override async ETTask Run(Unit unit, C2M_GMCommandRequest message)
		{
			try
			{
				if (!GMHelp.GmAccount.Contains(message.Account)
					&& !ComHelp.IsBanHaoZone(unit.DomainZone()))
				{
					return;
				}

				string[] commands = message.GMMsg.Split('#');
				if (commands.Length == 0)
				{
					return;
				}
				if (message.GMMsg == "#allmonster")
				{
					List<MonsterConfig> monsterConfigs = MonsterConfigCategory.Instance.GetAll().Values.ToList();
					for (int i = 0; i < monsterConfigs.Count; i++)
					{
						await TimerComponent.Instance.WaitAsync(1);
						Vector3 pos = unit.Position;
						Vector3 vector3 = new Vector3(pos.x + RandomHelper.RandFloat01() * 1, pos.y, pos.z + RandomHelper.RandFloat01() * 1);
						Unit monster = UnitFactory.CreateMonster(unit.DomainScene(), monsterConfigs[i].Id, vector3,  new CreateMonsterInfo()
						{ 
							Camp  = CampEnum.CampMonster1
						});
					}
					return;
				}
				if (message.GMMsg == "#mianshang")
				{
					BuffData buffData_1 = new BuffData();
					buffData_1.SkillId = 67000278;
					buffData_1.BuffId = 90106002;
					unit.GetComponent<BuffManagerComponent>().BuffFactory(buffData_1, unit, null);
					return;
				}
				if (message.GMMsg == "#wudi")
				{
                    BuffData buffData_2 = new BuffData();
					buffData_2.SkillId = 67000278;
					buffData_2.BuffId = 90106003;
					unit.GetComponent<BuffManagerComponent>().BuffFactory(buffData_2, unit, null);
					return;
				}
				if (message.GMMsg == "#openall")
				{
					unit.GetComponent<UserInfoComponent>().OpenAll();
					return;
				}
				if (message.GMMsg == "#resetlv")
				{
					int level = unit.GetComponent<UserInfoComponent>().UserInfo.Lv - 1;
					unit.GetComponent<UserInfoComponent>().UpdateRoleData( UserDataType.Lv, (level*-1).ToString());
					return;
				}
                if (message.GMMsg == "#jiandian")
                {
					unit.GetComponent<BagComponent>().OnAddJianDing();
                    return;
                }
                if (message.GMMsg == "#gaoji"
					|| message.GMMsg == "#zhongji")
				{
					if (!ComHelp.IsBanHaoZone(unit.DomainZone()))
					{
						return;
					}

					//if (unit.GetComponent<UserInfoComponent>().UserInfo.HorseIds.Count > 2)
					//{
					//	return;
					//}
					int level = message.GMMsg == "#gaoji" ? 1 : 2;

                    //激活全部坐骑
                    //家园等级提升到满级
                    //赛季等级达到25级
                    unit.GetComponent<UserInfoComponent>().OnGmGaoJi(level);


                    //角色装备强化全部到上限
                    unit.GetComponent<BagComponent>().OnGmGaoJi(level);

					//每个宠物附带满级的宠物之核,并进化
					unit.GetComponent<PetComponent>().OnGmGaoJi();

                    //激活全部收集
                    //激活全部珍宝
                    unit.GetComponent<ShoujiComponent>().OnGmGaoJi();

                   
                    //激活全部称号
                    unit.GetComponent<TitleComponent>().OnGmGaoJi();

                    //激活全部成就
                    //激活全部精灵
                    unit.GetComponent<ChengJiuComponent>().OnGmGaoJi();

					//激活试炼之地所有层数
					 int trialFubenid = TowerHelper.GetLastTowerIdByScene(SceneTypeEnum.TrialDungeon);
					 unit.GetComponent<NumericComponent>().ApplyValue(NumericType.TrialDungeonId, trialFubenid);

                    //激活家园里的美味大师//激活家园里的菜谱
                    //家园土地默认全部开启
                    unit.GetComponent<JiaYuanComponent>().OnGmGaoJi();
                }

                if (message.GMMsg == "#killall")
				{
					List<Unit> units = unit.GetParent<UnitComponent>().GetAll();
					for(int i = units.Count - 1; i >= 0; i--)
					{
						if (units[i].Type != UnitType.Monster)
						{
							continue;
						}
						units[i].GetComponent<NumericComponent>().ApplyChange(unit, NumericType.Now_Hp, -1000000000, 0);
					}
					return;
				}
				if (message.GMMsg == "#killmonster")
				{
					List<Unit> units = unit.GetParent<UnitComponent>().GetAll();
					for (int i = units.Count - 1; i >= 0; i--)
					{
						if (units[i].Type != UnitType.Monster)
						{
							continue;
						}
						if (units[i].GetMonsterType() == (int)MonsterTypeEnum.Boss)
						{
							continue;
						}
						units[i].GetComponent<NumericComponent>().ApplyChange(unit, NumericType.Now_Hp, -1000000000, 0);
					}
					return;
				}
				if (message.GMMsg == "#resetguide")
				{
					unit.GetComponent<UserInfoComponent>().UserInfo.CompleteGuideIds.Clear();
					return;
				}
				if (message.GMMsg == "#resetfuben")
				{
					unit.GetComponent<NumericComponent>().ApplyValue(NumericType.TeamDungeonTimes, 0);
					unit.GetComponent<NumericComponent>().ApplyValue(NumericType.TeamDungeonXieZhu, 0);
					unit.GetComponent<UserInfoComponent>().UserInfo.DayFubenTimes.Clear();
					return;
				}
                if (message.GMMsg == "#resettower")
                {
                    unit.GetComponent<NumericComponent>().ApplyValue(NumericType.SeasonTowerId, 0);
                    return;
                }
                if (message.GMMsg == "#completetask")
				{
					unit.GetComponent<TaskComponent>().CompletCurrentTask();
					return;
				}
				if (message.GMMsg.Contains("#addack"))  //#addack#30090010
                {
					int addAck = int.Parse(commands[2]);
					unit.GetComponent<NumericComponent>().Set(NumericType.Extra_Buff_MaxAct_Add, addAck);
					return;
				}
				switch (int.Parse(commands[0]))
				{
					case 1:             //新增道具1#12000003#200 【添加道具/道具id/道具数量】
						int itemId = int.Parse(commands[1]);
						int itemNumber = int.Parse(commands[2]);

						List<RewardItem> rewardItems = new List<RewardItem>();
						rewardItems.Add(new RewardItem() { ItemID = itemId, ItemNum = itemNumber });
						unit.GetComponent<BagComponent>().OnAddItemData(rewardItems, string.Empty, $"{ItemGetWay.GM}_{TimeHelper.ServerNow()}", true, true);
						break;
					case 2:       //72009041死亡技能      //2#-2#0#-2#70001001#50 70001001 90000005-爆炸怪 72002013-脱战技能没移除2#-78#0#0.7#72004002#1  70001001  72009001
                        float posX = float.Parse(commands[1]);
						float posY = float.Parse(commands[2]); 
						float posZ = float.Parse(commands[3]);
						int monsterId = int.Parse(commands[4]);
						int number = int.Parse(commands[5]);
						if (number > 100)
						{
							Log.Error("number > 100");
							return;
						}

                        for (int c = 0; c < number; c++)
						{
							await TimerComponent.Instance.WaitAsync(1);
							Vector3 vector3 = new Vector3(posX + RandomHelper.RandomNumberFloat(-1, 1), posY, RandomHelper.RandomNumberFloat(-1, 1));
							Unit monster = UnitFactory.CreateMonster(unit.DomainScene(), monsterId, vector3, new CreateMonsterInfo()
							{ 
								Camp = CampEnum.CampMonster1
							});

							//M2C_CreateSpilings createSpilings = new M2C_CreateSpilings();
							//SpilingInfo spilingInfo = UnitHelper.CreateSpilingInfo(monster);
							//createSpilings.Spilings.Add(spilingInfo);
							//MessageHelper.Broadcast(unit, createSpilings);
						}
						break;
					case 4: //直接接取某个任务      4#30090010
                        unit.GetComponent<TaskComponent>().OnGMGetTask(int.Parse(commands[1]));
						break;
					case 5: //直接获得某个宠物      5#1001301
                        unit.GetComponent<PetComponent>().OnAddPet(ItemGetWay.GM, int.Parse(commands[1]));
						break;
					case 6:
						int newLevel = int.Parse(commands[1]);
						if (newLevel <= 70)
						{
							int level = newLevel - unit.GetComponent<UserInfoComponent>().UserInfo.Lv;
							unit.GetComponent<UserInfoComponent>().UpdateRoleData(UserDataType.Lv, level.ToString());
						}
						break;
					case 7:
						long userID = long.Parse(commands[1]);
						long dbCacheId = DBHelper.GetDbCacheId(unit.DomainZone());

						List<string> componentList = new List<string>() { DBHelper.BagComponent, DBHelper.TaskComponent };
						D2G_GetComponent d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = userID, Component = DBHelper.UserInfoComponent });
						UserInfoComponent userInfoComponent = d2GGetUnit.Component as UserInfoComponent;
						for (int i = 0; i < componentList.Count; i++)
						{
							d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = userID, Component = componentList[i] });
							if (d2GGetUnit.Component == null)
							{
								continue;
							}
						}
						break;
					case 8:
						unit.GetComponent<NumericComponent>().ApplyValue(NumericType.Ling_DiLv, int.Parse(commands[1]));
						unit.GetComponent<NumericComponent>().ApplyValue(NumericType.Ling_DiExp, 0);
						break;
					case 9:
                        long robotSceneId = StartSceneConfigCategory.Instance.GetBySceneName(203, "Robot01").InstanceId;
                        MessageHelper.SendActor(robotSceneId, new G2Robot_MessageRequest()
                        {
                            Zone = unit.DomainZone(),
                            MessageType = NoticeType.YeWaiBoss,
                            Message = $"{2000002}@{7};{0};{15}@{72000003}@{commands[1]}"
                        });
						break;
					case 10:
                        Log.Warning("刷新机器人！！");
                        robotSceneId = StartSceneConfigCategory.Instance.GetBySceneName(203, "Robot01").InstanceId;
                        MessageHelper.SendActor(robotSceneId, new G2Robot_MessageRequest() { Zone = unit.DomainZone(), MessageType = 18, Message = $"1001#{commands[1]}" });
                        break;
					case 11: //11#80001003   11#80002003   11#80002005  11#97050403
                        BuffData buffData_2 = new BuffData();
                        buffData_2.SkillId = 67000278;
                        buffData_2.BuffId = int.Parse(commands[1]); 
                        unit.GetComponent<BuffManagerComponent>().BuffFactory(buffData_2, unit, null);
                        break;
					case 12:
						for (int i = 0; i < long.Parse(commands[1]); i++)
						{
                            buffData_2 = new BuffData();
                            buffData_2.SkillId = 67000278;
                            buffData_2.BuffId = int.Parse(commands[2]);
                            unit.GetComponent<BuffManagerComponent>().BuffFactory(buffData_2, unit, null);
                        }
						break;
					case 13:
						List<Unit> players = unit.GetParent<UnitComponent>().GetAll();
						for (int player = 0; player < players.Count; player++)
						{
                            for (int i = 0; i < long.Parse(commands[1]); i++)
                            {
                                buffData_2 = new BuffData();
                                buffData_2.SkillId = 67000278;
                                buffData_2.BuffId = int.Parse(commands[2]);
                                players[player].GetComponent<BuffManagerComponent>()?.BuffFactory(buffData_2, unit, null);
                            }
                        }
						break;
					case 14:
                        unit.GetComponent<BagComponent>().OnAddJianDing();
                        break;
					default:
						break;
				}
			}
			catch (Exception ex)
			{
				Log.Debug(ex.ToString());
			}

			await ETTask.CompletedTask;
		}
	}
}
