using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{

    [Timer(TimerType.UIUnitReviveTime)]
    public class UIUnitReviveTime : ATimer<UIUnitHpComponent>
    {
        public override void Run(UIUnitHpComponent self)
        {
            try
            {
                self.OnTimer();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }
    
    [Timer(TimerType.DialogTimer)]
    public class DialogTimer : ATimer<UIUnitHpComponent>
    {
        public override void Run(UIUnitHpComponent self)
        {
            try
            {
                self.HideDialog();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    public class UIUnitHpComponentAwakeSystem : AwakeSystem<UIUnitHpComponent>
    {
        public override void Awake(UIUnitHpComponent self)
        {
            self.Awake();
        }
    }

    public class UIUnitHpComponentDestroySystem : DestroySystem<UIUnitHpComponent>
    {
        public override void Destroy(UIUnitHpComponent self)
        {
            TimerComponent.Instance?.Remove(ref self.Timer);
            self.RecoverGameObject(self.GameObject);
            self.UIXuLieZhenComponent = null;
            if (self.GetParent<Unit>().Type == UnitType.Player)
            {
                DataUpdateComponent.Instance.RemoveListener(DataType.OnRecvChat, self);
            }
        }
    }

    /// <summary>
    /// 头部血条组件，负责血条的密度以及血条与人物的同步
    /// </summary>
    public class UIUnitHpComponent: Entity, IAwake, IDestroy
    {
        public GameObject DialogText;
        public GameObject Lal_Name;
        public GameObject Img_HpValue;
        public GameObject GameObject;
        public GameObject UIPlayerHpText;
        public GameObject BuffShieldValue;
        public GameObject Img_ChengHao;
        public Image Img_AngleValue;
        public GameObject Img_AngleValueDi;
        public GameObject Img_MpValueDi;
        public Image Img_MpValue;

        public Transform UIPosition;
        public string HeadBarPath;
        public Vector2 LastPositon;
        public GameObject Lal_AddtionName;
        public GameObject PlayerNameSet;
        public GameObject Lal_JiaZuName;
        public UIXuLieZhenComponent UIXuLieZhenComponent;
        public float LastTime;
        public long Timer;
        public long DialogTimer;

        public void  Awake( )
        {
            this.GameObject = null;
            this.Img_HpValue = null;
            this.Img_AngleValue = null;
            this.Img_AngleValueDi = null;
            this.Img_MpValueDi = null;
            this.Img_MpValue = null;
            this.HeadBarPath = "";
            this.LastTime = 0f;
            switch (this.GetParent<Unit>().Type)
            {
                case UnitType.Player:
                    this.HeadBarPath = ABPathHelper.GetUGUIPath("Blood/UIUnitHp");
                    DataUpdateComponent.Instance.AddListener(DataType.OnRecvChat, this);
                    break;
                case UnitType.Monster:
                    this.HeadBarPath = ABPathHelper.GetUGUIPath("Blood/UIMonsterHp");
                    break;
                case UnitType.Pet:
                case UnitType.JingLing:
                    this.HeadBarPath = ABPathHelper.GetUGUIPath("Blood/UIUnitHp");
                    break;
                case UnitType.Stall:
                    this.HeadBarPath = ABPathHelper.GetUGUIPath("Blood/UIStallIem");
                    break;
            }
            GameObjectPoolComponent.Instance.AddLoadQueue(HeadBarPath, this.InstanceId, this.OnLoadGameObject);
        }

        public void ShowHearBar(bool show)
        {
            this.GameObject.SetActive(show);
        }

        public void ExitStealth()
        {
            if (this.GameObject == null)
            {
                return;
            }

            Image[] hpImages = this.GameObject.GetComponentsInChildren<Image>();
            foreach (Image image in hpImages)
            {
                Color oldColor = image.color;
                image.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
            }

            //TextMeshProUGUI[] hpTextMeshPros = this.GameObject.GetComponentsInChildren<TextMeshProUGUI>();
            //foreach (TextMeshProUGUI textMeshPro in hpTextMeshPros)
            //{
            //    Color oldColor = textMeshPro.color;
            //    textMeshPro.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
            //}

            // 名称恢复
            Image[] nameImages = this.UIPlayerHpText.GetComponentsInChildren<Image>();
            foreach (Image image in nameImages)
            {
                Color oldColor = image.color;
                image.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
            }

            //TextMeshProUGUI[] nameTextMeshPros = this.UIPlayerHpText.GetComponentsInChildren<TextMeshProUGUI>();
            //foreach (TextMeshProUGUI textMeshPro in nameTextMeshPros)
            //{
            //    Color oldColor = textMeshPro.color;
            //    textMeshPro.color = new Color(oldColor.r, oldColor.g, oldColor.b, 1f);
            //}
        }


        public void EnterHide()
        {
            if (this.GameObject == null)
            {
                return;
            }
            this.GameObject.SetActive(false);
            this.UIPlayerHpText.SetActive(false);
        }

        public void ExitHide()
        {
            if (this.GameObject == null)
            {
                return;
            }
            this.GameObject.SetActive(true);
            this.UIPlayerHpText.SetActive(true);
        }

        public void EnterStealth(float alpha)
        {
            if (this.GameObject == null)
            {
                return;
            }

            // 血条隐形
            Image[] hpImages = this.GameObject.GetComponentsInChildren<Image>();
            foreach (Image image in hpImages)
            {
                Color oldColor = image.color;
                image.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
            }

            //TextMeshProUGUI[] hpTextMeshPros = this.GameObject.GetComponentsInChildren<TextMeshProUGUI>();
            //foreach (TextMeshProUGUI textMeshPro in hpTextMeshPros)
            //{
            //    Color oldColor = textMeshPro.color;
            //    textMeshPro.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
            //}

            // 名称隐形
            Image[] nameImages = this.UIPlayerHpText.GetComponentsInChildren<Image>();
            foreach (Image image in nameImages)
            {
                Color oldColor = image.color;
                image.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
            }

            //TextMeshProUGUI[] nameTextMeshPros = this.UIPlayerHpText.GetComponentsInChildren<TextMeshProUGUI>();
            //foreach (TextMeshProUGUI textMeshPro in nameTextMeshPros)
            //{
            //    Color oldColor = textMeshPro.color;
            //    textMeshPro.color = new Color(oldColor.r, oldColor.g, oldColor.b, alpha);
            //}
        }

        public void OnLoadGameObject(GameObject gameObject, long formId)
        {
            if (this.IsDisposed)
            {
                this.RecoverGameObject(gameObject);
                return;
            }
            this.GameObject = gameObject;
            Unit unit = this.GetParent<Unit>();
            ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();

            Unit mainUnit = UnitHelper.GetMyUnitFromZoneScene(this.ZoneScene());
            bool canAttack = mainUnit.IsCanAttackUnit(unit);
            this.Img_HpValue = rc.Get<GameObject>("Img_HpValue");
            switch (unit.Type)
            {
                case UnitType.Monster:
                    string imageHp = canAttack ? StringBuilderHelper.UI_pro_4_2 : StringBuilderHelper.UI_pro_3_2;
                    Sprite sp = rc.Get<GameObject>(imageHp).GetComponent<Image>().sprite;
                    rc.Get<GameObject>("Img_HpValue").SetActive(true);
                    this.Img_HpValue.GetComponent<Image>().sprite = sp;
                    rc.Get<GameObject>("Alive").SetActive(true);
                    rc.Get<GameObject>("Dead").SetActive(false);
                    rc.Get<GameObject>("ReviveTime").SetActive(false);
                    break;
                case UnitType.Player:
                    imageHp = canAttack ? StringBuilderHelper.UI_pro_4_2: StringBuilderHelper.UI_pro_3_2;
                    sp = rc.Get<GameObject>(imageHp).GetComponent<Image>().sprite;
                    this.DialogText = rc.Get<GameObject>("DialogText");
                    this.DialogText.SetActive(false);
                    this.PlayerNameSet = rc.Get<GameObject>("PlayerNameSet");
                    this.Img_HpValue.GetComponent<Image>().sprite = sp;
                    this.BuffShieldValue = rc.Get<GameObject>("BuffShieldValue");
                    this.Img_ChengHao = rc.Get<GameObject>("Img_ChengHao");
                    this.Img_ChengHao.SetActive(true);
                    this.PlayerNameSet.SetActive(true);
                    this.Lal_AddtionName = rc.Get<GameObject>("Lal_AddtionName");
                    this.Img_AngleValue = rc.Get<GameObject>("Img_AngleValue").GetComponent<Image>();
                    this.Img_AngleValue.gameObject.SetActive(false);
                    this.Img_AngleValueDi = rc.Get<GameObject>("Img_AngleValueDi");
                    this.Img_AngleValueDi.SetActive(false);
                    this.Img_MpValueDi = rc.Get<GameObject>("Img_MpValueDi");
                    this.Img_MpValueDi.SetActive(false);
                    this.Img_MpValue = rc.Get<GameObject>("Img_MpValue").GetComponent<Image>();
                    this.Img_MpValue.gameObject.SetActive(false);
                    this.UIXuLieZhenComponent = this.AddChild<UIXuLieZhenComponent, GameObject>(this.Img_ChengHao);
                    bool lierenxuetiao = unit.MainHero && unit.ConfigId == 3;
                    this.Img_HpValue.GetComponent<RectTransform>().sizeDelta = lierenxuetiao ? new Vector2(160f, 14f) : new Vector2(160f, 18f);
                    this.Img_HpValue.transform.localPosition = lierenxuetiao ? new Vector3(-82.5f, 1.9f, 0f) : new Vector3(-82.5f, 0.1f, 0f);

                    break;
                case UnitType.Pet:
                case UnitType.JingLing:
                    imageHp = canAttack ? StringBuilderHelper.UI_pro_4_2: StringBuilderHelper.UI_pro_3_4;
                    sp = rc.Get<GameObject>(imageHp).GetComponent<Image>().sprite;
                    this.PlayerNameSet = rc.Get<GameObject>("PlayerNameSet");
                    this.Img_HpValue.GetComponent<Image>().sprite = sp;
                    this.Img_ChengHao = rc.Get<GameObject>("Img_ChengHao");
                    this.Img_ChengHao.SetActive(false);
                    this.PlayerNameSet.SetActive(unit.Type == UnitType.Pet);
                    this.Img_AngleValue = rc.Get<GameObject>("Img_AngleValue").GetComponent<Image>();
                    this.Img_AngleValue.gameObject.SetActive(false);
                    this.Img_AngleValueDi = rc.Get<GameObject>("Img_AngleValueDi");
                    this.Img_AngleValueDi.SetActive(false);

                    this.Img_MpValueDi = rc.Get<GameObject>("Img_MpValueDi");
                    this.Img_MpValueDi.SetActive(false);
                    this.Img_MpValue = rc.Get<GameObject>("Img_MpValue").GetComponent<Image>();
                    this.Img_MpValue.gameObject.SetActive(false);
                    this.Img_HpValue.GetComponent<RectTransform>().sizeDelta =new Vector2(160f, 18f);
                    this.Img_HpValue.transform.localPosition =  new Vector3(-82.5f, 0.1f, 0f);
                    break;
                default:
                    break;
            }
            
            this.Lal_Name = rc.Get<GameObject>("Lal_Name");
            this.Lal_JiaZuName = rc.Get<GameObject>("Lal_JiaZuName");
            this.UIPosition = unit.GetComponent<HeroTransformComponent>().GetTranform(PosType.Head);
            GameObject bloodparent = unit.Type == UnitType.Monster ? UIEventComponent.Instance.BloodMonster :  UIEventComponent.Instance.BloodPlayer ;
            this.GameObject.transform.SetParent(bloodparent.transform);
            this.GameObject.transform.localScale = Vector3.one;

            this.UIPlayerHpText = rc.Get<GameObject>("UIPlayerHpText");
            this.UIPlayerHpText.transform.SetParent(UIEventComponent.Instance.BloodText.transform);
            this.UIPlayerHpText.transform.localScale = Vector3.one;
            HeadBarUI HeadBarUI_1 = this.UIPlayerHpText.GetComponent<HeadBarUI>();
            HeadBarUI_1.enabled = true;    /// !unit.MainHero;
            HeadBarUI_1.HeadPos = UIPosition;
            HeadBarUI_1.HeadBar = this.UIPlayerHpText;
            HeadBarUI_1.UiCamera = UIComponent.Instance.UICamera;
            HeadBarUI_1.MainCamera = UIComponent.Instance.MainCamera;
            HeadBarUI_1.UpdatePostion();

            HeadBarUI HeadBarUI_2 = this.GameObject.GetComponent<HeadBarUI>();
            HeadBarUI_2.enabled = true;    /// !unit.MainHero;
            HeadBarUI_2.HeadPos = UIPosition;
            HeadBarUI_2.HeadBar = GameObject;
            HeadBarUI_2.UiCamera = UIComponent.Instance.UICamera;
            HeadBarUI_2.MainCamera = UIComponent.Instance.MainCamera;
            HeadBarUI_2.UpdatePostion();

            if (unit.MainHero)
            {
                OnUpdateHorse();
                this.GameObject.transform.SetAsLastSibling();
            }
            else
            {
                this.GameObject.transform.SetAsFirstSibling();
            }
            this.GameObject.SetActive(SettingHelper.ShowBlood);

            //初始化当前血条
            this.UpdateBlood();
            //更新显示
            this.UpdateShow();

            this.ShowJueXingAnger();

            this.UpdateSkillUseMP();

            StateComponent stateComponent = unit.GetComponent<StateComponent>();
            if (stateComponent.StateTypeGet(StateTypeEnum.Stealth))
            {
                this.EnterStealth(canAttack ? 0f : 0.3f);
            }
            if (stateComponent.StateTypeGet(StateTypeEnum.Hide)
                     || unit.GetComponent<NumericComponent>().GetAsLong(NumericType.Now_Stall) > 0)
            {
                this.EnterHide();
            }
        }

        public  void EnableHeadBarUI(bool enable)
        {
            //HeadBarUI HeadBarUI_1 = this.UIPlayerHpText.GetComponent<HeadBarUI>();
            //HeadBarUI_1.enabled = enable;
            //HeadBarUI HeadBarUI_2 = this.GameObject.GetComponent<HeadBarUI>();
            //HeadBarUI_2.enabled = enable;
        }

        public void OnUpdateHorse(  )
        {
            //if (this.GameObject == null)
            //{
            //    return;
            //}

            //Unit unit = this.GetParent<Unit>();
            //NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            //int horseRide = numericComponent.GetAsInt(NumericType.HorseRide);

            //Vector3 vector3_zuoqi = new Vector3(0f, 180f, 0f);
            //Vector3 vector3_normal = new Vector3(0f, 120f, 0f);
            //if (horseRide > 0)
            //{
            //    ZuoQiShowConfig zuoQiShowConfig = ZuoQiShowConfigCategory.Instance.Get(horseRide);
            //    vector3_zuoqi.y +=(float) zuoQiShowConfig.NameShowUp;
            //}
       
            //this.GameObject.transform.localPosition = horseRide > 0 ? vector3_zuoqi : vector3_normal;
            //if (unit.MainHero)
            //{
            //    this.UIPlayerHpText.transform.localPosition = horseRide > 0 ? vector3_zuoqi : vector3_normal;
            //}
        }

        public void OnUpdateUnionName()
        {
            if (this.Lal_JiaZuName == null)
            {
                return;
            }
            Unit unit = this.GetParent<Unit>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            UnitInfoComponent infoComponent = unit.GetComponent<UnitInfoComponent>();
            int firstUnionName = numericComponent.GetAsInt(NumericType.FirstUnionName);

            string unionname = string.Empty;
            //判断自身是否有家族进行显示
            if (firstUnionName == 1 && infoComponent.UnionName.Length > 0)
            {
                string text1 = numericComponent.GetAsInt(NumericType.UnionLeader) == 1 ? StringBuilderHelper.族长 : StringBuilderHelper.成员;
                unionname = infoComponent.UnionName + text1;

                if (numericComponent.GetAsInt(NumericType.UnionRaceWin) == 1 && !string.IsNullOrEmpty(unionname))
                {
                    unionname += "(争霸)";
                }
            }
            if (string.IsNullOrEmpty(unionname) )
            {
                int rankId = numericComponent.GetAsInt(NumericType.CombatRankID);
                int occRankId = numericComponent.GetAsInt(NumericType.OccCombatRankID);
                int occ = unit.ConfigId;
                unionname = ConfigHelper.GetRankChengHao(rankId, occRankId, occ);
            }
            if (string.IsNullOrEmpty(unionname) )
            {
                unionname = infoComponent.UnionName;
            }

            this.Lal_JiaZuName.GetComponent<Text>().text = unionname;
            Vector3 vector3_pos = unionname.Length > 0 ? new Vector3(0f, 100f, 0f) : new Vector3(0f, 75f, 0f);
            this.Img_ChengHao.transform.localPosition = vector3_pos;
        }

        //更新显示
        public void UpdateShow()
        {
            Unit unit = this.GetParent<Unit>();
            UnitInfoComponent infoComponent = unit.GetComponent<UnitInfoComponent>();
            //显示玩家名称
            if (unit.Type == UnitType.Player)
            {
                NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

                int tilteid = numericComponent.GetAsInt(NumericType.TitleID);
                if (!SettingHelper.ShowTitle ||  UnitHelper.GetUnitList(unit.DomainScene(), UnitType.Player).Count > SettingHelper.NoShowTitle)
                {
                    tilteid = unit.MainHero ? tilteid : 0;
                }
                this.UIXuLieZhenComponent.OnUpdateTitle(tilteid).Coroutine();
                this.Lal_Name.GetComponent<Text>().text = infoComponent.UnitName;
                this.UpdateDemonName(infoComponent.DemonName);
                this.OnUpdateUnionName();
            }
            //显示怪物名称
            if (unit.Type == UnitType.Monster)
            {
                MonsterConfig monsterCof = MonsterConfigCategory.Instance.Get(this.GetParent<Unit>().ConfigId);
                Text textMeshProUGUI = this.Lal_Name.GetComponent<Text>();
                bool isboos = monsterCof.MonsterType == (int)MonsterTypeEnum.Boss;
                textMeshProUGUI.fontSize = isboos ? 32 : 26;
                textMeshProUGUI.color = isboos ? new Color(255,95,255): Color.white;
                string colorstr = isboos ? "<color=#FF5FFF>" : "<color=#FFFFFF>";
                //NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
                //this.ObjName.GetComponent<TextMeshProUGUI>().text = $"{colorstr}{monsterCof.MonsterName}_{numericComponent.GetAsInt(NumericType.Now_AI)}</color>";
                MapComponent mapComponent = unit.ZoneScene().GetComponent<MapComponent>();    
                bool shenYuan = mapComponent.SceneTypeEnum == SceneTypeEnum.TeamDungeon && mapComponent.FubenDifficulty == TeamFubenType.ShenYuan;
                if (shenYuan)
                {
                    if (monsterCof.MonsterType == 3)
                    {
                        this.Lal_Name.GetComponent<Text>().text = $"深渊召唤:{colorstr}{monsterCof.MonsterName}</color>";
                    }
                    else
                    {
                        this.Lal_Name.GetComponent<Text>().text = $"{colorstr}{monsterCof.MonsterName}</color>";
                    }
                }
                else
                {
                    this.Lal_Name.GetComponent<Text>().text = $"{colorstr}{monsterCof.MonsterName}</color>";
                }

                //怪物等级显示
                ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();
                int monsterLv = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Lv);
                if (monsterLv > 0)
                {
                    rc.Get<GameObject>("Lal_Lv").GetComponent<Text>().text = monsterLv.ToString();
                }
                else
                {
                    rc.Get<GameObject>("Lal_Lv").GetComponent<Text>().text = monsterCof.Lv.ToString();
                }
            }
            if (this.GetParent<Unit>().Type == UnitType.Pet) 
            {
                UnitInfoComponent unitInfoComponent = this.GetParent<Unit>().GetComponent<UnitInfoComponent>();
                this.Lal_Name.GetComponent<Text>().text = unitInfoComponent.UnitName;
                this.Lal_JiaZuName.GetComponent<Text>().text = $"{unitInfoComponent.MasterName }的宠物";
            }
            if (this.GetParent<Unit>().Type == UnitType.JingLing)
            {
                UnitInfoComponent unitInfoComponent = this.GetParent<Unit>().GetComponent<UnitInfoComponent>();
                this.Lal_Name.GetComponent<Text>().text = unitInfoComponent.UnitName;
                this.Lal_JiaZuName.GetComponent<Text>().text = $"{unitInfoComponent.MasterName }的精灵";
            }
            if (this.GetParent<Unit>().Type == UnitType.Stall)
            {
                UnitInfoComponent unitInfoComponent = this.GetParent<Unit>().GetComponent<UnitInfoComponent>();
                this.Lal_Name.GetComponent<Text>().text = unitInfoComponent.UnitName;
            }
        }

        public void OnRevive()
        {
            if (this.GameObject == null)
            {
                return;
            }
            TimerComponent.Instance.Remove(ref this.Timer);
            if (this.GetParent<Unit>().Type == UnitType.Monster)
            {
                ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();
                rc.Get<GameObject>("Alive").SetActive(true);
                rc.Get<GameObject>("Dead").SetActive(false);
                rc.Get<GameObject>("ReviveTime").SetActive(false);
            } 
            UpdateBlood();
        }

        public void OnDead()
        {
            if (this.GetParent<Unit>().Type != UnitType.Monster)
            {
                return;
            }
            if (this.GameObject == null)
            {
                return;
            }
            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(this.GetParent<Unit>().ConfigId);
            if (monsterConfig.ReviveTime > 0)
            {
                ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();
                rc.Get<GameObject>("Alive").SetActive(false);
                rc.Get<GameObject>("Dead").SetActive(true);
                rc.Get<GameObject>("ReviveTime").SetActive(true);

                TimerComponent.Instance.Remove(ref this.Timer);
                this.OnTimer();
                this.Timer = TimerComponent.Instance.NewRepeatedTimer(1000, TimerType.UIUnitReviveTime, this);
            }
        }

        public void UpdateAI()
        {
            if (this.Lal_Name != null)
            {
                UpdateShow();
            }
        }

        public void UpdateBlood()
        {
            NumericComponent numericComponent = this.Parent.GetComponent<NumericComponent>();
            float curhp = numericComponent.GetAsLong(NumericType.Now_Hp); // + value;
            float blood = curhp / numericComponent.GetAsLong(NumericType.Now_MaxHp);
            blood = Mathf.Max(blood, 0f);
            if (Img_HpValue == null)
            {
                return;
            }
            int unitType = this.GetParent<Unit>().Type;
            switch (unitType)
            {
                case UnitType.Player:
                    this.Img_HpValue.GetComponent<Image>().fillAmount = blood;
                    int shieldHp = numericComponent.GetAsInt(NumericType.Now_Shield_HP);
                    int shieldMax = numericComponent.GetAsInt(NumericType.Now_Shield_MaxHP);
                    if (shieldMax > 0)
                    {
                        this.BuffShieldValue.GetComponent<Image>().fillAmount = shieldHp * 1f / shieldMax;
                    }
                    else
                    {
                        this.BuffShieldValue.GetComponent<Image>().fillAmount = 0f;
                    }
                    break;
                case UnitType.Pet:
                    this.Img_HpValue.GetComponent<Image>().fillAmount = blood;
                    break;
                default:
                    this.Img_HpValue.GetComponent<Image>().fillAmount = blood;
                    break;
            }
        }

        public void OnTimer()
        {
            MonsterConfig monsterConfig = MonsterConfigCategory.Instance.Get(this.GetParent<Unit>().ConfigId);
            long leftTime = this.Parent.GetComponent<NumericComponent>().GetAsLong(NumericType.ReviveTime) - TimeHelper.ClientNow();
            leftTime = leftTime / 1000;
            ReferenceCollector rc = this.GameObject.GetComponent<ReferenceCollector>();
            GameObject reviveTime = rc.Get<GameObject>("ReviveTime");
            int hour = (int) leftTime / 3600;
            int min = (int)((leftTime - (hour * 3600))/60);
            int sec = (int)(leftTime - (hour * 3600) - (min * 60));
            string showStr = hour + "时" + min + "分" + sec + "秒";
            reviveTime.GetComponent<Text>().text = $"{monsterConfig.MonsterName} 刷新剩余时间:{showStr}";
        }

        public void OnGetUseInfoUpdate()
        {
            this.ShowJueXingAnger();

            this.UpdateSkillUseMP();    
        }

        public  void ShowJueXingAnger()
        {
            if (this.Img_AngleValue == null)
            {
                return;
            }

            Unit unit = this.GetParent<Unit>();
            if (!unit.MainHero)
            {
                this.Img_AngleValue.gameObject.SetActive(false);
                this.Img_AngleValueDi.SetActive(false);
                return;
            }
            UserInfoComponent userInfoComponent = this.ZoneScene().GetComponent<UserInfoComponent>();
            if (userInfoComponent == null || userInfoComponent.UserInfo == null)
            {
                return;
            }

            int occTwo = userInfoComponent.UserInfo.OccTwo;
            if (occTwo == 0)
            {
                return;
            }
            SkillSetComponent skillSetComponent = this.ZoneScene().GetComponent<SkillSetComponent>();
            OccupationTwoConfig occupationConfigCategory = OccupationTwoConfigCategory.Instance.Get(occTwo);
            this.Img_AngleValue.gameObject.SetActive(skillSetComponent.GetSkillPro(occupationConfigCategory.JueXingSkill[7]) != null);
            this.Img_AngleValueDi.SetActive(skillSetComponent.GetSkillPro(occupationConfigCategory.JueXingSkill[7]) != null);
        }


        public void RecoverGameObject(GameObject gameobject)
        {
            if (gameobject != null)
            {
                gameobject.GetComponent<HeadBarUI>().enabled = false;
                if (this.UIPlayerHpText != null)
                {
                    this.UIPlayerHpText.GetComponent<HeadBarUI>().enabled = false;
                    this.UIPlayerHpText.transform.SetParent(gameobject.transform);
                }
               
                GameObjectPoolComponent.Instance.RecoverGameObject(this.HeadBarPath, gameobject);
                this.GameObject = null;
            }
        }
    }

    public static class UIUnitHpComponentSystem
    {
        public static void ShowDialog(this UIUnitHpComponent self)
        {
            ChatInfo chatInfo = self.ZoneScene().GetComponent<ChatComponent>().LastChatInfo;
            if (chatInfo.ChannelId != ChannelEnum.Team)
            {
                return;
            }

            if (chatInfo.UserId != self.GetParent<Unit>().Id)
            {
                return;
            }

            if (!UnitTypeHelper.IsSameTeam(UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene()), self.GetParent<Unit>()))
            {
                return;
            }
            
            self.DialogText.SetActive(true);
            self.DialogText.GetComponent<Text>().text = chatInfo.ChatMsg;
            TimerComponent.Instance.Remove(ref self.DialogTimer);
            self.DialogTimer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 2000, TimerType.DialogTimer, self);
        }

        public static void HideDialog(this UIUnitHpComponent self)
        {
            self.DialogText.SetActive(false);
        }

        public static void UpdateDemonName(this UIUnitHpComponent self, string stallName)
        {
            if (string.IsNullOrEmpty(stallName))
            {
                self.Lal_AddtionName.GetComponent<Text>().text = string.Empty;
            }
            else
            {
                self.Lal_AddtionName.GetComponent<Text>().text = $"{stallName}的小跟班";
            }
        }

        public static void UpdateShield(this UIUnitHpComponent self)
        { 
            
        }

        public static void UpdateSkillUseMP(this UIUnitHpComponent self)
        {
            if (self.Img_MpValue == null)
            {
                return;
            }

            Unit unit = self.GetParent<Unit>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            int skillmp = numericComponent.GetAsInt(NumericType.SkillUseMP);
            int maxMp = numericComponent.GetAsInt(NumericType.Max_SkillUseMP);
            if (skillmp == 0)
            {
                self.Img_MpValueDi.gameObject.SetActive(false);
                self.Img_MpValue.gameObject.SetActive(false);
            }
            else
            {
                self.Img_MpValueDi.gameObject.SetActive(true);
                self.Img_MpValue.gameObject.SetActive(true);
                float value = skillmp * 1f / maxMp;
                self.Img_MpValue.fillAmount = Math.Min(value, 1f);
            }
        }

        public static void UptateJueXingAnger(this UIUnitHpComponent self)
        {
            if (self.Img_AngleValue == null)
            {
                return;
            }
            Unit unit = self.GetParent<Unit>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            float value = numericComponent.GetAsInt(NumericType.JueXingAnger) * 1f/ 500;
            self.Img_AngleValue.fillAmount = Math.Min( value, 1f );
        }

        /// <summary>
        /// 阵营改变，其他玩家的血条颜色做相应调整
        /// </summary>
        /// <param name="self"></param>
        public static void UpdateBattleCamp(Unit mainUnit,  long unitId)
        {
            if (mainUnit == null)
            {
                Log.Error("UpdateBattleCamp/mainUnit == null");

            }
            List<Unit> unitlist = UnitHelper.GetUnitList(mainUnit.DomainScene(), UnitType.Player);
            for (int i = 0; i < unitlist.Count; i++)
            {
                bool update = false;
                if (unitlist[i].Id == mainUnit.Id || unitlist[i].Id == unitId || unitId == mainUnit.Id)
                {
                    update = true;
                }

                if (!update)
                {
                    continue;
                }

                bool canAttack = mainUnit.IsCanAttackUnit(unitlist[i]);
                string imageHp = canAttack ? StringBuilderHelper.UI_pro_4_2 : StringBuilderHelper.UI_pro_3_2;
                UIUnitHpComponent uIUnitHpComponent = unitlist[i].GetComponent<UIUnitHpComponent>();
                if (uIUnitHpComponent!= null && uIUnitHpComponent.Img_HpValue!= null)
                {
                    ReferenceCollector rc = uIUnitHpComponent.GameObject.GetComponent<ReferenceCollector>();
                    Sprite sp = rc.Get<GameObject>(imageHp).GetComponent<Image>().sprite;
                    uIUnitHpComponent.Img_HpValue.GetComponent<Image>().sprite = sp;
                }
            }
        }
    }
}