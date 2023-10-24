﻿using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{

    public class UIChouKaRewardComponent : Entity, IAwake, IDestroy
    {
        public GameObject TextTitle;
        public GameObject Btn_Close;
        public GameObject RewardListNode;
        public GameObject UIChouKaRewardItem;
    }


    public class UIChouKaRewardComponentAwakeSystem : AwakeSystem<UIChouKaRewardComponent>
    {
        public override void Awake(UIChouKaRewardComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.TextTitle = rc.Get<GameObject>("TextTitle");
            self.Btn_Close = rc.Get<GameObject>("Btn_Close");
            self.Btn_Close.GetComponent<Button>().onClick.AddListener(() => { UIHelper.Remove(self.ZoneScene(), UIType.UIChouKaReward); } );

            self.RewardListNode = rc.Get<GameObject>("RewardListNode");
            self.UIChouKaRewardItem = rc.Get<GameObject>("UIChouKaRewardItem");
            self.UIChouKaRewardItem.SetActive(false);

            self.OnInitUI().Coroutine();
        }
    }

    public static class UIChouKaRewardComponentSystem
    {
        public static async ETTask OnInitUI(this UIChouKaRewardComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene( self.ZoneScene() );
            self.TextTitle.GetComponent<Text>().text = $"今日探宝次数:{unit.GetComponent<NumericComponent>().GetAsInt(NumericType.ChouKa)}";
            
            List<TakeCardRewardConfig> takeCardRewardConfigs = TakeCardRewardConfigCategory.Instance.GetAll().Values.ToList();
            for (int i = 0; i < takeCardRewardConfigs.Count; i++)
            {
                GameObject gameObject = GameObject.Instantiate(self.UIChouKaRewardItem);
                gameObject.SetActive(true);
                UICommonHelper.SetParent( gameObject, self.RewardListNode);
                UIChouKaRewardItemComponent itemComponent = self.AddChild<UIChouKaRewardItemComponent, GameObject>(gameObject);
                itemComponent.OnUpdateUI(takeCardRewardConfigs[i]);
            }
        }
    }
}
