using System;
using System.Collections.Generic;

namespace ET
{
    [ActorMessageHandler]
    public class C2M_RefreshUnitHandler : AMActorLocationHandler<Unit, C2M_RefreshUnitRequest>
    {
        protected override async ETTask Run(Unit unit, C2M_RefreshUnitRequest request)
        {
            M2C_CreateUnits createUnits = new M2C_CreateUnits();
            Dictionary<long, AOIEntity> dict = unit.GetSeeUnits();
            foreach (AOIEntity u in dict.Values)
            {
                UnitHelper.GetUnitInfo(u.Unit, createUnits);
            }
            createUnits.UpdateAll = 1;
            MessageHelper.SendToClient(unit, createUnits);
            await ETTask.CompletedTask;
        }
    }
}
