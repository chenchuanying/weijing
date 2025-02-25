using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{

    public class UIPetMiningChallengeComponent : Entity, IAwake,IDestroy
    {

        public GameObject TextChallengeTime;
        public GameObject ButtonReset;
        public GameObject TextChallengeCD;
        public GameObject RawImage;
        public GameObject ButtonClose;
        public GameObject ButtonConfirm;

        public GameObject Text_ming;
        public GameObject Text_player;
        public GameObject Text_chanchu;

        public GameObject TeamListNode;
        public GameObject DefendTeam;

        public PetMingPlayerInfo PetMingPlayerInfo;

        public List<Image> PetIconList = new List<Image>();
        public List<UIPetMiningChallengeTeamComponent> ChallengeTeamList = new List<UIPetMiningChallengeTeamComponent>();   

        public int MineTpe;
        public int Position;
        public int TeamId;
        
        public List<string> AssetPath = new List<string>();
    }

    public class UIPetMiningChallengeComponentAwake : AwakeSystem<UIPetMiningChallengeComponent>
    {
        public override void Awake(UIPetMiningChallengeComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.ButtonClose = rc.Get<GameObject>("ButtonClose");
            self.ButtonClose.GetComponent<Button>().onClick.AddListener(() => { UIHelper.Remove(self.ZoneScene(), UIType.UIPetMiningChallenge); });

            self.TextChallengeCD = rc.Get<GameObject>("TextChallengeCD");
            self.TextChallengeCD.GetComponent<Text>().text = string.Empty;

            self.ButtonConfirm = rc.Get<GameObject>("ButtonConfirm");
            ButtonHelp.AddListenerEx(self.ButtonConfirm, () => { self.OnButtonConfirm(); });

            self.RawImage = rc.Get<GameObject>("RawImage");
            self.Text_ming = rc.Get<GameObject>("Text_ming");
            self.Text_player = rc.Get<GameObject>("Text_player");
            self.Text_chanchu = rc.Get<GameObject>("Text_chanchu");
            self.ChallengeTeamList.Clear();

            self.DefendTeam = rc.Get<GameObject>("DefendTeam");
            self.PetIconList.Clear();

            self.TextChallengeTime = rc.Get<GameObject>("TextChallengeTime");

            self.ButtonReset = rc.Get<GameObject>("ButtonReset");
            ButtonHelp.AddListenerEx(self.ButtonReset, self.OnButtonReset);

            GameObject gameObject_0 = self.DefendTeam.transform.Find($"PetIcon_{0}").gameObject;
            gameObject_0.GetComponent<Button>().onClick.AddListener(() =>{  self.RequestPetInfo(0).Coroutine();  });
            self.PetIconList.Add(gameObject_0.GetComponent<Image>());

            GameObject gameObject_1= self.DefendTeam.transform.Find($"PetIcon_{1}").gameObject;
            gameObject_1.GetComponent<Button>().onClick.AddListener(() => { self.RequestPetInfo(1).Coroutine(); });
            self.PetIconList.Add(gameObject_1.GetComponent<Image>());

            GameObject gameObject_2 = self.DefendTeam.transform.Find($"PetIcon_{2}").gameObject;
            gameObject_2.GetComponent<Button>().onClick.AddListener(() => { self.RequestPetInfo(2).Coroutine(); });
            self.PetIconList.Add(gameObject_2.GetComponent<Image>());

            GameObject gameObject_3 = self.DefendTeam.transform.Find($"PetIcon_{3}").gameObject;
            gameObject_3.GetComponent<Button>().onClick.AddListener(() => { self.RequestPetInfo(3).Coroutine(); });
            self.PetIconList.Add(gameObject_3.GetComponent<Image>());

            GameObject gameObject_4 = self.DefendTeam.transform.Find($"PetIcon_{4}").gameObject;
            gameObject_4.GetComponent<Button>().onClick.AddListener(() => { self.RequestPetInfo(4).Coroutine(); });
            self.PetIconList.Add(gameObject_4.GetComponent<Image>());

            self.TeamId = -1;
            self.TeamListNode = rc.Get<GameObject>("TeamListNode");
            self.ChallengeTeamList.Clear();

            UI uI = UIHelper.GetUI(self.ZoneScene(), UIType.UIPetSet);
            UIPetMiningComponent petmingComponent = uI.GetComponent<UIPetSetComponent>().UIPageView.UISubViewList[(int)PetSetEnum.PetMining].GetComponent<UIPetMiningComponent>();
            List<int> defendteamids = petmingComponent.GetSelfPetMingTeam();
            for (int i = 0; i < 3; i++)
            {
                GameObject gameObject = self.TeamListNode.transform.GetChild(i).gameObject;
                UIPetMiningChallengeTeamComponent uIPetMining = self.AddChild<UIPetMiningChallengeTeamComponent, GameObject>(gameObject);
                uIPetMining.SelectHandler = self.OnSelectTeam;
                uIPetMining.TeamId = i;
                uIPetMining.OnUpdateUI(defendteamids.Contains(i));
                self.ChallengeTeamList.Add(uIPetMining);
            }
        }
    }
    public class UIPetMiningChallengeComponentDestroy : DestroySystem<UIPetMiningChallengeComponent>
    {
        public override void Destroy(UIPetMiningChallengeComponent self)
        {
            for(int i = 0; i < self.AssetPath.Count; i++)
            {
                if (!string.IsNullOrEmpty(self.AssetPath[i]))
                {
                    ResourcesComponent.Instance.UnLoadAsset(self.AssetPath[i]); 
                }
            }
            self.AssetPath = null;
        }
    }
    public static class UIPetMiningChallengeComponentSystem
    {

        public static  void OnButtonReset(this UIPetMiningChallengeComponent self)
        {
            PopupTipHelp.OpenPopupTip(self.ZoneScene(), "重置挑战", "是否花费350钻石重置5次挑战次数？/n提示:挑战次数上限为10", () =>
            {
                self.RequestPetMingReset().Coroutine();
            }, null).Coroutine();
        }

        public static async ETTask RequestPetMingReset(this UIPetMiningChallengeComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene() );
            if (unit.GetComponent<NumericComponent>().GetAsInt(NumericType.PetMineReset) >= 3)
            {
                FloatTipManager.Instance.ShowFloatTip("每天最多只能重置3次！");
                return;
            }

            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            if (userInfoComponent.UserInfo.Diamond < 350)
            {
                FloatTipManager.Instance.ShowFloatTip("钻石不足！");
                return;
            }

            long instanceid = self.InstanceId;
            int errorCode = await NetHelper.RequestPetMingReset( self.ZoneScene() );
            if (instanceid != self.InstanceId || errorCode != ErrorCode.ERR_Success)
            {
                return;
            }

            self.UpdateChallengeTime();
        }

        public static void UpdateChallengeTime(this UIPetMiningChallengeComponent self)
        {
            int sceneid = BattleHelper.GetSceneIdByType( SceneTypeEnum.PetMing );
            SceneConfig sceneConfig = SceneConfigCategory.Instance.Get( sceneid );

            int useTime = (int)self.ZoneScene().GetComponent<UserInfoComponent>().GetSceneFubenTimes(sceneid);
            self.TextChallengeTime.GetComponent<Text>().text = $"挑战剩余次数:{sceneConfig.DayEnterNum - useTime}/10";
        }

        public static async ETTask RequestPetInfo(this UIPetMiningChallengeComponent self, int index)
        {
            if (self.PetMingPlayerInfo == null)
            {
                return;
            }

            long instanceid = self.InstanceId;
            long untiid = self.PetMingPlayerInfo.UnitId;
            long petid = self.PetMingPlayerInfo.PetIdList[index];
            C2F_WatchPetRequest     request     = new C2F_WatchPetRequest() { UnitID = untiid, PetId = petid };
            F2C_WatchPetResponse response = (F2C_WatchPetResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call( request );
            if (instanceid != self.InstanceId)
            {
                return;
            }

            if (response.RolePetInfos == null)
            {
                FloatTipManager.Instance.ShowFloatTip("查看宠物信息出错！");
                return;
            }

            UI uI = await UIHelper.Create( self.ZoneScene(), UIType.UIPetInfo );
            if (instanceid != self.InstanceId)
            {
                return;
            }

            uI.GetComponent<UIPetInfoComponent>().OnUpdateUI(response.RolePetInfos, response.PetHeXinList, response.Ks, response.Vs);
        }

        public static void OnSelectTeam(this UIPetMiningChallengeComponent self, int teamid)
        {
            self.TeamId = teamid;
            for (int i = 0; i < self.ChallengeTeamList.Count; i++)
            {
                self.ChallengeTeamList[i].ImageSelect.SetActive( teamid == i );
                self.ChallengeTeamList[i].ButtonSelect.SetActive(teamid != i);
            }
        }

        public static void OnInitUI(this UIPetMiningChallengeComponent self, int mineType, int position, PetMingPlayerInfo petMingPlayerInfo)
        {

            UI uI = UIHelper.GetUI( self.ZoneScene(), UIType.UIPetSet );
            UIPetMiningComponent uIPetMining = uI.GetComponent<UIPetSetComponent>().UIPageView.UISubViewList[(int)PetSetEnum.PetMining].GetComponent<UIPetMiningComponent>();

            self.MineTpe = mineType;
            self.Position = position;   
            MineBattleConfig mineBattleConfig = MineBattleConfigCategory.Instance.Get(mineType);
            string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.OtherIcon, mineBattleConfig.Icon);
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.RawImage.GetComponent<Image>().sprite = sp;
            self.Text_ming.GetComponent<Text>().text = mineBattleConfig.Name;

            int zone = self.ZoneScene().GetComponent<AccountInfoComponent>().ServerId;
            int openDay = ServerHelper.GetOpenServerDay(false, zone);
            float coffi = ComHelp.GetMineCoefficient(openDay, mineType, position, uIPetMining.PetMineExtend);
            int chanchu = (int)(mineBattleConfig.GoldOutPut * coffi);


            self.Text_chanchu.GetComponent<Text>().text = $"产出:{chanchu}小时";

            self.PetMingPlayerInfo = petMingPlayerInfo;
            string playerName = string.Empty;  
            List<int> confids = new List<int>();
            if (petMingPlayerInfo != null)
            {
                playerName = $"占领者:{petMingPlayerInfo.PlayerName}";
                confids = petMingPlayerInfo.PetConfig;
            }
            else
            {
                playerName = "占领者:无";
            }
            self.Text_player.GetComponent<Text>().text = playerName;
            for (int i = 0; i< self.PetIconList.Count; i++)
            {
                if (i >= confids.Count || confids[i] == 0)
                {
                    self.PetIconList[i].gameObject.SetActive(false);
                }
                else
                {
                    self.PetIconList[i].gameObject.SetActive(true);
                    PetConfig petConfig = PetConfigCategory.Instance.Get(confids[i]);
                    string _path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.PetHeadIcon, petConfig.HeadIcon);
                    Sprite _sp = ResourcesComponent.Instance.LoadAsset<Sprite>(_path);
                    if (!self.AssetPath.Contains(_path))
                    {
                        self.AssetPath.Add(_path);
                    }
                    self.PetIconList[i].sprite = _sp;
                }
            }

            self.ShowChallengeCD().Coroutine();

            self.UpdateChallengeTime();
        }

        public static async ETTask ShowChallengeCD(this UIPetMiningChallengeComponent self)
        {
            long instanceid = self.InstanceId;
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            long cdTime = unit.GetComponent<NumericComponent>().GetAsLong(NumericType.PetMineCDTime) - TimeHelper.ServerNow();

            while (cdTime > 0)
            {
                self.TextChallengeCD.GetComponent<Text>().text = "挑战冷却时间: "+ TimeHelper.ShowLeftTime(cdTime);   
                await TimerComponent.Instance.WaitAsync(1000);
                if (instanceid != self.InstanceId)
                {
                    break;
                }

                cdTime -= 1000;
            }
        }

        public static   void OnButtonConfirm(this UIPetMiningChallengeComponent self)
        {
            if (self.TeamId == -1)
            {
                FloatTipManager.Instance.ShowFloatTip("请选择一个队伍！");
                return;
            }
            Unit unit = UnitHelper.GetMyUnitFromZoneScene( self.ZoneScene() );
            long cdTime = unit.GetComponent<NumericComponent>().GetAsLong( NumericType.PetMineCDTime );
            if (cdTime > TimeHelper.ServerNow())
            {
                FloatTipManager.Instance.ShowFloatTip("挑战冷却中！");
                return;
            }

            Scene zoneScene = self.ZoneScene();
            int sceneid = BattleHelper.GetSceneIdByType( SceneTypeEnum.PetMing );
            
            EnterFubenHelp.RequestTransfer(zoneScene, SceneTypeEnum.PetMing, sceneid, self.MineTpe, $"{self.Position}_{self.TeamId}").Coroutine();
            UIHelper.Remove( zoneScene, UIType.UIPetMiningChallenge );
            UIHelper.Remove(zoneScene, UIType.UIPetSet);
        }
    }
}