﻿using System.Collections.Generic;

namespace ET
{
    public class BuffManagerComponent : Entity, IAwake, IDestroy
    {
        public long Timer;
        public int SceneType;

#if !SERVER
        public List<KeyValuePair> t_Buffs = new List<KeyValuePair>();
        public List<ABuffHandler> m_Buffs = new List<ABuffHandler>();
#else

        public List<BuffHandler> m_Buffs = new List<BuffHandler>();

        public List<KeyValuePairLong> m_BuffRecord = new List<KeyValuePairLong>();  //buffid_增删_
        public readonly M2C_UnitBuffUpdate m2C_UnitBuffUpdate = new M2C_UnitBuffUpdate();
        public readonly M2C_UnitBuffRemove m2C_UnitBuffRemove = new M2C_UnitBuffRemove();   
#endif

    }
}
