﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace ET
{
    public class UIUnionMyComponent : Entity, IAwake, IDestroy
    {
        public GameObject Text_Exp;
        public GameObject Text_Level;
        public GameObject Text_EnterUnion;
        public GameObject Text_Button_1;
        public GameObject ButtonModify;
        public GameObject InputFieldPurpose;
        public GameObject LeadNode;
        public GameObject Text_OnLine;
        public GameObject Text_Purpose;
        public GameObject Text_Number;
        public GameObject Text_Leader;
        public GameObject Text_UnionName;
        public GameObject ButtonApplyList;
        public GameObject ButtonLeave;
        public GameObject ButtonName;
        public GameObject InputFieldName;
        public GameObject MemberListNode;
        public GameObject ShowSet;

        public UnionInfo UnionInfo;
        public List<long> OnLinePlayer;
    }


    public class UIUnionMyComponentAwakeSystem : AwakeSystem<UIUnionMyComponent>
    {
        public override void Awake(UIUnionMyComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_Button_1 = rc.Get<GameObject>("Text_Button_1");
            ButtonHelp.AddListenerEx(self.Text_Button_1, () => { self.OnShowModify(true);  });
            self.ButtonModify = rc.Get<GameObject>("ButtonModify");
            self.ButtonModify.SetActive(false);
            ButtonHelp.AddListenerEx(self.ButtonModify, () => { self.OnButtonModify().Coroutine(); });

            self.InputFieldPurpose = rc.Get<GameObject>("InputFieldPurpose");
            self.InputFieldPurpose.GetComponent<InputField>().onValueChanged.AddListener((string text) => { self.CheckSensitiveWords_2();  });

            self.Text_Level = rc.Get<GameObject>("Text_Level");
            self.Text_Exp = rc.Get<GameObject>("Text_Exp");
            self.LeadNode = rc.Get<GameObject>("LeadNode");
            self.Text_OnLine = rc.Get<GameObject>("Text_OnLine");
            self.Text_Purpose = rc.Get<GameObject>("Text_Purpose");
            self.Text_Number = rc.Get<GameObject>("Text_Number");
            self.Text_Leader = rc.Get<GameObject>("Text_Leader");
            self.Text_UnionName = rc.Get<GameObject>("Text_UnionName");
            self.ButtonApplyList = rc.Get<GameObject>("ButtonApplyList");
            self.ButtonApplyList.GetComponent<Button>().onClick.AddListener(() => { self.OnButtonApplyList(); });
            self.ButtonLeave = rc.Get<GameObject>("ButtonLeave");
            self.ShowSet = rc.Get<GameObject>("ShowSet");
            ButtonHelp.AddListenerEx(self.ButtonLeave, () => { self.OnButtonLeave(); });

            self.ButtonName = rc.Get<GameObject>("ButtonName");
            ButtonHelp.AddListenerEx(self.ButtonName, () => { self.OnButtonName().Coroutine(); });

            self.InputFieldName = rc.Get<GameObject>("InputFieldName");
            self.InputFieldName.GetComponent<InputField>().onValueChanged.AddListener((string text) => { self.CheckSensitiveWords(); });

            self.MemberListNode = rc.Get<GameObject>("MemberListNode");

            self.Text_EnterUnion = rc.Get<GameObject>("Text_EnterUnion");
            self.Text_EnterUnion.GetComponent<Button>().onClick.AddListener(self.OnText_EnterUnion);
            self.Text_EnterUnion.SetActive( GMHelp.GmAccount.Contains( self.ZoneScene().GetComponent<AccountInfoComponent>().Account ) );

            self.UnionInfo = null;
            self.GetParent<UI>().OnUpdateUI = () => { self.OnUpdateUI().Coroutine();  };

            ReddotViewComponent redPointComponent = self.ZoneScene().GetComponent<ReddotViewComponent>();
            redPointComponent.RegisterReddot(ReddotType.UnionApply, self.Reddot_UnionApply);
        }
    }


    public class UIUnionMyComponentDestroySystem : DestroySystem<UIUnionMyComponent>
    {
        public override void Destroy(UIUnionMyComponent self)
        {
            ReddotViewComponent redPointComponent = self.DomainScene().GetComponent<ReddotViewComponent>();
            redPointComponent.UnRegisterReddot(ReddotType.UnionApply, self.Reddot_UnionApply);
        }
    }

    public static class UIUnionMyComponentSystem
    {

        public static void  OnText_EnterUnion(this UIUnionMyComponent self)
        {
            EnterFubenHelp.RequestTransfer( self.ZoneScene(), SceneTypeEnum.Union, 2000009).Coroutine();
            UIHelper.Remove( self.ZoneScene(), UIType.UIFriend );
        }

        public static void OnShowModify(this UIUnionMyComponent self, bool val)
        {
            self.InputFieldPurpose.SetActive(val);
            self.ButtonModify.SetActive(val);
        }

        public static async ETTask OnButtonModify(this UIUnionMyComponent self)
        {
            string text = self.InputFieldPurpose.GetComponent<InputField>().text;
            bool mask = MaskWordHelper.Instance.IsContainSensitiveWords(text);
            if (mask)
            {
                FloatTipManager.Instance.ShowFloatTip("输入不合法!");
                return;
            }
            if (!StringHelper.IsSpecialChar(text))
            {
                FloatTipManager.Instance.ShowFloatTip("输入不合法!");
                return;
            }

            C2U_UnionOperatateRequest c2M_ItemHuiShouRequest = new C2U_UnionOperatateRequest()
            {
                UnionId = self.UnionInfo.UnionId,
                Operatate = 2,
                Value = text
            };
            U2C_UnionOperatateResponse r2c_roleEquip = (U2C_UnionOperatateResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_ItemHuiShouRequest);
            self.UnionInfo.UnionPurpose = text;
            self.Text_Purpose.GetComponent<Text>().text = text;
            self.OnShowModify(false);
        }

        public static void Reddot_UnionApply(this UIUnionMyComponent self, int num)
        {
            self.ButtonApplyList.transform .Find("Reddot").gameObject.SetActive(num > 0);
        }

        public static void CheckSensitiveWords(this UIUnionMyComponent self)
        {
            string text_new = "";
            string text_old = self.InputFieldName.GetComponent<InputField>().text;
            MaskWordHelper.Instance.IsContainSensitiveWords(ref text_old, out text_new);
            self.InputFieldName.GetComponent<InputField>().text = text_old;
        }

        public static void CheckSensitiveWords_2(this UIUnionMyComponent self)
        {
            string text_new = "";
            string text_old = self.InputFieldPurpose.GetComponent<InputField>().text;
            MaskWordHelper.Instance.IsContainSensitiveWords(ref text_old, out text_new);
            self.InputFieldPurpose.GetComponent<InputField>().text = text_old;
            self.ButtonModify.SetActive(true);
        }

        public static async ETTask OnButtonName(this UIUnionMyComponent self)
        {
            string text = self.InputFieldName.GetComponent<InputField>().text;
            bool mask = MaskWordHelper.Instance.IsContainSensitiveWords(text);
            if (mask)
            {
                FloatTipManager.Instance.ShowFloatTip("请重新输入！");
                return;
            }

            C2U_UnionOperatateRequest c2M_ItemHuiShouRequest = new C2U_UnionOperatateRequest()
            {
                UnionId = self.UnionInfo.UnionId,
                Operatate = 1,
                Value = text
            };
            U2C_UnionOperatateResponse r2c_roleEquip = (U2C_UnionOperatateResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_ItemHuiShouRequest);
            self.UnionInfo.UnionName = text;
            self.Text_UnionName.GetComponent<Text>().text = self.UnionInfo.UnionName;
        }

        public static  void OnButtonLeave(this UIUnionMyComponent self)
        {
            if (self.UnionInfo == null)
            {
                return;
            }
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            if (userInfoComponent.UserInfo.UserId == self.UnionInfo.LeaderId && self.UnionInfo.UnionPlayerList.Count > 1)
            {
                FloatTipManager.Instance.ShowFloatTip("族长不能离开家族, 请先转移族长！");
                return;
            }
            PopupTipHelp.OpenPopupTip( self.ZoneScene(), "离开家族", "离开家族24小时内无法加入新家族", ()=>
            {
                self.RequestLevelUnion().Coroutine();
            }, null).Coroutine();
        }

        public static async ETTask RequestLevelUnion(this UIUnionMyComponent self)
        {
            C2M_UnionLeaveRequest c2M_ItemHuiShouRequest = new C2M_UnionLeaveRequest() { };
            M2C_UnionLeaveResponse r2c_roleEquip = (M2C_UnionLeaveResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_ItemHuiShouRequest);
        }

        public static async void  OnButtonApplyList(this UIUnionMyComponent self)
        {
            if (self.UnionInfo == null)
            {
                return;
            }

            self.ShowSet.SetActive(false);
            UIHelper.Create( self.ZoneScene(), UIType.UIUnionApplyList ).Coroutine();
            UI ui = UIHelper.GetUI(self.ZoneScene(), UIType.UIUnionApplyList);
            ui.GetComponent<UIUnionApplyListComponent>().ActionFunc = ()=> { self.ShowSetShow(); };

            C2M_ReddotReadRequest m_ReddotReadRequest = new C2M_ReddotReadRequest() { ReddotType = ReddotType.UnionApply };
            M2C_ReddotReadResponse m_ReddotReadResponse = (M2C_ReddotReadResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(m_ReddotReadRequest);
            self.ZoneScene().GetComponent<ReddotComponent>().RemoveReddont(ReddotType.UnionApply);
        }

        public static void ShowSetShow(this UIUnionMyComponent self)
        {
            self.UnionInfo = null;
            self.ShowSet.SetActive(true);
            self.OnUpdateUI().Coroutine();
        }

        public static async ETTask OnUpdateUI(this UIUnionMyComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            long unionId = (unit.GetComponent<NumericComponent>().GetAsLong(NumericType.UnionId_0));
            C2U_UnionMyInfoRequest c2M_ItemHuiShouRequest = new C2U_UnionMyInfoRequest()
            {
                UnionId = unionId
            };
            U2C_UnionMyInfoResponse r2c_roleEquip = (U2C_UnionMyInfoResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_ItemHuiShouRequest);
            if (r2c_roleEquip.Error != ErrorCode.ERR_Success)
            {
                return;
            }
            if (self.IsDisposed)
            {
                return;
            }
            
            self.UnionInfo = r2c_roleEquip.UnionMyInfo;
            self.OnLinePlayer = r2c_roleEquip.OnLinePlayer;
            self.Text_Level.GetComponent<Text>().text = $"{r2c_roleEquip.UnionMyInfo.Level}";
            if (UnionConfigCategory.Instance.Contain(r2c_roleEquip.UnionMyInfo.Level))
            {
                self.Text_Exp.GetComponent<Text>().text = $"{r2c_roleEquip.UnionMyInfo.Exp}/{UnionConfigCategory.Instance.Get(r2c_roleEquip.UnionMyInfo.Level).Exp}"; 
            }
            else
            {
                self.Text_Exp.GetComponent<Text>().text = String.Empty;
            }
           
            self.UpdateMyUnion().Coroutine();
        }

        public static async ETTask UpdateMyUnion(this UIUnionMyComponent self)
        {
            //客户端获取家族等级
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            long unionId = (unit.GetComponent<NumericComponent>().GetAsLong(NumericType.UnionId_0));
            C2U_UnionMyInfoRequest request = new C2U_UnionMyInfoRequest()
            {
                UnionId = unionId
            };
            U2C_UnionMyInfoResponse respose = (U2C_UnionMyInfoResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(request);
            if (respose.Error != ErrorCode.ERR_Success)
            {
                return;
            }
            if (self.IsDisposed)
            {
                return;
            }
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            UnionPlayerInfo mainPlayerInfo = UnionHelper.GetUnionPlayerInfo(self.UnionInfo.UnionPlayerList, userInfoComponent.UserInfo.UserId);
            UnionConfig unionCof = UnionConfigCategory.Instance.Get((int)respose.UnionMyInfo.Level);
            bool leader = userInfoComponent.UserInfo.UserId == self.UnionInfo.LeaderId;
            self.Text_OnLine.GetComponent<Text>().text = $"在线人数 {self.OnLinePlayer.Count}";
            self.Text_Purpose.GetComponent<Text>().text = self.UnionInfo.UnionPurpose;
            self.Text_Number.GetComponent<Text>().text = $"{ self.UnionInfo.UnionPlayerList.Count}/{unionCof.PeopleNum}";
            self.Text_Leader.GetComponent<Text>().text = self.UnionInfo.LeaderName;
            self.Text_UnionName.GetComponent<Text>().text = self.UnionInfo.UnionName;
            self.LeadNode.SetActive(leader || mainPlayerInfo.Position != 0);

            long instanceid = self.InstanceId;
            var path = ABPathHelper.GetUGUIPath("Main/Union/UIUnionMyItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            if (instanceid != self.InstanceId)
            {
                return;
            }

            List<Entity> childs = self.Children.Values.ToList();
            self.UnionInfo.UnionPlayerList.Sort(delegate (UnionPlayerInfo a, UnionPlayerInfo b)
            {
                //int leaderida = (a.UserID == self.UnionInfo.LeaderId) ? 1 : 0;
                //int leaderidb = (b.UserID == self.UnionInfo.LeaderId) ? 1 : 0;
                //return (leaderidb - leaderida);
                int positiona = a.Position == 0 ? 10 :a.Position;
                int positionb = b.Position == 0 ? 10 : b.Position;
                return positiona - positionb;
            });

            for (int i = 0; i < self.UnionInfo.UnionPlayerList.Count; i++)
            {
                UnionPlayerInfo unionPlayerInfo = self.UnionInfo.UnionPlayerList[i];
                UIUnionMyItemComponent uIUnionMyItemComponent = null;
                if (i < childs.Count)
                {
                    uIUnionMyItemComponent = (childs[i] as UIUnionMyItemComponent);
                    uIUnionMyItemComponent.GameObject.SetActive(true);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(bundleGameObject);
                    UICommonHelper.SetParent(go, self.MemberListNode);
                    uIUnionMyItemComponent = self.AddChild<UIUnionMyItemComponent, GameObject>(go);
                }

                uIUnionMyItemComponent.OnUpdateUI(self.UnionInfo, unionPlayerInfo);
            }

            for (int i = self.UnionInfo.UnionPlayerList.Count; i < childs.Count; i++)
            {
                (childs[i] as UIUnionMyItemComponent).GameObject.SetActive(false);
            }
        }
    }
}
