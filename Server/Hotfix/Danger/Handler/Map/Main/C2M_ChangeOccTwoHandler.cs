using System;

namespace ET
{

    [ActorMessageHandler]
    public class C2M_ChangeOccTwoHandler : AMActorLocationRpcHandler<Unit, C2M_ChangeOccTwoRequest, M2C_ChangeOccTwoResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_ChangeOccTwoRequest request, M2C_ChangeOccTwoResponse response, Action reply)
        {
            //判断当前角色等级是否达到
            if (unit.GetComponent<UserInfoComponent>().UserInfo.Lv < 18) 
            {
                response.Error = ErrorCode.ERR_Occ_Hint_1;
                reply();
                return;
            }

            int OccTwo = unit.GetComponent<UserInfoComponent>().UserInfo.OccTwo;
            ////判断当前角色是否已经进行转职
            if (OccTwo != 0 )
            {
                response.Error = ErrorCode.ERR_Occ_Hint_2;
                reply();
                return;
            }

            if (!OccupationTwoConfigCategory.Instance.Contain(request.OccTwoID))
            {
                Log.Error($"C2M_ChangeOccTwoRequest.1");
                response.Error = ErrorCode.ERR_ModifyData;
                reply();
                return;
            }
            DataCollationComponent dataCollationComponent = unit.GetComponent<DataCollationComponent>();

            unit.GetComponent<SkillSetComponent>().OnChangeJueXing(dataCollationComponent.OccTwoOld, request.OccTwoID);
            unit.GetComponent<SkillSetComponent>().OnChangeOccTwoRequest(request.OccTwoID);
            unit.GetComponent<TaskComponent>().OnChangeOccTwo();

            //if (OccTwo == 0 && !GMHelp.GmAccount.Contains(unit.GetComponent<UserInfoComponent>().Account))
            if (OccTwo == 0)
            {
                string userName = unit.GetComponent<UserInfoComponent>().UserInfo.Name;
                string noticeContent = $"{userName} 在主城转职大师处成功转职:<color=#C4FF00>{OccupationTwoConfigCategory.Instance.Get(request.OccTwoID).OccupationName}</color>";
                ServerMessageHelper.SendBroadMessage(unit.DomainZone(), NoticeType.Notice, noticeContent);
            }

            reply();
            await ETTask.CompletedTask;
        }
    }
}
