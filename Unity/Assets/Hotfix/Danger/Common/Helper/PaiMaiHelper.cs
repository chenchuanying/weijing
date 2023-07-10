﻿using System.Linq;
using System.Collections.Generic;

namespace ET
{

    public enum PaiMaiTypeEnum : int
    { 
        None = 0,
        CaiLiao = 1,
        CostItem = 2,
        PetItem = 3,
        Number = 4,
    }

    //成就类型数据
    public class PaiMaiTypeData
    {
        //每个章节对应的拍卖道具
        public Dictionary<int, List<int>> PaiMaiIDItemList = new Dictionary<int, List<int>>();
    }

    public class PaiMaiHelper : Singleton<PaiMaiHelper>
    {

        public List<PaiMaiTypeData> PaiMaiTypeData = new List<PaiMaiTypeData>();

        public List<string> PaiMaiTypeText = new List<string>() { "", "材料", "消耗品","宠物" };

        public Dictionary<int, string> PaiMaiIndexText = new Dictionary<int, string>()
        {
            {1,  "第一章" },
            {2,  "第二章" },
            {3,  "第三章" },
            {4,  "第四章" },
            {5,  "第五章" },
            {21,  "通用" },
            {22,  "宝石" },
            {31,  "技能" },
            {32,  "消耗" },
            /*
            {31,  "红色插槽" },
            {32,  "紫色插槽" },
            {33,  "蓝色插槽" },
            {34,  "绿色插槽" },
            {35,  "白色插槽" },
            {36,  "抗性宝石" },
            */
        };

        protected override void InternalInit()
        {
            base.InternalInit();

            InitPaiMaiData();
        }

        public List<int> GetChaptersByType(PaiMaiTypeEnum pype)
        {
            return PaiMaiTypeData[(int)pype].PaiMaiIDItemList.Keys.ToList();
        }

        public List<int> GetItemsByChapter(int typeid, int chapterId)
        {
            return PaiMaiTypeData[typeid].PaiMaiIDItemList[chapterId];
        }

        public void InitPaiMaiData()
        {
            for (int i = 0; i < (int)PaiMaiTypeEnum.Number+1; i++)
            {
                PaiMaiTypeData.Add(new PaiMaiTypeData());
            }

            Dictionary<int, PaiMaiSellConfig> allPaiMaiData = PaiMaiSellConfigCategory.Instance.GetAll();
            foreach (var item in allPaiMaiData)
            {
                PaiMaiSellConfig paiMaiSellConfig = item.Value;
                int paiMaiType = paiMaiSellConfig.PaiMaiType;
                int chapterId = paiMaiSellConfig.ChapterId;
                PaiMaiTypeData paiMaiTypeDatas = PaiMaiTypeData[paiMaiType];
                if (!paiMaiTypeDatas.PaiMaiIDItemList.ContainsKey(chapterId))
                {
                    paiMaiTypeDatas.PaiMaiIDItemList.Add(chapterId, new List<int>());
                }
                paiMaiTypeDatas.PaiMaiIDItemList[chapterId].Add(item.Key);
            }
        }

