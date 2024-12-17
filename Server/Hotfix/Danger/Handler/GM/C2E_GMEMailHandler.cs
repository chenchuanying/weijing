using System;
using System.Collections.Generic;

namespace ET
{
    [ActorMessageHandler]
    public class C2E_GMEMailHandler : AMActorRpcHandler<Scene, C2E_GMEMailRequest, E2C_GMEMailResponse>
    {
        protected override async ETTask Run(Scene scene, C2E_GMEMailRequest request, E2C_GMEMailResponse response, Action reply)
        {
            int errorCode = ErrorCode.ERR_Success;
            string[] mailInfo = request.MailInfo.Split(" ");
            if (mailInfo[0] != "mail" && mailInfo.Length < 6)
            {
                Log.Warning("邮件发送失败！");
                errorCode = ErrorCode.ERR_Parameter;
            }
            try
            {
                int mailtype = int.Parse(mailInfo[4]);
                Game.EventSystem.Publish(new EventType.GMCommonRequest() { Context = request.MailInfo });
            }
            catch (Exception ex)
            {
                Log.Warning("邮件发送失败！" + ex.ToString() + "   " + request.MailInfo);
                errorCode = ErrorCode.ERR_Parameter;
            }

            int zone = int.Parse(mailInfo[1]);
            if (zone!= 0 &&  mailInfo[2]!= "0")
            { 
                string userName = mailInfo[2];
                List<UserInfoComponent> result = await Game.Scene.GetComponent<DBComponent>().Query<UserInfoComponent>(zone, _account => _account.UserName == userName);
                if (result == null || result.Count == 0)
                {
                    errorCode = ErrorCode.ERR_NonePlayerError;
                }
            }
            response.Error = errorCode;
            reply();
            await ETTask.CompletedTask;
        }

    }
}
