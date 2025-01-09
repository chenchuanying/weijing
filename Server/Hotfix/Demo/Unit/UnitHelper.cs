using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public static class UnitHelper
    {
        public static int GetFubenDifficulty(Unit unit)
        {
            int fubenDifficulty = FubenDifficulty.None;
            MapComponent mapComponent = unit.DomainScene().GetComponent<MapComponent>();
            if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.CellDungeon)
            {
                fubenDifficulty = unit.DomainScene().GetComponent<CellDungeonComponent>().FubenDifficulty;
            }
            if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.CellDungeon)
            {
                fubenDifficulty = unit.DomainScene().GetComponent<LocalDungeonComponent>().FubenDifficulty;
            }
            return fubenDifficulty;
        }

        public static UnitInfo CreateUnitInfo(Unit unit)
        {
            UnitInfo unitInfo = new UnitInfo();
            UnitInfoComponent unitInfoComponent = unit.GetComponent<UnitInfoComponent>();
            NumericComponent nc = unit.GetComponent<NumericComponent>();
            unitInfo.UnitId = unit.Id;
            unitInfo.ConfigId = unit.ConfigId;
            Vector3 position = unit.Position;
            unitInfo.X = position.x;
            unitInfo.Y = position.y;
            unitInfo.Z = position.z;
            Vector3 forward = unit.Forward;
            unitInfo.ForwardX = forward.x;
            unitInfo.ForwardY = forward.y;
            unitInfo.ForwardZ = forward.z;
            unitInfo.UnitType = unit.Type;

            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            if (moveComponent != null)
            {
                if (!moveComponent.IsArrived())
                {
                    unitInfo.MoveInfo = new MoveInfo();
                    for (int i = moveComponent.N; i < moveComponent.Targets.Count; ++i)
                    {
                        Vector3 pos = moveComponent.Targets[i];
                        unitInfo.MoveInfo.X.Add(pos.x);
                        unitInfo.MoveInfo.Y.Add(pos.y);
                        unitInfo.MoveInfo.Z.Add(pos.z);
                    }
                }
            }

            //创建玩家循环赋值属性
            if (nc != null)
            {
                foreach ((int key, long value) in nc.NumericDic)
                {
                    if (key >= (int)NumericType.Max)
                    {
                        continue;
                    }
                    unitInfo.Ks.Add(key);
                    unitInfo.Vs.Add(value);
                }
            }
  
            switch (unit.Type)
            {
                case UnitType.Player:
                    //携带的buff
                    unitInfo.Buffs = unit.GetComponent<BuffManagerComponent>().GetMessageBuff();
                    unitInfo.Skills = unit.GetComponent<SkillManagerComponent>().GetMessageSkill();
                    //设置数据
                    UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
                    unitInfo.UnitName = userInfoComponent.UserInfo.Name;
                    unitInfo.ConfigId = userInfoComponent.UserInfo.Occ;
                    unitInfo.UnionName = string.IsNullOrWhiteSpace(userInfoComponent.UserInfo.UnionName) ? string.Empty : userInfoComponent.UserInfo.UnionName;
                    unitInfo.DemonName = unitInfoComponent.DemonName;
                    unitInfo.FashionEquipList = unit.GetComponent<BagComponent>().FashionEquipList;
                    break;
                case UnitType.JingLing:
                    unitInfo.MasterName = unitInfoComponent.MasterName;
                    unitInfo.UnitName = unitInfoComponent.UnitName;
                    break;
                case UnitType.Pasture:
                case UnitType.Plant:
                    unitInfo.MasterName = unitInfoComponent.MasterName;
                    unitInfo.UnitName = unitInfoComponent.UnitName;
                    break;
                case UnitType.Pet:
                    unitInfo.MasterName = unit.GetComponent<UnitInfoComponent>().MasterName;
                    unitInfo.UnitName = unit.GetComponent<UnitInfoComponent>().UnitName;
                    break;
                case UnitType.Bullet:
                    unitInfo.UnitName = unit.GetComponent<UnitInfoComponent>().UnitName;
                    break;
                case UnitType.Stall:
                    unitInfo.UnitName = unit.GetComponent<UnitInfoComponent>().UnitName;
                    break;
                case UnitType.Npc:
                    break;
                default:
                    break;
            }
            return unitInfo;
        }

        /// <summary>
        /// 获取看见unit的玩家，主要用于广播
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static Dictionary<long, AOIEntity> GetBeSeePlayers(this Unit self)
        {
            return self.GetComponent<AOIEntity>().GetBeSeePlayers();
        }

        public static Dictionary<long, AOIEntity> GetSeeUnits(this Unit self)
        {
            return self.GetComponent<AOIEntity>().GetSeeUnits();
        }

        public static void NoticeUnitAdd(Unit unit, Unit sendUnit)
        {
            //非自己击杀的怪物。不同步
            if (sendUnit.Type == UnitType.DropItem)
            {
                DropComponent dropComponent = sendUnit.GetComponent<DropComponent>();
                if (dropComponent.IfDamgeDrop == 1 && !dropComponent.BeAttackPlayerList.Contains(unit.Id))
                {
                    return;
                }
            }

            M2C_CreateUnits createUnits = new M2C_CreateUnits();
            GetUnitInfo(sendUnit, createUnits);
            MessageHelper.SendToClient(unit, createUnits);
        }

        public static void GetUnitInfo(Unit sendUnit, M2C_CreateUnits createUnits)
        {
            switch (sendUnit.Type)
            {
                case UnitType.Player:
                case UnitType.JingLing:
                case UnitType.Pasture:
                case UnitType.Plant:
                case UnitType.Pet:
                case UnitType.Bullet:
                case UnitType.Npc:
                case UnitType.Stall:
                    createUnits.Units.Add(CreateUnitInfo(sendUnit));
                    break;
                case UnitType.Monster:
                    //NumericComponent numericComponent = sendUnit.GetComponent<NumericComponent>();
                    //int now_dead = numericComponent != null ? numericComponent.GetAsInt(NumericType.Now_Dead) : 0;
                    //if (now_dead == 0)
                    //{
                    //    createUnits.Spilings.Add(CreateSpilingInfo(sendUnit));
                    //    break;
                    //}
                    //long reviveTime = numericComponent != null ? numericComponent.GetAsLong(NumericType.ReviveTime) : 0;
                    //if (reviveTime > 0)
                    //{
                    //    createUnits.Spilings.Add(CreateSpilingInfo(sendUnit));
                    //}
                    createUnits.Spilings.Add(CreateSpilingInfo(sendUnit));
                    break;
                case UnitType.DropItem:
                    createUnits.Drops.Add(CreateDropInfo(sendUnit));
                    break;
                case UnitType.Chuansong:
                    createUnits.Transfers.Add(CreateTransferInfo(sendUnit));
                    break;
                //case UnitType.Npc:
                //    createUnits.Npcs.Add(CreateNpcInfo(sendUnit));
                //    break;
                //case UnitType.Pet:
                //    createUnits.Pets.Add(CreatePetInfo(sendUnit));
                //    break;
                
            }
        }

        public static void NoticeUnitRemove(Unit unit, Unit sendUnit)
        {
            M2C_RemoveUnits removeUnits = new M2C_RemoveUnits();
            removeUnits.Units.Add(sendUnit.Id);
            MessageHelper.SendToClient(unit, removeUnits);
        }

        public static SpilingInfo CreateSpilingInfo(Unit unit)
        {
            SpilingInfo spilingInfo = new SpilingInfo();
            unit.GetComponent<UnitInfoComponent>();
            spilingInfo.X = unit.Position.x;
            spilingInfo.Y = unit.Position.y;
            spilingInfo.Z = unit.Position.z;
            Vector3 forward = unit.Forward;
            spilingInfo.ForwardX = forward.x;
            spilingInfo.ForwardY = forward.y;
            spilingInfo.ForwardZ = forward.z;
            spilingInfo.UnitId = unit.Id;

            NumericComponent nc = unit.GetComponent<NumericComponent>();
            if (nc != null)
            {
                foreach ((int key, long value) in nc.NumericDic)
                {
                    if (key >= (int)NumericType.Max)
                    {
                        continue;
                    }
                    spilingInfo.Ks.Add(key);
                    spilingInfo.Vs.Add(value);
                }
            }

            if (unit.GetComponent<BuffManagerComponent>() != null)
            {
                spilingInfo.Buffs = unit.GetComponent<BuffManagerComponent>().GetMessageBuff();
                spilingInfo.Skills = unit.GetComponent<SkillManagerComponent>().GetMessageSkill();
            }
            //广播创建的是那个怪物ID
            spilingInfo.SkillId = unit.GetComponent<UnitInfoComponent>().EnergySkillId;
            spilingInfo.MonsterID = unit.ConfigId;
            return spilingInfo;
        }

        public static DropInfo CreateDropInfo(Unit unit)
        {
            DropInfo dropinfo = new DropInfo();
            dropinfo.UnitId = unit.Id;
            //DropType == 0 公共掉落 2保护掉落   1私有掉落
            DropComponent dropComponent = unit.GetComponent<DropComponent>();
            dropinfo.DropType = dropComponent.OwnerId > 0 ? 2 : 0;
            dropinfo.ItemID = dropComponent.ItemID;
            dropinfo.ItemNum = dropComponent.ItemNum;
            dropinfo.CellIndex = dropComponent.CellIndex;
            dropinfo.BeKillId = dropComponent.BeKillId;
            dropinfo.X = unit.Position.x;
            dropinfo.Y = unit.Position.y;
            dropinfo.Z = unit.Position.z;
            return dropinfo;
        }

        public static TransferInfo CreateTransferInfo(Unit unit)
        {
            TransferInfo transferinfo = new TransferInfo();
            ChuansongComponent chuansongComponent = unit.GetComponent<ChuansongComponent>();

            transferinfo.UnitId = unit.Id;
            transferinfo.X = unit.Position.x;
            transferinfo.Y = unit.Position.y;
            transferinfo.Z = unit.Position.z;
            transferinfo.CellIndex = chuansongComponent.CellIndex;
            transferinfo.Direction = chuansongComponent.DirectionType;
            transferinfo.TransferId = unit.ConfigId;
            return transferinfo;
        }

        public static int GetRealPlayer(Scene scene)
        {
            int realPlayer = 0;
            List<Unit> allunits = scene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                if (allunits[i].Type != UnitType.Player)
                {
                    continue;
                }
                if (allunits[i].GetComponent<UserInfoComponent>().UserInfo.RobotId == 0)
                {
                    realPlayer++;
                }
            }
            return realPlayer;
        }

        public static bool IsHaveNpc(Scene scene, int npcId)
        {
            List<Unit> allunits = scene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                if (allunits[i].Type != UnitType.Npc)
                {
                    continue;
                }
                if (allunits[i].ConfigId == npcId)
                {
                    return true;
                }
            }
            return false;
        }

        public static List<Unit> GetUnitList(Scene scene, int unitType)
        {
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

        public static List<Unit> GetUnitList(Scene scene, Vector3 position, int unitType, float distance)
        {
            List<Unit> units = new List<Unit>();
            List<Unit> allunits = scene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                if (allunits[i].Type != unitType)
                {
                    continue;
                }
                if (Vector3.Distance(allunits[i].Position, position ) > distance)
                {
                    continue;
                }

                units.Add(allunits[i]);
            }
            return units;
        }

        public static bool IsRobot(this Unit self)
        {
            return self.Type == UnitType.Player && self.GetComponent<UserInfoComponent>().UserInfo.RobotId > 0;
        }

        public static int GetWeaponSkill(this Unit self, int skillId, List<SkillPro> skillPros)
        {
            int EquipType = self.GetEquipType();
            return SkillHelp.GetWeaponSkill(skillId, EquipType, skillPros);
        }

        public static int GetEquipType(this Unit self)
        {
            if (self.Type != UnitType.Player)
            { 
                return ItemEquipType.Common;    
            }
            int itemId = self.GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Weapon);
            return ItemHelper.GetEquipType(self.ConfigId, itemId);
        }

        public static int GetWuqiItemID(this Unit self)
        {
            int itemId = self.GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Weapon);
            return itemId;
        }


        public static void SetBornPosition(this Unit self, Vector3 vector3, bool notice)
        {
            NumericComponent numericComponent = self.GetComponent<NumericComponent>();
            numericComponent.ApplyValue(NumericType.Born_X, (long)(vector3.x * 10000), notice);
            numericComponent.ApplyValue(NumericType.Born_Y, (long)(vector3.y * 10000), notice);
            numericComponent.ApplyValue(NumericType.Born_Z, (long)(vector3.z * 10000), notice);
        }

        public static Vector3 GetBornPostion(this Unit self)
        {
            NumericComponent numericComponent = self.GetComponent<NumericComponent>();
            return new Vector3(numericComponent.GetAsFloat(NumericType.Born_X),
                numericComponent.GetAsFloat(NumericType.Born_Y),
                numericComponent.GetAsFloat(NumericType.Born_Z));
        }

        public static List<Unit> GetUnitListByCamp(Scene scene, int unitType, int camp)
        {
            List<Unit> units = new List<Unit>();
            List<Unit> allunits = scene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                if (allunits[i].Type == unitType && allunits[i].GetBattleCamp() == camp)
                {
                    units.Add(allunits[i]);
                }
            }
            return units;
        }

        public static List<Unit> GetAliveUnitList(Scene scene, int unitType)
        {
            List<Unit> units = new List<Unit>();
            List<Unit> allunits = scene.GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < allunits.Count; i++)
            {
                if (allunits[i].Type == unitType && allunits[i].GetComponent<NumericComponent>().GetAsInt(NumericType.Now_Dead) == 0)
                {
                    units.Add(allunits[i]);
                }
            }
            return units;
        }

        public static int GetMaoXianExp(this Unit self)
        {
            int rechargeNum = self.GetComponent<NumericComponent>().GetAsInt(NumericType.RechargeNumber);
            rechargeNum *= 10;
            rechargeNum += self.GetComponent<NumericComponent>().GetAsInt(NumericType.MaoXianExp);
            return rechargeNum;
        }

        public static void RecordPostion(this Unit self, int sceneType, int sceneId)
        {
            bool record = false;
            NumericComponent numericComponent = self.GetComponent<NumericComponent>();
            if (!SceneConfigHelper.UseSceneConfig(sceneType) || sceneId == 0)
            {
                record = false;
            }
            else
            {
                if (!SceneConfigCategory.Instance.Contain(sceneId))
                {
                    record = false;
                    Log.Debug($"sceneconfig ==null:  sceneType: {sceneType} sceneId: {sceneId}");
                }
                else
                {
                    record = SceneConfigCategory.Instance.Get(sceneId).IfInitPosi == 1;
                }
            }
            if (record)
            {
                numericComponent.Set(NumericType.MainCity_X, self.Position.x);
                numericComponent.Set(NumericType.MainCity_Y, self.Position.y);
                numericComponent.Set(NumericType.MainCity_Z, self.Position.z);
            }
            else
            {
                numericComponent.Set(NumericType.MainCity_X, 0f);
                numericComponent.Set(NumericType.MainCity_Y, 0f);
                numericComponent.Set(NumericType.MainCity_Z, 0f);
            }
        }
    }
}