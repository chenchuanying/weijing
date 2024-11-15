namespace ET
{
    public class Skill_OnBuffBounce : AEventClass<EventType.BuffBounce>
    {
        protected override void Run(object numerice)
        {
            EventType.BuffBounce args = numerice as EventType.BuffBounce;
            if (args.Unit == null || args.Unit.IsDisposed)
            {
                return;
            }

            CameraComponent cameraComponent = args.ZoneScene.CurrentScene().GetComponent<CameraComponent>();
            cameraComponent.CameraMoveType = args.OperateType == 1 ? CameraMoveType.BuffBounce : CameraMoveType.Normal;

            UIUnitHpComponent uIUnitHpComponent = args.Unit.GetComponent<UIUnitHpComponent>();

            uIUnitHpComponent.EnableHeadBarUI(args.OperateType == 1);
        }
    }
}