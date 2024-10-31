using UnityEngine;

namespace ET
{

    [MessageHandler]
    public class M2C_SkillXuanZhuanMessageHandler : AMHandler<M2C_SkillXuanZhuanMessage>
    {
        protected override void Run(Session session, M2C_SkillXuanZhuanMessage message)
        {
            Unit unit = session.ZoneScene().CurrentScene().GetComponent<UnitComponent>().Get(message.UnitID);
            if (unit == null)
            {
                return;
            }
            unit.Rotation = Quaternion.Euler(0, message.Angle, 0);
        }
    }
}
