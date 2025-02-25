﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ET
{
    public class UISkillTipsComponent : Entity, IAwake,IDestroy
    {
        public GameObject TextTip2;
        public GameObject UnActiveTip;
        public GameObject PositionNode;
        public GameObject ImageButton;
        public GameObject Lab_SkillDes;
        public GameObject Lab_SkillName;
        public GameObject Image_SkillIcon;
        public GameObject Lab_SkillType;
        public List<string> AssetPath = new List<string>();
    }


    public class UISkillTipsComponentAwakeSystem : AwakeSystem<UISkillTipsComponent>
    {
        public override void Awake(UISkillTipsComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.PositionNode = rc.Get<GameObject>("PositionNode");
            self.TextTip2 = rc.Get<GameObject>("TextTip2");
            self.UnActiveTip = rc.Get<GameObject>("UnActiveTip");

            self.ImageButton = rc.Get<GameObject>("ImageButton");
            self.ImageButton.GetComponent<Button>().onClick.AddListener(() => { self.OnImageButton(); });

            self.Lab_SkillDes = rc.Get<GameObject>("Lab_SkillDes");
            self.Lab_SkillName = rc.Get<GameObject>("Lab_SkillName");
            self.Image_SkillIcon = rc.Get<GameObject>("Image_SkillIcon");
            self.Lab_SkillType = rc.Get<GameObject>("Lab_SkillType");
        }

    }
    public class UISkillTipsComponentDestroy: DestroySystem<UISkillTipsComponent>
    {
        public override void Destroy(UISkillTipsComponent self)
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
    public static class UISkillTipsComponentSystem
    {
        public static void OnImageButton(this UISkillTipsComponent self)
        {
            UIHelper.Remove( self.DomainScene(), UIType.UISkillTips );
        }

        public static void ShowUnActive(this UISkillTipsComponent self, int skillId, int skillNum)
        {
            self.UnActiveTip.SetActive(true);

            int hideId = HideProListConfigCategory.Instance.PetSkillToHideProId[skillId];
            HideProListConfig hideProListConfig = HideProListConfigCategory.Instance.Get(hideId);
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);
            self.Lab_SkillName.GetComponent<Text>().text = skillConfig.SkillName + $"{skillNum}/{hideProListConfig.NeedNumber}";
            self.TextTip2.GetComponent<Text>().text = $"套装技能穿戴{hideProListConfig.NeedNumber}个时激活此技能";
        }

        public static void OnUpdateData(this UISkillTipsComponent self, int skillId, Vector3 vector3, string aBAtlasTypes = ABAtlasTypes.RoleSkillIcon, string addTip = "")
        {
            if (!SkillConfigCategory.Instance.Contain(skillId))
            {
                ///可能是道具
                return;
            }

            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);
            string path =ABPathHelper.GetAtlasPath_2(aBAtlasTypes, skillConfig.SkillIcon);
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.Image_SkillIcon.GetComponent<Image>().sprite = sp;

            self.Lab_SkillName.GetComponent<Text>().text = skillConfig.SkillName;
            self.Lab_SkillDes.GetComponent<Text>().text = skillConfig.SkillDescribe + addTip;

            if (skillConfig.SkillType == 1)
            {
                self.Lab_SkillType.GetComponent<Text>().text = GameSettingLanguge.LoadLocalization("类型：主动技能");
            }
            else
            {
                self.Lab_SkillType.GetComponent<Text>().text = GameSettingLanguge.LoadLocalization("类型：被动技能");
            }


            if (vector3.x > UnityEngine.Screen.width * -0.5 + 500)
            {
                self.PositionNode.transform.localPosition = vector3 + new Vector3(-50f, 50f, 0f);
            }
            else
            {
                self.PositionNode.transform.localPosition = vector3 + new Vector3(450f, 50f, 0f);
            }

            self.UnActiveTip.SetActive(false);
        }

    }

}

