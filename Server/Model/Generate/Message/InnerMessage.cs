using ET;
using ProtoBuf;
using System.Collections.Generic;
namespace ET
{
	[ResponseType(nameof(ObjectQueryResponse))]
	[Message(InnerOpcode.ObjectQueryRequest)]
	[ProtoContract]
	public partial class ObjectQueryRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long InstanceId { get; set; }

	}

	[ResponseType(nameof(A2G_Reload))]
	[Message(InnerOpcode.G2A_Reload)]
	[ProtoContract]
	public partial class G2A_Reload: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public int LoadType { get; set; }

		[ProtoMember(4)]
		public string LoadValue { get; set; }

	}

	[Message(InnerOpcode.A2G_Reload)]
	[ProtoContract]
	public partial class A2G_Reload: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//广播
	[ResponseType(nameof(A2R_Broadcast))]
	[Message(InnerOpcode.R2A_Broadcast)]
	[ProtoContract]
	public partial class R2A_Broadcast: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public int LoadType { get; set; }

		[ProtoMember(4)]
		public string LoadValue { get; set; }

		[ProtoMember(6)]
		public ServerInfo ServerInfo { get; set; }

	}

	[Message(InnerOpcode.A2R_Broadcast)]
	[ProtoContract]
	public partial class A2R_Broadcast: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2G_LockResponse))]
	[Message(InnerOpcode.G2G_LockRequest)]
	[ProtoContract]
	public partial class G2G_LockRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public string Address { get; set; }

	}

	[Message(InnerOpcode.G2G_LockResponse)]
	[ProtoContract]
	public partial class G2G_LockResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2M_RechargeResponse))]
	[Message(InnerOpcode.M2R_RechargeRequest)]
	[ProtoContract]
	public partial class M2R_RechargeRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long UnitId { get; set; }

		[ProtoMember(1)]
		public int RechargeNumber { get; set; }

		[ProtoMember(2)]
		public long PayType { get; set; }

		[ProtoMember(3)]
		public int Zone { get; set; }

		[ProtoMember(4)]
		public string payMessage { get; set; }

		[ProtoMember(5)]
		public string UnitName { get; set; }

		[ProtoMember(6)]
		public string Account { get; set; }

		[ProtoMember(7)]
		public string ClientIp { get; set; }

	}

	[Message(InnerOpcode.R2M_RechargeResponse)]
	[ProtoContract]
	public partial class R2M_RechargeResponse: Object, IActorResponse
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

	[ResponseType(nameof(G2R_RechargeResultResponse))]
	[Message(InnerOpcode.R2G_RechargeResultRequest)]
	[ProtoContract]
	public partial class R2G_RechargeResultRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public int RechargeNumber { get; set; }

		[ProtoMember(3)]
		public long UserID { get; set; }

		[ProtoMember(4)]
		public string OrderInfo { get; set; }

		[ProtoMember(5)]
		public string CpOrder { get; set; }

		[ProtoMember(6)]
		public int RechargetType { get; set; }

	}

	[Message(InnerOpcode.G2R_RechargeResultResponse)]
	[ProtoContract]
	public partial class G2R_RechargeResultResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2G_RechargeResultResponse))]
	[Message(InnerOpcode.G2M_RechargeResultRequest)]
	[ProtoContract]
	public partial class G2M_RechargeResultRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public int RechargeNumber { get; set; }

		[ProtoMember(4)]
		public string OrderInfo { get; set; }

	}

	[Message(InnerOpcode.M2G_RechargeResultResponse)]
	[ProtoContract]
	public partial class M2G_RechargeResultResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Center2A_CenterServerList))]
	[Message(InnerOpcode.A2Center_CenterServerList)]
	[ProtoContract]
	public partial class A2Center_CenterServerList: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(InnerOpcode.Center2A_CenterServerList)]
	[ProtoContract]
	public partial class Center2A_CenterServerList: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<ServerItem> ServerItems = new List<ServerItem>();

	}

	[ResponseType(nameof(Center2A_CheckAccount))]
	[Message(InnerOpcode.A2Center_CheckAccount)]
	[ProtoContract]
	public partial class A2Center_CheckAccount: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string AccountName { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

		[ProtoMember(4)]
		public string ThirdLogin { get; set; }

		[ProtoMember(5)]
		public string DeviceID { get; set; }

		[ProtoMember(6)]
		public string OAID { get; set; }

	}

	[Message(InnerOpcode.Center2A_CheckAccount)]
	[ProtoContract]
	public partial class Center2A_CheckAccount: Object, IActorResponse
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
		public long AccountId { get; set; }

		[ProtoMember(3)]
		public bool IsHoliday { get; set; }

		[ProtoMember(4)]
		public bool StopServer { get; set; }

		[ProtoMember(5)]
		public string TaprepRequest { get; set; }

		[ProtoMember(13)]
		public int TodayCreateRole { get; set; }

	}

	[ResponseType(nameof(Center2A_SaveAccount))]
	[Message(InnerOpcode.A2Center_SaveAccount)]
	[ProtoContract]
	public partial class A2Center_SaveAccount: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string AccountName { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

		[ProtoMember(3)]
		public PlayerInfo PlayerInfo { get; set; }

		[ProtoMember(4)]
		public long AccountId { get; set; }

	}

	[Message(InnerOpcode.Center2A_SaveAccount)]
	[ProtoContract]
	public partial class Center2A_SaveAccount: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Center2A_RegisterAccount))]
	[Message(InnerOpcode.A2Center_RegisterAccount)]
	[ProtoContract]
	public partial class A2Center_RegisterAccount: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string AccountName { get; set; }

		[ProtoMember(2)]
		public string Password { get; set; }

		[ProtoMember(3)]
		public int LoginType { get; set; }

		[ProtoMember(4)]
		public int age_type { get; set; }

	}

	[Message(InnerOpcode.Center2A_RegisterAccount)]
	[ProtoContract]
	public partial class Center2A_RegisterAccount: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

	}

	[ResponseType(nameof(C2C_CenterServerInfoRespone))]
	[Message(InnerOpcode.C2C_CenterServerInfoReuest)]
	[ProtoContract]
	public partial class C2C_CenterServerInfoReuest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int infoType { get; set; }

		[ProtoMember(2)]
		public int Zone { get; set; }

	}

	[Message(InnerOpcode.C2C_CenterServerInfoRespone)]
	[ProtoContract]
	public partial class C2C_CenterServerInfoRespone: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Value { get; set; }

	}

	[ResponseType(nameof(A2A_ServerMessageRResponse))]
	[Message(InnerOpcode.A2A_ServerMessageRequest)]
	[ProtoContract]
	public partial class A2A_ServerMessageRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public int MessageType { get; set; }

		[ProtoMember(5)]
		public string MessageValue { get; set; }

	}

	[Message(InnerOpcode.A2A_ServerMessageRResponse)]
	[ProtoContract]
	public partial class A2A_ServerMessageRResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2G_LockReleaseResponse))]
	[Message(InnerOpcode.G2G_LockReleaseRequest)]
	[ProtoContract]
	public partial class G2G_LockReleaseRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public string Address { get; set; }

	}

	[Message(InnerOpcode.G2G_LockReleaseResponse)]
	[ProtoContract]
	public partial class G2G_LockReleaseResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectAddResponse))]
	[Message(InnerOpcode.ObjectAddRequest)]
	[ProtoContract]
	public partial class ObjectAddRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.ObjectAddResponse)]
	[ProtoContract]
	public partial class ObjectAddResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectLockResponse))]
	[Message(InnerOpcode.ObjectLockRequest)]
	[ProtoContract]
	public partial class ObjectLockRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long InstanceId { get; set; }

		[ProtoMember(3)]
		public int Time { get; set; }

	}

	[Message(InnerOpcode.ObjectLockResponse)]
	[ProtoContract]
	public partial class ObjectLockResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectUnLockResponse))]
	[Message(InnerOpcode.ObjectUnLockRequest)]
	[ProtoContract]
	public partial class ObjectUnLockRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

		[ProtoMember(2)]
		public long OldInstanceId { get; set; }

		[ProtoMember(3)]
		public long InstanceId { get; set; }

	}

	[Message(InnerOpcode.ObjectUnLockResponse)]
	[ProtoContract]
	public partial class ObjectUnLockResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectRemoveResponse))]
	[Message(InnerOpcode.ObjectRemoveRequest)]
	[ProtoContract]
	public partial class ObjectRemoveRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

	}

	[Message(InnerOpcode.ObjectRemoveResponse)]
	[ProtoContract]
	public partial class ObjectRemoveResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(ObjectGetResponse))]
	[Message(InnerOpcode.ObjectGetRequest)]
	[ProtoContract]
	public partial class ObjectGetRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Key { get; set; }

	}

	[Message(InnerOpcode.ObjectGetResponse)]
	[ProtoContract]
	public partial class ObjectGetResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long InstanceId { get; set; }

	}

	[ResponseType(nameof(Q2G_ExitGame))]
	[Message(InnerOpcode.G2Q_ExitGame)]
	[ProtoContract]
	public partial class G2Q_ExitGame: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

	}

	[Message(InnerOpcode.Q2G_ExitGame)]
	[ProtoContract]
	public partial class Q2G_ExitGame: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2G_ExitGame))]
	[Message(InnerOpcode.G2A_ExitGame)]
	[ProtoContract]
	public partial class G2A_ExitGame: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

	}

	[Message(InnerOpcode.A2G_ExitGame)]
	[ProtoContract]
	public partial class A2G_ExitGame: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Q2A_EnterQueue))]
	[Message(InnerOpcode.A2Q_EnterQueue)]
	[ProtoContract]
	public partial class A2Q_EnterQueue: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public string Token { get; set; }

	}

	[Message(InnerOpcode.Q2A_EnterQueue)]
	[ProtoContract]
	public partial class Q2A_EnterQueue: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int QueueNumber { get; set; }

	}

	[ResponseType(nameof(L2G_AddLoginRecord))]
	[Message(InnerOpcode.G2L_AddLoginRecord)]
	[ProtoContract]
	public partial class G2L_AddLoginRecord: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public int ServerId { get; set; }

	}

	[Message(InnerOpcode.L2G_AddLoginRecord)]
	[ProtoContract]
	public partial class L2G_AddLoginRecord: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2G_RequestEnterGameState))]
	[Message(InnerOpcode.G2M_RequestEnterGameState)]
	[ProtoContract]
	public partial class G2M_RequestEnterGameState: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long GateSessionActorId { get; set; }

	}

	[Message(InnerOpcode.M2G_RequestEnterGameState)]
	[ProtoContract]
	public partial class M2G_RequestEnterGameState: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2L_DisconnectGateUnit))]
	[Message(InnerOpcode.L2G_DisconnectGateUnit)]
	[ProtoContract]
	public partial class L2G_DisconnectGateUnit: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(4)]
		public bool Relink { get; set; }

	}

	[Message(InnerOpcode.G2L_DisconnectGateUnit)]
	[ProtoContract]
	public partial class G2L_DisconnectGateUnit: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2G_RequestExitGame))]
	[Message(InnerOpcode.G2M_RequestExitGame)]
	[ProtoContract]
	public partial class G2M_RequestExitGame: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerOpcode.M2G_RequestExitGame)]
	[ProtoContract]
	public partial class M2G_RequestExitGame: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(L2G_RemoveLoginRecord))]
	[Message(InnerOpcode.G2L_RemoveLoginRecord)]
	[ProtoContract]
	public partial class G2L_RemoveLoginRecord: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public int ServerId { get; set; }

	}

	[Message(InnerOpcode.L2G_RemoveLoginRecord)]
	[ProtoContract]
	public partial class L2G_RemoveLoginRecord: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2A_GetRealmKey))]
	[Message(InnerOpcode.A2R_GetRealmKey)]
	[ProtoContract]
	public partial class A2R_GetRealmKey: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

	}

	[Message(InnerOpcode.R2A_GetRealmKey)]
	[ProtoContract]
	public partial class R2A_GetRealmKey: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string RealmKey { get; set; }

	}

	[ResponseType(nameof(G2R_GetLoginGateKey))]
	[Message(InnerOpcode.R2G_GetLoginGateKey)]
	[ProtoContract]
	public partial class R2G_GetLoginGateKey: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

	}

	[Message(InnerOpcode.G2R_GetLoginGateKey)]
	[ProtoContract]
	public partial class G2R_GetLoginGateKey: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string GateSessionKey { get; set; }

	}

	[Message(InnerOpcode.G2M_SessionDisconnect)]
	[ProtoContract]
	public partial class G2M_SessionDisconnect: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(InnerOpcode.G2M_ActivityUpdate)]
	[ProtoContract]
	public partial class G2M_ActivityUpdate: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ActivityType { get; set; }

	}

	[ResponseType(nameof(E2M_EMailSendResponse))]
	[Message(InnerOpcode.M2E_EMailSendRequest)]
	[ProtoContract]
	public partial class M2E_EMailSendRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(3)]
		public MailInfo MailInfo { get; set; }

		[ProtoMember(4)]
		public int GetWay { get; set; }

	}

	[Message(InnerOpcode.E2M_EMailSendResponse)]
	[ProtoContract]
	public partial class E2M_EMailSendResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(E2M_EMailReceiveResponse))]
	[Message(InnerOpcode.M2E_EMailReceiveRequest)]
	[ProtoContract]
	public partial class M2E_EMailReceiveRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Id { get; set; }

		[ProtoMember(2)]
		public long MailId { get; set; }

	}

	[Message(InnerOpcode.E2M_EMailReceiveResponse)]
	[ProtoContract]
	public partial class E2M_EMailReceiveResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(4)]
		public MailInfo MailInfo { get; set; }

	}

	[ResponseType(nameof(A2A_ActivityUpdateResponse))]
	[Message(InnerOpcode.A2A_ActivityUpdateRequest)]
	[ProtoContract]
	public partial class A2A_ActivityUpdateRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Hour { get; set; }

		[ProtoMember(2)]
		public int OpenDay { get; set; }

		[ProtoMember(3)]
		public int FunctionId { get; set; }

		[ProtoMember(4)]
		public int FunctionType { get; set; }

	}

	[Message(InnerOpcode.A2A_ActivityUpdateResponse)]
	[ProtoContract]
	public partial class A2A_ActivityUpdateResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2M_ZhanQuInfoResponse))]
	[Message(InnerOpcode.M2A_ZhanQuInfoRequest)]
	[ProtoContract]
	public partial class M2A_ZhanQuInfoRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserId { get; set; }

	}

	[Message(InnerOpcode.A2M_ZhanQuInfoResponse)]
	[ProtoContract]
	public partial class A2M_ZhanQuInfoResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<int> DayTeHui = new List<int>();

		[ProtoMember(2)]
		public List<ZhanQuReceiveNumber> ReceiveNum = new List<ZhanQuReceiveNumber>();

	}

	[ResponseType(nameof(A2M_ZhanQuReceiveResponse))]
	[Message(InnerOpcode.M2A_ZhanQuReceiveRequest)]
	[ProtoContract]
	public partial class M2A_ZhanQuReceiveRequest: Object, IActorRequest
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
		public long UnitId { get; set; }

	}

	[Message(InnerOpcode.A2M_ZhanQuReceiveResponse)]
	[ProtoContract]
	public partial class A2M_ZhanQuReceiveResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2M_PetRankUpdateResponse))]
	[Message(InnerOpcode.M2R_PetRankUpdateRequest)]
	[ProtoContract]
	public partial class M2R_PetRankUpdateRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long EnemyId { get; set; }

		[ProtoMember(2)]
		public RankPetInfo RankPetInfo { get; set; }

		[ProtoMember(3)]
		public int Win { get; set; }

	}

	[Message(InnerOpcode.R2M_PetRankUpdateResponse)]
	[ProtoContract]
	public partial class R2M_PetRankUpdateResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SelfRank { get; set; }

	}

	[ResponseType(nameof(R2M_RankUpdateResponse))]
	[Message(InnerOpcode.M2R_RankUpdateRequest)]
	[ProtoContract]
	public partial class M2R_RankUpdateRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int CampId { get; set; }

		[ProtoMember(2)]
		public RankingInfo RankingInfo { get; set; }

	}

	[Message(InnerOpcode.R2M_RankUpdateResponse)]
	[ProtoContract]
	public partial class R2M_RankUpdateResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int RankId { get; set; }

		[ProtoMember(2)]
		public int PetRankId { get; set; }

		[ProtoMember(3)]
		public int SoloRankId { get; set; }

		[ProtoMember(4)]
		public int OccRankId { get; set; }

	}

	[ResponseType(nameof(R2M_RankShowLieResponse))]
	[Message(InnerOpcode.M2R_RankShowLieRequest)]
	[ProtoContract]
	public partial class M2R_RankShowLieRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int CampId { get; set; }

		[ProtoMember(2)]
		public RankShouLieInfo RankingInfo { get; set; }

	}

	[Message(InnerOpcode.R2M_RankShowLieResponse)]
	[ProtoContract]
	public partial class R2M_RankShowLieResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2M_PaiMaiSellResponse))]
	[Message(InnerOpcode.M2P_PaiMaiSellRequest)]
	[ProtoContract]
	public partial class M2P_PaiMaiSellRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

		[ProtoMember(3)]
		public long UnitID { get; set; }

		[ProtoMember(4)]
		public long PaiMaiTodayGold { get; set; }

	}

	[Message(InnerOpcode.P2M_PaiMaiSellResponse)]
	[ProtoContract]
	public partial class P2M_PaiMaiSellResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2M_PaiMaiBuyResponse))]
	[Message(InnerOpcode.M2P_PaiMaiBuyRequest)]
	[ProtoContract]
	public partial class M2P_PaiMaiBuyRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

		[ProtoMember(2)]
		public long Gold { get; set; }

		[ProtoMember(3)]
		public int BuyNum { get; set; }

	}

	[Message(InnerOpcode.P2M_PaiMaiBuyResponse)]
	[ProtoContract]
	public partial class P2M_PaiMaiBuyResponse: Object, IActorResponse
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

	[ResponseType(nameof(P2M_PaiMaiXiaJiaResponse))]
	[Message(InnerOpcode.M2P_PaiMaiXiaJiaRequest)]
	[ProtoContract]
	public partial class M2P_PaiMaiXiaJiaRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ItemType { get; set; }

		[ProtoMember(2)]
		public long PaiMaiItemInfoId { get; set; }

		[ProtoMember(3)]
		public long UnitID { get; set; }

	}

	[Message(InnerOpcode.P2M_PaiMaiXiaJiaResponse)]
	[ProtoContract]
	public partial class P2M_PaiMaiXiaJiaResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

	}

	[ResponseType(nameof(P2M_PaiMaiShopResponse))]
	[Message(InnerOpcode.M2P_PaiMaiShopRequest)]
	[ProtoContract]
	public partial class M2P_PaiMaiShopRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ItemID { get; set; }

		[ProtoMember(2)]
		public int BuyNum { get; set; }

		[ProtoMember(3)]
		public int Price { get; set; }

	}

	[Message(InnerOpcode.P2M_PaiMaiShopResponse)]
	[ProtoContract]
	public partial class P2M_PaiMaiShopResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public PaiMaiShopItemInfo PaiMaiShopItemInfo { get; set; }

	}

	[ResponseType(nameof(E2P_PaiMaiOverTimeResponse))]
	[Message(InnerOpcode.P2E_PaiMaiOverTimeRequest)]
	[ProtoContract]
	public partial class P2E_PaiMaiOverTimeRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

	}

	[Message(InnerOpcode.E2P_PaiMaiOverTimeResponse)]
	[ProtoContract]
	public partial class E2P_PaiMaiOverTimeResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2M_MysteryBuyResponse))]
	[Message(InnerOpcode.M2A_MysteryBuyRequest)]
	[ProtoContract]
	public partial class M2A_MysteryBuyRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public MysteryItemInfo MysteryItemInfo { get; set; }

	}

	[Message(InnerOpcode.A2M_MysteryBuyResponse)]
	[ProtoContract]
	public partial class A2M_MysteryBuyResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(G2G_UnitListResponse))]
	[Message(InnerOpcode.G2G_UnitListRequest)]
	[ProtoContract]
	public partial class G2G_UnitListRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(InnerOpcode.G2G_UnitListResponse)]
	[ProtoContract]
	public partial class G2G_UnitListResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int OnLinePlayer { get; set; }

		[ProtoMember(2)]
		public int OnLineRobot { get; set; }

		[ProtoMember(3)]
		public int YaCeRobot { get; set; }

		[ProtoMember(4)]
		public List<long> UnitList = new List<long>();

	}

