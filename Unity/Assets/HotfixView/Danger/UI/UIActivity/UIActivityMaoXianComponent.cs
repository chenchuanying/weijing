﻿using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ET
{
    public class UIActivityMaoXianComponent : Entity, IAwake
    {
        public GameObject Text_Progress;
        public GameObject ImageReceived;
        public GameObject ButtonRight;
        public GameObject ButtonLeft;
        public GameObject ItemListNode;
        public GameObject ImageProgress;
        public GameObject Btn_GoToSupport;
        public GameObject Btn_GetReward;
        public GameObject Text_Title;
        public int CurActivityId;
    }


    public class UIActivityMaoXianComponentAwakeSystem : AwakeSystem<UIActivityMaoXianComponent>
    {
        public override void Awake(UIActivityMaoXianComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Text_Progress = rc.Get<GameObject>("Text_Progress");
            self.ImageReceived = rc.Get<GameObject>("ImageReceived");

            self.ButtonRight = rc.Get<GameObject>("ButtonRight");
            ButtonHelp.AddListenerEx(self.ButtonRight, () => { self.OnButtonActivty(1); });
            self.ButtonLeft = rc.Get<GameObject>("ButtonLeft");
            ButtonHelp.AddListenerEx( self.ButtonLeft, () => { self.OnButtonActivty(-1); } );

            self.ItemListNode = rc.Get<GameObject>("ItemListNode");
            self.ImageProgress = rc.Get<GameObject>("ImageProgress");
            //self.ButtonRight.SetActive(false);
            //self.ButtonLeft.SetActive(false);

            self.Btn_GoToSupport = rc.Get<GameObject>("Btn_GoToSupport");
            ButtonHelp.AddListenerEx(self.Btn_GoToSupport, () => { self.OnBtn_GoToSupport(); });
            self.Btn_GetReward = rc.Get<GameObject>("Btn_GetReward");
            ButtonHelp.AddListenerEx(self.Btn_GetReward, () => { self.OnBtn_GetReward().Coroutine(); });

            self.Text_Title = rc.Get<GameObject>("Text_Title");
            self.OnInitUI();
        }
    }

    public static class UIActivityMaoXianComponentSystem
    {

        public static void OnBtn_GoToSupport(this UIActivityMaoXianComponent self)
        {
            UIHelper.Create(self.ZoneScene(), UIType.UIRecharge).Coroutine();
        }

        public static async ETTask OnBtn_GetReward(this UIActivityMaoXianComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());

            ActivityComponent activityComponent = self.ZoneScene().GetComponent<ActivityComponent>();
            ActivityConfig activityConfig = ActivityConfigCategory.Instance.Get(self.CurActivityId);
            int rechargeNum = unit.GetMaoXianExp();
            int needNumber = int.Parse(activityConfig.Par_2);
            if (rechargeNum < needNumber)
            {
                FloatTipManager.Instance.ShowFloatTip("冒险家积分不足！");
                return;
            }
            if (activityComponent.ActivityReceiveIds.Contains(self.CurActivityId))
            {
                FloatTipManager.Instance.ShowFloatTip("当前奖励已领取！");
                return;
            }
            int errorCode = await activityComponent.GetActivityReward(activityConfig.ActivityType, activityConfig.Id);
            if (errorCode != ErrorCode.ERR_Success)
            {
                return;
            }
            self.Btn_GetReward.SetActive(!activityComponent.ActivityReceiveIds.Contains(self.CurActivityId));
            self.ImageReceived.SetActive(activityComponent.ActivityReceiveIds.Contains(self.CurActivityId));

            self.OnUpdateUI(activityComponent.GetCurActivityId(rechargeNum));
        }

      

        public static void OnInitUI(this UIActivityMaoXianComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int rechargeNum = unit.GetMaoXianExp();
            ActivityComponent activityComponent = self.ZoneScene().GetComponent<ActivityComponent>();

            self.OnUpdateUI(activityComponent.GetCurActivityId(rechargeNum));
        }

        public static void OnButtonActivty(this UIActivityMaoXianComponent self, int index)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int rechargeNum = unit.GetMaoXianExp();
            ActivityComponent activityComponent = self.ZoneScene().GetComponent<ActivityComponent>();
            int selId = activityComponent.GetCurActivityId(rechargeNum);

            int maxId = ActivityHelper.GetMaxActivityId(101);
            int minId = ActivityHelper.GetMinActivityId(101);
            int curId = self.CurActivityId;

            curId += index;
            //if (curId < minId)
            //{
            //    self.ButtonLeft.SetActive(false);
            //    return;
            //}
            //if (curId > maxId || curId > selId + 2)
            //{
            //    self.ButtonRight.SetActive(true);
            //    return;
            //}
            self.OnUpdateUI(curId);
        }
      
        public static void OnUpdateUI(this UIActivityMaoXianComponent self, int maoxianId)
        {
            if (maoxianId == 0)
            {
                return;
            }
            self.CurActivityId = maoxianId;
            //self.ButtonRight.SetActive(ActivityConfigCategory.Instance.Contain(maoxianId + 1));
            ActivityComponent activityComponent =  self.ZoneScene().GetComponent<ActivityComponent>();

            ActivityConfig activityConfig = ActivityConfigCategory.Instance.Get(maoxianId);
            self.Text_Title.GetComponent<Text>().text = activityConfig.Par_4;

            Unit unit = UnitHelper.GetMyUnitFromZoneScene( self.ZoneScene() );

            int rechargeNum = unit.GetMaoXianExp();
            int needNumber = int.Parse( activityConfig.Par_2);
            float value = rechargeNum * 1f / needNumber;
            value = Mathf.Clamp01(value);
            self.ImageProgress.transform.localScale = new Vector3(value, 1f, 1f);
            self.Text_Progress.GetComponent<Text>().text = $"{rechargeNum}/{needNumber}";
            self.Btn_GetReward.SetActive(!activityComponent.ActivityReceiveIds.Contains(self.CurActivityId));
            self.ImageReceived.SetActive(activityComponent.ActivityReceiveIds.Contains(self.CurActivityId));

            UICommonHelper.DestoryChild(self.ItemListNode);
            UICommonHelper.ShowItemList(activityConfig.Par_3, self.ItemListNode, self, 1f);

            int selId = activityComponent.GetCurActivityId(rechargeNum);
            int maxId = ActivityHelper.GetMaxActivityId(101);
            int minId = ActivityHelper.GetMinActivityId(101);
            self.ButtonLeft.SetActive(self.CurActivityId > minId);

            //Log.Info("selId = " + selId);

            int addNum = 4;
            int chaValue = 30007 - selId;
            if (chaValue >= 0) {
                if (addNum < chaValue) {
                    addNum = chaValue;
                }
            }
            //Log.Info("addNum = " + addNum + "  chaValue = " + chaValue + "  maxId = " + maxId + " self.CurActivityId =" + self.CurActivityId);
            self.ButtonRight.SetActive(self.CurActivityId < (selId + addNum) && self.CurActivityId < maxId);
        }

    }

}
