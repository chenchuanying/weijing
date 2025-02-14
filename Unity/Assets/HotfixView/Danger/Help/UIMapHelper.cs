
using System.Collections.Generic;
using UnityEngine;

namespace ET
{

    public static class UIMapHelper
    {

        public static void OnMainHeroPath(this Unit self, MapComponent mapComponent)
        {
            if (!self.MainHero)
            {
                return;
            }

            int navmesh = 0;
            int sceneType = mapComponent.SceneTypeEnum;
            if (!SceneConfigHelper.IfSceneCanMove(sceneType, mapComponent.SceneId))
            {
                return;
            }

            if (SceneConfigHelper.UseSceneConfig(sceneType))
            {
                navmesh = SceneConfigCategory.Instance.Get(mapComponent.SceneId).MapID;
            }
            else
            {
                switch (sceneType)
                {
                    case SceneTypeEnum.LocalDungeon:
                        navmesh = DungeonConfigCategory.Instance.Get(mapComponent.SceneId).MapID;
                        break;
                    default:
                        break;
                }
            }

            self.AddComponent<PathfindingComponent, int>(navmesh);
            //unit.AddComponent<ClientPathfinding2Component>();
        }

        public static void OnMainHeroMove(Unit self)
        {
            float curTime = Time.time;
            List<Unit> units = self.DomainScene().GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];
                if (curTime <= unit.UpdateUITime)
                {
                    continue;
                }
                if (unit.Type == UnitType.Npc)
                {
                    unit.UpdateUITime = curTime;
                    NpcHeadBarComponent npcHeadBarComponent = unit.GetComponent<NpcHeadBarComponent>();
                    npcHeadBarComponent?.OnUpdateNpcTalk(self);
                    continue;
                }
                if (unit.Type == UnitType.Pasture)
                {
                    unit.UpdateUITime = curTime;
                    JiaYuanPastureUIComponent npcHeadBarComponent = unit.GetComponent<JiaYuanPastureUIComponent>();
                    npcHeadBarComponent?.OnUpdateNpcTalk(self);
                    continue;
                }
                if (unit.Type == UnitType.Chuansong)
                {
                    unit.UpdateUITime = curTime;
                    unit.GetComponent<TransferUIComponent>()?.OnCheckChuanSong(self);
                    continue;
                }
            }
        }

    }

}