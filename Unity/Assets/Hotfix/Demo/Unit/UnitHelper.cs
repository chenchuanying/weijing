﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ET
{
    public static class UnitHelper
    {
        public static long GetMyUnitId(Scene zoneScene)
        {
            AccountInfoComponent playerComponent = zoneScene.GetComponent<AccountInfoComponent>();
            return playerComponent.MyId;    
        }

        public static bool IsShared(Scene zoneScene,  int sType)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(zoneScene);
            long shareSet = unit.GetComponent<NumericComponent>().GetAsLong(NumericType.FenShangSet);
            return (shareSet & sType) > 0;
        }

        public static bool IsGmAccount(Scene zoneScene)
        {
            AccountInfoComponent playerComponent = zoneScene.GetComponent<AccountInfoComponent>();
            return GMHelp.GmAccount.Contains(playerComponent.Account);
        }

        public static Unit GetMyUnitFromZoneScene(Scene zoneScene)
        {
            AccountInfoComponent playerComponent = zoneScene.GetComponent<AccountInfoComponent>();
            Scene currentScene = zoneScene.GetComponent<CurrentScenesComponent>().Scene;
            return currentScene.GetComponent<UnitComponent>().Get(playerComponent.MyId);
        }
        
        public static Unit GetMyUnitFromCurrentScene(Scene currentScene)
        {
            AccountInfoComponent playerComponent = currentScene.Parent.Parent.GetComponent<AccountInfoComponent>();
            return currentScene.GetComponent<UnitComponent>().Get(playerComponent.MyId);
        }

        public static Unit GetUnitFromZoneSceneByID(Scene zoneScene, long id)
        {
            Scene currentScene = zoneScene.GetComponent<CurrentScenesComponent>().Scene;
            return currentScene.GetComponent<UnitComponent>().Get(id);
        }

        public static TeamPlayerInfo GetSelfTeamPlayerInfo(Scene zoneScene)
        {
            UserInfoComponent userInfoComponent = zoneScene.GetComponent<UserInfoComponent>();
            BagInfo bagInfo = zoneScene.GetComponent<BagComponent>().GetEquipBySubType(ItemLocType.ItemLocEquip, (int)ItemSubTypeEnum.Wuqi);
        
            return new TeamPlayerInfo()
            {
                UserID = userInfoComponent.UserInfo.UserId,
                PlayerLv = userInfoComponent.UserInfo.Lv,
                PlayerName = userInfoComponent.UserInfo.Name,
                WeaponId = bagInfo != null ? bagInfo.ItemID : 0,
                Occ = userInfoComponent.UserInfo.Occ,
                Combat = userInfoComponent.UserInfo.Combat,
                RobotId = userInfoComponent.UserInfo.RobotId,
                OccTwo = userInfoComponent.UserInfo.OccTwo, 
                FashionIds = zoneScene.GetComponent<BagComponent>().FashionEquipList,
            };
        }

        //public static bool IsRobot(this Unit self)
        //{
        //    AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
        //    return self.MainHero && accountInfoComponent.Password == ComHelp.RobotPassWord;
        //}

        public static bool IsSelfRobot(this Unit self)
        {
            return self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.RobotId > 0;
        }

        public static bool IsCanChangeEquip(this Unit self)
        {
            int configId = self.ConfigId;
            return OccupationConfigCategory.Instance.Get(configId).ChangeEquip == 1;
        }

        public static Vector3 GetBornPostion(this Unit self)
        {
            NumericComponent numericComponent = self.GetComponent<NumericComponent>();
            return new Vector3(numericComponent.GetAsFloat(NumericType.Born_X),
                numericComponent.GetAsFloat(NumericType.Born_Y),
                numericComponent.GetAsFloat(NumericType.Born_Z));
        }

        public static bool IsHaveBoss(Scene scene, Vector3 vector3, float dis)
        {
            List<Unit> allunits = scene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                if (allunits[i].Type != UnitType.Monster)
                {
                    continue;
                }
                if (allunits[i].IsBoss()
                    && allunits[i].GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Dead) == 0
                    && PositionHelper.Distance2D(vector3, allunits[i].Position) < dis)
                {
                    return true;
                }
            }

            return false;
        }

        public static int GetWuqiItemID(Scene zoneScene)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(zoneScene);
            int itemId = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Weapon);
            return itemId;
        }

        public static int GetEquipType(Scene zoneScene)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(zoneScene);
            int itemId = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Weapon);
            return ItemHelper.GetEquipType(unit.ConfigId, itemId);
        }

        public static int GetEquipType(this Unit self)
        {
            int itemId = self.GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Weapon);
            return ItemHelper.GetEquipType(self.ConfigId, itemId);
        }

        public static bool IsBackRoom(this Unit self)
        {
            int black = self.GetComponent<NumericComponent>().GetAsInt(NumericType.BlackRoom);
            return black != 0;  
        }


        /// <summary>
        /// 是否小黑屋
        /// </summary>
        /// <param name="zoneScene"></param>
        /// <returns></returns>
        public static bool IsBackRoom( Scene zoneScene)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene( zoneScene );
            return unit!= null ? unit.IsBackRoom(): false;
        }

        public static int GetMaoXianExp(this Unit self)
        {
            int rechargeNum = self.GetComponent<NumericComponent>().GetAsInt(NumericType.RechargeNumber);
            rechargeNum *= 10;
            rechargeNum += self.GetComponent<NumericComponent>().GetAsInt(NumericType.MaoXianExp);

            Log.ILog.Debug($" GetMaoXianExp:  {self.GetComponent<NumericComponent>().GetAsInt(NumericType.RechargeNumber)}   {self.GetComponent<NumericComponent>().GetAsInt(NumericType.MaoXianExp)}");

            return rechargeNum;
        }

        public static List<Unit> GetUnitListByDis(Scene scene, Vector3 pos, int unitType, float maxdis)
        {
            List<Unit> list = new List<Unit>();
            List<Unit> allunits = scene.GetComponent<UnitComponent>().GetAll();

            for (int i = 0; i < allunits.Count; i++)
            {
                Unit unit = allunits[i];
                if (unit.Type != unitType)
                {
                    continue;
                }

                if (Vector3.Distance(pos, unit.Position) > maxdis)
                {
                    continue;
                }
                list.Add(unit);
            }
            return list;
        }

        public static List<Unit> GetUnitList(Scene scene, int unitType)
        {
            //using var list = ListComponent<Unit>.Create();
            List<Unit> list = new List<Unit>();
            List<Unit> allunits = scene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                if (allunits[i].Type == unitType)
                {
                    list.Add(allunits[i]);
                }
            }
            return list;
        }

    }
}