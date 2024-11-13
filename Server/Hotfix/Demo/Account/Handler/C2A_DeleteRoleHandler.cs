using System;
using System.Collections.Generic;

namespace ET
{
    [MessageHandler]
	public class C2A_DeleteRoleHandler : AMRpcHandler<C2A_DeleteRoleData, A2C_DeleteRoleData>
	{
		protected override async ETTask Run(Session session, C2A_DeleteRoleData request, A2C_DeleteRoleData response, Action reply)
		{
           
            Log.Warning($"C2A_DeleteRoleData: {request.AccountId} {request.DeleUserID}");
            try
            {
                if (session.GetComponent<SessionLockingComponent>() != null)
                {
                    response.Error = ErrorCode.ERR_RequestRepeatedly;
                    reply();
                    session.Disconnect().Coroutine();
                    return;
                }

                using (session.AddComponent<SessionLockingComponent>())
                {
                    //存储账号信息
                    int zone = session.DomainZone();
                    List<DBAccountInfo> newAccountList = await Game.Scene.GetComponent<DBComponent>().Query<DBAccountInfo>(session.DomainZone(), d => d.Id == request.AccountId);
                    if (newAccountList.Count == 0)
                    {
                        response.Error = ErrorCode.ERR_NotFindAccount;
                        reply();
                        return;
                    }
                    
                    DBAccountInfo newAccount = newAccountList[0];
                    //移除角色
                    if (newAccount.UserList.Count - 1 >= request.DeleXuhaoID)
                    {
                        newAccount.UserList.Remove(request.DeleUserID);
                        newAccount.DeleteUserList.Add(request.DeleUserID);
                    }
                    await Game.Scene.GetComponent<DBComponent>().Save<DBAccountInfo>(session.DomainZone(), newAccount);
                    long mapInstanceId = DBHelper.GetRankServerId(session.DomainZone());
                    R2A_DeleteRoleData deleteResponse = (R2A_DeleteRoleData)await ActorMessageSenderComponent.Instance.Call
                    (mapInstanceId, new A2R_DeleteRoleData()
                    {
                        DeleUserID = request.DeleUserID,
                        AccountId = request.AccountId
                    });
                    long paimaiInstanceid = DBHelper.GetPaiMaiServerId(session.DomainZone());
                    P2A_DeleteRoleData deleteResponse2 = (P2A_DeleteRoleData)await ActorMessageSenderComponent.Instance.Call
                   (paimaiInstanceid, new A2P_DeleteRoleData()
                   {
                       DeleUserID = request.DeleUserID,
                       AccountId = request.AccountId,
                       DeleteType = 0,
                   });

                    DBHelper.DeleteUnitCache(session.DomainZone(), request.DeleUserID).Coroutine();
                    UserInfoComponent userInfoComponent = await DBHelper.GetComponentCache<UserInfoComponent>(zone, request.DeleUserID);
                    NumericComponent numericComponent = await DBHelper.GetComponentCache<NumericComponent>(zone, request.DeleUserID);
                    if (userInfoComponent != null && userInfoComponent.UserInfo.Lv <= 10 &&
                        (numericComponent.GetAsInt(NumericType.RechargeNumber) <= 0 || newAccount.Account.Contains("7344243772311526")))
                    {
                        List<string> allComponets = DBHelper.GetAllUnitComponent();
                        for (int i = 0; i < allComponets.Count; i++)
                        {
                            Game.Scene.GetComponent<DBComponent>().Remove<Entity>(zone, request.DeleUserID, allComponets[i]).Coroutine();
                        }
                    }
                    reply();
                }
            }
            catch(Exception ex) 
            {
                Log.Error(ex.ToString());
            }
		}
	}
}