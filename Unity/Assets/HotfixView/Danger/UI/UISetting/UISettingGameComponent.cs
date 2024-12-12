using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


namespace ET
{

    public class UISettingGameComponent : Entity, IAwake
    {
        public GameObject ZhuBoSet;
        public GameObject ReSetCameraBtn;
        public GameObject RotaAngleSet;
        public GameObject LenDepthSet;
        public GameObject SkillAttackPlayerFirst;
        public GameObject FirstUnionName;
        public GameObject HideNode;
        public GameObject HideLeftBottom;
        public GameObject NoMoving;
        public GameObject AutoAttack;
        public GameObject GameMemory;
        public GameObject ActTargetSelect;
        public GameObject ActTypeSet;
        public GameObject ButtonSkillSet;
        public GameObject OneSellSet;
        public GameObject PickSet;
        public GameObject RandomHorese;
        public GameObject HighFps;
        public GameObject Smooth;
        public GameObject ButtonPhone;
        public GameObject LastLoginTime;
        public GameObject TextVersion;
        public GameObject Image_YinYing;
        public GameObject Btn_YinYing;
        public GameObject Image_Click;
        public GameObject Btn_Click;
        public Toggle ScreenToggle0;
        public Toggle ScreenToggle1;
        public GameObject HideDi;
        public GameObject Image_fps;
        public GameObject Btn_Fps;
        public GameObject SliderSound;
        public GameObject SliderMusic;
        public GameObject Image_Fixed;
        public GameObject Image_Move;
        public GameObject Btn_Fixed;
        public GameObject Btn_Close;
        public GameObject TextZhangHaoID;
        public GameObject Btn_CloseGame;
        public GameObject Btn_ReturnDengLu;
        public GameObject Image_Sound;
        public GameObject Image_yinyu;
        public GameObject Btn_Sound;
        public GameObject Btn_YinYue;
        public GameObject Btn_ChengHao;
        public GameObject ButtonRname;
        public GameObject InputFieldCName;
        public GameObject NoShowOther;

        public UserInfoComponent UserInfoComponent;
        public List<KeyValuePair> GameSettingInfos = new List<KeyValuePair>();
    }


