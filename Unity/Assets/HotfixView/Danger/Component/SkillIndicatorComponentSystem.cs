﻿using System;
using UnityEngine;

namespace ET
{


    public class SkillIndicatorComponentAwakeSystem : AwakeSystem<SkillIndicatorComponent>
    {
        public override void Awake(SkillIndicatorComponent self)
        {
            self.SkillIndicator = null;
            self.MainCamera = self.ZoneScene().GetComponent<UIComponent>().MainCamera;
        }
    }


    public class SkillIndicatorComponentDestroySystem : DestroySystem<SkillIndicatorComponent>
    {
        public override void Destroy(SkillIndicatorComponent self)
        {
            self.RecoveryEffect();
        }
    }

    public static class SkillIndicatorComponentSystem
    {

        public static void BeginEnterScene(this SkillIndicatorComponent self)
        {
            self.RecoveryEffect();
        }

        public static void OnSelfDead(this SkillIndicatorComponent self)
        {
            self.RecoveryEffect();
        }

        public static string GetIndicatorPath(this SkillIndicatorComponent self, int skillZhishiType)
        {
            string effect = "";
            switch (skillZhishiType)
            {
                case SkillZhishiType.CommonAttack:
                    effect = "SkillZhishi/Skill_CommonAttack";
                    break;
                case SkillZhishiType.Position:
                    effect = "SkillZhishi/Skill_Position";
                    break;
                case SkillZhishiType.Line:
                    effect = "SkillZhishi/Skill_Dir";
                    break;
                case SkillZhishiType.Angle60:
                    effect = "SkillZhishi/Skill_Area_60";
                    break;
                case SkillZhishiType.Angle120:
                    effect = "SkillZhishi/Skill_Area_120";
                    break;
               case  SkillZhishiType.TargetOnly:
                    effect = "SkillZhishi/Skill_Position";
                    break;
            }

            return ABPathHelper.GetEffetPath(effect);
        }

        public static void OnLoadGameObject(this SkillIndicatorComponent self, GameObject gameObject, long instanceId)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            SkillIndicatorItem skillIndicatorItem = self.SkillIndicator;
            if (self.IsDisposed || skillIndicatorItem == null || unit == null)
            {
                GameObject.DestroyImmediate(gameObject);
                return;
            }
            if (skillIndicatorItem.SkillZhishiType != instanceId)
            {
                GameObject.DestroyImmediate(gameObject);
                return;
            }
            if (skillIndicatorItem.GameObject != null)
            {
                GameObject.DestroyImmediate(skillIndicatorItem.GameObject);
            }
            UICommonHelper.SetParent(gameObject, GlobalComponent.Instance.Unit.gameObject);
            gameObject.transform.localPosition = unit.Position;
            skillIndicatorItem.GameObject = gameObject;
            skillIndicatorItem.GameObject.SetActive(true);
            self.InitZhishiEffect(skillIndicatorItem);
            self.OnMouseDrag(Vector2.zero);
        }

        /// <summary>
        /// 显示技能指示器
        /// </summary>
        public static void  ShowSkillIndicator(this SkillIndicatorComponent self, SkillConfig skillconfig)
        {
            //0  立即释放,自身中心点
            //1  技能指示器
            //2  立即释放,目标中心点
            self.RecoveryEffect();
            self.mSkillConfig = skillconfig;
            self.SkillRangeSize = (float)self.mSkillConfig.SkillRangeSize;
            SkillIndicatorItem skillIndicatorItem = new SkillIndicatorItem();
            self.SkillIndicator = skillIndicatorItem;
            skillIndicatorItem.SkillZhishiType = skillconfig.SkillTargetType == SkillTargetType.TargetOnly ? SkillZhishiType.TargetOnly : skillconfig.SkillZhishiType;
            
            skillIndicatorItem.EffectPath = self.GetIndicatorPath(skillIndicatorItem.SkillZhishiType);
            GameObjectPoolComponent.Instance.AddLoadQueue(skillIndicatorItem.EffectPath, skillIndicatorItem.SkillZhishiType, self.OnLoadGameObject);
        }

        /// <summary>
        /// 普攻预警
        /// </summary>
        /// <param name="self"></param>
        public static void  ShowCommonAttackZhishi(this SkillIndicatorComponent self)
        {
            self.RecoveryEffect();
            SkillIndicatorItem skillIndicatorItem = new SkillIndicatorItem();
            self.SkillIndicator = skillIndicatorItem;
            skillIndicatorItem.SkillZhishiType = SkillZhishiType.CommonAttack;
            skillIndicatorItem.EffectPath = self.GetIndicatorPath(skillIndicatorItem.SkillZhishiType);
            GameObjectPoolComponent.Instance.AddLoadQueue(skillIndicatorItem.EffectPath, skillIndicatorItem.SkillZhishiType, self.OnLoadGameObject);
        }

