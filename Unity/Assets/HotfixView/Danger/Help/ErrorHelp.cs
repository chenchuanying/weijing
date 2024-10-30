using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class ErrorHelp:Singleton<ErrorHelp>
    {

        public Dictionary<int,string> ErrorHintList = new Dictionary<int, string>();

        //单例的初始化
        protected override void InternalInit()
        {
            base.InternalInit();
            
            ErrorHintList.Add(ErrorCode.ERR_NetWorkError, "网络错误!");
            ErrorHintList.Add(ErrorCode.ERR_AccountAlreadyRegister, "账号已注册!");
            ErrorHintList.Add(ErrorCode.ERR_AccountInBlackListError, "账号异常!");
            ErrorHintList.Add(ErrorCode.ERR_LoginInfoIsNull, "未找到账号数据,请确认账号是否已经注册");
            ErrorHintList.Add(ErrorCode.ERR_AccountOrPasswordError, "密码错误,请检查重新输入");
            ErrorHintList.Add(ErrorCode.ERR_OtherAccountLogin, "账号异地登录");
            ErrorHintList.Add(ErrorCode.ERR_RequestRepeatedly, "请求重复");
            ErrorHintList.Add(ErrorCode.ERR_EnterQueue, "服务器已满,进入排队系统");
            ErrorHintList.Add(ErrorCode.ERR_RequestExitFuben, "请先退出副本");
     
            ErrorHintList.Add(ErrorCode.ERR_StopServer, "停服维护");
            ErrorHintList.Add(ErrorCode.ERR_BingPhoneError_1, "手机号已经注册过账号");
            ErrorHintList.Add(ErrorCode.ERR_BingPhoneError_2, "手机号只能绑定一个账号");
            ErrorHintList.Add(ErrorCode.ERR_VersionNoMatch, "版本不一致，请重开客户端");
            ErrorHintList.Add(ErrorCode.ERR_EnterGameError, "角色登录异常,请尝试再次重新登录账号!");
            ErrorHintList.Add(ErrorCode.ERR_OtherNotExist, "该玩家不在主城!");
            ErrorHintList.Add(ErrorCode.ERR_NonePlayerError, "玩家不存在!");
            
            //ErrorHintList.Add(ErrorCore.ERR_AccountOrPasswordError, "账号未注册!");
            ErrorHintList.Add(ErrorCode.ERR_GoldNotEnoughError, "金币不足!");
            ErrorHintList.Add(ErrorCode.ERR_DiamondNotEnoughError, "钻石不足!");
            ErrorHintList.Add(ErrorCode.ERR_HouBiNotEnough, "货币不足!");
            ErrorHintList.Add(ErrorCode.ERR_ItemBing, "此道具为绑定道具!");
            ErrorHintList.Add(ErrorCode.ERR_EquipChuanChengFail, "装备传承失败!");
            ErrorHintList.Add(ErrorCode.ERR_AlreadyLearn, "已经学习了该技能!");
            ErrorHintList.Add(ErrorCode.ERR_UseSkillInCD1, "技能冷却中...");
            ErrorHintList.Add(ErrorCode.ERR_UseSkillInCD2, "技能公共冷却中!");
            ErrorHintList.Add(ErrorCode.ERR_UseSkillInCD3, "主动技能冷却中!");
            ErrorHintList.Add(ErrorCode.ERR_UseSkillInCD4, "被动技能冷却中!");
            ErrorHintList.Add(ErrorCode.ERR_UseSkillInCD5, "技能冷却中!");
            ErrorHintList.Add(ErrorCode.ERR_UseSkillInCD6, "公共技能冷却中!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotUseSkill_1, "当前状态无法释放技能!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotUseSkill_Rigidity, "僵直状态无法释放技能!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotUseSkill, "当前状态无法释放技能!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotUseSkill_NetWait, "消息未返回无法释放技能!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotUseSkill_Dizziness, "眩晕状态无法释放技能!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotUseSkill_JiTui, "击退状态无法释放技能!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotUseSkill_Silence, "沉默状态无法释放技能!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotUseSkill_Sleep, "沉睡状态无法释放技能!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotUseSkill_Hung, "悬浮状态无法释放技能!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotUseSkill_MP, "魔法值不足!");
            ErrorHintList.Add(ErrorCode.ERR_SkillMoveTime, "当前为技能释放状态!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotSkillDead, "死亡状态无法释放技能!");
            ErrorHintList.Add(ErrorCode.ERR_UseSkillError, "没有目标!");
            ErrorHintList.Add(ErrorCode.ERR_NoUseItemSkill, "该场景不能使用药剂道具技能!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotMove_1, "当前状态无法移动!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotMove_Dead, "死亡状态无法移动!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotMove_Rigidity, "僵直状态无法移动!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotMove_NetWait, "消息未返回无法移动!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotMove_Dizziness, "眩晕状态无法移动!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotMove_JiTui, "击退状态无法移动!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotMove_Shackle, "禁锢状态无法移动!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotMove_Sleep, "沉睡状态无法移动!");
            ErrorHintList.Add(ErrorCode.ERR_CanNotMove_Fear,"恐惧状态无法移动!");
            ErrorHintList.Add(ErrorCode.ERR_UnionXiuLianMax, "已达修炼上限!");
            ErrorHintList.Add(ErrorCode.ERR_Union_Not_Exist, "没找到家族!");

            ErrorHintList.Add(ErrorCode.ERR_UnSafeSqlString, "非法字符串!");
            ErrorHintList.Add(ErrorCode.ERR_EquipLvLimit, "角色等级不足!");
            ErrorHintList.Add(ErrorCode.ERR_BagIsFull, "背包已满!");
            ErrorHintList.Add(ErrorCode.ERR_ItemNotEnoughError, "道具不足!");
            ErrorHintList.Add(ErrorCode.Error_AngleNotEnough, "怒气不足!");
            ErrorHintList.Add(ErrorCode.Error_PickWaitSelect, "拾取等待中!");
            ErrorHintList.Add(ErrorCode.ERR_ItemDropProtect, "掉落保护中!");
            ErrorHintList.Add(ErrorCode.ERR_ItemBelongOther, "该道具归属其他玩家!");
            ErrorHintList.Add(ErrorCode.ERR_NoPayValueError, "赞助额度不足!");
            ErrorHintList.Add(ErrorCode.ERR_ItemNoUseTime, "道具今天不能再使用!");
            ErrorHintList.Add(ErrorCode.ERR_EquipAppraisal_Item,"鉴定失败!所需鉴定道具不足...");
            ErrorHintList.Add(ErrorCode.ERR_Occ_Hint_1, "转职失败!请先将角色等级提升至18级");
            ErrorHintList.Add(ErrorCode.ERR_Occ_Hint_2, "请不要重复进行转职噢");
            ErrorHintList.Add(ErrorCode.ERR_NotRealName, "请先实名认证");
            ErrorHintList.Add(ErrorCode.ERR_RealNameFail, "实名认证失败!");
            ErrorHintList.Add(ErrorCode.ERR_TeamDungeonTimesMax, "副本参与次数已经最大");
            ErrorHintList.Add(ErrorCode.ERR_WordChat, "世界频道发消息 1分钟1次");
            ErrorHintList.Add(ErrorCode.ERR_InMakeCD, "制作冷却中!");
            ErrorHintList.Add(ErrorCode.ERR_MakeTypeError, "制作类型不符!");
            ErrorHintList.Add(ErrorCode.ERR_ZhuaBuFail, "抓捕失败!");
            ErrorHintList.Add(ErrorCode.ERR_UnionChatLimit, "发消息太频繁 5秒1次");
            ErrorHintList.Add(ErrorCode.ERR_EquipRepeat, "已经有相同类型的装备");
            ErrorHintList.Add(ErrorCode.ERR_WarehouseIsFull, "仓库已满");
            ErrorHintList.Add(ErrorCode.ERR_WordChatLimit, "当前已经被禁言！");

            ErrorHintList.Add(ErrorCode.ERR_CreateRoleName, "角色名非法!");
            ErrorHintList.Add(ErrorCode.ERR_RoleNameRepeat, "角色名重复!");
            ErrorHintList.Add(ErrorCode.ERR_ShuLianDuNotEnough, "当前制造需求熟练度不足!");
            ErrorHintList.Add(ErrorCode.ERR_EquipType, "装备穿戴类型不符!");
            ErrorHintList.Add(ErrorCode.ERR_GemNoError, "宝石一旦镶嵌无法卸载!");
            ErrorHintList.Add(ErrorCode.ERR_GemShiShiNumFull, "史诗宝石最多可镶嵌4个!");

            ErrorHintList.Add(ErrorCode.ERR_TeamIsFull, "队伍已满");
            ErrorHintList.Add(ErrorCode.ERR_LevelIsNot, "等级不足");
            ErrorHintList.Add(ErrorCode.ERR_IsHaveTeam, "已经有组队了");
            ErrorHintList.Add(ErrorCode.ERR_TimesIsNot, "次数不足");
            ErrorHintList.Add(ErrorCode.ERR_IsNotLeader, "队长才能创建副本");
            ErrorHintList.Add(ErrorCode.ERR_PlayerIsNot, "人数不足");
            ErrorHintList.Add(ErrorCode.ERR_TeamerLevelIsNot, "队员等级不足");
            ErrorHintList.Add(ErrorCode.Err_OnLineTimeNot, "在线时间不足30分钟领取奖励");
            ErrorHintList.Add(ErrorCode.Err_OnLineTimeNotFenXiang, "在线时间不足30分钟无法分享");
            ErrorHintList.Add(ErrorCode.ERR_LevelIsNotFenXiang, "领取分享奖励需要等级达到10级且在线时间达到30分钟");
            ErrorHintList.Add(ErrorCode.ERR_HuoYueNot, "活跃不足30点不能分享");
            

            ErrorHintList.Add(ErrorCode.ERR_FangChengMi_Tip1, "您目前为未成年人账号，已被纳入防沉迷系统。根据国家新闻出版署《关于进一步严格管理切实防止未成年人沉迷网络游戏的通知》，仅每周五、周六、周日和法定节假日每日20时至21时提供1小时网络游戏服务。您今日游戏剩余时间{0}分钟。");
            ErrorHintList.Add(ErrorCode.ERR_FangChengMi_Tip2, "您目前为未成年人账号，已被纳入防沉迷系统。根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》，您已超出支付上限，无法继续充值。");
            ErrorHintList.Add(ErrorCode.ERR_FangChengMi_Tip3, "您目前为未成年人账号，已被纳入防沉迷系统。根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》，本游戏不为未满8周岁的用户提供游戏充值服务。");
            ErrorHintList.Add(ErrorCode.ERR_FangChengMi_Tip4, "您目前为未成年人账号，已被纳入防沉迷系统。根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》，游戏中8周岁以上未满16周岁的用户，单次充值金额不得超过50元人民币，每月充值金额不得超过200元人民币。");
            ErrorHintList.Add(ErrorCode.ERR_FangChengMi_Tip5, "您目前为未成年人账号，已被纳入防沉迷系统。根据国家新闻出版署《关于防止未成年人沉迷网络游戏的通知》，游戏中16周岁以上未满18周岁的用户，单次充值金额不得超过100元人民币，每月充值金额不得超过400元人民币。");
            ErrorHintList.Add(ErrorCode.ERR_FangChengMi_Tip6, "您目前为未成年人账号，已被纳入防沉迷系统。根据适龄提示，此时段本游戏将无法为不满12周岁未成年人用户提供任何形式的游戏服务。");
            ErrorHintList.Add(ErrorCode.ERR_FangChengMi_Tip7, "您目前为未成年人账号，已被纳入防沉迷系统。根据国家新闻出版署《关于进一步严格管理切实防止未成年人沉迷网络游戏的通知》，仅每周五、周六、周日和法定节假日每日20时至21时提供1小时网络游戏服务。");

            ErrorHintList.Add(ErrorCode.ERR_TaskCommited, "当前任务奖励已领取");
            ErrorHintList.Add(ErrorCode.ERR_NotFindLevel, "未找到对应关卡");
            ErrorHintList.Add(ErrorCode.ERR_TiLiNoEnough, "体力不足");
            ErrorHintList.Add(ErrorCode.ERR_LevelNotOpen, "请先通关前置关卡");
            ErrorHintList.Add(ErrorCode.ERR_ExpNoEnough, "经验不足");
            ErrorHintList.Add(ErrorCode.ERR_LevelNoEnough, "等级不足");
            ErrorHintList.Add(ErrorCode.ERR_LevelNormalNoPass, "普通关卡未通关");
            ErrorHintList.Add(ErrorCode.ERR_LevelChallengeNoPass, "挑战关卡未通关");
            ErrorHintList.Add(ErrorCode.ERR_NotFindNpc, "任务点不在此地图，请根据指示前往其他地图。");
            ErrorHintList.Add(ErrorCode.ERR_TaskCanNotGet, "未达到领取条件");
            ErrorHintList.Add(ErrorCode.ERR_TaskNoComplete, "前先完成此类任务");
            ErrorHintList.Add(ErrorCode.ERR_TowerOfSealReachTop, "已通关");
            ErrorHintList.Add(ErrorCode.ERR_Challenge_CD, "挑战CD中");
            ErrorHintList.Add(ErrorCode.ERR_CreateRole_Limit, "1个帐号1天最多能建立8个角色 ");
            ErrorHintList.Add(ErrorCode.ERR_CreateRole_Limit_2, "注册人数已满 ");

            ErrorHintList.Add(ErrorCode.ERR_Pet_Hint_1, "宠物星级出错");
            ErrorHintList.Add(ErrorCode.ERR_Pet_UpStar, "宠物星级失败");
            ErrorHintList.Add(ErrorCode.ERR_Pet_UpStage, "宠物进化失败");
            ErrorHintList.Add(ErrorCode.ERR_Pet_AddSkillSame, "具有相同技能,无法使用此道具");
            ErrorHintList.Add(ErrorCode.ERR_Pet_NoUseItem, "此道具无法应用于宠物身上");
            ErrorHintList.Add(ErrorCode.ERR_PetIsFull, "当前携带的宠物数量已达上限");
            ErrorHintList.Add(ErrorCode.ERR_Pet_Hint_2, "出战宠物不能被融合");
            ErrorHintList.Add(ErrorCode.ERR_Pet_Hint_3, "出战宠物不能散步");
            ErrorHintList.Add(ErrorCode.ERR_Pet_Hint_4, "出战宠物不能分解");
            ErrorHintList.Add(ErrorCode.ERR_Pet_CanNotLock, "小于两技能宠物不能锁定技能");
            

            ErrorHintList.Add(ErrorCode.ERR_ItemOnlyUseMiJing, "该道具只能在宝藏之地使用");
            ErrorHintList.Add(ErrorCode.ERR_ItemOnlyUseOcc, "该道具只有{0}可以使用");

            ErrorHintList.Add(ErrorCode.ERR_Union_Same_Name, "已经存在同名的家族");
            ErrorHintList.Add(ErrorCode.ERR_Union_PeopleMax, "家族成员人数已达上限");
            ErrorHintList.Add(ErrorCode.ERR_PlayerHaveUnion, "玩家已经有家族了");
            ErrorHintList.Add(ErrorCode.ERR_Union_HavActive, "只能同时研究一个科技");
            ErrorHintList.Add(ErrorCode.ERR_Union_NotActive, "当前没有正在研究的科技");
            ErrorHintList.Add(ErrorCode.ERR_Already_Guess, "已经竞猜该数字");
            ErrorHintList.Add(ErrorCode.ERR_Union_NotRemove, "家族还有其他玩家，族长不能离开！");

            ErrorHintList.Add(ErrorCode.ERR_RoleYueKaRepeat, "周卡重复开启");
            ErrorHintList.Add(ErrorCode.ERR_AlreadyFinish, "活动已经结束");
            ErrorHintList.Add(ErrorCode.ERR_MysteryItem_Max, "此道具每日购买次数已达上限");
            ErrorHintList.Add(ErrorCode.ERR_HongBaoTime, "距离上次领取红包在线时间大于30分钟");
            ErrorHintList.Add(ErrorCode.ERR_HongBaoLevel, "需要等级大于12级");
            ErrorHintList.Add(ErrorCode.ERR_SoloNumMax, "竞技场今日挑战次数已达上限");
            ErrorHintList.Add(ErrorCode.ERR_SoloExist, "已存在竞技场匹配列表中!");
            ErrorHintList.Add(ErrorCode.ERR_MapLimit, "当前地图人数已达上限!");
            ErrorHintList.Add(ErrorCode.ERR_TurtleSupport_1, "同账号只能支持一次!");
            

            ErrorHintList.Add(ErrorCode.ERR_ShangJinNumFull, "今日领取赏金次数已满");
            ErrorHintList.Add(ErrorCode.ERR_TaskLimited, "今日任务次数已满");

            ErrorHintList.Add(ErrorCode.ERR_BattleJoined, "已经参与过战场活动");

            ErrorHintList.Add(ErrorCode.Err_PaiMaiPriceLow, "拍卖行出售价格过低");
            ErrorHintList.Add(ErrorCode.Err_StopPaiMai, "此道具无法在拍卖行中进行出售!");
            ErrorHintList.Add(ErrorCode.Err_Auction_Low, "出价低于其他玩家!");
            ErrorHintList.Add(ErrorCode.Err_Auction_Finish, "竞拍已结束!");
            ErrorHintList.Add(ErrorCode.ERR_Parameter, "参数错误!");
            ErrorHintList.Add(ErrorCode.ERR_Chat_JinYan, "你已经被禁言一天!");
            
            ErrorHintList.Add(ErrorCode.ERR_HoreseNotFight,"请到主城坐骑训练师选择自己的骑乘坐骑吧");
            ErrorHintList.Add(ErrorCode.ERR_HoreseNotActive, "坐骑未出战");
            
            ErrorHintList.Add(ErrorCode.ERR_VitalityNotEnoughError, "活力不足");
            ErrorHintList.Add(ErrorCode.Err_TeamDungeonXieZhu, "帮助副本只能创建比自己等级低10级以上的副本");

            ErrorHintList.Add(ErrorCode.ERR_AlreadyReceived, "已经领取过奖励");
            ErrorHintList.Add(ErrorCode.ERR_AlreadyReceived2, "已经领取过该批奖励");
            

            ErrorHintList.Add(ErrorCode.ERR_TitleNoActived, "称号未激活");

            ErrorHintList.Add(ErrorCode.ERR_AlreadyPlant, "土地已经种植了");
            ErrorHintList.Add(ErrorCode.ERR_JiaYuanLevel, "家园等级不足");
            ErrorHintList.Add(ErrorCode.ERR_PeopleNumber, "人口已达上限");
            ErrorHintList.Add(ErrorCode.ERR_JiaYuanSteal, "该作物已达偷取上限");
            ErrorHintList.Add(ErrorCode.ERR_PeopleNoEnough, "购买后人口超过上限");
            ErrorHintList.Add(ErrorCode.ERR_CanNotGather, "未成熟或者收获次数用完，请重登尝试");
            
            ErrorHintList.Add(ErrorCode.ERR_PopularizeThe, "相同账号的角色不能互相推广");
            ErrorHintList.Add(ErrorCode.ERR_SerialNoExist, "序列号不存在");
            ErrorHintList.Add(ErrorCode.ERR_PopularizeNot, "被推广人不存在");
            ErrorHintList.Add(ErrorCode.ERR_PopularizeMax, "被推广次数已达上限");
            

            ErrorHintList.Add(ErrorCode.ERR_ModifyData, "数据异常,请稍后再试");
            ErrorHintList.Add(ErrorCode.ERR_PackageFrequent, "消息异常,请稍后再试");
            ErrorHintList.Add(ErrorCode.ERR_PaiMaiBuyMaxPage, "拍卖道具页数已达底部");
            ErrorHintList.Add(ErrorCode.ERR_FenXiangMaxNum, "今日分享次数已达上限,请明日再来");
            ErrorHintList.Add(ErrorCode.ERR_RoleInBlackListError, "该角色已被禁止登陆");
            ErrorHintList.Add(ErrorCode.ERR_PaiMaiSellLimit, "当前上架已达上限");
            ErrorHintList.Add(ErrorCode.ERR_BuyMaxLimit, "该道具购买今日已达上限");

            ErrorHintList.Add(ErrorCode.Pre_Condition_Error, "前置条件不足！");
        }

        public string GetHint(int code)
        {
            if (code == 0)
            {
                return "";
            }
            string hintStr = code.ToString();
            ErrorHintList.TryGetValue(code, out hintStr);
            return hintStr;
        }

        public void ErrorHint(int code) 
        {
            if (code == 0)
            {
                return;
            }
            string hintStr = code.ToString();
            ErrorHintList.TryGetValue(code, out hintStr);
            if (string.IsNullOrEmpty(hintStr))
            {
                FloatTipManager.Instance.ShowFloatTipDi(code.ToString());
            }
            else
            {
                FloatTipManager.Instance.ShowFloatTipDi(hintStr);
            }
        }

    }
}
