using System;
using System.Collections.Generic;

namespace ET
{
    public class C2A_LoginAccountHandler : AMRpcHandler<C2A_LoginAccount, A2C_LoginAccount>
    {
        
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            try
            {
                LogHelper.LogWarning($"LoginTest request.AccountName:{request.AccountName} {request.Password} {session.RemoteAddress}", true);
                if (session.DomainScene().SceneType != SceneType.Account)
                {
                    Log.Warning($"LoginTest C2A_LoginAccount请求的Scene错误，当前Scene为：{session.DomainScene().SceneType}");
                    session.Dispose();
                    return;
                }
                session.RemoveComponent<SessionAcceptTimeoutComponent>();

                if (session.GetComponent<SessionLockingComponent>() != null)
                {
                    response.Error = ErrorCode.ERR_RequestRepeatedly;
                    reply();
                    session.Disconnect().Coroutine();
                    return;
                }

                if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.Password))
                {
                    response.Error = ErrorCode.ERR_LoginInfoIsNull;
                    reply();
                    session.Disconnect().Coroutine();
                    return;
                }
                if (request.AccountName.Contains("请选择一种登录方式"))
                {
                    response.Error = ErrorCode.ERR_LoginInfoIsNull;
                    response.Message = "请联系qq136087482处理";
                    reply();
                    session.Disconnect().Coroutine();
                    return;
                }


                if (session.DomainZone() == 128 && !GMHelp.ZhuBoURBossAccount.Contains(request.AccountName))
                {
                    response.Error = ErrorCode.ERR_VersionNoMatch;
                    reply();
                    return;
                }

                if (!request.Password.Equals( request.ThirdLogin ) && !request.Password.Equals(ComHelp.RobotPassWord))
                {
                    Log.Warning($"登录的账号:  {request.AccountName} {request.Password} {request.ThirdLogin}");
                }

                if (!ComHelp.RobotPassWord.Equals(request.Password))
                {
                    //Log.Console($"客户端登录: {TimeHelper.DateTimeNow().ToString()} {session.RemoteAddress}");
                    Log.Warning($"账号登录(LoginAccount):{session.DomainZone()}   {request.AccountName}  {session.RemoteAddress} ");
                }

                if (session.RemoteAddress.ToString().Contains("42.177.217.71"))
                {
                    response.Error = ErrorCode.ERR_LoginInfoIsNull;
                    reply();
                    session.Disconnect().Coroutine();
                    return;
                }

                if (request.Password == "3" || request.Password == "4")
                {
                    if (request.AccountName.Length < 3)
                    {
                        response.Error = ErrorCode.ERR_LoginInfoIsNull;
                        reply();
                        session.Disconnect().Coroutine();
                        return;
                    }
                    string head = request.AccountName.Substring(0, 3);
                    if (GMHelp.IllegalPhone.Contains(head))
                    {
                        response.Error = ErrorCode.ERR_IllegalPhoneError;
                        reply();
                        session.Disconnect().Coroutine();
                        return;
                    }
                }

                //密码要md5
                //if (!Regex.IsMatch(request.AccountName.Trim(),@"^(?=.*[0-9].*)(?=.*[A-Z].*)(?=.*[a-z].*).{6,15}$"))
                //{
                //    response.Error = ErrorCode.ERR_AccountNameFormError;
                //    reply();
                //    session.Disconnect().Coroutine();
                //    return;
                //}

