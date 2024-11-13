using System;
using System.Collections.Generic;

namespace ET
{

    [ObjectSystem]
    public class BagComponentAwakeSystem : AwakeSystem<BagComponent>
    {

        public override void Awake(BagComponent self)
        {
            self.OnAddItemData(GlobalValueConfigCategory.Instance.Get(9).Value, $"{ItemGetWay.System}_{TimeHelper.ServerNow()}", false);
            if (ComHelp.IsBanHaoZone(self.DomainZone()))
            {
                self.OnAddItemData($"10030001;1", $"{ItemGetWay.System}_{TimeHelper.ServerNow()}", false);
            }
        }
    }

    public static class BagComponentSystem
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool HaveOccEquip(this BagComponent self)
        {
            List<BagInfo> allequiplist = new List<BagInfo>();
            allequiplist.AddRange(self.EquipList);
            allequiplist.AddRange(self.EquipList_2);

            for (int i = 0; i < allequiplist.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(allequiplist[i].ItemID);
                if (itemConfig.ItemType == 3
                    && itemConfig.EquipType >= 0 && itemConfig.EquipType <= 100
                    && itemConfig.ItemSubType >= 0 && itemConfig.ItemSubType <= 12)
                {
                    return true;
                }
            }

            return false;
        }

        public static List<PropertyValue> GetGemProLists(this BagComponent self)
        {
            List<PropertyValue> list = new List<PropertyValue>();
            for (int i = 0; i < self.GemList.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.GemList[i].ItemID);
                string itemUsePar = itemConfig.ItemUsePar;
                if (string.IsNullOrEmpty(itemUsePar) || itemUsePar == "0")
                {
                    continue;
                }
                string[] attributes = itemUsePar.Split('@');
                for (int a = 0; a < attributes.Length; a++)
                {
                    string[] attributeItem = attributes[a].Split(';');
                    int hideId = int.Parse(attributeItem[0]);
                    long hide_value = 0;
                    if (NumericHelp.GetNumericValueType(hideId) == 2)
                    {
                        hide_value = (long)(float.Parse(attributeItem[1]) * 10000);
                    }
                    else
                    {
                        hide_value = long.Parse(attributeItem[1]);
                    }
                    list.Add(new PropertyValue() { HideID = hideId, HideValue = hide_value });
                }
            }

