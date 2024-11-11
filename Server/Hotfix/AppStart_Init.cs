using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace ET
{
    public class AppStart_Init: AEvent<EventType.AppStart>
    {
        protected override void Run(EventType.AppStart args)
        {
            RunAsync(args).Coroutine();
        }
        
        private async ETTask RunAsync(EventType.AppStart args)
        {
            Game.Scene.AddComponent<ConfigComponent>();
            Game.Scene.AddComponent<TimerComponent>();

            TimeInfo.Instance.TimeZone = 8;
            await ConfigComponent.Instance.LoadAsync();

            StartProcessConfig processConfig = StartProcessConfigCategory.Instance.Get(Game.Options.Process);
            

            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            Game.Scene.AddComponent<SessionStreamDispatcher>();
            Game.Scene.AddComponent<CoroutineLockComponent>();
            // 发送普通actor消息
            Game.Scene.AddComponent<ActorMessageSenderComponent>();
            // 发送location actor消息
            Game.Scene.AddComponent<ActorLocationSenderComponent>();
            // 访问location server的组件
            Game.Scene.AddComponent<LocationProxyComponent>();
            Game.Scene.AddComponent<ActorMessageDispatcherComponent>();
            // 数值订阅组件
            Game.Scene.AddComponent<NumericWatcherComponent>();
            
            Game.Scene.AddComponent<NetThreadComponent>();
            
            Game.Scene.AddComponent<NavmeshComponent, Func<string, byte[]>>(RecastFileReader.Read);

            Game.Scene.AddComponent<RecastPathComponent>();

            //添加db数据库的链接
            //"mongodb://127.0.0.1:27017/", "ET"
            Game.Scene.AddComponent<DBComponent>();
            Game.Scene.AddComponent<AIDispatcherComponent>();
            Game.Scene.AddComponent<SkillDispatcherComponent>();
            Game.Scene.AddComponent<BuffDispatcherComponent>();
            Game.Scene.AddComponent<ShouJiChapterInfoComponent>();

            long unitid = DBHelper.DebugUnitId;
            // int n = (int)((unitid / 99) % 4);
            Log.Console($"unit.zone1112: {UnitIdStruct.GetUnitZone(unitid)}");
            Console.WriteLine($"unit.zone1112: {UnitIdStruct.GetUnitZone(unitid)}");

            //string filePath = "C:\\WJ\\WJBeta115\\BagComponent.bson"; // 替换为你的.bson文件路径
            //var list = new List<BagComponent>();
            //using (var stream = File.OpenRead(filePath))
            //{
            //    while (stream.Position != stream.Length)
            //    {
            //        var document = BsonSerializer.Deserialize<BsonDocument>(stream);
            //        var obj = BsonSerializer.Deserialize<BagComponent>(document);
            //        list.Add((BagComponent)obj);
            //    }
            //}

            switch (Game.Options.AppType)
            {
                case AppType.Server:
                {
                    if (!string.IsNullOrEmpty(Game.Options.Parameters))
                    {
                        DyncCSHelper.Test_2(Game.Options.Parameters);
                    }
                    Game.Scene.AddComponent<NetInnerComponent, IPEndPoint, int>(processConfig.InnerIPPort, SessionStreamDispatcherType.SessionStreamDispatcherServerInner);

                    var processScenes = StartSceneConfigCategory.Instance.GetByProcess(Game.Options.Process);
                    foreach (StartSceneConfig startConfig in processScenes)
                    {
                         SceneFactory.Create(Game.Scene, startConfig.Id, startConfig.InstanceId, startConfig.Zone, startConfig.Name,
                            startConfig.Type, startConfig);
                    }
                    break;
                }
                case AppType.MergeZone:
                    //Parameters=31_30   31区合并到30区
                    string[] zones =  Game.Options.Parameters.Split('_');
                    int oldzone = int.Parse(zones[0]);
                    int newzone = int.Parse(zones[1]);
                    //await  MergeZoneHelper.MergeZone(oldzone, newzone);
                    Game.EventSystem.Publish(new EventType.MergeZone(){  oldzone = oldzone, newzone = newzone });
                    break;
                case AppType.UpdateDB:
                    List<int> mergezones = ServerMessageHelper.GetAllZone();
                    for (int i = 0; i < mergezones.Count; i++)
                    {
                        var startZoneConfig = StartZoneConfigCategory.Instance.Get(mergezones[i]);
                        Game.Scene.GetComponent<DBComponent>().InitDatabase(startZoneConfig);
                    }

                    for (int zone = 0; zone < mergezones.Count; zone++)
                    {
                       
                        try
                        {
                            int[] number_list = new int[4];
                            int pyzone = StartZoneConfigCategory.Instance.Get(mergezones[zone]).PhysicZone;

                            List<SkillSetComponent> skillsetComponentList = await Game.Scene.GetComponent<DBComponent>().Query<SkillSetComponent>(pyzone, d => d.Id > 0);

                            Console.WriteLine($"UpdateDB1  :{pyzone}  {skillsetComponentList.Count}");


                            for (int userinfo = 0; userinfo < skillsetComponentList.Count; userinfo++)
                            {
                                SkillSetComponent skillSetComponent = skillsetComponentList[userinfo];

                                List<int> equiptianfuids = new List<int>();
                                List<BagComponent> bagComponentList = await Game.Scene.GetComponent<DBComponent>().Query<BagComponent>(pyzone, d => d.Id == skillSetComponent.Id);
                                if (bagComponentList.Count > 0)
                                {
                                    equiptianfuids.AddRange(bagComponentList[0].GetEquipTianFuIds());
                                    equiptianfuids.AddRange(skillSetComponent.TianFuAddition);
                                }

                                //0没有天赋技能  1技能找不到天赋id 2自身丢失天赋 3成功找到天赋
                                int errorcode = skillSetComponent.CheckSkillToTalent(equiptianfuids);
                                number_list[errorcode]++;
                                if (errorcode == 2 ||  errorcode == 3)
                                {
                                    await Game.Scene.GetComponent<DBComponent>().Save(pyzone, skillSetComponent);
                                }
                            }

                            Console.WriteLine($"UpdateDB2  :{mergezones[zone]}  {number_list[0]}  {number_list[1]}  {number_list[2]}   {number_list[3]}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());       
                        }
                    }

                    Console.WriteLine($"UpdateDB  :End");
                    break;
                case AppType.DeleteZone:
                    int delezone = int.Parse(Game.Options.Parameters);
                    await DeleteZoneHelper.DeletionZone(delezone);
                    Log.Warning("DeleteZone完成！");
                    break;
                case AppType.Watcher:
                {
                    StartMachineConfig startMachineConfig = WatcherHelper.GetThisMachineConfig();
                    WatcherComponent watcherComponent = Game.Scene.AddComponent<WatcherComponent>();
                    watcherComponent.Start(Game.Options.CreateScenes);
                    Game.Scene.AddComponent<NetInnerComponent, IPEndPoint, int>(NetworkHelper.ToIPEndPoint($"{startMachineConfig.InnerIP}:{startMachineConfig.WatcherPort}"), SessionStreamDispatcherType.SessionStreamDispatcherServerInner);
                    break;
                }
                case AppType.GameTool:
                    break;
            }

            if (Game.Options.Console == 1)
            {
                Game.Scene.AddComponent<ConsoleComponent>();
            }
        }
    }
}