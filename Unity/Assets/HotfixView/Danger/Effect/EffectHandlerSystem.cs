using UnityEngine;

namespace ET
{
    public static class EffectHandlerSystem
    {

        /// <summary>
        /// 添加服务器的碰撞范围,显示作用
        /// </summary>
        /// <param name="effect"></param>
        /// <param name="rangeType"></param>
        /// <param name="rangeValue"></param>
        public static void AddCollider(this AEffectHandler self, GameObject effect,int rangeType,  float[] rangeValue )
        {
            //Log.Debug("实装碰撞体:" + self.EffectConfig.Id.ToString() + "rangeType :" + rangeType + "rangeValue:" + rangeValue);
            if (rangeType == 1 && effect.GetComponent<SphereCollider>() == null)
            {
                SphereCollider collider = effect.AddComponent<SphereCollider>();
                collider.radius = rangeValue[0];
            }
            if (rangeType == 2 && effect.GetComponent<BoxCollider>() == null)
            {
                BoxCollider collider = effect.AddComponent<BoxCollider>();
                collider.center = new Vector3(0,0, rangeValue[1]*0.5f);
                collider.size = new Vector3(rangeValue[0], 1, rangeValue[1]);
            }
        }

        /// <summary>
        /// 实时更新当前特效位置
        /// </summary>
        
        public static void UpdateEffectPosition(this AEffectHandler self, Vector3 vec3, float angle)
        {
            if (self.EffectObj == null)
            {
                return;
            }
            if (angle != -1)
            {
                self.EffectObj.transform.rotation = Quaternion.Euler(0, angle, 0);
            }
            self.EffectObj.transform.position = vec3;
        }
        
    }
}