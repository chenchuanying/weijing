using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{

    [ActorMessageHandler]
    public class C2M_SkillXuanZhuanHandler : AMActorLocationRpcHandler<Unit, C2M_SkillXuanZhuanRequest, M2C_SkillXuanZhuanResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_SkillXuanZhuanRequest request, M2C_SkillXuanZhuanResponse response, Action reply)
        {
            unit.Rotation = Quaternion.Euler(0, request.Angle, 0);
            M2C_SkillXuanZhuanMessage m2C_SkillXuanZhuan = new M2C_SkillXuanZhuanMessage() { UnitID = unit.Id, Angle = request.Angle };
            MessageHelper.Broadcast(unit, m2C_SkillXuanZhuan);
            response.Error = ErrorCode.ERR_Success;
            reply();
            await ETTask.CompletedTask;
        }
    }
}
