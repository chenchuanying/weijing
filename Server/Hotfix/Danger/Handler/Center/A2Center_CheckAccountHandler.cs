using System.Collections.Generic;
using System;

namespace ET
{

    [ActorMessageHandler]
    public class A2Center_CheckAccountHandler : AMActorRpcHandler<Scene, A2Center_CheckAccount, Center2A_CheckAccount>
    {
        protected override async ETTask Run(Scene scene, A2Center_CheckAccount request, Center2A_CheckAccount response, Action reply)
        {
            Log.Warning(($"A2Center_CheckAccount:{request.AccountName}"));
            List<DBCenterAccountInfo> centerAccountInfoList = await Game.Scene.GetComponent<DBComponent>().Query<DBCenterAccountInfo>(scene.DomainZone(), d => d.Account == request.AccountName && d.Password == request.Password); ;

            //手机号判断3/4
            if (centerAccountInfoList.Count == 0 && (request.ThirdLogin == "3"|| request.ThirdLogin == "4"))
            {
                string Password = request.Password == "3" ? "4" : "3";
                centerAccountInfoList = await Game.Scene.GetComponent<DBComponent>().Query<DBCenterAccountInfo>(scene.DomainZone(), d => d.Account == request.AccountName && d.Password == Password);
            }
            //绑定手机号的账号
            if (centerAccountInfoList.Count == 0 && (request.ThirdLogin == "3"|| request.ThirdLogin == "4"))
            {
                centerAccountInfoList = await Game.Scene.GetComponent<DBComponent>().Query<DBCenterAccountInfo>(scene.DomainZone(),
                   _account => _account.PlayerInfo != null && _account.PlayerInfo.PhoneNumber.Equals(request.AccountName));
            }

            DBCenterAccountInfo dBCenterAccountInfo = centerAccountInfoList != null && centerAccountInfoList.Count > 0 ? centerAccountInfoList[0] : null;
            response.PlayerInfo = dBCenterAccountInfo != null ? dBCenterAccountInfo.PlayerInfo : null;
            response.AccountId = dBCenterAccountInfo != null ? dBCenterAccountInfo.Id : 0;
            
            if (response.PlayerInfo != null)
            {
                for (int i = 0; i < response.PlayerInfo.RechargeInfos.Count; i++)
                {
                    response.PlayerInfo.RechargeInfos[i].OrderInfo = string.Empty;
                }
            }

            //判断是否为taprep用户
            if (response.PlayerInfo!= null &&  !string.IsNullOrEmpty(request.OAID))
            {
                List<DBCenterTaprepRequest> centerTaprepRequests = await Game.Scene.GetComponent<DBComponent>().Query<DBCenterTaprepRequest>(scene.DomainZone(), d => d.anid == request.OAID);
                if (centerTaprepRequests != null && centerTaprepRequests.Count > 0)
                {
                    DBCenterTaprepRequest dBCenterTaprep = centerTaprepRequests[0];
                    //response.TaprepRequest = $"{dBCenterTaprep.callback}&{dBCenterTaprep.tap_project_id}&{dBCenterTaprep.tap_track_id}";
                    response.TaprepRequest = dBCenterTaprep.callback;
                }
            }
     
            response.IsHoliday = scene.GetComponent<FangChenMiComponent>().IsHoliday;
            response.StopServer = scene.GetComponent<FangChenMiComponent>().StopServer;
            response.Message = dBCenterAccountInfo!=null? dBCenterAccountInfo.AccountType.ToString():string.Empty;


            if (dBCenterAccountInfo != null && !string.IsNullOrEmpty(dBCenterAccountInfo.DeviceID) && !dBCenterAccountInfo.DeviceID.Equals(request.DeviceID))
            {
                if (request.ThirdLogin == "3" || request.ThirdLogin == "4")
                {
                    response.Error = ErrorCode.ERR_LoginInfoExpire;
                    Console.WriteLine($"无效设备id:  {dBCenterAccountInfo.Account}  {request.DeviceID}");
                }
            }
            if (dBCenterAccountInfo != null && !string.IsNullOrEmpty(request.DeviceID)  && !dBCenterAccountInfo.DeviceID.Equals(request.DeviceID))
            {
                dBCenterAccountInfo.DeviceID = request.DeviceID;    
                await Game.Scene.GetComponent<DBComponent>().Save<DBCenterAccountInfo>(202, dBCenterAccountInfo);

                Console.WriteLine($"更新设备id:  {dBCenterAccountInfo.Account}  {request.DeviceID}");
            }
           
            reply();
            await ETTask.CompletedTask;
        }
    }
}