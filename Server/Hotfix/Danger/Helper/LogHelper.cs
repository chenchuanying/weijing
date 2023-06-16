﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.Text;

namespace ET
{
    public static class LogHelper
    {

        /// <summary>
        /// 0 无日志 1 info  2debug  3 waring 4 error
        /// </summary>
        public static int LogLevel = 3;

        public static void LogWarning(string msg, bool log = false)
        {
            if (LogLevel >= 3 && log)
            {
                Log.Warning(msg);
            }
        }

        public static void LogDebug(string msg)
        {
            if (LogLevel >= 2)
            {
                Log.Debug(msg);
            }
        }

        public static List<string> KillInfoList = new List<string>();
        public static void KillPlayerInfo(Unit attack, Unit defend)
        {
            if (attack.IsRobot() || defend.IsRobot())
            {
                return;
            }
            int zone = attack.DomainZone();
            ServerItem serverItem  = ServerHelper.GetGetServerItem(false, zone);
            if (serverItem == null)
            {
                return;
            }
            
            MapComponent mapComponent = attack.DomainScene().GetComponent<MapComponent>();
            if (!SceneConfigHelper.UseSceneConfig(mapComponent.SceneTypeEnum))
            {
                return;
            }
            string serverName = serverItem.ServerName;
            string sceneName = SceneConfigCategory.Instance.Get(mapComponent.SceneId).Name;

            UserInfoComponent attackUserinfo = attack.GetComponent<UserInfoComponent>();
            UserInfoComponent defendUserinfo = defend.GetComponent<UserInfoComponent>();
            string attackName = attackUserinfo.UserInfo.Name;
            string defendName = defendUserinfo.UserInfo.Name;
            attackName = attack.IsRobot() ? $"{attackName}（人机）" : attackName;
            defendName = defend.IsRobot() ? $"{defendName}（人机）" : defendName;
            int attackOcc = attackUserinfo.UserInfo.OccTwo > 0 ? attackUserinfo.UserInfo.OccTwo : attackUserinfo.UserInfo.Occ;
            int defendOcc = defendUserinfo.UserInfo.OccTwo > 0 ? defendUserinfo.UserInfo.OccTwo : defendUserinfo.UserInfo.Occ;

            string log = $"{TimeHelper.DateTimeNow().ToString()}:  {serverName}：{sceneName}： {attackName} 等级({attackUserinfo.UserInfo.Lv}) 职业:({attackOcc}) 战力:({attackUserinfo.UserInfo.Combat}) 击杀了： {defendName} 等级({defendUserinfo.UserInfo.Lv}) 职业:({defendOcc}) 战力:({defendUserinfo.UserInfo.Combat})";
            KillInfoList.Add(log);
            if (KillInfoList.Count >= 10)
            {
                string filePath = "../Logs/WJ_KillPlayer.txt";
                WriteLogList(KillInfoList, filePath);
                KillInfoList.Clear();
            }

        }

        public static string GetNotice()
        {
            string filePath = "../Logs/WJ_Notice.txt";
            if (File.Exists(filePath))
            {
                StreamReader sr = new StreamReader(filePath, Encoding.Default);
                string notice = string.Empty;
                string content = string.Empty;
                int index = 0;
                while ((content = sr.ReadLine()) != null)
                {
                    if (index == 0)
                    {
                        notice = $"{content}";
                    }
                    if (index == 1)
                    {
                        notice += $"@{content}";
                    }
                    if (index >= 2)
                    {
                        notice += $"\r\n{content}";
                    }
                    index++;
                }
                return notice;
            }
            else
            {
                return string.Empty;
            }
        }

        public static void OnStopServer()
        {
            string filePath = "../Logs/WJ_login.txt";
            WriteLogList(LoginInfoList, filePath);
            LoginInfoList.Clear();

            filePath = "../Logs/WJ_ZuoBi.txt";
            WriteLogList(ZuobiInfoList, filePath);
            ZuobiInfoList.Clear();

            filePath = "../Logs/WJ_KillPlayer.txt";
            WriteLogList(KillInfoList, filePath);
            KillInfoList.Clear();
        }

