﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIRechargeComponent: Entity, IAwake, IDestroy
    {

        public GameObject Loading;
        public GameObject Img_Loading;
        public GameObject ImageSelect2;
        public GameObject ImageSelect1;
        public GameObject ButtonAliPay;
        public GameObject ButtonWeiXin;

        public GameObject ImageButton;
        public GameObject RechargeList;

        public int PayType; //1微信  2支付宝
        public int ReChargeNumber;

        public string AssetPath = string.Empty;
    }

    public class UIRechargeComponentDestroy : DestroySystem<UIRechargeComponent>
    {
        public override void Destroy(UIRechargeComponent self)
        {
            //GameObject.Find("Global").GetComponent<Init>().OnRiskControlInfoHandler = null;
            if (!string.IsNullOrEmpty(self.AssetPath))
            {
                ResourcesComponent.Instance.UnLoadAsset(self.AssetPath);
            }
        }
    }

    public class UIRechargeComponentAwakeSystem : AwakeSystem<UIRechargeComponent>
    {

        public override void Awake(UIRechargeComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Loading = rc.Get<GameObject>("Loading");
            self.Loading.SetActive(false);
            self.ImageSelect2 = rc.Get<GameObject>("ImageSelect2");
            self.ImageSelect1 = rc.Get<GameObject>("ImageSelect1");
            self.Img_Loading = rc.Get<GameObject>("Img_Loading");

            UI uirotate = self.AddChild<UI, string, GameObject>("Img_Loading", self.Img_Loading);
            uirotate.AddComponent<UIRotateComponent>().Start = true;

            self.ButtonAliPay = rc.Get<GameObject>("ButtonAliPay");
            self.ButtonAliPay.GetComponent<Button>().onClick.AddListener(() => 
            {
                self.ImageSelect1.SetActive(false);
                self.ImageSelect2.SetActive(true);
                self.PayType = PayTypeEnum.AliPay;
            } );
            self.ButtonWeiXin = rc.Get<GameObject>("ButtonWeiXin");
            self.ButtonWeiXin.GetComponent<Button>().onClick.AddListener(() =>
            {
                self.ImageSelect1.SetActive(true);
                self.ImageSelect2.SetActive(false);
                self.PayType = PayTypeEnum.WeiXinPay;
            });
            self.ImageSelect1.SetActive(false);
            self.ImageSelect2.SetActive(true);
            self.RechargeList = rc.Get<GameObject>("RechargeList");

            self.ImageButton = rc.Get<GameObject>("ImageButton");
            self.ImageButton.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_Close(); });
            self.PayType = PayTypeEnum.AliPay;

            self.InitRechargeList().Coroutine();

            if (GlobalHelp.GetPlatform() == 5)
            {
                self.PayType = PayTypeEnum.TikTok;
                self.ImageSelect1.SetActive(false);
                self.ImageSelect2.SetActive(false);
                self.ButtonAliPay.SetActive(false);
                self.ButtonWeiXin.SetActive(false);
            }
#if UNITY_IPHONE && !UNITY_EDITOR
            self.ImageSelect1.SetActive(false);
            self.ImageSelect2.SetActive(false);
            self.ButtonAliPay.SetActive(false);
            self.ButtonWeiXin.SetActive(false);    
