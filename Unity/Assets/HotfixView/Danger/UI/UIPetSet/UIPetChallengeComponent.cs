﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace ET
{
    public class UIPetChallengeComponent : Entity, IAwake
    {
        public GameObject TextTimes;
        public GameObject TextStar;
        public GameObject ButtonReward;
        public ScrollRect ScrollRect;
        public GameObject ChallengeListNode;
        public GameObject ButtonSet;
        public GameObject ButtonChallenge;
        public GameObject FormationNode;
        public UIPetFormationSetComponent UIPetFormationSet;
        public List<UIPetChallengeItemComponent> ChallengeItemList = new List<UIPetChallengeItemComponent>();

        public int PetFubenId;
        public int ShowReward;
    }


    public class UIPetChallengeComponentAwakeSystem : AwakeSystem<UIPetChallengeComponent>
    {
        public override void Awake(UIPetChallengeComponent self)
        {
            self.ChallengeItemList.Clear();
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.FormationNode = rc.Get<GameObject>("FormationNode");
            self.ChallengeListNode = rc.Get<GameObject>("ChallengeListNode");

            self.ButtonSet = rc.Get<GameObject>("ButtonSet");
            self.ButtonChallenge = rc.Get<GameObject>("ButtonChallenge");
            self.ScrollRect = rc.Get<GameObject>("ScrollRect").GetComponent<ScrollRect>();
            self.ButtonReward = rc.Get<GameObject>("ButtonReward");
            self.TextStar = rc.Get<GameObject>("TextStar");
            self.TextTimes = rc.Get<GameObject>("TextTimes");

            ButtonHelp.AddListenerEx( self.ButtonSet, () => { self.OnButtonSet().Coroutine(); } );
            ButtonHelp.AddListenerEx(self.ButtonChallenge, () => { self.OnButtonChallenge().Coroutine(); });
            //ButtonHelp.AddListenerEx(self.ButtonReward, () => { self.OnButtonReward().Coroutine(); });

            ButtonHelp.AddEventTriggers(self.ButtonReward, (PointerEventData pdata) => { self.BeginDrag(pdata).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(self.ButtonReward, (PointerEventData pdata) => { self.EndDrag(pdata); }, EventTriggerType.PointerUp);

            self.InitSubView();
            self.OnUpdateStar();
            self.InitItemList().Coroutine();
        }
    }

    public static class UIPetChallengeComponentSystem
    {

        public static async ETTask InitItemList(this UIPetChallengeComponent self)
        {
            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            Unit unitmain = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int petfubenId = petComponent.GetPetFuben();

            var path = ABPathHelper.GetUGUIPath("Main/PetSet/UIPetChallengeItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            List<PetFubenConfig> petFubenConfigs = PetFubenConfigCategory.Instance.GetAll().Values.ToList();
            int index = -1;
            for (int i = 0; i < petFubenConfigs.Count; i++)
            {
                GameObject gameObject = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent( gameObject, self.ChallengeListNode);
                UIPetChallengeItemComponent ChallengeItem = self.AddChild<UIPetChallengeItemComponent, GameObject>(gameObject);
                bool locked = i > 0 && petFubenConfigs[i].Id - petfubenId >= 2;
                ChallengeItem.OnUpdateUI(petFubenConfigs[i], i, petComponent.GetFubenStar(petFubenConfigs[i].Id), locked);
                ChallengeItem.SetClickHandler(self.OnClickChallengeItem);
                self.ChallengeItemList.Add(ChallengeItem);
                if (petFubenConfigs[i].Id == petfubenId)
                {
                    index = i;
                }
                if (petFubenConfigs.Count - 1 == i)
                {
                    ChallengeItem.HideLines();
                }
            }
            if (index != petFubenConfigs.Count - 1)
            {
                index++;
            }
            self.ChallengeItemList[index].OnClickChallengeItem();
            self.ChallengeListNode.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, index * 200f);
            //self.ScrollRect.normalizedPosition = new Vector2(0f, 1f- (1+index) * 1f / petFubenConfigs.Count) ;
        }

        public static void OnClickChallengeItem(this UIPetChallengeComponent self, int petfubenId)
        {
            self.PetFubenId = petfubenId;
            for (int i = 0; i < self.ChallengeItemList.Count; i++)
            {
                self.ChallengeItemList[i].SetSelected(petfubenId);
            }
        }

        public static  async ETTask OnButtonSet(this UIPetChallengeComponent self)
        {
            Scene scene = self.ZoneScene();
            UI ui = await UIHelper.Create(scene, UIType.UIPetFormation );
            ui.GetComponent<UIPetFormationComponent>().OnInitUI(SceneTypeEnum.PetDungeon, self.UpdateFormationSet);
        }

        public static void UpdateFormationSet(this UIPetChallengeComponent self)
        {
            self.UIPetFormationSet.OnUpdateFormation(SceneTypeEnum.PetDungeon,
                self.ZoneScene().GetComponent<PetComponent>().PetFormations, false).Coroutine();
        }

        public static void InitSubView(this UIPetChallengeComponent self)
        {
            var path = ABPathHelper.GetUGUIPath("Main/PetSet/UIPetFormationSet");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject go = GameObject.Instantiate(bundleGameObject);
            UICommonHelper.SetParent(go, self.FormationNode);
            self.UIPetFormationSet = self.AddChild<UIPetFormationSetComponent, GameObject>(go);
            self.UIPetFormationSet.OnUpdateFormation(SceneTypeEnum.PetDungeon,
                self.ZoneScene().GetComponent<PetComponent>().PetFormations, false).Coroutine();

            int sceneId = BattleHelper.GetSceneIdByType(SceneTypeEnum.PetDungeon);
            SceneConfig sceneConfig = SceneConfigCategory.Instance.Get(sceneId);
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            self.TextTimes.GetComponent<Text>().text = $"{userInfoComponent.GetSceneFubenTimes(sceneId)}/{sceneConfig.DayEnterNum}";
        }

        public static async ETTask BeginDrag(this UIPetChallengeComponent self, PointerEventData pdata)
        {
            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            //int canRewardId = petComponent.GetCanRewardId();
            //if (canRewardId == 0)
            //{
            //    FloatTipManager.Instance.ShowFloatTip("当前没有可以领取的奖励");
            //    return;
            //}
            
            UI skillTips = await UIHelper.Create(self.DomainScene(), UIType.UICountryTips);
            Vector2 localPoint;
            RectTransform canvas = UIEventComponent.Instance.UILayers[(int)UILayer.Mid].gameObject.GetComponent<RectTransform>();
            Camera uiCamera = self.DomainScene().GetComponent<UIComponent>().UICamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pdata.position, uiCamera, out localPoint);
            PetFubenRewardConfig shouJiConfig = PetFubenRewardConfigCategory.Instance.Get(self.ShowReward);
            string rewards = shouJiConfig.RewardItems;
            skillTips.GetComponent<UICountryTipsComponent>().OnUpdateUI(rewards, new Vector3(localPoint.x, localPoint.y + 50f, 0f), 1);
        }

        public static async void EndDrag(this UIPetChallengeComponent self, PointerEventData pdata)
        {
            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            int canRewardId = petComponent.GetCanRewardId();
            if (canRewardId == 0)
            {
                return;
            }

            long instanceid = self.InstanceId;
            C2M_PetFubenRewardRequest request = new C2M_PetFubenRewardRequest();
            M2C_PetFubenRewardResponse response = (M2C_PetFubenRewardResponse) await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            if (instanceid != self.InstanceId)
            {
                return;
            }
            if (response.Error == ErrorCode.ERR_Success)
            {
                petComponent.PetFubeRewardId = canRewardId;
                self.OnUpdateStar();
            }
            else
            {
                await NetHelper.RequestAllPets(self.ZoneScene());
                self.OnUpdateStar();
            }
            
            UIHelper.Remove(self.DomainScene(), UIType.UICountryTips);
        }

        public static async ETTask OnButtonChallenge(this UIPetChallengeComponent self)
        {
            bool locked = false;
            for (int i = 0; i < self.ChallengeItemList.Count; i++)
            {
                if (self.ChallengeItemList[i].PetFubenId == self.PetFubenId)
                {
                    locked = self.ChallengeItemList[i].Node_2.activeSelf;
                }
            }
            if (locked)
            {
                FloatTipManager.Instance.ShowFloatTipDi("关卡未解锁！");
                return;
            }
            bool havepet = false;
            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            for (int i = 0; i < petComponent.PetFormations.Count; i++)
            {
                if (petComponent.PetFormations[i] != 0)
                {
                    havepet = true;
                    break;
                }
            }
            if (!havepet)
            {
                FloatTipManager.Instance.ShowFloatTipDi("请设置上阵宠物！");
                return;
            }

            int errorCode = await EnterFubenHelp.RequestTransfer(self.ZoneScene(), (int)SceneTypeEnum.PetDungeon, BattleHelper.GetSceneIdByType(SceneTypeEnum.PetDungeon), 0, self.PetFubenId.ToString());
            if (errorCode != ErrorCode.ERR_Success)
            {
                return;
            }
            UIHelper.Remove(self.ZoneScene(), UIType.UIPetSet);
        }

        public static void OnUpdateStar(this UIPetChallengeComponent self)
        {
            List<PetFubenRewardConfig> petFubenRewardConfigs = PetFubenRewardConfigCategory.Instance.GetAll().Values.ToList();
            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            int canrewardId = petComponent.GetCanRewardId();
            int fubenreward = petComponent.PetFubeRewardId;

            int rewardid = 0;
            if (fubenreward == 0 && canrewardId == 0)
            {
                rewardid = petFubenRewardConfigs[0].Id;
            }
            if (fubenreward == 0 && canrewardId != 0)
            {
                rewardid = canrewardId;
            }
            if (fubenreward != 0 && canrewardId == 0)
            {
                rewardid = fubenreward + 1;
            }
            if (fubenreward != 0 && canrewardId != 0)
            {
                rewardid = canrewardId;
            }
            self.ShowReward = rewardid;
            PetFubenRewardConfig petFubenRewardConfig = PetFubenRewardConfigCategory.Instance.Get(rewardid);
            self.TextStar.GetComponent<Text>().text = $"{petComponent.GetTotalStar()}/{petFubenRewardConfig.NeedStar}";
        }
    }
}
