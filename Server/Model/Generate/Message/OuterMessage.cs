using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
//IRequest   IResponse   请求返回配合使用   直连网关服的
//IActorLocationRequest  IActorLocationResponse   切场景请求   请求返回配合使用  需要网关服转换的
//IActorMessage           服务器主动发送给前端不需要返回值
	[ResponseType(nameof(A2C_Register))]
	[Message(OuterOpcode.C2A_Register)]
	[ProtoContract]
	public partial class C2A_Register: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.A2C_Register)]
	[ProtoContract]
	public partial class A2C_Register: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Center2C_Register))]
	[Message(OuterOpcode.C2Center_Register)]
	[ProtoContract]
	public partial class C2Center_Register: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.Center2C_Register)]
	[ProtoContract]
	public partial class Center2C_Register: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2C_RealNameResponse))]
	[Message(OuterOpcode.C2A_RealNameRequest)]
	[ProtoContract]
	public partial class C2A_RealNameRequest: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Name { get; set; }

		[ProtoMember(2)]
		public string IdCardNO { get; set; }

		[ProtoMember(3)]
		public int AiType { get; set; }

		[ProtoMember(4)]
		public long AccountId { get; set; }

	}

	[Message(OuterOpcode.A2C_RealNameResponse)]
	[ProtoContract]
	public partial class A2C_RealNameResponse: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int ErrorCode { get; set; }

	}

	[ResponseType(nameof(M2C_TestResponse))]
	[Message(OuterOpcode.C2M_TestRequest)]
	[ProtoContract]
	public partial class C2M_TestRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string request { get; set; }

	}

	[Message(OuterOpcode.M2C_TestResponse)]
	[ProtoContract]
	public partial class M2C_TestResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string response { get; set; }

	}

	[ResponseType(nameof(Actor_TransferResponse))]
	[Message(OuterOpcode.Actor_TransferRequest)]
	[ProtoContract]
	public partial class Actor_TransferRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int SceneType { get; set; }

		[ProtoMember(2)]
		public int SceneId { get; set; }

		[ProtoMember(5)]
		public int Difficulty { get; set; }

		[ProtoMember(6)]
		public string paramInfo { get; set; }

	}

	[Message(OuterOpcode.Actor_TransferResponse)]
	[ProtoContract]
	public partial class Actor_TransferResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2C_CreateRoleData))]
	[Message(OuterOpcode.C2A_CreateRoleData)]
	[ProtoContract]
	public partial class C2A_CreateRoleData: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int CreateOcc { get; set; }

		[ProtoMember(3)]
		public string CreateName { get; set; }

		[ProtoMember(4)]
		public long AccountId { get; set; }

	}

	[Message(OuterOpcode.A2C_CreateRoleData)]
	[ProtoContract]
	public partial class A2C_CreateRoleData: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public CreateRoleInfo createRoleInfo { get; set; }

		[ProtoMember(13)]
		public int TodayCreateRole { get; set; }

	}

	[ResponseType(nameof(A2C_DeleteRoleData))]
	[Message(OuterOpcode.C2A_DeleteRoleData)]
	[ProtoContract]
	public partial class C2A_DeleteRoleData: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int DeleXuhaoID { get; set; }

		[ProtoMember(3)]
		public long DeleUserID { get; set; }

		[ProtoMember(4)]
		public long AccountId { get; set; }

	}

	[Message(OuterOpcode.A2C_DeleteRoleData)]
	[ProtoContract]
	public partial class A2C_DeleteRoleData: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Q2C_EnterQueue))]
	[Message(OuterOpcode.C2Q_EnterQueue)]
	[ProtoContract]
	public partial class C2Q_EnterQueue: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Token { get; set; }

		[ProtoMember(3)]
		public long AccountId { get; set; }

	}

	[Message(OuterOpcode.Q2C_EnterQueue)]
	[ProtoContract]
	public partial class Q2C_EnterQueue: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.Q2C_EnterGame)]
	[ProtoContract]
	public partial class Q2C_EnterGame: Object, IMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public string Token { get; set; }

	}

	[ResponseType(nameof(A2C_GetRealmKey))]
	[Message(OuterOpcode.C2A_GetRealmKey)]
	[ProtoContract]
	public partial class C2A_GetRealmKey: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Token { get; set; }

		[ProtoMember(2)]
		public int ServerId { get; set; }

		[ProtoMember(3)]
		public long AccountId { get; set; }

	}

	[Message(OuterOpcode.A2C_GetRealmKey)]
	[ProtoContract]
	public partial class A2C_GetRealmKey: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string RealmKey { get; set; }

		[ProtoMember(2)]
		public string RealmAddress { get; set; }

	}

	[ResponseType(nameof(R2C_LoginRealm))]
	[Message(OuterOpcode.C2R_LoginRealm)]
	[ProtoContract]
	public partial class C2R_LoginRealm: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string RealmTokenKey { get; set; }

	}

	[Message(OuterOpcode.R2C_LoginRealm)]
	[ProtoContract]
	public partial class R2C_LoginRealm: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string GateSessionKey { get; set; }

		[ProtoMember(2)]
		public string GateAddress { get; set; }

	}

	[ResponseType(nameof(G2C_LoginGameGate))]
	[Message(OuterOpcode.C2G_LoginGameGate)]
	[ProtoContract]
	public partial class C2G_LoginGameGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Key { get; set; }

		[ProtoMember(2)]
		public long RoleId { get; set; }

		[ProtoMember(3)]
		public long Account { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginGameGate)]
	[ProtoContract]
	public partial class G2C_LoginGameGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

// 自己的unit id
		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[ResponseType(nameof(G2C_EnterGame))]
	[Message(OuterOpcode.C2G_EnterGame)]
	[ProtoContract]
	public partial class C2G_EnterGame: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MapId { get; set; }

		[ProtoMember(2)]
		public long UserID { get; set; }

		[ProtoMember(3)]
		public long AccountId { get; set; }

		[ProtoMember(4)]
		public bool Relink { get; set; }

		[ProtoMember(5)]
		public string DeviceName { get; set; }

		[ProtoMember(6)]
		public int Version { get; set; }

		[ProtoMember(7)]
		public int Platform { get; set; }

		[ProtoMember(8)]
		public int Simulator { get; set; }

		[ProtoMember(9)]
		public int Root { get; set; }

		[ProtoMember(10)]
		public int IsRecharge { get; set; }

		[ProtoMember(11)]
		public string DeviceID { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterGame)]
	[ProtoContract]
	public partial class G2C_EnterGame: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

// 自己的unit id
		[ProtoMember(1)]
		public long MyId { get; set; }

		[ProtoMember(2)]
		public int IsPopUp { get; set; }

		[ProtoMember(3)]
		public string PopUpInfo { get; set; }

		[ProtoMember(4)]
		public long AccInfoID { get; set; }

	}

	[Message(OuterOpcode.MoveInfo)]
	[ProtoContract]
	public partial class MoveInfo: Object
	{
		[ProtoMember(1)]
		public List<float> X = new List<float>();

		[ProtoMember(2)]
		public List<float> Y = new List<float>();

		[ProtoMember(3)]
		public List<float> Z = new List<float>();

		[ProtoMember(4)]
		public float A { get; set; }

		[ProtoMember(5)]
		public float B { get; set; }

		[ProtoMember(6)]
		public float C { get; set; }

		[ProtoMember(7)]
		public float W { get; set; }

		[ProtoMember(8)]
		public int TurnSpeed { get; set; }

	}

//等级 经验 货币 或者不变的数值都放在这。
	[Message(OuterOpcode.UserInfo)]
	[ProtoContract]
	public partial class UserInfo: Object
	{
		[ProtoMember(1)]
		public long AccInfoID { get; set; }

		[ProtoMember(2)]
		public string Name { get; set; }

		[ProtoMember(3)]
		public long Gold { get; set; }

//钻石
		[ProtoMember(4)]
		public long Diamond { get; set; }

// 等级
		[ProtoMember(5)]
		public int Lv { get; set; }

// 经验
		[ProtoMember(6)]
		public long Exp { get; set; }

// 疲劳
		[ProtoMember(7)]
		public long PiLao { get; set; }

//职业
		[ProtoMember(8)]
		public int Occ { get; set; }

//职业
		[ProtoMember(9)]
		public int OccTwo { get; set; }

		[ProtoMember(10)]
		public int Combat { get; set; }

		[ProtoMember(11)]
		public int RobotId { get; set; }

		[ProtoMember(13)]
		public int Sp { get; set; }

		[ProtoMember(14)]
		public int Vitality { get; set; }

		[ProtoMember(16)]
		public long RongYu { get; set; }

		[ProtoMember(17)]
		public string UnionName { get; set; }

		[ProtoMember(18)]
		public long UserId { get; set; }

		[ProtoMember(19)]
		public List<KeyValuePair> GameSettingInfos = new List<KeyValuePair>();

		[ProtoMember(20)]
		public List<int> MakeList = new List<int>();

		[ProtoMember(21)]
		public List<int> CompleteGuideIds = new List<int>();

		[ProtoMember(22)]
		public List<KeyValuePairInt> DayFubenTimes = new List<KeyValuePairInt>();

		[ProtoMember(23)]
		public List<KeyValuePair> MonsterRevives = new List<KeyValuePair>();

		[ProtoMember(24)]
		public List<int> TowerRewardIds = new List<int>();

		[ProtoMember(25)]
		public List<int> ChouKaRewardIds = new List<int>();

		[ProtoMember(26)]
		public List<int> XiuLianRewardIds = new List<int>();

//购买过的神秘商品
		[ProtoMember(27)]
		public List<KeyValuePairInt> MysteryItems = new List<KeyValuePairInt>();

//已开启的宝箱记录
		[ProtoMember(28)]
		public List<KeyValuePair> OpenChestList = new List<KeyValuePair>();

		[ProtoMember(29)]
		public List<KeyValuePairInt> MakeIdList = new List<KeyValuePairInt>();

//已通关的副本列表
		[ProtoMember(30)]
		public List<FubenPassInfo> FubenPassList = new List<FubenPassInfo>();

//每日道具使用限制
		[ProtoMember(31)]
		public List<KeyValuePairInt> DayItemUse = new List<KeyValuePairInt>();

		[ProtoMember(32)]
		public List<int> HorseIds = new List<int>();

//剧情副本每日刷新 global79
		[ProtoMember(33)]
		public List<KeyValuePairInt> DayMonsters = new List<KeyValuePairInt>();

//随机精灵每日刷新 global80
		[ProtoMember(34)]
		public List<int> DayJingLing = new List<int>();

		[ProtoMember(35)]
		public long JiaYuanFund { get; set; }

		[ProtoMember(36)]
		public long JiaYuanExp { get; set; }

		[ProtoMember(37)]
		public int JiaYuanLv { get; set; }

		[ProtoMember(38)]
		public int BaoShiDu { get; set; }

		[ProtoMember(39)]
		public List<KeyValuePair> FirstWinSelf = new List<KeyValuePair>();

		[ProtoMember(40)]
		public long UnionZiJin { get; set; }

		[ProtoMember(41)]
		public int ServerMailIdCur { get; set; }

		[ProtoMember(42)]
		public List<int> DiamondGetWay = new List<int>();

		[ProtoMember(43)]
		public string DemonName { get; set; }

		[ProtoMember(44)]
		public List<int> PetMingRewards = new List<int>();

		[ProtoMember(45)]
		public List<int> OpenJingHeIds = new List<int>();

		[ProtoMember(46)]
		public int SeasonLevel { get; set; }

		[ProtoMember(47)]
		public int SeasonExp { get; set; }

		[ProtoMember(48)]
		public long SeasonCoin { get; set; }

		[ProtoMember(49)]
		public List<int> WelfareTaskRewards = new List<int>();

		[ProtoMember(50)]
		public long CreateTime { get; set; }

		[ProtoMember(51)]
		public List<int> WelfareInvestList = new List<int>();

		[ProtoMember(52)]
		public List<int> RechargeReward = new List<int>();

		[ProtoMember(53)]
		public List<int> UnionKeJiList = new List<int>();

		[ProtoMember(54)]
		public List<int> PetExploreRewardIds = new List<int>();

		[ProtoMember(55)]
		public List<int> PetHeXinExploreRewardIds = new List<int>();

		[ProtoMember(56)]
		public string StallName { get; set; }

		[ProtoMember(57)]
		public List<int> SingleRechargeIds = new List<int>();

		[ProtoMember(58)]
		public List<int> SingleRewardIds = new List<int>();

		[ProtoMember(59)]
		public List<int> ItemXiLianNumRewardIds = new List<int>();

		[ProtoMember(60)]
		public List<int> DefeatedBossIds = new List<int>();

		[ProtoMember(61)]
		public List<int> GoldGetWay = new List<int>();

		[ProtoMember(62)]
		public List<int> ExpGetWay = new List<int>();

		[ProtoMember(63)]
		public long WeiJingGold { get; set; }

//购买过的神秘商品
		[ProtoMember(64)]
		public List<KeyValuePairInt> BuyStoreItems = new List<KeyValuePairInt>();

		[ProtoMember(65)]
		public List<int> OccTwoOld = new List<int>();

		[ProtoMember(66)]
		public List<int> SerialRewards = new List<int>();

//总共使用次数
		[ProtoMember(67)]
		public List<KeyValuePairInt> TotalUseTimes = new List<KeyValuePairInt>();

	}

	[Message(OuterOpcode.KeyValuePair)]
	[ProtoContract]
	public partial class KeyValuePair: Object
	{
		[ProtoMember(1)]
		public int KeyId { get; set; }

		[ProtoMember(2)]
		public string Value { get; set; }

		[ProtoMember(3)]
		public string Value2 { get; set; }

	}

	[Message(OuterOpcode.KeyValuePairInt)]
	[ProtoContract]
	public partial class KeyValuePairInt: Object
	{
		[ProtoMember(1)]
		public int KeyId { get; set; }

		[ProtoMember(2)]
		public long Value { get; set; }

	}

	[Message(OuterOpcode.KeyValuePairLong)]
	[ProtoContract]
	public partial class KeyValuePairLong: Object
	{
		[ProtoMember(1)]
		public long KeyId { get; set; }

		[ProtoMember(2)]
		public long Value { get; set; }

		[ProtoMember(3)]
		public long Value2 { get; set; }

	}

	[Message(OuterOpcode.CreateRoleInfo)]
	[ProtoContract]
	public partial class CreateRoleInfo: Object
	{
		[ProtoMember(2)]
		public long UserID { get; set; }

		[ProtoMember(4)]
		public int PlayerLv { get; set; }

		[ProtoMember(5)]
		public int PlayerOcc { get; set; }

		[ProtoMember(6)]
		public int WeaponId { get; set; }

		[ProtoMember(7)]
		public string PlayerName { get; set; }

		[ProtoMember(8)]
		public int OccTwo { get; set; }

		[ProtoMember(9)]
		public int EquipIndex { get; set; }

		[ProtoMember(10)]
		public List<int> FashionIds = new List<int>();

	}

	[Message(OuterOpcode.UnitInfo)]
	[ProtoContract]
	public partial class UnitInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int UnitType { get; set; }

		[ProtoMember(3)]
		public int ConfigId { get; set; }

		[ProtoMember(4)]
		public int RoleCamp { get; set; }

		[ProtoMember(6)]
		public float X { get; set; }

		[ProtoMember(7)]
		public float Y { get; set; }

		[ProtoMember(8)]
		public float Z { get; set; }

		[ProtoMember(9)]
		public List<int> Ks = new List<int>();

		[ProtoMember(10)]
		public List<long> Vs = new List<long>();

		[ProtoMember(14)]
		public float ForwardX { get; set; }

		[ProtoMember(15)]
		public float ForwardY { get; set; }

		[ProtoMember(16)]
		public float ForwardZ { get; set; }

		[ProtoMember(17)]
		public MoveInfo MoveInfo { get; set; }

		[ProtoMember(19)]
		public List<KeyValuePair> Buffs = new List<KeyValuePair>();

		[ProtoMember(20)]
		public List<SkillInfo> Skills = new List<SkillInfo>();

		[ProtoMember(21)]
		public string UnitName { get; set; }

		[ProtoMember(22)]
		public string MasterName { get; set; }

		[ProtoMember(24)]
		public string UnionName { get; set; }

		[ProtoMember(25)]
		public string DemonName { get; set; }

		[ProtoMember(26)]
		public List<int> FashionEquipList = new List<int>();

	}

	[Message(OuterOpcode.M2C_CreateUnits)]
	[ProtoContract]
	public partial class M2C_CreateUnits: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public List<UnitInfo> Units = new List<UnitInfo>();

		[ProtoMember(3)]
		public List<SpilingInfo> Spilings = new List<SpilingInfo>();

		[ProtoMember(4)]
		public List<DropInfo> Drops = new List<DropInfo>();

		[ProtoMember(5)]
		public List<TransferInfo> Transfers = new List<TransferInfo>();

		[ProtoMember(8)]
		public int UpdateAll { get; set; }

	}

	[Message(OuterOpcode.M2C_CreateMyUnit)]
	[ProtoContract]
	public partial class M2C_CreateMyUnit: Object, IActorMessage
	{
		[ProtoMember(1)]
		public UnitInfo Unit { get; set; }

	}

	[Message(OuterOpcode.M2C_StartSceneChange)]
	[ProtoContract]
	public partial class M2C_StartSceneChange: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long SceneInstanceId { get; set; }

		[ProtoMember(2)]
		public int SceneType { get; set; }

		[ProtoMember(3)]
		public int ChapterId { get; set; }

		[ProtoMember(4)]
		public int Difficulty { get; set; }

		[ProtoMember(5)]
		public string ParamInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_RemoveUnits)]
	[ProtoContract]
	public partial class M2C_RemoveUnits: Object, IActorMessage
	{
		[ProtoMember(2)]
		public List<long> Units = new List<long>();

	}

	[Message(OuterOpcode.C2M_PathfindingRequest)]
	[ProtoContract]
	public partial class C2M_PathfindingRequest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public float X { get; set; }

		[ProtoMember(2)]
		public float Y { get; set; }

		[ProtoMember(3)]
		public float Z { get; set; }

		[ProtoMember(4)]
		public bool YaoGan { get; set; }

		[ProtoMember(5)]
		public long UnitId { get; set; }

		[ProtoMember(6)]
		public int Direction { get; set; }

		[ProtoMember(7)]
		public float Distance { get; set; }

	}

///客户端寻路...
	[Message(OuterOpcode.C2M_PathfindingResult)]
	[ProtoContract]
	public partial class C2M_PathfindingResult: Object, IActorLocationMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(5)]
		public List<float> Xs = new List<float>();

		[ProtoMember(6)]
		public List<float> Ys = new List<float>();

		[ProtoMember(7)]
		public List<float> Zs = new List<float>();

	}

	[Message(OuterOpcode.M2C_PathfindingResult)]
	[ProtoContract]
	public partial class M2C_PathfindingResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(4)]
		public bool YaoGan { get; set; }

		[ProtoMember(5)]
		public List<float> Xs = new List<float>();

		[ProtoMember(6)]
		public List<float> Ys = new List<float>();

		[ProtoMember(7)]
		public List<float> Zs = new List<float>();

		[ProtoMember(8)]
		public float X { get; set; }

		[ProtoMember(9)]
		public float Y { get; set; }

		[ProtoMember(10)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.C2M_StopResult)]
	[ProtoContract]
	public partial class C2M_StopResult: Object, IActorLocationMessage
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.M2C_StopResult)]
	[ProtoContract]
	public partial class M2C_StopResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public float A { get; set; }

		[ProtoMember(7)]
		public float B { get; set; }

		[ProtoMember(8)]
		public float C { get; set; }

		[ProtoMember(9)]
		public float W { get; set; }

	}

	[Message(OuterOpcode.C2M_Stop)]
	[ProtoContract]
	public partial class C2M_Stop: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.C2M_StopTest)]
	[ProtoContract]
	public partial class C2M_StopTest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_PathfindingRequest)]
	[ProtoContract]
	public partial class M2C_PathfindingRequest: Object, IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

		[ProtoMember(5)]
		public List<float> Xs = new List<float>();

		[ProtoMember(6)]
		public List<float> Ys = new List<float>();

		[ProtoMember(7)]
		public List<float> Zs = new List<float>();

	}

	[Message(OuterOpcode.M2C_PathfindingListRequest)]
	[ProtoContract]
	public partial class M2C_PathfindingListRequest: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<M2C_PathfindingRequest> PathList = new List<M2C_PathfindingRequest>();

	}

	[Message(OuterOpcode.M2C_Stop)]
	[ProtoContract]
	public partial class M2C_Stop: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public float X { get; set; }

		[ProtoMember(4)]
		public float Y { get; set; }

		[ProtoMember(5)]
		public float Z { get; set; }

		[ProtoMember(6)]
		public float A { get; set; }

		[ProtoMember(7)]
		public float B { get; set; }

		[ProtoMember(8)]
		public float C { get; set; }

		[ProtoMember(9)]
		public float W { get; set; }

	}

	[ResponseType(nameof(G2C_Ping))]
	[Message(OuterOpcode.C2G_Ping)]
	[ProtoContract]
	public partial class C2G_Ping: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_Ping)]
	[ProtoContract]
	public partial class G2C_Ping: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long Time { get; set; }

	}

	[Message(OuterOpcode.G2C_Test)]
	[ProtoContract]
	public partial class G2C_Test: Object, IMessage
	{
	}

	[ResponseType(nameof(G2C_Reload))]
	[Message(OuterOpcode.C2G_Reload)]
	[ProtoContract]
	public partial class C2G_Reload: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

		[ProtoMember(3)]
		public int LoadType { get; set; }

		[ProtoMember(4)]
		public string LoadValue { get; set; }

	}

	[Message(OuterOpcode.G2C_Reload)]
	[ProtoContract]
	public partial class G2C_Reload: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2C_LoginAccount))]
	[Message(OuterOpcode.C2A_LoginAccount)]
	[ProtoContract]
	public partial class C2A_LoginAccount: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string AccountName { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

		[ProtoMember(3)]
		public string Token { get; set; }

		[ProtoMember(4)]
		public string ThirdLogin { get; set; }

		[ProtoMember(5)]
		public bool Relink { get; set; }

		[ProtoMember(6)]
		public int age_type { get; set; }

		[ProtoMember(7)]
		public int Simulator { get; set; }

		[ProtoMember(11)]
		public string DeviceID { get; set; }

		[ProtoMember(12)]
		public string OAID { get; set; }

	}

	[Message(OuterOpcode.A2C_LoginAccount)]
	[ProtoContract]
	public partial class A2C_LoginAccount: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Token { get; set; }

		[ProtoMember(2)]
		public long AccountId { get; set; }

		[ProtoMember(6)]
		public int QueueNumber { get; set; }

		[ProtoMember(7)]
		public string QueueAddres { get; set; }

		[ProtoMember(8)]
		public PlayerInfo PlayerInfo { get; set; }

		[ProtoMember(9)]
		public List<CreateRoleInfo> RoleLists = new List<CreateRoleInfo>();

		[ProtoMember(10)]
		public string TaprepRequest { get; set; }

		[ProtoMember(13)]
		public int TodayCreateRole { get; set; }

	}

	[Message(OuterOpcode.A2C_Disconnect)]
	[ProtoContract]
	public partial class A2C_Disconnect: Object, IMessage
	{
		[ProtoMember(1)]
		public int Error { get; set; }

	}

	[Message(OuterOpcode.PlayerInfo)]
	[ProtoContract]
	public partial class PlayerInfo: Object
	{
		[ProtoMember(1)]
		public int RealName { get; set; }

		[ProtoMember(2)]
		public string Name { get; set; }

		[ProtoMember(3)]
		public string IdCardNo { get; set; }

		[ProtoMember(4)]
		public int RealNameReward { get; set; }

		[ProtoMember(5)]
		public List<RechargeInfo> RechargeInfos = new List<RechargeInfo>();

		[ProtoMember(6)]
		public List<KeyValuePair> DeleteUserList = new List<KeyValuePair>();

		[ProtoMember(7)]
		public List<int> BuChangZone = new List<int>();

		[ProtoMember(8)]
		public string PhoneNumber { get; set; }

		[ProtoMember(9)]
		public List<long> ShareTimes = new List<long>();

	}

	[ResponseType(nameof(M2C_TestActorResponse))]
	[Message(OuterOpcode.C2M_TestActorRequest)]
	[ProtoContract]
	public partial class C2M_TestActorRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[Message(OuterOpcode.M2C_TestActorResponse)]
	[ProtoContract]
	public partial class M2C_TestActorResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Info { get; set; }

	}

	[ResponseType(nameof(G2C_PlayerInfo))]
	[Message(OuterOpcode.C2G_PlayerInfo)]
	[ProtoContract]
	public partial class C2G_PlayerInfo: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_PlayerInfo)]
	[ProtoContract]
	public partial class G2C_PlayerInfo: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(3)]
		public List<string> TestRepeatedString = new List<string>();

		[ProtoMember(4)]
		public List<int> TestRepeatedInt32 = new List<int>();

		[ProtoMember(5)]
		public List<long> TestRepeatedInt64 = new List<long>();

	}

	[Message(OuterOpcode.C2M_Move)]
	[ProtoContract]
	public partial class C2M_Move: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long Id { get; set; }

		[ProtoMember(1)]
		public float X { get; set; }

		[ProtoMember(2)]
		public float Y { get; set; }

		[ProtoMember(3)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.M2C_MoveResult)]
	[ProtoContract]
	public partial class M2C_MoveResult: Object, IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

	}

	[ResponseType(nameof(G2C_HeartBeat))]
	[Message(OuterOpcode.C2G_HeartBeat)]
	[ProtoContract]
	public partial class C2G_HeartBeat: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.G2C_HeartBeat)]
	[ProtoContract]
	public partial class G2C_HeartBeat: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(C2R_TestCall))]
	[Message(OuterOpcode.C2R_TestCall)]
	[ProtoContract]
	public partial class C2R_TestCall: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.R2C_TestResponse)]
	[ProtoContract]
	public partial class R2C_TestResponse: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2C_Notice))]
	[Message(OuterOpcode.C2A_Notice)]
	[ProtoContract]
	public partial class C2A_Notice: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.A2C_Notice)]
	[ProtoContract]
	public partial class A2C_Notice: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(A2C_Thanks))]
	[Message(OuterOpcode.C2A_Thanks)]
	[ProtoContract]
	public partial class C2A_Thanks: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.A2C_Thanks)]
	[ProtoContract]
	public partial class A2C_Thanks: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[Message(OuterOpcode.ServerItem)]
	[ProtoContract]
	public partial class ServerItem: Object
	{
		[ProtoMember(1)]
		public int ServerId { get; set; }

		[ProtoMember(2)]
		public string ServerIp { get; set; }

		[ProtoMember(3)]
		public string ServerName { get; set; }

		[ProtoMember(4)]
		public long ServerOpenTime { get; set; }

		[ProtoMember(5)]
		public int Show { get; set; }

		[ProtoMember(6)]
		public int New { get; set; }

		[ProtoMember(7)]
		public List<int> PlatformList = new List<int>();

	}

	[ResponseType(nameof(A2C_ServerList))]
	[Message(OuterOpcode.C2A_ServerList)]
	[ProtoContract]
	public partial class C2A_ServerList: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public string Account { get; set; }

	}

	[Message(OuterOpcode.A2C_ServerList)]
	[ProtoContract]
	public partial class A2C_ServerList: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public List<ServerItem> ServerItems = new List<ServerItem>();

		[ProtoMember(3)]
		public string NoticeVersion { get; set; }

		[ProtoMember(4)]
		public string NoticeText { get; set; }

		[ProtoMember(5)]
		public string AccountCenterIp { get; set; }

		[ProtoMember(6)]
		public string RealAndGate { get; set; }

	}

	[ResponseType(nameof(G2C_CreateRole))]
	[Message(OuterOpcode.C2G_CreateRole)]
	[ProtoContract]
	public partial class C2G_CreateRole: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public string RoleName { get; set; }

	}

	[Message(OuterOpcode.G2C_CreateRole)]
	[ProtoContract]
	public partial class G2C_CreateRole: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string RoleName { get; set; }

		[ProtoMember(2)]
		public int RoleLv { get; set; }

		[ProtoMember(3)]
		public long RoleLvExp { get; set; }

		[ProtoMember(4)]
		public long Money { get; set; }

	}

	[Message(OuterOpcode.M2C_PetDataUpdate)]
	[ProtoContract]
	public partial class M2C_PetDataUpdate: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PetId { get; set; }

		[ProtoMember(2)]
		public int UpdateType { get; set; }

		[ProtoMember(3)]
		public string UpdateTypeValue { get; set; }

	}

	[Message(OuterOpcode.M2C_PetDataBroadcast)]
	[ProtoContract]
	public partial class M2C_PetDataBroadcast: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long PetId { get; set; }

		[ProtoMember(2)]
		public int UpdateType { get; set; }

		[ProtoMember(3)]
		public string UpdateTypeValue { get; set; }

		[ProtoMember(4)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.M2C_RoleDataUpdate)]
	[ProtoContract]
	public partial class M2C_RoleDataUpdate: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int UpdateType { get; set; }

		[ProtoMember(2)]
		public string UpdateTypeValue { get; set; }

		[ProtoMember(3)]
		public long UpdateValueLong { get; set; }

	}

	[Message(OuterOpcode.M2C_RoleDataBroadcast)]
	[ProtoContract]
	public partial class M2C_RoleDataBroadcast: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int UpdateType { get; set; }

		[ProtoMember(2)]
		public string UpdateTypeValue { get; set; }

		[ProtoMember(3)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.BagInfo)]
	[ProtoContract]
	public partial class BagInfo: Object
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Name { get; set; }

		[ProtoMember(1)]
		public long BagInfoID { get; set; }

		[ProtoMember(2)]
		public int ItemID { get; set; }

		[ProtoMember(3)]
		public int ItemNum { get; set; }

		[ProtoMember(4)]
		public string ItemPar { get; set; }

		[ProtoMember(5)]
		public int HideID { get; set; }

		[ProtoMember(6)]
		public string GemHole { get; set; }

		[ProtoMember(8)]
		public int Loc { get; set; }

		[ProtoMember(9)]
		public bool IfJianDing { get; set; }

		[ProtoMember(10)]
		public List<HideProList> HideProLists = new List<HideProList>();

		[ProtoMember(11)]
		public List<HideProList> XiLianHideProLists = new List<HideProList>();

		[ProtoMember(12)]
		public List<int> HideSkillLists = new List<int>();

		[ProtoMember(13)]
		public bool isBinging { get; set; }

		[ProtoMember(14)]
		public List<HideProList> XiLianHideTeShuProLists = new List<HideProList>();

		[ProtoMember(16)]
		public string GetWay { get; set; }

		[ProtoMember(17)]
		public string GemIDNew { get; set; }

		[ProtoMember(18)]
		public string MakePlayer { get; set; }

		[ProtoMember(20)]
		public List<HideProList> FumoProLists = new List<HideProList>();

		[ProtoMember(21)]
		public int InheritTimes { get; set; }

		[ProtoMember(22)]
		public List<int> InheritSkills = new List<int>();

		[ProtoMember(23)]
		public bool IsProtect { get; set; }

		[ProtoMember(24)]
		public List<HideProList> IncreaseProLists = new List<HideProList>();

		[ProtoMember(25)]
		public List<int> IncreaseSkillLists = new List<int>();

		[ProtoMember(26)]
		public int EquipPlan { get; set; }

		[ProtoMember(27)]
		public int EquipIndex { get; set; }

		[ProtoMember(28)]
		public int FuLing { get; set; }

	}

	[Message(OuterOpcode.HideProList)]
	[ProtoContract]
	public partial class HideProList: Object
	{
		[ProtoMember(1)]
		public int HideID { get; set; }

		[ProtoMember(2)]
		public long HideValue { get; set; }

	}

	[ResponseType(nameof(M2C_GetHeroDataResponse))]
	[Message(OuterOpcode.C2M_GetHeroDataRequest)]
	[ProtoContract]
	public partial class C2M_GetHeroDataRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public long ActorId { get; set; }

