﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIRandomTowerComponent : Entity, IAwake
    {
        public GameObject ListNode;
        public GameObject UIRandomTowerItem;
        public GameObject ButtonEnter;
        public GameObject Text_LayerNum;
        public GameObject Text_OpenTime;
    }


    public class UIRandomTowerComponentAwakeSystem : AwakeSystem<UIRandomTowerComponent>
    {
        public override void Awake(UIRandomTowerComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.ListNode = rc.Get<GameObject>("ListNode");
            self.UIRandomTowerItem = rc.Get<GameObject>("UIRandomTowerItem");
            self.UIRandomTowerItem.SetActive(false);

            self.ButtonEnter = rc.Get<GameObject>("ButtonEnter");
            ButtonHelp.AddListenerEx( self.ButtonEnter, () => { self.OnButtonEnter().Coroutine(); } );

            self.Text_LayerNum = rc.Get<GameObject>("Text_LayerNum");
            self.Text_OpenTime = rc.Get<GameObject>("Text_OpenTime");

            self.OnInitUI();
        }
    }

    public static class UIRandomTowerComponentSystem
    {

        public static async ETTask OnButtonEnter(this UIRandomTowerComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int randowTowerId = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.RandomTowerId);
            if (TowerHelper.GetLastTowerIdByScene(SceneTypeEnum.RandomTower) == randowTowerId)
            {
                FloatTipManager.Instance.ShowFloatTip("已通关！");
                return;
            }

            int errorCode = await EnterFubenHelp.RequestTransfer(self.ZoneScene(), (int)SceneTypeEnum.RandomTower, BattleHelper.GetSceneIdByType(SceneTypeEnum.RandomTower));
            if (errorCode != ErrorCode.ERR_Success)
            {
                return;
            }
            UIHelper.Remove( self.ZoneScene(), UIType.UIRandomTower );
        }

        public static  void OnInitUI(this UIRandomTowerComponent self)
        {
            
            List<TowerConfig> towerRewardConfigs = TowerConfigCategory.Instance.GetAll().Values.ToList();
            for (int i = 0; i < towerRewardConfigs.Count; i++)
            {
                TowerConfig towerConfig = towerRewardConfigs[i];
                if (towerConfig.MapType!=SceneTypeEnum.RandomTower)
                {
                    continue;
                }
                if (ComHelp.IfNull(towerConfig.DropShow))
                {
                    continue;
                }
                GameObject bagSpace = GameObject.Instantiate(self.UIRandomTowerItem);
                bagSpace.SetActive(true);
                UIRandomTowerItemComponent uITowerItemComponent = self.AddChild<UIRandomTowerItemComponent, GameObject>(bagSpace);
                uITowerItemComponent.OnInitUI(towerRewardConfigs[i]);
                UICommonHelper.SetParent(bagSpace, self.ListNode);
            }

            Unit unit = UnitHelper.GetMyUnitFromZoneScene( self.ZoneScene() );
            int towerId = unit.GetComponent<NumericComponent>().GetAsInt( NumericType.RandomTowerId );
            if (towerId == 0)
            {
                self.Text_LayerNum.GetComponent<Text>().text = "0";
            }
            else
            {
                self.Text_LayerNum.GetComponent<Text>().text = TowerConfigCategory.Instance.Get(towerId).CengNum.ToString() ;
            }
        }
    }
}