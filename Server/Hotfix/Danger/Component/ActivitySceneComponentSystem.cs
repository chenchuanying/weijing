using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{

    [Timer(TimerType.ActivitySceneTimer)]
    public class ActivitySceneTimer : ATimer<ActivitySceneComponent>
    {
        public override void Run(ActivitySceneComponent self)
        {
            try
            {
                self.OnCheck();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    [Timer(TimerType.ActivityTipTimer)]
    public class ActivityTipTimer : ATimer<ActivitySceneComponent>
    {
        public override void Run(ActivitySceneComponent self)
        {
            try
            {
                self.OnCheckFuntionButton().Coroutine();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    /// <summary>
    /// 定时刷新的暂时都作为活动来处理
    /// </summary>
    [ObjectSystem]
    public class ActivitySceneComponentAwakeSystem : AwakeSystem<ActivitySceneComponent>
    {
        public override void Awake(ActivitySceneComponent self)
        {
            self.MapIdList.Clear();
            self.MapIdList.Add(DBHelper.GetGateServerId(self.DomainZone()));
            self.MapIdList.Add(DBHelper.GetPaiMaiServerId(self.DomainZone()));
            self.MapIdList.Add(DBHelper.GetRankServerId(self.DomainZone()));
            self.MapIdList.Add(DBHelper.GetFubenCenterId(self.DomainZone()));
            self.MapIdList.Add(DBHelper.GetArenaServerId(self.DomainZone()));
            self.MapIdList.Add(DBHelper.GetBattleServerId(self.DomainZone()));
            self.MapIdList.Add(DBHelper.GetUnionServerId(self.DomainZone()));
            self.MapIdList.Add(DBHelper.GetSoloServerId(self.DomainZone()));
            self.MapIdList.Add(DBHelper.GetDbCacheId(self.DomainZone()));
            self.InitDayActivity().Coroutine();
            self.InitFunctionButton();
        }
    }

    [ObjectSystem]
    public class ActivitySceneComponentDestroySystem : DestroySystem<ActivitySceneComponent>
    {

        public override void Destroy(ActivitySceneComponent self)
        {
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }

    public static class ActivitySceneComponentSystem
    {
        public static void OnCheck(this ActivitySceneComponent self)
        {
            DateTime dateTime = TimeHelper.DateTimeNow();

            if (self.DBDayActivityInfo.LastHour != dateTime.Hour)
            {
                self.DBDayActivityInfo.LastHour = dateTime.Hour;
                self.NoticeActivityUpdate_Hour(dateTime).Coroutine();
            }

            self.CheckIndex += 1000;
            if (self.CheckIndex >= TimeHelper.Minute * 5)
            {
                self.CheckPetMine();
                self.SaveDB();
                self.CheckIndex = 0;
            }

            //self.TeamUpdateHandler().Coroutine();
        }

        public static void InitPetMineExtend(this ActivitySceneComponent self)
        {
            List<MineBattleConfig> mineBattleConfig = MineBattleConfigCategory.Instance.GetAll().Values.ToList();
            for (int i = 0; i < mineBattleConfig.Count; i++)
            {
                int totalNumber = ConfigHelper.PetMiningList[mineBattleConfig[i].Id].Count;

                int hexinNumber = 1;

                if (mineBattleConfig[i].Id == 10001)
                {
                    hexinNumber = 1;
                }
                if (mineBattleConfig[i].Id == 10002)
                {
                    hexinNumber = 2;
                }
                if (mineBattleConfig[i].Id == 10003)
                {
                    hexinNumber = 5;
                }

                int[] index = RandomHelper.GetRandoms(hexinNumber, 0, totalNumber);
                List<int> hexinlist = new List<int>(index);

                for (int hexin = 0; hexin < hexinlist.Count; hexin++)
                {
                    self.DBDayActivityInfo.PetMingHexinList.Add(new KeyValuePairInt()
                    {
                        KeyId = mineBattleConfig[i].Id,
                        Value = hexinlist[hexin]
                    });
                }
            }
        }

        public static void CheckPetMine(this ActivitySceneComponent self)
        {
            
            {
                int openDay = ServerHelper.GetOpenServerDay(false, self.DomainZone());

                List<PetMingPlayerInfo> petMingPlayers = self.DBDayActivityInfo.PetMingList;

                Dictionary<long, long> playerLimitList = new Dictionary<long, long>();
                for (int i = 0; i < petMingPlayers.Count; i++)
                {
                    MineBattleConfig mineBattleConfig = MineBattleConfigCategory.Instance.Get(petMingPlayers[i].MineType);
                    int chanchu = mineBattleConfig.ChanChuLimit;

                    if (!playerLimitList.ContainsKey(petMingPlayers[i].UnitId))
                    {
                        playerLimitList.Add(petMingPlayers[i].UnitId, 0);
                    }
                    playerLimitList[petMingPlayers[i].UnitId] += chanchu;
                }

                for (int i = 0; i < petMingPlayers.Count; i++)
                {
                    long playerLimit = playerLimitList[petMingPlayers[i].UnitId];

                    float coffi = ComHelp.GetMineCoefficient(openDay, petMingPlayers[i].MineType, petMingPlayers[i].Postion, self.DBDayActivityInfo.PetMingHexinList);

                    MineBattleConfig mineBattleConfig = MineBattleConfigCategory.Instance.Get(petMingPlayers[i].MineType);
                    int chanchu = (int)(mineBattleConfig.GoldOutPut * coffi * (self.CheckIndex * 1f/ TimeHelper.Hour));

                    if (!self.DBDayActivityInfo.PetMingChanChu.ContainsKey(petMingPlayers[i].UnitId))
                    {
                        self.DBDayActivityInfo.PetMingChanChu.Add(petMingPlayers[i].UnitId, chanchu);
                    }
                    else
                    {
                        long oldValue = self.DBDayActivityInfo.PetMingChanChu[petMingPlayers[i].UnitId];
                        oldValue += chanchu;
                        oldValue = Math.Min(oldValue, playerLimit);

                        self.DBDayActivityInfo.PetMingChanChu[petMingPlayers[i].UnitId] = oldValue;
                    }
                }
            }
        }

        public static async ETTask TeamUpdateHandler(this ActivitySceneComponent self)
        {
            DateTime dateTime = TimeHelper.DateTimeNow();

            //if (dateTime.Year == 2024 && dateTime.Month == 1 && dateTime.Day == 5 && dateTime.Hour == 15 && dateTime.Minute == 19)
            //{

            //    A2A_ActivityUpdateResponse m2m_TrasferUnitResponse = (A2A_ActivityUpdateResponse)await ActorMessageSenderComponent.Instance.Call
            //             (DBHelper.GetRankServerId(self.DomainZone()), new A2A_ActivityUpdateRequest() { Hour = 16, OpenDay = 1 });
            //}
            await ETTask.CompletedTask;
        }

        public static async ETTask InitDayActivity(this ActivitySceneComponent self)
        {
            int zone = self.DomainZone();
            long dbCacheId = DBHelper.GetDbCacheId(zone);
            await TimerComponent.Instance.WaitAsync(RandomHelper.RandomNumber(1000, 2000));
            D2G_GetComponent d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = self.DomainZone(), Component = DBHelper.DBDayActivityInfo });
            if (d2GGetUnit.Component == null)
            {
                self.DBDayActivityInfo = new DBDayActivityInfo();
                self.DBDayActivityInfo.Id = self.DomainZone();
            }
            else
            {
                self.DBDayActivityInfo = d2GGetUnit.Component as DBDayActivityInfo;
                self.DBDayActivityInfo.Id = self.DomainZone();
            }
            int openServerDay = DBHelper.GetOpenServerDay(zone);
            LogHelper.LogDebug($"InitDayActivity: {zone}  {openServerDay}");
            self.DBDayActivityInfo.MysteryItemInfos = MysteryShopHelper.InitMysteryItemInfos(openServerDay);

            if (self.DBDayActivityInfo.PetMingHexinList.Count == 0)
            {
                self.InitPetMineExtend();
            }
            self.SaveDB();

            //每日活动
            self.Timer = TimerComponent.Instance.NewRepeatedTimer(TimeHelper.Second, TimerType.ActivitySceneTimer, self);
        }

        public static async ETTask OnCheckFuntionButton(this ActivitySceneComponent self)
        {
            long serverTime = TimeHelper.ServerNow();
            DateTime dateTime = TimeInfo.Instance.ToDateTime(serverTime);

            if (self.ActivityTimerList.Count > 0)
            {
                int functionId = self.ActivityTimerList[0].FunctionId;
                bool todayopen = FunctionHelp.IsFunctionDayOpen((int)dateTime.DayOfWeek, functionId);
                Log.Warning($"OnCheckFuntionButton: {functionId} {self.ActivityTimerList[0].FunctionType}");

                long sceneserverid = 0;
                switch (functionId)
                {
                    case 1025://战场
                        sceneserverid = DBHelper.GetBattleServerId(self.DomainZone());
                        break;
                    case 1043: //家族Boss
                        sceneserverid = DBHelper.GetUnionServerId(self.DomainZone());
                        break;
                    case 1044:  //家族争霸
                        sceneserverid = DBHelper.GetUnionServerId(self.DomainZone());
                        break;
                    case 1045:
                        sceneserverid = DBHelper.GetSoloServerId(self.DomainZone());
                        break;
                    case 1052://狩猎活动
                        sceneserverid = DBHelper.GetRankServerId(self.DomainZone());
                        break;
                    case 1055:
                        //喜从天降
                        sceneserverid = DBHelper.GetHappyServerId(self.DomainZone());
                        break;
                    case 1057: //小龟大赛
                        sceneserverid = DBHelper.MapCityServerId(self.DomainZone());
                        break;
                    case 1058://奔跑比赛
                    case 1059://恶魔活动
                        sceneserverid = DBHelper.GetFubenCenterId(self.DomainZone());
                        break;
                    case 2000:
                        sceneserverid = DBHelper.GetGateServerId(self.DomainZone());
                        break;
                    default:
                        break;
                }

                if (todayopen)
                {
                    A2A_ActivityUpdateResponse m2m_TrasferUnitResponse = (A2A_ActivityUpdateResponse)await ActorMessageSenderComponent.Instance.Call
                                    (sceneserverid, new A2A_ActivityUpdateRequest() { Hour = -1, FunctionId = functionId, FunctionType = self.ActivityTimerList[0].FunctionType });
                }
                if (todayopen && functionId == 1044 && self.ActivityTimerList[0].FunctionType == 2)
                {
                    //1044
                    long rankserverid = DBHelper.GetRankServerId(self.DomainZone());
                    ////家族战结束. 发送奖励
                    A2A_ActivityUpdateResponse m2m_TrasferUnitResponse = (A2A_ActivityUpdateResponse)await ActorMessageSenderComponent.Instance.Call
                                 (rankserverid, new A2A_ActivityUpdateRequest() { Hour = -1, FunctionId = functionId, FunctionType = 2 });
                }

                self.ActivityTimerList.RemoveAt(0);
            }

            TimerComponent.Instance.Remove(ref self.ActivityTimer);
            if (self.ActivityTimerList.Count > 0)
            {
                self.ActivityTimer = TimerComponent.Instance.NewOnceTimer(self.ActivityTimerList[0].BeginTime, TimerType.ActivityTipTimer, self);
            }
        }

        public static void InitFunctionButton(this ActivitySceneComponent self)
        {
            self.ActivityTimerList.Clear();
            Log.Warning("InitFunctionButton");
            long serverTime = TimeHelper.ServerNow();
            DateTime dateTime = TimeInfo.Instance.ToDateTime(serverTime);
            long curTime = (dateTime.Hour * 60 + dateTime.Minute) * 60 + dateTime.Second;
            TimerComponent.Instance.Remove(ref self.ActivityTimer);
            ///1025 战场 1043家族boss 1044家族争霸  1045竞技 1052狩猎活动  1055喜从天降  1057小龟大赛  1058奔跑比赛 1059恶魔活动
            List<int> functonIds = ConfigData.FunctionOpenIds;
            for (int i = 0; i < functonIds.Count; i++)
            {
                long startTime = FunctionHelp.GetOpenTime(functonIds[i]);
                long endTime = FunctionHelp.GetCloseTime(functonIds[i]);
                bool functionopne = FunctionHelp.IsFunctionDayOpen((int)dateTime.DayOfWeek, functonIds[i]);

                if (functonIds[i] == 2000)
                {
                    if (ComHelp.IsInnerNet())
                    {
                        continue;
                    }
                    startTime = curTime + RandomHelper.NextLong(TimeHelper.Second * 5, TimeHelper.Hour * 10);
                }

                if (curTime < startTime)
                {
                    long sTime = serverTime + (startTime - curTime) * 1000;
                    self.ActivityTimerList.Add(new ActivityTimer() { FunctionId = functonIds[i], BeginTime = sTime, FunctionType = 1 });
                }
                if (curTime < endTime)
                {
                    long sTime = serverTime + (endTime - curTime) * 1000;
                    self.ActivityTimerList.Add(new ActivityTimer() { FunctionId = functonIds[i], BeginTime = sTime, FunctionType = 2 });
                }
                bool inTime = functionopne && curTime >= startTime && curTime <= endTime;
                if (inTime)
                {
                    self.ActivityTimerList.Add(new ActivityTimer() { FunctionId = functonIds[i], BeginTime = serverTime, FunctionType = 1 });
                }
            }

            if (self.ActivityTimerList.Count > 0)
            {
                self.ActivityTimerList.Sort(delegate (ActivityTimer a, ActivityTimer b)
                {
                    return (int)(a.BeginTime - b.BeginTime);
                });

                self.ActivityTimer = TimerComponent.Instance.NewOnceTimer(self.ActivityTimerList[0].BeginTime, TimerType.ActivityTipTimer, self);
            }
        }

        public static void SaveDB(this ActivitySceneComponent self)
        {
            DBHelper.SaveComponentCache(self.DomainZone(), self.DomainZone(), self.DBDayActivityInfo).Coroutine();
        }

        public static int OnMysteryBuyRequest(this ActivitySceneComponent self, MysteryItemInfo mysteryInfo)
        {
            for (int i = 0; i < self.DBDayActivityInfo.MysteryItemInfos.Count; i++)
            {
                MysteryItemInfo mysteryItemInfo1 = self.DBDayActivityInfo.MysteryItemInfos[i];

                if (mysteryItemInfo1.MysteryId != mysteryInfo.MysteryId)
                {
                    continue;
                }
                if (mysteryItemInfo1.ItemNumber < mysteryInfo.ItemNumber)
                {
                    return ErrorCode.ERR_ItemNotEnoughError;
                }

                mysteryItemInfo1.ItemNumber -= mysteryInfo.ItemNumber;
                return ErrorCode.ERR_Success;
            }
            return ErrorCode.ERR_ItemNotEnoughError;
        }

        public static async ETTask NoticeActivityUpdate_Hour(this ActivitySceneComponent self, DateTime dateTime)
        {
            DayOfWeek dayOfWeek = dateTime.DayOfWeek;
            //int yeardate = dateTime.Year * 10000 + dateTime.Month * 100 + dateTime.Day;  //20230412
            int hour = dateTime.Hour;
            int openServerDay = DBHelper.GetOpenServerDay(self.DomainZone());
            LogHelper.LogWarning($"NoticeActivityUpdate_Hour: zone: {self.DomainZone()} openday: {openServerDay}  {hour}", true);
            for (int i = 0; i < self.MapIdList.Count; i++)
            {
                A2A_ActivityUpdateResponse m2m_TrasferUnitResponse = (A2A_ActivityUpdateResponse)await ActorMessageSenderComponent.Instance.Call
                        (self.MapIdList[i], new A2A_ActivityUpdateRequest() { Hour = hour, OpenDay = openServerDay });
            }

            if (hour == 0)
            {
                LogHelper.LogWarning($"神秘商品刷新: {self.DomainZone()}", true);
                self.DBDayActivityInfo.MysteryItemInfos = MysteryShopHelper.InitMysteryItemInfos(openServerDay);
                self.DBDayActivityInfo.PetMingHexinList.Clear();

                self.InitPetMineExtend();
                self.InitFunctionButton();
            }
            if ((hour == 0 || hour == 22) && self.DomainZone() == 3) //通知中心服
            {
                long centerid = DBHelper.GetAccountCenter();
                A2A_ActivityUpdateResponse m2m_TrasferUnitResponse = (A2A_ActivityUpdateResponse)await ActorMessageSenderComponent.Instance.Call
                             (centerid, new A2A_ActivityUpdateRequest() { Hour = 0 });
            }
            if (!ComHelp.IsInnerNet() && self.DomainZone() != 3 && hour == 6)
            {
                Log.Warning($"刷新机器人: {self.DomainZone()}");
                int createRobotNumber = 0;
                if (openServerDay >= 30)
                {
                    createRobotNumber = 10;
                }
                else if (openServerDay >= 20)
                {
                    createRobotNumber = 15;
                }
                else if (openServerDay >= 15)
                {
                    createRobotNumber = 20;
                }
                else if (openServerDay >= 10)
                {
                    createRobotNumber = 25;
                }
                else
                {
                    createRobotNumber = 30;
                }
                long robotSceneId = StartSceneConfigCategory.Instance.GetBySceneName(203, "Robot01").InstanceId;
                MessageHelper.SendActor(robotSceneId, new G2Robot_MessageRequest() { Zone = self.DomainZone(), MessageType = NoticeType.CreateRobot, Message = $"1001#{createRobotNumber}" });
            }

            if (ActivityConfigHelper.GuessRewardList.ContainsKey(hour))
            {
                int guessIndex = RandomHelper.RandomNumber(0, ActivityConfigHelper.GuessNumber);
                List<long> playerIds = null;
                self.DBDayActivityInfo.GuessPlayerList.TryGetValue(guessIndex, out playerIds);
                if (playerIds == null)
                {
                    playerIds = new List<long>();
                }

                List<BagInfo> itemList = new List<BagInfo>();
                string[] rewardItem = ActivityConfigHelper.GuessRewardList[hour].Split('@');
                for (int i = 0; i < rewardItem.Length; i++)
                {
                    string[] itemInfo = rewardItem[i].Split(';');
                    itemList.Add(new BagInfo() { ItemID = int.Parse(itemInfo[0]), ItemNum = int.Parse(itemInfo[1]) });
                }
                long mailServerId = DBHelper.GetMailServerId(self.DomainZone());
                for (int i = 0; i < playerIds.Count; i++)
                {
                    Log.Warning($"发放竞猜奖励: {self.DomainZone()}  {guessIndex} {playerIds[i]}");

                    MailInfo mailInfo = new MailInfo();
                    mailInfo.Status = 0;
                    mailInfo.Title = "竞猜奖励";
                    mailInfo.MailId = IdGenerater.Instance.GenerateId();
                    mailInfo.ItemList.AddRange(itemList);

                    E2M_EMailSendResponse g_EMailSendResponse = (E2M_EMailSendResponse)await ActorMessageSenderComponent.Instance.Call
                                       (mailServerId, new M2E_EMailSendRequest()
                                       {
                                           Id = playerIds[i],
                                           MailInfo = mailInfo,
                                           GetWay = ItemGetWay.Activity,
                                       });
                }

                if (hour == 0)
                {
                    self.DBDayActivityInfo.GuessRewardList.Clear();
                    self.DBDayActivityInfo.OpenGuessIds.Clear();
                }
                if (self.DBDayActivityInfo.GuessRewardList.ContainsKey(hour))
                {
                    self.DBDayActivityInfo.GuessRewardList[hour] = playerIds;
                    self.DBDayActivityInfo.OpenGuessIds.Add(guessIndex);
                }
                else
                {
                    self.DBDayActivityInfo.GuessRewardList.Add(hour, playerIds);
                    self.DBDayActivityInfo.OpenGuessIds.Add(guessIndex);
                }

                self.DBDayActivityInfo.GuessPlayerList.Clear();
            }
        }
    }
}
