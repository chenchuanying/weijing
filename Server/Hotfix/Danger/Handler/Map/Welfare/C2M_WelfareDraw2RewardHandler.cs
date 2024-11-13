using System;
using System.Collections.Generic;

namespace ET
{
    [ActorMessageHandler]
    public class C2M_WelfareDraw2RewardHandler : AMActorLocationRpcHandler<Unit, C2M_WelfareDraw2RewardRequest, M2C_WelfareDraw2RewardResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_WelfareDraw2RewardRequest request, M2C_WelfareDraw2RewardResponse response, Action reply)
        {
            int index = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.DrawIndex2);
            if (index == 0 || index > ActivityHelper.WelfareChouKaList.Count)
            {
                reply();
                return;
            }
            int occ = unit.GetComponent<UserInfoComponent>().UserInfo.Occ;
            int occTwo = unit.GetComponent<UserInfoComponent>().UserInfo.OccTwo;
            List<string> rewardItems = ActivityHelper.GetWelfareChouKaReward(unit.GetComponent<BagComponent>().GetAllItems(occ, occTwo));
            string reward = rewardItems[index - 1];

            unit.GetComponent<BagComponent>().OnAddItemData(reward, $"{ItemGetWay.Welfare}_{TimeHelper.ServerNow()}");
            unit.GetComponent<NumericComponent>().ApplyValue(NumericType.DrawIndex2, 0);

            reply();
            await ETTask.CompletedTask;
        }
    }
}