//unit的ID
		[ProtoMember(1)]
		public long unitID { get; set; }

	}

	[Message(OuterOpcode.M2C_GetHeroDataResponse)]
	[ProtoContract]
	public partial class M2C_GetHeroDataResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public long heroDataID { get; set; }

		[ProtoMember(94)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_SkillCmd))]
	[Message(OuterOpcode.C2M_SkillCmd)]
	[ProtoContract]
	public partial class C2M_SkillCmd: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int SkillID { get; set; }

		[ProtoMember(2)]
		public long TargetID { get; set; }

		[ProtoMember(3)]
		public int TargetAngle { get; set; }

		[ProtoMember(4)]
		public float TargetDistance { get; set; }

		[ProtoMember(5)]
		public int WeaponSkillID { get; set; }

		[ProtoMember(6)]
		public int ItemId { get; set; }

		[ProtoMember(7)]
		public float SingValue { get; set; }

	}

	[Message(OuterOpcode.M2C_SkillCmd)]
	[ProtoContract]
	public partial class M2C_SkillCmd: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long CDEndTime { get; set; }

		[ProtoMember(2)]
		public long PublicCDTime { get; set; }

	}

	[Message(OuterOpcode.M2C_UnitUseSkill)]
	[ProtoContract]
	public partial class M2C_UnitUseSkill: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

		[ProtoMember(1)]
		public int SkillID { get; set; }

		[ProtoMember(3)]
		public int TargetAngle { get; set; }

		[ProtoMember(4)]
		public List<SkillInfo> SkillInfos = new List<SkillInfo>();

		[ProtoMember(6)]
		public int ItemId { get; set; }

		[ProtoMember(7)]
		public long CDEndTime { get; set; }

		[ProtoMember(8)]
		public long PublicCDTime { get; set; }

	}

//闪电链
	[Message(OuterOpcode.M2C_ChainLightning)]
	[ProtoContract]
	public partial class M2C_ChainLightning: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public long TargetID { get; set; }

		[ProtoMember(3)]
		public int SkillID { get; set; }

		[ProtoMember(6)]
		public float PosX { get; set; }

		[ProtoMember(7)]
		public float PosY { get; set; }

		[ProtoMember(8)]
		public float PosZ { get; set; }

		[ProtoMember(9)]
		public int Type { get; set; }

		[ProtoMember(10)]
		public long InstanceId { get; set; }

	}

	[Message(OuterOpcode.SkillInfo)]
	[ProtoContract]
	public partial class SkillInfo: Object
	{
		[ProtoMember(2)]
		public long TargetID { get; set; }

		[ProtoMember(3)]
		public int TargetAngle { get; set; }

		[ProtoMember(5)]
		public int WeaponSkillID { get; set; }

		[ProtoMember(6)]
		public float PosX { get; set; }

		[ProtoMember(7)]
		public float PosY { get; set; }

		[ProtoMember(8)]
		public float PosZ { get; set; }

		[ProtoMember(11)]
		public long SkillBeginTime { get; set; }

		[ProtoMember(12)]
		public long SkillEndTime { get; set; }

		[ProtoMember(13)]
		public float SingValue { get; set; }

		[ProtoMember(14)]
		public int SkillID { get; set; }

	}

	[Message(OuterOpcode.C2M_CreateSpiling)]
	[ProtoContract]
	public partial class C2M_CreateSpiling: Object, IActorLocationMessage
	{
		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

//所归属的父实体id
		[ProtoMember(5)]
		public long ParentUnitId { get; set; }

		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long Id { get; set; }

	}

//创建木桩
	[Message(OuterOpcode.M2C_CreateSpilings)]
	[ProtoContract]
	public partial class M2C_CreateSpilings: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<SpilingInfo> Spilings = new List<SpilingInfo>();

	}

	[Message(OuterOpcode.SpilingInfo)]
	[ProtoContract]
	public partial class SpilingInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public float X { get; set; }

		[ProtoMember(3)]
		public float Y { get; set; }

		[ProtoMember(4)]
		public float Z { get; set; }

		[ProtoMember(5)]
		public List<int> Ks = new List<int>();

		[ProtoMember(6)]
		public List<long> Vs = new List<long>();

		[ProtoMember(8)]
		public int RoleCamp { get; set; }

		[ProtoMember(9)]
		public int MonsterID { get; set; }

		[ProtoMember(10)]
		public int SkillId { get; set; }

		[ProtoMember(12)]
		public float ForwardX { get; set; }

		[ProtoMember(13)]
		public float ForwardY { get; set; }

		[ProtoMember(14)]
		public float ForwardZ { get; set; }

		[ProtoMember(19)]
		public List<KeyValuePair> Buffs = new List<KeyValuePair>();

		[ProtoMember(20)]
		public List<SkillInfo> Skills = new List<SkillInfo>();

	}

	[Message(OuterOpcode.M2C_UnitNumericUpdate)]
	[ProtoContract]
	public partial class M2C_UnitNumericUpdate: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

		[ProtoMember(1)]
		public int SkillId { get; set; }

		[ProtoMember(2)]
		public int NumericType { get; set; }

		[ProtoMember(3)]
		public long OldValue { get; set; }

		[ProtoMember(4)]
		public long NewValue { get; set; }

		[ProtoMember(5)]
		public int DamgeType { get; set; }

		[ProtoMember(6)]
		public long AttackId { get; set; }

	}

	[Message(OuterOpcode.M2C_SyncUnitPos)]
	[ProtoContract]
	public partial class M2C_SyncUnitPos: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public float PosX { get; set; }

		[ProtoMember(3)]
		public float PosY { get; set; }

		[ProtoMember(4)]
		public float PosZ { get; set; }

	}

	[Message(OuterOpcode.M2C_CreateDropItems)]
	[ProtoContract]
	public partial class M2C_CreateDropItems: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

		[ProtoMember(1)]
		public List<DropInfo> Drops = new List<DropInfo>();

	}

	[Message(OuterOpcode.DropInfo)]
	[ProtoContract]
	public partial class DropInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public int ItemID { get; set; }

		[ProtoMember(4)]
		public int ItemNum { get; set; }

		[ProtoMember(5)]
		public float X { get; set; }

		[ProtoMember(6)]
		public float Y { get; set; }

		[ProtoMember(7)]
		public float Z { get; set; }

		[ProtoMember(8)]
		public int DropType { get; set; }

		[ProtoMember(9)]
		public int CellIndex { get; set; }

		[ProtoMember(10)]
		public long BeKillId { get; set; }

	}

	[Message(OuterOpcode.TransferInfo)]
	[ProtoContract]
	public partial class TransferInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int Direction { get; set; }

		[ProtoMember(3)]
		public int CellIndex { get; set; }

		[ProtoMember(5)]
		public float X { get; set; }

		[ProtoMember(6)]
		public float Y { get; set; }

		[ProtoMember(7)]
		public float Z { get; set; }

		[ProtoMember(8)]
		public int TransferId { get; set; }

	}

	[Message(OuterOpcode.NpcInfo)]
	[ProtoContract]
	public partial class NpcInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int NpcID { get; set; }

		[ProtoMember(5)]
		public float X { get; set; }

		[ProtoMember(6)]
		public float Y { get; set; }

		[ProtoMember(7)]
		public float Z { get; set; }

	}

	[Message(OuterOpcode.M2C_CancelAttack)]
	[ProtoContract]
	public partial class M2C_CancelAttack: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UnitId { get; set; }

	}

	[ResponseType(nameof(M2C_TestRobotCase))]
	[Message(OuterOpcode.C2M_TestRobotCase)]
	[ProtoContract]
	public partial class C2M_TestRobotCase: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[Message(OuterOpcode.M2C_TestRobotCase)]
	[ProtoContract]
	public partial class M2C_TestRobotCase: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int N { get; set; }

	}

	[ResponseType(nameof(M2C_TransferMap))]
	[Message(OuterOpcode.C2M_TransferMap)]
	[ProtoContract]
	public partial class C2M_TransferMap: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_TransferMap)]
	[ProtoContract]
	public partial class M2C_TransferMap: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.C2M_UnitStateUpdate)]
	[ProtoContract]
	public partial class C2M_UnitStateUpdate: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long StateType { get; set; }

		[ProtoMember(3)]
		public int StateOperateType { get; set; }

		[ProtoMember(4)]
		public int StateTime { get; set; }

		[ProtoMember(5)]
		public string StateValue { get; set; }

	}

	[Message(OuterOpcode.M2C_UnitStateUpdate)]
	[ProtoContract]
	public partial class M2C_UnitStateUpdate: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public long StateType { get; set; }

		[ProtoMember(3)]
		public int StateOperateType { get; set; }

		[ProtoMember(4)]
		public int StateTime { get; set; }

		[ProtoMember(5)]
		public string StateValue { get; set; }

	}

//message M2C_BuffInfo // IActorMessage
//{
//    int32 RpcId = 90;
//    int64 ActorId = 93;
//    int64 UnitId = 1; //要发送到的目标UnitId
//    int64 SkillId = 96; //目标技能Id
//    string BBKey = 2; //黑板键，此键对应值将会被设置为Buff层数
//    int64 TheUnitBelongToId = 95; //Buff归属的UnitId
//    int64 TheUnitFromId = 91; //Buff来自的UnitId
//    int32 BuffLayers = 3; //Buff层数
//    float BuffMaxLimitTime = 4; //Buff最大持续到的时间点
//}
	[Message(OuterOpcode.UnitBuffInfo)]
	[ProtoContract]
	public partial class UnitBuffInfo: Object
	{
		[ProtoMember(1)]
		public int BuffID { get; set; }

		[ProtoMember(2)]
		public long UnitIdBelongTo { get; set; }

		[ProtoMember(4)]
		public int BuffOperateType { get; set; }

		[ProtoMember(5)]
		public List<float> TargetPostion = new List<float>();

		[ProtoMember(6)]
		public long BuffEndTime { get; set; }

		[ProtoMember(7)]
		public string Spellcaster { get; set; }

		[ProtoMember(8)]
		public int UnitType { get; set; }

		[ProtoMember(9)]
		public int UnitConfigId { get; set; }

		[ProtoMember(10)]
		public int SkillId { get; set; }

	}

	[Message(OuterOpcode.M2C_UnitBuffUpdate)]
	[ProtoContract]
	public partial class M2C_UnitBuffUpdate: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int BuffID { get; set; }

		[ProtoMember(2)]
		public long UnitIdBelongTo { get; set; }

		[ProtoMember(4)]
		public int BuffOperateType { get; set; }

		[ProtoMember(5)]
		public List<float> TargetPostion = new List<float>();

		[ProtoMember(6)]
		public long BuffEndTime { get; set; }

		[ProtoMember(7)]
		public string Spellcaster { get; set; }

		[ProtoMember(8)]
		public int UnitType { get; set; }

		[ProtoMember(9)]
		public int UnitConfigId { get; set; }

		[ProtoMember(10)]
		public int SkillId { get; set; }

		[ProtoMember(11)]
		public long UnitIdFrom { get; set; }

	}

	[Message(OuterOpcode.M2C_UnitBuffRemove)]
	[ProtoContract]
	public partial class M2C_UnitBuffRemove: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int BuffID { get; set; }

		[ProtoMember(2)]
		public long UnitIdBelongTo { get; set; }

	}

	[Message(OuterOpcode.M2C_UnitBuffStatus)]
	[ProtoContract]
	public partial class M2C_UnitBuffStatus: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int BuffID { get; set; }

		[ProtoMember(2)]
		public string FlyText { get; set; }

		[ProtoMember(3)]
		public int FlyType { get; set; }

		[ProtoMember(4)]
		public long UnitID { get; set; }

	}

	[ResponseType(nameof(G2C_OffLine))]
	[Message(OuterOpcode.C2G_OffLine)]
	[ProtoContract]
	public partial class C2G_OffLine: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int unitId { get; set; }

	}

	[Message(OuterOpcode.G2C_OffLine)]
	[ProtoContract]
	public partial class G2C_OffLine: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(D2C_Test))]
	[Message(OuterOpcode.C2D_Test)]
	[ProtoContract]
	public partial class C2D_Test: Object, IDBCacheActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string TestMsg { get; set; }

	}

	[Message(OuterOpcode.D2C_Test)]
	[ProtoContract]
	public partial class D2C_Test: Object, IDBCacheActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.C2M_GMCommandRequest)]
	[ProtoContract]
	public partial class C2M_GMCommandRequest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string GMMsg { get; set; }

		[ProtoMember(2)]
		public string Account { get; set; }

	}

	[ResponseType(nameof(C2C_SendChatResponse))]
	[Message(OuterOpcode.C2C_SendChatRequest)]
	[ProtoContract]
	public partial class C2C_SendChatRequest: Object, IChatActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public ChatInfo ChatInfo { get; set; }

	}

	[Message(OuterOpcode.C2C_SendChatResponse)]
	[ProtoContract]
	public partial class C2C_SendChatResponse: Object, IChatActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string ChatMsg { get; set; }

		[ProtoMember(2)]
		public int ChannelId { get; set; }

	}

	[ResponseType(nameof(C2C_ChatJinYanResponse))]
	[Message(OuterOpcode.C2C_ChatJinYanRequest)]
	[ProtoContract]
	public partial class C2C_ChatJinYanRequest: Object, IChatActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public long JinYanId { get; set; }

		[ProtoMember(3)]
		public string JinYanPlayer { get; set; }

	}

	[Message(OuterOpcode.C2C_ChatJinYanResponse)]
	[ProtoContract]
	public partial class C2C_ChatJinYanResponse: Object, IChatActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.ChatInfo)]
	[ProtoContract]
	public partial class ChatInfo: Object
	{
		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(3)]
		public string ChatMsg { get; set; }

		[ProtoMember(4)]
		public string PlayerName { get; set; }

		[ProtoMember(2)]
		public int ChannelId { get; set; }

		[ProtoMember(5)]
		public long ParamId { get; set; }

		[ProtoMember(6)]
		public long Time { get; set; }

		[ProtoMember(7)]
		public int Occ { get; set; }

		[ProtoMember(8)]
		public int PlayerLevel { get; set; }

	}

	[Message(OuterOpcode.M2C_SyncChatInfo)]
	[ProtoContract]
	public partial class M2C_SyncChatInfo: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public ChatInfo ChatInfo { get; set; }

	}

	[ResponseType(nameof(M2C_RechargeResponse))]
	[Message(OuterOpcode.C2M_RechargeRequest)]
	[ProtoContract]
	public partial class C2M_RechargeRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RechargeNumber { get; set; }

		[ProtoMember(2)]
		public long PayType { get; set; }

		[ProtoMember(3)]
		public string RiskControlInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_RechargeResponse)]
	[ProtoContract]
	public partial class M2C_RechargeResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string PayMessage { get; set; }

	}

	[Message(OuterOpcode.M2C_HorseNoticeInfo)]
	[ProtoContract]
	public partial class M2C_HorseNoticeInfo: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string NoticeText { get; set; }

		[ProtoMember(2)]
		public int NoticeType { get; set; }

	}

//物品排序[通知服务器排序，暂时不需要返回]
	[Message(OuterOpcode.C2M_ItemSortRequest)]
	[ProtoContract]
	public partial class C2M_ItemSortRequest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[ResponseType(nameof(M2C_ItemHuiShouResponse))]
//回收装备
	[Message(OuterOpcode.C2M_ItemHuiShouRequest)]
	[ProtoContract]
	public partial class C2M_ItemHuiShouRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<long> OperateBagID = new List<long>();

	}

	[Message(OuterOpcode.M2C_ItemHuiShouResponse)]
	[ProtoContract]
	public partial class M2C_ItemHuiShouResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_ItemMeltingResponse))]
//装备熔炼
	[Message(OuterOpcode.C2M_ItemMeltingRequest)]
	[ProtoContract]
	public partial class C2M_ItemMeltingRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public List<long> OperateBagID = new List<long>();

		[ProtoMember(3)]
		public int MakeType { get; set; }

		[ProtoMember(4)]
		public int Plan { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemMeltingResponse)]
	[ProtoContract]
	public partial class M2C_ItemMeltingResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_ItemQiangHuaResponse))]
//强化槽位
	[Message(OuterOpcode.C2M_ItemQiangHuaRequest)]
	[ProtoContract]
	public partial class C2M_ItemQiangHuaRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int WeiZhi { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemQiangHuaResponse)]
	[ProtoContract]
	public partial class M2C_ItemQiangHuaResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public int QiangHuaLevel { get; set; }

	}

	[ResponseType(nameof(M2C_ItemXiLianResponse))]
//洗练装备
	[Message(OuterOpcode.C2M_ItemXiLianRequest)]
	[ProtoContract]
	public partial class C2M_ItemXiLianRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long OperateBagID { get; set; }

		[ProtoMember(1)]
		public int Times { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemXiLianResponse)]
	[ProtoContract]
	public partial class M2C_ItemXiLianResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<ItemXiLianResult> ItemXiLianResults = new List<ItemXiLianResult>();

	}

	[ResponseType(nameof(M2C_ItemInheritResponse))]
//装备传承
	[Message(OuterOpcode.C2M_ItemInheritRequest)]
	[ProtoContract]
	public partial class C2M_ItemInheritRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long OperateBagID { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemInheritResponse)]
	[ProtoContract]
	public partial class M2C_ItemInheritResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public List<int> InheritSkills = new List<int>();

	}

	[ResponseType(nameof(M2C_ItemProtectResponse))]
//装备锁定
	[Message(OuterOpcode.C2M_ItemProtectRequest)]
	[ProtoContract]
	public partial class C2M_ItemProtectRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long OperateBagID { get; set; }

		[ProtoMember(2)]
		public bool IsProtect { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemProtectResponse)]
	[ProtoContract]
	public partial class M2C_ItemProtectResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_ItemInheritSelectResponse))]
//传承确认
	[Message(OuterOpcode.C2M_ItemInheritSelectRequest)]
	[ProtoContract]
	public partial class C2M_ItemInheritSelectRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long OperateBagID { get; set; }

		[ProtoMember(2)]
		public List<int> InheritSkills = new List<int>();

	}

	[Message(OuterOpcode.M2C_ItemInheritSelectResponse)]
	[ProtoContract]
	public partial class M2C_ItemInheritSelectResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_ItemOperateResponse))]
//穿戴装备
	[Message(OuterOpcode.C2M_ItemOperateRequest)]
	[ProtoContract]
	public partial class C2M_ItemOperateRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public long OperateBagID { get; set; }

		[ProtoMember(3)]
		public string OperatePar { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemOperateResponse)]
	[ProtoContract]
	public partial class M2C_ItemOperateResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public string OperatePar { get; set; }

	}

//道具[装备]更新
	[Message(OuterOpcode.M2C_RoleBagUpdate)]
	[ProtoContract]
	public partial class M2C_RoleBagUpdate: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<BagInfo> BagInfoAdd = new List<BagInfo>();

		[ProtoMember(2)]
		public List<BagInfo> BagInfoUpdate = new List<BagInfo>();

		[ProtoMember(3)]
		public List<BagInfo> BagInfoDelete = new List<BagInfo>();

	}

	[ResponseType(nameof(Actor_FubenEnergySkillResponse))]
	[Message(OuterOpcode.Actor_FubenEnergySkillRequest)]
	[ProtoContract]
	public partial class Actor_FubenEnergySkillRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int SkillId { get; set; }

	}

	[Message(OuterOpcode.Actor_FubenEnergySkillResponse)]
	[ProtoContract]
	public partial class Actor_FubenEnergySkillResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Actor_EnterFubenResponse))]
	[Message(OuterOpcode.Actor_EnterFubenRequest)]
	[ProtoContract]
	public partial class Actor_EnterFubenRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ChapterId { get; set; }

		[ProtoMember(2)]
		public int Difficulty { get; set; }

		[ProtoMember(3)]
		public int RepeatEnter { get; set; }

	}

	[Message(OuterOpcode.Actor_EnterFubenResponse)]
	[ProtoContract]
	public partial class Actor_EnterFubenResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public FubenInfo FubenInfo { get; set; }

		[ProtoMember(2)]
		public SonFubenInfo SonFubenInfo { get; set; }

	}

	[Message(OuterOpcode.FubenInfo)]
	[ProtoContract]
	public partial class FubenInfo: Object
	{
		[ProtoMember(2)]
		public int StartCell { get; set; }

		[ProtoMember(3)]
		public int EndCell { get; set; }

		[ProtoMember(4)]
		public List<KeyValuePair> FubenCellNpcs = new List<KeyValuePair>();

	}

	[Message(OuterOpcode.SonFubenInfo)]
	[ProtoContract]
	public partial class SonFubenInfo: Object
	{
		[ProtoMember(1)]
		public int SonSceneId { get; set; }

		[ProtoMember(2)]
		public int CurrentCell { get; set; }

		[ProtoMember(3)]
		public List<int> PassableFlag = new List<int>();

	}

	[Message(OuterOpcode.FubenPassInfo)]
	[ProtoContract]
	public partial class FubenPassInfo: Object
	{
		[ProtoMember(1)]
		public int FubenId { get; set; }

		[ProtoMember(2)]
		public int Difficulty { get; set; }

	}

	[ResponseType(nameof(Actor_GetFubenInfoResponse))]
	[Message(OuterOpcode.Actor_GetFubenInfoRequest)]
	[ProtoContract]
	public partial class Actor_GetFubenInfoRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.Actor_GetFubenInfoResponse)]
	[ProtoContract]
	public partial class Actor_GetFubenInfoResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<FubenPassInfo> FubenPassInfos = new List<FubenPassInfo>();

	}

	[ResponseType(nameof(Actor_EnterSonFubenResponse))]
	[Message(OuterOpcode.Actor_EnterSonFubenRequest)]
	[ProtoContract]
	public partial class Actor_EnterSonFubenRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public int CurrentCell { get; set; }

		[ProtoMember(3)]
		public int DirectionType { get; set; }

		[ProtoMember(4)]
		public int ChuansongId { get; set; }

	}

	[Message(OuterOpcode.Actor_EnterSonFubenResponse)]
	[ProtoContract]
	public partial class Actor_EnterSonFubenResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public SonFubenInfo SonFubenInfo { get; set; }

	}

	[ResponseType(nameof(Actor_QuitFubenResponse))]
	[Message(OuterOpcode.Actor_QuitFubenRequest)]
	[ProtoContract]
	public partial class Actor_QuitFubenRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MapIndex { get; set; }

	}

	[Message(OuterOpcode.Actor_QuitFubenResponse)]
	[ProtoContract]
	public partial class Actor_QuitFubenResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Actor_SendReviveResponse))]
	[Message(OuterOpcode.Actor_SendReviveRequest)]
	[ProtoContract]
	public partial class Actor_SendReviveRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MapIndex { get; set; }

		[ProtoMember(2)]
		public bool Revive { get; set; }

	}

	[Message(OuterOpcode.Actor_SendReviveResponse)]
	[ProtoContract]
	public partial class Actor_SendReviveResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.M2C_FubenSettlement)]
	[ProtoContract]
	public partial class M2C_FubenSettlement: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int BattleResult { get; set; }

		[ProtoMember(2)]
		public int BattleGrade { get; set; }

		[ProtoMember(3)]
		public int RewardExp { get; set; }

		[ProtoMember(4)]
		public int RewardGold { get; set; }

		[ProtoMember(5)]
		public List<RewardItem> ReardList = new List<RewardItem>();

		[ProtoMember(6)]
		public List<RewardItem> ReardListExcess = new List<RewardItem>();

		[ProtoMember(7)]
		public List<int> StarInfos = new List<int>();

	}

	[ResponseType(nameof(Actor_GetFubenRewardReponse))]
	[Message(OuterOpcode.Actor_GetFubenRewardRequest)]
	[ProtoContract]
	public partial class Actor_GetFubenRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public RewardItem RewardItem { get; set; }

	}

	[Message(OuterOpcode.Actor_GetFubenRewardReponse)]
	[ProtoContract]
	public partial class Actor_GetFubenRewardReponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(Actor_PickItemResponse))]
	[Message(OuterOpcode.Actor_PickItemRequest)]
	[ProtoContract]
	public partial class Actor_PickItemRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<DropInfo> ItemIds = new List<DropInfo>();

	}

	[Message(OuterOpcode.Actor_PickItemResponse)]
	[ProtoContract]
	public partial class Actor_PickItemResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<long> RemoveIds = new List<long>();

	}

	[ResponseType(nameof(Actor_ItemInitResponse))]
	[Message(OuterOpcode.Actor_ItemInitRequest)]
	[ProtoContract]
	public partial class Actor_ItemInitRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.Actor_ItemInitResponse)]
	[ProtoContract]
	public partial class Actor_ItemInitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<BagInfo> BagInfos = new List<BagInfo>();

		[ProtoMember(2)]
		public List<int> QiangHuaLevel = new List<int>();

		[ProtoMember(3)]
		public List<int> QiangHuaFails = new List<int>();

//int32 BagAddedCell = 4;
		[ProtoMember(5)]
		public List<int> WarehouseAddedCell = new List<int>();

		[ProtoMember(6)]
		public List<int> FashionActiveIds = new List<int>();

		[ProtoMember(7)]
		public List<int> FashionEquipList = new List<int>();

		[ProtoMember(8)]
		public int SeasonJingHePlan { get; set; }

		[ProtoMember(9)]
		public List<int> AdditionalCellNum = new List<int>();

	}

//活跃宝箱
	[ResponseType(nameof(M2C_TaskHuoYueRewardResponse))]
	[Message(OuterOpcode.C2M_TaskHuoYueRewardRequest)]
	[ProtoContract]
	public partial class C2M_TaskHuoYueRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int HuoYueId { get; set; }

	}

	[Message(OuterOpcode.M2C_TaskHuoYueRewardResponse)]
	[ProtoContract]
	public partial class M2C_TaskHuoYueRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//每日活跃
	[ResponseType(nameof(M2C_TaskCountryInitResponse))]
	[Message(OuterOpcode.C2M_TaskCountryInitRequest)]
	[ProtoContract]
	public partial class C2M_TaskCountryInitRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_TaskCountryInitResponse)]
	[ProtoContract]
	public partial class M2C_TaskCountryInitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<TaskPro> TaskCountryList = new List<TaskPro>();

		[ProtoMember(2)]
		public List<int> ReceiveHuoYueIds = new List<int>();

	}

//提交任务
	[ResponseType(nameof(M2C_CommitTaskCountryResponse))]
	[Message(OuterOpcode.C2M_CommitTaskCountryRequest)]
	[ProtoContract]
	public partial class C2M_CommitTaskCountryRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TaskId { get; set; }

		[ProtoMember(2)]
		public long BagInfoID { get; set; }

	}

	[Message(OuterOpcode.M2C_CommitTaskCountryResponse)]
	[ProtoContract]
	public partial class M2C_CommitTaskCountryResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.M2C_TaskCountryUpdate)]
	[ProtoContract]
	public partial class M2C_TaskCountryUpdate: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int UpdateMode { get; set; }

		[ProtoMember(2)]
		public List<TaskPro> TaskCountryList = new List<TaskPro>();

	}

//任务列表
	[ResponseType(nameof(M2C_TaskInitResponse))]
	[Message(OuterOpcode.C2M_TaskInitRequest)]
	[ProtoContract]
	public partial class C2M_TaskInitRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_TaskInitResponse)]
	[ProtoContract]
	public partial class M2C_TaskInitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<TaskPro> RoleTaskList = new List<TaskPro>();

		[ProtoMember(2)]
		public List<int> RoleComoleteTaskList = new List<int>();

		[ProtoMember(3)]
		public List<TaskPro> TaskCountryList = new List<TaskPro>();

		[ProtoMember(4)]
		public List<int> ReceiveHuoYueIds = new List<int>();

	}

//接取任务
	[ResponseType(nameof(M2C_TaskGetResponse))]
	[Message(OuterOpcode.C2M_TaskGetRequest)]
	[ProtoContract]
	public partial class C2M_TaskGetRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TaskId { get; set; }

		[ProtoMember(2)]
		public int TaskStatus { get; set; }

	}

	[Message(OuterOpcode.M2C_TaskGetResponse)]
	[ProtoContract]
	public partial class M2C_TaskGetResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public TaskPro TaskPro { get; set; }

	}

