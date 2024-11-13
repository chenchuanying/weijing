using System;
using System.Collections.Generic;


namespace ET
{

    [ActorMessageHandler]
    public class A2P_DeleteRoleDataHandler : AMActorRpcHandler<Scene, A2P_DeleteRoleData, P2A_DeleteRoleData>
    {

        protected override async ETTask Run(Scene scene, A2P_DeleteRoleData request, P2A_DeleteRoleData response, Action reply)
        {
            PaiMaiSceneComponent rankScene = scene.GetComponent<PaiMaiSceneComponent>();
            rankScene.OnDeleteRole(request.DeleteType, request.DeleUserID);

            reply();
            await ETTask.CompletedTask;
        }
    }
}
