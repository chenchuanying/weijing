using UnityEngine;
using System.Collections;
using System;

namespace cn.sharesdk.unity3d 
{
	[HideInInspector]
	public class DevInfoSet
	{
		public SinaWeiboDevInfo sinaweibo;
		public QQ qq;
		public QZone qzone;
		public WeChat wechat;
		public WeChatMoments wechatMoments; 
		public WeChatFavorites wechatFavorites;
    }

    [HideInInspector]
    public class DevInfo 
	{	
		public bool Enable = true;
	}

    [HideInInspector]
    public class SinaWeiboDevInfo : DevInfo 
	{
#if UNITY_ANDROID
		public const int type = (int)PlatformType.SinaWeibo;
		public string SortId = "4";
		public string AppKey = "4243223096";
		public string AppSecret = "a94b60256e1652fdcb379984db56a158";
		public string RedirectUrl = "http://www.sharesdk.cn";
		public bool ShareByAppClient = true;
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.SinaWeibo;
		public string app_key = "568898243";
		public string app_secret = "38a4f8204cc784f81f9f0daaf31e02e3";
		public string redirect_uri = "http://www.sharesdk.cn";
		public string app_universalLink = "https://bj2ks.share2dlink.com/";
//		public string auth_type = "both";	//can pass "both","sso",or "web"  
#endif
	}

	[HideInInspector]
	public class QQ : DevInfo 
	{
#if UNITY_ANDROID
		public const int type = (int)PlatformType.QQ;
		public string SortId = "2";
		public string AppId = "1105893765";
		public string AppKey = "8DpWsEXj40TfCKzz";
		public bool ShareByAppClient = true;
        //========================================================
        //when you test QQ miniprogram, you should use this params
        //At the same time, the package name and signature should 
        //correspond to the package name signature of the specific 
        //QQ sharing small program applied in the background of tencent
        //========================================================
        //public const int type = (int) PlatformType.QQ;
        //public string SortId = "2";
        //public string AppId = "222222";
        //public string AppKey = "aed9b0303e3ed1e27bae87c33761161d";
        //public bool ShareByAppClient = true;
        //========================================================
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QQ;
		public string app_id = "101883752";
		public string app_key = "ab9d332ee43d3259991047c7796767dd";
//		public string auth_type = "both";  //can pass "both","sso",or "web" 
#endif
    }

    [HideInInspector]
	public class QZone : DevInfo 
	{
#if UNITY_ANDROID
		public string SortId = "1";
		public const int type = (int)PlatformType.QZone;
		public string AppId = "1105893765";
		public string AppKey = "8DpWsEXj40TfCKzz";
		public bool ShareByAppClient = true;
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.QZone;
		public string app_id = "101883752";
		public string app_key = "ab9d332ee43d3259991047c7796767dd";
//		public string auth_type = "both";  //can pass "both","sso",or "web" 
#endif
    }


    [HideInInspector]
	public class WeChat : DevInfo 
	{
#if UNITY_ANDROID
		public string SortId = "5";
		public const int type = (int)PlatformType.WeChat;
		public string AppId = "wx638f7f0efe37a825";
		public string AppSecret = "c45e594ab681035a1cae6ab166f64a20";
		public string UserName = "gh_afb25ac019c9@app";
		public string Path = "/page/API/pages/share/share";
		public bool BypassApproval = false;
		public bool WithShareTicket = true;
		public string MiniprogramType = "0";
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChat;
		public string app_id = "wx638f7f0efe37a825";
        public string app_secret = "c45e594ab681035a1cae6ab166f64a20";
        public string app_universalLink = "https://c4ovz.share2dlink.com/";
#endif
	}

	[HideInInspector]
	public class WeChatMoments : DevInfo 
	{
#if UNITY_ANDROID
		public string SortId = "6";
		public const int type = (int)PlatformType.WeChatMoments;
		public string AppId = "wx638f7f0efe37a825";
		public string AppSecret = "c45e594ab681035a1cae6ab166f64a20";
		public bool BypassApproval = true;
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatMoments;
		public string app_id = "wx638f7f0efe37a825";
		public string app_secret = "c45e594ab681035a1cae6ab166f64a20";
        public string app_universalLink = "https://c4ovz.share2dlink.com/";
#endif
	}

	[HideInInspector]
	public class WeChatFavorites : DevInfo 
	{
#if UNITY_ANDROID
		public string SortId = "7";
		public const int type = (int)PlatformType.WeChatFavorites;
		public string AppId = "wx638f7f0efe37a825";
		public string AppSecret = "c45e594ab681035a1cae6ab166f64a20";
#elif UNITY_IPHONE
		public const int type = (int) PlatformType.WeChatFavorites;
		public string app_id = "wx638f7f0efe37a825";
		public string app_secret = "c45e594ab681035a1cae6ab166f64a20";
        public string app_universalLink = "https://c4ovz.share2dlink.com/";
#endif
	}

	// 下列为闭环分享相关类
	[HideInInspector]
    public class RestoreSceneConfigure
    {
        public bool Enable = false;
#if UNITY_ANDROID

#elif UNITY_IPHONE
        public string capabilititesAssociatedDomain = "applinks:ahmn.t4m.cn";
        public string capabilititesEntitlementsPathInXcode = "Unity-iPhone/Base.entitlements";
#endif
    }

    public class RestoreSceneInfo
    {
        public string path;
        public Hashtable customParams;

        public RestoreSceneInfo(string scenePath, Hashtable sceneCustomParams)
        {
            try 
            {
                this.path = scenePath;
                this.customParams = sceneCustomParams;
            } catch(Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e); 
            }
        }
    }


}
