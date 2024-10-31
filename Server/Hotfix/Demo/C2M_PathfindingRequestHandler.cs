using UnityEngine;

namespace ET
{
	[ActorMessageHandler]
	public class C2M_PathfindingRequestHandler : AMActorLocationHandler<Unit, C2M_PathfindingRequest>
	{
		protected override async ETTask Run(Unit unit, C2M_PathfindingRequest message)
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

            if (message.Distance > 0f)
            {
                Quaternion rotation = Quaternion.Euler(0, message.Direction, 0);
				Vector3 target = unit.Position + rotation * Vector3.forward * message.Distance;
                unit.GetComponent<DBSaveComponent>().NoFindPath = 0;
                skillManagerComponent.InterruptSing(0, true);
                unit.FindPathMoveToAsync(target, null, message.YaoGan).Coroutine();
            }
			else
            {
                Vector3 target = new Vector3(message.X, message.Y, message.Z);
                unit.GetComponent<DBSaveComponent>().NoFindPath = 0;
                skillManagerComponent.InterruptSing(0, true);

                unit.FindPathMoveToAsync(target, null, message.YaoGan).Coroutine();
            }

            await ETTask.CompletedTask;
        }
	}
}