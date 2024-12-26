using cn.sharesdk.unity3d;
using ET.EventType;
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
                bool taptap = false;
                int bigversion = GlobalHelp.GetBigVersion();
				int platform = GlobalHelp.GetPlatform();

#if UNITY_ANDROID
    //            if (bigversion < 18)
				//{
				//	string apk_Extension = (platform == 5 || platform == 6) ? "tiktok" : "taptap";
    //                apk_Extension  = apk_Extension  + ".apk";
				//    string apk_Url =	"http://verification.weijinggame.com/weijing/apk/weijing_" + apk_Extension;
    //                Application.OpenURL(apk_Url);	
				//	return;
				//}
#endif 

                self.InitSdk();
				Application.runInBackground = true;
                //关闭垂直同步
                libx.Assets.MAX_BUNDLES_PERFRAME = 32;
                // Screen.sleepTimeout = SleepTimeout.NeverSleep;
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
                ButtonHelp.AddListenerEx(self.ZhuCe.transform.Find("Btn_Apple").gameObject, () => { self.OnBtn_Apple(); });
                self.ZhuCe.transform.Find("Btn_ZhuCe").gameObject.SetActive(GlobalHelp.IsEditorMode);

				self.DeleteAccountBtn = rc.Get<GameObject>("DeleteAccountBtn");
				self.DeleteAccountBtn.SetActive(false);
				ButtonHelp.AddListenerEx(self.DeleteAccountBtn, () => { self.OnDeleteAccountBtn(); });

#if UNITY_ANDROID
                taptap = bigversion >= 15 && platform == 1;
