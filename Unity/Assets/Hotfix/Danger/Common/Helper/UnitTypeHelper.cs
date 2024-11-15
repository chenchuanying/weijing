
using MongoDB.Bson;

namespace ET
{
    public static class UnitTypeHelper
    {
        public static bool IsCanAttackUnit(this Unit self, Unit defend, bool checkdead = true)
        {
            if (self.Id == defend.Id)
            {
                return false;
            }

            if (!defend.IsCanBeAttack(checkdead))
            {
                return false;
            }

            MapComponent mapComponent = null;
            PetComponent petComponent = null;
#if SERVER
            mapComponent = self.DomainScene().GetComponent<MapComponent>();
            petComponent = self.Type == UnitType.Player ? self.GetComponent<PetComponent>() : null;
#else
            mapComponent = defend.ZoneScene().GetComponent<MapComponent>();
            petComponent = self.ZoneScene().GetComponent<PetComponent>();
#endif


#if SERVER
            if (mapComponent.SceneTypeEnum != SceneTypeEnum.Battle &&
                self.Type == UnitType.Monster && defend.Type == UnitType.Monster
             && self.MasterId == 0 && defend.MasterId == 0)
            {
                return false;
            }
#else
            if (mapComponent.SceneTypeEnum != SceneTypeEnum.Battle 
            && self.IsYeWaiMonster() && defend.IsYeWaiMonster())
            {
                return false;
            }
#endif

            if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.PetDungeon
             || mapComponent.SceneTypeEnum == (int)SceneTypeEnum.PetTianTi
             || mapComponent.SceneTypeEnum == (int)SceneTypeEnum.PetMing)
            {
                if (self.Type == UnitType.Player)
                {
                    return self.GetBattleCamp() != defend.GetBattleCamp();
                }
                else
                {
                    return defend.Type != UnitType.Player && self.GetBattleCamp() != defend.GetBattleCamp();
                }
            }


