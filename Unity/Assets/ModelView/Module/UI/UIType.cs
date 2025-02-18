using System;
using System.Collections.Generic;

namespace ET
{

	public class UIType
	{ 
		public const string Root = "Root";
		public const string UIRegister = "Login/UIRegister";
		public const string UILogin = "Login/UILogin";
        public const string UIYinSi = "Login/UIYinSi";
        public const string UIRelink = "Login/UIRelink";
		public const string UIPhoneCode = "Login/UIPhoneCode";
		public const string UILobby = "CreateRole/UILobby";
		public const string UICreateRole = "CreateRole/UICreateRole";
		public const string UIMain = "Main/Main/UIMain";
		public const string UIActivityV1 = "Main/ActivityV1/UIActivityV1";
		public const string UISelectReward = "Main/Main/UISelectReward";
		public const string UIOpenChest = "Main/Main/UIOpenChest";
		public const string UIGuideEquip = "Main/Main/UIGuideEquip";
		public const string UIRole = "Main/Role/UIRole";
		public const string UIProLucklyExplain = "Main/Role/UIProLucklyExplain";
		public const string UIOneSellSet = "Main/Role/UIOneSellSet";
        public const string UIRoleQiangHuaTip = "Main/Role/UIRoleQiangHuaTip";
        public const string UIZhuaPu = "Main/Main/UIZhuaPu";
		public const string UIRoleZodiac = "Main/Role/UIRoleZodiac";
		public const string UIRoleBagSplit = "Main/Role/UIRoleBagSplit";
		public const string UITreasureOpen = "Main/Role/UITreasureOpen";
		public const string UIPropertyHint = "Main/Role/UIPropertyHint";
		public const string UITask = "Main/Task/UITask";
		public const string UITaskGet = "Main/Task/UITaskGet";
		public const string UIGivePet = "Main/Task/UIGivePet";
		public const string UIGiveTask = "Main/Task/UIGiveTask";
		public const string UIStorySpeak = "Main/RoleStory/UIStorySpeak";
		public const string UIPopupview = "Common/UI_CommonHint";
		public const string UI_CommonHint_2 = "Common/UI_CommonHint_2";
        public const string UIItemTips = "Main/ItemTips/UIItemTips";
		public const string UIItemSellTip = "Main/ItemTips/UIItemSellTip";
        public const string UIItemExpBox = "Main/ItemTips/UIItemExpBox";
        public const string UIItemChangeOcc = "Main/ItemTips/UIItemChangeOcc";
        public const string UIItemBatchUse = "Main/ItemTips/UIItemBatchUse";
		public const string UIItemFumoSelect = "Main/ItemTips/UIItemFumoSelect";
		public const string UIEquipDuiBiTips = "Main/ItemTips/UIEquipDuiBiTips";
		public const string UICommonHuoBiSet = "Common/UICommonHuoBiSet";
		public const string UINotice = "Login/UINotice";
		public const string UISelectServer = "Login/UISelectServer";
		public const string UIServerShow = "Login/UIServerShow";
        public const string UILoading = "Common/UILoading";
		public const string UIPet = "Main/Pet/UIPet";
		public const string UIPetHeChengExplain = "Main/Pet/UIPetHeChengExplain";
		public const string UIPetHeChengPreview = "Main/Pet/UIPetHeChengPreview";
		public const string UIPetXiLianLockSkill = "Main/Pet/UIPetXiLianLockSkill";
        public const string UIPetCangKu = "Main/Pet/UIPetCangKu";
        public const string UIPetXianji = "Main/Pet/UIPetXianji";
		public const string UIPetChouKaGet = "Main/PetEgg/UIPetChouKaGet";
		public const string UIRolePetBag = "Main/PetEgg/UIRolePetBag";
		public const string UIPetEggFuLing = "Main/PetEgg/UIPetEggFuLing";
		public const string UIPetEggLucklyExplain = "Main/PetEgg/UIPetEggLucklyExplain";
		public const string UIPetEggChouKaProbExplain = "Main/PetEgg/UIPetEggChouKaProbExplain";
		public const string UIPetHeXinChouKaProbExplain = "Main/PetEgg/UIPetHeXinChouKaProbExplain";
		public const string UIPetSelect = "Main/Pet/UIPetSelect";
        public const string UIPetQuickFight = "Main/Pet/UIPetQuickFight";
        public const string UIShenShouJiBan = "Main/Pet/UIShenShouJiBan";
        public const string UIPetHeXinSuit = "Main/Pet/UIPetHeXinSuit";
        public const string UISkill = "Main/Skill/UISkill";
		public const string UISkillTips = "Main/Skill/UISkillTips";
		public const string UIBuffTips = "Main/Skill/UIBuffTips";
		public const string UICellChapterSelect = "CellDungeon/UICellChapterSelect";
		public const string UICellDungeonCell = "CellDungeon/UICellDungeonCell";
		public const string UICellDungeonSelect = "CellDungeon/UICellDungeonSelect";
		public const string UICellDungeonRevive = "CellDungeon/UICellDungeonRevive";
		public const string UICellDungeonSettlement = "CellDungeon/UICellDungeonSettlement";
		public const string UIDungeon = "Dungeon/UIDungeon";
		public const string UIEnterMapHint = "Dungeon/UIEnterMapHint";
		public const string UIDungeonLevel = "Dungeon/UIDungeonLevel";
		public const string UIDungeonHappyMain = "Dungeon/UIDungeonHappyMain";
		public const string UIDungeonMapTransfer = "Dungeon/UIDungeonMapTransfer";
        public const string UIMapBig = "Main/MiniMap/UIMapBig";
		public const string UIOccTwo = "Main/OccTwo/UIOccTwo";
		public const string UIOccTwoShow = "Main/OccTwo/UIOccTwoShow";
		public const string UIStore = "Main/Store/UIStore";
		public const string UIMystery = "Main/Mystery/UIMystery";
		public const string UIChat = "Main/Chat/UIChat";
		public const string UISetting = "Main/Setting/UISetting";
        public const string UISettingPool = "Main/Setting/UISettingPool";
        public const string UISettingFrame = "Main/Setting/UISettingFrame";
        public const string UISettingSkill = "Main/Setting/UISettingSkill";
        public const string UIWarehouse = "Main/Role/UIWarehouse";
        public const string UIWarehouseAccount = "Main/Role/UIWarehouseAccount";
		public const string UIMail = "Main/Mail/UIMail";
		public const string UIQueue = "Login/UIQueue";
		public const string UIRealName = "Main/RealName/UIRealName";
		public const string UIRecharge = "Main/Recharge/UIRecharge";
		public const string UIRechargeReward = "Main/Recharge/UIRechargeReward";
        public const string UIChengJiu = "Main/ChengJiu/UIChengJiu";
		public const string UISeason = "Main/Season/UISeason";
		public const string UISeasonMain = "Main/Season/UISeasonMain";
        public const string UISeasonLordDetail = "Main/Season/UISeasonLordDetail";
		public const string UISeasonJingHeZhuru = "Main/Season/UISeasonJingHeZhuru";
		public const string UISeasonTower = "Main/Season/UISeasonTower";
		public const string UISeasonTowerReward = "Main/Season/UISeasonTowerReward";
		public const string UIJingLingGet = "Main/ChengJiu/UIJingLingGet";
		public const string UIChengJiuActivite = "Main/ChengJiu/UIChengJiuActivite";
		public const string UISpirit = "Main/ChengJiu/UISpirit";
		public const string UIChouKa = "Main/ChouKa/UIChouKa";
		public const string UIChouKaReward = "Main/ChouKa/UIChouKaReward";
		public const string UIChouKaProbExplain = "Main/ChouKa/UIChouKaProbExplain";
		public const string UIChouKaWarehouse = "Main/ChouKa/UIChouKaWarehouse";
		public const string UICommonReward = "Main/Common/UICommonReward";
		public const string UICommonProperty = "Main/Common/UICommonProperty";
		public const string UICountry = "Main/Country/UICountry";
		public const string UICountryTips = "Main/Country/UICountryTips";
		public const string UICountryHuoDongJieShao = "Main/Country/UICountryHuoDongJieShao";
		public const string UIMakeLearn = "Main/MakeLearn/UIMakeLearn";
		public const string UIRank = "Main/Rank/UIRank";
		public const string UIRankReward = "Main/Rank/UIRankReward";
		public const string UIPaiMai = "Main/PaiMai/UIPaiMai";
		public const string UIPaiMaiBuyTip = "Main/PaiMai/UIPaiMaiBuyTip";
		public const string UIPaiMaiStall = "Main/PaiMai/UIPaiMaiStall";
		public const string UIAuctionRecode = "Main/PaiMai/UIAuctionRecord";
		public const string UIPaiMaiAuction = "Main/PaiMai/UIPaiMaiAuction";
		public const string UIPaiMaiStallBuy = "Main/PaiMai/UIPaiMaiStallBuy";
		public const string UIPaiMaiSellPrice = "Main/PaiMai/UIPaiMaiSellPrice";
		public const string UIStallSellPrice = "Main/PaiMai/UIStallSellPrice";
		public const string UIWeiJingShop = "Main/PaiMai/UIWeiJingShop";
        public const string UIEnergy = "Main/Energy/UIEnergy";
		public const string UIHorseNotice = "Main/HorseNotice/UIHorseNotice";
		public const string UIGM = "Main/GM/UIGM";
		public const string UICommand = "Main/GM/UICommand";
		public const string UIWatch = "Main/Watch/UIWatch";
		public const string UIPetInfo = "Main/Watch/UIPetInfo";
        public const string UIWatchMenu = "Main/Watch/UIWatchMenu";
		public const string UIWatchPaiMai = "Main/Watch/UIWatchPaiMai";
		public const string UIActivity = "Main/Activity/UIActivity";
		public const string UIZhanQu = "Main/ZhanQu/UIZhanQu";
		public const string UITeam = "Main/Team/UITeam";
		public const string UITeamMain = "TeamDungeon/UITeamMain";
		public const string UITeamDungeon = "TeamDungeon/UITeamDungeon";
		public const string UITeamApplyList = "Main/Team/UITeamApplyList";
		public const string UITeamDungeonCreate = "TeamDungeon/UITeamDungeonCreate";
		public const string UITeamDungeonPrepare = "TeamDungeon/UITeamDungeonPrepare";
		public const string UITeamDungeonSettlement = "TeamDungeon/UITeamDungeonSettlement";
		public const string UIFriend = "Main/Friend/UIFriend";
		public const string UIUnionXiuLian = "Main/Union/UIUnionXiuLian";
		public const string UIUnionKeJi = "Main/Union/UIUnionKeJi";
		public const string UIUnionDonation = "Main/Union/UIUnionDonation";
		public const string UIUnionApplyList = "Main/Union/UIUnionApplyList";
		public const string UIUnionDonationRecord = "Main/Union/UIUnionDonationRecord";
		public const string UIUnionMystery = "Main/Union/UIUnionMystery";
		public const string UIUnionJingXuan = "Main/Union/UIUnionJingXuan";
		public const string UIUnionRecords = "Main/Union/UIUnionRecords";
        public const string UIGuide = "Main/Guide/UIGuide";
		public const string UIShouJi = "Main/ShouJi/UIShouJi";
		public const string UIShouJiSelect = "Main/ShouJi/UIShouJiSelect";
		public const string UIRoleXiLian = "Main/RoleXiLian/UIRoleXiLian";
		public const string UIRoleXiLianNumReward = "Main/RoleXiLian/UIRoleXiLianNumReward";
		public const string UIRoleXiLianExplain = "Main/RoleXiLian/UIRoleXiLianExplain";
		public const string UIRoleXiLianTen = "Main/RoleXiLian/UIRoleXiLianTen";
        public const string UIEquipmentIncrease = "Main/EquipmentIncrease/UIEquipmentIncrease";
		public const string UILingDi = "Main/LingDi/UILingDi";
		public const string UILingDiReward = "Main/LingDi/UILingDiReward";
		public const string UITuZhiMake = "Main/TuZhiMake/UITuZhiMake";
		public const string UIXiuLian = "Main/XiuLian/UIXiuLian";
		public const string UIPetEgg = "Main/PetEgg/UIPetEgg";
		public const string UIPetEggChouKaReward = "Main/PetEgg/UIPetEggChouKaReward";
		public const string UIPetHeXinChouKaReward = "Main/PetEgg/UIPetHeXinChouKaReward";
		public const string UIPetFubenResult = "PetFuben/UIPetFubenResult";
		public const string UIPetMain = "Main/PetSet/UIPetMain";
		public const string UIPetMiningTeam = "Main/PetSet/UIPetMiningTeam";
		public const string UIPetMiningRecord = "Main/PetSet/UIPetMiningRecord";
        public const string UIPetMiningReward = "Main/PetSet/UIPetMiningReward";
        public const string UIPetMiningChallenge = "Main/PetSet/UIPetMiningChallenge";
        public const string UIHongBao = "Main/HongBao/UIHongBao";
		public const string UIRandomTower = "Main/RandomTower/UIRandomTower";
		public const string UIRandomOpen = "Main/RandomTower/UIRandomOpen";
		public const string UIRandomTowerResult = "Main/RandomTower/UIRandomTowerResult";
		public const string UIPetHeXinHeCheng = "Main/Pet/UIPetHeXinHeCheng";
        public const string UIPetHeXinBag = "Main/Pet/UIPetHeXinBag";
        public const string UICamp = "Main/Camp/UICamp";
		public const string UIWorldLv = "Main/WorldLv/UIWorldLv";
		public const string UIAppraisalSelect = "Main/Appraisal/UIAppraisalSelect";
		public const string UIGemMake = "Main/Make/UIGemMake";
		public const string UIShenQiMake = "Main/Make/UIShenQiMake";
        public const string UIPetSet = "Main/PetSet/UIPetSet";
		public const string UIPetFormation = "Main/PetSet/UIPetFormation";
		public const string UIPetMiningFormation = "Main/PetSet/UIPetMiningFormation";
        public const string UIBattle = "BattleDungeon/UIBattle";
		public const string UIBattleMain = "BattleDungeon/UIBattleMain";
		public const string UIBattleRecruit = "BattleDungeon/UIBattleRecruit";
		public const string UITrial = "TrialDungeon/UITrial";
		public const string UITrialMain = "TrialDungeon/UITrialMain";
		public const string UITrialReward = "TrialDungeon/UITrialReward";
        public const string UITowerOfSeal = "TowerOfSeal/UITowerOfSeal";
		public const string UITowerOfSealMain = "TowerOfSeal/UITowerOfSealMain";
		public const string UITowerOfSealCost = "TowerOfSeal/UITowerOfSealCost";
		public const string UITowerOfSealJump = "TowerOfSeal/UITowerOfSealJump";
		public const string UIZuoQi = "ZuoQi/UIZuoQi";
		public const string UITower = "TowerDungeon/UITower";
		public const string UITowerOpen = "TowerDungeon/UITowerOpen";
		public const string UITowerFightReward = "TowerDungeon/UITowerFightReward";
		public const string UIMiJingMain = "MiJing/UIMiJingMain";
		public const string UIFenXiang = "Main/FenXiang/UIFenXiang";
		public const string UINewYear = "Main/NewYear/UINewYear";
		public const string UIDeleteAccount = "Login/UIDeleteAccount";
		public const string UIArenaMain = "Arena/UIArenaMain";
		public const string UIJiaYuanBag = "JiaYuan/UIJiaYuanBag";
		public const string UIJiaYuanPet = "JiaYuan/UIJiaYuanPet";
		public const string UIJiaYuanMenu = "JiaYuan/UIJiaYuanMenu";
		public const string UIJiaYuanUpLv = "JiaYuan/UIJiaYuanUpLv";
		public const string UIJiaYuanMain = "JiaYuan/UIJiaYuanMain";
		public const string UIJiaYuanFood = "JiaYuan/UIJiaYuanFood";
		public const string UIJiaYuanDaShi = "JiaYuan/UIJiaYuanDaShi";
		public const string UIJiaYuanRecord = "JiaYuan/UIJiaYuanRecord";
		public const string UIJiaYuanPetFeed = "JiaYuan/UIJiaYuanPetFeed";
		public const string UIJiaYuanMystery = "JiaYuan/UIJiaYuanMystery";
		public const string UIJiaYuanPasture = "JiaYuan/UIJiaYuanPasture";
		public const string UIJiaYuanPlanWatch = "JiaYuan/UIJiaYuanPlanWatch";
		public const string UIJiaYuanWarehouse = "JiaYuan/UIJiaYuanWarehouse";
		public const string UIJiaYuanTreasureMapStorage = "JiaYuan/UIJiaYuanTreasureMapStorage";
		public const string UIJiaYuanOneKeyPlant = "JiaYuan/UIJiaYuanOneKeyPlant";
		public const string UIDonation = "Main/Donation/UIDonation";
		public const string UISolo = "Solo/UISolo";
		public const string UISoloReward = "Solo/UISoloReward";
		public const string UIHunt = "Hunt/UIHunt";
		public const string UIProtect = "Main/Protect/UIProtect";
		public const string UIWearWeapon = "Main/Role/UIWearWeapon";
		public const string UIFashion = "Main/Fashion/UIFashion";
		public const string UIFashionExplain = "Main/Fashion/UIFashionExplain";
        public const string UITurtle = "Main/Turtle/UITurtle";
		public const string UIJueXing = "Main/JueXing/UIJueXing";
        public const string UIHappyMain = "Happy/UIHappyMain";
		public const string UIRunRaceMain = "RunRace/UIRunRaceMain";
		public const string UIRunRace = "RunRace/UIRunRace";
        public const string UIDemonMain = "Demon/UIDemonMain";
        public const string UIDemon = "Demon/UIDemon";
		public const string UIWelfare = "Main/Welfare/UIWelfare";


