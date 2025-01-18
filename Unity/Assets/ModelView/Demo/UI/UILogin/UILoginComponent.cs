using cn.sharesdk.unity3d;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class UILoginComponent: Entity, IAwake
	{

		public GameObject buttonAgeTip;
		public GameObject UIAgeTip;

        public GameObject DeleteAccountBtn;
		public GameObject TextYanzheng;
		public GameObject AccountText;

		public GameObject TextButton_2_2;
		public GameObject TextButton_2_1;
		public GameObject YinSiToggle2;
		public GameObject TextYinSi;


        public GameObject BanHanNode;
		public GameObject HideNode;

		public GameObject YongHuXieYiClose;
		public GameObject YongHuXieYi;
		public GameObject YinSiXieYi;
		public GameObject YinSiXieYiClose;
		public GameObject YinSiToggle;
		public GameObject TextButton_2;
		public GameObject TextButton_1;

		public GameObject Btn_Return;
		public GameObject ZhuCe;
		public GameObject ButtonOtherLogin;
		public GameObject ButtonYiJianLogin;
		public GameObject TextPhoneNumber;
		public GameObject ThirdLoginBg;
		public GameObject YiJianDengLu;
		public GameObject ButtonCommitCode;
		public GameObject YanZheng;
		public GameObject SendYanzheng;
		public GameObject IPhone;
		public GameObject ButtonGetCode;
		public GameObject TextPhoneCode;
		public GameObject PhoneNumber;
		public GameObject RealNameButton;
		public GameObject Account;
		public GameObject Password;
		public GameObject loginBtn;
		public GameObject registerBtn;
		public GameObject ObjNoticeBtn;
		public GameObject ObjSelectBtn;
		public GameObject SelectServerName;
		public GameObject Loading;

		public long LastLoginTime;
		
		public ServerItem ServerInfo;
		public AccountInfoComponent AccountInfoComponent;
		public UI UIRotateComponent;

		public string LoginType;

		public int LoginErrorNumber;

		public bool IOSReview = true;

		public ShareSDK ssdk;

		public string AccountReversal;

		public GameObject ServerBtn;


    }
}
