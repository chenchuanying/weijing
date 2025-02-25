﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    public class Task_OnTaskNpcDialog : AEventClass<EventType.TaskNpcDialog>
    {

        protected override void Run(object numerice)
        {
            EventType.TaskNpcDialog args = numerice as EventType.TaskNpcDialog;

            if (args.ErrorCode == 0)
            {
                OperaComponent operaComponent = args.zoneScene.CurrentScene().GetComponent<OperaComponent>();
                operaComponent.OpenNpcTaskUI(args.NpcId).Coroutine();
            }
            if (args.ErrorCode > 200000)
            {
                ErrorHelp.Instance.ErrorHint(args.ErrorCode);
            }
        }
    }
}
