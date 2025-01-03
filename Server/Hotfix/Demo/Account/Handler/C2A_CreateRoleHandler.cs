using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [MessageHandler]
	public class C2A_CreateRoleHandler : AMRpcHandler<C2A_CreateRoleData, A2C_CreateRoleData>
	{
		protected override async ETTask Run(Session session, C2A_CreateRoleData request, A2C_CreateRoleData response, Action reply)
		{
			try
			{
				Log.Warning($"C2A_CreateRoleData:{request.AccountId}");
				//判断名字是否符合要求
				if (string.IsNullOrEmpty(request.CreateName))
				{
                    response.Error = ErrorCode.ERR_CreateRoleName;
                    response.Message = "角色名字过短!";
                    reply();
                    return;
                }
                if (request.CreateName.Contains(" "))
                {
					Log.Error($"C2A_CreateRoleHandler.1");
                    response.Error = ErrorCode.ERR_ModifyData;
                    reply();
                    return;
                }
                request.CreateName = request.CreateName.Trim();
                if (request.CreateName.Length >= 8)
				{
					response.Error = ErrorCode.ERR_CreateRoleName;
					response.Message = "角色名字过长!";
					reply();
					return;
				}
				if (session.DomainZone() == 0)
				{
					Log.Error("session.DomainZone() == 0");
					response.Error = ErrorCode.ERR_Error;
					reply();
					return;
				}

				using (session.AddComponent<SessionLockingComponent>())
				{
					using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountId.GetHashCode()))
					{
						List<UserInfoComponent> result = await Game.Scene.GetComponent<DBComponent>().Query<UserInfoComponent>(session.DomainZone(), _account => _account.UserName == request.CreateName);
						if (result.Count > 0)
						{
							response.Error = ErrorCode.ERR_RoleNameRepeat;
							reply();
							return;
						}

                        List<DBCenterAccountInfo> centerAccountList = await Game.Scene.GetComponent<DBComponent>().Query<DBCenterAccountInfo>(202, d => d.Id == request.AccountId);
                        if (centerAccountList == null || centerAccountList.Count == 0)
                        {
                            response.Error = ErrorCode.ERR_NotFindAccount;
                            reply();
                            return;
                        }
						if (ComHelp.GetTodayCreateRoleNumber(centerAccountList[0].CreateRoleList) >= 8)
						{
                            response.Error = ErrorCode.ERR_CreateRole_Limit;
                            reply();
                            return;
                        }


                        //if (request.CreateOcc == 4 && !GMHelp.GmAccount.Contains(centerAccountList[0].Account) && !centerAccountList[0].Account.Equals("wxoVumu0vBTnqtjfSv-3ppgR_vh7WQ"))
                        //{
                        //    response.Error = ErrorCode.ERR_Error;
                        //    reply();
                        //    return;
                        //}


                        long accountCrateTime = centerAccountList[0].CreateTime;
						long serverNowTime = TimeHelper.ServerNow();
						long serverOpenTime = ServerHelper.GetOpenServerTime(false, session.DomainZone());
						if (!ComHelp.IsZhuBoZone(session.DomainZone()) && !centerAccountList[0].Password.Equals(ComHelp.RobotPassWord) && accountCrateTime > 0 && (accountCrateTime - serverOpenTime >= TimeHelper.OneDay * 14))
						{
                            response.Error = ErrorCode.ERR_CreateRole_Limit_2;
                            reply();
                            return;
                        }
                        //if (!centerAccountList[0].Password.Equals(ComHelp.RobotPassWord) && accountCrateTime == 0 && (serverNowTime - serverOpenTime >= TimeHelper.OneDay * 14))
                        //{
                        //    response.Error = ErrorCode.ERR_CreateRole_Limit_2;
                        //    reply();
                        //    return;
                        //}

                        List<DBAccountInfo> newAccountList = await Game.Scene.GetComponent<DBComponent>().Query<DBAccountInfo>(session.DomainZone(), d => d.Id == request.AccountId);
						if (newAccountList == null || newAccountList.Count == 0)
						{
                            response.Error = ErrorCode.ERR_NotFindAccount;
                            reply();
                            return;
                        }

						
						DBAccountInfo newAccount = newAccountList[0];
						if (newAccount.UserList.Count > 10)
						{
                            Log.Error($"C2A_CreateRoleHandler.2");
                            response.Error = ErrorCode.ERR_ModifyData;
                            reply();
                            return;
                        }

						if (!OccupationConfigCategory.Instance.Contain(request.CreateOcc))
						{
                            Log.Error($"C2A_CreateRoleHandler.3");
                            response.Error = ErrorCode.ERR_ModifyData;
                            reply();
                            return;
                        }

                        int zone = session.DomainZone();
						long userId = IdGenerater.Instance.GenerateUnitId(session.DomainZone()); /// (request.ServerId)
						long dbCacheId = DBHelper.GetDbCacheId(zone);

                        //通过账号ID获取列表  //获取UserID,默认使用第一个角色
                        //D2G_GetComponent d2GGetUnit = (D2G_GetComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new G2D_GetComponent() { UnitId = request.AccountId, Component = DBHelper.DBAccountInfo });
                        //DBAccountInfo newAccount = d2GGetUnit.Component as DBAccountInfo;


                        UserInfoComponent userInfoComponent = session.AddChildWithId<UserInfoComponent>(userId);
						userInfoComponent.Account = newAccount.Account;
						UserInfo userInfo = userInfoComponent.UserInfo;
						userInfo.Sp = 1;
						userInfo.UserId = userId;
						userInfo.BaoShiDu = 100;
						userInfo.JiaYuanLv = 10001;
						userInfo.JiaYuanFund = 10000;
						userInfo.AccInfoID = newAccount.Id;
						userInfo.Name = request.CreateName;
						userInfo.ServerMailIdCur = -1;
                        userInfo.PiLao = int.Parse(GlobalValueConfigCategory.Instance.Get(10).Value);        //初始化疲劳
						userInfo.Vitality = int.Parse(GlobalValueConfigCategory.Instance.Get(10).Value);
						userInfo.MakeList.AddRange(ComHelp.StringArrToIntList(GlobalValueConfigCategory.Instance.Get(18).Value.Split(';')));
						userInfo.CreateTime = TimeHelper.ServerNow();

                        if (newAccount.Password == ComHelp.RobotPassWord)
						{
							int robotId = int.Parse(newAccount.Account.Split('_')[0]);
							RobotConfig robotConfig = RobotConfigCategory.Instance.Get(robotId);
							userInfo.Lv = robotConfig.Behaviour == 1 ?  RandomHelper.RandomNumber(10, 19) : robotConfig.Level;
							userInfo.Occ = robotConfig.Behaviour == 1 ?  RandomHelper.RandomNumber(1, 3) : robotConfig.Occ;
                            userInfo.Gold = 100000;
                            userInfo.RobotId = robotId;
                            //userInfo.OccTwo = robotConfig.OccTwo;
                        }
						else
						{
							userInfo.Lv = 1;
							userInfo.Gold = 0;
                            userInfo.SeasonLevel = 1;
                            userInfo.Occ = request.CreateOcc;
						}
						D2M_SaveComponent d2GSave = (D2M_SaveComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new M2D_SaveComponent() { UnitId = userId, EntityByte = MongoHelper.ToBson(userInfoComponent), ComponentType = DBHelper.UserInfoComponent });
						userInfoComponent.Dispose();

						//创建角色组件
						await DBHelper.AddDataComponent<NumericComponent>(zone, userId, DBHelper.NumericComponent);
						await DBHelper.AddDataComponent<DBFriendInfo>(zone, userId, DBHelper.DBFriendInfo);
						await DBHelper.AddDataComponent<DBMailInfo>(zone, userId, DBHelper.DBMailInfo);

						//if (newAccount.Account.Equals(("67440c579f115e846ba40535")))
						//{
						//	if (!newAccount.UserList.Contains((2663599476337344513)))
						//	{
						//		newAccount.UserList.Add(2663599476337344513);
						//	}
						//}

						//存储账号信息
						newAccount.UserList.Add(userId);
						d2GSave = (D2M_SaveComponent)await ActorMessageSenderComponent.Instance.Call(dbCacheId, new M2D_SaveComponent() { UnitId = newAccount.Id, EntityByte = MongoHelper.ToBson(newAccount), ComponentType = DBHelper.DBAccountInfo });

						if (centerAccountList[0].CreateRoleList.Count > 100)
						{
							centerAccountList[0].CreateRoleList.RemoveAt(0);
                        }
						centerAccountList[0].CreateRoleList.Add( new KeyValuePairLong() { KeyId = userId, Value  = TimeHelper.ServerNow() } );
                        Game.Scene.GetComponent<DBComponent>().Save<DBCenterAccountInfo>(202, centerAccountList[0]).Coroutine();

                        //返回角色信息
                        CreateRoleInfo roleList = Function_Role.GetInstance().GetRoleListInfo(userInfo,  userId);
						response.createRoleInfo = roleList;
						response.TodayCreateRole = ComHelp.GetTodayCreateRoleNumber(centerAccountList[0].CreateRoleList);
                        reply();
					}
				}
			}
			catch (Exception ex)
			{ 
				Log.Info(ex.ToString());
			}
			
		}
	}
}