#endif 
               
                self.AccountText = rc.Get<GameObject>("AccountText");

				self.AccountText.GetComponent<Text>().text = GlobalHelp.IsBanHaoMode ? "注册账号" : "切换账号";

				
				if (self.IOSReview && bigversion == 21)
				{
#if UNITY_IPHONE || UNITY_IOS
				self.DeleteAccountBtn.SetActive(true);
				self.AccountText.GetComponent<Text>().text = "注册账号";
#endif
				}

                self.ZhuCe.transform.Find("Btn_TapTap").gameObject.SetActive(taptap);
                self.ZhuCe.transform.Find("Btn_Apple").gameObject.SetActive(bigversion >= 21 && platform == 20001);

                Log.ILog.Debug($"self.IOSReview:  {self.IOSReview}");
                Log.ILog.Debug($"UILoginComponent  bigversion:{bigversion}   platform:{platform}");

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
				ButtonHelp.AddListenerEx(self.ButtonOtherLogin, self.OnButtonOtherLogin);
				ButtonHelp.AddListenerEx(self.ButtonYiJianLogin, self.OnButtonYiJianLogin);

				GameObject.Find("Global").GetComponent<SMSSDemo>().CommitCodeSucessHandler = (string text) => { self.OnCommitCodeHandler(text); };
				GameObject.Find("Global").GetComponent<Init>().OnGetPhoneNumHandler = (string text) => { self.OnGetPhoneNum(text); };

                self.RealNameButton = rc.Get<GameObject>("RealNameButton");
				self.RealNameButton.GetComponent<Button>().onClick.AddListener(self.OnRealNameButton);

				self.loginBtn = rc.Get<GameObject>("LoginBtn");
				ButtonHelp.AddListenerEx(self.loginBtn, self.OnLogin);

				self.ServerBtn = rc.Get<GameObject>("ServerBtn");
                ButtonHelp.AddListenerEx(self.ServerBtn, self.OnServerBtn);

                self.registerBtn = rc.Get<GameObject>("CreateAccountBtn");
				self.registerBtn.GetComponent<Button>().onClick.AddListener(self.OnRegister);
                self.registerBtn.SetActive(GlobalHelp.GetPlatform() != 5);


                self.UIAgeTip = rc.Get<GameObject>("UIAgeTip");
				self.UIAgeTip.SetActive(false);

                self.buttonAgeTip = rc.Get<GameObject>("buttonAgeTip");
				self.buttonAgeTip.GetComponent<Button>().onClick.AddListener(self.OnButton_ShowAgeTip);

                self.UIAgeTip.transform.Find("UIAgeTipClose").GetComponent<Button>().onClick.AddListener(self.OnButton_CloseAgeTip);
                self.UIAgeTip.transform.Find("ButtonClose").GetComponent<Button>().onClick.AddListener(self.OnButton_CloseAgeTip);

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
				if (platform == 6)
				{
					self.YinSiToggle2.GetComponent<Toggle>().isOn = false;
                    self.ZhuCe.transform.Find("Btn_iPhone").gameObject.SetActive(false);	
                }

                self.TextButton_2.GetComponent<Button>().onClick.AddListener(() => { self.YongHuXieYi.SetActive(true); });
				self.TextButton_1.GetComponent<Button>().onClick.AddListener(() => { self.YinSiXieYi.SetActive(true); });
				self.TextButton_2_2.GetComponent<Button>().onClick.AddListener(() => { self.YongHuXieYi.SetActive(true); });
				self.TextButton_2_1.GetComponent<Button>().onClick.AddListener(() => { self.YinSiXieYi.SetActive(true); });
				self.YongHuXieYiClose.GetComponent<Button>().onClick.AddListener(() => { self.YongHuXieYi.SetActive(false); });
				self.YinSiXieYiClose.GetComponent<Button>().onClick.AddListener(() => { self.YinSiXieYi.SetActive(false); });

				self.TextYinSi.SetActive(false);
                UILoginHelper.ShowTextList(self.TextYinSi, GlobalHelp.GetPlatform());

                self.LoginErrorNumber = 0;
				self.Loading = rc.Get<GameObject>("Loading");
				UI uirotate = self.AddChild<UI, string, GameObject>("RightPositionSet", rc.Get<GameObject>("Img_Loading"));
				uirotate.AddComponent<UIRotateComponent>();
				self.UIRotateComponent = uirotate;
				self.Loading.SetActive(false);
				
				GameSettingLanguge.Instance.InitRandomName().Coroutine();
				self.AccountInfoComponent = self.DomainScene().GetComponent<AccountInfoComponent>();

				if (bigversion >= 18)
				{
					EventType.LoginCheckRoot.Instance.ZoneScene = self.ZoneScene();
					EventSystem.Instance.PublishClass(EventType.LoginCheckRoot.Instance);
				}
				else
				{
					self.AccountInfoComponent.Root = IPHoneHelper.IsRoot() ? 1 : 0;
					self.AccountInfoComponent.Simulator = IPHoneHelper.IsSimulator() ? 1 : 0;	
                }
				
				//Game.Scene.GetComponent<SoundComponent>().PlayBgmSound(self.ZoneScene(), (int)SceneTypeEnum.LoginScene);

				self.InitLoginType();
                self.RequestAllServer().Coroutine();

                if ((bigversion >= 14 && bigversion < 16) && string.IsNullOrEmpty(PlayerPrefsHelp.GetString("UIYinSi0627")))
                {
                    UIHelper.Create(self.ZoneScene(), UIType.UIYinSi).Coroutine();
					PlayerPrefsHelp.SetString("UIYinSi0627", "1");
                }
#if UNITY_EDITOR
				//if (self.Password.GetComponent<InputField>().text == "6")
                {
                    self.AccountInfoComponent.Age_Type = 100;
                }

                //self.TestTapHttp_2().Coroutine();