//创建组队副本
	[ResponseType(nameof(T2M_TeamDungeonCreateResponse))]
	[Message(InnerOpcode.M2T_TeamDungeonCreateRequest)]
	[ProtoContract]
	public partial class M2T_TeamDungeonCreateRequest: Object, IActorRequest
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

	[Message(InnerOpcode.T2M_TeamDungeonCreateResponse)]
	[ProtoContract]
	public partial class T2M_TeamDungeonCreateResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public TeamInfo TeamInfo { get; set; }

	}

//开启组队副本
	[ResponseType(nameof(T2M_TeamDungeonOpenResponse))]
	[Message(InnerOpcode.M2T_TeamDungeonOpenRequest)]
	[ProtoContract]
	public partial class M2T_TeamDungeonOpenRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

		[ProtoMember(3)]
		public int FubenType { get; set; }

	}

	[Message(InnerOpcode.T2M_TeamDungeonOpenResponse)]
	[ProtoContract]
	public partial class T2M_TeamDungeonOpenResponse: Object, IActorResponse
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

//请求准备
	[ResponseType(nameof(T2M_TeamDungeonPrepareResponse))]
	[Message(InnerOpcode.M2T_TeamDungeonPrepareRequest)]
	[ProtoContract]
	public partial class M2T_TeamDungeonPrepareRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long TeamId { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

		[ProtoMember(3)]
		public int Prepare { get; set; }

		[ProtoMember(4)]
		public int ErrorCode { get; set; }

	}

	[Message(InnerOpcode.T2M_TeamDungeonPrepareResponse)]
	[ProtoContract]
	public partial class T2M_TeamDungeonPrepareResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public TeamInfo TeamInfo { get; set; }

	}

