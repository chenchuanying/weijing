using System;
using System.Collections.Generic;

namespace ET
{
    public class DBCacheComponentAwakeSystem : AwakeSystem<DBCacheComponent>
    {
        public override void Awake(DBCacheComponent self)
        {
            self.UnitCacheKeyList.Clear();
            foreach (Type type in Game.EventSystem.GetTypes().Values)
            {
                if (type != typeof(IUnitCache) && typeof(IUnitCache).IsAssignableFrom(type))
                {
                    self.UnitCacheKeyList.Add(type.Name);
                }
            }

            foreach (string key in self.UnitCacheKeyList)
            {
                UnitCache unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCaches.Add(key, unitCache);
            }
        }
    }

    public static class DBCacheComponentSystem
    {
        public static async ETTask<Entity> Get(this DBCacheComponent self, long unitId, string key)
        {
            self.SetUnitCacheTime(unitId);

            if (!self.UnitCaches.TryGetValue(key, out UnitCache unitCache))
            {
                if (self.DomainZone() == 5)
                {
                    Console.WriteLine($"self.UnitCaches.Add:  {self.DomainZone()} {key}  {unitId}");
                }
                unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCaches.Add(key, unitCache);
            }
            return await unitCache.Get(unitId);
        }

        public static void SetUnitCacheTime(this DBCacheComponent self, long unitId)
        {
            if (self.WaitDeletUnit.Contains(unitId))
            {
                self.WaitDeletUnit.Remove(unitId);
            }
            if (self.UnitCachesTime.ContainsKey(unitId))
            {
                self.UnitCachesTime[unitId] = self.CurHourTime;
                return;
            }
            self.UnitCachesTime.Add(unitId, self.CurHourTime);
        }

        public static void CheckUnitCacheList(this DBCacheComponent self)
        {
            self.WaitDeletUnit.Clear();
            long serverTime = TimeHelper.ServerNow();
            int zone = self.DomainZone();

            foreach ((long unitid, long lasttime) in self.UnitCachesTime)
            {
                if (lasttime != 0 && serverTime - lasttime > TimeHelper.Hour * 4)
                {
                    if (!self.WaitDeletUnit.Contains(unitid))
                    {
                        self.WaitDeletUnit.Add(unitid);
                    }
                }
            }

            int removeNumber = 200;
            Log.Warning($"待移除缓存玩家: {self.DomainZone()} {self.WaitDeletUnit.Count}");
            for (int i = self.WaitDeletUnit.Count - 1; i >= 0; i--)
            {
                //Log.Console($"长期离线，移除玩家11: {zone}  {self.WaitDeletUnit[i]}");
                self.DeleteRole(self.WaitDeletUnit[i]);
                removeNumber--;
                if (removeNumber <= 0)
                {
                    break;
                }
            }
            self.WaitDeletUnit.Clear();
            self.CurHourTime = TimeHelper.ServerNow();
        }

        public static void DeleteRole(this DBCacheComponent self, long unitId)
        {
            self.Delete(unitId);

            //长期离线玩家移除
            if (ConfigLoader.RemovePlayer)
            {
                Log.Console($"长期离线，移除玩家22: {self.DomainZone()}  {unitId}");
                List<string> allComponets = DBHelper.GetAllUnitComponent();
                for (int i = 0; i < allComponets.Count; i++)
                {
                    Game.Scene.GetComponent<DBComponent>().Remove<Entity>(self.DomainZone(), unitId, allComponets[i]).Coroutine();
                }
            }
        }

        public static async ETTask<T> Get<T>(this DBCacheComponent self, long unitId) where T : Entity
        {
            self.SetUnitCacheTime(unitId);

            string key = typeof(T).Name;
            if (!self.UnitCaches.TryGetValue(key, out UnitCache unitCache))
            {
                unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCaches.Add(key, unitCache);
            }
            return await unitCache.Get(unitId) as T;
        }

        public static void Delete(this DBCacheComponent self, long unitId)
        {
            foreach (UnitCache cache in self.UnitCaches.Values)
            {
                cache.Delete(unitId);
            }
        }

        public static async ETTask AddOrUpdate(this DBCacheComponent self, long id, Entity entity)
        {
            self.SetUnitCacheTime(id);

            string key = entity.GetType().Name;
            if (!self.UnitCaches.TryGetValue(key, out UnitCache unitCache))
            {
                unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCaches.Add(key, unitCache);
            }
            unitCache.AddOrUpdate(entity);
            await Game.Scene.GetComponent<DBComponent>().Save(self.DomainZone(), id, entity);
        }

        public static async ETTask AddOrUpdate(this DBCacheComponent self, long id, ListComponent<Entity> entityList)
        {
            self.SetUnitCacheTime(id);

            using (ListComponent<Entity> list = ListComponent<Entity>.Create())
            {
                foreach (Entity entity in entityList)
                {
                    string key = entity.GetType().Name;
                    if (!self.UnitCaches.TryGetValue(key, out UnitCache unitCache))
                    {
                        unitCache = self.AddChild<UnitCache>();
                        unitCache.key = key;
                        self.UnitCaches.Add(key, unitCache);
                    }
                    unitCache.AddOrUpdate(entity);
                    list.Add(entity);

                    if (id == DBHelper.DebugUnitId)
                    {
                        Log.Warning($"CacheCompoenntsDictionary.Count:  {key} {unitCache.CacheCompoenntsDictionary.Count} ");
                    }
                }

                if (list.Count > 0)
                {
                    await Game.Scene.GetComponent<DBComponent>().Save(self.DomainZone(), id, list);
                }
            }
        }
    }
}