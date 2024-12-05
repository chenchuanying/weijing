using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace ET
{


    [Timer(TimerType.TransferUITimer)]
    public class TransferUITimer : ATimer<TransferUIComponent>
    {
        public override void Run(TransferUIComponent self)
        {
            try
            {
                self.OnTimer();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    public class TransferUIComponentDestroySystem : DestroySystem<TransferUIComponent>
    {
        public override void Destroy(TransferUIComponent self)
        {
            TimerComponent.Instance?.Remove(ref self.Timer);

            if (self.HeadBar != null)
            {
                GameObject.Destroy(self.HeadBar);
                self.HeadBar = null;
            }
        }
    }

    public class TransferUIComponentAwakeSystem : AwakeSystem<TransferUIComponent>
    {
        public override void Awake(TransferUIComponent self)
        {
            self.HeadBar = null;
            self.EnterRange = false;
            self.InitTime = TimeHelper.ServerNow() + TimeHelper.Second * 3; ;
            self.UICamera = GameObject.Find("Global/UI/UICamera").GetComponent<Camera>();
            self.MainCamera = GameObject.Find("Global/Main Camera").GetComponent<Camera>();
        }
    }

    public static class TransferUIComponentSystem
    {
        public static async ETTask OnInitUI(this TransferUIComponent self, int transferId)
        {
            DungeonTransferConfig monsterConfig = DungeonTransferConfigCategory.Instance.Get(transferId);
            string path = ABPathHelper.GetUGUIPath("Blood/UITransfer");
            GameObject prefab = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
            self.HeadBar = UnityEngine.Object.Instantiate(prefab, GlobalComponent.Instance.Unit, true);
            self.HeadBar.transform.SetParent(UIEventComponent.Instance.UILayers[(int)UILayer.Low]);
            self.HeadBar.transform.localScale = Vector3.one;

            self.UIPosition = self.GetParent<Unit>().GetComponent<GameObjectComponent>().GameObject.transform.Find("UIPosition");
            self.HeadBar.Get<GameObject>("Lal_Name").GetComponent<Text>().text = monsterConfig.Name;

            if (self.HeadBar.GetComponent<HeadBarUI>() == null)
            {
                self.HeadBar.AddComponent<HeadBarUI>();
            }
            self.HeadBarUI = self.HeadBar.GetComponent<HeadBarUI>();
            self.HeadBarUI.HeadPos = self.UIPosition;
            self.HeadBarUI.HeadBar = self.HeadBar;
        }

        public static void LateUpdate(this TransferUIComponent self)
        {
            if (self.HeadBar == null)
            {
                return;
            }
            Transform UIPosition = self.UIPosition;
            Vector2 OldPosition = WorldPosiToUIPos.WorldPosiToUIPosition(UIPosition.position, self.HeadBar, self.UICamera, self.MainCamera);

            Vector3 NewPosition = Vector3.zero;
            NewPosition.x = OldPosition.x;
            NewPosition.y = OldPosition.y;
            self.HeadBar.transform.localPosition = NewPosition;
        }

        public static void OnTimer(this TransferUIComponent self)
        {
            if (!self.EnterRange)
            {
                return;
            }
            EnterFubenHelp.RequestTransfer(self.ZoneScene(),
                (int)SceneTypeEnum.LocalDungeon,
                0,
                self.ZoneScene().GetComponent<MapComponent>().FubenDifficulty,
                self.GetParent<Unit>().ConfigId.ToString() ).Coroutine();
        }

        public static void StartTimer(this TransferUIComponent self)
        {
            TimerComponent.Instance?.Remove(ref self.Timer);
            long leftTime = self.InitTime - TimeHelper.ServerNow();
            leftTime = Math.Max( 10, leftTime );
            self.Timer = TimerComponent.Instance.NewOnceTimer(  TimeHelper.ServerNow() + leftTime,  TimerType.TransferUITimer, self );
        }

        public static void OnCheckChuanSong(this TransferUIComponent self, Unit mainhero)
        {
           
            Vector3 vector3 = self.GetParent<Unit>().Position;
            float distance = PositionHelper.Distance2D(vector3, mainhero.Position);

            if (distance <= 1.5f && !self.EnterRange)
            {
                self.EnterRange = true;
                if (UnitHelper.IsHaveBoss(mainhero.DomainScene(), vector3, 8f))
                {
                    PopupTipHelp.OpenPopupTip(self.ZoneScene(), "系统提示", "附近有领主出现,请问是否进入新地图?", () =>
                    {
                        self.StartTimer();
                    }, null).Coroutine();
                }
                else
                {
                    self.StartTimer();
                }
            }
            if (distance > 1.5f && self.EnterRange)
            {
                self.EnterRange = false;
            }
        }
    }
}
