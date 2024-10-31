using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
	/// <summary>
	/// 监视hp数值变化，改变血条值
	/// </summary>
	[NumericWatcher(NumericType.Now_Hp)]
	public class NumericWatcher_Now_Hp : INumericWatcher
	{
		public void Run(EventType.NumericChangeEvent args)
		{
			Unit unit = args.Defend;
			if (unit == null || unit.IsDisposed)
			{
				Log.TraceInfo("NumericType.Now_Hp == null");
			}
			UnitInfoComponent unitInfoComponent = unit.GetComponent<UnitInfoComponent>();
			NumericComponent numericComponentDefend = unit.GetComponent<NumericComponent>();
#if SERVER
			Scene DomainScene = args.Defend.DomainScene();
			MapComponent mapComponent = DomainScene.GetComponent<MapComponent>();	
			int sceneTypeEnum = mapComponent.SceneTypeEnum;
			int sceneId = mapComponent.SceneId;	

			if (args.NewValue <= 0 && numericComponentDefend.GetAsInt(NumericType.Now_Dead) == 1)
			{
				return;
			}

            Unit attack = args.Attack;
            if (args.NewValue <= 0 && numericComponentDefend.GetAsInt(NumericType.Now_Dead) == 0)
			{
                if (attack == null || attack.IsDisposed)
                {
                    Log.Warning($"NumericWatcher_Now_Hp.args.NewValue <= 0: {attack.Type}");
                }

                // 死亡召唤
                if (args.SkillId > 0 && SkillConfigCategory.Instance.Get(args.SkillId).GameObjectName == "Skill_Com_Summon_5")
                {
					ZhaoHuanHelper.DeadCreateZhaoHuan(args);
                }

                unit.GetComponent<HeroDataComponent>().OnKillZhaoHuan(attack);
                unit.GetComponent<HeroDataComponent>().OnDead(attack);
                unit.GetComponent<HeroDataComponent>().PlayDeathSkill(attack);
               
                //unit.GetComponent<HeroDataComponent>().OnKillZhaoHuan(args.Attack);
                //unit.GetComponent<HeroDataComponent>().PlayDeathSkill();
                //unit.GetComponent<HeroDataComponent>().OnDead(args.Attack);
            }

			if (attack != null && !attack.IsDisposed && (args.OldValue > args.NewValue))
			{
				Unit player = attack;
               
                if (attack.MasterId > 0 &&( attack.Type == UnitType.Pet || attack.Type == UnitType.Monster))
				{
                    player = attack.GetParent<UnitComponent>().Get(attack.MasterId);
				}

                if (player != null && player.Type != UnitType.Player)
                {
                    player = null;
                }

                if (sceneTypeEnum == (int)SceneTypeEnum.CellDungeon)   //个人副本接受到的伤害
				{
					DomainScene.GetComponent<CellDungeonComponent>().OnRecivedHurt(args.OldValue - args.NewValue);
				}
				if (sceneTypeEnum == (int)SceneTypeEnum.TeamDungeon && player != null)  //组队副本输出伤害
				{
					DomainScene.GetComponent<TeamDungeonComponent>()?.OnUpdateDamage(player, unit, args.OldValue - args.NewValue);
				}
				if (sceneTypeEnum == (int)SceneTypeEnum.MiJing && player != null)  //秘境伤害
				{
					DomainScene.GetComponent<MiJingComponent>()?.OnUpdateDamage(player,unit, args.OldValue - args.NewValue);
				}
                if (sceneTypeEnum == (int)SceneTypeEnum.TrialDungeon && player != null)  //试炼副本伤害
                {
                    DomainScene.GetComponent<TrialDungeonComponent>()?.OnUpdateDamage(player, attack, unit, args.OldValue - args.NewValue, args.SkillId);
                }
            }
#else
			long nowHpValue = numericComponentDefend.GetAsLong(NumericType.Now_Hp);
			long costHp = (nowHpValue - args.OldValue);
		
			EventType.UnitHpUpdate.Instance.Defend = unit;
			EventType.UnitHpUpdate.Instance.ChangeHpValue = costHp;
			EventType.UnitHpUpdate.Instance.DamgeType = args.DamgeType;
			EventType.UnitHpUpdate.Instance.SkillID = args.SkillId;
			EventType.UnitHpUpdate.Instance.Attack = args.Attack;
            Game.EventSystem.PublishClass(EventType.UnitHpUpdate.Instance);
#endif
		}
	}


	[NumericWatcher(NumericType.Now_Dead)]
	public class NumericWatcher_Now_Dead : INumericWatcher
	{
		public void Run(EventType.NumericChangeEvent args)
		{
#if !SERVER
			Unit unit = args.Defend;
			if (args.NewValue == 0)//复活
			{
                unit.Position = unit.GetBornPostion();
				unit.GetComponent<StateComponent>().SetRigidityEndTime(0);
                EventType.UnitRevive.Instance.Unit = unit;
				Game.EventSystem.PublishClass(EventType.UnitRevive.Instance);
			}
			if (args.NewValue == 1)//死亡
			{
                unit.GetComponent<HeroDataComponent>().OnDead(args.Attack);
				EventType.UnitDead.Instance.Unit = unit;
				Game.EventSystem.PublishClass(EventType.UnitDead.Instance);
			}
#endif
        }
    }
}