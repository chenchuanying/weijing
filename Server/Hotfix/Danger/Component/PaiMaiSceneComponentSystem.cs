using System;
using System.Collections.Generic;

namespace ET
{

    [Timer(TimerType.PaiMaiTimer)]
    public class PaiMaiTimer : ATimer<PaiMaiSceneComponent>
    {
        public override void Run(PaiMaiSceneComponent self)
        {
            try
            {
                self.SaveDB(1).Coroutine();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    [Timer(TimerType.AuctionOverTimer)]
    public class AuctionOverTimer : ATimer<PaiMaiSceneComponent>
    {
        public override void Run(PaiMaiSceneComponent self)
        {
            try
            {

                self.OnAuctionOver().Coroutine();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    

    public class PaiMaiComponentAwakeSystem : AwakeSystem<PaiMaiSceneComponent>
    {
        public override void Awake(PaiMaiSceneComponent self)
        {
            self.InitDBData().Coroutine();
        }
    }

    [ObjectSystem]
    public class PaiMaiComponentDestroySystem : DestroySystem<PaiMaiSceneComponent>
    {

        public override void Destroy(PaiMaiSceneComponent self)
        {
            TimerComponent.Instance.Remove(ref self.Timer);
            TimerComponent.Instance.Remove(ref self.AuctionOverTimer);
        }
    }


    public static class PaiMaiSceneComponentSystem
    {

        public static  void OnAuctionBegin(this PaiMaiSceneComponent self, long overlefttime)
        {
            self.AuctionRecords.Clear();
            //初始化拍卖价格
            self.AuctionPrice = 1000000;
            self.AuctionStart = self.AuctionPrice;
            self.AuctioUnitId = 0;
            self.AuctionPlayer = String.Empty;

            int openDay = DBHelper.GetOpenServerDay(self.DomainZone());
            //第1天
            if (openDay == 1)
            {
                self.AuctionItem = 14060005;
                self.AuctionItemNum = 1;
            }

            //第2天
            if (openDay == 2)
            {
                self.AuctionItem = 15207003;
                self.AuctionItemNum = 1;
            }

            //第3天
            if (openDay == 3)
            {
                self.AuctionItem = 15306003;
                self.AuctionItemNum = 1;
            }

            //第4天
            if (openDay == 4)
            {
                self.AuctionItem = 15302007;
                self.AuctionItemNum = 1;
            }

            //第5天
            if (openDay == 5)
            {
                self.AuctionItem = 15406003;
                self.AuctionItemNum = 1;
            }

            //第6天
            if (openDay == 6)
            {
                self.AuctionItem = 15407003;
                self.AuctionItemNum = 1;
            }

            //第7天
            if (openDay == 7)
            {
                self.AuctionItem = 15506003;
                self.AuctionItemNum = 1;
            }

            //进入循环随机
            if (openDay > 7)
            {
                int[] weights = new int[] { 10, 10, 10, 10, 10, 10, 10, 20, 5 };
                string[] weightsItem = new string[] { "10000143,10", "10000141,1", "10000152,3", "10000150,1", "10000165,1", "10010053,1", "10010040,1", "10045108,1", "10010094,1" };
                int id = RandomHelper.RandomByWeight(weights);

                self.AuctionItem = int.Parse(weightsItem[id].Split(',')[0]);
                self.AuctionItemNum = int.Parse(weightsItem[id].Split(',')[1]);
            }

            //拍卖会开始
            ServerMessageHelper.SendServerMessage(DBHelper.GetChatServerId(self.DomainZone()), NoticeType.PaiMaiAuction,
            $"{self.AuctionItem}_{self.AuctionItemNum}_{self.AuctionPrice}_{self.AuctionPlayer}_1").Coroutine();

            Log.Warning($"拍卖会开始:  {self.DomainZone()}  {self.AuctioUnitId} {self.AuctionPlayer}");
            Log.Warning($"拍卖会开始结束时间:  {overlefttime}");
            self.AuctionStatus = TimeHelper.ServerNow() + overlefttime;
            TimerComponent.Instance.Remove(ref self.AuctionOverTimer);
            self.AuctionOverTimer = TimerComponent.Instance.NewOnceTimer(self.AuctionStatus, TimerType.AuctionOverTimer, self );
        }

        public static void ExtendOverTime(this PaiMaiSceneComponent self)
        {
            if (self.AuctionOverTimer <= 0)
            {
                return;
            }

            DateTime dateTime = TimeInfo.Instance.ToDateTime( TimeHelper.ServerNow() );
            int curTime = dateTime.Hour * 60 + dateTime.Minute;
            int maxTime = 23 * 60 + 58;

            if (curTime <= maxTime &&  self.AuctionStatus - TimeHelper.ServerNow() < TimeHelper.Minute)
            {
                //Console.WriteLine($"有人加价 延迟时间！   {self.DomainZone()}");
                self.AuctionStatus = TimeHelper.ServerNow() + TimeHelper.Minute;
                TimerComponent.Instance.Remove(ref self.AuctionOverTimer);
                self.AuctionOverTimer = TimerComponent.Instance.NewOnceTimer(self.AuctionStatus, TimerType.AuctionOverTimer, self);
            }
            else
            { 
                //Console.WriteLine($"有人加价！   {self.DomainZone()}");
            }
        }

        public static async ETTask OnAuctionOver(this PaiMaiSceneComponent self)
        {
            Console.WriteLine($"拍卖结束！   {self.DomainZone()}");
            long gateServerId = DBHelper.GetGateServerId(self.DomainZone());

            if (self.AuctioUnitId != 0)
            {
                G2T_GateUnitInfoResponse g2M_UpdateUnitResponse = (G2T_GateUnitInfoResponse)await ActorMessageSenderComponent.Instance.Call
                   (gateServerId, new T2G_GateUnitInfoRequest()
                   {
                       UserID = self.AuctioUnitId
                   });

                bool getitem = false;

                //在线
                if (g2M_UpdateUnitResponse.PlayerState == (int)PlayerState.Game && g2M_UpdateUnitResponse.SessionInstanceId > 0)
                {
                    P2M_PaiMaiAuctionOverRequest p2M_PaiMaiAuctionOverRequest = new P2M_PaiMaiAuctionOverRequest()
                    {
                        Price = self.AuctionPrice,
                        ItemID = self.AuctionItem,
                        ItemNumber = self.AuctionItemNum,
                    };

                    Log.Warning($"OnAuctionOver[在线]:  {self.DomainZone()}  {self.AuctioUnitId}  {self.AuctionPlayer}");

                    M2P_PaiMaiAuctionOverResponse m2G_RechargeResponse = (M2P_PaiMaiAuctionOverResponse)await ActorLocationSenderComponent.Instance.Call(self.AuctioUnitId, p2M_PaiMaiAuctionOverRequest);
                    if (m2G_RechargeResponse.Error == ErrorCode.ERR_Success)
                    {
                        getitem = true;
                    }
                    else
                    {
                        //流派则不退还保证金
                        if (self.AuctionJoinList.Contains(self.AuctioUnitId))
                        {
                            self.AuctionJoinList.Remove(self.AuctioUnitId);
                        }
                    }

                    Log.Warning($"OnAuctionOver[在线]:  {m2G_RechargeResponse.Error}  {getitem}");
                }
                else
                {
                    Log.Warning($"OnAuctionOver[离线]:  {self.DomainZone()}  {self.AuctioUnitId}  {self.AuctionPlayer}");
                    UserInfoComponent userInfoComponent = await DBHelper.GetComponentCache<UserInfoComponent>(self.DomainZone(), self.AuctioUnitId);
                    if (userInfoComponent.UserInfo.Gold >= self.AuctionPrice)
                    {
                        userInfoComponent.UserInfo.Gold -= self.AuctionPrice;
                        DBHelper.SaveComponentCache(self.DomainZone(), self.AuctioUnitId, userInfoComponent).Coroutine();

                        //发送道具
                        getitem = true;
                    }
                    else
                    {
                        //流派则不退还保证金
                        if (self.AuctionJoinList.Contains(self.AuctioUnitId))
                        {
                            self.AuctionJoinList.Remove(self.AuctioUnitId);
                        }
                    }

                    Log.Warning($"OnAuctionOver[离线]:   {getitem}");
                }

                if (getitem)
                {
                    MailInfo mailInfo = new MailInfo();
                    mailInfo.Status = 0;
                    mailInfo.Context = "竞拍道具";
                    mailInfo.Title = "竞拍道具";
                    mailInfo.MailId = IdGenerater.Instance.GenerateId();
                    mailInfo.ItemList.Add(new BagInfo() { ItemID = self.AuctionItem, ItemNum = self.AuctionItemNum, GetWay = $"{ItemGetWay.Auction}_{TimeHelper.ServerNow()}" });
                    await MailHelp.SendUserMail(self.DomainZone(), self.AuctioUnitId, mailInfo);
                }
                else
                {
                    MailInfo mailInfo = new MailInfo();
                    mailInfo.Status = 0;
                    mailInfo.Context = "竞拍失败";
                    mailInfo.Title = $"金币小于{self.AuctionPrice},竞拍失败";
                    mailInfo.MailId = IdGenerater.Instance.GenerateId();
                    await MailHelp.SendUserMail(self.DomainZone(), self.AuctioUnitId, mailInfo);
                }
            }

            //退还保证金
            int returnggold = (int)( self.AuctionStart * 0.1f);
            for (int i = 0; i < self.AuctionJoinList.Count; i++)
            {
                MailInfo mailInfo = new MailInfo();
                mailInfo.Status = 0;
                mailInfo.Context = "退还保证金";
                mailInfo.Title = "退还保证金";
                mailInfo.MailId = IdGenerater.Instance.GenerateId();
                mailInfo.ItemList.Add(new BagInfo() { ItemID = 1, ItemNum = returnggold, GetWay = $"{ItemGetWay.Auction}_{TimeHelper.ServerNow()}" });

                await MailHelp.SendUserMail(self.DomainZone(), self.AuctionJoinList[i], mailInfo);
            }

            //其他玩家退还保证金
            self.AuctionJoinList.Clear();
            self.AuctionStatus = -1;

            //拍卖会结束
            ServerMessageHelper.SendServerMessage(DBHelper.GetChatServerId(self.DomainZone()), NoticeType.PaiMaiAuction,
            $"{self.AuctionItem}_{self.AuctionItemNum}_{self.AuctionPrice}_{self.AuctionPlayer}_2").Coroutine();

            Log.Warning($"拍卖会结束:  {self.DomainZone()} {self.AuctionPlayer}  {self.AuctionPrice} {self.AuctionItem}:{self.AuctionItemNum}");
        }

        public static async ETTask BeginAuctionTimer(this PaiMaiSceneComponent self)
        {
            self.AuctionStatus = 0;
            self.AuctionRecords.Clear();
            DateTime dateTime = TimeHelper.DateTimeNow();
            long curTime = (dateTime.Hour * 60 + dateTime.Minute) * 60 + dateTime.Second;
            long openTime = FunctionHelp.GetOpenTime(1040);
            long closeTime = FunctionHelp.GetCloseTime(1040);

            if (curTime < openTime)
            {
                await TimerComponent.Instance.WaitAsync((openTime - curTime) * TimeHelper.Second);
                dateTime = TimeHelper.DateTimeNow();
                curTime = (dateTime.Hour * 60 + dateTime.Minute) * 60 + dateTime.Second;
                self.OnAuctionBegin((closeTime - curTime) * 1000);
            }
            else if (curTime >= openTime && curTime <= closeTime)
            {
                self.OnAuctionBegin((closeTime - curTime) * 1000);
            }
            else
            {

            }
        }


        /// <summary>
        /// 拍卖商店
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETTask InitPaiMainShop(this PaiMaiSceneComponent self, int itemType, List<PaiMaiShopItemInfo> oldPaiMaiShop)
        {
            int zone = self.DomainZone();
            long unitid = PaiMaiHelper.Instance.GetPaiMaiId(itemType);
            long dbCacheId = DBHelper.GetDbCacheId(zone);

            //D2G_GetComponent d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = unitid, Component = DBHelper.DBPaiMainInfo });
            List<DBPaiMainInfo> paimaiList = await Game.Scene.GetComponent<DBComponent>().Query<DBPaiMainInfo>(self.DomainZone(), d => d.Id == unitid);
            if (zone == 66)
            {
                Log.Console("zone == 66");
            }
            if (paimaiList == null || paimaiList.Count == 0)
            {
                //初始拍卖行商店
                DBPaiMainInfo dBPaiMainInfo = new DBPaiMainInfo();
                dBPaiMainInfo.Id = unitid;
                self.dBPaiMainInfo_Shop = dBPaiMainInfo;
                //存储拍卖行商店
                //D2M_SaveComponent d2GSave = (D2M_SaveComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new M2D_SaveComponent() { UnitId = unitid, EntityByte = MongoHelper.ToBson(dBPaiMainInfo), ComponentType = DBHelper.DBPaiMainInfo });
                await Game.Scene.GetComponent<DBComponent>().Save<DBPaiMainInfo>(self.DomainZone(), dBPaiMainInfo);
            }
            else
            {
                self.dBPaiMainInfo_Shop = paimaiList[0];
            }

            //更新快捷购买列表
            self.UpdatePaiMaiShopItemList();
        }

        public static async ETTask InitPaiMainStall(this PaiMaiSceneComponent self, int itemType, List<PaiMaiItemInfo> oldPaiMaiStall)
        {
            int zone = self.DomainZone();
            long unitid = PaiMaiHelper.Instance.GetPaiMaiId(itemType);
            long dbCacheId = DBHelper.GetDbCacheId(zone);

            // D2G_GetComponent d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = unitid, Component = DBHelper.DBPaiMainInfo });
            List<DBPaiMainInfo> paimaiList = await Game.Scene.GetComponent<DBComponent>().Query<DBPaiMainInfo>(self.DomainZone(), d => d.Id == unitid);
            if (zone == 66)
            {
                Log.Console("zone == 66");
            }

            if (paimaiList == null || paimaiList.Count == 0)
            {
                //初始摆摊数据
                DBPaiMainInfo dBPaiMainInfo = new DBPaiMainInfo();
                dBPaiMainInfo.Id = unitid;
                self.dBPaiMainInfo_Stall = dBPaiMainInfo;
                self.dBPaiMainInfo_Stall.PaiMaiItemInfos = oldPaiMaiStall;
                //存储摆摊数据
                //D2M_SaveComponent d2GSave = (D2M_SaveComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new M2D_SaveComponent() { UnitId = unitid, EntityByte = MongoHelper.ToBson(dBPaiMainInfo), ComponentType = DBHelper.DBPaiMainInfo });
                await Game.Scene.GetComponent<DBComponent>().Save<DBPaiMainInfo>(self.DomainZone(), dBPaiMainInfo);
            }
            else
            {
                self.dBPaiMainInfo_Stall = paimaiList[0];
            }
        }

        public static List<PaiMaiItemInfo> GetItemListByUser(this PaiMaiSceneComponent self, long useriD, List<PaiMaiItemInfo> oldPaiMaiAl)
        {
            List<PaiMaiItemInfo> paiMaiType = new List<PaiMaiItemInfo>();

            for (int i = 0; i < oldPaiMaiAl.Count; i++)
            {
                if (useriD != 9 && oldPaiMaiAl[i].UserId == useriD)
                {
                    paiMaiType.Add(oldPaiMaiAl[i]);
                }
            }

            return paiMaiType;
        }

        public static List<PaiMaiItemInfo> GetItemListByType(this PaiMaiSceneComponent self, int itemType, List<PaiMaiItemInfo> oldPaiMaiAl)
        {
            List<PaiMaiItemInfo> paiMaiType = new List<PaiMaiItemInfo>();

            for (int i = 0;  i < oldPaiMaiAl.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(oldPaiMaiAl[i].BagInfo.ItemID);
                if (itemConfig.ItemType == itemType)
                {
                    paiMaiType.Add(oldPaiMaiAl[i]);
                }
            }

            return paiMaiType;
        }

        public static void UpdatePaiMaiDBByType(this PaiMaiSceneComponent self, int itemType, DBPaiMainInfo dBPaiMainInfo_Type)
        {
            switch (itemType)
            {
                case 1:
                    self.dBPaiMainInfo_Consume = dBPaiMainInfo_Type;
                    break;
                case 2:
                    self.dBPaiMainInfo_Material = dBPaiMainInfo_Type;
                    break;
                case 3:
                    self.dBPaiMainInfo_Equipment = dBPaiMainInfo_Type;
                    break;
                case 4:
                    self.dBPaiMainInfo_Gemstone = dBPaiMainInfo_Type;
                    break;
                default:
                    Log.Error($"InitPaiMainShangJia: {itemType}");
                    break;
            }

        }

        public static DBPaiMainInfo GetPaiMaiDBByType(this PaiMaiSceneComponent self, int itemType)
        {
            DBPaiMainInfo dBPaiMainInfo_Type = null;
            switch (itemType)
            {
                case 1:
                    dBPaiMainInfo_Type = self.dBPaiMainInfo_Consume;
                    break;
                case 2:
                    dBPaiMainInfo_Type = self.dBPaiMainInfo_Material;
                    break;
                case 3:
                    dBPaiMainInfo_Type = self.dBPaiMainInfo_Equipment;
                    break;
                case 4:
                    dBPaiMainInfo_Type = self.dBPaiMainInfo_Gemstone;
                    break;
                default:
                    Log.Warning($"InitPaiMainShangJia: {itemType}");
                    break;
            }

            return dBPaiMainInfo_Type;  
        }

        public static async ETTask InitPaiMaiShangJia(this PaiMaiSceneComponent self, int itemType, List<PaiMaiItemInfo> oldPaiMaiAll)
        {
            int zone = self.DomainZone();
            long unitid = PaiMaiHelper.Instance.GetPaiMaiId(itemType);
            long dbCacheId = DBHelper.GetDbCacheId(zone);

            //D2G_GetComponent d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = unitid, Component = DBHelper.DBPaiMainInfo });
            List<DBPaiMainInfo> paimaiList = await Game.Scene.GetComponent<DBComponent>().Query<DBPaiMainInfo>(self.DomainZone(), d => d.Id == unitid);
            if (zone == 66)
            {
                Log.Console("zone == 66");
            }

            if (paimaiList == null || paimaiList.Count == 0)
            {
                //初始摆摊数据
                DBPaiMainInfo dBPaiMainInfo = new DBPaiMainInfo();
                dBPaiMainInfo.Id = unitid;
                dBPaiMainInfo.PaiMaiItemInfos = self.GetItemListByType(itemType, oldPaiMaiAll);
                self.UpdatePaiMaiDBByType(itemType, dBPaiMainInfo);
                //存储摆摊数据
                //D2M_SaveComponent d2GSave = (D2M_SaveComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new M2D_SaveComponent() { UnitId = unitid, EntityByte = MongoHelper.ToBson(dBPaiMainInfo), ComponentType = DBHelper.DBPaiMainInfo });
                await Game.Scene.GetComponent<DBComponent>().Save<DBPaiMainInfo>(self.DomainZone(), dBPaiMainInfo);
            }
            else
            {
                self.UpdatePaiMaiDBByType(itemType, paimaiList[0]);
            }
        }

        public static async ETTask InitDBData(this PaiMaiSceneComponent self)
        {
            int zone = self.DomainZone();
            long dbCacheId = DBHelper.GetDbCacheId(zone);
            await TimerComponent.Instance.WaitAsync(RandomHelper.RandomNumber(5000, 10000));

            List<PaiMaiShopItemInfo> oldPaiMaiShop = new List<PaiMaiShopItemInfo>();
            List<PaiMaiItemInfo> oldPaiMaiAll = new List<PaiMaiItemInfo>();
            List<PaiMaiItemInfo> oldPaiMaiStall = new List<PaiMaiItemInfo>();

            //D2G_GetComponent d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = zone, Component = DBHelper.DBPaiMainInfo });
            List<DBPaiMainInfo> paimaiList = await Game.Scene.GetComponent<DBComponent>().Query<DBPaiMainInfo>(self.DomainZone(), d => d.Id == zone);

            if (paimaiList != null && paimaiList.Count > 0)
            {
                DBPaiMainInfo oldDBPaiMainInfo = paimaiList[0];
                if (oldDBPaiMainInfo.PaiMaiShopItemInfos.Count > 0
                    || oldDBPaiMainInfo.PaiMaiItemInfos.Count > 0
                    || oldDBPaiMainInfo.StallItemInfos.Count > 0)
                {
                    Console.WriteLine($"拍卖有旧数据:  {zone}   {oldDBPaiMainInfo.PaiMaiItemInfos.Count}");

                    oldPaiMaiShop = oldDBPaiMainInfo.PaiMaiShopItemInfos;
                    oldPaiMaiAll = oldDBPaiMainInfo.PaiMaiItemInfos;
                    oldPaiMaiStall = oldDBPaiMainInfo.StallItemInfos;
                }
            }
            else
            {
                Console.WriteLine($"拍卖无旧数据:  {zone}");
            }

            await self.InitPaiMaiShangJia(1, oldPaiMaiAll);
            await self.InitPaiMaiShangJia(2, oldPaiMaiAll);
            await self.InitPaiMaiShangJia(3, oldPaiMaiAll);
            await self.InitPaiMaiShangJia(4, oldPaiMaiAll);

            await self.InitPaiMainShop(11, oldPaiMaiShop);
            await self.InitPaiMainStall(12, oldPaiMaiStall);

            self.Timer = TimerComponent.Instance.NewRepeatedTimer(TimeHelper.Minute * 30 + RandomHelper.RandomNumber(1000, 10000), TimerType.PaiMaiTimer, self);
            self.OnZeroClockUpdate();
        }

        //更新快捷购买列表
        public static void UpdatePaiMaiShopItemList(this PaiMaiSceneComponent self)
        {
            self.dBPaiMainInfo_Shop.PaiMaiShopItemInfos = PaiMaiHelper.Instance.InitPaiMaiShopItemList(self.dBPaiMainInfo_Shop.PaiMaiShopItemInfos);
        }

        //零点刷新
        public static void OnZeroClockUpdate(this PaiMaiSceneComponent self)
        {
            //更新价格
            self.UpdatePaiMaiShopItemPrice();

            self.UpdateShangJiaItems();

            self.BeginAuctionTimer().Coroutine();
        }

        //每天更新道具物品价格
        public static void UpdatePaiMaiShopItemPrice(this PaiMaiSceneComponent self)
        {
            int curzone = ServerHelper.GetOldServerId(self.DomainZone());
            int openserverDay = DBHelper.GetOpenServerDay(curzone);
            Log.Info($"curzone = {curzone} openserverDay = {openserverDay} PaiMaiScene开服天数 {self.DomainZone()} {openserverDay}");
            if (openserverDay == 0)
            {
                return;
            }

            List<PaiMaiShopItemInfo> paiMaiShopItemInfos = self.dBPaiMainInfo_Shop.PaiMaiShopItemInfos;
            for (int i = 0; i < paiMaiShopItemInfos.Count; i++)
            {
                float upPrice = RandomHelper.RandomNumberFloat(0.03f, 0.06f);
                PaiMaiShopItemInfo info = paiMaiShopItemInfos[i];
                int sellid = PaiMaiHelper.Instance.GetPaiMaiSellId((int)info.Id);
                if (sellid == 0)
                {
                    continue;
                }

                int PriceMax = PaiMaiSellConfigCategory.Instance.Get(sellid).PriceMax;
                int PriceMin = PaiMaiSellConfigCategory.Instance.Get(sellid).PriceMin;
                /*
                if (openserverDay > 15 && info.Price <= PriceMax)
                {
                    continue;    
                }
                */

                if (info.Price > PriceMax)
                {
                    info.Price = PriceMax;
                    info.PricePro = 1f;
                }
                else
                {
                    //如果当前出售数量为0,则进行降价
                    if (info.BuyNum == 0)
                    {
                        upPrice = upPrice * -1;
                    }

                    info.PricePro = 1 + upPrice;
                    int Price = (int)(info.Price * info.PricePro);
                    //物价限制最低和最高
                    info.Price = Math.Min(Price, PriceMax);
                    int minPrice = PriceMin;
                    info.Price = Math.Max(info.Price, minPrice);

                    //清空当天的购买记录
                    info.BuyNum = 0;
                }

                //Log.Info($"{info.Id} curzone = {curzone} Price = {info.Price} buy = {info.BuyNum} PricePro = {info.PricePro}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <param name="deleteType">0删角 1回档</param>
        /// <param name="userId"></param>
        public static void OnDeleteRole(this PaiMaiSceneComponent self, int deleteType, long userId)
        {
            if (userId <= 0)
            {
                return;
            }
            //Console.WriteLine($"OnDeleteRole.PaiMai :  {self.DomainZone()} {userId}");
            self.OnDeleteRole_ByType(userId, self.dBPaiMainInfo_Consume);
            self.OnDeleteRole_ByType(userId, self.dBPaiMainInfo_Material);
            self.OnDeleteRole_ByType(userId, self.dBPaiMainInfo_Gemstone);
            self.OnDeleteRole_ByType(userId, self.dBPaiMainInfo_Equipment);
        }

        public static void OnDeleteRole_ByType(this PaiMaiSceneComponent self, long userId, DBPaiMainInfo dBPaiMainInfo)
        {
            List<PaiMaiItemInfo> paimaiItems = dBPaiMainInfo.PaiMaiItemInfos;

            for (int i = paimaiItems.Count - 1; i >= 0; i--)
            {
                PaiMaiItemInfo paiMaiItem = paimaiItems[i];
                if (paiMaiItem.UserId != userId)
                {
                    continue;
                }

                dBPaiMainInfo.PaiMaiItemInfos.RemoveAt(i);
            }
        }

        //遍历上架道具
        public static void UpdateShangJiaItems(this PaiMaiSceneComponent self)
        {
            self.UpdateShangJiaItems_ByType(self.dBPaiMainInfo_Consume );
            self.UpdateShangJiaItems_ByType(self.dBPaiMainInfo_Material);
            self.UpdateShangJiaItems_ByType(self.dBPaiMainInfo_Equipment);
            self.UpdateShangJiaItems_ByType(self.dBPaiMainInfo_Gemstone);
        }


        public static void UpdateShangJiaItems_ByType(this PaiMaiSceneComponent self, DBPaiMainInfo dBPaiMainInfo)
        {
            List<PaiMaiItemInfo> paimaiItems = dBPaiMainInfo.PaiMaiItemInfos;

            for (int i = paimaiItems.Count - 1; i >= 0; i--)
            {
                PaiMaiItemInfo paiMaiItem = paimaiItems[i];

                //int price = 0;
                PaiMaiShopItemInfo shopInfo = self.GetPaiMaiShopInfo(paiMaiItem.BagInfo.ItemID);
                if (shopInfo != null && shopInfo.Price <= 500000 && ItemConfigCategory.Instance.Get(paiMaiItem.BagInfo.ItemID).ItemType != 3)
                {
                    //int singPro = (int)(paiMaiItem.Price / paiMaiItem.BagInfo.ItemNum);  //单价
                    float pro = paiMaiItem.Price / shopInfo.Price;
                    float buyPro = 0;

                    if (pro <= 0.5f)
                    {
                        buyPro = 0.3f;
                    }
                    else if (pro <= 0.75f)
                    {
                        buyPro = 0.2f;
                    }
                    else if (pro <= 1f)
                    {
                        buyPro = 0.1f;
                    }
                    else if (pro <= 1.2f)
                    {
                        buyPro = 0.05f;
                    }
                    else if (pro <= 1.5f)
                    {
                        buyPro = 0.025f;
                    }

                    ItemConfig itemCof = ItemConfigCategory.Instance.Get(paiMaiItem.BagInfo.ItemID);
                    int costNum = 0;
                    switch (itemCof.ItemQuality)
                    {

                        case 1:
                            costNum = RandomHelper.NextInt(1, 100);
                            break;
                        //绿色道具随机数量
                        case 2:
                            costNum = RandomHelper.NextInt(1, 100);
                            break;
                        //蓝道具随机数量
                        case 3:
                            costNum = RandomHelper.NextInt(1, 10);
                            break;
                        //紫色道具随机数量
                        case 4:
                            costNum = RandomHelper.NextInt(1, 5);
                            break;

                    }
                    //不能超过当前拥有上限
                    costNum = Math.Min(costNum, paiMaiItem.BagInfo.ItemNum);

                    //概率购买
                    if (pro < 1.5f && RandomHelper.RandFloat01() < buyPro)
                    {
                        Log.Info("拍卖行系统购买 概率:" + buyPro + "出售价格:" + paiMaiItem.Price * costNum + "玩家名称:" + paiMaiItem.PlayerName + "出售道具:" + paiMaiItem.BagInfo.ItemID + "出售单价:" + paiMaiItem.Price + "道具拥有数量:" + paiMaiItem.BagInfo.ItemNum);
                        MailHelp.SendPaiMaiEmail(self.DomainZone(), paiMaiItem, costNum, 0).Coroutine();

                        paiMaiItem.BagInfo.ItemNum -= costNum;
                        if (paiMaiItem.BagInfo.ItemNum <= 0)
                        {
                            dBPaiMainInfo.PaiMaiItemInfos.RemoveAt(i);
                        }
                    }
                }
            }
        }

        //根据道具ID获取对应快捷购买的列表
        public static PaiMaiShopItemInfo GetPaiMaiShopInfo(this PaiMaiSceneComponent self, long needItemID)
        {
            //获取当前的数据
            foreach (PaiMaiShopItemInfo info in self.dBPaiMainInfo_Shop.PaiMaiShopItemInfos)
            {
                if (info.Id == needItemID)
                {
                    return info;
                }
            }
            return null;
        }

        //根据道具ID获取对应快捷购买的列表
        public static void PaiMaiShopInfoAddBuyNum(this PaiMaiSceneComponent self, long needItemID, int buyNum)
        {
            foreach (PaiMaiShopItemInfo info in self.dBPaiMainInfo_Shop.PaiMaiShopItemInfos)
            {
                if (info.Id == needItemID)
                {
                    info.BuyNum += buyNum;
                }
            }
        }

        public static async ETTask SaveDB(this PaiMaiSceneComponent self, int random)
        {
            //if (random == 1)
            //{
            //    if (RandomHelper.RandomNumber(1,3) != 1)
            //    {
            //        return;
            //    }
            //}

            int zone = self.DomainZone();
            await self.CheckOverTime(self.dBPaiMainInfo_Consume);
            await self.CheckOverTime(self.dBPaiMainInfo_Material);
            await self.CheckOverTime(self.dBPaiMainInfo_Equipment);
            await self.CheckOverTime(self.dBPaiMainInfo_Gemstone);
            await self.CheckOverTime(self.dBPaiMainInfo_Stall);

            await self.SavePaiMaiData(PaiMaiHelper.Instance.GetPaiMaiId(1), self.dBPaiMainInfo_Consume);
            await TimerComponent.Instance.WaitAsync(RandomHelper.RandomNumber(1000,5000));
            await self.SavePaiMaiData(PaiMaiHelper.Instance.GetPaiMaiId(2), self.dBPaiMainInfo_Material);
            await TimerComponent.Instance.WaitAsync(RandomHelper.RandomNumber(1000, 5000));
            await self.SavePaiMaiData(PaiMaiHelper.Instance.GetPaiMaiId(3), self.dBPaiMainInfo_Equipment);
            await TimerComponent.Instance.WaitAsync(RandomHelper.RandomNumber(1000, 5000));
            await self.SavePaiMaiData(PaiMaiHelper.Instance.GetPaiMaiId(4), self.dBPaiMainInfo_Gemstone);

            await self.SavePaiMaiData(PaiMaiHelper.Instance.GetPaiMaiId(11), self.dBPaiMainInfo_Shop);
            await self.SavePaiMaiData(PaiMaiHelper.Instance.GetPaiMaiId(12), self.dBPaiMainInfo_Stall);
        }

        public static async ETTask SavePaiMaiData(this PaiMaiSceneComponent self, long unitId, DBPaiMainInfo dBPaiMainInfo)
        {
            Log.Warning($"PaiMaiSceneComponent.SaveDB:  zone:{self.DomainZone()}  id:{unitId}  {dBPaiMainInfo.PaiMaiItemInfos.Count}");
            
            //long dbCacheId = DBHelper.GetDbCacheId(self.DomainZone());
            //D2M_SaveComponent d2GSave = (D2M_SaveComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new M2D_SaveComponent() { UnitId = unitId, EntityByte = MongoHelper.ToBson(dBPaiMainInfo), ComponentType = DBHelper.DBPaiMainInfo });
            await Game.Scene.GetComponent<DBComponent>().Save<DBPaiMainInfo>(self.DomainZone(), dBPaiMainInfo);
        }

        public static async ETTask CheckOverTime(this PaiMaiSceneComponent self, DBPaiMainInfo dBPaiMainInfo)
        {
            //检测超时的道具
            long currentTime = TimeHelper.ServerNow();

            List<long> removeIds = new List<long>();
            for (int i = 1600; i < dBPaiMainInfo.PaiMaiItemInfos.Count; i++ )
            {
                removeIds.Add(dBPaiMainInfo.PaiMaiItemInfos[i].Id);
            }

            for (int i = dBPaiMainInfo.PaiMaiItemInfos.Count - 1; i >= 0; i--)
            {
                PaiMaiItemInfo paiMaiItemInfo = dBPaiMainInfo.PaiMaiItemInfos[i];
                if (currentTime - paiMaiItemInfo.SellTime >= TimeHelper.OneDay || removeIds.Contains(dBPaiMainInfo.PaiMaiItemInfos[i].Id))
                {
                    long emaiId = StartSceneConfigCategory.Instance.GetBySceneName(self.DomainZone(), Enum.GetName(SceneType.EMail)).InstanceId;
                    E2P_PaiMaiOverTimeResponse g_SendChatRequest = (E2P_PaiMaiOverTimeResponse)await ActorMessageSenderComponent.Instance.Call
                        (emaiId, new P2E_PaiMaiOverTimeRequest()
                        {
                            PaiMaiItemInfo = paiMaiItemInfo
                        });
                    dBPaiMainInfo.PaiMaiItemInfos.RemoveAt(i);
                }
            }
        }

    }
}
