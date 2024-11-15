using System;
using System.Collections.Generic;

namespace ET
{

    [ObjectSystem]
    public class DataCollationComponentAwake : AwakeSystem<DataCollationComponent>
    {
        public override void Awake(DataCollationComponent self)
        {
            self.CreateRoleTime = TimeHelper.DateTimeNow().ToString();
        }
    }


    public static class DataCollationComponentSystem
    {

        public static void Check(this DataCollationComponent self)
        {
            self.TotalOnLine++;

            self.TodayOnLine = self.GetParent<Unit>().GetComponent<UserInfoComponent>().TodayOnLine;
        }

        public static void OnXiLian(this DataCollationComponent self, int times)
        {
            self.XiLianTimes += times;

            if (times > 1)
            {
                self.DiamondXiLianTimes += times;
            }
        }

        public static void OnSceondHurt(this DataCollationComponent self, long hurtValue)
        {
            self.SceondHurt = hurtValue;
        }

        public static void OnChouKa(this DataCollationComponent self, int choukaType)
        {
            self.ChouKaTimes += choukaType;
        }

        public static void OnPetChouKa(this DataCollationComponent self, int choukaType)
        {
            self.PetChouKaTimes += choukaType;
        }

        public static void OnPetDuiHuan(this DataCollationComponent self)
        {
            self.PetDuiHuanTimes += 1;
        }

        public static void UpdateRoleMoneySub(this DataCollationComponent self, int Type, int getWay, long value)
        {
            if (value > 0)
            {
                return;
            }
            value *= -1;
            if (Type == UserDataType.Gold)
            {
                self.OnAddCostList(self.GoldCostList, getWay, value);
            }
            if (Type == UserDataType.Diamond)
            {
                self.OnAddCostList(self.DiamondCostList, getWay, value);
            }
        }

        public static void UpdateRoleMoneyAdd(this DataCollationComponent self, int Type, int getWay, long value)
        {
            if (value < 0)
            {
                Log.Warning($"UpdateRoleMoneyAdd<0 : {Type}  {value}");
                return;
            }
            if (Type == UserDataType.Gold)
            {
                self.OnAddCostList(self.GoldGetList, getWay, value);
            }
            if (Type == UserDataType.Diamond)
            {
                self.OnAddCostList(self.DiamondGetList, getWay, value);
            }
        }

        public static void OnZeroClockUpdate(this DataCollationComponent self, bool notice)
        {
            self.PaiMaiCostGoldToday = 0;
        }


        public static void UpdateBuySelfPlayerList(this DataCollationComponent self, long addgold, long unitid, long baginfoid,  bool notice)
        {
            if (unitid == 0)
            {
                return;
            }
            self.PaiMaiGold += addgold;

            if(baginfoid > 0 && !self.SoldBagInfoIDList.Contains(baginfoid))
            {
                self.SoldBagInfoIDList.Add(baginfoid);
            }

            for (int i = self.BuySelfPlayerList.Count - 1; i >= 0 ; i--)
            {
                if (self.BuySelfPlayerList[i].KeyId == unitid)
                {
                    self.BuySelfPlayerList[i].Value += addgold;
                    return;
                }
            }

            self.BuySelfPlayerList.Add( new KeyValuePairLong()
            { 
                KeyId = unitid,
                Value = addgold
            });
        }

        public static long GetCostByType(this DataCollationComponent self, int getWay)
        {
            if (string.IsNullOrEmpty(self.GoldCost))
            { 
                return 0; 
            }

            string[] costlist = self.GoldCost.Split('_');
            for (int i = 0; i < costlist.Length; i++)
            {
                string[] costinfo = costlist[i].Split(',');
                if (costinfo.Length < 3)
                {
                    continue;
                }

                if (int.Parse(costinfo[0]) == getWay)
                {
                    long value = long.Parse(costinfo[2]);
                    return value;
                }
            }
            return 0;
        }


        public static long GetGoldByType(this DataCollationComponent self, int getWay)
        {
            if (string.IsNullOrEmpty(self.GoldGet))
            {
                return 0;
            }

            string[] costlist = self.GoldGet.Split('_');
            for (int i = 0; i < costlist.Length; i++)
            {
                string[] costinfo = costlist[i].Split(',');
                if (costinfo.Length < 3)
                {
                    continue;
                }

                if (int.Parse(costinfo[0]) == getWay)
                {
                    long value = long.Parse(costinfo[2]);
                    return value;
                }
            }
            return 0;
        }

        public static void OnAddCostList(this DataCollationComponent self, List<KeyValuePairInt> pairInts, int getWay, long value)
        {
            bool have = false;
            for (int i = 0; i < pairInts.Count; i++)
            {
                if (pairInts[i].KeyId == getWay)
                {
                    have = true;
                    pairInts[i].Value += value;
                }
            }
            if (!have)
            {
                pairInts.Add(new KeyValuePairInt() { KeyId = getWay, Value = value });
            }
        }

        public static void SetAllCostList(this DataCollationComponent self, List<KeyValuePairInt> pairInts, string costValue)
        {
            if (string.IsNullOrEmpty(costValue))
            {
                return;
            }
            string[] costlist = costValue.Split('_');
            for (int i = 0; i < costlist.Length; i++)
            {
                string[] costinfo = costlist[i].Split(',');
                if (costinfo.Length < 3)
                {
                    continue;
                }

                int getWay = int.Parse(costinfo[0]);    
                long value = long.Parse(costinfo[2]);
                self.OnAddCostList(pairInts, getWay, value);
            }
        }

