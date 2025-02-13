using cn.sharesdk.unity3d;
using System;
using System.Collections;
using System.Collections.Generic;

using System.Threading;
using UnityEngine;
using TapTap.Bootstrap;
using TapTap.Common;
using TapTap.TapDB;
using TapTap.AntiAddiction;
using TapTap.AntiAddiction.Model;
using cn.SMSSDK.Unity;
using TapTap.Login;
using AppleAuth;
using AppleAuth.Native;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using System.Text;
using AppleAuth.Extensions;
using System.Net;



#if UNITY_IPHONE && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

namespace ET
{
    public struct FenXiangContent
	{
		public string FenXiang_Title;          //分享标题
		public string FenXiang_Text;           //分享文本
		public string FenXiang_ImageUrl;       //分享图片路径
		public string FenXiang_ClickUrl;       //分享点击链接
		public int Fenxiangtype; 
	}

	// 1 mono模式 2 ILRuntime模式 3 mono热重载模式
	public enum CodeMode
	{
		Mono = 1,
		ILRuntime = 2,
		Reload = 3,
		HuaTuo = 4,
	}

	public class Init : MonoBehaviour
	{
		public bool Development;
		public bool EditorMode;
		public bool OueNetMode;
		public int BigVersion = 21;      
		//17部分包含抖音sdk能力 18(模拟器检测) 19 3D视角  20 Tap实名  21tap设备Id             22客户端寻路
		public int BigVersionIOS = 21;
        //17部分包含抖音sdk能力 18(模拟器检测) 19 3D视角  20 Tap实名  21tap设备Id Apple登陆   22客户端寻路
        public GameObject Updater;
		public Action<int, bool> OnShareHandler;
		public Action<string> OnGetPhoneNumHandler;
        public Action<string> OnGetPermissionsHandler;
        public Action<bool> OnApplicationFocusHandler;
		public Action<string> OnRiskControlInfoHandler;
        public Action<string> OnTikTokAccesstokenHandler;
		public Action<string> OnGetDeviceOAIDHandler;
        public Action OnApplicationQuitHandler;

		public Action<int> OnGetKeyHandler;
		public Action OnGetMouseButtonDown_1;
		public Action OnGetMouseButtonDown_0;

		public ShareSDK ssdk;
		public MobSDK mobsdk;

		public string WXAppID = "wx638f7f0efe37a825";           //危境  74俄罗斯消除
		public string WXAppSecret = "c45e594ab681035a1cae6ab166f64a20";

		public string QQAppID = "1105893765";
		//apk sign 1  b119680ac96937de65f5c989ce485fb3   user_weijing2.keystore	//勇士/圣光
		//apk sign 2  3a0a616cdbf889b3565ba81fca3bed49   user.keystore			//危境

		public AndroidJavaClass jc;
		public AndroidJavaObject jo;

		//是java里的类，一些静态方法可以直接通过这个调用。
		//androidjavaobject 调用的话，会生成一个对象，就和java里new一个对象一样，可以通过对象去调用里面的方法以及属性。
		public AndroidJavaClass javaClass;
		public AndroidJavaObject javaActive;

		public AppleAuthManager appleAuthManager;
        public Action<string> AppleSignInHandler;

        //"com.mafeng.alinewsdk.AliSDKActivity"是2018.11.01日更新的版本 对应安卓工程中的alinewsdk Module
        //而"com.mafeng.aliopensdk.AliSDKActivity"是之前的版本 对应安卓工程中的aliopensdk Module
        public string javaClassStr = "com.example.alinewsdk.AliSDKActivity";  //"com.mafeng.aliopensdk.AliSDKActivity";
		public string javaActiveStr = "currentActivity";

		public CodeMode CodeMode = CodeMode.Mono;
		public VersionMode VersionMode = VersionMode.Alpha;

        [HideInInspector]
        public int Platform = 0;

		[HideInInspector]
		public bool HotUpdateComplete = false;

		public string Apk_Extension = string.Empty;

		public int IsRoot;
        public int IsEmulator;

#if UNITY_IPHONE && !UNITY_EDITOR
     [DllImport("__Internal")]
     private static extern void CheckIphoneYueyu( string str );

	  [DllImport("__Internal")]
     private static extern void FuncTapTapShare( string str );

	 //[DllImport("__Internal")]
  //   private static extern string GetPhoneNum( string str );
#endif

