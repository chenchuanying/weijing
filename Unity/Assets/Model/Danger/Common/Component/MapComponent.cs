using System.Collections.Generic;

namespace ET
{
    public class MapComponent : Entity, ITransfer, IAwake, IDestroy
    {
        public int SceneId;
        public int SonSceneId;
        public int NavMeshId;
        public int SceneTypeEnum;
        public long LastQuitTime = 0;
        public bool OldNavMesh = true;
        public int FubenDifficulty = 0;

    }
}
