﻿using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace ET
{
    /// <summary>
    /// 用来缓存数据
    /// </summary>
    public class DBComponent: Entity, IAwake, IAwake<string, string, int>, IDestroy
    {
		public static DBComponent Instance;

		public List<string> Transfers = new List<string>();
		public const int TaskCount = 32;
		public Dictionary<int, IMongoDatabase> ZoneDatabases = new Dictionary<int, IMongoDatabase>();


		public IMongoCollection<T> GetCollection<T>(int zone, string collection = null)
		{
			return this.ZoneDatabases[zone].GetCollection<T>(collection ?? typeof(T).Name);
		}

		public IMongoCollection<Entity> GetCollection(int zone, string name)
		{
			return this.ZoneDatabases[zone].GetCollection<Entity>(name);
		}
	}
}