﻿using System;

namespace ET
{ 

    [ActorMessageHandler]
    public class C2M_TaskGetRequestHandler : AMActorLocationRpcHandler<Unit, C2M_TaskGetRequest, M2C_TaskGetResponse>
    {

        protected override async ETTask Run(Unit unit, C2M_TaskGetRequest request, M2C_TaskGetResponse response, Action reply)
        {
            TaskConfig taskConfig = TaskConfigCategory.Instance.Get(request.TaskId);
            if (taskConfig.TaskType == TaskTypeEnum.EveryDay)
            {
                TaskComponent taskComponent = unit.GetComponent<TaskComponent>();
                if (taskComponent.GetTaskList(TaskTypeEnum.EveryDay).Count > 0)
                {
                    response.Error = ErrorCode.ERR_TaskCanNotGet;
                    reply();
                    return;
                }

                //获取当前任务是否已达上限
                if (unit.GetComponent<NumericComponent>().GetAsInt(NumericType.DailyTaskNumber) >= GlobalValueConfigCategory.Instance.Get(58).Value2)
                {
                    response.Error = ErrorCode.ERR_ShangJinNumFull;
                    reply();
                    return;
                }

                NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
                int dailyTask = numericComponent.GetAsInt(NumericType.DailyTaskID);
                if (dailyTask == 0)
                {
                    LogHelper.LogDebug($"{unit.Id}  taskLoopId == 0");
                    response.Error = ErrorCode.ERR_TaskCanNotGet;
                    reply();
                    return;
                }
                numericComponent.ApplyChange(null, NumericType.DailyTaskNumber, 1, 0);
                response.TaskPro = taskComponent.OnGetLoopTask(dailyTask);
            }
            else if (taskConfig.TaskType == TaskTypeEnum.Union)
            {
                TaskComponent taskComponent = unit.GetComponent<TaskComponent>();
                if (taskComponent.GetTaskList(TaskTypeEnum.Union).Count > 0)
                {
                    response.Error = ErrorCode.ERR_TaskNoComplete;
                    reply();
                    return;
                }

                //获取当前任务是否已达上限
                int uniontask = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.UnionTaskNumber);
                if (uniontask >= 10)
                {
                    response.Error = ErrorCode.ERR_TaskLimited;
                    reply();
                    return;
                }

                NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
                int taskLoopId = numericComponent.GetAsInt(NumericType.UnionTaskId);
                if (taskLoopId == 0)
                {
                    LogHelper.LogDebug($"{unit.Id}  taskLoopId == 0");
                    response.Error = ErrorCode.ERR_TaskCanNotGet;
                    reply();
                    return;
                }

                numericComponent.ApplyChange(null, NumericType.UnionTaskNumber,1, 0);
                response.TaskPro = taskComponent.OnGetLoopTask(taskLoopId);
            }
            else
            {
                TaskPro taskPro = unit.GetComponent<TaskComponent>().OnAcceptedTask(request.TaskId);
                response.Error = taskPro != null ? ErrorCode.ERR_Success : ErrorCode.ERR_TaskCanNotGet;
                response.TaskPro = taskPro;
            }
           
            reply();
            await ETTask.CompletedTask;
        }
    }
}