//进入组队副本
	[ResponseType(nameof(T2M_TeamDungeonEnterResponse))]
	[Message(InnerOpcode.M2T_TeamDungeonEnterRequest)]
	[ProtoContract]
	public partial class M2T_TeamDungeonEnterRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long TeamId { get; set; }

		[ProtoMember(2)]
		public long UserID { get; set; }

	}

	[Message(InnerOpcode.T2M_TeamDungeonEnterResponse)]
	[ProtoContract]
	public partial class T2M_TeamDungeonEnterResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int FubenId { get; set; }

		[ProtoMember(2)]
		public long FubenInstanceId { get; set; }

		[ProtoMember(3)]
		public int FubenType { get; set; }

	}

	[ResponseType(nameof(G2T_GateUnitInfoResponse))]
	[Message(InnerOpcode.T2G_GateUnitInfoRequest)]
	[ProtoContract]
	public partial class T2G_GateUnitInfoRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

	}

	[Message(InnerOpcode.G2T_GateUnitInfoResponse)]
	[ProtoContract]
	public partial class G2T_GateUnitInfoResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long SessionInstanceId { get; set; }

		[ProtoMember(2)]
		public int PlayerState { get; set; }

		[ProtoMember(3)]
		public long UnitId { get; set; }

	}

	[ResponseType(nameof(T2C_GetTeamInfoResponse))]
	[Message(InnerOpcode.C2T_GetTeamInfoRequest)]
	[ProtoContract]
	public partial class C2T_GetTeamInfoRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

	}

	[Message(InnerOpcode.T2C_GetTeamInfoResponse)]
	[ProtoContract]
	public partial class T2C_GetTeamInfoResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public TeamInfo TeamInfo { get; set; }

	}

	[ResponseType(nameof(U2M_UnionCreateResponse))]
	[Message(InnerOpcode.M2U_UnionCreateRequest)]
	[ProtoContract]
	public partial class M2U_UnionCreateRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string UnionName { get; set; }

		[ProtoMember(2)]
		public string UnionPurpose { get; set; }

		[ProtoMember(4)]
		public long UserID { get; set; }

	}

	[Message(InnerOpcode.U2M_UnionCreateResponse)]
	[ProtoContract]
	public partial class U2M_UnionCreateResponse: Object, IActorResponse
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

