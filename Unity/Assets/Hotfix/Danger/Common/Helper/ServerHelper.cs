using System;
using System.Collections.Generic;

namespace ET
{
    public static class ServerHelper
    {
        public static int UpdateServerList = 0;  //改成int
        public static List<ServerItem> ServerItems = new List<ServerItem>();

        //Alpha = 0,              //仅内部人员使用。一般不向外部发布
        //Beta = 1,               //公开测试版
        //BanHao = 2,
        public static string GetServerIpList(bool innerNet, int zone)
        {
            ServerItem serverItem = GetGetServerItem(innerNet, zone);
            return serverItem.ServerIp;
        }

        public static ServerItem GetGetServerItem(bool innerNet, int zone)
        {
            ServerItem serverItem = null;
            List<ServerItem> serverItems = ServerHelper.GetServerList(innerNet);
            for (int i = 0; i < serverItems.Count; i++)
            {
                if (serverItems[i].ServerId == zone)
                {
                    serverItem = serverItems[i];
                }
            }
            return serverItem;
        }

        public static long GetOpenServerTime(bool innerNet, int zone)
        {
            ServerItem serverItem = GetGetServerItem(innerNet, zone);
            if (serverItem == null)
            {
                Log.Debug($"serverItem == null {zone}");
                return 0;
            }
            return serverItem.ServerOpenTime;
        }

        public static int GetOpenServerDay(bool innerNet, int zone)
        {
            long serverNow = TimeHelper.ServerNow();
            long openSerTime = GetOpenServerTime(innerNet, zone);
            if (openSerTime == 0 || serverNow < openSerTime)
            {
                return 0;
            }

            int openserverDay = DateDiff_Time(serverNow, openSerTime);
            return openserverDay;
        }

        public static int DateDiff_Time(long time1, long time2)
        {
            DateTime d1 = TimeInfo.Instance.ToDateTime(time1);
            DateTime d2 = TimeInfo.Instance.ToDateTime(time2);
            DateTime d3 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d1.Year, d1.Month, d1.Day));

