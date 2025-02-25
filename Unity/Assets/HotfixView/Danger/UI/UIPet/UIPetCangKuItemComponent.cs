using ET;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIPetCangKuItemComponent : Entity, IAwake<GameObject>,IDestroy
    {
        public GameObject GameObject;
        public GameObject ButtonPut;
        public GameObject Lab_PinFen;
        public GameObject Lab_PetLv;
        public GameObject Lab_PetName;
        public GameObject Img_PetHeroIon;

        public RolePetInfo RolePetInfo;

        public Action PetCangKuAction;
        public int Index;
        
        public List<string> AssetPath = new List<string>();
    }

    public class UIPetCangKuItemComponentAwake : AwakeSystem<UIPetCangKuItemComponent, GameObject>
    {
        public override void Awake(UIPetCangKuItemComponent self, GameObject go)
        {
            self.GameObject = go;

            ReferenceCollector rc = go.GetComponent<ReferenceCollector>();
            self.ButtonPut = rc.Get<GameObject>("ButtonPut");
            self.Lab_PinFen = rc.Get<GameObject>("Lab_PinFen");
            self.Lab_PetLv = rc.Get<GameObject>("Lab_PetLv");
            self.Lab_PetName = rc.Get<GameObject>("Lab_PetName");
            self.Img_PetHeroIon = rc.Get<GameObject>("Img_PetHeroIon");

            ButtonHelp.AddListenerEx(self.ButtonPut, () => { self.OnButtonPut().Coroutine(); });
        }
    }
    public class UIPetCangKuItemComponentDestroy : DestroySystem<UIPetCangKuItemComponent>
    {
        public override void Destroy(UIPetCangKuItemComponent self)
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
    public static class UIPetCangKuItemComponentSystem
    {

        public static void SetAction(this UIPetCangKuItemComponent self, Action action)
        { 
            self.PetCangKuAction = action;  
        }

        public static void OnUpdateUI(this UIPetCangKuItemComponent self, RolePetInfo rolePetInfo)
        {
            self.RolePetInfo = rolePetInfo;
            self.Lab_PetName.GetComponent<Text>().text = rolePetInfo.PetName;

            PetConfig petConfig = PetConfigCategory.Instance.Get(rolePetInfo.ConfigId);
            PetSkinConfig petSkinConfig = PetSkinConfigCategory.Instance.Get(rolePetInfo.SkinId);
            string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.PetHeadIcon, petSkinConfig.IconID.ToString());
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.Img_PetHeroIon.GetComponent<Image>().sprite = sp;
        }

        public static async ETTask OnButtonPut(this UIPetCangKuItemComponent self)
        {
            C2M_PetPutCangKu c2M_PetPutCangKu = new C2M_PetPutCangKu() {   PetInfoId = self.RolePetInfo.Id, PetStatus = 3 };
            M2C_PetPutCangKu m2C_PetPutCangKu = (M2C_PetPutCangKu)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(c2M_PetPutCangKu);
            if (m2C_PetPutCangKu.Error != 0)
            {
                return;
            }
            self.ZoneScene().GetComponent<PetComponent>().GetPetInfoByID(self.RolePetInfo.Id).PetStatus = 3;
            self.PetCangKuAction?.Invoke();
        }
    }
}