//放弃任务
	[ResponseType(nameof(M2C_TaskGiveUpResponse))]
	[Message(OuterOpcode.C2M_TaskGiveUpRequest)]
	[ProtoContract]
	public partial class C2M_TaskGiveUpRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TaskId { get; set; }

	}

	[Message(OuterOpcode.M2C_TaskGiveUpResponse)]
	[ProtoContract]
	public partial class M2C_TaskGiveUpResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//任务追踪
	[ResponseType(nameof(M2C_TaskTrackResponse))]
	[Message(OuterOpcode.C2M_TaskTrackRequest)]
	[ProtoContract]
	public partial class C2M_TaskTrackRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TaskId { get; set; }

		[ProtoMember(2)]
		public int TrackStatus { get; set; }

	}

	[Message(OuterOpcode.M2C_TaskTrackResponse)]
	[ProtoContract]
	public partial class M2C_TaskTrackResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//任务通知【目前用于对话完成】
	[ResponseType(nameof(M2C_TaskNoticeResponse))]
	[Message(OuterOpcode.C2M_TaskNoticeRequest)]
	[ProtoContract]
	public partial class C2M_TaskNoticeRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TaskId { get; set; }

		[ProtoMember(2)]
		public int TaskStatus { get; set; }

	}

	[Message(OuterOpcode.M2C_TaskNoticeResponse)]
	[ProtoContract]
	public partial class M2C_TaskNoticeResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//提交任务
	[ResponseType(nameof(M2C_TaskCommitResponse))]
	[Message(OuterOpcode.C2M_TaskCommitRequest)]
	[ProtoContract]
	public partial class C2M_TaskCommitRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TaskId { get; set; }

		[ProtoMember(2)]
		public long BagInfoID { get; set; }

	}

	[Message(OuterOpcode.M2C_TaskCommitResponse)]
	[ProtoContract]
	public partial class M2C_TaskCommitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<int> RoleComoleteTaskList = new List<int>();

	}

	[Message(OuterOpcode.M2C_TaskUpdate)]
	[ProtoContract]
	public partial class M2C_TaskUpdate: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<TaskPro> RoleTaskList = new List<TaskPro>();

		[ProtoMember(2)]
		public List<TaskPro> DayTaskList = new List<TaskPro>();

	}

//任务列表
	[Message(OuterOpcode.TaskPro)]
	[ProtoContract]
	public partial class TaskPro: Object
	{
		[ProtoMember(1)]
		public int taskID { get; set; }

		[ProtoMember(2)]
		public int taskTargetNum_1 { get; set; }

		[ProtoMember(5)]
		public int taskTargetNum_2 { get; set; }

		[ProtoMember(6)]
		public int taskTargetNum_3 { get; set; }

		[ProtoMember(3)]
		public int taskStatus { get; set; }

		[ProtoMember(4)]
		public int TrackStatus { get; set; }

		[ProtoMember(7)]
		public int FubenId { get; set; }

		[ProtoMember(8)]
		public int WaveId { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetList))]
	[Message(OuterOpcode.C2M_RolePetList)]
	[ProtoContract]
	public partial class C2M_RolePetList: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetList)]
	[ProtoContract]
	public partial class M2C_RolePetList: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<RolePetInfo> RolePetInfos = new List<RolePetInfo>();

		[ProtoMember(2)]
		public List<long> TeamPetList = new List<long>();

		[ProtoMember(3)]
		public List<RolePetEgg> RolePetEggs = new List<RolePetEgg>();

		[ProtoMember(4)]
		public List<long> PetFormations = new List<long>();

		[ProtoMember(5)]
		public List<PetFubenInfo> PetFubenInfos = new List<PetFubenInfo>();

		[ProtoMember(6)]
		public List<KeyValuePair> PetSkinList = new List<KeyValuePair>();

		[ProtoMember(7)]
		public int PetFubeRewardId { get; set; }

		[ProtoMember(8)]
		public List<long> PetShouHuList = new List<long>();

		[ProtoMember(9)]
		public int PetShouHuActive { get; set; }

		[ProtoMember(10)]
		public List<int> PetCangKuOpen = new List<int>();

		[ProtoMember(11)]
		public List<long> PetMingList = new List<long>();

		[ProtoMember(12)]
		public List<long> PetMingPosition = new List<long>();

		[ProtoMember(13)]
		public List<RolePetInfo> RolePetBag = new List<RolePetInfo>();

	}

	[Message(OuterOpcode.PetFubenInfo)]
	[ProtoContract]
	public partial class PetFubenInfo: Object
	{
		[ProtoMember(1)]
		public int PetFubenId { get; set; }

		[ProtoMember(2)]
		public int Star { get; set; }

		[ProtoMember(3)]
		public int Reward { get; set; }

	}

//宠物更新
	[Message(OuterOpcode.M2C_RolePetUpdate)]
	[ProtoContract]
	public partial class M2C_RolePetUpdate: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<RolePetInfo> PetInfoAdd = new List<RolePetInfo>();

		[ProtoMember(2)]
		public List<RolePetInfo> PetInfoUpdate = new List<RolePetInfo>();

		[ProtoMember(3)]
		public List<RolePetInfo> PetInfoDelete = new List<RolePetInfo>();

		[ProtoMember(4)]
		public int GetWay { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetFormationSet))]
//宠物出战设置
	[Message(OuterOpcode.C2M_RolePetFormationSet)]
	[ProtoContract]
	public partial class C2M_RolePetFormationSet: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Index { get; set; }

		[ProtoMember(2)]
		public long PetId { get; set; }

		[ProtoMember(3)]
		public int OperateType { get; set; }

		[ProtoMember(4)]
		public int SceneType { get; set; }

		[ProtoMember(5)]
		public List<long> PetFormat = new List<long>();

		[ProtoMember(6)]
		public List<long> PetPosition = new List<long>();

	}

	[Message(OuterOpcode.M2C_RolePetFormationSet)]
	[ProtoContract]
	public partial class M2C_RolePetFormationSet: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetSkinSet))]
//更改宠物皮肤
	[Message(OuterOpcode.C2M_RolePetSkinSet)]
	[ProtoContract]
	public partial class C2M_RolePetSkinSet: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public int SkinId { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetSkinSet)]
	[ProtoContract]
	public partial class M2C_RolePetSkinSet: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetHeXin))]
//更改宠物之核
	[Message(OuterOpcode.C2M_RolePetHeXin)]
	[ProtoContract]
	public partial class C2M_RolePetHeXin: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public long BagInfoId { get; set; }

		[ProtoMember(3)]
		public int Position { get; set; }

		[ProtoMember(4)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetHeXin)]
	[ProtoContract]
	public partial class M2C_RolePetHeXin: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetInfo RolePetInfo { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetEggPut))]
//宠物蛋放入
	[Message(OuterOpcode.C2M_RolePetEggPut)]
	[ProtoContract]
	public partial class C2M_RolePetEggPut: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long BagInfoId { get; set; }

		[ProtoMember(2)]
		public int Index { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetEggPut)]
	[ProtoContract]
	public partial class M2C_RolePetEggPut: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetEgg RolePetEgg { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetEggHatch))]
//宠物蛋孵化
	[Message(OuterOpcode.C2M_RolePetEggHatch)]
	[ProtoContract]
	public partial class C2M_RolePetEggHatch: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long BagInfoId { get; set; }

		[ProtoMember(2)]
		public int Index { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetEggHatch)]
	[ProtoContract]
	public partial class M2C_RolePetEggHatch: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetEgg RolePetEgg { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetEggOpen))]
//宠物蛋开启【时间未到需要扣除钻石】
	[Message(OuterOpcode.C2M_RolePetEggOpen)]
	[ProtoContract]
	public partial class C2M_RolePetEggOpen: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Index { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetEggOpen)]
	[ProtoContract]
	public partial class M2C_RolePetEggOpen: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetInfo PetInfo { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetRName))]
//宠物改名
	[Message(OuterOpcode.C2M_RolePetRName)]
	[ProtoContract]
	public partial class C2M_RolePetRName: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public string PetName { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetRName)]
	[ProtoContract]
	public partial class M2C_RolePetRName: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetFight))]
//宠物出战[1出战 0休息]
	[Message(OuterOpcode.C2M_RolePetFight)]
	[ProtoContract]
	public partial class C2M_RolePetFight: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public int PetStatus { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetFight)]
	[ProtoContract]
	public partial class M2C_RolePetFight: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_PetPutCangKu))]
	[Message(OuterOpcode.C2M_PetPutCangKu)]
	[ProtoContract]
	public partial class C2M_PetPutCangKu: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public int PetStatus { get; set; }

		[ProtoMember(3)]
		public int OpenIndex { get; set; }

	}

	[Message(OuterOpcode.M2C_PetPutCangKu)]
	[ProtoContract]
	public partial class M2C_PetPutCangKu: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_PetOpenCangKu))]
	[Message(OuterOpcode.C2M_PetOpenCangKu)]
	[ProtoContract]
	public partial class C2M_PetOpenCangKu: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OpenIndex { get; set; }

	}

	[Message(OuterOpcode.M2C_PetOpenCangKu)]
	[ProtoContract]
	public partial class M2C_PetOpenCangKu: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetProtect))]
	[Message(OuterOpcode.C2M_RolePetProtect)]
	[ProtoContract]
	public partial class C2M_RolePetProtect: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public bool IsProtect { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetProtect)]
	[ProtoContract]
	public partial class M2C_RolePetProtect: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetFenjie))]
//宠物分解
	[Message(OuterOpcode.C2M_RolePetFenjie)]
	[ProtoContract]
	public partial class C2M_RolePetFenjie: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetFenjie)]
	[ProtoContract]
	public partial class M2C_RolePetFenjie: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetJiadian))]
//宠物加点
	[Message(OuterOpcode.C2M_RolePetJiadian)]
	[ProtoContract]
	public partial class C2M_RolePetJiadian: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public List<int> AddPropretyValue = new List<int>();

	}

	[Message(OuterOpcode.M2C_RolePetJiadian)]
	[ProtoContract]
	public partial class M2C_RolePetJiadian: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public RolePetInfo RolePetInfo { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetHeCheng))]
//宠物合成
	[Message(OuterOpcode.C2M_RolePetHeCheng)]
	[ProtoContract]
	public partial class C2M_RolePetHeCheng: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId1 { get; set; }

		[ProtoMember(2)]
		public long PetInfoId2 { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetHeCheng)]
	[ProtoContract]
	public partial class M2C_RolePetHeCheng: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetInfo rolePetInfo { get; set; }

		[ProtoMember(2)]
		public long DeletePetInfoId { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetUpStar))]
//宠物合成
	[Message(OuterOpcode.C2M_RolePetUpStar)]
	[ProtoContract]
	public partial class C2M_RolePetUpStar: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public List<long> CostPetInfoIds = new List<long>();

	}

	[Message(OuterOpcode.M2C_RolePetUpStar)]
	[ProtoContract]
	public partial class M2C_RolePetUpStar: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetInfo rolePetInfo { get; set; }

		[ProtoMember(2)]
		public List<long> CostPetInfoIds = new List<long>();

	}

	[ResponseType(nameof(M2C_RolePetXiLian))]
//宠物洗练
	[Message(OuterOpcode.C2M_RolePetXiLian)]
	[ProtoContract]
	public partial class C2M_RolePetXiLian: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public long BagInfoID { get; set; }

		[ProtoMember(3)]
		public int CostItemNum { get; set; }

		[ProtoMember(4)]
		public string ParamInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetXiLian)]
	[ProtoContract]
	public partial class M2C_RolePetXiLian: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetInfo rolePetInfo { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetXiuLian))]
//宠物修炼
	[Message(OuterOpcode.C2M_RolePetXiuLian)]
	[ProtoContract]
	public partial class C2M_RolePetXiuLian: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public int XiuLianId { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetXiuLian)]
	[ProtoContract]
	public partial class M2C_RolePetXiuLian: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetInfo rolePetInfo { get; set; }

	}

	[Message(OuterOpcode.RolePetEgg)]
	[ProtoContract]
	public partial class RolePetEgg: Object
	{
		[ProtoMember(1)]
		public int ItemId { get; set; }

		[ProtoMember(2)]
		public long EndTime { get; set; }

		[ProtoMember(3)]
		public int FuLing { get; set; }

	}

	[Message(OuterOpcode.RolePetInfo)]
	[ProtoContract]
	public partial class RolePetInfo: Object
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public int PetStatus { get; set; }

		[ProtoMember(3)]
		public int ConfigId { get; set; }

		[ProtoMember(4)]
		public int PetLv { get; set; }

		[ProtoMember(5)]
		public int Star { get; set; }

		[ProtoMember(7)]
		public int PetExp { get; set; }

		[ProtoMember(8)]
		public string PetName { get; set; }

		[ProtoMember(9)]
		public bool IfBaby { get; set; }

		[ProtoMember(10)]
		public int AddPropretyNum { get; set; }

		[ProtoMember(11)]
		public string AddPropretyValue { get; set; }

		[ProtoMember(12)]
		public int PetPingFen { get; set; }

		[ProtoMember(13)]
		public int ZiZhi_Hp { get; set; }

		[ProtoMember(14)]
		public int ZiZhi_Act { get; set; }

		[ProtoMember(15)]
		public int ZiZhi_MageAct { get; set; }

		[ProtoMember(16)]
		public int ZiZhi_Def { get; set; }

		[ProtoMember(17)]
		public int ZiZhi_Adf { get; set; }

		[ProtoMember(18)]
		public int ZiZhi_ActSpeed { get; set; }

		[ProtoMember(19)]
		public float ZiZhi_ChengZhang { get; set; }

		[ProtoMember(20)]
		public List<int> PetSkill = new List<int>();

		[ProtoMember(21)]
		public int EquipID_1 { get; set; }

		[ProtoMember(22)]
		public string EquipIDHide_1 { get; set; }

		[ProtoMember(23)]
		public int EquipID_2 { get; set; }

		[ProtoMember(24)]
		public string EquipIDHide_2 { get; set; }

		[ProtoMember(25)]
		public int EquipID_3 { get; set; }

		[ProtoMember(26)]
		public string EquipIDHide_3 { get; set; }

		[ProtoMember(30)]
		public List<int> Ks = new List<int>();

		[ProtoMember(31)]
		public List<long> Vs = new List<long>();

		[ProtoMember(32)]
		public int RoleCamp { get; set; }

		[ProtoMember(33)]
		public string PlayerName { get; set; }

		[ProtoMember(34)]
		public int SkinId { get; set; }

		[ProtoMember(35)]
		public List<long> PetHeXinList = new List<long>();

		[ProtoMember(38)]
		public int UpStageStatus { get; set; }

		[ProtoMember(39)]
		public int ShouHuPos { get; set; }

		[ProtoMember(40)]
		public bool IsProtect { get; set; }

		[ProtoMember(41)]
		public List<long> PetEquipList = new List<long>();

		[ProtoMember(42)]
		public List<int> LockSkill = new List<int>();

		[ProtoMember(43)]
		public int Luckly { get; set; }

	}

	[ResponseType(nameof(M2C_SkillInitResponse))]
//技能升级
	[Message(OuterOpcode.C2M_SkillInitRequest)]
	[ProtoContract]
	public partial class C2M_SkillInitRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_SkillInitResponse)]
	[ProtoContract]
	public partial class M2C_SkillInitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public SkillSetInfo SkillSetInfo { get; set; }

	}

	[Message(OuterOpcode.LifeShieldInfo)]
	[ProtoContract]
	public partial class LifeShieldInfo: Object
	{
		[ProtoMember(1)]
		public int ShieldType { get; set; }

		[ProtoMember(2)]
		public int Level { get; set; }

		[ProtoMember(3)]
		public int Exp { get; set; }

		[ProtoMember(4)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_SkillUp))]
//技能升级
	[Message(OuterOpcode.C2M_SkillUp)]
	[ProtoContract]
	public partial class C2M_SkillUp: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SkillID { get; set; }

	}

	[Message(OuterOpcode.M2C_SkillUp)]
	[ProtoContract]
	public partial class M2C_SkillUp: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public int NewSkillID { get; set; }

	}

	[ResponseType(nameof(M2C_SkillSet))]
//技能设置
	[Message(OuterOpcode.C2M_SkillSet)]
	[ProtoContract]
	public partial class C2M_SkillSet: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SkillID { get; set; }

		[ProtoMember(2)]
		public int Position { get; set; }

		[ProtoMember(3)]
		public int SkillType { get; set; }

	}

	[Message(OuterOpcode.M2C_SkillSet)]
	[ProtoContract]
	public partial class M2C_SkillSet: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_SkillOperation))]
//技能操作
	[Message(OuterOpcode.C2M_SkillOperation)]
	[ProtoContract]
	public partial class C2M_SkillOperation: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SkillID { get; set; }

		[ProtoMember(3)]
		public int OperationType { get; set; }

		[ProtoMember(4)]
		public string OperationValue { get; set; }

	}

	[Message(OuterOpcode.M2C_SkillOperation)]
	[ProtoContract]
	public partial class M2C_SkillOperation: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

//技能列表
	[Message(OuterOpcode.SkillPro)]
	[ProtoContract]
	public partial class SkillPro: Object
	{
		[ProtoMember(1)]
		public int SkillID { get; set; }

		[ProtoMember(2)]
		public int SkillPosition { get; set; }

		[ProtoMember(3)]
		public int SkillSetType { get; set; }

		[ProtoMember(4)]
		public int Actived { get; set; }

		[ProtoMember(5)]
		public int SkillSource { get; set; }

		[ProtoMember(6)]
		public int ParamId { get; set; }

	}

//通过奖励
	[Message(OuterOpcode.RewardItem)]
	[ProtoContract]
	public partial class RewardItem: Object
	{
		[ProtoMember(1)]
		public int ItemID { get; set; }

		[ProtoMember(2)]
		public int ItemNum { get; set; }

	}

	[ResponseType(nameof(M2C_ChangeOccTwoResponse))]
//转换职业
	[Message(OuterOpcode.C2M_ChangeOccTwoRequest)]
	[ProtoContract]
	public partial class C2M_ChangeOccTwoRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OccTwoID { get; set; }

	}

	[Message(OuterOpcode.M2C_ChangeOccTwoResponse)]
	[ProtoContract]
	public partial class M2C_ChangeOccTwoResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_StoreBuyResponse))]
//商店购买
	[Message(OuterOpcode.C2M_StoreBuyRequest)]
	[ProtoContract]
	public partial class C2M_StoreBuyRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SellItemID { get; set; }

		[ProtoMember(2)]
		public int SellItemNum { get; set; }

	}

	[Message(OuterOpcode.M2C_StoreBuyResponse)]
	[ProtoContract]
	public partial class M2C_StoreBuyResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_GameSettingResponse))]
//游戏设置
	[Message(OuterOpcode.C2M_GameSettingRequest)]
	[ProtoContract]
	public partial class C2M_GameSettingRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<KeyValuePair> GameSettingInfos = new List<KeyValuePair>();

	}

	[Message(OuterOpcode.M2C_GameSettingResponse)]
	[ProtoContract]
	public partial class M2C_GameSettingResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_ModifyNameResponse))]
//改游戏名
	[Message(OuterOpcode.C2M_ModifyNameRequest)]
	[ProtoContract]
	public partial class C2M_ModifyNameRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string NewName { get; set; }

	}

	[Message(OuterOpcode.M2C_ModifyNameResponse)]
	[ProtoContract]
	public partial class M2C_ModifyNameResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[Message(OuterOpcode.M2C_UpdateMailInfo)]
	[ProtoContract]
	public partial class M2C_UpdateMailInfo: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<MailInfo> MailInfos = new List<MailInfo>();

	}

	[Message(OuterOpcode.MailInfo)]
	[ProtoContract]
	public partial class MailInfo: Object
	{
		[ProtoMember(1)]
		public int Status { get; set; }

		[ProtoMember(3)]
		public string Context { get; set; }

		[ProtoMember(5)]
		public long MailId { get; set; }

		[ProtoMember(6)]
		public string Title { get; set; }

		[ProtoMember(7)]
		public List<BagInfo> ItemList = new List<BagInfo>();

		[ProtoMember(8)]
		public BagInfo ItemSell { get; set; }

		[ProtoMember(9)]
		public long BuyPlayerId { get; set; }

	}

	[ResponseType(nameof(M2C_ReceiveMailResponse))]
	[Message(OuterOpcode.C2M_ReceiveMailRequest)]
	[ProtoContract]
	public partial class C2M_ReceiveMailRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long MailId { get; set; }

	}

	[Message(OuterOpcode.M2C_ReceiveMailResponse)]
	[ProtoContract]
	public partial class M2C_ReceiveMailResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(E2C_ReceiveMailResponse))]
	[Message(OuterOpcode.C2E_ReceiveMailRequest)]
	[ProtoContract]
	public partial class C2E_ReceiveMailRequest: Object, IMailActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long MailId { get; set; }

	}

	[Message(OuterOpcode.E2C_ReceiveMailResponse)]
	[ProtoContract]
	public partial class E2C_ReceiveMailResponse: Object, IMailActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(E2C_GetAllMailResponse))]
	[Message(OuterOpcode.C2E_GetAllMailRequest)]
	[ProtoContract]
	public partial class C2E_GetAllMailRequest: Object, IMailActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.E2C_GetAllMailResponse)]
	[ProtoContract]
	public partial class E2C_GetAllMailResponse: Object, IMailActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<MailInfo> MailInfos = new List<MailInfo>();

	}

	[ResponseType(nameof(M2C_MakeEquipResponse))]
	[Message(OuterOpcode.C2M_MakeEquipRequest)]
	[ProtoContract]
	public partial class C2M_MakeEquipRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MakeId { get; set; }

		[ProtoMember(2)]
		public long BagInfoID { get; set; }

		[ProtoMember(3)]
		public int Plan { get; set; }

	}

	[Message(OuterOpcode.M2C_MakeEquipResponse)]
	[ProtoContract]
	public partial class M2C_MakeEquipResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int ItemId { get; set; }

		[ProtoMember(2)]
		public int NewMakeId { get; set; }

	}

	[ResponseType(nameof(M2C_MakeLearnResponse))]
	[Message(OuterOpcode.C2M_MakeLearnRequest)]
	[ProtoContract]
	public partial class C2M_MakeLearnRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MakeId { get; set; }

		[ProtoMember(2)]
		public int Plan { get; set; }

	}

	[Message(OuterOpcode.M2C_MakeLearnResponse)]
	[ProtoContract]
	public partial class M2C_MakeLearnResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_TianFuActiveResponse))]
	[Message(OuterOpcode.C2M_TianFuActiveRequest)]
	[ProtoContract]
	public partial class C2M_TianFuActiveRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int TianFuId { get; set; }

	}

	[Message(OuterOpcode.M2C_TianFuActiveResponse)]
	[ProtoContract]
	public partial class M2C_TianFuActiveResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_RealNameRewardResponse))]
	[Message(OuterOpcode.C2M_RealNameRewardRequest)]
	[ProtoContract]
	public partial class C2M_RealNameRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_RealNameRewardResponse)]
	[ProtoContract]
	public partial class M2C_RealNameRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_YueKaOpenResponse))]
	[Message(OuterOpcode.C2M_YueKaOpenRequest)]
	[ProtoContract]
	public partial class C2M_YueKaOpenRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_YueKaOpenResponse)]
	[ProtoContract]
	public partial class M2C_YueKaOpenResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_YueKaRewardResponse))]
	[Message(OuterOpcode.C2M_YueKaRewardRequest)]
	[ProtoContract]
	public partial class C2M_YueKaRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_YueKaRewardResponse)]
	[ProtoContract]
	public partial class M2C_YueKaRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//成就进度
	[Message(OuterOpcode.ChengJiuInfo)]
	[ProtoContract]
	public partial class ChengJiuInfo: Object
	{
		[ProtoMember(1)]
		public int ChengJiuID { get; set; }

		[ProtoMember(2)]
		public int ChengJiuProgess { get; set; }

		[ProtoMember(3)]
		public long ChengJiuProgessLong { get; set; }

	}

	[ResponseType(nameof(M2C_ChengJiuListResponse))]
	[Message(OuterOpcode.C2M_ChengJiuListRequest)]
	[ProtoContract]
	public partial class C2M_ChengJiuListRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_ChengJiuListResponse)]
	[ProtoContract]
	public partial class M2C_ChengJiuListResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<ChengJiuInfo> ChengJiuProgessList = new List<ChengJiuInfo>();

		[ProtoMember(2)]
		public List<int> ChengJiuCompleteList = new List<int>();

		[ProtoMember(3)]
		public int TotalChengJiuPoint { get; set; }

		[ProtoMember(4)]
		public List<int> AlreadReceivedId = new List<int>();

		[ProtoMember(5)]
		public List<int> JingLingList = new List<int>();

		[ProtoMember(6)]
		public int JingLingId { get; set; }

		[ProtoMember(7)]
		public int RandomDrop { get; set; }

	}

