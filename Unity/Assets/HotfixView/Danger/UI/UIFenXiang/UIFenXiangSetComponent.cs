﻿using System;
using UnityEngine;
using cn.sharesdk.unity3d;

namespace ET
{
    public class UIFenXiangSetComponent : Entity, IAwake
    {
        public GameObject Button_support;
        public GameObject FenXiang_WeiXin;
        public GameObject FenXiang_QQ;
        public GameObject Button_AddQQ;
        public string PopularizeCode;

        public int ShareType;
    }


    public class UIFenXiangSetComponentAwake : AwakeSystem<UIFenXiangSetComponent>
    {
        public override void Awake(UIFenXiangSetComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
    
            self.Button_support = rc.Get<GameObject>("Button_support");
            self.FenXiang_WeiXin = rc.Get<GameObject>("FenXiang_WeiXin");
            self.FenXiang_QQ = rc.Get<GameObject>("FenXiang_QQ");
            self.Button_AddQQ = rc.Get<GameObject>("Button_AddQQ");

            ButtonHelp.AddListenerEx(self.FenXiang_QQ.transform.Find("Button_Share").gameObject, self.OnQQZone);
            ButtonHelp.AddListenerEx(self.FenXiang_QQ.transform.Find("Button_Friend").gameObject, self.OnQQShare);
            ButtonHelp.AddListenerEx(self.FenXiang_WeiXin.transform.Find("Button_Share").gameObject, self.OnWeiXinShare);
            ButtonHelp.AddListenerEx(self.FenXiang_WeiXin.transform.Find("Button_Friend").gameObject, self.OnWeChatMoments);
            ButtonHelp.AddListenerEx(self.Button_support, () => { UIHelper.Create(self.ZoneScene(), UIType.UIRecharge).Coroutine(); });
            ButtonHelp.AddListenerEx(self.Button_AddQQ, () => { self.OpenAddQQ(); });

            GameObject.Find("Global").GetComponent<Init>().OnShareHandler = (int pType, bool value) => { self.OnShareHandler(pType, value).Coroutine(); };
            self.RequestPopularizeCode().Coroutine();
            self.OnUpdateUI();
        }
    }

    public static class UIFenXiangSetComponentSystem
    {

        public static void  OpenAddQQ(this UIFenXiangSetComponent self) 
        {
            Application.OpenURL("http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=UZgs61u2uXu5TRE1tbhd_jOb-aDGqTIS&authKey=xlB8sICK4wNrhaBXRsM6Mz0aqksXdHhpL3iqcDsWVNZLcMlgdTehrPZKYmiBnziK&noverify=0&group_code=91958754");
        }

        public static async ETTask RequestPopularizeCode(this UIFenXiangSetComponent self)
        {
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            C2Popularize_ListRequest request = new C2Popularize_ListRequest() { ActorId = userInfoComponent.UserInfo.UserId };
            Popularize2C_ListResponse response = (Popularize2C_ListResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            self.PopularizeCode = response.PopularizeCode.ToString();
            Log.Debug($"self.PopularizeCode :{self.PopularizeCode}");
        }

        public static async ETTask OnShareHandler(this UIFenXiangSetComponent self, int pType, bool share)
        {
            //1 4微信  2 5QQ
            Log.ILog.Debug($"分享回调：  {pType} {share}");
            int sType = self.ShareType;
            if (sType != 1 && sType != 2)
            {
                return;
            }

            long instanceid = self.InstanceId;
            C2M_ShareSucessRequest c2M_ShareSucessRequest = new C2M_ShareSucessRequest() { ShareType = sType };
            await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(c2M_ShareSucessRequest);
            if (instanceid != self.InstanceId)
            {
                return;
            }
            self.OnUpdateUI();
        }

        public static void OnUpdateUI(this UIFenXiangSetComponent self)
        {
            self.FenXiang_WeiXin.transform.Find("Image_complete").gameObject.SetActive(self.IsShared(1));
            self.FenXiang_QQ.transform.Find("Image_complete").gameObject.SetActive(self.IsShared(2));
        }

        public static bool IsShared(this UIFenXiangSetComponent self, int sType)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            long shareSet = unit.GetComponent<NumericComponent>().GetAsLong(NumericType.FenShangSet);
            return (shareSet & sType) > 0;
        }

        public static void FenXiangByType(this UIFenXiangSetComponent self, int shareType)
        {
            string title = "危境";
            string text = "暗黑系列ARPG探索类手游《危境》系列正式开启！";

            if (shareType == 4 || shareType == 5) {

                title = "快来和我一起玩危境吧!";
                text = "一把木剑，一件布衣,点击这个链接开始你的探险吧!";
                if (self.PopularizeCode != "") {
                    //text += "记得输入我的邀请码喔:" + self.PopularizeCode;
                }
            }

            FenXiangContent fenXiangContent = new FenXiangContent();
            fenXiangContent.FenXiang_Title = title;
            fenXiangContent.FenXiang_Text = text;
            fenXiangContent.FenXiang_ImageUrl = "https://img.71acg.net/kbdev/opensj/20230109/15243214265";
            //fenXiangContent.FenXiang_ClickUrl = "http://verification.weijinggame.com/weijing/";
            fenXiangContent.FenXiang_ClickUrl = "https://l.tapdb.net/08MLKXV5?channel=rep-rep_d2ves97egb7";
        
            fenXiangContent.Fenxiangtype = shareType;
            self.ShareType = shareType;

#if UNITY_EDITOR
            self.OnShareHandler(shareType, true).Coroutine();
#else
            GlobalHelp.FenXiang(fenXiangContent);
#endif

        }

        public static void OnQQShare(this UIFenXiangSetComponent self)
        {
            //QQ好友
            self.FenXiangByType(5);
        }

        public static void OnWeiXinShare(this UIFenXiangSetComponent self)
        {
            //微信朋友圈
            if (self.IsShared(1))
            {
                return;
            }
            self.FenXiangByType(1);
        }

        public static void OnWeChatMoments(this UIFenXiangSetComponent self)
        {
            //微信好友
            self.FenXiangByType(4);
        }

        public static void OnQQZone(this UIFenXiangSetComponent self)
        {
            //QQ空间
            if (self.IsShared(2))
            {
                return;
            }
            self.FenXiangByType(2);
        }
    }
}
