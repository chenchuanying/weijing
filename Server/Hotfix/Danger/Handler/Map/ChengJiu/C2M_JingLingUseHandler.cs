﻿using System;

namespace ET
{

    [ActorMessageHandler]
    public class C2M_JingLingUseHandler : AMActorLocationRpcHandler<Unit, C2M_JingLingUseRequest, M2C_JingLingUseResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_JingLingUseRequest request, M2C_JingLingUseResponse response, Action reply)
        {
            ChengJiuComponent chengJiuComponent = unit.GetComponent<ChengJiuComponent>();
            if (unit.GetParent<UnitComponent>().Get(chengJiuComponent.JingLingUnitId) != null)
            {
                unit.GetParent<UnitComponent>().Remove(chengJiuComponent.JingLingUnitId);
            }
            if (chengJiuComponent.JingLingId != 0)
            {
                JingLingConfig jingLingConfig = JingLingConfigCategory.Instance.Get(chengJiuComponent.JingLingId);
                if (jingLingConfig.FunctionType == JingLingFunctionType.AddSkill)
                {
                    int skillid = int.Parse(jingLingConfig.FunctionValue);
                    BuffManagerComponent buffManagerComponent = unit.GetComponent<BuffManagerComponent>();
                    buffManagerComponent.BuffRemoveBySkillid(skillid);
                }
            }

            if (chengJiuComponent.JingLingId == request.JingLingId)
            {
                chengJiuComponent.JingLingId = 0;
                chengJiuComponent.JingLingUnitId = 0;
            }
            else
            {
                chengJiuComponent.JingLingId = (request.JingLingId);
                chengJiuComponent.JingLingUnitId = UnitFactory.CreateJingLing(unit, chengJiuComponent.JingLingId).Id;
            }
            response.JingLingId = chengJiuComponent.JingLingId;

            reply();
            await ETTask.CompletedTask;
        }
    }
}