        //初始化拍卖行快捷购买数据
        public List<PaiMaiShopItemInfo> InitPaiMaiShopItemList(List<PaiMaiShopItemInfo> shopItemList = null) {

            Dictionary<int, PaiMaiSellConfig> allPaiMaiData = PaiMaiSellConfigCategory.Instance.GetAll();

            List<PaiMaiShopItemInfo> shopListInfos = new List<PaiMaiShopItemInfo>();

            //写入缓存
            Dictionary<long, PaiMaiShopItemInfo> dicInfos = new Dictionary<long, PaiMaiShopItemInfo>();
            if (shopItemList != null)
            {
                //如果列表一样，则直接返回数据
                if (allPaiMaiData.Count == shopItemList.Count)
                {
                    return shopItemList;
                }

                foreach (PaiMaiShopItemInfo info in shopItemList)
                {
                    if (dicInfos.ContainsKey(info.Id) == false)
                    {
                        dicInfos.Add(info.Id, info);
                    }
                }
            }

            foreach (var item in allPaiMaiData)
            {
                PaiMaiShopItemInfo shopList = new PaiMaiShopItemInfo();
                if (item.Value.Price.Length < 2)
                {
                    Log.Debug( $"item.Value.Price: {item.Value.Id} {item.Value.Price.Length}");
                }
                if (dicInfos.ContainsKey(item.Value.Id) == false) 
                {
                    shopList.ItemNumber = 999;
                    shopList.Id = item.Value.ItemID;
                    shopList.PriceType = item.Value.Price[0];
                    shopList.Price = item.Value.Price[1];
                    shopList.PricePro = 1;
                    shopListInfos.Add(shopList);
                }
            }

            return shopListInfos;
        }


        public int GetPaiMaiSellId(int itemid)
        {
            int sellid = 0;
            foreach( var item in PaiMaiSellConfigCategory.Instance.GetAll() )
            {
                if (item.Value.ItemID == itemid)
                {
                    return item.Key;
                }
            }

            return sellid;
        }

#if SERVER
        //刷新拍卖数据
        public static void UpdatePaiMaiDate(Scene scene,int showType) {

            PaiMaiSceneComponent paiMaiComponent = scene.GetComponent<PaiMaiSceneComponent>();
            List<PaiMaiItemInfo> PaiMaiItemInfo = paiMaiComponent.dBPaiMainInfo.PaiMaiItemInfos;
            List<PaiMaiItemInfo> paimaiListShow = new List<PaiMaiItemInfo>();

            switch (showType)
            {

                //消耗品
                case 1:

                    for (int i = 0; i < PaiMaiItemInfo.Count; i++)
                    {
                        ItemConfig itemcof = ItemConfigCategory.Instance.Get(PaiMaiItemInfo[i].BagInfo.ItemID);
                        if (itemcof.ItemType == 1)
                        {
                            paimaiListShow.Add(PaiMaiItemInfo[i]);
                        }
                    }

                    paiMaiComponent.dBPaiMainInfo.PaiMaiItemInfos_Consume = paimaiListShow;

                    break;

                //材料
                case 2:

                    for (int i = 0; i < PaiMaiItemInfo.Count; i++)
                    {
                        ItemConfig itemcof = ItemConfigCategory.Instance.Get(PaiMaiItemInfo[i].BagInfo.ItemID);
                        //材料或者宠物技能
                        if (itemcof.ItemType == 2 || itemcof.ItemType == 5)
                        {
                            paimaiListShow.Add(PaiMaiItemInfo[i]);
                        }
                    }

                    paiMaiComponent.dBPaiMainInfo.PaiMaiItemInfos_Material = paimaiListShow;

                    break;

                //装备
                case 3:

                    for (int i = 0; i < PaiMaiItemInfo.Count; i++)
                    {
                        ItemConfig itemcof = ItemConfigCategory.Instance.Get(PaiMaiItemInfo[i].BagInfo.ItemID);
                        if (itemcof.ItemType == 3)
                        {
                            paimaiListShow.Add(PaiMaiItemInfo[i]);
                        }
                    }

                    paiMaiComponent.dBPaiMainInfo.PaiMaiItemInfos_Equipment = paimaiListShow;

                    break;

                //宝石
                case 4:

                    for (int i = 0; i < PaiMaiItemInfo.Count; i++)
                    {
                        ItemConfig itemcof = ItemConfigCategory.Instance.Get(PaiMaiItemInfo[i].BagInfo.ItemID);
                        if (itemcof.ItemType == 4)
                        {
                            paimaiListShow.Add(PaiMaiItemInfo[i]);
                        }
                    }

                    paiMaiComponent.dBPaiMainInfo.PaiMaiItemInfos_Gemstone = paimaiListShow;

                    break;

            }
        }
#endif
    }
}
