﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIDonationUnionComponent : Entity, IAwake
    {
        public GameObject Text_Bonus;
        public GameObject Text_Open_Time;
        public GameObject Button_Signup;
        public GameObject Text_Tip_5;
        public GameObject Button_Race;
        public List<UnionListItem> UnionListItems = new List<UnionListItem>();
    }

    public class UIDonationUnionComponentAwake : AwakeSystem<UIDonationUnionComponent>
    {
        public override void Awake(UIDonationUnionComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_Bonus = rc.Get<GameObject>("Text_Bonus");
            self.Text_Open_Time = rc.Get<GameObject>("Text_Open_Time");
            self.Text_Tip_5 = rc.Get<GameObject>("Text_Tip_5");

            self.Button_Signup = rc.Get<GameObject>("Button_Signup");
            ButtonHelp.AddListenerEx(self.Button_Signup, () => { self.OnButton_Signup().Coroutine();  });

            self.Button_Race = rc.Get<GameObject>("Button_Race");
            ButtonHelp.AddListenerEx(self.Button_Race, () => { self.OnButton_Race(); });
            self.Button_Race.SetActive(false);
            self.OnUpdateUI().Coroutine();
        }
    }

    public static class UIDonationUnionComponentSystem
    {

        public static void ShowOpenTime(this UIDonationUnionComponent self)
        {
            long serverTime = TimeHelper.ServerNow();
            DateTime dateTime = TimeInfo.Instance.ToDateTime(serverTime);
            int day = FunctionHelp.GetUnionRaceDay();
            long opentime = FunctionHelp.GetUnionRaceBeginTime();
            long curTime = (dateTime.Hour * 60 + dateTime.Minute) * 60 + dateTime.Second;
            if (day == (int)dateTime.DayOfWeek && curTime < opentime)
            {
                self.Text_Open_Time.GetComponent<Text>().text = $"{dateTime.Month}月{dateTime.Day}日 21点30开启";
            }
            else
            {
                long addTime = (7 - (int)dateTime.DayOfWeek) * TimeHelper.OneDay + (opentime  - curTime )* TimeHelper.Second;
                serverTime += addTime;
                dateTime = TimeInfo.Instance.ToDateTime(serverTime);
                self.Text_Open_Time.GetComponent<Text>().text = $"{dateTime.Month}月{dateTime.Day}日 21点30开启";
            }
        }

        public static void OnButton_Race(this UIDonationUnionComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

            bool signup = false;
            for (int i = 0; i < self.UnionListItems.Count; i++)
            {
                if (self.UnionListItems[i].UnionId == numericComponent.GetAsLong(NumericType.UnionId_0))
                {
                    signup = true;
                    break;
                }
            }
            if (signup)
            {
                EnterFubenHelp.RequestTransfer(self.ZoneScene(), SceneTypeEnum.UnionRace, 2000008).Coroutine();
                UIHelper.Remove( self.ZoneScene(), UIType.UIDonation );
            }
            else
            {
                FloatTipManager.Instance.ShowFloatTip("未报名！");
            }
        }

        public static async ETTask OnButton_Signup(this UIDonationUnionComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene( self.ZoneScene() );
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

            if (numericComponent.GetAsLong(NumericType.UnionId_0) == 0)
            {
                FloatTipManager.Instance.ShowFloatTip("没有家族！");
                return;
            }

            for (int i = 0; i < self.UnionListItems.Count; i++)
            {
                if (self.UnionListItems[i].UnionId == numericComponent.GetAsLong(NumericType.UnionId_0))
                {
                    FloatTipManager.Instance.ShowFloatTip("已报名！");
                    return;
                }
            }

            C2U_UnionMyInfoRequest  unionrequest = new C2U_UnionMyInfoRequest() { UnionId = numericComponent.GetAsLong(NumericType.UnionId_0) };
            U2C_UnionMyInfoResponse unionrespose = (U2C_UnionMyInfoResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(unionrequest);
            UnionPlayerInfo unionPlayerInfo = UnionHelper.GetUnionPlayerInfo(unionrespose.UnionMyInfo.UnionPlayerList, unit.Id);
            if (unionPlayerInfo.Position == 0)
            {
                FloatTipManager.Instance.ShowFloatTip("没有权限！");
                return;
            }

            C2U_UnionSignUpRequest  request = new C2U_UnionSignUpRequest() { UnionId = numericComponent.GetAsLong(NumericType.UnionId_0) };
            U2C_UnionSignUpResponse response = (U2C_UnionSignUpResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);

            self.OnUpdateUI().Coroutine();
        }

        public static async ETTask OnUpdateUI(this UIDonationUnionComponent self)
        {
            C2U_UnionRaceInfoRequest  request = new C2U_UnionRaceInfoRequest();
            U2C_UnionRaceInfoResponse response = (U2C_UnionRaceInfoResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            self.UnionListItems = response.UnionInfoList;
            self.Text_Bonus.GetComponent<Text>().text = $"累计总奖金： {response.TotalDonation}";

            string unionnamelist = "已报名家族: ";
            for (int i = 0; i < self.UnionListItems.Count; i++)
            {
                unionnamelist = unionnamelist + self.UnionListItems[i].UnionName + "   ";
            }

            self.Text_Tip_5.GetComponent<Text>().text = unionnamelist;

            self.Button_Race.SetActive(FunctionHelp.IsInUnionRaceTime());
            self.Button_Signup.SetActive(!FunctionHelp.IsInUnionRaceTime());

            self.ShowOpenTime();
        }
    }
}
