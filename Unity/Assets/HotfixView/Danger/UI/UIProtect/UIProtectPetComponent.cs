﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIProtectPetComponent : Entity, IAwake,IDestroy
    {

        public GameObject PetIcon;
        public GameObject Text_Name;
        public GameObject PetListNode;
        public GameObject UnlockButton;
        public GameObject XiLianButton;

        public PetComponent PetComponent;
        public List<UIPetListItemComponent> PetUIList = new List<UIPetListItemComponent>();

        public long PetInfoId;
        
        public List<string> AssetPath = new List<string>();
    }

    public class UIProtectPetComponentAwake : AwakeSystem<UIProtectPetComponent>
    {
        public override void Awake(UIProtectPetComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.PetListNode    = rc.Get<GameObject>("EquipListNode");

            self.UnlockButton   = rc.Get<GameObject>("UnlockButton");
            ButtonHelp.AddListenerEx(self.UnlockButton, () => { self.RequestProtect(false).Coroutine();   });

            self.XiLianButton   = rc.Get<GameObject>("XiLianButton");
            ButtonHelp.AddListenerEx(self.XiLianButton, () => { self.RequestProtect(true).Coroutine(); });

            self.PetIcon         = rc.Get<GameObject>("PetIcon");
            self.Text_Name      = rc.Get<GameObject>("Text_Name");

            self.PetComponent = self.ZoneScene().GetComponent<PetComponent>();
            self.GetParent<UI>().OnUpdateUI = self.OnUpdateUI;
        }
    }
    public class UIProtectPetComponentDestroy: DestroySystem<UIProtectPetComponent>
    {
        public override void Destroy(UIProtectPetComponent self)
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
    public static class UIProtectPetComponentSystem
    {

        public static void OnUpdateUI(this UIProtectPetComponent self)
        {
            self.PetInfoId = 0;
            self.OnInitPetList();
            if (self.PetUIList.Count > 0)
            {
                self.PetUIList[0].OnClickPetItem();
            }
        }

        public static void OnClickPetHandler(this UIProtectPetComponent self, long petId)
        {
            RolePetInfo rolePetInfo = self.PetComponent.GetPetInfoByID(petId);
            if (rolePetInfo == null)
            {
                return;
            }
            for (int i = 0; i < self.PetUIList.Count; i++)
            {
                self.PetUIList[i].OnSelectUI(rolePetInfo);
            }
            self.PetInfoId = petId;
            self.XiLianButton.SetActive(!rolePetInfo.IsProtect);
            self.UnlockButton.SetActive(rolePetInfo.IsProtect);
            self.Text_Name.GetComponent<Text>().text = rolePetInfo.PetName;
            PetSkinConfig petSkinConfig = PetSkinConfigCategory.Instance.Get(rolePetInfo.SkinId);
            string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.PetHeadIcon, petSkinConfig.IconID.ToString());
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.PetIcon.GetComponent<Image>().sprite = sp;
        }

        public static async ETTask RequestProtect(this UIProtectPetComponent self, bool isprotectd)
        {
            C2M_RolePetProtect c2M_RolePetProtect = new C2M_RolePetProtect() { PetInfoId = self.PetInfoId, IsProtect = isprotectd };
            M2C_RolePetProtect m2C_RolePetProtect = (M2C_RolePetProtect)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(c2M_RolePetProtect);

            if (self.IsDisposed)
            {
                return;
            }
            string tip = isprotectd ? "锁定" : "解锁";
            FloatTipManager.Instance.ShowFloatTip($"宠物{tip}成功");
            self.PetComponent.OnPetProtect( self.PetInfoId, isprotectd );
            self.OnInitPetList();
            self.OnClickPetHandler(self.PetInfoId);
        }

        public static  void OnInitPetList(this UIProtectPetComponent self)
        {
            var path = ABPathHelper.GetUGUIPath("Main/Pet/UIPetListItem");
            var bundleGameObject =  ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            List<RolePetInfo> rolePetInfos = self.PetComponent.RolePetInfos;

            List<RolePetInfo> showList = new List<RolePetInfo>();
            showList.AddRange(rolePetInfos);
            for (int i = 0; i < showList.Count; i++)
            {
                UIPetListItemComponent ui_pet = null;
                if (i < self.PetUIList.Count)
                {
                    ui_pet = self.PetUIList[i];
                    ui_pet.GameObject.SetActive(true);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(bundleGameObject);
                    UICommonHelper.SetParent(go, self.PetListNode);
                    ui_pet = self.AddChild<UIPetListItemComponent, GameObject>(go);
                    ui_pet.SetClickHandler((long petId) => { self.OnClickPetHandler(petId); });
                    self.PetUIList.Add(ui_pet);
                }
                ui_pet.OnInitData(showList[i], 0);
            }

            for (int i = showList.Count; i < self.PetUIList.Count; i++)
            {
                self.PetUIList[i].GameObject.SetActive(false);
            }
        }

    }
}
