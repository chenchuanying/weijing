using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    [Timer(TimerType.SkillInfoShowTimer)]
    public class SkillInfoShowTimer : ATimer<UISkillGridComponent>
    {
        public override void Run(UISkillGridComponent self)
        {
            try
            {
               self.SkillInfoShow().Coroutine();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    public class UISkillButtonComponentAwakeSystem : AwakeSystem<UISkillGridComponent, GameObject>
    {
        public override void Awake(UISkillGridComponent self, GameObject gameObjectnt)
        {
            self.Awake(gameObjectnt);
        }
    }

    public class UISkillGridComponentDestroySystem: DestroySystem<UISkillGridComponent>
    {
        public override void Destroy(UISkillGridComponent self)
        {
            if (self.SkillInfoShowTimer != 0 )
            {
                TimerComponent.Instance?.Remove(ref self.SkillInfoShowTimer);
                self.SkillInfoShowTimer = 0;
            }
        }
    }

    public class UISkillGridComponent : Entity, IAwake, IAwake<GameObject>,IDestroy
    {
        public GameObject SkillYanGan;
        public GameObject Button_Cancle;
        public GameObject SkillDi;
        public GameObject Btn_SkillStart;
        public GameObject Img_SkillIcon;
        public GameObject Text_SkillItemNum;
        public Image Img_SkillCD;
        public Text Text_SkillCD;
        public Image Img_PublicSkillCD;
        public GameObject Img_Mask;
        public GameObject GameObject;
        public Image SkillSecondCD;

        public SkillConfig SkillWuqiConfig;
        public SkillConfig SkillBaseConfig;

        public bool UseSkill;
        public bool CancelSkill;
        public SkillPro SkillPro;
        public Action<bool> SkillCancelHandler;

        public LockTargetComponent LockTargetComponent;
        public SkillIndicatorComponent SkillIndicatorComponent;
        public long SkillInfoShowTimer;
        public int Index;
        public Action<int> UseSkillHandler;
        public List<string> AssetPath = new List<string>();

        public int SkillSecond = 0;    //1 可以二段 
        public int CdRate = 1;

        
        public void Awake(GameObject gameObject)
        {
            this.GameObject = gameObject;
            this.Button_Cancle = gameObject.transform.Find("Button_Cancle").gameObject;
            this.SkillDi = gameObject.transform.Find("SkillDi").gameObject;
            this.Btn_SkillStart = gameObject.transform.Find("Btn_SkillStart").gameObject;
            this.Img_SkillIcon = gameObject.transform.Find("Img_Mask/Img_SkillIcon").gameObject;
            this.Text_SkillItemNum = gameObject.transform.Find("Text_SkillItemNum").gameObject;
            this.Img_SkillCD = gameObject.transform.Find("Img_SkillCD").gameObject.GetComponent<Image>();
            this.Text_SkillCD = gameObject.transform.Find("Text_SkillCD").gameObject.GetComponent<Text>();
            this.Img_PublicSkillCD = gameObject.transform.Find("Img_PublicSkillCD").gameObject.GetComponent<Image>();
            this.Img_Mask = gameObject.transform.Find("Img_Mask").gameObject;
            this.SkillYanGan = gameObject.transform.Find("SkillYanGan").gameObject;
            this.SkillSecondCD = gameObject.transform.Find("SkillSecondCD").gameObject.GetComponent<Image>();
            this.SkillSecondCD.gameObject.SetActive(false);
            this.Button_Cancle.SetActive(false);
            this.SkillYanGan.SetActive(false);
            this.Text_SkillItemNum.SetActive(false);

            this.RemoveEventTriggers();

            ButtonHelp.AddListenerEx(this.Button_Cancle, this.SendCancleSkill);
            ButtonHelp.AddEventTriggers(this.Btn_SkillStart, (PointerEventData pdata) => { this.Draging(pdata); }, EventTriggerType.Drag);
            ButtonHelp.AddEventTriggers(this.Btn_SkillStart, (PointerEventData pdata) => { this.EndDrag(pdata); }, EventTriggerType.EndDrag);
            ButtonHelp.AddEventTriggers(this.Btn_SkillStart, (PointerEventData pdata) => { this.OnPointDown(pdata); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(this.Btn_SkillStart, (PointerEventData pdata) => { this.PointerUp(pdata); }, EventTriggerType.PointerUp);
            ButtonHelp.AddEventTriggers(this.Btn_SkillStart, (PointerEventData pdata) => { this.OnCancel(pdata); }, EventTriggerType.Cancel);
            this.LockTargetComponent = this.ZoneScene().GetComponent<LockTargetComponent>();
            this.SkillIndicatorComponent = this.ZoneScene().GetComponent<SkillIndicatorComponent>();
        }
    }
    public class UISkillGridComponentDestroy: DestroySystem<UISkillGridComponent>
    {
        public override void Destroy(UISkillGridComponent self)
        {
            for (int i = 0; i < self.AssetPath.Count; i++)
            {
                if (!string.IsNullOrEmpty(self.AssetPath[i]))
                {
                    ResourcesComponent.Instance.UnLoadAsset(self.AssetPath[i]);
                }
            }

            self.AssetPath = null;
        }
    }
    public static class UISkillGridComponentSystem
    {

        public static void RemoveEventTriggers(this UISkillGridComponent self)
        {
            EventTrigger eventTrigger = self.Btn_SkillStart.GetComponent<EventTrigger>();
            eventTrigger.triggers.RemoveRange(0, eventTrigger.triggers.Count);

            self.Button_Cancle.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        public static async ETTask SkillInfoShow(this UISkillGridComponent self)
        {
            UI skillTips = await UIHelper.Create(self.DomainScene(), UIType.UISkillTips);

            Vector2 localPoint;
            RectTransform canvas = UIEventComponent.Instance.UILayers[(int)UILayer.Mid].gameObject.GetComponent<RectTransform>();
            Camera uiCamera = self.DomainScene().GetComponent<UIComponent>().UICamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, uiCamera, out localPoint);
            skillTips.GetComponent<UISkillTipsComponent>().OnUpdateData(self.SkillPro.SkillID, new Vector3(localPoint.x, localPoint.y, 0f));
        }


        public static void OnCancel(this UISkillGridComponent self, PointerEventData eventData)
        {

        }

        public static int GetSkillId(this UISkillGridComponent self)
        {
            return self.SkillBaseConfig != null ? self.SkillBaseConfig.Id : 0;
        }

        public static void OnUpdate(this UISkillGridComponent self, long leftCDTime, long pulicCd)
        {
            //显示冷却CD
            if (leftCDTime > 0)
            {
                int showCostTime = (int)(leftCDTime / 1000) + 1;
                self.Text_SkillCD.text = showCostTime.ToString();
                float proValue = (float)leftCDTime / ((float)self.SkillBaseConfig.SkillCD * 1000f * self.CdRate);
                self.Img_SkillCD.fillAmount = proValue;
                if (self.SkillSecond == 1)  //已释放二段斩 进入CD
                {
                    self.SkillSecond = 0;
                    self.SkillSecondCD.gameObject.SetActive(false);  
                }
            }
            else
            {
                self.Img_SkillCD.fillAmount = 0f;
                self.Text_SkillCD.text = string.Empty;
            }

            //显示公共CD
            if (pulicCd > 0)
            {
                float proValue = (float)(pulicCd / 800f);     //1秒公共CD
                self.Img_PublicSkillCD.fillAmount = proValue;
            }
            else
            {
                self.Img_PublicSkillCD.fillAmount = 0f;
            }
        }

        public static void RemoveSkillInfoShow(this UISkillGridComponent self)
        {
            if (self.SkillInfoShowTimer != 0)
            {
                TimerComponent.Instance.Remove(ref self.SkillInfoShowTimer);
                self.SkillInfoShowTimer = 0;
            }
            UIHelper.Remove(self.DomainScene(), UIType.UISkillTips);
        }

        public static void Draging(this UISkillGridComponent self, PointerEventData eventData)
        {
            if (self.SkillInfoShowTimer != 0)
            {
                TimerComponent.Instance.Remove(ref self.SkillInfoShowTimer);
                self.SkillInfoShowTimer = 0;
            }
            
            if (self.IfShowSkillZhishi() == false || !self.UseSkill)
            {
                return;
            }
            
            self.SkillIndicatorComponent.OnMouseDrag(eventData.delta);
        }

        /// <summary>
        /// 0  立即释放,自身中心点
        /// 1  技能指示器
        /// 2  立即释放,目标中心点[需要传目标ID]
        /// </summary>
        /// <returns></returns>
        public static bool IfShowSkillZhishi(this UISkillGridComponent self)
        {
            if (self.SkillWuqiConfig == null)
                return false;
            if (self.SkillWuqiConfig.SkillTargetType == 7)
                return true;
            return self.SkillWuqiConfig.SkillZhishiType > 0;
        }

        public static void SendUseSkill(this UISkillGridComponent self, int angle, float distance)
        {
            self.SkillCancelHandler(false);
            if (self.CancelSkill)
            {
                return;
            }

            Unit myUnit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            long targetId = self.LockTargetComponent.LastLockId;

            if (self.SkillWuqiConfig.SkillTargetType == (int)SkillTargetType.TargetOnly)
            {
                Unit targetUnit = null;
                if (targetId == 0)
                {
                    FloatTipManager.Instance.ShowFloatTip("请选中施法目标");
                    return;
                }
                targetUnit = myUnit.GetParent<UnitComponent>().Get(targetId);
              
                // 判断施法距离
                //if (direction.sqrMagnitude > self.SkillWuqiConfig.SkillRangeSize * 20)
                //{
                //    FloatTipManager.Instance.ShowFloatTip("施法距离太远");
                //    return;
                //}
                if (targetUnit == null || Vector3.Distance(targetUnit.Position, myUnit.Position) > self.SkillWuqiConfig.SkillRangeSize)
                {
                    FloatTipManager.Instance.ShowFloatTip("施法距离太远");
                    return;
                }
                Vector3 direction = targetUnit.Position - myUnit.Position;
                angle = Mathf.FloorToInt(Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg);
            }

            EventType.BeforeSkill.Instance.ZoneScene = self.ZoneScene();
            EventSystem.Instance.PublishClass(EventType.BeforeSkill.Instance);
            if (self.SkillPro.SkillSetType == (int)SkillSetEnum.Skill)
            {
                int skillId = self.SkillBaseConfig.Id;
                SkillManagerComponent skillManagerComponent = myUnit.GetComponent<SkillManagerComponent>();
                if (self.SkillSecond == 1)
                {
                    //用二段技能
                    SkillSetComponent skillSetComponent = self.ZoneScene().GetComponent<SkillSetComponent>();
                    skillId = SkillHelp.GetNewSkill(skillSetComponent.SkillList, skillId);

                    skillId = (int)SkillConfigCategory.Instance.BuffSecondSkill[skillId].Value2;
                    skillManagerComponent.AddSkillSecond(self.SkillBaseConfig.Id, skillId);
                }
                skillManagerComponent.SendUseSkill(skillId, 0, angle, targetId, distance).Coroutine();
            }
            else
            {
                BagInfo bagInfo = self.ZoneScene().GetComponent<BagComponent>().GetBagInfo(self.SkillPro.SkillID);
                if (bagInfo == null)
                {
                    return;
                }
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.SkillPro.SkillID);
                if (itemConfig.ItemSubType == 101) // 药剂、鞭炮 走的使用技能的流程
                {
                    myUnit.GetComponent<SkillManagerComponent>().SendUseSkill(int.Parse(itemConfig.ItemUsePar), self.SkillPro.SkillID, angle, targetId, distance).Coroutine();
                }
                else // 道具 走的使用道具的流程
                {
                    self.ZoneScene().GetComponent<BagComponent>().SendUseItem(bagInfo, null).Coroutine();
                }
            }

            if (self.SkillPro.SkillSource == SkillSourceEnum.Buff)
            {
                self.UseSkillHandler?.Invoke(self.Index);
            }
        }

        public static void EndDrag(this UISkillGridComponent self, PointerEventData eventData)
        {
            self.RemoveSkillInfoShow();
            self.SkillCancelHandler(false);
            if (self.SkillWuqiConfig != null && self.SkillWuqiConfig.SkillZhishiType == 1)
            {
                self.SkillYanGan.SetActive(false);
            }
            if (self.IfShowSkillZhishi() == false || !self.UseSkill)
            {
                return;
            }
            self.UseSkill = false;
            self.SendUseSkill(self.GetTargetAngle(), self.GetTargetDistance());
            self.SkillIndicatorComponent.RecoveryEffect();
        }


        public static void PointerUp(this UISkillGridComponent self, PointerEventData eventData)
        {
            self.RemoveSkillInfoShow();
            if (self.SkillWuqiConfig!=null && self.SkillWuqiConfig.SkillZhishiType == 1)
            {
                self.SkillYanGan.SetActive(false);
            }
            if (!self.UseSkill)
            {
                return;
            }
            
            self.UseSkill = false;
            self.SendUseSkill(self.GetTargetAngle(), self.GetTargetDistance());
            self.ZoneScene().GetComponent<SkillIndicatorComponent>().RecoveryEffect();
        }

        public static void OnPointDown(this UISkillGridComponent self, PointerEventData eventData)
        {
            if (self.SkillBaseConfig == null)
            {
                return;
            }

            self.RemoveSkillInfoShow();
            self.SkillInfoShowTimer = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + 2000, TimerType.SkillInfoShowTimer, self);
            if (self.SkillWuqiConfig.SkillZhishiType == 1)
            {
                self.SkillYanGan.SetActive(true);
            }
            self.CancelSkill = false;
            Unit myUnit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());

            //锁定目标
            self.LockTargetComponent.SkillLock(myUnit, self.SkillWuqiConfig);

            if (!self.IfShowSkillZhishi())
            {
                self.UseSkill = false;
                self.SkillCancelHandler(false);
                self.SendUseSkill((int)myUnit.Rotation.eulerAngles.y, 0);
                self.SkillIndicatorComponent.RecoveryEffect();
                return;
            }

            //long targetId = self.LockTargetComponent.LastLockId;
            //UnitComponent unitComponent = myUnit.GetParent<UnitComponent>();
            //Unit targetUnit = unitComponent.Get(targetId);
            ////获取当前目标和自身目标的距离
            //if (targetUnit == null || (PositionHelper.Distance2D(targetUnit, myUnit) + 4) > self.SkillWuqiConfig.SkillRangeSize)
            //{
            //    //获取当前最近的单位
            //    Unit enemy = AIHelp.GetNearestEnemy_Client(myUnit, (float)self.SkillWuqiConfig.SkillRangeSize + 4);
            //    //设置目标
            //    if (targetUnit == null && enemy != null)
            //    {
            //        self.LockTargetComponent.LockTargetUnitId(enemy.Id);
            //    }
            //}

            self.UseSkill = true;
            self.SkillCancelHandler(true);
            self.SkillIndicatorComponent.ShowSkillIndicator(self.SkillWuqiConfig);
            self.SkillIndicatorComponent.OnMouseDown(self.LockTargetComponent.LastLockId);
        }

        public static int GetTargetAngle(this UISkillGridComponent self)
        {
            return self.ZoneScene().GetComponent<SkillIndicatorComponent>().GetIndicatorAngle();
        }

        //X100
        public static float GetTargetDistance(this UISkillGridComponent self)
        {
            return self.ZoneScene().GetComponent<SkillIndicatorComponent>().GetIndicatorDistance();
        }

        public static void OnEnterCancelButton(this UISkillGridComponent self)
        {
            self.CancelSkill = true;
        }

        public static void UpdateItemNumber(this UISkillGridComponent self)
        {
            if (self.SkillPro == null)
            {
                return;
            }
            if (self.SkillPro.SkillSetType != (int)SkillSetEnum.Item)
            {
                return;
            }
            long number = self.ZoneScene().GetComponent<BagComponent>().GetItemNumber(self.SkillPro.SkillID);
            self.Text_SkillItemNum.SetActive(true);
            self.Text_SkillItemNum.GetComponent<Text>().text = number.ToString();

            UICommonHelper.SetImageGray(self.SkillDi.gameObject, number == 0);
            UICommonHelper.SetImageGray(self.Img_SkillIcon.gameObject, number == 0);
        }

        public static void SendCancleSkill(this UISkillGridComponent self)
        {
            C2M_SkillInterruptRequest request = new C2M_SkillInterruptRequest() { SkillID = self.SkillPro.SkillID };
            self.ZoneScene().GetComponent<SessionComponent>().Session.Send(request);
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            unit.GetComponent<SkillManagerComponent>().AddSkillCD(self.SkillPro.SkillID, TimeHelper.ServerNow() + 10000, TimeHelper.ServerNow() + 1000);
            self.Button_Cancle.SetActive(false);
        }

        public static void CheckSkillSecond(this UISkillGridComponent self)
        {
            Unit main = UnitHelper.GetMyUnitFromZoneScene( self.ZoneScene() );
            //有对应的buff才能触发二段斩
            int buffId = (int)SkillConfigCategory.Instance.BuffSecondSkill[self.SkillPro.SkillID].KeyId;

            bool havebuff  = false;
            List<Unit> allDefend = main.GetParent<UnitComponent>().GetAll();
            for (int defend = 0; defend < allDefend.Count; defend++)
            {
                BuffManagerComponent buffManagerComponent = allDefend[defend].GetComponent<BuffManagerComponent>();
                if (buffManagerComponent == null || allDefend[defend].Id == main.Id) //|| allDefend[defend].Id == request.TargetID 
                {
                    continue;
                }
                int buffNum = buffManagerComponent.GetBuffSourceNumber(main.Id, buffId);
                if (buffNum <= 0)
                {
                    continue;
                }

                havebuff = true;
                break;
            }

            if (!havebuff)
            {
                self.OnSkillSecondResult(null);
            }
        }

        public static void OnSkillSecondResult(this UISkillGridComponent self, M2C_SkillSecondResult message)
        {
            if (self.SkillPro == null)
            {
                return;
            }

            if (message != null && message.HurtIds.Count > 0)
            {
                ///可以释放二段技能
                if (self.SkillSecond == 0)
                {
                    self.SkillSecond = 1;
                    self.SkillSecondCD.gameObject.SetActive(true);
                    Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
                    unit.GetComponent<SkillManagerComponent>().ClearSkillCD(message.SkillId);
                    self.ShowSkillSecondCD(self.SkillPro.SkillID).Coroutine();
                }
            }

            if (message == null)
            {
                //未及时释放二段斩，进入CD
                self.SkillSecond = 0;
                self.SkillSecondCD.gameObject.SetActive(false);
                Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
                SkillConfig skillConfig = SkillConfigCategory.Instance.Get(self.SkillPro.SkillID);
                unit.GetComponent<SkillManagerComponent>().AddSkillCD(self.SkillPro.SkillID, TimeHelper.ServerNow() + (long)(skillConfig.SkillCD * 1000), TimeHelper.ServerNow() + 500);
            }
        }

        public static void ResetSkillSecond(this UISkillGridComponent self)
        {
            self.SkillSecond = 0;
            self.SkillSecondCD.gameObject.SetActive(false);
        }

        public static async ETTask ShowSkillSecondCD(this UISkillGridComponent self, int skillId)
        {
            KeyValuePairLong4 keyValuePairLong = null;
            SkillConfigCategory.Instance.BuffSecondSkill.TryGetValue(skillId, out keyValuePairLong);
            if (keyValuePairLong == null)
            {
                return;
            }

            long allTime = SkillBuffConfigCategory.Instance.Get((int)keyValuePairLong.KeyId).BuffTime;
            long passTime = 0;
            while (true) 
            {
                if (self.IsDisposed)
                {
                    self.SkillSecond = 0;
                    break;
                }
                if (self.SkillPro == null || self.SkillPro.SkillID != skillId)
                {
                    self.SkillSecond = 0;
                    self.SkillSecondCD.gameObject.SetActive(false);
                    break;
                }
                if (passTime >= allTime)
                {
                    self.OnSkillSecondResult(null);
                    break;
                }
                if (self.SkillSecond == 0)
                {
                    break;
                }
                self.SkillSecondCD.fillAmount = 1f * (allTime - passTime) / allTime;
                await TimerComponent.Instance.WaitAsync(100);
                passTime += 100;
            }
        }

        public static void UpdateSkillInfo(this UISkillGridComponent self, SkillPro skillpro)
        {
            self.SkillPro = skillpro;
            if (skillpro == null)
            {
                self.SkillWuqiConfig = null;
                self.SkillBaseConfig = null;
                self.SkillPro = null;
                self.Img_PublicSkillCD.fillAmount = 0;
                self.Img_SkillIcon.SetActive(false);
                self.Img_Mask.SetActive(false);
                return;
            }
            if (skillpro.SkillSetType == (int)SkillSetEnum.Skill)
            {
                SkillSetComponent skillSetComponent = self.ZoneScene().GetComponent<SkillSetComponent>();
                int skillid = SkillHelp.GetWeaponSkill(skillpro.SkillID, UnitHelper.GetEquipType(self.ZoneScene()), skillSetComponent.SkillList);
                if (!SkillConfigCategory.Instance.Contain(skillid))
                {
                    self.SkillPro = null;
                    return;
                }
                SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillid);
                string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.RoleSkillIcon, skillConfig.SkillIcon);
                Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
                if (!self.AssetPath.Contains(path))
                {
                    self.AssetPath.Add(path);
                }
                self.Img_SkillIcon.GetComponent<Image>().sprite = sp;

                self.SkillWuqiConfig = skillConfig;
                self.SkillBaseConfig = SkillConfigCategory.Instance.Get(skillpro.SkillID);
                self.CdRate = 1;
            }
            else
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(skillpro.SkillID);
                if (itemConfig.ItemSubType == 101) // 药剂、鞭炮 走的使用技能的流程
                {
                    int skillid = int.Parse(itemConfig.ItemUsePar);
                    if (!SkillConfigCategory.Instance.Contain(skillid))
                    {
                        self.SkillPro = null;
                        return;
                    }

                    self.SkillWuqiConfig = SkillConfigCategory.Instance.Get(skillid);
                    self.SkillBaseConfig = self.SkillWuqiConfig;

                    MapComponent mapComponent = self.ZoneScene().GetComponent<MapComponent>();
                    self.CdRate = ComHelp.GetSkillCdRate(mapComponent.SceneTypeEnum);
                }
                else // 道具 走的使用道具的流程
                {
                    self.SkillWuqiConfig = new SkillConfig();
                    self.SkillBaseConfig = self.SkillWuqiConfig;
                    self.CdRate = 1;    
                }

                string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
                Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
                if (!self.AssetPath.Contains(path))
                {
                    self.AssetPath.Add(path);
                }
                self.Img_SkillIcon.GetComponent<Image>().sprite = sp;
            }
            self.Button_Cancle.SetActive(false);
            self.Img_SkillIcon.SetActive(true);
            self.Img_Mask.SetActive(true);
            self.Text_SkillCD.text = string.Empty;
            self.Text_SkillItemNum.SetActive(false);
            self.UpdateItemNumber();
        }
    }
}
