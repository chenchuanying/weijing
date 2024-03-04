﻿using System;

namespace ET
{ 

    [ActorMessageHandler]
    public class C2M_TaskGetRequestHandler : AMActorLocationRpcHandler<Unit, C2M_TaskGetRequest, M2C_TaskGetResponse>
    {

        protected override async ETTask Run(Unit unit, C2M_TaskGetRequest request, M2C_TaskGetResponse response, Action reply)
        {
            if (!TaskConfigCategory.Instance.Contain(request.TaskId))
            {
                response.Error = ErrorCode.ERR_ModifyData;
                reply();
                return;
            }

            TaskConfig taskConfig = TaskConfigCategory.Instance.Get(request.TaskId);
            if (taskConfig.TaskType == TaskTypeEnum.Daily)
            {
                TaskComponent taskComponent = unit.GetComponent<TaskComponent>();
                if (taskComponent.GetTaskList(TaskTypeEnum.Daily).Count > 0)
                {
                    response.Error = ErrorCode.ERR_TaskCanNotGet;
                    reply();
                    return;
                }

                //获取当前任务是否已达上限
                if (unit.GetComponent<NumericComponent>().GetAsInt(NumericType.DailyTaskNumber) >=  GlobalValueConfigCategory.Instance.Get(58).Value2)
                {
                    response.Error = ErrorCode.ERR_ShangJinNumFull;
                    reply();
                    return;
                }

                NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
                int dailyTask = numericComponent.GetAsInt(NumericType.DailyTaskID);
                if (dailyTask == 0)
                {
                    LogHelper.LogDebug($"{unit.Id}  dailyTask == 0");
                    response.Error = ErrorCode.ERR_TaskCanNotGet;
                    reply();
                    return;
                }
                response.TaskPro = taskComponent.OnGetDailyTask(dailyTask);
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
                if (uniontask >= GlobalValueConfigCategory.Instance.Get(108).Value2)
                {
                    response.Error = ErrorCode.ERR_TaskLimited;
                    reply();
                    return;
                }

                NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
                int unionTaskId = numericComponent.GetAsInt(NumericType.UnionTaskId);
                if (unionTaskId == 0)
                {
                    LogHelper.LogDebug($"{unit.Id}  unionTaskId == 0");
                    response.Error = ErrorCode.ERR_TaskCanNotGet;
                    reply();
                    return;
                }
                response.TaskPro = taskComponent.OnGetDailyTask(unionTaskId);
            }
            else if (taskConfig.TaskType == TaskTypeEnum.Treasure)
            {
                if (unit.GetComponent<TaskComponent>().GetTaskList(TaskTypeEnum.Treasure).Count > 1)
                {
                    response.Error = ErrorCode.ERR_TaskCanNotGet;
                    reply();
                    return;
                }
                (TaskPro taskPro, int error) = unit.GetComponent<TaskComponent>().OnAcceptedTask(request.TaskId);
                response.Error = error;
                response.TaskPro = taskPro;
            }
            else
            {
                (TaskPro taskPro, int error) = unit.GetComponent<TaskComponent>().OnAcceptedTask(request.TaskId);
                response.Error = error;
                response.TaskPro = taskPro;
            }
            reply();
            await ETTask.CompletedTask;
        }
    }
}
