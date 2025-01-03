using System.Collections.Generic;

namespace ET
{
    public class AccountInfoComponent: Entity, IAwake
    {
        public long MyId;

		public List<ServerItem> AllServerList = new List<ServerItem>();         //服务器列表存内容

		//当前角色列表数据
		public List<CreateRoleInfo> CreateRoleList = new List<CreateRoleInfo>();

		public PlayerInfo PlayerInfo;

		public int Age_Type = -1;	

		public long AccountId = 0;

		public string Token;

		public string RealmKey;
		public string RealmAddress;

		public string TaprepRequest;

		public int TodayCreateRole;

        //当前登录角色
        public int ServerId;
		public string ServerIp;
		public string Account;
		public string Password;
		public string LoginType;
		public long CurrentRoleId;
		public string ServerName;
		public long LastTime = 0;

		public string TianQiValue = "1";

		public string NoticeVersion = string.Empty;
		public string NoticeText = string.Empty;

		public int SerialErrorTime = 0;

		public int IsPopUp = 0;
		public string PopUpInfo = string.Empty;

		public int Simulator = 0;
		public int Root = 0;
		public string DeviceID = string.Empty;
        public string OAID = string.Empty;


        public int GetRecharge()
		{
			int recharge = 0;
			for (int i = 0; i < PlayerInfo.RechargeInfos.Count; i++ )
			{
				recharge += PlayerInfo.RechargeInfos[i].Amount;
            }
			return recharge;
        }
    }
}