﻿using System;
using System.Collections.Generic;

namespace ET
{

    [ActorMessageHandler]
    public class C2M_MakeSelectHandler : AMActorLocationRpcHandler<Unit, C2M_MakeSelectRequest, M2C_MakeSelectResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_MakeSelectRequest request, M2C_MakeSelectResponse response, Action reply)
        {
            //int makeType = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.MakeType);
            //if (makeType != 0)
            //{
            //    int cost = GlobalValueConfigCategory.Instance.Get(46).Value2;
            //    if (unit.GetComponent<UserInfoComponent>().UserInfo.Diamond < cost)
            //    {
            //        response.Error = ErrorCode.ERR_DiamondNotEnoughError;
            //        reply();
            //        return;
            //    }
            //    unit.GetComponent<UserInfoComponent>().UpdateRoleData( UserDataType.Diamond,(cost* -1).ToString() );
            //}

            unit.GetComponent<UserInfoComponent>().UserInfo.MakeList.Clear();
            unit.GetComponent<UserInfoComponent>().UserInfo.MakeList = MakeHelper.GetInitMakeList(request.MakeType);
            unit.GetComponent<NumericComponent>().ApplyValue( NumericType.MakeType, request.MakeType);
            unit.GetComponent<NumericComponent>().ApplyValue( NumericType.MakeShuLianDu, 0);
            unit.GetComponent<ChengJiuComponent>().OnSkillShuLianDu(0);
            response.MakeList = MakeHelper.GetInitMakeList(request.MakeType);
            reply();
            await ETTask.CompletedTask;
        }
    }
}
