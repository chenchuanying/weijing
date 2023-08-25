using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public static class MoveHelper
    {
        public static C2M_PathfindingResult c2M_PathfindingResult = new C2M_PathfindingResult();


        /// <summary>
        /// 主角移动
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="targetPos"></param>
        /// <param name="yangan"></param>
        /// <returns></returns>
        public static async ETTask<int> MoveByYaoGan(this Unit unit, Vector3 targetPos, int direction, float distance,  ETCancellationToken cancellationToken = null)
        {
            StateComponent stateComponent = unit.GetComponent<StateComponent>();
            stateComponent.ObstructStatus = 0;
            int errorCode = stateComponent.CanMove();
            if (ErrorCore.ERR_Success != errorCode)
            {
                HintHelp.GetInstance().ShowHintError(errorCode);
                stateComponent.CheckSilence();
                return -1;
            }
            float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Now_Speed);
            if (speed <= 0f)
            {
                HintHelp.GetInstance().ShowHint("速度异常,请重新登录");
            }

            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            moveComponent.MoveWait = false;
            moveComponent.YaoganMove = true;
            C2M_PathfindingResult msg = c2M_PathfindingResult;
          
            msg.YaoGan = true;
            msg.UnitId = unit.Id;
            msg.Direction = direction;
            msg.Distance = distance;
            msg.X = targetPos.x;
            msg.Y = targetPos.y;
            msg.Z = targetPos.z;
            unit.ZoneScene().GetComponent<SessionComponent>().Session.Send(msg);

            ObjectWait objectWait = unit.GetComponent<ObjectWait>();

            // 要取消上一次的移动协程
            objectWait.Notify(new WaitType.Wait_UnitStop() { Error = WaitTypeError.Cancel });

            // 一直等到unit发送stop
            WaitType.Wait_UnitStop waitUnitStop = await objectWait.Wait<WaitType.Wait_UnitStop>(cancellationToken);
            return waitUnitStop.Error;
        }


        /// <summary>
        /// 主角移动
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="targetPos"></param>
        /// <param name="yangan"></param>
        /// <returns></returns>
        public static async ETTask<int> MoveToAsync2(this Unit unit, Vector3 targetPos,bool yangan=true, ETCancellationToken cancellationToken = null, int direction = 0)
        {
            StateComponent stateComponent = unit.GetComponent<StateComponent>();
            stateComponent.ObstructStatus = 0;  
            int errorCode = stateComponent.CanMove();
            if (ErrorCore.ERR_Success!= errorCode)
            {
                HintHelp.GetInstance().ShowHintError(errorCode);
                stateComponent.CheckSilence();
                return -1;
            }
            float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Now_Speed);
            if (speed <= 0f)
            {
                HintHelp.GetInstance().ShowHint("速度异常,请重新登录");
            }

            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            moveComponent.MoveWait = false;
            moveComponent.YaoganMove = yangan;
            C2M_PathfindingResult msg = c2M_PathfindingResult;
            msg.X = targetPos.x;
            msg.Y = targetPos.y;
            msg.Z = targetPos.z;    
            msg.YaoGan = yangan;
            msg.UnitId = direction;
            msg.Direction = 0;
            msg.Distance = 0;
            unit.ZoneScene().GetComponent<SessionComponent>().Session.Send(msg);

            ObjectWait objectWait = unit.GetComponent<ObjectWait>();

            // 要取消上一次的移动协程
            objectWait.Notify(new WaitType.Wait_UnitStop() { Error = WaitTypeError.Cancel });

            // 一直等到unit发送stop
            WaitType.Wait_UnitStop waitUnitStop = await objectWait.Wait<WaitType.Wait_UnitStop>(cancellationToken);
            return waitUnitStop.Error;
        }


        /// <summary>
        /// 机器人移动
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="targetPos"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        // 可以多次调用，多次调用的话会取消上一次的协程
        public static async ETTask<int> MoveToAsync(this Unit unit, Vector3 targetPos, ETCancellationToken cancellationToken = null)
        {
            C2M_PathfindingResult msg = c2M_PathfindingResult;
            msg.X = targetPos.x;
            msg.Y = targetPos.y;
            msg.Z = targetPos.z;
            msg.Distance = 0;
            msg.Direction = 0;
            unit.ZoneScene().GetComponent<SessionComponent>().Session.Send(msg);

            ObjectWait objectWait = unit.GetComponent<ObjectWait>();
            
            // 要取消上一次的移动协程
            objectWait.Notify(new WaitType.Wait_UnitStop() { Error = WaitTypeError.Cancel });
            
            // 一直等到unit发送stop
            WaitType.Wait_UnitStop waitUnitStop = await objectWait.Wait<WaitType.Wait_UnitStop>(cancellationToken);
            return waitUnitStop.Error;
        }
        
        public static async ETTask<bool> MoveToAsync(this Unit unit, List<Vector3> path)
        {
            float speed = unit.GetComponent<NumericComponent>().GetAsFloat(NumericType.Now_Speed);
            MoveComponent moveComponent = unit.GetComponent<MoveComponent>();
            bool ret = await moveComponent.MoveToAsync(path, speed);
            return ret;
        }
    }
}