        private void Awake()
		{
			//#if ENABLE_IL2CPP
			//			this.CodeMode = CodeMode.ILRuntime;
			//#endif
            System.AppDomain.CurrentDomain.UnhandledException += (sender, e) =>
			{
				Log.Error(e.ExceptionObject.ToString());
			};

			SynchronizationContext.SetSynchronizationContext(ThreadSynchronizationContext.Instance);

			DontDestroyOnLoad(gameObject);

			LitJson.UnityTypeBindings.Register();

            ETTask.ExceptionHandler -= Log.Error;
			ETTask.ExceptionHandler += Log.Error;
			Application.logMessageReceived -= OnLogMessageReceived;
			Application.logMessageReceived += OnLogMessageReceived;

			Log.ILog = new UnityLogger();

			Options.Instance = new Options();

			CodeLoader.Instance.CodeMode = this.CodeMode;
			Options.Instance.Develop =  OueNetMode ? 0 : 1;
			Options.Instance.LogLevel = OueNetMode ? 6 : 1;

            Log.ILog.Debug("unity111  Awake");
            Log.ILog.Debug($"Application.persistentDataPath: {Application.persistentDataPath}");
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            ///平台Id定义 不得更改
            ///0 默认 taptap1  QQ2 platform3 小说推广 platform4备用  TikTok5  TikTokMuBao6(抖音母包) QuDao100渠道母包(100以上为渠道包) ios20001
#if TapTap1
            Log.ILog.Debug("unity111  TapTap1=true");
			this.Platform = 1;
			this.Apk_Extension = "taptap";
#elif QQ2
            Log.ILog.Debug("unity111  QQ2=true");
			this.Platform = 2;
			this.Apk_Extension = "taptap";
#elif Platform3
			Log.ILog.Debug("unity111  Platform3=true");
			this.Platform = 3;
			this.Apk_Extension = "taptap";
#elif Platform4
			Log.ILog.Debug("unity111  Platform4=true");
			this.Platform = 4;
			this.Apk_Extension = "taptap";
#elif TikTok5             //抖音                   
			Log.ILog.Debug("unity111  TikTok5=true");
			this.Platform = 5;
			this.Apk_Extension = "tiktok";
#elif TikTokMuBao6            //抖音母包                
			Log.ILog.Debug("unity111  TikTokMuBao6=true");
			this.Platform = 6;
			this.Apk_Extension = "tiktok";
#elif QuDao
			Log.ILog.Debug("unity111  TikTokMuBao6=true");
			this.Platform = 100;
			this.Apk_Extension = "qudao";
#else
			this.Apk_Extension = "taptap";
#endif


#if UNITY_IPHONE || UNITY_IOS
			this.Platform = 20001;
#endif

            Log.ILog.Debug($"unity111  this.Platform = {this.Platform}");

            if (AppleAuthManager.IsCurrentPlatformSupported)
			{
				// Creates a default JSON deserializer, to transform JSON Native responses to C# instances
				var deserializer = new PayloadDeserializer();
				// Creates an Apple Authentication manager with the deserializer
				this.appleAuthManager = new AppleAuthManager(deserializer);
				this.InitializeLoginMenu();
                Log.ILog.Debug("AppleAuthManager.IsCurrentPlatformSupported true");
			}
			else
			{
                Log.ILog.Debug("AppleAuthManager.IsCurrentPlatformSupported false");
            }


#if UNITY_ANDROID && !UNITY_EDITOR
		jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
		jo.Call("CallNative", InitHelper.InitKey);

		if(this.Platform!=5)
		{
			jo.Call("WechatInit", WXAppID);
		}
#elif UNITY_IPHONE && !UNITY_EDITOR
		 CheckIphoneYueyu( string.Empty ); 
#endif

            BuglyInit.PlatForm = this.Platform;

            GameObject sharesdk = GameObject.Find("Global");
			ssdk = sharesdk.GetComponent<ShareSDK>();
			//ssdk.authHandler = OnAuthResultHandler;
			//ssdk.showUserHandler = OnGetUserInfoResultHandler;
			ssdk.shareHandler = OnShareResultHandler;
			ssdk.getFriendsHandler = OnGetFriendsResultHandler;
			ssdk.followFriendHandler = OnFollowFriendResultHandler;
			mobsdk = gameObject.GetComponent<MobSDK>();
        }

		public void OnLogMessageReceived(string condition, string stackTrace, LogType type)
		{
			if (type == LogType.Error || type == LogType.Assert
					|| type == LogType.Exception)
			{
				UnityEngine.Debug.LogError(stackTrace);
			}
		}

        public void TikTokInit()
        {
			if (this.Platform != 5)
			{
				return;
			}

#if UNITY_ANDROID && !UNITY_EDITOR
			jo.Call(InitHelper.TikTokInitFunc, InitHelper.TikTokInitParam);
#else

#endif
        }

