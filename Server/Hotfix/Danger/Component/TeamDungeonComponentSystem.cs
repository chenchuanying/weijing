﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [Timer(TimerType.TeamDropTimer)]
    public class TeamDropTimer : ATimer<TeamDungeonComponent>
    {
        public override void Run(TeamDungeonComponent self)
        {
            try
            {
                self.Check();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    [ObjectSystem]
    public class TeamDungeonComponentAwakeSystem : AwakeSystem<TeamDungeonComponent>
    {
        public override void Awake(TeamDungeonComponent self)
        {
            self.EnterTime = 0;
            self.TeamInfo = null;
            self.BoxReward.Clear();
            self.ItemFlags.Clear();
            self.TeamDropItems.Clear();
            self.KillBossList.Clear();
            self.TeamPlayers.Clear();
        }
    }

    [ObjectSystem]
    public class TeamDungeonComponentDestroySystem : DestroySystem<TeamDungeonComponent>
    {
        public override void Destroy(TeamDungeonComponent self)
        {
            TimerComponent.Instance.Remove(ref self.Timer);
        }
    }

    public static class TeamDungeonComponentSystem
    {

        public static void InitPlayerList(this TeamDungeonComponent self)
        {
            for (int i = 0; i < self.TeamInfo.PlayerList.Count; i++)
            {
                if (self.TeamPlayers.ContainsKey(self.TeamInfo.PlayerList[i].UserID))
                {
                    Log.Warning($"InitPlayerList.Error: {self.TeamInfo.PlayerList[i].UserID}");
                    continue;
                }
                self.TeamPlayers.Add(self.TeamInfo.PlayerList[i].UserID, self.TeamInfo.PlayerList[i]);
            }
        }

        public static int InitPlayerNumber(this TeamDungeonComponent self)
        {
            int InitPlayerNumber = 0;
            for (int i = 0; i < self.TeamInfo.PlayerList.Count; i++)
            {
                InitPlayerNumber += (self.TeamInfo.PlayerList[i].RobotId == 0 ? 1 : 0);
            }
            return InitPlayerNumber;
        }

        public static void Check(this TeamDungeonComponent self)
        {
            if (self.TeamDropItems.Count == 0)
            {
                //TimerComponent.Instance.Remove(ref self.Timer);
                return;
            }
            long serverTime = TimeHelper.ServerNow();
            int playerCount = UnitHelper.GetUnitList(self.DomainScene(),  UnitType.Player).Count;
            for (int i = self.TeamDropItems.Count - 1; i >= 0; i--)
            {
                TeamDropItem teamDropItem = self.TeamDropItems[i];
                if (teamDropItem.EndTime == -1)
                {
                    continue;
                }
                List<long> needIds = teamDropItem.NeedPlayers;
                List<long> giveIds = teamDropItem.GivePlayers;
                bool finish = serverTime >= teamDropItem.EndTime || (needIds.Count + giveIds.Count >= playerCount);
                if (!finish)
                {
                    continue;
                }
                teamDropItem.EndTime = -1;

                int maxNumber = 0;
                //全部放弃则默认分配
                if (giveIds.Count >= playerCount || needIds.Count == 0)
                {
                    needIds.AddRange(giveIds);
                }
                List<int> randomNumbers = new List<int>();
                for (int p = 0; p < needIds.Count; p++)
                {
                    randomNumbers.Add(RandomHelper.RandomNumber(1, 100));
                    if (randomNumbers[p] > maxNumber)
                    {
                        maxNumber = randomNumbers[p];
                    }
                }
                int onwerIndex = randomNumbers.IndexOf(maxNumber);
                long unitid = needIds.Count == 0 ? 0 : teamDropItem.NeedPlayers[onwerIndex];
                Unit unit = self.DomainScene().GetComponent<UnitComponent>().Get(unitid);
                if (unit != null)
                {
                    List<RewardItem> rewardItems = new List<RewardItem>();
                    rewardItems.Add(new RewardItem() { ItemID = teamDropItem.DropInfo.ItemID, ItemNum = teamDropItem.DropInfo.ItemNum });
                    bool ret = unit.GetComponent<BagComponent>().OnAddItemData(rewardItems, string.Empty, $"{ItemGetWay.PickItem}_{TimeHelper.ServerNow()}");
                    Log.Debug($"TeamDungeonComponent.DropInfo： {unit.Id}  {teamDropItem.DropInfo.ItemID} {teamDropItem.DropInfo.ItemNum} {ret}");
                    if (ret)
                    {
                        FubenHelp.SendTeamPickMessage(unit, teamDropItem.DropInfo, needIds, randomNumbers);
                        self.DomainScene().GetComponent<UnitComponent>().Remove(teamDropItem.DropInfo.UnitId);       //移除掉落ID
                        continue;
                    }
                }

                if (!self.ItemFlags.ContainsKey(teamDropItem.DropInfo.UnitId) && unitid  > 0)
                {
                    self.ItemFlags.Add(teamDropItem.DropInfo.UnitId, unitid);
                }
            }
        }

        public static TeamDropItem AddTeamDropItem(this TeamDungeonComponent self, Unit unit, DropInfo dropInfo)
        {
            for (int i = self.TeamDropItems.Count - 1; i >=  0; i--)
            {
                if (self.TeamDropItems[i].DropInfo.UnitId == dropInfo.UnitId)
                {
                    self.Check();
                    return null;
                }
            }

            TeamDropItem teamDropItem = self.AddChildWithId<TeamDropItem>(dropInfo.UnitId);
            teamDropItem.DropInfo = dropInfo;
            teamDropItem.EndTime = TimeHelper.ServerNow() + 40 * 1000;

            self.TeamDropItems.Add(teamDropItem);
            if (self.Timer == 0)
            {
                self.Timer = TimerComponent.Instance.NewRepeatedTimer(1000, TimerType.TeamDropTimer, self);
            }
            M2C_TeamPickMessage teamPickMessage = self.m2C_TeamPickMessage;
            teamPickMessage.DropItems.Clear();
            teamPickMessage.DropItems.Add(dropInfo);

            List<Unit> players = UnitHelper.GetUnitList(unit.DomainScene(),UnitType.Player);
            MessageHelper.SendToClient(players, teamPickMessage);
            return teamDropItem;
        }

        public static bool IsHavePlayer(this TeamDungeonComponent self)
        {
            bool haveplayer = false;
            List<Entity> units = self.DomainScene().GetComponent<UnitComponent>().Children.Values.ToList() ;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i] as Unit;
                if (unit.Type == UnitType.Player)
                {
                    haveplayer = true;
                    break;
                }
            }
            return haveplayer;
        }

        public static void OnUpdateDamage(this TeamDungeonComponent self, Unit attack, Unit defend, long damage)
        {
            if (defend.Type != UnitType.Monster)
            {
                return;
            }

            List<TeamPlayerInfo> vs = self.TeamInfo.PlayerList;
            for (int i = 0; i < vs.Count; i++)
            {
                if (vs[i].UserID == attack.Id)
                {
                    vs[i].Damage += (int)damage;
                }
            }
           
            if (TimeHelper.ServerNow() - self.EnterTime < 5000)
            {
                return;
            }
            self.EnterTime = TimeHelper.ServerNow();
            List<Unit> allPlayer = UnitHelper.GetUnitList(self.DomainScene(), UnitType.Player);
            for (int i = 0; i < allPlayer.Count; i++)
            {
                M2C_SyncMiJingDamage m2C_SyncMiJingDamage = new M2C_SyncMiJingDamage();
                m2C_SyncMiJingDamage.DamageList.AddRange(self.TeamInfo.PlayerList);
                MessageHelper.SendToClient(allPlayer[i], m2C_SyncMiJingDamage);
            }
        }

        public static void OnKillEvent(this TeamDungeonComponent self, Unit unit)
        {
            if (unit.Type != UnitType.Monster)
            {
                return;
            }

            if (unit.IsBoss())
            {
                Unit leader = unit.GetParent<UnitComponent>().Get(self.TeamInfo.TeamId);
                if (leader != null && self.FubenType == TeamFubenType.XieZhu)
                {
                    //协助副本掉落
                    int dropId = int.Parse(GlobalValueConfigCategory.Instance.Get(75).Value);
                    UnitFactory.CreateDropItems(leader, unit, 1, dropId, "1");
                }
                if (self.FubenType == TeamFubenType.ShenYuan)
                {
                    //深渊副本掉落
                    int dropId = int.Parse(GlobalValueConfigCategory.Instance.Get(77).Value);
                    UnitFactory.CreateDropItems(null, unit, 0, dropId, "1");
                }

                self.BossDeadPosition = unit.Position;
            }

            SceneConfig sceneConfig = SceneConfigCategory.Instance.Get(self.TeamInfo.SceneId);
            if (unit.ConfigId != sceneConfig.BossId)
            {
                return;
            }

            M2C_TeamDungeonSettlement m2C_FubenSettlement = new M2C_TeamDungeonSettlement();
            m2C_FubenSettlement.PassTime = 5*60 * 1000;
            m2C_FubenSettlement.PlayerList = self.TeamInfo.PlayerList;

            DropHelper.DropIDToDropItem_2(sceneConfig.BoxDropID, m2C_FubenSettlement.RewardExtraItem);

            DropHelper.DropIDToDropItem_2(sceneConfig.BoxDropID, m2C_FubenSettlement.ReardList);
            DropHelper.DropIDToDropItem_2(sceneConfig.BoxDropID, m2C_FubenSettlement.ReardList);
            DropHelper.DropIDToDropItem_2(sceneConfig.BoxDropID, m2C_FubenSettlement.ReardList);

            DropHelper.DropIDToDropItem_2(sceneConfig.BoxDropID, m2C_FubenSettlement.ReardListExcess);
            DropHelper.DropIDToDropItem_2(sceneConfig.BoxDropID, m2C_FubenSettlement.ReardListExcess);
            DropHelper.DropIDToDropItem_2(sceneConfig.BoxDropID, m2C_FubenSettlement.ReardListExcess);

            //最高伤害额外奖励
            long idExtra = 0;
            int damageMax = 0;
            int damageTotal = 0;    
            Dictionary<long, int> hurtList = new Dictionary<long, int>();
            for (int i = 0; i < m2C_FubenSettlement.PlayerList.Count; i++)
            {
                TeamPlayerInfo teamPlayerInfo = m2C_FubenSettlement.PlayerList[i];
                if (teamPlayerInfo.Damage >= damageMax)
                {
                    damageMax = teamPlayerInfo.Damage;
                    idExtra = teamPlayerInfo.UserID;
                }
                damageTotal += teamPlayerInfo.Damage;
                if(!hurtList.ContainsKey(teamPlayerInfo.UserID))
                {
                    hurtList.Add(teamPlayerInfo.UserID, teamPlayerInfo.Damage);
                }
            }

            //TeamDungeonHurt_136
            List<Unit> units = unit.GetParent<UnitComponent>().GetAll();
            for (int i = 0; i < units.Count; i++)
            {
                Unit unititem = units[i] as Unit;
                if (unititem.Type != UnitType.Player || unititem.IsRobot())
                {
                    continue;
                }

                int hurtvalue = hurtList[unititem.Id];
                int hurtRate = (int)(hurtvalue * 100f / damageTotal);
                unititem.GetComponent<TaskComponent>().TriggerTaskEvent( TaskTargetType.TeamDungeonHurt_136, self.TeamInfo.SceneId, hurtRate);

                unititem.GetComponent<TaskComponent>().OnPassTeamFuben();
                unititem.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.PassTeamFubenNumber_20, 0, 1);
                if (self.FubenType == TeamFubenType.ShenYuan)
                {
                    unititem.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.PassTeamShenYuanNumber_21, 0, 1);
                }
                if (unititem.GetComponent<UserInfoComponent>().UserInfo.UserId == idExtra)
                {
                    unititem.GetComponent<BagComponent>().OnAddItemData(m2C_FubenSettlement.RewardExtraItem, string.Empty, $"{ItemGetWay.FubenGetReward}_{TimeHelper.ServerNow()}");
                }
                MessageHelper.SendToClient(unititem, m2C_FubenSettlement);
            }

            (self.Parent.Parent as TeamSceneComponent).OnDungeonOver(self.TeamInfo.TeamId);
        }
    }
}
