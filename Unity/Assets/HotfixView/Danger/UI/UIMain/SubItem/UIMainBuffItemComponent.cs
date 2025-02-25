﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace ET
{
    public class UIMainBuffItemComponent : Entity, IAwake<GameObject>,IDestroy
    {
        public Text TextNumber;
        public GameObject TextLeftTime;
        public GameObject TextBuffName;
        public GameObject Img_BuffCD;
        public GameObject ImgBufflIcon;
        public GameObject GameObject;

        public int BuffID;
        public long BuffTime;
        public long EndTime;
        public string SpellCast;
        public string showTimeStr;
        public string BuffIcon;
        public string aBAtlasTypes;

        public long UnitId;
        public BuffManagerComponent BuffManagerComponent;
        public List<string> AssetPath = new List<string>();
    }

    public class UIMainBuffItemComponentAwakeSystem : AwakeSystem<UIMainBuffItemComponent, GameObject>
    {

        public override void Awake(UIMainBuffItemComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;   
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            self.TextBuffName = rc.Get<GameObject>("TextBuffName");
            self.Img_BuffCD = rc.Get<GameObject>("Img_BuffCD");
            self.ImgBufflIcon = rc.Get<GameObject>("ImgBufflIcon");
            self.TextLeftTime = rc.Get<GameObject>("TextLeftTime");
            self.TextNumber = rc.Get<GameObject>("TextNumber").GetComponent<Text>();
            self.TextNumber.gameObject.SetActive(true); 

            ButtonHelp.AddEventTriggers(self.ImgBufflIcon, (PointerEventData pdata) => { self.BeginDrag(pdata).Coroutine(); }, EventTriggerType.PointerDown);
            ButtonHelp.AddEventTriggers(self.ImgBufflIcon, (PointerEventData pdata) => { self.EndDrag(pdata); }, EventTriggerType.PointerUp);
        }
    }
    public class UIMainBuffItemComponentDestroy : DestroySystem<UIMainBuffItemComponent>
    {
        public override void Destroy(UIMainBuffItemComponent self)
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
    public static class UIMainBuffItemComponentSystem
    {
        public static async ETTask BeginDrag(this UIMainBuffItemComponent self, PointerEventData pdata)
        {
            UI skillTips = await UIHelper.Create(self.DomainScene(), UIType.UIBuffTips);
            if (self.IsDisposed)
            {
                return;
            }
            if (self.BuffID == 0)
            {
                Log.Error($"UIMainBuffItemComponent {self.BuffID == 0}");
                return;
            }

            Vector2 localPoint;
            RectTransform canvas = UIEventComponent.Instance.UILayers[(int)UILayer.Mid].gameObject.GetComponent<RectTransform>();
            Camera uiCamera = self.DomainScene().GetComponent<UIComponent>().UICamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pdata.position, uiCamera, out localPoint);
            skillTips.GetComponent<UIBuffTipsComponent>().OnUpdateData(self.BuffID, new Vector3(localPoint.x, localPoint.y, 0f), self.showTimeStr, self.SpellCast, self.aBAtlasTypes, self.BuffIcon);
        }

        public static void BeforeRemove(this UIMainBuffItemComponent self)
        { 
            UI uI = UIHelper.GetUI(self.DomainScene(), UIType.UIBuffTips);
            if (uI != null && self.BuffID == uI.GetComponent<UIBuffTipsComponent>().BuffId)
            {
                UIHelper.Remove(self.DomainScene(), UIType.UIBuffTips);
            }
        }

        public static void EndDrag(this UIMainBuffItemComponent self, PointerEventData pdata)
        {
            UIHelper.Remove(self.DomainScene(), UIType.UIBuffTips);
        }

        public static bool OnUpdate(this UIMainBuffItemComponent self)
        {
            long leftTime  = self.EndTime - TimeHelper.ClientNow();

            self.Img_BuffCD.GetComponent<Image>().fillAmount = (self.BuffTime - leftTime) * 1f /  self.BuffTime;
            leftTime = leftTime / 1000;
            self.TextLeftTime.GetComponent<Text>().text = self.showTimeStr;
            return leftTime > 0;
        }

        public static void OnResetBuff(this UIMainBuffItemComponent self, ABuffHandler aBuffHandler)
        {
            self.EndTime = aBuffHandler.BuffEndTime;
        }

        public static void UpdateBuffNumber(this UIMainBuffItemComponent self, ABuffHandler buffHandler, int number)
        {
            int BuffNumber = self.BuffManagerComponent.GetBuffNumber(buffHandler.BuffData.BuffId) + number;
            if (BuffNumber == 0)
            {
                self.BuffID = 0;
                self.EndTime = 0;
            }
            else if(number >= 0)
            {
                self.EndTime = buffHandler.BuffEndTime;
            }
            self.TextNumber.text = BuffNumber > 1 ? BuffNumber.ToString() : string.Empty;
        }

        public static void OnAddBuff(this UIMainBuffItemComponent self, ABuffHandler buffHandler)
        {
            long endTime = buffHandler.BuffData.BuffEndTime;
            SkillBuffConfig skillBuffConfig = buffHandler.mSkillBuffConf;
            self.BuffTime = skillBuffConfig.BuffTime;
            self.TextBuffName.GetComponent<Text>().text = skillBuffConfig.BuffName; 
            self.SpellCast = buffHandler.BuffData.Spellcaster;
            self.EndTime = endTime;
            self.BuffID = skillBuffConfig.Id;
            self.UnitId = buffHandler.TheUnitBelongto.Id;
            self.TextNumber.text = string.Empty;
            self.BuffManagerComponent = buffHandler.TheUnitBelongto.GetComponent<BuffManagerComponent>();
            string bufficon = skillBuffConfig.BuffIcon;
            //Buff表BuffIcon为0时,显示图标显示为对应的技能图标,如果没找到对应资源,
            //释放者是怪物,那么就显示怪物的头像Icon,最后还是没找到显示默认图标b001
            string aBAtlasTypes = ABAtlasTypes.RoleSkillIcon;

            if (!ComHelp.IfNull(bufficon) && skillBuffConfig.BuffIconType.Equals("ItemIcon"))
            {
                aBAtlasTypes = ABAtlasTypes.ItemIcon;
            }
            if (ComHelp.IfNull(bufficon) && buffHandler.BuffData.SkillId != 0)
            {
                bufficon = SkillConfigCategory.Instance.Get(buffHandler.BuffData.SkillId).SkillIcon;
            }
            if (ComHelp.IfNull(bufficon) && buffHandler.BuffData.UnitType == UnitType.Monster)
            {
                aBAtlasTypes = ABAtlasTypes.MonsterIcon;
                bufficon = MonsterConfigCategory.Instance.Get(buffHandler.BuffData.UnitConfigId).MonsterHeadIcon;
            }
            if (ComHelp.IfNull(bufficon))
            {
                bufficon = "b001";
            }
            self.aBAtlasTypes = aBAtlasTypes;
            self.BuffIcon = bufficon;
            string path =ABPathHelper.GetAtlasPath_2(aBAtlasTypes, bufficon);
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.ImgBufflIcon.GetComponent<Image>().sprite = sp;
        }

    }

}