        public static void InitZhishiEffect(this SkillIndicatorComponent self, SkillIndicatorItem skillIndicatorItem)
        {
            try
            {
                switch (skillIndicatorItem.SkillZhishiType)
                {
                    case SkillZhishiType.CommonAttack:
                        int occ = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Occ;
                        float[] scaleList = new float[3] { 6f, 12f, 6f };
                        //法师加长
                        if (occ == 2)
                        {
                            scaleList = new float[3] { 12f, 12f, 12f };
                        }

                        //猎人加长
                        if (occ == 3)
                        {
                            scaleList = new float[3] { 15f, 15f, 15f };
                        }

                        skillIndicatorItem.GameObject.Get<GameObject>("Skill_Area").transform.localScale = Vector3.one * scaleList[occ - 1];
                        break;
                    case SkillZhishiType.Position:
                        float innerRadius = (float)self.mSkillConfig.DamgeRange[0] * 2f;
                        float outerRadius = (float)self.mSkillConfig.SkillRangeSize * 2f;   //半径 * 2
                        skillIndicatorItem.GameObject.Get<GameObject>("Skill_InnerArea").transform.localScale = Vector3.one * innerRadius;
                        skillIndicatorItem.GameObject.Get<GameObject>("Skill_Area").transform.localScale = Vector3.one * outerRadius;
                        break;

                    case SkillZhishiType.Line:
                        innerRadius = (float)self.mSkillConfig.SkillRangeSize * 2f;
                        outerRadius = (float)self.mSkillConfig.SkillRangeSize * 6f;
                        skillIndicatorItem.GameObject.Get<GameObject>("Skill_Dir").transform.localScale = Vector3.one * innerRadius;
                        skillIndicatorItem.GameObject.Get<GameObject>("Skill_Area").transform.localScale = Vector3.one * outerRadius;
                        break;
                    case SkillZhishiType.Angle60:
                        innerRadius = (float)self.mSkillConfig.SkillRangeSize * 1f;
                        outerRadius = (float)self.mSkillConfig.SkillRangeSize * 6f;
                        skillIndicatorItem.GameObject.Get<GameObject>("Skill_Area_60").transform.localScale = Vector3.one * innerRadius;
                        skillIndicatorItem.GameObject.Get<GameObject>("Skill_Area").transform.localScale = Vector3.one * outerRadius;
                        break;
                    case SkillZhishiType.Angle120:
                        innerRadius = (float)self.mSkillConfig.SkillRangeSize * 1f;
                        outerRadius = (float)self.mSkillConfig.SkillRangeSize * 6f;
                        skillIndicatorItem.GameObject.Get<GameObject>("Skill_Area_120").transform.localScale = Vector3.one * innerRadius;
                        skillIndicatorItem.GameObject.Get<GameObject>("Skill_Area").transform.localScale = Vector3.one * outerRadius;
                        break;
                    case SkillZhishiType.TargetOnly:
                        innerRadius = (float)self.mSkillConfig.DamgeRange[0] * 2f;
                         outerRadius = (float)self.mSkillConfig.SkillRangeSize * 2f;   //半径 * 2
                        skillIndicatorItem.GameObject.Get<GameObject>("Skill_InnerArea").transform.localScale = Vector3.one * innerRadius;
                        skillIndicatorItem.GameObject.Get<GameObject>("Skill_Area").transform.localScale = Vector3.one * outerRadius;

                        break;

                }
            }
            catch (Exception ex)
            {
                Log.Error("SkillIndicator1: " + ex.ToString());
            }
        }

        public static bool ToSelfSkill(this SkillIndicatorComponent self, int skillId)
        {
            return skillId >= 62023401 && skillId <= 62023406;
        }

        //鼠标按下
        /// <summary>
        /// /0 默认为目标点  1 默认为自身位置 2 当前朝向最远处
        /// </summary>
        /// <param name="self"></param>
        /// <param name="targetId"></param>
        public static void OnMouseDown(this SkillIndicatorComponent self, long targetId = 0)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            Unit target = unit.GetParent<UnitComponent>().Get(targetId);
            Vector2 vector2 = Vector2.zero;
            self.StartIndicator = vector2;

