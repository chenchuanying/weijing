﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIPaiMaiBuyComponent : Entity, IAwake
    {
        public GameObject Btn_Refresh;
        public GameObject TypeListNode;
        public GameObject Btn_Search;
        public GameObject InputField;
        public GameObject ItemListNode;

        public List<UIPaiMaiBuyItemComponent> PaiMaiList = new List<UIPaiMaiBuyItemComponent>();
        public UITypeViewComponent UITypeViewComponent;
        public int PageIndex;
    }


    public class UIPaiMaiBuyComponentAwakeSystem : AwakeSystem<UIPaiMaiBuyComponent>
    {
        public override void Awake(UIPaiMaiBuyComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.TypeListNode = rc.Get<GameObject>("TypeListNode");
            self.UITypeViewComponent = self.AddChild<UITypeViewComponent, GameObject>(self.TypeListNode);
            self.UITypeViewComponent.TypeButtonItemAsset = ABPathHelper.GetUGUIPath("Main/Common/UITypeItem");
            self.UITypeViewComponent.TypeButtonAsset = ABPathHelper.GetUGUIPath("Main/Common/UITypeButton");
            self.UITypeViewComponent.ClickTypeItemHandler = (int typeid, int subtypeid) => { self.OnClickTypeItem(typeid, subtypeid); };

            self.UITypeViewComponent.TypeButtonInfos = self.InitTypeButtonInfos();
            self.UITypeViewComponent.OnInitUI().Coroutine();

            self.Btn_Search = rc.Get<GameObject>("Btn_Search");
            self.Btn_Search.GetComponent<Button>().onClick.AddListener(() => { self.OnClickBtn_Search(); });

            self.Btn_Refresh = rc.Get<GameObject>("Btn_Refresh");
            self.Btn_Refresh.GetComponent<Button>().onClick.AddListener(() => { self.OnClickBtn_Refresh(); });

            self.InputField = rc.Get<GameObject>("InputField");
            self.ItemListNode = rc.Get<GameObject>("ItemListNode");

            self.PaiMaiList.Clear();
            self.GetParent<UI>().OnUpdateUI = () => { self.OnUpdateUI(); };
        }
    }

    public static class UIPaiMaiBuyComponentSystem
    {
        //初始化数据列表
        public static List<TypeButtonInfo> InitTypeButtonInfos(this UIPaiMaiBuyComponent self)
        {

            //显示列表
            TypeButtonInfo typeButtonInfo = new TypeButtonInfo();
            List<TypeButtonInfo> typeButtonInfos = new List<TypeButtonInfo>();
            typeButtonInfo = new TypeButtonInfo();
            foreach (int key in ItemViewHelp.ItemSubType1Name.Keys) 
            {
                //typeButtonInfo.typeButtonItems.Add(new TypeButtonItem() { SubTypeId = 1, ItemName = name });
                typeButtonInfo.typeButtonItems.Add(new TypeButtonItem() { SubTypeId = key, ItemName = ItemViewHelp.ItemSubType1Name[key] });
            }

            typeButtonInfo.TypeId = 1;
            //typeButtonInfo.typeButtonItems = new List<TypeButtonItem>();
            typeButtonInfo.TypeName = ItemViewHelp.ItemTypeName[ItemTypeEnum.Consume];

            typeButtonInfos.Add(typeButtonInfo);

            typeButtonInfo = new TypeButtonInfo();
            foreach (int key in ItemViewHelp.ItemSubType2Name.Keys)
            {
                //typeButtonInfo.typeButtonItems.Add(new TypeButtonItem() { SubTypeId = 2, ItemName = name });
                typeButtonInfo.typeButtonItems.Add(new TypeButtonItem() { SubTypeId = key, ItemName = ItemViewHelp.ItemSubType2Name[key] });
            }


            typeButtonInfo.TypeId = 2;
            //typeButtonInfo.typeButtonItems = new List<TypeButtonItem>();
            typeButtonInfo.TypeName = ItemViewHelp.ItemTypeName[ItemTypeEnum.Material];
            typeButtonInfos.Add(typeButtonInfo);

            typeButtonInfo = new TypeButtonInfo();
            foreach (int key in ItemViewHelp.ItemSubType3Name.Keys)
            {
                typeButtonInfo.typeButtonItems.Add(new TypeButtonItem() { SubTypeId = key, ItemName = ItemViewHelp.ItemSubType3Name[key] });
            }


            typeButtonInfo.TypeId = 3;
            //typeButtonInfo.typeButtonItems = new List<TypeButtonItem>();
            typeButtonInfo.TypeName = ItemViewHelp.ItemTypeName[ItemTypeEnum.Equipment];
            typeButtonInfos.Add(typeButtonInfo);


            typeButtonInfo = new TypeButtonInfo();
            foreach (int key in ItemViewHelp.ItemSubType4Name.Keys)
            {
                typeButtonInfo.typeButtonItems.Add(new TypeButtonItem() { SubTypeId = key, ItemName = ItemViewHelp.ItemSubType4Name[key] });
            }

            typeButtonInfo.TypeId = 4;
            //typeButtonInfo.typeButtonItems = new List<TypeButtonItem>();
            typeButtonInfo.TypeName = ItemViewHelp.ItemTypeName[ItemTypeEnum.Gemstone];
            typeButtonInfos.Add(typeButtonInfo);

            return typeButtonInfos;
        }

        public static void OnClickTypeItem(this UIPaiMaiBuyComponent self, int typeid, int subtypeid)
        {
            for (int i = 0; i < self.PaiMaiList.Count; i++)
            {
                UIPaiMaiBuyItemComponent paimaibuy = self.PaiMaiList[i];
                if (paimaibuy.PaiMaiItemInfo == null)
                {
                    paimaibuy.GameObject.SetActive(false);
                    continue;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(paimaibuy.PaiMaiItemInfo.BagInfo.ItemID);
                //显示   0表示通用
                if (itemConfig.ItemType == typeid && subtypeid == 0)
                {
                    paimaibuy.GameObject.SetActive(true);
                }
                else
                {
                    //子类符合对应关系
                    int itemSubType = itemConfig.ItemSubType;
                    //生肖特殊处理
                    if (itemConfig.ItemType == 3 && itemConfig.ItemSubType >= 1101 && itemConfig.ItemSubType < 1600)
                    {
                        itemSubType = 1100;
                    }
                    paimaibuy.GameObject.SetActive(itemConfig.ItemType == typeid && itemSubType == subtypeid);
                }
            }
        }

        public static void OnUpdateUI(this UIPaiMaiBuyComponent self)
        {
            self.PageIndex = 1;
            self.RequestAllPaiMaiList().Coroutine();
        }

        public static void OnClickBtn_Refresh(this UIPaiMaiBuyComponent self)
        {
            self.PageIndex++;
            self.RequestAllPaiMaiList().Coroutine();
        }

        public static void OnClickBtn_Search(this UIPaiMaiBuyComponent self)
        {
            string text = self.InputField.GetComponent<InputField>().text;

            for (int i = 0; i < self.PaiMaiList.Count; i++)
            {
                UIPaiMaiBuyItemComponent uIPaiMaiBuy = self.PaiMaiList[i];
                if (uIPaiMaiBuy.PaiMaiItemInfo == null)
                {
                    uIPaiMaiBuy.GameObject.SetActive(false);
                    continue;
                }
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(uIPaiMaiBuy.PaiMaiItemInfo.BagInfo.ItemID);
                uIPaiMaiBuy.GameObject.SetActive(itemConfig.ItemName.Contains(text));
            }
        }

        public static async ETTask RequestAllPaiMaiList(this UIPaiMaiBuyComponent self)
        {
            long instanceId = self.InstanceId;
            C2P_PaiMaiListRequest c2M_PaiMaiBuyRequest = new C2P_PaiMaiListRequest()
            {
                ActorId = self.PageIndex,
                PaiMaiType = 2,
                UserId = UnitHelper.GetMyUnitId(self.ZoneScene()),
            };
            P2C_PaiMaiListResponse m2C_PaiMaiBuyResponse = (P2C_PaiMaiListResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_PaiMaiBuyRequest);
            if (instanceId != self.InstanceId)
            {
                return;
            }
            self.Btn_Refresh.SetActive(m2C_PaiMaiBuyResponse.NextPage == 1);

            instanceId = self.InstanceId;
            var path = ABPathHelper.GetUGUIPath("Main/PaiMai/UIPaiMaiBuyItem");
            var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            if (instanceId != self.InstanceId)
            {
                return;
            }

            int number = 0;
            List<PaiMaiItemInfo> PaiMaiItemInfos = m2C_PaiMaiBuyResponse.PaiMaiItemInfos;
            for (int i = 0; i < PaiMaiItemInfos.Count; i++)
            {
                PaiMaiItemInfo paiMaiItemInfo = PaiMaiItemInfos[i];
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(paiMaiItemInfo.BagInfo.ItemID);
                if (!ComHelp.IsShowPaiMai(itemConfig.ItemType, itemConfig.ItemSubType))
                {
                    continue;
                }

                UIPaiMaiBuyItemComponent uI = null;
                if (number < self.PaiMaiList.Count)
                {
                    uI = self.PaiMaiList[number];
                    uI.GameObject.SetActive(true);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(bundleGameObject);
                    UICommonHelper.SetParent(go, self.ItemListNode);
                    go.transform.localScale = Vector3.one * 1f;
                    uI = self.AddChild<UIPaiMaiBuyItemComponent, GameObject>( go);
                    self.PaiMaiList.Add(uI);
                }

                uI.OnUpdateItem(paiMaiItemInfo);
                number++;
            }
            //刷新列表
            for (int i = number; i < self.PaiMaiList.Count; i++)
            {
                self.PaiMaiList[i].OnUpdateItem(null);
                self.PaiMaiList[i].GameObject.SetActive(false);
            }
            //选择刷新列表
            self.UITypeViewComponent.TypeButtonComponents[0].OnClickTypeButton();
        }
    }
}
