using System.Collections.Generic;

namespace ET
{
    public static  class PlayerInfoListComponentSystem
    {

        public static bool IsArchiveing(this PlayerInfoListComponent self, string account, long unitid)
        {

            for (int i = 0; i < self.ArchivePlayerList.Count; i++)
            {
                if (self.ArchivePlayerList[i].Key.Equals(unitid)
                    || self.ArchivePlayerList[i].Value.Equals(account))
                { 
                    return true;    
                }
            }

            return false;

        }


        public static bool OnAddArchive(this PlayerInfoListComponent self, string account, long unitid, int archive)
        {
            for (int i = self.ArchivePlayerList.Count - 1; i >= 0; i--)
            {
                if (self.ArchivePlayerList[i].Key.Equals(unitid)
                    || self.ArchivePlayerList[i].Value.Equals(account))
                {
                    if (archive == 1)
                    {
                        return false;
                    }
                    else
                    {
                        self.ArchivePlayerList.RemoveAt(i); 
                    }
                }
            }
            if (archive == 1)
            {
                self.ArchivePlayerList.Add(new KeyValuePair<long, string>(unitid, account));
            }
            return true;
        }

    }
}
