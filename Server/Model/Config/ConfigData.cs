using System.Collections.Generic;

namespace ET
{
    public static class ConfigData
    {
        public static int PackageLimit = 100;

        public static int PopularizeZone = 2;

        public static bool AccountOldLogic = false;

        public static bool CleanSkill = false;

        public static bool LogRechargeNumber = true;

        public static bool ShowLieOpen = false;

        public static Dictionary<int , ServerInfo> ServerInfoList = new Dictionary<int , ServerInfo>();    


        public static List<int> FunctionOpenIds = new List<int> { 1025, 1043, 1044, 1045, 1052, 1055, 1057, 1058, 1059, 2000 };
    }
}