        public void TikTokLogin()
		{
			if (this.Platform != 5)
			{
				return; 
            }

#if UNITY_ANDROID && !UNITY_EDITOR
			jo.Call(InitHelper.TikTokLoginFunc, InitHelper.InitKey );
#else
            this.OnRecvTikTokAccesstoken("Q2dZSUFTQUhLQUVTcndJS3JBSWs5VGQ4aFFGZXlhODJocEZKQUVYNU42RFN2QlVnREJNS2ZoVFRwYVNuQzNVblNkQkhFN1VGbWh3WTBZeG54Y3FMblBvWFoxemNhSXFkM1owSjMrc0N0clNnMG5pVGJZa3dULzF1eFdSVmFHdUZ2QTRVRW1OZFUvak5Bb0ZXbkFIb3F5MlVZR00vU1FxVFdOWDJxWEhtRUswODRpTkNUNnVlQ0Yrdno2OHZ0alhoVWNNd2FDSWJocUFkRlNWZVluTnVZSUNoMlZFdkM0TjZpREcwVCtaZUREQWR2N2dCbSt1SWRFR25CdHNJd0xveVJ2T0FOR3BKbmpMVmQwV1lzcHZZU3NKOTF4QmVNSE8wbGNzUTc2YlltTjZESVBZdCt3dStrcXlEeExVcldIS2JhWjMxSXR2VjYyVVdYTWdwTDV2dHZKVzk1NzU1cGE4SXovc1VZWE50VXVSOXlNV01waHBUNk1JTThlWmU2d2lqekllUFprMHo0dS96ZkxqeEExYnZCeVgyQzBGelZabnFFYzRhQUE9PQ==");
#endif
        }

        /// <summary>
        /// Tap
        /// </summary>
        public void TapTapShare(string OrderInfo)
        {
			string[] infos = OrderInfo.Split('&');
            Log.ILog.Debug("TapTapShare: " + OrderInfo);

#if UNITY_EDITOR
			this.OnShareHandler(8, true);
#endif

#if UNITY_ANDROID && !UNITY_EDITOR
			jo.Call("TapTapShare", OrderInfo);
#endif

#if UNITY_IPHONE && !UNITY_EDITOR
			FuncTapTapShare(OrderInfo);
#endif
        }

        public void OnTapTapShareHandler(string resultCodestr)
		{
			Log.ILog.Debug($"OnTapTapShareHandler:  {resultCodestr}");
			int resultCode = int.Parse(resultCodestr);	
            // 0 正常分享  -1未安装 -2不支持
            if (resultCode == 0)
			{
                this.OnShareHandler(8, true);
            }
			else
			{
                this.OnShareHandler(8, false);
            }
        }
        public void GetDeviceOAID()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
            if (this.Platform == 1)
            {
                jo.Call("GetDeviceOAID", InitHelper.InitKey);
            }
#else
            this.OnGetDeviceOAID(SystemInfo.deviceUniqueIdentifier);
#endif
        }


        public void OnGetDeviceOAID(string oaid)
		{
            Log.ILog.Debug($"OnGetDeviceOAID: {oaid}");
            this.OnGetDeviceOAIDHandler?.Invoke(oaid);
        }

        /// <summary>
        ///  由接入方实现，通过游戏服务端向抖音游戏服务端校验用户登录态、获取sdk_open_id，参考服务端接入登录验证部分
        /// </summary>
        /// <param name="access_token"></param>
        public void OnRecvTikTokAccesstoken(string access_token)
        {
            Log.ILog.Debug($"OnRecvTikTokAccesstoken: {access_token}");
            this.OnTikTokAccesstokenHandler?.Invoke(access_token);
        }

		public void TikTokRiskControlInfo()
		{
            if (this.Platform != 5)
            {
                return;
            }
#if UNITY_ANDROID && !UNITY_EDITOR
			jo.Call("GetTikTokRiskControlInfo", "weijing" );
#else
			string riskinfo = "{\"app_name\":\"危境\",\"bd_did\":\"ZCU2UQLICRZL35L3735CUZQTCNQZRY3TTVN74WYXU6CEENNLPWBQ01\",\"iid\":\"1154923994817115\",\"os_version\":\"10\",\"app_package\":\"com.example.weijinggame.bytedance.gamecenter\",\"version_code\":\"2\",\"device_platform\":\"android\",\"device_type\":\"POT-AL00a\",\"device_brand\":\"HUAWEI\",\"channel\":\"osdk\",\"update_version_code\":\"22300\",\"sdk_version\":\"2.2.3.0\",\"aid\":\"554726\",\"language\":\"zh\",\"open_udid\":\"077ce378500da5ca\",\"resolution\":\"1506*720\",\"os\":\"android\",\"os_api\":29,\"manifest_version_code\":\"2\",\"dpi\":320,\"screen_width\":1506,\"version_name\":\"2.0.3\",\"uuid\":\"\",\"user_agent\":\"Dalvik\\/2.1.0 (Linux; U; Android 10; POT-AL00a Build\\/HUAWEIPOT-AL00a)\",\"is_cloud_env\":false} ";
            this.OnRiskControlInfoHandler?.Invoke(riskinfo);
#endif
        }
        public void OnRecvRiskControlInfo(string riskcontrol)
        {
            this.OnRiskControlInfoHandler?.Invoke(riskcontrol);
        }