#endif

                Log.ILog.Debug($"GetBigVersion.:{GlobalHelp.GetBigVersion()}  GetPlatform:{GlobalHelp.GetPlatform()} IsEditorMode:{GlobalHelp.IsEditorMode}");

                if (GlobalHelp.GetBigVersion() >= 21
					&& GlobalHelp.GetPlatform()  == 1
					&& !GlobalHelp.IsEditorMode)
				{
					Log.ILog.Debug("eventType.TapTapGetOAID.Instance");
					EventType.TapTapGetOAID.Instance.ZoneScene = self.ZoneScene();
					Game.EventSystem.PublishClass(EventType.TapTapGetOAID.Instance);
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

#if !UNITY_EDITOR
			GameObject sharesdk = GameObject.Find("Global");
			ShareSDK ssdk = sharesdk.GetComponent<ShareSDK>();
			ssdk.authHandler = (int reqID, ResponseState state, cn.sharesdk.unity3d.PlatformType type, Hashtable result) => 
			{
				self.OnAuthResultHandler(reqID, state, type, result); 
			};
			ssdk.showUserHandler = self.OnGetUserInfoResultHandler;
			self.ssdk = ssdk;
#endif


#if UNITY_ANDROID
            TapSDKHelper.Init();
#endif
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
					self.OnAuthorize($"sucess");
					break;
				case cn.sharesdk.unity3d.PlatformType.QQ:
					self.OnAuthorize($"sucess");
					break;
				default:
					break;
			}
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
			if (self.IOSReview && GlobalHelp.GetBigVersion() == 21)
			{
#if UNITY_IPHONE || UNITY_IOS
			self.LoginType =  LoginTypeEnum.RegisterLogin.ToString();
#endif
			}

            //抖音只有一个登录方式
            if (GlobalHelp.GetBigVersion() >= 17 && GlobalHelp.GetPlatform() == 5)
            {
#if UNITY_ANDROID
                self.LoginType = LoginTypeEnum.TikTok.ToString();
#endif

            }

            Log.ILog.Debug($"lastloginType: {lastloginType} { self.LoginType}");
			self.Account.GetComponent<InputField>().text = PlayerPrefsHelp.GetString(PlayerPrefsHelp.LastAccount(self.LoginType));
			self.Password.GetComponent<InputField>().text = PlayerPrefsHelp.GetString(PlayerPrefsHelp.LastPassword(self.LoginType));
			self.ServerBtn.SetActive( GMHelp.GmAccount.Contains(self.Account.GetComponent<InputField>().text) );
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

			self.AccountReversal = string.Empty;
			self.ThirdLoginBg.SetActive(true);
			self.YiJianDengLu.SetActive(int.Parse(self.LoginType) == LoginTypeEnum.PhoneNumLogin);
			string lastAccount = PlayerPrefsHelp.GetString(PlayerPrefsHelp.LastAccount(self.LoginType));

			bool uppos = GlobalHelp.IsBanHaoMode || LoginTypeEnum.RegisterLogin.ToString() == self.LoginType;
			if (self.IOSReview && GlobalHelp.GetBigVersion() == 21)
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

#if !UNITY_EDITOR
                    self.GetTapUserInfo(self.LoginType).Coroutine();
#endif

                    break;
				case LoginTypeEnum.TikTok:
                    self.ThirdLoginBg.SetActive(false);

#if !UNITY_EDITOR
					EventType.TikTokGetAccesstoken.Instance.ZoneScene = self.ZoneScene();
                    EventType.TikTokGetAccesstoken.Instance.AccesstokenHandler = (string text) => { self.OnRecvTikTokAccesstoken(text).Coroutine(); };
                    EventSystem.Instance.PublishClass(EventType.TikTokGetAccesstoken.Instance);
