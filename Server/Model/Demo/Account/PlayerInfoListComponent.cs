using System.Collections.Generic;

namespace ET
{
    public class PlayerInfoListComponent : Entity, IAwake, IDestroy
    {

        public Dictionary<string, long> PlayerIpNumber = new Dictionary<string, long>();    


        /// <summary>
        /// 正在回档的玩家
        /// </summary>
        public List<KeyValuePair<long, string>> ArchivePlayerList = new List<KeyValuePair<long, string>>();       

    }
}