        //TikTokPay(String cpOrderId, int amountInCent, String productId, String productName, String sdkParam)
        public void TikTokPay(string cpOrderId, int amountInCent, string productId, string productName, string sdkParam)
		{
            if (this.Platform != 5)
            {
                return;
            }
#if UNITY_ANDROID && !UNITY_EDITOR
			jo.Call(InitHelper.TikTokPayFunc, cpOrderId,amountInCent,  productId,productName, sdkParam);
#else
#endif
        }

        public void TikTokShareImage(string strings_1, string strings_2)
        {
            if (this.Platform != 5)
            {
                return;
            }

#if UNITY_ANDROID && !UNITY_EDITOR
			jo.Call("TikTokShareImage", strings_1, strings_2);
#else
#endif
        }

        public async ETTask<string> TapTapLogin()
        {
            await ETTask.CompletedTask;
            Log.ILog.Debug("TapTapLogin111");
#if UNITY_ANDROID
            var currentUser = TDSUser.GetCurrent();
            if (null == currentUser)
            {
                Log.ILog.Debug("TapTap 当前未登录");
                // 开始登录
            }
            else
            {
                Log.ILog.Debug("TapTap 已登录");
                // 进入游戏
            }
            try
            {
                Log.ILog.Debug("TapTapLogin222");
                // 在 iOS、Android 系统下会唤起 TapTap 客户端或以 WebView 方式进行登录
                // 在 Windows、macOS 系统下显示二维码（默认）和跳转链接（需配置）
                var tdsUser = await TDSUser.LoginWithTapTap();
                Log.ILog.Debug($"TapTapLogin333 success1:{tdsUser}");
                // 获取 TDSUser 属性
                var objectId = tdsUser.ObjectId;     // 用户唯一标识
                var nickname = tdsUser["nickname"];  // 昵称
                var avatar = tdsUser["avatar"];      // 头像
                Log.ILog.Debug($"TapTapLogin444 success2:{objectId}");
                return objectId;
            }
            catch (Exception e)
            {
                Log.ILog.Debug("登录异常");
                if (e is TapException tapError)  // using TapTap.Common
                {
                    Log.ILog.Debug($"encounter exception:{tapError.code} message:{tapError.message}");
                    if (tapError.code == (int)TapErrorCode.ERROR_CODE_BIND_CANCEL) // 取消登录
                    {
                        Log.ILog.Debug("登录取消");
                    }
                }
            }
            return string.Empty;
#else
            return string.Empty;
#endif
        }

        public async ETTask TapTapLogoutAsync()
        {
			await ETTask.CompletedTask;
#if UNITY_ANDROID
            await TDSUser.Logout();
#endif
		}


        /// <summary>
        /// 调用位置开发者可以自己指定，只需在使用SDK功能之前调用即可，
        /// 强烈建议开发者在终端用户点击应用隐私协议弹窗同意按钮后调用。
        /// </summary>
        public void SetIsPermissionGranted()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			//传入的第一个参数为Boolean类型的，true 代表同意授权、false代表不同意授权
			//该接口必须接入，否则可能造成无法使用MobTech各SDK提供的相关服务。
			mobsdk.submitPolicyGrantResult(true);
			jo.Call("SetIsPermissionGranted", QQAppID);

