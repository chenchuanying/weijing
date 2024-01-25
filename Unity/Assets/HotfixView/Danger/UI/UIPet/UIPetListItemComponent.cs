﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    public class UIPetListItemComponent: Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject Image_Protect;
        public GameObject Lab_Status;
        public GameObject Reddot;
        public GameObject Node_2;
        public GameObject Node_1;
        public GameObject Text_Open;
        public GameObject ImageDiButton;
        public GameObject ImageDiEventTrigger;
        public GameObject Img_CanZhan;
        public GameObject Lab_PetLv;
        public GameObject Lab_PetName;
        public GameObject Img_PetHeroIon;
        public GameObject Img_PeteroQuality;
        public GameObject ImageXuanzhong;
        public GameObject Img_Start_1;
        public GameObject Img_Start_2;
        public GameObject Img_Start_3;
        public GameObject Img_Start_4;
        public GameObject Img_Start_5;
        public GameObject Lab_StartShow;
        public GameObject StartShowSet;
        public GameObject StartSet;
        public GameObject Lab_PetQuality;
        public GameObject GameObject;

        public long PetId;
        public Action<long> ClickPetHandler;
        public List<string> AssetPath = new List<string>();
    }

    public class UIPetListItemComponentAwakeSystem: AwakeSystem<UIPetListItemComponent, GameObject>
    {
        public override void Awake(UIPetListItemComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.Node_2 = rc.Get<GameObject>("Node_2");
            self.Node_1 = rc.Get<GameObject>("Node_1");
            self.Text_Open = rc.Get<GameObject>("Text_Open");
            self.Reddot = rc.Get<GameObject>("Reddot");

            self.Img_CanZhan = rc.Get<GameObject>("Img_CanZhan");
            self.Lab_PetLv = rc.Get<GameObject>("Lab_PetLv");
            self.Lab_PetName = rc.Get<GameObject>("Lab_PetName");
            self.Img_PetHeroIon = rc.Get<GameObject>("Img_PetHeroIon");
            self.Img_PeteroQuality = rc.Get<GameObject>("Img_PeteroQuality");
            self.ImageXuanzhong = rc.Get<GameObject>("ImageXuanzhong");
            self.ImageXuanzhong.SetActive(false);
            self.Lab_Status = rc.Get<GameObject>("Lab_Status");

            self.Img_Start_1 = rc.Get<GameObject>("Img_Start_1");
            self.Img_Start_2 = rc.Get<GameObject>("Img_Start_2");
            self.Img_Start_3 = rc.Get<GameObject>("Img_Start_3");
            self.Img_Start_4 = rc.Get<GameObject>("Img_Start_4");
            self.Img_Start_5 = rc.Get<GameObject>("Img_Start_5");

            self.Lab_StartShow = rc.Get<GameObject>("Lab_StartShow");
            self.StartShowSet = rc.Get<GameObject>("StartShowSet");
            self.StartSet = rc.Get<GameObject>("StartSet");
            self.Lab_PetQuality = rc.Get<GameObject>("Lab_PetQuality");

            self.Image_Protect = rc.Get<GameObject>("Image_Protect");
            self.Image_Protect?.SetActive(false);

            self.ImageDiButton = rc.Get<GameObject>("ImageDiButton");
            self.ImageDiButton.GetComponent<Button>().onClick.AddListener(() => { self.OnClickPetItem(); });

            self.ImageDiEventTrigger = rc.Get<GameObject>("ImageDiEventTrigger");
            self.ImageDiEventTrigger.SetActive(false);
        }
    }

    public class UIPetListItemComponentDestroy: DestroySystem<UIPetListItemComponent>
    {
        public override void Destroy(UIPetListItemComponent self)
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

    public static class UIPetListItemComponentSystem
    {
        public static void OnClickPetItem(this UIPetListItemComponent self)
        {
            self.ClickPetHandler(self.PetId);
        }

        public static void OnSelectUI(this UIPetListItemComponent self, RolePetInfo rolePetInfo)
        {
            self.ImageXuanzhong.SetActive(self.PetId == rolePetInfo.Id);
        }

        public static void OnUpdatePetPoint(this UIPetListItemComponent self, RolePetInfo rolePetInfo)
        {
            if (rolePetInfo == null)
            {
                return;
            }

            if (rolePetInfo.Id != self.PetId)
            {
                return;
            }

            self.PetId = rolePetInfo.Id;
            self.Reddot.SetActive(rolePetInfo.AddPropretyNum > 0);
        }

        public static void OnRName(this UIPetListItemComponent self, RolePetInfo rolePetInfo)
        {
            self.Lab_PetName.GetComponent<Text>().text = rolePetInfo.PetName;
        }

        public static void OnPetFightingSet(this UIPetListItemComponent self, RolePetInfo rolePetInfo)
        {
            self.Img_CanZhan.SetActive(rolePetInfo != null && self.PetId == rolePetInfo.Id);
        }

        public static void OnPetProtectSet(this UIPetListItemComponent self, RolePetInfo rolePetInfo)
        {
            self.Image_Protect.SetActive(rolePetInfo != null && self.PetId == rolePetInfo.Id);
        }

        public static void SetClickHandler(this UIPetListItemComponent self, Action<long> action)
        {
            self.ClickPetHandler = action;
        }

        public static void OnInitData(this UIPetListItemComponent self, RolePetInfo rolePetInfo, int nextLv)
        {
            if (rolePetInfo != null)
            {
                self.PetId = rolePetInfo.Id;
            }
            else
            {
                self.PetId = 0;
            }

            self.Node_1.SetActive(rolePetInfo != null);
            self.Node_2.SetActive(rolePetInfo == null);
            if (rolePetInfo != null)
            {
                self.OnUpdatePetPoint(rolePetInfo);
                PetConfig petConfig = PetConfigCategory.Instance.Get(rolePetInfo.ConfigId);
                PetSkinConfig petSkinConfig = PetSkinConfigCategory.Instance.Get(rolePetInfo.SkinId);
                string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.PetHeadIcon, petSkinConfig.IconID.ToString());
                Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
                if (!self.AssetPath.Contains(path))
                {
                    self.AssetPath.Add(path);
                }

                self.Img_PetHeroIon.GetComponent<Image>().sprite = sp;

                self.Img_CanZhan.SetActive(rolePetInfo.PetStatus == 1);
                self.Lab_PetName.GetComponent<Text>().text = rolePetInfo.PetName;
                self.Lab_PetLv.GetComponent<Text>().text = rolePetInfo.PetLv.ToString() + GameSettingLanguge.LoadLocalization("级");

                self.Lab_PetQuality.GetComponent<Text>().text = UICommonHelper.GetPetQualityName(petConfig.PetQuality);
                self.Lab_PetQuality.GetComponent<Text>().color = UICommonHelper.QualityReturnColor(petConfig.PetQuality);

                self.Lab_Status.GetComponent<Text>().text = rolePetInfo.PetStatus == 2? "散步中..." : String.Empty;

                self.Image_Protect?.SetActive(rolePetInfo.IsProtect);
            }
            else
            {
                self.Text_Open.GetComponent<Text>().text = $"{nextLv}级开启";
            }
        }

        public static void StartShowImg(this UIPetListItemComponent self, GameObject startObj)
        {
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.OtherIcon, "Start_2");
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }

            startObj.GetComponent<Image>().sprite = sp;
        }
    }
}