using System;


namespace ET
{

    [ActorMessageHandler]
    public class A2M_KickOutPlayerHandler : AMActorRpcHandler<Unit, A2M_KickOutPlayerRequest, M2A_KickOutPlayerResponse>
    {
        protected override async ETTask Run(Unit unit, A2M_KickOutPlayerRequest request, M2A_KickOutPlayerResponse response, Action reply)
        {
            Console.WriteLine($"A2M_KickOutPlayerRequest:  {unit.DomainZone()} {unit.Id}");

            unit.OnKickPlayer(false).Coroutine();

            reply();
            await ETTask.CompletedTask;
        }
    }
}
