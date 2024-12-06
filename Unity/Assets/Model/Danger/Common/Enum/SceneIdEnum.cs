

namespace ET
{

    public struct CreateMonsterInfo
    {
        public int SkillId;             //魔能房间台子技能
        public int PlayerLevel;
        public string AttributeParams;
        public int Camp;            //阵营
        public long MasterID;
        public int Rotation;         //朝向
        public long BornTime;        //出生的时间(毫秒)
        public long UnitId;
        public int AI;
        public int SkinId;
    }

    public static class TeamFubenType
    {
        public const int Normal = 1;
        public const int XieZhu = 2;
        public const int ShenYuan = 3;
    }

    public static class FubenDifficulty
    {
        public const int None = 0;
        public const int Normal = 1;
        public const int TiaoZhan = 2;
        public const int DiYu = 3;
    }

    //NONE = 0,
    //INIT = 1,
    //LOGIN = 2,
    //MAIN_CITY = 3,
    //BATTLE = 4,
    //FUBEN = 5,
    public static class SceneTypeEnum
    {
        public const int NONE = 0;
        public const int InitScene = 1;
        public const int LoginScene = 2;         //登录scene
        public const int MainCityScene = 3;      //主城
        public const int CellDungeon = 4;       //格子副本
        public const int TeamDungeon = 5;        //组队副本
        public const int PetTianTi = 6;         //宠物天梯
        public const int BaoZang = 7;        //野外地图 [宝藏之地]
        public const int Tower = 8;             //爬塔   别名挑战之地
        public const int LocalDungeon = 9;      //本地副本
        public const int PetDungeon = 10;       //宠物闯关副本
        public const int RandomTower = 11;       //通天塔
        public const int Battle = 12;           //阵营战场
        public const int TrialDungeon = 13;     //试炼之地
        public const int MiJing = 14;        //野外地图 [秘境]
        public const int Arena = 15;        //角斗场
        public const int JiaYuan = 16;       //家园
        public const int Union = 17;        //家族
        public const int UnionRace = 18;    //家族争霸赛
        public const int Solo = 19;         //竞技场单人
        public const int TowerOfSeal = 20;  // 封印之塔
        public const int Happy = 21;        //喜从天降
        public const int RunRace = 22;      //奔跑比赛
        public const int Demon = 23;        //恶魔活动
        public const int PetMing = 24;      //宠物矿场
        public const int SeasonTower = 25;  //赛季之塔
        public const int OneChallenge = 26;    //1v1挑战
    }

    public static class SceneSubTypeEnum
    {
        public const int LocalDungeon_1 = 1;      //本地副本[喜从天降]
    }
}