            if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.BaoZang 
             || mapComponent.SceneTypeEnum == (int)SceneTypeEnum.MiJing)
            {
                //0不允许pvp
                if (SceneConfigCategory.Instance.Get(mapComponent.SceneId).IfPVP == 0)
                {
                    return self.GetBattleCamp() != defend.GetBattleCamp() && !self.IsSameTeam(defend);
                }

                //0全体: 对全部造成伤害
                //1队伍 : 对自己队伍之外的人和怪造成伤害
                //2家族：对自己家族外的人和怪造成伤害
                //3和平：对任何人都不造成伤害
                int attackmode = self.GetAttackMode();
                if (attackmode == 1 && self.IsSameTeam(defend))
                {
                    return false;
                }
                if (attackmode == 2 && self.IsSameUnion(defend))
                {
                    return false;
                }
                if (attackmode == 3 && defend.Type == UnitType.Player)
                {
                    return false;
                }
                //允许pk地图
                return !self.IsMasterOrPet(defend, petComponent);
            }
            if (mapComponent.SceneTypeEnum == SceneTypeEnum.UnionRace)
            {
                if (self.IsSameUnion(defend))
                {
                    return false;
                }
                if (self.IsMasterOrPet(defend, petComponent))
                {
                    return false;
                }
                return true;
            }
            if (mapComponent.SceneTypeEnum == SceneTypeEnum.Union)
            {
                return self.GetBattleCamp() != defend.GetBattleCamp();
            }
            if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.Arena
             || mapComponent.SceneTypeEnum == (int)SceneTypeEnum.Solo
             || mapComponent.SceneTypeEnum == SceneTypeEnum.RunRace
             || mapComponent.SceneTypeEnum == SceneTypeEnum.OneChallenge )
            {
                //允许pk地图
                return  !self.IsMasterOrPet(defend, petComponent);
            }

            if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.Battle
             || mapComponent.SceneTypeEnum == SceneTypeEnum.Demon)
            {

                return self.GetBattleCamp() != defend.GetBattleCamp();
            }
            if (mapComponent.SceneTypeEnum == (int)SceneTypeEnum.JiaYuan)
            {
                return false;
            }
           return self.GetBattleCamp() != defend.GetBattleCamp() && !self.IsSameTeam(defend);
        }

        public static bool IsYeWaiMonster(this Unit self)
        {
            return self.Type == UnitType.Monster && self.GetComponent<NumericComponent>().GetAsLong(NumericType.MasterId) == 0;   
        }

        public static long GetTeamId(this Unit self)
        {
            return self.GetComponent<NumericComponent>().GetAsInt(NumericType.TeamId);
        }

        public static long GetUnionId(this Unit self)
        {
            return self.GetComponent<NumericComponent>().GetAsLong(NumericType.UnionId_0);
        }

        public static bool IsSameUnion(this Unit self, Unit other)
        {
            long teamid_1 = self.GetUnionId();
            long teamid_2 = other.GetUnionId();
            return teamid_1 == teamid_2 && teamid_1 != 0;
        }

        public static bool IsSameTeam(this Unit self, Unit other)
        {
            if (self.Id == other.Id)
            {
                return true;
            }
            long teamid_1 = self.GetTeamId();
            long teamid_2 = other.GetTeamId();
            return teamid_1 == teamid_2 && teamid_1 != 0;
        }

        public static bool MasterIsPlayer(this Unit self)
        {
            if (self.MasterId == 0)
            {
                return false;
            }
            Unit master = self.GetParent<UnitComponent>().Get(self.MasterId);
            if (master == null)
            {
                return false;
            }
            return master.Type == UnitType.Player;
        }

        public static bool IsMasterOrPet(this Unit self, Unit defend, PetComponent petComponent)
        {
            long masterId = self.GetComponent<NumericComponent>().GetAsLong(NumericType.MasterId);
            long othermaster = defend.GetComponent<NumericComponent>().GetAsLong(NumericType.MasterId);
            if (self.Type != UnitType.Player && masterId == defend.Id)
            {
                return true;
            }
            if (self.Type == UnitType.Player && othermaster == self.Id)
            {
                return true;
            }
            if (masterId > 0 && masterId == othermaster)
            {
                return true;
            }
            if (self.Type == UnitType.Player && petComponent.GetFightPetId() == defend.Id)
            {
                return true;
            }
            return self.Id == defend.Id;
        }

        public static long GetMasterId(this Unit self)
        {
            if (self.Type == UnitType.Player)
            {
                return self.Id;
            }
            if (self.Type == UnitType.Pet || self.Type == UnitType.Monster 
                || self.Type == UnitType.JingLing || self.Type == UnitType.Pasture)
            {
                return self.GetComponent<NumericComponent>().GetAsLong(NumericType.MasterId);
            }
            return 0;
        }

        public static int GetBattleCamp(this Unit self)
        {
            return self.GetComponent<NumericComponent>().GetAsInt(NumericType.BattleCamp);
        }

        public static int GetAttackMode(this Unit self)
        {
            return self.GetComponent<NumericComponent>().GetAsInt(NumericType.AttackMode);
        }

        public static bool IsCanBeAttack(this Unit self, bool checkdead = true)
        {
            if (self.Type == UnitType.Npc || self.Type == UnitType.DropItem
                || self.Type == UnitType.Chuansong || self.Type == UnitType.JingLing
                || self.Type == UnitType.Pasture || self.Type == UnitType.Plant
                || self.Type == UnitType.Bullet || self.Type == UnitType.Stall)
            {
                return false;
            }

            if (self.Type == UnitType.Monster && (self.GetMonsterType() == (int)MonsterTypeEnum.SceneItem))
            {
                return false;
            }
                
            StateComponent stateComponent = self.GetComponent<StateComponent>();
            if (stateComponent.StateTypeGet(StateTypeEnum.JiTui))
            {
                return false;
            }

            if (checkdead)
            {
                NumericComponent numericComponent = self.GetComponent<NumericComponent>();
                if (numericComponent.GetAsLong((int)NumericType.Now_Hp) <= 0
                    || numericComponent.GetAsLong((int)NumericType.Now_Dead) == 1)
                    return false;
            }

            return true;
        }

        public static int GetMonsterType(this Unit self)
        {
            return MonsterConfigCategory.Instance.Get(self.ConfigId).MonsterType;
        }

        public static bool IsSceneItem(this Unit self)
        {
            if (self.Type != UnitType.Monster)
            {
                return false;
            }
            return self.GetMonsterType() == MonsterTypeEnum.SceneItem;
        }

        public static bool IsBoss(this Unit self)
        {
            if (self.Type != UnitType.Monster)
            {
                return false;
            }
            return self.GetMonsterType() == MonsterTypeEnum.Boss;
        }

        public static bool IsJingLingMonster(this Unit self)
        {
            if (self.Type != UnitType.Monster)
            {
                return false;
            }
            int sonType = MonsterConfigCategory.Instance.Get(self.ConfigId).MonsterSonType;
            return sonType == 58 || sonType == 59;
        }

        public static bool IsPasture(this Unit self)
        {
            return self.Type == UnitType.Pasture;
        }

        public static bool IsChest(this Unit self)
        {
            if (self.Type != UnitType.Monster)
            {
                return false;
            }
            int sonType = MonsterConfigCategory.Instance.Get(self.ConfigId).MonsterSonType;
            return sonType == 55 || sonType == 56 || sonType == 57;
        }
    }
}