                //if (!Regex.IsMatch(request.Password.Trim(),@"^[A-Za-z0-9]+$"))
                //{
                //    response.Error = ErrorCode.ERR_PasswordFormError;
                //    reply();
                //    session.Disconnect().Coroutine();
                //    return;
                //}
                //public const int RegisterLogin = 0;     //注册账号登录
                //public const int WeixLogin = 1;         //微信登录
                //public const int QQLogin = 2;           //QQ登录
                //public const int PhoneCodeLogin = 3;         //短信验证吗登录
                //public const int PhoneNumLogin = 4;        //手机号登录
                //public const int TapTap = 5;                //taptap登录
                //先检测一下QQ和微信登录
                long AccountId = 0;
                long sessionId  = session.InstanceId;
                if (!string.IsNullOrEmpty(request.ThirdLogin) && request.ThirdLogin.Length > 0)
                {
                    using (session.AddComponent<SessionLockingComponent>())
                    {
                        using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountName.Trim().GetHashCode()))
                        {
                            long accountZone = DBHelper.GetAccountCenter();
                            Center2A_CheckAccount centerAccount = (Center2A_CheckAccount)await ActorMessageSenderComponent.Instance.Call(accountZone, new A2Center_CheckAccount()
                            {
                                AccountName = request.AccountName,
                                Password = request.Password,
                                ThirdLogin = request.ThirdLogin
                            });
                            PlayerInfo playerInfo = centerAccount.PlayerInfo != null ? centerAccount.PlayerInfo : null;

                            //没有则注册
                            if (centerAccount.PlayerInfo == null)
                            {
                                Center2A_RegisterAccount saveAccount = (Center2A_RegisterAccount)await ActorMessageSenderComponent.Instance.Call(accountZone, new A2Center_RegisterAccount()
                                {
                                    AccountName = request.AccountName,
                                    Password = request.Password
                                });
                                AccountId = saveAccount.AccountId;

                                if (request.AccountName == "7303474616922905355")
                                {
                                    Console.WriteLine($"注册新账号:    {request.AccountName}   {AccountId}");
                                }
                            }
                            else
                            {
                                AccountId = centerAccount.AccountId;

                                if (request.AccountName == "7303474616922905355")
                                {
                                    Console.WriteLine($"已有老账号:    {request.AccountName}   {AccountId}");
                                }
                            }
                        }
                    }
                }

                if( sessionId != session.InstanceId)
                {
                    return;
                }
                using (session.AddComponent<SessionLockingComponent>())
                {
                    using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountName.Trim().GetHashCode()))
                    {
                        if (session.IsDisposed || session.DomainZone() == 0)
                        {
                            Log.Console($"session.IsDisposed: {request.AccountName}");
                            response.Error = ErrorCode.ERR_LoginInfoIsNull;
                            reply();
                            session.Disconnect().Coroutine();
                            return;
                        }

                        List<DBAccountInfo> accountInfoList = await Game.Scene.GetComponent<DBComponent>().Query<DBAccountInfo>(session.DomainZone(), d => d.Account == request.AccountName && d.Password == request.Password); 
                        if (accountInfoList.Count == 0 && AccountId > 0)
                        {
                            accountInfoList = await Game.Scene.GetComponent<DBComponent>().Query<DBAccountInfo>(session.DomainZone(), d => d.Id == AccountId);
                        }

                        if (accountInfoList.Count == 0 && (request.Password == "3" || request.Password == "4"))
                        {
                            string password = request.Password == "3" ? "4" : "3";
                            accountInfoList = await Game.Scene.GetComponent<DBComponent>().Query<DBAccountInfo>(session.DomainZone(), d => d.Account == request.AccountName && d.Password == password);
                        }

                        DBAccountInfo account = accountInfoList != null && accountInfoList.Count > 0 ? accountInfoList[0] : null;
                        if (AccountId > 0 && account == null)
                        {
                            //Log.Console($"当前区找不到账号: {session.DomainZone()} {request.AccountName} {request.Password}");
                            //Log.Warning($"当前区找不到账号: {session.DomainZone()} {request.AccountName} {request.Password}");
                        }
                        bool IsHoliday = false;
                        bool StopServer = false;
                        long accountZone = DBHelper.GetAccountCenter();

                        if (!string.IsNullOrEmpty(request.OAID))
                        {
                            //Console.WriteLine($"account:{request.AccountName}    oaid:{request.OAID}");
                        }

                        Center2A_CheckAccount centerAccount = (Center2A_CheckAccount)await ActorMessageSenderComponent.Instance.Call(accountZone, new A2Center_CheckAccount()
                        {
                            AccountName = request.AccountName,
                            Password = request.Password,
                            ThirdLogin = request.ThirdLogin,
                            DeviceID = request.DeviceID,
                            OAID = request.OAID,
                        });
                        PlayerInfo centerPlayerInfo = centerAccount.PlayerInfo;
                        IsHoliday = centerAccount.IsHoliday;
                        StopServer = centerAccount.StopServer;
                        if (StopServer && !GMHelp.GmAccount.Contains(request.AccountName) && !session.RemoteAddress.ToString().Contains("119.39.6.77")) //黑名单
                        {
                            response.Error = ErrorCode.ERR_StopServer;
                            reply();
                            session.Disconnect().Coroutine();
                            return;
                        }

                        if (centerPlayerInfo == null)
                        {
                            response.Error = ErrorCode.ERR_LoginInfoIsNull;
                            reply();
                            session.Disconnect().Coroutine();
                            return;
                        }

                        if (request.AccountName == "7303474616922905355" && account != null && centerAccount.AccountId != account.Id)
                        {
                            Console.WriteLine($"账号不匹配:    {request.AccountName}   {AccountId}  {account.Id}");
                            await  Game.Scene.GetComponent<DBComponent>().Remove<DBAccountInfo>(session.DomainZone(), account.Id);
                            account = null;
                        }

                        if (account == null)
                        {
                            //在该区创建账号信息
                            account = session.AddChildWithId<DBAccountInfo>(centerAccount.AccountId);
                            account.Account = request.AccountName;
                            account.Password = request.Password;
                            account.CreateTime = TimeHelper.ServerNow();
                            await Game.Scene.GetComponent<DBComponent>().Save<DBAccountInfo>(session.DomainZone(), account);
                        }
                        
                        if (centerAccount.Message == "2"
                            && !session.RemoteAddress.ToString().Contains("119.39.6.47")
                            && !session.RemoteAddress.ToString().Contains("119.39.6.77")
                            && !session.RemoteAddress.ToString().Contains("117.152.141.41")) //黑名单
                        {
                            response.Error = ErrorCode.ERR_AccountInBlackListError;
                            response.AccountId = account.Id;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                        if (centerPlayerInfo.RealName == 0)
                        {
                            response.Error = ErrorCode.ERR_NotRealName;
                            response.AccountId = account.Id;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                        if (session.IsDisposed || session.DomainZone() == 0)
                        {
                            Log.Console($"session.IsDisposed: {request.AccountName}");
                            response.Error = ErrorCode.ERR_LoginInfoIsNull;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                        //if (!account.Password.Equals(request.Password))
                        //{
                        //    response.Error = ErrorCode.ERR_AccountOrPasswordError;
                        //    reply();
                        //    session.Disconnect().Coroutine();
                        //    account?.Dispose();
                        //    return;
                        //}
                        //防沉迷相关
                        string idCardNo = centerPlayerInfo.IdCardNo;
                        int canLogin = CanLogin(idCardNo, IsHoliday, request.age_type);
                        if (canLogin != ErrorCode.ERR_Success)
                        {
                            response.Error = canLogin;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }

                        TokenComponent tokenComponent = session.DomainScene().GetComponent<TokenComponent>();
                        string queueToken = tokenComponent.Get(account.Id);

                        //在线人数判断有问题。[获取的是在保存在账号服的玩家数量]
                        AccountSessionsComponent accountSessionsComponent = session.DomainScene().GetComponent<AccountSessionsComponent>();
                        long onlineNumber = accountSessionsComponent.GetAll().Values.Count;
                        int maxNumber = GlobalValueConfigCategory.Instance.OnLineLimit;
                        //Log.Console($" {session.DomainZone()} ---  onlineNumber:{onlineNumber}");
                        if (accountSessionsComponent.Get(account.Id) == 0 &&
                            onlineNumber >= maxNumber && (string.IsNullOrEmpty(queueToken) || queueToken != request.Token))
                        {
                            Log.Console($" {session.DomainZone()} --- onlineNumber: {onlineNumber}  queueToken:{queueToken} request.Token:{request.Token}");

                            queueToken = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString();
                            tokenComponent.Remove(account.Id);
                            tokenComponent.Add(account.Id, queueToken, true);

                            long queueServerId = DBHelper.GetQueueServerId(session.DomainZone());
                            Q2A_EnterQueue d2GGetUnit = (Q2A_EnterQueue)await ActorMessageSenderComponent.Instance.Call(queueServerId, new A2Q_EnterQueue()
                            {
                                AccountId = account.Id,
                                Token = queueToken
                            });

                            //进入排队
                            response.Error = ErrorCode.ERR_EnterQueue;
                            response.AccountId = account.Id;
                            response.QueueNumber = d2GGetUnit.QueueNumber;
                            response.QueueAddres = StartSceneConfigCategory.Instance.Queues[session.DomainZone()].OuterIPPort.ToString();
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }

                        //请求登录中心服查询有没有同账号玩家登录[uwa]
                        //StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(session.DomainZone(), "LoginCenter");
                        //long loginCenterInstanceId = startSceneConfig.InstanceId;
                        long loginCenterInstanceId = StartSceneConfigCategory.Instance.LoginCenterConfig.InstanceId;//踢掉进入gate的玩家
                        var loginAccountResponse = (L2A_LoginAccountResponse)await ActorMessageSenderComponent.Instance.Call(loginCenterInstanceId, new A2L_LoginAccountRequest() { AccountId = account.Id, Relink = request.Relink });

                        if (session.IsDisposed)
                        { 
                            return;
                        }

                        if (loginAccountResponse.Error != ErrorCode.ERR_Success)
                        {
                            response.Error = loginAccountResponse.Error;

                            reply();
                            session?.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                        //AccountSessionsComponent.Remove 需要在适当的时候移除
                        long accountSessionInstanceId = accountSessionsComponent.Get(account.Id);
                        Session otherSession = Game.EventSystem.Get(accountSessionInstanceId) as Session;
                        if (otherSession != null)
                        {
                            LogHelper.LogDebug($"LoginTest C2A_LoginAccount.ERR_OtherAccountLogin1 account.Id: {account.Id}");
                        }
                        otherSession?.Send(new A2C_Disconnect() { Error = ErrorCode.ERR_OtherAccountLogin });                 //踢accout服的玩家下线
                        otherSession?.Disconnect().Coroutine();
                        accountSessionsComponent.Add(account.Id, session.InstanceId);
                        session.AddComponent<AccountCheckOutTimeComponent, long>(account.Id);   //自己在账号服只能停留600秒

                        string Token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString();
                        tokenComponent.Remove(account.Id);    //Token也是保留十分钟
                        tokenComponent.Add(account.Id, Token);

                        response.RoleLists.Clear();
                        long dbCacheId = DBHelper.GetDbCacheId(session.DomainZone());
                        for (int i = 0; i < account.UserList.Count; i++)
                        {
                            D2G_GetComponent d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = account.UserList[i], Component = DBHelper.UserInfoComponent });
                            if (d2GGetUnit.Component == null)
                            {
                                continue;
                            }

                            UserInfoComponent userinfo = d2GGetUnit.Component as UserInfoComponent;
                            CreateRoleInfo roleList = Function_Role.GetInstance().GetRoleListInfo(userinfo.UserInfo, account.UserList[i]);

                            d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = account.UserList[i], Component = DBHelper.NumericComponent });
                            if (d2GGetUnit.Component!=null)
                            {
                                NumericComponent numericComponent = d2GGetUnit.Component as NumericComponent;
                                roleList.WeaponId = numericComponent.GetAsInt(NumericType.Now_Weapon);
                                roleList.EquipIndex = numericComponent.GetAsInt(NumericType.EquipIndex);
                            }
                           
                            d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = account.UserList[i], Component = DBHelper.BagComponent });
                            if (d2GGetUnit.Component != null)
                            {
                                BagComponent bagComponent = d2GGetUnit.Component as BagComponent;
                                roleList.FashionIds = bagComponent.FashionEquipList;
                            }
                            
                            response.RoleLists.Add(roleList);
                        }

                        response.TaprepRequest = centerAccount.TaprepRequest;
                        response.PlayerInfo = centerPlayerInfo;
                        response.AccountId = account.Id;
                        response.Token = Token;
                        for (int r = 0; r < response.PlayerInfo.RechargeInfos.Count; r++)
                        {
                            response.PlayerInfo.RechargeInfos[r].OrderInfo = String.Empty;
                        }
                        account?.Dispose();
                        reply();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public int CanLogin(string identityCard, bool isHoliday, int age_type)
        {
            int age = IDCardHelper.GetBirthdayAgeSex(identityCard, age_type);
            if (age >= 18)
            {
                return ErrorCode.ERR_Success;
            }
            if (age < 12)
            {
                return ErrorCode.ERR_FangChengMi_Tip6;
            }
            DateTime dateTime = TimeHelper.DateTimeNow();
            if (isHoliday)
            {
                if (dateTime.Hour == 20)
                {
                    return ErrorCode.ERR_Success;           //允许登录
                }
                else
                {
                    return ErrorCode.ERR_FangChengMi_Tip7;
                }
            }
            else
            {
                return ErrorCode.ERR_FangChengMi_Tip7;
            }
        }

    }
}