//激活成就
	[Message(OuterOpcode.M2C_ChengJiuActiveMessage)]
	[ProtoContract]
	public partial class M2C_ChengJiuActiveMessage: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int ChengJiuId { get; set; }

	}

	[ResponseType(nameof(M2C_ChengJiuRewardResponse))]
	[Message(OuterOpcode.C2M_ChengJiuRewardRequest)]
	[ProtoContract]
	public partial class C2M_ChengJiuRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RewardId { get; set; }

	}

	[Message(OuterOpcode.M2C_ChengJiuRewardResponse)]
	[ProtoContract]
	public partial class M2C_ChengJiuRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_ChouKaResponse))]
	[Message(OuterOpcode.C2M_ChouKaRequest)]
	[ProtoContract]
	public partial class C2M_ChouKaRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ChouKaType { get; set; }

		[ProtoMember(2)]
		public int ChapterId { get; set; }

	}

	[Message(OuterOpcode.M2C_ChouKaResponse)]
	[ProtoContract]
	public partial class M2C_ChouKaResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RewardItem> RewardList = new List<RewardItem>();

	}

	[ResponseType(nameof(R2C_RankListResponse))]
	[Message(OuterOpcode.C2R_RankListRequest)]
	[ProtoContract]
	public partial class C2R_RankListRequest: Object, IRankActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.R2C_RankListResponse)]
	[ProtoContract]
	public partial class R2C_RankListResponse: Object, IRankActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RankingInfo> RankList = new List<RankingInfo>();

	}

	[ResponseType(nameof(R2C_RankShowLieResponse))]
	[Message(OuterOpcode.C2R_RankShowLieRequest)]
	[ProtoContract]
	public partial class C2R_RankShowLieRequest: Object, IRankActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.R2C_RankShowLieResponse)]
	[ProtoContract]
	public partial class R2C_RankShowLieResponse: Object, IRankActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RankShouLieInfo> RankList = new List<RankShouLieInfo>();

	}

	[Message(OuterOpcode.RankingInfo)]
	[ProtoContract]
	public partial class RankingInfo: Object
	{
		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public string PlayerName { get; set; }

		[ProtoMember(3)]
		public int PlayerLv { get; set; }

		[ProtoMember(4)]
		public long Combat { get; set; }

		[ProtoMember(5)]
		public int Occ { get; set; }

	}

	[Message(OuterOpcode.RankShouLieInfo)]
	[ProtoContract]
	public partial class RankShouLieInfo: Object
	{
		[ProtoMember(1)]
		public long UnitID { get; set; }

		[ProtoMember(2)]
		public string PlayerName { get; set; }

		[ProtoMember(3)]
		public long KillNumber { get; set; }

		[ProtoMember(4)]
		public int Occ { get; set; }

	}

	[Message(OuterOpcode.RankPetInfo)]
	[ProtoContract]
	public partial class RankPetInfo: Object
	{
		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public string PlayerName { get; set; }

		[ProtoMember(3)]
		public string TeamName { get; set; }

		[ProtoMember(4)]
		public int RankId { get; set; }

		[ProtoMember(5)]
		public List<int> PetConfigId = new List<int>();

		[ProtoMember(6)]
		public List<long> PetUId = new List<long>();

	}

	[ResponseType(nameof(R2C_RankPetListResponse))]
	[Message(OuterOpcode.C2R_RankPetListRequest)]
	[ProtoContract]
	public partial class C2R_RankPetListRequest: Object, IRankActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(94)]
		public long UserId { get; set; }

	}

	[Message(OuterOpcode.R2C_RankPetListResponse)]
	[ProtoContract]
	public partial class R2C_RankPetListResponse: Object, IRankActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int LeftCombatTime { get; set; }

		[ProtoMember(2)]
		public List<RankPetInfo> RankPetList = new List<RankPetInfo>();

		[ProtoMember(3)]
		public long RankNumber { get; set; }

	}

	[ResponseType(nameof(M2C_RankPetCombatResponse))]
	[Message(OuterOpcode.C2M_RankPetCombatRequest)]
	[ProtoContract]
	public partial class C2M_RankPetCombatRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.M2C_RankPetCombatResponse)]
	[ProtoContract]
	public partial class M2C_RankPetCombatResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_QuitPetRankResponse))]
	[Message(OuterOpcode.C2M_QuitPetRankRequest)]
	[ProtoContract]
	public partial class C2M_QuitPetRankRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MapIndex { get; set; }

	}

	[Message(OuterOpcode.M2C_QuitPetRankResponse)]
	[ProtoContract]
	public partial class M2C_QuitPetRankResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_PaiMaiShopResponse))]
	[Message(OuterOpcode.C2M_PaiMaiShopRequest)]
	[ProtoContract]
	public partial class C2M_PaiMaiShopRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int PaiMaiId { get; set; }

		[ProtoMember(2)]
		public int BuyNum { get; set; }

	}

	[Message(OuterOpcode.M2C_PaiMaiShopResponse)]
	[ProtoContract]
	public partial class M2C_PaiMaiShopResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2C_PaiMaiShopShowListResponse))]
	[Message(OuterOpcode.C2P_PaiMaiShopShowListRequest)]
	[ProtoContract]
	public partial class C2P_PaiMaiShopShowListRequest: Object, IPaiMaiListRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.P2C_PaiMaiShopShowListResponse)]
	[ProtoContract]
	public partial class P2C_PaiMaiShopShowListResponse: Object, IPaiMaiListResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<PaiMaiShopItemInfo> PaiMaiShopItemInfos = new List<PaiMaiShopItemInfo>();

	}

	[ResponseType(nameof(M2C_PaiMaiBuyResponse))]
	[Message(OuterOpcode.C2M_PaiMaiBuyRequest)]
	[ProtoContract]
	public partial class C2M_PaiMaiBuyRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

		[ProtoMember(2)]
		public int BuyNum { get; set; }

		[ProtoMember(3)]
		public int IsRecharge { get; set; }

	}

	[Message(OuterOpcode.M2C_PaiMaiBuyResponse)]
	[ProtoContract]
	public partial class M2C_PaiMaiBuyResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_PaiMaiSellResponse))]
	[Message(OuterOpcode.C2M_PaiMaiSellRequest)]
	[ProtoContract]
	public partial class C2M_PaiMaiSellRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

		[ProtoMember(3)]
		public int IsRecharge { get; set; }

	}

	[Message(OuterOpcode.M2C_PaiMaiSellResponse)]
	[ProtoContract]
	public partial class M2C_PaiMaiSellResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

	}

	[Message(OuterOpcode.PaiMaiItemInfo)]
	[ProtoContract]
	public partial class PaiMaiItemInfo: Object
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public long UserId { get; set; }

		[ProtoMember(3)]
		public BagInfo BagInfo { get; set; }

		[ProtoMember(5)]
		public int Price { get; set; }

		[ProtoMember(6)]
		public string PlayerName { get; set; }

		[ProtoMember(7)]
		public long SellTime { get; set; }

		[ProtoMember(8)]
		public string Account { get; set; }

		[ProtoMember(9)]
		public int Recharget { get; set; }

	}

	[Message(OuterOpcode.PaiMaiShopItemInfo)]
	[ProtoContract]
	public partial class PaiMaiShopItemInfo: Object
	{
		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public int ItemNumber { get; set; }

		[ProtoMember(3)]
		public int PriceType { get; set; }

		[ProtoMember(4)]
		public int Price { get; set; }

		[ProtoMember(5)]
		public float PricePro { get; set; }

		[ProtoMember(6)]
		public int BuyNum { get; set; }

	}

	[ResponseType(nameof(P2C_PaiMaiListResponse))]
	[Message(OuterOpcode.C2P_PaiMaiListRequest)]
	[ProtoContract]
	public partial class C2P_PaiMaiListRequest: Object, IPaiMaiListRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public int PaiMaiType { get; set; }

		[ProtoMember(3)]
		public int Page { get; set; }

		[ProtoMember(4)]
		public int PaiMaiShowType { get; set; }

	}

	[Message(OuterOpcode.P2C_PaiMaiListResponse)]
	[ProtoContract]
	public partial class P2C_PaiMaiListResponse: Object, IPaiMaiListResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<PaiMaiItemInfo> PaiMaiItemInfos = new List<PaiMaiItemInfo>();

		[ProtoMember(2)]
		public int NextPage { get; set; }

		[ProtoMember(3)]
		public long PaiMaiCostGoldToday { get; set; }

	}

	[ResponseType(nameof(M2C_PaiMaiXiaJiaResponse))]
	[Message(OuterOpcode.C2M_PaiMaiXiaJiaRequest)]
	[ProtoContract]
	public partial class C2M_PaiMaiXiaJiaRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ItemType { get; set; }

		[ProtoMember(2)]
		public long PaiMaiItemInfoId { get; set; }

	}

	[Message(OuterOpcode.M2C_PaiMaiXiaJiaResponse)]
	[ProtoContract]
	public partial class M2C_PaiMaiXiaJiaResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//摆摊
	[ResponseType(nameof(M2C_StallOperationResponse))]
	[Message(OuterOpcode.C2M_StallOperationRequest)]
	[ProtoContract]
	public partial class C2M_StallOperationRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int StallType { get; set; }

		[ProtoMember(2)]
		public string Value { get; set; }

	}

	[Message(OuterOpcode.M2C_StallOperationResponse)]
	[ProtoContract]
	public partial class M2C_StallOperationResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_PaiMaiDuiHuanResponse))]
	[Message(OuterOpcode.C2M_PaiMaiDuiHuanRequest)]
	[ProtoContract]
	public partial class C2M_PaiMaiDuiHuanRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long DiamondsNumber { get; set; }

	}

	[Message(OuterOpcode.M2C_PaiMaiDuiHuanResponse)]
	[ProtoContract]
	public partial class M2C_PaiMaiDuiHuanResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.MysteryItemInfo)]
	[ProtoContract]
	public partial class MysteryItemInfo: Object
	{
		[ProtoMember(1)]
		public int MysteryId { get; set; }

		[ProtoMember(3)]
		public int ItemID { get; set; }

		[ProtoMember(4)]
		public int ItemNumber { get; set; }

		[ProtoMember(5)]
		public int ProductId { get; set; }

	}

	[ResponseType(nameof(M2C_MysteryBuyResponse))]
	[Message(OuterOpcode.C2M_MysteryBuyRequest)]
	[ProtoContract]
	public partial class C2M_MysteryBuyRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public MysteryItemInfo MysteryItemInfo { get; set; }

		[ProtoMember(2)]
		public int NpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_MysteryBuyResponse)]
	[ProtoContract]
	public partial class M2C_MysteryBuyResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Actor_FubenMoNengResponse))]
	[Message(OuterOpcode.Actor_FubenMoNengRequest)]
	[ProtoContract]
	public partial class Actor_FubenMoNengRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.Actor_FubenMoNengResponse)]
	[ProtoContract]
	public partial class Actor_FubenMoNengResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<MysteryItemInfo> MysteryItemInfos = new List<MysteryItemInfo>();

	}

	[ResponseType(nameof(M2C_RolePetChouKaResponse))]
	[Message(OuterOpcode.C2M_RolePetChouKaRequest)]
	[ProtoContract]
	public partial class C2M_RolePetChouKaRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ChouKaType { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetChouKaResponse)]
	[ProtoContract]
	public partial class M2C_RolePetChouKaResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public RolePetInfo RolePetInfo { get; set; }

	}

	[ResponseType(nameof(M2C_EnergyReceiveResponse))]
	[Message(OuterOpcode.C2M_EnergyReceiveRequest)]
	[ProtoContract]
	public partial class C2M_EnergyReceiveRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RewardType { get; set; }

	}

	[Message(OuterOpcode.M2C_EnergyReceiveResponse)]
	[ProtoContract]
	public partial class M2C_EnergyReceiveResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//答题
	[ResponseType(nameof(M2C_EnergyAnswerResponse))]
	[Message(OuterOpcode.C2M_EnergyAnswerRequest)]
	[ProtoContract]
	public partial class C2M_EnergyAnswerRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int AnswerIndex { get; set; }

		[ProtoMember(2)]
		public int QuestionId { get; set; }

	}

	[Message(OuterOpcode.M2C_EnergyAnswerResponse)]
	[ProtoContract]
	public partial class M2C_EnergyAnswerResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_EnergyInfoResponse))]
	[Message(OuterOpcode.C2M_EnergyInfoRequest)]
	[ProtoContract]
	public partial class C2M_EnergyInfoRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_EnergyInfoResponse)]
	[ProtoContract]
	public partial class M2C_EnergyInfoResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<int> GetRewards = new List<int>();

		[ProtoMember(2)]
		public List<int> QuestionList = new List<int>();

		[ProtoMember(3)]
		public int QuestionIndex { get; set; }

	}

//开启宝箱
	[ResponseType(nameof(Actor_OpenBoxResponse))]
	[Message(OuterOpcode.Actor_OpenBoxRequest)]
	[ProtoContract]
	public partial class Actor_OpenBoxRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.Actor_OpenBoxResponse)]
	[ProtoContract]
	public partial class Actor_OpenBoxResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//GM数据
	[ResponseType(nameof(C2C_GMInfoResponse))]
	[Message(OuterOpcode.C2C_GMInfoRequest)]
	[ProtoContract]
	public partial class C2C_GMInfoRequest: Object, ICenterActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

	}

	[Message(OuterOpcode.C2C_GMInfoResponse)]
	[ProtoContract]
	public partial class C2C_GMInfoResponse: Object, ICenterActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int OnLineNumber { get; set; }

		[ProtoMember(2)]
		public int OnLineRobot { get; set; }

	}

//GM后台指令
	[ResponseType(nameof(C2C_GMCommonResponse))]
	[Message(OuterOpcode.C2C_GMCommonRequest)]
	[ProtoContract]
	public partial class C2C_GMCommonRequest: Object, ICenterActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Context { get; set; }

	}

	[Message(OuterOpcode.C2C_GMCommonResponse)]
	[ProtoContract]
	public partial class C2C_GMCommonResponse: Object, ICenterActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//GM邮件
	[ResponseType(nameof(E2C_GMEMailResponse))]
	[Message(OuterOpcode.C2E_GMEMailRequest)]
	[ProtoContract]
	public partial class C2E_GMEMailRequest: Object, IMailActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public string MailInfo { get; set; }

	}

	[Message(OuterOpcode.E2C_GMEMailResponse)]
	[ProtoContract]
	public partial class E2C_GMEMailResponse: Object, IMailActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_DBServerInfoResponse))]
	[Message(OuterOpcode.C2R_DBServerInfoRequest)]
	[ProtoContract]
	public partial class C2R_DBServerInfoRequest: Object, IRankActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.R2C_DBServerInfoResponse)]
	[ProtoContract]
	public partial class R2C_DBServerInfoResponse: Object, IRankActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public ServerInfo ServerInfo { get; set; }

	}

	[ResponseType(nameof(A2C_MysteryListResponse))]
	[Message(OuterOpcode.C2A_MysteryListRequest)]
	[ProtoContract]
	public partial class C2A_MysteryListRequest: Object, IActivityActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserId { get; set; }

	}

	[Message(OuterOpcode.A2C_MysteryListResponse)]
	[ProtoContract]
	public partial class A2C_MysteryListResponse: Object, IActivityActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<MysteryItemInfo> MysteryItemInfos = new List<MysteryItemInfo>();

	}

//领取奖励
	[ResponseType(nameof(M2C_ActivityReceiveResponse))]
	[Message(OuterOpcode.C2M_ActivityReceiveRequest)]
	[ProtoContract]
	public partial class C2M_ActivityReceiveRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ActivityType { get; set; }

		[ProtoMember(2)]
		public int ActivityId { get; set; }

		[ProtoMember(3)]
		public int ReceiveIndex { get; set; }

	}

	[Message(OuterOpcode.M2C_ActivityReceiveResponse)]
	[ProtoContract]
	public partial class M2C_ActivityReceiveResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//活动信息
	[ResponseType(nameof(M2C_ActivityInfoResponse))]
	[Message(OuterOpcode.C2M_ActivityInfoRequest)]
	[ProtoContract]
	public partial class C2M_ActivityInfoRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ActivityType { get; set; }

		[ProtoMember(2)]
		public int ActivityId { get; set; }

	}

	[Message(OuterOpcode.M2C_ActivityInfoResponse)]
	[ProtoContract]
	public partial class M2C_ActivityInfoResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<int> ReceiveIds = new List<int>();

		[ProtoMember(3)]
		public long LastSignTime { get; set; }

		[ProtoMember(4)]
		public int TotalSignNumber { get; set; }

		[ProtoMember(5)]
		public List<TokenRecvive> QuTokenRecvive = new List<TokenRecvive>();

		[ProtoMember(6)]
		public long LastLoginTime { get; set; }

		[ProtoMember(7)]
		public List<int> DayTeHui = new List<int>();

		[ProtoMember(8)]
		public ActivityV1Info ActivityV1Info { get; set; }

	}

//战区活动
	[ResponseType(nameof(M2C_ZhanQuReceiveResponse))]
	[Message(OuterOpcode.C2M_ZhanQuReceiveRequest)]
	[ProtoContract]
	public partial class C2M_ZhanQuReceiveRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ActivityType { get; set; }

		[ProtoMember(2)]
		public int ActivityId { get; set; }

		[ProtoMember(3)]
		public int ReceiveIndex { get; set; }

	}

	[Message(OuterOpcode.M2C_ZhanQuReceiveResponse)]
	[ProtoContract]
	public partial class M2C_ZhanQuReceiveResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_ZhanQuInfoResponse))]
	[Message(OuterOpcode.C2M_ZhanQuInfoRequest)]
	[ProtoContract]
	public partial class C2M_ZhanQuInfoRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_ZhanQuInfoResponse)]
	[ProtoContract]
	public partial class M2C_ZhanQuInfoResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<int> ReceiveIds = new List<int>();

		[ProtoMember(2)]
		public List<ZhanQuReceiveNumber> ReceiveNum = new List<ZhanQuReceiveNumber>();

	}

//战区领取记录
	[Message(OuterOpcode.ZhanQuReceiveNumber)]
	[ProtoContract]
	public partial class ZhanQuReceiveNumber: Object
	{
		[ProtoMember(1)]
		public int ActivityId { get; set; }

		[ProtoMember(2)]
		public int ReceiveNum { get; set; }

		[ProtoMember(3)]
		public List<long> ReceiveUnitIds = new List<long>();

	}

	[Message(OuterOpcode.TokenRecvive)]
	[ProtoContract]
	public partial class TokenRecvive: Object
	{
		[ProtoMember(1)]
		public int ActivityId { get; set; }

		[ProtoMember(2)]
		public int ReceiveIndex { get; set; }

	}

	[Message(OuterOpcode.TeamInfo)]
	[ProtoContract]
	public partial class TeamInfo: Object
	{
		[ProtoMember(1)]
		public int SceneId { get; set; }

		[ProtoMember(2)]
		public List<TeamPlayerInfo> PlayerList = new List<TeamPlayerInfo>();

		[ProtoMember(3)]
		public long TeamId { get; set; }

		[ProtoMember(4)]
		public long FubenInstanceId { get; set; }

		[ProtoMember(5)]
		public long FubenUUId { get; set; }

		[ProtoMember(6)]
		public int FubenType { get; set; }

	}

	[Message(OuterOpcode.TeamPlayerInfo)]
	[ProtoContract]
	public partial class TeamPlayerInfo: Object
	{
		[ProtoMember(1)]
		public int HeadId { get; set; }

		[ProtoMember(2)]
		public int PlayerLv { get; set; }

		[ProtoMember(3)]
		public int WeaponId { get; set; }

		[ProtoMember(4)]
		public string PlayerName { get; set; }

		[ProtoMember(5)]
		public long UserID { get; set; }

		[ProtoMember(6)]
		public int Damage { get; set; }

		[ProtoMember(7)]
		public long Combat { get; set; }

		[ProtoMember(8)]
		public int Occ { get; set; }

		[ProtoMember(9)]
		public int InFuben { get; set; }

		[ProtoMember(10)]
		public int RobotId { get; set; }

		[ProtoMember(11)]
		public int OccTwo { get; set; }

		[ProtoMember(12)]
		public int Prepare { get; set; }

		[ProtoMember(13)]
		public List<int> FashionIds = new List<int>();

	}

//邀请组队
	[ResponseType(nameof(T2C_TeamInviteResponse))]
	[Message(OuterOpcode.C2T_TeamInviteRequest)]
	[ProtoContract]
	public partial class C2T_TeamInviteRequest: Object, ITeamActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

		[ProtoMember(2)]
		public TeamPlayerInfo TeamPlayerInfo { get; set; }

	}

	[Message(OuterOpcode.T2C_TeamInviteResponse)]
	[ProtoContract]
	public partial class T2C_TeamInviteResponse: Object, ITeamActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//同意组队
	[ResponseType(nameof(T2C_TeamAgreeResponse))]
	[Message(OuterOpcode.C2T_TeamAgreeRequest)]
	[ProtoContract]
	public partial class C2T_TeamAgreeRequest: Object, ITeamActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public TeamPlayerInfo TeamPlayerInfo_1 { get; set; }

		[ProtoMember(2)]
		public TeamPlayerInfo TeamPlayerInfo_2 { get; set; }

	}

	[Message(OuterOpcode.T2C_TeamAgreeResponse)]
	[ProtoContract]
	public partial class T2C_TeamAgreeResponse: Object, ITeamActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//离开组队
	[ResponseType(nameof(T2C_TeamLeaveResponse))]
	[Message(OuterOpcode.C2T_TeamLeaveRequest)]
	[ProtoContract]
	public partial class C2T_TeamLeaveRequest: Object, ITeamActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserId { get; set; }

	}

	[Message(OuterOpcode.T2C_TeamLeaveResponse)]
	[ProtoContract]
	public partial class T2C_TeamLeaveResponse: Object, ITeamActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//踢出队伍
	[ResponseType(nameof(T2C_TeamKickOutResponse))]
	[Message(OuterOpcode.C2T_TeamKickOutRequest)]
	[ProtoContract]
	public partial class C2T_TeamKickOutRequest: Object, ITeamActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserId { get; set; }

	}

	[Message(OuterOpcode.T2C_TeamKickOutResponse)]
	[ProtoContract]
	public partial class T2C_TeamKickOutResponse: Object, ITeamActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//组队副本
	[ResponseType(nameof(T2C_TeamDungeonInfoResponse))]
	[Message(OuterOpcode.C2T_TeamDungeonInfoRequest)]
	[ProtoContract]
	public partial class C2T_TeamDungeonInfoRequest: Object, ITeamActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserId { get; set; }

	}

	[Message(OuterOpcode.T2C_TeamDungeonInfoResponse)]
	[ProtoContract]
	public partial class T2C_TeamDungeonInfoResponse: Object, ITeamActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<TeamInfo> TeamList = new List<TeamInfo>();

	}

//组队副本申请
	[ResponseType(nameof(T2C_TeamDungeonApplyResponse))]
	[Message(OuterOpcode.C2T_TeamDungeonApplyRequest)]
	[ProtoContract]
	public partial class C2T_TeamDungeonApplyRequest: Object, ITeamActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long TeamId { get; set; }

		[ProtoMember(2)]
		public TeamPlayerInfo TeamPlayerInfo { get; set; }

	}

	[Message(OuterOpcode.T2C_TeamDungeonApplyResponse)]
	[ProtoContract]
	public partial class T2C_TeamDungeonApplyResponse: Object, ITeamActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//同意组队副本申请
	[ResponseType(nameof(T2C_TeamDungeonAgreeResponse))]
	[Message(OuterOpcode.C2T_TeamDungeonAgreeRequest)]
	[ProtoContract]
	public partial class C2T_TeamDungeonAgreeRequest: Object, ITeamActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long TeamId { get; set; }

		[ProtoMember(2)]
		public TeamPlayerInfo TeamPlayerInfo { get; set; }

	}

	[Message(OuterOpcode.T2C_TeamDungeonAgreeResponse)]
	[ProtoContract]
	public partial class T2C_TeamDungeonAgreeResponse: Object, ITeamActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//创建组队副本
	[ResponseType(nameof(M2C_TeamDungeonCreateResponse))]
	[Message(OuterOpcode.C2M_TeamDungeonCreateRequest)]
	[ProtoContract]
	public partial class C2M_TeamDungeonCreateRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int FubenId { get; set; }

		[ProtoMember(2)]
		public TeamPlayerInfo TeamPlayerInfo { get; set; }

		[ProtoMember(3)]
		public int FubenType { get; set; }

	}

	[Message(OuterOpcode.M2C_TeamDungeonCreateResponse)]
	[ProtoContract]
	public partial class M2C_TeamDungeonCreateResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public TeamInfo TeamInfo { get; set; }

		[ProtoMember(3)]
		public int FubenType { get; set; }

	}

//开启组队副本
	[ResponseType(nameof(M2C_TeamDungeonOpenResponse))]
	[Message(OuterOpcode.C2M_TeamDungeonOpenRequest)]
	[ProtoContract]
	public partial class C2M_TeamDungeonOpenRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

		[ProtoMember(2)]
		public int FubenType { get; set; }

	}

	[Message(OuterOpcode.M2C_TeamDungeonOpenResponse)]
	[ProtoContract]
	public partial class M2C_TeamDungeonOpenResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(3)]
		public int FubenType { get; set; }

	}

//副本开启
	[Message(OuterOpcode.M2C_TeamDungeonOpenResult)]
	[ProtoContract]
	public partial class M2C_TeamDungeonOpenResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public TeamInfo TeamInfo { get; set; }

	}

//请求准备
	[ResponseType(nameof(M2C_TeamDungeonPrepareResponse))]
	[Message(OuterOpcode.C2M_TeamDungeonPrepareRequest)]
	[ProtoContract]
	public partial class C2M_TeamDungeonPrepareRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public TeamInfo TeamInfo { get; set; }

		[ProtoMember(2)]
		public int Prepare { get; set; }

	}

	[Message(OuterOpcode.M2C_TeamDungeonPrepareResponse)]
	[ProtoContract]
	public partial class M2C_TeamDungeonPrepareResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//玩家准备
	[Message(OuterOpcode.M2C_TeamDungeonPrepareResult)]
	[ProtoContract]
	public partial class M2C_TeamDungeonPrepareResult: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public TeamInfo TeamInfo { get; set; }

		[ProtoMember(2)]
		public int ErrorCode { get; set; }

	}

//退出组队广播
	[Message(OuterOpcode.M2C_TeamDungeonQuitMessage)]
	[ProtoContract]
	public partial class M2C_TeamDungeonQuitMessage: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//副本选择奖励
	[ResponseType(nameof(M2C_TeamDungeonBoxRewardResponse))]
	[Message(OuterOpcode.C2M_TeamDungeonBoxRewardRequest)]
	[ProtoContract]
	public partial class C2M_TeamDungeonBoxRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int BoxIndex { get; set; }

		[ProtoMember(2)]
		public RewardItem RewardItem { get; set; }

	}

	[Message(OuterOpcode.M2C_TeamDungeonBoxRewardResponse)]
	[ProtoContract]
	public partial class M2C_TeamDungeonBoxRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int Mail { get; set; }

	}

	[Message(OuterOpcode.M2C_TeamDungeonBoxRewardResult)]
	[ProtoContract]
	public partial class M2C_TeamDungeonBoxRewardResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public int BoxIndex { get; set; }

		[ProtoMember(3)]
		public string PlayerName { get; set; }

	}

//组队副本结算
	[Message(OuterOpcode.M2C_TeamDungeonSettlement)]
	[ProtoContract]
	public partial class M2C_TeamDungeonSettlement: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long PassTime { get; set; }

		[ProtoMember(2)]
		public List<TeamPlayerInfo> PlayerList = new List<TeamPlayerInfo>();

		[ProtoMember(4)]
		public List<RewardItem> RewardExtraItem = new List<RewardItem>();

		[ProtoMember(5)]
		public List<RewardItem> ReardList = new List<RewardItem>();

		[ProtoMember(6)]
		public List<RewardItem> ReardListExcess = new List<RewardItem>();

		[ProtoMember(7)]
		public int Star { get; set; }

	}

	[Message(OuterOpcode.M2C_TeamInviteResult)]
	[ProtoContract]
	public partial class M2C_TeamInviteResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public TeamPlayerInfo TeamPlayerInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_TeamDungeonApplyResult)]
	[ProtoContract]
	public partial class M2C_TeamDungeonApplyResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public TeamPlayerInfo TeamPlayerInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_TeamUpdateResult)]
	[ProtoContract]
	public partial class M2C_TeamUpdateResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public TeamInfo TeamInfo { get; set; }

	}

	[Message(OuterOpcode.ChatUnitInfo)]
	[ProtoContract]
	public partial class ChatUnitInfo: Object
	{
		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public long UnionId { get; set; }

		[ProtoMember(6)]
		public long GateSessionActorId { get; set; }

	}

	[ResponseType(nameof(F2C_WatchPlayerResponse))]
	[Message(OuterOpcode.C2F_WatchPlayerRequest)]
	[ProtoContract]
	public partial class C2F_WatchPlayerRequest: Object, IFriendActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public int WatchType { get; set; }

	}

	[Message(OuterOpcode.F2C_WatchPlayerResponse)]
	[ProtoContract]
	public partial class F2C_WatchPlayerResponse: Object, IFriendActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Name { get; set; }

		[ProtoMember(2)]
		public int Lv { get; set; }

		[ProtoMember(3)]
		public List<BagInfo> EquipList = new List<BagInfo>();

		[ProtoMember(4)]
		public long TeamId { get; set; }

		[ProtoMember(5)]
		public int Occ { get; set; }

		[ProtoMember(7)]
		public List<RolePetInfo> RolePetInfos = new List<RolePetInfo>();

		[ProtoMember(8)]
		public List<KeyValuePair> PetSkinList = new List<KeyValuePair>();

		[ProtoMember(9)]
		public List<BagInfo> PetHeXinList = new List<BagInfo>();

		[ProtoMember(11)]
		public List<int> Ks = new List<int>();

		[ProtoMember(12)]
		public List<long> Vs = new List<long>();

		[ProtoMember(13)]
		public List<int> FashionIds = new List<int>();

	}

//好友列表
	[ResponseType(nameof(F2C_FriendInfoResponse))]
	[Message(OuterOpcode.C2F_FriendInfoRequest)]
	[ProtoContract]
	public partial class C2F_FriendInfoRequest: Object, IFriendActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

	}

	[Message(OuterOpcode.F2C_FriendInfoResponse)]
	[ProtoContract]
	public partial class F2C_FriendInfoResponse: Object, IFriendActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public List<FriendInfo> FriendList = new List<FriendInfo>();

		[ProtoMember(3)]
		public List<FriendInfo> ApplyList = new List<FriendInfo>();

		[ProtoMember(4)]
		public List<FriendInfo> Blacklist = new List<FriendInfo>();

		[ProtoMember(5)]
		public List<ChatInfo> FriendChats = new List<ChatInfo>();

	}

//好友申请
	[ResponseType(nameof(F2C_FriendApplyResponse))]
	[Message(OuterOpcode.C2F_FriendApplyRequest)]
	[ProtoContract]
	public partial class C2F_FriendApplyRequest: Object, IFriendActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long UserID { get; set; }

		[ProtoMember(1)]
		public FriendInfo RoleInfo { get; set; }

	}

	[Message(OuterOpcode.F2C_FriendApplyResponse)]
	[ProtoContract]
	public partial class F2C_FriendApplyResponse: Object, IFriendActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//黑名单
	[ResponseType(nameof(F2C_FriendBlacklistResponse))]
	[Message(OuterOpcode.C2F_FriendBlacklistRequest)]
	[ProtoContract]
	public partial class C2F_FriendBlacklistRequest: Object, IFriendActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public long UserID { get; set; }

		[ProtoMember(3)]
		public long FriendId { get; set; }

	}

	[Message(OuterOpcode.F2C_FriendBlacklistResponse)]
	[ProtoContract]
	public partial class F2C_FriendBlacklistResponse: Object, IFriendActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.M2C_FriendApplyResult)]
	[ProtoContract]
	public partial class M2C_FriendApplyResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public FriendInfo FriendInfo { get; set; }

	}

//好友申请回复
	[ResponseType(nameof(F2C_FriendApplyReplyResponse))]
	[Message(OuterOpcode.C2F_FriendApplyReplyRequest)]
	[ProtoContract]
	public partial class C2F_FriendApplyReplyRequest: Object, IFriendActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

		[ProtoMember(2)]
		public long FriendID { get; set; }

		[ProtoMember(3)]
		public int ReplyCode { get; set; }

	}

	[Message(OuterOpcode.F2C_FriendApplyReplyResponse)]
	[ProtoContract]
	public partial class F2C_FriendApplyReplyResponse: Object, IFriendActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//好友删除
	[ResponseType(nameof(F2C_FriendDeleteResponse))]
	[Message(OuterOpcode.C2F_FriendDeleteRequest)]
	[ProtoContract]
	public partial class C2F_FriendDeleteRequest: Object, IFriendActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

		[ProtoMember(2)]
		public long FriendID { get; set; }

	}

	[Message(OuterOpcode.F2C_FriendDeleteResponse)]
	[ProtoContract]
	public partial class F2C_FriendDeleteResponse: Object, IFriendActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.FriendInfo)]
	[ProtoContract]
	public partial class FriendInfo: Object
	{
		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public int PlayerLevel { get; set; }

		[ProtoMember(3)]
		public string PlayerName { get; set; }

		[ProtoMember(4)]
		public long OnLineTime { get; set; }

		[ProtoMember(5)]
		public List<string> ChatMsgList = new List<string>();

		[ProtoMember(6)]
		public int Occ { get; set; }

	}

	[ResponseType(nameof(M2C_UserInfoInitResponse))]
	[Message(OuterOpcode.C2M_UserInfoRequest)]
	[ProtoContract]
	public partial class C2M_UserInfoRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_UserInfoInitResponse)]
	[ProtoContract]
	public partial class M2C_UserInfoInitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public UserInfo UserInfo { get; set; }

		[ProtoMember(3)]
		public List<KeyValuePair> ReddontList = new List<KeyValuePair>();

		[ProtoMember(4)]
		public List<KeyValuePairInt> TreasureInfo = new List<KeyValuePairInt>();

		[ProtoMember(5)]
		public List<ShouJiChapterInfo> ShouJiChapterInfos = new List<ShouJiChapterInfo>();

		[ProtoMember(6)]
		public List<KeyValuePairInt> TitleList = new List<KeyValuePairInt>();

	}

	[Message(OuterOpcode.RechargeInfo)]
	[ProtoContract]
	public partial class RechargeInfo: Object
	{
		[ProtoMember(1)]
		public int Amount { get; set; }

		[ProtoMember(2)]
		public long Time { get; set; }

		[ProtoMember(3)]
		public long UserId { get; set; }

		[ProtoMember(4)]
		public string OrderInfo { get; set; }

		[ProtoMember(5)]
		public int RechargeType { get; set; }

	}

	[Message(OuterOpcode.ShouJiChapterInfo)]
	[ProtoContract]
	public partial class ShouJiChapterInfo: Object
	{
		[ProtoMember(1)]
		public int ChapterId { get; set; }

		[ProtoMember(2)]
		public int StarNum { get; set; }

		[ProtoMember(3)]
		public int RewardInfo { get; set; }

		[ProtoMember(4)]
		public List<int> ShouJiItemList = new List<int>();

	}

	[Message(OuterOpcode.M2C_GameNotice)]
	[ProtoContract]
	public partial class M2C_GameNotice: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int UpdateType { get; set; }

		[ProtoMember(2)]
		public string UpdateTypeValue { get; set; }

	}

