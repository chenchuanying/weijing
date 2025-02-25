﻿using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;


namespace ET
{
    public class UIEquipSetComponent : Entity, IAwake, IAwake<GameObject, int>
    {

        public GameObject RawImage;
        public UIModelShowComponent UIModelShowComponent;

        public List<UIEquipSetItemComponent> EquipList = new List<UIEquipSetItemComponent>();
        public List<UIEquipSetItemComponent> EquipList_2 = new List<UIEquipSetItemComponent>();

        public List<BagInfo> EquipInfoList = new List<BagInfo>();
        public ItemOperateEnum ItemOperateEnum;
        public GameObject GameObject;

        public int Position;
        public int Index;
        public int Occ;
    }


    public class UIEquipSetComponentAwakeSystem : AwakeSystem<UIEquipSetComponent, GameObject, int>
    {
        public override void Awake(UIEquipSetComponent self,GameObject gameObject, int index)
        {
            self.GameObject = gameObject;   
            self.EquipInfoList.Clear();
            self.EquipList.Clear();
            self.EquipList_2.Clear();   

            int occ = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Occ;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            for (int i = 1; i <= 13; i++)
            {
                GameObject go = gameObject.transform.Find("Equip_" + i).gameObject;
                UIEquipSetItemComponent uiitem = self.AddChild<UIEquipSetItemComponent, GameObject>(go);
                self.EquipList.Add(uiitem);
            }

            for (int i = 1; i <= 1; i++)
            {
                GameObject go = gameObject.transform.Find("Equip_2_" + i).gameObject;
                UIEquipSetItemComponent uiitem = self.AddChild<UIEquipSetItemComponent, GameObject>(go);
                self.EquipList_2.Add(uiitem);
                go.SetActive(occ == 3);
            }

            if (!GlobalHelp.IsBanHaoMode)
            {
                gameObject.transform.Find("Equip_6").gameObject.SetActive(true);
                gameObject.transform.Find("Equip_7").gameObject.SetActive(true);
            }

            self.RawImage = gameObject.transform.Find("EquipSetHide/RawImage").gameObject;
            self.RawImage.SetActive(false);
            self.UIModelShowComponent = null;
            self.Index = index;
            self.Position = index;
        }
    }

    public static class UIEquipSetComponentSystem
    {
        public static void SetCallBack(this UIEquipSetComponent self, Action<BagInfo> action)
        {
            foreach (UIEquipSetItemComponent uiEquipSetItemComponent in self.EquipList)
            {
                uiEquipSetItemComponent.OnClickAction = action;
            }

            foreach (UIEquipSetItemComponent uiEquipSetItemComponent in self.EquipList_2)
            {
                uiEquipSetItemComponent.OnClickAction = action;
            }
        }

        public static  void InitModelShowView(this UIEquipSetComponent self, int index)
        {
            //模型展示界面
            var path = ABPathHelper.GetUGUIPath("Common/UIModelShow" + (index+1).ToString());
            GameObject bundleGameObject =  ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UICommonHelper.SetParent(gameObject, self.RawImage);
            gameObject.transform.localPosition = new Vector3(self.Position * 2000, 4000, 0);
            gameObject.transform.Find("Camera").localPosition = new Vector3(0f, 70f, 150f);

            UI ui = self.AddChild<UI, string, GameObject>( "UIModelShow", gameObject);
            self.UIModelShowComponent = ui.AddComponent<UIModelShowComponent, GameObject>(self.RawImage);
            self.RawImage.SetActive(true);
        }

        public static  void PlayShowIdelAnimate(this UIEquipSetComponent self, BagInfo bagInfo)
        {
            if (self.UIModelShowComponent == null)
            {
               self.InitModelShowView(self.Index);
            }
            self.UIModelShowComponent.PlayShowIdelAnimate();
        }