    public class UISettingGameComponentAwake : AwakeSystem<UISettingGameComponent>
    {
        public override void Awake(UISettingGameComponent self)
        {
            self.GameSettingInfos.Clear();

            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.Btn_Close = rc.Get<GameObject>("Btn_Close");
            self.TextVersion = rc.Get<GameObject>("TextVersion");
            self.Image_Click = rc.Get<GameObject>("Image_Click");
            self.Btn_Click = rc.Get<GameObject>("Btn_Click");
            self.LastLoginTime = rc.Get<GameObject>("LastLoginTime");
            self.Btn_Click.GetComponent<Button>().onClick.AddListener(self.OnBtn_Click);

            self.ButtonSkillSet = rc.Get<GameObject>("ButtonSkillSet");
            self.ButtonSkillSet.GetComponent<Button>().onClick.AddListener(self.OnButtonSkillSet);

            self.Image_YinYing = rc.Get<GameObject>("Image_YinYing");
            self.Btn_YinYing = rc.Get<GameObject>("Btn_YinYing");
            self.Btn_YinYing.GetComponent<Button>().onClick.AddListener(self.OnBtn_Shadow);

            self.RandomHorese = rc.Get<GameObject>("RandomHorese");
            self.RandomHorese.transform.Find("Btn_Click").GetComponent<Button>().onClick.AddListener(self.OnBtn_RandomHorese);

            self.LenDepthSet = rc.Get<GameObject>("LenDepthSet");
            self.LenDepthSet.transform.GetComponentInChildren<Slider>().onValueChanged.AddListener((value) => { self.OnLenDepth(value);});
            self.LenDepthSet.gameObject.SetActive(GMHelp.GmAccount.Contains(self.ZoneScene().GetComponent<AccountInfoComponent>().Account) || GlobalHelp.BigVersion >= 19);

            self.RotaAngleSet = rc.Get<GameObject>("RotaAngleSet");
            self.RotaAngleSet.transform.Find("Btn_Click").GetComponent<Button>().onClick.AddListener(self.OnBtn_RotaAngle);
            self.RotaAngleSet.gameObject.SetActive(GMHelp.GmAccount.Contains(self.ZoneScene().GetComponent<AccountInfoComponent>().Account) || GlobalHelp.BigVersion >= 19);

            self.ReSetCameraBtn = rc.Get<GameObject>("ReSetCameraBtn");
            self.ReSetCameraBtn.GetComponent<Button>().onClick.AddListener(self.OnReSetCameraBtn);
            self.ReSetCameraBtn.gameObject.SetActive(GMHelp.GmAccount.Contains(self.ZoneScene().GetComponent<AccountInfoComponent>().Account) || GlobalHelp.BigVersion >= 19);

            self.ZhuBoSet = rc.Get<GameObject>("ZhuBoSet");
            self.ZhuBoSet.transform.Find("Btn_Click").GetComponent<Button>().onClick.AddListener(self.OnBtn_ZhuBo);
            self.ZhuBoSet.gameObject.SetActive(GMHelp.GmAccount.Contains(self.ZoneScene().GetComponent<AccountInfoComponent>().Account) || GlobalHelp.BigVersion >= 19);
            
            self.FirstUnionName = rc.Get<GameObject>("FirstUnionName");
            self.FirstUnionName.transform.Find("Btn_Click").GetComponent<Button>().onClick.AddListener(self.OnBtn_FirstUnionName);

            self.SkillAttackPlayerFirst = rc.Get<GameObject>("SkillAttackPlayerFirst");
            self.SkillAttackPlayerFirst.transform.Find("Btn_Click").GetComponent<Button>().onClick.AddListener(self.OnSkillAttackPlayerFirst);

            self.HideLeftBottom = rc.Get<GameObject>("HideLeftBottom");
            self.HideLeftBottom.transform.Find("Btn_Click").GetComponent<Button>().onClick.AddListener(self.OnBtn_HideLeftBottom);
            
            self.AutoAttack = rc.Get<GameObject>("AutoAttack");
            self.AutoAttack.transform.Find("Btn_Click").GetComponent<Button>().onClick.AddListener(self.OnBtn_AutoAttack);

            self.NoMoving = rc.Get<GameObject>("NoMoving");
            self.NoMoving.GetComponent<Button>().onClick.AddListener(self.OnBtn_NoMoving);

            self.HideNode = rc.Get<GameObject>("HideNode");
            self.HideNode.SetActive( GlobalHelp.GetPlatform() != 5 && GlobalHelp.GetPlatform() != 6);

            self.OneSellSet = rc.Get<GameObject>("OneSellSet");
            self.OneSellSet.transform.Find("Btn_Click_0").GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_OneSellSet(0); });
            self.OneSellSet.transform.Find("Btn_Click_1").GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_OneSellSet(1); });
            self.OneSellSet.transform.Find("Btn_Click_2").GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_OneSellSet(2); });
            self.OneSellSet.transform.Find("Btn_Click_3").GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_OneSellSet(3); });

            self.PickSet = rc.Get<GameObject>("PickSet");
            self.PickSet.transform.Find("Btn_Click_0").GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_PickSet(0); });
            self.PickSet.transform.Find("Btn_Click_1").GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_PickSet(1); });

            self.ActTypeSet = rc.Get<GameObject>("ActTypeSet");
            self.ActTypeSet.transform.Find("Btn_Click_0").GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_AttackMode("0"); });
            self.ActTypeSet.transform.Find("Btn_Click_1").GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_AttackMode("1"); });
            self.ActTypeSet.transform.Find("Btn_Click_2").GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_AttackMode("2"); });
            self.ActTypeSet.transform.Find("Btn_Click_3").GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_AttackMode("3"); });

            self.ActTargetSelect = rc.Get<GameObject>("ActTargetSelect");
            self.ActTargetSelect.transform.Find("Btn_Click_0").GetComponent<Button>().onClick.AddListener(() => { self.OnActTargetSelect("0"); });
            self.ActTargetSelect.transform.Find("Btn_Click_1").GetComponent<Button>().onClick.AddListener(() => { self.OnActTargetSelect("1"); });

            self.HideDi = rc.Get<GameObject>("HideDi");
            self.SliderSound = rc.Get<GameObject>("SliderSound");
            self.SliderMusic = rc.Get<GameObject>("SliderMusic");
            self.SliderSound.GetComponent<Slider>().onValueChanged.AddListener((float value) => { self.OnSliderSoundChange(value); });
            self.SliderMusic.GetComponent<Slider>().onValueChanged.AddListener((float value) => { self.OnSliderMusicChange(value); });

            self.Image_fps = rc.Get<GameObject>("Image_fps");
            self.Btn_Fps = rc.Get<GameObject>("Btn_Fps");
            self.Btn_Fps.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_Fps(); });

            UI uimain = UIHelper.GetUI(self.DomainScene(), UIType.UIMain);
            GameObject fps = uimain.GetComponent<UIMainComponent>().Fps;
            self.Image_fps.SetActive(fps.activeSelf);

            self.TextZhangHaoID = rc.Get<GameObject>("TextZhangHaoID");
            self.Btn_CloseGame = rc.Get<GameObject>("Btn_CloseGame");
            self.Btn_ReturnDengLu = rc.Get<GameObject>("Btn_ReturnDengLu");
            self.Image_Sound = rc.Get<GameObject>("Image_Sound");
            self.Image_yinyu = rc.Get<GameObject>("Image_yinyu");
            self.Btn_Sound = rc.Get<GameObject>("Btn_Sound");
            self.Btn_YinYue = rc.Get<GameObject>("Btn_YinYue");
            self.ButtonRname = rc.Get<GameObject>("ButtonRname");

            self.InputFieldCName = rc.Get<GameObject>("InputFieldCName");
            self.InputFieldCName.GetComponent<InputField>().onValueChanged.AddListener((string text) => { self.CheckSensitiveWords(); });

            self.Btn_Close.GetComponent<Button>().onClick.AddListener(() => { self.OnBeforeClose(); });
            self.Btn_CloseGame.GetComponent<Button>().onClick.AddListener(() => { self.OnCloseGame(); });
            self.Btn_ReturnDengLu.GetComponent<Button>().onClick.AddListener(() => { self.OnReturnLogin(); });

            self.Btn_Sound.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_Sound(); });
            self.Btn_YinYue.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_YinYue(); });
            self.ButtonRname.GetComponent<Button>().onClick.AddListener(() => { self.OnButtonRname().Coroutine(); });

            self.ButtonPhone = rc.Get<GameObject>("ButtonPhone");
            ButtonHelp.AddListenerEx(self.ButtonPhone, self.OnButtonPhone);
            AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
            self.ButtonPhone.SetActive(true);
            if (GlobalHelp.GetPlatform() == 5 || GlobalHelp.GetPlatform() == 6)
            {
                self.ButtonPhone.SetActive(false);
            }

            self.HighFps = rc.Get<GameObject>("HighFps");
            ButtonHelp.AddListenerEx(self.HighFps.transform.Find("Btn_Click").gameObject, self.OnHighFps);

            self.Smooth = rc.Get<GameObject>("Smooth");
            ButtonHelp.AddListenerEx(self.Smooth.transform.Find("Btn_Click").gameObject, self.OnSmooth);

            self.NoShowOther = rc.Get<GameObject>("NoShowOther");
            ButtonHelp.AddListenerEx(self.NoShowOther.transform.Find("Btn_Click").gameObject, self.OnNoShowOther);

            self.GameMemory = rc.Get<GameObject>("GameMemory");
            ButtonHelp.AddListenerEx(self.GameMemory, self.OnGameMemory);

            self.Image_Fixed = rc.Get<GameObject>("Image_Fixed");
            self.Btn_Fixed = rc.Get<GameObject>("Btn_Fixed");
            self.Btn_Fixed.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_Fixed(); });
          
            self.ScreenToggle0 = rc.Get<GameObject>("ScreenToggle0").GetComponent<Toggle>();
            self.ScreenToggle1 = rc.Get<GameObject>("ScreenToggle1").GetComponent<Toggle>();
            self.ScreenToggle0.onValueChanged.AddListener((bool ison) => { self.OnScreenToggle0_Ex(ison); });
            self.ScreenToggle1.onValueChanged.AddListener((bool ison) => { self.OnScreenToggle1_Ex(ison); });

            self.UserInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            self.InitUI();
        }


    }

    public static class UISettingGameComponentSystem
    {
        public static void OnScreenToggle0_Ex(this UISettingGameComponent self, bool value)
        {
            if (value)
            {
                self.SaveSettings(GameSettingEnum.FenBianlLv, "1");
                UIHelper.GetUI(self.ZoneScene(), UIType.UIMain).GetComponent<UIMainComponent>().SetFenBianLv1();
            }
        }

        public static void OnScreenToggle1_Ex(this UISettingGameComponent self, bool value)
        {
            if (value)
            {
                self.SaveSettings(GameSettingEnum.FenBianlLv, "2");
                UIHelper.GetUI(self.ZoneScene(), UIType.UIMain).GetComponent<UIMainComponent>().SetFenBianLv2();
            }
        }

        public static void OnSliderSoundChange(this UISettingGameComponent self, float value)
        {
            Game.Scene.GetComponent<SoundComponent>().ChangeSoundVolume(value);
        }

        public static void OnSliderMusicChange(this UISettingGameComponent self, float value)
        {
            Game.Scene.GetComponent<SoundComponent>().ChangeMusicVolume(value);
        }

        public static void OnButtonSkillSet(this UISettingGameComponent self)
        {
            UI uI = UIHelper.GetUI(self.ZoneScene(), UIType.UIMain );
            if (!uI.GetComponent<UIMainComponent>().UIMainSkill.activeSelf)
            {
                FloatTipManager.Instance.ShowFloatTip("请移动至有技能框的区域,比如探险地区进行更改");
                return;
            }

            uI.GetComponent<UIMainComponent>().UIMainButtonPositionComponent.ShowSkillPositionSet();
            UIHelper.Remove( self.ZoneScene(), UIType.UISetting );
        }

        public static void OnBtn_Click(this UISettingGameComponent self)
        {
            string value = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.Click);
            self.Image_Click.SetActive(value == "0");
            self.SaveSettings(GameSettingEnum.Click, value == "0" ? "1" : "0");
            self.ZoneScene().CurrentScene().GetComponent<OperaComponent>().UpdateClickMode();
        }

        public static void OnBtn_Shadow(this UISettingGameComponent self)
        {
            string value = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.Shadow);
            self.Image_YinYing.SetActive(value == "0");
            self.SaveSettings(GameSettingEnum.Shadow, value == "0" ? "1" : "0");
            UI uimain = UIHelper.GetUI(self.ZoneScene(), UIType.UIMain);
            uimain.GetComponent<UIMainComponent>().UpdateShadow(value == "0" ? "1" : "0");
        }

        public static void UpdateShadow(this UISettingGameComponent self)
        {
            UI uimain = UIHelper.GetUI(self.ZoneScene(), UIType.UIMain);
            uimain.GetComponent<UIMainComponent>().UpdateShadow();
        }

        public static void OnBtn_AttackMode(this UISettingGameComponent self, string attackmode)
        {
            self.SaveSettings(GameSettingEnum.AttackMode, attackmode);
            self.UpdateAttackMode();
        }

        public static void OnActTargetSelect(this UISettingGameComponent self, string attackmode)
        {
            self.SaveSettings(GameSettingEnum.AttackTarget, attackmode);
            self.ZoneScene().GetComponent<LockTargetComponent>().AttackTarget = int.Parse(attackmode);
            self.UpdateAttackTarget();
        }

        public static void OnBtn_OneSellSet(this UISettingGameComponent self, int index)
        {
            string value = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.OneSellSet);
            string[] setvalues = value.Split('@');
            if (setvalues.Length < 4)
            {
                value = "1@0@0@0";
            }
            setvalues = value.Split('@');
            setvalues[index] = setvalues[index] == "1" ? "0" : "1";

            value = $"{setvalues[0]}@{setvalues[1]}@{setvalues[2]}@{setvalues[3]}";

            self.OneSellSet.transform.Find($"Image_Click_{index}").gameObject.SetActive(setvalues[index] == "1");
            self.SaveSettings(GameSettingEnum.OneSellSet, value);
        }

        public static void OnBtn_PickSet(this UISettingGameComponent self, int index)
        {
            string value = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.PickSet);
            string[] setvalues = value.Split('@');
            setvalues[index] = setvalues[index] == "1"? "0" : "1";
            value = $"{setvalues[0]}@{setvalues[1]}";

            self.PickSet.transform.Find($"Image_Click_{index}").gameObject.SetActive(setvalues[index] == "1");
            self.SaveSettings(GameSettingEnum.PickSet, value);
            self.UserInfoComponent.PickSet = value.Split('@');
        }

        public static void OnBtn_RotaAngle(this UISettingGameComponent self)
        {
            int value = PlayerPrefsHelp.GetInt(PlayerPrefsHelp.RotaAngle);
            self.RotaAngleSet.transform.Find("Image_Click").gameObject.SetActive(value == 0);
            PlayerPrefsHelp.SetInt(PlayerPrefsHelp.RotaAngle, value == 0? 1 : 0);
            UIHelper.GetUI(self.ZoneScene(), UIType.UIMain).GetComponent<UIMainComponent>().DragPanel.SetActive(value == 0);
        }

        public static void OnBtn_ZhuBo(this UISettingGameComponent self)
        {
            int value = PlayerPrefsHelp.GetInt(PlayerPrefsHelp.ZhuBo);
            self.ZhuBoSet.transform.Find("Image_Click").gameObject.SetActive(value == 0);
            PlayerPrefsHelp.SetInt(PlayerPrefsHelp.ZhuBo, value == 0? 1 : 0);
        }
        
        public static void OnBtn_RandomHorese(this UISettingGameComponent self)
        {
            string value = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.RandomHorese);
            self.RandomHorese.transform.Find("Image_Click").gameObject.SetActive(value == "0");
            self.SaveSettings(GameSettingEnum.RandomHorese, value == "0" ? "1" : "0");
        }

        public static void OnLenDepth(this UISettingGameComponent self, float value)
        {
            float va = 0.1f + value * 2;
            PlayerPrefsHelp.SetFloat(PlayerPrefsHelp.LenDepth, va);
            self.ZoneScene().CurrentScene().GetComponent<CameraComponent>().LenDepth = va;
        }

        public static void OnReSetCameraBtn(this UISettingGameComponent self)
        {
            PlayerPrefsHelp.SetFloat(PlayerPrefsHelp.LenDepth, 1f);
            Vector3 offsetposition = new Vector3(0, 10f, -6f);
            self.ZoneScene().CurrentScene().GetComponent<CameraComponent>().LenDepth = 1f;
            self.ZoneScene().CurrentScene().GetComponent<CameraComponent>().OffsetPostion = offsetposition;

            float va = PlayerPrefsHelp.GetFloat(PlayerPrefsHelp.LenDepth);
            if (va <= 0)
            {
                self.LenDepthSet.transform.GetComponentInChildren<Slider>().value = 0.4f;
            }
            else
            {
                self.LenDepthSet.transform.GetComponentInChildren<Slider>().value = (va - 0.1f) / 2;
            }

            PlayerPrefsHelp.SetInt(PlayerPrefsHelp.RotaAngle, 0);
            PlayerPrefsHelp.SetString(PlayerPrefsHelp.CameraParams, $"{offsetposition.x}_{offsetposition.y}_{offsetposition.z}");

            self.RotaAngleSet.transform.Find("Image_Click").gameObject.SetActive(false);
            UIHelper.GetUI(self.ZoneScene(), UIType.UIMain).GetComponent<UIMainComponent>().DragPanel.SetActive(false);
        }

        public static void OnBtn_FirstUnionName(this UISettingGameComponent self)
        {
            string value = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.FirstUnionName);
            self.FirstUnionName.transform.Find("Image_Click").gameObject.SetActive(value == "0");
            self.SaveSettings(GameSettingEnum.FirstUnionName, value == "0" ? "1" : "0");
        }

        public static void OnSkillAttackPlayerFirst(this UISettingGameComponent self)
        {
            string value = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.SkillAttackPlayerFirst);
            self.SkillAttackPlayerFirst.transform.Find("Image_Click").gameObject.SetActive(value == "0");
            self.SaveSettings(GameSettingEnum.SkillAttackPlayerFirst, value == "0" ? "1" : "0");
            self.ZoneScene().GetComponent<LockTargetComponent>().SkillAttackPlayerFirst = int.Parse(value == "0"? "1" : "0");
        }
        
        public static void OnBtn_NoMoving(this UISettingGameComponent self)
        {
            SettingHelper.ShowNoMoving = !SettingHelper.ShowNoMoving;
        }

        public static void OnBtn_HideLeftBottom(this UISettingGameComponent self)
        {
            string value = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.HideLeftBottom);
            self.HideLeftBottom.transform.Find("Image_Click").gameObject.SetActive(value == "0");
            self.SaveSettings(GameSettingEnum.HideLeftBottom, value == "0" ? "1" : "0");
        }
        
        public static void OnBtn_AutoAttack(this UISettingGameComponent self)
        {
            string value = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.AutoAttack);
            self.AutoAttack.transform.Find("Image_Click").gameObject.SetActive(value == "0");
            self.SaveSettings(GameSettingEnum.AutoAttack, value == "0" ? "1" : "0");

            AttackComponent attackComponent = self.ZoneScene().GetComponent<AttackComponent>();
            attackComponent.AutoAttack = value == "0";
        }

        public static void InitUI(this UISettingGameComponent self)
        {
            self.Image_yinyu.SetActive(self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.Music) == "1");
            self.Image_Sound.SetActive(self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.Sound) == "1");
            self.Image_YinYing.SetActive(self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.Shadow) == "1");

       
            string music = PlayerPrefsHelp.GetString(PlayerPrefsHelp.MusicVolume);
            float musicvalue = string.IsNullOrEmpty(music) ? 1f : float.Parse(music);
            self.SliderSound.GetComponent<Slider>().value = musicvalue;

            string sound = PlayerPrefsHelp.GetString(PlayerPrefsHelp.SoundVolume);
            float soundvalue = string.IsNullOrEmpty(sound) ? 1f : float.Parse(sound);
            self.SliderMusic.GetComponent<Slider>().value = soundvalue;

            self.ScreenToggle0.isOn = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.FenBianlLv) == "1";
            self.ScreenToggle1.isOn = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.FenBianlLv) == "2";
            self.Image_Click.SetActive(self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.Click) == "1");

            self.RandomHorese.transform.Find("Image_Click").gameObject.SetActive(self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.RandomHorese) == "1");
            self.FirstUnionName.transform.Find("Image_Click").gameObject.SetActive(self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.FirstUnionName) == "1");
            self.SkillAttackPlayerFirst.transform.Find("Image_Click").gameObject.SetActive(self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.SkillAttackPlayerFirst) == "1");
            self.AutoAttack.transform.Find("Image_Click").gameObject.SetActive(self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.AutoAttack) == "1");
            self.HideLeftBottom.transform.Find("Image_Click").gameObject.SetActive(self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.HideLeftBottom) == "1");
            self.NoShowOther.transform.Find("Image_Click").gameObject.SetActive(self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.NoShowOther) == "1");
            self.RotaAngleSet.transform.Find("Image_Click").gameObject.SetActive(PlayerPrefsHelp.GetInt(PlayerPrefsHelp.RotaAngle) == 1);
            float va = PlayerPrefsHelp.GetFloat(PlayerPrefsHelp.LenDepth);
            if (va <= 0)
            {
                self.LenDepthSet.transform.GetComponentInChildren<Slider>().value = 0.4f;
            }
            else
            {
                self.LenDepthSet.transform.GetComponentInChildren<Slider>().value = (va - 0.1f) / 2;
            }
            self.ZhuBoSet.transform.Find("Image_Click").gameObject.SetActive(PlayerPrefsHelp.GetInt(PlayerPrefsHelp.ZhuBo) == 1);


            string value = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.OneSellSet);
            string[] setvalues = value.Split('@');
           
            self.OneSellSet.transform.Find("Image_Click_0").gameObject.SetActive(setvalues[0] == "1");
            self.OneSellSet.transform.Find("Image_Click_1").gameObject.SetActive(setvalues[1] == "1");
            self.OneSellSet.transform.Find("Image_Click_2").gameObject.SetActive(setvalues[2] == "1");

            if (setvalues.Length > 3)
            {
                self.OneSellSet.transform.Find("Image_Click_3").gameObject.SetActive(setvalues[3] == "1");
            }
            
            string value2 = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.PickSet);
            string[] setvalues2 = value2.Split('@');
            self.PickSet.transform.Find("Image_Click_0").gameObject.SetActive(setvalues2[0] == "1");
            self.PickSet.transform.Find("Image_Click_1").gameObject.SetActive(setvalues2[1] == "1");
            
            self.UpdateYaoGan();
            self.UpdateShadow();
            self.UpdateHighFps();
            self.UpdateSmooth();
            self.UpdateNoShowOther();
            self.UpdateAttackMode();
            self.UpdateAttackTarget();
            self.TextVersion.GetComponent<Text>().text = GlobalHelp.GetBigVersion().ToString();
            self.InputFieldCName.GetComponent<InputField>().text = self.UserInfoComponent.UserInfo.Name;
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            long lastTime = unit.GetComponent<NumericComponent>().GetAsLong(NumericType.LastGameTime);
            self.LastLoginTime.GetComponent<Text>().text = TimeInfo.Instance.ToDateTime(lastTime).ToString();
        }

        public static async ETTask UpdatePosition(this UISettingGameComponent self)
        {
            await TimerComponent.Instance.WaitAsync(200);
            if (self.IsDisposed )
                return;

            ///self.ScrollRect.GetComponent<ScrollRect>().verticalNormalizedPosition = 0f;
            //RectTransform rectTransform = self.ChatContent.GetComponent<RectTransform>();
            //if (rectTransform.sizeDelta.y > 600)
            //{
            //    rectTransform.anchoredPosition = new Vector2(0, rectTransform.sizeDelta.y - 600);
            //}
        }

        public static void UpdateAttackMode(this UISettingGameComponent self)
        {
            string acttype = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.AttackMode);
            self.ActTypeSet.transform.Find("Image_Click_0").gameObject.SetActive(acttype == "0");
            self.ActTypeSet.transform.Find("Image_Click_1").gameObject.SetActive(acttype == "1");
            self.ActTypeSet.transform.Find("Image_Click_2").gameObject.SetActive(acttype == "2");
            self.ActTypeSet.transform.Find("Image_Click_3").gameObject.SetActive(acttype == "3");
        }

        public static void UpdateAttackTarget(this UISettingGameComponent self)
        {
            string acttype = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.AttackTarget);
            self.ActTargetSelect.transform.Find("Image_Click_0").gameObject.SetActive(acttype == "0");
            self.ActTargetSelect.transform.Find("Image_Click_1").gameObject.SetActive(acttype == "1");
        }

        public static void OnBeforeClose(this UISettingGameComponent self)
        {
            self.SendGameSetting().Coroutine();
        }

        public static void OnBtn_Fps(this UISettingGameComponent self)
        {
            UI uimain = UIHelper.GetUI(self.DomainScene(), UIType.UIMain);
            self.Image_fps.SetActive(!self.Image_fps.activeSelf);
            uimain.GetComponent<UIMainComponent>().ShowPing();
        }

        public static async ETTask SendGameSetting(this UISettingGameComponent self)
        {
            if (self.GameSettingInfos.Count > 0)
            {
                self.ZoneScene().GetComponent<UserInfoComponent>().UpdateGameSetting(self.GameSettingInfos);
                HintHelp.GetInstance().DataUpdate(DataType.SettingUpdate);
                C2M_GameSettingRequest c2M_GameSettingRequest = new C2M_GameSettingRequest() { GameSettingInfos = self.GameSettingInfos };
                M2C_GameSettingResponse r2c_roleEquip = (M2C_GameSettingResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_GameSettingRequest);
            }
        }

        public static void OnCloseGame(this UISettingGameComponent self)
        {
            PopupTipHelp.OpenPopupTip(self.ZoneScene(), "设置", "是否退出游戏?", () => { Application.Quit(); }).Coroutine();
        }

        public static void OnReturnLogin(this UISettingGameComponent self)
        {
            self.HideDi.SetActive(true);
            //加载登录场景
            EventType.ReturnLogin.Instance.ZoneScene = self.DomainScene();
            Game.EventSystem.PublishClass(EventType.ReturnLogin.Instance);
        }

        public static void SaveSettings(this UISettingGameComponent self, GameSettingEnum gameSettingEnum, string value)
        {
            bool exit = false;
            for (int i = 0; i < self.GameSettingInfos.Count; i++)
            {
                if (self.GameSettingInfos[i].KeyId == (int)gameSettingEnum)
                {
                    self.GameSettingInfos[i].Value = value;
                    exit = true;
                    break;
                }
            }
            if (!exit)
            {
                self.GameSettingInfos.Add(new KeyValuePair() { KeyId = (int)gameSettingEnum, Value = value });
            }
            self.ZoneScene().GetComponent<UserInfoComponent>().UpdateGameSetting(self.GameSettingInfos);
        }

        public static void OnBtn_Sound(this UISettingGameComponent self)
        {
            self.Image_Sound.SetActive(!self.Image_Sound.activeSelf);
            self.SaveSettings(GameSettingEnum.Sound, self.Image_Sound.activeSelf ? "1" : "0");
        }

        public static void OnBtn_YinYue(this UISettingGameComponent self)
        {
            self.Image_yinyu.SetActive(!self.Image_yinyu.activeSelf);
            self.SaveSettings(GameSettingEnum.Music, self.Image_yinyu.activeSelf ? "1" : "0");
        }

        public static void OnBtn_Fixed(this UISettingGameComponent self)
        {
            string value = self.Image_Fixed.gameObject.activeSelf ? "1" : "0";
            self.SaveSettings(GameSettingEnum.YanGan, value);
            self.UpdateYaoGan();
        }

        public static void UpdateYaoGan(this UISettingGameComponent self)
        {
            self.Image_Fixed.SetActive(self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.YanGan) == "0");
        }

        public static void OnHighFps(this UISettingGameComponent self)
        {
            string oldValue = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.HighFps);
            if (oldValue == "0")
            {
                PopupTipHelp.OpenPopupTip(self.ZoneScene(), "系统提示", "开启高帧模式，根据手机的配置不同可能导致手机发热耗电的情况，如果出现此现象请及时关闭喔!", () =>
                {
                    self.SaveSettings(GameSettingEnum.HighFps, oldValue == "0" ? "1" : "0");
                    self.UpdateHighFps();
                }, null).Coroutine();
            }
            else
            {
                self.SaveSettings(GameSettingEnum.HighFps, oldValue == "0" ? "1" : "0");
                self.UpdateHighFps();
            }
        }

        public static void OnSmooth(this UISettingGameComponent self)
        {
            string oldValue = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.Smooth);
            self.SaveSettings(GameSettingEnum.Smooth, oldValue == "0" ? "1" : "0");
            self.UpdateSmooth();
            SettingHelper.OnSmooth(oldValue == "0" ? "1" : "0");
        }

        public static void  OnGameMemory(this UISettingGameComponent self)
        {
            self.SendGameMemory().Coroutine();
        }

        public static async ETTask SendGameMemory(this UISettingGameComponent self)
        {
            if (GlobalHelp.GetBigVersion() < 17)
            {
                return;
            }
            BattleMessageComponent battleMessage = self.ZoneScene().GetComponent<BattleMessageComponent>();
            if (TimeHelper.ServerNow() - battleMessage.UploadMemoryTime < TimeHelper.Minute)
            {
                FloatTipManager.Instance.ShowFloatTip("请不要频繁操作！");
                return;
            }

            long monouse        = UnityEngine.Profiling.Profiler.GetMonoUsedSizeLong() ;               //使用的
            long totalallocated = UnityEngine.Profiling.Profiler.GetTotalAllocatedMemoryLong();         //unity分配的
            long totalreserved = UnityEngine.Profiling.Profiler.GetTotalReservedMemoryLong();           //总内存
            long unusedreserved = UnityEngine.Profiling.Profiler.GetTotalUnusedReservedMemoryLong();    //未使用的内存

            StringBuilder stringBuilder = StringBuilderHelper.stringBuilder;
            stringBuilder.Clear();
            stringBuilder.Append($"内存占用: 当前使用:{monouse/1024/1024}MB Unity分配:{totalallocated / 1024 / 1024}MB 总内存:{totalreserved / 1024 / 1024}MB 空闲内存:{unusedreserved / 1024 / 1024}MB");
            stringBuilder.AppendLine();

            stringBuilder.Append("EventSystem:");
            stringBuilder.AppendLine();
            stringBuilder.Append(EventSystem.Instance.ToString());
            stringBuilder.Append("ObjectPool:");
            stringBuilder.AppendLine();
            stringBuilder.Append(ObjectPool.Instance.ToString());
            stringBuilder.Append("MonoPool:");
            stringBuilder.AppendLine();
            stringBuilder.Append(MonoPool.Instance.ToString());
            stringBuilder.Append("GameObjectPool:");
            stringBuilder.AppendLine();
            stringBuilder.Append(GameObjectPoolComponent.Instance.ToString2());
           
            battleMessage.UploadMemoryTime = TimeHelper.ServerNow();

            C2Popularize_UploadRequest request = new C2Popularize_UploadRequest() { MemoryInfo = stringBuilder.ToString() };
            Popularize2C_UploadResponse response = (Popularize2C_UploadResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call( request );
        }

        public static void OnNoShowOther(this UISettingGameComponent self)
        {
            string oldValue = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.NoShowOther);
            self.SaveSettings(GameSettingEnum.NoShowOther, oldValue == "0" ? "1" : "0");
            self.UpdateNoShowOther();
            SettingHelper.OnShowOther(oldValue == "0" ? "1" : "0");

            List<Unit> units = UnitHelper.GetUnitList( self.ZoneScene().CurrentScene(), UnitType.Player );
            for (int i = units.Count - 1; i >= 0; i--)
            {
                if (!units[i].MainHero && units[i].Type == UnitType.Player)
                {
                    units[i].GetParent<UnitComponent>().Remove(units[i].Id);
                }
            }
        }

        public static void UpdateHighFps(this UISettingGameComponent self)
        {
            string oldValue = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.HighFps);
            self.HighFps.transform.Find("Image_Click").gameObject.SetActive(oldValue == "1");
            UICommonHelper.TargetFrameRate (oldValue == "1" ? 60 : 30 );
        }

        public static void UpdateSmooth(this UISettingGameComponent self)
        {
            string oldValue = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.Smooth);
            self.Smooth.transform.Find("Image_Click").gameObject.SetActive(oldValue == "1");
        }

        public static void UpdateNoShowOther(this UISettingGameComponent self)
        {
            string oldValue = self.UserInfoComponent.GetGameSettingValue(GameSettingEnum.NoShowOther);
            self.NoShowOther.transform.Find("Image_Click").gameObject.SetActive(oldValue == "1");
        }

        public static void CheckSensitiveWords(this UISettingGameComponent self)
        {
            string text_new = "";
            string text_old = self.InputFieldCName.GetComponent<InputField>().text;
            MaskWordHelper.Instance.IsContainSensitiveWords(ref text_old, out text_new);
            self.InputFieldCName.GetComponent<InputField>().text = text_old;
        }

        public static void OnButtonPhone(this UISettingGameComponent self)
        {
            PlayerInfo playerInfo = self.ZoneScene().GetComponent<AccountInfoComponent>().PlayerInfo;
            if (!string.IsNullOrEmpty(playerInfo.PhoneNumber))
            {
                FloatTipManager.Instance.ShowFloatTip($"已绑定手机号:{playerInfo.PhoneNumber}");
                return;
            }

            UIHelper.Create(self.ZoneScene(), UIType.UIPhoneCode).Coroutine();
        }

        public static async ETTask OnButtonRname(this UISettingGameComponent self)
        {
            string text = self.InputFieldCName.GetComponent<InputField>().text;

            if (string.IsNullOrEmpty(text))
            {
                FloatTipManager.Instance.ShowFloatTip("名字不合法，请重新输入！");
                return;
            }
            if (text.Length >= 8)
            {
                FloatTipManager.Instance.ShowFloatTip("角色名字过长，请重新输入！");
                return;
            }

            if (text.Contains("*") || !StringHelper.IsSpecialChar(text))
            {
                FloatTipManager.Instance.ShowFloatTip("名字不合法，请重新输入！");
                return;
            }
            GlobalValueConfig globalValueConfig = GlobalValueConfigCategory.Instance.Get(70);
            BagComponent bagComponent = self.ZoneScene().GetComponent<BagComponent>();
            if (!bagComponent.CheckNeedItem(globalValueConfig.Value))
            {
                string needItem = UICommonHelper.GetNeedItemDesc(globalValueConfig.Value);
                FloatTipManager.Instance.ShowFloatTip($"需要 {needItem}");
                return;
            }

            C2M_ModifyNameRequest c2M_GameSettingRequest = new C2M_ModifyNameRequest() { NewName = text };
            M2C_ModifyNameResponse r2c_roleEquip = (M2C_ModifyNameResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_GameSettingRequest);

            if (r2c_roleEquip.Error == ErrorCode.ERR_Success)
            {
                FloatTipManager.Instance.ShowFloatTip("修改名称成功！");
            }
        }
    }
}