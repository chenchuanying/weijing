﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{
    public class UIPetFormationComponent : Entity, IAwake,IDestroy
    {
        public GameObject CloseButton;
        public GameObject ButtonChallenge;
        public GameObject TextNumber;
        public GameObject FormationNode;
        public GameObject ButtonConfirm;
        public GameObject PetListNode;

        public GameObject IconItemDrag;
        public UIPetFormationSetComponent UIPetFormationSet;
        public List<UIPetFormationItemComponent> uIPetFormations = new List<UIPetFormationItemComponent>();
        public List<long> PetTeamList = new List<long>() { };

        public Action SetHandler = null;
        public int SceneTypeEnum;
        
        public List<string> AssetPath = new List<string>();
    }


    public class UIPetFormationComponentAwakeSystem : AwakeSystem<UIPetFormationComponent>
    {
        public override void Awake(UIPetFormationComponent self)
        {
            self.SetHandler = null;
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.uIPetFormations.Clear();
            self.ButtonChallenge = rc.Get<GameObject>("ButtonChallenge");
            self.TextNumber = rc.Get<GameObject>("TextNumber");
            self.FormationNode = rc.Get<GameObject>("FormationNode");
            self.ButtonConfirm = rc.Get<GameObject>("ButtonConfirm");
            self.PetListNode = rc.Get<GameObject>("PetListNode");
            self.IconItemDrag = rc.Get<GameObject>("IconItemDrag");
            self.CloseButton = rc.Get<GameObject>("CloseButton");
            self.IconItemDrag.SetActive(false);
            self.PetTeamList.Clear();

            ButtonHelp.AddListenerEx( self.ButtonConfirm, () => { self.OnButtonConfirm().Coroutine(); } );
            ButtonHelp.AddListenerEx(self.ButtonChallenge, self.OnButtonChallenge);
            self.CloseButton.GetComponent<Button>().onClick.AddListener(() => 
            {
                self.SetHandler?.Invoke();
                UIHelper.Remove(self.ZoneScene(), UIType.UIPetFormation); 
            });
        }
    }
    public class UIPetFormationComponentDestroy : DestroySystem<UIPetFormationComponent>
    {
        public override void Destroy(UIPetFormationComponent self)
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
    public static class UIPetFormationComponentSystem
    {

        public static void OnUpdateNumber(this UIPetFormationComponent self,int sceneType)
        {
            int number = 0;
            List<long> pets = self.PetTeamList;
            for (int i = 0; i < pets.Count; i++)
            {
                number += (pets[i] != 0 ? 1 : 0);
            }
            self.TextNumber.GetComponent<Text>().text = $"阵容限制：{number}/5";
        }

        public static void UpdateFighting(this UIPetFormationComponent self, int sceneType)
        {
            List<long> pets = self.PetTeamList;
            for (int i = 0; i < self.uIPetFormations.Count; i++)
            {
                long petId = self.uIPetFormations[i].RolePetInfo.Id;
                self.uIPetFormations[i].SetFighting(pets.Contains(petId));
            }
        }

        public static async ETTask OnButtonConfirm(this UIPetFormationComponent self)
        {
            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            long instanceid = self.InstanceId;
            int errorCode = await petComponent.RequestPetFormationSet(self.SceneTypeEnum, self.PetTeamList, null);
            if (errorCode!= ErrorCode.ERR_Success || instanceid != self.InstanceId)
            {
                return;
            }
            self.OnUpdateNumber(self.SceneTypeEnum);
            self.UpdateFighting(self.SceneTypeEnum);
        }

        public static void OnButtonChallenge(this UIPetFormationComponent self)
        {
            Scene scene = self.ZoneScene();
            UIHelper.Remove(scene, UIType.UIPetFormation);
            if (self.SceneTypeEnum == SceneTypeEnum.PetDungeon)
            {
                UIHelper.Create(scene, UIType.UIPetSet).Coroutine();
                return;
            }
            if (self.SceneTypeEnum == SceneTypeEnum.PetTianTi)
            {
                return;
            }
        }

        public static void RequestFormationSet(this UIPetFormationComponent self, long rolePetInfoId, int index, int operateType)
        {
            UI ui = UIHelper.GetUI(self.ZoneScene(), UIType.UIPetFormation);
            ui.GetComponent<UIPetFormationComponent>().OnDragFormationSet(rolePetInfoId, index, operateType);
        }

        public static  void OnInitUI(this UIPetFormationComponent self, int sceneType, Action action)
        {
            self.SetHandler = action;
            self.SceneTypeEnum = sceneType;
            self.PetTeamList.AddRange(self.ZoneScene().GetComponent<PetComponent>().GetPetFormatList(sceneType));
            var path = ABPathHelper.GetUGUIPath("Main/PetSet/UIPetFormationSet");
            var bundleGameObject =  ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject go = GameObject.Instantiate(bundleGameObject);
            self.UIPetFormationSet = self.AddChild<UIPetFormationSetComponent, GameObject>(go);
            self.UIPetFormationSet.DragEndHandler = self.RequestFormationSet;
            self.UIPetFormationSet.OnUpdateFormation(self.SceneTypeEnum, self.PetTeamList, true);
            UICommonHelper.SetParent(go, self.FormationNode);

            self.OnUpdateNumber(sceneType);
            self.UpdateFighting(sceneType);
            self.OnInitPetList(sceneType).Coroutine();
        }

        /// <summary>
        /// 1 上阵 2 交换位置 3 下阵
        /// </summary>
        /// <param name="self"></param>
        /// <param name="rolePetInfoId"></param>
        /// <param name="index"></param>
        /// <param name="operateType"></param>
        public static void OnDragFormationSet(this UIPetFormationComponent self, long rolePetInfoId, int index, int operateType)
        {
            //如果是上阵并且之前在队伍中
            int number = 0;
            for (int i = 0; i < self.PetTeamList.Count; i++)
            {
                number += (self.PetTeamList[i] != 0 ? 1 : 0);
            }
            if (index != -1 && number >= 5 && self.PetTeamList[index] == 0 && operateType!=2)
            {
                FloatTipManager.Instance.ShowFloatTip("已达到上阵最大数量！");
                return;
            }

            //index == -1 下阵
            if (operateType == 1)
            {
                for (int i = 0; i < self.PetTeamList.Count; i++)
                {
                    if (self.PetTeamList[i] == rolePetInfoId && i != index)
                    {
                        self.PetTeamList[i] = 0;
                    }
                }
                self.PetTeamList[index] = rolePetInfoId;
            }
            if (operateType == 2)
            {
                int oldIndex = -1;
                for (int i = 0; i < self.PetTeamList.Count; i++)
                {
                    if (self.PetTeamList[i] == rolePetInfoId)
                    {
                        oldIndex = i;
                    }
                }
                self.PetTeamList[oldIndex] = self.PetTeamList[index];
                self.PetTeamList[index] = rolePetInfoId;
            }
            if (operateType == 3)
            {
                for (int i = 0; i < self.PetTeamList.Count; i++)
                {
                    if (self.PetTeamList[i] == rolePetInfoId)
                    {
                        self.PetTeamList[i] = 0;
                    }
                }
            }
            self.UIPetFormationSet.OnUpdateFormation(self.SceneTypeEnum, self.PetTeamList, true);

            self.OnUpdateNumber(self.SceneTypeEnum);
            self.UpdateFighting(self.SceneTypeEnum);
            self.OnInitPetList(self.SceneTypeEnum).Coroutine();
        }

        public static async ETTask OnInitPetList(this UIPetFormationComponent self, int sceneType)
        {
            long instanceId = self.InstanceId;
            var path = ABPathHelper.GetUGUIPath("Main/PetSet/UIPetFormationItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            if (instanceId != self.InstanceId)
            {
                return;
            }

            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            List<RolePetInfo> rolePetInfos = petComponent.RolePetInfos;
            List<long> pets = self.PetTeamList;
            for (int i = 0; i < rolePetInfos.Count; i++)
            {
                UIPetFormationItemComponent uIRolePetItemComponent = null;
                if (i < self.uIPetFormations.Count)
                {
                    uIRolePetItemComponent = self.uIPetFormations[i];
                }
                else
                {
                    GameObject go = GameObject.Instantiate(bundleGameObject);
                    UICommonHelper.SetParent(go, self.PetListNode);
                    uIRolePetItemComponent = self.AddChild<UIPetFormationItemComponent, GameObject>(go);
                    uIRolePetItemComponent.BeginDragHandler = (RolePetInfo binfo, PointerEventData pdata) => { self.BeginDrag(binfo, pdata); };
                    uIRolePetItemComponent.DragingHandler = (RolePetInfo binfo, PointerEventData pdata) => { self.Draging(binfo, pdata); };
                    uIRolePetItemComponent.EndDragHandler = (RolePetInfo binfo, PointerEventData pdata) => { self.EndDrag(binfo, pdata); };
                    self.uIPetFormations.Add(uIRolePetItemComponent);
                }
                uIRolePetItemComponent.OnInitUI(rolePetInfos[i], pets.Contains(rolePetInfos[i].Id));
            }
        }

        public static void BeginDrag(this UIPetFormationComponent self, RolePetInfo binfo, PointerEventData pdata)
        {
            self.IconItemDrag.SetActive(true);
            PetConfig petConfig = PetConfigCategory.Instance.Get(binfo.ConfigId);
            string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.PetHeadIcon, petConfig.HeadIcon);
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            GameObject icon = self.IconItemDrag.transform.Find("ImageIcon").gameObject;
            icon.GetComponent<Image>().sprite = sp;
            UICommonHelper.SetParent(self.IconItemDrag, UIEventComponent.Instance.UILayers[(int)UILayer.Mid].gameObject);
        }

        public static void Draging(this UIPetFormationComponent self, RolePetInfo binfo, PointerEventData pdata)
        {
            Vector2 localPoint;
            RectTransform canvas = self.IconItemDrag.transform.parent.GetComponent<RectTransform>();
            Camera uiCamera = self.DomainScene().GetComponent<UIComponent>().UICamera;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, pdata.position, uiCamera, out localPoint);

            self.IconItemDrag.transform.localPosition = new Vector3(localPoint.x, localPoint.y, 0f);
        }

        public static void EndDrag(this UIPetFormationComponent self, RolePetInfo binfo, PointerEventData pdata)
        {
            RectTransform canvas = self.IconItemDrag.transform.parent.GetComponent<RectTransform>();
            GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pdata, results);

            for (int i = 0; i < results.Count; i++)
            {
                string name = results[i].gameObject.name;
                if (!name.Contains("FormationSet"))
                {
                    continue;
                }
                int index = int.Parse(name.Substring(name.Length - 1, 1));
                self.OnDragFormationSet(binfo.Id, index, 1);
                break;
            }
            UICommonHelper.SetParent(self.IconItemDrag, self.GetParent<UI>().GameObject);
            self.IconItemDrag.SetActive(false);
        }
    }
}
