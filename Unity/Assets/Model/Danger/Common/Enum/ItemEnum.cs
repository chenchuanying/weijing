
using System.Collections.Generic;

namespace ET
{

    /// <summary>
    /// 道具获取方式[0系统默认1]
    /// </summary>
    public static class ItemGetWay
    {
        public const int System = 1;                //系统赠与
        public const int FubenGetReward = 2;        //副本结算奖励
        public const int ChouKa = 3;                //抽卡
        public const int Energy = 4;                //正能量
        public const int GM = 5;                    //GM
        public const int ItemBox_6 = 6;               //道具盒子
        public const int XiLianLevel = 7;           //洗练大师
        public const int LingDiReward = 8;          //领地
        public const int MysteryBuy = 9;            //神秘商店
        public const int PetFubenReward = 10;       //宠物副本奖励
        public const int PetHeXinHeCheng = 11;      //宠物之核合成
        public const int RandomTowerReward = 12;    //随机塔奖励
        public const int ShoujiReward = 13;         //收集奖励
        public const int StoreBuy = 14;             //商店购买
        public const int TaskCountry = 15;          //活跃任务
        public const int YueKaReward = 16;          //月卡奖励
        public const int ChengJiuRward = 17;        //成就奖励
        public const int RankReward = 18;           //排行榜奖励
        public const int FirstWin = 19;             //首胜
        public const int PickItem = 20;             //拾取
        public const int PaiMaiShop = 21;           //拍賣商店
        public const int PetEggDuiHuan = 22;        //宠物蛋兑换
        public const int TaskReward = 23;           //任务奖励
        public const int PetFenjie = 24;            //宠物分解
        public const int BattleWin = 25;            //战场胜利
        public const int ReceieMail = 26;           //邮件
        public const int Melting = 27;              //熔炼
        public const int TiaoZhan = 28;             //挑战之地
        public const int SkillMake = 29;            //生活制造
        public const int HuiShou = 30;              //回收系统
        public const int XiaJia = 31;               //拍卖行下架
        public const int DuiHuan = 32;              //兑换
        public const int HongBao = 33;              //红包系统
        public const int CostItem = 34;             //扣除道具
        public const int Share = 35;                //分享
        public const int PaiMaiBuy = 36;            //拍卖购买
        public const int XiuLian = 37;              //修炼
        public const int TreasureMap = 38;          //藏宝图获得
        public const int Sell = 39;                 //出售获得
        public const int PaiMaiSell = 40;           //拍賣出售
        public const int BuChang = 41;              //补偿
        public const int JingLing = 42;             //精灵
        public const int JiaYuanGather = 43;        //家园种植
        public const int JiaYuanMale = 44;          //JiaYuanMale
        public const int JiaYuanSell = 45;          //家园出售
        public const int JiaYuanCost = 46;          //家园资金兑换
        public const int Popularize = 47;           //推广
        public const int Serial = 48;               //序列号奖励
        public const int JiaYuanCook = 49;          //家园厨房
        public const int Donation = 50;             //捐献
        public const int UnionXiuLian = 51;         //家族修炼
        public const int UnionBoss = 52;            //家族BOSS
        public const int UnionRace = 53;            //家族争霸
        public const int Auction = 54;              //竞拍
        public const int PetChouKa = 55;             //宠物抽卡
        public const int ItemBox_8 = 56;               //道具盒子
        public const int ItemBox_9 = 57;               //道具盒子
        public const int ItemBox_104 = 58;               //道具盒子
        public const int ItemBox_106 = 59;               //道具盒子
        public const int PetTianTiReward = 60;          //宠物天梯
        public const int JiaYuanExchange = 61;          //家园兑换
        public const int GemHuiShou = 62;               //宝石回收  
        public const int ArenaWin = 63;                 //角斗场胜利
        public const int MiJingBoss = 64;               //密境BOSS
        public const int AuctionJoin = 65;              //竞拍保证金
        public const int UnionUpLv = 66;                 //家族升级
        public const int GemHeCheng = 67;                //宝石合成
        public const int SoloReward = 68;               //竞技场奖励
        public const int Activity_MaoXianJia = 69;      //冒险家
        public const int Activity_ZhanQu = 70;          //战区活动
        public const int Recharge = 71;                 //充值
        public const int TowerOfSealCost = 72;          // 封印之塔消耗钻石
        public const int JueXing = 73;                  //觉醒
        public const int Activity_DayTeHui = 74;           //每日特惠
        public const int UnionMysteryBuy = 75;            //家族神秘商店
        public const int HappyMove = 76;                    //喜从天降
        public const int Turtle = 77;                   //小龟大赛
        public const int ShowLie = 78;                  //狩猎
        public const int RunRace = 79;                  // 奔跑大赛
        public const int Demon = 80;                  // 恶魔
        public const int PetMine = 81;                  //矿场
        public const int Season = 82;                   //赛季奖励
        public const int Welfare = 83;                  //福利
        public const int LeavlReward = 84;              //等级奖励
        public const int KillMonsterReward = 85;        //击败怪物奖励
        public const int StallBuy = 86;                 //摆摊购买
        public const int ActivityChouKa = 87;                 //摆摊购买
        public const int ActivityConsume = 88;         //消耗钻石
        public const int ActivityHongBao = 89;          //红包
        public const int PetExplore = 90;               //宠物探索
        public const int SingleRecharge = 91;           //单笔充值奖励
        public const int ItemXiLian = 92;               //道具洗练
        public const int RechargeReward = 93;                 //充值
        public const int ChouKaReward = 94;                //抽卡奖励
        public const int PetExploreReward = 95;
        public const int PetHeXinExplore = 96;
        public const int ActivityNewYear = 97;
        public const int Fashion = 98;
        public const int PetEggPutOut = 99;
        public const int Activity = 100;                //活动


