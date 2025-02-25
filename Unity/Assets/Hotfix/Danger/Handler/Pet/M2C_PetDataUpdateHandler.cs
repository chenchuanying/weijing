﻿namespace ET
{

    [MessageHandler]
    public class M2C_PetDataUpdateHandler : AMHandler<M2C_PetDataUpdate>
    {

        protected override void Run(Session session, M2C_PetDataUpdate message)
        {
            switch (message.UpdateType)
            {
                case (int)UserDataType.Lv:
                    RolePetInfo rolePetInfo = session.ZoneScene().GetComponent<PetComponent>().GetPetInfoByID(message.PetId);
                    if (rolePetInfo != null && rolePetInfo.Id == message.PetId)
                    {
                        rolePetInfo.AddPropretyNum += (int.Parse(message.UpdateTypeValue) - rolePetInfo.PetLv) * 5;
                        rolePetInfo.PetLv = int.Parse(message.UpdateTypeValue);
                    }
                    break;
                case (int)UserDataType.Exp:
                    rolePetInfo = session.ZoneScene().GetComponent<PetComponent>().GetPetInfoByID(message.PetId);
                    if (rolePetInfo != null && rolePetInfo.Id == message.PetId)
                    {

                        rolePetInfo.PetExp = int.Parse(message.UpdateTypeValue);
                    }
                    break;
                case (int)UserDataType.PetStatus:
                    rolePetInfo = session.ZoneScene().GetComponent<PetComponent>().GetPetInfoByID(message.PetId);
                    if (rolePetInfo != null)
                    {
                        rolePetInfo.PetStatus = int.Parse(message.UpdateTypeValue);
                    }
                    break;
                case (int)UserDataType.Name:
                    rolePetInfo = session.ZoneScene().GetComponent<PetComponent>().GetPetInfoByID(message.PetId);
                    if (rolePetInfo != null)
                    {
                        rolePetInfo.PetName = message.UpdateTypeValue;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