			jo.Call("QuDaoRequestPermissions");
#else
            this.OnGetPermissionsHandler?.Invoke("1_1");
#endif
        }

        public void onRequestPermissionsResult(string permissons)
        {
			this.OnGetPermissionsHandler?.Invoke(permissons);
        }

        public void OpenBuglyAgent(string userId)
		{
			if (this.Platform == 5)
			{
				return;
			}

#if UNITY_ANDROID
			Log.ILog.Info("OpenBuglyAgent: " + userId);
			// 开启SDK的日志打印，发布版本请务必关闭
			BuglyAgent.ConfigDebugMode(false);
			// 注册日志回调，替换使用 'Application.RegisterLogCallback(Application.LogCallback)'注册日志回调的方式
			// BuglyAgent.RegisterLogCallback (CallbackDelegate.Instance.OnApplicationLogCallbackHandler);
			BuglyAgent.ConfigDefault(null, Application.version, userId.ToString(), 0);
#if UNITY_IPHONE || UNITY_IOS
			BuglyAgent.InitWithAppId (BuglyInit.BuglyAppID);
#elif UNITY_ANDROID
			BuglyAgent.InitWithAppId(BuglyInit.BuglyAppID);
#endif
			// 如果你确认已在对应的iOS工程或Android工程中初始化SDK，那么在脚本中只需启动C#异常捕获上报功能即可
			BuglyAgent.EnableExceptionHandler();

#endif
		}

		//private void Start()
		//{
		//	CodeLoader.Instance.Start();
		//}

		public void OnHotUpdateComplete()
		{
			HotUpdateComplete = true;
			GameObject.Find("Global/UI/Hidden/UIYinSi").SetActive(false);
            CodeLoader.Instance.Start();
		}

		private void Update()
		{
            if (this.appleAuthManager != null)
            {
                this.appleAuthManager.Update();
            }

            if (HotUpdateComplete)
			{
                CodeLoader.Instance.Update();
                this.CheckMouseInput();
            }
		}

		private void CheckMouseInput()
		{
			if (InputHelper.GetKey(257))
			{
				this.OnGetKeyHandler?.Invoke(257);
			}
			if (InputHelper.GetKey(119))
			{
				this.OnGetKeyHandler?.Invoke(119);
			}
			if (InputHelper.GetKey(97))
			{
				this.OnGetKeyHandler?.Invoke(97);
			}
			if (InputHelper.GetKey(115))
			{
				this.OnGetKeyHandler?.Invoke(115);
			}
			if (InputHelper.GetKey(100))
			{
				this.OnGetKeyHandler?.Invoke(100);
			}
			if (InputHelper.GetMouseButtonDown(1))
			{
				this.OnGetMouseButtonDown_1?.Invoke();
				return;
			}
			if (InputHelper.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
			{
				this.OnGetMouseButtonDown_0?.Invoke();
				return;
			}
		}

		private void LateUpdate()
		{
			if (HotUpdateComplete)
			{
                CodeLoader.Instance.LateUpdate();
            }
		}

		private void OnApplicationQuit()
		{
			if (HotUpdateComplete)
			{
                OnApplicationQuitHandler?.Invoke();
                CodeLoader.Instance.OnApplicationQuit();
                CodeLoader.Instance.Dispose();
            }
		}

		/// 当程序获得或者是去焦点时
		/// </summary>
		/// <param name="focus"></param>
		private void OnApplicationFocus(bool hasFocus)
		{
			try
			{
				OnApplicationFocusHandler?.Invoke(hasFocus);
			}
			catch (System.Exception)
			{

				throw;
			}
		}

		public void PemoveAccount(string fenxiangtype)
		{
			//plat.removeAccount(true)
			switch (fenxiangtype)
			{
				case "1":
					ssdk.CancelAuthorize(PlatformType.WeChat);
					break;
				case "2":
					ssdk.CancelAuthorize(PlatformType.QQ);
					break;
			}
		}

		/// <summary>
		/// 各平台授权
		/// </summary>
		/// <param name="fenxiangtype"></param>
		public void Authorize(string fenxiangtype)
		{
			switch (fenxiangtype)
			{
				case "1":
					ssdk.Authorize(PlatformType.WeChat);
					break;
				case "2":
					ssdk.Authorize(PlatformType.QQ);
					break;
			}
		}

		/// <summary>
		/// 授权返回
		/// </summary>
		/// <param name="reqID"></param>
		/// <param name="state"></param>
		/// <param name="type"></param>
		/// <param name="result"></param>
		//void OnAuthResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
		//{
		//	Log.ILog.Debug("OnAuthResultHandler:" + MiniJSON.jsonEncode(result));
		//	if (state != ResponseState.Success)
		//	{
		//		this.OnAuthorizeHandler("fail");
		//		return;
		//	}

		//	switch (type)
		//	{ 
		//		case PlatformType.WeChat:
		//			string openId = result["openid"].ToString();
		//			this.OnAuthorizeHandler($"sucess");
		//			break;
		//		case PlatformType.QQ:
		//			openId = result["openid"].ToString();
		//			this.OnAuthorizeHandler($"sucess");
		//			break;
		//		default:
		//			break;
		//	}
		//}

		/// <summary>
		/// 获取各平台用户信息
		/// </summary>
		/// <param name="fenxiangtype"></param>
		public void GetUserInfo(string fenxiangtype)
		{
			Log.ILog.Debug($"sharesdk GetUserInfo1");
#if !UNITY_EDITOR
			Log.ILog.Debug($"sharesdk GetUserInfo2");

			switch (fenxiangtype)
			{
				case "1":
					ssdk.GetUserInfo(PlatformType.WeChat);
					break;
				case "2":
					ssdk.GetUserInfo(PlatformType.QQ);
					break;
			}
#else
			string add = fenxiangtype == "1" ? "wx" : "qq";
			//this.OnGetUserInfoHandler($"{add}{PhoneNumberHelper.getRandomTel()};{add}{PhoneNumberHelper.getRandomTel()}");
#endif
		}

		/// <summary>
		/// 返回各平台用户信息
		/// </summary>
		/// <param name="reqID"></param>
		/// <param name="state"></param>
		/// <param name="type"></param>
		/// <param name="result"></param>
		void OnGetUserInfoResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
		{
			print("get user info result:");
			print(MiniJSON.jsonEncode(result));
			print("get user info sucess ! platform :" + type);
			if (type == PlatformType.WeChat)
			{
				print("get user info:   " + MiniJSON.jsonEncode(ssdk.GetAuthInfo(type)));
				if (state == ResponseState.Success)
				{
					result = ssdk.GetAuthInfo(type);
#if UNITY_ANDROID
					string openId = result["openID"].ToString();  //openID == userID
					print("get user info openId :" + openId);
					string userId = result["unionID"].ToString();
					print("get user info userId :" + userId);
#elif UNITY_IPHONE
					string openId = result["uid"].ToString();  //openID == userID
					print("get user info openId :" + openId);
					string userId = result["token"].ToString();
					print("get user info userId :" + userId);
#endif
					//this.OnGetUserInfoHandler($"wx{openId};wx{userId}");
				}
				else
				{
					//this.OnGetUserInfoHandler("fail");
				}
			}
			if (type == PlatformType.QQ)
			{
				print("get user info:   " + MiniJSON.jsonEncode(ssdk.GetAuthInfo(type)));
				if (state == ResponseState.Success)
				{
					result = ssdk.GetAuthInfo(type);
#if UNITY_ANDROID
					string openId = result["unionID"].ToString();
					string userId = result["userID"].ToString();
#elif UNITY_IPHONE
					string openId = result["uid"].ToString();
					string userId = result["token"].ToString();
#endif
					Log.ILog.Debug($"openId: {openId}:  userId:{userId}");
					//this.OnGetUserInfoHandler($"qq{openId};qq{userId}");
				}
				else
				{
					//this.OnGetUserInfoHandler("fail");
				}
			}
		}

		public void OnNativeToUnit(string msg)
		{
			string[] msginfo = msg.Split('_');
			switch (msginfo[0])
			{
				case "QQLogin":
					string openid = msginfo[1];
					if (string.IsNullOrEmpty(openid))
					{
						return;
					}
					Log.ILog.Debug($"QQLogin:  {openid}");
					//this.OnGetUserInfoHandler($"qq{openid};qq{msginfo[2]}");
					break;
			}
		}

		public void FenXiang(FenXiangContent fenxiangConent)
		{
			//auth和getuser接口都可以实现授权登录功能，可以任意调用一个
			//授权（每次都会跳转到第三方平台进行授权）进行授权
			//ssdk.Authorize(PlatformType.WeChat);
			//ssdk.GetUserInfo(PlatformType.WeChat);
			//设置分享
			ShareContent content = new ShareContent();
			//title标题，印象笔记、邮箱、信息、微信、人人网、QQ和QQ空间使用
			content.SetTitle(fenxiangConent.FenXiang_Title);
			//分享文本
			content.SetText(fenxiangConent.FenXiang_Text);
			//分享网络图片，新浪微博分享网络图片需要通过审核后申请高级写入接口，否则请注释掉测试新浪微博
			content.SetImageUrl(fenxiangConent.FenXiang_ImageUrl);
			// titleUrl是标题的网络链接，仅在Linked-in,QQ和QQ空间使用
			content.SetTitleUrl(fenxiangConent.FenXiang_ClickUrl);
			// imagePath是图片的本地路径，Linked-In以外的平台都支持此参数
			//content.SetImagePath("/sdcard/test.jpg");//确保SDcard下面存在此张图片
			// url仅在微信（包括好友和朋友圈）中使用
			content.SetUrl(fenxiangConent.FenXiang_ClickUrl);
			// site是分享此内容的网站名称，仅在QQ空间使用
			content.SetSite(fenxiangConent.FenXiang_Title);
			// siteUrl是分享此内容的网站地址，仅在QQ空间使用
			content.SetSiteUrl(fenxiangConent.FenXiang_ClickUrl);
			//设置分享类型
			content.SetShareType(ContentType.Webpage);
			//content.SetShareType(ContentType.Text);

			FenXiangShareContent(content, fenxiangConent.Fenxiangtype);
		}

		public void FenXiangShareContent(ShareContent content, int fenxiangtype)
		{
			switch (fenxiangtype)
			{
				//微信朋友圈
				case 1:
					//弹出菜单
					ssdk.ShareContent(PlatformType.WeChatMoments, content);
					break;
				//QQ空间
				case 2:
					//弹出菜单
					ssdk.ShareContent(PlatformType.QZone, content);
					break;
				//新浪微博
				case 3:
					//弹出菜单
					ssdk.ShareContent(PlatformType.SinaWeibo, content);
					break;
				//微信好友
				case 4:
					//弹出菜单
					ssdk.ShareContent(PlatformType.WeChat, content);
					break;
				//QQ好友
				case 5:
					//弹出菜单
					ssdk.ShareContent(PlatformType.QQ, content);
					break;
			}
		}

		void OnShareResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
		{
			//Game_PublicClassVar.Get_function_UI.GameGirdHint_Front("进入微信用户回调！" + state);
			Debug.Log("进入微信用户回调1！" + state);
			Log.ILog.Debug("进入微信用户回调2: " + MiniJSON.jsonEncode(result));
			string showText = "进入微信用户回调！";

			if (state == ResponseState.Success)
			{
				showText = "分享成功1！";
				this.OnShareHandler((int)type, true);
			}
			else if (state == ResponseState.Fail)
			{
				showText = "分享失败！";
#if UNITY_ANDROID
				Log.ILog.Debug("fail! throwable stack = " + result["stack"] + "; error msg = " + result["msg"]);
#elif UNITY_IPHONE
				Log.ILog.Debug("fail! error code = " + result["error_code"] + "; error msg = " + result["error_msg"]);
#endif
				this.OnShareHandler((int)type, false);
			}
			else if (state == ResponseState.Cancel)
			{
				showText = "进入指定用户回调取消！";
				Log.ILog.Debug("cancel !");
				this.OnShareHandler((int)type, false);
			}
			Log.ILog.Debug(showText);
		}

		void OnGetFriendsResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
		{
			Log.ILog.Debug("OnGetFriendsResultHandler");
		}

		void OnFollowFriendResultHandler(int reqID, ResponseState state, PlatformType type, Hashtable result)
		{
			Log.ILog.Debug("OnFollowFriendResultHandler");
		}

     
        /// <summary>
        /// 支付宝支付
        /// </summary>
        public void AliPay(string OrderInfo)
		{
			Log.ILog.Debug("AliPay: " + OrderInfo);
#if UNITY_ANDROID && !UNITY_EDITOR
			if (javaActive == null)
			{
				javaActive = new AndroidJavaObject(javaClassStr);
			}

			object[] objs = new object[] { OrderInfo, "Global", "AliPayCallback" };
			javaActive.Call("AliPay", objs);
#endif
		}

		/// <summary>支付宝支付回调</summary>
		//这里是同步调用,由SDK反馈支付结果
		public void AliPayCallback(string result)
		{
			Log.ILog.Debug("支付宝支付结果来了：" + result);
			//aliPayCallBack(result);
			//告诉服务器已经支付 等待返回结果
			//再监听结果 进行发放奖励 实际上都是独立的
			if (result == "支付成功")
			{
				Log.ILog.Debug("支付宝支付成功");
			}
			else
			{
				Log.ILog.Debug("支付宝支付失败");
				//清理支付信息
			}
		}

		/// <summary>微信充值</summary>
		public void WeChatPay(string orderInfo)
		{
			string[] orderInfos = orderInfo.Split(',');
			string appid = orderInfos[0];
			string mchid = orderInfos[1];
			string prepayid = orderInfos[2];
			string noncestr = orderInfos[3];
			string timestamp = orderInfos[4];
			string packageValue = orderInfos[5];
			string sign = orderInfos[6];
#if UNITY_ANDROID && !UNITY_EDITOR
		//将服务器返回的参数 封装到object数组里 分别是:会话ID,随机字符串,时间戳,签名,支付结果通知回调的物体,物体上的某个回调函数名称
		object[] objs = new object[] { appid, mchid, prepayid, noncestr, timestamp, sign, "Global", "WechatPayCallback" };
		//调用安卓层的WeiChatPayReq方法 进行支付
		jo.Call("WeChatPayReq", objs);
#endif
		}

		public void OnGetPhoneNum()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			jo.Call("GetPhoneNum", "+86");
#elif UNITY_IPHONE && !UNITY_EDITOR
		   //string phonenum = GetPhoneNum("+86");
		   //Log.ILog.Debug("phonenum: " +  phonenum);
		   //this.OnGetPhoneNumHandler(phonenum);
		    this.OnGetPhoneNumHandler("");
#else
            this.OnGetPhoneNumHandler("");
#endif
		}

		public void OnGetPhoneNum_2()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			jo.Call("OnGetPhoneNum_2", "+86");
