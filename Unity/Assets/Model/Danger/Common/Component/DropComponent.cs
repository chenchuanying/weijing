﻿using System.Collections.Generic;

namespace ET
{
    public class DropComponent : Entity, IAwake
    {
        public int ItemID;
        public int ItemNum;
        public int DropType;  //0 公共掉落    1私人掉落 2 保护掉落 3 归属掉落

        public int CellIndex;  //喜从天降格子位

        public long OwnerId;
        public long ProtectTime;

        public long BeKillId;
#if SERVER
        public int IfDamgeDrop;
        public List<long> BeAttackPlayerList = new List<long>();
#else 
        public DropInfo DropInfo;
#endif
    }
}