//创建公会
	[ResponseType(nameof(M2C_UnionCreateResponse))]
	[Message(OuterOpcode.C2M_UnionCreateRequest)]
	[ProtoContract]
	public partial class C2M_UnionCreateRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string UnionName { get; set; }

		[ProtoMember(2)]
		public string UnionPurpose { get; set; }

	}

	[Message(OuterOpcode.M2C_UnionCreateResponse)]
	[ProtoContract]
	public partial class M2C_UnionCreateResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

	}

//邀请玩家加入家族
	[Message(OuterOpcode.C2M_UnionInviteRequest)]
	[ProtoContract]
	public partial class C2M_UnionInviteRequest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long InviteId { get; set; }

	}

	[Message(OuterOpcode.M2C_UnionInviteMessage)]
	[ProtoContract]
	public partial class M2C_UnionInviteMessage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public string UnionName { get; set; }

		[ProtoMember(2)]
		public string PlayerName { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public long UnionId { get; set; }

	}

//邀请回复
	[Message(OuterOpcode.C2M_UnionInviteReplyRequest)]
	[ProtoContract]
	public partial class C2M_UnionInviteReplyRequest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public int ReplyCode { get; set; }

	}

//公会列表
	[ResponseType(nameof(U2C_UnionListResponse))]
	[Message(OuterOpcode.C2U_UnionListRequest)]
	[ProtoContract]
	public partial class C2U_UnionListRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionListResponse)]
	[ProtoContract]
	public partial class U2C_UnionListResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<UnionListItem> UnionList = new List<UnionListItem>();

	}

//申请入会
	[ResponseType(nameof(U2C_UnionApplyResponse))]
	[Message(OuterOpcode.C2U_UnionApplyRequest)]
	[ProtoContract]
	public partial class C2U_UnionApplyRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public long UserId { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionApplyResponse)]
	[ProtoContract]
	public partial class U2C_UnionApplyResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//我的公会
	[ResponseType(nameof(U2C_UnionMyInfoResponse))]
	[Message(OuterOpcode.C2U_UnionMyInfoRequest)]
	[ProtoContract]
	public partial class C2U_UnionMyInfoRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionMyInfoResponse)]
	[ProtoContract]
	public partial class U2C_UnionMyInfoResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public UnionInfo UnionMyInfo { get; set; }

		[ProtoMember(2)]
		public List<long> OnLinePlayer = new List<long>();

	}

//捐献记录
	[ResponseType(nameof(U2C_UnionRecordResponse))]
	[Message(OuterOpcode.C2U_UnionRecordRequest)]
	[ProtoContract]
	public partial class C2U_UnionRecordRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionRecordResponse)]
	[ProtoContract]
	public partial class U2C_UnionRecordResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(11)]
		public List<DonationRecord> DonationRecords = new List<DonationRecord>();

	}

//离开公会
	[ResponseType(nameof(M2C_UnionLeaveResponse))]
	[Message(OuterOpcode.C2M_UnionLeaveRequest)]
	[ProtoContract]
	public partial class C2M_UnionLeaveRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_UnionLeaveResponse)]
	[ProtoContract]
	public partial class M2C_UnionLeaveResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//转让族长
	[ResponseType(nameof(M2C_UnionTransferResponse))]
	[Message(OuterOpcode.C2M_UnionTransferRequest)]
	[ProtoContract]
	public partial class C2M_UnionTransferRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long NewLeader { get; set; }

	}

	[Message(OuterOpcode.M2C_UnionTransferResponse)]
	[ProtoContract]
	public partial class M2C_UnionTransferResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//申请列表
	[ResponseType(nameof(U2C_UnionApplyListResponse))]
	[Message(OuterOpcode.C2U_UnionApplyListRequest)]
	[ProtoContract]
	public partial class C2U_UnionApplyListRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionApplyListResponse)]
	[ProtoContract]
	public partial class U2C_UnionApplyListResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(10)]
		public List<UnionPlayerInfo> UnionPlayerList = new List<UnionPlayerInfo>();

	}

//申请回复
	[ResponseType(nameof(U2C_UnionApplyReplyResponse))]
	[Message(OuterOpcode.C2U_UnionApplyReplyRequest)]
	[ProtoContract]
	public partial class C2U_UnionApplyReplyRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public int ReplyCode { get; set; }

		[ProtoMember(3)]
		public long UnionId { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionApplyReplyResponse)]
	[ProtoContract]
	public partial class U2C_UnionApplyReplyResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//公会操作
	[ResponseType(nameof(U2C_UnionOperatateResponse))]
	[Message(OuterOpcode.C2U_UnionOperatateRequest)]
	[ProtoContract]
	public partial class C2U_UnionOperatateRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public long UnionId { get; set; }

		[ProtoMember(3)]
		public int Operatate { get; set; }

		[ProtoMember(4)]
		public string Value { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionOperatateResponse)]
	[ProtoContract]
	public partial class U2C_UnionOperatateResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//公会踢人
	[ResponseType(nameof(U2C_UnionKickOutResponse))]
	[Message(OuterOpcode.C2U_UnionKickOutRequest)]
	[ProtoContract]
	public partial class C2U_UnionKickOutRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public long UserId { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionKickOutResponse)]
	[ProtoContract]
	public partial class U2C_UnionKickOutResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.M2C_UnionApplyResult)]
	[ProtoContract]
	public partial class M2C_UnionApplyResult: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(U2C_DonationRankListResponse))]
	[Message(OuterOpcode.C2U_DonationRankListRequest)]
	[ProtoContract]
	public partial class C2U_DonationRankListRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RankType { get; set; }

	}

	[Message(OuterOpcode.U2C_DonationRankListResponse)]
	[ProtoContract]
	public partial class U2C_DonationRankListResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RankingInfo> RankList = new List<RankingInfo>();

	}

	[ResponseType(nameof(U2C_UnionRaceInfoResponse))]
	[Message(OuterOpcode.C2U_UnionRaceInfoRequest)]
	[ProtoContract]
	public partial class C2U_UnionRaceInfoRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RankType { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionRaceInfoResponse)]
	[ProtoContract]
	public partial class U2C_UnionRaceInfoResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<UnionListItem> UnionInfoList = new List<UnionListItem>();

		[ProtoMember(2)]
		public long TotalDonation { get; set; }

	}

//报名
	[ResponseType(nameof(U2C_UnionSignUpResponse))]
	[Message(OuterOpcode.C2U_UnionSignUpRequest)]
	[ProtoContract]
	public partial class C2U_UnionSignUpRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionSignUpResponse)]
	[ProtoContract]
	public partial class U2C_UnionSignUpResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//家族修炼
	[ResponseType(nameof(M2C_UnionXiuLianResponse))]
	[Message(OuterOpcode.C2M_UnionXiuLianRequest)]
	[ProtoContract]
	public partial class C2M_UnionXiuLianRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Position { get; set; }

		[ProtoMember(2)]
		public int Type { get; set; }

	}

	[Message(OuterOpcode.M2C_UnionXiuLianResponse)]
	[ProtoContract]
	public partial class M2C_UnionXiuLianResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//家族竞选
	[ResponseType(nameof(U2C_UnionJingXuanResponse))]
	[Message(OuterOpcode.C2U_UnionJingXuanRequest)]
	[ProtoContract]
	public partial class C2U_UnionJingXuanRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionJingXuanResponse)]
	[ProtoContract]
	public partial class U2C_UnionJingXuanResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(12)]
		public List<long> JingXuanList = new List<long>();

		[ProtoMember(13)]
		public long JingXuanEndTime { get; set; }

	}

//科技升级
	[ResponseType(nameof(U2C_UnionKeJiActiteResponse))]
	[Message(OuterOpcode.C2U_UnionKeJiActiteRequest)]
	[ProtoContract]
	public partial class C2U_UnionKeJiActiteRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public int Position { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionKeJiActiteResponse)]
	[ProtoContract]
	public partial class U2C_UnionKeJiActiteResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public UnionInfo UnionInfo { get; set; }

	}

//加速完成
	[ResponseType(nameof(U2C_UnionKeJiQuickResponse))]
	[Message(OuterOpcode.C2U_UnionKeJiQuickRequest)]
	[ProtoContract]
	public partial class C2U_UnionKeJiQuickRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public int Position { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionKeJiQuickResponse)]
	[ProtoContract]
	public partial class U2C_UnionKeJiQuickResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public UnionInfo UnionInfo { get; set; }

	}

//科技学习
	[ResponseType(nameof(M2C_UnionKeJiLearnResponse))]
	[Message(OuterOpcode.C2M_UnionKeJiLearnRequest)]
	[ProtoContract]
	public partial class C2M_UnionKeJiLearnRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public int Position { get; set; }

	}

	[Message(OuterOpcode.M2C_UnionKeJiLearnResponse)]
	[ProtoContract]
	public partial class M2C_UnionKeJiLearnResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<int> UnionKeJiList = new List<int>();

	}

	[Message(OuterOpcode.UnionInfo)]
	[ProtoContract]
	public partial class UnionInfo: Object
	{
		[ProtoMember(1)]
		public string UnionName { get; set; }

		[ProtoMember(2)]
		public long LeaderId { get; set; }

		[ProtoMember(3)]
		public string LeaderName { get; set; }

		[ProtoMember(4)]
		public int LevelLimit { get; set; }

		[ProtoMember(5)]
		public string UnionPurpose { get; set; }

		[ProtoMember(6)]
		public List<long> ApplyList = new List<long>();

		[ProtoMember(7)]
		public long UnionId { get; set; }

		[ProtoMember(8)]
		public int Level { get; set; }

		[ProtoMember(9)]
		public int Exp { get; set; }

		[ProtoMember(10)]
		public List<UnionPlayerInfo> UnionPlayerList = new List<UnionPlayerInfo>();

		[ProtoMember(11)]
		public List<DonationRecord> DonationRecords = new List<DonationRecord>();

		[ProtoMember(12)]
		public List<long> JingXuanList = new List<long>();

		[ProtoMember(13)]
		public long JingXuanEndTime { get; set; }

		[ProtoMember(14)]
		public List<int> UnionKeJiList = new List<int>();

		[ProtoMember(15)]
		public int KeJiActitePos { get; set; }

		[ProtoMember(16)]
		public long KeJiActiteTime { get; set; }

		[ProtoMember(17)]
		public long UnionGold { get; set; }

		[ProtoMember(18)]
		public List<string> ActiveRecord = new List<string>();

	}

	[Message(OuterOpcode.DonationRecord)]
	[ProtoContract]
	public partial class DonationRecord: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public long Time { get; set; }

		[ProtoMember(3)]
		public int Gold { get; set; }

		[ProtoMember(4)]
		public string Name { get; set; }

		[ProtoMember(5)]
		public int Occ { get; set; }

		[ProtoMember(6)]
		public int Diamond { get; set; }

	}

	[Message(OuterOpcode.UnionApplyItem)]
	[ProtoContract]
	public partial class UnionApplyItem: Object
	{
		[ProtoMember(1)]
		public string PlayerName { get; set; }

		[ProtoMember(2)]
		public int PlayerLevel { get; set; }

		[ProtoMember(3)]
		public int Combat { get; set; }

	}

	[Message(OuterOpcode.UnionPlayerInfo)]
	[ProtoContract]
	public partial class UnionPlayerInfo: Object
	{
		[ProtoMember(1)]
		public string PlayerName { get; set; }

		[ProtoMember(2)]
		public int PlayerLevel { get; set; }

		[ProtoMember(3)]
		public int Position { get; set; }

		[ProtoMember(4)]
		public long UserID { get; set; }

		[ProtoMember(5)]
		public int Combat { get; set; }

		[ProtoMember(6)]
		public int Occ { get; set; }

		[ProtoMember(7)]
		public int OccTwo { get; set; }

	}

	[Message(OuterOpcode.UnionListItem)]
	[ProtoContract]
	public partial class UnionListItem: Object
	{
		[ProtoMember(1)]
		public string UnionName { get; set; }

		[ProtoMember(2)]
		public long UnionId { get; set; }

		[ProtoMember(3)]
		public int PlayerNumber { get; set; }

		[ProtoMember(4)]
		public int LevelLimit { get; set; }

		[ProtoMember(5)]
		public int UnionLevel { get; set; }

		[ProtoMember(6)]
		public string UnionLeader { get; set; }

	}

	[ResponseType(nameof(M2C_ReddotReadResponse))]
	[Message(OuterOpcode.C2M_ReddotReadRequest)]
	[ProtoContract]
	public partial class C2M_ReddotReadRequest: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int ReddotType { get; set; }

	}

	[Message(OuterOpcode.M2C_ReddotReadResponse)]
	[ProtoContract]
	public partial class M2C_ReddotReadResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_GuideUpdateResponse))]
	[Message(OuterOpcode.C2M_GuideUpdateRequest)]
	[ProtoContract]
	public partial class C2M_GuideUpdateRequest: Object, IActorLocationRequest
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int GuideId { get; set; }

		[ProtoMember(3)]
		public int GuideStep { get; set; }

	}

	[Message(OuterOpcode.M2C_GuideUpdateResponse)]
	[ProtoContract]
	public partial class M2C_GuideUpdateResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_YeWaiSceneResponse))]
	[Message(OuterOpcode.C2M_YeWaiSceneRequest)]
	[ProtoContract]
	public partial class C2M_YeWaiSceneRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int SceneId { get; set; }

	}

	[Message(OuterOpcode.M2C_YeWaiSceneResponse)]
	[ProtoContract]
	public partial class M2C_YeWaiSceneResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_YeWaiSceneQuitResponse))]
	[Message(OuterOpcode.C2M_YeWaiSceneQuitRequest)]
	[ProtoContract]
	public partial class C2M_YeWaiSceneQuitRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MapIndex { get; set; }

	}

	[Message(OuterOpcode.M2C_YeWaiSceneQuitResponse)]
	[ProtoContract]
	public partial class M2C_YeWaiSceneQuitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_ShoujiRewardResponse))]
	[Message(OuterOpcode.C2M_ShoujiRewardRequest)]
	[ProtoContract]
	public partial class C2M_ShoujiRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ChapterId { get; set; }

		[ProtoMember(2)]
		public int RewardIndex { get; set; }

	}

	[Message(OuterOpcode.M2C_ShoujiRewardResponse)]
	[ProtoContract]
	public partial class M2C_ShoujiRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_ShouJiTreasureResponse))]
//收集珍宝
	[Message(OuterOpcode.C2M_ShouJiTreasureRequest)]
	[ProtoContract]
	public partial class C2M_ShouJiTreasureRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ShouJiId { get; set; }

		[ProtoMember(2)]
		public List<long> ItemIds = new List<long>();

	}

	[Message(OuterOpcode.M2C_ShouJiTreasureResponse)]
	[ProtoContract]
	public partial class M2C_ShouJiTreasureResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public int ActiveNum { get; set; }

	}

	[ResponseType(nameof(M2C_LingDiUpResponse))]
	[Message(OuterOpcode.C2M_LingDiUpRequest)]
	[ProtoContract]
	public partial class C2M_LingDiUpRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_LingDiUpResponse)]
	[ProtoContract]
	public partial class M2C_LingDiUpResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_LingDiRewardResponse))]
	[Message(OuterOpcode.C2M_LingDiRewardRequest)]
	[ProtoContract]
	public partial class C2M_LingDiRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RewardId { get; set; }

	}

	[Message(OuterOpcode.M2C_LingDiRewardResponse)]
	[ProtoContract]
	public partial class M2C_LingDiRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_XiuLianCenterResponse))]
	[Message(OuterOpcode.C2M_XiuLianCenterRequest)]
	[ProtoContract]
	public partial class C2M_XiuLianCenterRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int XiuLianType { get; set; }

	}

	[Message(OuterOpcode.M2C_XiuLianCenterResponse)]
	[ProtoContract]
	public partial class M2C_XiuLianCenterResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_PetFubenBeginResponse))]
//宠物副本开始战斗
	[Message(OuterOpcode.C2M_PetFubenBeginRequest)]
	[ProtoContract]
	public partial class C2M_PetFubenBeginRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetFubenBeginResponse)]
	[ProtoContract]
	public partial class M2C_PetFubenBeginResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//宠物副本结束战斗
	[Message(OuterOpcode.C2M_PetFubenOverRequest)]
	[ProtoContract]
	public partial class C2M_PetFubenOverRequest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[ResponseType(nameof(M2C_PetFubenRewardResponse))]
//宠物副本星级奖励
	[Message(OuterOpcode.C2M_PetFubenRewardRequest)]
	[ProtoContract]
	public partial class C2M_PetFubenRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetFubenRewardResponse)]
	[ProtoContract]
	public partial class M2C_PetFubenRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_HongBaoOpenResponse))]
//开启红包
	[Message(OuterOpcode.C2M_HongBaoOpenRequest)]
	[ProtoContract]
	public partial class C2M_HongBaoOpenRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_HongBaoOpenResponse)]
	[ProtoContract]
	public partial class M2C_HongBaoOpenResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int HongbaoAmount { get; set; }

	}

	[ResponseType(nameof(M2C_RandomTowerBeginResponse))]
//随机副本开始战斗
	[Message(OuterOpcode.C2M_RandomTowerBeginRequest)]
	[ProtoContract]
	public partial class C2M_RandomTowerBeginRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RandomNumber { get; set; }

	}

	[Message(OuterOpcode.M2C_RandomTowerBeginResponse)]
	[ProtoContract]
	public partial class M2C_RandomTowerBeginResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_RandomTowerRewardResponse))]
//随机副本领取奖励
	[Message(OuterOpcode.C2M_RandomTowerRewardRequest)]
	[ProtoContract]
	public partial class C2M_RandomTowerRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RewardId { get; set; }

		[ProtoMember(2)]
		public int SceneType { get; set; }

	}

	[Message(OuterOpcode.M2C_RandomTowerRewardResponse)]
	[ProtoContract]
	public partial class M2C_RandomTowerRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_PetHeXinHeChengResponse))]
//宠物之核合成
	[Message(OuterOpcode.C2M_PetHeXinHeChengRequest)]
	[ProtoContract]
	public partial class C2M_PetHeXinHeChengRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long BagInfoID_1 { get; set; }

		[ProtoMember(2)]
		public long BagInfoID_2 { get; set; }

	}

	[Message(OuterOpcode.M2C_PetHeXinHeChengResponse)]
	[ProtoContract]
	public partial class M2C_PetHeXinHeChengResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_ChouKaRewardResponse))]
	[Message(OuterOpcode.C2M_ChouKaRewardRequest)]
	[ProtoContract]
	public partial class C2M_ChouKaRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RewardId { get; set; }

	}

	[Message(OuterOpcode.M2C_ChouKaRewardResponse)]
	[ProtoContract]
	public partial class M2C_ChouKaRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_RoleAddPointResponse))]
	[Message(OuterOpcode.C2M_RoleAddPointRequest)]
	[ProtoContract]
	public partial class C2M_RoleAddPointRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(6)]
		public List<int> PointList = new List<int>();

	}

	[Message(OuterOpcode.M2C_RoleAddPointResponse)]
	[ProtoContract]
	public partial class M2C_RoleAddPointResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(R2C_CampRankListResponse))]
	[Message(OuterOpcode.C2R_CampRankListRequest)]
	[ProtoContract]
	public partial class C2R_CampRankListRequest: Object, IRankActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.R2C_CampRankListResponse)]
	[ProtoContract]
	public partial class R2C_CampRankListResponse: Object, IRankActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<RankingInfo> RankList_1 = new List<RankingInfo>();

		[ProtoMember(2)]
		public List<RankingInfo> RankList_2 = new List<RankingInfo>();

	}

	[ResponseType(nameof(M2C_CampRankSelectResponse))]
	[Message(OuterOpcode.C2M_CampRankSelectRequest)]
	[ProtoContract]
	public partial class C2M_CampRankSelectRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int CampId { get; set; }

	}

	[Message(OuterOpcode.M2C_CampRankSelectResponse)]
	[ProtoContract]
	public partial class M2C_CampRankSelectResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_RoleOpenCangKuResponse))]
	[Message(OuterOpcode.C2M_RoleOpenCangKuRequest)]
	[ProtoContract]
	public partial class C2M_RoleOpenCangKuRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_RoleOpenCangKuResponse)]
	[ProtoContract]
	public partial class M2C_RoleOpenCangKuResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_PetEggDuiHuanResponse))]
	[Message(OuterOpcode.C2M_PetEggDuiHuanRequest)]
	[ProtoContract]
	public partial class C2M_PetEggDuiHuanRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ChouKaId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetEggDuiHuanResponse)]
	[ProtoContract]
	public partial class M2C_PetEggDuiHuanResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<RewardItem> ReardList = new List<RewardItem>();

	}

	[ResponseType(nameof(M2C_PetEggChouKaResponse))]
	[Message(OuterOpcode.C2M_PetEggChouKaRequest)]
	[ProtoContract]
	public partial class C2M_PetEggChouKaRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ChouKaType { get; set; }

	}

	[Message(OuterOpcode.M2C_PetEggChouKaResponse)]
	[ProtoContract]
	public partial class M2C_PetEggChouKaResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<RewardItem> ReardList = new List<RewardItem>();

	}

	[ResponseType(nameof(M2C_RolePetEggPutOut))]
//宠物蛋卸下
	[Message(OuterOpcode.C2M_RolePetEggPutOut)]
	[ProtoContract]
	public partial class C2M_RolePetEggPutOut: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int Index { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetEggPutOut)]
	[ProtoContract]
	public partial class M2C_RolePetEggPutOut: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetEgg RolePetEgg { get; set; }

	}

	[ResponseType(nameof(M2C_PetHeXinHeChengQuickResponse))]
//宠物之核一键合成
	[Message(OuterOpcode.C2M_PetHeXinHeChengQuickRequest)]
	[ProtoContract]
	public partial class C2M_PetHeXinHeChengQuickRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetHeXinHeChengQuickResponse)]
	[ProtoContract]
	public partial class M2C_PetHeXinHeChengQuickResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.ServerInfo)]
	[ProtoContract]
	public partial class ServerInfo: Object
	{
		[ProtoMember(1)]
		public int WorldLv { get; set; }

		[ProtoMember(2)]
		public long ExChangeGold { get; set; }

		[ProtoMember(4)]
		public RankingInfo RankingInfo { get; set; }

		[ProtoMember(5)]
		public int TianQi { get; set; }

		[ProtoMember(6)]
		public bool ShouLieOpen { get; set; }

		[ProtoMember(7)]
		public int ChouKaDropId { get; set; }

	}

	[ResponseType(nameof(R2C_WorldLvResponse))]
	[Message(OuterOpcode.C2R_WorldLvRequest)]
	[ProtoContract]
	public partial class C2R_WorldLvRequest: Object, IRankActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RankType { get; set; }

	}

	[Message(OuterOpcode.R2C_WorldLvResponse)]
	[ProtoContract]
	public partial class R2C_WorldLvResponse: Object, IRankActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public ServerInfo ServerInfo { get; set; }

	}

	[ResponseType(nameof(M2C_ExpToGoldResponse))]
	[Message(OuterOpcode.C2M_ExpToGoldRequest)]
	[ProtoContract]
	public partial class C2M_ExpToGoldRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_ExpToGoldResponse)]
	[ProtoContract]
	public partial class M2C_ExpToGoldResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_MakeSelectResponse))]
	[Message(OuterOpcode.C2M_MakeSelectRequest)]
	[ProtoContract]
	public partial class C2M_MakeSelectRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MakeType { get; set; }

		[ProtoMember(2)]
		public int Plan { get; set; }

	}

	[Message(OuterOpcode.M2C_MakeSelectResponse)]
	[ProtoContract]
	public partial class M2C_MakeSelectResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<int> MakeList = new List<int>();

	}

	[Message(OuterOpcode.FirstWinInfo)]
	[ProtoContract]
	public partial class FirstWinInfo: Object
	{
		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public int FirstWinId { get; set; }

		[ProtoMember(3)]
		public string PlayerName { get; set; }

		[ProtoMember(4)]
		public long KillTime { get; set; }

		[ProtoMember(5)]
		public int Difficulty { get; set; }

	}

	[ResponseType(nameof(A2C_FirstWinInfoResponse))]
	[Message(OuterOpcode.C2A_FirstWinInfoRequest)]
	[ProtoContract]
	public partial class C2A_FirstWinInfoRequest: Object, IActivityActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.A2C_FirstWinInfoResponse)]
	[ProtoContract]
	public partial class A2C_FirstWinInfoResponse: Object, IActivityActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<FirstWinInfo> FirstWinInfos = new List<FirstWinInfo>();

	}

	[ResponseType(nameof(M2C_ItemXiLianRewardResponse))]
	[Message(OuterOpcode.C2M_ItemXiLianRewardRequest)]
	[ProtoContract]
	public partial class C2M_ItemXiLianRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int XiLianId { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemXiLianRewardResponse)]
	[ProtoContract]
	public partial class M2C_ItemXiLianRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_GemHeChengQuickResponse))]
//宝石一键合成
	[Message(OuterOpcode.C2M_GemHeChengQuickRequest)]
	[ProtoContract]
	public partial class C2M_GemHeChengQuickRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int LocType { get; set; }

	}

	[Message(OuterOpcode.M2C_GemHeChengQuickResponse)]
	[ProtoContract]
	public partial class M2C_GemHeChengQuickResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_BuChangeResponse))]
	[Message(OuterOpcode.C2M_BuChangeRequest)]
	[ProtoContract]
	public partial class C2M_BuChangeRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long BuChangId { get; set; }

	}

	[Message(OuterOpcode.M2C_BuChangeResponse)]
	[ProtoContract]
	public partial class M2C_BuChangeResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public PlayerInfo PlayerInfo { get; set; }

		[ProtoMember(2)]
		public int BuChangRecharge { get; set; }

		[ProtoMember(3)]
		public int BuChangDiamond { get; set; }

	}

	[Message(OuterOpcode.ItemXiLianResult)]
	[ProtoContract]
	public partial class ItemXiLianResult: Object
	{
		[ProtoMember(1)]
		public List<HideProList> XiLianHideProLists = new List<HideProList>();

		[ProtoMember(2)]
		public List<int> HideSkillLists = new List<int>();

		[ProtoMember(3)]
		public List<HideProList> XiLianHideTeShuProLists = new List<HideProList>();

	}

	[ResponseType(nameof(M2C_ItemXiLianSelectResponse))]
//洗练装备
	[Message(OuterOpcode.C2M_ItemXiLianSelectRequest)]
	[ProtoContract]
	public partial class C2M_ItemXiLianSelectRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long OperateBagID { get; set; }

		[ProtoMember(1)]
		public ItemXiLianResult ItemXiLianResult { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemXiLianSelectResponse)]
	[ProtoContract]
	public partial class M2C_ItemXiLianSelectResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_ItemXiLianTransferResponse))]
//洗练转移
	[Message(OuterOpcode.C2M_ItemXiLianTransferRequest)]
	[ProtoContract]
	public partial class C2M_ItemXiLianTransferRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long OperateBagID_1 { get; set; }

		[ProtoMember(2)]
		public long OperateBagID_2 { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemXiLianTransferResponse)]
	[ProtoContract]
	public partial class M2C_ItemXiLianTransferResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(G2C_ExitGameGate))]
	[Message(OuterOpcode.C2G_ExitGameGate)]
	[ProtoContract]
	public partial class C2G_ExitGameGate: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Key { get; set; }

		[ProtoMember(2)]
		public long RoleId { get; set; }

		[ProtoMember(3)]
		public long Account { get; set; }

	}

	[Message(OuterOpcode.G2C_ExitGameGate)]
	[ProtoContract]
	public partial class G2C_ExitGameGate: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

// 自己的unit id
		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

	[ResponseType(nameof(M2C_ItemOperateGemResponse))]
	[Message(OuterOpcode.C2M_ItemOperateGemRequest)]
	[ProtoContract]
	public partial class C2M_ItemOperateGemRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public long OperateBagID { get; set; }

		[ProtoMember(3)]
		public string OperatePar { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemOperateGemResponse)]
	[ProtoContract]
	public partial class M2C_ItemOperateGemResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public string OperatePar { get; set; }

	}

	[ResponseType(nameof(M2C_PetDuiHuanResponse))]
	[Message(OuterOpcode.C2M_PetDuiHuanRequest)]
	[ProtoContract]
	public partial class C2M_PetDuiHuanRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetDuiHuanResponse)]
	[ProtoContract]
	public partial class M2C_PetDuiHuanResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetInfo RolePetInfo { get; set; }

	}

//领取充值签到奖励
	[ResponseType(nameof(M2C_ActivityRechargeSignResponse))]
	[Message(OuterOpcode.C2M_ActivityRechargeSignRequest)]
	[ProtoContract]
	public partial class C2M_ActivityRechargeSignRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ActivityType { get; set; }

		[ProtoMember(2)]
		public int ActivityId { get; set; }

	}

	[Message(OuterOpcode.M2C_ActivityRechargeSignResponse)]
	[ProtoContract]
	public partial class M2C_ActivityRechargeSignResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//重连成功刷新Unit
	[Message(OuterOpcode.C2M_RefreshUnitRequest)]
	[ProtoContract]
	public partial class C2M_RefreshUnitRequest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_BattleInfoResult)]
	[ProtoContract]
	public partial class M2C_BattleInfoResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public int CampKill_1 { get; set; }

		[ProtoMember(4)]
		public int CampKill_2 { get; set; }

		[ProtoMember(5)]
		public int SceneType { get; set; }

	}

//紫色道具判断是否需要拾取
	[Message(OuterOpcode.M2C_TeamPickMessage)]
	[ProtoContract]
	public partial class M2C_TeamPickMessage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public List<DropInfo> DropItems = new List<DropInfo>();

	}

	[Message(OuterOpcode.C2M_TeamPickRequest)]
	[ProtoContract]
	public partial class C2M_TeamPickRequest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public DropInfo DropItem { get; set; }

		[ProtoMember(2)]
		public int Need { get; set; }

	}

	[ResponseType(nameof(G2C_LoginRobotResponse))]
	[Message(OuterOpcode.C2G_LoginRobotRequest)]
	[ProtoContract]
	public partial class C2G_LoginRobotRequest: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Key { get; set; }

		[ProtoMember(2)]
		public long RoleId { get; set; }

		[ProtoMember(3)]
		public long Account { get; set; }

	}

	[Message(OuterOpcode.G2C_LoginRobotResponse)]
	[ProtoContract]
	public partial class G2C_LoginRobotResponse: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

