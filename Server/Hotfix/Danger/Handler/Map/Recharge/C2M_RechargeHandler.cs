using System;

namespace ET
{

    [ActorMessageHandler]
    public class C2M_RechargeHandler : AMActorLocationRpcHandler<Unit, C2M_RechargeRequest, M2C_RechargeResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_RechargeRequest request, M2C_RechargeResponse response, Action reply)
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.Recharge, unit.Id))
            {
                long dbCacheId = DBHelper.GetCenterServerId();
                C2C_CenterServerInfoRespone d2GGetUnit = (C2C_CenterServerInfoRespone)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new C2C_CenterServerInfoReuest() { Zone = unit.DomainZone(), infoType = 1 });
                //Log.ILog.Info("d2GGetUnit.Value = " + d2GGetUnit.Value);
                if (int.Parse(d2GGetUnit.Value) != 1)
                {
                    reply();
                    return;
                }
                if (ComHelp.IsBanHaoZone(unit.DomainZone()))
                {
                    LogHelper.LogWarning($"充值[版号服]SendDiamondToUnit: {unit.Id}");
                    Console.WriteLine($"充值[版号服]SendDiamondToUnit: {unit.Id}");
                    RechargeHelp.SendDiamondToUnit(unit, request.RechargeNumber, "版号服");
                    reply();
                    return;
                }
                if (ComHelp.IsInnerNet())
                {
                    //RechargeHelp.SendDiamondToUnit(unit, request.RechargeNumber, "内测服");
                    reply();
                    return;
                }

                if (request.RechargeNumber <= 0 || ConfigHelper.GetDiamondNumber(request.RechargeNumber) <= 0)
                {
                    Log.Console($"充值作弊： 区：{unit.DomainZone()}  ID：{unit.Id}  rechargenumber: {request.RechargeNumber}");
                    Log.Warning($"充值作弊： 区：{unit.DomainZone()}  ID：{unit.Id}  rechargenumber: {request.RechargeNumber}");
                    reply();
                    return;
                }

                string serverName = ServerHelper.GetGetServerItem(false, unit.DomainZone()).ServerName;
                UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
                string userName = userInfoComponent.UserInfo.Name;

                if (request.PayType == PayTypeEnum.IOSPay)
                {
                    ///IOS仅用来打印日志
                    Log.Warning($"支付订单[IOS]拉起: 服务器:{serverName} 玩家:{userName}  充值金额:{request.RechargeNumber}");
                    Log.Console($"支付订单[IOS]拉起: 服务器:{serverName} 玩家:{userName}  充值金额:{request.RechargeNumber}  时间:{TimeHelper.DateTimeNow().ToString()}");
                    reply();
                    return;
                }

                if (request.PayType == PayTypeEnum.WeiXinPay)
                {
                    Log.Warning($"支付订单[微信支付]拉起:服务器:{serverName} 玩家:{userName}   充值金额:{request.RechargeNumber}");
                    Log.Console($"支付订单[微信支付]拉起:服务器:{serverName} 玩家:{userName}   充值金额:{request.RechargeNumber}  时间:{TimeHelper.DateTimeNow().ToString()}");
                }

                if (request.PayType == PayTypeEnum.AliPay)
                {
                    Log.Warning($"支付订单[支付宝]拉起: 服务器:{serverName} 玩家:{userName}   充值金额:{request.RechargeNumber}");
                    Log.Console($"支付订单[支付宝]拉起: 服务器:{serverName} 玩家:{userName}   充值金额:{request.RechargeNumber}  时间:{TimeHelper.DateTimeNow().ToString()}");
                }

                if (request.PayType == PayTypeEnum.TikTok)
                {
                    Log.Warning($"支付订单[TikTok]拉起: 服务器:{serverName} 玩家:{userName}   充值金额:{request.RechargeNumber}");
                    Log.Console($"支付订单[TikTok]拉起: 服务器:{serverName} 玩家:{userName}   充值金额:{request.RechargeNumber}  时间:{TimeHelper.DateTimeNow().ToString()}");
                }

                long rechareId = DBHelper.GetRechargeCenter();
              
                R2M_RechargeResponse r2M_RechargeResponse = (R2M_RechargeResponse)await ActorMessageSenderComponent.Instance.Call(rechareId, new M2R_RechargeRequest()
                {
                    Zone = unit.DomainZone(),
                    PayType = request.PayType,
                    UnitId = unit.Id,
                    UnitName = userName,
                    RechargeNumber = request.RechargeNumber,
                    Account = userInfoComponent.Account,
                    payMessage = request.RiskControlInfo,
                    ClientIp = userInfoComponent.RemoteAddress
                });

                response.Message = r2M_RechargeResponse.Message;
            }
            reply();
            await ETTask.CompletedTask;
        }
    }
}
