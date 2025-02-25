using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace ET
{ 
    
    public class UIJueXingShowComponent : Entity, IAwake,IDestroy
    {

        public GameObject Text_11;
        public GameObject Text_Gold;
        public GameObject ButtonActive;
        public GameObject TextSkillName;
        public GameObject ImageSkillIcon;
        public GameObject ImageJueXingExp;
        public GameObject Text_JueXingExp;
        public UICommonCostItemComponent UICommonCostItem;

        public List<UIJueXingShowItemComponent> UIJueXingShowItems = new List<UIJueXingShowItemComponent>();

        public int JueXingId;
        
        public List<string> AssetPath = new List<string>();
    }

    public class UIJueXingShowComponentAwake : AwakeSystem<UIJueXingShowComponent>
    {

        public override void Awake(UIJueXingShowComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.UIJueXingShowItems.Clear();

            self.Text_11 = rc.Get<GameObject>("Text_11");
            self.Text_Gold = rc.Get<GameObject>("Text_Gold");
            self.ButtonActive = rc.Get<GameObject>("ButtonActive");
            ButtonHelp.AddListenerEx(self.ButtonActive, () => { self.OnButtonActive().Coroutine(); });

            self.TextSkillName = rc.Get<GameObject>("TextSkillName");
            self.ImageSkillIcon = rc.Get<GameObject>("ImageSkillIcon");
            self.ImageJueXingExp = rc.Get<GameObject>("ImageJueXingExp");
            self.Text_JueXingExp = rc.Get<GameObject>("Text_JueXingExp");

            GameObject UICommonCostItem = rc.Get<GameObject>("UICommonCostItem");
            self.UICommonCostItem =  self.AddChild<UICommonCostItemComponent, GameObject>(UICommonCostItem);


            self.OnInitUI();
        }
    }
    public class UIJueXingShowComponentDestroy : DestroySystem<UIJueXingShowComponent>
    {
        public override void Destroy(UIJueXingShowComponent self)
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
    public static class UIJueXingShowComponentSystem
    {
        public static void OnInitUI(this UIJueXingShowComponent self)
        {
            int occtweo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.OccTwo;
            if (occtweo == 0)
            {
                return;
            }

            OccupationTwoConfig occupationConfig = OccupationTwoConfigCategory.Instance.Get( occtweo );
            int[] juexingids = occupationConfig.JueXingSkill;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            for (int i = 0; i < juexingids.Length; i++)
            {
                GameObject juexingitem = rc.Get<GameObject>($"SkillItem_{i}");
                UIJueXingShowItemComponent uIJueXingShowItem = self.AddChild<UIJueXingShowItemComponent, GameObject>(juexingitem);
                uIJueXingShowItem.OnInitUI( self.OnClickHandler, juexingids[i]);
                self.UIJueXingShowItems.Add(uIJueXingShowItem);
            }

            self.UIJueXingShowItems[0].OnClickImageIcon();
        }

        public static async ETTask OnButtonActive(this UIJueXingShowComponent self)
        {
            if (self.JueXingId == 0)
            {
                return;
            }
            long instanceid = self.InstanceId;
            C2M_SkillJueXingRequest request = new C2M_SkillJueXingRequest() { JueXingId = self.JueXingId };
            M2C_SkillJueXingResponse response = (M2C_SkillJueXingResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            if (instanceid != self.InstanceId || response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            self.OnClickHandler( self.JueXingId );
            for (int i = 0; i < self.UIJueXingShowItems.Count; i++)
            {
                self.UIJueXingShowItems[i].OnUpdateUI();
            }

            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            unit.GetComponent<UIUnitHpComponent>()?.ShowJueXingAnger();
        }


        public static void OnClickHandler(this UIJueXingShowComponent self, int juexingid)
        {
            self.JueXingId = juexingid;
            for (int i = 0; i < self.UIJueXingShowItems.Count; i++)
            {
                self.UIJueXingShowItems[i].SetSelected(juexingid);  
            }

            OccupationJueXingConfig occupationJueXingConfig = OccupationJueXingConfigCategory.Instance.Get(juexingid);
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(juexingid);

            string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.RoleSkillIcon, skillConfig.SkillIcon);
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.ImageSkillIcon.GetComponent<Image>().sprite = sp;


            self.Text_Gold.GetComponent<Text>().text = $"消耗：{occupationJueXingConfig.costGold}金币";

            self.TextSkillName.GetComponentInChildren<Text>().text =skillConfig.SkillName;

            self.Text_11.GetComponent<Text>().text =skillConfig.SkillDescribe;

            Unit unit = UnitHelper.GetMyUnitFromZoneScene( self.ZoneScene() ) ;
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            int juexingexp = numericComponent.GetAsInt(NumericType.JueXingExp);
          
            float value = 1f * juexingexp / occupationJueXingConfig.costExp;
            value = Math.Max(value, 1f);

            self.Text_JueXingExp.GetComponent<Text>().text = $"{juexingexp}/{occupationJueXingConfig.costExp}";
            self.ImageJueXingExp.GetComponent<Image>().fillAmount = Math.Min( value, 1f );

            string[] costitem =  occupationJueXingConfig.costItem.Split(';');
            self.UICommonCostItem.UpdateItem(int.Parse(costitem[0]), int.Parse(costitem[1]));

            SkillSetComponent skillSetComponent = self.ZoneScene().GetComponent<SkillSetComponent>();
            self.ButtonActive.SetActive(skillSetComponent.GetBySkillID(juexingid) == null);
        }

    }
}