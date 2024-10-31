using UnityEngine;

namespace ET
{

    [Event]
    public class UnitRevive_PlayIdleAnimate : AEventClass<EventType.UnitRevive>
    {
        protected override void Run(object cls)
        {
            EventType.UnitRevive args = cls as EventType.UnitRevive;
            Unit unit = args.Unit;  
            unit.GetComponent<FsmComponent>()?.ChangeState(FsmStateEnum.FsmIdleState);
            unit.GetComponent<UIUnitHpComponent>()?.OnRevive();
            unit.GetComponent<GameObjectComponent>()?.OnRevive();
        }
    }
}
