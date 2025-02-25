﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    [Timer(TimerType.PetMainTimer)]
    public class PetMainTimer : ATimer<UIPetMainComponent>
    {
        public override void Run(UIPetMainComponent self)
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

    public class UIPetMainComponent : Entity, IAwake, IDestroy
    {
        public GameObject MonsterHpNode;
        public GameObject PetHpNode;
        public GameObject UIMonsterHp;
        public GameObject UIPetHp;
        public GameObject Image_3;
        public GameObject Image_2;
        public GameObject Image_1;
        public GameObject Di;

        public GameObject PetFubenFinger;
        public GameObject Btn_RerurnBuilding;
        public GameObject CountdownTime;

        //手指第一次触摸点的位置
        public Vector2 m_scenePos = new Vector2();
        //摄像机
        public Transform CameraTarget;

        public long Timer;
        public float BeginTime;

        public Dictionary<long, PetBattleInfo> PetBattleList= new Dictionary<long, PetBattleInfo>();

        public M2C_FubenSettlement M2C_FubenSettlement;

        public int EnemyNumber = 0;
    }


    public class UIPetMainComponentDestroySystem : DestroySystem<UIPetMainComponent>
    {
        public override void Destroy(UIPetMainComponent self)
        {
            TimerComponent.Instance?.Remove(ref self.Timer);
            self.PetBattleList.Clear();
        }
    }

    public class PetBattleInfo
    {
        public Image Image;
        public Text Text;
        public long hurt;
        public long receive;
    }

    public class UIPetMainComponentAwakeSystem : AwakeSystem<UIPetMainComponent>
    {
        public override void Awake(UIPetMainComponent self)
        {
            self.EnemyNumber = 0;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.CameraTarget = UIComponent.Instance.MainCamera.transform;

            self.Btn_RerurnBuilding = rc.Get<GameObject>("Btn_RerurnBuilding");
            self.Btn_RerurnBuilding.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_RerurnBuilding(); });
            self.PetFubenFinger = rc.Get<GameObject>("PetFubenFinger");
            self.PetFubenFinger.GetComponent<Image>().color = new Color(255, 255, 255, 0);

            ButtonHelp.AddEventTriggers(self.PetFubenFinger, (PointerEventData pdata) => { self.BeginDrag(pdata); }, EventTriggerType.BeginDrag);
            ButtonHelp.AddEventTriggers(self.PetFubenFinger, (PointerEventData pdata) => { self.Draging(pdata); }, EventTriggerType.Drag);
            ButtonHelp.AddEventTriggers(self.PetFubenFinger, (PointerEventData pdata) => { self.EndDrag(pdata); }, EventTriggerType.EndDrag);
            MapComponent mapComponent = self.ZoneScene().GetComponent<MapComponent>();
            self.PetFubenFinger.SetActive(mapComponent.SceneTypeEnum== SceneTypeEnum.PetDungeon);

            self.PetBattleList.Clear();
            self.UIMonsterHp = rc.Get<GameObject>("UIMonsterHp");
            self.UIMonsterHp.SetActive(false);
            self.UIPetHp = rc.Get<GameObject>("UIPetHp");
            self.UIPetHp.SetActive(false);
            self.MonsterHpNode = rc.Get<GameObject>("MonsterHpNode");
            self.PetHpNode = rc.Get<GameObject>("PetHpNode");

            self.Di = rc.Get<GameObject>("Di");
            self.Image_3 = rc.Get<GameObject>("Image_3");
            self.Image_2 = rc.Get<GameObject>("Image_2");
            self.Image_1 = rc.Get<GameObject>("Image_1");
            self.Image_3.SetActive(false);
            self.Image_2.SetActive(false);
            self.Image_1.SetActive(false);

            self.M2C_FubenSettlement = null;
            self.CountdownTime = rc.Get<GameObject>("CountdownTime");

            //DataUpdateComponent.Instance.RemoveListener(DataType.UnitHpUpdate, self);
            self.InitHpList();
            self.OnPlayAnimation().Coroutine();
        }
    }

    public static class UIPetMainComponentSystem
    {

        public static void OnFubenResult(this UIPetMainComponent self, M2C_FubenSettlement m2C_FubenSettlement)
        {
            self.M2C_FubenSettlement = m2C_FubenSettlement;
        }

        public static void OnUnitHpUpdate(this UIPetMainComponent self, Unit defend, Unit attack, long changeHp)
        {
            if (!self.PetBattleList.ContainsKey(defend.Id))
            {
                return;
            }

            NumericComponent numericComponent = defend.GetComponent<NumericComponent>();
            float curhp = numericComponent.GetAsLong(NumericType.Now_Hp);
            float blood = curhp / numericComponent.GetAsLong(NumericType.Now_MaxHp);
            blood = Mathf.Max(blood, 0f);
            self.PetBattleList[defend.Id].Image.fillAmount = blood;

            if (attack != null && self.PetBattleList.ContainsKey(attack.Id) && defend !=attack && changeHp < 0)
            {
                self.PetBattleList[attack.Id].hurt += (changeHp * -1);
                self.PetBattleList[defend.Id].receive += (changeHp * -1);
                self.UpdateHurtValue(attack.Id);
                self.UpdateHurtValue(defend.Id);
            }
        }

        public static void UpdateHurtValue(this UIPetMainComponent self, long unitId)
        { 
            PetBattleInfo petBattleInfo = self.PetBattleList[unitId];
            self.PetBattleList[unitId].Text.text = $"造成伤害:{petBattleInfo.hurt} 承受伤害:{petBattleInfo.receive}";
        }

        public static void InitHpList(this UIPetMainComponent self)
        {
            List<Unit> entities = self.ZoneScene().CurrentScene().GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < entities.Count; i++)
            {
                if (entities[i].Type == UnitType.Player)
                {
                    continue;
                }
                int camp = entities[i].GetBattleCamp();

                self.PetBattleList.Add(entities[i].Id, new PetBattleInfo());
                if (camp == CampEnum.CampPlayer_1)
                {
                    GameObject gameObject = GameObject.Instantiate(self.UIPetHp);
                    UICommonHelper.SetParent(gameObject, self.PetHpNode);
                    gameObject.SetActive(true);

                    gameObject.transform.Find("Lal_Name").GetComponent<Text>().text = PetConfigCategory.Instance.Get(entities[i].ConfigId).PetName;
                    self.PetBattleList[entities[i].Id].Image = gameObject.transform.Find("Img_HpValue").GetComponent<Image>();
                    self.PetBattleList[entities[i].Id].Text = gameObject.transform.Find("Lal_Hurt").GetComponent<Text>();
                    continue;
                }
                if (entities[i].Type == UnitType.Pet)
                {
                    self.EnemyNumber ++;
                    GameObject gameObject = GameObject.Instantiate(self.UIMonsterHp);
                    UICommonHelper.SetParent(gameObject, self.MonsterHpNode);
                    gameObject.SetActive(true);

                    gameObject.transform.Find("Lal_Name").GetComponent<Text>().text = PetConfigCategory.Instance.Get(entities[i].ConfigId).PetName;
                    gameObject.transform.Find("Lal_Lv").GetComponent<Text>().text = "";
                    self.PetBattleList[entities[i].Id].Image = gameObject.transform.Find("Img_HpValue").GetComponent<Image>();
                    self.PetBattleList[entities[i].Id].Text = gameObject.transform.Find("Lal_Hurt").GetComponent<Text>();
                    continue;
                }
                if (entities[i].Type == UnitType.Monster)
                {
                    self.EnemyNumber++;
                    GameObject gameObject = GameObject.Instantiate(self.UIMonsterHp);
                    UICommonHelper.SetParent(gameObject, self.MonsterHpNode);
                    gameObject.SetActive(true);

                    MonsterConfig monsterCof = MonsterConfigCategory.Instance.Get(entities[i].ConfigId);
                    gameObject.transform.Find("Lal_Name").GetComponent<Text>().text = monsterCof.MonsterName;
                    gameObject.transform.Find("Lal_Lv").GetComponent<Text>().text = monsterCof.Lv.ToString();
                    self.PetBattleList[entities[i].Id].Image = gameObject.transform.Find("Img_HpValue").GetComponent<Image>();
                    self.PetBattleList[entities[i].Id].Text = gameObject.transform.Find("Lal_Hurt").GetComponent<Text>();
                    continue;
                }
            }
        }

        public static void  OnTimer(this UIPetMainComponent self)
        {
            Camera camera = UIComponent.Instance.MainCamera;
            float passTime = Time.time - self.BeginTime;
            float fieldOfView = 50f - (passTime / 10f) * 20;
            fieldOfView = Math.Max(fieldOfView, 30);
            camera.GetComponent<Camera>().fieldOfView = fieldOfView;
        }

        public static async ETTask OnPlayAnimation(this UIPetMainComponent self)
        {
            long instanceId = self.InstanceId;
            await TimerComponent.Instance.WaitAsync(500);
            self.Image_3.SetActive(true);
            self.BeginTime = Time.time;
            self.Timer = TimerComponent.Instance.NewFrameTimer(TimerType.PetMainTimer, self);
            UICommonHelper.DOScale(self.Image_3.transform, Vector3.zero, 1f);
            await TimerComponent.Instance.WaitAsync(1000);
            if (instanceId != self.InstanceId)
            {
                return;
            }
            self.Image_3.SetActive(false);
            self.Image_2.SetActive(true);
            UICommonHelper.DOScale(self.Image_2.transform, Vector3.zero, 1f);
            await TimerComponent.Instance.WaitAsync(1000);
            if (instanceId != self.InstanceId)
            {
                return;
            }
            self.Image_2.SetActive(false);
            self.Image_1.SetActive(true);
            UICommonHelper.DOScale(self.Image_1.transform, Vector3.zero, 1f);
            await TimerComponent.Instance.WaitAsync(1000);
            if (instanceId != self.InstanceId)
            {
                return;
            }
            self.Image_1.SetActive(false);
            self.Di.SetActive(false);
            C2M_PetFubenBeginRequest c2M_PetFubenBeginRequest = new C2M_PetFubenBeginRequest();
            await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(c2M_PetFubenBeginRequest);
            self.BeginCountdown().Coroutine();
        }

        public static async ETTask BeginCountdown(this UIPetMainComponent self)
        {
            long instanceId = self.InstanceId;
            int cdTime =  GlobalValueConfigCategory.Instance.Get(60).Value2;

            if (self.EnemyNumber == 0)
            {
                cdTime = 10;
            }
            for (int i = cdTime; i >= 0; i--)
            {
                if (instanceId != self.InstanceId)
                {
                    return;
                }
                if (self.M2C_FubenSettlement != null)
                {
                    return;
                }
                if (i == 0)
                {
                    self.CountdownTime.GetComponent<Text>().text = i.ToString();
                    self.ZoneScene().GetComponent<SessionComponent>().Session.Send(new C2M_PetFubenOverRequest());
                }
                else
                {
                    self.CountdownTime.GetComponent<Text>().text = i.ToString();
                    await TimerComponent.Instance.WaitAsync(1000);
                }
            }
        }

        public static void BeginDrag(this UIPetMainComponent self, PointerEventData pdata)
        {
            self.m_scenePos = Input.mousePosition;
        }

        public static void Draging(this UIPetMainComponent self, PointerEventData pdata)
        {
            Vector3 move = new Vector3(pdata.delta.x * -0.01f, 0f, pdata.delta.y * -0.01f);
            Quaternion rotaion = Quaternion.Euler(0, -90,0);
            Vector3 targ = self.CameraTarget.localPosition + rotaion * move;
            targ.x = Mathf.Clamp(targ.x, AIHelp.FuBenCameraPositionMin_X, AIHelp.FuBenCameraPositionMax_X);
            targ.z = Mathf.Clamp(targ.z, AIHelp.FuBenCameraPositionMin_Z, AIHelp.FuBenCameraPositionMax_Z);
            self.CameraTarget.localPosition = targ;
        }

        public static void EndDrag(this UIPetMainComponent self, PointerEventData pdata)
        {
        }

        public static void OnBtn_RerurnBuilding(this UIPetMainComponent self)
        {
            PopupTipHelp.OpenPopupTip(self.DomainScene(), "", GameSettingLanguge.LoadLocalization("确定返回主城？"),
               () =>
               {
                   EnterFubenHelp.RequestQuitFuben(self.ZoneScene());
               },
               null).Coroutine();
        }
    }
}