            if (self.mSkillConfig.SkillZhishiTargetType == 0 && target != null)
            {
                float distance = PositionHelper.Distance2D(target.Position, unit.Position);
                float rate = distance / self.SkillRangeSize;
                rate = Mathf.Min(rate, 1f);
                Vector3 direction = target.Position - unit.Position;
                vector2.x = direction.x;
                vector2.y = direction.z;
                vector2 = vector2.normalized * 120f * rate;
                self.OnMouseDrag(vector2);
            }
            else if (self.mSkillConfig.SkillZhishiTargetType == 2)
            {
                float roationy = Mathf.FloorToInt(unit.Rotation.eulerAngles.y);
                Quaternion rotation = Quaternion.Euler(0, roationy, 0);
                Vector3 postition = rotation * Vector3.forward * 0.01f;
                vector2.x = postition.x;
                vector2.y = postition.z;
                vector2 = vector2.normalized * 120f * 1f;
                self.OnMouseDrag(vector2);
            }
            else
            {
                //float roationy = Mathf.FloorToInt(unit.Rotation.eulerAngles.y);
                //Quaternion rotation = Quaternion.Euler(0, roationy, 0);
                //Vector3 postition = rotation * Vector3.forward * 0.01f;
                //vector2.x = postition.x;
                //vector2.y = postition.z;
                self.OnMouseDrag(vector2);
            }
        }

        //鼠标拖拽
        public static void OnMouseDrag(this SkillIndicatorComponent self,Vector2 indicator)
        {
            self.StartIndicator += indicator;
            SkillIndicatorItem skillIndicatorItem = self.SkillIndicator;
            if (skillIndicatorItem == null)
            {
                return;
            }

            if (skillIndicatorItem.SkillZhishiType == SkillZhishiType.Position)
            {
                float rate = 1;
                rate = self.StartIndicator.magnitude / 120f;
                rate = (rate > 1f) ? 1f : rate;
                skillIndicatorItem.AttackDistance = Mathf.FloorToInt(self.SkillRangeSize * rate);
                skillIndicatorItem.TargetAngle = 90 - (int)(Mathf.Atan2(self.StartIndicator.y, self.StartIndicator.x) * Mathf.Rad2Deg);
            }
            else
            {
                skillIndicatorItem.AttackDistance = 0;
                skillIndicatorItem.TargetAngle = 90 - (int)(Mathf.Atan2(self.StartIndicator.y, self.StartIndicator.x) * Mathf.Rad2Deg);
            }

            skillIndicatorItem.TargetAngle += (int)self.MainCamera.transform.eulerAngles.y;
            self.OnMainHeroMove();
        }

        public static float GetIndicatorDistance(this SkillIndicatorComponent self)
        {
            return self.SkillIndicator!=null ? self.SkillIndicator.AttackDistance:0;
        }

        public static int GetIndicatorAngle(this SkillIndicatorComponent self)
        {
            if (self.SkillIndicator != null)
            {
                return self.SkillIndicator.TargetAngle;
            }
            else
            {
                Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
                if (unit != null)
                {
                    return (int)unit.Rotation.eulerAngles.y;
                }
                else
                {
                    return 0;
                }
            }
        }

        public static void RecoveryEffect(this SkillIndicatorComponent self)
        {
            SkillIndicatorItem skillIndicatorItem = self.SkillIndicator;
            self.SkillIndicator = null;
            self.StartIndicator = Vector2.zero;
            if (skillIndicatorItem == null || skillIndicatorItem.GameObject == null)
            {
                return;
            }
            skillIndicatorItem.GameObject.SetActive(false);
            GameObjectPoolComponent.Instance.RecoverGameObject(skillIndicatorItem.EffectPath, skillIndicatorItem.GameObject);
        }

        public static void OnMainHeroMove(this SkillIndicatorComponent self)
        {
            SkillIndicatorItem skillIndicatorItem = self.SkillIndicator;
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            if (skillIndicatorItem == null || skillIndicatorItem.GameObject == null || unit == null)
            {
                return;
            }

            Quaternion rotation;
            skillIndicatorItem.GameObject.transform.localPosition = unit.Position;
            switch (skillIndicatorItem.SkillZhishiType)
            {
                case SkillZhishiType.CommonAttack:
                    break;
                case SkillZhishiType.Position:
                    rotation = Quaternion.Euler(0, skillIndicatorItem.TargetAngle, 0);
                    skillIndicatorItem.GameObject.Get<GameObject>("Skill_InnerArea").transform.localPosition = rotation * Vector3.forward * skillIndicatorItem.AttackDistance;
                    break;
                case SkillZhishiType.Line:
                    rotation = Quaternion.Euler(0, skillIndicatorItem.TargetAngle, 0);
                    Vector3 skillTarget = rotation * Vector3.forward + unit.Position;
                    skillIndicatorItem.GameObject.Get<GameObject>("Skill_Dir").transform.LookAt(skillTarget);
                    break;
                case SkillZhishiType.Angle60:
                    rotation = Quaternion.Euler(0, skillIndicatorItem.TargetAngle, 0);
                    skillTarget = rotation * Vector3.forward + unit.Position;
                    skillIndicatorItem.GameObject.Get<GameObject>("Skill_Area_60").transform.LookAt(skillTarget);
                    break;
                case SkillZhishiType.Angle120:
                    rotation = Quaternion.Euler(0, skillIndicatorItem.TargetAngle, 0);
                    skillTarget = rotation * Vector3.forward + unit.Position;
                    skillIndicatorItem.GameObject.Get<GameObject>("Skill_Area_120").transform.LookAt(skillTarget);
                    break;
            }
        }

    }
}
