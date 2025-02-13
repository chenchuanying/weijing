

namespace ET
{ 

    public static class SettingHelper
    {

        public static void OnSmooth(string value)
        {
            ShowTitle = value == "0";
            ShowGuangHuan = value == "0";  
        }

        public static void OnShowOther(string value)
        { 
            NoShowOther = value == "1";   
        }

        /// <summary>
        /// 主城周围超过50个人不再进行显示
        /// </summary>
        public static int NoShowPlayer = 50;

        /// <summary>
        /// 称号周围同屏超过50个人不再进行显示
        /// </summary>
        public static int NoShowTitle = 50;

        /// <summary>
        /// 同屏超过50个人不显示光环特效(特效名称关键字: GuangHuan)
        /// </summary>
        public static int NotGuangHuan = 50;


        public static bool NoShowOther = false;

        /// <summary>
        /// 称号
        /// </summary>
        public static bool ShowTitle = true;    

        /// <summary>
        /// 血条
        /// </summary>
        public static bool ShowBlood = true;
        /// <summary>
        /// 光环
        /// </summary>
        public static bool ShowGuangHuan = true;

        /// <summary>
        /// 特效
        /// </summary>
        public static bool ShowEffect = true;
        /// <summary>
        /// 动画
        /// </summary>
        public static bool ShowAnimation = true;
        
        /// <summary>
        /// 音效
        /// </summary>
        public static bool PlaySound = true;

        public static bool UsePool = true;


        public static bool ShowNoMoving = false;


        /// <summary>
        /// 0 服务器寻路   1 客户端寻路
        /// </summary>
        public static bool ClintFindPath = false;
    }
}