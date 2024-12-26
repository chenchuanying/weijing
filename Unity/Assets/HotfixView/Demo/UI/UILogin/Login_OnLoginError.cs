using UnityEngine;

namespace ET
{

    [Event]
    public class Login_OnLoginError : AEventClass<EventType.LoginError>
    {
        protected override void Run(object cls)
        {
            RunAsync(cls as EventType.LoginError).Coroutine();
        }

        private async ETTask RunAsync(EventType.LoginError args)
        {
            if (args.ErrorCore == ErrorCode.ERR_LoginInfoExpire)
            {
                UI uilogin = UIHelper.GetUI( args.ZoneScene, UIType.UILogin );
                if (uilogin!=null)
                {
                    uilogin.GetComponent<UILoginComponent>().OnRegister();
                }
            }
            if (args.ErrorCore == ErrorCode.ERR_FangChengMi_Tip1)
            {
                string content = ErrorHelp.Instance.ErrorHintList[args.ErrorCore];
                content = content.Replace("{0}", args.Value);
                PopupTipHelp.OpenPopupTip_3(args.ZoneScene, "防沉迷提示",
                    content,
                    () => { }
                    ).Coroutine();
            }
            if (args.ErrorCore == ErrorCode.ERR_FangChengMi_Tip6
                || args.ErrorCore == ErrorCode.ERR_FangChengMi_Tip7)
            {
                string content = ErrorHelp.Instance.ErrorHintList[args.ErrorCore];
                PopupTipHelp.OpenPopupTip_3(args.ZoneScene, "防沉迷提示",
                    content,
                    () => { }
                    ).Coroutine();
            }
            if (args.ErrorCore == ErrorCode.ERR_NotRealName)
            {
                UI ui = await UIHelper.Create(args.ZoneScene, UIType.UIRealName);
                ui.GetComponent<UIRealNameComponent>().AccountId = args.AccountId;
            }
            if (args.ErrorCore == ErrorCode.ERR_StopServer)
            {
                PopupTipHelp.OpenPopupTip_3(args.ZoneScene, "系统提示", UILoginHelper.GetGongGaoText(), null).Coroutine();
            }
        }
    }
}
