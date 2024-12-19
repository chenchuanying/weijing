
namespace ET
{

    [ActorMessageHandler]
    public class G2M_ActivityUpdateHandler : AMActorLocationHandler<Unit, G2M_ActivityUpdate>
    {

        protected override async ETTask Run(Unit unit, G2M_ActivityUpdate message)
        {
            switch (message.ActivityType)
            {
                case 0:
                    Log.Debug($"OnZeroClockUpdate [零点刷新]: {unit.Id}");
                    UserInfo userInfo = unit.GetComponent<UserInfoComponent>().UserInfo;
                    unit.GetComponent<HeroDataComponent>().OnZeroClockUpdate(true);
                    unit.GetComponent<EnergyComponent>().OnZeroClockUpdate();
                    unit.GetComponent<UserInfoComponent>().OnHourUpdate(0, true);
                    unit.GetComponent<UserInfoComponent>().OnZeroClockUpdate(true);
                    unit.GetComponent<TaskComponent>().CheckWeeklyUpdate();
                    unit.GetComponent<TaskComponent>().OnZeroClockUpdate(true);
                    unit.GetComponent<ActivityComponent>().OnZeroClockUpdate(userInfo.Lv);
                    unit.GetComponent<ChengJiuComponent>().OnZeroClockUpdate();
                    unit.GetComponent<JiaYuanComponent>().OnZeroClockUpdate(true);
                    unit.GetComponent<DataCollationComponent>().OnZeroClockUpdate(true);
                    break;
                case -1:
                    Log.Debug($"OnZeroClockUpdate防止卡死 [-1]");
                    //LocationProxyComponent.Instance.Remove(unit.Id).Coroutine();
                    break;
                default:
                    //if (message.ActivityType == 18  && unit.DomainZone() == 81)
                    //{
                    //    UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
                    //    DataCollationComponent dataCollationComponent = unit.GetComponent<DataCollationComponent>();
                    //    ChengJiuComponent chengJiuComponent = unit.GetComponent<ChengJiuComponent>();

                    //    int chengjiuNumber = 0;
                    //    if (chengJiuComponent.ChengJiuCompleteList.Contains(10000002))
                    //    {
                    //        chengjiuNumber++;
                    //    }
                    //    if (chengJiuComponent.ChengJiuCompleteList.Contains(10000003))
                    //    {
                    //        chengjiuNumber++;
                    //    }
                    //    if (chengJiuComponent.ChengJiuCompleteList.Contains(10000004))
                    //    {
                    //        chengjiuNumber++;
                    //    }
                    //    if (chengJiuComponent.ChengJiuCompleteList.Contains(10000005))
                    //    {
                    //        chengjiuNumber++;
                    //    }

                    //    string gongzuoshiInfo = $"账号: {userInfoComponent.Account}  \t名称：{userInfoComponent.UserInfo.Name}  \t等级:{userInfoComponent.UserInfo.Lv}   \t充值:{dataCollationComponent.Recharge}" +
                    //      $"\t体力:{userInfoComponent.UserInfo.PiLao}  \t金币:{userInfoComponent.UserInfo.Gold}   \t成就值:{chengJiuComponent.TotalChengJiuPoint}   \t成就任务:{chengjiuNumber}" +
                    //      $"\t拍卖消耗:{dataCollationComponent.GetCostByType(ItemGetWay.PaiMaiBuy)}" +
                    //      $"\t当前主线:{dataCollationComponent.MainTask}  \t角色天数:{userInfoComponent.GetCrateDay()} \n";

                    //    LogHelper.OnLineInfo(gongzuoshiInfo);    
                    //}

                    unit.GetComponent<UserInfoComponent>().OnHourUpdate(message.ActivityType, true);
                    break;
            }
   
            await ETTask.CompletedTask;
        }
    }
}
