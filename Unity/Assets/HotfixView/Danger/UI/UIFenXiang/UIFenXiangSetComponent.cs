﻿using System;
using UnityEngine;
using cn.sharesdk.unity3d;
using System.Collections.Generic;

namespace ET
{
    public class UIFenXiangSetComponent : Entity, IAwake
    {

        public GameObject Text_tip1;
        public GameObject Button_support;
        public GameObject FenXiang_WeiXin;
        public GameObject FenXiang_QQ;
        public GameObject Button_AddQQ;
        public GameObject FenXiang_TikTok;
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
            self.FenXiang_TikTok = rc.Get<GameObject>("FenXiang_TikTok");
            self.Text_tip1 = rc.Get<GameObject>("Text_tip1");

            if (GlobalHelp.GetPlatform() == 5)
            {
                self.FenXiang_TikTok.SetActive(false);
                self.FenXiang_WeiXin.SetActive(false);
                self.FenXiang_QQ.SetActive(true);
                self.FenXiang_QQ.transform.localPosition = new Vector3(0f, 112f, 0f);
                self.Text_tip1.SetActive(false);
                self.Button_AddQQ.SetActive(false);
            }
            else
            {
                self.FenXiang_TikTok.SetActive(false);
                self.FenXiang_WeiXin.SetActive(true);
                self.FenXiang_QQ.SetActive(true);
                self.FenXiang_QQ.transform.localPosition = new Vector3(-257f, 112f, 0f);
            }

            if (GlobalHelp.IsBanHaoMode)
            {
                self.Text_tip1.SetActive(false);
            }

            ButtonHelp.AddListenerEx(self.FenXiang_QQ.transform.Find("Button_Share").gameObject, self.OnQQZone);
            ButtonHelp.AddListenerEx(self.FenXiang_QQ.transform.Find("Button_Friend").gameObject, self.OnQQShare);
            ButtonHelp.AddListenerEx(self.FenXiang_WeiXin.transform.Find("Button_Share").gameObject, self.OnWeiXinShare);
            ButtonHelp.AddListenerEx(self.FenXiang_WeiXin.transform.Find("Button_Friend").gameObject, self.OnWeChatMoments);
            ButtonHelp.AddListenerEx(self.Button_support, () => { UIHelper.Create(self.ZoneScene(), UIType.UIRecharge).Coroutine(); });
            ButtonHelp.AddListenerEx(self.Button_AddQQ, () => { self.OpenAddQQ(); });

            ButtonHelp.AddListenerEx(self.FenXiang_TikTok.transform.Find("Button_Share").gameObject, self.OnTikTokShare);
            ButtonHelp.AddListenerEx(self.FenXiang_TikTok.transform.Find("Button_Friend").gameObject, self.OnTikTokShare);

            GameObject.Find("Global").GetComponent<Init>().OnShareHandler = (int pType, bool value) => { self.OnShareHandler(pType, value).Coroutine(); };
            self.RequestPopularizeCode().Coroutine();
            self.OnUpdateUI();
        }
    }

    public static class UIFenXiangSetComponentSystem
    {

        public static void  OpenAddQQ(this UIFenXiangSetComponent self) 
        {
            Application.OpenURL("http://qm.qq.com/cgi-bin/qm/qr?_wv=1027&k=8IC8YKKACgjkp_zJyDN0GGCf4baWxU7B&authKey=gQTYvxiFMLkP9I7u%2FmzapPS3M29gId4jQ8mzYVcwpQ3dOyCXBqVOJKcQZrA8mYhN&noverify=0&group_code=719546102");
        }

        public static async ETTask RequestPopularizeCode(this UIFenXiangSetComponent self)
        {
            BattleMessageComponent battleMessageComponent = self.ZoneScene().GetComponent<BattleMessageComponent>();
            if ( TimeHelper.ServerNow() -  battleMessageComponent.LastPopularize_ListTime < TimeHelper.Minute)
            {
                return;
            }
            battleMessageComponent.LastPopularize_ListTime = TimeHelper.ServerNow();    
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

            TaskComponent taskComponent = self.ZoneScene().GetComponent<TaskComponent>();
            if (taskComponent.GetHuoYueDu() < 30)
            {
                FloatTipManager.Instance.ShowFloatTip("活跃度低于30没有奖励！");
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

        public static void OnTikTokShareHandler(this UIFenXiangSetComponent self, int sharemode, bool sucess)
        { 
            
        }

        public static void OnTikTokShare(this UIFenXiangSetComponent self)
        {
            Log.ILog.Debug($"OnTikTokShare:");
            EventType.TikTokShare.Instance.ZoneScene = self.ZoneScene();
            EventType.TikTokShare.Instance.ShareHandler = self.OnTikTokShareHandler;
            EventType.TikTokShare.Instance.ShareMessage = new List<string>() {
                "https://img.71acg.net/kbdev/opensj/20230109/15243214265",
                "https://l.tapdb.net/08MLKXV5?channel=rep-rep_d2ves97egb7"
             };
            EventSystem.Instance.PublishClass(EventType.TikTokShare.Instance);
        }
    }
}
