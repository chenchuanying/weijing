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

            UIUnitHpComponent uIUnitHpComponent = args.Unit.GetComponent<UIUnitHpComponent>();

            uIUnitHpComponent.EnableHeadBarUI(args.OperateType == 1);
        }
    }
}