﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ET
{
    public class UIRankPetComponent : Entity, IAwake
    {
        public GameObject UIRankPetItem;
        public GameObject PetListNode;
        public GameObject Button_Add;
        public GameObject Button_Refresh;
        public GameObject Button_Reward;
        public GameObject Button_Team;
        public GameObject Text_LeftTime;
        public GameObject Text_Rank;

        public List<UI> PetUIList;
    }


    public class UIRankPetComponentAwakeSystem : AwakeSystem<UIRankPetComponent>
    {
        public override void Awake(UIRankPetComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.PetUIList = new List<UI>();

            self.UIRankPetItem = rc.Get<GameObject>("UIRankPetItem");
            self.UIRankPetItem.SetActive(false);
            self.PetListNode = rc.Get<GameObject>("PetListNode");
            self.Button_Add = rc.Get<GameObject>("Button_Add");
            self.Button_Add.GetComponent<Button>().onClick.AddListener(() => { self.OnButton_Add(); });
            self.Button_Refresh = rc.Get<GameObject>("Button_Refresh");
            self.Button_Refresh.GetComponent<Button>().onClick.AddListener(() => { self.OnButton_Refresh(); });
            self.Button_Reward = rc.Get<GameObject>("Button_Reward");
            self.Button_Reward.GetComponent<Button>().onClick.AddListener(() => { self.OnButton_Reward(); });
            self.Button_Team = rc.Get<GameObject>("Button_Team");
            self.Button_Team.GetComponent<Button>().onClick.AddListener(() => { self.OnButton_Team().Coroutine(); });
            self.Text_LeftTime = rc.Get<GameObject>("Text_LeftTime");
            self.Text_Rank = rc.Get<GameObject>("Text_Rank");

            self.OnUpdateUI().Coroutine();
            self.OnUpdateTimes();
        }
    }

    public static class UIRankPetComponentSystem
    {
        public static async ETTask OnUpdateUI(this UIRankPetComponent self)
        {
            long instacnid = self.InstanceId;
            C2R_RankPetListRequest c2R_RankPetListRequest = new C2R_RankPetListRequest() {  UserId = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.UserId  };
            R2C_RankPetListResponse m2C_RolePetXiLian = (R2C_RankPetListResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2R_RankPetListRequest);
            if(instacnid != self.InstanceId)
            {
                return;
            }
            for (int i = 0; i < m2C_RolePetXiLian.RankPetList.Count; i++)
            {
                UI ui_1 = null;
                if (i < self.PetUIList.Count)
                {
                    ui_1 = self.PetUIList[i];
                    ui_1.GameObject.SetActive(true);
                }
                else
                {
                    GameObject skillItem = GameObject.Instantiate(self.UIRankPetItem);
                    skillItem.SetActive(true);
                    UICommonHelper.SetParent(skillItem, self.PetListNode);
                    ui_1 = self.AddChild<UI, string, GameObject>("UIRankPetItem_" + i, skillItem);
                    ui_1.AddComponent<UIRankPetItemComponent>();
                    self.PetUIList.Add(ui_1);
                }
                ui_1.GetComponent<UIRankPetItemComponent>().OnInitUI(m2C_RolePetXiLian.RankPetList[i]);
            }

            if (m2C_RolePetXiLian.RankNumber == 0)
            {
                self.Text_Rank.GetComponent<Text>().text = "无";
            }
            else
            {
                self.Text_Rank.GetComponent<Text>().text = m2C_RolePetXiLian.RankNumber.ToString();
            }
        }

        public static void OnUpdateTimes(this UIRankPetComponent self)
        {
            int sceneId = BattleHelper.GetSceneIdByType(SceneTypeEnum.PetTianTi);
            int totalTimes = SceneConfigCategory.Instance.Get(sceneId).DayEnterNum;

            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            int useTimes = (int)userInfoComponent.GetSceneFubenTimes(sceneId);
            self.Text_LeftTime.GetComponent<Text>().text = $"{totalTimes - useTimes}/{totalTimes}";
        }

        public static void OnButton_Add(this UIRankPetComponent self)
        {
            PopupTipHelp.OpenPopupTip(self.ZoneScene(), "重置次数",
                "是否花费200钻石重置次数",
                () =>
                {
                    self.RequestReset().Coroutine();
                }, null).Coroutine();
        }

        public static async ETTask RequestReset(this UIRankPetComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            long resetValue = numericComponent.GetAsLong(NumericType.FubenTimesReset);
            if ((resetValue & SceneTypeEnum.PetTianTi) > 0)
            {
                FloatTipManager.Instance.ShowFloatTip("每天只能重置一次");
                return;
            }

            C2M_FubenTimesResetRequest request  = new C2M_FubenTimesResetRequest() { SceneType = SceneTypeEnum.PetTianTi };
            M2C_FubenTimesResetResponse response = (M2C_FubenTimesResetResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            if (response.Error != 0)
            {
                return;
            }
            int sceneId = BattleHelper.GetSceneIdByType(SceneTypeEnum.PetTianTi);
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            userInfoComponent.ClearFubenTimes(sceneId);
            self.OnUpdateTimes();
        }

        public static void OnButton_Refresh(this UIRankPetComponent self)
        {
            self.OnUpdateUI().Coroutine();
        }

        public static void OnButton_Reward(this UIRankPetComponent self)
        {

        }

        public static async ETTask OnButton_Team(this UIRankPetComponent self)
        {
            Scene scene = self.DomainScene();
            UI ui = await UIHelper.Create(scene, UIType.UIPetFormation);
            ui.GetComponent<UIPetFormationComponent>().OnInitUI(SceneTypeEnum.PetTianTi);
        }
    }

}