// 自己的unit id
		[ProtoMember(1)]
		public long PlayerId { get; set; }

	}

//试炼副本结束
	[ResponseType(nameof(M2C_TrialDungeonFinishResponse))]
	[Message(OuterOpcode.C2M_TrialDungeonFinishRequest)]
	[ProtoContract]
	public partial class C2M_TrialDungeonFinishRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_TrialDungeonFinishResponse)]
	[ProtoContract]
	public partial class M2C_TrialDungeonFinishResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int CombatResult { get; set; }

	}

//试炼副本开始
	[ResponseType(nameof(M2C_TrialDungeonBeginResponse))]
	[Message(OuterOpcode.C2M_TrialDungeonBeginRequest)]
	[ProtoContract]
	public partial class C2M_TrialDungeonBeginRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_TrialDungeonBeginResponse)]
	[ProtoContract]
	public partial class M2C_TrialDungeonBeginResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//上下马
	[ResponseType(nameof(M2C_HorseRideResponse))]
	[Message(OuterOpcode.C2M_HorseRideRequest)]
	[ProtoContract]
	public partial class C2M_HorseRideRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int HorseId { get; set; }

		[ProtoMember(2)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_HorseRideResponse)]
	[ProtoContract]
	public partial class M2C_HorseRideResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//坐骑出战
	[ResponseType(nameof(M2C_HorseFightResponse))]
	[Message(OuterOpcode.C2M_HorseFightRequest)]
	[ProtoContract]
	public partial class C2M_HorseFightRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int HorseId { get; set; }

		[ProtoMember(2)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_HorseFightResponse)]
	[ProtoContract]
	public partial class M2C_HorseFightResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_TowerFightBeginResponse))]
//挑战之地开始战斗
	[Message(OuterOpcode.C2M_TowerFightBeginRequest)]
	[ProtoContract]
	public partial class C2M_TowerFightBeginRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_TowerFightBeginResponse)]
	[ProtoContract]
	public partial class M2C_TowerFightBeginResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_TowerExitResponse))]
	[Message(OuterOpcode.C2M_TowerExitRequest)]
	[ProtoContract]
	public partial class C2M_TowerExitRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_TowerExitResponse)]
	[ProtoContract]
	public partial class M2C_TowerExitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(C2C_SendBroadcastResponse))]
	[Message(OuterOpcode.C2C_SendBroadcastRequest)]
	[ProtoContract]
	public partial class C2C_SendBroadcastRequest: Object, IChatActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public ChatInfo ChatInfo { get; set; }

		[ProtoMember(2)]
		public int ZoneType { get; set; }

	}

	[Message(OuterOpcode.C2C_SendBroadcastResponse)]
	[ProtoContract]
	public partial class C2C_SendBroadcastResponse: Object, IChatActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//终止技能
	[Message(OuterOpcode.M2C_UnitFinishSkill)]
	[ProtoContract]
	public partial class M2C_UnitFinishSkill: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.M2C_SyncMiJingDamage)]
	[ProtoContract]
	public partial class M2C_SyncMiJingDamage: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<TeamPlayerInfo> DamageList = new List<TeamPlayerInfo>();

	}

	[ResponseType(nameof(M2C_FubenTimesResetResponse))]
	[Message(OuterOpcode.C2M_FubenTimesResetRequest)]
	[ProtoContract]
	public partial class C2M_FubenTimesResetRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int SceneType { get; set; }

	}

	[Message(OuterOpcode.M2C_FubenTimesResetResponse)]
	[ProtoContract]
	public partial class M2C_FubenTimesResetResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_IOSPayVerifyResponse))]
	[Message(OuterOpcode.C2M_IOSPayVerifyRequest)]
	[ProtoContract]
	public partial class C2M_IOSPayVerifyRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public string payMessage { get; set; }

	}

	[Message(OuterOpcode.M2C_IOSPayVerifyResponse)]
	[ProtoContract]
	public partial class M2C_IOSPayVerifyResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//通用协议 应急用
	[ResponseType(nameof(M2C_FubenMessageResponse))]
	[Message(OuterOpcode.C2M_FubenMessageRequest)]
	[ProtoContract]
	public partial class C2M_FubenMessageRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int SceneType { get; set; }

		[ProtoMember(2)]
		public int MessageType { get; set; }

	}

	[Message(OuterOpcode.M2C_FubenMessageResponse)]
	[ProtoContract]
	public partial class M2C_FubenMessageResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string MessageValue { get; set; }

	}

///////Max OpcodeID
	[Message(OuterOpcode.M2C_UpdateVersion)]
	[ProtoContract]
	public partial class M2C_UpdateVersion: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Version { get; set; }

	}

	[ResponseType(nameof(M2C_ShareSucessResponse))]
	[Message(OuterOpcode.C2M_ShareSucessRequest)]
	[ProtoContract]
	public partial class C2M_ShareSucessRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ShareType { get; set; }

	}

	[Message(OuterOpcode.M2C_ShareSucessResponse)]
	[ProtoContract]
	public partial class M2C_ShareSucessResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_UnitInfoResponse))]
	[Message(OuterOpcode.C2M_UnitInfoRequest)]
	[ProtoContract]
	public partial class C2M_UnitInfoRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitID { get; set; }

	}

	[Message(OuterOpcode.M2C_UnitInfoResponse)]
	[ProtoContract]
	public partial class M2C_UnitInfoResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(6)]
		public List<int> Ks = new List<int>();

		[ProtoMember(7)]
		public List<long> Vs = new List<long>();

	}

	[ResponseType(nameof(Center2C_DeleteAccountResponse))]
	[Message(OuterOpcode.C2Center_DeleteAccountRequest)]
	[ProtoContract]
	public partial class C2Center_DeleteAccountRequest: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.Center2C_DeleteAccountResponse)]
	[ProtoContract]
	public partial class Center2C_DeleteAccountResponse: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Center2C_BlackAccountResponse))]
	[Message(OuterOpcode.C2Center_BlackAccountRequest)]
	[ProtoContract]
	public partial class C2Center_BlackAccountRequest: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.Center2C_BlackAccountResponse)]
	[ProtoContract]
	public partial class Center2C_BlackAccountResponse: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_IOSPayVerifyResponse))]
	[Message(OuterOpcode.C2R_IOSPayVerifyRequest)]
	[ProtoContract]
	public partial class C2R_IOSPayVerifyRequest: Object, IRechargeActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public string payMessage { get; set; }

		[ProtoMember(3)]
		public string UnitName { get; set; }

	}

	[Message(OuterOpcode.R2C_IOSPayVerifyResponse)]
	[ProtoContract]
	public partial class R2C_IOSPayVerifyResponse: Object, IRechargeActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//技能打断
	[Message(OuterOpcode.C2M_SkillInterruptRequest)]
	[ProtoContract]
	public partial class C2M_SkillInterruptRequest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int SkillID { get; set; }

	}

	[Message(OuterOpcode.M2C_SkillInterruptResult)]
	[ProtoContract]
	public partial class M2C_SkillInterruptResult: Object, IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int SkillId { get; set; }

	}

	[ResponseType(nameof(M2C_RolePetUpStage))]
//宠物进化
	[Message(OuterOpcode.C2M_RolePetUpStage)]
	[ProtoContract]
	public partial class C2M_RolePetUpStage: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public long PetInfoXianJiId { get; set; }

	}

	[Message(OuterOpcode.M2C_RolePetUpStage)]
	[ProtoContract]
	public partial class M2C_RolePetUpStage: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetInfo OldPetInfo { get; set; }

		[ProtoMember(2)]
		public RolePetInfo NewPetInfo { get; set; }

	}

	[ResponseType(nameof(Center2C_PhoneBinging))]
//手机号绑定
	[Message(OuterOpcode.C2Center_PhoneBinging)]
	[ProtoContract]
	public partial class C2Center_PhoneBinging: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string PhoneNumber { get; set; }

		[ProtoMember(2)]
		public long AccountId { get; set; }

		[ProtoMember(3)]
		public string Account { get; set; }

		[ProtoMember(4)]
		public string Password { get; set; }

	}

	[Message(OuterOpcode.Center2C_PhoneBinging)]
	[ProtoContract]
	public partial class Center2C_PhoneBinging: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_ItemTreasureOpenResponse))]
//藏宝图开启
	[Message(OuterOpcode.C2M_ItemTreasureOpenRequest)]
	[ProtoContract]
	public partial class C2M_ItemTreasureOpenRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public long OperateBagID { get; set; }

		[ProtoMember(3)]
		public string OperatePar { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemTreasureOpenResponse)]
	[ProtoContract]
	public partial class M2C_ItemTreasureOpenResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public string OperatePar { get; set; }

		[ProtoMember(5)]
		public RewardItem ReardItem { get; set; }

	}

	[ResponseType(nameof(A2C_ActivityInfoResponse))]
	[Message(OuterOpcode.C2A_ActivityInfoRequest)]
	[ProtoContract]
	public partial class C2A_ActivityInfoRequest: Object, IActivityActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ActivityType { get; set; }

	}

	[Message(OuterOpcode.A2C_ActivityInfoResponse)]
	[ProtoContract]
	public partial class A2C_ActivityInfoResponse: Object, IActivityActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string ActivityContent { get; set; }

	}

//激活称号
	[ResponseType(nameof(M2C_TitleUseResponse))]
	[Message(OuterOpcode.C2M_TitleUseRequest)]
	[ProtoContract]
	public partial class C2M_TitleUseRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int TitleId { get; set; }

		[ProtoMember(2)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_TitleUseResponse)]
	[ProtoContract]
	public partial class M2C_TitleUseResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.M2C_TitleUpdateResult)]
	[ProtoContract]
	public partial class M2C_TitleUpdateResult: Object, IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(6)]
		public List<KeyValuePairInt> TitleList = new List<KeyValuePairInt>();

	}

	[Message(OuterOpcode.M2C_AreneInfoResult)]
	[ProtoContract]
	public partial class M2C_AreneInfoResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public int LeftPlayer { get; set; }

		[ProtoMember(4)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_LifeShieldCostResponse))]
	[Message(OuterOpcode.C2M_LifeShieldCostRequest)]
	[ProtoContract]
	public partial class C2M_LifeShieldCostRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public List<long> OperateBagID = new List<long>();

	}

	[Message(OuterOpcode.M2C_LifeShieldCostResponse)]
	[ProtoContract]
	public partial class M2C_LifeShieldCostResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<LifeShieldInfo> ShieldList = new List<LifeShieldInfo>();

		[ProtoMember(2)]
		public int AddExp { get; set; }

	}

	[ResponseType(nameof(M2C_ItemSplitResponse))]
	[Message(OuterOpcode.C2M_ItemSplitRequest)]
	[ProtoContract]
	public partial class C2M_ItemSplitRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public long OperateBagID { get; set; }

		[ProtoMember(3)]
		public string OperatePar { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemSplitResponse)]
	[ProtoContract]
	public partial class M2C_ItemSplitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public string OperatePar { get; set; }

	}

//副本击杀boss
	[Message(OuterOpcode.M2C_TeamDungeonKillBossMessage)]
	[ProtoContract]
	public partial class M2C_TeamDungeonKillBossMessage: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<int> KillBossList = new List<int>();

	}

	[Message(OuterOpcode.M2C_KickPlayerMessage)]
	[ProtoContract]
	public partial class M2C_KickPlayerMessage: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//出战精灵
	[ResponseType(nameof(M2C_JingLingUseResponse))]
	[Message(OuterOpcode.C2M_JingLingUseRequest)]
	[ProtoContract]
	public partial class C2M_JingLingUseRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int JingLingId { get; set; }

		[ProtoMember(2)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_JingLingUseResponse)]
	[ProtoContract]
	public partial class M2C_JingLingUseResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int JingLingId { get; set; }

	}

//抓捕精灵
	[ResponseType(nameof(M2C_JingLingCatchResponse))]
	[Message(OuterOpcode.C2M_JingLingCatchRequest)]
	[ProtoContract]
	public partial class C2M_JingLingCatchRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long JingLingId { get; set; }

		[ProtoMember(2)]
		public int ItemId { get; set; }

		[ProtoMember(5)]
		public string OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_JingLingCatchResponse)]
	[ProtoContract]
	public partial class M2C_JingLingCatchResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//精灵掉落
	[ResponseType(nameof(M2C_JingLingDropResponse))]
	[Message(OuterOpcode.C2M_JingLingDropRequest)]
	[ProtoContract]
	public partial class C2M_JingLingDropRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_JingLingDropResponse)]
	[ProtoContract]
	public partial class M2C_JingLingDropResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_TianFuPlanResponse))]
	[Message(OuterOpcode.C2M_TianFuPlanRequest)]
	[ProtoContract]
	public partial class C2M_TianFuPlanRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int TianFuPlan { get; set; }

	}

	[Message(OuterOpcode.M2C_TianFuPlanResponse)]
	[ProtoContract]
	public partial class M2C_TianFuPlanResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.SkillSetInfo)]
	[ProtoContract]
	public partial class SkillSetInfo: Object
	{
		[ProtoMember(1)]
		public List<SkillPro> SkillList = new List<SkillPro>();

		[ProtoMember(2)]
		public List<int> TianFuList = new List<int>();

		[ProtoMember(3)]
		public List<LifeShieldInfo> LifeShieldList = new List<LifeShieldInfo>();

		[ProtoMember(4)]
		public List<int> TianFuList1 = new List<int>();

		[ProtoMember(5)]
		public int TianFuPlan { get; set; }

	}

//技能天赋更新
	[Message(OuterOpcode.M2C_SkillSetMessage)]
	[ProtoContract]
	public partial class M2C_SkillSetMessage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public SkillSetInfo SkillSetInfo { get; set; }

	}

	[ResponseType(nameof(M2C_ItemBuyCellResponse))]
	[Message(OuterOpcode.C2M_ItemBuyCellRequest)]
	[ProtoContract]
	public partial class C2M_ItemBuyCellRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemBuyCellResponse)]
	[ProtoContract]
	public partial class M2C_ItemBuyCellResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

//int32 BagAddedCell = 1;
		[ProtoMember(2)]
		public List<int> WarehouseAddedCell = new List<int>();

		[ProtoMember(3)]
		public string GetItem { get; set; }

	}

	[Message(OuterOpcode.JiaYuanPlant)]
	[ProtoContract]
	public partial class JiaYuanPlant: Object
	{
		[ProtoMember(1)]
		public int CellIndex { get; set; }

		[ProtoMember(2)]
		public int ItemId { get; set; }

		[ProtoMember(3)]
		public long StartTime { get; set; }

		[ProtoMember(4)]
		public int GatherNumber { get; set; }

		[ProtoMember(5)]
		public long GatherLastTime { get; set; }

		[ProtoMember(6)]
		public long UnitId { get; set; }

		[ProtoMember(7)]
		public int StealNumber { get; set; }

		[ProtoMember(8)]
		public List<JiaYuanRecord> GatherRecord = new List<JiaYuanRecord>();

	}

	[Message(OuterOpcode.JiaYuanPastures)]
	[ProtoContract]
	public partial class JiaYuanPastures: Object
	{
		[ProtoMember(2)]
		public int ConfigId { get; set; }

		[ProtoMember(3)]
		public long StartTime { get; set; }

		[ProtoMember(4)]
		public int GatherNumber { get; set; }

		[ProtoMember(5)]
		public long GatherLastTime { get; set; }

		[ProtoMember(6)]
		public long UnitId { get; set; }

		[ProtoMember(7)]
		public int StealNumber { get; set; }

	}

	[ResponseType(nameof(M2C_JiaYuanInitResponse))]
	[Message(OuterOpcode.C2M_JiaYuanInitRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanInitRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long MasterId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanInitResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanInitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(3)]
		public int JiaYuanLv { get; set; }

		[ProtoMember(4)]
		public string MasterName { get; set; }

		[ProtoMember(5)]
		public long JiaYuanDaShiTime { get; set; }

		[ProtoMember(6)]
		public List<int> LearnMakeIds = new List<int>();

		[ProtoMember(10)]
		public List<int> PlanOpenList = new List<int>();

		[ProtoMember(11)]
		public List<JiaYuanPet> JiaYuanPetList = new List<JiaYuanPet>();

		[ProtoMember(12)]
		public List<KeyValuePair> JiaYuanProList = new List<KeyValuePair>();

		[ProtoMember(13)]
		public List<JiaYuanRecord> JiaYuanRecordList = new List<JiaYuanRecord>();

		[ProtoMember(14)]
		public List<JiaYuanPastures> JiaYuanPastureList = new List<JiaYuanPastures>();

		[ProtoMember(15)]
		public List<JiaYuanPurchaseItem> PurchaseItemList = new List<JiaYuanPurchaseItem>();

	}

	[ResponseType(nameof(M2C_JiaYuanPlantResponse))]
	[Message(OuterOpcode.C2M_JiaYuanPlantRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanPlantRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int CellIndex { get; set; }

		[ProtoMember(2)]
		public int ItemId { get; set; }

		[ProtoMember(3)]
		public long OperateBagID { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanPlantResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanPlantResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_JiaYuanWatchResponse))]
	[Message(OuterOpcode.C2M_JiaYuanWatchRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanWatchRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long MasterId { get; set; }

		[ProtoMember(3)]
		public int OperateType { get; set; }

		[ProtoMember(4)]
		public long OperateId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanWatchResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanWatchResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<JiaYuanRecord> JiaYuanRecord = new List<JiaYuanRecord>();

	}

//一键放入
	[ResponseType(nameof(M2C_JiaYuanStoreResponse))]
	[Message(OuterOpcode.C2M_JiaYuanStoreRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanStoreRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int HorseId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanStoreResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanStoreResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_JiaYuanGatherResponse))]
	[Message(OuterOpcode.C2M_JiaYuanGatherRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanGatherRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int CellIndex { get; set; }

		[ProtoMember(3)]
		public long UnitId { get; set; }

		[ProtoMember(4)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanGatherResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanGatherResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_JiaYuanUprootResponse))]
	[Message(OuterOpcode.C2M_JiaYuanUprootRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanUprootRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int CellIndex { get; set; }

		[ProtoMember(3)]
		public long UnitId { get; set; }

		[ProtoMember(4)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanUprootResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanUprootResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public int CellIndex { get; set; }

		[ProtoMember(4)]
		public List<JiaYuanPastures> JiaYuanPastureList = new List<JiaYuanPastures>();

	}

	[ResponseType(nameof(M2C_JiaYuanCangKuOpenResponse))]
	[Message(OuterOpcode.C2M_JiaYuanCangKuOpenRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanCangKuOpenRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanCangKuOpenResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanCangKuOpenResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_JiaYuanMysteryListResponse))]
	[Message(OuterOpcode.C2M_JiaYuanMysteryListRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanMysteryListRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int NpcID { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanMysteryListResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanMysteryListResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<MysteryItemInfo> MysteryItemInfos = new List<MysteryItemInfo>();

	}

	[ResponseType(nameof(M2C_JiaYuanMysteryBuyResponse))]
	[Message(OuterOpcode.C2M_JiaYuanMysteryBuyRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanMysteryBuyRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ProductId { get; set; }

		[ProtoMember(2)]
		public int MysteryId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanMysteryBuyResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanMysteryBuyResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<MysteryItemInfo> MysteryItemInfos = new List<MysteryItemInfo>();

	}

	[ResponseType(nameof(M2C_JiaYuanPastureListResponse))]
	[Message(OuterOpcode.C2M_JiaYuanPastureListRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanPastureListRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanPastureListResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanPastureListResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<MysteryItemInfo> MysteryItemInfos = new List<MysteryItemInfo>();

	}

	[ResponseType(nameof(M2C_JiaYuanPastureBuyResponse))]
	[Message(OuterOpcode.C2M_JiaYuanPastureBuyRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanPastureBuyRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ProductId { get; set; }

		[ProtoMember(2)]
		public int MysteryId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanPastureBuyResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanPastureBuyResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<MysteryItemInfo> MysteryItemInfos = new List<MysteryItemInfo>();

		[ProtoMember(4)]
		public List<JiaYuanPastures> JiaYuanPastureList = new List<JiaYuanPastures>();

	}

	[ResponseType(nameof(M2C_JiaYuanPlanOpenResponse))]
	[Message(OuterOpcode.C2M_JiaYuanPlanOpenRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanPlanOpenRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int CellIndex { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanPlanOpenResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanPlanOpenResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<int> PlanOpenList = new List<int>();

	}

	[ResponseType(nameof(M2C_ItemOneSellResponse))]
//一键盘出售
	[Message(OuterOpcode.C2M_ItemOneSellRequest)]
	[ProtoContract]
	public partial class C2M_ItemOneSellRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public List<long> BagInfoIds = new List<long>();

	}

	[Message(OuterOpcode.M2C_ItemOneSellResponse)]
	[ProtoContract]
	public partial class M2C_ItemOneSellResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[Message(OuterOpcode.JiaYuanPurchaseItem)]
	[ProtoContract]
	public partial class JiaYuanPurchaseItem: Object
	{
		[ProtoMember(1)]
		public int ItemID { get; set; }

		[ProtoMember(2)]
		public int BuyZiJin { get; set; }

		[ProtoMember(3)]
		public long MakeTime { get; set; }

		[ProtoMember(4)]
		public int LeftNum { get; set; }

		[ProtoMember(5)]
		public int PurchaseId { get; set; }

		[ProtoMember(6)]
		public long EndTime { get; set; }

	}

	[ResponseType(nameof(M2C_JiaYuanPurchaseResponse))]
//家园收购
	[Message(OuterOpcode.C2M_JiaYuanPurchaseRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanPurchaseRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public int ItemId { get; set; }

		[ProtoMember(5)]
		public int PurchaseId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanPurchaseResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanPurchaseResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public List<JiaYuanPurchaseItem> PurchaseItemList = new List<JiaYuanPurchaseItem>();

	}

	[ResponseType(nameof(M2C_JiaYuanPurchaseRefresh))]
//家园收购刷新
	[Message(OuterOpcode.C2M_JiaYuanPurchaseRefresh)]
	[ProtoContract]
	public partial class C2M_JiaYuanPurchaseRefresh: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public int ItemId { get; set; }

		[ProtoMember(5)]
		public int PurchaseId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanPurchaseRefresh)]
	[ProtoContract]
	public partial class M2C_JiaYuanPurchaseRefresh: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public List<JiaYuanPurchaseItem> PurchaseItemList = new List<JiaYuanPurchaseItem>();

	}

	[ResponseType(nameof(M2C_JiaYuanCookResponse))]
//制作菜肴
	[Message(OuterOpcode.C2M_JiaYuanCookRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanCookRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public List<long> BagInfoIds = new List<long>();

	}

	[Message(OuterOpcode.M2C_JiaYuanCookResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanCookResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public int ItemId { get; set; }

		[ProtoMember(2)]
		public List<int> LearnMakeIds = new List<int>();

		[ProtoMember(3)]
		public int LearnId { get; set; }

	}

	[ResponseType(nameof(M2C_JiaYuanCookBookOpen))]
//学习菜单
	[Message(OuterOpcode.C2M_JiaYuanCookBookOpen)]
	[ProtoContract]
	public partial class C2M_JiaYuanCookBookOpen: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int LearnMakeId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanCookBookOpen)]
	[ProtoContract]
	public partial class M2C_JiaYuanCookBookOpen: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public List<int> LearnMakeIds = new List<int>();

	}

//家园刷新
	[Message(OuterOpcode.M2C_JiaYuanUpdate)]
	[ProtoContract]
	public partial class M2C_JiaYuanUpdate: Object, IActorMessage
	{
		[ProtoMember(2)]
		public List<JiaYuanPurchaseItem> PurchaseItemList = new List<JiaYuanPurchaseItem>();

	}

	[ResponseType(nameof(M2C_JiaYuanUpLvResponse))]
//家园升级
	[Message(OuterOpcode.C2M_JiaYuanUpLvRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanUpLvRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanUpLvResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanUpLvResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_JiaYuanExchangeResponse))]
//家园兑换
	[Message(OuterOpcode.C2M_JiaYuanExchangeRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanExchangeRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ExchangeType { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanExchangeResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanExchangeResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_JiaYuanDaShiResponse))]
//家园大师
	[Message(OuterOpcode.C2M_JiaYuanDaShiRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanDaShiRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public List<long> BagInfoIDs = new List<long>();

	}

	[Message(OuterOpcode.M2C_JiaYuanDaShiResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanDaShiResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public long JiaYuanDaShiTime { get; set; }

		[ProtoMember(2)]
		public List<KeyValuePairInt> JiaYuanProAdd = new List<KeyValuePairInt>();

		[ProtoMember(7)]
		public List<KeyValuePair> JiaYuanProList = new List<KeyValuePair>();

	}

	[ResponseType(nameof(Popularize2C_ListResponse))]
//我的推广列表
	[Message(OuterOpcode.C2Popularize_ListRequest)]
	[ProtoContract]
	public partial class C2Popularize_ListRequest: Object, IPopularizeActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.Popularize2C_ListResponse)]
	[ProtoContract]
	public partial class Popularize2C_ListResponse: Object, IPopularizeActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public long PopularizeCode { get; set; }

		[ProtoMember(2)]
		public long BePopularizeId { get; set; }

		[ProtoMember(3)]
		public List<PopularizeInfo> MyPopularizeList = new List<PopularizeInfo>();

	}

	[ResponseType(nameof(Popularize2C_UploadResponse))]
//内存占用
	[Message(OuterOpcode.C2Popularize_UploadRequest)]
	[ProtoContract]
	public partial class C2Popularize_UploadRequest: Object, IPopularizeActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string MemoryInfo { get; set; }

	}

	[Message(OuterOpcode.Popularize2C_UploadResponse)]
	[ProtoContract]
	public partial class Popularize2C_UploadResponse: Object, IPopularizeActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(Popularize2C_RewardResponse))]
//我的推广奖励
	[Message(OuterOpcode.C2Popularize_RewardRequest)]
	[ProtoContract]
	public partial class C2Popularize_RewardRequest: Object, IPopularizeActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.Popularize2C_RewardResponse)]
	[ProtoContract]
	public partial class Popularize2C_RewardResponse: Object, IPopularizeActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(Popularize2C_PlayerResponse))]
//我推广的玩家
	[Message(OuterOpcode.C2Popularize_PlayerRequest)]
	[ProtoContract]
	public partial class C2Popularize_PlayerRequest: Object, IPopularizeActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long PopularizeId { get; set; }

	}

	[Message(OuterOpcode.Popularize2C_PlayerResponse)]
	[ProtoContract]
	public partial class Popularize2C_PlayerResponse: Object, IPopularizeActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[Message(OuterOpcode.PopularizeInfo)]
	[ProtoContract]
	public partial class PopularizeInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public string Nmae { get; set; }

		[ProtoMember(3)]
		public int Level { get; set; }

		[ProtoMember(4)]
		public List<int> Rewards = new List<int>();

		[ProtoMember(5)]
		public int Occ { get; set; }

		[ProtoMember(6)]
		public int OccTwo { get; set; }

	}

	[ResponseType(nameof(M2C_SerialReardResponse))]
//序列号奖励
	[Message(OuterOpcode.C2M_SerialReardRequest)]
	[ProtoContract]
	public partial class C2M_SerialReardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string SerialNumber { get; set; }

	}

	[Message(OuterOpcode.M2C_SerialReardResponse)]
	[ProtoContract]
	public partial class M2C_SerialReardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.JiaYuanMonster)]
	[ProtoContract]
	public partial class JiaYuanMonster: Object
	{
		[ProtoMember(1)]
		public long unitId { get; set; }

		[ProtoMember(2)]
		public float x { get; set; }

		[ProtoMember(3)]
		public float y { get; set; }

		[ProtoMember(4)]
		public float z { get; set; }

		[ProtoMember(5)]
		public long BornTime { get; set; }

		[ProtoMember(6)]
		public int ConfigId { get; set; }

	}

//开启宝箱
	[ResponseType(nameof(Actor_JiaYuanPickResponse))]
	[Message(OuterOpcode.Actor_JiaYuanPickRequest)]
	[ProtoContract]
	public partial class Actor_JiaYuanPickRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public long MasterId { get; set; }

	}

	[Message(OuterOpcode.Actor_JiaYuanPickResponse)]
	[ProtoContract]
	public partial class Actor_JiaYuanPickResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.JiaYuanPet)]
	[ProtoContract]
	public partial class JiaYuanPet: Object
	{
		[ProtoMember(1)]
		public long unitId { get; set; }

		[ProtoMember(2)]
		public int ConfigId { get; set; }

		[ProtoMember(3)]
		public long TotalExp { get; set; }

		[ProtoMember(4)]
		public long CurExp { get; set; }

		[ProtoMember(5)]
		public long StartTime { get; set; }

		[ProtoMember(6)]
		public int MoodValue { get; set; }

		[ProtoMember(7)]
		public int PetLv { get; set; }

		[ProtoMember(8)]
		public long LastExpTime { get; set; }

		[ProtoMember(9)]
		public string PlayerName { get; set; }

		[ProtoMember(10)]
		public string PetName { get; set; }

		[ProtoMember(11)]
		public int Position { get; set; }

	}

