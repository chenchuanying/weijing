﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if !NOT_UNITY
#endif

namespace ET
{
    public class UIEquipSetItemComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject Img_EquipBack;
        public GameObject Img_EquipIcon;
        public GameObject Img_EquipQuality;
        public GameObject Btn_Equip;
        public GameObject Img_EquipBangDing;
        public List<BagInfo> EquipIdList = new List<BagInfo>();
        public GameObject GameObject;
        public BagInfo BagInfo;
        public int Occ;
        public Action<BagInfo> OnClickAction;

        public ItemOperateEnum itemOperateEnum = ItemOperateEnum.Juese;
        
        public List<string> AssetPath = new List<string>();
    }


    public class UIEquipSetItemComponentAwakeSystem : AwakeSystem<UIEquipSetItemComponent, GameObject>
    {
        public override void Awake(UIEquipSetItemComponent self, GameObject gameObject)
        {
            //ReferenceCollector rc = .GetComponent<ReferenceCollector>();
            self.GameObject = gameObject;
            Transform tf = gameObject.transform;
            self.Img_EquipIcon = tf.Find("Img_EquipIcon").gameObject;
            self.Btn_Equip = tf.Find("Btn_Equip").gameObject;
            self.Img_EquipQuality = tf.Find("Img_EquipQuality").gameObject;
            self.Img_EquipBangDing = tf.Find("Img_BangDing").gameObject;
            self.Img_EquipBack = tf.Find("Img_EquipBack").gameObject;
            self.Btn_Equip.GetComponent<Button>().onClick.AddListener(() => { self.OnClickEquip(); });
        }
    }
    public class UIEquipSetItemComponentDestroy : DestroySystem<UIEquipSetItemComponent>
    {
        public override void Destroy(UIEquipSetItemComponent self)
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
    public static class UIEquipSetItemComponentSystem
    {
        public static void OnClickEquip(this UIEquipSetItemComponent self)
        {
            if (self.BagInfo == null)
                return;

            if (self.OnClickAction != null)
            {
               self.OnClickAction.Invoke(self.BagInfo); 
               return;
            }
            
            EventType.ShowItemTips.Instance.ZoneScene = self.DomainScene();
            EventType.ShowItemTips.Instance.bagInfo = self.BagInfo;
            EventType.ShowItemTips.Instance.itemOperateEnum = self.itemOperateEnum;
            EventType.ShowItemTips.Instance.inputPoint = Input.mousePosition;
            EventType.ShowItemTips.Instance.Occ = self.Occ;
            EventType.ShowItemTips.Instance.EquipList = self.EquipIdList;
            Game.EventSystem.PublishClass(EventType.ShowItemTips.Instance);
        }

        public static void InitUI(this UIEquipSetItemComponent self, int subType)
        {
            self.BagInfo = null;

            self.Img_EquipIcon.SetActive(false);
            self.Img_EquipQuality.SetActive(false);
            self.Img_EquipBangDing.SetActive(false);

            if (subType < 100)
            {
                string qianghuaName = ItemViewHelp.EquipWeiZhiToName[subType].Icon;
                string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.OtherIcon, qianghuaName);
                Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
                if (!self.AssetPath.Contains(path))
                {
                    self.AssetPath.Add(path);
                }
                self.Img_EquipBack.GetComponent<Image>().sprite = sp;
            }
        }

        public static void UpdateData(this UIEquipSetItemComponent self, BagInfo bagInfo, int occ, ItemOperateEnum itemOperateEnum, List<BagInfo> equipIdList)
        {
            try
            {
                self.Occ = occ;
                self.BagInfo = bagInfo;
                self.itemOperateEnum = itemOperateEnum;
                self.EquipIdList = equipIdList;
                ItemConfig itemconfig = ItemConfigCategory.Instance.Get(bagInfo.ItemID);

                string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemconfig.Icon);
                Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
                if (!self.AssetPath.Contains(path))
                {
                    self.AssetPath.Add(path);
                }
                self.Img_EquipIcon.SetActive(true);
                self.Img_EquipIcon.GetComponent<Image>().sprite = sp;

                //设置品质
                string ItemQuality = FunctionUI.GetInstance().ItemQualiytoPath(itemconfig.ItemQuality);
                self.Img_EquipQuality.SetActive(true);
                string path2 = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemQualityIcon, ItemQuality);
                Sprite sp2 = ResourcesComponent.Instance.LoadAsset<Sprite>(path2);
                if (!self.AssetPath.Contains(path2))
                {
                    self.AssetPath.Add(path2);
                }
                self.Img_EquipQuality.GetComponent<Image>().sprite = sp2;

                //显示绑定
                if (bagInfo.isBinging)
                {
                    self.Img_EquipBangDing.SetActive(true);
                }
                else
                {
                    self.Img_EquipBangDing.SetActive(false);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}