//离开公会
	[ResponseType(nameof(U2M_UnionLeaveResponse))]
	[Message(InnerOpcode.M2U_UnionLeaveRequest)]
	[ProtoContract]
	public partial class M2U_UnionLeaveRequest: Object, IActorRequest
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

	[Message(InnerOpcode.U2M_UnionLeaveResponse)]
	[ProtoContract]
	public partial class U2M_UnionLeaveResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//转让族长
	[ResponseType(nameof(U2M_UnionTransferResponse))]
	[Message(InnerOpcode.M2U_UnionTransferRequest)]
	[ProtoContract]
	public partial class M2U_UnionTransferRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long NewLeader { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

		[ProtoMember(3)]
		public long UnionId { get; set; }

	}

	[Message(InnerOpcode.U2M_UnionTransferResponse)]
	[ProtoContract]
	public partial class U2M_UnionTransferResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

///转让族长
	[Message(InnerOpcode.M2M_UnionTransferMessage)]
	[ProtoContract]
	public partial class M2M_UnionTransferMessage: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public int UnionLeader { get; set; }

	}

//家族操作  1增加经验  2获取等级
	[ResponseType(nameof(U2M_UnionOperationResponse))]
	[Message(InnerOpcode.M2U_UnionOperationRequest)]
	[ProtoContract]
	public partial class M2U_UnionOperationRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public int OperateType { get; set; }

		[ProtoMember(3)]
		public string Par { get; set; }

		[ProtoMember(4)]
		public long UnitId { get; set; }

	}

	[Message(InnerOpcode.U2M_UnionOperationResponse)]
	[ProtoContract]
	public partial class U2M_UnionOperationResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public string Par { get; set; }

	}

//公会踢人
	[ResponseType(nameof(M2U_UnionKickOutResponse))]
	[Message(InnerOpcode.U2M_UnionKickOutRequest)]
	[ProtoContract]
	public partial class U2M_UnionKickOutRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserId { get; set; }

	}

	[Message(InnerOpcode.M2U_UnionKickOutResponse)]
	[ProtoContract]
	public partial class M2U_UnionKickOutResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//加速完成
	[ResponseType(nameof(M2U_UnionKeJiQuickResponse))]
	[Message(InnerOpcode.U2M_UnionKeJiQuickRequest)]
	[ProtoContract]
	public partial class U2M_UnionKeJiQuickRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Cost { get; set; }

	}

	[Message(InnerOpcode.M2U_UnionKeJiQuickResponse)]
	[ProtoContract]
	public partial class M2U_UnionKeJiQuickResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(U2M_UnionKeJiLearnResponse))]
	[Message(InnerOpcode.M2U_UnionKeJiLearnRequest)]
	[ProtoContract]
	public partial class M2U_UnionKeJiLearnRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public int KeJiId { get; set; }

		[ProtoMember(3)]
		public int Position { get; set; }

	}

	[Message(InnerOpcode.U2M_UnionKeJiLearnResponse)]
	[ProtoContract]
	public partial class U2M_UnionKeJiLearnResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//入会通知
	[ResponseType(nameof(M2U_UnionApplyResponse))]
	[Message(InnerOpcode.U2M_UnionApplyRequest)]
	[ProtoContract]
	public partial class U2M_UnionApplyRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public string UnionName { get; set; }

	}

	[Message(InnerOpcode.M2U_UnionApplyResponse)]
	[ProtoContract]
	public partial class M2U_UnionApplyResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(InnerOpcode.M2U_UnionInviteReplyMessage)]
	[ProtoContract]
	public partial class M2U_UnionInviteReplyMessage: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public int ReplyCode { get; set; }

		[ProtoMember(3)]
		public long UnitID { get; set; }

	}

	[ResponseType(nameof(R2A_DeleteRoleData))]
	[Message(InnerOpcode.A2R_DeleteRoleData)]
	[ProtoContract]
	public partial class A2R_DeleteRoleData: Object, IActorRequest
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

	[Message(InnerOpcode.R2A_DeleteRoleData)]
	[ProtoContract]
	public partial class R2A_DeleteRoleData: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2A_DeleteRoleData))]
	[Message(InnerOpcode.A2P_DeleteRoleData)]
	[ProtoContract]
	public partial class A2P_DeleteRoleData: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public int DeleXuhaoID { get; set; }

		[ProtoMember(3)]
		public long DeleUserID { get; set; }

		[ProtoMember(4)]
		public long AccountId { get; set; }

		[ProtoMember(5)]
		public int DeleteType { get; set; }

	}

	[Message(InnerOpcode.P2A_DeleteRoleData)]
	[ProtoContract]
	public partial class P2A_DeleteRoleData: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2M_DBServerInfoResponse))]
	[Message(InnerOpcode.M2R_DBServerInfoRequest)]
	[ProtoContract]
	public partial class M2R_DBServerInfoRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(InnerOpcode.R2M_DBServerInfoResponse)]
	[ProtoContract]
	public partial class R2M_DBServerInfoResponse: Object, IActorResponse
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

	[ResponseType(nameof(F2M_FubenCenterListResponse))]
	[Message(InnerOpcode.M2F_FubenCenterListRequest)]
	[ProtoContract]
	public partial class M2F_FubenCenterListRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(InnerOpcode.F2M_FubenCenterListResponse)]
	[ProtoContract]
	public partial class F2M_FubenCenterListResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<long> FubenInstanceList = new List<long>();

	}

//副本分配中心服
	[ResponseType(nameof(F2M_FubenCenterOpenResponse))]
	[Message(InnerOpcode.M2F_FubenCenterOperateRequest)]
	[ProtoContract]
	public partial class M2F_FubenCenterOperateRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int OperateType { get; set; }

		[ProtoMember(2)]
		public long FubenInstanceId { get; set; }

		[ProtoMember(3)]
		public int SceneType { get; set; }

	}

	[Message(InnerOpcode.F2M_FubenCenterOpenResponse)]
	[ProtoContract]
	public partial class F2M_FubenCenterOpenResponse: Object, IActorResponse
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

