using System;
using System.Collections.Generic;

namespace ET
{

    [ActorMessageHandler]
    public class C2Popularize_ListHandler : AMActorRpcHandler<Scene, C2Popularize_ListRequest, Popularize2C_ListResponse>
    {
        protected override async ETTask Run(Scene scene, C2Popularize_ListRequest request, Popularize2C_ListResponse response, Action reply)
        {
            Log.Warning($"C2Popularize_ListRequest:{request.ActorId}");
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Popularize, scene.DomainZone()))
            {
                
                DBPopularizeInfo dBPopularizeInfo = await DBHelper.GetComponentCache<DBPopularizeInfo>(scene.DomainZone(), request.ActorId);
                if (dBPopularizeInfo == null)
                {
                    if (scene.GetChild<DBPopularizeInfo>(request.ActorId) != null)
                    {
                        reply();
                        return;
                    }

                    dBPopularizeInfo = scene.AddChildWithId<DBPopularizeInfo>(request.ActorId);

                    int newzone = scene.DomainZone();
                    List<DBPopularizeInfo> dBPopularizeInfoList = await Game.Scene.GetComponent<DBComponent>().Query<DBPopularizeInfo>(newzone, d => d.Id > 0);
                    int xuindex = dBPopularizeInfoList.Count + 1;

                    //推广码生成规则
                    dBPopularizeInfo.PopularizeCode = newzone * 1000000 + xuindex;

                    await DBHelper.SaveComponentCache(scene.DomainZone(), request.ActorId, dBPopularizeInfo);
                    dBPopularizeInfo.Dispose();
                }
             
                for (int i = 0; i < dBPopularizeInfo.MyPopularizeList.Count; i++)
                {
                    long unitid = dBPopularizeInfo.MyPopularizeList[i].UnitId;
                    int oldZone = UnitIdStruct.GetUnitZone(unitid);
                    int newZone = ServerHelper.GetNewServerId(oldZone);
                    if (newZone < 5)
                    {
                        continue;
                    }

                    UserInfoComponent userInfoComponent = await DBHelper.GetComponentCache<UserInfoComponent>(newZone, unitid);
                    if (userInfoComponent == null)
                    {
                        continue;
                    }

                    dBPopularizeInfo.MyPopularizeList[i].Nmae = userInfoComponent.UserInfo.Name;
                    dBPopularizeInfo.MyPopularizeList[i].Level = userInfoComponent.UserInfo.Lv;
                    dBPopularizeInfo.MyPopularizeList[i].Occ = userInfoComponent.UserInfo.Occ;
                    dBPopularizeInfo.MyPopularizeList[i].OccTwo = userInfoComponent.UserInfo.OccTwo;
                }

                response.PopularizeCode = dBPopularizeInfo.PopularizeCode;
                response.BePopularizeId = dBPopularizeInfo.BePopularizeId;
                response.MyPopularizeList = dBPopularizeInfo.MyPopularizeList;

            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}
