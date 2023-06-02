﻿
using System.Collections.Generic;
 
namespace ET
{
    public class PetComponent : Entity, IAwake, ITransfer, IUnitCache
    {
        public int PetFubeRewardId = 0;

        public int PetShouHuActive = 0;
        public List<long> TeamPetList = new List<long>() { };       //宠物天梯
        public List<long> PetFormations = new List<long>() { };     //宠物副本
        public List<long> PetShouHuList = new List<long>() { };     //守护列表
        public List<int>  PetCangKuOpen = new List<int>() { };   

        public List<RolePetInfo> RolePetInfos = new List<RolePetInfo>();
        public List<RolePetEgg> RolePetEggs = new List<RolePetEgg>(3);
        public List<PetFubenInfo> PetFubenInfos = new List<PetFubenInfo>();
        public List<KeyValuePair> PetSkinList = new List<KeyValuePair>() { };

    }
}