//通知其他服务进程刷新肝帝
	[ResponseType(nameof(F2R_WorldLvUpdateResponse))]
	[Message(InnerOpcode.R2F_WorldLvUpdateRequest)]
	[ProtoContract]
	public partial class R2F_WorldLvUpdateRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public ServerInfo ServerInfo { get; set; }

	}

	[Message(InnerOpcode.F2R_WorldLvUpdateResponse)]
	[ProtoContract]
	public partial class F2R_WorldLvUpdateResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//通知其他服务进程刷新肝帝
	[ResponseType(nameof(M2F_ServerInfoUpdateResponse))]
	[Message(InnerOpcode.F2M_ServerInfoUpdateRequest)]
	[ProtoContract]
	public partial class F2M_ServerInfoUpdateRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public ServerInfo ServerInfo { get; set; }

		[ProtoMember(2)]
		public int operareType { get; set; }

		[ProtoMember(3)]
		public string operateValue { get; set; }

	}

	[Message(InnerOpcode.M2F_ServerInfoUpdateResponse)]
	[ProtoContract]
	public partial class M2F_ServerInfoUpdateResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(InnerOpcode.M2A_FirstWinInfoMessage)]
	[ProtoContract]
	public partial class M2A_FirstWinInfoMessage: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public FirstWinInfo FirstWinInfo { get; set; }

	}

	[ResponseType(nameof(Center2A_RechargeResponse))]
	[Message(InnerOpcode.A2Center_RechargeRequest)]
	[ProtoContract]
	public partial class A2Center_RechargeRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(2)]
		public RechargeInfo RechargeInfo { get; set; }

	}

	[Message(InnerOpcode.Center2A_RechargeResponse)]
	[ProtoContract]
	public partial class Center2A_RechargeResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Center2M_ShareSucessResponse))]
	[Message(InnerOpcode.M2Center_ShareSucessRequest)]
	[ProtoContract]
	public partial class M2Center_ShareSucessRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int ShareType { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public long AccountId { get; set; }

	}

	[Message(InnerOpcode.Center2M_ShareSucessResponse)]
	[ProtoContract]
	public partial class Center2M_ShareSucessResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Center2M_BuChangeResponse))]
	[Message(InnerOpcode.M2Center_BuChangeRequest)]
	[ProtoContract]
	public partial class M2Center_BuChangeRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long BuChangId { get; set; }

		[ProtoMember(2)]
		public long UserId { get; set; }

		[ProtoMember(3)]
		public long AccountId { get; set; }

	}

	[Message(InnerOpcode.Center2M_BuChangeResponse)]
	[ProtoContract]
	public partial class Center2M_BuChangeResponse: Object, IActorResponse
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

	[ResponseType(nameof(Center2M_SerialReardResponse))]
//序列号奖励
	[Message(InnerOpcode.M2Center_SerialReardRequest)]
	[ProtoContract]
	public partial class M2Center_SerialReardRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string SerialNumber { get; set; }

	}

	[Message(InnerOpcode.Center2M_SerialReardResponse)]
	[ProtoContract]
	public partial class Center2M_SerialReardResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Center2M_SerialQueryResponse))]
//序列号查询
	[Message(InnerOpcode.M2Center_SerialQueryRequest)]
	[ProtoContract]
	public partial class M2Center_SerialQueryRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public string SerialNumber { get; set; }

	}

	[Message(InnerOpcode.Center2M_SerialQueryResponse)]
	[ProtoContract]
	public partial class Center2M_SerialQueryResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int SerialIndex { get; set; }

		[ProtoMember(2)]
		public int IsRewarded { get; set; }

	}

	[ResponseType(nameof(E2M_GMEMailSendResponse))]
	[Message(InnerOpcode.M2E_GMEMailSendRequest)]
	[ProtoContract]
	public partial class M2E_GMEMailSendRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public string Itemlist { get; set; }

		[ProtoMember(3)]
		public string Title { get; set; }

		[ProtoMember(4)]
		public string UserName { get; set; }

		[ProtoMember(5)]
		public int MailType { get; set; }

	}

	[Message(InnerOpcode.E2M_GMEMailSendResponse)]
	[ProtoContract]
	public partial class E2M_GMEMailSendResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(L2A_LoginAccountResponse))]
	[Message(InnerOpcode.A2L_LoginAccountRequest)]
	[ProtoContract]
	public partial class A2L_LoginAccountRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

		[ProtoMember(5)]
		public bool Relink { get; set; }

	}

	[Message(InnerOpcode.L2A_LoginAccountResponse)]
	[ProtoContract]
	public partial class L2A_LoginAccountResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Chat2G_EnterChat))]
	[Message(InnerOpcode.G2Chat_EnterChat)]
	[ProtoContract]
	public partial class G2Chat_EnterChat: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Name { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public long GateSessionActorId { get; set; }

		[ProtoMember(4)]
		public long UnionId { get; set; }

		[ProtoMember(5)]
		public int Level { get; set; }

	}

	[Message(InnerOpcode.Chat2G_EnterChat)]
	[ProtoContract]
	public partial class Chat2G_EnterChat: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long ChatInfoUnitInstanceId { get; set; }

	}

	[ResponseType(nameof(Chat2M_UpdateLevel))]
	[Message(InnerOpcode.M2Chat_UpdateLevel)]
	[ProtoContract]
	public partial class M2Chat_UpdateLevel: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(5)]
		public int Level { get; set; }

	}

	[Message(InnerOpcode.Chat2M_UpdateLevel)]
	[ProtoContract]
	public partial class Chat2M_UpdateLevel: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(Chat2G_RequestExitChat))]
	[Message(InnerOpcode.G2Chat_RequestExitChat)]
	[ProtoContract]
	public partial class G2Chat_RequestExitChat: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerOpcode.Chat2G_RequestExitChat)]
	[ProtoContract]
	public partial class Chat2G_RequestExitChat: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//进入战场
	[ResponseType(nameof(B2M_BattleEnterResponse))]
	[Message(InnerOpcode.M2B_BattleEnterRequest)]
	[ProtoContract]
	public partial class M2B_BattleEnterRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

		[ProtoMember(2)]
		public int SceneId { get; set; }

	}

	[Message(InnerOpcode.B2M_BattleEnterResponse)]
	[ProtoContract]
	public partial class B2M_BattleEnterResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public long FubenInstanceId { get; set; }

		[ProtoMember(3)]
		public int Camp { get; set; }

	}

//通知机器人进程
	[Message(InnerOpcode.G2Robot_MessageRequest)]
	[ProtoContract]
	public partial class G2Robot_MessageRequest: Object, IActorMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int Zone { get; set; }

		[ProtoMember(2)]
		public int MessageType { get; set; }

		[ProtoMember(3)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(D2M_DeleteUnit))]
	[Message(InnerOpcode.M2D_DeleteUnit)]
	[ProtoContract]
	public partial class M2D_DeleteUnit: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(InnerOpcode.D2M_DeleteUnit)]
	[ProtoContract]
	public partial class D2M_DeleteUnit: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//野外副本Id
	[ResponseType(nameof(F2M_YeWaiSceneIdResponse))]
	[Message(InnerOpcode.M2F_YeWaiSceneIdRequest)]
	[ProtoContract]
	public partial class M2F_YeWaiSceneIdRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public int SceneId { get; set; }

		[ProtoMember(3)]
		public long UnitId { get; set; }

	}

	[Message(InnerOpcode.F2M_YeWaiSceneIdResponse)]
	[ProtoContract]
	public partial class F2M_YeWaiSceneIdResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public long FubenInstanceId { get; set; }

	}

	[Message(InnerOpcode.T2M_TeamUpdateRequest)]
	[ProtoContract]
	public partial class T2M_TeamUpdateRequest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public long TeamId { get; set; }

	}

//进入角斗场
	[ResponseType(nameof(Arena2M_ArenaEnterResponse))]
	[Message(InnerOpcode.M2Arena_ArenaEnterRequest)]
	[ProtoContract]
	public partial class M2Arena_ArenaEnterRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

		[ProtoMember(2)]
		public int SceneId { get; set; }

	}

	[Message(InnerOpcode.Arena2M_ArenaEnterResponse)]
	[ProtoContract]
	public partial class Arena2M_ArenaEnterResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public long FubenInstanceId { get; set; }

	}