//宠物散步
	[ResponseType(nameof(M2C_JiaYuanPetWalkResponse))]
	[Message(OuterOpcode.C2M_JiaYuanPetWalkRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanPetWalkRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public long PetId { get; set; }

		[ProtoMember(3)]
		public int PetStatus { get; set; }

		[ProtoMember(4)]
		public int Position { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanPetWalkResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanPetWalkResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(11)]
		public List<JiaYuanPet> JiaYuanPetList = new List<JiaYuanPet>();

	}

//宠物喂食
	[ResponseType(nameof(M2C_JiaYuanPetFeedResponse))]
	[Message(OuterOpcode.C2M_JiaYuanPetFeedRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanPetFeedRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public long PetId { get; set; }

		[ProtoMember(3)]
		public List<long> BagInfoIDs = new List<long>();

	}

	[Message(OuterOpcode.M2C_JiaYuanPetFeedResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanPetFeedResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int MoodAdd { get; set; }

		[ProtoMember(2)]
		public List<JiaYuanPet> JiaYuanPetList = new List<JiaYuanPet>();

	}

	[Message(OuterOpcode.JiaYuanVisit)]
	[ProtoContract]
	public partial class JiaYuanVisit: Object
	{
		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int Occ { get; set; }

		[ProtoMember(3)]
		public int OccTwo { get; set; }

		[ProtoMember(4)]
		public int Rubbish { get; set; }

		[ProtoMember(5)]
		public int Gather { get; set; }

		[ProtoMember(6)]
		public string PlayerName { get; set; }

	}

//好友家园
	[ResponseType(nameof(M2C_JiaYuanVisitListResponse))]
	[Message(OuterOpcode.C2M_JiaYuanVisitListRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanVisitListRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public long MasterId { get; set; }

		[ProtoMember(3)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanVisitListResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanVisitListResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<JiaYuanVisit> JiaYuanVisit_1 = new List<JiaYuanVisit>();

		[ProtoMember(2)]
		public List<JiaYuanVisit> JiaYuanVisit_2 = new List<JiaYuanVisit>();

	}

	[ResponseType(nameof(M2C_JiaYuanGatherOtherResponse))]
	[Message(OuterOpcode.C2M_JiaYuanGatherOtherRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanGatherOtherRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int CellIndex { get; set; }

		[ProtoMember(3)]
		public long UnitId { get; set; }

		[ProtoMember(4)]
		public long MasterId { get; set; }

		[ProtoMember(5)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanGatherOtherResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanGatherOtherResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_JiaYuanPetPositionResponse))]
	[Message(OuterOpcode.C2M_JiaYuanPetPositionRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanPetPositionRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(3)]
		public long UnitId { get; set; }

		[ProtoMember(4)]
		public long MasterId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanPetPositionResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanPetPositionResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(5)]
		public List<UnitInfo> PetList = new List<UnitInfo>();

	}

	[ResponseType(nameof(M2C_JiaYuanPetOperateResponse))]
	[Message(OuterOpcode.C2M_JiaYuanPetOperateRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanPetOperateRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public int Operate { get; set; }

		[ProtoMember(4)]
		public long MasterId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanPetOperateResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanPetOperateResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(5)]
		public List<UnitInfo> PetList = new List<UnitInfo>();

	}

	[Message(OuterOpcode.JiaYuanRecord)]
	[ProtoContract]
	public partial class JiaYuanRecord: Object
	{
		[ProtoMember(1)]
		public long Time { get; set; }

		[ProtoMember(2)]
		public string PlayerName { get; set; }

		[ProtoMember(3)]
		public int OperateType { get; set; }

		[ProtoMember(4)]
		public int OperateId { get; set; }

		[ProtoMember(7)]
		public long PlayerId { get; set; }

	}

	[Message(OuterOpcode.JiaYuanOperate)]
	[ProtoContract]
	public partial class JiaYuanOperate: Object
	{
		[ProtoMember(1)]
		public long Time { get; set; }

		[ProtoMember(2)]
		public string PlayerName { get; set; }

		[ProtoMember(3)]
		public int OperateType { get; set; }

		[ProtoMember(4)]
		public int OperateId { get; set; }

		[ProtoMember(5)]
		public string OperatePar { get; set; }

		[ProtoMember(6)]
		public long UnitId { get; set; }

		[ProtoMember(7)]
		public long PlayerId { get; set; }

	}

	[ResponseType(nameof(M2C_JiaYuanRecordListResponse))]
	[Message(OuterOpcode.C2M_JiaYuanRecordListRequest)]
	[ProtoContract]
	public partial class C2M_JiaYuanRecordListRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_JiaYuanRecordListResponse)]
	[ProtoContract]
	public partial class M2C_JiaYuanRecordListResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(5)]
		public List<JiaYuanRecord> JiaYuanRecordList = new List<JiaYuanRecord>();

	}

	[ResponseType(nameof(A2C_TianQiResponse))]
	[Message(OuterOpcode.C2A_TianQiRequest)]
	[ProtoContract]
	public partial class C2A_TianQiRequest: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.A2C_TianQiResponse)]
	[ProtoContract]
	public partial class A2C_TianQiResponse: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public int TianQiValue { get; set; }

	}

	[ResponseType(nameof(M2C_PetShouHuResponse))]
//宠物守护
	[Message(OuterOpcode.C2M_PetShouHuRequest)]
	[ProtoContract]
	public partial class C2M_PetShouHuRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public int Position { get; set; }

	}

	[Message(OuterOpcode.M2C_PetShouHuResponse)]
	[ProtoContract]
	public partial class M2C_PetShouHuResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<long> PetShouHuList = new List<long>();

	}

	[ResponseType(nameof(M2C_PetShouHuActiveResponse))]
//宠物守护
	[Message(OuterOpcode.C2M_PetShouHuActiveRequest)]
	[ProtoContract]
	public partial class C2M_PetShouHuActiveRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int PetShouHuActive { get; set; }

	}

	[Message(OuterOpcode.M2C_PetShouHuActiveResponse)]
	[ProtoContract]
	public partial class M2C_PetShouHuActiveResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int PetShouHuActive { get; set; }

	}

	[ResponseType(nameof(M2C_PaiMaiAuctionJoinResponse))]
//参入竞拍
	[Message(OuterOpcode.C2M_PaiMaiAuctionJoinRequest)]
	[ProtoContract]
	public partial class C2M_PaiMaiAuctionJoinRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_PaiMaiAuctionJoinResponse)]
	[ProtoContract]
	public partial class M2C_PaiMaiAuctionJoinResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_PaiMaiAuctionPriceResponse))]
	[Message(OuterOpcode.C2M_PaiMaiAuctionPriceRequest)]
	[ProtoContract]
	public partial class C2M_PaiMaiAuctionPriceRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Price { get; set; }

		[ProtoMember(2)]
		public string AuctionPlayer { get; set; }

	}

	[Message(OuterOpcode.M2C_PaiMaiAuctionPriceResponse)]
	[ProtoContract]
	public partial class M2C_PaiMaiAuctionPriceResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2C_PaiMaiAuctionInfoResponse))]
	[Message(OuterOpcode.C2P_PaiMaiAuctionInfoRequest)]
	[ProtoContract]
	public partial class C2P_PaiMaiAuctionInfoRequest: Object, IPaiMaiListRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.P2C_PaiMaiAuctionInfoResponse)]
	[ProtoContract]
	public partial class P2C_PaiMaiAuctionInfoResponse: Object, IPaiMaiListResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int AuctionItem { get; set; }

		[ProtoMember(2)]
		public long AuctionPrice { get; set; }

		[ProtoMember(3)]
		public long AuctionStatus { get; set; }

		[ProtoMember(4)]
		public int AuctionNumber { get; set; }

		[ProtoMember(5)]
		public string AuctionPlayer { get; set; }

		[ProtoMember(6)]
		public long AuctionStart { get; set; }

		[ProtoMember(7)]
		public bool AuctionJoin { get; set; }

	}

	[ResponseType(nameof(P2C_PaiMaiAuctionRecordResponse))]
	[Message(OuterOpcode.C2P_PaiMaiAuctionRecordRequest)]
	[ProtoContract]
	public partial class C2P_PaiMaiAuctionRecordRequest: Object, IPaiMaiListRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.P2C_PaiMaiAuctionRecordResponse)]
	[ProtoContract]
	public partial class P2C_PaiMaiAuctionRecordResponse: Object, IPaiMaiListResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<PaiMaiAuctionRecord> RecordList = new List<PaiMaiAuctionRecord>();

	}

	[ResponseType(nameof(M2C_ItemDestoryResponse))]
//销毁装备
	[Message(OuterOpcode.C2M_ItemDestoryRequest)]
	[ProtoContract]
	public partial class C2M_ItemDestoryRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public long OperateBagID { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemDestoryResponse)]
	[ProtoContract]
	public partial class M2C_ItemDestoryResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_ItemFumoUseResponse))]
//附魔使用
	[Message(OuterOpcode.C2M_ItemFumoUseRequest)]
	[ProtoContract]
	public partial class C2M_ItemFumoUseRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public long OperateBagID { get; set; }

		[ProtoMember(3)]
		public List<HideProList> FuMoProList = new List<HideProList>();

	}

	[Message(OuterOpcode.M2C_ItemFumoUseResponse)]
	[ProtoContract]
	public partial class M2C_ItemFumoUseResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_ItemFumoProResponse))]
//附魔属性
	[Message(OuterOpcode.C2M_ItemFumoProRequest)]
	[ProtoContract]
	public partial class C2M_ItemFumoProRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public int Index { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemFumoProResponse)]
	[ProtoContract]
	public partial class M2C_ItemFumoProResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_FirstWinSelfRewardResponse))]
	[Message(OuterOpcode.C2M_FirstWinSelfRewardRequest)]
	[ProtoContract]
	public partial class C2M_FirstWinSelfRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int FirstWinId { get; set; }

		[ProtoMember(2)]
		public int Difficulty { get; set; }

	}

	[Message(OuterOpcode.M2C_FirstWinSelfRewardResponse)]
	[ProtoContract]
	public partial class M2C_FirstWinSelfRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<KeyValuePair> FirstWinInfos = new List<KeyValuePair>();

	}

	[Message(OuterOpcode.M2C_FirstWinSelfUpdateMessage)]
	[ProtoContract]
	public partial class M2C_FirstWinSelfUpdateMessage: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<KeyValuePair> FirstWinInfos = new List<KeyValuePair>();

	}

//家族争霸赛捐献
	[ResponseType(nameof(M2C_DonationResponse))]
	[Message(OuterOpcode.C2M_DonationRequest)]
	[ProtoContract]
	public partial class C2M_DonationRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Price { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.M2C_DonationResponse)]
	[ProtoContract]
	public partial class M2C_DonationResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_UnionDonationResponse))]
	[Message(OuterOpcode.C2M_UnionDonationRequest)]
	[ProtoContract]
	public partial class C2M_UnionDonationRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Price { get; set; }

		[ProtoMember(2)]
		public int Type { get; set; }

	}

	[Message(OuterOpcode.M2C_UnionDonationResponse)]
	[ProtoContract]
	public partial class M2C_UnionDonationResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_SoloMatchResponse))]
	[Message(OuterOpcode.C2M_SoloMatchRequest)]
	[ProtoContract]
	public partial class C2M_SoloMatchRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_SoloMatchResponse)]
	[ProtoContract]
	public partial class M2C_SoloMatchResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//solo战绩
	[ResponseType(nameof(S2C_SoloMyInfoResponse))]
	[Message(OuterOpcode.C2S_SoloMyInfoRequest)]
	[ProtoContract]
	public partial class C2S_SoloMyInfoRequest: Object, ISoloActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.S2C_SoloMyInfoResponse)]
	[ProtoContract]
	public partial class S2C_SoloMyInfoResponse: Object, ISoloActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long MathTime { get; set; }

		[ProtoMember(2)]
		public int WinTime { get; set; }

		[ProtoMember(3)]
		public int FailTime { get; set; }

		[ProtoMember(4)]
		public List<SoloPlayerResultInfo> SoloPlayerResultInfoList = new List<SoloPlayerResultInfo>();

	}

	[Message(OuterOpcode.M2C_SoloMatchResult)]
	[ProtoContract]
	public partial class M2C_SoloMatchResult: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Result { get; set; }

		[ProtoMember(2)]
		public long FubenId { get; set; }

	}

	[Message(OuterOpcode.M2C_SoloDungeon)]
	[ProtoContract]
	public partial class M2C_SoloDungeon: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<RewardItem> RewardItem = new List<RewardItem>();

		[ProtoMember(2)]
		public int SoloResult { get; set; }

	}

	[Message(OuterOpcode.SoloPlayerResultInfo)]
	[ProtoContract]
	public partial class SoloPlayerResultInfo: Object
	{
		[ProtoMember(1)]
		public long MatchTime { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public long Combat { get; set; }

		[ProtoMember(4)]
		public string Name { get; set; }

		[ProtoMember(5)]
		public int Occ { get; set; }

		[ProtoMember(6)]
		public int WinNum { get; set; }

		[ProtoMember(7)]
		public int FailNum { get; set; }

	}

	[Message(OuterOpcode.SoloPlayerInfo)]
	[ProtoContract]
	public partial class SoloPlayerInfo: Object
	{
		[ProtoMember(1)]
		public long MatchTime { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public long Combat { get; set; }

		[ProtoMember(4)]
		public string Name { get; set; }

		[ProtoMember(5)]
		public int Occ { get; set; }

		[ProtoMember(6)]
		public int WinNum { get; set; }

		[ProtoMember(7)]
		public int FailNum { get; set; }

	}

	[Message(OuterOpcode.SoloResultInfo)]
	[ProtoContract]
	public partial class SoloResultInfo: Object
	{
		[ProtoMember(3)]
		public int WinTime { get; set; }

		[ProtoMember(4)]
		public int FailTime { get; set; }

	}

	[Message(OuterOpcode.SoloMatchInfo)]
	[ProtoContract]
	public partial class SoloMatchInfo: Object
	{
		[ProtoMember(1)]
		public long UnitId_1 { get; set; }

		[ProtoMember(2)]
		public long UnitId_2 { get; set; }

		[ProtoMember(3)]
		public long FubenId { get; set; }

	}

	[Message(OuterOpcode.PaiMaiAuctionRecord)]
	[ProtoContract]
	public partial class PaiMaiAuctionRecord: Object
	{
		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public int Occ { get; set; }

		[ProtoMember(3)]
		public string PlayerName { get; set; }

		[ProtoMember(4)]
		public long Price { get; set; }

		[ProtoMember(5)]
		public long Time { get; set; }

	}

	[Message(OuterOpcode.M2C_UnionRaceInfoResult)]
	[ProtoContract]
	public partial class M2C_UnionRaceInfoResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(5)]
		public int SceneType { get; set; }

	}

//邀请组队
	[ResponseType(nameof(T2C_TeamRobotResponse))]
	[Message(OuterOpcode.C2T_TeamRobotRequest)]
	[ProtoContract]
	public partial class C2T_TeamRobotRequest: Object, ITeamActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(OuterOpcode.T2C_TeamRobotResponse)]
	[ProtoContract]
	public partial class T2C_TeamRobotResponse: Object, ITeamActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_PetFragmentDuiHuan))]
	[Message(OuterOpcode.C2M_PetFragmentDuiHuan)]
	[ProtoContract]
	public partial class C2M_PetFragmentDuiHuan: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetFragmentDuiHuan)]
	[ProtoContract]
	public partial class M2C_PetFragmentDuiHuan: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetInfo RolePetInfo { get; set; }

	}

	[ResponseType(nameof(M2C_FindNearMonsterResponse))]
	[Message(OuterOpcode.C2M_FindNearMonsterRequest)]
	[ProtoContract]
	public partial class C2M_FindNearMonsterRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_FindNearMonsterResponse)]
	[ProtoContract]
	public partial class M2C_FindNearMonsterResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public float x { get; set; }

		[ProtoMember(2)]
		public float y { get; set; }

		[ProtoMember(3)]
		public float z { get; set; }

		[ProtoMember(4)]
		public bool IfFindStatus { get; set; }

		[ProtoMember(5)]
		public long MonsterID { get; set; }

	}

	[ResponseType(nameof(M2C_FindJingLingResponse))]
	[Message(OuterOpcode.C2M_FindJingLingRequest)]
	[ProtoContract]
	public partial class C2M_FindJingLingRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_FindJingLingResponse)]
	[ProtoContract]
	public partial class M2C_FindJingLingResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(5)]
		public int MonsterID { get; set; }

	}

// 封印之塔继续挑战
	[ResponseType(nameof(M2C_TowerOfSealNextResponse))]
	[Message(OuterOpcode.C2M_TowerOfSealNextRequest)]
	[ProtoContract]
	public partial class C2M_TowerOfSealNextRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int DiceResult { get; set; }

		[ProtoMember(2)]
		public int CostType { get; set; }

	}

	[Message(OuterOpcode.M2C_TowerOfSealNextResponse)]
	[ProtoContract]
	public partial class M2C_TowerOfSealNextResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

// 装备增幅
	[ResponseType(nameof(M2C_EquipmentIncreaseResponse))]
	[Message(OuterOpcode.C2M_EquipmentIncreaseRequest)]
	[ProtoContract]
	public partial class C2M_EquipmentIncreaseRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public BagInfo EquipmentBagInfo { get; set; }

		[ProtoMember(2)]
		public BagInfo ReelBagInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_EquipmentIncreaseResponse)]
	[ProtoContract]
	public partial class M2C_EquipmentIncreaseResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

//一键取出
	[ResponseType(nameof(M2C_TakeOutAllResponse))]
	[Message(OuterOpcode.C2M_TakeOutAllRequest)]
	[ProtoContract]
	public partial class C2M_TakeOutAllRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int HorseId { get; set; }

	}

	[Message(OuterOpcode.M2C_TakeOutAllResponse)]
	[ProtoContract]
	public partial class M2C_TakeOutAllResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_FashionActiveResponse))]
	[Message(OuterOpcode.C2M_FashionActiveRequest)]
	[ProtoContract]
	public partial class C2M_FashionActiveRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int FashionId { get; set; }

	}

	[Message(OuterOpcode.M2C_FashionActiveResponse)]
	[ProtoContract]
	public partial class M2C_FashionActiveResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_FashionWearResponse))]
	[Message(OuterOpcode.C2M_FashionWearRequest)]
	[ProtoContract]
	public partial class C2M_FashionWearRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int FashionId { get; set; }

		[ProtoMember(2)]
		public int OperatateType { get; set; }

	}

	[Message(OuterOpcode.M2C_FashionWearResponse)]
	[ProtoContract]
	public partial class M2C_FashionWearResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[Message(OuterOpcode.M2C_FashionUpdate)]
	[ProtoContract]
	public partial class M2C_FashionUpdate: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitID { get; set; }

		[ProtoMember(2)]
		public List<int> FashionEquipList = new List<int>();

	}

	[ResponseType(nameof(M2C_SkillJueXingResponse))]
	[Message(OuterOpcode.C2M_SkillJueXingRequest)]
	[ProtoContract]
	public partial class C2M_SkillJueXingRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int JueXingId { get; set; }

	}

	[Message(OuterOpcode.M2C_SkillJueXingResponse)]
	[ProtoContract]
	public partial class M2C_SkillJueXingResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_SkillXuanZhuanResponse))]
	[Message(OuterOpcode.C2M_SkillXuanZhuanRequest)]
	[ProtoContract]
	public partial class C2M_SkillXuanZhuanRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Angle { get; set; }

	}

	[Message(OuterOpcode.M2C_SkillXuanZhuanResponse)]
	[ProtoContract]
	public partial class M2C_SkillXuanZhuanResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public int Angle { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

	}

	[Message(OuterOpcode.M2C_SkillXuanZhuanMessage)]
	[ProtoContract]
	public partial class M2C_SkillXuanZhuanMessage: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Angle { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

	}

	[ResponseType(nameof(M2C_ItemIncreaseTransferResponse))]
//增幅转移
	[Message(OuterOpcode.C2M_ItemIncreaseTransferRequest)]
	[ProtoContract]
	public partial class C2M_ItemIncreaseTransferRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long OperateBagID_1 { get; set; }

		[ProtoMember(2)]
		public long OperateBagID_2 { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemIncreaseTransferResponse)]
	[ProtoContract]
	public partial class M2C_ItemIncreaseTransferResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[Message(OuterOpcode.BattleSummonInfo)]
	[ProtoContract]
	public partial class BattleSummonInfo: Object
	{
		[ProtoMember(1)]
		public int SummonId { get; set; }

		[ProtoMember(2)]
		public long SummonTime { get; set; }

		[ProtoMember(3)]
		public int SummonNumber { get; set; }

	}

	[ResponseType(nameof(M2C_BattleSummonRecord))]
//战场召唤记录
	[Message(OuterOpcode.C2M_BattleSummonRecord)]
	[ProtoContract]
	public partial class C2M_BattleSummonRecord: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_BattleSummonRecord)]
	[ProtoContract]
	public partial class M2C_BattleSummonRecord: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public List<BattleSummonInfo> BattleSummonList = new List<BattleSummonInfo>();

	}

	[ResponseType(nameof(M2C_BattleSummonResponse))]
//战场召唤士兵
	[Message(OuterOpcode.C2M_BattleSummonRequest)]
	[ProtoContract]
	public partial class C2M_BattleSummonRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int SummonId { get; set; }

	}

	[Message(OuterOpcode.M2C_BattleSummonResponse)]
	[ProtoContract]
	public partial class M2C_BattleSummonResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(2)]
		public List<BattleSummonInfo> BattleSummonList = new List<BattleSummonInfo>();

	}

//家族神秘商店道具
	[ResponseType(nameof(U2C_UnionMysteryListResponse))]
	[Message(OuterOpcode.C2U_UnionMysteryListRequest)]
	[ProtoContract]
	public partial class C2U_UnionMysteryListRequest: Object, IUnionActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

	}

	[Message(OuterOpcode.U2C_UnionMysteryListResponse)]
	[ProtoContract]
	public partial class U2C_UnionMysteryListResponse: Object, IUnionActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<MysteryItemInfo> MysteryItemInfos = new List<MysteryItemInfo>();

	}

//家族神秘商店道具
	[ResponseType(nameof(M2C_UnionMysteryBuyResponse))]
	[Message(OuterOpcode.C2M_UnionMysteryBuyRequest)]
	[ProtoContract]
	public partial class C2M_UnionMysteryBuyRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MysteryId { get; set; }

		[ProtoMember(3)]
		public int BuyNumber { get; set; }

	}

	[Message(OuterOpcode.M2C_UnionMysteryBuyResponse)]
	[ProtoContract]
	public partial class M2C_UnionMysteryBuyResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//喜从天降刷新
	[ResponseType(nameof(M2C_HappyMoveResponse))]
	[Message(OuterOpcode.C2M_HappyMoveRequest)]
	[ProtoContract]
	public partial class C2M_HappyMoveRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int OperatateType { get; set; }

	}

	[Message(OuterOpcode.M2C_HappyMoveResponse)]
	[ProtoContract]
	public partial class M2C_HappyMoveResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.M2C_HappyInfoResult)]
	[ProtoContract]
	public partial class M2C_HappyInfoResult: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public long NextRefreshTime { get; set; }

	}

//小龟大赛记录
	[ResponseType(nameof(M2C_TurtleRecordResponse))]
	[Message(OuterOpcode.C2M_TurtleRecordRequest)]
	[ProtoContract]
	public partial class C2M_TurtleRecordRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_TurtleRecordResponse)]
	[ProtoContract]
	public partial class M2C_TurtleRecordResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public int SupportId { get; set; }

		[ProtoMember(3)]
		public List<int> WinTimes = new List<int>();

		[ProtoMember(4)]
		public List<int> SupportTimes = new List<int>();

	}

//小龟大赛支持
	[ResponseType(nameof(M2C_TurtleSupportResponse))]
	[Message(OuterOpcode.C2M_TurtleSupportRequest)]
	[ProtoContract]
	public partial class C2M_TurtleSupportRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int SupportId { get; set; }

	}

	[Message(OuterOpcode.M2C_TurtleSupportResponse)]
	[ProtoContract]
	public partial class M2C_TurtleSupportResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_RankUnionRaceResponse))]
	[Message(OuterOpcode.C2R_RankUnionRaceRequest)]
	[ProtoContract]
	public partial class C2R_RankUnionRaceRequest: Object, IRankActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.R2C_RankUnionRaceResponse)]
	[ProtoContract]
	public partial class R2C_RankUnionRaceResponse: Object, IRankActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RankShouLieInfo> RankList = new List<RankShouLieInfo>();

	}

	[ResponseType(nameof(R2C_RankRunRaceResponse))]
	[Message(OuterOpcode.C2R_RankRunRaceRequest)]
	[ProtoContract]
	public partial class C2R_RankRunRaceRequest: Object, IRankActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.R2C_RankRunRaceResponse)]
	[ProtoContract]
	public partial class R2C_RankRunRaceResponse: Object, IRankActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RankingInfo> RankList = new List<RankingInfo>();

	}

	[Message(OuterOpcode.M2C_RankRunRaceMessage)]
	[ProtoContract]
	public partial class M2C_RankRunRaceMessage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<RankingInfo> RankList = new List<RankingInfo>();

	}

	[Message(OuterOpcode.M2C_RunRaceBattleInfo)]
	[ProtoContract]
	public partial class M2C_RunRaceBattleInfo: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long NextTransforTime { get; set; }

	}

	[Message(OuterOpcode.M2C_RankRunRaceReward)]
	[ProtoContract]
	public partial class M2C_RankRunRaceReward: Object, IActorMessage
	{
		[ProtoMember(1)]
		public int RankId { get; set; }

		[ProtoMember(2)]
		public int ByMail { get; set; }

		[ProtoMember(3)]
		public List<RewardItem> RewardList = new List<RewardItem>();

	}

	[Message(OuterOpcode.M2C_TurtleRewardMessage)]
	[ProtoContract]
	public partial class M2C_TurtleRewardMessage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long UnitID { get; set; }

		[ProtoMember(2)]
		public List<string> PlayerName = new List<string>();

	}

	[ResponseType(nameof(R2C_RankDemonResponse))]
	[Message(OuterOpcode.C2R_RankDemonRequest)]
	[ProtoContract]
	public partial class C2R_RankDemonRequest: Object, IRankActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.R2C_RankDemonResponse)]
	[ProtoContract]
	public partial class R2C_RankDemonResponse: Object, IRankActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RankingInfo> RankList = new List<RankingInfo>();

	}

	[Message(OuterOpcode.M2C_RankDemonMessage)]
	[ProtoContract]
	public partial class M2C_RankDemonMessage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<RankingInfo> RankList = new List<RankingInfo>();

	}

	[Message(OuterOpcode.M2C_UnitNumericListUpdate)]
	[ProtoContract]
	public partial class M2C_UnitNumericListUpdate: Object, IActorMessage
	{
		[ProtoMember(1)]
		public long UnitID { get; set; }

		[ProtoMember(2)]
		public List<int> Ks = new List<int>();

		[ProtoMember(3)]
		public List<long> Vs = new List<long>();

	}

//个人副本喜从天降移动
	[ResponseType(nameof(M2C_DungeonHappyMoveResponse))]
	[Message(OuterOpcode.C2M_DungeonHappyMoveRequest)]
	[ProtoContract]
	public partial class C2M_DungeonHappyMoveRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int OperatateType { get; set; }

	}

	[Message(OuterOpcode.M2C_DungeonHappyMoveResponse)]
	[ProtoContract]
	public partial class M2C_DungeonHappyMoveResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_ItemOperateWearResponse))]
//猎人穿戴装备特殊处理
	[Message(OuterOpcode.C2M_ItemOperateWearRequest)]
	[ProtoContract]
	public partial class C2M_ItemOperateWearRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public long OperateBagID { get; set; }

		[ProtoMember(3)]
		public string OperatePar { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemOperateWearResponse)]
	[ProtoContract]
	public partial class M2C_ItemOperateWearResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public string OperatePar { get; set; }

	}

	[ResponseType(nameof(M2C_ItemEquipIndexResponse))]
