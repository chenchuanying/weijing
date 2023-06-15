﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ET
{
    [Event]
    public class UISoloEnterEvent : AEventClass<EventType.UISoloEnter>
    {
        protected override async void Run(object cls)
        {
            //获取事件对应传参
            EventType.UISoloEnter args = cls as EventType.UISoloEnter;

            string tipStr = "竞技场匹配完成,请尽快进入!";
            bool removeStatus = false;

            PopupTipHelp.OpenPopupTip(args.ZoneScene, "", GameSettingLanguge.LoadLocalization(tipStr),
                () =>
                {
                    EnterFubenHelp.RequestTransfer(
                        args.ZoneScene, SceneTypeEnum.Solo, 2000010, 0, args.m2C_SoloMatch.FubenId.ToString()
                        ).Coroutine();
                    removeStatus = true;
                },
                () => {
                    //关闭界面
                    removeStatus = true;
                }).Coroutine();


            //10秒后将自动关闭竞技场
            await TimerComponent.Instance.WaitAsync(10000);
            if (removeStatus == false)
            {
                UIHelper.Remove(args.ZoneScene, UIType.UIPopupview);
            }
           
        }
    }
}