//踢出掉线玩家
	[Message(InnerOpcode.G2M_KickPlayerRequest)]
	[ProtoContract]
	public partial class G2M_KickPlayerRequest: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public long SceneId { get; set; }

	}

	[ResponseType(nameof(M2A_KickOutPlayerResponse))]
	[Message(InnerOpcode.A2M_KickOutPlayerRequest)]
	[ProtoContract]
	public partial class A2M_KickOutPlayerRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

	}

	[Message(InnerOpcode.M2A_KickOutPlayerResponse)]
	[ProtoContract]
	public partial class M2A_KickOutPlayerResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//排行榜刷新
	[Message(InnerOpcode.R2M_RankUpdateMessage)]
	[ProtoContract]
	public partial class R2M_RankUpdateMessage: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int RankType { get; set; }

		[ProtoMember(2)]
		public int RankId { get; set; }

		[ProtoMember(3)]
		public int OccRankId { get; set; }

	}

	[ResponseType(nameof(Rank2G_EnterRank))]
	[Message(InnerOpcode.G2Rank_EnterRank)]
	[ProtoContract]
	public partial class G2Rank_EnterRank: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Name { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public int Occ { get; set; }

	}

	[Message(InnerOpcode.Rank2G_EnterRank)]
	[ProtoContract]
	public partial class Rank2G_EnterRank: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int RankId { get; set; }

		[ProtoMember(2)]
		public int PetRankId { get; set; }

		[ProtoMember(3)]
		public int SoloRankId { get; set; }

		[ProtoMember(4)]
		public int TrialRankId { get; set; }

		[ProtoMember(5)]
		public int OccRankId { get; set; }

	}

	[ResponseType(nameof(Union2G_EnterUnion))]
	[Message(InnerOpcode.G2Union_EnterUnion)]
	[ProtoContract]
	public partial class G2Union_EnterUnion: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public string Name { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

	}

	[Message(InnerOpcode.Union2G_EnterUnion)]
	[ProtoContract]
	public partial class Union2G_EnterUnion: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public long WinUnionId { get; set; }

		[ProtoMember(3)]
		public int DonationRankId { get; set; }

		[ProtoMember(4)]
		public long LeaderId { get; set; }

	}

//进入家园
	[ResponseType(nameof(J2M_JiaYuanEnterResponse))]
	[Message(InnerOpcode.M2J_JiaYuanEnterRequest)]
	[ProtoContract]
	public partial class M2J_JiaYuanEnterRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long MasterId { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public int SceneId { get; set; }

	}

	[Message(InnerOpcode.J2M_JiaYuanEnterResponse)]
	[ProtoContract]
	public partial class J2M_JiaYuanEnterResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int FubenId { get; set; }

		[ProtoMember(2)]
		public long FubenInstanceId { get; set; }

	}

	[ResponseType(nameof(M2Popularize_RewardResponse))]
	[Message(InnerOpcode.Popularize2M_RewardRequest)]
	[ProtoContract]
	public partial class Popularize2M_RewardRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(5)]
		public List<RewardItem> ReardList = new List<RewardItem>();

	}

	[Message(InnerOpcode.M2Popularize_RewardResponse)]
	[ProtoContract]
	public partial class M2Popularize_RewardResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[Message(InnerOpcode.M2M_JiaYuanOperateMessage)]
	[ProtoContract]
	public partial class M2M_JiaYuanOperateMessage: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public JiaYuanOperate JiaYuanOperate { get; set; }

	}

	[ResponseType(nameof(P2M_PaiMaiAuctionPriceResponse))]
	[Message(InnerOpcode.M2P_PaiMaiAuctionPriceRequest)]
	[ProtoContract]
	public partial class M2P_PaiMaiAuctionPriceRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Price { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

		[ProtoMember(3)]
		public int Occ { get; set; }

		[ProtoMember(5)]
		public string AuctionPlayer { get; set; }

	}

	[Message(InnerOpcode.P2M_PaiMaiAuctionPriceResponse)]
	[ProtoContract]
	public partial class P2M_PaiMaiAuctionPriceResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public string Message { get; set; }

		[ProtoMember(92)]
		public int Error { get; set; }

	}

	[ResponseType(nameof(P2M_PaiMaiAuctionJoinResponse))]
//参入竞拍
	[Message(InnerOpcode.M2P_PaiMaiAuctionJoinRequest)]
	[ProtoContract]
	public partial class M2P_PaiMaiAuctionJoinRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Gold { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

	}

	[Message(InnerOpcode.P2M_PaiMaiAuctionJoinResponse)]
	[ProtoContract]
	public partial class P2M_PaiMaiAuctionJoinResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long CostGold { get; set; }

	}

	[ResponseType(nameof(M2P_PaiMaiAuctionOverResponse))]
	[Message(InnerOpcode.P2M_PaiMaiAuctionOverRequest)]
	[ProtoContract]
	public partial class P2M_PaiMaiAuctionOverRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long Price { get; set; }

		[ProtoMember(2)]
		public long UnitID { get; set; }

		[ProtoMember(3)]
		public int ItemID { get; set; }

		[ProtoMember(4)]
		public int ItemNumber { get; set; }

	}

	[Message(InnerOpcode.M2P_PaiMaiAuctionOverResponse)]
	[ProtoContract]
	public partial class M2P_PaiMaiAuctionOverResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//进入家族地图
	[ResponseType(nameof(U2M_UnionEnterResponse))]
	[Message(InnerOpcode.M2U_UnionEnterRequest)]
	[ProtoContract]
	public partial class M2U_UnionEnterRequest: Object, IActorRequest
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
		public int SceneId { get; set; }

		[ProtoMember(4)]
		public int OperateType { get; set; }

	}

	[Message(InnerOpcode.U2M_UnionEnterResponse)]
	[ProtoContract]
	public partial class U2M_UnionEnterResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int FubenId { get; set; }

		[ProtoMember(2)]
		public long FubenInstanceId { get; set; }

	}

//捐献
	[ResponseType(nameof(U2M_DonationResponse))]
	[Message(InnerOpcode.M2U_DonationRequest)]
	[ProtoContract]
	public partial class M2U_DonationRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public RankingInfo RankingInfo { get; set; }

	}

	[Message(InnerOpcode.U2M_DonationResponse)]
	[ProtoContract]
	public partial class U2M_DonationResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int RankId { get; set; }

	}

	[ResponseType(nameof(S2M_SoloMatchResponse))]
	[Message(InnerOpcode.M2S_SoloMatchRequest)]
	[ProtoContract]
	public partial class M2S_SoloMatchRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public SoloPlayerInfo SoloPlayerInfo { get; set; }

	}

	[Message(InnerOpcode.S2M_SoloMatchResponse)]
	[ProtoContract]
	public partial class S2M_SoloMatchResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(S2M_SoloEnterResponse))]
	[Message(InnerOpcode.M2S_SoloEnterRequest)]
	[ProtoContract]
	public partial class M2S_SoloEnterRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long FubenId { get; set; }

	}

	[Message(InnerOpcode.S2M_SoloEnterResponse)]
	[ProtoContract]
	public partial class S2M_SoloEnterResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public long FubenInstanceId { get; set; }

	}

	[ResponseType(nameof(R2S_SoloResultResponse))]
	[Message(InnerOpcode.S2R_SoloResultRequest)]
	[ProtoContract]
	public partial class S2R_SoloResultRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int CampId { get; set; }

		[ProtoMember(2)]
		public RankingInfo RankingInfo { get; set; }

	}

	[Message(InnerOpcode.R2S_SoloResultResponse)]
	[ProtoContract]
	public partial class R2S_SoloResultResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int RankId { get; set; }

		[ProtoMember(2)]
		public int PetRankId { get; set; }

	}

