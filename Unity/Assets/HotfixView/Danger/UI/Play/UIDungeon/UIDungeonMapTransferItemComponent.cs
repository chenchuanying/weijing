using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIDungeonMapTransferItemComponent: Entity, IAwake<GameObject>
    {
        public GameObject Lab_ChapIndex;
        public GameObject ImageBossIcon;
        public GameObject Lab_EnterLevel;
        public GameObject ImageSelect;
        public GameObject PostionSet;
        public GameObject RightPositionSet;
        public GameObject Lab_ChapSonNameOut;
        public GameObject ButtonEnter;

        public int LevelIndex;
        public int ChapterId;
        public float StartPosX;
        public Action<int> ClickHandler;
        public Vector3 CurPosition;
        public float SendTime;
    }

    public class UIDungeonMapTransferItemComponentAwakeSystem: AwakeSystem<UIDungeonMapTransferItemComponent, GameObject>
    {
        public override void Awake(UIDungeonMapTransferItemComponent self, GameObject gameObject)
        {
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.SendTime = 0f;
            self.LevelIndex = 0;
            self.CurPosition = Vector3.one;
            self.Lab_ChapIndex = rc.Get<GameObject>("Lab_ChapIndex");
            self.ImageBossIcon = rc.Get<GameObject>("ImageBossIcon");
            self.PostionSet = rc.Get<GameObject>("PostionSet");
            self.RightPositionSet = rc.Get<GameObject>("RightPositionSet");
            self.StartPosX = self.PostionSet.transform.localPosition.x;
            self.Lab_ChapSonNameOut = rc.Get<GameObject>("Lab_ChapSonNameOut");
            self.Lab_EnterLevel = rc.Get<GameObject>("Lab_EnterLevel");
            self.ImageSelect = rc.Get<GameObject>("ImageSelect");
            self.ButtonEnter = rc.Get<GameObject>("ButtonEnter");

            ButtonHelp.AddListenerEx(self.ButtonEnter, () => { self.OnEnterChapter().Coroutine(); });
            //self.ImageButton.GetComponent<Button>().onClick.AddListener(() => { self.OnClikButton(); });
        }
    }

    public static class UIDungeonMapTransferItemComponentSystem
    {
        public static void SetSelected(this UIDungeonMapTransferItemComponent self, bool value)
        {
            self.ImageSelect.SetActive(value);
        }

        public static void OnClikButton(this UIDungeonMapTransferItemComponent self)
        {
            self.ClickHandler(self.ChapterId);
        }

        public static void SetClickHandler(this UIDungeonMapTransferItemComponent self, Action<int> action)
        {
            self.ClickHandler = action;
        }

        public static async ETTask OnEnterChapter(this UIDungeonMapTransferItemComponent self)
        {
            try
            {
                if (Time.time - self.SendTime < 2f)
                {
                    return;
                }

                self.SendTime = Time.time;

                if (self.ZoneScene().GetComponent<LockTargetComponent>().LastLockId != 0)
                {
                    FloatTipManager.Instance.ShowFloatTip("战斗状态不能传送地图!");
                    return;
                }
                

                int errorCode = await EnterFubenHelp.RequestTransfer(self.ZoneScene(),
                    SceneTypeEnum.LocalDungeon, 
                    self.ChapterId,
                    self.ZoneScene().GetComponent<MapComponent>().FubenDifficulty);
                if (errorCode != ErrorCode.ERR_Success)
                {
                    ErrorHelp.Instance.ErrorHint(errorCode);
                    return;
                }

                if (self.IsDisposed)
                {
                    return;
                }

                UIHelper.Remove(self.DomainScene(), UIType.UIDungeonMapTransfer);
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }
        }

        public static void OnInitData(this UIDungeonMapTransferItemComponent self, int ChapterIndex, int levelIndex, int levelId)
        {
            self.LevelIndex = levelIndex;
            self.ChapterId = levelId;
            DungeonConfig chapterConfig = DungeonConfigCategory.Instance.Get(levelId);
            self.Lab_ChapSonNameOut.GetComponent<Text>().text = chapterConfig.ChapterName;
            self.Lab_ChapIndex.GetComponent<Text>().text = $"第{levelIndex + 1}关";
            self.Lab_EnterLevel.GetComponent<Text>().text = "挑战等级:" + chapterConfig.EnterLv.ToString();
        }
    }
}