            return list;
        }

        public static List<BagInfo> GetItemByLoc(this BagComponent self, ItemLocType itemEquipType)
        {
            List<BagInfo> ItemTypeList = null;
            switch (itemEquipType)
            {
                case ItemLocType.ItemLocBag:
                    ItemTypeList = self.BagItemList;
                    break;
                case ItemLocType.ItemPetHeXinBag:
                    ItemTypeList = self.BagItemPetHeXin;
                    break;
                case ItemLocType.ItemLocGem:
                    ItemTypeList = self.GemList;
                    break;
                case ItemLocType.ItemLocEquip:
                    ItemTypeList = self.EquipList;
                    break;
                case ItemLocType.ItemPetHeXinEquip:
                    ItemTypeList = self.PetHeXinList;
                    break;
                case ItemLocType.ItemWareHouse1:
                    ItemTypeList = self.Warehouse1;
                    break;
                case ItemLocType.ItemWareHouse2:
                    ItemTypeList = self.Warehouse2;
                    break;
                case ItemLocType.ItemWareHouse3:
                    ItemTypeList = self.Warehouse3;
                    break;
                case ItemLocType.ItemWareHouse4:
                    ItemTypeList = self.Warehouse4;
                    break;
                case ItemLocType.JianYuanWareHouse1:
                    ItemTypeList = self.JianYuanWareHouse1;
                    break;
                case ItemLocType.JianYuanWareHouse2:
                    ItemTypeList = self.JianYuanWareHouse2;
                    break;
                case ItemLocType.JianYuanWareHouse3:
                    ItemTypeList = self.JianYuanWareHouse3;
                    break;
                case ItemLocType.JianYuanWareHouse4:
                    ItemTypeList = self.JianYuanWareHouse4;
                    break;
                case ItemLocType.JianYuanTreasureMapStorage1:
                    ItemTypeList = self.JianYuanTreasureMapStorage1;
                    break;
                case ItemLocType.JianYuanTreasureMapStorage2:
                    ItemTypeList = self.JianYuanTreasureMapStorage2;
                    break;
                case ItemLocType.ChouKaWarehouse:
                    ItemTypeList = self.ChouKaWarehouse;
                    break;
                case ItemLocType.ItemLocEquip_2:
                    ItemTypeList = self.EquipList_2;
                    break;
                case ItemLocType.SeasonJingHe:
                    ItemTypeList = self.SeasonJingHe;
                    break;
                case ItemLocType.PetLocEquip:
                    ItemTypeList = self.PetEquipList;
                    break;
                case ItemLocType.GemWareHouse1:
                    ItemTypeList = self.GemWareHouse1;
                    break;
            }
            return ItemTypeList;
        }

        public static void ZhengLiItemList(this BagComponent self, Dictionary<int, List<BagInfo>> ItemSameList, M2C_RoleBagUpdate m2c_bagUpdate)
        {
            foreach (var item in ItemSameList)
            {
                List<BagInfo> bagInfos = item.Value;
                if (bagInfos.Count == 1)
                {
                    continue;
                }
                ItemConfig itemCof = ItemConfigCategory.Instance.Get(bagInfos[0].ItemID);

                int totalNum = 0;
                int needGrid = 0;
                int finalNum = 0;
                for (int i = 0; i < bagInfos.Count; i++)
                {
                    totalNum += bagInfos[i].ItemNum;
                }
                needGrid = totalNum / itemCof.ItemPileSum;
                needGrid += (totalNum % itemCof.ItemPileSum > 0 ? 1 : 0);
                finalNum = totalNum - (needGrid - 1) * itemCof.ItemPileSum;

                if (needGrid <= 0 || needGrid > bagInfos.Count)
                {
                    Console.WriteLine($"RecvItemSortError: {self.GetParent<Unit>().Id} {bagInfos[0].ItemID}   {totalNum}   {needGrid}  {bagInfos.Count}");
                    continue;
                }
                bagInfos[needGrid - 1].ItemNum = finalNum;
                m2c_bagUpdate.BagInfoUpdate.Add(bagInfos[needGrid - 1]);
                for (int i = 0; i < needGrid - 1; i++)
                {
                    bagInfos[i].ItemNum = itemCof.ItemPileSum;
                    m2c_bagUpdate.BagInfoUpdate.Add(bagInfos[i]);
                }
                //删除后面的空格子
                for (int i = needGrid; i < bagInfos.Count; i++)
                {
                    bagInfos[i].ItemNum = 0;
                    m2c_bagUpdate.BagInfoDelete.Add(bagInfos[i]);
                }
            }
        }


        public static void OnRecvItemSort(this BagComponent self, ItemLocType itemEquipType)
        {
            List<BagInfo> ItemTypeList = self.GetItemByLoc(itemEquipType);

            M2C_RoleBagUpdate m2c_bagUpdate = new M2C_RoleBagUpdate();


            //绑定的
            Dictionary<int, List<BagInfo>> ItemSameList_1 = new Dictionary<int, List<BagInfo>>();
            //未绑定
            Dictionary<int, List<BagInfo>> ItemSameList_2 = new Dictionary<int, List<BagInfo>>();
            //找出可以堆叠并且格子未放满的道具
            for (int i = 0; i < ItemTypeList.Count; i++)
            {
                BagInfo bagInfo = ItemTypeList[i];

                //最大堆叠数量
                ItemConfig itemCof = ItemConfigCategory.Instance.Get(bagInfo.ItemID);
                if (bagInfo.ItemNum >= itemCof.ItemPileSum)
                {
                    continue;
                }

                if (bagInfo.isBinging)
                {
                    if (!ItemSameList_1.ContainsKey(bagInfo.ItemID))
                    {
                        ItemSameList_1[bagInfo.ItemID] = new List<BagInfo>();
                    }
                    ItemSameList_1[bagInfo.ItemID].Add(bagInfo);
                }
                else
                {
                    if (!ItemSameList_2.ContainsKey(bagInfo.ItemID))
                    {
                        ItemSameList_2[bagInfo.ItemID] = new List<BagInfo>();
                    }
                    ItemSameList_2[bagInfo.ItemID].Add(bagInfo);
                }
            }

            self.ZhengLiItemList(ItemSameList_1, m2c_bagUpdate);
            self.ZhengLiItemList(ItemSameList_2, m2c_bagUpdate);

            for (int i = ItemTypeList.Count - 1; i >= 0; i--)
            {
                if (ItemTypeList[i].ItemNum == 0)
                {
                    ItemTypeList.RemoveAt(i);
                }
            }

            //通知客户端背包道具发生改变
            MessageHelper.SendToClient(self.GetParent<Unit>(), m2c_bagUpdate);

            ItemHelper.ItemLitSort(ItemTypeList);
        }

        public static void CheckValiedItem(this BagComponent self, List<BagInfo> bagInfos, int occ, int occTwo)
        {
            Unit unit = self.GetParent<Unit>();
           
            for (int i = bagInfos.Count - 1; i >= 0; i--)
            {
                if (!ItemConfigCategory.Instance.Contain(bagInfos[i].ItemID))
                {
                    Console.WriteLine($"CheckValiedItem11:  {bagInfos[i].ItemID}   {bagInfos[i].ItemNum}");
                    bagInfos.RemoveAt(i);
                    continue;
                }
                if (bagInfos[i].ItemNum <= 0)
                {
                    Console.WriteLine($"CheckValiedItem22:  {bagInfos[i].ItemID}   {bagInfos[i].ItemNum}");
                    bagInfos[i].ItemNum = 1;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(bagInfos[i].ItemID);
                if (itemConfig.EquipType != 101 && itemConfig.ItemType == ItemTypeEnum.Equipment && bagInfos[i].InheritSkills.Count == 0 && itemConfig.ItemQuality >= 5 && itemConfig.UseLv >= 60)
                {
                    int skillid = XiLianHelper.XiLianChuanChengJianDing(itemConfig, occ, occTwo);
                    if (skillid != 0)
                    {
                        bagInfos[i].InheritSkills.Add(skillid);
                    }
                }

                if (itemConfig.ItemType == ItemTypeEnum.Equipment && itemConfig.ItemQuality <= 4)
                {
                    bagInfos[i].InheritSkills.Clear();
                }

                if (itemConfig.ItemType == ItemTypeEnum.Equipment && itemConfig.ItemQuality >= 5 && itemConfig.UseLv < 60)
                {
                    bagInfos[i].InheritSkills.Clear();
                }

                if (itemConfig.EquipType == 101 && bagInfos[i].HideProLists != null)
                {
                    bagInfos[i].HideProLists.Clear();
                }
                if (itemConfig.EquipType == 101 && bagInfos[i].InheritSkills != null)
                {
                    bagInfos[i].InheritSkills.Clear();
                }
            }
        }

        //获取自身所有的道具
        public static List<BagInfo> GetAllItems(this BagComponent self, int occ, int occTwo)
        {
            List<BagInfo> bagList = new List<BagInfo>();

            self.CheckValiedItem(self.GemList, occ, occTwo);
            self.CheckValiedItem(self.BagItemList, occ, occTwo);
            self.CheckValiedItem(self.EquipList, occ, occTwo);
            self.CheckValiedItem(self.BagItemPetHeXin, occ, occTwo);
            self.CheckValiedItem(self.PetHeXinList, occ, occTwo);
            self.CheckValiedItem(self.Warehouse1, occ, occTwo);
            self.CheckValiedItem(self.Warehouse2, occ, occTwo);
            self.CheckValiedItem(self.Warehouse3, occ, occTwo);
            self.CheckValiedItem(self.Warehouse4, occ, occTwo);
            //self.CheckValiedItem(self.JianYuanWareHouse1, occ, occTwo);
            //self.CheckValiedItem(self.JianYuanWareHouse2, occ, occTwo);
            //self.CheckValiedItem(self.JianYuanWareHouse3, occ, occTwo);
            //self.CheckValiedItem(self.JianYuanWareHouse4, occ, occTwo);
            //self.CheckValiedItem(self.JianYuanTreasureMapStorage1, occ, occTwo);
            //self.CheckValiedItem(self.JianYuanTreasureMapStorage2, occ, occTwo);
            //self.CheckValiedItem(self.ChouKaWarehouse, occ, occTwo);
            self.CheckValiedItem(self.EquipList_2, occ, occTwo);
            //self.CheckValiedItem(self.SeasonJingHe, occ, occTwo);
            //self.CheckValiedItem(self.PetEquipList, occ, occTwo);
            //self.CheckValiedItem(self.GemWareHouse1, occ, occTwo);

            for (int i =  self.EquipList.Count - 1; i >=0; i--)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.EquipList[i].ItemID);
                if (itemConfig.ItemType == ItemTypeEnum.Gemstone)
                {
                    Log.Warning($"误穿宝石！！  {itemConfig.ItemName}");
                    self.EquipList.RemoveAt(i);
                    break;
                }
            }

            bagList.AddRange(self.GemList);
            bagList.AddRange(self.BagItemList);
            bagList.AddRange(self.BagItemPetHeXin);
            bagList.AddRange(self.EquipList);
            bagList.AddRange(self.PetHeXinList);
            bagList.AddRange(self.Warehouse1);
            bagList.AddRange(self.Warehouse2);
            bagList.AddRange(self.Warehouse3);
            bagList.AddRange(self.Warehouse4);
            bagList.AddRange(self.JianYuanWareHouse1);
            bagList.AddRange(self.JianYuanWareHouse2);
            bagList.AddRange(self.JianYuanWareHouse3);
            bagList.AddRange(self.JianYuanWareHouse4);
            bagList.AddRange(self.JianYuanTreasureMapStorage1);
            bagList.AddRange(self.JianYuanTreasureMapStorage2);
            bagList.AddRange(self.ChouKaWarehouse);
            bagList.AddRange(self.EquipList_2);
            bagList.AddRange(self.SeasonJingHe);
            bagList.AddRange(self.PetEquipList);
            bagList.AddRange(self.GemWareHouse1);

            return bagList;
        }

        public static List<BagInfo> GetIdItemListByLoc(this BagComponent self, int itemId, ItemLocType loc)
        {
            List<BagInfo> baginfo = new List<BagInfo>();
            List<BagInfo> bagList = self.GetItemByLoc(loc);
            for (int i = 0; i < bagList.Count; i++)
            {
                if (bagList[i].ItemID == itemId)
                {
                    baginfo.Add(bagList[i]);
                }
            }
            return baginfo;
        }

        public static List<BagInfo> GetIdItemList(this BagComponent self, int itemId)
        {
            List<BagInfo> baginfo = new List<BagInfo>();
            for (int i = 0; i < self.BagItemList.Count; i++)
            {
                if (self.BagItemList[i].ItemID == itemId)
                {
                    baginfo.Add(self.BagItemList[i]);
                }
            }
            return baginfo;
        }

        public static int GetNeedCell(this BagComponent self, List<RewardItem> itemids, ItemLocType itemLocType)
        {
            int needcell = 0;
            for  ( int i =0; i < itemids.Count; i++ )
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemids[i].ItemID);
                long curNumber = self.GetItemNumber(itemids[i].ItemID, itemLocType);

                if (curNumber > 0 && curNumber + itemids[i].ItemNum < itemConfig.ItemPileSum)
                {
                    needcell = 0;
                }
                else
                {
                    int temp = 0;
                    temp += (int)(1f * itemids[i].ItemNum / itemConfig.ItemPileSum);
                    temp += (itemids[i].ItemNum % itemConfig.ItemPileSum > 0 ? 1 : 0);

                    needcell += temp;

                    if (temp != 1)
                    {
                        Console.WriteLine($"needcell:{needcell}  ItemNum:{itemids[i].ItemNum}   ItemPileSum:{itemConfig.ItemPileSum}");
                    }
                }
            }

            return needcell;
        }

        //获取某个道具的数量
        public static long GetItemNumber(this BagComponent self, int itemId, ItemLocType itemLocType = ItemLocType.ItemLocBag)
        {
            int userDataType = ItemHelper.GetItemToUserDataType(itemId);
            long number = 0;
            switch (userDataType)
            {
                case UserDataType.None:
                    List<BagInfo> bagInfos = self.GetItemByLoc(itemLocType);
                    for (int i = 0; i < bagInfos.Count; i++)
                    {
                        if (bagInfos[i].ItemID == itemId)
                        {
                            number += bagInfos[i].ItemNum;
                        }
                    }
                    break;
                case UserDataType.Gold:
                    number = self.GetParent<Unit>().GetComponent<UserInfoComponent>().UserInfo.Gold;
                    break;
                case UserDataType.Diamond:
                    number = self.GetParent<Unit>().GetComponent<UserInfoComponent>().UserInfo.Diamond;
                    break;
                case UserDataType.RongYu:
                    number = self.GetParent<Unit>().GetComponent<UserInfoComponent>().UserInfo.RongYu;
                    break;
                case UserDataType.JiaYuanFund:
                    number = self.GetParent<Unit>().GetComponent<UserInfoComponent>().UserInfo.JiaYuanFund;
                    break;
                case UserDataType.UnionContri:
                    number = self.GetParent<Unit>().GetComponent<UserInfoComponent>().UserInfo.UnionZiJin;
                    break;
                case UserDataType.SeasonCoin:
                    number = self.GetParent<Unit>().GetComponent<UserInfoComponent>().UserInfo.SeasonCoin;
                    break;
                case UserDataType.WeiJingGold:
                    number = self.GetParent<Unit>().GetComponent<UserInfoComponent>().UserInfo.WeiJingGold;
                    break;
                default:
                    break;
            }
            return number;
        }


        //根据ID获取对应的背包数据
        public static BagInfo GetItemByLoc(this BagComponent self, ItemLocType itemLocType, long bagId)
        {
            if (bagId == 0)
                return null;
            List<BagInfo> ItemTypeList = self.GetItemByLoc(itemLocType);
            for (int i = 0; i < ItemTypeList.Count; i++)
            {
                if (ItemTypeList[i].BagInfoID == bagId)
                {
                    return ItemTypeList[i];
                }
            }
            return null;
        }

        public static bool IsBagFull(this BagComponent self)
        {
            return self.GetBagLeftCell() <= 0;
        }

        public static int GetBagLeftCell(this BagComponent self)
        {
            return self.GetBagTotalCell() - self.BagItemList.Count;
        }

        public static int GetBagTotalCell(this BagComponent self)
        {
            if (self.WarehouseAddedCell.Count == 0 || self.AdditionalCellNum.Count == 0)
            {
                return GlobalValueConfigCategory.Instance.BagInitCapacity;
            }
            return self.WarehouseAddedCell[0] + self.AdditionalCellNum[0] + + GlobalValueConfigCategory.Instance.BagInitCapacity;
        }

        public static bool IsHourseFullByLoc(this BagComponent self, int hourseId)
        {
            List<BagInfo> ItemTypeList = self.GetItemByLoc((ItemLocType)hourseId);
            return ItemTypeList.Count >= self.GetHourseTotalCell(hourseId);
        }

        public static int GetHourseLeftCell(this BagComponent self, int hourseId)
        {
            List<BagInfo> ItemTypeList = self.GetItemByLoc((ItemLocType)hourseId);
            return self.GetHourseTotalCell(hourseId) - ItemTypeList.Count;
        }

        public static int GetHourseTotalCell(this BagComponent self, int hourseId)
        {
            int storeCapacity = GlobalValueConfigCategory.Instance.HourseInitCapacity;
            if (hourseId == (int)ItemLocType.GemWareHouse1)
            {
                storeCapacity = GlobalValueConfigCategory.Instance.GemStoreInitCapacity;
            }
            return storeCapacity + self.WarehouseAddedCell[hourseId] + self.AdditionalCellNum[hourseId];
        }

        /// <summary>
        /// 获取抽卡仓库剩余的格数，上限100
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static int GetChouKaLeftSpace(this BagComponent self)
        {
            return 100 - self.ChouKaWarehouse.Count;
        }

        public static void OnChangeItemLoc(this BagComponent self, BagInfo bagInfo, ItemLocType itemLocTypeTo, ItemLocType itemLocTypeFrom)
        {
            List<BagInfo> ItemTypeListSour = self.GetItemByLoc(itemLocTypeFrom);
            for (int i = ItemTypeListSour.Count - 1; i >= 0; i--)
            {
                if (ItemTypeListSour[i].BagInfoID == bagInfo.BagInfoID)
                {
                    ItemTypeListSour.RemoveAt(i);
                }
            }

            List<BagInfo> ItemTypeListDest = self.GetItemByLoc(itemLocTypeTo);
            bagInfo.Loc = (int)itemLocTypeTo;
            ItemTypeListDest.Add(bagInfo);
        }

        /// <summary>
        /// 是否有装备技能
        /// </summary>
        /// <param name="self"></param>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public static bool IsHaveEquipSkill(this BagComponent self, int skillId, long xilianequip)
        {
            for (int i = 0; i < self.EquipList.Count; i++)
            {
                if (self.EquipList[i].BagInfoID == xilianequip)
                {
                    continue;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.EquipList[i].ItemID);
                if (itemConfig.SkillID.Contains(skillId.ToString()))
                {
                    return true;
                }
                if (self.EquipList[i].HideSkillLists.Contains(skillId))
                {
                    return true;
                }
                if (self.EquipList[i].InheritSkills.Contains(skillId))
                {
                    return true;
                }
            }
            return false;
        }

        public static void OnResetSeason(this BagComponent self, bool notice)
        { 
            self.SeasonJingHePlan = 0;
            self.SeasonJingHe.Clear();

            self.ClearJingHeItem(self.BagItemList);
            self.ClearJingHeItem(self.Warehouse1);
            self.ClearJingHeItem(self.Warehouse2);
            self.ClearJingHeItem(self.Warehouse3);
            self.ClearJingHeItem(self.Warehouse4);
            self.ClearJingHeItem(self.SeasonJingHe);
        }

        public static void ClearJingHeItem(this BagComponent self, List<BagInfo> bagInfos)
        {
            for (int i = bagInfos.Count - 1; i >= 0; i--)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(bagInfos[i].ItemID);
                if (itemConfig.EquipType == 201)
                {
                    bagInfos.RemoveAt(i);
                }
            }
        }

        public static List<BagInfo> GetCurJingHeList(this BagComponent self)
        {
            List<BagInfo> bagInfos = new List<BagInfo>();
            for (  int i = 0; i < self.SeasonJingHe.Count; i++ )
            {
                if (self.SeasonJingHe[i].EquipPlan == self.SeasonJingHePlan)
                {
                    bagInfos.Add(self.SeasonJingHe[i]);
                }
            }
            return bagInfos;
        }

        public static bool IsEquipJingHe(this BagComponent self, int itemId)
        {
            List<BagInfo> bagInfos  =self.GetCurJingHeList();
            for (int i = 0; i < bagInfos.Count; i++)
            {
                if (bagInfos[i].ItemID == itemId)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<int> GetEquipTianFuIds(this BagComponent self)
        {
            List<int> equiptianfuids = new List<int>(); 
            List<BagInfo> equiplist = new List<BagInfo>();
            equiplist.AddRange(self.EquipList );
            equiplist.AddRange(self.EquipList_2);
            equiplist.AddRange(self.SeasonJingHe);

            for (int i = 0; i < self.EquipList.Count; i++)
            {
                if (!ItemConfigCategory.Instance.Contain(self.EquipList[i].ItemID))
                {
                    continue;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.EquipList[i].ItemID);

                if (!EquipConfigCategory.Instance.Contain(itemConfig.ItemEquipID))
                {
                    continue;
                }
                EquipConfig equipConfig = EquipConfigCategory.Instance.Get(itemConfig.ItemEquipID);
                

                if (equipConfig.TianFuId != 0)
                {
                    equiptianfuids.Add(equipConfig.TianFuId);
                }
            }

            return equiptianfuids;
        }

        public static BagInfo GetJingHeByWeiZhi(this BagComponent self, int subType)
        {
            List<BagInfo> bagInfos = self.GetCurJingHeList();
            for (int i = 0; i < bagInfos.Count; i++)
            {
                if (bagInfos[i].EquipIndex == subType)
                { 
                    return bagInfos[i]; 
                }
            }
            return null;
        }

        public static List<BagInfo> GetEquipListByWeizhi(this BagComponent self, ItemLocType equipIndex, int position)
        {
            List<BagInfo> bagInfos = new List<BagInfo>();
            List<BagInfo> equipList = self.GetItemByLoc(equipIndex);
            for (int i = 0; i < equipList.Count; i++)
            {
                ItemConfig itemCof = ItemConfigCategory.Instance.Get(equipList[i].ItemID);
                if (itemCof.ItemSubType == position)
                {
                    bagInfos.Add(equipList[i]);
                }
            }
            return bagInfos;
        }

        public static int GetMaxQiangHuaLevel(this BagComponent self)
        {
            int maxLevel = 0;
            for (int i = 0; i < self.QiangHuaLevel.Count; i++)
            {
                if (self.QiangHuaLevel[i] > maxLevel)
                {
                    maxLevel = self.QiangHuaLevel[i];
                }
            }
            return maxLevel;
        }

        //获取某个装备位置的道具数据
        public static BagInfo GetEquipBySubType(this BagComponent self, ItemLocType equipIndex, int subType)
        {
            List<BagInfo> equipList = self.GetItemByLoc(equipIndex);
            for (int i = 0; i < equipList.Count; i++)
            {
                ItemConfig itemCof = ItemConfigCategory.Instance.Get(equipList[i].ItemID);
                if (itemCof.ItemSubType == subType)
                {
                    return equipList[i];
                }
            }
            return null;
        }

        public static void OnLogin(this BagComponent self, int robotId)
        {

            Unit unit = self.GetParent<Unit>();
            int zodiacnumber = self.GetZodiacnumber();
            unit.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.ZodiacEquipNumber_215, 0, zodiacnumber);


            ///old
            //int warehourseNumber = (int)ItemLocType.ItemLocMax - 5;
            //if (self.WarehouseAddedCell.Count < warehourseNumber)  // 11)
            //{
            //    for (int i = self.WarehouseAddedCell.Count; i < warehourseNumber; i++)
            //    {
            //        self.WarehouseAddedCell.Add(0);
            //    }
            //}

            //new 20240110
            if (self.BagAddedCell >= 0)
            {
                //if (self.WarehouseAddedCell.Count > 0 && self.WarehouseAddedCell.Count < (int)ItemLocType.ItemLocMax - 5)
                if (self.WarehouseAddedCell.Count > 0 )
                {
                    List<int> bagaddCell = new List<int>() { self.BagAddedCell, 0,0,0,0 };
                    self.WarehouseAddedCell.InsertRange(0, bagaddCell);
                }

                self.BagAddedCell = -1;  //该字段废弃掉
            }

            for (int i = self.WarehouseAddedCell.Count; i < (int)ItemLocType.ItemLocMax; i++)
            {
                self.WarehouseAddedCell.Add(0);
            }
            for (int i = self.AdditionalCellNum.Count; i < (int)ItemLocType.ItemLocMax; i++)
            {
                self.AdditionalCellNum.Add(0);
            }


            if (self.QiangHuaLevel.Count == 0)
            {
                for (int i = 0; i <= 11; i++)
                {
                    self.QiangHuaLevel.Add(0);
                    self.QiangHuaFails.Add(0);
                }
            }
            else
            {
                for (int i = 0; i <= 11; i++)
                {
                    int maxLevel = QiangHuaHelper.GetQiangHuaMaxLevel(i);
                    if (self.QiangHuaLevel[i] >= maxLevel)
                    {
                        self.QiangHuaLevel[i] = maxLevel - 1;

                        if (i != 0)
                        {
                            Log.Error($"self.QiangHuaLevel[i] >= maxLevel： {unit.Id}   {i}  {self.QiangHuaLevel[i]}");
                        }
                    }
                }
            }

            if (robotId != 0)
            {
                int[] equipList = new int[0];
                UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
                RobotConfig robotConfig = RobotConfigCategory.Instance.Get(robotId);

                if (robotConfig.Behaviour != 1 && robotConfig.Level > userInfoComponent.UserInfo.Lv)
                {
                    userInfoComponent.UserInfo.Lv = robotConfig.Level;
                }
                if (robotConfig.EquipList != null)
                {
                    equipList = robotConfig.EquipList != null ? robotConfig.EquipList : equipList;
                }
                else
                {
                    equipList = ItemConfigCategory.Instance.GetRandomEquipList(userInfoComponent.UserInfo.Occ, userInfoComponent.UserInfo.Lv);
                }
                for (int i = 0; i < equipList.Length; i++)
                {
                    if (equipList[i] == 0)
                    {
                        continue;
                    }
                    ItemConfig itemconfig = ItemConfigCategory.Instance.Get(equipList[i]);
                    if (self.GetEquipBySubType(ItemLocType.ItemLocEquip, itemconfig.ItemSubType) != null)
                    {
                        continue;
                    }
                    if (self.GetIdItemList(equipList[i]).Count > 0)
                    {
                        continue;
                    }

                    self.OnAddItemData($"{equipList[i]};1", $"{ItemGetWay.System}_0", false);
                    List<BagInfo> bagInfo = self.GetIdItemList(equipList[i]);
                    if (bagInfo.Count == 0)
                    {
                        Log.Warning("机器人装备 bagInfo.Count == 0");
                        continue;
                    }

                    self.OnChangeItemLoc(bagInfo[0], ItemLocType.ItemLocEquip, ItemLocType.ItemLocBag);
                }
            }
        }

        public static int GetZodiacnumber(this BagComponent self)
        {
            int number = 0;
            for (int i = 0; i < self.EquipList.Count; i++)
            {
                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(self.EquipList[i].ItemID);
                if (itemConfig.EquipType == 101)
                {
                    number++;
                }
            }

            return number;
        }

        public static int GetWuqiItemId(this BagComponent self)
        {
            BagInfo bagInfo = self.GetEquipBySubType(ItemLocType.ItemLocEquip, (int)ItemSubTypeEnum.Wuqi);
            return bagInfo != null ? bagInfo.ItemID : 0;
        }

        public static void OnAddJianDing(this BagComponent self)
        {
            self.OnAddItemData( $"11200001;1@11200002;1@11200003;1", $"{ItemGetWay.GM}_{TimeHelper.ServerNow()}" );
        }

        //字符串添加道具 
        public static bool OnAddItemData(this BagComponent self, string rewardItems, string getType, bool notice = true)
        {
            List<RewardItem> costItems = new List<RewardItem>();
            string[] needList = rewardItems.Split('@');
            for (int i = 0; i < needList.Length; i++)
            {
                string[] itemInfo = needList[i].Split(';');
                if (itemInfo.Length < 2)
                {
                    continue;
                }
                int itemId = int.Parse(itemInfo[0]);
                int itemNum = int.Parse(itemInfo[1]);
                costItems.Add(new RewardItem() { ItemID = itemId, ItemNum = itemNum });
            }
            return self.OnAddItemData(costItems, string.Empty, getType, notice);
        }

        public static void OnAddItemData(this BagComponent self, List<BagInfo> bagInfos, string getType)
        {
            for (int i = 0; i < bagInfos.Count; i++)
            {
                self.OnAddItemData(bagInfos[i], getType);
            }
        }

        public static bool OnAddItemData(this BagComponent self, BagInfo bagInfo, string getType)
        {
            ItemConfig itemCof = ItemConfigCategory.Instance.Get(bagInfo.ItemID);
            int maxPileSum = itemCof.ItemPileSum;

            if (maxPileSum > 1 || bagInfo.BagInfoID == 0)
            {
                return self.OnAddItemData($"{bagInfo.ItemID};{bagInfo.ItemNum}", string.IsNullOrEmpty(bagInfo.GetWay) ? getType : bagInfo.GetWay);
            }
            else
            {
                self.BagItemList.Add(bagInfo);

                M2C_RoleBagUpdate m2c_bagUpdate = new M2C_RoleBagUpdate();
                m2c_bagUpdate.BagInfoAdd.Add(bagInfo);
                //通知客户端背包道具发生改变
                MessageHelper.SendToClient(self.GetParent<Unit>(), m2c_bagUpdate);

                //检测任务需求道具
                ItemAddHelper.OnGetItem(self.GetParent<Unit>(), int.Parse(getType.Split('_')[0]), bagInfo.ItemID, bagInfo.ItemNum);
                return true;
            }
        }

        public static void OnAddItemToStore(this BagComponent self, int itemlockType, int itemid, int itemnumber, string getType)
        {
            BagInfo useBagInfo = new BagInfo();
            useBagInfo.ItemID = itemid;
            useBagInfo.ItemNum = itemnumber;
            useBagInfo.Loc = itemlockType;
            useBagInfo.BagInfoID = IdGenerater.Instance.GenerateId();
            useBagInfo.GemHole = ItemHelper.DefaultGem;
            useBagInfo.GemIDNew = ItemHelper.DefaultGem;
            useBagInfo.GetWay = getType;
            self.GetItemByLoc((ItemLocType)useBagInfo.Loc).Add(useBagInfo);

            M2C_RoleBagUpdate m2c_bagUpdate = new M2C_RoleBagUpdate();
            m2c_bagUpdate.BagInfoAdd.Add(useBagInfo);
            //通知客户端背包道具发生改变
            MessageHelper.SendToClient(self.GetParent<Unit>(), m2c_bagUpdate);
        }

        public static void OnAddItemDataNewCell(this BagComponent self, BagInfo bagInfo, int itemnumber)
        {
            int itemid = bagInfo.ItemID;
            BagInfo useBagInfo = new BagInfo();
            useBagInfo.ItemID = itemid;
            useBagInfo.ItemNum = itemnumber;
            ItemConfig itemCof = ItemConfigCategory.Instance.Get(itemid);
            useBagInfo.Loc = itemCof.ItemType == (int)ItemTypeEnum.PetHeXin ? (int)ItemLocType.ItemPetHeXinBag : (int)ItemLocType.ItemLocBag;
            useBagInfo.BagInfoID = IdGenerater.Instance.GenerateId();
            useBagInfo.GemHole = ItemHelper.DefaultGem;
            useBagInfo.GemIDNew = ItemHelper.DefaultGem;
            useBagInfo.GetWay = bagInfo.GetWay;
            useBagInfo.isBinging = bagInfo.isBinging;
            self.GetItemByLoc((ItemLocType)useBagInfo.Loc).Add(useBagInfo);

            M2C_RoleBagUpdate m2c_bagUpdate = new M2C_RoleBagUpdate();
            m2c_bagUpdate.BagInfoAdd.Add(useBagInfo);
            //通知客户端背包道具发生改变
            MessageHelper.SendToClient(self.GetParent<Unit>(), m2c_bagUpdate);
        }

        /// <summary>
        /// 暂时只有宝石仓库用到
        /// </summary>
        /// <param name="self"></param>
        /// <param name="itemId"></param>
        /// <param name="itemNumber"></param>
        /// <param name="itemLocType"></param>
        /// <returns></returns>
        public static bool CheckCanAddItem(this BagComponent self, int itemId , int itemNumber, ItemLocType itemLocType)
        {
            if (itemLocType == ItemLocType.GemWareHouse1)
            {
                if (self.IsHourseFullByLoc((int)itemLocType))
                {
                    List<BagInfo> bagInfoList = self.GetItemByLoc(itemLocType);
                    for (int i = 0; i <bagInfoList.Count; i++)
                    {
                        if (bagInfoList[i].ItemID!= itemId)
                        {
                            continue;
                        }
                        ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemId);
                        if (bagInfoList[i].ItemNum + itemNumber <= itemConfig.ItemPileSum)
                        {
                            return true;
                        }
                    }

                    return false;
                }
                else 
                {

                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        //添加背包道具道具[支持同时添加多个]
        public static bool OnAddItemData(this BagComponent self, List<RewardItem> rewardItems_init, string makeUserID, string getWay, bool notice = true, bool gm = false, ItemLocType UseLocType = ItemLocType.ItemLocBag)
        {
            int bagCellNumber = 0;
            int petHeXinNumber = 0;
            string[] getWayInfo = getWay.Split('_');
            int getType = int.Parse(getWayInfo[0]);
            Unit unit = self.GetParent<Unit>();
            if (unit.IsRobot() && getType == ItemGetWay.PickItem)
            {
                return true;
            }

            if (getType == ItemGetWay.GM)
            {
                gm = true;
            }

            List<RewardItem> rewardItems = new List<RewardItem>();
            for (int i = rewardItems_init.Count - 1; i >= 0; i--)
            {
                bool have = false;
                for (int bb = rewardItems.Count - 1; bb >= 0; bb--)
                {
                    if (rewardItems[bb].ItemID == rewardItems_init[i].ItemID)
                    {
                        rewardItems[bb].ItemNum += rewardItems_init[i].ItemNum;
                        have = true;
                        break;
                    }
                }

                if (!have)
                {
                    RewardItem item = new RewardItem();
                    item.ItemID = rewardItems_init[i].ItemID;
                    item.ItemNum = rewardItems_init[i].ItemNum;
                    rewardItems.Add(item);
                }
            }

            for (int i = rewardItems.Count - 1; i >= 0; i--)
            {
                if (rewardItems[i].ItemID == 0 || !ItemConfigCategory.Instance.Contain(rewardItems[i].ItemID))
                {
                    continue;
                }

                ItemConfig itemCof = ItemConfigCategory.Instance.Get(rewardItems[i].ItemID);
                int userDataType = ItemHelper.GetItemToUserDataType(rewardItems[i].ItemID);
                if (userDataType != UserDataType.None)
                {
                    continue;
                }

                int ItemPileSum = (gm && itemCof.ItemPileSum > 1) ? 1000000 : itemCof.ItemPileSum;
                if (UseLocType >= ItemLocType.ItemWareHouse1)
                {
                    continue;
                }
                if (itemCof.ItemType == ItemTypeEnum.PetHeXin)
                {
                    petHeXinNumber += rewardItems[i].ItemNum;
                    continue;
                }

                if (itemCof.ItemType == ItemTypeEnum.Equipment)
                {
                    ItemPileSum = itemCof.ItemPileSum;
                }
                if (ItemPileSum == 1)
                {
                    bagCellNumber += rewardItems[i].ItemNum;
                }
                else if (rewardItems[i].ItemNum <= ItemPileSum)
                {
                    bagCellNumber += 1;
                }
                else
                {
                    bagCellNumber += (int)(1f * rewardItems[i].ItemNum / ItemPileSum);
                    bagCellNumber += (rewardItems[i].ItemNum % ItemPileSum > 0 ? 1 : 0);
                }
            }
            if (rewardItems.Count == 0)
            {
                return true;
            }


            if (getType != ItemGetWay.GemHeCheng)
            {
                if (bagCellNumber > self.GetBagLeftCell() && UseLocType == ItemLocType.ItemLocBag)
                {
                    return false;
                }
                if ((petHeXinNumber + self.BagItemPetHeXin.Count > ComHelp.PetHeXinMax) && UseLocType == ItemLocType.ItemLocBag)
                {
                    return false;
                }
            }


            //通知客户端背包刷新
            M2C_RoleBagUpdate m2c_bagUpdate = self.message;
            m2c_bagUpdate.BagInfoAdd.Clear();
            m2c_bagUpdate.BagInfoUpdate.Clear();
            m2c_bagUpdate.BagInfoDelete.Clear();
            for (int i = rewardItems.Count - 1; i >= 0; i--)
            {
                int itemID = rewardItems[i].ItemID;
                if (itemID == 0 || !ItemConfigCategory.Instance.Contain(itemID))
                {
                    continue;
                }

                int leftNum = rewardItems[i].ItemNum;
                int userDataType = ItemHelper.GetItemToUserDataType(itemID);
                if (userDataType == UserDataType.Gold && rewardItems[i].ItemNum > 1000000)
                {
                    Log.Warning($"[获取金币]UserDataType.Gold  {unit.Id} {getType} {unit.GetComponent<UserInfoComponent>().UserName} {rewardItems[i].ItemNum}");
                }
                if (userDataType == UserDataType.Diamond)
                {
                    Log.Warning($"[获取钻石]UserDataType.Diamond  {unit.Id} {getType} {unit.GetComponent<UserInfoComponent>().UserName} {rewardItems[i].ItemNum}");
                }
                if (userDataType == UserDataType.PiLao)
                {
                    //Log.Warning($"[增加疲劳] {unit.DomainZone()}  {unit.Id}   {getType}  {rewardItems[i].ItemNum}");
                }
                if (userDataType != UserDataType.None)
                {
                    //检测任务需求道具
                    unit.GetComponent<UserInfoComponent>().UpdateRoleMoneyAdd(userDataType, leftNum.ToString(), true, getType);
                    ItemAddHelper.OnGetItem(unit, getType, itemID, leftNum);
                    continue;
                }

                //最大堆叠数量
                ItemConfig itemCof = ItemConfigCategory.Instance.Get(itemID);
                if (itemCof.EquipType == 101 || itemCof.ItemQuality >= 4 || (itemCof.Id >= 16000101 && itemCof.Id <= 16000312) || (itemCof.Id >= 10030011 && itemCof.Id <= 10030019))
                {
                    Log.Warning($"[获取道具] {unit.Id} {getType} {itemID} {rewardItems[i].ItemNum}");
                }

                //神兽碎片
                if (itemCof.Id == 10000136)
                {
                    string username = unit.GetComponent<UserInfoComponent>().UserInfo.Name;
                    string getwayname = string.Empty;
                        ItemHelper.ItemGetWayNameList.TryGetValue(getType, out getwayname);
                    string loginfo = $"区服:{unit.DomainZone()} 玩家:id{unit.Id} 名字:{username} 通过:{getType}({getwayname}) 获取神兽碎片X{leftNum}";
                    LogHelper.CoreLogInfo(loginfo);
                }

                //10000143 10000152   10010086  10025009
                if ( ( itemCof.Id == 10000143 || itemCof.Id == 10000152 || itemCof.Id == 10010086 || itemCof.Id == 10025009)
                    && getType == ItemGetWay.PickItem && unit.GetComponent<UserInfoComponent>().UserInfo.Lv < 20)
                {
                    MapComponent mapComponent = unit.DomainScene().GetComponent<MapComponent>();
                    int sceneId = mapComponent.SceneId;
                    if (sceneId >= 100001 && sceneId <= 100601)
                    {
                        Log.Error($"[获取道具作弊]{itemCof.Id}：  {unit.DomainZone()}  {unit.Id}  {mapComponent.SceneTypeEnum}  {mapComponent.SceneId}");
                    }
                }

                if (itemCof.Id == 10000136 && (getType != ItemGetWay.ChouKa && getType != ItemGetWay.ChouKaReward
                    && getType != ItemGetWay.Activity_MaoXianJia && getType != ItemGetWay.PetExplore && getType != ItemGetWay.PetExploreReward))
                {
                    Log.Error($"[获取道具作弊]{itemCof.Id}：  {unit.DomainZone()}  {unit.Id}  {getType}");
                }
                if (leftNum >= 99)
                {
                    Log.Warning($"[获取道具]leftNum >= 99    {unit.DomainZone()} {unit.Id} {getType} {itemID} {rewardItems[i].ItemNum}");
                    //if ( getType == ItemGetWay.PickItem &&  rewardItems[i].ItemNum >= 5)
                    //{
                    //    Log.Error($"[获取道具]leftNum >= 99_2    {unit.DomainZone()} {unit.Id} {getType} {itemID} {rewardItems[i].ItemNum}");
                    //}
                }

                int maxPileSum = (gm && itemCof.ItemPileSum > 1) ? 1000000 : itemCof.ItemPileSum;
                ItemLocType itemLockType = ItemLocType.ItemLocBag;
                List<BagInfo> itemlist = null;
                if (itemCof.ItemType == ItemTypeEnum.Equipment)
                {
                    maxPileSum = itemCof.ItemPileSum;
                }
                if (itemCof.ItemType == ItemTypeEnum.PetHeXin)
                {
                    maxPileSum = itemCof.ItemPileSum;
                    itemLockType = ItemLocType.ItemPetHeXinBag;
                    itemlist = self.GetItemByLoc(itemLockType);
                }
                else if (getType == ItemGetWay.PickItem && itemCof.ItemType == ItemTypeEnum.Gemstone && itemCof.ItemQuality > 3)
                {
                    NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
                    if (numericComponent.GetAsInt(NumericType.GemWarehouseOpen) >=1 && self.CheckCanAddItem(itemID, leftNum, ItemLocType.GemWareHouse1))
                    {
                        itemLockType = ItemLocType.GemWareHouse1;
                        itemlist = self.GetItemByLoc(itemLockType);
                    }
                    else
                    {
                        itemLockType = UseLocType;
                        itemlist = self.GetItemByLoc(itemLockType);
                    }
                }
                else
                {
                    itemLockType = UseLocType;
                    itemlist = self.GetItemByLoc(itemLockType);
                }

                for (int k = 0; k < itemlist.Count; k++)
                {
                    BagInfo userBagInfo = itemlist[k];
                    if (userBagInfo.ItemID != itemID)
                    {
                        continue;
                    }
                    if (userBagInfo.ItemNum >= maxPileSum)
                    {
                        continue;
                    }
                    int newNum = leftNum + userBagInfo.ItemNum;
                    if (newNum > maxPileSum)
                    {
                        leftNum = newNum - maxPileSum;
                        newNum = maxPileSum;
                    }
                    else
                    {
                        leftNum = 0;
                    }
                    userBagInfo.ItemNum = newNum;
                    m2c_bagUpdate.BagInfoUpdate.Add(userBagInfo);

                    if (leftNum == 0)
                    {
                        //跳出循环
                        break;
                    }
                }

                //还没有插入完，需要开启新格子
                while (leftNum > 0)
                {
                    BagInfo useBagInfo = new BagInfo();
                    useBagInfo.ItemID = itemID;
                    useBagInfo.ItemNum = (leftNum > maxPileSum) ? maxPileSum : leftNum;
                    useBagInfo.Loc = (int)itemLockType;
                    useBagInfo.BagInfoID = IdGenerater.Instance.GenerateId();
                    useBagInfo.GemHole = ItemHelper.DefaultGem;
                    useBagInfo.GemIDNew = ItemHelper.DefaultGem;
                    useBagInfo.GetWay = getWay;
                    leftNum -= useBagInfo.ItemNum;

                    //记录制造的玩家
                    useBagInfo.MakePlayer = makeUserID;
                    //蓝色品质的装备需要进行鉴定
                    if (!ItemHelper.IsBuyItem(getType) && itemCof.ItemType == 3)
                    {
                        if (itemCof.ItemQuality >= 4)
                        {
                            useBagInfo.IfJianDing = true;
                        }
                        else
                        {
                            //白色和绿色品质都是100% 紫色概率出鉴定
                            float jianDingPro = 0f;

                            if (itemCof.ItemQuality == 1)
                            {
                                jianDingPro = 0f;
                            }
                            if (itemCof.ItemQuality == 2)
                            {
                                jianDingPro = 0f;
                            }
                            if (itemCof.ItemQuality == 3)
                            {
                                jianDingPro = 0f;
                            }
                            if (itemCof.ItemQuality == 4)
                            {
                                jianDingPro = 0.75f;
                            }

                            if (RandomHelper.RandFloat() <= jianDingPro)
                            {
                                useBagInfo.IfJianDing = true;
                            }
                        }

                        //特殊处理不坚定
                        if (useBagInfo.ItemID == 14100021 || useBagInfo.ItemID == 14100022 || useBagInfo.ItemID == 14100121 || useBagInfo.ItemID == 14100122 || useBagInfo.ItemID == 14100221 || useBagInfo.ItemID == 14060006)
                        {
                            useBagInfo.IfJianDing = false;
                        }

                        int equipId = itemCof.ItemEquipID;
                        if (equipId != 0 && EquipConfigCategory.Instance.Get(equipId).AppraisalItem == 0)
                        {
                            useBagInfo.IfJianDing = false;
                        }

                        if (itemCof.EquipType == 101)
                        {
                            useBagInfo.IfJianDing = itemCof.ItemQuality >= 5;
                        }
                    }
                    //默认洗练
                    if (!ItemHelper.IsBuyItem(getType) && itemCof.ItemEquipID != 0)
                    {
                        int xilianLevel = XiLianHelper.GetXiLianId(unit.GetComponent<NumericComponent>().GetAsInt(NumericType.ItemXiLianDu));
                        xilianLevel = xilianLevel != 0 ? EquipXiLianConfigCategory.Instance.Get(xilianLevel).XiLianLevel : 0;

                        int xilianType = 0;
                        if (getType == ItemGetWay.SkillMake || getType == ItemGetWay.TreasureMap)
                        {
                            xilianType = 2;
                        }

                       
                        ItemXiLianResult itemXiLian = new ItemXiLianResult();
                        if (itemCof.EquipType < 101) //装备洗炼
                        {
                            itemXiLian = XiLianHelper.XiLianItem(unit, useBagInfo, xilianType, xilianLevel, 0,0);
                        }
                        else if(itemCof.EquipType == 101)//生肖洗炼
                        {
                            itemXiLian = XiLianHelper.XiLianShengXiao(useBagInfo);
                        }

                        //for(int skill = 0; skill < itemXiLian.HideSkillLists.Count; skill++)
                        //{
                        //    unit.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.EquipActiveSkillId_222, itemXiLian.HideSkillLists[skill], 1);
                        //}

                        useBagInfo.XiLianHideProLists = itemXiLian.XiLianHideProLists;              //基础属性洗炼
                        useBagInfo.HideSkillLists = itemXiLian.HideSkillLists;                      //隐藏技能
                        useBagInfo.XiLianHideTeShuProLists = itemXiLian.XiLianHideTeShuProLists;    //特殊属性洗炼
                    }


                    if (ItemGetWay.ItemGetBing.Contains(getType))
                    {
                        useBagInfo.isBinging = true;
                    }

                    //掉落的橙色装备默认为绑定的物品
                    if (((getType == ItemGetWay.PickItem
                        || getType == ItemGetWay.ChouKa)
                        && itemCof.ItemQuality >= 5) || itemCof.IfLock == 1)
                    {
                        useBagInfo.isBinging = true;
                    }
                    if (getType == ItemGetWay.System )
                    {
                        useBagInfo.IfJianDing = false;
                    }
                    //藏宝图
                    if (itemCof.ItemSubType == 113 || itemCof.ItemSubType == 127)
                    {
                        ItemAddHelper.TreasureItem(unit, useBagInfo);
                    }
                    //鉴定符
                    if (itemCof.ItemSubType == 121)
                    {
                        int makePlan = 1;
                        if (getType == ItemGetWay.SkillMake && getWayInfo.Length >= 3)
                        {
                            makePlan = int.Parse(getWayInfo[1]);
                        }
                        if (makePlan != 1 && makePlan != 2)
                        {
                            makePlan = 1;
                        }
                        int shulianduNumeric = makePlan == 1 ? NumericType.MakeShuLianDu_1 : NumericType.MakeShuLianDu_2;
                        int shuliandu = unit.GetComponent<NumericComponent>().GetAsInt(shulianduNumeric);
                        ItemAddHelper.JianDingFuItem(useBagInfo, shuliandu, getType);

                        if (getType == ItemGetWay.GM)
                        {
                            useBagInfo.ItemPar = "100";
                        }
                    }
                    if (getType == ItemGetWay.PetEggPutOut && itemCof.ItemSubType == 102)
                    {
                        if (getWayInfo.Length >= 3)
                        {
                            useBagInfo.FuLing = int.Parse(getWayInfo[2]);
                        }
                    }
                    //食物
                    if (itemCof.ItemType == 1 && itemCof.ItemSubType == 131)
                    {
                        useBagInfo.ItemPar = RandomHelper.RandomNumber(1, 100).ToString();
                    }
                    //家园烹饪
                    if (getType == ItemGetWay.JiaYuanCook)
                    {
                        useBagInfo.ItemPar = RandomHelper.RandomNumber(1, 100).ToString();
                    }


                    //拾取到橙色装备
                    if (itemCof.ItemType == 3 && itemCof.ItemQuality >= 5 && getType == ItemGetWay.PickItem)
                    {
                        string name = unit.GetComponent<UserInfoComponent>().UserInfo.Name;
                        string noticeContent = $"恭喜玩家 {name} 获得装备: <color=#{ComHelp.QualityReturnColor(5)}>{itemCof.ItemName}</color>";
                        ServerMessageHelper.SendBroadMessage(self.DomainZone(), NoticeType.Notice, noticeContent);
                    }

                    //刷新传承属性
                    if (itemCof.ItemType == ItemTypeEnum.Equipment && itemCof.EquipType != 101
                     && itemCof.ItemQuality >= 5 && itemCof.UseLv >= 60)
                    {
                        int occ = unit.GetComponent<UserInfoComponent>().UserInfo.Occ;
                        int occTwo = unit.GetComponent<UserInfoComponent>().UserInfo.OccTwo;
                        int skillid = XiLianHelper.XiLianChuanChengJianDing(itemCof, occ, occTwo);
                        if (skillid != 0)
                        {
                            useBagInfo.InheritSkills.Add(skillid);
                        }
                    }

                    // 振幅卷轴
                    if (itemCof.ItemType == ItemTypeEnum.Consume && itemCof.ItemSubType == 17)
                    {
                        // 属性
                        useBagInfo.IncreaseProLists.AddRange(XiLianHelper.GetHidePro(useBagInfo.ItemID));
                        // 技能
                        useBagInfo.IncreaseSkillLists.AddRange(XiLianHelper.GetHideSkill(useBagInfo.ItemID));
                    }

                    // 赛季晶核
                    if (itemCof.ItemType == ItemTypeEnum.Equipment && itemCof.EquipType == 201)
                    {
                        useBagInfo.ItemPar = ItemHelper.GetJingHeInitQulity(useBagInfo.ItemID).ToString();

                        //增加技能的晶核无须鉴定
                        int jingheSkill = ItemHelper.GetJingHeSkillId(useBagInfo.ItemID);
                        if (jingheSkill > 0)
                        {
                            useBagInfo.IfJianDing = false;
                            useBagInfo.HideSkillLists.Add(jingheSkill); 
                        }
                        else
                        {
                            useBagInfo.IfJianDing = true;
                        }
                    }

                    if (getType == ItemGetWay.PaiMaiShop || getType == ItemGetWay.StoreBuy || getType == ItemGetWay.RandomTowerReward || getType == 97)
                    {
                        useBagInfo.isBinging = true;    
                    }

                    self.GetItemByLoc((ItemLocType)useBagInfo.Loc).Add(useBagInfo);
                    m2c_bagUpdate.BagInfoAdd.Add(useBagInfo);
                }
                //检测任务需求道具
                ItemAddHelper.OnGetItem(unit, getType, itemID, leftNum);
            }

            //通知客户端背包道具发生改变
            if (notice)
            {
                MessageHelper.SendToClient(unit, m2c_bagUpdate);
            }

            return true;
        }

        public static bool CheckCostItem(this BagComponent self, string rewardItems)
        {
            string[] needList = rewardItems.Split('@');
            for (int i = 0; i < needList.Length; i++)
            {
                string[] itemInfo = needList[i].Split(';');
                if (itemInfo.Length < 2)
                {
                    continue;
                }
                int itemId = int.Parse(itemInfo[0]);
                int itemNum = int.Parse(itemInfo[1]);
                if (self.GetItemNumber(itemId) < itemNum)
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CheckCostItem(this BagComponent self, List<RewardItem> rewardItems)
        {
            for (int i = 0; i < rewardItems.Count; i++)
            {
                RewardItem itemInfo = rewardItems[i];
                if (self.GetItemNumber(itemInfo.ItemID) < itemInfo.ItemNum)
                {
                    return false;
                }
            }
            return true;
        }

        //字符串删除道具
        public static bool OnCostItemData(this BagComponent self, string rewardItems, ItemLocType itemLocType, int itemGetWay)
        {
            List<RewardItem> costItems = new List<RewardItem>();
            string[] needList = rewardItems.Split('@');
            for (int i = 0; i < needList.Length; i++)
            {
                string[] itemInfo = needList[i].Split(';');
                if (itemInfo.Length < 2)
                {
                    continue;
                }
                int itemId = int.Parse(itemInfo[0]);
                int itemNum = int.Parse(itemInfo[1]);
                costItems.Add(new RewardItem() { ItemID = itemId, ItemNum = itemNum });
            }
            return self.OnCostItemData(costItems, itemLocType, itemGetWay);
        }

        //删除背包道具道具[支持同时添加多个]
        public static bool OnCostItemData(this BagComponent self, List<long> costItems, ItemLocType itemLocType = ItemLocType.ItemLocBag)
        {
            //通知客户端背包刷新
            M2C_RoleBagUpdate m2c_bagUpdate = new M2C_RoleBagUpdate();

            List<BagInfo> ItemTypeList = self.GetItemByLoc(itemLocType);

            for (int i = 0; i < costItems.Count; i++)
            {
                for (int k = ItemTypeList.Count - 1; k >= 0; k--)
                {
                    if (ItemTypeList[k].BagInfoID == costItems[i])
                    {
                        m2c_bagUpdate.BagInfoDelete.Add(ItemTypeList[k]);
                        ItemTypeList.RemoveAt(k);
                        break;
                    }
                }
            }

            //通知客户端背包道具发生改变
            MessageHelper.SendToClient(self.GetParent<Unit>(), m2c_bagUpdate);
            return true;
        }

        //指定某一个格子的ID
        public static bool OnCostItemData(this BagComponent self, long uid, int number)
        {
            //通知客户端背包刷新
            M2C_RoleBagUpdate m2c_bagUpdate = new M2C_RoleBagUpdate();

            List<BagInfo> ItemTypeList = self.GetItemByLoc(ItemLocType.ItemLocBag);
            for (int k = ItemTypeList.Count - 1; k >= 0; k--)
            {
                if (ItemTypeList[k].BagInfoID == uid)
                {
                    ItemTypeList[k].ItemNum -= number;

                    if (ItemTypeList[k].ItemNum <= 0)
                    {
                        m2c_bagUpdate.BagInfoDelete.Add(ItemTypeList[k]);
                        ItemTypeList.RemoveAt(k);
                    }
                    else
                    {
                        m2c_bagUpdate.BagInfoUpdate.Add(ItemTypeList[k]);
                    }
                    break;
                }
            }
            //通知客户端背包道具发生改变
            MessageHelper.SendToClient(self.GetParent<Unit>(), m2c_bagUpdate);
            return true;
        }

        //删除背包道具道具[支持同时添加多个]
        public static bool OnCostItemData(this BagComponent self, List<RewardItem> costItems, ItemLocType itemLocType, int itemGetWay)
        {
            for (int i = costItems.Count - 1; i >= 0; i--)
            {
                int itemID = costItems[i].ItemID;
                long itemNum = costItems[i].ItemNum;

                //获取背包内的道具是否足够
                if (self.GetItemNumber(itemID, itemLocType) < itemNum)
                {
                    return false;
                }
            }

            //通知客户端背包刷新
            Unit unit = self.GetParent<Unit>();
            M2C_RoleBagUpdate m2c_bagUpdate = new M2C_RoleBagUpdate();
            m2c_bagUpdate.BagInfoAdd = new List<BagInfo>();

            for (int i = costItems.Count - 1; i >= 0; i--)
            {
                int itemID = costItems[i].ItemID;
                int itemNum = costItems[i].ItemNum;

                //扣除金币
                if (itemID == (int)UserDataType.Gold)
                {
                    itemNum = -1 * itemNum;
                    unit.GetComponent<UserInfoComponent>().UpdateRoleMoneySub(UserDataType.Gold, itemNum.ToString(), true, itemGetWay);
                    continue;
                }
                if (itemID == (int)UserDataType.WeiJingGold)
                {
                    itemNum = -1 * itemNum;
                    unit.GetComponent<UserInfoComponent>().UpdateRoleMoneySub(UserDataType.WeiJingGold, itemNum.ToString(), true, itemGetWay);
                    continue;
                }
                if (itemID == (int)UserDataType.Diamond)
                {
                    itemNum = -1 * itemNum;
                    unit.GetComponent<UserInfoComponent>().UpdateRoleMoneySub(UserDataType.Diamond, itemNum.ToString(), true, itemGetWay);
                    continue;
                }
                if (itemID == (int)UserDataType.RongYu)
                {
                    itemNum = -1 * itemNum;
                    unit.GetComponent<UserInfoComponent>().UpdateRoleData(UserDataType.RongYu, itemNum.ToString());
                    continue;
                }
                if (itemID == (int)UserDataType.JiaYuanFund)
                {
                    itemNum = -1 * itemNum;
                    unit.GetComponent<UserInfoComponent>().UpdateRoleData(UserDataType.JiaYuanFund, itemNum.ToString());
                    continue;
                }
                if (itemID == (int)UserDataType.SeasonCoin)
                {
                    itemNum = -1 * itemNum;
                    unit.GetComponent<UserInfoComponent>().UpdateRoleData(UserDataType.SeasonCoin, itemNum.ToString());
                    continue;
                }
                if (itemID == (int)UserDataType.UnionContri)
                {
                    itemNum = -1 * itemNum;
                    unit.GetComponent<UserInfoComponent>().UpdateRoleData(UserDataType.UnionContri, itemNum.ToString());
                    continue;
                }
                //if (!DllHelper.CheckItem)
                //{
                //    continue;
                //}
                LogHelper.LogWarning($"消耗道具: {unit.Id} {itemID} {itemNum}", false);
                List<BagInfo> bagInfos = self.GetItemByLoc(itemLocType);
                for (int k = bagInfos.Count - 1; k >= 0; k--)
                {
                    BagInfo userBagInfo = bagInfos[k];
                    if (userBagInfo.ItemID == itemID)
                    {
                        if (userBagInfo.ItemNum >= itemNum)
                        {
                            //满足扣除数
                            int costNum = itemNum;
                            itemNum -= userBagInfo.ItemNum;
                            userBagInfo.ItemNum -= costNum;
                            if (userBagInfo.ItemNum <= 0)
                            {
                                m2c_bagUpdate.BagInfoDelete.Add(userBagInfo);
                                bagInfos.RemoveAt(k);
                            }
                            else
                            {
                                m2c_bagUpdate.BagInfoUpdate.Add(userBagInfo);
                            }
                        }
                        else
                        {
                            itemNum -= userBagInfo.ItemNum;
                            //完全删除道具
                            userBagInfo.ItemNum = 0;
                            m2c_bagUpdate.BagInfoDelete.Add(userBagInfo);
                            bagInfos.RemoveAt(k);
                        }

                        //扣除完道具直接跳出当前循环
                        if (itemNum <= 0)
                        {
                            break;
                        }
                    }
                }
                ItemAddHelper.OnCostItem(unit, itemID);
            }

            //通知客户端背包道具发生改变
            MessageHelper.SendToClient(unit, m2c_bagUpdate);
            return true;
        }

        public static int GetQiangHuaLevel(this BagComponent self, int subType)
        {
            if (subType > 1000)
            {
                return 0;
            }
            return self.QiangHuaLevel[subType];
        }

        public static void OnEquipFuMo(this BagComponent self, int itemid, List<HideProList> hideProLists, int index)
        {
            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(itemid);
            string[] itemparams = itemConfig.ItemUsePar.Split('@');
            int weizhi = int.Parse(itemparams[0]);
            List<BagInfo> bagInfos = self.GetEquipListByWeizhi(ItemLocType.ItemLocEquip, weizhi);
            if (bagInfos.Count <= index)
            {
                return;
            }
            //通知客户端背包刷新
            M2C_RoleBagUpdate m2c_bagUpdate = new M2C_RoleBagUpdate();
            m2c_bagUpdate.BagInfoUpdate.Add(bagInfos[index]);

            //9@200103; 0.03; 0.03
            bagInfos[index].FumoProLists.Clear();
            bagInfos[index].FumoProLists.AddRange(hideProLists);
            //bagInfos[index].FumoProLists.AddRange( ItemHelper.GetItemFumoPro(itemid) );

            //通知客户端背包道具发生改变
            MessageHelper.SendToClient(self.GetParent<Unit>(), m2c_bagUpdate);
            Function_Fight.GetInstance().UnitUpdateProperty_Base(self.GetParent<Unit>(), true, true);
        }



        public static void OnGmGaoJi(this BagComponent self, int level)
        {
            List<string> gmitemList = new List<string>()
            {
                //道具
                "1#10#648",
                "1#1#599999999",
                "1#3#99999999",
                "1#13#9999999",
                "1#16#999999",
                "1#10020001#9999",
                "1#10021010#9999",
                "1#10021009#9999",
                "1#10021008#9999",
                "1#10021005#9999",
                "1#10010037#9999",
                "1#10000136#9999",
                "1#11300001#1",
                "1#11300004#1",
                "1#11300003#1",
                "1#11300502#1",
                "1#11300503#1",
                "1#11300504#1",
                "1#10000162#9999",
                "1#10000164#9999",
                "1#10000165#9999",
                "1#10000155#9999",
                "1#10010083#9999",
                "1#10010078#9999",
                "1#10010079#9999",
                "1#10010085#99999",
                "1#10010086#9999",
                "1#10010087#9999",
                "1#10010093#1",
                "1#10010093#1",
                "1#10010093#1",
                "1#10010103#999",
                "1#10010029#9999",
                "1#10010030#9999",
                "1#10036021#1",
                "1#10036028#1",
                "1#10036030#1",
                "1#10033001#999",
                "1#10035001#999",
                "1#10034001#999",

                //装备
                "1#14060004#1",
                "1#14100004#1",
                "1#14100008#1",
                "1#14100104#1",
                "1#14100108#1",
                "1#14100204#1",

                //宝石
                "1#10045101#999",
                "1#10045102#999",
                "1#10045103#999",
                "1#10045201#999",
                "1#10045202#999",
                "1#10045203#999",
                "1#10045301#999",
                "1#10045302#999",
                "1#10045303#999",
                "1#10045401#999",
                "1#10045402#999",
                "1#10045403#999",
                "1#10000143#99999",
                "1#10000132#99999",
                "1#10000144#99999",
                "1#10000145#99999",
                "1#10000146#99999",
                "1#10000147#99999",
                "1#10000131#99999",
                "1#10000151#99999",
                "1#10000150#9999",
                "1#10000148#99999",
                "1#10000149#99999",
                "1#10000152#99999",
                "1#10000156#999",
                "1#10000157#99999",
                "1#17002006#1",
                "1#17002003#1",
                "1#17002002#1",
                "1#10000159#9999",
            };

            self.AdditionalCellNum[0] = ConfigHelper.BuyBagCellCosts.Count; 

            Unit unit = self.GetParent<Unit>();

            List<int> equipList = new List<int>() 
            {
                
            };

            UserInfo userInfo = unit.GetComponent<UserInfoComponent>().UserInfo;

            if (level == 1) //高级
            {
                if (userInfo.Occ == 1)  //战士
                {
                    equipList.Add(15810001);
                    equipList.Add(15810002);
                }
                if (userInfo.Occ == 2)  //法师
                {
                    equipList.Add(15810101);
                    equipList.Add(15810102);
                }
                if (userInfo.Occ == 3)  //猎人
                {
                    equipList.Add(15810001);
                    equipList.Add(15810201);
                }
                //11:布甲
                //12:轻甲
                //13:重甲
                int occTwo = userInfo.OccTwo;
                if (occTwo == 0)
                {
                    equipList.Add(10030001);
                    equipList.Add(15806001);
                    equipList.Add(15807001);
                    equipList.Add(15808001);
                    equipList.Add(15809001);
                    equipList.Add(15806001);
                    equipList.Add(15807001);
                    equipList.Add(15808001);
                    equipList.Add(15809001);
                }
                else
                {
                    OccupationTwoConfig occupationTwo = OccupationTwoConfigCategory.Instance.Get(occTwo);
                    if (occupationTwo.ArmorMastery == 1)
                    {
                        equipList.Add(15801002);
                        equipList.Add(15802002);
                        equipList.Add(15803002);
                        equipList.Add(15804002);
                        equipList.Add(15805002);
                        equipList.Add(15811002);
                    }
                    if (occupationTwo.ArmorMastery == 12)
                    {
                        equipList.Add(15801001);
                        equipList.Add(15802001);
                        equipList.Add(15803001);
                        equipList.Add(15804001);
                        equipList.Add(15805001);
                        equipList.Add(15811001);
                    }
                    if (occupationTwo.ArmorMastery == 13)
                    {
                        equipList.Add(15801003);
                        equipList.Add(15802003);
                        equipList.Add(15803003);
                        equipList.Add(15804003);
                        equipList.Add(15805003);
                        equipList.Add(15811003);
                    }
                }
            }
            if (level == 2)  //中级158-154
            {
                if (userInfo.Occ == 1)  //战士
                {
                    equipList.Add(15410001);
                    equipList.Add(15410002);
                }
                if (userInfo.Occ == 2)  //法师
                {
                    equipList.Add(15410101);
                    equipList.Add(15410102);
                }
                if (userInfo.Occ == 3)  //猎人
                {
                    equipList.Add(15410001);
                    equipList.Add(15410201);
                }
                //11:布甲
                //12:轻甲
                //13:重甲
                int occTwo = userInfo.OccTwo;
                if (occTwo == 0)
                {
                    equipList.Add(10030001);
                    equipList.Add(15406001);
                    equipList.Add(15407001);
                    equipList.Add(15408001);
                    equipList.Add(15409001);
                    equipList.Add(15406001);
                    equipList.Add(15407001);
                    equipList.Add(15408001);
                    equipList.Add(15409001);
                }
                else
                {
                    OccupationTwoConfig occupationTwo = OccupationTwoConfigCategory.Instance.Get(occTwo);
                    if (occupationTwo.ArmorMastery == 1)
                    {
                        equipList.Add(15401002);
                        equipList.Add(15402002);
                        equipList.Add(15403002);
                        equipList.Add(15404002);
                        equipList.Add(15405002);
                        equipList.Add(15411002);
                    }
                    if (occupationTwo.ArmorMastery == 12)
                    {
                        equipList.Add(15401001);
                        equipList.Add(15402001);
                        equipList.Add(15403001);
                        equipList.Add(15404001);
                        equipList.Add(15405001);
                        equipList.Add(15411001);
                    }
                    if (occupationTwo.ArmorMastery == 13)
                    {
                        equipList.Add(15401003);
                        equipList.Add(15402003);
                        equipList.Add(15403003);
                        equipList.Add(15404003);
                        equipList.Add(15405003);
                        equipList.Add(15411003);
                    }
                }
            }
            if (level == 3)  //低级账号
            {

            }


            for (int i = 0; i < gmitemList.Count; i++)
            { 
                string[] itemInfo = gmitemList[i].Split('#');
                int itemId = int.Parse(itemInfo[0]);

                if (equipList.Contains(itemId) && self.GetItemNumber( itemId )> 0)
                {
                    continue;
                }

                self.OnAddItemData($"{itemInfo[1]};{itemInfo[2]}", $"{ItemGetWay.GM}_{TimeHelper.ServerNow()}");
            }

            for (int i = 0; i < equipList.Count; i++)
            {
                self.OnAddItemData($"{equipList[i]};1", $"{ItemGetWay.GM}_{TimeHelper.ServerNow()}");
            }

                //"1#14060004#1",
                //"1#14100004#1",
                //"1#14100008#1",
                //"1#14100104#1",
                //"1#14100108#1",
                //"1#14100204#1",
                //穿戴装备
                for (int i = 0; i < equipList.Count; i++)
            { 
                List<BagInfo> equiplist = self.GetIdItemList(equipList[i]);
                if (equiplist.Count == 0)
                {
                    continue;
                }

                ItemConfig itemConfig = ItemConfigCategory.Instance.Get(equipList[i]);
                int itemsubType = itemConfig.ItemSubType;

                BagInfo useBagInfo = equiplist[0];
                useBagInfo.HideProLists = ItemAddHelper.GetEquipZhuanJingHidePro(itemConfig.ItemEquipID, itemConfig.Id, 100, unit, false);
                useBagInfo.IfJianDing = false;
                if (self.GetEquipListByWeizhi(ItemLocType.ItemLocEquip, itemsubType).Count > 0)
                {
                    continue;
                }

                unit.GetComponent<BagComponent>().OnChangeItemLoc(useBagInfo, ItemLocType.ItemLocEquip, ItemLocType.ItemLocBag);
                unit.GetComponent<SkillSetComponent>().OnWearEquip(useBagInfo);
            }
            
            //激活全部探险家,可领取
            //激活令牌 累充98和298
            self.OnAddItemData("10;98", $"{ItemGetWay.GM}_{TimeHelper.ServerNow()}");
            self.OnAddItemData("10;298", $"{ItemGetWay.GM}_{TimeHelper.ServerNow()}");
            for (int i = 0; i < 50; i++)
            {
                self.OnAddItemData("10;648", $"{ItemGetWay.GM}_{TimeHelper.ServerNow()}");
            }

            for (int i = 0; i < self.QiangHuaLevel.Count; i++)
            {
                int maxLevel = QiangHuaHelper.GetQiangHuaMaxLevel(i);
                self.QiangHuaLevel[i] = maxLevel - 1;
            }
        }

        public static bool OnCostItemData(this BagComponent self, BagInfo bagInfo, ItemLocType locType, int number)
        {
            List<BagInfo> bagInfos = self.GetItemByLoc(locType);

            if (bagInfo.ItemNum >= number)
            {
                bagInfo.ItemNum -= number;

                if (bagInfo.ItemNum <= 0)
                {
                    bagInfos.Remove(bagInfo);
                }
                LogHelper.LogWarning($"消耗道具: {self.GetParent<Unit>().Id} {bagInfo.ItemID} {number}", false);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}