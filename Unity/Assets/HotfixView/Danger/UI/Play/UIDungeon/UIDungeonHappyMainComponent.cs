using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{

    [Timer(TimerType.DungeonHappyMainTimer)]
    public class UIDungeonHappyMainTimer : ATimer<UIDungeonHappyMainComponent>
    {
        public override void Run(UIDungeonHappyMainComponent self)
        {
            try
            {
                self.OnUpdate();
            }
            catch (Exception e)
            {
                Log.Error($"move timer error: {self.Id}\n{e}");
            }
        }
    }

    public class UIDungeonHappyMainComponent : Entity, IAwake, IDestroy
    {

        public Text TextTip_MianFeiTime;
        public Text EndTimeText;

        public Text TextTip_3;
        public Text TextTip_2;
        public Text TextTip_1;

        public GameObject ButtonMove_3;
        public GameObject ButtonMove_2;
        public GameObject ButtonMove_1;

        public GameObject ButtonPick;


        public long NextMoveTime;

        public long EndTime;

        public long Timer;

        public string DefaultTime = "0:0";
    }

    public class UIDungeonHappyMainComponentAwake : AwakeSystem<UIDungeonHappyMainComponent>
    {
        public override void Awake(UIDungeonHappyMainComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.EndTimeText = rc.Get<GameObject>("EndTimeText").GetComponent<Text>();
          
            self.TextTip_3 = rc.Get<GameObject>("TextTip_3").GetComponent<Text>();
            self.TextTip_2 = rc.Get<GameObject>("TextTip_2").GetComponent<Text>();
            self.TextTip_3.gameObject.SetActive(false);
            self.TextTip_2.gameObject.SetActive(false);

            self.TextTip_MianFeiTime = rc.Get<GameObject>("TextTip_MianFeiTime").GetComponent<Text>();

            self.TextTip_1 = rc.Get<GameObject>("TextTip_1").GetComponent<Text>();

            self.ButtonMove_3 = rc.Get<GameObject>("ButtonMove_3");
            self.ButtonMove_3.SetActive(false);
            self.ButtonMove_2 = rc.Get<GameObject>("ButtonMove_2");
            self.ButtonMove_2.SetActive(false);

            self.ButtonMove_1 = rc.Get<GameObject>("ButtonMove_1");
            ButtonHelp.AddListenerEx(self.ButtonMove_3, () => { self.OnButtonMove(3).Coroutine(); });
            ButtonHelp.AddListenerEx(self.ButtonMove_2, () => { self.OnButtonMove(2).Coroutine(); });
            ButtonHelp.AddListenerEx(self.ButtonMove_1, () => { self.OnButtonMove(1).Coroutine(); });

            self.ButtonPick = rc.Get<GameObject>("ButtonPick");
            ButtonHelp.AddListenerEx(self.ButtonPick, () => { self.OnButtonPick(); });

            self.EndTime = TimeHelper.ServerNow() + TimeHelper.Minute * 10;
            self.Timer = TimerComponent.Instance.NewRepeatedTimer(TimeHelper.Second, TimerType.DungeonHappyMainTimer, self);
            self.OnUpdateMoney();
            self.OnUpdate();
        }
    }

    public class UIDungeonHappyMainComponentDestroy : DestroySystem<UIDungeonHappyMainComponent>
    {
        public override void Destroy(UIDungeonHappyMainComponent self)
        {
            TimerComponent.Instance?.Remove(ref self.Timer);
        }
    }

    public static class UIDungeonHappyMainComponentSystem
    {

        public static void OnUpdate(this UIDungeonHappyMainComponent self)
        {
            long endTime = self.EndTime -TimeHelper.ServerNow();
            endTime /= 1000;
            if (endTime > 60)
            {
                self.EndTimeText.text = $"神秘之门倒计时 {endTime / 60}:{endTime % 60}";
            }
            else
            {
                self.EndTimeText.text = $"神秘之门还剩{endTime % 60}秒，活动结束将强制离开地图哦。";
            }
            if (endTime <= 60)
            {
                TimerComponent.Instance.Remove(ref self.Timer); 
                int sceneid = self.ZoneScene().GetComponent<BattleMessageComponent>().LastDungeonId;
                if (sceneid == 0)
                {
                    EnterFubenHelp.RequestQuitFuben(self.ZoneScene());
                }
                else
                {
                    MapComponent mapComponent = self.ZoneScene().GetComponent<MapComponent>();
                    EnterFubenHelp.RequestTransfer(self.ZoneScene(), SceneTypeEnum.LocalDungeon, sceneid, mapComponent.FubenDifficulty, "0").Coroutine();
                }
                return;
            }


            long moveTime = self.NextMoveTime - TimeHelper.ServerNow();
            if (moveTime >= 0)
            {
                moveTime /= 1000;
                int minute = (int)(moveTime / 60);
                int second = (int)(moveTime % 60);
                self.TextTip_1.text = $"{minute}:{second}";
            }
            else
            {
                self.TextTip_1.text = "可以进行移动";
            }
        }

        public static void OnUpdateMoney(this UIDungeonHappyMainComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            long lastmovetime = unit.GetComponent<NumericComponent>().GetAsLong(NumericType.HappyMoveTime);
            self.NextMoveTime = lastmovetime;

            //金币消耗
            GlobalValueConfig globalValueConfig1 = GlobalValueConfigCategory.Instance.Get(94);
            self.TextTip_2.text = $"金币消耗:{globalValueConfig1.Value2}";

            //钻石消耗
            GlobalValueConfig globalValueConfig2 = GlobalValueConfigCategory.Instance.Get(95);
            self.TextTip_3.text = $"钻石消耗:{globalValueConfig2.Value2}";

            int useTimes = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.HappyMoveNumber);
            self.TextTip_MianFeiTime.text = $"移动次数: {5 -useTimes}/5";
        }

        public static void OnButtonPick(this UIDungeonHappyMainComponent self)
        {
            if (self.ZoneScene().GetComponent<BagComponent>().GetBagLeftCell() <= 0)
            {
                ErrorHelp.Instance.ErrorHint(ErrorCode.ERR_BagIsFull);
                return;
            }
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int cellindex = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.HappyCellIndex);

            List<DropInfo> ids = MapHelper.GetCanShiQuByCell(self.ZoneScene(), cellindex);
            if (ids.Count > 0)
            {
                UI uimain = UIHelper.GetUI(self.ZoneScene(), UIType.UIMain);
                uimain.GetComponent<UIMainComponent>().UIMainSkillComponent.RequestShiQu(ids).Coroutine();

                //播放音效
                UIHelper.PlayUIMusic("10004");
                return;
            }
        }

        public static async ETTask OnButtonMove(this UIDungeonHappyMainComponent self, int moveType)
        {
            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();

            if (moveType == 1)
            {
                //非免费时间则返回
                long moveTime = self.NextMoveTime - TimeHelper.ServerNow();
                if (moveTime > 0)
                {
                    return;
                }
            }
            if (moveType == 2)
            {
                GlobalValueConfig globalValueConfig = GlobalValueConfigCategory.Instance.Get(94);
                if (userInfoComponent.UserInfo.Gold < globalValueConfig.Value2)
                {
                    FloatTipManager.Instance.ShowFloatTip(ErrorHelp.Instance.GetHint(ErrorCode.ERR_GoldNotEnoughError));
                    return;
                }
                PopupTipHelp.OpenPopupTip(self.ZoneScene(), "神秘之门", $"是否消耗{globalValueConfig.Value2}金币?", async () =>
                {
                    long instanceId = self.InstanceId;
                    C2M_DungeonHappyMoveRequest request = new C2M_DungeonHappyMoveRequest() { OperatateType = moveType };
                    M2C_DungeonHappyMoveResponse response = (M2C_DungeonHappyMoveResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
                    if (instanceId != self.InstanceId || response.Error != ErrorCode.ERR_Success)
                    {
                        return;
                    }

                    Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
                    long lastmovetime = unit.GetComponent<NumericComponent>().GetAsLong(NumericType.HappyMoveTime);
                   
                    FunctionEffect.GetInstance().PlayDropEffect(unit, 30000003);
                    self.OnButtonPick();

                    self.OnUpdateMoney();
                }).Coroutine();
                return;
            }
            if (moveType == 3)
            {
                GlobalValueConfig globalValueConfig = GlobalValueConfigCategory.Instance.Get(95);
                if (userInfoComponent.UserInfo.Diamond < globalValueConfig.Value2)
                {
                    FloatTipManager.Instance.ShowFloatTip(ErrorHelp.Instance.GetHint(ErrorCode.ERR_DiamondNotEnoughError));
                    return;
                }
                PopupTipHelp.OpenPopupTip(self.ZoneScene(), "神秘之门", $"是否消耗{globalValueConfig.Value2}钻石?", async () =>
                {
                    long instanceId = self.InstanceId;
                    C2M_DungeonHappyMoveRequest request = new C2M_DungeonHappyMoveRequest() { OperatateType = moveType };
                    M2C_DungeonHappyMoveResponse response = (M2C_DungeonHappyMoveResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
                    if (instanceId != self.InstanceId || response.Error != ErrorCode.ERR_Success)
                    {
                        return;
                    }

                    Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
                    long lastmovetime = unit.GetComponent<NumericComponent>().GetAsLong(NumericType.HappyMoveTime);

                    FunctionEffect.GetInstance().PlayDropEffect(unit, 30000003);
                    self.OnButtonPick();

                    self.OnUpdateMoney();
                }).Coroutine();
                return;
            }

            long instanceId = self.InstanceId;
            C2M_DungeonHappyMoveRequest request = new C2M_DungeonHappyMoveRequest() { OperatateType = moveType };
            M2C_DungeonHappyMoveResponse response = (M2C_DungeonHappyMoveResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            if (instanceId != self.InstanceId || response.Error != ErrorCode.ERR_Success)
            {
                return;
            }

            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            FunctionEffect.GetInstance().PlayDropEffect(unit, 30000003);
            self.OnButtonPick();
            self.OnUpdateMoney();

            int moveNumber  = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.HappyMoveNumber);
            if (moveNumber >= 5)
            {
                await TimerComponent.Instance.WaitAsync(1000);
                if (instanceId != self.InstanceId)
                {
                    return;
                }
                UI uImain = UIHelper.GetUI( self.ZoneScene(), UIType.UIMain );
                uImain.GetComponent<UIMainComponent>().OnBtn_RerurnDungeon();
            }
        }
    }
}