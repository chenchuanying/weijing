﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public enum PetOperationType
    { 
        HeCheng = 1,
        XiLian = 2,
        UpStar_Main = 3,
        UpStar_FuZh = 4,
        RankPet_Team = 5,
        XianJi  = 6,
        JiaYuan_Walk  = 7,
    }


    public class UIPetSelectComponent : Entity, IAwake
    {
        public PetOperationType OperationType;

        public GameObject Btn_Close;
        public GameObject PetListNode;
        public GameObject UIPetSelectItem;
    }


    public class UIPetSelectComponentAwakeSystem : AwakeSystem<UIPetSelectComponent>
    {
        public override void Awake(UIPetSelectComponent self)
        {

            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.Btn_Close = rc.Get<GameObject>("Btn_Close");
            self.PetListNode = rc.Get<GameObject>("PetListNode");
            self.UIPetSelectItem = rc.Get<GameObject>("UIPetSelectItem");
            self.UIPetSelectItem.SetActive(false);

            self.Btn_Close.GetComponent<Button>().onClick.AddListener(()=> { self.OnClickCoseButton(); });
        }
    }


    public static class UIPetSelectComponentSystem
    {
        public static void OnSetType(this UIPetSelectComponent self, PetOperationType bagOperationType)
        {
            self.OperationType = bagOperationType;
            self.OnInitData();
        }

        public static List<long> GetSelectedPet(this UIPetSelectComponent self)
        {
            List<long> selected = new List<long>();
            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            List<long> petTeamList = new List<long>();
            //petTeamList.AddRange(petComponent.TeamPetList);
            //petTeamList.AddRange(petComponent.PetFormations);
            if (self.OperationType == PetOperationType.HeCheng)
            {
                UI uipet = UIHelper.GetUI(self.DomainScene(), UIType.UIPet);
                UIPetComponent uIPetComponent = uipet.GetComponent<UIPetComponent>();
                selected = uIPetComponent.UIPageView.UISubViewList[(int)PetPageEnum.PetHeCheng].GetComponent<UIPetHeChengComponent>().GetSelectedPet();

                long petid = petComponent.GetFightPetId();
                if (petid != 0 && !selected.Contains(petid))
                {
                    selected.Add(petid);
                }
                selected.AddRange(petTeamList);
            }
            if (self.OperationType == PetOperationType.UpStar_FuZh)
            {
                UI uipet = UIHelper.GetUI(self.DomainScene(), UIType.UIPet);
                selected = uipet.GetComponent<UIPetComponent>().UIPageView.UISubViewList[(int)PetPageEnum.PetUpStar].GetComponent<UIPetUpStarComponent>().GetSelectedPet();
                selected.AddRange(petTeamList);
            }
            if (self.OperationType == PetOperationType.XianJi)
            {
                UI uipet = UIHelper.GetUI(self.DomainScene(), UIType.UIPet);
                long petinfoId = uipet.GetComponent<UIPetComponent>().UIPageView.UISubViewList[(int)PetPageEnum.PetList].GetComponent<UIPetListComponent>().LastSelectItem.Id;
                selected.Add(petinfoId);
            }
            if (self.OperationType == PetOperationType.JiaYuan_Walk)
            {
                //UI uipet = UIHelper.GetUI(self.DomainScene(), UIType.UIJiaYuanPet);
                //selected = uipet.GetComponent<UIJiaYuanPetComponent>().UIPageView.UISubViewList[(int)JiaYuanPetEnum.Walk].GetComponent<UIJiaYuanPetWalkComponent>().
                JiaYuanComponent jiaYuanComponent = self.ZoneScene().GetComponent<JiaYuanComponent>();
                for (int i = 0; i < jiaYuanComponent.JiaYuanPetList_2.Count; i++)
                {
                    selected.Add(jiaYuanComponent.JiaYuanPetList_2[i].unitId);
                }
            }
            for (int i = 0; i < petComponent.RolePetInfos.Count; i++)
            { 
                if (petComponent.RolePetInfos[i].PetStatus == 3 && !selected.Contains(petComponent.RolePetInfos[i].Id))
                {
                    selected.Add((petComponent.RolePetInfos[i].Id));
                }
            }
            return selected;
        }

        public static  void OnInitData(this UIPetSelectComponent self)
        {
            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            List<RolePetInfo> list = petComponent.RolePetInfos;

            List<long> selected = self.GetSelectedPet();
            for (int i = 0; i < list.Count; i++)
            {
                if (selected.Contains(list[i].Id))
                {
                    continue;
                }
                if (list[i].PetStatus == 2 || list[i].PetStatus == 3)
                {
                    continue;
                }

                if (self.OperationType == PetOperationType.UpStar_FuZh)
                {
                    UI uipet = UIHelper.GetUI(self.DomainScene(), UIType.UIPet);
                    UIPetUpStarComponent uIPetUpStarComponent = uipet.GetComponent<UIPetComponent>().UIPageView.UISubViewList[(int)PetPageEnum.PetUpStar].GetComponent<UIPetUpStarComponent>();
                    RolePetInfo rolePetInfo = uIPetUpStarComponent.MainPetInfo;
                    if (list[i].Star != rolePetInfo.Star)
                    {
                        continue;
                    }
                }

                GameObject go = GameObject.Instantiate(self.UIPetSelectItem);
                go.SetActive(true);
                UICommonHelper.SetParent(go, self.PetListNode);

                UI uiitem = self.AddChild<UI, string, GameObject>( "UIPetXuanZeItem_" + i, go);
                UIPetSelectItemComponent uIPetHeChengXuanZeItemComponent = uiitem.AddComponent<UIPetSelectItemComponent>();
                uIPetHeChengXuanZeItemComponent.OnInitData(list[i]);
                uIPetHeChengXuanZeItemComponent.OperationType  = self.OperationType;
            }
        }

        public static void OnClickCoseButton(this UIPetSelectComponent self)
        {
            UIHelper.Remove(self.DomainScene(), UIType.UIPetSelect);
        }

    }
}
