﻿using UnityEngine;

namespace ET
{
    //接受Buff改变消息
    [MessageHandler]
    public class M2C_UnitBuffUpdateHandler : AMHandler<M2C_UnitBuffUpdate>
    {
        protected override void Run(Session session, M2C_UnitBuffUpdate message)
        {
            //抛出事件处理属性改变
            Unit msgUnitBelongTo = session.ZoneScene().CurrentScene()?.GetComponent<UnitComponent>().Get(message.UnitIdBelongTo);
            if (msgUnitBelongTo == null)
            {
                Log.Debug($"M2C_UnitBuffUpdate :{message.UnitIdBelongTo} == null");
                return;
            }

            switch (message.BuffOperateType)
            {
                case 1: //增加
                    BuffData buffData = new BuffData();
                    buffData.TargetAngle = 0;
                    buffData.BuffId = (int)message.BuffID;
                    buffData.Spellcaster = message.Spellcaster;
                    buffData.BuffEndTime = message.BuffEndTime;
                    buffData.UnitType = message.UnitType;
                    buffData.UnitConfigId = message.UnitConfigId;
                    buffData.SkillId = message.SkillId;
                    msgUnitBelongTo.GetComponent<BuffManagerComponent>().BuffFactory(buffData);
                    break;
                case 2: //移除
                    msgUnitBelongTo.GetComponent<BuffManagerComponent>().RemoveBuff(message.BuffID);
                    break;
                case 3: //重置
                    ABuffHandler buffHandler = msgUnitBelongTo.GetComponent<BuffManagerComponent>().GetBuffById(message.BuffID);
                    if (buffHandler != null)
                    {
                        buffHandler.OnReset();
                    }
                    break;
            }
        }
    }
}
