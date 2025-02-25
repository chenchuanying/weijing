﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    public class UIRoleAddPointComponent : Entity, IAwake<GameObject>
    {
        public GameObject RecommendAddPointBtn;
        public GameObject Lab_ShengYuNum;
        public GameObject Btn_Confirm;

        public GameObject AddProperty_LiLiang;
        public GameObject AddProperty_ZhiLi;
        public GameObject AddProperty_TiZhi;
        public GameObject AddProperty_NaiLi;
        public GameObject AddProperty_MingJie;

        public List<int> PointList = new List<int>();
        public List<int> PointInit = new List<int>();
        public int PointRemain = 0;
        public GameObject GameObject;
        public bool IsHoldDown;
    }


    public class UIRoleAddPointComponentAwakeSystem : AwakeSystem<UIRoleAddPointComponent, GameObject>
    {
        public override void Awake(UIRoleAddPointComponent self, GameObject gameObject)
        {
            self.PointList.Clear();
            self.PointInit.Clear();
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.RecommendAddPointBtn = rc.Get<GameObject>("RecommendAddPointBtn");
            self.Lab_ShengYuNum = rc.Get<GameObject>("Lab_ShengYuNum");

            self.Btn_Confirm = rc.Get<GameObject>("Btn_Confirm");
            self.RecommendAddPointBtn.GetComponent<Button>().onClick.AddListener(()=>{self.OnRecommendAddPointBtn();});
            self.Btn_Confirm.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_Confirm().Coroutine(); });

            self.AddProperty_LiLiang = rc.Get<GameObject>("AddProperty_LiLiang");
            self.AddProperty_ZhiLi = rc.Get<GameObject>("AddProperty_ZhiLi");
            self.AddProperty_TiZhi = rc.Get<GameObject>("AddProperty_TiZhi");
            self.AddProperty_NaiLi = rc.Get<GameObject>("AddProperty_NaiLi");
            self.AddProperty_MingJie = rc.Get<GameObject>("AddProperty_MingJie");

            GameObject LiLiang_Btn_Add = self.AddProperty_LiLiang.transform.Find("Btn_Add").gameObject;
            GameObject LiLiang_Btn_Cost = self.AddProperty_LiLiang.transform.Find("Btn_Cost").gameObject;
            ButtonHelp.AddEventTriggers(LiLiang_Btn_Add, (PointerEventData pdata) => { self.PointerDown_Btn_AddNum(0).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(LiLiang_Btn_Cost, (PointerEventData pdata) => { self.PointerDown_Btn_CostNum(0).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(LiLiang_Btn_Add, (PointerEventData pdata) => { self.PointerUp_Btn_AddNum(); }, EventTriggerType.PointerUp);
            ButtonHelp.AddEventTriggers(LiLiang_Btn_Cost, (PointerEventData pdata) => { self.PointerUp_Btn_AddNum(); }, EventTriggerType.PointerUp);

            GameObject ZhiLi_Btn_Add = self.AddProperty_ZhiLi.transform.Find("Btn_Add").gameObject;
            GameObject ZhiLi_Btn_Cost = self.AddProperty_ZhiLi.transform.Find("Btn_Cost").gameObject;
            ButtonHelp.AddEventTriggers(ZhiLi_Btn_Add, (PointerEventData pdata) => { self.PointerDown_Btn_AddNum(1).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(ZhiLi_Btn_Cost, (PointerEventData pdata) => { self.PointerDown_Btn_CostNum(1).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(ZhiLi_Btn_Add, (PointerEventData pdata) => { self.PointerUp_Btn_AddNum(); }, EventTriggerType.PointerUp);
            ButtonHelp.AddEventTriggers(ZhiLi_Btn_Cost, (PointerEventData pdata) => { self.PointerUp_Btn_AddNum(); }, EventTriggerType.PointerUp);

            GameObject TiZhi_Btn_Add = self.AddProperty_TiZhi.transform.Find("Btn_Add").gameObject;
            GameObject TiZhi_Btn_Cost = self.AddProperty_TiZhi.transform.Find("Btn_Cost").gameObject;
            ButtonHelp.AddEventTriggers(TiZhi_Btn_Add, (PointerEventData pdata) => { self.PointerDown_Btn_AddNum(2).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(TiZhi_Btn_Cost, (PointerEventData pdata) => { self.PointerDown_Btn_CostNum(2).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(TiZhi_Btn_Add, (PointerEventData pdata) => { self.PointerUp_Btn_AddNum(); }, EventTriggerType.PointerUp);
            ButtonHelp.AddEventTriggers(TiZhi_Btn_Cost, (PointerEventData pdata) => { self.PointerUp_Btn_AddNum(); }, EventTriggerType.PointerUp);

            GameObject NaiLi_Btn_Add = self.AddProperty_NaiLi.transform.Find("Btn_Add").gameObject;
            GameObject NaiLi_Btn_Cost = self.AddProperty_NaiLi.transform.Find("Btn_Cost").gameObject;
            ButtonHelp.AddEventTriggers(NaiLi_Btn_Add, (PointerEventData pdata) => { self.PointerDown_Btn_AddNum(3).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(NaiLi_Btn_Cost, (PointerEventData pdata) => { self.PointerDown_Btn_CostNum(3).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(NaiLi_Btn_Add, (PointerEventData pdata) => { self.PointerUp_Btn_AddNum(); }, EventTriggerType.PointerUp);
            ButtonHelp.AddEventTriggers(NaiLi_Btn_Cost, (PointerEventData pdata) => { self.PointerUp_Btn_AddNum(); }, EventTriggerType.PointerUp);

            GameObject MinJie_Btn_Add = self.AddProperty_MingJie.transform.Find("Btn_Add").gameObject;
            GameObject MinJie_Btn_Cost = self.AddProperty_MingJie.transform.Find("Btn_Cost").gameObject;
            ButtonHelp.AddEventTriggers(MinJie_Btn_Add, (PointerEventData pdata) => { self.PointerDown_Btn_AddNum(4).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(MinJie_Btn_Cost, (PointerEventData pdata) => { self.PointerDown_Btn_CostNum(4).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(MinJie_Btn_Add, (PointerEventData pdata) => { self.PointerUp_Btn_AddNum(); }, EventTriggerType.PointerUp);
            ButtonHelp.AddEventTriggers(MinJie_Btn_Cost, (PointerEventData pdata) => { self.PointerUp_Btn_AddNum(); }, EventTriggerType.PointerUp);

            self.IsHoldDown = false;
        }
    }

    public static class UIRoleAddPointComponentSystem
    {

        public static async ETTask PointerDown_Btn_AddNum(this UIRoleAddPointComponent self, int addType)
        {
            self.IsHoldDown = true;
            self.Btn_AddProprety(addType, 1);
            int interval = 0;
            while (self.IsHoldDown)
            {
                interval++;
                if (interval > 60)
                {
                    self.Btn_AddProprety(addType, 1);
                }

                await TimerComponent.Instance.WaitFrameAsync();
            }
        }

        public static async ETTask PointerDown_Btn_CostNum(this UIRoleAddPointComponent self, int addType)
        {
            self.IsHoldDown = true;
            self.Btn_AddProprety(addType, -1);
            int interval = 0;
            while (self.IsHoldDown)
            {
                interval++;
                if (interval > 60)
                {
                    self.Btn_AddProprety(addType, -1);
                }

                await TimerComponent.Instance.WaitFrameAsync();
            }
        }

        public static void PointerUp_Btn_AddNum(this UIRoleAddPointComponent self)
        {
            self.IsHoldDown = false;
        }

        public static void OnBtn_Close(this UIRoleAddPointComponent self)
        {
            self.GameObject.SetActive(false);
        }

        public static void Btn_AddProprety(this UIRoleAddPointComponent self, int addType, int value)
        {
            if (self.PointRemain <= 0 && value > 0)
            {
                return;
            }
            int typeValue = self.PointList[addType];
            if (typeValue <= self.PointInit[addType] && value < 0)
            {
                return;
            }
            self.PointRemain += (value * -1);
            self.PointList[addType] += value;
            self.OnUpdateUI();
        }

        public static int GetNumericValue(this UIRoleAddPointComponent self, NumericComponent numericComponent,  int addType)
        {
            int typeValue = 0;
            switch (addType)
            {
                case 0:
                    typeValue = numericComponent.GetAsInt(NumericType.PointLiLiang);
                    break;
                case 1:
                    typeValue = numericComponent.GetAsInt(NumericType.PointZhiLi);
                    break;
                case 2:
                    typeValue = numericComponent.GetAsInt(NumericType.PointTiZhi);
                    break;
                case 3:
                    typeValue = numericComponent.GetAsInt(NumericType.PointNaiLi);
                    break;
                case 4:
                    typeValue = numericComponent.GetAsInt(NumericType.PointMinJie);
                    break;
            }
            return typeValue;
        }

        public static async ETTask OnBtn_Confirm(this UIRoleAddPointComponent self)
        {
            long instanceId = self.InstanceId;
            C2M_RoleAddPointRequest request = new C2M_RoleAddPointRequest()
            {
                PointList = self.PointList,
            };
            M2C_RoleAddPointResponse response = (M2C_RoleAddPointResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            if (instanceId != self.InstanceId)
            {
                return;
            }
            self.OnInitUI();
        }

        public static void OnRecommendAddPointBtn(this UIRoleAddPointComponent self)
        {
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            int occ = userInfoComponent.UserInfo.Occ;
            int occTwo = userInfoComponent.UserInfo.OccTwo;

            // 按比例推荐加点 可以在OccupationConfig和OccupationTwoConfig加个这样的字段
            string recommendAddPoint = string.Empty;
            if (ConfigHelper.RecommendAddPoint.ContainsKey(occTwo))
            {
                recommendAddPoint = ConfigHelper.RecommendAddPoint[occTwo];
            }else if (ConfigHelper.RecommendAddPoint.ContainsKey(occ))
            {
                recommendAddPoint = ConfigHelper.RecommendAddPoint[occ];
            }
            else
            {
                return;
            }

            string[] str = recommendAddPoint.Split('@');
            int all = 0;
            List<int> points = new List<int>();
            foreach (string s in str)
            {
                all += int.Parse(s);
                points.Add(int.Parse(s));
            }

            int red = 0;
            for (int i = 0; i < 5; i++)
            {
                int add =  self.PointRemain / all * points[i];
                red += add;
                self.PointList[i] += add;
            }

            self.PointRemain -= red;
            self.OnUpdateUI();
        }

        public static void OnUpdateUI(this UIRoleAddPointComponent self)
        {
            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;

            self.Lab_ShengYuNum.GetComponent<Text>().text = self.PointRemain.ToString();
            self.OnUpdateItem(self.AddProperty_LiLiang, self.PointList[0], userInfo.Lv);
            self.OnUpdateItem(self.AddProperty_ZhiLi, self.PointList[1], userInfo.Lv);
            self.OnUpdateItem(self.AddProperty_TiZhi, self.PointList[2], userInfo.Lv);
            self.OnUpdateItem(self.AddProperty_NaiLi, self.PointList[3], userInfo.Lv);
            self.OnUpdateItem(self.AddProperty_MingJie, self.PointList[4], userInfo.Lv);
        }

        public static void OnInitUI(this UIRoleAddPointComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene( self.ZoneScene() );
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            self.PointList.Clear();
            self.PointList.Add(self.GetNumericValue(numericComponent, 0));
            self.PointList.Add(self.GetNumericValue(numericComponent, 1));
            self.PointList.Add(self.GetNumericValue(numericComponent, 2));
            self.PointList.Add(self.GetNumericValue(numericComponent, 3));
            self.PointList.Add(self.GetNumericValue(numericComponent, 4));
            
            self.PointInit.Clear();
            self.PointInit.AddRange(self.PointList);
            self.PointRemain = numericComponent.GetAsInt(NumericType.PointRemain);

            self.OnUpdateUI();
        }

        public static void OnUpdateItem(this UIRoleAddPointComponent self, GameObject gameObject, int number, int level)
        {
            gameObject.transform.Find("Lab_Value").GetComponent<Text>().text = (number + level * 2).ToString();
        }
    }

}