        public static long GetGoldGetTotal(this DataCollationComponent self)
        {
            if (string.IsNullOrEmpty(self.GoldGet))
            {
                return 0;
            }
            long value = 0;
            string[] costlist = self.GoldGet.Split('_');
            for (int i = 0; i < costlist.Length; i++)
            {
                string[] costinfo = costlist[i].Split(',');
                if (costinfo.Length < 3)
                {
                    continue;
                }

                value += long.Parse(costinfo[2]);

            }
            return value;
        }

        public static long GetGoldCostTotal(this DataCollationComponent self)
        {
            if (string.IsNullOrEmpty(self.GoldCost))
            {
                return 0;
            }
            long value = 0;
            string[] costlist = self.GoldCost.Split('_');
            for (int i = 0; i < costlist.Length; i++)
            {
                string[] costinfo = costlist[i].Split(',');
                if (costinfo.Length < 3)
                {
                    continue;
                }

                int getWay = int.Parse(costinfo[0]);
                value += long.Parse(costinfo[2]);

            }
            return value;
        }

        public static string CostListToString(this DataCollationComponent self, List<KeyValuePairInt> pairInts)
        {
            string str = string.Empty;
            for (int i = 0; i < pairInts.Count; i++)
            {
                str += $"{pairInts[i].KeyId},{ItemHelper.ItemGetWayName(pairInts[i].KeyId)},{pairInts[i].Value}_";
            }
            return str;
        }

        public static void UpdatePlatName(this DataCollationComponent self, int platform, int simulator, int  root, string deviceId)
        {
            string platformName = PlatformHelper.GetPlatformName(platform);
            if (!string.IsNullOrEmpty(self.Platform) && !self.Platform.Contains('_'))
            {
                self.Platform = string.Empty;
            }
            if (!string.IsNullOrEmpty(self.Platform) && self.Platform.Contains(platformName))
            {
                return;
            }
            self.Platform += $"{platformName}: {TimeInfo.Instance.ToDateTime(TimeHelper.ServerNow()).ToString()}_";
            self.Simulator = simulator;
            self.IsRoot = root;   
            self.DeviceID = deviceId;       
        }


        public static string GetDeviceID(this DataCollationComponent self)
        { 
            string device = string.Empty;

            if (self.Simulator == 1)
            {
                device = "模拟器_";
            }
            else
            {
                device = "真机_";
            }

            if (self.IsRoot == 1)
            {
                device += "Root_";
            }

            device += self.DeviceID;
            return device;
        }

        public static void UpdateData(this DataCollationComponent self)
        {
            self.SetAllCostList(self.GoldCostList, self.GoldCost);
            self.GoldCost = self.CostListToString(self.GoldCostList);
            self.GoldCostList.Clear();

            self.SetAllCostList(self.GoldGetList, self.GoldGet);
            self.GoldGet = self.CostListToString(self.GoldGetList);
            self.GoldGetList.Clear();

            self.SetAllCostList(self.DiamondGetList, self.DiamondGet);
            self.DiamondGet = self.CostListToString(self.DiamondGetList);
            self.DiamondGetList.Clear();

            self.SetAllCostList(self.DiamondCostList, self.DiamondCost);
            self.DiamondCost = self.CostListToString(self.DiamondCostList);
            self.DiamondCostList.Clear();
        }

        public static void OnOffLine(this DataCollationComponent self)
        {
            Unit unit = self.GetParent<Unit>();

            UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            PetComponent petComponent = unit.GetComponent<PetComponent>();  
            BagComponent bagComponent = unit.GetComponent<BagComponent>();  

            self.Name = userInfoComponent.UserInfo.Name;
            self.Level = userInfoComponent.UserInfo.Lv;
            self.Account = userInfoComponent.Account;

            self.Occ = OccupationConfigCategory.Instance.Get(userInfoComponent.UserInfo.Occ).OccupationName;

            if (userInfoComponent.UserInfo.OccTwo > 0)
            {
                self.OccTwo = OccupationTwoConfigCategory.Instance.Get(userInfoComponent.UserInfo.OccTwo).OccupationName;
            }

            self.Combat = userInfoComponent.UserInfo.Combat;

            self.Gold = userInfoComponent.UserInfo.Gold;    

            self.Diamond = userInfoComponent.UserInfo.Diamond;

            self.Recharge = numericComponent.GetAsLong( NumericType.RechargeNumber );

            self.TodayOnLine = userInfoComponent.TodayOnLine;

            self.LastLoginTime = TimeHelper.DateTimeNow().ToString();

            self.MainTask = unit.GetComponent<TaskComponent>().GetMainTaskId();   

            self.PetPingfen = petComponent.GetPingfenList();

            self.UnionName = userInfoComponent.UserInfo.UnionName;

            self.JiaYuanLv = userInfoComponent.UserInfo.JiaYuanLv;

            self.JiaYuanFund = userInfoComponent.UserInfo.JiaYuanFund;

            self.PiLao = userInfoComponent.UserInfo.PiLao;

            self.Vitality = userInfoComponent.UserInfo.Vitality;

            int makeType = numericComponent.GetAsInt( NumericType.MakeType_1 );
            self.MakeSkill = MakeHelper.GetMakeTypeName( makeType );

            self.MakeShuLiandu = numericComponent.GetAsInt( NumericType.MakeShuLianDu_1 );

            self.PetFubenId = petComponent.GetPassMaxFubenId();

            self.TrialFubenId = numericComponent.GetAsInt( NumericType.TrialDungeonId );

            //self.ChouKaTimes 
            //self.PetChouKaTimes
            self.ChengZhuangNumber = ItemHelper.GetNumberByQulity(bagComponent.EquipList, 5);

            self.XiLianExp = numericComponent.GetAsInt( NumericType.ItemXiLianDu);

            self.LastSealTowerId = numericComponent.GetAsInt( NumericType.TowerOfSealArrived);

            self.UpdateData();
        }
    }
}
