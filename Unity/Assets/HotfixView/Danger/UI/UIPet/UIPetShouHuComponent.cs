﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ET
{

    public class UIPetShouHuComponent : Entity, IAwake, IDestroy
    {
        public GameObject PetListNode;
        public GameObject UIPetShouHuItem;
        public GameObject ButtonSet;

      
        public List<UIPetShouHuInfoComponent> ShouHuInfoList = new List<UIPetShouHuInfoComponent>();
        public List<UIPetShouHuItemComponent> ShouHuItemList = new List<UIPetShouHuItemComponent>();

        public int SelectIndex;
    }

    public class UIPetShouHuComponentAwake : AwakeSystem<UIPetShouHuComponent>
    {
        public override void Awake(UIPetShouHuComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.ShouHuInfoList.Clear();
            self.ShouHuItemList.Clear();

            self.PetListNode = rc.Get<GameObject>("PetListNode");
            self.UIPetShouHuItem = rc.Get<GameObject>("UIPetShouHuItem");
            self.UIPetShouHuItem.SetActive(false);
            for (int i = 0; i < 4; i++)
            {
                UIPetShouHuInfoComponent uIPetShouHuInfo = self.AddChild<UIPetShouHuInfoComponent, GameObject>(rc.Get<GameObject>($"shouhuInfo{i}"));
                uIPetShouHuInfo.SetSelectHandler(i, self.OnSetSelectHandler);
                self.ShouHuInfoList.Add(uIPetShouHuInfo);
            }
            self.ShouHuInfoList[0].OnClickButton();
            self.ButtonSet = rc.Get<GameObject>("ButtonSet");
            ButtonHelp.AddListenerEx( self.ButtonSet, () => { self.OnButtonSet().Coroutine(); } );
            self.OnUpdateUI();
        }
    }

    public static class UIPetShouHuComponentSystem
    {

        public static async ETTask OnButtonSet(this UIPetShouHuComponent self)
        {
            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            C2M_PetShouHuActiveRequest  request = new C2M_PetShouHuActiveRequest() { PetShouHuActive = self.SelectIndex + 1};
            M2C_PetShouHuActiveResponse response = (M2C_PetShouHuActiveResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            if (self.IsDisposed || response.Error != ErrorCode.ERR_Success)
            {
                return;
            }
            petComponent.PetShouHuActive = response.PetShouHuActive;
            self.SetShouHuActive(petComponent.PetShouHuActive - 1);
            FloatTipManager.Instance.ShowFloatTip($"激活: {ConfigHelper.PetShouHuAttri[petComponent.PetShouHuActive - 1].Value}");
        }

        public static async  ETTask OnButtonShouHuHandler(this UIPetShouHuComponent self, long petid)
        {
            C2M_PetShouHuRequest    request = new C2M_PetShouHuRequest() { PetInfoId = petid, Position = self.SelectIndex  };
            M2C_PetShouHuResponse response = (M2C_PetShouHuResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);

            self.ZoneScene().GetComponent<PetComponent>().PetShouHuList = response.PetShouHuList;

            self.OnUpdateUI();
        }

        public static async ETTask UpdatePetList(this UIPetShouHuComponent self)
        {
            long instanceid = self.InstanceId;
            
            if (instanceid != self.InstanceId)
            {
                return;
            }

            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            List<RolePetInfo> rolePetInfos = petComponent.RolePetInfos;

            for (int i = 0; i < rolePetInfos.Count; i++)
            {
                UIPetShouHuItemComponent ui_pet = null;
                if (i < self.ShouHuItemList.Count)
                {
                    ui_pet = self.ShouHuItemList[i];
                    ui_pet.GameObject.SetActive(true);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(self.UIPetShouHuItem);
                    go.SetActive(true);
                    UICommonHelper.SetParent(go, self.PetListNode);
                    ui_pet = self.AddChild<UIPetShouHuItemComponent, GameObject>(go);
                    ui_pet.SetButtonShouHuHandler((long petid) => { self.OnButtonShouHuHandler(petid).Coroutine(); });
                    self.ShouHuItemList.Add(ui_pet);
                }

                ui_pet.OnInitUI(rolePetInfos[i]);
            }
        }

        public static void OnSetSelectHandler(this UIPetShouHuComponent self, int index)
        {
            self.SelectIndex = index;

            for (int i = 0; i < self.ShouHuInfoList.Count; i++)
            {
                self.ShouHuInfoList[i].ImageSelect.SetActive( i == index );
            }
        }

        public static void SetShouHuActive(this UIPetShouHuComponent self, int index)
        {
            for (int i = 0; i < self.ShouHuInfoList.Count; i++)
            {
                self.ShouHuInfoList[i].ImageActive.SetActive(i == index);
            }
        }

        public static void UpdateShouwHuInfo(this UIPetShouHuComponent self)
        {
            for (int i = 0; i < self.ShouHuInfoList.Count; i++)
            {
                self.ShouHuInfoList[i].OnUpdateUI(i);
            }
        }

        public static  void OnUpdateUI(this UIPetShouHuComponent self)
        {
            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            self.UpdatePetList().Coroutine();
            self.UpdateShouwHuInfo();
            self.SetShouHuActive(petComponent.PetShouHuActive - 1);
        }
    }
}
