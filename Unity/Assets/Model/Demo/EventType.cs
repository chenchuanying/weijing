using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{

    namespace EventType
    {

        public struct AppStart
        {
        }

        public class SceneChangeStart : DisposeObject
        {
            public static readonly SceneChangeStart Instance = new SceneChangeStart();
            public Scene ZoneScene;
            public int LastSceneType;
            public int LastChapterId;
            public int SceneType;
            public int ChapterId;
        }

        public class SceneChangeFinish : DisposeObject
        {
            public static readonly SceneChangeFinish Instance = new SceneChangeFinish();
            public Scene ZoneScene;
            public Scene CurrentScene;

            public override void Dispose()
            {
                this.ZoneScene = null;
                this.CurrentScene = null;
            }
        }

        public class ChangePosition : DisposeObject
        {
            public static readonly ChangePosition Instance = new ChangePosition();
            public Unit Unit;
            public WrapVector3 OldPos = new WrapVector3();

            // 因为是重复利用的，所以用完PublishClass会调用Dispose
            public override void Dispose()
            {
            }
        }

        public class ChangeRotation : DisposeObject
        {
            public static readonly ChangeRotation Instance = new ChangeRotation();
            public Unit Unit;

            // 因为是重复利用的，所以用完PublishClass会调用Dispose
            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        public class PingChange : DisposeObject
        {
            public static readonly PingChange Instance = new PingChange();
            public Scene ZoneScene;
            public long Ping;
        }

        public class AfterCreateZoneScene : DisposeObject
        {
            public static readonly AfterCreateZoneScene Instance = new AfterCreateZoneScene();
            public Scene ZoneScene;
        }

        public class AfterCreateCurrentScene : DisposeObject
        {
            public static readonly AfterCreateCurrentScene Instance = new AfterCreateCurrentScene();
            public Scene CurrentScene;
        }

        public class AppStartInitFinish : DisposeObject
        {
            public static readonly AppStartInitFinish Instance = new AppStartInitFinish();
            public Scene ZoneScene;
        }

        public class LoginFinish : DisposeObject
        {
            public static readonly LoginFinish Instance = new LoginFinish();
            public Scene ZoneScene;
        }

        public class LoadingFinish : DisposeObject
        {
            public static readonly LoadingFinish Instance = new LoadingFinish();
            public Scene Scene;
        }

        public class EnterMapFinish : DisposeObject
        {
            public static readonly EnterMapFinish Instance = new EnterMapFinish();
            public Scene ZoneScene;
        }

        public class BeginRelink : DisposeObject
        {
            public static readonly BeginRelink Instance = new BeginRelink();
            public Scene ZoneScene;
        }

        public class RelinkSucess : DisposeObject
        {
            public static readonly RelinkSucess Instance = new RelinkSucess();
            public Scene ZoneScene;
            public int ErrorCode;
        }

        public class ReturnLogin : DisposeObject
        {
            public static readonly ReturnLogin Instance = new ReturnLogin();
            public Scene ZoneScene;
            public int ErrorCode;
        }

        public class LoginError : DisposeObject
        {
            public static readonly LoginError Instance = new LoginError();
            public int ErrorCore;
            public long AccountId;
            public Scene ZoneScene;
            public string Value;
        }

        public class EnterQueue : DisposeObject
        {
            public static readonly EnterQueue Instance = new EnterQueue();
            public int ErrorCore;
            public long AccountId;
            public Scene ZoneScene;
            public string Value;
        }

        public class QueueEnterGame : DisposeObject
        {
            public static readonly QueueEnterGame Instance = new QueueEnterGame();
            public Scene ZoneScene;
            public string Token;
        }

        public class FashionUpdate : DisposeObject
        {
            public static readonly FashionUpdate Instance = new FashionUpdate();
            public Unit Unit;
        }

        public class RoleDataBroadcase : DisposeObject
        {
            public static readonly RoleDataBroadcase Instance = new RoleDataBroadcase();
            public int UserDataType;
            public string UserDataValue;
            public Unit Unit;

            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        public class BeforeEnterSonFuben : DisposeObject
        {
            public static readonly BeforeEnterSonFuben Instance = new BeforeEnterSonFuben();
            public Scene ZoneScene;
        }

        //副本
        public class AfterEnterFuben : DisposeObject
        {
            public static readonly AfterEnterFuben Instance = new AfterEnterFuben();
            public Scene ZoneScene;
            public bool EnterSonScene;
            public int DirectionType;
        }

        public class UnitDead : DisposeObject
        {
            public static readonly UnitDead Instance = new UnitDead();
            public Unit Unit;

            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        public class UnitRemove : DisposeObject
        {
            public static readonly UnitRemove Instance = new UnitRemove();
            public Scene ZoneScene; 
            public List<long> RemoveIds;
        }

        public class UnitRevive : DisposeObject
        {
            public static readonly UnitRevive Instance = new UnitRevive();
            public Unit Unit;

            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        /// <summary>
        /// 数据更新
        /// </summary>
        public class DataUpdate : DisposeObject
        {
            public static readonly DataUpdate Instance = new DataUpdate();
            public string DataParamString;
            public long UpdateValue;
            public int DataType;
        }

        public class BattleInfo : DisposeObject
        {
            public static readonly BattleInfo Instance = new BattleInfo();
            public M2C_BattleInfoResult m2C_Battle;
            public Scene ZoneScene;
        }

        public class UnionRaceInfo : DisposeObject
        {
            public static readonly UnionRaceInfo Instance = new UnionRaceInfo();
            public M2C_UnionRaceInfoResult m2C_Battle;
            public Scene ZoneScene;
        }

        public class AreneInfo : DisposeObject
        {
            public static readonly AreneInfo Instance = new AreneInfo();
            public M2C_AreneInfoResult m2C_Battle;
            public Scene ZoneScene;
        }

        public class TurtleReward : DisposeObject
        {
            public static readonly TurtleReward Instance = new TurtleReward();
            public M2C_TurtleRewardMessage m2C_TurtleReward;
            public Scene ZoneScene;
        }

        public class HappyInfo : DisposeObject
        {
            public static readonly HappyInfo Instance = new HappyInfo();
            public M2C_HappyInfoResult m2C_Battle;
            public Scene ZoneScene;
        }

        public class RunRaceInfo: DisposeObject
        {
            public static readonly RunRaceInfo Instance = new RunRaceInfo();
            public M2C_RankRunRaceMessage M2CRankRunRaceMessage;
            public Scene ZoneScene;
        }

        public class RankDemonInfo: DisposeObject
        {
            public static readonly RankDemonInfo Instance = new RankDemonInfo();
            public M2C_RankDemonMessage M2CRankDemonMessage;
            public Scene ZoneScene;
        }

        public class RunRaceRewardInfo: DisposeObject
        {
            public static readonly RunRaceRewardInfo Instance = new RunRaceRewardInfo();
            public M2C_RankRunRaceReward M2CRankRunRaceReward;
            public Scene ZonScene;
        }

        public class RunRaceBattleInfo : DisposeObject
        {
            public static readonly RunRaceBattleInfo Instance = new RunRaceBattleInfo();
            public M2C_RunRaceBattleInfo M2C_RunRaceBattleInfo;
            public Scene ZonScene;
        }

        public class ChengJiuActive : DisposeObject
        {
            public static readonly ChengJiuActive Instance = new ChengJiuActive();
            public M2C_ChengJiuActiveMessage m2C_ChengJiu;
            public Scene ZoneScene;
        }

        public class RolePetAdd : DisposeObject
        {
            public static readonly RolePetAdd Instance = new RolePetAdd();
            public List<KeyValuePair> OldPetSkin;
            public RolePetInfo RolePetInfo;
            public Scene ZoneScene;
        }

        public class RolePetUpdate : DisposeObject
        {
            public static readonly RolePetUpdate Instance = new RolePetUpdate();
            public Scene ZoneScene;
            public long PetId;
            public int UpdateType;
            public string UpdateValue;
        }

        public class TeamDungeonQuit : DisposeObject
        {
            public static readonly TeamDungeonQuit Instance = new TeamDungeonQuit();
            public M2C_TeamDungeonQuitMessage m2C_Battle;
            public Scene ZoneScene;
        }

        public class UISoloQuit : DisposeObject
        {
            public static readonly UISoloQuit Instance = new UISoloQuit();
            public Scene ZoneScene;
        }

        public class UISoloReward : DisposeObject
        {
            public static readonly UISoloReward Instance = new UISoloReward();
            public M2C_SoloDungeon m2C_SoloDungeon;
            public Scene ZoneScene;
        }

        public class UISoloEnter : DisposeObject
        {
            public static readonly UISoloEnter Instance = new UISoloEnter();
            public M2C_SoloMatchResult m2C_SoloMatch;
            public Scene ZoneScene;
        }

        /// <summary>
        /// 通用提示
        /// </summary>
        public class CommonHint : DisposeObject
        {
            public static readonly CommonHint Instance = new CommonHint();
            public string HintText;
        }

        public class CommonPopup : DisposeObject
        {
            public static readonly CommonPopup Instance = new CommonPopup();
            public Scene ZoneScene;
            public string HintText;
        }

        /// <summary>
        /// 错误码通用提示
        /// </summary>
        public class CommonHintError : DisposeObject
        {
            public static readonly CommonHintError Instance = new CommonHintError();
            public Scene ZoneScene;
            public int errorValue;
        }

        /// <summary>
        /// 好友申请
        /// </summary>
        public class ReddotChange : DisposeObject
        {
            public static readonly ReddotChange Instance = new ReddotChange();
            public Scene ZoneScene;
            public int ReddotType;
            public int Number;
        }

        /// <summary>
        /// 收到组队邀请
        /// </summary>
        public class RecvTeamInvite : DisposeObject
        {
            public static readonly RecvTeamInvite Instance = new RecvTeamInvite();
            public M2C_TeamInviteResult m2C_TeamInviteResult;
            public Scene ZoneScene;
        }

        /// <summary>
        /// 收到进入组队的申请
        /// </summary>
        public class RecvTeamApply : DisposeObject
        {
            public static readonly RecvTeamApply Instance = new RecvTeamApply();
            public M2C_TeamDungeonApplyResult m2C_TeamDungeonApplyResult;
            public Scene ZoneScene;
        }

        public class RecvTeamDungeonOpen : DisposeObject
        {
            public static readonly RecvTeamDungeonOpen Instance = new RecvTeamDungeonOpen();
            public TeamInfo TeamInfo;
            public Scene ZoneScene;
        }

        public class RecvTeamDungeonPrepare : DisposeObject
        {
            public static readonly RecvTeamDungeonPrepare Instance = new RecvTeamDungeonPrepare();
            public M2C_TeamDungeonPrepareResult PrepareResult;
            public Scene ZoneScene;
        }

        public class RecvTeamUpdate : DisposeObject
        {
            public static readonly RecvTeamUpdate Instance = new RecvTeamUpdate();
            public Scene ZoneScene;
        }

        public class ChuanSongOpen : DisposeObject
        {
            public static readonly ChuanSongOpen Instance = new ChuanSongOpen();
            public Scene ZoneScene;
        }

        //点击物品
        public class ShowItemTips : DisposeObject
        {
            public static readonly ShowItemTips Instance = new ShowItemTips();
            public Scene ZoneScene;
            public BagInfo bagInfo;
            public ItemOperateEnum itemOperateEnum;
            public Vector3 inputPoint;
            public List<BagInfo> EquipList = new List<BagInfo>();
            public int Occ;
        }

        //副本结算
        public class FubenSettlement : DisposeObject
        {
            public static readonly FubenSettlement Instance = new FubenSettlement();
            public M2C_FubenSettlement m2C_FubenSettlement;
            public Scene Scene;
        }

        //组队副本结算
        public class TeamDungeonSettlement : DisposeObject
        {
            public static readonly TeamDungeonSettlement Instance = new TeamDungeonSettlement();
            public M2C_TeamDungeonSettlement m2C_FubenSettlement;
            public Scene ZoneScene;
        }

        //组队副本宝箱奖励
        public class TeamDungeonBoxReward : DisposeObject
        {
            public static readonly TeamDungeonBoxReward Instance = new TeamDungeonBoxReward();
            public M2C_TeamDungeonBoxRewardResult m2C_FubenSettlement;
            public Scene Scene;
        }

        public class AfterUnitCreate : DisposeObject
        {
            public static readonly AfterUnitCreate Instance = new AfterUnitCreate();

            public Unit Unit;
            public override void Dispose()
            {
                //this.Unit = null;
            }
        }

        public class ShowGuide : DisposeObject
        {
            public static readonly ShowGuide Instance = new ShowGuide();
            public Scene ZoneScene;
            public int GuideId;
            public int GroupId;
        }


        public class BeforeSkill : DisposeObject
        {
            public static readonly BeforeSkill Instance = new BeforeSkill();
            public Scene ZoneScene;
        }

        public class MoveStart : DisposeObject
        {
            public static readonly MoveStart Instance = new MoveStart();
            public Unit Unit;
            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        //更新血量
        public class UnitHpUpdate : DisposeObject
        {
            public static readonly UnitHpUpdate Instance = new UnitHpUpdate();
            public Unit Attack;
            public Unit Defend;
            public int SkillID;
            public int DamgeType;
            public long ChangeHpValue;

            public override void Dispose()
            {
                this.Defend = null;
            }
        }

        /// <summary>
        /// Unit(NumberType)数据更新
        /// </summary>
        public class UnitNumericUpdate : DisposeObject
        {
            public static readonly UnitNumericUpdate Instance = new UnitNumericUpdate();
            public Unit Unit;
            public long OldValue;
            public int NumericType;
            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        public class SkillInterrup : DisposeObject
        {
            public static readonly SkillInterrup Instance = new SkillInterrup();
            public Scene ZoneScene;
            public Unit Unit;
        }

        public class M2C_SkillSecond : DisposeObject
        {
            public static readonly M2C_SkillSecond Instance = new M2C_SkillSecond();
            public M2C_SkillSecondResult M2C_SkillSecondResult;
            public Scene ZoneScene;
            public Unit Unit;
        }

        //技能预警
        public class SkillYuJing : DisposeObject
        {
            public static readonly SkillYuJing Instance = new SkillYuJing();

            public SkillConfig SkillConfig;
            public SkillInfo SkillInfo;
            public Unit Unit;
            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        //技能音效
        public class SkillSound : DisposeObject
        {
            public static readonly SkillSound Instance = new SkillSound();
            public string Asset;
        }

        public class MoveStop : DisposeObject
        {
            public static readonly MoveStop Instance = new MoveStop();
            public Unit Unit;
            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        //技能特效
        public class SkillEffect : DisposeObject
        {
            public static readonly SkillEffect Instance = new SkillEffect();
            public EffectData EffectData;
            public Unit Unit;
            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        public class SkillEffectMove : DisposeObject
        {
            public static readonly SkillEffectMove Instance = new SkillEffectMove();
            public long EffectInstanceId = 0;
            public Vector3 Postion;
            public float Angle;
            public Unit Unit;
            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        public class SkillEffectFinish : DisposeObject
        {
            public static readonly SkillEffectFinish Instance = new SkillEffectFinish();
            public long EffectInstanceId = 0;
            public Unit Unit;

            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        public class TeamPickNotice : DisposeObject
        {
            public static readonly TeamPickNotice Instance = new TeamPickNotice();
            public M2C_TeamPickMessage m2C_TeamPickMessage;
            public Scene ZoneScene;
        }

        public class SkillChainLight : DisposeObject
        {
            public static readonly SkillChainLight Instance = new SkillChainLight();

            public M2C_ChainLightning M2C_ChainLightning;

            public Scene ZoneScene;
        }

        public class SyncMiJingDamage : DisposeObject
        {
            public static readonly SyncMiJingDamage Instance = new SyncMiJingDamage();

            public M2C_SyncMiJingDamage M2C_SyncMiJingDamage;

            public Scene ZoneScene;
        }

        public class SkillEffectReset : DisposeObject
        {
            public static readonly SkillEffectReset Instance = new SkillEffectReset();
            public long EffectInstanceId = 0;
            public Unit Unit;

            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        //npc對話
        public class TaskNpcDialog : DisposeObject
        {
            public static readonly TaskNpcDialog Instance = new TaskNpcDialog();
            //public TaskPro TaskPro;
            public Scene zoneScene;
            public int ErrorCode;
            public int NpcId;
        }

        public class StateChange : DisposeObject
        {
            public static readonly StateChange Instance = new StateChange();

            public M2C_UnitStateUpdate m2C_UnitStateUpdate;
            public Unit Unit;
        }

        //动画
        public class PlayAnimator : DisposeObject
        {
            public static readonly PlayAnimator Instance = new PlayAnimator();

            public string Animator;
            public Unit Unit;

            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        //状态机
        public class FsmChange : DisposeObject
        {
            public static readonly FsmChange Instance = new FsmChange();

            public int FsmHandlerType;
            public int SkillId;
            public Unit Unit;

            public override void Dispose()
            {
                this.Unit = null;
            }
        }

        //吟唱
        public class SingingUpdate : DisposeObject
        {
            public static readonly SingingUpdate Instance = new SingingUpdate();

            public Scene ZoneScene;
            public long PassTime;
            public long TotalTime;
            public int Type;
        }

        //挖宝
        public class DigForTreasure : DisposeObject
        {
            public static readonly DigForTreasure Instance = new DigForTreasure();
            public Scene ZoneScene;
            public BagInfo BagInfo;
        }

        public class BuffUpdate : DisposeObject
        {
            public static readonly BuffUpdate Instance = new BuffUpdate();
            public ABuffHandler ABuffHandler;
            public int OperateType;

            public Scene ZoneScene;
            public Unit Unit;
        }
        
        public class AddBuff : DisposeObject
        {
            public static readonly AddBuff Instance = new AddBuff();

            public Scene ZoneScene;
            public Unit Unit;
            public int BuffId;
        }

        public class BuffScale : DisposeObject
        {
            public static readonly BuffScale Instance = new BuffScale();
            public ABuffHandler ABuffHandler;
            public int OperateType;    //0开始 1结束

            public Scene ZoneScene;
            public Unit Unit;
        }

        public class BuffBounce : DisposeObject
        {
            public static readonly BuffBounce Instance = new BuffBounce();
            public int OperateType;    //0开始 1结束

            public Scene ZoneScene;
            public Unit Unit;
        }

        public class JingLingGet : DisposeObject
        {
            public static readonly JingLingGet Instance = new JingLingGet();
            public Scene ZoneScene;
            public int JingLingId;
        }

        public class JiaYuanInit : DisposeObject
        {
            public static readonly JiaYuanInit Instance = new JiaYuanInit();
            public Scene ZoneScene;
        }

        public class UnionInvite : DisposeObject
        {
            public static readonly UnionInvite Instance = new UnionInvite();
            public M2C_UnionInviteMessage M2C_UnionInviteMessage;
            public Scene ZoneScene;
        }

        public class SMSSVerify : DisposeObject
        {
            public static readonly SMSSVerify Instance = new SMSSVerify();
            public Action<string> Action;
            public string Phone;
            public string Code;
        }

        public struct SkillEventType
        {
            //public ESkillEventType skillEventType;
            public Unit owner;
            public Unit target;
            //技能数据来源放在此处，如果有技能编辑器，对接编辑器数据；如果是表格配置技能数据则来源表格。
        }

        public class ChangeCameraMoveType: DisposeObject
        {
            public static readonly ChangeCameraMoveType Instance = new ChangeCameraMoveType();
            public int CameraType;
            public Scene ZoneScene;
        }

        public class LoginCheckRoot : DisposeObject
        {
            public static readonly LoginCheckRoot Instance = new LoginCheckRoot();
            public Scene ZoneScene;
        }

        public class ShareSDKInit : DisposeObject
        {
            public static readonly ShareSDKInit Instance = new ShareSDKInit();
            public Scene ZoneScene;
        }

        public class TikTokGetAccesstoken : DisposeObject
        {
            public static readonly TikTokGetAccesstoken Instance = new TikTokGetAccesstoken();
            public Action<string> AccesstokenHandler;
            public Scene ZoneScene;
        }

        public class TikTokRiskControlInfo : DisposeObject
        {
            public static readonly TikTokRiskControlInfo Instance = new TikTokRiskControlInfo();
            public Action<string> RiskControlInfoHandler;
            public Scene ZoneScene;
        }

        public class TikTokPayRequest : DisposeObject
        {
            public static readonly TikTokPayRequest Instance = new TikTokPayRequest();
            public string PayMessage;
            public int RechargeNumber;
            public Scene ZoneScene;
        }

        public class TikTokShare : DisposeObject
        {
            public static readonly TikTokShare Instance = new TikTokShare();
            public int ShareMode;
            public List<string> ShareMessage;
            public Action<int, bool> ShareHandler;
            public Scene ZoneScene;
        }

        public class TapTapAuther : DisposeObject
        {
            public static readonly TapTapAuther Instance = new TapTapAuther();
            public Scene ZoneScene;
            public string Account;
        }

        public class TapTapGetOAID : DisposeObject
        {
            public static readonly TapTapGetOAID Instance = new TapTapGetOAID();
            public Scene ZoneScene;
        }

        public class TapTapShare : DisposeObject
        {
            public static readonly TapTapShare Instance = new TapTapShare();
            public Scene ZoneScene;
            public string Content;
        }

        public class AppleSignIn : DisposeObject
        {
            public static readonly AppleSignIn Instance = new AppleSignIn();
            public Action<string> AppleSignInHandler;
            public Scene ZoneScene;
            public string Account;
        }

        public class UIOneChallenge : DisposeObject
        {
            public static readonly UIOneChallenge Instance = new UIOneChallenge();
            public M2C_OneChallenge m2C_OneChallenge;
            public Scene ZoneScene;
        }
    }
}