        //以下途径获取的道具绑定道具,其他途径为非绑定道具
        public static List<int> ItemGetBing = new List<int>()
        {
                RankReward,	//拍卖行
                BattleWin,   //战场胜利
                BuChang,   //补偿
                Share, 	//分享
                Popularize, //推广
                Serial,  //序列号奖励
                Activity_DayTeHui,
        };

    }


    //道具类型
    //1：消耗性道具
    //2：材料
    //3：装备
    //4：宝石
    public static class ItemTypeEnum
    {
        public const int ALL = 0;
        public const int Consume = 1;
        public const int Material = 2;
        public const int Equipment = 3;
        public const int Gemstone = 4;
        public const int PetHeXin = 5;
    }

    //装备类型细分
    //0:通用
    //1:剑
    //2:刀
    //3:法杖
    //4:魔法书
    //5:弓手
    //11:布甲
    //12:轻甲
    //13:重甲
    public static class ItemEquipType
    {
        public const int Common = 0;
        public const int Sword = 1;
        public const int Knife = 2;
        public const int Wand = 3;
        public const int Book = 4;
        public const int Bow = 5;

        public const int Bujia = 11;
        public const int QingJia = 12;
        public const int ZhongJia = 13;
    }

    //道具存放位置
    //0背包
    //1宠物之核背包
    //2装备
    //3宝石 (废弃)
    //4宠物装备
    //5仓库1
    //6仓库2
    //7仓库3
    //8仓库4
    //9-12 家园仓库
    //13-14 家园藏宝仓库
    public enum ItemLocType : int
    {
        ItemLocBag = 0,
        ItemPetHeXinBag = 1,
        ItemLocEquip = 2,
        ItemLocGem = 3,
        ItemPetHeXinEquip = 4,
        ItemWareHouse1 = 5,
        ItemWareHouse2 = 6,
        ItemWareHouse3 = 7,
        ItemWareHouse4 = 8,
        JianYuanWareHouse1 = 9,
        JianYuanWareHouse2 = 10,
        JianYuanWareHouse3 = 11,
        JianYuanWareHouse4 = 12,
        JianYuanTreasureMapStorage1 = 13, // 家园藏宝图仓库_存藏宝图的
        JianYuanTreasureMapStorage2 = 14, // 家园藏宝图仓库_存生活材料的
        ChouKaWarehouse = 15, // 抽卡仓库
        ItemLocEquip_2 = 16,    //第二套装备
        SeasonJingHe = 17,      //赛季晶核
        PetLocEquip = 18,       //宠物装备
        GemWareHouse1 = 19,     //宝石仓库

        ItemLocMax,
    }

    //道具装备位置
    //1 武器
    //2 衣服
    //3 护符
    //4 戒指
    //5 饰品
    //6 鞋子
    //7 裤子
    //8 腰带
    //9 手镯
    //10 头盔
    //11 项链
    public enum ItemSubTypeEnum : int
    {
        Wuqi    = 1,
        Yifu    = 2,
        Fuhu    = 3,
        Jiezhi  =4,
        Shiping =5,
        Xiezi   =6,
        Kuzi    =7,
        Yaodai  =8,
        Shouzhuo=9,
        Toukui  =10,
        Xianglian=11,
    }

    //1:白色
    //2：绿色
    //3：蓝色
    //4：紫色
    //5：橙色
    public enum ItemQualityEnem : int
    {
        Quality1 = 1,
        Quality2,
        Quality3,
        Quality4,
        Quality5
    }

    public enum ItemOperateEnum : int
    { 
        None = 0,
        Bag = 1,                    //背包打开显示对应功能按钮
        Juese = 2,                  //角色栏打开显示对应功能按钮
        Shop = 3,                   //商店查看属性
        Cangku = 4,                 //仓库查看属性
        Watch = 5,                   //查看其他玩家
        HuishouBag = 6,             //回收背包打开
        HuishouShow = 7,            //回收列表展示
        XiangQianGem = 8,          //镶嵌在装备上的宝石
        Muchang = 9,                //牧场  不显示任何按钮
        MuchangCangku = 10,         //牧场仓库  显示出售按钮
        PetXiLian = 11,
        SkillSet = 12,
        CangkuBag = 13,             //仓库背包
        MakeItem = 14,
        MailReward = 15,
        ItemXiLian = 16,
        PaiMaiSell = 17,            //拍卖出售
        TaskItem = 18,              //任务
        XiangQianBag = 19,          //在镶嵌切页的背包中
        PetHeXinBag = 20,
        JianYuanBag = 21,     
        PaiMaiBuy = 22,
        PetEquipBag = 23,
        AccountBag = 24,             //账号仓库
        AccountCangku = 25,
        GemBag = 26,                //宝石仓库背包
        GemCangku = 27,             //宝石仓库
    }

}
