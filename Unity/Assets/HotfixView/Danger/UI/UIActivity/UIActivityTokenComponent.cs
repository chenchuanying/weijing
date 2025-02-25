﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

namespace ET
{
    public class UIActivityTokenComponent : Entity, IAwake, IDestroy
    {

        public GameObject Btn_GoPay;
        public GameObject TextRecharge;
        public GameObject ItemNodeList;

        public string AssetPath = string.Empty;
    }

    public class UIActivityTokenComponentDestroy : DestroySystem<UIActivityTokenComponent>
    {
        public override void Destroy(UIActivityTokenComponent self)
        {
            if (!string.IsNullOrEmpty(self.AssetPath))
            {
                ResourcesComponent.Instance.UnLoadAsset(self.AssetPath);
            }
        }
    }

    public class UIActivityTokenComponentAwakeSystem : AwakeSystem<UIActivityTokenComponent >
    {
        public override void Awake(UIActivityTokenComponent  self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.AssetPath = string.Empty;
            self.TextRecharge = rc.Get<GameObject>("TextRecharge");
            self.ItemNodeList = rc.Get<GameObject>("ItemNodeList");
            self.Btn_GoPay = rc.Get<GameObject>("Btn_GoPay");
            ButtonHelp.AddListenerEx( self.Btn_GoPay, () => { self.OnBtn_GoPay();  } );

            self.OnInitUI().Coroutine();
        }
    }

    public static class UIActivityTokenComponentSystem
    {

        public static void OnBtn_GoPay(this UIActivityTokenComponent  self)
        {
            UIHelper.Create(self.ZoneScene(), UIType.UIRecharge).Coroutine() ;
            UIHelper.Remove( self.ZoneScene(), UIType.UIActivity );
        }

        public static async ETTask OnInitUI(this UIActivityTokenComponent  self)
        {
            long instanceid = self.InstanceId;
            var path =  ABPathHelper.GetUGUIPath("Main/Activity/UIActivityTokenItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            if (instanceid != self.InstanceId)
            {
                return;
            }
            self.AssetPath = path;
            List<ActivityConfig> activityConfigs = ActivityConfigCategory.Instance.GetAll().Values.ToList();
            for (int i = 0; i < activityConfigs.Count; i++)
            {
                if (activityConfigs[i].ActivityType != 24)
                {
                    continue;
                }

                GameObject bagSpace = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(bagSpace, self.ItemNodeList);

                UI ui_item = self.AddChild<UI, string, GameObject>( "UIItem_" + i.ToString(), bagSpace);
                UIActivityTokenItemComponent uIItemComponent = ui_item.AddComponent<UIActivityTokenItemComponent>();

                uIItemComponent.OnInitUI(activityConfigs[i]);
            }

            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int selfRechage = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.RechargeNumber);
            //self.ZoneScene().GetComponent<AccountInfoComponent>().GetRechargeNumber(userInfoComponent.UserInfo.UserId);
            self.TextRecharge.GetComponent<Text>().text = $"当前额度：{selfRechage}/298";
            self.TextRecharge.SetActive(selfRechage < 298);
        }
    }
}
