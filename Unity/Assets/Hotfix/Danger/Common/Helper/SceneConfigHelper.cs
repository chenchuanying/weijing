using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public static class SceneConfigHelper
    {

        public static bool CanTransfer(int oldScene, int newScene)
        {
            if (newScene == SceneTypeEnum.TeamDungeon 
                && oldScene == SceneTypeEnum.LocalDungeon  )
            {
                return true;
            }
            if (newScene == SceneTypeEnum.Solo
                && (oldScene == SceneTypeEnum.LocalDungeon || oldScene == SceneTypeEnum.JiaYuan))
            {
                return true;
            }

            if (oldScene == newScene
                        && oldScene != SceneTypeEnum.LocalDungeon
                        && oldScene != SceneTypeEnum.JiaYuan
                        && oldScene != SceneTypeEnum.PetDungeon
                        && oldScene != SceneTypeEnum.SeasonTower)
            {
                return false;
            };
            if (oldScene != newScene
                && oldScene > SceneTypeEnum.MainCityScene
                && newScene > SceneTypeEnum.MainCityScene)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 单人副本
        /// </summary>
        /// <param name="sceneTypeEnum"></param>
        /// <returns></returns>
        public static bool IsSingleFuben(int sceneTypeEnum)
        {
            return sceneTypeEnum == SceneTypeEnum.CellDungeon
                || sceneTypeEnum == SceneTypeEnum.PetTianTi
                || sceneTypeEnum == SceneTypeEnum.Tower
                || sceneTypeEnum == SceneTypeEnum.LocalDungeon
                || sceneTypeEnum == SceneTypeEnum.PetDungeon
                || sceneTypeEnum == SceneTypeEnum.RandomTower
                || sceneTypeEnum == SceneTypeEnum.TrialDungeon
                || sceneTypeEnum == SceneTypeEnum.TowerOfSeal
                || sceneTypeEnum == SceneTypeEnum.PetMing
                || sceneTypeEnum == SceneTypeEnum.SeasonTower; 
        }

        public static bool IfSceneCanMove(int sceneType, int sceneid)
        {
            if (sceneType == SceneTypeEnum.Happy
             || sceneType == SceneTypeEnum.PetTianTi
             || sceneType == SceneTypeEnum.PetDungeon
             || sceneType == SceneTypeEnum.PetMing)
            {
                return false;
            }
            if (sceneType == SceneTypeEnum.LocalDungeon)
            {
                if (DungeonConfigCategory.Instance.Get(sceneid).MapType == SceneSubTypeEnum.LocalDungeon_1)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool ShowRightTopButton(int sceneType)
        {
            return sceneType != SceneTypeEnum.Battle
                && sceneType != SceneTypeEnum.TrialDungeon
                && sceneType != SceneTypeEnum.Tower
                && sceneType != SceneTypeEnum.Arena
                && sceneType != SceneTypeEnum.Happy
                && sceneType != SceneTypeEnum.RunRace
                && sceneType != SceneTypeEnum.Demon
                && sceneType != SceneTypeEnum.SeasonTower;
        }

        public static bool ShowLeftButton(int sceneType)
        {
            return sceneType != SceneTypeEnum.TrialDungeon
                && sceneType != SceneTypeEnum.MiJing
                && sceneType != SceneTypeEnum.Happy
                && sceneType != SceneTypeEnum.RunRace
                && sceneType != SceneTypeEnum.Demon;
        }

        public static bool UseSceneConfig(int sceneType)
        {
            return sceneType != SceneTypeEnum.LocalDungeon
                 && sceneType != SceneTypeEnum.CellDungeon;
        }

        public static bool IfCanRevive(int sceneType, int sceneId)
        {
            if (!UseSceneConfig(sceneType))
            {
                return true;
            }
            return SceneConfigCategory.Instance.Get(sceneId).IfUseRes == 0;
        }

        public static bool ShowMiniMap(int sceneType, int sceneId)
        {
            if (!UseSceneConfig(sceneType))
            {
                return true;
            }
            return SceneConfigCategory.Instance.Get(sceneId).ifShowMinMap == 1;
        }

		public static List<int> CreateMonsterList( string createMonster)
		{
			List<int> monsterId = new List<int>();
			if (ComHelp.IfNull(createMonster))
			{
				return monsterId;
			}

			string[] monsters = createMonster.Split('@');
			for (int i = 0; i < monsters.Length; i++)
			{
				if (monsters[i] == "0")
				{
					continue;
				}
				string[] mondels = monsters[i].Split(';');
				int monsterid = int.Parse(mondels[2]);
				if (!MonsterConfigCategory.Instance.Contain(monsterid))
				{
					Log.Error($"monsterid==null {monsterid}");
					continue;
				}
                monsterId.Add(monsterid);
            }
            return monsterId;

        }

        public static List<int> CreateMonsterList(int[] monsterPos)
        {
            List<int> monsterIds = new List<int>();
            if (monsterPos == null || monsterPos.Length == 0)
            {
                return monsterIds;
            }
            for (int i = 0; i < monsterPos.Length; i++)
            {
                int posid = monsterPos[i];
                while (posid != 0)
                {
                    posid = CreateMonsterByPos(posid, monsterIds);
                }
            }
            return monsterIds;
        }

		public static int CreateMonsterByPos(int monsterPos, List<int> monsterIds)
		{
			if (monsterPos == 0)
			{
				return 0;
			}
			
			MonsterPositionConfig monsterPosition = MonsterPositionConfigCategory.Instance.Get(monsterPos);
            monsterIds.Add(monsterPosition.MonsterID);

            return monsterPosition.NextID;
		}

		public static List<int> GetSceneMonsterList( int sceneId)
        {
            List<int> monsterid = new List<int>();

            SceneConfig sceneConfig = SceneConfigCategory.Instance.Get(sceneId);
            if (sceneConfig == null)
            {
                return monsterid;
            }


            monsterid.AddRange(CreateMonsterList(sceneConfig.CreateMonster));
            monsterid.AddRange(CreateMonsterList(sceneConfig.CreateMonsterPosi));
            return monsterid;
        }

        public static string GetLocalDungeonMonsters_2(int mapid)
        {
            if (mapid == 0)
            {
                return string.Empty;
            }

            DungeonConfig chapterSonConfig = DungeonConfigCategory.Instance.Get(mapid);
            string allmonster = string.Empty;
            if (!ComHelp.IfNull(chapterSonConfig.CreateMonster))
            {
                allmonster = chapterSonConfig.CreateMonster;
            }
            if (chapterSonConfig.MonsterGroup != 0)
            {
                MonsterGroupConfig monsterGroupConfig = MonsterGroupConfigCategory.Instance.Get(chapterSonConfig.MonsterGroup);
                if (allmonster != null && allmonster.Length > 1)
                {
                    allmonster = allmonster + "@" + monsterGroupConfig.CreateMonster;
                }
                else
                {
                    allmonster = monsterGroupConfig.CreateMonster;
                }
                //FubenHelp.CreateMonsterList(self.DomainScene(), monsterGroupConfig.CreateMonster);
            }
            if (chapterSonConfig.MonsterPosition != 0)
            {
                int posid = chapterSonConfig.MonsterPosition;
                while (posid != 0)
                {
                    MonsterPositionConfig monsterPosition = MonsterPositionConfigCategory.Instance.Get(posid);
                    //1;157.69,29.24,49.88;41005001;1
                    posid = monsterPosition.NextID;

                    string monsterinfo = string.Empty;
                    int mtype = monsterPosition.Type;
                    int monsterid = monsterPosition.MonsterID;
                    switch (mtype)
                    {
                        case 1:
                            monsterinfo = $"1;{monsterPosition.Position};{monsterid};{monsterPosition.CreateNum}";
                            break;
                        case 2:
                            monsterinfo = $"2;{monsterPosition.Position};{monsterid};{monsterPosition.CreateNum},{monsterPosition.CreateRange}";
                            break;
                        default:
                            break;
                    }

                    if (!string.IsNullOrEmpty(monsterinfo))
                    {
                        if (!string.IsNullOrEmpty(allmonster))
                        {
                            allmonster = allmonster + "@" + monsterinfo;
                        }
                        else
                        {
                            allmonster = monsterinfo;
                        }
                    }
                    if (posid == 0)
                    {
                        break; 
                    }
                }
            }
            return allmonster;
        }



        public static string[] GetPostionMonster(int fubenid, int monsterId, int wave)
        {
            string monsterlist = GetLocalDungeonMonsters_2(fubenid);
            string[] monsters = monsterlist.Split('@');
            for (int i = 0; i < monsters.Length; i++)
            {
                if (ComHelp.IfNull(monsters[i]))
                {
                    continue;
                }
                string[] mondels = monsters[i].Split(';');
                string[] position = mondels[1].Split(',');
                int monsterid = int.Parse(mondels[2]);
                if (monsterid == monsterId || i == wave)
                {
                    return position;
                }
            }
            return null;
        }

        public static Dictionary<int, int> MonsterToFuben = new Dictionary<int, int>();
        public static int GetFubenByMonster(int monsterId)
        {
            if (MonsterToFuben.ContainsKey(monsterId))
            {
                return MonsterToFuben[monsterId];
            }

            List<DungeonConfig> dungeonConfigs = DungeonConfigCategory.Instance.GetAll().Values.ToList();
            for (int i = 0; i < dungeonConfigs.Count; i++)
            {
                List<int> allmonster = SceneConfigHelper.GetLocalDungeonMonsters(dungeonConfigs[i].Id);
                if (allmonster.Contains(monsterId))
                {
                    MonsterToFuben.Add(monsterId, dungeonConfigs[i].Id);
                    return dungeonConfigs[i].Id;
                }
            }
            return 0;
        }

        public static List<int> GetLocalDungeonMonsters(int mapid)
        {
            List<int> monsterIds = new List<int>();
            //DungeonConfig chapterSonConfig = DungeonConfigCategory.Instance.Get(mapid);
            //string createMonster = chapterSonConfig.CreateMonster;
            //if (chapterSonConfig.MonsterGroup != 0)
            //{
            //    MonsterGroupConfig monsterGroupConfig = MonsterGroupConfigCategory.Instance.Get(chapterSonConfig.MonsterGroup);
            //    createMonster = createMonster + "@" + monsterGroupConfig.CreateMonster;
            //}
            string createMonster = GetLocalDungeonMonsters_2(mapid);
            string[] monsters = createMonster.Split('@');
            for (int i = 0; i < monsters.Length; i++)
            {
                if (ComHelp.IfNull(monsters[i]))
                {
                    continue;
                }
                string[] mondels = monsters[i].Split(';');
                string[] monsterid = mondels[2].Split(',');
                monsterIds.Add(int.Parse(monsterid[0]));
            }
            return monsterIds;
        }
    }
}
