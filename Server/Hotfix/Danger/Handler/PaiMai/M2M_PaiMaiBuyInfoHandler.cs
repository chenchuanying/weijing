using System;


namespace ET
{

    [ActorMessageHandler]
    public class M2M_PaiMaiBuyInfoHandler: AMActorLocationRpcHandler<Unit, M2M_PaiMaiBuyInfoRequest, M2M_PaiMaiBuyInfoResponse>
    {
        protected override async ETTask Run(Unit unit, M2M_PaiMaiBuyInfoRequest request, M2M_PaiMaiBuyInfoResponse response, Action reply)
        {
            unit.GetComponent<DataCollationComponent>().UpdateBuySelfPlayerList( request.CostGold, request.BagInfoID, request.PlayerId, true );

            long paimaiGold = unit.GetComponent<NumericComponent>().GetAsLong(NumericType.PaiMaiTodayGold);
            unit.GetComponent<NumericComponent>().ApplyValue(NumericType.PaiMaiTodayGold, paimaiGold + request.CostGold, true);
            reply();

            await ETTask.CompletedTask;
        }
    }
}