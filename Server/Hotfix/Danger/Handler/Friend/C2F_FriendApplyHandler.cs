using System;

namespace ET
{

    [ActorMessageHandler]
    public class C2F_FriendApplyHandler : AMActorRpcHandler<Scene, C2F_FriendApplyRequest, F2C_FriendApplyResponse>
    {

        protected override async ETTask Run(Scene scene, C2F_FriendApplyRequest request, F2C_FriendApplyResponse response, Action reply)
        {
            long dbCacheId = DBHelper.GetDbCacheId( scene.DomainZone() );
            D2G_GetComponent d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = request.UserID, Component = DBHelper.DBFriendInfo });
            DBFriendInfo dBFriendInfo = d2GGetUnit.Component as DBFriendInfo;

            if (dBFriendInfo == null)
            {
                ///dBFriendInfo = (DBFriendInfo)await DBHelper.AddDataComponent<DBFriendInfo>(scene.DomainZone(), request.UserID, DBHelper.DBFriendInfo);
                //Log.Error($"C2F_FriendApplyRequest.1");
                response.Error = ErrorCode.ERR_NonePlayerError;
                reply();
                return;
            }
            if (dBFriendInfo.FriendList.Contains(request.RoleInfo.UserId))
            {
                reply();
                return;
            }
            if (!dBFriendInfo.ApplyList.Contains(request.RoleInfo.UserId))
            {
                dBFriendInfo.ApplyList.Add(request.RoleInfo.UserId);
                D2M_SaveComponent d2GSave = (D2M_SaveComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new M2D_SaveComponent() { UnitId = request.UserID, EntityByte = MongoHelper.ToBson(dBFriendInfo), ComponentType = DBHelper.DBFriendInfo });

                long gateServerId = StartSceneConfigCategory.Instance.GetBySceneName(scene.DomainZone(), "Gate1").InstanceId;
                G2T_GateUnitInfoResponse g2M_UpdateUnitResponse = (G2T_GateUnitInfoResponse)await ActorMessageSenderComponent.Instance.Call
                    (gateServerId, new T2G_GateUnitInfoRequest()
                    {
                        UserID = request.UserID
                    });

                if (g2M_UpdateUnitResponse.PlayerState == (int)PlayerState.Game && g2M_UpdateUnitResponse.SessionInstanceId > 0)
                {
                    M2C_FriendApplyResult m2C_FriendApplyResult = new M2C_FriendApplyResult() {  FriendInfo = request.RoleInfo };
                    MessageHelper.SendActor(g2M_UpdateUnitResponse.SessionInstanceId, m2C_FriendApplyResult);
                }
            }

            reply();
        }
    }
}
