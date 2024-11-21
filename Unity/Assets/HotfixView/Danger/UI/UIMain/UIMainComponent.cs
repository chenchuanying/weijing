using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    [Timer(TimerType.UIMainTimer)]
    public class UIMainTimer : ATimer<UIMainComponent>
    {
        public override void Run(UIMainComponent self)
        {
            try
            {
                self.OnCheckFuntionButton();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    [Timer(TimerType.UIMainFPSTimer)]
    public class UIMainFPSTimer : ATimer<UIMainComponent>
    {
        public override void Run(UIMainComponent self)
        {
            try
            {
                self.UpdatePing();
                self.UpdateMessage();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    public class UIMainComponent : Entity, IAwake, IDestroy
    {
        public GameObject DragPanel;
        public GameObject Button_ActivityV1;
        public GameObject Button_RechargeReward;
        public GameObject Button_ZhanKai;
        public GameObject Button_Welfare;
        public GameObject Button_Season;
        public GameObject Btn_RerurnDungeon;
        public GameObject Btn_MapTransfer;
        public GameObject ShrinkBtn;
        public GameObject Button_Demon;
        public GameObject Button_RunRace;
        public GameObject Button_Happy;
        public GameObject Btn_Union;
        public GameObject Button_Hunt;
        public GameObject Button_Solo;
        public GameObject Button_Donation;
        public GameObject Btn_Auction;
        public GameObject Button_JiaYuan;
        public GameObject DoMoveLeft;
        public GameObject DoMoveRight;
        public GameObject DoMoveBottom;
        public GameObject Button_NewYear;
        public GameObject Button_FenXiang;
        public GameObject FunctionSetBtn;
        public GameObject Button_Horse;
        public GameObject Button_CityHorse;
        public GameObject Button_WorldLv;
        public GameObject Button_ZhenYing;
        public GameObject Button_HongBao;
        public GameObject Btn_PetFormation;
        public GameObject Btn_GM;
        public GameObject Btn_Task;
        public Text TextPing;
        public Text TextMessage;
        public GameObject Btn_LvReward;
        public GameObject Btn_KillMonsterReward;
        public GameObject MailHintTip;
        public GameObject UIStall;
        public GameObject Btn_Friend;
        public GameObject TeamDungeonBtn;
        public GameObject Btn_HuoDong;
        public GameObject Button_ZhanQu;
        public GameObject Fps;
        public GameObject Button_Energy;
        public GameObject LeftBottomBtns;
        public GameObject Btn_PaiMaiHang;
        public GameObject Btn_EveryTask;
        public GameObject bagButton;
        public GameObject HomeButton;
        public GameObject UIMainSkill;
        public GameObject buttonReturn;
        public GameObject chengjiuButton;
        public GameObject adventureBtn;
        public GameObject duihuaButton;
        public GameObject petButton;
        public GameObject roleSkillBtn;
        public GameObject miniMapButton;
        public GameObject LevelGuideMini;
        public GameObject Obj_Img_ExpPro;
        public GameObject Obj_Lab_ExpValue;
        public GameObject Obj_Btn_ShouSuo;
        public GameObject Btn_Battle;
        public GameObject Btn_TopRight_2;
        public GameObject Btn_TopRight_1;
        public GameObject Button_Recharge;
        public GameObject Btn_Rank;
        public GameObject UGuaJiSet;
        public GameObject Btn_StopGuaJi;
        public UIMainChatComponent UIMainChat;
        public UIMainTaskComponent UIMainTask;
        public UIMapMiniComponent UIMapMini;
        public UIRoleHeadComponent UIRoleHead;
        public UIMainHpBarComponent UIMainHpBar;
        public UIMainTeamComponent UIMainTeam;
        public UIPageButtonComponent UIPageButtonComponent;
        public UIMainBuffComponent UIMainBuffComponent;
        public UIJoystickMoveComponent UIJoystickMoveComponent;
        public UIMainSkillComponent UIMainSkillComponent;
        public UIOpenBoxComponent UIOpenBoxComponent;
        public UISingingComponent UISingingComponent;
        public UIDigTreasureComponent UIDigTreasureComponent;
        public UIMainActivityTipComponent UIMainActivityTipComponent;
        public UIMainButtonPositionComponent UIMainButtonPositionComponent;

        public LockTargetComponent LockTargetComponent;
        public SkillIndicatorComponent SkillIndicatorComponent;

        public List<string> AssetPath = new List<string>();
        public List<ActivityTimer> FunctionButtons = new List<ActivityTimer>();
        public UI UILevelGuideMini;
        public UI UIMailHintTip;

        public GameObject TianQiEffectObj;
        public string TianQiEffectPath;
        public long TimerFunctiuon;
        public long TimerPing;
        public int LevelRewardKey;
        public int KillMonsterRewardKey;

        public Unit MainUnit;
        
        public float DRAG_TO_ANGLE = 0.5f;
        public Vector2 PreviousPressPosition;
        public float AngleX;
        public float AngleY;
    }


    public class UIMainComponentAwakeSystem : AwakeSystem<UIMainComponent>
    {

        public override void Awake(UIMainComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            Transform transform = self.GetParent<UI>().GameObject.transform;
            self.DoMoveLeft = transform.Find("DoMoveLeft").gameObject;
            self.DoMoveRight = transform.Find("DoMoveRight").gameObject;
            self.DoMoveBottom = transform.Find("DoMoveBottom").gameObject;

            self.DragPanel = rc.Get<GameObject>("DragPanel");
            self.DragPanel.SetActive(GMHelp.GmAccount.Contains(self.ZoneScene().GetComponent<AccountInfoComponent>().Account));
            ButtonHelp.AddEventTriggers(self.DragPanel, (pdata) => { self.BeginDrag(pdata); }, EventTriggerType.BeginDrag);
            ButtonHelp.AddEventTriggers(self.DragPanel, (pdata) => { self.Drag(pdata); }, EventTriggerType.Drag);
            ButtonHelp.AddEventTriggers(self.DragPanel, (pdata) => { self.EndDrag(pdata); }, EventTriggerType.EndDrag);
            
            self.Button_Donation = rc.Get<GameObject>("Button_Donation");
            self.Button_Donation.SetActive(  true );
            self.Button_Donation.GetComponent<Button>().onClick.AddListener(() => { UIHelper.Create(self.ZoneScene(), UIType.UIDonation).Coroutine(); });

            self.Btn_PetFormation = rc.Get<GameObject>("Btn_PetFormation");
            ButtonHelp.AddListenerEx(self.Btn_PetFormation, () => { UIHelper.Create(self.ZoneScene(), UIType.UIPetSet).Coroutine(); });

            self.Button_Hunt = rc.Get<GameObject>("Button_Hunt");
            self.Button_Hunt.GetComponent<Button>().onClick.AddListener((() => { UIHelper.Create(self.ZoneScene(), UIType.UIHunt).Coroutine(); }));
            self.Button_Hunt.SetActive(false);

            self.Button_Solo = rc.Get<GameObject>("Button_Solo");
            self.Button_Solo.GetComponent<Button>().onClick.AddListener(() => { UIHelper.Create(self.ZoneScene(), UIType.UISolo).Coroutine(); });
            self.Button_Solo.SetActive(false);

            self.Btn_Union = rc.Get<GameObject>("Btn_Union");
            self.Btn_Union.GetComponent<Button>().onClick.AddListener(self.OnBtn_Union);

            self.Button_Happy = rc.Get<GameObject>("Button_Happy");
            self.Button_Happy.GetComponent<Button>().onClick.AddListener(self.OnButton_Happy);
            self.Button_Happy.SetActive(false);

            self.Button_RunRace = rc.Get<GameObject>("Button_RunRace");
            self.Button_RunRace.GetComponent<Button>().onClick.AddListener(self.OnButton_RunRace);
            self.Button_RunRace.SetActive(false);

            self.Button_Demon = rc.Get<GameObject>("Button_Demon");
            self.Button_Demon.GetComponent<Button>().onClick.AddListener(self.OnButton_Demon);
            self.Button_Demon.SetActive(false);

            self.Btn_RerurnDungeon = rc.Get<GameObject>("Btn_RerurnDungeon");
            self.Btn_RerurnDungeon.GetComponent<Button>().onClick.AddListener(self.OnBtn_RerurnDungeon);
            self.Btn_RerurnDungeon.SetActive(false);

            self.Btn_MapTransfer = rc.Get<GameObject>("Btn_MapTransfer");
            self.Btn_MapTransfer.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_MapTransfer().Coroutine(); });
            self.Btn_MapTransfer.SetActive(false);

            self.Button_Season = rc.Get<GameObject>("Button_Season");
            self.Button_Season.GetComponent<Button>().onClick.AddListener(() => { self.OnButton_Season().Coroutine(); });
            self.Button_Season.SetActive(true);

            self.Button_Welfare = rc.Get<GameObject>("Button_Welfare");
            self.Button_Welfare.GetComponent<Button>().onClick.AddListener(self.OnButton_Welfare);
            self.Button_Welfare.SetActive(false);

            self.Btn_Auction = rc.Get<GameObject>("Btn_Auction");
            ButtonHelp.AddListenerEx(self.Btn_Auction, () => { UIHelper.Create(self.ZoneScene(), UIType.UIPaiMaiAuction).Coroutine(); });
            self.Btn_Auction.SetActive(false);
            self.Btn_GM = rc.Get<GameObject>("Btn_GM");
            ButtonHelp.AddListenerEx(self.Btn_GM, () => { UIHelper.Create(self.ZoneScene(), UIType.UIGM).Coroutine(); });

            self.Btn_Task = rc.Get<GameObject>("Btn_Task");
            ButtonHelp.AddListenerEx(self.Btn_Task, self.OnOpenTask);

            self.UIStall = rc.Get<GameObject>("UIStall");
            GameObject buttonStallOpen = rc.Get<GameObject>("ButtonStallOpen");
            ButtonHelp.AddListenerEx(buttonStallOpen, () => { self.OnButtonStallOpen().Coroutine(); });

            GameObject buttonStallCancel = rc.Get<GameObject>("ButtonStallCancel");
            ButtonHelp.AddListenerEx(buttonStallCancel, self.OnButtonStallCancel);

            self.Btn_Friend = rc.Get<GameObject>("Btn_Friend");
            ButtonHelp.AddListenerEx(self.Btn_Friend, self.OnBtn_Friend);

            self.Button_HongBao = rc.Get<GameObject>("Button_HongBao");
            ButtonHelp.AddListenerEx(self.Button_HongBao, self.OnButton_HongBao);
            self.Button_HongBao.SetActive(false);

            self.Button_JiaYuan = rc.Get<GameObject>("Btn_JiaYuan");
            ButtonHelp.AddListenerEx(self.Button_JiaYuan, self.OnButton_JiaYuan);

            self.Button_ZhenYing = rc.Get<GameObject>("Button_ZhenYing");
            ButtonHelp.AddListenerEx(self.Button_ZhenYing, self.OnButton_ZhenYing);
            self.Button_ZhenYing.SetActive(false);

            self.Button_WorldLv = rc.Get<GameObject>("Button_WorldLv");
            ButtonHelp.AddListenerEx(self.Button_WorldLv, self.OnButton_WorldLv);

            self.Button_Horse = rc.Get<GameObject>("Button_Horse");
            self.Button_CityHorse = rc.Get<GameObject>("Button_CityHorse");
            ButtonHelp.AddListenerEx(self.Button_Horse, () => { self.OnButton_Horse(true); });
            ButtonHelp.AddListenerEx(self.Button_CityHorse, () => { self.OnButton_Horse(true); });

            self.Button_FenXiang = rc.Get<GameObject>("Button_FenXiang");
            ButtonHelp.AddListenerEx(self.Button_FenXiang, self.OnButton_FenXiang);

            self.Button_NewYear = rc.Get<GameObject>("Button_NewYear");
            ButtonHelp.AddListenerEx(self.Button_NewYear, self.OnButton_NewYear);

            self.Btn_LvReward = rc.Get<GameObject>("Btn_LvReward");
            ButtonHelp.AddListenerEx(self.Btn_LvReward.GetComponent<ReferenceCollector>().Get<GameObject>("Image_ItemButton"),
                () => { self.OnBtn_LvReward().Coroutine(); });

            self.Btn_KillMonsterReward = rc.Get<GameObject>("Btn_KillMonsterReward");
            ButtonHelp.AddListenerEx(self.Btn_KillMonsterReward.GetComponent<ReferenceCollector>().Get<GameObject>("Image_ItemButton"),
                () => { self.OnBtn_KillMonsterReward().Coroutine(); });
            
            self.MailHintTip = rc.Get<GameObject>("MailHintTip");
            ButtonHelp.AddListenerEx(self.MailHintTip, () => { self.OnMailHintTip(); });
            UI mailHintTipUI = self.AddChild<UI, string, GameObject>("MailHintTip", self.MailHintTip);
            self.UIMailHintTip = mailHintTipUI;

            self.Btn_Battle = rc.Get<GameObject>("Btn_Battle");
            ButtonHelp.AddListenerEx(self.Btn_Battle, self.OnBtn_Battle);
            self.Btn_Battle.SetActive(false);

            self.Fps = rc.Get<GameObject>("Fps");
            self.Fps.SetActive(false);

            //获取相关组件
            self.Obj_Img_ExpPro = rc.Get<GameObject>("Img_ExpPro");
            self.Obj_Lab_ExpValue = rc.Get<GameObject>("Lab_ExpValue");
            self.bagButton = rc.Get<GameObject>("Btn_RoseEquip");
            //self.bagButton.GetComponent<Button>().onClick.AddListener(() => { self.OnOpenBag(); });
            ButtonHelp.AddListenerEx(self.bagButton, () => { self.OnOpenBag(); });

            self.TextPing = rc.Get<GameObject>("TextPing").GetComponent<Text>();
            self.TextMessage = rc.Get<GameObject>("TextMessage").GetComponent<Text>();

            self.buttonReturn = rc.Get<GameObject>("Btn_RerurnBuilding");
            //self.buttonReturn.GetComponent<Button>().onClick.AddListener(() => { self.OnClickReturnButton(); });
            ButtonHelp.AddListenerEx(self.buttonReturn, () => { self.OnClickReturnButton(); });

            self.chengjiuButton = rc.Get<GameObject>("Btn_ChengJiu");
            //self.chengjiuButton.GetComponent<Button>().onClick.AddListener(() => { self.OnOpenChengjiu(); });
            ButtonHelp.AddListenerEx(self.chengjiuButton, () => { self.OnOpenChengjiu(); });
            self.adventureBtn = rc.Get<GameObject>("AdventureBtn");
            //self.adventureBtn.GetComponent<Button>().onClick.AddListener();
            ButtonHelp.AddListenerEx(self.adventureBtn, () => { self.OnEnterChapter().Coroutine(); });

            self.duihuaButton = rc.Get<GameObject>("Btn_NpcDuiHua");
            //self.duihuaButton.GetComponent<Button>().onClick.AddListener(() => { self.MoveToNpcDialog(); });
            ButtonHelp.AddListenerEx(self.duihuaButton, () => { DuiHuaHelper.MoveToNpcDialog(self.ZoneScene()); });

            //ButtonHelp.AddListenerEx(self.shiquButton, () => { self.OnShiquItem(); });

            self.petButton = rc.Get<GameObject>("Btn_Pet");
            //self.petButton.GetComponent<Button>().onClick.AddListener(() => { self.OnClickPetButton(); });
            self.petButton.GetComponent<Button>().onClick.AddListener(() => { self.OnClickPetButton(); });

            self.roleSkillBtn = rc.Get<GameObject>("Btn_RoseSkill");
            //self.roleSkillBtn.GetComponent<Button>().onClick.AddListener(() => { self.OnClickSkillButton(); });
            ButtonHelp.AddListenerEx(self.roleSkillBtn, () => { self.OnClickSkillButton(); });

            self.miniMapButton = rc.Get<GameObject>("MiniMapButton");
            //self.miniMap.GetComponent<Button>().onClick.AddListener(() => { self.OnOpenMap(); });
            ButtonHelp.AddListenerEx(self.miniMapButton, () => { self.OnOpenMap(); });

            self.Obj_Btn_ShouSuo = rc.Get<GameObject>("Btn_ShouSuo");
            self.Obj_Btn_ShouSuo.GetComponent<Button>().onClick.AddListener(() => { self.OnOpenShouSuo(); });
            //ButtonHelp.AddListenerEx(self.Obj_Btn_ShouSuo, () => { self.OnOpenShouSuo(); });

            self.Btn_EveryTask = rc.Get<GameObject>("Btn_EveryTask");
            ButtonHelp.AddListenerEx(self.Btn_EveryTask, self.OnBtn_EveryTask);

            self.Btn_PaiMaiHang = rc.Get<GameObject>("Btn_PaiMaiHang");
            //self.Btn_PaiMaiHang.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_PaiMaiHang(); });
            ButtonHelp.AddListenerEx(self.Btn_PaiMaiHang, () => { self.OnBtn_PaiMaiHang(); });

            self.Button_Energy = rc.Get<GameObject>("Button_Energy");
            //self.Button_Energy.GetComponent<Button>().onClick.AddListener(() => { self.OnButton_Energy(); });
            ButtonHelp.AddListenerEx(self.Button_Energy, self.OnButton_Energy);
            self.Button_Energy.SetActive(false);

            self.TeamDungeonBtn = rc.Get<GameObject>("TeamDungeonBtn");
            ButtonHelp.AddListenerEx(self.TeamDungeonBtn, self.OnTeamDungeonBtn);

            self.Btn_HuoDong = rc.Get<GameObject>("Btn_HuoDong");
            ButtonHelp.AddListenerEx(self.Btn_HuoDong, self.OnBtn_HuoDong);

            self.Button_ZhanQu = rc.Get<GameObject>("Button_ZhanQu");
            ButtonHelp.AddListenerEx(self.Button_ZhanQu, () => { self.OnButton_ZhanQu().Coroutine();  });
            //int serverid = self.ZoneScene().GetComponent<AccountInfoComponent>().ServerId;
            //Button_ZhanQu.SetActive( !ServerHelper.IsOldServer(serverid) );

            self.Button_Recharge = rc.Get<GameObject>("Button_Recharge");
            ButtonHelp.AddListenerEx(self.Button_Recharge, self.OnButton_Recharge);

            self.Btn_Rank = rc.Get<GameObject>("Btn_Rank");
            ButtonHelp.AddListenerEx(self.Btn_Rank, () => { self.OnBtn_Rank(); });


            self.LeftBottomBtns = rc.Get<GameObject>("LeftBottomBtns");
            self.ShrinkBtn = rc.Get<GameObject>("ShrinkBtn");
            ButtonHelp.AddListenerEx(self.ShrinkBtn, () => { self.OnShrinkBtn(); });

            self.HomeButton = rc.Get<GameObject>("HomeButton");
            self.UIMainSkill = rc.Get<GameObject>("UIMainSkill");
            self.Btn_TopRight_1 = rc.Get<GameObject>("Btn_TopRight_1");
            self.Btn_TopRight_2 = rc.Get<GameObject>("Btn_TopRight_2");
            self.UGuaJiSet = rc.Get<GameObject>("UGuaJiSet");
            self.Btn_StopGuaJi = rc.Get<GameObject>("Btn_StopGuaJi");
            ButtonHelp.AddListenerEx(self.Btn_StopGuaJi, () => { self.OnStopGuaJi(); });

            self.Button_ZhanKai = rc.Get<GameObject>("Button_ZhanKai");
            self.Button_ZhanKai.GetComponent<Button>().onClick.AddListener(self.OnButton_ZhanKai);

            self.Button_RechargeReward = rc.Get<GameObject>("Button_RechargeReward");
            self.Button_RechargeReward.GetComponent<Button>().onClick.AddListener(self.OnButton_RechargeReward);

            self.Button_ActivityV1 = rc.Get<GameObject>("Button_ActivityV1");
            self.Button_ActivityV1.GetComponent<Button>().onClick.AddListener(() => { self.Button_ActivityV1().Coroutine();    });

            List<string> AdminAccount = new List<string>()
            {
                "tcg01",
                "test01",
                "18652422521",
                "18319670288",
            };
            self.Btn_GM.SetActive(AdminAccount.Contains(self.ZoneScene().GetComponent<AccountInfoComponent>().Account));
            self.Button_ActivityV1.SetActive(GMHelp.GmAccount.Contains(self.ZoneScene().GetComponent<AccountInfoComponent>().Account));
         
            self.LockTargetComponent = self.ZoneScene().GetComponent<LockTargetComponent>();
            self.SkillIndicatorComponent = self.ZoneScene().GetComponent<SkillIndicatorComponent>();

            //初始化子UI
            self.initSubUI();

            //初始化基础属性
            self.InitShow();

            self.OnSettingUpdate();

            self.RegisterReddot();

            self.InitFunctionButton();

            self.CheckPopUP();

            self.CheckTapRepCiLiu();

            self.RequestChatList().Coroutine();

            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
            int guideid = PlayerPrefsHelp.GetInt($"{PlayerPrefsHelp.LastGuide}_{userInfo.UserId}");
            if (userInfo.Lv == 1 || guideid > 0)
            {
                self.ZoneScene().GetComponent<GuideComponent>().SetGuideId(guideid);
            }

            self.GetParent<UI>().OnShowUI = self.OnShowUIHandler;

            DataUpdateComponent.Instance.AddListener(DataType.SkillSetting, self);
            DataUpdateComponent.Instance.AddListener(DataType.SkillReset, self);
            DataUpdateComponent.Instance.AddListener(DataType.EquipWear, self);
            DataUpdateComponent.Instance.AddListener(DataType.TaskUpdate, self);
            DataUpdateComponent.Instance.AddListener(DataType.TaskTrace, self);
            DataUpdateComponent.Instance.AddListener(DataType.TaskGet, self);
            DataUpdateComponent.Instance.AddListener(DataType.TaskComplete, self);
            DataUpdateComponent.Instance.AddListener(DataType.OnRecvChat, self);
            DataUpdateComponent.Instance.AddListener(DataType.HorseNotice, self);
            DataUpdateComponent.Instance.AddListener(DataType.OnPetFightSet, self);
            DataUpdateComponent.Instance.AddListener(DataType.UpdateUserData, self);
            DataUpdateComponent.Instance.AddListener(DataType.UpdateUserDataExp, self);
            DataUpdateComponent.Instance.AddListener(DataType.UpdateUserDataPiLao, self);
            DataUpdateComponent.Instance.AddListener(DataType.BagItemUpdate, self);
            DataUpdateComponent.Instance.AddListener(DataType.SettingUpdate, self);
            DataUpdateComponent.Instance.AddListener(DataType.BagItemAdd, self);
            DataUpdateComponent.Instance.AddListener(DataType.TaskGiveUp, self);
            DataUpdateComponent.Instance.AddListener(DataType.TeamUpdate, self);
            DataUpdateComponent.Instance.AddListener(DataType.OnActiveTianFu, self);
            DataUpdateComponent.Instance.AddListener(DataType.MainHeroMove, self);
            DataUpdateComponent.Instance.AddListener(DataType.BeforeMove, self);
            DataUpdateComponent.Instance.AddListener(DataType.SkillUpgrade, self);
        }
    }

    //通用提示事件
    [Event]
    public class CommonHintEvent : AEventClass<EventType.CommonHint>
    {
        protected override void Run(object cls)
        {
            EventType.CommonHint args = cls as EventType.CommonHint;
            FloatTipManager.Instance.ShowFloatTipDi(args.HintText);
        }
    }

    //通用提示事件
    [Event]
    public class CommonPopupEvent : AEventClass<EventType.CommonPopup>
    {
        protected override void Run(object cls)
        {
            EventType.CommonPopup args = cls as EventType.CommonPopup;
            PopupTipHelp.OpenPopupTip(args.ZoneScene, "系统提示", args.HintText, null, null).Coroutine();
        }
    }

    //通用提示事件
    [Event]
    public class CommonHintErrorEvent : AEventClass<EventType.CommonHintError>
    {
        protected override void Run(object cls)
        {
            EventType.CommonHintError args = cls as EventType.CommonHintError;

            if (args.errorValue == ErrorCode.ERR_ModifyData && args.ZoneScene!=null)
            {
                args.ZoneScene.GetComponent<RelinkComponent>()?.OnModifyData();
            }

            ErrorHelp.Instance.ErrorHint(args.errorValue);
        }
    }



    public class UIMainComponentDestroySystem : DestroySystem<UIMainComponent>
    {
        public override void Destroy(UIMainComponent self)
        {
            DataUpdateComponent.Instance.RemoveListener(DataType.SkillSetting, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.SkillReset, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.EquipWear, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.TaskUpdate, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.TaskTrace, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.TaskGet, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.TaskComplete, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.OnRecvChat, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.HorseNotice, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.OnPetFightSet, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.UpdateUserData, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.UpdateUserDataExp, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.UpdateUserDataPiLao, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.BagItemUpdate, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.SettingUpdate, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.BagItemAdd, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.TaskGiveUp, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.TeamUpdate, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.OnActiveTianFu, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.MainHeroMove, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.BeforeMove, self);
            DataUpdateComponent.Instance.RemoveListener(DataType.SkillUpgrade, self);

            if (self.TianQiEffectObj != null)
            {
                GameObjectPoolComponent.Instance.RecoverGameObject(self.TianQiEffectPath, self.TianQiEffectObj);
                self.TianQiEffectObj = null;
            }

            for (int i = 0; i < self.AssetPath.Count; i++)
            {
                if (!string.IsNullOrEmpty(self.AssetPath[i]))
                {
                    ResourcesComponent.Instance.UnLoadAsset(self.AssetPath[i]);
                }
            }

            TimerComponent.Instance?.Remove(ref self.TimerFunctiuon);
            TimerComponent.Instance?.Remove(ref self.TimerPing);
            self.UnRegisterRedot();
        }
    }

    public static class UIMainComponentSystem
    {
        public static void BeginDrag(this UIMainComponent self, PointerEventData pdata)
        {
            self.PreviousPressPosition = pdata.position;
            self.ZoneScene().CurrentScene().GetComponent<CameraComponent>().StartRotate();
        }

        public static void Drag(this UIMainComponent self, PointerEventData pdata)
        {
            self.AngleX = (pdata.position.x - self.PreviousPressPosition.x) * self.DRAG_TO_ANGLE;
            self.AngleY = (pdata.position.y - self.PreviousPressPosition.y) * self.DRAG_TO_ANGLE;
            self.ZoneScene().CurrentScene().GetComponent<CameraComponent>().Rotate(-self.AngleX, -self.AngleY);
            self.PreviousPressPosition = pdata.position;

            self.RecordCameraParams();
        }

        public static void EndDrag(this UIMainComponent self, PointerEventData pdata)
        {
            self.ZoneScene().CurrentScene().GetComponent<CameraComponent>().EndRotate();

            self.RecordCameraParams();
        }

        public static void RecordCameraParams(this UIMainComponent self)
        {
            Vector3 offsetposition = self.ZoneScene().CurrentScene().GetComponent<CameraComponent>().OffsetPostion;
            PlayerPrefsHelp.SetString(PlayerPrefsHelp.CameraParams, $"{offsetposition.x}_{offsetposition.y}_{offsetposition.z}");
        }

        public static async ETTask RequestChatList(this UIMainComponent self)
        {
            C2Chat_GetChatRequest request = new C2Chat_GetChatRequest();
            Chat2C_GetChatResponse response = (Chat2C_GetChatResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);

            ChatComponent chatComponent = self.ZoneScene().GetComponent<ChatComponent>();
            for (int i = 0; i < response.ChatInfos.Count; i++)
            {
                chatComponent.OnRecvChat(response.ChatInfos[i]);
            }
        }

        public static void CheckRechargeRewardButton(this UIMainComponent self)
        {
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();

            bool showButton = false;
            foreach (var item in ConfigHelper.RechargeReward)
            {
                if (!userInfoComponent.UserInfo.RechargeReward.Contains(item.Key))
                {
                    showButton = true;
                    break; 
                }
            }
            self.Button_RechargeReward.SetActive(showButton);
        }

        public static void OnBtn_Union(this UIMainComponent self)
        {
            EnterFubenHelp.RequestTransfer(self.ZoneScene(), SceneTypeEnum.Union, 2000009).Coroutine();
        }

        public static void OnButton_Happy(this UIMainComponent self)
        {
            EnterFubenHelp.RequestTransfer(self.ZoneScene(), SceneTypeEnum.Happy, BattleHelper.GetSceneIdByType(SceneTypeEnum.Happy)).Coroutine();
        }

        public static void OnButton_RunRace(this UIMainComponent self)
        {
            UIHelper.Create(self.ZoneScene(), UIType.UIRunRace).Coroutine();
        }

        public static void OnButton_Demon(this UIMainComponent self)
        {
            UIHelper.Create(self.ZoneScene(), UIType.UIDemon).Coroutine();
        }

        public static void OnButton_Fashion(this UIMainComponent self)
        {
            UIHelper.Create(self.ZoneScene(), UIType.UIFashion).Coroutine();
        }

        public static void OnShowUIHandler(this UIMainComponent self)
        {
            self.ZoneScene().GetComponent<GuideComponent>().OnTrigger(GuideTriggerType.OpenUI, UIType.UIMain);

#if UNITY_ANDROID

            try
            {
                Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
                int rechargenumber = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.RechargeNumber);

                AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
                string serverName = accountInfoComponent.ServerName;

                //UploadUserData
                UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;

                //已经明确用法
                //TapSDKHelper.InitUserData(userInfo.UserId, userInfo.Name, userInfo.Lv, userInfo.Combat, rechargenumber, serverName, accountInfoComponent.ServerId, userInfo.Occ, userInfo.OccTwo);
                //TapSDKHelper.InitUserData_2(userInfo.UserId, userInfo.Name, userInfo.Lv, userInfo.Combat, rechargenumber, serverName);

            }
            catch (Exception ex) {

                Log.Debug("tapsdk报错:" + ex);

            }
#endif
        }

        public static void OnMainHeroMove(this UIMainComponent self)
        {
            self.UIMapMini.OnMainHeroMove();
            self.LockTargetComponent.OnMainHeroMove();
            self.SkillIndicatorComponent.OnMainHeroMove();

            if (self.TianQiEffectObj != null)
            {
                self.TianQiEffectObj.transform.localPosition = self.MainUnit.Position;
            }
        }

        public static void OnUpdateCombat(this UIMainComponent self)
        {
            self.UIRoleHead.OnUpdateCombat();
        }

        public static void UpdatePing(this UIMainComponent self)
        {
            SessionComponent sessionComponent = self.ZoneScene()?.GetComponent<SessionComponent>();
            if (sessionComponent == null || sessionComponent.Session == null)
            {
                return;
            }
            PingComponent pingComponent = sessionComponent.Session.GetComponent<PingComponent>();
            if (pingComponent == null)
            {
                return;
            }

            long ping = pingComponent.Ping;
            self.TextPing.text = StringBuilderHelper.GetPing(ping); 
            if (ping <= 200)
            {
                self.TextPing.color = Color.green;
                return;
            }
            if (ping <= 500)
            {
                self.TextPing.color = Color.yellow;
                return;
            }
            self.TextPing.color = Color.red;
        }

        public static void UpdateMessage(this UIMainComponent self)
        {
             self.TextMessage.text = StringBuilderHelper.GetMessageCnt(OpcodeHelper.OneTotalNumber);
             OpcodeHelper.OneTotalNumber = 0;
        }

        public static void ShowPing(this UIMainComponent self)
        {
            if (self.Fps.activeSelf)
            {
                self.Fps.SetActive(false);
                OpcodeHelper.ShowMessage = false;
                TimerComponent.Instance?.Remove(ref self.TimerPing);
            }
            else
            {
                self.Fps.SetActive(true);
                self.TextMessage.text = string.Empty;
                OpcodeHelper.ShowMessage = true;
                OpcodeHelper.OneTotalNumber = 0;
                TimerComponent.Instance?.Remove(ref self.TimerPing);
                self.TimerPing = TimerComponent.Instance.NewRepeatedTimer(5000, TimerType.UIMainFPSTimer, self);
            }
        }

        public static void OnBagItemAdd(this UIMainComponent self, string dataPaams)
        {
            if (UIHelper.GetUI(self.ZoneScene(), UIType.UITreasureOpen) != null)
            {
                return;
            }

            string[] iteminfo = dataPaams.Split('_');
            int itemId = int.Parse(iteminfo[0]);
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemId);
            FloatTipManager.Instance.ShowFloatTip($"获得物品 {itemConfig.ItemName} x{iteminfo[1]}");
        }

        public static void OnUpdateHP(this UIMainComponent self, int sceneType, Unit defend, Unit attack, long hurtvalue)
        {
            int unitType = defend.Type;
            if (unitType == UnitType.Player && sceneType == SceneTypeEnum.TeamDungeon)
            {
                self.UIMainTeam.OnUpdateHP(defend);
            }
            if (unitType == UnitType.Monster)
            {
                self.UIMainHpBar.OnUpdateHP(defend);
                self.UIMainHpBar.OnUpdateHP(defend, attack, hurtvalue);
            }
            if (unitType == UnitType.Pet)
            {
                self.UIRoleHead.OnUpdatePetHP(defend);
            }
        }

        public static void OnTeamUpdate(this UIMainComponent self)
        {
            self.UIMainTeam.OnUpdateUI();
        }

        public static void OnUpdateUserDataExp(this UIMainComponent self, string updateType, long updateValue)
        {
            //if (updateValue > 0)
            {
                FloatTipManager.Instance.ShowFloatTip(StringBuilderHelper.GetExpTip(updateValue));
                self.UpdateShowRoleExp();
            }
        }

        public static void OnUpdateUserDataPiLao(this UIMainComponent self, string updateType, long updateValue)
        {
            self.UIRoleHead.UpdateShowRolePiLao();
        }

        public static async ETTask UpdateTaskList(this UIMainComponent self)
        {
            //此处新加一个协议。 调用服务器的<TaskComponent>().OnLogin() 刷新任务
            await TimerComponent.Instance.WaitFrameAsync();
            C2M_TaskOnLoginRequest request = new C2M_TaskOnLoginRequest();
            M2C_TaskOnLoginResponse response = (M2C_TaskOnLoginResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(request);
            await NetHelper.RequestIniTask(self.ZoneScene());
            self.OnRecvTaskUpdate();
        }

        public static void OnUpdateUserData(this UIMainComponent self, string updateType)
        {
            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
            int userDataType = int.Parse(updateType.Split('_')[0]);

            string updateValue = updateType.Split('_')[1];

            switch (userDataType)
            {
                case UserDataType.Exp:
                    //self.UpdateShowRoleExp();
                    break;
                case UserDataType.PiLao:
                    //self.UIRoleHead.UpdateShowRolePiLao();
                    break;
                case UserDataType.Lv:
                    self.UpdateShowRoleExp();
                    self.UIRoleHead.UpdateShowRoleExp();
                    self.CheckFuntionButtonByLv(int.Parse(updateValue));
                    FunctionEffect.GetInstance().PlaySelfEffect(self.MainUnit, 60000002);
                    self.ZoneScene().GetComponent<GuideComponent>().OnTrigger(GuideTriggerType.LevelUp, userInfo.Lv.ToString());
                    FloatTipManager.Instance.ShowFloatTipDi(GameSettingLanguge.LoadLocalization("恭喜你!等级提升至:") + userInfo.Lv);
                    self.UpdateLvReward();
                    self.CheckCanEquip().Coroutine();
                    if (int.Parse(updateValue) > 30)
                    {
                        self.UpdateTaskList().Coroutine();
                    }
                    break;
                case UserDataType.Name:
                    self.UIRoleHead.UpdateShowRoleName();
                    break;
                case UserDataType.Vitality:
                    self.UIRoleHead.UpdateShowRoleHuoLi();
                    break;

                case UserDataType.UnionContri:
                    if (UIHelper.GetUI(self.ZoneScene(), UIType.UITreasureOpen) != null)
                    {
                        return;
                    }
                    if (int.Parse(updateValue) > 0)
                    {
                        FloatTipManager.Instance.ShowFloatTip($"获得{updateValue} 家族捐献");
                    }
                    if (int.Parse(updateValue) < 0)
                    {
                        FloatTipManager.Instance.ShowFloatTip($"消耗{int.Parse(updateValue) * -1} 家族捐献");
                    }
                    break;

                case UserDataType.Gold:
                    if (UIHelper.GetUI(self.ZoneScene(), UIType.UITreasureOpen) != null)
                    {
                        return;
                    }
                    if (int.Parse(updateValue) > 0)
                    {
                        FloatTipManager.Instance.ShowFloatTip($"获得{updateValue} 金币");
                    }
                    if (int.Parse(updateValue) < 0)
                    {
                        FloatTipManager.Instance.ShowFloatTip($"消耗{int.Parse(updateValue) * -1} 金币");
                    }
                    break;
                case UserDataType.WeiJingGold:
                    if (int.Parse(updateValue) > 0)
                    {
                        FloatTipManager.Instance.ShowFloatTip($"获得{updateValue} 兑换币");
                    }
                    if (int.Parse(updateValue) < 0)
                    {
                        FloatTipManager.Instance.ShowFloatTip($"消耗{int.Parse(updateValue) * -1} 兑换币");
                    }
                    break;
                case UserDataType.RongYu:
                    if (int.Parse(updateValue) > 0)
                    {
                        FloatTipManager.Instance.ShowFloatTip($"获得{updateValue} 荣誉");
                    }
                    if (int.Parse(updateValue) < 0)
                    {
                        FloatTipManager.Instance.ShowFloatTip($"消耗{int.Parse(updateValue) * -1} 荣誉");
                    }
                    break;
                case UserDataType.JiaYuanFund:
                    if (int.Parse(updateValue) > 0)
                    {
                        FloatTipManager.Instance.ShowFloatTip($"获得{updateValue} 家园资金");
                    }
                    break;
                case UserDataType.BaoShiDu:
                    if (int.Parse(updateValue) > 0)
                    {
                        FloatTipManager.Instance.ShowFloatTip($"获得{updateValue} 饱食度");
                    }
                    break;

                case UserDataType.Combat:
                    self.OnUpdateCombat();

                    UI ui = UIHelper.GetUI(self.ZoneScene(), UIType.UIRole);
                    if (ui != null)
                    {
                        ui.GetComponent<UIRoleComponent>().UpdateShowComBat();
                    }
                    break;
                case UserDataType.Sp:
                    ReddotComponent reddotComponent = self.ZoneScene().GetComponent<ReddotComponent>();
                    reddotComponent.UpdateReddont(ReddotType.SkillUp);
                    break;
                case UserDataType.Message:
                    PopupTipHelp.OpenPopupTip_2(self.ZoneScene(), "系统消息", updateValue, null).Coroutine();
                    break;
                case UserDataType.PullBack:
                    FloatTipManager.Instance.ShowFloatTip("所有人不要乱跑哦");
                    FunctionEffect.GetInstance().PlaySelfEffect(self.MainUnit, 30000002);
                    break;
                default:
                    break;
            }
        }

        public static void OnRechageSucess(this UIMainComponent self, int addNumber)
        {
            FloatTipManager.Instance.ShowFloatTipDi($"充值{addNumber}元成功");

            self.ZoneScene().GetComponent<AccountInfoComponent>().PlayerInfo.RechargeInfos.Add(new RechargeInfo()
            {
                Amount = addNumber,
                Time = TimeHelper.ClientNow(),
                UserId = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.UserId
            });

#if UNITY_ANDROID
            AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
            if (GlobalHelp.GetPlatform() == 1 && !string.IsNullOrEmpty(accountInfoComponent.TaprepRequest))
            {
                TapSDKHelper.TapReqEvent(accountInfoComponent.TaprepRequest, 3, addNumber + "").Coroutine();
            }
#endif
        }

        public static  void CheckTapRepCiLiu(this UIMainComponent self)
        {
#if UNITY_ANDROID
            AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
            if (GlobalHelp.GetPlatform() != 1 || string.IsNullOrEmpty(accountInfoComponent.TaprepRequest))
            {
                return;
            }

            string registerTime = PlayerPrefsHelp.GetString( PlayerPrefsHelp.TapRepRegister );
            if ( string.IsNullOrEmpty(registerTime))
            {
                return;
            }
            if (PlayerPrefsHelp.GetInt(PlayerPrefsHelp.TapRepCiLiu) == 1)
            {
                return;
            }

            long time1 = long.Parse(registerTime);
            long time2 = TimeHelper.ServerNow();

            if (ComHelp.GetDayByTime(time2) - ComHelp.GetDayByTime(time1) == 1)
            {
                PlayerPrefsHelp.SetInt(PlayerPrefsHelp.TapRepCiLiu, 1);
                TapSDKHelper.TapReqEvent(accountInfoComponent.TaprepRequest, 4, string.Empty).Coroutine();
            }

#endif
        }

        public static void OnSettingUpdate(this UIMainComponent self)
        {
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            int operatMode = int.Parse(userInfoComponent.GetGameSettingValue(GameSettingEnum.YanGan));
            self.UIJoystickMoveComponent.UpdateOperateMode(operatMode);

            string oldValue = userInfoComponent.GetGameSettingValue(GameSettingEnum.Smooth);
            SettingHelper.OnSmooth(oldValue);

            oldValue = userInfoComponent.GetGameSettingValue(GameSettingEnum.NoShowOther);
            SettingHelper.OnShowOther(oldValue);

            string value = userInfoComponent.GetGameSettingValue(GameSettingEnum.AutoAttack);
            AttackComponent attackComponent = self.ZoneScene().GetComponent<AttackComponent>();
            attackComponent.AutoAttack = value == "1";
        }

        public static async ETTask CheckCanEquip(this UIMainComponent self)
        {
            //有bug 先注释
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            UserInfo useInfo = userInfoComponent.UserInfo;
            if (useInfo.Lv > 16)
            {
                return;
            }

            // 检测是否有可以穿戴的装备
            BagComponent bagComponent = self.ZoneScene().GetComponent<BagComponent>();
            List<BagInfo> bagInfos = bagComponent.GetItemsByLoc(ItemLocType.ItemLocBag);
            for (int i = bagInfos.Count - 1; i >= 0; i--)
            {
                if (bagInfos.Count <= i)
                {
                    continue;
                }
                BagInfo baginfo1 = bagInfos[i];
                if (baginfo1 == null)
                {
                    continue;
                }
                
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(baginfo1.ItemID);

                //判断等级
                int roleLv = useInfo.Lv;
                int equipLv = itemConfig.UseLv;

                //简易
                if (baginfo1.HideSkillLists.Contains(68000103))
                {
                    equipLv = equipLv - 5;
                }

                //无级别
                if (baginfo1.HideSkillLists.Contains(68000106))
                {
                    equipLv = 1;
                }

                if (roleLv < equipLv)
                {
                    continue;
                }

                if (baginfo1.IfJianDing)
                {
                    continue;
                }

                if (itemConfig.ItemType == ItemTypeEnum.Equipment)
                {
                    bool ifWear = false;
                    if (itemConfig.EquipType == ItemEquipType.Common)
                    {
                        ifWear = true;
                    }
                    else if (itemConfig.EquipType >= 1 && itemConfig.EquipType <= 5)
                    {
                        if (useInfo.Occ == 1 && (itemConfig.EquipType == 1 || itemConfig.EquipType == 2))
                        {
                            ifWear = true;
                        }
                        else if (useInfo.Occ == 2 && (itemConfig.EquipType == 3 || itemConfig.EquipType == 4))
                        {
                            ifWear = true;
                        }
                        else if (useInfo.Occ == 3 && (itemConfig.EquipType == 1 || itemConfig.EquipType == 5))
                        {
                            ifWear = true;
                        }
                    }
                    else if (itemConfig.EquipType >= 11 && itemConfig.EquipType <= 13)
                    {
                        if (useInfo.OccTwo > 0)
                        {
                            OccupationTwoConfig occtwoCof = OccupationTwoConfigCategory.Instance.Get(useInfo.OccTwo);
                            if (occtwoCof.ArmorMastery == itemConfig.EquipType)
                            {
                                ifWear = true;
                            }
                        }
                        else
                        {
                            ifWear = true;
                        }
                    }
                    else
                    {
                        // 生肖、晶核这些不用
                        ifWear = false;
                    }

                    if (ifWear == false)
                    {
                        continue;
                    }
                    
                    int weizhi = itemConfig.ItemSubType;
                    //获取之前的位置是否有装备
                    BagInfo beforeequip = null;
                    if (weizhi == (int)ItemSubTypeEnum.Shiping)
                    {
                        List<BagInfo> equipList = bagComponent.GetEquipListByWeizhi(weizhi);
                        if (equipList.Count < 3)
                        {
                            foreach (BagInfo bagInfo in equipList)
                            {
                                if (bagInfo.ItemID == baginfo1.ItemID)
                                {
                                    beforeequip = bagInfo;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            beforeequip = equipList[0];
                        }
                    }
                    else
                    {
                        beforeequip = bagComponent.GetEquipBySubType(ItemLocType.ItemLocEquip, weizhi);
                    }

                    if (beforeequip == null)
                    {
                        UI ui = UIHelper.GetUI(self.ZoneScene(), UIType.UIGuideEquip);
                        if (ui != null)
                        {
                            // ui.GetComponent<UIGuideEquipComponent>().UpdateInfo(bagInfos[i]);
                        }
                        else
                        {
                            ui = await UIHelper.Create(self.ZoneScene(), UIType.UIGuideEquip);
                            ui?.GetComponent<UIGuideEquipComponent>()?.UpdateInfo(baginfo1);
                        }

                        break;
                    }
                }
            }

            await ETTask.CompletedTask;
        }

        public static  void OnBagItemUpdate(this UIMainComponent self)
        {
            self.UIMainSkillComponent.OnBagItemUpdate();

            self.CheckCanEquip().Coroutine();
        }

        public static void OnEquipWear(this UIMainComponent self)
        {
            self.OnSkillSetUpdate();
        }

        public static void OnTaskGet(this UIMainComponent self, string taskid)
        {
            self.ZoneScene().GetComponent<GuideComponent>().OnTrigger(GuideTriggerType.AcceptTask, taskid);
        }

        public static async ETTask OnCompleteTask(this UIMainComponent self, string taskid)
        {
            // 完成藏宝图任务后自动接取藏宝图任务
            if (TaskConfigCategory.Instance.Contain(int.Parse(taskid)) &&
                TaskConfigCategory.Instance.Get(int.Parse(taskid)).TaskType == TaskTypeEnum.Treasure)
            {
                // 任务使者赛纳
                List<int> tasList = self.ZoneScene().GetComponent<TaskComponent>().GetOpenTaskIds(20000024);
                foreach (int id in tasList)
                {
                    TaskConfig taskConfig = TaskConfigCategory.Instance.Get(id);
                    if (taskConfig.TaskType == TaskTypeEnum.Treasure)
                    {
                        NetHelper.SendGetTask(self.ZoneScene(), taskConfig.Id).Coroutine();
                        break;
                    }
                }
            }

            await TimerComponent.Instance.WaitAsync(200);
            self.ZoneScene().GetComponent<GuideComponent>().OnTrigger(GuideTriggerType.CommitTask, taskid);
        }

        public static void OnRecvTaskUpdate(this UIMainComponent self)
        {
            self.UIMainTask.OnUpdateUI();

            self.UpdateNpcTaskUI();

            self.ZoneScene().GetComponent<ReddotComponent>().UpdateReddont(ReddotType.WelfareTask);
        }

        public static void UpdateNpcTaskUI(this UIMainComponent self)
        {
            List<Unit> allunit = self.DomainScene().CurrentScene().GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunit.Count; i++)
            {
                Unit unit = allunit[i];
                if (unit.InstanceId == 0 || unit.IsDisposed)
                {
                    continue;
                }
                if (unit.Type != UnitType.Npc)
                {
                    continue;
                }
                if (unit.GetComponent<NpcHeadBarComponent>() != null)
                {
                    unit.GetComponent<NpcHeadBarComponent>().OnRecvTaskUpdate();
                }
            }
        }

        public static void OnTaskGiveUp(this UIMainComponent self)
        {
            self.UIMainTask.OnUpdateUI();
        }

        public static void OnRecvTaskTrace(this UIMainComponent self)
        {
            self.UIMainTask.OnUpdateUI();
        }

        public static void OnSkillSetUpdate(this UIMainComponent self)
        {
            self.UIMainSkillComponent.OnSkillSetUpdate();
        }

        public static async ETTask OnStopServer(this UIMainComponent self)
        {
            PopupTipHelp.OpenPopupTip_2(self.ZoneScene(), "停服维护", "十分钟后停服维护，请暂时退出游戏！",
               () =>
               {
               }).Coroutine();

            long instanceId = self.InstanceId;
            await TimerComponent.Instance.WaitAsync(10 * 60000);
            if (instanceId != self.InstanceId)
            {
                return;
            }
            EventType.ReturnLogin.Instance.ZoneScene = self.DomainScene();
            Game.EventSystem.PublishClass(EventType.ReturnLogin.Instance);


#if UNITY_ANDROID
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int rechargenumber = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.RechargeNumber);

            AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
            string serverName = accountInfoComponent.ServerName;

            //UploadUserData
            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
            TapSDKHelper.UploadUserData(userInfo.Name, userInfo.Lv, userInfo.Combat, rechargenumber, serverName);
#endif
        }

        public static async ETTask OnTurtleEnd(this UIMainComponent self, bool show)
        {
            if (show)
            {
                var path = ABPathHelper.GetUnitPath("Component/XiaoGuiZhongDian");
                GameObject prefab = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
                GameObject gameObject = UnityEngine.Object.Instantiate(prefab);
                gameObject.name = "XiaoGuiZhongDian";

                NpcConfig npcConfig = NpcConfigCategory.Instance.Get(ConfigHelper.TurtleList[1]);
                string[] potioninfo = npcConfig.MovePosition.Split(';');
                float x = float.Parse(potioninfo[0]);
                float y = float.Parse(potioninfo[1]);
                float z = float.Parse(potioninfo[2]);
                gameObject.transform.position = new Vector3(x, y, z);
            }
            else
            {
                GameObject gameObject = GameObject.Find("XiaoGuiZhongDian");
                if (gameObject != null)
                {
                    GameObject.Destroy(gameObject);
                }
            }
        }

        public static void OnTianQiChange(this UIMainComponent self, string tianqivalue)
        {
            if (tianqivalue == "0" && self.TianQiEffectObj != null)
            {
                GameObjectPoolComponent.Instance.RecoverGameObject(self.TianQiEffectPath, self.TianQiEffectObj);
                self.TianQiEffectObj = null;
            }
            if (tianqivalue == "1" && tianqivalue == "2")
            {
                self.TianQiEffectPath = ABPathHelper.GetEffetPath($"ScenceEffect/Effect_Rain_{tianqivalue}");
                GameObjectPoolComponent.Instance.AddLoadQueue(self.TianQiEffectPath, self.InstanceId, self.OnLoadTianQiGameObject);
            }
            self.UIMapMini.UpdateTianQi(tianqivalue);
        }

        public static void OnLoadTianQiGameObject(this UIMainComponent self, GameObject gameObject, long instanceId)
        {
            UICommonHelper.SetParent(gameObject, GlobalComponent.Instance.Unit.gameObject);
            gameObject.SetActive(true);
            self.TianQiEffectObj = gameObject;
        }

        public static void OnUnionRace(this UIMainComponent self)
        {
            PopupTipHelp.OpenPopupTip(self.ZoneScene(), "家族争霸赛", "是否参与家族争霸赛?", () =>
           {
               UIHelper.Remove(self.ZoneScene(), UIType.UIDonation);
               EnterFubenHelp.RequestTransfer(self.ZoneScene(), SceneTypeEnum.UnionRace, 2000008).Coroutine();
           }, null).Coroutine();
        }

        public static void OnHorseNotice(this UIMainComponent self)
        {
            M2C_HorseNoticeInfo m2C_HorseNoticeInfo = self.ZoneScene().GetComponent<ChatComponent>().HorseNoticeInfo;
            switch (m2C_HorseNoticeInfo.NoticeType)
            {
                case NoticeType.KillEvent:
                    ChatInfo chatInfo = new ChatInfo() { };
                    chatInfo.ChatMsg = m2C_HorseNoticeInfo.NoticeText;
                    chatInfo.Time = TimeHelper.ServerNow();
                    chatInfo.ChannelId = ChannelEnum.System;
                    self.ZoneScene().GetComponent<ChatComponent>().OnRecvChat(chatInfo);
                    break;
                case NoticeType.UnionRace:
                    self.OnUnionRace();
                    break;
                case NoticeType.StopSever:
                    self.OnStopServer().Coroutine();
                    break;
                case NoticeType.TianQiChange:
                    self.OnTianQiChange(m2C_HorseNoticeInfo.NoticeText);
                    break;
                case NoticeType.PaiMaiAuction:
                    UI uI = UIHelper.GetUI(self.DomainScene(), UIType.UIPaiMaiAuction);
                    if (uI == null)
                    {
                        return;
                    }
                    uI.GetComponent<UIPaiMaiAuctionComponent>()?.OnRecvHorseNotice(m2C_HorseNoticeInfo.NoticeText);
                    break;
                default:
                    if (m2C_HorseNoticeInfo.NoticeText.Contains(ConfigHelper.TurtleWinNotice))
                    {
                        //隐藏终点
                        self.OnTurtleEnd(false).Coroutine();
                    }
                    uI = UIHelper.GetUI(self.DomainScene(), UIType.UIHorseNotice);
                    if (uI == null)
                    {
                        return;
                    }
                    uI.GetComponent<UIHorseNoticeComponent>()?.OnRecvHorseNotice(m2C_HorseNoticeInfo);
                    break;
            }
        }

        public static void OnRecvChat(this UIMainComponent self)
        {
            self.UIMainChat.OnRecvChat(self.ZoneScene().GetComponent<ChatComponent>().LastChatInfo);
        }

        public static void OnPetFightSet(this UIMainComponent self)
        {
            self.UIRoleHead.OnPetFightSet();
        }

        public static void OnUpdateRoleName(this UIMainComponent self)
        {
            self.UIRoleHead.UpdateShowRoleName();
        }

        public static void OnClickPageButton(this UIMainComponent self, int page)
        {
            self.UIMainTask.GameObject.SetActive(page == 0);
            self.UIMainTeam.GameObject.SetActive(page == 1);
            //if (self.Obj_Btn_ShouSuo.transform.localScale.x > 0.9f)
            //{
            //    self.UIMainTask.GameObject.SetActive(page == 0);
            //    self.UIMainTeam.GameObject.SetActive(page == 1);
            //}
            //else
            //{
            //    self.UIMainTask.GameObject.SetActive(false);
            //    self.UIMainTeam.GameObject.SetActive(false);
            //}
        }

        public static void OnOpenShouSuo(this UIMainComponent self)
        {
            //if (self.Obj_Btn_ShouSuo.transform.localScale.x > 0.9f)
            //{
            //    self.Obj_Btn_ShouSuo.transform.localScale = new Vector3(-1, 1, 1);
            //    self.UIMainTask.GameObject.SetActive(false);
            //    self.UIMainTeam.GameObject.SetActive(false);
            //}
            //else
            //{
            //    self.Obj_Btn_ShouSuo.transform.localScale = new Vector3(1, 1, 1);
            //    int page = self.UIPageButtonComponent.CurrentIndex;
            //    self.UIMainTask.GameObject.SetActive(page == 0);
            //    self.UIMainTeam.GameObject.SetActive(page == 1);
            //}
        }

        public static void OnRelinkUpdate(this UIMainComponent self)
        {
            self.UIRoleHead.UpdateShowRolePiLao();
        }

        public static void initSubUI(this UIMainComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            //技能
            GameObject mainSkill = rc.Get<GameObject>("UIMainSkill");
            UI uiskill = self.AddChild<UI, string, GameObject>("SubMainSkill", mainSkill);
            self.UIMainSkillComponent = uiskill.AddComponent<UIMainSkillComponent>();

            //摇杆
            GameObject JoystickMove = rc.Get<GameObject>("JoystickMove");
            self.UIJoystickMoveComponent = self.AddComponent<UIJoystickMoveComponent, GameObject>(JoystickMove);

            GameObject UIMainBuff = rc.Get<GameObject>("UIMainBuff");
            self.UIMainBuffComponent = self.AddChild<UIMainBuffComponent, GameObject>(UIMainBuff);

            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            List<ABuffHandler> aBuffs = unit.GetComponent<BuffManagerComponent>().m_Buffs;
            for (int i = 0; i < aBuffs.Count; i++)
            {
                ABuffHandler buffHandler = aBuffs[i];
                self.UIMainBuffComponent.OnAddBuff(buffHandler);
            }

            GameObject UIOpenBox = rc.Get<GameObject>("UIOpenBox");
            UI uiopenbox = self.AddChild<UI, string, GameObject>("UIMainBuff", UIOpenBox);
            self.UIOpenBoxComponent = uiopenbox.AddComponent<UIOpenBoxComponent>();

            GameObject uisinging = rc.Get<GameObject>("UISinging");
            self.UISingingComponent = self.AddChild<UISingingComponent, GameObject>(uisinging);

            //任务
            GameObject taskShowSet = rc.Get<GameObject>("UIMainTask");
            self.UIMainTask = self.AddChild<UIMainTaskComponent, GameObject>(taskShowSet);

            //组队
            GameObject mainTeamSet = rc.Get<GameObject>("UIMainTeam");
            self.UIMainTeam = self.AddChild<UIMainTeamComponent, GameObject>(mainTeamSet);

            //关卡小地图
            self.LevelGuideMini = rc.Get<GameObject>("LevelGuideMini");
            self.UILevelGuideMini = self.AddChild<UI, string, GameObject>("LevelGuideMini", self.LevelGuideMini);
            self.UILevelGuideMini.AddComponent<UICellDungeonCellMiniComponent>();

            GameObject UIMapMini = rc.Get<GameObject>("UIMapMini");
            self.UIMapMini = self.AddChild<UIMapMiniComponent, GameObject>(UIMapMini);

            //聊天
            GameObject MainChat = rc.Get<GameObject>("UIMainChat");
            self.UIMainChat = self.AddChild<UIMainChatComponent, GameObject>(MainChat);

            GameObject DigTreasure = rc.Get<GameObject>("UIDigTreasure");
            self.UIDigTreasureComponent = self.AddChild<UIDigTreasureComponent, GameObject>(DigTreasure);
            self.UIDigTreasureComponent.GameObject.SetActive(false);

            GameObject MainActivityTip = rc.Get<GameObject>("UIMainActivityTip");
            self.UIMainActivityTipComponent = self.AddChild<UIMainActivityTipComponent, GameObject>(MainActivityTip);

            //左上角头像
            GameObject RoleHead = rc.Get<GameObject>("UIRoleHead");
            self.UIRoleHead = self.AddChild<UIRoleHeadComponent, GameObject>(RoleHead);

            GameObject UIMainHpBar = rc.Get<GameObject>("UIMainHpBar");
            self.UIMainHpBar = self.AddChild<UIMainHpBarComponent, GameObject>(UIMainHpBar);

            self.FunctionSetBtn = rc.Get<GameObject>("FunctionSetBtn");
            UI buttonSet = self.AddChild<UI, string, GameObject>("FunctionBtnSet", self.FunctionSetBtn);
            UIPageButtonComponent uIPageViewComponent = buttonSet.AddComponent<UIPageButtonComponent>();
            uIPageViewComponent.SetClickHandler((int page) =>
            {
                self.OnClickPageButton(page);
            });
            uIPageViewComponent.OnSelectIndex(0);
            self.UIPageButtonComponent = uIPageViewComponent;

            GameObject ButtonPositionSet = rc.Get<GameObject>("ButtonPositionSet");
            self.UIMainButtonPositionComponent = self.AddChild<UIMainButtonPositionComponent, GameObject>(ButtonPositionSet);
            self.UIMainButtonPositionComponent.InitButtons(self.GetParent<UI>().GameObject);
            self.UIMainButtonPositionComponent.GameObject.SetActive(false);

            //IOS适配
            GameObject PhoneLeft = rc.Get<GameObject>("PhoneLeft");
            IPHoneHelper.SetPosition(PhoneLeft, new Vector2(120f, 0f));
            IPHoneHelper.SetPosition(mainSkill, new Vector2(-120f, 0f));
            //IPHoneHelper.SetPosition(self.duihuaButton, new Vector2(-120f, 0f));

            UIHelper.Create(self.DomainScene(), UIType.UIHorseNotice).Coroutine();

            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            if (userInfoComponent.GetGameSettingValue(GameSettingEnum.FenBianlLv) == "1")
            {
                self.SetFenBianLv1();
            }
            if (userInfoComponent.GetGameSettingValue(GameSettingEnum.FenBianlLv) == "2")
            {
                self.SetFenBianLv2();
            }

            if (PlayerPrefsHelp.GetInt(PlayerPrefsHelp.LastFrame) == 0)
            {
                UIHelper.Create(self.ZoneScene(), UIType.UISettingFrame).Coroutine();
            }
            else
            {
                string oldValue = userInfoComponent.GetGameSettingValue(GameSettingEnum.HighFps);
                UICommonHelper.TargetFrameRate(oldValue == "1" ? 60 : 30);
            }

            string attackmode = userInfoComponent.GetGameSettingValue(GameSettingEnum.AttackTarget);
            self.ZoneScene().GetComponent<LockTargetComponent>().AttackTarget = int.Parse(attackmode);

            self.ZoneScene().GetComponent<LockTargetComponent>().SkillAttackPlayerFirst =
                    int.Parse(userInfoComponent.GetGameSettingValue(GameSettingEnum.SkillAttackPlayerFirst));

            float lenDepth = PlayerPrefsHelp.GetFloat(PlayerPrefsHelp.LenDepth);
            self.ZoneScene().CurrentScene().GetComponent<CameraComponent>().LenDepth = lenDepth <= 0? 1 : lenDepth;
            
            self.DragPanel.SetActive(PlayerPrefsHelp.GetInt(PlayerPrefsHelp.RotaAngle) == 1);

            userInfoComponent.PickSet = userInfoComponent.GetGameSettingValue(GameSettingEnum.PickSet).Split('@');

            self.CheckRechargeRewardButton();
        }

        public static void OnZeroClockUpdate(this UIMainComponent self)
        {
            self.InitFunctionButton();
        }

        public static void OnHongBao(this UIMainComponent self, int value)
        {
            if (value == 1)
            {
                self.Button_HongBao.SetActive(false);
            }
        }

        public static void CheckPopUP(this UIMainComponent self)
        {
            AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
            int ispopup = accountInfoComponent.IsPopUp;
            if (ispopup == 1)
            {
                string[] popInfos = self.ZoneScene().GetComponent<AccountInfoComponent>().PopUpInfo.Split('@');

                // 服务器
                if (popInfos[0] != "0" && popInfos[0] != accountInfoComponent.ServerId.ToString())
                {
                    return;
                }

                // 角色名字
                if (popInfos[1] != "0" && popInfos[1] != self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Name)
                {
                    return;
                }

                PopupTipHelp.OpenPopupTip(self.ZoneScene(), "系统提示", popInfos[2], null).Coroutine();
            }
        }

        public static void InitFunctionButton(this UIMainComponent self)
        {
            self.FunctionButtons.Clear();

            long serverTime = TimeHelper.ServerNow();
            DateTime dateTime = TimeInfo.Instance.ToDateTime(serverTime);
            long curTime = (dateTime.Hour * 60 + dateTime.Minute) * 60 + dateTime.Second;
            self.MainUnit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());

            //1058变身大赛 1055喜从天降 1052狩猎活动 1045竞技场    1062争霸捐献 1063开区奖励 1064活跃     1065商城 1066活动      
            //1040拍卖特惠 1023红包活动 1067新年活动 1068萌新福利  1069分享     1016排行榜   1025战场活动 1070世界等级 1014拍卖行

            List<int> functonIds = new List<int>() { 1023, 1025, 1031, 1040, 1045, 1052, 1055, 1057, 1058, 1059,
                                                     1062, 1063, 1064, 1065, 1066, 1067, 1068 ,1069, 1016, 1070, 1014, 1071 };
            for (int i = 0; i < functonIds.Count; i++)
            {
                long startTime = FunctionHelp.GetOpenTime(functonIds[i]);
                long endTime = FunctionHelp.GetCloseTime(functonIds[i]) - 10;

                if (functonIds[i] == 1025) //战场按钮延长30分钟消失
                {
                    endTime += (30 * 60);
                }
                if (functonIds[i] == 1040)  //拍卖特惠显示到12点
                {
                    int maxTime = 23 * 60 + 58;
                    int curMinute = dateTime.Hour * 60 + dateTime.Minute;
                    Log.ILog.Debug($"拍卖特惠剩余显示时间:{maxTime - curMinute}");
                    endTime += (( maxTime - curMinute ) * 60);
                }
                if (functonIds[i] == 1052)
                {
                    endTime += (10 * 60);
                }
                if (curTime >= endTime)
                {
                    continue;
                }

                long sTime = serverTime + (startTime - curTime) * 1000;
                self.FunctionButtons.Add(new ActivityTimer() { FunctionId = functonIds[i], FunctionType = 1, BeginTime = sTime });  //FunctionType1 并且大于beingTime 开启

                long eTime = serverTime + (endTime - curTime) * 1000;
                self.FunctionButtons.Add(new ActivityTimer() { FunctionId = functonIds[i], FunctionType = 0, BeginTime = eTime });  //FunctionType0 并且大于beingTime 关闭时间点

                //if (curTime < startTime)
                //{
                //    long sTime = serverTime + (startTime - curTime) * 1000;
                //    self.FunctionButtons.Add(new ActivityTimer() { FunctionId = functonIds[i], FunctionType = 1, BeginTime = sTime });  //FunctionType1 并且大于beingTime 开启

                //    long eTime = serverTime + (endTime - curTime) * 1000;
                //    self.FunctionButtons.Add(new ActivityTimer() { FunctionId = functonIds[i], FunctionType = 0, BeginTime = eTime });  //FunctionType0 并且大于beingTime 关闭时间点
                //}
                //else
                //{
                //    self.FunctionButtons.Add(new ActivityTimer() { FunctionId = functonIds[i], FunctionType = 1, BeginTime = serverTime });//FunctionType1 并且大于beingTime 开启 会直接开启

                //    long eTime = serverTime + (endTime - curTime) * 1000;
                //    self.FunctionButtons.Add(new ActivityTimer() { FunctionId = functonIds[i], FunctionType = 0, BeginTime = eTime });  //FunctionType0 并且大于beingTime 关闭时间点
                //}
            }

            TimerComponent.Instance.Remove(ref self.TimerFunctiuon);
            if (self.FunctionButtons.Count > 0)
            {
                self.FunctionButtons.Sort(delegate (ActivityTimer a, ActivityTimer b)
                {
                    long endTime_1 = a.BeginTime;
                    long endTime_2 = b.BeginTime;
                    return (int)(endTime_1 - endTime_2);
                });

                self.TimerFunctiuon = TimerComponent.Instance.NewOnceTimer(self.FunctionButtons[0].BeginTime, TimerType.UIMainTimer, self);
            }
        }

        public static void CheckFuntionButtonByLv(this UIMainComponent self, int lv)
        {
            long serverTime = TimeHelper.ServerNow();
            DateTime dateTime = TimeInfo.Instance.ToDateTime(serverTime);

            long curTime = (dateTime.Hour * 60 + dateTime.Minute) * 60 + dateTime.Second;

            for (int i = 0; i < self.FunctionButtons.Count; i++)
            {
                if (self.FunctionButtons[i].FunctionType != 0)
                {
                    continue;
                }

                int functionId = self.FunctionButtons[i].FunctionId;

                long startTime = FunctionHelp.GetOpenTime(functionId);

                bool todayopen = FunctionHelp.IsFunctionDayOpen((int)dateTime.DayOfWeek, functionId);

                FuntionConfig funtionConfig = FuntionConfigCategory.Instance.Get(functionId);
                bool functionOn = FunctionHelp.CheckFuncitonOn(self.ZoneScene(), funtionConfig);

                //只做开启
                if (todayopen && functionOn && curTime >= startTime)
                {
                    self.ShowFunctionButton(functionId, true);
                } 
            }
        }

        public static void ShowFunctionButton(this UIMainComponent self, int functionId,  bool showButton)
        {
            bool blackroom = UnitHelper.IsBackRoom(  self.ZoneScene() );
            if (blackroom && !string.IsNullOrEmpty(FuntionConfigCategory.Instance.Get(functionId).OpenTime))
            {
                showButton = false;
            }

            switch (functionId)
            {
                case 1014:
                    self.Btn_PaiMaiHang.SetActive(showButton);
                    break;
                case 1070:
                    int zone = self.ZoneScene().GetComponent<AccountInfoComponent>().ServerId;
                    int openDay = ServerHelper.GetOpenServerDay(!GlobalHelp.IsOutNetMode, zone);
                    int lastDay = WorldLvHelper.GetWorldLvLastDay();
                    self.Button_WorldLv.SetActive(showButton && openDay <= lastDay + 1);
                    break;
                case 1016:
                    self.Btn_Rank.SetActive(showButton);
                    break;
                case 1069:
                    self.Button_FenXiang.SetActive(showButton && GlobalHelp.GetPlatform() != 6);
                    break;
                case 1067:
                    self.Button_NewYear.SetActive(showButton);
                    break;
                case 1066:
                    self.Btn_HuoDong.SetActive(showButton);
                    break;
                case 1065:
                    self.Button_Recharge.SetActive(showButton);
                    break;
                case 1064:
                    self.Btn_EveryTask.SetActive(showButton);
                    break;
                case 1063:
                    self.Button_ZhanQu.SetActive(showButton);
                    break;
                case 1062:
                    self.Button_Donation.SetActive(showButton);
                    break;
                case 1068:
                    int openday = self.ZoneScene().GetComponent<UserInfoComponent>().GetCrateDay();
                    self.Button_Welfare.SetActive(showButton && openday <= 8);
                    break;
                case 1023:
                    int honbao = self.MainUnit.GetComponent<NumericComponent>().GetAsInt(NumericType.HongBao);
                    self.Button_HongBao.SetActive(showButton && honbao == 0);
                    break;
                case 1025:
                    self.Btn_Battle.SetActive(showButton);
                    break;
                case 1031:
                    if (showButton)
                    {
                        ActivityTipHelper.OnActiviyTip(self.ZoneScene(), functionId);
                    }
                    break;
                case 1040:
                    self.Btn_Auction.SetActive(showButton);
                    break;
                case 1045:
                    self.Button_Solo.SetActive(showButton);
                    break;
                case 1052:
                    self.Button_Hunt.SetActive(showButton);
                    break;
                case 1055:
                    self.Button_Happy.SetActive(showButton);
                    if (showButton)
                    {
                        ActivityTipHelper.OnActiviyTip(self.ZoneScene(), functionId);
                    }
                    break;
                case 1057:
                    if (showButton)
                    {
                        //出现终点位置
                        self.OnTurtleEnd(true).Coroutine();
                    }
                    break;
                case 1058:
                    if (showButton)
                    {
                        ActivityTipHelper.OnActiviyTip(self.ZoneScene(), functionId);
                    }
                    self.Button_RunRace.SetActive(showButton);
                    break;
                case 1059:
                    if (showButton)
                    {
                        ActivityTipHelper.OnActiviyTip(self.ZoneScene(), functionId);
                    }
                    self.Button_Demon.SetActive(showButton);
                    break;
                case 1071:
                    self.Button_Season.SetActive(showButton);
                    break;
                default:
                    break;
            }

        }

        public static void OnCheckFuntionButton(this UIMainComponent self)
        {
            long serverTime = TimeHelper.ServerNow();
            DateTime dateTime = TimeInfo.Instance.ToDateTime(serverTime);

            //1058变身大赛 1055喜从天降 1052狩猎活动 1045竞技场    1062争霸捐献 1063开区奖励 1064活跃     1065商城 1066活动      
            //1040拍卖特惠 1023红包活动 1067新年活动 1068萌新福利  1069分享     1016排行榜   1025战场活动 1070世界等级 1014拍卖行
            int removeNumber = 0;
            for (int i = 0; i< self.FunctionButtons.Count; i++)
            {
                int functionId = self.FunctionButtons[i].FunctionId;
                long sTime = self.FunctionButtons[i].BeginTime;
                bool todayopen = FunctionHelp.IsFunctionDayOpen((int)dateTime.DayOfWeek, functionId);

                FuntionConfig funtionConfig = FuntionConfigCategory.Instance.Get(functionId);
                bool functionOn = FunctionHelp.CheckFuncitonOn(self.ZoneScene(), funtionConfig);

                bool showButton = functionOn && todayopen && self.FunctionButtons[i].FunctionType == 1;

                if (serverTime >= sTime)
                {
                    self.ShowFunctionButton(functionId, showButton);
                    removeNumber++;
                }
            }
            self.FunctionButtons.RemoveRange(0, removeNumber);
            TimerComponent.Instance.Remove(ref self.TimerFunctiuon);

            self.TimerFunctiuon = (self.FunctionButtons.Count > 0) ? TimerComponent.Instance.NewOnceTimer(self.FunctionButtons[0].BeginTime + 10000, TimerType.UIMainTimer, self) : 0;
        }

        public static void SetFenBianLv1(this UIMainComponent self)
        {
            //AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
            //float dishu = GMHelp.IsTestPlayer(accountInfoComponent.Account) ? 0.8f : 1f;
            Screen.SetResolution((int)(UIComponent.Instance.ResolutionWidth ), (int)(UIComponent.Instance.ResolutionHeight), true);
        }

        public static void SetFenBianLv2(this UIMainComponent self)
        {
            //AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
            //float dishu = GMHelp.IsTestPlayer(accountInfoComponent.Account) ? 0.8f : 1f;
            Screen.SetResolution((int)(UIComponent.Instance.ResolutionWidth * 0.8f), (int)(UIComponent.Instance.ResolutionHeight * 0.8f), true);
        }

        public static void UnRegisterRedot(this UIMainComponent self)
        {
            ReddotViewComponent redPointComponent = self.DomainScene().GetComponent<ReddotViewComponent>();
            redPointComponent.UnRegisterReddot(ReddotType.Friend, self.Reddot_Frined);
            redPointComponent.UnRegisterReddot(ReddotType.Team, self.Reddot_Team);
            redPointComponent.UnRegisterReddot(ReddotType.Email, self.Reddot_Email);
            redPointComponent.UnRegisterReddot(ReddotType.RolePoint, self.Reddot_RolePoint);
            redPointComponent.UnRegisterReddot(ReddotType.SkillUp, self.Reddot_SkillUp);
            redPointComponent.UnRegisterReddot(ReddotType.PetSet, self.Reddot_PetSet);
            redPointComponent.UnRegisterReddot(ReddotType.Welfare, self.Reddot_Welfare);
        }

        public static void RegisterReddot(this UIMainComponent self)
        {
            ReddotViewComponent redPointComponent = self.ZoneScene().GetComponent<ReddotViewComponent>();
            redPointComponent.RegisterReddot(ReddotType.Friend, self.Reddot_Frined);
            redPointComponent.RegisterReddot(ReddotType.Team, self.Reddot_Team);
            redPointComponent.RegisterReddot(ReddotType.Email, self.Reddot_Email);
            redPointComponent.RegisterReddot(ReddotType.RolePoint, self.Reddot_RolePoint);
            redPointComponent.RegisterReddot(ReddotType.SkillUp, self.Reddot_SkillUp);
            redPointComponent.RegisterReddot(ReddotType.PetSet, self.Reddot_PetSet);
            redPointComponent.RegisterReddot(ReddotType.Welfare, self.Reddot_Welfare);
            redPointComponent.RegisterReddot(ReddotType.Activity, self.Reddot_Activity);

            ReddotComponent reddotComponent = self.ZoneScene().GetComponent<ReddotComponent>();
            if (reddotComponent.GetReddot(ReddotType.UnionApply) > 0)
            {
                reddotComponent.AddReddont(ReddotType.UnionApply);
            }
            if (reddotComponent.GetReddot(ReddotType.Email) > 0)
            {
                reddotComponent.AddReddont(ReddotType.Email);
            }
            if (reddotComponent.GetReddot(ReddotType.PetMine) > 0)
            {
                reddotComponent.AddReddont(ReddotType.PetMine);
            }

            reddotComponent.UpdateReddont(ReddotType.RolePoint);
            reddotComponent.UpdateReddont(ReddotType.SkillUp);
            reddotComponent.UpdateReddont(ReddotType.WelfareDraw);
            reddotComponent.UpdateReddont(ReddotType.WelfareLogin);
            reddotComponent.UpdateReddont(ReddotType.WelfareTask);
            reddotComponent.UpdateReddont(ReddotType.FriendApply);
            reddotComponent.UpdateReddont(ReddotType.FriendChat);
            reddotComponent.UpdateReddont(ReddotType.SingleRecharge);
        }

        public static void BeginWaterMove(this UIMainComponent self)
        {
            GameObject water = GameObject.Find("zhucheng_dimianshuimian/zhucheng_shui");
            Material material = water.GetComponent<MeshRenderer>().materials[0];
            material.SetTextureOffset("_MainTex", new Vector2(1, 1));
        }

        public static void Reddot_PetSet(this UIMainComponent self, int num)
        {
            self.Btn_PetFormation.transform.Find("Reddot").gameObject.SetActive(num > 0);
        }

        public static void Reddot_Frined(this UIMainComponent self, int num)
        {
            self.Btn_Friend.transform.Find("Reddot").gameObject.SetActive(num > 0);
        }

        public static void Reddot_Welfare(this UIMainComponent self, int num)
        {
            self.Button_Welfare.transform.Find("Reddot").gameObject.SetActive(num > 0);
        }

        public static void Reddot_Activity(this UIMainComponent self, int num)
        { 
           self.Btn_HuoDong.transform.Find("Reddot").gameObject.SetActive(num > 0);
        }

        public static void Reddot_Team(this UIMainComponent self, int num)
        {
            self.UIPageButtonComponent.SetButtonReddot(1, num > 0);
            self.TeamDungeonBtn.transform.Find("Reddot").gameObject.SetActive(num > 0);
        }

        public static void Reddot_RolePoint(this UIMainComponent self, int num)
        { 
            self.bagButton.transform.Find("Reddot").gameObject.SetActive(num > 0);
        }

        public static void Reddot_SkillUp(this UIMainComponent self, int num)
        {
            self.roleSkillBtn.transform.Find("Reddot").gameObject.SetActive(num > 0);
        }

        public static void Reddot_Email(this UIMainComponent self, int num)
        {
            self.MailHintTip.SetActive(num > 0);
            //if (num > 0)
            //{
            //    self.UIMailHintTip.RemoveComponent<UIFadeComponent>();
            //    self.UIMailHintTip.AddComponent<UIFadeComponent>();
            //}
            //else
            //{
            //    self.UIMailHintTip.RemoveComponent<UIFadeComponent>();
            //}
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        public static void BeginEnterScene(this UIMainComponent self, int lastScene)
        {
            self.UIMainTeam.ResetUI();
            self.UIMainSkillComponent.ResetUI();
            self.UIMainBuffComponent.ResetUI();
            self.UIJoystickMoveComponent.ResetUI();

            self.UIMapMini.BeginChangeScene(lastScene);
            self.UISingingComponent.GameObject.SetActive(false);
            self.UIMainHpBar.BeginEnterScene();
            self.ZoneScene().GetComponent<SkillIndicatorComponent>().BeginEnterScene();
            self.ZoneScene().GetComponent<LockTargetComponent>().BeginEnterScene();
            self.ZoneScene().GetComponent<BattleMessageComponent>().CancelRideTargetUnit(0);
            self.ZoneScene().GetComponent<BattleMessageComponent>().AttackSelfPlayer.Clear();
            self.ZoneScene().RemoveComponent<UnitGuaJiComponen>();
        }

        public static void ShowMainUI(this UIMainComponent self, bool show)
        {
            MapComponent mapComponent = self.ZoneScene().GetComponent<MapComponent>();
            int sceneType = mapComponent.SceneTypeEnum;
            self.DoMoveLeft.SetActive(show);
            self.DoMoveRight.SetActive(show);
            self.DoMoveBottom.SetActive(show );
            if (show)
            {
                self.UIMainChat.UpdatePosition().Coroutine();
            }
            else
            {
                self.ZoneScene().GetComponent<SkillIndicatorComponent>()?.RecoveryEffect();
                //self.UIJoystickMoveComponent.ResetUI(); //防止打开其他界面摇杆接受不到ui事件
            }

            switch (sceneType)
            {
                case SceneTypeEnum.JiaYuan:
                    UIHelper.GetUI(self.ZoneScene(), UIType.UIJiaYuanMain).GameObject.SetActive(show);
                    break;
                default:
                    break;
            }
        }

        public static async ETTask CheckMailReddot(this UIMainComponent self)
        {
            if (!self.MailHintTip.activeSelf)
            {
                return;
            }

            C2E_GetAllMailRequest c2E_GetAllMailRequest = new C2E_GetAllMailRequest() { ActorId = self.MainUnit.Id };
            E2C_GetAllMailResponse sendChatResponse = (E2C_GetAllMailResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2E_GetAllMailRequest);
            if(sendChatResponse.MailInfos.Count == 0)
            {
                self.MailHintTip.SetActive(false);
            }
        }

        /// <summary>
        /// 场景加载完成
        /// </summary>
        /// <param name="self"></param>
        /// <param name="sceneTypeEnum"></param>
        public static void AfterEnterScene(this UIMainComponent self, int sceneTypeEnum)
        {
            bool zhankai = self.Button_ZhanKai.transform.localScale == new Vector3(-1f, 1f, 1f);
            self.MainUnit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            self.Btn_TopRight_1.SetActive(zhankai && SceneConfigHelper.ShowRightTopButton(sceneTypeEnum));
            self.Btn_TopRight_2.SetActive(zhankai && SceneConfigHelper.ShowRightTopButton(sceneTypeEnum));
            self.buttonReturn.SetActive(sceneTypeEnum != SceneTypeEnum.MainCityScene && sceneTypeEnum != SceneTypeEnum.JiaYuan);
            self.Btn_MapTransfer.SetActive(sceneTypeEnum == SceneTypeEnum.LocalDungeon);
            self.LevelGuideMini.SetActive(sceneTypeEnum == SceneTypeEnum.CellDungeon);
            self.duihuaButton.SetActive(sceneTypeEnum == SceneTypeEnum.MainCityScene);
            self.ShrinkBtn.SetActive(sceneTypeEnum != SceneTypeEnum.RunRace && sceneTypeEnum != SceneTypeEnum.Demon);
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            string value = userInfoComponent.GetGameSettingValue(GameSettingEnum.HideLeftBottom);
            if (value=="1")
            {
                self.LeftBottomBtns.SetActive(sceneTypeEnum == SceneTypeEnum.MainCityScene);
            }
            else
            {
                 self.LeftBottomBtns.SetActive(self.LeftBottomBtns.activeSelf && sceneTypeEnum != SceneTypeEnum.RunRace && sceneTypeEnum != SceneTypeEnum.Demon);
            }
            self.UIJoystickMoveComponent.AfterEnterScene();
            if(!SceneConfigHelper.ShowLeftButton(sceneTypeEnum))
            {
                self.FunctionSetBtn.SetActive(false);
                self.OnClickPageButton(-1);
            }
            else
            {
                self.FunctionSetBtn.SetActive(true);
                self.UIPageButtonComponent.OnSelectIndex(sceneTypeEnum == SceneTypeEnum.TeamDungeon ? 1 : 0);
            }
            int sceneid = self.ZoneScene().GetComponent<MapComponent>().SceneId;
            switch (sceneTypeEnum)
            {
                case SceneTypeEnum.CellDungeon:
                    self.UILevelGuideMini.GetComponent<UICellDungeonCellMiniComponent>().OnUpdateUI();
                    break;
                case SceneTypeEnum.MainCityScene:
                    self.UIMainHpBar.MonsterNode.SetActive(false);
                    self.UIMainHpBar.BossNode.SetActive(false);
                    self.HomeButton.SetActive(true);
                    self.UIMainSkill.SetActive(false);
                    self.UIMainTask.GameObject.SetActive(true);
                    self.FunctionSetBtn.SetActive(true);
                    self.UIJoystickMoveComponent.GameObject.SetActive(true);
                    break;
                case SceneTypeEnum.Happy:
                    self.HomeButton.SetActive(false);
                    self.UIMainSkill.SetActive(false);
                    self.UIJoystickMoveComponent.GameObject.SetActive(false);
                    break;
                case SceneTypeEnum.RunRace:
                    self.HomeButton.SetActive(false);
                    self.UIMainSkill.SetActive(true);
                    break;
                case SceneTypeEnum.Demon:
                    self.HomeButton.SetActive(false);
                    self.UIMainSkill.SetActive(true);
                    break;
                case SceneTypeEnum.JiaYuan:
                    self.HomeButton.SetActive(false);
                    self.UIMainSkill.SetActive(false);
                    self.UIJoystickMoveComponent.GameObject.SetActive(true);
                    break;
                case SceneTypeEnum.TowerOfSeal:
                    self.UIMainTask.GameObject.SetActive(false);
                    self.FunctionSetBtn.SetActive(false);
                    self.HomeButton.SetActive(false);
                    self.UIMainSkill.SetActive(true);
                    self.UIJoystickMoveComponent.GameObject.SetActive(true);
                    break;
                case SceneTypeEnum.LocalDungeon:
                    DungeonConfig dungeonConfig = DungeonConfigCategory.Instance.Get(sceneid);
                    switch (dungeonConfig.MapType)
                    {
                        case SceneSubTypeEnum.LocalDungeon_1:
                            self.HomeButton.SetActive(false);
                            self.UIMainSkill.SetActive(false);
                            self.Btn_TopRight_1.SetActive(false);
                            self.Btn_TopRight_2.SetActive(false);
                            self.UIJoystickMoveComponent.GameObject.SetActive(false);
                            break;
                        default:
                            self.HomeButton.SetActive(false);
                            self.UIMainSkill.SetActive(true);
                            self.UIJoystickMoveComponent.GameObject.SetActive(true);
                            break;
                    }
                    break;
                default:
                    self.HomeButton.SetActive(false);
                    self.UIMainSkill.SetActive(true);
                    self.UIJoystickMoveComponent.GameObject.SetActive(true);
                    break;
            }

            self.OnHorseRide();
            self.UpdateShadow();
            self.UpdateNpcTaskUI();
            self.UIMapMini.OnEnterScene();
            self.UIMainSkillComponent.OnEnterScene(self.MainUnit, sceneTypeEnum);
            self.UIMainSkillComponent.OnSkillSetUpdate();
            self.UIRoleHead.OnEnterScene(sceneTypeEnum);
            self.ZoneScene().GetComponent<RelinkComponent>().OnApplicationFocusHandler(true);

            self.Btn_Union.SetActive(self.MainUnit.GetComponent<NumericComponent>().GetAsLong(NumericType.UnionId_0) > 0);
            if (sceneTypeEnum == SceneTypeEnum.LocalDungeon)
            {
                self.ZoneScene().GetComponent<GuideComponent>().OnTrigger(GuideTriggerType.EnterFuben, sceneid.ToString());
                bool shenmizhimen = DungeonSectionConfigCategory.Instance.MysteryDungeonList.Contains(self.ZoneScene().GetComponent<MapComponent>().SceneId);
                self.Btn_RerurnDungeon.SetActive(shenmizhimen);
                self.Btn_MapTransfer.SetActive(!shenmizhimen);
                self.buttonReturn.SetActive(!shenmizhimen);
            }

            if (!self.UIJoystickMoveComponent.GameObject.activeSelf
                || sceneTypeEnum == SceneTypeEnum.PetDungeon
                || sceneTypeEnum == SceneTypeEnum.PetTianTi
                || sceneTypeEnum == SceneTypeEnum.PetMing)
            {
                self.MainUnit.GetComponent<StateComponent>().StateTypeAdd(StateTypeEnum.NoMove);
            }

            self.ZoneScene().RemoveComponent<UnitGuaJiComponen>();
            self.UGuaJiSet.SetActive(false);
            
            self.UpdateLvReward();
            self.UpdateKillMonsterReward();

            self.CheckMailReddot().Coroutine();
        }

        public static void OnOpenTask(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UITask).Coroutine();
        }

        public static async ETTask OnButtonStallOpen(this UIMainComponent self)
        {
            UI uI = await UIHelper.Create(self.DomainScene(), UIType.UIPaiMaiStall);

            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            Unit stall = unit.GetParent<UnitComponent>().Get(unit.GetComponent<NumericComponent>().GetAsLong(NumericType.Now_Stall));
            uI.GetComponent<UIPaiMaiStallComponent>().OnUpdateUI(stall);
        }

        public static void OnButtonStallCancel(this UIMainComponent self)
        {
            PopupTipHelp.OpenPopupTip(self.DomainScene(), "摊位提示", "是否收起自己的摊位?\n 支持下线,摊位可以离线显示6小时!",
                       () =>
                       {
                           NetHelper.PaiMaiStallRequest(self.DomainScene(), 0).Coroutine();
                           //界面存在就销毁界面
                           UIHelper.Remove(self.DomainScene(), UIType.UIPaiMaiStall);
                           //弹出提示
                           FloatTipManager.Instance.ShowFloatTipDi("摊位已收起!");
                       }).Coroutine();
        }

        public static async ETTask OnBtn_LvReward(this UIMainComponent self)
        {
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            string[] occItems = ConfigHelper.LevelRewardItem[self.LevelRewardKey].Split('&');
            string[] items;
            if (occItems.Length == 3)
            {
                items = occItems[userInfoComponent.UserInfo.Occ - 1].Split('@');
            }
            else
            {
                items = occItems[0].Split('@');
            }
            string[] item = items[0].Split(';');
            if (userInfoComponent.UserInfo.Lv < self.LevelRewardKey)
            {
                EventType.ShowItemTips.Instance.ZoneScene = self.DomainScene();
                EventType.ShowItemTips.Instance.bagInfo = new BagInfo()
                {
                    ItemID = int.Parse(item[0]),
                    ItemNum = int.Parse(item[1])
                };
                EventType.ShowItemTips.Instance.itemOperateEnum = ItemOperateEnum.None;
                EventType.ShowItemTips.Instance.inputPoint = Input.mousePosition;
                EventType.ShowItemTips.Instance.Occ = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Occ;
                Game.EventSystem.PublishClass(EventType.ShowItemTips.Instance);
            }
            else
            {
                if (items.Length > 1)
                {
                    UI ui = await UIHelper.Create(self.ZoneScene(), UIType.UISelectReward);
                    ui.GetComponent<UISelectRewardComponent>().UpdateInfo(self.LevelRewardKey, 0);
                }
                else
                {
                    // 一个道具直接领取
                    C2M_LeavlRewardRequest request = new C2M_LeavlRewardRequest() { LvKey = self.LevelRewardKey };
                    M2C_LeavlRewardResponse response =
                            await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request) as M2C_LeavlRewardResponse;
                    self.UpdateLvReward();
                }
            }
        }

        public static void UpdateLvReward(this UIMainComponent self)
        {
            NumericComponent numericComponent = self.MainUnit.GetComponent<NumericComponent>();
            int oldLv = numericComponent.GetAsInt(NumericType.LeavlReward);

            int newLv = int.MaxValue;
            bool flag = false;
            foreach (int key in ConfigHelper.LevelRewardItem.Keys)
            {
                if (key > oldLv)
                {
                    newLv = Math.Min(key, newLv);
                    flag = true;
                }
            }

            if (flag)
            {
                self.LevelRewardKey = newLv;

                string[] occItems = ConfigHelper.LevelRewardItem[self.LevelRewardKey].Split('&');
                string[] items;
                UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
                if (occItems.Length > 1)
                {
                    items = occItems[userInfoComponent.UserInfo.Occ - 1].Split('@');
                }
                else
                {
                    items = occItems[0].Split('@');
                }
                
                string[] item = items[0].Split(';');

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(int.Parse(item[0]));
                ReferenceCollector rc = self.Btn_LvReward.GetComponent<ReferenceCollector>();

                string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
                Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
                if (!self.AssetPath.Contains(path))
                {
                    self.AssetPath.Add(path);
                }
                rc.Get<GameObject>("Image_ItemIcon").GetComponent<Image>().sprite = sp;

                string qualityiconStr = FunctionUI.GetInstance().ItemQualiytoPath(itemConfig.ItemQuality);
                string path1 = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, qualityiconStr);
                Sprite sp1 = ResourcesComponent.Instance.LoadAsset<Sprite>(path1);
                if (!self.AssetPath.Contains(path1))
                {
                    self.AssetPath.Add(path);
                }
                rc.Get<GameObject>("Image_ItemQuality").GetComponent<Image>().sprite = sp1;

                rc.Get<GameObject>("Label_ItemNum").GetComponent<Text>().text = item[1];

                string color = "FFFFFF";
                if (userInfoComponent.UserInfo.Lv >= newLv)
                {
                    color = "C4FF00";
                }
                rc.Get<GameObject>("LvText (1)").GetComponent<Text>().text = $"<color=#{color}>等级奖励</color>";
                rc.Get<GameObject>("LvText").GetComponent<Text>().text = $"<color=#{color}>{newLv}级领取</color>";
                self.Btn_LvReward.SetActive(true);
            }
            else
            {
                self.Btn_LvReward.SetActive(false);
            }
        }

        public static async ETTask OnBtn_KillMonsterReward(this UIMainComponent self)
        {
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            NumericComponent numericComponent = self.MainUnit.GetComponent<NumericComponent>();
            string[] occItems = ConfigHelper.KillMonsterReward[self.KillMonsterRewardKey].Split('&');
            string[] items;
            if (occItems.Length == 3)
            {
                items = occItems[userInfoComponent.UserInfo.Occ - 1].Split('@');
            }
            else
            {
                items = occItems[0].Split('@');
            }
            string[] item = items[0].Split(';');
            if (numericComponent.GetAsInt(NumericType.KillMonsterNumber) < self.KillMonsterRewardKey)
            {
                EventType.ShowItemTips.Instance.ZoneScene = self.DomainScene();
                EventType.ShowItemTips.Instance.bagInfo = new BagInfo()
                {
                    ItemID = int.Parse(item[0]),
                    ItemNum = int.Parse(item[1])
                };
                EventType.ShowItemTips.Instance.itemOperateEnum = ItemOperateEnum.None;
                EventType.ShowItemTips.Instance.inputPoint = Input.mousePosition;
                EventType.ShowItemTips.Instance.Occ = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Occ;
                Game.EventSystem.PublishClass(EventType.ShowItemTips.Instance);
            }
            else
            {
                if (items.Length > 1)
                {
                    UI ui = await UIHelper.Create(self.ZoneScene(), UIType.UISelectReward);
                    ui.GetComponent<UISelectRewardComponent>().UpdateInfo(self.KillMonsterRewardKey, 1);
                }
                else
                {
                    // 一个道具直接领取
                    C2M_KillMonsterRewardRequest request = new C2M_KillMonsterRewardRequest() { Key = self.KillMonsterRewardKey };
                    M2C_KillMonsterRewardResponse response =
                            await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request) as M2C_KillMonsterRewardResponse;

                    if (!self.IsDisposed)
                    {
                        self.UpdateKillMonsterReward();
                    }
                }
            }
        }

        public static void UpdateKillMonsterReward(this UIMainComponent self)
        {
            if (self.MainUnit == null)
            {
                return;
            }
            NumericComponent numericComponent = self.MainUnit.GetComponent<NumericComponent>();
            int oldNum = numericComponent.GetAsInt(NumericType.KillMonsterReward);
            int newNum = int.MaxValue;
            bool flag = false;
            foreach (int key in ConfigHelper.KillMonsterReward.Keys)
            {
                if (key > oldNum)
                {
                    newNum = Math.Min(key, newNum);
                    flag = true;
                }
            }

            if (flag)
            {
                self.KillMonsterRewardKey = newNum;

                string[] occItems = ConfigHelper.KillMonsterReward[self.KillMonsterRewardKey].Split('&');
                string[] items;
                if (occItems.Length == 3)
                {
                    UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
                    items = occItems[userInfoComponent.UserInfo.Occ - 1].Split('@');
                }
                else
                {
                    items = occItems[0].Split('@');
                }

                string[] item = items[0].Split(';');

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(int.Parse(item[0]));
                ReferenceCollector rc = self.Btn_KillMonsterReward.GetComponent<ReferenceCollector>();

                string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
                Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
                if (!self.AssetPath.Contains(path))
                {
                    self.AssetPath.Add(path);
                }
                rc.Get<GameObject>("Image_ItemIcon").GetComponent<Image>().sprite = sp;

                string qualityiconStr = FunctionUI.GetInstance().ItemQualiytoPath(itemConfig.ItemQuality);
                string path1 = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, qualityiconStr);
                Sprite sp1 = ResourcesComponent.Instance.LoadAsset<Sprite>(path1);
                if (!self.AssetPath.Contains(path1))
                {
                    self.AssetPath.Add(path);
                }
                rc.Get<GameObject>("Image_ItemQuality").GetComponent<Image>().sprite = sp1;
                rc.Get<GameObject>("Label_ItemNum").GetComponent<Text>().text = item[1];
                
                string color = "FFFFFF";
                if (numericComponent.GetAsInt(NumericType.KillMonsterNumber) >= newNum)
                {
                    color = "C4FF00";
                }
                rc.Get<GameObject>("LvText (1)").GetComponent<Text>().text = $"<color=#{color}>击败怪物</color>";
                rc.Get<GameObject>("LvText").GetComponent<Text>().text =
                        $"<color=#{color}>{numericComponent.GetAsInt(NumericType.KillMonsterNumber)}/{newNum}</color>";
                self.Btn_KillMonsterReward.SetActive(true);
            }
            else
            {
                self.Btn_KillMonsterReward.SetActive(false);
            }
        }

        public static void OnMailHintTip(this UIMainComponent self)
        {
            MapComponent mapComponent = self.ZoneScene().GetComponent<MapComponent>();
            if (mapComponent.SceneTypeEnum != (int)SceneTypeEnum.MainCityScene)
            {
                FloatTipManager.Instance.ShowFloatTipDi("请前往主城!");
                return;
            }
            self.ZoneScene().CurrentScene().GetComponent<OperaComponent>().OnClickNpc(20000006).Coroutine();
        }

        public static void OnBtn_Battle(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIBattle).Coroutine();
        }

        public static void OnBtn_Friend(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIFriend).Coroutine();
        }

        public static void OnButton_HongBao(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIHongBao).Coroutine();
        }

        public static void OnButton_Tower(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIRandomTower).Coroutine();
        }

        public static void OnButton_JiaYuan(this UIMainComponent self)
        {
            Scene zoneScene = self.ZoneScene();
      
            UserInfoComponent userInfoComponent = zoneScene.GetComponent<UserInfoComponent>();
            zoneScene.GetComponent<JiaYuanComponent>().MasterId = userInfoComponent.UserInfo.UserId;
            EnterFubenHelp.RequestTransfer(zoneScene, SceneTypeEnum.JiaYuan, 2000011, 1, userInfoComponent.UserInfo.UserId.ToString()).Coroutine();
        }

        public static void OnButton_ZhenYing(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UICamp).Coroutine();
        }

        public static void OnButton_WorldLv(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIWorldLv).Coroutine();
        }

        public static void OnButton_Horse(this UIMainComponent self, bool showtip)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene( self.ZoneScene() );
            int now_horse = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.HorseRide);
            if (now_horse == 0 && !self.ZoneScene().GetComponent<BattleMessageComponent>().IsCanRideHorse())
            {
                FloatTipManager.Instance.ShowFloatTip("战斗状态不能骑马!");
                return;
            }

            MapComponent mapComponent = self.ZoneScene().GetComponent<MapComponent>();
            if (SceneConfigHelper.UseSceneConfig(mapComponent.SceneTypeEnum))
            {
                int sceneid = mapComponent.SceneId;
                SceneConfig sceneConfig = SceneConfigCategory.Instance.Get(sceneid);
                if (sceneConfig.IfMount == 1)
                {
                    if (showtip)
                    {
                        FloatTipManager.Instance.ShowFloatTip("该场景不能骑马!");
                    }
                    return;
                }
            }
           
            C2M_HorseRideRequest request = new C2M_HorseRideRequest() {  };
            self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request).Coroutine();
        }

        public static void OnButton_FenXiang(this UIMainComponent self)
        {
            UIHelper.Create(self.ZoneScene(), UIType.UIFenXiang).Coroutine();
        }


        public static async ETTask Button_ActivityV1(this UIMainComponent self)
        {
            await NetHelper.RequestActivityInfo(self.ZoneScene());
            await UIHelper.Create(self.ZoneScene(), UIType.UIActivityV1);
            //UI命名规则
            //UIActivityV1  UIActivityV1ChouKa  UIActivityV1Guess    UIActivityV1Consume UIActivityV1HongBao  UIActivityV1Shop
            //Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());    
            //NumericComponent numericComponent= unit.GetComponent<NumericComponent>();

            //Log.ILog.Debug($"今日消耗钻石:{numericComponent.GetAsInt(NumericType.V1DayCostDiamond)}");
            //Log.ILog.Debug($"今日充值数量:{numericComponent.GetAsInt(NumericType.V1RechageNumber)}");     //充值数量/98=总共可以领取的红包数量
            //Log.ILog.Debug($"今日领取红包:{numericComponent.GetAsInt(NumericType.V1HongBaoNumber)}");     

            //C2M_ActivityInfoRequest request_1 = new C2M_ActivityInfoRequest();
            //M2C_ActivityInfoResponse response_1 = (M2C_ActivityInfoResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request_1);

            //Log.ILog.Debug($"今日抽卡掉落ID: {response_1.ActivityV1Info.ChouKaDropId}");   //DropHelper.DropIDToShowItem() 展示今日奖品
            //Log.ILog.Debug($"今日已竞猜数字: {response_1.ActivityV1Info.GuessIds.Count}");  //数字012345 对应元旦新年快乐
            //Log.ILog.Debug($"今日竞猜中奖纪录: {response_1.ActivityV1Info.LastGuessReward.Count}");
            //Log.ILog.Debug($"今日已领取抽卡次数奖励: {response_1.ActivityV1Info.ChouKaNumberReward.Count}");
            //Log.ILog.Debug($"今日已领取消费次数奖励: {response_1.ActivityV1Info.ConsumeDiamondReward.Count}");

            ////开始抽奖
            //C2M_ActivityChouKaRequest request_2 = new C2M_ActivityChouKaRequest() { };
            //M2C_ActivityChouKaResponse response_2 = (M2C_ActivityChouKaResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request_2);

            ////今日抽卡次数奖励
            //C2M_ActivityRewardRequest request_3 = new C2M_ActivityRewardRequest() { ActivityType = ActivityConfigHelper.ActivityV1_ChouKa, RewardId = 1 };
            //M2C_ActivityRewardResponse response_3 = (M2C_ActivityRewardResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request_3);
            //self.ZoneScene().GetComponent<ActivityComponent>().ActivityV1Info.ChouKaNumberReward.Add(1);
            //Log.Debug("已领取抽卡奖励：" + self.ZoneScene().GetComponent<ActivityComponent>().ActivityV1Info.ChouKaNumberReward.Count);

            ////竞猜。 第二个数字要需要需要道具ActivityConfigHelper.GuessCostItem  竞猜奖励ActivityConfigHelper.GuessRewardList
            //C2M_ActivityGuessRequest request_4 = new C2M_ActivityGuessRequest() { GuessId = 0 };
            //M2C_ActivityGuessResponse response_4 = (M2C_ActivityGuessResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request_4);
            //self.ZoneScene().GetComponent<ActivityComponent>().ActivityV1Info.GuessIds.Add(0);
            //Log.Debug("已竞猜数字：" + self.ZoneScene().GetComponent<ActivityComponent>().ActivityV1Info.GuessIds.Count);
            //新年商店参考战场商店。StoreSellConfig  C2M_StoreBuyRequest

            //C2M_ActivityRewardRequest reuqest_1 = new C2M_ActivityRewardRequest() { ActivityType = ActivityConfigHelper.ActivityV1_LiBao, RewardId = 1 };
            //M2C_ActivityRewardResponse response_1 = (M2C_ActivityRewardResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(reuqest_1);
            //C2M_ActivityFeedRequest reuqest_1 = new C2M_ActivityFeedRequest() {  ItemID = 10030013 };
            //M2C_ActivityFeedResponse response_1 = (M2C_ActivityFeedResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(reuqest_1);
            C2M_ChouKa2RefreshRequest request_3 = new C2M_ChouKa2RefreshRequest() { };
            M2C_ChouKa2RefreshResponse response_3 = (M2C_ChouKa2RefreshResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request_3);
        }

        public static void OnButton_RechargeReward(this UIMainComponent self)
        {
            UIHelper.Create( self.ZoneScene(), UIType.UIRechargeReward ).Coroutine();
        }

        public static void OnButton_ZhanKai(this UIMainComponent self)
        {
            bool active = self.Btn_TopRight_1.activeSelf;
            self.Btn_TopRight_1.SetActive(!active);
            self.Btn_TopRight_2.SetActive(!active);

            self.Button_ZhanKai.transform.localScale = active ? new Vector3(1f, 1f, 1f) :  new Vector3(-1f, 1f, 1f);
        }

        public static void OnButton_NewYear(this UIMainComponent self)
        {
            UIHelper.Create(self.ZoneScene(), UIType.UINewYear).Coroutine();
        }

        public static void OnOpenBag(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIRole).Coroutine();
        }

        public static void OnOpenChengjiu(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIChengJiu).Coroutine();
        }

        public static void  OnButton_Welfare(this UIMainComponent self)
        {
            UIHelper.Create(self.ZoneScene(), UIType.UIWelfare).Coroutine(); 
        }

        public static async ETTask OnButton_Season(this UIMainComponent self)
        {
            int lv = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Lv;
            if (SeasonHelper.GetOpenSeason(lv) == null)
            {
                FloatTipManager.Instance.ShowFloatTip("赛季已结束！");
                return;
            }

            UIHelper.Create(self.DomainScene(), UIType.UISeason).Coroutine();
            ////宠物穿戴装备
            //PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            //long petInfoId = petComponent.RolePetInfos[0].Id;

            //long bagInfoId = 0;
            //List<BagInfo> itemlist = self.ZoneScene().GetComponent<BagComponent>().GetBagList();
            //for (int i = 0; i < itemlist.Count; i++)
            //{
            //    if (ItemConfigCategory.Instance.Get(itemlist[i].ItemID).EquipType == 301)
            //    {
            //        bagInfoId = itemlist[i].BagInfoID;
            //        break;
            //    }
            //}

            ////宠物穿戴装备
            //await NetHelper.ReqeustPetEquip(self.ZoneScene(), bagInfoId, petInfoId, 1);
            //await TimerComponent.Instance.WaitAsync(2000);
            ////宠物卸载装备
            //await NetHelper.ReqeustPetEquip(self.ZoneScene(), bagInfoId, petInfoId, 2);

            await ETTask.CompletedTask;
        }

        public static void OnBtn_RerurnDungeon(this UIMainComponent self)
        {
            PopupTipHelp.OpenPopupTip(self.DomainScene(), "返回副本", GameSettingLanguge.LoadLocalization("移动次数消耗完毕,请返回副本!"),
                () =>
                {
                    int sceneid = self.ZoneScene().GetComponent<BattleMessageComponent>().LastDungeonId;
                    if (sceneid == 0)
                    {
                        EnterFubenHelp.RequestQuitFuben(self.ZoneScene());
                    }
                    else 
                    {
                        EnterFubenHelp.RequestTransfer(self.ZoneScene(), SceneTypeEnum.LocalDungeon, sceneid, 0, "0").Coroutine();
                    }
                },
                null).Coroutine();
        }

        public static async ETTask OnBtn_MapTransfer(this UIMainComponent self)
        {
            UI uI = await UIHelper.Create(self.DomainScene(), UIType.UIDungeonMapTransfer);
            uI.GetComponent<UIDungeonMapTransferComponent>().UpdateChapterList();
            uI.GetComponent<UIDungeonMapTransferComponent>().UpdateBossRefreshTimeList().Coroutine();
        }

        public static void OnClickReturnButton(this UIMainComponent self)
        {
            Scene zoneScene = self.ZoneScene();
            MapComponent mapComponent = zoneScene.GetComponent<MapComponent>();
            UI uI = UIHelper.GetUI(self.ZoneScene(), UIType.UITowerOpen);
            if (uI != null && uI.GetComponent<UITowerOpenComponent>().M2C_FubenSettlement == null)
            {
                uI.GetComponent<UITowerOpenComponent>().RequestTowerQuit();
                return;
            }

            string tipStr = "确定返回主城？";
            if (mapComponent.SceneTypeEnum == SceneTypeEnum.Battle)
            {
                tipStr = "现在离开战场,将不会获得战场胜利的奖励哦";
            }

            PopupTipHelp.OpenPopupTip(self.DomainScene(), "", GameSettingLanguge.LoadLocalization(tipStr),
                () =>
                {
                    EnterFubenHelp.RequestQuitFuben(self.ZoneScene());
                },
                null).Coroutine();
        }

        //初始化界面基础信息
        public static void InitShow(this UIMainComponent self)
        {
            self.UpdateShowRoleExp();

            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            self.ShowUIStall(unit.GetComponent<NumericComponent>().GetAsLong((int)NumericType.Now_Stall));

            self.OnTianQiChange(self.ZoneScene().GetComponent<AccountInfoComponent>().TianQiValue);
        }

        public static void ShowUIStall(this UIMainComponent self, long stallId)
        {
            self.UIStall.SetActive(stallId > 0);
        }

        //角色经验更新
        public static void UpdateShowRoleExp(this UIMainComponent self)
        {
            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
            if (!ExpConfigCategory.Instance.Contain(userInfo.Lv))
            {
                FloatTipManager.Instance.ShowFloatTip("非法修改数据！");
                return;
            }
            self.Obj_Lab_ExpValue.GetComponent<Text>().text = userInfo.Exp.ToString() + "/" + ExpConfigCategory.Instance.Get(userInfo.Lv).UpExp;
            self.Obj_Img_ExpPro.GetComponent<Image>().fillAmount = (float)userInfo.Exp / (float)ExpConfigCategory.Instance.Get(userInfo.Lv).UpExp;
        }


        public static void OnZhaoHuan(this UIMainComponent self)
        {
            MapHelper.SendZhaoHuan(self.DomainScene());
        }

        public static async ETTask OnEnterChapter(this UIMainComponent self)
        {
            self.adventureBtn.SetActive(false);
            UI uI = await UIHelper.Create(self.DomainScene(), UIType.UIDungeon);
            uI.GetComponent<UIDungeonComponent>().UpdateChapterList().Coroutine();
            uI.GetComponent<UIDungeonComponent>().UpdateBossRefreshTimeList().Coroutine();
            self.adventureBtn.SetActive(true);
        }

        public static void OnHorseRide(this UIMainComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            self.Button_Horse.SetActive(unit.GetComponent<NumericComponent>().GetAsInt(NumericType.HorseFightID) > 0);
            self.Button_CityHorse.SetActive(unit.GetComponent<NumericComponent>().GetAsInt(NumericType.HorseFightID) > 0);
        }

        public static void OnShowFubenIndex(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UICellDungeonCell).Coroutine();
        }

        public static void OnClickSkillButton(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UISkill).Coroutine();
        }

        public static void OnOpenBigMap(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIMapBig).Coroutine();
        }

        public static void OnOpenMap(this UIMainComponent self)
        {
            int sceneType = self.ZoneScene().GetComponent<MapComponent>().SceneTypeEnum;
            int sceneId = self.ZoneScene().GetComponent<MapComponent>().SceneId;
            switch (sceneType)
            {
                case (int)SceneTypeEnum.MainCityScene:
                case (int)SceneTypeEnum.LocalDungeon:
                    self.OnOpenBigMap();        //打开主城
                    break;
                case (int)SceneTypeEnum.CellDungeon:
                    self.OnShowFubenIndex();        //打开副本小地图
                    break;
                default:
                    SceneConfig sceneConfig = SceneConfigCategory.Instance.Get(sceneId);
                    if (sceneConfig.ifShowMinMap == 0)
                    {
                        FloatTipManager.Instance.ShowFloatTip(GameSettingLanguge.LoadLocalization("当前场景不支持查看小地图"));
                    }
                    else
                    {
                        self.OnOpenBigMap();        //打开主城
                    }
                    break;
            }
        }

        public static  void OnChapterOpen(this UIMainComponent self)
        {
            var BaseObj = ResourcesComponent.Instance.LoadAsset<GameObject>(ABPathHelper.GetUGUIPath("Chapter/UIChapterOpen"));
            UI uiskillButton = self.AddChild<UI, string, GameObject>("UIChapterOpen", GameObject.Instantiate(BaseObj));
            uiskillButton.AddComponent<UICellChapterOpenComponent>().OnUpdateUI();
            UICommonHelper.SetParent(uiskillButton.GameObject, UIEventComponent.Instance.UILayers[(int)UILayer.Mid].gameObject);

            self.UILevelGuideMini.GetComponent<UICellDungeonCellMiniComponent>().OnChapterOpen(true);
        }

        public static  void OnCellDungeonEnterShow(this UIMainComponent self, int chapterId)
        {
            if (chapterId == 0)
                return;

            var BaseObj = ResourcesComponent.Instance.LoadAsset<GameObject>(ABPathHelper.GetUGUIPath("CellDungeon/UICellDungeonEnterShow"));
            UI uiskillButton = self.AddChild<UI, string, GameObject>("ChapterEnterShow", GameObject.Instantiate(BaseObj));
            uiskillButton.AddComponent<UICellDungeonEnterShowComponent>().OnUpdateUI(chapterId);

            UICommonHelper.SetParent(uiskillButton.GameObject, UIEventComponent.Instance.UILayers[(int)UILayer.Mid].gameObject);
        }

        public static void OnBtn_Email(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIMail).Coroutine();
        }

        public static void OnBtn_EveryTask(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UICountry).Coroutine();
        }

        public static void OnBtn_PaiMaiHang(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIPaiMai).Coroutine();
        }

        public static void OnButton_Energy(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIEnergy).Coroutine();
        }

        public static void OnTeamDungeonBtn(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UITeamDungeon).Coroutine();
        }

        public static void OnBtn_HuoDong(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIActivity).Coroutine();
        }

        public static async ETTask OnButton_ZhanQu(this UIMainComponent self)
        {
            await NetHelper.RequestZhanQuInfo(self.ZoneScene());

            UIHelper.Create(self.DomainScene(), UIType.UIZhanQu).Coroutine();
        }

        public static void OnButton_Recharge(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIRecharge).Coroutine();
        }

        public static void OnBtn_Rank(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIRank).Coroutine();
        }

        public static void OnShrinkBtn(this UIMainComponent self)
        {
            self.LeftBottomBtns.SetActive(!self.LeftBottomBtns.activeSelf);
        }

        public static void OnClickPetButton(this UIMainComponent self)
        {
            UIHelper.Create(self.DomainScene(), UIType.UIPet).Coroutine();
        }

        public static void AutoHorse(this UIMainComponent self)
        {
            NumericComponent numericComponent = self.MainUnit.GetComponent<NumericComponent>();
            if (numericComponent.GetAsInt(NumericType.HorseRide) == 0 && numericComponent.GetAsInt(NumericType.HorseFightID) > 0)
            {
                self.OnButton_Horse(false);
            }
        }

        public static void OnMoveStart(this UIMainComponent self)
        {
            if (self.UIOpenBoxComponent != null && self.UIOpenBoxComponent.BoxUnitId> 0)
            {
                self.UIOpenBoxComponent.OnOpenBox(null);
            }
            self.UIMainSkillComponent.UIAttackGrid.OnMoveStart();

            self.MainUnit.GetComponent<SingingComponent>()?.BeginMove();
        }

        public static void OnSpellStart(this UIMainComponent self)
        {
            if (self.UIOpenBoxComponent != null && self.UIOpenBoxComponent.BoxUnitId > 0)
            {
                self.UIOpenBoxComponent.OnOpenBox(null);
            }
        }

        public static void OnBeforeSkill(this UIMainComponent self)
        {
            self.UIJoystickMoveComponent.lastSendTime = 0;
        }

        public static void OnStopAction(this UIMainComponent self)
        {
            self.UIJoystickMoveComponent.ResetUI();
            self.OnMoveStart();
        }

        public static void UpdateShadow(this UIMainComponent self, string usevalue = "")
        {
            GameObject gameObject = GameObject.Find("Directional Light");
            if (gameObject == null)
            {
                return;
            }
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            string value = usevalue != "" ? usevalue :  userInfoComponent.GetGameSettingValue(GameSettingEnum.Shadow);
            Light light = gameObject.GetComponent<Light>();
            light.shadows = value == "0" ? LightShadows.None : LightShadows.Soft;
        }

        //停止挂机
        public static void OnStopGuaJi(this UIMainComponent self)
        {
            if (self.ZoneScene().GetComponent<UnitGuaJiComponen>() != null)
            {
                //移除挂机组件
                self.ZoneScene().RemoveComponent<UnitGuaJiComponen>();
                FloatTipManager.Instance.ShowFloatTip("取消挂机!");
            }
            self.UGuaJiSet.SetActive(false);
        }
    }
}