            DateTime d4 = Convert.ToDateTime(string.Format("{0}-{1}-{2}", d2.Year, d2.Month, d2.Day));
            int days = (d3 - d4).Days + 1;
            return days;
        }

        public const string LogicServer = "weijinggameserver.weijinggame.com";//"weijinggameserver.weijinggame.com"
        public const string LogicServerBanHao = "47.94.107.92";

        public static string GetLogicServer(bool innerNet, VersionMode versionMode)
        {
            return innerNet ? ComHelp.LocalIp : LogicServer;
            //switch (versionMode)
            //{
            //    case VersionMode.BanHao:
            //        return innerNet ? ComHelp.LocalIp : LogicServerBanHao;
            //    default:
            //        return innerNet ? ComHelp.LocalIp : LogicServer;

            //}
        }

        /// <summary>
        /// 合区后的区
        /// </summary>
        /// <param name="banhao"></param>
        /// <param name="zone"></param>
        /// <returns></returns>
        public static int GetNewServerId(int zone)
        {
            List<ServerItem> serverItems_1 = GetServerList(false);
            string serverip = string.Empty;
            for (int i = 0; i < serverItems_1.Count; i++)
            {
                if (serverItems_1[i].ServerId == zone)
                {
                    serverip = serverItems_1[i].ServerIp;
                    break;
                }
            }
            for (int i = 0; i < serverItems_1.Count; i++)
            {
                if (serverItems_1[i].ServerIp == serverip && serverItems_1[i].Show == 1)
                {
                    zone = serverItems_1[i].ServerId;
                    break;
                }
            }
            return zone;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerNet"></param>
        /// <param name="zone"></param>
        /// <returns></returns>
        public static int GetOldServerId(int zone)
        {
            List<ServerItem> serverItems_1 = GetServerList(false);

            string serverip = string.Empty;
            for (int i = 0; i < serverItems_1.Count; i++)
            {
                if (serverItems_1[i].ServerId == zone)
                {
                    serverip = serverItems_1[i].ServerIp;
                    break;
                }
            }
            for (int i = 0; i < serverItems_1.Count; i++)
            {
                if (serverItems_1[i].ServerIp == serverip)
                {
                    zone = serverItems_1[i].ServerId;
                }
            }
            return zone;
        }

        public static bool IsOldServer(int zone)
        {
            List<ServerItem> serverItems_1 = GetServerList(false);
            string serverip = string.Empty;
            for (int i = 0; i < serverItems_1.Count; i++)
            {
                if (serverItems_1[i].ServerId == zone)
                {
                    serverip = serverItems_1[i].ServerIp;
                    break;
                }
            }

            int servernumber = 0;
            for (int i = 0; i < serverItems_1.Count; i++)
            {
                if (serverItems_1[i].ServerIp == serverip)
                {
                    servernumber++;
                }
            }
            return servernumber > 1;
        }

        public static int GetFirstServerId()
        {
            int firstserver = 3;
            for (int i = 0; i < ServerItems.Count; i++)
            {
                if (ServerItems[i].Show == 1)
                {
                    firstserver = ServerItems[i].ServerId;
                    break;
                }
            }
            return firstserver;
        }

        public static List<ServerItem> GetServerList(bool innerNet)
        {
            if (ServerItems.Count > 0 && UpdateServerList == 1)
            {
                return ServerItems;
            }
            //Log.Debug("UpdateServerList");
            UpdateServerList = 1;
            ServerItems.Clear();

            string ip = GetLogicServer(innerNet, VersionMode.Beta);
            List<ServerItem> serverItems_1 = ServerItems;


            //128服 指定的主播账号才可以进入。。
            serverItems_1.Add(new ServerItem() { ServerId = 128, ServerIp = $"{ip}:20425", ServerName = "主播专服", ServerOpenTime = 1731236400000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //serverItems_1.Add(new ServerItem() { ServerId = 2, ServerIp = $"{ip}:20325", ServerName = "审核专区", ServerOpenTime = 1662189906681, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 3, ServerIp = $"{ip}:20325", ServerName = "审核专区", ServerOpenTime = 1662189906681, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 4, ServerIp = $"{ip}:20325", ServerName = "审核专区", ServerOpenTime = 1662189906681, New = 0, Show = 0 });

            serverItems_1.Add(new ServerItem() { ServerId = 5, ServerIp = $"{ip}:20335", ServerName = "先锋一区", ServerOpenTime = 1669435366360, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 6, ServerIp = $"{ip}:20335", ServerName = "先锋二区", ServerOpenTime = 1669435366360, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 7, ServerIp = $"{ip}:20335", ServerName = "先锋三区", ServerOpenTime = 1669435366360, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 8, ServerIp = $"{ip}:20335", ServerName = "先锋四区", ServerOpenTime = 1669435366360, New = 0, Show = 0 });

            serverItems_1.Add(new ServerItem() { ServerId = 9, ServerIp = $"{ip}:20345", ServerName = "先锋五区", ServerOpenTime = 1671793200000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 10, ServerIp = $"{ip}:20345", ServerName = "先锋六区", ServerOpenTime = 1671793200000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 11, ServerIp = $"{ip}:20345", ServerName = "先锋七区", ServerOpenTime = 1671793200000, New = 0, Show = 0 });


            //2024/05/20 19:00:00 1716202800000 合区 兔年大吉-烟雨云烟 12/22
            serverItems_1.Add(new ServerItem() { ServerId = 12, ServerIp = $"{ip}:20355", ServerName = "兔年大吉", ServerOpenTime = 1676012400000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 13, ServerIp = $"{ip}:20355", ServerName = "玉兔新春", ServerOpenTime = 1676012400000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 14, ServerIp = $"{ip}:20355", ServerName = "元宵佳节", ServerOpenTime = 1676012400000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 15, ServerIp = $"{ip}:20355", ServerName = "金戈铁马", ServerOpenTime = 1676012400000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 16, ServerIp = $"{ip}:20355", ServerName = "世外桃源", ServerOpenTime = 1676012400000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 17, ServerIp = $"{ip}:20355", ServerName = "名扬四海", ServerOpenTime = 1674212400000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 18, ServerIp = $"{ip}:20355", ServerName = "华灯初上", ServerOpenTime = 1677841200000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 19, ServerIp = $"{ip}:20355", ServerName = "灯火阑珊", ServerOpenTime = 1678446000000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 20, ServerIp = $"{ip}:20355", ServerName = "似水流年", ServerOpenTime = 1679050800000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 21, ServerIp = $"{ip}:20355", ServerName = "秋水人家", ServerOpenTime = 1679655600000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 22, ServerIp = $"{ip}:20355", ServerName = "烟雨云烟", ServerOpenTime = 1680260400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 23, ServerIp = $"{ip}:20355", ServerName = "烟雨云烟", ServerOpenTime = 1680850800000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 24, ServerIp = $"{ip}:20355", ServerName = "碧空之歌", ServerOpenTime = 1681470000000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 25, ServerIp = $"{ip}:20355", ServerName = "灰烬使者", ServerOpenTime = 1682074800000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 26, ServerIp = $"{ip}:20355", ServerName = "劳动光荣", ServerOpenTime = 1682679600000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 27, ServerIp = $"{ip}:20355", ServerName = "逐风者", ServerOpenTime = 1683284400000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 28, ServerIp = $"{ip}:20355", ServerName = "命运之剑", ServerOpenTime = 1683889200000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 29, ServerIp = $"{ip}:20355", ServerName = "紫禁之巅", ServerOpenTime = 1684494000000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 30, ServerIp = $"{ip}:20355", ServerName = "天空之城", ServerOpenTime = 1685073900000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 31, ServerIp = $"{ip}:20355", ServerName = "遗忘之海", ServerOpenTime = 1685703600000, New = 0, Show = 0 });


            //2024/05/24 19:00:00 1716548400000  合区：金榜题名-曙光之城  32/40
            serverItems_1.Add(new ServerItem() { ServerId = 32, ServerIp = $"{ip}:20385", ServerName = "金榜题名", ServerOpenTime = 1686308400000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 33, ServerIp = $"{ip}:20385", ServerName = "鱼跃龙门", ServerOpenTime = 1686913200000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 34, ServerIp = $"{ip}:20385", ServerName = "龙舟竞渡", ServerOpenTime = 1687494600000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 35, ServerIp = $"{ip}:20385", ServerName = "梦境之地", ServerOpenTime = 1688122800000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 36, ServerIp = $"{ip}:20385", ServerName = "叹息森林", ServerOpenTime = 1688702700000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 37, ServerIp = $"{ip}:20385", ServerName = "风之国度", ServerOpenTime = 1688789100000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 38, ServerIp = $"{ip}:20385", ServerName = "燃烧之刃", ServerOpenTime = 1689314400000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 39, ServerIp = $"{ip}:20385", ServerName = "北海之都", ServerOpenTime = 1689937200000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 40, ServerIp = $"{ip}:20385", ServerName = "曙光之城", ServerOpenTime = 1690542000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 41, ServerIp = $"{ip}:20385", ServerName = "世界之树", ServerOpenTime = 1691146800000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 42, ServerIp = $"{ip}:20385", ServerName = "雷霆之路", ServerOpenTime = 1691730000000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 43, ServerIp = $"{ip}:20385", ServerName = "花开彼岸", ServerOpenTime = 1692356400000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 44, ServerIp = $"{ip}:20385", ServerName = "天涯海角", ServerOpenTime = 1692937800000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 45, ServerIp = $"{ip}:20385", ServerName = "长相思", ServerOpenTime = 1693566000000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 46, ServerIp = $"{ip}:20385", ServerName = "执子之手", ServerOpenTime = 1694170800000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 47, ServerIp = $"{ip}:20385", ServerName = "与子偕老", ServerOpenTime = 1694752200000, New = 0, Show = 0 });


            //2024/05/24 19:00:00 1716548400000  合区：流云若梦-精灵国度  48/56
            serverItems_1.Add(new ServerItem() { ServerId = 48, ServerIp = $"{ip}:20365", ServerName = "流云若梦", ServerOpenTime = 1695380400000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 49, ServerIp = $"{ip}:20365", ServerName = "欢度国庆", ServerOpenTime = 1695961800000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 50, ServerIp = $"{ip}:20365", ServerName = "金戈铁马", ServerOpenTime = 1696566600000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 51, ServerIp = $"{ip}:20365", ServerName = "迷雾森林", ServerOpenTime = 1697171400000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 52, ServerIp = $"{ip}:20365", ServerName = "希望之树", ServerOpenTime = 1697799600000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 53, ServerIp = $"{ip}:20365", ServerName = "梦境之海", ServerOpenTime = 1698381000000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 54, ServerIp = $"{ip}:20365", ServerName = "百年风华", ServerOpenTime = 1698985800000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 55, ServerIp = $"{ip}:20365", ServerName = "盛世如愿", ServerOpenTime = 1699590600000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 56, ServerIp = $"{ip}:20365", ServerName = "精灵国度", ServerOpenTime = 1700195400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 57, ServerIp = $"{ip}:20365", ServerName = "梦回华夏", ServerOpenTime = 1700800200000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 58, ServerIp = $"{ip}:20365", ServerName = "璀璨之境", ServerOpenTime = 1701405000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 59, ServerIp = $"{ip}:20365", ServerName = "遗忘森林", ServerOpenTime = 1702009800000, New = 0, Show = 0 });
            serverItems_1.Add(new ServerItem() { ServerId = 60, ServerIp = $"{ip}:20365", ServerName = "希望之光", ServerOpenTime = 1702614600000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 61, ServerIp = $"{ip}:20365", ServerName = "斗战胜佛", ServerOpenTime = 1703242800000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 62, ServerIp = $"{ip}:20365", ServerName = "万里华夏", ServerOpenTime = 1703847600000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 63, ServerIp = $"{ip}:20365", ServerName = "万里华夏", ServerOpenTime = 1704427200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });

            //2024/06/21 19:00:00 1718967600000 合区 繁星之梦-新年快乐  64/72
            serverItems_1.Add(new ServerItem() { ServerId = 64, ServerIp = $"{ip}:20485", ServerName = "繁星之梦", ServerOpenTime = 1705057200000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 65, ServerIp = $"{ip}:20485", ServerName = "天下无双", ServerOpenTime = 1705662000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 66, ServerIp = $"{ip}:20485", ServerName = "万里冰封", ServerOpenTime = 1705914000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 67, ServerIp = $"{ip}:20485", ServerName = "天空之城", ServerOpenTime = 1706243400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 68, ServerIp = $"{ip}:20485", ServerName = "梦境森林", ServerOpenTime = 1706589000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 69, ServerIp = $"{ip}:20485", ServerName = "勇者之路", ServerOpenTime = 1706871600000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 70, ServerIp = $"{ip}:20485", ServerName = "碧蓝之海", ServerOpenTime = 1707217200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 71, ServerIp = $"{ip}:20485", ServerName = "龙年大吉", ServerOpenTime = 1707476400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 72, ServerIp = $"{ip}:20485", ServerName = "新年快乐", ServerOpenTime = 1707735600000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 73, ServerIp = $"{ip}:20485", ServerName = "恭喜发财", ServerOpenTime = 1708081200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 74, ServerIp = $"{ip}:20485", ServerName = "龙凤呈祥", ServerOpenTime = 1708340400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 75, ServerIp = $"{ip}:20485", ServerName = "万家灯火", ServerOpenTime = 1708686000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 76, ServerIp = $"{ip}:20485", ServerName = "花好月圆", ServerOpenTime = 1708945200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 77, ServerIp = $"{ip}:20485", ServerName = "名扬四海", ServerOpenTime = 1709290800000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 78, ServerIp = $"{ip}:20485", ServerName = "烟雨云烟", ServerOpenTime = 1709550000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 79, ServerIp = $"{ip}:20485", ServerName = "九霄云外", ServerOpenTime = 1709895600000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });

            ///2024/05/24 19:00:00 1716548400000  合区：烽烟四起-万水千山  80/82
            serverItems_1.Add(new ServerItem() { ServerId = 80, ServerIp = $"{ip}:20535", ServerName = "烽烟四起", ServerOpenTime = 1710154800000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 81, ServerIp = $"{ip}:20535", ServerName = "燃烧之地", ServerOpenTime = 1710500400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 82, ServerIp = $"{ip}:20535", ServerName = "万水千山", ServerOpenTime = 1710824400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 83, ServerIp = $"{ip}:20535", ServerName = "梦想之城", ServerOpenTime = 1711105200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            serverItems_1.Add(new ServerItem() { ServerId = 84, ServerIp = $"{ip}:20625", ServerName = "龙的传人", ServerOpenTime = 1711364400000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2024/05/31 19:00:00 1717153200000 合区 春意阑珊-岁月如歌  85/87
            serverItems_1.Add(new ServerItem() { ServerId = 85, ServerIp = $"{ip}:20635", ServerName = "春意阑珊", ServerOpenTime = 1711710000000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 86, ServerIp = $"{ip}:20635", ServerName = "光芒万丈", ServerOpenTime = 1711969200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 87, ServerIp = $"{ip}:20635", ServerName = "岁月如歌", ServerOpenTime = 1712228400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 88, ServerIp = $"{ip}:20635", ServerName = "龙飞凤舞", ServerOpenTime = 1712574000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2024/11/29 19:00:00 1732878000000 合区 梦想之旅-诗情画意 89/93
            serverItems_1.Add(new ServerItem() { ServerId = 89, ServerIp = $"{ip}:20435", ServerName = "诗情画意", ServerOpenTime = 1712919600000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 90, ServerIp = $"{ip}:20435", ServerName = "问鼎江湖", ServerOpenTime = 1713178800000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 91, ServerIp = $"{ip}:20435", ServerName = "万里山河", ServerOpenTime = 1713524400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 92, ServerIp = $"{ip}:20435", ServerName = "征服之海", ServerOpenTime = 1713783600000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 93, ServerIp = $"{ip}:20435", ServerName = "梦想之旅", ServerOpenTime = 1714129200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 94, ServerIp = $"{ip}:20435", ServerName = "时光如梭", ServerOpenTime = 1714388400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 95, ServerIp = $"{ip}:20435", ServerName = "披星戴月", ServerOpenTime = 1714734000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 96, ServerIp = $"{ip}:20435", ServerName = "繁花满地", ServerOpenTime = 1714993200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2024/12/20 19:00:00 1734692400000 合区 萤火森林-长风破浪 97/101
            serverItems_1.Add(new ServerItem() { ServerId = 97, ServerIp = $"{ip}:20555", ServerName = "萤火森林", ServerOpenTime = 1715338800000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 98, ServerIp = $"{ip}:20555", ServerName = "前程似锦", ServerOpenTime = 1715598000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 99, ServerIp = $"{ip}:20555", ServerName = "皓月千里", ServerOpenTime = 1715943600000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 100, ServerIp = $"{ip}:20555", ServerName = "希望之城", ServerOpenTime = 1716202800000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 101, ServerIp = $"{ip}:20555", ServerName = "长风破浪", ServerOpenTime = 1716548400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 102, ServerIp = $"{ip}:20555", ServerName = "璀璨星空", ServerOpenTime = 1717153200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 103, ServerIp = $"{ip}:20555", ServerName = "翡翠之光", ServerOpenTime = 1717758000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 104, ServerIp = $"{ip}:20555", ServerName = "琉璃之海", ServerOpenTime = 1718362800000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2025/01/03 19:00:00 1735902000000 合区 锦绣山河-星辰大海 105/109
            serverItems_1.Add(new ServerItem() { ServerId = 105, ServerIp = $"{ip}:20395", ServerName = "锦绣山河", ServerOpenTime = 1718967600000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 106, ServerIp = $"{ip}:20395", ServerName = "巨龙之魂", ServerOpenTime = 1719572400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 107, ServerIp = $"{ip}:20395", ServerName = "海纳百川", ServerOpenTime = 1720177200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 108, ServerIp = $"{ip}:20395", ServerName = "夏日之歌", ServerOpenTime = 1720436400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 109, ServerIp = $"{ip}:20395", ServerName = "星辰大海", ServerOpenTime = 1720782000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 110, ServerIp = $"{ip}:20395", ServerName = "繁花似锦", ServerOpenTime = 1721041200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 111, ServerIp = $"{ip}:20395", ServerName = "流年不负", ServerOpenTime = 1721386800000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 112, ServerIp = $"{ip}:20395", ServerName = "海之王国", ServerOpenTime = 1721991600000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2024/11/29 19:00:00 1732878000000 合区 纵横驰骋-九州四海 113/115
            serverItems_1.Add(new ServerItem() { ServerId = 113, ServerIp = $"{ip}:20545", ServerName = "纵横驰骋", ServerOpenTime = 1722596400000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 114, ServerIp = $"{ip}:20545", ServerName = "晴空万里", ServerOpenTime = 1723201200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 115, ServerIp = $"{ip}:20545", ServerName = "九州四海", ServerOpenTime = 1723806000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 116, ServerIp = $"{ip}:20545", ServerName = "熔火之心", ServerOpenTime = 1724410800000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2024/12/20 19:00:00 1734692400000 合区 朝花夕拾-龙焰平原 117/119
            serverItems_1.Add(new ServerItem() { ServerId = 117, ServerIp = $"{ip}:20575", ServerName = "朝花夕拾", ServerOpenTime = 1725015600000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 118, ServerIp = $"{ip}:20575", ServerName = "决胜千里", ServerOpenTime = 1725620400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 119, ServerIp = $"{ip}:20575", ServerName = "龙焰平原", ServerOpenTime = 1726225200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 120, ServerIp = $"{ip}:20575", ServerName = "埋骨之地", ServerOpenTime = 1726830000000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2024/11/15 19:00:00 1731668400000 合区 太平盛世-欢度国庆 121/122
            serverItems_1.Add(new ServerItem() { ServerId = 121, ServerIp = $"{ip}:20465", ServerName = "太平盛世", ServerOpenTime = 1727434800000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 122, ServerIp = $"{ip}:20465", ServerName = "欢度国庆", ServerOpenTime = 1728039600000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2024/11/29 19:00:00 1732878000000 合区 春花秋月-光明之旅 123/124
            serverItems_1.Add(new ServerItem() { ServerId = 123, ServerIp = $"{ip}:20525", ServerName = "春花秋月", ServerOpenTime = 1728644400000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 124, ServerIp = $"{ip}:20525", ServerName = "光明之旅", ServerOpenTime = 1729249200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2024/12/20 19:00:00 1734692400000 合区 断桥残雪-南屏晚钟 125/126
            serverItems_1.Add(new ServerItem() { ServerId = 125, ServerIp = $"{ip}:20605", ServerName = "断桥残雪", ServerOpenTime = 1729854000000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 126, ServerIp = $"{ip}:20605", ServerName = "南屏晚钟", ServerOpenTime = 1730458800000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2025/01/03 19:00:00 1735902000000 合区 璀璨梦境-巅峰对决 127/130
            serverItems_1.Add(new ServerItem() { ServerId = 127, ServerIp = $"{ip}:20375", ServerName = "璀璨梦境", ServerOpenTime = 1731063600000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 129, ServerIp = $"{ip}:20375", ServerName = "永恒结界", ServerOpenTime = 1731668400000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 130, ServerIp = $"{ip}:20375", ServerName = "巅峰对决", ServerOpenTime = 1732273200000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2025/01/03 19:00:00 1735902000000 合区 花满楼-星辰之怒 131/132
            serverItems_1.Add(new ServerItem() { ServerId = 131, ServerIp = $"{ip}:20515", ServerName = "花满楼", ServerOpenTime = 1732878000000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 132, ServerIp = $"{ip}:20515", ServerName = "星辰之怒", ServerOpenTime = 1733482800000, New = 0, Show = 0, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            
            
            serverItems_1.Add(new ServerItem() { ServerId = 133, ServerIp = $"{ip}:20495", ServerName = "奇迹之光", ServerOpenTime = 1734087600000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 134, ServerIp = $"{ip}:20565", ServerName = "风暴之怒", ServerOpenTime = 1734692400000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });
            serverItems_1.Add(new ServerItem() { ServerId = 135, ServerIp = $"{ip}:20405", ServerName = "永恒之境", ServerOpenTime = 1735297200000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            //2025/01/03 19:00:00 1735902000000 新区 烽火狼烟 136
            //2025/01/03 19:00:00 1735902000000 合区 锦绣山河-星辰大海 105/109
            //2025/01/03 19:00:00 1735902000000 合区 璀璨梦境-巅峰对决 127/130
            //2025/01/03 19:00:00 1735902000000 合区 花满楼-星辰之怒 131/132
            serverItems_1.Add(new ServerItem() { ServerId = 136, ServerIp = $"{ip}:20415", ServerName = "烽火狼烟", ServerOpenTime = 1735902000000, New = 0, Show = 1, PlatformList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 20001 } });


            ///PlatformHelper.GetPlatformName(); 所有渠道ID定义
            List<int> allserverId = new List<int>();
            for (int i = 0; i < serverItems_1.Count; i++)
            {
                if (allserverId.Contains(serverItems_1[i].ServerId))
                {
                    Console.WriteLine($"服务器ID冲突： {serverItems_1[i].ServerId}");
                }
                else
                {
                    allserverId.Add(serverItems_1[i].ServerId);
                }
            }

            return serverItems_1;
        }
    }
}