#endif

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
                case LoginTypeEnum.Apple:
                    EventType.AppleSignIn.Instance.ZoneScene = self.ZoneScene();
					EventType.AppleSignIn.Instance.Account = lastAccount;
                    EventType.AppleSignIn.Instance.AppleSignInHandler = (string text) => { self.OnGetAppleSignInfo(text); };
                    EventSystem.Instance.PublishClass(EventType.AppleSignIn.Instance);
                    break;
            }
		}
		
		public static string GetPhoneZone(this UILoginComponent self)
		{
			return "86";
		}

		public static void OnButtonOtherLogin(this UILoginComponent self)
		{
            if (!self.YinSiToggle2.GetComponent<Toggle>().isOn)
            {
                FloatTipManager.Instance.ShowFloatTip("请选勾选用户隐私协议！");
                return;
            }
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
            if (!self.YinSiToggle2.GetComponent<Toggle>().isOn)
            {
                FloatTipManager.Instance.ShowFloatTip("请选勾选用户隐私协议！");
                return;
            }
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
				string account = self.Account.GetComponent<InputField>().text;

                if (GlobalHelp.IsOutNetMode)
				{
					erroCode = await LoginHelper.OnServerListAsyncRelease(self.DomainScene(), GlobalHelp.VersionMode, account);
				}
				else
				{
					erroCode = await LoginHelper.OnServerListAsyncDebug(self.DomainScene(), GlobalHelp.VersionMode, account	);
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

				ServerItem serverItem = self.AccountInfoComponent.AllServerList[self.AccountInfoComponent.AllServerList.Count - 1];
				List<int> myids = new List<int>();
				int myserver = PlayerPrefsHelp.GetInt(PlayerPrefsHelp.MyServerID);
				myserver = ServerHelper.GetNewServerId(myserver);

				for (int i = 0; i < self.AccountInfoComponent.AllServerList.Count; i++)
				{
					if (self.AccountInfoComponent.AllServerList[i].ServerId == myserver)
					{
						serverItem = self.AccountInfoComponent.AllServerList[i];
						myids.Add(serverItem.ServerId);
						break;
					}
				}
				self.OnSelectServer(serverItem);
                self.UpdateLoginType();
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

		public static void OnBtn_Apple(this UILoginComponent self)
		{
            if (!self.YinSiToggle2.GetComponent<Toggle>().isOn)
            {
                FloatTipManager.Instance.ShowFloatTip("请选勾选用户隐私协议！");
                return;
            }
            PlayerPrefs.DeleteKey("AppleUserId");
            self.LoginType = LoginTypeEnum.Apple.ToString();
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

			self.AccountReversal = StringBuilderHelper.Encrypt(platinfo);
            self.Account.GetComponent<InputField>().text = platinfo;
            self.Password.GetComponent<InputField>().text = self.LoginType;
            self.ZhuCe.SetActive(false);
            self.YiJianDengLu.SetActive(false);
            self.ThirdLoginBg.SetActive(false);
            self.Account.SetActive(false);
            self.Password.SetActive(false);
            self.HideNode.SetActive(true);
        }

		public static void OnGetAppleSignInfo(this UILoginComponent self,  string appuserinfo)
		{
			if (string.IsNullOrEmpty(appuserinfo))
			{
                FloatTipManager.Instance.ShowFloatTip($"获取用户信息失败， 请选择其他登陆方式！");
                return;
            }

            self.LoginType = LoginTypeEnum.Apple.ToString();
            self.Account.GetComponent<InputField>().text = appuserinfo;
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
            if (self.LoginType != "1" && self.LoginType != "2")
			{
                if (planids[0].Contains("wx"))
                {
                    self.LoginType = "1";
                }
                if (planids[0].Contains("qq"))
				{
					self.LoginType = "2";
                }
            }
			self.AccountReversal = StringBuilderHelper.Encrypt(planids[0]);	
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
            string code = self.TextPhoneCode.GetComponent<InputField>().text;
            if (GlobalHelp.IsEditorMode)
			{
#if UNITY_EDITOR
				self.OnCommitCodeHandler(phone);
                return;
#endif
			}

            GlobalHelp.OnButtonCommbitCode(self.OnCommitCodeHandler, phone, code);
		}

		public static void OnButtonGetCode(this UILoginComponent self)
		{ 
			string phoneNum = self.PhoneNumber.GetComponent<InputField>().text;
			if (string.IsNullOrEmpty(phoneNum)|| phoneNum.Length < 3)
			{
				return;
			}

            string head = phoneNum.Substring(0, 3);
            if (GMHelp.IllegalPhone.Contains(head))
			{
				FloatTipManager.Instance.ShowFloatTip("此号码不支持注册,请使用其他方式登录游戏！");
				return;
			}

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

		public static void OnServerBtn(this UILoginComponent self)
		{
			UIHelper.Create( self.ZoneScene(), UIType.UIServerShow ).Coroutine();
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
			self.RequestLogin(account, password, self.LoginType);
		}


		/// <summary>
		/// 实名认证返回
		/// </summary>
		/// <param name="self"></param>
		public static void OnReLogin(this UILoginComponent self)
		{
			string account = self.AccountInfoComponent.Account;
			string password = self.AccountInfoComponent.Password;
			Log.ILog.Debug($"Login: {account} {password}");
			self.RequestLogin(account, password, self.AccountInfoComponent.LoginType);
		}

		public static  void RequestLogin(this UILoginComponent self, string account ,string password, string loginType)
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
            long serverTime = TimeHelper.ServerNow();
            string errorTime = PlayerPrefsHelp.GetString(PlayerPrefsHelp.LoginErrorTime);
			if (!ComHelp.IfNull(errorTime))
			{
				loginErrorTime = long.Parse(errorTime);
			}
			if (serverTime < loginErrorTime)
			{
				FloatTipManager.Instance.ShowFloatTip("稍后登录！");
				return;
			}
			if (GlobalHelp.GetPlatform() == 5 && self.AccountInfoComponent.Age_Type < 0)
			{
                FloatTipManager.Instance.ShowFloatTip("稍后登录！");
                return;
            }


			if (!string.IsNullOrEmpty(self.AccountReversal))
			{
				string accountReversal = StringBuilderHelper.Decrypt(self.AccountReversal);
                if (! accountReversal.Equals(account))
				{
					self.LoginErrorNumber++;
					if (self.LoginErrorNumber >= 10)
					{
						PlayerPrefsHelp.SetString( PlayerPrefsHelp.LoginErrorTime,  (serverTime + TimeHelper.Hour).ToString() );
					}
                    if (self.LoginErrorNumber >= 30)
                    {
                        PlayerPrefsHelp.SetString(PlayerPrefsHelp.LoginErrorTime, (serverTime + TimeHelper.OneDay).ToString());
                    }
                    FloatTipManager.Instance.ShowFloatTip("数据异常！");
                    return;
                }
            }
           
            self.Loading.SetActive(true);
			account = account.Replace(" ", "");
			password = password.Replace(" ", "");
			self.LastLoginTime = TimeHelper.ClientNow();
			self.AccountInfoComponent.ServerId = self.ServerInfo.ServerId;
			self.AccountInfoComponent.ServerIp = self.ServerInfo.ServerIp;
			self.AccountInfoComponent.ServerName = self.ServerInfo.ServerName;
			self.AccountInfoComponent.Account = account;
			self.AccountInfoComponent.Password = password;
			self.AccountInfoComponent.LoginType = loginType;

			if (GlobalHelp.GetPlatform() != 5)
			{
                self.AccountInfoComponent.DeviceID = SystemInfo.deviceUniqueIdentifier;
            }
			Log.ILog.Debug($"DeviceID:{self.AccountInfoComponent.DeviceID}");

            self.UIRotateComponent.GameObject.SetActive(true);
			self.UIRotateComponent.GetComponent<UIRotateComponent>().StartRotate(true);

			PlayerPrefsHelp.SetInt(PlayerPrefsHelp.MyServerID, self.ServerInfo.ServerId);
			PlayerPrefsHelp.SetOldServerIds(self.ServerInfo.ServerId);
			PlayerPrefsHelp.SetString(PlayerPrefsHelp.LastLoginType, self.LoginType);
			PlayerPrefsHelp.SetString(PlayerPrefsHelp.LastAccount(self.LoginType), account);
			PlayerPrefsHelp.SetString(PlayerPrefsHelp.LastPassword(self.LoginType), password);

			//if (GlobalHelp.GetBigVersion() >= 20
			//	&& GlobalHelp.GetPlatform() != 5
			//	&& !GlobalHelp.IsEditorMode)
			//{
   //             EventType.TapTapAuther.Instance.ZoneScene = self.ZoneScene();
   //             EventType.TapTapAuther.Instance.Account = account;
   //             Game.EventSystem.PublishClass(EventType.TapTapAuther.Instance);
   //         }
			//else
            {
                if (self.LoginType == "3" || self.LoginType == "4")
                {
                    password = "3";
                    loginType = "3";
                }
                self.RequestLoginV20(account, password, loginType).Coroutine();
            }
        }

		public static void OnGetDeviceOAID(this UILoginComponent self, string oaid)
		{
			self.AccountInfoComponent.OAID = oaid;
			Log.ILog.Debug($"OnGetDeviceOAID:{oaid}");
        }

        public static async ETTask TestTapHttp_2(this UILoginComponent self)
        {
            string taphost = !GlobalHelp.IsOutNetMode ?  "127.0.0.1": "39.96.194.143";
            int tapport = !GlobalHelp.IsOutNetMode ? ComHelp.TapHttpIneer : ComHelp.TapHttpOuter;
			string callback = $"http://{taphost}:{tapport}/wjtaprepcallback";

            string url = $"http://{taphost}:{tapport}/wjtaprepjiance?OAID=58bcedf9-fdff-4333-ce8f-ffeedfef4514&adset_id=&adset_net=&callback=https%3A%2F%2Ftap-op.tapapis.cn%2Frep%2Fbe%2Fv1%2Fopen%2Fconvert_track%2Factive%3Fdevice_code%3D1%26tap_project_id%3D271100%26tap_track_id%3D527087767763576587&conversion_type=&creative_id=&device=1&device_brand=&device_model=&game_id=271100&game_name=&idfa=&ip=&sub_channel=rep&tap_project_id=271100&tap_track_id=527087767763576587&time=1723698391";
            //string url = $"http://{taphost}:{tapport}/wjtaprepjiance?idfa=asedfstUfe&time=1605432321&ip=10.33.25.54&anid={SystemInfo.deviceUniqueIdentifier}&game_id=13&game_name=游戏名称&adset_id=132214&adset_net=计划名称&device_brand=苹果&device_model=iPhone3,2&creative_id=131232&conversion_type=TapRep&device=1&OAID=&callback={callback}&tap_track_id=xYTKx4rSFFWx&tap_project_id=13";
            Log.ILog.Debug($"TestTapHttp_2 url : {url}");
            string routerInfo = await HttpClientHelper.Get(url);
            Log.ILog.Debug($"TestTapHttp_2 reponse: {routerInfo}");
            //formjson
        }

        public static async ETTask RequestLoginV20(this UILoginComponent self, string account, string password, string loginType)
		{
           
            long serverTime = TimeHelper.ServerNow();
            int loginError = await LoginHelper.Login(
                self.DomainScene(),
                self.ServerInfo.ServerIp,
                account,
                password,
                false,
                string.Empty,
                loginType);

			if (loginError == ErrorCode.ERR_Success)
			{

#if UNITY_ANDROID
                //登陆成功 ，会自动注册账号
                AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();

				Log.ILog.Debug($"GetPlatform:  {GlobalHelp.GetPlatform()}");
                Log.ILog.Debug($"TaprepRequest:  {accountInfoComponent.TaprepRequest}");
                Log.ILog.Debug($"TapRepRegister:  {PlayerPrefsHelp.GetString(PlayerPrefsHelp.TapRepRegister)}");

                if (GlobalHelp.GetPlatform() == 1 && !string.IsNullOrEmpty(accountInfoComponent.TaprepRequest) && string.IsNullOrEmpty(PlayerPrefsHelp.GetString(PlayerPrefsHelp.TapRepRegister)))
				{
					PlayerPrefsHelp.SetString(PlayerPrefsHelp.TapRepRegister, TimeHelper.ClientNow() + "");
					//tapreq 激活
					await TapSDKHelper.TapReqEvent(accountInfoComponent.TaprepRequest, 1, string.Empty);

                    //tapreq 注册
                    await TapSDKHelper.TapReqEvent(accountInfoComponent.TaprepRequest, 2, string.Empty);
                }
#endif
			}
			else
			{
                self.LoginErrorNumber++;
                self.Loading?.SetActive(false);
                self.UIRotateComponent.GameObject.SetActive(false);
                self.UIRotateComponent.GetComponent<UIRotateComponent>().StartRotate(false);

                if (self.LoginErrorNumber >= 50)
                {
                    PlayerPrefsHelp.SetString(PlayerPrefsHelp.LoginErrorTime, (serverTime + TimeHelper.Minute * 10).ToString());
                }
            }
        }

        public static void OnButton_ShowAgeTip(this UILoginComponent self)
		{
            self.UIAgeTip.SetActive(true);
        }

		public static void OnButton_CloseAgeTip(this UILoginComponent self)
		{
			self.UIAgeTip.SetActive(false);
		}

        public static void OnRegister(this UILoginComponent self)
		{
			Log.ILog.Debug("OnButtonOtherLogin");
			self.AccountReversal = string.Empty;
			bool register = false;
			if (self.IOSReview && GlobalHelp.GetBigVersion() == 21)
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
            self.ResetPlayerPrefs(LoginTypeEnum.TapTap.ToString());
            self.ResetPlayerPrefs(LoginTypeEnum.TikTok.ToString());
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
