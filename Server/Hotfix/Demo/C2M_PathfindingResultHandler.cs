using NLog.Targets;
using UnityEngine;

namespace ET
{

    [ActorMessageHandler]
    public class C2M_PathfindingResultHandler : AMActorLocationHandler<Unit, C2M_PathfindingResult>
    {
        protected override async ETTask Run(Unit unit, C2M_PathfindingResult message)
        {
            SkillManagerComponent skillManagerComponent = unit.GetComponent<SkillManagerComponent>();
            if (skillManagerComponent.HaveSkillType(SkillHelp.Skill_Other_ChongJi_1))
            {
                return;
            }

            unit.GetComponent<MoveComponent>().SyncPosition();
            unit.GetComponent<SkillPassiveComponent>().OnPlayerMove();
            unit.GetComponent<BuffManagerComponent>().BuffRemoveType(1);

            MapComponent mapComponent = unit.DomainScene().GetComponent<MapComponent>();
            if (mapComponent.SceneTypeEnum == SceneTypeEnum.Happy
             || mapComponent.SceneTypeEnum == SceneTypeEnum.PetTianTi
             || mapComponent.SceneTypeEnum == SceneTypeEnum.PetDungeon
             || mapComponent.SceneTypeEnum == SceneTypeEnum.PetMing)
            {
                return;
            }
            if (mapComponent.SceneTypeEnum == SceneTypeEnum.LocalDungeon)
            {
                if (DungeonConfigCategory.Instance.Get(mapComponent.SceneId).MapType == SceneSubTypeEnum.LocalDungeon_1)
                {
                    return;
                }
            }

            unit.GetComponent<DBSaveComponent>().NoFindPath = 0;
            skillManagerComponent.InterruptSing(0, true);

            M2C_PathfindingResult m2C_PathfindingResult = new M2C_PathfindingResult();
            m2C_PathfindingResult.Id = unit.Id;
            m2C_PathfindingResult.YaoGan = true;
            m2C_PathfindingResult.Xs = message.Xs;
            m2C_PathfindingResult.Ys = message.Ys;
            m2C_PathfindingResult.Zs = message.Zs;
            MessageHelper.Broadcast(unit, m2C_PathfindingResult);

            using (ListComponent<Vector3> list = ListComponent<Vector3>.Create())
            {
                for (int i = 0; i < message.Xs.Count; i++ )
                {
                    list.Add( new Vector3(message.Xs[i], message.Ys[i], message.Zs[i]) );
                }
                unit.FindPathResultToAsync(list).Coroutine();
            }

            await ETTask.CompletedTask;
        }
    }
}
