using System;
using System.Collections.Generic;

namespace ET
{
    [ObjectSystem]
    public class UnitCacheDestroySystem : DestroySystem<UnitCache>
    {
        public override void Destroy(UnitCache self)
        {
            foreach (Entity entity in self.CacheCompoenntsDictionary.Values)
            {
                entity.Dispose();
            }
            self.CacheCompoenntsDictionary.Clear();
            self.key = null;
        }
    }

    public static class UnitCacheSystem
    {
        public static void AddOrUpdate(this UnitCache self, Entity entity)
        {
            if (entity == null)
            {
                return;
            }

            if (self.CacheCompoenntsDictionary.TryGetValue(entity.Id, out Entity oldEntity))
            {
                if (entity != oldEntity)
                {
                    oldEntity.Dispose();
                }

                self.CacheCompoenntsDictionary.Remove(entity.Id);
            }

            if (entity.Id == DBHelper.DebugUnitId)
            {
                Log.Warning($"{entity.Id}: {self.CacheCompoenntsDictionary.ContainsKey(entity.Id)} DBHelperAddDB");
            }

            self.CacheCompoenntsDictionary.Add(entity.Id, entity);
        }

        public static async ETTask<Entity> Get(this UnitCache self, long unitId)
        {
            Entity entity = null;
            if (!self.CacheCompoenntsDictionary.TryGetValue(unitId, out entity))
            {
                entity = await Game.Scene.GetComponent<DBComponent>().Query<Entity>(self.DomainZone(), unitId, self.key);
                if (entity != null)
                {
                    self.AddOrUpdate(entity);
                }
                if (self.DomainZone() == 5)
                {
                    //Console.WriteLine($"Get.UnitCache :{entity!=null}  {self.DomainZone()} {unitId}  {self.key}");
                }
            }

            if (entity!= null && entity.Id == DBHelper.DebugUnitId)
            {
                Log.Warning($"{entity.Id}: {self.CacheCompoenntsDictionary.ContainsKey(entity.Id)} DBHelperGetDB");
            }

            return entity;
        }

        public static void Delete(this UnitCache self, long id)
        {
            if (self.CacheCompoenntsDictionary.TryGetValue(id, out Entity entity))
            {
                if (self.DomainZone() == 5)
                {
                    Console.WriteLine($"Delete.UnitCache:  {self.DomainZone()} {id}  {self.key}");
                }

                entity.Dispose();
                self.CacheCompoenntsDictionary.Remove(id);
            }
        }
    }
}