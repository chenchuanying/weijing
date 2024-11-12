using System;
using System.Collections.Generic;

namespace ET
{
    public static class MailHelp
    {

        public static async ETTask SendPaiMaiEmail(int zone, PaiMaiItemInfo paiMaiItemInfo,int costNum, long unitid)
        {
            MailInfo mailInfo = new MailInfo();
            ItemConfig itemCof = ItemConfigCategory.Instance.Get(paiMaiItemInfo.BagInfo.ItemID);
            mailInfo.Status = 0;
            mailInfo.Context = "你拍卖行出售的道具:" + itemCof.ItemName + ",已经被其他玩家购买" + costNum + "个。";
            mailInfo.Title = "拍卖行邮件";
            mailInfo.MailId = IdGenerater.Instance.GenerateId();
            BagInfo reward = new BagInfo();
            reward.ItemID = 1;
            int sellPrice = (int)(paiMaiItemInfo.Price * 0.95f) * costNum;     //5%手续费
            reward.ItemNum = sellPrice;
            reward.GetWay = $"{ItemGetWay.PaiMaiSell}_{TimeHelper.ServerNow()}";
            mailInfo.ItemList.Add(reward);
            mailInfo.ItemSell = paiMaiItemInfo.BagInfo;
            mailInfo.BuyPlayerId = unitid;

            //发送到邮件服
            long mailServerId = StartSceneConfigCategory.Instance.GetBySceneName(zone, "EMail").InstanceId;      //获取邮件消息ID
            E2M_EMailSendResponse g_EMailSendResponse = (E2M_EMailSendResponse)await ActorMessageSenderComponent.Instance.Call
                (mailServerId, new M2E_EMailSendRequest() { Id = paiMaiItemInfo.UserId, MailInfo = mailInfo });
        }

        public static void  SendServerMail(int zone, long userID, ServerMailItem serverMailItem)
        {
            Mail2M_SendServerMailItem mail2M_SendServer = new Mail2M_SendServerMailItem();
            mail2M_SendServer.ServerMailItem = serverMailItem;
            MessageHelper.SendToLocationActor( userID, mail2M_SendServer);
        }

        public static bool CheckSendMail(int MailType, string Title, NumericComponent numericComponent, UserInfoComponent userInfoComponent, BagComponent bagComponent)
        {
            if (numericComponent == null || userInfoComponent == null || bagComponent == null)
            {
                return false;
            }

            switch (MailType)
            {
                case 2: // 充值>=6元 10011003
                    if (numericComponent.GetAsLong(NumericType.RechargeNumber) < int.Parse(Title))
                    {
                        return false;
                    }
                    break;
                case 3: //20级以上 补
                    if (userInfoComponent.UserInfo.Lv < int.Parse(Title))
                    {
                        return false;
                    }
                    break;
                case 4: //开启第二个仓库并且格子没有开完的
                    if (numericComponent.GetAsInt(NumericType.CangKuNumber) < 2)
                    {
                        return false;
                    }
                    if (bagComponent.WarehouseAddedCell.Count < 10)
                    {
                        return false;
                    }
                    if (bagComponent.WarehouseAddedCell[6] >= 10)
                    {
                        return false;
                    }
                    break;
                case 5:
                    // 充值>=6<30元 10011003
                    //充值额度某个区间段
                    string[] needrecharge = Title.Split('_');
                    int min_value = int.Parse(needrecharge[0]);
                    int max_value = int.Parse(needrecharge[1]);
                    if (numericComponent.GetAsLong(NumericType.RechargeNumber) < min_value
                        || numericComponent.GetAsLong(NumericType.RechargeNumber) >= max_value)
                    {
                        return false;
                    }
                    break;
                case 6:

                    break;
                default:
                    break;
            }
            //Log.Console($"CheckSendMail.true : {MailType} {Title}");
            return true;
        }

        public static async ETTask ServerMailItem(int zone, long userID, ServerMailItem serverMailItem)
        {
            //判断条件
            long dbCacheId = DBHelper.GetDbCacheId(zone);

            UserInfoComponent userInfoComponent = null;
            NumericComponent numericComponent = null;
            BagComponent bagComponent = null;
            D2G_GetComponent d2GGetUnit1 = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = userID, Component = DBHelper.UserInfoComponent });
            if (d2GGetUnit1.Component == null)
            {
                return;
            }
            userInfoComponent = d2GGetUnit1.Component as UserInfoComponent;
            if (userInfoComponent.UserInfo.RobotId > 0)
            {
                return;
            }