//进入副本
	[ResponseType(nameof(LocalDungeon2M_EnterResponse))]
	[Message(InnerOpcode.M2LocalDungeon_EnterRequest)]
	[ProtoContract]
	public partial class M2LocalDungeon_EnterRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UserID { get; set; }

		[ProtoMember(2)]
		public int SceneId { get; set; }

		[ProtoMember(3)]
		public int TransferId { get; set; }

		[ProtoMember(4)]
		public int Difficulty { get; set; }

		[ProtoMember(5)]
		public int SceneType { get; set; }

	}

	[Message(InnerOpcode.LocalDungeon2M_EnterResponse)]
	[ProtoContract]
	public partial class LocalDungeon2M_EnterResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public long FubenInstanceId { get; set; }

		[ProtoMember(3)]
		public long FubenId { get; set; }

	}

//退出副本
	[ResponseType(nameof(LocalDungeon2M_ExitResponse))]
	[Message(InnerOpcode.M2LocalDungeon_ExitRequest)]
	[ProtoContract]
	public partial class M2LocalDungeon_ExitRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int SceneType { get; set; }

		[ProtoMember(2)]
		public long FubenInstanceId { get; set; }

		[ProtoMember(3)]
		public long FubenId { get; set; }

		[ProtoMember(4)]
		public List<long> Camp1Player = new List<long>();

		[ProtoMember(5)]
		public List<long> Camp2Player = new List<long>();

	}

	[Message(InnerOpcode.LocalDungeon2M_ExitResponse)]
	[ProtoContract]
	public partial class LocalDungeon2M_ExitResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[Message(InnerOpcode.ServerMailItem)]
	[ProtoContract]
	public partial class ServerMailItem: Object
	{
		[ProtoMember(1)]
		public int MailType { get; set; }

		[ProtoMember(2)]
		public string ParasmNew { get; set; }

		[ProtoMember(3)]
		public List<BagInfo> ItemList = new List<BagInfo>();

		[ProtoMember(4)]
		public long EndTime { get; set; }

		[ProtoMember(5)]
		public int ServerMailIId { get; set; }

		[ProtoMember(6)]
		public int Parasm { get; set; }

	}

	[ResponseType(nameof(Chat2Mail_GetUnitList))]
	[Message(InnerOpcode.Mail2Chat_GetUnitList)]
	[ProtoContract]
	public partial class Mail2Chat_GetUnitList: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

	}

	[Message(InnerOpcode.Chat2Mail_GetUnitList)]
	[ProtoContract]
	public partial class Chat2Mail_GetUnitList: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<long> OnlineUnitIdList = new List<long>();

	}

	[Message(InnerOpcode.Mail2M_SendServerMailItem)]
	[ProtoContract]
	public partial class Mail2M_SendServerMailItem: Object, IActorLocationMessage
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public ServerMailItem ServerMailItem { get; set; }

	}

	[ResponseType(nameof(Mail2G_EnterMail))]
	[Message(InnerOpcode.G2Mail_EnterMail)]
	[ProtoContract]
	public partial class G2Mail_EnterMail: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(3)]
		public int ServerMailIdCur { get; set; }

	}

	[Message(InnerOpcode.Mail2G_EnterMail)]
	[ProtoContract]
	public partial class Mail2G_EnterMail: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(3)]
		public int ServerMailIdMax { get; set; }

	}

	[ResponseType(nameof(Chat2M_UpdateUnion))]
	[Message(InnerOpcode.M2Chat_UpdateUnion)]
	[ProtoContract]
	public partial class M2Chat_UpdateUnion: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(2)]
		public long UnitId { get; set; }

		[ProtoMember(4)]
		public long UnionId { get; set; }

	}

	[Message(InnerOpcode.Chat2M_UpdateUnion)]
	[ProtoContract]
	public partial class Chat2M_UpdateUnion: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(U2M_UnionMysteryBuyResponse))]
	[Message(InnerOpcode.M2U_UnionMysteryBuyRequest)]
	[ProtoContract]
	public partial class M2U_UnionMysteryBuyRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnionId { get; set; }

		[ProtoMember(2)]
		public int MysteryId { get; set; }

		[ProtoMember(3)]
		public int BuyNumber { get; set; }

	}

	[Message(InnerOpcode.U2M_UnionMysteryBuyResponse)]
	[ProtoContract]
	public partial class U2M_UnionMysteryBuyResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

//进入喜从天降
	[ResponseType(nameof(H2M_HapplyEnterResponse))]
	[Message(InnerOpcode.M2H_HapplyEnterRequest)]
	[ProtoContract]
	public partial class M2H_HapplyEnterRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int SceneId { get; set; }

	}

	[Message(InnerOpcode.H2M_HapplyEnterResponse)]
	[ProtoContract]
	public partial class H2M_HapplyEnterResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public long FubenInstanceId { get; set; }

		[ProtoMember(3)]
		public int Position { get; set; }

	}

	[ResponseType(nameof(A2M_TurtleRecordResponse))]
	[Message(InnerOpcode.M2A_TurtleRecordRequest)]
	[ProtoContract]
	public partial class M2A_TurtleRecordRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long AccountId { get; set; }

	}

	[Message(InnerOpcode.A2M_TurtleRecordResponse)]
	[ProtoContract]
	public partial class A2M_TurtleRecordResponse: Object, IActorResponse
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

	[ResponseType(nameof(A2M_TurtleSupportResponse))]
	[Message(InnerOpcode.M2A_TurtleSupportRequest)]
	[ProtoContract]
	public partial class M2A_TurtleSupportRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int SupportId { get; set; }

		[ProtoMember(2)]
		public long AccountId { get; set; }

		[ProtoMember(3)]
		public long UnitId { get; set; }

	}

	[Message(InnerOpcode.A2M_TurtleSupportResponse)]
	[ProtoContract]
	public partial class A2M_TurtleSupportResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2M_TurtleReportResponse))]
	[Message(InnerOpcode.M2A_TurtleReportRequest)]
	[ProtoContract]
	public partial class M2A_TurtleReportRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int TurtleId { get; set; }

	}

	[Message(InnerOpcode.A2M_TurtleReportResponse)]
	[ProtoContract]
	public partial class A2M_TurtleReportResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2M_RankUnionRaceResponse))]
	[Message(InnerOpcode.M2R_RankUnionRaceRequest)]
	[ProtoContract]
	public partial class M2R_RankUnionRaceRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int CampId { get; set; }

		[ProtoMember(2)]
		public RankShouLieInfo RankingInfo { get; set; }

	}

	[Message(InnerOpcode.R2M_RankUnionRaceResponse)]
	[ProtoContract]
	public partial class R2M_RankUnionRaceResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(R2M_RankRunRaceResponse))]
	[Message(InnerOpcode.M2R_RankRunRaceRequest)]
	[ProtoContract]
	public partial class M2R_RankRunRaceRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public RankingInfo RankingInfo { get; set; }

	}

	[Message(InnerOpcode.R2M_RankRunRaceResponse)]
	[ProtoContract]
	public partial class R2M_RankRunRaceResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int RankId { get; set; }

		[ProtoMember(2)]
		public List<RankingInfo> RankList = new List<RankingInfo>();

	}

	[ResponseType(nameof(R2M_RankDemonResponse))]
	[Message(InnerOpcode.M2R_RankDemonRequest)]
	[ProtoContract]
	public partial class M2R_RankDemonRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public RankingInfo RankingInfo { get; set; }

	}

	[Message(InnerOpcode.R2M_RankDemonResponse)]
	[ProtoContract]
	public partial class R2M_RankDemonResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int RankId { get; set; }

		[ProtoMember(2)]
		public List<RankingInfo> RankList = new List<RankingInfo>();

	}

