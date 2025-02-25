﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace ET
{
    public class UIUnionApplyListItemComponent : Entity, IAwake<GameObject>
    {
        public GameObject Text_Occ;
        public GameObject GameObject;
        public GameObject Text_Combat;
        public GameObject Text_Name;
        public GameObject Text_Level;
        public GameObject ButtonAgree;
        public GameObject ButtonRefuse;

        public UnionPlayerInfo UnionPlayerInfo;
    }


    public class UIUnionApplyListItemComponentAwakeSystem : AwakeSystem<UIUnionApplyListItemComponent, GameObject>
    {
        public override void Awake(UIUnionApplyListItemComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Text_Combat = rc.Get<GameObject>("Text_Combat");
            self.Text_Name = rc.Get<GameObject>("Text_Name");
            self.Text_Level = rc.Get<GameObject>("Text_Level");
            self.Text_Occ = rc.Get<GameObject>("Text_Occ");

            self.ButtonAgree = rc.Get<GameObject>("ButtonAgree");
            ButtonHelp.AddListenerEx(self.ButtonAgree, () => { self.OnButtonReply(1).Coroutine(); });

            self.ButtonRefuse = rc.Get<GameObject>("ButtonRefuse");
            ButtonHelp.AddListenerEx(self.ButtonRefuse, () => { self.OnButtonReply(0).Coroutine(); });
        }
    }

    public static class UIUnionApplyListItemComponentSystem
    {
        public static async ETTask OnButtonReply(this UIUnionApplyListItemComponent self, int replyCode)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            long unionId = (unit.GetComponent<NumericComponent>().GetAsLong(NumericType.UnionId_0));
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            C2U_UnionApplyReplyRequest c2M_ItemHuiShouRequest = new C2U_UnionApplyReplyRequest()
            {
                ReplyCode = replyCode,
                UserId = self.UnionPlayerInfo.UserID,
                UnionId = unionId,
            };
            U2C_UnionApplyReplyResponse r2c_roleEquip = (U2C_UnionApplyReplyResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_ItemHuiShouRequest);
            UIHelper.GetUI(self.ZoneScene(), UIType.UIUnionApplyList).GetComponent<UIUnionApplyListComponent>().OnUnionApplyReply(self.UnionPlayerInfo.UserID);
            
        }

        public static void OnUpdateUI(this UIUnionApplyListItemComponent self, UnionPlayerInfo unionPlayerInfo)
        {
            self.UnionPlayerInfo = unionPlayerInfo;
            self.Text_Combat.GetComponent<Text>().text = unionPlayerInfo.Combat.ToString();
            self.Text_Name.GetComponent<Text>().text = unionPlayerInfo.PlayerName.ToString();
            self.Text_Level.GetComponent<Text>().text = unionPlayerInfo.PlayerLevel.ToString();

            if (unionPlayerInfo.OccTwo != 0)
            {
                self.Text_Occ.GetComponent<Text>().text = OccupationTwoConfigCategory.Instance.Get(unionPlayerInfo.OccTwo).OccupationName;
            }
            else
            {
                self.Text_Occ.GetComponent<Text>().text = OccupationConfigCategory.Instance.Get(unionPlayerInfo.Occ).OccupationName;
            }
        }
    }
}