        public static void WriteLogList(List<string> infolist, string filePath)
        {
            //if (File.Exists(filePath))
            //{
            //    StreamWriter sw = File.AppendText(filePath);

            //    for (int i = 0; i < infolist.Count; i++)
            //    {
            //        sw.WriteLineAsync(infolist[i]);
            //    }

            //    sw.Flush();
            //    sw.Close();
            //}
            //else
            //{
            //    FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
            //    StreamWriter sw = new StreamWriter(fs);
            //    //开始写入
            //    for (int i = 0; i < infolist.Count; i++)
            //    {
            //        sw.WriteLineAsync(infolist[i]);
            //    }
            //    //清空缓冲区
            //    sw.Flush();
            //    //关闭流
            //    sw.Close();
            //    fs.Close();
            //}
        }

        public static List<string> LoginInfoList = new List<string>();
        public static void LoginInfo(string log)
        {
            log = TimeHelper.DateTimeNow().ToString() + " " + log;
            LoginInfoList.Add(log);
            if (LoginInfoList.Count >= 100)
            {
                string filePath = "../Logs/WJ_login.txt";
                WriteLogList(LoginInfoList, filePath);

                LoginInfoList.Clear();
            }
        }

        public static List<string> ZuobiInfoList  = new List<string>(); 
        public static void ZuobiInfo(string log)
        {
            log = TimeHelper.DateTimeNow().ToString() + " " + log;
            ZuobiInfoList.Add(log);
            if (ZuobiInfoList.Count >= 10)
            {
                string filePath = "../Logs/WJ_ZuoBi.txt";
                WriteLogList(ZuobiInfoList, filePath);
                ZuobiInfoList.Clear();
            }
        }

        /// <summary>
        /// 每小时检测一次
        /// </summary>
        /// <param name="unit"></param>
        public static void CheckZuoBi(Unit unit)
        {
            UserInfoComponent userInfo = unit.GetComponent<UserInfoComponent>();

            //GM账号免于检测
            if (GMHelp.GmAccount.Contains(userInfo.Account)) {
                return;
            }

            int openDay = DBHelper.GetOpenServerDay(unit.DomainZone());
            //钻石线
            if (userInfo.UserInfo.Diamond >= unit.GetComponent<NumericComponent>().GetAsLong(NumericType.RechargeNumber) * 150 + 50000)
            {
                LogHelper.ZuobiInfo("钻石作弊:" + userInfo.UserInfo.Diamond + "服务器:" + unit.DomainZone() + "名字:" + userInfo.UserName);
            }

            //等级线
            ServerInfo serverInfo = unit.DomainScene().GetComponent<ServerInfoComponent>().ServerInfo;
            if (userInfo.UserInfo.Lv > serverInfo.WorldLv) 
            {
                LogHelper.ZuobiInfo("玩家等级超过服务器等级限制:" + userInfo.UserInfo.Lv + "服务器:" + unit.DomainZone() + "名字:" + userInfo.UserName);
            }

            if (openDay <= 180 || userInfo.UserInfo.Lv < 60)
            {
                //金币线
                if (userInfo.UserInfo.Gold >= unit.GetComponent<NumericComponent>().GetAsLong(NumericType.RechargeNumber) * 300000 + 5000000 + userInfo.UserInfo.Lv * 500000)
                {
                    LogHelper.ZuobiInfo("金币作弊:" + userInfo.UserInfo.Diamond + "服务器:" + unit.DomainZone() + "名字:" + userInfo.UserName);
                }

                //道具线
                if (unit.GetComponent<BagComponent>().GetItemNumber(10010083) > 1000 + unit.GetComponent<NumericComponent>().GetAsLong(NumericType.RechargeNumber) * 10) {
                    LogHelper.ZuobiInfo("洗练石作弊:" + unit.GetComponent<BagComponent>().GetItemNumber(10010083) + "服务器:" + unit.DomainZone() + "名字:" + userInfo.UserName);
                }
            }

            BagComponent bagComponent = unit.GetComponent<BagComponent>();
            List<BagInfo> bagInfos =  bagComponent.GetAllItems();
            for (int i = 0; i < bagInfos.Count; i++)
            {
                if (bagInfos[i].ItemID > 100 && bagInfos[i].ItemNum >= 10000)
                {
                    LogHelper.ZuobiInfo($"道具数量异常： {unit.DomainZone()} {unit.Id} {bagInfos[i].ItemID} {bagInfos[i].ItemNum} ");
                }
            }
        }
    }
}
