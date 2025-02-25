﻿using System;
using System.Collections.Generic;

namespace ET
{

    [ActorMessageHandler]
    public class C2M_PetEggChouKaHandler : AMActorLocationRpcHandler<Unit, C2M_PetEggChouKaRequest, M2C_PetEggChouKaResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_PetEggChouKaRequest request, M2C_PetEggChouKaResponse response, Action reply)
        {
            if (unit.GetComponent<BagComponent>().GetBagLeftCell() < request.ChouKaType)
            {
                response.Error = ErrorCode.ERR_BagIsFull;
                reply();
                return;
            }
            if(request.ChouKaType!=1 && request.ChouKaType!= 10)
            {
                Log.Error($"C2M_PetEggChouKaRequest 1");
                response.Error = ErrorCode.ERR_ModifyData;
                reply();
                return;
            }

            int dropId = 0;
            int exlporeNumber = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.PetExploreNumber);
            string[] set = GlobalValueConfigCategory.Instance.Get(107).Value.Split(';');
            float discount;
            if (exlporeNumber < int.Parse(set[0])) // 超过300次打8折
            {
                discount = 1;
            }
            else
            {
                discount = float.Parse(set[1]);
            }

            if (request.ChouKaType == 1)
            {
                string needItems = GlobalValueConfigCategory.Instance.Get(39).Value.Split('@')[0];
                dropId = int.Parse(GlobalValueConfigCategory.Instance.Get(39).Value.Split('@')[1]);
                bool sucess = unit.GetComponent<BagComponent>().OnCostItemData(needItems, ItemLocType.ItemLocBag, ItemGetWay.PetEggDuiHuan);
                if (!sucess)
                {
                    response.Error = ErrorCode.ERR_ItemNotEnoughError;
                    reply();
                    return;
                }

                unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.PetExploreNumber, 1, 0);
            }
            else if (request.ChouKaType == 10)
            {
                
                UserInfo userInfo = unit.GetComponent<UserInfoComponent>().UserInfo;
                int needDimanond = int.Parse(GlobalValueConfigCategory.Instance.Get(40).Value.Split('@')[0]);
                dropId = int.Parse(GlobalValueConfigCategory.Instance.Get(40).Value.Split('@')[1]);
                if (userInfo.Diamond < (int)(needDimanond * discount))
                {
                    response.Error = ErrorCode.ERR_DiamondNotEnoughError;
                    reply();
                    return;
                }
                unit.GetComponent<UserInfoComponent>().UpdateRoleMoneySub(UserDataType.Diamond, (-1 * (int)(needDimanond * discount)).ToString(), true,ItemGetWay.PetChouKa);
                unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.PetExploreNumber, 10, 0);
            }

            int oldValue = exlporeNumber / 10;
            int newValue = (exlporeNumber + request.ChouKaType ) / 10;

            if (newValue > oldValue)
            {
                unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.PetExploreLuckly, RandomHelper.RandomNumber(5,16), 0);
            }
            int exploreLuck = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.PetExploreLuckly);
            List <RewardItem> rewardItems = new List<RewardItem>();
            for (int i = 0; i < request.ChouKaType; i++)
            {
                DropHelper.DropIDToDropItem_2(dropId, rewardItems);
            }
            unit.GetComponent<BagComponent>().OnAddItemData(rewardItems, string.Empty, $"{ItemGetWay.PetExplore}_{TimeHelper.ServerNow()}_{exploreLuck}");
            response.ReardList = rewardItems;
            reply();
            await ETTask.CompletedTask;
        }
    }
}
