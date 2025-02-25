﻿using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 比较杂
    /// </summary>
    public class AttackRecordComponent : Entity, IAwake, IDestroy
    {
        public long AttackingId;

        public long BeAttackId;

        public long PetLockId;

        public long LastBelongTime;

        /// <summary>
        /// 抢夺BOSS显示当前怪物掉落归属 怪物表 DropType 字段为1 的显示 [掉落归属是统计玩家伤害最高的,
        /// 如果脱离战斗或者死亡清空伤害数据]
        /// </summary>
        public int DropType = 0;
        /// <summary>
        /// 玩家ID
        /// </summary>
        public Dictionary<long, long> BeAttackPlayerList = new Dictionary<long, long>();


        /// <summary>
        /// 战场召唤记录
        /// </summary>
        public List<BattleSummonInfo> BattleSummonList = new List<BattleSummonInfo>();  


    }
}
