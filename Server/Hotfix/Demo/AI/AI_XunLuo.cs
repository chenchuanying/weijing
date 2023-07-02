using UnityEngine;

namespace ET
{
    [AIHandler]
    public class AI_XunLuo: AAIHandler
    {
        public override bool Check(AIComponent aiComponent, AIConfig aiConfig)
        {
            if (aiComponent.TargetID != 0 || aiComponent.IsRetreat)
            {
                return false;
            }
            Unit unit = aiComponent.GetParent<Unit>();
            Unit nearest = AIHelp.GetNearestEnemy(unit, aiComponent.ActRange);
            if (nearest == null)
            {
                aiComponent.TargetID = 0;
                aiComponent.noCheckStatus = true;
                return false;
            }

            if ( unit.IsBoss())
            {
                unit.GetComponent<NumericComponent>().ApplyValue(NumericType.BossInCombat, 1, true, true);
            }
            aiComponent.TargetID = nearest.Id;
            return aiComponent.TargetID > 0;
        }

        public static Vector3 GetInitRandomVec3(Vector3 initVec3, double ai_PatrolRange)
        {
            System.Random rand = new System.Random();
            double randValue = rand.NextDouble();
            double random_x = (randValue - 0.5f) * ai_PatrolRange * 2;
            double random_z = (randValue - 0.5f) * ai_PatrolRange * 2;
            Vector3 retVec = new Vector3((float)(initVec3.x + random_x), initVec3.y, (float)(initVec3.z + random_z));
            //Log.Debug("retVec = " + retVec);
            return retVec;
        }

        public override async ETTask Execute(AIComponent aiComponent, AIConfig aiConfig, ETCancellationToken cancellationToken)
        {
            Unit unit = aiComponent.GetParent<Unit>();
            while (true)
            {
                //Log.Debug("开始巡逻1111： " + TimeHelper.ClientNowSeconds());
                //Vector3 nextTarget = xunLuoPathComponent.GetCurrent();      //随机获取一个坐标点
                //Vector3 nextTarget = GetInitRandomVec3(aiComponent.InitVec3, aiComponent.PatrolRange);      //随机获取一个坐标点
                //unit.FindPathMoveToAsync(nextTarget, cancellationToken).Coroutine();

                //等5秒原地延迟
                bool timeRet = await TimerComponent.Instance.WaitAsync(5000, cancellationToken);
                if (!timeRet)
                {
                    //Log.Debug("延迟被打断！！" );
                    return;
                }
                //Log.Debug("巡逻正常： " + ret);
                //==0 才开始下一个寻路点
                //xunLuoPathComponent.MoveNext();
            }
        }

    }
}