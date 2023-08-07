﻿using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class Skill_Com_Summon_2 : SkillHandler
    {

        //初始化
        public override void OnInit(SkillInfo skillId, Unit theUnitFrom)
        {
            this.BaseOnInit(skillId, theUnitFrom);
        }

        //退出
        public override void OnExecute()
        {
            Unit theUnitFrom = this.TheUnitFrom;
            UnitInfoComponent unitInfoComponent = theUnitFrom.GetComponent<UnitInfoComponent>();
            if (unitInfoComponent.GetZhaoHuanNumber() >= 10)
            {
                return;
            }
            //'90000001;1;1;1;0.5,0.5,0.5,0.5,0.5;0,0,0,0,0
            //召唤ID；是否复刻玩家形象（0不是，1是）；范围；数量；血量比例,攻击比例,魔法比例,物防比例，魔防比例；血量固定值,攻击固定值，魔法固定值，物防固定值，魔防固定值
            string gameObjectParameter = SkillConfigCategory.Instance.Get(this.SkillInfo.WeaponSkillID).GameObjectParameter;
            string[] summonParList = gameObjectParameter.Split(';');
            int monsterId = int.Parse(summonParList[0]);
            float range = float.Parse(summonParList[2]);
            int number = int.Parse(summonParList[3]);
            for (int y = 0; y < number; y++)
            {
                //随机坐标
                float ran_x = RandomHelper.RandomNumberFloat(-1 * range, range);
                float ran_z = RandomHelper.RandomNumberFloat(-1 * range, range);
                Vector3 initPosi = new Vector3(theUnitFrom.Position.x + ran_x, theUnitFrom.Position.y, theUnitFrom.Position.z + ran_z); ;
                Unit unitMonster = UnitFactory.CreateMonster(theUnitFrom.DomainScene(), monsterId, initPosi, new CreateMonsterInfo()
                { Camp = theUnitFrom.GetBattleCamp(), MasterID = theUnitFrom.Id, AttributeParams = gameObjectParameter });
                unitInfoComponent.ZhaohuanIds.Add(unitMonster.Id);
            }

            if (theUnitFrom.ConfigId == 72009045)
            {
                Log.Console("Skill_Com_Summon_1: 72009045");
            }
        }

        public override void OnUpdate()
        {
            this.BaseOnUpdate();
            this.CheckChiXuHurt();
        }

        public override void OnFinished()
        {
            this.Clear();
        }
    }
}