#endif
            //GameObject.Find("Global").GetComponent<Init>().OnRiskControlInfoHandler =  (string text) => { self.OnGetRiskControlInfo(text); };
        }
    }

    public static class UIRechargeComponentSystem
    {
        public static async ETTask InitRechargeList(this UIRechargeComponent self)
        {
            long instanceid = self.InstanceId;
            string path = ABPathHelper.GetUGUIPath("Main/Recharge/UIRechargeItem");
            GameObject bundleObj =await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            if (instanceid != self.InstanceId)
            {
                return;
            }
            self.AssetPath = path;
            foreach (var item in ConfigHelper.RechargeGive)
            {
                GameObject skillItem = GameObject.Instantiate(bundleObj);
                UICommonHelper.SetParent(skillItem, self.RechargeList);
                UI ui_1 = self.AddChild<UI, string, GameObject>("rewardItem_" + item.Key.ToString(), skillItem);
                UIRechargeItemComponent uIItemComponent = ui_1.AddComponent<UIRechargeItemComponent>();
                uIItemComponent.OnInitData(item.Key, item.Value);
                uIItemComponent.SetClickHandler((int number) => { self.OnClickRechargeItem(number).Coroutine(); });
            }
        }

        public static  void OnGetRiskControlInfo(this UIRechargeComponent self, string riskControl)
        {
            self.RequestRecharge(riskControl).Coroutine();
        }

        public static async ETTask RequestRecharge(this UIRechargeComponent self, string riskControl = "")
        {
            C2M_RechargeRequest c2E_GetAllMailRequest = new C2M_RechargeRequest()
            {
                RiskControlInfo = riskControl,  
                RechargeNumber = self.ReChargeNumber,
                PayType = self.PayType 
            };

            M2C_RechargeResponse sendChatResponse = (M2C_RechargeResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2E_GetAllMailRequest);

            if (sendChatResponse.Error != ErrorCode.ERR_Success)
            {
                return;
            }
            if (GlobalHelp.IsBanHaoMode || string.IsNullOrEmpty(sendChatResponse.Message))
            {
                return;
            }
            if (self.PayType == PayTypeEnum.AliPay)
            {
                GlobalHelp.AliPay(sendChatResponse.Message);
            }
            if (self.PayType == PayTypeEnum.WeiXinPay)
            {
                GlobalHelp.WeChatPay(sendChatResponse.Message);
            }
            if (self.PayType == PayTypeEnum.TikTok)
            {
                GameObject.Find("Global").GetComponent<Init>().TikTokPay(sendChatResponse.Message);
            }
        }

        public static async ETTask OnClickRechargeItem(this UIRechargeComponent self, int chargetNumber)
        {

            FangChenMiComponent fangChenMiComponent = self.ZoneScene().GetComponent<FangChenMiComponent>();
            int code = fangChenMiComponent.CanRechage(chargetNumber);
            if (code != ErrorCode.ERR_Success)
            {
                //EventSystem.Instance.Publish( new EventType.CommonHintError() {  errorValue = code } );
                string tips = "";
                if (code == ErrorCode.ERR_FangChengMi_Tip3)
                {
                    tips = $"{ErrorHelp.Instance.ErrorHintList[code]}";
                }
                else {
                    tips = $"{ErrorHelp.Instance.ErrorHintList[code]} 你本月已充值{fangChenMiComponent.GetMouthTotal()}元";
                }

                PopupTipHelp.OpenPopupTip_3(self.ZoneScene(),"防沉迷提示", tips, () => { }).Coroutine();
                return;
            }
            self.ReChargeNumber = chargetNumber;

#if UNITY_IPHONE
            self.Loading.SetActive(true);
            GlobalHelp.OnIOSPurchase(chargetNumber);
            C2M_RechargeRequest c2E_GetAllMailRequest = new C2M_RechargeRequest() { RechargeNumber = chargetNumber, PayType = PayTypeEnum.IOSPay };
            self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2E_GetAllMailRequest).Coroutine();
#else

            if (GlobalHelp.GetPlatform() == 5)
            {
                //GameObject.Find("Global").GetComponent<Init>().TikTokRiskControlInfo();
            }
            else
            {
                self.RequestRecharge().Coroutine();
            }

            //记录tap数据
            try
            {
                AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
                string serverName = ServerHelper.GetGetServerItem(!GlobalHelp.IsOutNetMode, accountInfoComponent.ServerId).ServerName;
                UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
                TapSDKHelper.UpLoadPlayEvent(userInfo.Name, serverName, userInfo.Lv, 4, chargetNumber);
            }
            catch (Exception ex) {
                Log.Debug("UIRecharge ex:" + ex);
            }

#endif

            await ETTask.CompletedTask;
        }

        public static void OnBtn_Close(this UIRechargeComponent self)
        {
            UIHelper.Remove(self.DomainScene(), UIType.UIRecharge);
        }
    }

}