#elif UNITY_IPHONE && !UNITY_EDITOR
			Log.ILog.Debug($"UNITY_IPHONE:");
#else
			this.OnGetPhoneNumHandler(PhoneNumberHelper.getRandomTel());
#endif
		}

		public void OnGetPhoneNum_3()
		{
#if UNITY_ANDROID && !UNITY_EDITOR
			jo.Call("OnGetPhoneNum_3", "+86");
#elif UNITY_IPHONE && !UNITY_EDITOR
			Log.ILog.Debug($"UNITY_IPHONE:");
#else
			this.OnGetPhoneNumHandler(PhoneNumberHelper.getRandomTel());
#endif
		}

		public void OnRecvPhoneNum(string phoneNum)
		{
			Log.ILog.Debug($"OnRecvPhoneNum: {phoneNum}");
			this.OnGetPhoneNumHandler(phoneNum);
		}

		public void OnRecvRoot(string root)
		{
			this.IsRoot = int.Parse(root);
			Log.ILog.Debug($"OnRecvRoot:  {root}");
		}

        public void OnRecvEmulator(string emulator)
        {
			this.IsEmulator = int.Parse(emulator);
            Log.ILog.Debug($"OnRecvEmulator:  {emulator}");
        }


        private const string AppleUserIdKey = "AppleUserId";


        private void InitializeLoginMenu()
        {
			if (this.appleAuthManager == null)
			{
				return;
			}

            // If at any point we receive a credentials revoked notification, we delete the stored User ID, and go back to login
            this.appleAuthManager.SetCredentialsRevokedCallback(result =>
            {
                Debug.Log("Received revoked callback " + result);
                PlayerPrefs.DeleteKey(AppleUserIdKey);
            });

            // If we have an Apple User Id available, get the credential status for it
            if (PlayerPrefs.HasKey(AppleUserIdKey))
            {
                var storedAppleUserId = PlayerPrefs.GetString(AppleUserIdKey);
                this.CheckCredentialStatusForUserId(storedAppleUserId);
            }
            // If we do not have an stored Apple User Id, attempt a quick login
           
        }

        private void CheckCredentialStatusForUserId(string appleUserId)
        {
            if (this.appleAuthManager == null)
            {
                return;
            }
            // If there is an apple ID available, we should check the credential state
            this.appleAuthManager.GetCredentialState(
                appleUserId,
                state =>
                {
                    switch (state)
                    {
                        // If it's authorized, login with that user id
                        case CredentialState.Authorized:
                            return;

                        // If it was revoked, or not found, we need a new sign in with apple attempt
                        // Discard previous apple user id
                        case CredentialState.Revoked:
                        case CredentialState.NotFound:
                            PlayerPrefs.DeleteKey(AppleUserIdKey);
                            return;
                    }
                },
                error =>
                {
                    var authorizationErrorCode = error.GetAuthorizationErrorCode();
                    Debug.LogWarning("Error while trying to get credential state " + authorizationErrorCode.ToString() + " " + error.ToString());
                });
        }

        private void AttemptQuickLogin()
        {
            if (this.appleAuthManager == null)
            {
                return;
            }

            var quickLoginArgs = new AppleAuthQuickLoginArgs();

            // Quick login should succeed if the credential was authorized before and not revoked
            this.appleAuthManager.QuickLogin(
                quickLoginArgs,
                credential =>
                {
                    // If it's an Apple credential, save the user ID, for later logins
                    var appleIdCredential = credential as IAppleIDCredential;
                    if (appleIdCredential != null)
                    {
                        PlayerPrefs.SetString(AppleUserIdKey, credential.User);
                    }
                },
                error =>
                {
                    // If Quick Login fails, we should show the normal sign in with apple menu, to allow for a normal Sign In with apple
                    var authorizationErrorCode = error.GetAuthorizationErrorCode();
                    Debug.LogWarning("Quick Login Failed " + authorizationErrorCode.ToString() + " " + error.ToString());
                });
        }

        public void SignInWithApple(string oldaccount)
        {
			Log.ILog.Debug($"SignInWithApple Begin");
            var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName);

			string appkey = PlayerPrefs.GetString(AppleUserIdKey);

            Log.ILog.Debug($"SignInWithApple Begin:  {appkey}   {oldaccount}");

            if (string.IsNullOrEmpty(oldaccount) || string.IsNullOrEmpty(appkey)  || !appkey.Equals(oldaccount))
			{
				this.appleAuthManager.LoginWithAppleId(
			   loginArgs,
			   credential =>
			   {
				   // If a sign in with apple succeeds, we should have obtained the credential with the user id, name, and email, save it
				   PlayerPrefs.SetString(AppleUserIdKey, credential.User);
				   //this.SetupGameMenu(credential.User, credential);

				   Log.ILog.Debug($"SignInWithApple Sucess :  {credential.User}");
				   this.AppleSignInHandler?.Invoke(credential.User);
			   },
			   error =>
			   {
				   this.AppleSignInHandler?.Invoke(string.Empty);
				   var authorizationErrorCode = error.GetAuthorizationErrorCode();
				   Log.ILog.Debug("SignInWithApple failed " + authorizationErrorCode.ToString() + " " + error.ToString());
			   });
			}
			else
			{
                this.AppleSignInHandler?.Invoke(appkey);
            }
        }
    }
}