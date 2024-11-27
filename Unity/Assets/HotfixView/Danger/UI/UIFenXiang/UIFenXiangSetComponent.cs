using System.Collections.Generic;
using UnityEngine;

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
        public GameObject Button_Taptap;
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
            self.Button_Taptap = rc.Get<GameObject>("Button_Taptap");
            self.FenXiang_TikTok.SetActive(false);

            //if (GlobalHelp.GetPlatform() == 5)
            //{
            //    //self.FenXiang_TikTok.SetActive(false);
            //    self.FenXiang_WeiXin.SetActive(false);
            //    self.FenXiang_QQ.SetActive(true);
            //    self.FenXiang_QQ.transform.localPosition = new Vector3(0f, 112f, 0f);
            //    self.Text_tip1.SetActive(false);
            //    self.Button_AddQQ.SetActive(false);
            //}
            //else
            {
                //self.FenXiang_TikTok.SetActive(false);
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
            ButtonHelp.AddListenerEx(self.Button_AddQQ, self.OpenAddQQ);
            ButtonHelp.AddListenerEx(self.Button_Taptap, self.OnButton_Taptap);

            self.Button_Taptap.SetActive(false );

            ButtonHelp.AddListenerEx(self.FenXiang_TikTok.transform.Find("Button_Share").gameObject, self.OnTikTokShare);
            ButtonHelp.AddListenerEx(self.FenXiang_TikTok.transform.Find("Button_Friend").gameObject, self.OnTikTokShare);

            GameObject.Find("Global").GetComponent<Init>().OnShareHandler = (int pType, bool value) => { self.OnShareHandler(pType, value).Coroutine(); };
            self.RequestPopularizeCode().Coroutine();
            self.OnUpdateUI();
        }
    }

    public static class UIFenXiangSetComponentSystem
    {

        public static void OnButton_Taptap(this UIFenXiangSetComponent self)
        {
            if (UnitHelper.IsShared( self.ZoneScene(), 8))
            {
                Log.Debug("已经领取过taptap奖励");
            }

            self.ShareType = 8;
            EventType.TapTapShare.Instance.ZoneScene = self.ZoneScene();
            EventType.TapTapShare.Instance.Content = $"{ConfigHelper.TapTapShareTitle}&{ConfigHelper.TapTapShareContent}";
            Game.EventSystem.PublishClass(EventType.TapTapShare.Instance);
        }

        public static void  OpenAddQQ(this UIFenXiangSetComponent self) 
        {
            Application.OpenURL("https://qm.qq.com/q/UaV8WFeN6G");
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
            //1 4微信  2 5QQ     8 taptap
           
            int sType = self.ShareType;
            Log.ILog.Debug($"OnShareHandler:  pType: {pType}  sType:{sType}   share:{share}");
            if (sType != 1 && sType != 2 && sType != 8)
            {
                return;
            }
            Log.ILog.Debug($"OnShareHandler: 111");
            if (sType == 8 && !share)
            {
                Log.ILog.Debug($"分享回调TapTap：  pType: {pType}  sType:{sType}   share:{share}");
                FloatTipManager.Instance.ShowFloatTipDi("TapTap未安装或该版本不支持分享！");
                return;
            }

            Log.ILog.Debug($"OnShareHandler: 222");
            TaskComponent taskComponent = self.ZoneScene().GetComponent<TaskComponent>();
            if (taskComponent.GetHuoYueDu() < 30)
            {
                FloatTipManager.Instance.ShowFloatTip("活跃度低于30没有奖励！");
                return;
            }
            Log.ILog.Debug($"OnShareHandler: 333");
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
            self.FenXiang_WeiXin.transform.Find("Image_complete").gameObject.SetActive(UnitHelper.IsShared(self.ZoneScene(),  1));
            self.FenXiang_QQ.transform.Find("Image_complete").gameObject.SetActive(UnitHelper.IsShared(self.ZoneScene(), 2));
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

            if (GlobalHelp.IsEditorMode)
            {
                self.OnShareHandler(shareType, true).Coroutine();
            }
            else
            {
                GlobalHelp.FenXiang(fenXiangContent);
            }
        }

        public static void OnQQShare(this UIFenXiangSetComponent self)
        {
            //QQ好友
            self.FenXiangByType(5);
        }

        public static void OnWeiXinShare(this UIFenXiangSetComponent self)
        {
            //微信朋友圈
            if (UnitHelper.IsShared(self.ZoneScene(),    1))
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
            if (UnitHelper.IsShared(self.ZoneScene(),   2))
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