            D2G_GetComponent d2GGetUnit2 = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = userID, Component = DBHelper.NumericComponent });
            if (d2GGetUnit2.Component == null)
            {
                return;
            }
            numericComponent = d2GGetUnit2.Component as NumericComponent;

            D2G_GetComponent d2GGetUnit3 = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = userID, Component = DBHelper.BagComponent });
            if (d2GGetUnit3.Component == null)
            {
                return;
            }
            bagComponent = d2GGetUnit3.Component as BagComponent;

            bool cansendMail = MailHelp.CheckSendMail(serverMailItem.MailType, serverMailItem.ParasmNew, numericComponent, userInfoComponent, bagComponent);
            if (cansendMail == false)
            {
                return;
            }

            MailInfo mailInfo = new MailInfo();
            mailInfo.Status = 0;
            mailInfo.Title = "奖励";
            mailInfo.Context = "全服补偿邮件";
            mailInfo.ItemList = serverMailItem.ItemList;
            mailInfo.MailId = IdGenerater.Instance.GenerateId();
            await SendUserMail(zone, userID, mailInfo);
        }

        //指定玩家发送邮件
        public static async ETTask<int> SendUserMail(int zone,long userID, MailInfo mailInfo )
        {
           
            {
                long dbCacheId = DBHelper.GetDbCacheId(zone);
                D2G_GetComponent d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = userID, Component = DBHelper.DBMailInfo });
                DBMailInfo dBMainInfo = d2GGetUnit.Component as DBMailInfo;
                if (dBMainInfo == null)
                {
                    //有可能玩家自己删除角色了。。还收到邮件。。列如：道具被拍卖。。。。=====
                    dBMainInfo = (DBMailInfo)await DBHelper.AddDataComponent<DBMailInfo>(zone, userID, DBHelper.DBMailInfo);
                    Console.WriteLine($"AddDataComponent.DBMailInfo  {userID}");
                }
                if (dBMainInfo == null)
                {
                    return ErrorCode.ERR_NotFindAccount;
                }

                int totalGold = 0;
                List<MailInfo> mailinfolist = dBMainInfo.MailInfoList;
                for (int i = mailinfolist.Count - 1; i >= 0; i--)
                {

                    if (mailinfolist[i].ItemList.Count != 1)
                    {
                        continue;
                    }

                    if (mailinfolist[i].ItemList[0].ItemID == 1)
                    {
                        totalGold += mailinfolist[i].ItemList[0].ItemNum;
                    }

                    if (mailinfolist[i].ItemList[0].ItemID == 10000151)  //之前有一次全服误发精灵龟 /羽毛
                    {
                        mailinfolist.RemoveAt(i);
                        continue;
                    }

                    if (mailinfolist[i].ItemList.Count >= 1 && !ItemConfigCategory.Instance.Contain(mailinfolist[i].ItemList[0].ItemID))  //
                    {
                        mailinfolist.RemoveAt(i);
                        continue;
                    }

                    if (mailinfolist[i].ItemList.Count >= 2 && !ItemConfigCategory.Instance.Contain(mailinfolist[i].ItemList[1].ItemID))
                    {
                        mailinfolist.RemoveAt(i);
                        continue;
                    }
                }

                if (totalGold > 100000000)
                {
                    Log.Error($"邮件大量金币: {zone}  {userID}   {totalGold}");

                    if (userID == 2309112494397915136)
                    {
                        mailinfolist.Clear();
                    }
                }

                //存储邮件
                if (mailinfolist.Count > 100)
                {
                    mailinfolist.RemoveAt(0);
                }
                mailinfolist.Add(mailInfo);

                D2M_SaveComponent d2GSave = (D2M_SaveComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new M2D_SaveComponent() { UnitId = userID, EntityByte = MongoHelper.ToBson(dBMainInfo), ComponentType = DBHelper.DBMailInfo });
                return d2GSave.Error;
            }
        }
    }
}