//试炼副本伤害
	[ResponseType(nameof(R2M_RankTrialResponse))]
	[Message(InnerOpcode.M2R_RankTrialRequest)]
	[ProtoContract]
	public partial class M2R_RankTrialRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int CampId { get; set; }

		[ProtoMember(2)]
		public KeyValuePairLong RankingInfo { get; set; }

	}

	[Message(InnerOpcode.R2M_RankTrialResponse)]
	[ProtoContract]
	public partial class R2M_RankTrialResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int RankId { get; set; }

	}

	[ResponseType(nameof(A2M_PetMingPlayerInfoResponse))]
	[Message(InnerOpcode.M2A_PetMingPlayerInfoRequest)]
	[ProtoContract]
	public partial class M2A_PetMingPlayerInfoRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MingType { get; set; }

		[ProtoMember(2)]
		public int Postion { get; set; }

	}

	[Message(InnerOpcode.A2M_PetMingPlayerInfoResponse)]
	[ProtoContract]
	public partial class A2M_PetMingPlayerInfoResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public PetMingPlayerInfo PetMingPlayerInfo { get; set; }

	}

	[ResponseType(nameof(A2M_PetMingBattleWinResponse))]
	[Message(InnerOpcode.M2A_PetMingBattleWinRequest)]
	[ProtoContract]
	public partial class M2A_PetMingBattleWinRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public int MingType { get; set; }

		[ProtoMember(2)]
		public int Postion { get; set; }

		[ProtoMember(3)]
		public long UnitID { get; set; }

		[ProtoMember(4)]
		public int TeamId { get; set; }

		[ProtoMember(5)]
		public string WinPlayer { get; set; }

	}

	[Message(InnerOpcode.A2M_PetMingBattleWinResponse)]
	[ProtoContract]
	public partial class A2M_PetMingBattleWinResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2A_PetMingChanChuResponse))]
	[Message(InnerOpcode.A2M_PetMingChanChuRequest)]
	[ProtoContract]
	public partial class A2M_PetMingChanChuRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long UnitID { get; set; }

		[ProtoMember(4)]
		public long ChanChu { get; set; }

	}

	[Message(InnerOpcode.M2A_PetMingChanChuResponse)]
	[ProtoContract]
	public partial class M2A_PetMingChanChuResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2A_PetMingLoginResponse))]
	[Message(InnerOpcode.A2M_PetMingLoginRequest)]
	[ProtoContract]
	public partial class A2M_PetMingLoginRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long UnitID { get; set; }

		[ProtoMember(1)]
		public List<PetMingPlayerInfo> PetMineList = new List<PetMingPlayerInfo>();

		[ProtoMember(2)]
		public List<KeyValuePairInt> PetMingExtend = new List<KeyValuePairInt>();

	}

	[Message(InnerOpcode.M2A_PetMingLoginResponse)]
	[ProtoContract]
	public partial class M2A_PetMingLoginResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(M2A_PetMingRecordResponse))]
	[Message(InnerOpcode.A2M_PetMingRecordRequest)]
	[ProtoContract]
	public partial class A2M_PetMingRecordRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(3)]
		public long UnitID { get; set; }

		[ProtoMember(1)]
		public PetMingRecord PetMingRecord { get; set; }

	}

	[Message(InnerOpcode.M2A_PetMingRecordResponse)]
	[ProtoContract]
	public partial class M2A_PetMingRecordResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2M_StallSellResponse))]
	[Message(InnerOpcode.M2P_StallSellRequest)]
	[ProtoContract]
	public partial class M2P_StallSellRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

	}

	[Message(InnerOpcode.P2M_StallSellResponse)]
	[ProtoContract]
	public partial class P2M_StallSellResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(P2M_StallBuyResponse))]
	[Message(InnerOpcode.M2P_StallBuyRequest)]
	[ProtoContract]
	public partial class M2P_StallBuyRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

	}

	[Message(InnerOpcode.P2M_StallBuyResponse)]
	[ProtoContract]
	public partial class P2M_StallBuyResponse: Object, IActorResponse
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

	[ResponseType(nameof(P2M_StallXiaJiaResponse))]
	[Message(InnerOpcode.M2P_StallXiaJiaRequest)]
	[ProtoContract]
	public partial class M2P_StallXiaJiaRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public long PaiMaiItemInfoId { get; set; }

		[ProtoMember(3)]
		public long UnitID { get; set; }

	}

	[Message(InnerOpcode.P2M_StallXiaJiaResponse)]
	[ProtoContract]
	public partial class P2M_StallXiaJiaResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(2)]
		public PaiMaiItemInfo PaiMaiItemInfo { get; set; }

	}

//赛季副本
	[ResponseType(nameof(R2M_RankSeasonTowerResponse))]
	[Message(InnerOpcode.M2R_RankSeasonTowerRequest)]
	[ProtoContract]
	public partial class M2R_RankSeasonTowerRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(2)]
		public KeyValuePairLong RankingInfo { get; set; }

	}

	[Message(InnerOpcode.R2M_RankSeasonTowerResponse)]
	[ProtoContract]
	public partial class R2M_RankSeasonTowerResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int RankId { get; set; }

	}

	[ResponseType(nameof(A2M_ActivityGuessResponse))]
	[Message(InnerOpcode.M2A_ActivityGuessRequest)]
	[ProtoContract]
	public partial class M2A_ActivityGuessRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

		[ProtoMember(2)]
		public int GuessId { get; set; }

	}

	[Message(InnerOpcode.A2M_ActivityGuessResponse)]
	[ProtoContract]
	public partial class A2M_ActivityGuessResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

	[ResponseType(nameof(A2M_ActivitySelfInfo))]
	[Message(InnerOpcode.M2A_ActivitySelfInfo)]
	[ProtoContract]
	public partial class M2A_ActivitySelfInfo: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitId { get; set; }

	}

	[Message(InnerOpcode.A2M_ActivitySelfInfo)]
	[ProtoContract]
	public partial class A2M_ActivitySelfInfo: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<int> GuessIds = new List<int>();

		[ProtoMember(2)]
		public List<int> LastGuessReward = new List<int>();

		[ProtoMember(3)]
		public int BaoShiDu { get; set; }

		[ProtoMember(4)]
		public List<int> OpenGuessIds = new List<int>();

	}

//喂食物
	[ResponseType(nameof(A2M_ActivityFeedResponse))]
	[Message(InnerOpcode.M2A_ActivityFeedRequest)]
	[ProtoContract]
	public partial class M2A_ActivityFeedRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long UnitID { get; set; }

	}

	[Message(InnerOpcode.A2M_ActivityFeedResponse)]
	[ProtoContract]
	public partial class A2M_ActivityFeedResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public int BaoShiDu { get; set; }

	}

	[ResponseType(nameof(M2M_AllPlayerListResponse))]
	[Message(InnerOpcode.M2M_AllPlayerListRequest)]
	[ProtoContract]
	public partial class M2M_AllPlayerListRequest: Object, IActorRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

	}

	[Message(InnerOpcode.M2M_AllPlayerListResponse)]
	[ProtoContract]
	public partial class M2M_AllPlayerListResponse: Object, IActorResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

		[ProtoMember(1)]
		public List<long> AllPlayers = new List<long>();

	}

	[ResponseType(nameof(M2M_PaiMaiBuyInfoResponse))]
	[Message(InnerOpcode.M2M_PaiMaiBuyInfoRequest)]
	[ProtoContract]
	public partial class M2M_PaiMaiBuyInfoRequest: Object, IActorLocationRequest
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(93)]
		public long ActorId { get; set; }

		[ProtoMember(1)]
		public long PlayerId { get; set; }

		[ProtoMember(2)]
		public long CostGold { get; set; }

		[ProtoMember(3)]
		public long BagInfoID { get; set; }

	}

	[Message(InnerOpcode.M2M_PaiMaiBuyInfoResponse)]
	[ProtoContract]
	public partial class M2M_PaiMaiBuyInfoResponse: Object, IActorLocationResponse
	{
		[ProtoMember(90)]
		public int RpcId { get; set; }

		[ProtoMember(91)]
		public int Error { get; set; }

		[ProtoMember(92)]
		public string Message { get; set; }

	}

}
