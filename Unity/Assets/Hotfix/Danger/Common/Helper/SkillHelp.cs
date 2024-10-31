
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{ 
    public static class SkillHelp
    {

        public static string Skill_Other_ChongJi_1 = "Skill_Other_ChongJi_1";

        public static string Skill_XuanZhuan_Attack_2 = "Skill_XuanZhuan_Attack_2";

        public static List<string> NotCombatSkill = new List<string>() { "Act_11", "Act_12", "Act_13" };

        public static Dictionary<string, long> AckExitTime = new Dictionary<string, long>()
                {
                    {"Act_1", 700 },
                    {"Act_2", 1100 },
                    {"Act_3", 1100 },
                    {"Act_11", 900 },
                    {"Act_12", 900 },
                    {"Act_13", 900 },
        };


        /// <summary>
        /// 不触发被动技能
        /// </summary>
        public static List<int> NOPassiveSkill = new List<int>()    
        {
            62000004
        };

        
        
        public static Dictionary<int, (string, string)> BuffFallingFont = new Dictionary<int, (string, string)>()
        {
            { 100912, ("加速", "减速") },
            { 100612, ("物防增加", "物防减低") },
            { 1, (string.Empty, string.Empty) }
        };

        public static bool IsChongJi(string skillname)
        {
            return skillname.Equals(Skill_Other_ChongJi_1);
        }

        public static bool havePassiveSkillType(int[] typelist, int passType)
        {
            if (typelist == null)
            {
                return false;
            }
            for (int i = 0; i < typelist.Length; i++)
            {
                if (typelist[i] == passType)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 强化技能
        /// </summary>
        /// <param name="occ"></param>
        /// <param name="skillId"></param>
        /// <returns></returns>
        public static bool IsQiangHuaSkill(int occ, int skillId)
        {
            int baseskill = SkillConfigCategory.Instance.GetInitSkill(skillId); ;

            //if (skillId >= 64015201 && 64015201 <= 64015206)
            //{
            //    baseskill = 64015201;
            //}
            //else
            //{
            //    baseskill = SkillConfigCategory.Instance.GetInitSkill(skillId);
            //}
            
            if (baseskill == 0)
            {
                return false;
            }

            int[] baseList = OccupationConfigCategory.Instance.Get(occ).BaseSkill;
            for (int i = 0; i < baseList.Length; i++)
            {
                if (baseList[i] == baseskill)
                {
                    return true;
                }
            }
           
            return false;
        }

        /// <summary>
        /// 武器技能
        /// </summary>
        /// <param name="weapSkillId"></param>
        /// <returns></returns>
        public static int GetBaseSkill(int weapSkillId)
        {
            Dictionary<int, SkillWeaponConfig> skillWeapons = SkillWeaponConfigCategory.Instance.GetAll();

            foreach (var item in skillWeapons)
            {
                if(item.Key == weapSkillId || item.Value.InitSkillID == weapSkillId || item.Value.InitSkillID_1 == weapSkillId
                    || item.Value.InitSkillID_2 == weapSkillId || item.Value.InitSkillID_3 == weapSkillId || item.Value.InitSkillID_4 == weapSkillId)
                {

                    return item.Key;
                }
            }

            return weapSkillId;
        }

        public static int GetNewSkill(List<SkillPro> skillPros, int oldskiull)
        {
            if (skillPros == null)
            {
                return oldskiull;
            }

            List<int> findIds = new List<int>();    
            for (int i = 0; i < skillPros.Count; i++)
            {
                List<KeyValuePairInt> equipSkillds = null;
                SkillConfigCategory.Instance.EquipSkillList.TryGetValue(skillPros[i].SkillID, out equipSkillds);
                if (equipSkillds == null)
                {
                    continue;
                }
                if (findIds.Contains(skillPros[i].SkillID))
                {
                    continue;
                }

                for (int skillindex = 0; skillindex < equipSkillds.Count; skillindex++)
                {
                    if (equipSkillds[skillindex].KeyId == oldskiull)
                    {
                        findIds.Add(skillPros[i].SkillID);
                        oldskiull =(int)equipSkillds[skillindex].Value;
                        break;
                    }
                }
            }
            return oldskiull;
        }

        public static int GetWeaponSkill(int skillId, int weapType, List<SkillPro> skillPros)
        {
            SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillId);

            if (skillConfig.WeaponType != 0)
            {
                if (!SkillWeaponConfigCategory.Instance.Contain(skillId))
                {
                    Log.Error("错误的技能配置[WeaponType]_:" + skillId.ToString());
                    return skillId;
                }

                SkillWeaponConfig skillWeaponConfig = SkillWeaponConfigCategory.Instance.Get(skillId);
                if (skillWeaponConfig == null)
                    return skillId;
                switch (weapType)
                {
                    case ItemEquipType.Sword:
                        skillId = skillWeaponConfig.InitSkillID_1;
                        break;
                    case ItemEquipType.Knife:
                        skillId = skillWeaponConfig.InitSkillID_2;
                        break;
                    case ItemEquipType.Wand:
                        skillId = skillWeaponConfig.InitSkillID_3;
                        break;
                    case ItemEquipType.Book:
                        skillId = skillWeaponConfig.InitSkillID_4;
                        break;
                    case ItemEquipType.Bow:
                        skillId = skillWeaponConfig.InitSkillID_5;
                        break;
                    default:
                        skillId = skillWeaponConfig.InitSkillID;
                        break;
                }

               
                int weaponid = skillId != 0 ? skillId : skillWeaponConfig.InitSkillID;
                if (SkillConfigCategory.Instance.Get(weaponid).SkillLv == 0)
                {
                    return weaponid;
                }

                return GetNewSkill(skillPros, weaponid); 
            }
            else
            {
                if (skillConfig.SkillLv == 0)
                { 
                    return skillId;
                }

                int newskill = GetNewSkill(skillPros, skillId);
                newskill = newskill != 0 ? newskill : skillId;
                return newskill;

            }
        }

        /// <summary>
        /// 模型高度
        /// </summary>
        /// <param name="unitType"></param>
        /// <param name="configId"></param>
        /// <returns></returns>
        public static float GetCenterHigh(int unitType, int configId)
        {
            if (unitType != UnitType.Monster)
            {
                return 0.5f;
            }
            return (float)MonsterConfigCategory.Instance.Get(configId).ActBasePosiY;
        }

    }
}
