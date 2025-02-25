﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIBuffTipsComponent : Entity, IAwake, IDestroy
    {
        public GameObject Lab_Spellcaster;
        public GameObject PositionNode;
        public GameObject ImageButton;
        public GameObject Lab_SkillDes;
        public GameObject Lab_SkillName;
        public GameObject Image_SkillIcon;
        public GameObject Lab_BuffTime;
        public int BuffId;
        public List<string> AssetPath = new List<string>();
    }


    public class UIBuffTipsComponentAwakeSystem : AwakeSystem<UIBuffTipsComponent>
    {
        public override void Awake(UIBuffTipsComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.PositionNode = rc.Get<GameObject>("PositionNode");

            self.Lab_Spellcaster = rc.Get<GameObject>("Lab_Spellcaster");
            self.ImageButton = rc.Get<GameObject>("ImageButton");
            self.ImageButton.GetComponent<Button>().onClick.AddListener(() => { self.OnImageButton(); });
            self.ImageButton.SetActive(false);

            self.Lab_SkillDes = rc.Get<GameObject>("Lab_SkillDes");
            self.Lab_SkillName = rc.Get<GameObject>("Lab_SkillName");
            self.Image_SkillIcon = rc.Get<GameObject>("Image_SkillIcon");
            self.Lab_SkillDes = rc.Get<GameObject>("Lab_SkillDes");
            self.Lab_BuffTime = rc.Get<GameObject>("Lab_BuffTime");
        }
    }

    public class UIBuffTipsComponentDestroy : DestroySystem<UIBuffTipsComponent>
    {
        public override void Destroy(UIBuffTipsComponent self)
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
    
    public static class UIBuffTipsComponentSystem
    {
        public static void OnImageButton(this UIBuffTipsComponent self)
        {
            UIHelper.Remove(self.DomainScene(), UIType.UISkillTips);
        }

        public static void OnUpdateData(this UIBuffTipsComponent self, int buffid, Vector3 vector3,string showStr, string spellcast, string aBAtlasTypes, string bufficon)
        {
            self.BuffId = buffid;
            SkillBuffConfig skillBufConfig = SkillBuffConfigCategory.Instance.Get(buffid);

            string path =ABPathHelper.GetAtlasPath_2(aBAtlasTypes, bufficon);
            Sprite sprite = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.Image_SkillIcon.GetComponent<Image>().sprite = sprite;
        
            self.Lab_SkillName.GetComponent<Text>().text = skillBufConfig.BuffName;
            self.Lab_SkillDes.GetComponent<Text>().text = skillBufConfig.BuffDescribe;

            self.Lab_BuffTime.GetComponent<Text>().text = GameSettingLanguge.LoadLocalization("剩余时间") + ":" + showStr;

            self.PositionNode.transform.localPosition = vector3 + new Vector3(100, 0f, 0f);
            self.Lab_Spellcaster.GetComponent<Text>().text = $"施法者：{spellcast}";
        }

    }

}
