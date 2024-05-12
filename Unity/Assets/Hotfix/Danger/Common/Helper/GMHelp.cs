﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ET
{
    public static class GMHelp
    {

        public static List<string> GmAccount = new List<string>()
        {
            "tcg01",
            "test01",
            "18319670288",          //唐
            "18652422521",
        };

        public static List<long> BanChatPlayer = new List<long>()
        {

        };

        ////if (head == "170" || head == "171" || head == "162" || head == "165" || head == "167" || head == "192")
        public static List<string> IllegalPhone = new List<string>()
        {
            "170","171","162","165","167","192"
        };

        /// <summary>
        /// //{ 2103768474428964866, "9@景沫渺@你作弊了！！！" }
        /// 弹窗玩家
        /// </summary>
        public static Dictionary<long, string> PopUpPlayer = new Dictionary<long, string>()
        {
            // 账号->服务器ID(配0表示全部,9表示先锋五区,与ServerHelper.GetServerList中对应) 角色名字(配0表示全部) 弹出内容
            //{ 2252183319905107986, "0@0@经过大数据排查，我们查询到您使用了第三方违规充值，请主动加QQ群: 719546102 (联系群主) 我们收到消息会主动与你处理此事！" }// 7339802912786291506
        
            { 2288147623695155242, "0@0@经过大数据排查，我们查询到您使用了违规操作，请主动加QQ群: 719546102 (联系群主) 我们收到消息会主动与你处理此事！" },// 13349997943
            { 2324766652299804677, "0@0@经过大数据排查，我们查询到您使用了违规操作，请主动加QQ群: 719546102 (联系群主) 我们收到消息会主动与你处理此事！" },// 661cafba01bc2ff3a137c7c9 已删号
            { 2200452466049351691, "0@0@经过大数据排查，我们查询到您使用了违规操作，请主动加QQ群: 719546102 (联系群主) 我们收到消息会主动与你处理此事！" },//65ae45f2e9fead94daa9e076
            { 2291802434705621005, "0@0@经过大数据排查，我们查询到您使用了违规操作，请主动加QQ群: 719546102 (联系群主) 我们收到消息会主动与你处理此事！" },//17771209756
            { 2279088214817964055, "0@0@经过大数据排查，我们查询到您使用了违规操作，请主动加QQ群: 719546102 (联系群主) 我们收到消息会主动与你处理此事！" },//wxoVumu0sS2oVeshzAO26e0t409m3Y
            { 2254045136688316430, "0@0@经过大数据排查，我们查询到您使用了违规操作，请主动加QQ群: 719546102 (联系群主) 我们收到消息会主动与你处理此事！" },//7340268157128907572
            //{ 2292834549706588169, "0@0@经过大数据排查，我们查询到您使用了违规操作，请主动加QQ群: 719546102 (联系群主) 我们收到消息会主动与你处理此事！" } //wxoVumu0kmRD14OGzQDmkOzGVKXysE
        };

        public static List<string> GetChuJi()
        {
            List<string> vs = new List<string>();
            vs.Add("1#1#1000000");
            vs.Add("1#3#1000000");
            vs.Add("6#20");
            vs.Add("8#10");
            vs.Add("1#10030301#1");
            vs.Add("1#10030303#1");
            vs.Add("1#10030305#1");
            vs.Add("1#10030307#1");
            vs.Add("1#10030309#1");
            vs.Add("1#10030311#1");
            vs.Add("1#10030313#1");
            vs.Add("1#10030315#1");
            vs.Add("1#10030316#1");
            vs.Add("1#10030317#1");
            vs.Add("1#10030320#1");
            vs.Add("1#10010083#999");
            vs.Add("1#10010026#99");


            return vs;
        }

        public static List<string> GetZhongJi()
        {
            List<string> vs = new List<string>();
            vs.Add("6#35");
            vs.Add("1#1#9999999");
            vs.Add("1#3#9999999");
            vs.Add("1#6#9999999");
            vs.Add("8#15");
            vs.Add("1#10030401#1");
            vs.Add("1#10030403#1");
            vs.Add("1#10030405#1");
            vs.Add("1#10030407#1");
            vs.Add("1#10030409#1");
            vs.Add("1#10030411#1");
            vs.Add("1#10030413#1");
            vs.Add("1#10030415#1");
            vs.Add("1#10030416#1");
            vs.Add("1#10030418#1");
            vs.Add("1#10030420#1");
            vs.Add("1#10020212#10");
            vs.Add("1#10020056#99");
            vs.Add("1#10011002#10");
            vs.Add("1#10010083#999");
            vs.Add("1#10010026#99");
            vs.Add("1#10020015#10");

            return vs;
        }

        public static List<string> GetGaopJi()
        {
            List<string> vs = new List<string>();
            vs.Add("6#50");
            vs.Add("6#60");
            vs.Add("1#1#9999999");
            vs.Add("1#3#9999999");
            vs.Add("1#6#9999999");
            vs.Add("8#25");
            vs.Add("1#10030630#1");
            vs.Add("1#10030631#1");
            vs.Add("1#10030632#1");
            vs.Add("1#10030633#1");
            vs.Add("1#10030634#1");
            vs.Add("1#10030635#1");
            vs.Add("1#10030636#1");
            vs.Add("1#10030637#1");
            vs.Add("1#10030638#1");
            vs.Add("1#10030639#1");
            vs.Add("1#10030640#1");
            vs.Add("1#10020212#10");
            vs.Add("1#10020056#99");
            vs.Add("1#10011004#10");
            vs.Add("1#10010083#999");
            vs.Add("1#10010026#99");
            vs.Add("1#10020015#10");
            vs.Add("1#10020063#50");
            vs.Add("1#10020110#50");
            vs.Add("1#10020161#50");
            vs.Add("1#10020215#50");
            vs.Add("1#10020216#50");
            vs.Add("1#10010532#1");
            vs.Add("1#10020209#50");
            vs.Add("1#10020210#50");
            vs.Add("1#10020211#50");

            return vs;
        }

#if !SERVER

        public static async ETTask SendFengHao(Scene zoneScene)
        {
            await ETTask.CompletedTask;

            string filePath = "H:\\FengHao.txt";
            if (!File.Exists(filePath))
            {
                Log.ILog.Debug("不存在");
                return;
            }

            string playerList = string.Empty;

            StreamReader sr = new StreamReader(filePath, Encoding.Default);
            string content;
            while ((content = sr.ReadLine()) != null)
            {
                string account = content.Trim();
                if (string.IsNullOrEmpty(account))
                {
                    continue;
                }

                if (account[0] == '1')
                {
                    playerList += $"{account}_3&";
                }

                Log.ILog.Debug("封号:" + content.ToString());
               C2C_GMCommonRequest request = new C2C_GMCommonRequest()
                {
                    Account = zoneScene.GetComponent<AccountInfoComponent>().Account,
                   Context = $"black2 {content} 35"
                };
                C2C_GMCommonResponse repose = (C2C_GMCommonResponse)await zoneScene.GetComponent<SessionComponent>().Session.Call(request);
            }
            //CreateRobot --Zone=5 --Num=-1 --RobotId=1001
            Log.ILog.Debug(playerList);
        }

        public static void ExcurteGmList(Scene zongscene, List<string> gms)
        {
            for (int i = 0; i < gms.Count; i++)
            {
                SendGmCommand(zongscene, gms[i]);
            }
        }

        public static void SendGmCommand(Scene zongscene, string gm)
        {
            C2M_GMCommandRequest c2M_GMCommandRequest = new C2M_GMCommandRequest() { 
                GMMsg = gm,
                Account = zongscene.GetComponent<AccountInfoComponent>().Account   
            };
            zongscene.GetComponent<SessionComponent>().Session.Send(c2M_GMCommandRequest);
        }

        public static void SendGmCommands(Scene zongscene, string gmlist)
        {
            //if (gmlist.Contains("chuji"))
            //{
            //    ExcurteGmList(zongscene, GetChuJi());
            //    return;
            //}
            //if (gmlist.Contains("zhongji"))
            //{
            //    ExcurteGmList(zongscene, GetZhongJi());
            //    return;
            //}
            //if (gmlist.Contains("gaoji"))
            //{
            //    ExcurteGmList(zongscene, GetGaopJi());
            //    return;
            //}
        }
#endif

        public static int GetRandomNumber()
        {
            return 2;
        }
    }
}
