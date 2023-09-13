﻿using cn.sharesdk.unity3d;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{

    public class UILoginComponentAwakeSystem : AwakeSystem<UILoginComponent>
	{

		public void TestQulity()
		{
			QualitySettings.vSyncCount = 0;
		}

		public override void Awake(UILoginComponent self)
		{
			try
			{
                Log.ILog.Debug($"UILoginComponent  111");

                self.InitSdk();

				Application.runInBackground = true;
				//关闭垂直同步
				libx.Assets.MAX_BUNDLES_PERFRAME = 32;
				//Screen.sleepTimeout = SleepTimeout.NeverSleep; 
				self.ZoneScene().GetComponent<MapComponent>().SetMapInfo((int)SceneTypeEnum.LoginScene, 0, 0);
				self.LastLoginTime = 0;
				ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
                self.ZhuCe = rc.Get<GameObject>("ZhuCe");
				self.ZhuCe.SetActive(false);
				self.Btn_Return = rc.Get<GameObject>("Btn_Return");
				self.Btn_Return.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_Return(); });

				ButtonHelp.AddListenerEx(self.ZhuCe.transform.Find("Btn_QQ").gameObject, () => { self.OnBtn_QQLogin(); });
                ButtonHelp.AddListenerEx(self.ZhuCe.transform.Find("Btn_WeChat").gameObject, () => { self.OnBtn_WeChatLogin(); });
				ButtonHelp.AddListenerEx(self.ZhuCe.transform.Find("Btn_ZhuCe").gameObject, () => { self.OnBtn_ZhuCe(); });
				ButtonHelp.AddListenerEx(self.ZhuCe.transform.Find("Btn_iPhone").gameObject, () => { self.OnBtn_iPhone(); });
				ButtonHelp.AddListenerEx(self.ZhuCe.transform.Find("Btn_TapTap").gameObject, () => { self.OnBtn_TapTap(); });
                self.ZhuCe.transform.Find("Btn_ZhuCe").gameObject.SetActive(GlobalHelp.IsEditorMode);

				self.DeleteAccountBtn = rc.Get<GameObject>("DeleteAccountBtn");
				self.DeleteAccountBtn.SetActive(false);
				ButtonHelp.AddListenerEx(self.DeleteAccountBtn, () => { self.OnDeleteAccountBtn(); });

				bool taptap = GlobalHelp.GetPlatform() == 1;
#if TapTap1
				taptap = true;
#endif
                int bigversion = GlobalHelp.GetBigVersion();
                self.ZhuCe.transform.Find("Btn_TapTap").gameObject.SetActive(bigversion >= 15 && taptap);
                self.AccountText = rc.Get<GameObject>("AccountText");


				self.AccountText.GetComponent<Text>().text = GlobalHelp.IsBanHaoMode ? "注册账号" : "切换账号";

				Log.ILog.Debug($"self.IOSReview:  {self.IOSReview}");
				Log.ILog.Debug($"self.GetBigVersion:  {GlobalHelp.GetBigVersion()}");
				if (self.IOSReview && GlobalHelp.GetBigVersion() == 15)
				{
#if UNITY_IPHONE || UNITY_IOS
				self.DeleteAccountBtn.SetActive(true);
				self.AccountText.GetComponent<Text>().text = "注册账号";
#endif
				}
                self.YanZheng = rc.Get<GameObject>("YanZheng");
				self.SendYanzheng = rc.Get<GameObject>("SendYanzheng");
				self.IPhone = rc.Get<GameObject>("IPhone");
				self.YanZheng.SetActive(false);
				self.SendYanzheng.SetActive(false);
				self.IPhone.SetActive(false);
				self.ButtonCommitCode = rc.Get<GameObject>("ButtonCommitCode");
				self.ButtonGetCode = rc.Get<GameObject>("ButtonGetCode");
				self.TextPhoneCode = rc.Get<GameObject>("TextPhoneCode");
				self.PhoneNumber = rc.Get<GameObject>("PhoneNumber");
				ButtonHelp.AddListenerEx(self.ButtonCommitCode, () => { self.OnButtonCommitCode(); });
				ButtonHelp.AddListenerEx(self.ButtonGetCode, () => { self.OnButtonGetCode(); });

				self.BanHanNode = rc.Get<GameObject>("BanHanNode");
				self.HideNode = rc.Get<GameObject>("HideNode");

				self.ButtonOtherLogin = rc.Get<GameObject>("ButtonOtherLogin");
				self.ButtonYiJianLogin = rc.Get<GameObject>("ButtonYiJianLogin");
				self.TextPhoneNumber = rc.Get<GameObject>("TextPhoneNumber");
				self.TextPhoneNumber.GetComponent<Text>().text = string.Empty;
                self.ThirdLoginBg = rc.Get<GameObject>("ThirdLoginBg");
				self.YiJianDengLu = rc.Get<GameObject>("YiJianDengLu");
				self.YiJianDengLu.SetActive(false);
				//切换替他登录方式 });
				ButtonHelp.AddListenerEx(self.ButtonOtherLogin, () => { self.OnButtonOtherLogin(); });
				ButtonHelp.AddListenerEx(self.ButtonYiJianLogin, () => { self.OnButtonYiJianLogin(); });

				GameObject.Find("Global").GetComponent<SMSSDemo>().CommitCodeSucessHandler = (string text) => { self.OnCommitCodeHandler(text); };
				GameObject.Find("Global").GetComponent<Init>().OnGetPhoneNumHandler = (string text) => { self.OnGetPhoneNum(text); };

				self.RealNameButton = rc.Get<GameObject>("RealNameButton");
				self.RealNameButton.GetComponent<Button>().onClick.AddListener(() => { self.OnRealNameButton(); });

				self.loginBtn = rc.Get<GameObject>("LoginBtn");
				//self.loginBtn.GetComponent<Button>().onClick.AddListener();
				ButtonHelp.AddListenerEx(self.loginBtn, () => { self.OnLogin(); });
				self.registerBtn = rc.Get<GameObject>("CreateAccountBtn");
				self.registerBtn.GetComponent<Button>().onClick.AddListener(() => { self.OnRegister(); });
				self.Account = rc.Get<GameObject>("Account");
				self.Password = rc.Get<GameObject>("Password");

				self.ObjNoticeBtn = rc.Get<GameObject>("NoticeBtn");
				self.ObjSelectBtn = rc.Get<GameObject>("SelectBtn");
				self.SelectServerName = rc.Get<GameObject>("SelectServerName");
				self.TextYanzheng = rc.Get<GameObject>("TextYanzheng");

				self.ObjNoticeBtn.GetComponent<Button>().onClick.AddListener(() => { self.OnNotice(); });
				self.ObjSelectBtn.GetComponent<Button>().onClick.AddListener(() => { self.OnSelectServerList(); });

				self.YongHuXieYiClose = rc.Get<GameObject>("YongHuXieYiClose");
				self.YongHuXieYi = rc.Get<GameObject>("YongHuXieYi");
				self.YinSiXieYi = rc.Get<GameObject>("YinSiXieYi");
				self.YinSiXieYiClose = rc.Get<GameObject>("YinSiXieYiClose");
				self.YinSiToggle = rc.Get<GameObject>("YinSiToggle");
				self.TextButton_2 = rc.Get<GameObject>("TextButton_2");
				self.TextButton_1 = rc.Get<GameObject>("TextButton_1");
				self.YongHuXieYi.SetActive(false);
				self.YinSiXieYi.SetActive(false);

				self.TextButton_2_2 = rc.Get<GameObject>("TextButton_2_2");
				self.TextButton_2_1 = rc.Get<GameObject>("TextButton_2_1");
				self.YinSiToggle2 = rc.Get<GameObject>("YinSiToggle2");
				self.TextYinSi = rc.Get<GameObject>("TextYinSi");

                self.TextButton_2.GetComponent<Button>().onClick.AddListener(() => { self.YongHuXieYi.SetActive(true); });
				self.TextButton_1.GetComponent<Button>().onClick.AddListener(() => { self.YinSiXieYi.SetActive(true); });
				self.TextButton_2_2.GetComponent<Button>().onClick.AddListener(() => { self.YongHuXieYi.SetActive(true); });
				self.TextButton_2_1.GetComponent<Button>().onClick.AddListener(() => { self.YinSiXieYi.SetActive(true); });
				self.YongHuXieYiClose.GetComponent<Button>().onClick.AddListener(() => { self.YongHuXieYi.SetActive(false); });
				self.YinSiXieYiClose.GetComponent<Button>().onClick.AddListener(() => { self.YinSiXieYi.SetActive(false); });

				self.TextYinSi.SetActive(false);
                UILoginHelper.ShowTextList(self.TextYinSi);

                self.LoginErrorNumber = 0;
				self.Loading = rc.Get<GameObject>("Loading");
				UI uirotate = self.AddChild<UI, string, GameObject>("RightPositionSet", rc.Get<GameObject>("Img_Loading"));
				uirotate.AddComponent<UIRotateComponent>();
				self.UIRotateComponent = uirotate;
				self.Loading.SetActive(false);
				self.RequestAllServer().Coroutine();
				GameSettingLanguge.Instance.InitRandomName().Coroutine();
				self.PlayerComponent = self.DomainScene().GetComponent<AccountInfoComponent>();
				Game.Scene.GetComponent<SoundComponent>().InitMusicVolume();
                Game.Scene.GetComponent<SceneManagerComponent>().PlayBgmSound(self.ZoneScene(), (int)SceneTypeEnum.LoginScene);
				self.InitLoginType();
				self.UpdateLoginType();

                Log.ILog.Debug($"UILoginComponent  222");

                if ((bigversion >= 14 && bigversion < 16) && string.IsNullOrEmpty(PlayerPrefsHelp.GetString("UIYinSi0627")))
                {
                    UIHelper.Create(self.ZoneScene(), UIType.UIYinSi).Coroutine();
					PlayerPrefsHelp.SetString("UIYinSi0627", "1");
                }
			}
			catch (Exception E)
			{
				Log.Error(E.ToString());
			}
		}
	}
	
	public static class UILoginComponentSystem
	{

		public static void InitSdk(this UILoginComponent self)
		{
			GameObject sharesdk = GameObject.Find("Global");
			ShareSDK ssdk = sharesdk.GetComponent<ShareSDK>();
			ssdk.authHandler = (int reqID, ResponseState state, cn.sharesdk.unity3d.PlatformType type, Hashtable result) => 
			{
				self.OnAuthResultHandler(reqID, state, type, result); 
			};
			ssdk.showUserHandler = self.OnGetUserInfoResultHandler;
			self.ssdk = ssdk;

#if UNITY_ANDROID
            TapSDKHelper.Init();
#endif
        }

        /// <summary>
        /// 返回各平台用户信息
        /// </summary>
        /// <param name="reqID"></param>
        /// <param name="state"></param>
        /// <param name="type"></param>
        /// <param name="result"></param>
        public static void OnGetUserInfoResultHandler(this UILoginComponent self,  int reqID, ResponseState state, cn.sharesdk.unity3d.PlatformType type, Hashtable result)
		{
			Log.ILog.Debug("get user info result:");
			Log.ILog.Debug((MiniJSON.jsonEncode(result)));
			Log.ILog.Debug(("get user info sucess ! platform :" + type));
			if (type == cn.sharesdk.unity3d.PlatformType.WeChat)
			{
				Log.ILog.Debug(("get user info:   " + MiniJSON.jsonEncode(self.ssdk.GetAuthInfo(type))));
				if (state == ResponseState.Success)
				{
					result = self.ssdk.GetAuthInfo(type);
#if UNITY_ANDROID
					string openId = result["openID"].ToString();  //openID == userID
					Log.ILog.Debug("get user info openId :" + openId);
					string userId = result["unionID"].ToString();
					Log.ILog.Debug("get user info userId :" + userId);
#elif UNITY_IPHONE
					string openId = result["uid"].ToString();  //openID == userID
					Log.ILog.Debug("get user info openId :" + openId);
					string userId = result["token"].ToString();
					Log.ILog.Debug("get user info userId :" + userId);
#endif
					self.OnGetUserInfo($"wx{openId};wx{userId}");
				}
				else
				{
					self.OnGetUserInfo("fail");
				}
			}
			if (type == cn.sharesdk.unity3d.PlatformType.QQ)
			{
				Log.ILog.Debug("get user info:   " + MiniJSON.jsonEncode(self.ssdk.GetAuthInfo(type)));
				if (state == ResponseState.Success)
				{
					result = self.ssdk.GetAuthInfo(type);
#if UNITY_ANDROID
					string openId = result["unionID"].ToString();
					string userId = result["userID"].ToString();
#elif UNITY_IPHONE
					string openId = result["uid"].ToString();
					string userId = result["token"].ToString();
#endif
					Log.ILog.Debug($"openId: {openId}:  userId:{userId}");
					self.OnGetUserInfo($"qq{openId};qq{userId}");
				}
				else
				{
					self.OnGetUserInfo("fail");
				}
			}
		}


		/// <summary>
		/// 授权返回
		/// </summary>
		/// <param name="reqID"></param>
		/// <param name="state"></param>
		/// <param name="type"></param>
		/// <param name="result"></param>
		public static void OnAuthResultHandler(this UILoginComponent self, int reqID, ResponseState state, cn.sharesdk.unity3d.PlatformType type, Hashtable result)
		{
			Log.ILog.Debug("OnAuthResultHandler:" + MiniJSON.jsonEncode(result));
			if (state != cn.sharesdk.unity3d.ResponseState.Success)
			{
				self.OnAuthorize("fail");
				return;
			}

			switch (type)
			{
				case cn.sharesdk.unity3d.PlatformType.WeChat:
					string openId = result["openid"].ToString();
					self.OnAuthorize($"sucess");
					break;
				case cn.sharesdk.unity3d.PlatformType.QQ:
					openId = result["openid"].ToString();
					self.OnAuthorize($"sucess");
					break;
				default:
					break;
			}
		}

		public static async ETTask GetTapUserInfo(this UILoginComponent self, string logintype)
        {
			await ETTask.CompletedTask;
            Init init = GameObject.Find("Global").GetComponent<Init>();
            Log.ILog.Debug("GetTapUserInfo1111: ");
			string tatapid =  await init.TapTapLogin();
			if (string.IsNullOrEmpty(tatapid))
			{
				FloatTipManager.Instance.ShowFloatTip("请确认是否登录TapTap！");
				return;
			}
			self.LoginType = logintype;
			Log.ILog.Debug($"GetTapUserInfo2222: {tatapid}");
            self.OnGetTapUserInfo(tatapid);
        }

        /// <summary>
        /// 获取各平台用户信息
        /// </summary>
        /// <param name="fenxiangtype"></param>
        public static void GetUserInfo(this UILoginComponent self ,string fenxiangtype)
		{
			Log.ILog.Debug($"sharesdk GetUserInfo1");
#if !UNITY_EDITOR
			Log.ILog.Debug($"sharesdk GetUserInfo2");

			switch (fenxiangtype)
			{
				case "1":
					self.ssdk.GetUserInfo(cn.sharesdk.unity3d.PlatformType.WeChat);
					break;
				case "2":
					self.ssdk.GetUserInfo(cn.sharesdk.unity3d.PlatformType.QQ);
					break;
			}
#else
			string add = fenxiangtype == "1" ? "wx" : "qq";
			self.OnGetUserInfo($"{add}{PhoneNumberHelper.getRandomTel()};{add}{PhoneNumberHelper.getRandomTel()}");
#endif
		}

		public static  void OnDeleteAccountBtn(this UILoginComponent sel)
		{
			UIHelper.Create(sel.ZoneScene(), UIType.UIDeleteAccount).Coroutine();
		}

		public static void OnBtn_Return(this UILoginComponent self)
		{
			if (self.ZhuCe.activeSelf)
			{
				self.ZhuCe.SetActive(false);
				self.ThirdLoginBg.SetActive(false);
				self.HideNode.SetActive(true);
			}
			else
			{
				self.ZhuCe.SetActive(true);
				self.YiJianDengLu.SetActive(false);
				self.IPhone.SetActive(false);
			}
		}

		public static void InitLoginType(this UILoginComponent self)
		{
            string lastloginType = PlayerPrefsHelp.GetString(PlayerPrefsHelp.LastLoginType);
			if (string.IsNullOrEmpty(lastloginType))
			{
				self.LoginType =  LoginTypeEnum.PhoneNumLogin.ToString();    //默认手机号一键登录
			}
			else
			{
				self.LoginType = lastloginType;
			}
			self.LoginType = GlobalHelp.IsBanHaoMode ? LoginTypeEnum.RegisterLogin.ToString() : self.LoginType;
			if (self.IOSReview && GlobalHelp.GetBigVersion() == 15)
			{
#if UNITY_IPHONE || UNITY_IOS
			self.LoginType =  LoginTypeEnum.RegisterLogin.ToString();
#endif
			}
            Log.ILog.Debug($"lastloginType: {lastloginType} { self.LoginType}");
			self.Account.GetComponent<InputField>().text = PlayerPrefsHelp.GetString(PlayerPrefsHelp.LastAccount(self.LoginType));
			self.Password.GetComponent<InputField>().text = PlayerPrefsHelp.GetString(PlayerPrefsHelp.LastPassword(self.LoginType));
        }

        //public const int RegisterLogin = 0;     //注册账号登录
        //public const int WeixLogin = 1;         //微信登录
        //public const int QQLogin = 2;           //QQ登录
        //public const int PhoneCodeLogin = 3;         //短信验证吗登录
        //public const int PhoneNumLogin = 4;        //手机号登录
        //public const int TapTap = 5;                //taptap登录
        public static void UpdateLoginType(this UILoginComponent self)
		{
			Log.ILog.Debug($"UpdateLoginType : {self.LoginType}");
			self.ThirdLoginBg.SetActive(true);
			self.YiJianDengLu.SetActive(int.Parse(self.LoginType) == LoginTypeEnum.PhoneNumLogin);
			string lastAccount = PlayerPrefsHelp.GetString(PlayerPrefsHelp.LastAccount(self.LoginType));

			bool uppos = GlobalHelp.IsBanHaoMode || LoginTypeEnum.RegisterLogin.ToString() == self.LoginType;
			if (self.IOSReview && GlobalHelp.GetBigVersion() == 15)
			{
#if UNITY_IPHONE || UNITY_IOS
			uppos = true;
#endif
			}

			self.BanHanNode.transform.localPosition = uppos ? new Vector3(0f, -20f, 0f) : new Vector3(0f,160f,0f);

			switch (int.Parse(self.LoginType))
			{
				case LoginTypeEnum.RegisterLogin:
					self.ZhuCe.SetActive(false);
					self.ThirdLoginBg.SetActive(false);
					self.Account.SetActive(true);
					self.Password.SetActive(true);
					self.HideNode.SetActive(true);
					break;
				case LoginTypeEnum.WeixLogin:
				case LoginTypeEnum.QQLogin:
					self.GetUserInfo(self.LoginType);
					break;
				case LoginTypeEnum.TapTap:
                    self.GetTapUserInfo(self.LoginType).Coroutine();
                    break;
				case LoginTypeEnum.PhoneCodeLogin:
					if (string.IsNullOrEmpty(lastAccount))
					{
						self.ZhuCe.SetActive(false);
						self.IPhone.SetActive(true);
						self.HideNode.SetActive(false);
						self.SendYanzheng.SetActive(true);
						self.YanZheng.SetActive(false);
                        GlobalHelp.OnButtonGetPhoneNum();
                    }
					else
					{
						self.OnCommitCodeHandler(lastAccount);
					}
					break;
				case LoginTypeEnum.PhoneNumLogin:
					//if (string.IsNullOrEmpty(lastAccount))
					//{
					//	//GlobalHelp.OnButtonGetPhoneNum();
					//}
					//else
					//{
					//	self.OnGetPhoneNum(lastAccount);
					//	self.OnButtonYiJianLogin();
					//}
					break;
			}
		}
		
		public static string GetPhoneZone(this UILoginComponent self)
		{
			return "86";
		}

		public static void OnButtonOtherLogin(this UILoginComponent self)
		{
			self.ThirdLoginBg.SetActive(true);
			self.YiJianDengLu.SetActive(false);
			self.ZhuCe.SetActive(true);
			self.HideNode.SetActive(false);
		}

        public static void OnGetPhoneNum(this UILoginComponent self, string phoneNum)
        {
            if (self.LoginType == LoginTypeEnum.PhoneNumLogin.ToString())
			{
				if (string.IsNullOrEmpty(phoneNum) || phoneNum.Length < 10)
				{
					FloatTipManager.Instance.ShowFloatTip("请选择其他登录方式！");
					return;
				}

                self.TextPhoneNumber.GetComponent<Text>().text = phoneNum;
                self.Account.GetComponent<InputField>().text = phoneNum;
                self.Password.GetComponent<InputField>().text = self.LoginType;
                self.ZhuCe.SetActive(false);
                self.YiJianDengLu.SetActive(false);
                self.ThirdLoginBg.SetActive(false);
                self.Account.SetActive(false);
                self.Password.SetActive(false);
            }

			if (self.LoginType == LoginTypeEnum.PhoneCodeLogin.ToString())
			{
				self.PhoneNumber.GetComponent<InputField>().text = phoneNum;
            }
        }

        public static void OnButtonYiJianLogin(this UILoginComponent self)
        {
            string phoneNum = self.TextPhoneNumber.GetComponent<Text>().text;
            if (string.IsNullOrEmpty(phoneNum))
            {
				GlobalHelp.OnButtonGetPhoneNum();
                return;
            }
        }

		public static void ShowNotice(this UILoginComponent self)
		{
			AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
			string noticeVersion = accountInfoComponent.NoticeVersion;
			if (noticeVersion != PlayerPrefsHelp.GetString(PlayerPrefsHelp.WJa_LastNotice))
			{
				PlayerPrefsHelp.SetString(PlayerPrefsHelp.WJa_LastNotice, noticeVersion);
				self.OnNotice();
			}
		}

		public static async ETTask RequestAllServer(this UILoginComponent self)
		{
			//请求服务器列表信息s
			try
			{
				int erroCode = ErrorCode.ERR_Success;
				long instanceid = self.InstanceId;
				if (GlobalHelp.IsOutNetMode)
				{
					erroCode = await LoginHelper.OnServerListAsyncRelease(self.DomainScene(), GlobalHelp.VersionMode);
				}
				else
				{
					erroCode = await LoginHelper.OnServerListAsyncDebug(self.DomainScene(), GlobalHelp.VersionMode);
				}
				if (instanceid != self.InstanceId)
				{
					return;
				}
				if (erroCode == ErrorCode.ERR_StopServer)
				{
					//FloatTipManager.Instance.ShowFloatTip(self.GetGongGaoText());
					PopupTipHelp.OpenPopupTip_3( self.ZoneScene(), "系统提示" , UILoginHelper.GetGongGaoText(), null).Coroutine();
                    return;
                }
				if (erroCode != ErrorCode.ERR_Success)
				{
					string msg = Application.internetReachability == NetworkReachability.NotReachable ? "请检查网络！: " : UILoginHelper.GetGongGaoText();
                    PopupTipHelp.OpenPopupTip_3(self.ZoneScene(), "系统提示", msg, null).Coroutine();
                    return;
				}
				self.ShowNotice();

				ServerItem serverItem = self.PlayerComponent.AllServerList[self.PlayerComponent.AllServerList.Count - 1];
				List<int> myids = new List<int>();
				int myserver = PlayerPrefsHelp.GetInt(PlayerPrefsHelp.MyServerID);
				myserver = ServerHelper.GetNewServerId(myserver);

				for (int i = 0; i < self.PlayerComponent.AllServerList.Count; i++)
				{
					if (self.PlayerComponent.AllServerList[i].ServerId == myserver)
					{
						serverItem = self.PlayerComponent.AllServerList[i];
						myids.Add(serverItem.ServerId);
						break;
					}
				}
				self.OnSelectServer(serverItem);
			}
			catch (Exception ex)
			{
				Log.Debug(ex.ToString());
				FloatTipManager.Instance.ShowFloatTip("请检查网络！: ");
			}
		}

		public static void OnRealNameButton(this UILoginComponent self)
		{
			UIHelper.Create( self.DomainScene(), UIType.UIRealName ).Coroutine();
		}

		public static void OnBtn_QQLogin(this UILoginComponent self)
		{
			if (!self.YinSiToggle2.GetComponent<Toggle>().isOn)
			{
				FloatTipManager.Instance.ShowFloatTip("请选勾选用户隐私协议！");
				return;
			}

			self.LoginType = LoginTypeEnum.QQLogin.ToString();
			self.UpdateLoginType();
		}

        public static void OnBtn_TapTap(this UILoginComponent self)
        {
            if (!self.YinSiToggle2.GetComponent<Toggle>().isOn)
            {
                FloatTipManager.Instance.ShowFloatTip("请选勾选用户隐私协议！");
                return;
            }
            self.LoginType = LoginTypeEnum.TapTap.ToString();
            self.UpdateLoginType();
        }

        public static void OnBtn_WeChatLogin(this UILoginComponent self)
		{
			if (!self.YinSiToggle2.GetComponent<Toggle>().isOn)
			{
				FloatTipManager.Instance.ShowFloatTip("请选勾选用户隐私协议！");
				return;
			}
			self.LoginType = LoginTypeEnum.WeixLogin.ToString();
			self.UpdateLoginType();
		}

		public static void OnBtn_ZhuCe(this UILoginComponent self)
		{
			if (!self.YinSiToggle2.GetComponent<Toggle>().isOn)
			{
				FloatTipManager.Instance.ShowFloatTip("请选勾选用户隐私协议！");
				return;
			}
			self.LoginType = LoginTypeEnum.RegisterLogin.ToString();
			UIHelper.Create(self.DomainScene(), UIType.UIRegister).Coroutine();
			self.UpdateLoginType();
		}

		public static void OnBtn_iPhone(this UILoginComponent self)
		{
			if (!self.YinSiToggle2.GetComponent<Toggle>().isOn)
			{
				FloatTipManager.Instance.ShowFloatTip("请选勾选用户隐私协议！");
				return;
			}
			self.LoginType = LoginTypeEnum.PhoneCodeLogin.ToString();

			self.UpdateLoginType();
		}

        public static void OnShareHandler(this UILoginComponent self, bool share)
		{ 
			
		}

		/// <summary>
		/// 各平台授权
		/// </summary>
		/// <param name="fenxiangtype"></param>
		public static void Authorize(this UILoginComponent self, string fenxiangtype)
		{
			switch (fenxiangtype)
			{
				case "1":
					self.ssdk.Authorize(cn.sharesdk.unity3d.PlatformType.WeChat);
					break;
				case "2":
					self.ssdk.Authorize(cn.sharesdk.unity3d.PlatformType.QQ);
					break;
			}
		}

        public static void OnGetTapUserInfo(this UILoginComponent self, string platinfo)
        {
            if (string.IsNullOrEmpty(platinfo))
            {
                return;
            }
            self.Account.GetComponent<InputField>().text = platinfo;
            self.Password.GetComponent<InputField>().text = self.LoginType;
            self.ZhuCe.SetActive(false);
            self.YiJianDengLu.SetActive(false);
            self.ThirdLoginBg.SetActive(false);
            self.Account.SetActive(false);
            self.Password.SetActive(false);
            self.HideNode.SetActive(true);
        }

        //QQ/WeiXin Login
        public static void OnGetUserInfo(this UILoginComponent self, string platinfo)
		{


			if (platinfo == "fail" || string.IsNullOrEmpty(platinfo) )
			{
				FloatTipManager.Instance.ShowFloatTip($"获取用户信息失败: {platinfo}");
				self.Authorize(self.LoginType);
				return;
			}
			string[] planids = platinfo.Split(';');  //openid, unionid
			self.Account.GetComponent<InputField>().text = planids[0];
			self.Password.GetComponent<InputField>().text = self.LoginType;
			self.ZhuCe.SetActive(false);
			self.YiJianDengLu.SetActive(false);
			self.ThirdLoginBg.SetActive(false);
			self.Account.SetActive(false);
			self.Password.SetActive(false);
			self.HideNode.SetActive(true);
		}

		public static void OnAuthorize(this UILoginComponent self, string platinfo)
		{
			if (platinfo == "fail" || string.IsNullOrEmpty(platinfo))
			{
                FloatTipManager.Instance.ShowFloatTip($"授权失败: {platinfo}");
                //self.Authorize(self.LoginType);
			}
			else
			{
				self.GetUserInfo(self.LoginType);
			}
			//string[] planids = platinfo.Split(';');  //openid, unionid
			//self.Account.GetComponent<InputField>().text = planids[0];
			//self.Password.GetComponent<InputField>().text = self.LoginType;
			//self.RequestLogin(planids[0], self.LoginType,self.LoginType).Coroutine();
			//Log.ILog.Debug($"Login: {planids[0]} {self.LoginType}");
		}

		public static void OnButtonCommitCode(this UILoginComponent self)
		{
			string phone = self.PhoneNumber.GetComponent<InputField>().text;
			string code =  self.TextPhoneCode.GetComponent<InputField>().text;
            GlobalHelp.OnButtonCommbitCode(self.OnCommitCodeHandler, phone, code);
		}


		public static void OnButtonGetCode(this UILoginComponent self)
		{
			string phoneNum = self.PhoneNumber.GetComponent<InputField>().text;
			GlobalHelp.OnButtonGetCode(phoneNum);
			self.TextYanzheng.GetComponent<Text>().text = $"已向手机号{phoneNum}发送短信验证";
			self.SendYanzheng.SetActive(false);
			self.YanZheng.SetActive(true);
		}

		public static void OnCommitCodeHandler(this UILoginComponent self, string phone)
		{
			if (string.IsNullOrEmpty(phone))
			{
				FloatTipManager.Instance.ShowFloatTipDi("验证失败！");
				return;
			}

			self.LoginType = LoginTypeEnum.PhoneCodeLogin.ToString();
			self.Account.GetComponent<InputField>().text = phone;
			self.Password.GetComponent<InputField>().text = self.LoginType;
			self.IPhone.SetActive(false);
			self.ThirdLoginBg.SetActive(false);
			self.ZhuCe.SetActive(false);
			self.HideNode.SetActive(true);
		}

		public static void  OnLogin(this UILoginComponent self)
		{
			if (!self.YinSiToggle.GetComponent<Toggle>().isOn)
			{
				FloatTipManager.Instance.ShowFloatTip("请选勾选用户隐私协议！");
				return;
			}

			string account = self.Account.GetComponent<InputField>().text;
			string password = self.Password.GetComponent<InputField>().text;
			Log.ILog.Debug($"Login: {account} {password}");
			self.RequestLogin(account, password, self.LoginType).Coroutine();
		}

		public static void OnReLogin(this UILoginComponent self)
		{
			string account = self.PlayerComponent.Account;
			string password = self.PlayerComponent.Password;
			Log.ILog.Debug($"Login: {account} {password}");
			self.RequestLogin(account, password, self.PlayerComponent.LoginType).Coroutine();
		}

		public static async ETTask RequestLogin(this UILoginComponent self, string account ,string password, string loginType)
		{
			if (self.ServerInfo == null)
			{
				self.RequestAllServer().Coroutine();
				return;
			}
			if (string.IsNullOrEmpty(account) || string.IsNullOrEmpty(password))
			{
				FloatTipManager.Instance.ShowFloatTip("请选择登录方式");
				return;
			}
			if (TimeHelper.ClientNow() - self.LastLoginTime < 3000)
			{
				return;
			}
			long loginErrorTime = 0;
			string errorTime = PlayerPrefsHelp.GetString(PlayerPrefsHelp.LoginErrorTime);
			if (!ComHelp.IfNull(errorTime))
			{
				loginErrorTime = long.Parse(errorTime);
			}
			if (TimeHelper.ServerNow() < loginErrorTime)
			{
				FloatTipManager.Instance.ShowFloatTip("稍后登录！");
				return;
			}

			self.Loading.SetActive(true);
			account = account.Replace(" ", "");
			password = password.Replace(" ", "");
			self.LastLoginTime = TimeHelper.ClientNow();
			self.PlayerComponent.ServerId = self.ServerInfo.ServerId;
			self.PlayerComponent.ServerIp = self.ServerInfo.ServerIp;
			self.PlayerComponent.Account = account;
			self.PlayerComponent.Password = password;
			self.PlayerComponent.LoginType = loginType;
			self.UIRotateComponent.GameObject.SetActive(true);
			self.UIRotateComponent.GetComponent<UIRotateComponent>().StartRotate(true);

			PlayerPrefsHelp.SetInt(PlayerPrefsHelp.MyServerID, self.ServerInfo.ServerId);
			PlayerPrefsHelp.SetOldServerIds(self.ServerInfo.ServerId);
			PlayerPrefsHelp.SetString(PlayerPrefsHelp.LastLoginType, self.LoginType);
			PlayerPrefsHelp.SetString(PlayerPrefsHelp.LastAccount(self.LoginType), account);
			PlayerPrefsHelp.SetString(PlayerPrefsHelp.LastPassword(self.LoginType), password);

			if (self.LoginType == "3" || self.LoginType == "4")
			{
				password = "3";
				loginType = "3";
			}
			int loginError = await LoginHelper.Login(
				self.DomainScene(),
				self.ServerInfo.ServerIp,
				account,
				password,
				false, 
				string.Empty,
                loginType);

			if (loginError != ErrorCode.ERR_Success)
			{
				self.LoginErrorNumber++;
				self.Loading?.SetActive(false);
				self.UIRotateComponent.GameObject.SetActive(false);
				self.UIRotateComponent.GetComponent<UIRotateComponent>().StartRotate(false);
			}
			if (self.LoginErrorNumber >= 50)
			{
				PlayerPrefsHelp.SetString(PlayerPrefsHelp.LoginErrorTime, (TimeHelper.ServerNow() + 10 * 60 * 1000).ToString());
			}
		}	

		public static void OnRegister(this UILoginComponent self)
		{
			Log.ILog.Debug("OnButtonOtherLogin");

			bool register = false;
			if (self.IOSReview && GlobalHelp.GetBigVersion() == 15)
			{
#if UNITY_IPHONE || UNITY_IOS
				register = true;
#endif
			}
			if (GlobalHelp.IsBanHaoMode)
			{
				register = true;
			}

			if (register)
			{
				UIHelper.Create(self.ZoneScene(), UIType.UIRegister).Coroutine();
			}
			else
			{
				GlobalHelp.PemoveAccount("1");
				GlobalHelp.PemoveAccount("2");
				PlayerPrefsHelp.SetString(PlayerPrefsHelp.LastLoginType, "");
				self.ThirdLoginBg.SetActive(true);
				self.ZhuCe.SetActive(true);
				self.YiJianDengLu.SetActive(false);
				self.Account.SetActive(false);
				self.Password.SetActive(false);
				self.HideNode.SetActive(false);
			}

			self.ResetPlayerPrefs(LoginTypeEnum.RegisterLogin.ToString());
			self.ResetPlayerPrefs(LoginTypeEnum.WeixLogin.ToString());
			self.ResetPlayerPrefs(LoginTypeEnum.QQLogin.ToString());
			self.ResetPlayerPrefs(LoginTypeEnum.PhoneCodeLogin.ToString());
			self.ResetPlayerPrefs(LoginTypeEnum.PhoneNumLogin.ToString());
			self.InitLoginType();
		}

		public static void ResetPlayerPrefs(this UILoginComponent self, string loingType)
		{
			PlayerPrefsHelp.SetString(PlayerPrefsHelp.LastAccount(loingType), "");
			PlayerPrefsHelp.SetString(PlayerPrefsHelp.LastPassword(loingType), "");
		}

		public static void OnSelectServer(this UILoginComponent self, ServerItem serverId)
		{
			self.ServerInfo = serverId;
			self.SelectServerName.GetComponent<Text>().text = serverId.ServerName;
		}

		public static void  OnNotice(this UILoginComponent self)
		{
			//if (self.PlayerComponent.NoticeStr == null)
			//{
			//	await LoginHelper.OnNoticeAsync(self.DomainScene(), self.ServerInfo.ServerIp);
			//}

			//if (self.PlayerComponent.NoticeStr != null)
			//{
			//	PopupTipHelp.OpenPopupTip(self.DomainScene(), GameSettingLanguge.LoadLocalization("游戏公告"), self.PlayerComponent.NoticeStr,
			//	null,
			//	null
			//	).Coroutine();
			//}
			UIHelper.Create( self.ZoneScene(), UIType.UINotice).Coroutine();
		}

		public static void OnSelectServerList(this UILoginComponent self)
		{
			Log.Info("点击显示服务器列表...");
			//LoginHelper.OnNoticeAsync(self.DomainScene(), ConstValue.LoginAddress).Coroutine();
			UIHelper.Create(self.DomainScene(), UIType.UISelectServer).Coroutine();
		}

	}
}
