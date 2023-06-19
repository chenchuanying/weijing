﻿using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ET
{
    public  class UIRoleXiLianSkillItemComponent : Entity, IAwake<GameObject>
    {
        public GameObject ItemNode;
        public GameObject Text_XiLianName;
        public GameObject GameObject;
        public GameObject JiHuoSet;
        public EquipXiLianConfig EquipXiLianConfig;
        public List<UICommonSkillItemComponent> uIItems = new List<UICommonSkillItemComponent>();
    }


    public class UIRoleXiLianSkillItemComponentAwakeSystem : AwakeSystem<UIRoleXiLianSkillItemComponent, GameObject>
    {
        public override void Awake(UIRoleXiLianSkillItemComponent self, GameObject go)
        {
            self.uIItems.Clear();

            self.GameObject = go;
            self.Text_XiLianName = go.Get<GameObject>("Text_XiLianName");
            self.ItemNode = go.Get<GameObject>("ItemNode");
            self.JiHuoSet = go.Get<GameObject>("JiHuoSet"); 
        }
    }

    public static class UIRoleXiLianSkillItemComponentSystem
    {
        public static void OnInitUI(this UIRoleXiLianSkillItemComponent self, EquipXiLianConfig equipXiLianConfig)
        {
            self.EquipXiLianConfig = equipXiLianConfig;
            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonSkillItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            self.Text_XiLianName.GetComponent<Text>().text = equipXiLianConfig.Title+GameSettingLanguge.LoadLocalization("额外增加概率出现的特殊属性");
            List<KeyValuePairInt> xilianSkill = XiLianHelper.GetLevelSkill(equipXiLianConfig.XiLianLevel);

            int row = (xilianSkill.Count / 8);
            row += (xilianSkill.Count % 8 > 0 ? 1 : 0);
            self.GameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(1400f, 100f + row * 170f);

            for (int i = 0; i < xilianSkill.Count; i++)
            {
                GameObject bagSpace = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(bagSpace, self.ItemNode);
                UICommonSkillItemComponent ui_item = self.AddChild<UICommonSkillItemComponent, GameObject>(bagSpace);
                ui_item.OnUpdateUI((int)xilianSkill[i].Value, ABAtlasTypes.RoleSkillIcon, ItemViewHelp.XiLianWeiZhiTip(xilianSkill[i].KeyId ));

                Log.Info("xilianSkill[i] = " + xilianSkill[i]);
                SkillConfig skillcof = SkillConfigCategory.Instance.Get((int)xilianSkill[i].Value);
                ui_item.TextSkillName.GetComponent<Text>().text = skillcof.SkillName;
                ui_item.TextSkillName.SetActive(true);
                self.uIItems.Add(ui_item);
            }
        }

        public static void OnUpdateUI(this UIRoleXiLianSkillItemComponent self, int xilianlv)
        {
            bool gray = xilianlv < self.EquipXiLianConfig.XiLianLevel;
            for (int i = 0; i < self.uIItems.Count; i++)
            {
                UICommonHelper.SetImageGray(self.uIItems[i].ImageIcon, gray);
            }
            if (gray)
            {
                self.JiHuoSet.SetActive(false);
            }
        }
    }
}
