using UnityEngine;

namespace ET
{

    public static class SceneManagerSystem
    {

        public static void  UpdateChuanSong(this SceneManagerComponent self, Scene scene, int sceneTypeEnum)
        {
            AdditiveHide[] additiveHides = (AdditiveHide[])GameObject.FindObjectsOfType(typeof(AdditiveHide));
            for (int i = 0; i < additiveHides.Length; i++)
            {
                additiveHides[i].ToggleShow();
            }
            if (sceneTypeEnum == (int)SceneTypeEnum.CellDungeon)
            {
                //显示传送
                GameObject ChuanSongPosiSet = GameObject.Find("AdditiveHide/ChuanSongPosiSet");
                if (ChuanSongPosiSet == null)
                    return;
                ChuanSongPosiSet.SetActive(true);
                CellDungeonComponent fubenComponent = scene.GetComponent<CellDungeonComponent>();
                bool isEnd = fubenComponent.IsEndCell();
                if (isEnd)
                {
                    ChuanSongPosiSet.SetActive(false);
                    return;
                }
                for (int i = 0; i < fubenComponent.SonFubenInfo.PassableFlag.Count; i++)
                {
                    string path = $"NoChuanSong_{i + 1}";
                    GameObject obj = ChuanSongPosiSet.transform.Find(path).gameObject;
                    if (obj != null)
                    {
                        obj.SetActive(fubenComponent.SonFubenInfo.PassableFlag[i] == 0);
                    }
                }
            }
        }
        public static async ETTask ChangeSonScene(this SceneManagerComponent self, Scene scene, int sceneTypeEnum, string paramss)
        {
            scene.GetComponent<SkillIndicatorComponent>().BeginEnterScene();
            scene.GetComponent<LockTargetComponent>().BeginEnterScene();

            var path = ABPathHelper.GetScenePath(paramss);
            await ResourcesComponent.Instance.LoadSceneAdditive(path);

            self.UpdateChuanSong(scene, (int)sceneTypeEnum);
            //刷新主界面
            UI ui = UIHelper.GetUI(scene, UIType.UIMain);
            UIMainComponent uimain = ui.GetComponent<UIMainComponent>();
            uimain.AfterEnterScene(sceneTypeEnum);
        }

        public static async ETTask ChangeScene(this SceneManagerComponent self, Scene scene, int sceneTypeEnum,int lastScene, int chapterId)
        {
            string paramss = "";
            switch (sceneTypeEnum)
            {
                case (int)SceneTypeEnum.InitScene:
                    paramss = "Init";
                    break;
                case (int)SceneTypeEnum.LoginScene:
                    paramss = "Login";
                    break;
                case (int)SceneTypeEnum.MainCityScene:
                    AccountInfoComponent accountInfoComponent = scene.GetComponent<AccountInfoComponent>();
                    string scenepath = accountInfoComponent.Account == "tcg01" ? "101_test" : chapterId.ToString();
                    paramss = scenepath;
                    //paramss = chapterId.ToString();
                    break;
                case (int)SceneTypeEnum.CellDungeon:
                    paramss = ChapterSonConfigCategory.Instance.Get(scene.GetComponent<MapComponent>().SonSceneId).MapID.ToString();
                    break;
                case (int)SceneTypeEnum.LocalDungeon:
                    paramss = DungeonConfigCategory.Instance.Get(chapterId).MapID.ToString();
                    break;
                default:
                    paramss = SceneConfigCategory.Instance.Get(chapterId).MapID.ToString();
                    break;
            }

            int sousceneid = scene.GetComponent<MapComponent>().SonSceneId;
            GameObjectPoolComponent.Instance.DisposeAll();

            await ResourcesComponent.Instance.LoadEmptyScene(ABPathHelper.GetScenePath("Empty"));
            await TimerComponent.Instance.WaitFrameAsync();

            GameObjectPoolComponent.Instance.DisposeAll();

            var path = ABPathHelper.GetScenePath(paramss);
            await ResourcesComponent.Instance.LoadSceneAsync(path);
            self.UpdateChuanSong(scene, sceneTypeEnum);

            Game.Scene.GetComponent<SoundComponent>().PlayBgmSound(sceneTypeEnum, chapterId, sousceneid);
        }

    }
}
