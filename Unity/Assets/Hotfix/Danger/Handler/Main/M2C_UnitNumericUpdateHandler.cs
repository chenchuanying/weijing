﻿using ET;

namespace ET
{

    //接受属性改变消息
    [MessageHandler]
    public class M2C_UnitNumericUpdateHandler : AMHandler<M2C_UnitNumericUpdate>
    {
        protected override void  Run(Session session, M2C_UnitNumericUpdate message)
        {
            //Log.Info("获取最新的属性...");
            //同步我当前的属性
            Scene zongScene = session.ZoneScene();
            Scene currentScene = zongScene.CurrentScene();
            if (currentScene == null)
            {
                Log.ILog.Debug($"currentScene == null   {message.NumericType}");
                return;
            }
            Unit nowNunt = currentScene.GetComponent<UnitComponent>().Get(message.UnitId);
            if (nowNunt == null)
            {
                return;
            }

            //客户端的NumericComponent.Set不会抛出事件。需要自己手动抛出
            Unit attack = currentScene.GetComponent<UnitComponent>().Get(message.AttackId);
            NumericComponent numericComponent = nowNunt.GetComponent<NumericComponent>();
            numericComponent.ApplyValue(attack, message.NumericType, message.NewValue, message.SkillId, true, message.DamgeType);
            //EventType.NumericChangeEvent args = EventType.NumericChangeEvent.Instance;
            //args.Defend = nowNunt;
            //args.Attack = currentScene.GetComponent<UnitComponent>().Get(message.AttackId);
            //args.NumericType = message.NumericType;
            //args.OldValue = message.OldValue;
            //args.NewValue = message.NewValue;
            //args.SkillId = message.SkillId;
            //args.DamgeType = message.DamgeType;
            //Game.EventSystem.PublishClass(args);

        }
    }
}