        public static Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
		{
			{ "Root", Root},
			{ "UIMain", UIMain},
			{ "UIRole", UIRole},
			{ "UITask", UITask},
			{ "UINpcTask", UITaskGet},
			{ "UIOccTwo", UIOccTwo},
			{ "UIStore", UIStore},
			{ "UIMail", UIMail},
			{ "UIMystery", UIMystery},
			{ "UIMakeLearn", UIMakeLearn},
			{ "UIWarehouse", UIWarehouse},
			{ "UIPaiMai", UIPaiMai},
			{ "UIShouJi", UIShouJi},
			{ "UIRoleXiLian", UIRoleXiLian},
			{ "UIEquipmentIncrease", UIEquipmentIncrease},
			{ "UIRank", UIRank},
			{ "UILingDi", UILingDi},
			{ "UILingDiReward", UILingDiReward},
			{ "UIXiuLian", UIXiuLian},
			{ "UIPetEgg", UIPetEgg},
			{ "UIChouKa", UIChouKa},
			{ "UIGemMake", UIGemMake},
			{ "UITowerDungeon", UITower},
			{ "UITrial", UITrial},
			{ "UITowerOfSeal", UITowerOfSeal},
			{ "UIShenQiMake", UIShenQiMake},
			{ "UIZuoQi", UIZuoQi},
			{ "UIJiaYuanPet", UIJiaYuanPet },
			{ "UIJiaYuanUpLv", UIJiaYuanUpLv},
			{ "UIJiaYuanFood", UIJiaYuanFood },
			{ "UIJiaYuanDaShi", UIJiaYuanDaShi },
			{ "UIJiaYuanRecord", UIJiaYuanRecord },
			{ "UIJiaYuanPasture", UIJiaYuanPasture },
			{ "UIUIJiaYuanMystery", UIJiaYuanMystery},
			{ "UIJiaYuanWarehouse", UIJiaYuanWarehouse },
			{ "UIJiaYuanTreasureMapStorage", UIJiaYuanTreasureMapStorage},
			{ "UIBattleRecruit", UIBattleRecruit}
		};
	}
}