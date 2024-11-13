using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace ET
{
    public static class SceneFactory
    {
        public static Scene Create(Entity parent, string name, SceneType sceneType)
        {
            long instanceId = IdGenerater.Instance.GenerateInstanceId();
            return Create(parent, instanceId, instanceId, parent.DomainZone(), name, sceneType);
        }

        public static Scene Create(Entity parent, long id, long instanceId, int zone, string name, SceneType sceneType, StartSceneConfig startSceneConfig = null)
        {
            var startZoneConfig = StartZoneConfigCategory.Instance.Get(zone);
            Game.Scene.GetComponent<DBComponent>().InitDatabase(startZoneConfig);

            Scene scene = EntitySceneFactory.CreateScene(id, instanceId, zone, sceneType, name, parent);
            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);
            switch (scene.SceneType)
            {
                case SceneType.Account:
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.InnerIPOutPort, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    scene.AddComponent<TokenComponent>();
                    scene.AddComponent<AccountSessionsComponent>();
                    scene.AddComponent<ObjectWait>();
                    scene.AddComponent<PlayerInfoListComponent>();
                    break;
                case SceneType.LoginCenter:
                    scene.AddComponent<LoginInfoRecordComponent>();
                    break;
                case SceneType.AccountCenter:
                    int tapport = ComHelp.IsInnerNet() ? ComHelp.TapHttpIneer : ComHelp.TapHttpOuter;

                    scene.AddComponent<FangChenMiComponent>();
                    scene.AddComponent<AccountCenterComponent>();
                    scene.AddComponent<HttpComponent, string>($"http://*:{tapport}/");
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.InnerIPOutPort, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    break;
                case SceneType.Realm:
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.InnerIPOutPort, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    scene.AddComponent<TokenComponent>();
                    scene.AddComponent<PlayerInfoListComponent>();
                    break;
                case SceneType.Queue:
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.InnerIPOutPort, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    scene.AddComponent<QueueSessionsComponent>();
                    break;
                case SceneType.Gate:
                    scene.AddComponent<NetKcpComponent, IPEndPoint, int>(startSceneConfig.InnerIPOutPort, SessionStreamDispatcherType.SessionStreamDispatcherServerOuter);
                    scene.AddComponent<PlayerComponent>();
                    scene.AddComponent<GateSessionKeyComponent>();
                    scene.AddComponent<PlayerInfoListComponent>();
                    break;
                case SceneType.GateMap:
                    scene.AddComponent<UnitComponent>();
                    break;
                case SceneType.Location:
                    scene.AddComponent<LocationComponent>();
                    break;
                case SceneType.DBCache:
                    scene.AddComponent<DBCacheComponent>();
                    break;
                case SceneType.Chat:
                    scene.AddComponent<ChatSceneComponent>();
                    break;
                case SceneType.EMail:
                    scene.AddComponent<MailSceneComponent>();
                    break;
                case SceneType.Activity:
                    scene.AddComponent<ActivitySceneComponent>();
                    break;
                case SceneType.Rank:
                    scene.AddComponent<RankSceneComponent>();
                    break;
                case SceneType.PaiMai:
                    scene.AddComponent<PaiMaiSceneComponent>();
                    break;
                case SceneType.Center:
                    scene.AddComponent<CenterServerComponent>();
                    break;
                case SceneType.Team:
                    scene.AddComponent<TeamSceneComponent>();
                    break;
                case SceneType.Friend:
                    scene.AddComponent<FriendSceneComponent>();
                    break;
                case SceneType.FubenCenter:
                    scene.AddComponent<FubenCenterComponent>();
                    break;
                case SceneType.Union:
                    scene.AddComponent<UnionSceneComponent>();
                    break;
                case SceneType.LocalDungeon:
                    scene.AddComponent<LocalDungeonSceneComponent>();
                    break;
                case SceneType.Solo:
                    scene.AddComponent<SoloSceneComponent>();
                    break;
                case SceneType.ReCharge:
                    scene.AddComponent<RechargeSceneComponent>();
                    break;
                case SceneType.Happy:
                    scene.AddComponent<HappySceneComponent>();
                    break;
                case SceneType.Battle:
                    scene.AddComponent<BattleSceneComponent>();
                    break;
                case SceneType.Arena:
                    scene.AddComponent<ArenaSceneComponent>();
                    break;
                case SceneType.JiaYuan:
                    scene.AddComponent<JiaYuanSceneComponent>();
                    break;
                case SceneType.Fuben:           //副本地图
                    scene.AddComponent<MapComponent>();
                    scene.AddComponent<UnitComponent>();
                    scene.AddComponent<ServerInfoComponent>();
                    scene.AddComponent<AOIManagerComponent>();
                    //scene.AddComponent<RecastPathComponent>();
                    break;
                case SceneType.Map:             //野外地图
                    scene.AddComponent<MapComponent>();
                    scene.AddComponent<UnitComponent>();
                    scene.AddComponent<ServerInfoComponent>();
                    scene.AddComponent<AOIManagerComponent>();
                    scene.AddComponent<NpcComponent>();
                    //scene.AddComponent<RecastPathComponent>();
                    break;
                case SceneType.Popularize:

                    break;
                default:
                    break;  
            }
            return scene;
        }
    }
}