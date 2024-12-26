using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ET
{
    public enum AccountType
    {
        General = 0,

        NoPaiMai = 1, //禁止拍卖

        BlackList = 2, //黑名单

        Delete = 3, //删号
    }


    [BsonIgnoreExtraElements]
    public class DBCenterAccountInfo : Entity, IAwake
    {
        //用户名
        public string Account { get; set; }

        //密码
        public string Password { get; set; }

        public PlayerInfo PlayerInfo { get; set; }

        public int AccountType; //账号类型

        public long CreateTime; //创建时间
        
        public long BanTime;    //封号时间

        public string BanAccount;  //关联封禁帐号

        public string DeviceID;

        public List<KeyValuePairLong> CreateRoleList = new List<KeyValuePairLong>();


        public int GetTotalRecharge()
        {
            int total = 0;
            for (int i = 0; i < PlayerInfo.RechargeInfos.Count; i++)
            {
                total += PlayerInfo.RechargeInfos[i].Amount;
            }

            return total;
        }

    }
}
