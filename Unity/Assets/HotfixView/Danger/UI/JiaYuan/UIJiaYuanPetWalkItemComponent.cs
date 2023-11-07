﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIJiaYuanPetWalkItemComponent : Entity, IAwake<GameObject>,IDestroy
    {
        public GameObject Image_Lock;
        public GameObject Button_Add;
        public GameObject Button_Walk;
        public GameObject Text_Tip_121;
        public GameObject Text_TotalExp;
        public GameObject Button_Stop;
        public GameObject Text_TotalExpHour;

        public GameObject[] ImageMood_List = new GameObject[5];

        public GameObject Text_Level;
        public GameObject Text_Name;
        public GameObject ImagePetIcon;
        public GameObject Set;
        public GameObject OpenLv;

        public RolePetInfo RolePetInfo;

        public int Position;
        public Action<int> ClickAddHandler;
        public Action ClickStopHandler;
        public List<string> AssetPath = new List<string>();
    }

    public class UIJiaYuanPetWalkItemComponentA : AwakeSystem<UIJiaYuanPetWalkItemComponent, GameObject>
    {
        public override void Awake(UIJiaYuanPetWalkItemComponent self, GameObject a)
        {
            //ReferenceCollector rc = a.GetComponent<ReferenceCollector>();
            Transform transform = a.transform;
            self.Text_TotalExp = transform.Find("Set/Text_TotalExp").gameObject;
            self.Text_TotalExpHour = transform.Find("Set/Text_TotalExpHour").gameObject;

            self.Image_Lock = transform.Find("Image_Lock").gameObject;
            self.Set = transform.Find("Set").gameObject;
            self.OpenLv = transform.Find("Image_Lock/OpenLv").gameObject;

            self.Button_Stop = transform.Find("Set/Button_Stop").gameObject;
            ButtonHelp.AddListenerEx( self.Button_Stop, () => { self.OnButton_Stop().Coroutine();  } );

            self.Button_Walk = transform.Find("Set/Button_Walk").gameObject;
            self.Button_Walk.SetActive(false);

            self.Text_Tip_121 = transform.Find("Set/Text_Tip_121").gameObject;
            for (int i = 0; i < 5; i++)
            { 
                self.ImageMood_List[i] = transform.Find($"Set/ImageMood_{i}").gameObject;
            }

            self.Text_Level = transform.Find("Set/Text_Level").gameObject;
            self.Text_Name = transform.Find("Set/Text_Name").gameObject;
            self.ImagePetIcon = transform.Find("Set/ImagePetIcon").gameObject;

            self.Button_Add = transform.Find("Set/Button_Add").gameObject;
            self.Button_Add.GetComponent<Button>().onClick.AddListener(() => { self.OnButton_Add().Coroutine(); });
        }
    }
    public class UIJiaYuanPetWalkItemComponentDestroy : DestroySystem<UIJiaYuanPetWalkItemComponent>
    {
        public override void Destroy(UIJiaYuanPetWalkItemComponent self)
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
    public static class UIJiaYuanPetWalkItemComponentSystem
    {

        public static void SetClickAddHandler(this UIJiaYuanPetWalkItemComponent self, Action<int> action)
        {
            self.ClickAddHandler = action;
        }

        public static void SetClickStopHandler(this UIJiaYuanPetWalkItemComponent self, Action action)
        {
            self.ClickStopHandler = action;
        }

        public static async ETTask OnButton_Add(this UIJiaYuanPetWalkItemComponent self)
        {
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            JiaYuanConfig jiayuanCof = JiaYuanConfigCategory.Instance.Get(userInfoComponent.UserInfo.JiaYuanLv);

            if (self.Position == 1 && jiayuanCof.Lv < 10)
            {
                FloatTipManager.Instance.ShowFloatTip("10级开启！");
                return;
            }
            if (self.Position == 2 && jiayuanCof.Lv < 20)
            {
                FloatTipManager.Instance.ShowFloatTip("20级开启！");
                return;
            }

            UI ui = await UIHelper.Create(self.DomainScene(), UIType.UIPetSelect);
            ui.GetComponent<UIPetSelectComponent>().OnSetType(PetOperationType.JiaYuan_Walk);
            self.ClickAddHandler?.Invoke(self.Position);
        }

        public static async ETTask OnButton_Stop(this UIJiaYuanPetWalkItemComponent self)
        {
            long instanceid = self.InstanceId;
            C2M_JiaYuanPetWalkRequest request = new C2M_JiaYuanPetWalkRequest() { PetStatus = 0, PetId = self.RolePetInfo.Id };
            M2C_JiaYuanPetWalkResponse response = (M2C_JiaYuanPetWalkResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            if (instanceid != self.InstanceId)
            {
                return;
            }
            
            self.ZoneScene().GetComponent<JiaYuanComponent>().JiaYuanPetList_2 = response.JiaYuanPetList;
            self.ClickStopHandler?.Invoke();
        }

        public static void OnUpdateUI(this UIJiaYuanPetWalkItemComponent self, RolePetInfo rolePetInfo, JiaYuanPet jiaYuanPet)
        {
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            JiaYuanConfig jiayuanCof = JiaYuanConfigCategory.Instance.Get(userInfoComponent.UserInfo.JiaYuanLv);
            ;
            if (self.Position == 0)
            {
                self.Image_Lock.SetActive(false);
            }
            if (self.Position == 1)
            {
                self.Image_Lock.SetActive(jiayuanCof.Lv < 10);
                self.Set.SetActive(!(jiayuanCof.Lv < 10));
                self.OpenLv.GetComponent<Text>().text = "10级家园开启";
            }
            if (self.Position == 2)
            {
                self.Image_Lock.SetActive(jiayuanCof.Lv < 20);
                self.Set.SetActive(!(jiayuanCof.Lv < 20));
                self.OpenLv.GetComponent<Text>().text = "20级家园开启";
            }

            if (jiaYuanPet == null)
            {
                self.Button_Add.SetActive(true);

                self.Text_TotalExp.GetComponent<Text>().text = String.Empty;
                self.Button_Walk.SetActive(false);
                self.Button_Stop.SetActive(false);
            }
            else
            {
                self.Button_Add.SetActive(false);
                self.RolePetInfo = rolePetInfo;
                self.Text_TotalExp.GetComponent<Text>().text = $"{jiaYuanPet.CurExp}";

                for (int i = 0; i < self.ImageMood_List.Length; i++)
                {
                    self.ImageMood_List[i].SetActive(i < JiaYuanHelper.GetPetMoodStar(jiaYuanPet.MoodValue));
                }

                self.Text_Level.GetComponent<Text>().text = $"等级：{rolePetInfo.PetLv}";
                self.Text_Name.GetComponent<Text>().text = rolePetInfo.PetName;

                PetConfig petConfig = PetConfigCategory.Instance.Get(rolePetInfo.ConfigId);
                string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.PetHeadIcon, petConfig.HeadIcon);
                Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
                if (!self.AssetPath.Contains(path))
                {
                    self.AssetPath.Add(path);
                }
                self.ImagePetIcon.GetComponent<Image>().sprite = sp;

                long walkTime = jiaYuanPet.StartTime > 0 ? TimeHelper.ServerNow() - jiaYuanPet.StartTime : 0;
                self.Text_Tip_121.GetComponent<Text>().text = $"已经散步:{TimeHelper.ShowLeftTime(walkTime)}";

                self.Button_Walk.SetActive(self.RolePetInfo.PetStatus == 0);
                self.Button_Stop.SetActive(self.RolePetInfo.PetStatus == 2);

                self.Text_TotalExpHour.GetComponent<Text>().text = ComHelp.GetJiaYuanPetExp(rolePetInfo.PetLv, jiaYuanPet.MoodValue) + "/小时";
            }
        }

    }
}