        public static void  ShowPlayerModel(this UIEquipSetComponent self, BagInfo bagInfo, int occ, int equipIndex, List<int> fashonids)
        {
            if (self.UIModelShowComponent == null)
            {
                self.InitModelShowView(self.Index);
            }
            self.UIModelShowComponent.ShowPlayerModel(bagInfo, occ, equipIndex, fashonids);
        }

        public static  void ChangeWeapon(this UIEquipSetComponent self, BagInfo bagInfo, int occ)
        {
            if (self.UIModelShowComponent == null)
            {
                self.InitModelShowView(self.Index);
            }
            self.UIModelShowComponent.ChangeWeapon(bagInfo, occ);
        }

        public static void ResetEquipShow(this UIEquipSetComponent self)
        {
            for (int i = 0; i < self.EquipList.Count; i++)
            {
                self.EquipList[i].InitUI(FunctionUI.GetItemSubtypeByWeizhi(i));
            }
            for (int i = 0; i < self.EquipList_2.Count; i++)
            {
                self.EquipList_2[i].InitUI(FunctionUI.GetItemSubtypeByWeizhi(i));
            }
        }

        public static void EquipSetHide(this UIEquipSetComponent self, bool value)
        {
            self.GameObject.transform.Find("EquipSetHide").gameObject.SetActive(value);
        }

        public static void PlayerLv(this UIEquipSetComponent self, int lv)
        {
            self.GameObject.transform.Find("EquipSetHide/RoseNameLv/Lab_RoseLv").GetComponent<Text>().text = lv.ToString();
        }

        public static void PlayerName(this UIEquipSetComponent self, string playerName)
        {
            self.GameObject.transform.Find("EquipSetHide/RoseNameLv/Lab_RoseName").GetComponent<Text>().text = playerName;
        }

        public static void UpdateBagUI_2(this UIEquipSetComponent self, List<BagInfo> equiplist, int occ, ItemOperateEnum itemOperateEnum)
        {
            for (int i = 0; i < equiplist.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(equiplist[i].ItemID);
                if (itemConfig.EquipType == 101)
                {
                    continue;
                }

                self.EquipList_2[itemConfig.ItemSubType - 1].UpdateData(equiplist[i], occ, itemOperateEnum, equiplist);
            }
        }

        public static void UpdateBagUI(this UIEquipSetComponent self, List<BagInfo> equiplist, int occ, ItemOperateEnum itemOperateEnum)
        {
            self.ResetEquipShow();

            int shipingIndex = 0;
            self.Occ = occ;
            self.EquipInfoList = equiplist;
            self.ItemOperateEnum = itemOperateEnum;
            for (int i = 0; i < equiplist.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(equiplist[i].ItemID);
                if (itemConfig.EquipType == 101 || itemConfig.EquipType == 201)
                {
                    continue;
                }
                if (itemConfig.ItemType == 4)
                {
                    continue;
                }

                if (itemConfig.ItemSubType < (int)ItemSubTypeEnum.Shiping)
                {
                    self.EquipList[itemConfig.ItemSubType - 1].UpdateData(equiplist[i], occ, itemOperateEnum, equiplist);
                }
                if (itemConfig.ItemSubType == (int)ItemSubTypeEnum.Shiping)
                {
                    self.EquipList[itemConfig.ItemSubType + shipingIndex - 1].UpdateData(equiplist[i], occ, itemOperateEnum, equiplist);
                    shipingIndex++;
                }
                if (itemConfig.ItemSubType > (int)ItemSubTypeEnum.Shiping)
                {
                    self.EquipList[itemConfig.ItemSubType + 1].UpdateData(equiplist[i], occ, itemOperateEnum, equiplist);
                }
            }


            UI uI = UIHelper.GetUI(self.ZoneScene(), UIType.UIRoleZodiac);
            if (uI != null)
            {
                uI.GetComponent<UIRoleZodiacComponent>().UpdateBagUI(self.EquipInfoList, self.Occ, self.ItemOperateEnum);
            }
        }
    }

}
