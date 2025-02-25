﻿using System.Collections.Generic;

namespace ET
{

    [MessageHandler]
    public class M2C_RolePetUpdateHandler : AMHandler<M2C_RolePetUpdate>
    {
        protected override void  Run(Session session, M2C_RolePetUpdate message)
        {
            PetComponent petComponent = session.ZoneScene().GetComponent<PetComponent>();
            List<KeyValuePair> oldPetSkin = petComponent.GetPetSkinCopy();

            session.ZoneScene().GetComponent<PetComponent>().OnRecvRolePetUpdate(message);

            if (message.GetWay == 2 && message.PetInfoAdd.Count > 0)
            {
                PetSkinConfig petSkinConfig = PetSkinConfigCategory.Instance.Get(message.PetInfoAdd[0].SkinId);   
                HintHelp.GetInstance().ShowHint($"获得{petSkinConfig.Name}宠物!");
            }
            if (message.GetWay == 0 && message.PetInfoAdd.Count > 0)
            {
                EventType.RolePetAdd.Instance.ZoneScene = session.ZoneScene();
                EventType.RolePetAdd.Instance.OldPetSkin = oldPetSkin;
                EventType.RolePetAdd.Instance.RolePetInfo = message.PetInfoAdd[0];
                EventSystem.Instance.PublishClass(EventType.RolePetAdd.Instance);
            }
        }
    }
}