//猎人穿戴装备特殊处理
	[Message(OuterOpcode.C2M_ItemEquipIndexRequest)]
	[ProtoContract]
	public partial class C2M_ItemEquipIndexRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int EquipIndex { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemEquipIndexResponse)]
	[ProtoContract]
	public partial class M2C_ItemEquipIndexResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[Message(OuterOpcode.PetMingPlayerInfo)]
	[ProtoContract]
	public partial class PetMingPlayerInfo: Object
	{
		[ProtoMember(1)]
		public int MineType { get; set; }

		[ProtoMember(2)]
		public int Postion { get; set; }

		[ProtoMember(3)]
		public long UnitId { get; set; }

		[ProtoMember(4)]
		public string PlayerName { get; set; }

		[ProtoMember(5)]
		public List<int> PetConfig = new List<int>();

		[ProtoMember(6)]
		public List<long> PetIdList = new List<long>();

		[ProtoMember(7)]
		public int TeamId { get; set; }

		[ProtoMember(8)]
		public long OccupyTime { get; set; }

	}

	[ResponseType(nameof(A2C_PetMingListResponse))]
	[Message(OuterOpcode.C2A_PetMingListRequest)]
	[ProtoContract]
	public partial class C2A_PetMingListRequest: Object, IActivityActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.A2C_PetMingListResponse)]
	[ProtoContract]
	public partial class A2C_PetMingListResponse: Object, IActivityActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long ChanChu { get; set; }

		[ProtoMember(2)]
		public List<KeyValuePairInt> PetMineExtend = new List<KeyValuePairInt>();

		[ProtoMember(4)]
		public List<PetMingPlayerInfo> PetMingPlayerInfos = new List<PetMingPlayerInfo>();

	}

	[ResponseType(nameof(M2C_PetTargetLockResponse))]
	[Message(OuterOpcode.C2M_PetTargetLockRequest)]
	[ProtoContract]
	public partial class C2M_PetTargetLockRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long TargetId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetTargetLockResponse)]
	[ProtoContract]
	public partial class M2C_PetTargetLockResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2C_RankTrialListResponse))]
	[Message(OuterOpcode.C2R_RankTrialListRequest)]
	[ProtoContract]
	public partial class C2R_RankTrialListRequest: Object, IRankActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.R2C_RankTrialListResponse)]
	[ProtoContract]
	public partial class R2C_RankTrialListResponse: Object, IRankActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RankingTrialInfo> RankList = new List<RankingTrialInfo>();

	}

	[ResponseType(nameof(M2C_TeamerPositionResponse))]
	[Message(OuterOpcode.C2M_TeamerPositionRequest)]
	[ProtoContract]
	public partial class C2M_TeamerPositionRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_TeamerPositionResponse)]
	[ProtoContract]
	public partial class M2C_TeamerPositionResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<UnitInfo> UnitList = new List<UnitInfo>();

	}

	[ResponseType(nameof(M2C_GMCustomResponse))]
	[Message(OuterOpcode.C2M_GMCustomRequest)]
	[ProtoContract]
	public partial class C2M_GMCustomRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_GMCustomResponse)]
	[ProtoContract]
	public partial class M2C_GMCustomResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_PetMingRewardResponse))]
	[Message(OuterOpcode.C2M_PetMingRewardRequest)]
	[ProtoContract]
	public partial class C2M_PetMingRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Number { get; set; }

	}

	[Message(OuterOpcode.M2C_PetMingRewardResponse)]
	[ProtoContract]
	public partial class M2C_PetMingRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2C_PetMingChanChuResponse))]
	[Message(OuterOpcode.C2A_PetMingChanChuRequest)]
	[ProtoContract]
	public partial class C2A_PetMingChanChuRequest: Object, IActivityActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.A2C_PetMingChanChuResponse)]
	[ProtoContract]
	public partial class A2C_PetMingChanChuResponse: Object, IActivityActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(F2C_WatchPetResponse))]
	[Message(OuterOpcode.C2F_WatchPetRequest)]
	[ProtoContract]
	public partial class C2F_WatchPetRequest: Object, IFriendActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitID { get; set; }

		[ProtoMember(2)]
		public long PetId { get; set; }

	}

	[Message(OuterOpcode.F2C_WatchPetResponse)]
	[ProtoContract]
	public partial class F2C_WatchPetResponse: Object, IFriendActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(7)]
		public RolePetInfo RolePetInfos { get; set; }

		[ProtoMember(8)]
		public List<BagInfo> PetHeXinList = new List<BagInfo>();

		[ProtoMember(9)]
		public List<int> Ks = new List<int>();

		[ProtoMember(10)]
		public List<long> Vs = new List<long>();

	}

	[Message(OuterOpcode.PetMingRecord)]
	[ProtoContract]
	public partial class PetMingRecord: Object
	{
		[ProtoMember(1)]
		public long UnitID { get; set; }

		[ProtoMember(2)]
		public long Time { get; set; }

		[ProtoMember(3)]
		public int MineType { get; set; }

		[ProtoMember(4)]
		public int Position { get; set; }

		[ProtoMember(5)]
		public string WinPlayer { get; set; }

	}

	[ResponseType(nameof(M2C_PetMingRecordResponse))]
	[Message(OuterOpcode.C2M_PetMingRecordRequest)]
	[ProtoContract]
	public partial class C2M_PetMingRecordRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetMingRecordResponse)]
	[ProtoContract]
	public partial class M2C_PetMingRecordResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<PetMingRecord> PetMingRecords = new List<PetMingRecord>();

	}

//赛季等级奖励
	[ResponseType(nameof(M2C_SeasonLevelRewardResponse))]
	[Message(OuterOpcode.C2M_SeasonLevelRewardRequest)]
	[ProtoContract]
	public partial class C2M_SeasonLevelRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int SeasonLevel { get; set; }

	}

	[Message(OuterOpcode.M2C_SeasonLevelRewardResponse)]
	[ProtoContract]
	public partial class M2C_SeasonLevelRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//使用赛季果实， 更新boss刷新时间
	[ResponseType(nameof(M2C_SeasonUseFruitResponse))]
	[Message(OuterOpcode.C2M_SeasonUseFruitRequest)]
	[ProtoContract]
	public partial class C2M_SeasonUseFruitRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<long> BagInfoIDs = new List<long>();

	}

	[Message(OuterOpcode.M2C_SeasonUseFruitResponse)]
	[ProtoContract]
	public partial class M2C_SeasonUseFruitResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//开启晶核
	[ResponseType(nameof(M2C_SeasonOpenJingHeResponse))]
	[Message(OuterOpcode.C2M_SeasonOpenJingHeRequest)]
	[ProtoContract]
	public partial class C2M_SeasonOpenJingHeRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int JingHeId { get; set; }

	}

	[Message(OuterOpcode.M2C_SeasonOpenJingHeResponse)]
	[ProtoContract]
	public partial class M2C_SeasonOpenJingHeResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//幸运抽奖. 随机一个位置，并不会会道具， 该位置每天是固定的
	[ResponseType(nameof(M2C_WelfareDrawResponse))]
	[Message(OuterOpcode.C2M_WelfareDrawRequest)]
	[ProtoContract]
	public partial class C2M_WelfareDrawRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_WelfareDrawResponse)]
	[ProtoContract]
	public partial class M2C_WelfareDrawResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//转盘结束，给予道具
	[ResponseType(nameof(M2C_WelfareDrawRewardResponse))]
	[Message(OuterOpcode.C2M_WelfareDrawRewardRequest)]
	[ProtoContract]
	public partial class C2M_WelfareDrawRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_WelfareDrawRewardResponse)]
	[ProtoContract]
	public partial class M2C_WelfareDrawRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//当前的任务全部完成，才可以领取
	[ResponseType(nameof(M2C_WelfareTaskRewardResponse))]
	[Message(OuterOpcode.C2M_WelfareTaskRewardRequest)]
	[ProtoContract]
	public partial class C2M_WelfareTaskRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int day { get; set; }

	}

	[Message(OuterOpcode.M2C_WelfareTaskRewardResponse)]
	[ProtoContract]
	public partial class M2C_WelfareTaskRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//投资
	[ResponseType(nameof(M2C_WelfareInvestResponse))]
	[Message(OuterOpcode.C2M_WelfareInvestRequest)]
	[ProtoContract]
	public partial class C2M_WelfareInvestRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Index { get; set; }

	}

	[Message(OuterOpcode.M2C_WelfareInvestResponse)]
	[ProtoContract]
	public partial class M2C_WelfareInvestResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//投资奖励。第七天可以领取奖励
	[ResponseType(nameof(M2C_WelfareInvestRewardResponse))]
	[Message(OuterOpcode.C2M_WelfareInvestRewardRequest)]
	[ProtoContract]
	public partial class C2M_WelfareInvestRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_WelfareInvestRewardResponse)]
	[ProtoContract]
	public partial class M2C_WelfareInvestRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//累计充值奖励
	[ResponseType(nameof(M2C_RechargeRewardResponse))]
	[Message(OuterOpcode.C2M_RechargeRewardRequest)]
	[ProtoContract]
	public partial class C2M_RechargeRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RechargeNumber { get; set; }

	}

	[Message(OuterOpcode.M2C_RechargeRewardResponse)]
	[ProtoContract]
	public partial class M2C_RechargeRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_PetMingOccupyResponse))]
	[Message(OuterOpcode.C2M_PetMingOccupyRequest)]
	[ProtoContract]
	public partial class C2M_PetMingOccupyRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Operate { get; set; }

	}

	[Message(OuterOpcode.M2C_PetMingOccupyResponse)]
	[ProtoContract]
	public partial class M2C_PetMingOccupyResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_PetMingResetResponse))]
	[Message(OuterOpcode.C2M_PetMingResetRequest)]
	[ProtoContract]
	public partial class C2M_PetMingResetRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetMingResetResponse)]
	[ProtoContract]
	public partial class M2C_PetMingResetResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2C_PaiMaiFindResponse))]
	[Message(OuterOpcode.C2P_PaiMaiFindRequest)]
	[ProtoContract]
	public partial class C2P_PaiMaiFindRequest: Object, IPaiMaiListRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ItemType { get; set; }

		[ProtoMember(2)]
		public long PaiMaiItemInfoId { get; set; }

	}

	[Message(OuterOpcode.P2C_PaiMaiFindResponse)]
	[ProtoContract]
	public partial class P2C_PaiMaiFindResponse: Object, IPaiMaiListResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int Page { get; set; }

	}

	[ResponseType(nameof(A2C_TikTokVerifyUser))]
	[Message(OuterOpcode.C2A_TikTokVerifyUser)]
	[ProtoContract]
	public partial class C2A_TikTokVerifyUser: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string access_token { get; set; }

		[ProtoMember(2)]
		public string sdk_open_id { get; set; }

		[ProtoMember(3)]
		public int age_type { get; set; }

	}

	[Message(OuterOpcode.A2C_TikTokVerifyUser)]
	[ProtoContract]
	public partial class A2C_TikTokVerifyUser: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string sdk_open_id { get; set; }

		[ProtoMember(2)]
		public int age_type { get; set; }

	}

	[ResponseType(nameof(A2C_TapTapAuther))]
	[Message(OuterOpcode.C2A_TapTapAuther)]
	[ProtoContract]
	public partial class C2A_TapTapAuther: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

		[ProtoMember(3)]
		public int LoginType { get; set; }

		[ProtoMember(4)]
		public int age_type { get; set; }

	}

	[Message(OuterOpcode.A2C_TapTapAuther)]
	[ProtoContract]
	public partial class A2C_TapTapAuther: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//宠物更新
	[Message(OuterOpcode.M2C_PetListMessage)]
	[ProtoContract]
	public partial class M2C_PetListMessage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<RolePetInfo> PetList = new List<RolePetInfo>();

		[ProtoMember(2)]
		public long RemovePetId { get; set; }

	}

	[Message(OuterOpcode.RankingTrialInfo)]
	[ProtoContract]
	public partial class RankingTrialInfo: Object
	{
		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public string PlayerName { get; set; }

		[ProtoMember(3)]
		public int PlayerLv { get; set; }

		[ProtoMember(4)]
		public long Hurt { get; set; }

		[ProtoMember(5)]
		public int Occ { get; set; }

		[ProtoMember(6)]
		public int FubenId { get; set; }

	}

//一键存放
	[ResponseType(nameof(M2C_ItemQuickPutResponse))]
	[Message(OuterOpcode.C2M_ItemQuickPutRequest)]
	[ProtoContract]
	public partial class C2M_ItemQuickPutRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int HorseId { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemQuickPutResponse)]
	[ProtoContract]
	public partial class M2C_ItemQuickPutResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

//晶核注入
	[ResponseType(nameof(M2C_JingHeZhuruResponse))]
	[Message(OuterOpcode.C2M_JingHeZhuruRequest)]
	[ProtoContract]
	public partial class C2M_JingHeZhuruRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long BagInfoId { get; set; }

		[ProtoMember(2)]
		public List<long> OperateBagID = new List<long>();

	}

	[Message(OuterOpcode.M2C_JingHeZhuruResponse)]
	[ProtoContract]
	public partial class M2C_JingHeZhuruResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//晶核激活
	[ResponseType(nameof(M2C_JingHeActivateResponse))]
	[Message(OuterOpcode.C2M_JingHeActivateRequest)]
	[ProtoContract]
	public partial class C2M_JingHeActivateRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long BagInfoId { get; set; }

	}

	[Message(OuterOpcode.M2C_JingHeActivateResponse)]
	[ProtoContract]
	public partial class M2C_JingHeActivateResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//晶核使用方案
	[ResponseType(nameof(M2C_JingHePlanResponse))]
	[Message(OuterOpcode.C2M_JingHePlanRequest)]
	[ProtoContract]
	public partial class C2M_JingHePlanRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int JingHePlan { get; set; }

	}

	[Message(OuterOpcode.M2C_JingHePlanResponse)]
	[ProtoContract]
	public partial class M2C_JingHePlanResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.M2C_UpdateUserInfoMessage)]
	[ProtoContract]
	public partial class M2C_UpdateUserInfoMessage: Object, IActorMessage
	{
		[ProtoMember(1)]
		public UserInfo UserInfo { get; set; }

	}

	[ResponseType(nameof(M2C_JingHeWearResponse))]
	[Message(OuterOpcode.C2M_JingHeWearRequest)]
	[ProtoContract]
	public partial class C2M_JingHeWearRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public long OperateBagID { get; set; }

		[ProtoMember(3)]
		public string OperatePar { get; set; }

	}

	[Message(OuterOpcode.M2C_JingHeWearResponse)]
	[ProtoContract]
	public partial class M2C_JingHeWearResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_TaskOnLoginResponse))]
	[Message(OuterOpcode.C2M_TaskOnLoginRequest)]
	[ProtoContract]
	public partial class C2M_TaskOnLoginRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(OuterOpcode.M2C_TaskOnLoginResponse)]
	[ProtoContract]
	public partial class M2C_TaskOnLoginResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_LeavlRewardResponse))]
	[Message(OuterOpcode.C2M_LeavlRewardRequest)]
	[ProtoContract]
	public partial class C2M_LeavlRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int LvKey { get; set; }

		[ProtoMember(2)]
		public int Index { get; set; }

	}

	[Message(OuterOpcode.M2C_LeavlRewardResponse)]
	[ProtoContract]
	public partial class M2C_LeavlRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_KillMonsterRewardResponse))]
	[Message(OuterOpcode.C2M_KillMonsterRewardRequest)]
	[ProtoContract]
	public partial class C2M_KillMonsterRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Key { get; set; }

		[ProtoMember(2)]
		public int Index { get; set; }

	}

	[Message(OuterOpcode.M2C_KillMonsterRewardResponse)]
	[ProtoContract]
	public partial class M2C_KillMonsterRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(P2C_StallListResponse))]
	[Message(OuterOpcode.C2P_StallListRequest)]
	[ProtoContract]
	public partial class C2P_StallListRequest: Object, IPaiMaiListRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserId { get; set; }

	}

	[Message(OuterOpcode.P2C_StallListResponse)]
	[ProtoContract]
	public partial class P2C_StallListResponse: Object, IPaiMaiListResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<PaiMaiItemInfo> PaiMaiItemInfos = new List<PaiMaiItemInfo>();

	}

	[ResponseType(nameof(M2C_StallSellResponse))]
	[Message(OuterOpcode.C2M_StallSellRequest)]
	[ProtoContract]
	public partial class C2M_StallSellRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_StallSellResponse)]
	[ProtoContract]
	public partial class M2C_StallSellResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

	}

	[ResponseType(nameof(M2C_StallBuyResponse))]
	[Message(OuterOpcode.C2M_StallBuyRequest)]
	[ProtoContract]
	public partial class C2M_StallBuyRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

	}

	[Message(OuterOpcode.M2C_StallBuyResponse)]
	[ProtoContract]
	public partial class M2C_StallBuyResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_StallXiaJiaResponse))]
	[Message(OuterOpcode.C2M_StallXiaJiaRequest)]
	[ProtoContract]
	public partial class C2M_StallXiaJiaRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long PaiMaiItemInfoId { get; set; }

	}

	[Message(OuterOpcode.M2C_StallXiaJiaResponse)]
	[ProtoContract]
	public partial class M2C_StallXiaJiaResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(OuterOpcode.RankSeasonTowerInfo)]
	[ProtoContract]
	public partial class RankSeasonTowerInfo: Object
	{
		[ProtoMember(1)]
		public long UserId { get; set; }

		[ProtoMember(2)]
		public string PlayerName { get; set; }

		[ProtoMember(3)]
		public int Occ { get; set; }

		[ProtoMember(4)]
		public int PlayerLv { get; set; }

		[ProtoMember(5)]
		public int FubenId { get; set; }

		[ProtoMember(6)]
		public long TotalTime { get; set; }

	}

	[ResponseType(nameof(R2C_RankSeasonTowerResponse))]
	[Message(OuterOpcode.C2R_RankSeasonTowerRequest)]
	[ProtoContract]
	public partial class C2R_RankSeasonTowerRequest: Object, IRankActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.R2C_RankSeasonTowerResponse)]
	[ProtoContract]
	public partial class R2C_RankSeasonTowerResponse: Object, IRankActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<RankSeasonTowerInfo> RankList = new List<RankSeasonTowerInfo>();

	}

	[ResponseType(nameof(M2C_PetEquipResponse))]
//宠物装备
	[Message(OuterOpcode.C2M_PetEquipRequest)]
	[ProtoContract]
	public partial class C2M_PetEquipRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

		[ProtoMember(2)]
		public long BagInfoId { get; set; }

		[ProtoMember(4)]
		public int OperateType { get; set; }

	}

	[Message(OuterOpcode.M2C_PetEquipResponse)]
	[ProtoContract]
	public partial class M2C_PetEquipResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public RolePetInfo RolePetInfo { get; set; }

	}

//好友列表
	[ResponseType(nameof(F2C_FriendChatRead))]
	[Message(OuterOpcode.C2F_FriendChatRead)]
	[ProtoContract]
	public partial class C2F_FriendChatRead: Object, IFriendActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

		[ProtoMember(2)]
		public long FriendID { get; set; }

	}

	[Message(OuterOpcode.F2C_FriendChatRead)]
	[ProtoContract]
	public partial class F2C_FriendChatRead: Object, IFriendActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//第一版的活动
	[Message(OuterOpcode.ActivityV1Info)]
	[ProtoContract]
	public partial class ActivityV1Info: Object
	{
		[ProtoMember(1)]
		public List<int> GuessIds = new List<int>();

		[ProtoMember(2)]
		public List<int> LastGuessReward = new List<int>();

		[ProtoMember(3)]
		public List<int> ConsumeDiamondReward = new List<int>();

		[ProtoMember(4)]
		public List<int> ChouKaNumberReward = new List<int>();

		[ProtoMember(5)]
		public int ChouKaDropId { get; set; }

		[ProtoMember(6)]
		public List<int> LiBaoAllIds = new List<int>();

		[ProtoMember(7)]
		public List<int> LiBaoBuyIds = new List<int>();

		[ProtoMember(8)]
		public int BaoShiDu { get; set; }

		[ProtoMember(9)]
		public string ChouKa2ItemList { get; set; }

		[ProtoMember(10)]
		public List<int> ChouKa2RewardIds = new List<int>();

		[ProtoMember(11)]
		public List<int> OpenGuessIds = new List<int>();

	}

//v1活动.抽奖
	[ResponseType(nameof(M2C_ActivityChouKaResponse))]
	[Message(OuterOpcode.C2M_ActivityChouKaRequest)]
	[ProtoContract]
	public partial class C2M_ActivityChouKaRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_ActivityChouKaResponse)]
	[ProtoContract]
	public partial class M2C_ActivityChouKaResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_ActivityRewardResponse))]
	[Message(OuterOpcode.C2M_ActivityRewardRequest)]
	[ProtoContract]
	public partial class C2M_ActivityRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ActivityType { get; set; }

		[ProtoMember(2)]
		public int RewardId { get; set; }

	}

	[Message(OuterOpcode.M2C_ActivityRewardResponse)]
	[ProtoContract]
	public partial class M2C_ActivityRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public ActivityV1Info ActivityV1Info { get; set; }

	}

	[ResponseType(nameof(M2C_ActivityGuessResponse))]
	[Message(OuterOpcode.C2M_ActivityGuessRequest)]
	[ProtoContract]
	public partial class C2M_ActivityGuessRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int GuessId { get; set; }

	}

	[Message(OuterOpcode.M2C_ActivityGuessResponse)]
	[ProtoContract]
	public partial class M2C_ActivityGuessResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//刷新奖励
	[ResponseType(nameof(M2C_ChouKa2RefreshResponse))]
	[Message(OuterOpcode.C2M_ChouKa2RefreshRequest)]
	[ProtoContract]
	public partial class C2M_ChouKa2RefreshRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_ChouKa2RefreshResponse)]
	[ProtoContract]
	public partial class M2C_ChouKa2RefreshResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public ActivityV1Info ActivityV1Info { get; set; }

	}

//喂食物
	[ResponseType(nameof(M2C_ActivityFeedResponse))]
	[Message(OuterOpcode.C2M_ActivityFeedRequest)]
	[ProtoContract]
	public partial class C2M_ActivityFeedRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ItemID { get; set; }

	}

	[Message(OuterOpcode.M2C_ActivityFeedResponse)]
	[ProtoContract]
	public partial class M2C_ActivityFeedResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public ActivityV1Info ActivityV1Info { get; set; }

	}

	[ResponseType(nameof(M2C_PetExploreReward))]
	[Message(OuterOpcode.C2M_PetExploreReward)]
	[ProtoContract]
	public partial class C2M_PetExploreReward: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RewardId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetExploreReward)]
	[ProtoContract]
	public partial class M2C_PetExploreReward: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

//宠物更新
	[Message(OuterOpcode.M2C_RolePetBagUpdate)]
	[ProtoContract]
	public partial class M2C_RolePetBagUpdate: Object, IActorMessage
	{
		[ProtoMember(1)]
		public List<RolePetInfo> RolePetBag = new List<RolePetInfo>();

		[ProtoMember(2)]
		public int UpdateMode { get; set; }

	}

	[ResponseType(nameof(M2C_PetTakeOutBag))]
	[Message(OuterOpcode.C2M_PetTakeOutBag)]
	[ProtoContract]
	public partial class C2M_PetTakeOutBag: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long PetInfoId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetTakeOutBag)]
	[ProtoContract]
	public partial class M2C_PetTakeOutBag: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_PetHeXinChouKaResponse))]
	[Message(OuterOpcode.C2M_PetHeXinChouKaRequest)]
	[ProtoContract]
	public partial class C2M_PetHeXinChouKaRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int ChouKaType { get; set; }

	}

	[Message(OuterOpcode.M2C_PetHeXinChouKaResponse)]
	[ProtoContract]
	public partial class M2C_PetHeXinChouKaResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public List<RewardItem> ReardList = new List<RewardItem>();

	}

	[ResponseType(nameof(M2C_PetHeXinExploreReward))]
	[Message(OuterOpcode.C2M_PetHeXinExploreReward)]
	[ProtoContract]
	public partial class C2M_PetHeXinExploreReward: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RewardId { get; set; }

	}

	[Message(OuterOpcode.M2C_PetHeXinExploreReward)]
	[ProtoContract]
	public partial class M2C_PetHeXinExploreReward: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_WelfareDraw2Response))]
	[Message(OuterOpcode.C2M_WelfareDraw2Request)]
	[ProtoContract]
	public partial class C2M_WelfareDraw2Request: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_WelfareDraw2Response)]
	[ProtoContract]
	public partial class M2C_WelfareDraw2Response: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2C_WelfareDraw2RewardResponse))]
	[Message(OuterOpcode.C2M_WelfareDraw2RewardRequest)]
	[ProtoContract]
	public partial class C2M_WelfareDraw2RewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.M2C_WelfareDraw2RewardResponse)]
	[ProtoContract]
	public partial class M2C_WelfareDraw2RewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(E2C_AccountWarehousInfoResponse))]
	[Message(OuterOpcode.C2E_AccountWarehousInfoRequest)]
	[ProtoContract]
	public partial class C2E_AccountWarehousInfoRequest: Object, IMailActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long AccInfoID { get; set; }

	}

	[Message(OuterOpcode.E2C_AccountWarehousInfoResponse)]
	[ProtoContract]
	public partial class E2C_AccountWarehousInfoResponse: Object, IMailActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<BagInfo> BagInfos = new List<BagInfo>();

	}

	[ResponseType(nameof(M2C_AccountWarehousOperateResponse))]
	[Message(OuterOpcode.C2M_AccountWarehousOperateRequest)]
	[ProtoContract]
	public partial class C2M_AccountWarehousOperateRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public int OperatateType { get; set; }

		[ProtoMember(3)]
		public long OperateBagID { get; set; }

	}

	[Message(OuterOpcode.M2C_AccountWarehousOperateResponse)]
	[ProtoContract]
	public partial class M2C_AccountWarehousOperateResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public BagInfo BagInfo { get; set; }

	}

	[ResponseType(nameof(M2C_SingleRechargeRewardResponse))]
	[Message(OuterOpcode.C2M_SingleRechargeRewardRequest)]
	[ProtoContract]
	public partial class C2M_SingleRechargeRewardRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RewardId { get; set; }

	}

	[Message(OuterOpcode.M2C_SingleRechargeRewardResponse)]
	[ProtoContract]
	public partial class M2C_SingleRechargeRewardResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<int> RewardIds = new List<int>();

	}

	[ResponseType(nameof(M2C_ItemXiLianNumReward))]
	[Message(OuterOpcode.C2M_ItemXiLianNumReward)]
	[ProtoContract]
	public partial class C2M_ItemXiLianNumReward: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int RewardId { get; set; }

	}

	[Message(OuterOpcode.M2C_ItemXiLianNumReward)]
	[ProtoContract]
	public partial class M2C_ItemXiLianNumReward: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(M2C_BloodstoneQiangHuaResponse))]
	[Message(OuterOpcode.C2M_BloodstoneQiangHuaRequest)]
	[ProtoContract]
	public partial class C2M_BloodstoneQiangHuaRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int QiangHuaType { get; set; }

	}

	[Message(OuterOpcode.M2C_BloodstoneQiangHuaResponse)]
	[ProtoContract]
	public partial class M2C_BloodstoneQiangHuaResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public int Level { get; set; }

	}

	[ResponseType(nameof(Chat2C_GetChatResponse))]
	[Message(OuterOpcode.C2Chat_GetChatRequest)]
	[ProtoContract]
	public partial class C2Chat_GetChatRequest: Object, IChatActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(OuterOpcode.Chat2C_GetChatResponse)]
	[ProtoContract]
	public partial class Chat2C_GetChatResponse: Object, IChatActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<ChatInfo> ChatInfos = new List<ChatInfo>();

	}

	[ResponseType(nameof(M2C_OneChallengeResponse))]
	[Message(OuterOpcode.C2M_OneChallengeRequest)]
	[ProtoContract]
	public partial class C2M_OneChallengeRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long OtherId { get; set; }

		[ProtoMember(2)]
		public int Operatate { get; set; }

	}

	[Message(OuterOpcode.M2C_OneChallengeResponse)]
	[ProtoContract]
	public partial class M2C_OneChallengeResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

		[ProtoMember(1)]
		public int Operatate { get; set; }

	}

	[Message(OuterOpcode.M2C_OneChallenge)]
	[ProtoContract]
	public partial class M2C_OneChallenge: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Operatate { get; set; }

		[ProtoMember(2)]
		public long OtherId { get; set; }

		[ProtoMember(3)]
		public string OtherName { get; set; }

	}

	[ResponseType(nameof(M2C_PetChangePosResponse))]
	[Message(OuterOpcode.C2M_PetChangePosRequest)]
	[ProtoContract]
	public partial class C2M_PetChangePosRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int Index1 { get; set; }

		[ProtoMember(2)]
		public int Index2 { get; set; }

	}

	[Message(OuterOpcode.M2C_PetChangePosResponse)]
	[ProtoContract]
	public partial class M2C_PetChangePosResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

//二段技能
	[Message(OuterOpcode.M2C_SkillSecondResult)]
	[ProtoContract]
	public partial class M2C_SkillSecondResult: Object, IActorMessage
	{
		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int SkillId { get; set; }

		[ProtoMember(3)]
		public List<long> HurtIds = new List<long>();

	}

	[ResponseType(nameof(P2C_PaiMaiSearchResponse))]
	[Message(OuterOpcode.C2P_PaiMaiSearchRequest)]
	[ProtoContract]
	public partial class C2P_PaiMaiSearchRequest: Object, IPaiMaiListRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public List<int> FindItemIdList = new List<int>();

		[ProtoMember(2)]
		public List<int> FindTypeList = new List<int>();

	}

	[Message(OuterOpcode.P2C_PaiMaiSearchResponse)]
	[ProtoContract]
	public partial class P2C_PaiMaiSearchResponse: Object, IPaiMaiListResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<PaiMaiItemInfo> PaiMaiItemInfos = new List<PaiMaiItemInfo>();

	}

	[ResponseType(nameof(G2C_EnterGame))]
	[Message(OuterOpcode.C2G_EnterGameCheck)]
	[ProtoContract]
	public partial class C2G_EnterGameCheck: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int MapId { get; set; }

		[ProtoMember(2)]
		public long UserID { get; set; }

		[ProtoMember(3)]
		public long AccountId { get; set; }

		[ProtoMember(4)]
		public bool Relink { get; set; }

		[ProtoMember(5)]
		public string DeviceName { get; set; }

		[ProtoMember(6)]
		public int Version { get; set; }

		[ProtoMember(7)]
		public int Platform { get; set; }

		[ProtoMember(8)]
		public int Simulator { get; set; }

		[ProtoMember(9)]
		public int Root { get; set; }

		[ProtoMember(10)]
		public int IsRecharge { get; set; }

		[ProtoMember(11)]
		public string DeviceID { get; set; }

	}

	[Message(OuterOpcode.G2C_EnterGameCheck)]
	[ProtoContract]
	public partial class G2C_EnterGameCheck: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

// 自己的unit id
		[ProtoMember(1)]
		public long MyId { get; set; }

		[ProtoMember(2)]
		public int IsPopUp { get; set; }

		[ProtoMember(3)]
		public string PopUpInfo { get; set; }

	}

	[ResponseType(nameof(R2C_LoginRealmCheck))]
	[Message(OuterOpcode.C2R_LoginRealmCheck)]
	[ProtoContract]
	public partial class C2R_LoginRealmCheck: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string RealmTokenKey { get; set; }

	}

	[Message(OuterOpcode.R2C_LoginRealmCheck)]
	[ProtoContract]
	public partial class R2C_LoginRealmCheck: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string GateSessionKey { get; set; }

		[ProtoMember(2)]
		public string GateAddress { get; set; }

	}

	[ResponseType(nameof(R2C_LoginGatemCheck))]
	[Message(OuterOpcode.C2R_LoginGateCheck)]
	[ProtoContract]
	public partial class C2R_LoginGateCheck: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string RealmTokenKey { get; set; }

	}

	[Message(OuterOpcode.R2C_LoginGatemCheck)]
	[ProtoContract]
	public partial class R2C_LoginGatemCheck: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string GateSessionKey { get; set; }

		[ProtoMember(2)]
		public string GateAddress { get; set; }

	}

	[ResponseType(nameof(Center2C_BlackAccountResponse))]
	[Message(OuterOpcode.C2Center_QueryAccountRequest)]
	[ProtoContract]
	public partial class C2Center_QueryAccountRequest: Object, IRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string UserName { get; set; }

	}

	[Message(OuterOpcode.Center2C_QueryAccountResponse)]
	[ProtoContract]
	public partial class Center2C_QueryAccountResponse: Object, IResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Account { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

	}

}
