﻿using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace ET
{
    public static class MapHelper
    {

        public static void LogMoveInfo(string message)
        {
            Log.Debug(message);
        }

        public static void  SendZhaoHuan(Scene zoneScene)
        {
            try
            {
                Unit unit = UnitHelper.GetMyUnitFromZoneScene(zoneScene);
                if (unit == null)
                {
                    return;
                }
                C2M_CreateSpiling zhaohuancmd = new C2M_CreateSpiling()
                {
                    X = unit.Position.x,
                    Y = unit.Position.y,
                    Z = unit.Position.z,
                    ParentUnitId = unit.Id
                };
                zoneScene.GetComponent<SessionComponent>().Session.Send(zhaohuancmd);
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static Unit GetNearestUnit(Unit main)
        {
            List<Entity> units = main.ZoneScene().CurrentScene().GetComponent<UnitComponent>().Children.Values.ToList();
            Unit nearest = null;
            float distance = -1f;
            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i] as Unit;
                if (main.Id == unit.Id)
                {
                    continue;
                }
               
                float dd = Vector3.Distance(main.Position, unit.Position);
                if (dd > distance && distance > 0)
                {
                    continue;
                }
                if (!main.IsCanAttackUnit(unit))
                {
                    continue;
                }
                if (distance < 0f || dd < distance)
                {
                    nearest = unit;
                    distance = dd;
                }
            }
            return nearest;
        }

        public static Unit GetNearItem(Scene zoneScene)
        {
            float distance = 10f;
            Unit unit = null;
            Unit main = UnitHelper.GetMyUnitFromZoneScene(zoneScene);
            List<Unit> units = zoneScene.CurrentScene().GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < units.Count; i++)
            {
                Unit uu = units[i] as Unit;
                if (uu.Type != UnitType.DropItem)
                {
                    continue;
                }
                float dd = PositionHelper.Distance2D(main, uu);
                if (dd < distance)
                {
                    unit = uu;
                    distance = dd;
                }
            }
            return unit;
        }

        public static long GetChestBox(Scene zoneScene)
        {
            float distance = 10f;
            Unit unit = null;
            Unit main = UnitHelper.GetMyUnitFromZoneScene(zoneScene);
            List<Unit> units = zoneScene.CurrentScene().GetComponent<UnitComponent>().GetAll();
            for (int i = 0; i < units.Count; i++)
            {
                Unit uu = units[i];
                if (!uu.IsChest())
                {
                    continue;
                }
                float dd = PositionHelper.Distance2D(main, uu);
                if (dd < distance)
                {
                    unit = uu;
                    distance = dd;
                }
            }
            return unit!=null ? unit.Id : 0;
        }

        public static List<DropInfo> GetCanShiQuByCell(Scene zoneScene, int cell)
        {
            List<DropInfo> ids = new List<DropInfo>();
            List<Entity> units = zoneScene.CurrentScene().GetComponent<UnitComponent>().Children.Values.ToList();
            for (int i = 0; i < units.Count; i++)
            {
                Unit uu = units[i] as Unit;
                if (uu.Type != UnitType.DropItem)
                {
                    continue;
                }
                int dropcell = uu.GetComponent<DropComponent>().CellIndex;
                if (dropcell == cell)
                {
                    ids.Add(uu.GetComponent<DropComponent>().DropInfo);
                }
                if (ids.Count >= 20)
                {
                    break;
                }
            }
            return ids;
        }

        public static List<DropInfo> GetCanShiQu(Scene zoneScene, float distance)
        {
            List<DropInfo> ids = new List<DropInfo>();
            List<Entity> units = zoneScene.CurrentScene().GetComponent<UnitComponent>().Children.Values.ToList();
            for (int i = 0; i < units.Count; i++)
            {
                Unit uu = units[i] as Unit;
                if (uu.Type != UnitType.DropItem)
                {
                    continue;
                }
                if (PositionHelper.Distance2D(UnitHelper.GetMyUnitFromZoneScene(zoneScene), uu) < 3f)
                {
                    ids.Add(uu.GetComponent<DropComponent>().DropInfo);
                }
                if (ids.Count >= 20)
                {
                    break;
                }
            }
            return ids;
        }

        public static async ETTask SendShiquItem(Scene zoneScene, List<DropInfo> ids)
        {
            try
            {
                Actor_PickItemRequest actor_PickItemRequest = new Actor_PickItemRequest() { ItemIds = ids };
                Actor_PickItemResponse actor_PickItemResponse = await zoneScene.GetComponent<SessionComponent>().Session.Call(actor_PickItemRequest) as Actor_PickItemResponse;

                for (int i = 0; i < ids.Count; i++)
                {
                    if (ids[i].DropType == 1)
                    {
                        //私有掉落，本地移除
                        zoneScene.CurrentScene().GetComponent<UnitComponent>().Remove(ids[i].UnitId);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }
    }
}
