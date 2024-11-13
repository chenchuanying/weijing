namespace ET
{
    public static partial class ErrorCode
    {
        public const int ERR_Success = 0;

        // 1-11004 是SocketError请看SocketError定义
        //-----------------------------------
        // 100000-109999是Core层的错误

        // 110000以下的错误请看ErrorCore.cs

        // 这里配置逻辑层的错误码
        // 110000 - 200000是抛异常的错误
        // 200001以上不抛异常

        public const int ERR_Error = 200002;                                       //通用错误

        public const int ERR_NetWorkError = 200003;                                 //网络错误

        public const int ERR_OperationOften = 200004;                               //操作太频繁


        public const int ERR_TransferFailError = 200006;
        public const int ERR_EnterSonFubenError = 200008;
        public const int ERR_GoldNotEnoughError = 200009;                           //金币不足
        public const int ERR_DiamondNotEnoughError = 200010;                        //钻石不足
        public const int ERR_UnSafeSqlString = 200012;                              //非法字符串
        public const int ERR_EquipLvLimit = 200013;                                 //装备等级不足
        public const int ERR_BagIsFull = 200014;                                    //背包已满
        public const int ERR_ItemNotEnoughError = 200015;                           //道具不足
        public const int ERR_UserNoFind = 200016;                                   //未查询到角色      
        public const int ERR_RequestSceneTypeError = 200017;                        //"请求的Scene错误
        public const int ERR_ItemOnlyUseMiJing = 200018;                            //该道具只能在秘境使用
        public const int ERR_ItemOnlyUseOcc = 200019;                               //只有某职业才能使用
        public const int ERR_VitalityNotEnoughError = 200020;                       //活力不足
        public const int ERR_ItemDropProtect = 200021;                              //掉落保护中
        public const int ERR_NoPayValueError = 200022;                              //赞助额度不足
        public const int ERR_ItemNoUseTime = 200023;                                //道具使用次数不足
        public const int ERR_ItemBelongOther = 200024;                              //道具归属别人
        public const int ERR_ZhuaBuFail = 200025;
        public const int ERR_AleardyMaxCell = 200026;
        public const int ERR_HouBiNotEnough = 200027;                                 //金币不足
        public const int ERR_ItemNotExist = 200028;                                  //道具不存在
        public const int ERR_ItemBing = 200029;                                      //道具绑定状态
        public const int ERR_EquipChuanChengFail = 200030;                           //装备传承失败
        public const int ERR_AlreadyLearn = 200031;
        public const int Error_AngleNotEnough = 200032;
        public const int Error_PickErrorCell = 200033;
        public const int Error_PickWaitSelect = 200034;
        public const int ERR_WarehouseIsFull = 200035;                                    //仓库已满
        public const int ERR_OtherNotExist = 200036;

        //登录
        public const int ERR_AccountAlreadyRegister = 200101;                       //表示账号已经被注册
        public const int ERR_AccountOrPasswordError = 200102;                       //登录时,表示密码错误
        public const int ERR_RequestRepeatedly = 200103;                            //请求重复
        public const int ERR_LoginInfoIsNull = 200104;                              //登录出错
        public const int ERR_AccountInBlackListError = 200105;                      //账号黑名单
        public const int ERR_OtherAccountLogin = 200106;                            //异地登录
        public const int ERR_TokenError = 200107;                                   //token错误
        public const int ERR_EnterGameError = 200108;
        public const int ERR_ReEnterGameError = 200109;
        public const int ERR_ReEnterGameError2 = 200109;
        public const int ERR_SessionPlayerError = 200110;
        public const int ERR_NonePlayerError = 200111;
        public const int ERR_PlayerSessionError = 200112;
        public const int ERR_SessionStateError = 200113;
        public const int ERR_LoginTimeOut = 200114;                                 //登录超时
        public const int ERR_EnterQueue = 200115;                                   //进入排队    
        public const int ERR_LoginRealm = 200116;
        public const int ERR_StopServer = 200117;
        public const int ERR_BingPhoneError_1 = 200118;
        public const int ERR_BingPhoneError_2 = 200119;
        public const int ERR_VersionNoMatch = 200120;
        public const int ERR_ModifyData = 200121;
        public const int ERR_PaiMaiBuyMaxPage = 200122;                             //拍卖达到最大页数
        public const int Pre_Condition_Error = 200123;                              //前置条件不足
        public const int ERR_RequestExitFuben = 200124;
        public const int ERR_KickOutPlayer = 200125;                                //长时间不操作被踢下线
        public const int ERR_PackageFrequent = 200126;                                //发送太频繁
        public const int ERR_AlreadyHave = 200127;
        public const int ERR_RoleInBlackListError = 200128;                      //账号黑名单
        public const int ERR_IllegalPhoneError = 200129;
        public const int ERR_PaiMaiSellLimit = 200130;
        public const int ERR_BuyMaxLimit = 200131;                             //拍卖达到最大页数
        public const int ERR_Archiveing = 200132;

        //300001-400001 服务器提示性错误码
        //角色
        public const int ERR_EquipAppraisal_Item = 300001;                          //装备鉴定道具不足
        public const int ERR_ItemUseError = 300002;                                 //道具使用失败
        public const int ERR_Occ_Hint_1 = 300101;                                   //转职等级不足
        public const int ERR_Occ_Hint_2 = 300102;                                   //已经进行转职
        public const int ERR_NotRealName = 300202;                                  //登录时,未实名认证
        public const int ERR_GMError = 300203;
        public const int ERR_AlreadyReceived = 300204;                              //领取过奖励
        public const int ERR_RealNameFail = 300205;                              //实名认证失败
        public const int ERR_TeamDungeonTimesMax = 300206;                         //副本参与次数已经最大
        public const int ERR_AlreadyReceived2 = 300207;                              //领取过奖励

        public const int ERR_MailFull = 300301;                                     //邮箱已满
        public const int ERR_CreateRoleName = 300302;
        public const int ERR_WordChat = 300303;                                     //世界频道发消息 1分钟1次
        public const int ERR_InMakeCD = 300304;
        public const int ERR_MakeTypeError = 300305;
        public const int ERR_RoleNameRepeat = 300306;
        public const int ERR_ShuLianDuNotEnough = 300307;                           //熟练度不足
        public const int ERR_EquipType = 300308;                                    //装备类型不符
        public const int ERR_GemNoError = 300309;                                   //道具使用失败
        public const int ERR_GemShiShiNumFull = 300310;                             //镶嵌史诗宝石达到上限
        public const int ERR_UnionChatLimit = 300311;
        public const int ERR_EquipRepeat = 300312;                                    //装备类型不符
        public const int ERR_Parameter = 300313;
        public const int ERR_Chat_JinYan = 300314;
        public const int ERR_Chat_JinYan_3 = 300315;

        //队伍
        public const int ERR_TeamIsFull = 300401;                                   //队伍已满
        public const int ERR_LevelIsNot = 300402;                                   //等级不足
        public const int ERR_IsHaveTeam = 300403;                                   //已经有组队了
        public const int ERR_TimesIsNot = 300404;                                   //组队次数不足
        public const int ERR_IsNotLeader = 300405;                                  //队长才能创建副本
        public const int ERR_PlayerIsNot = 300406;                                  //人数不足
        public const int ERR_TeamerLevelIsNot = 300407;                            //队员等级不足
        public const int Err_TeamDungeonXieZhu = 300408;
        public const int Err_TeamIsNull = 300409;                                   //队伍信息为空
        public const int Err_HaveNotPrepare = 300410;                                //还有没准备的
        public const int Err_ShenYuanItemError = 300411;                             //深渊道具不足
        public const int Err_PlayerRefuse = 300412;
        public const int Err_OnLineTimeNot = 300413;
        public const int Err_OnLineTimeNotFenXiang = 300414;
        public const int ERR_LevelIsNotFenXiang = 300415;                                   //等级不足
        public const int ERR_HuoYueNot = 300416;

        //防沉迷
        public const int ERR_FangChengMi_Tip1 = 300501;
        public const int ERR_FangChengMi_Tip2 = 300502;
        public const int ERR_FangChengMi_Tip3 = 300503;
        public const int ERR_FangChengMi_Tip4 = 300504;
        public const int ERR_FangChengMi_Tip5 = 300505;
        public const int ERR_FangChengMi_Tip6 = 300506;
        public const int ERR_FangChengMi_Tip7 = 300507;

        public const int ERR_TaskCommited = 300601;                             //已经领取
        public const int ERR_NotFindLevel = 300602;                             //找不到对应关卡
        public const int ERR_TiLiNoEnough = 300603;                             //体力不足
        public const int ERR_LevelNotOpen = 300604;                             //前置关卡未通关
        public const int ERR_LevelNoEnough = 300605;                            //等级不足
        public const int ERR_LevelNormalNoPass = 300606;                        //普通关卡未通关
        public const int ERR_LevelChallengeNoPass = 300607;                     //挑战关卡未通关
        public const int ERR_NotFindNpc = 300608;                               //"当前场景未找到NPC"
        public const int ERR_TaskCanNotGet = 300609;                             //任务未达到领取条件
        public const int ERR_ExpNoEnough = 300610;                            //经验不足
        public const int ERR_NotFindAccount = 300611;
        public const int ERR_TaskNoComplete = 300612;                               //任务未完成
        public const int ERR_TaskLimited = 300613;                              //任务已达上限
        public const int ERR_TowerOfSealReachTop = 300614;                      // 封印之塔已经通关塔顶
        public const int ERR_Challenge_CD = 300615;                             //挑战CD
        public const int ERR_CreateRole_Limit= 300616;
        public const int ERR_CreateRole_Limit_2 = 300617;

        //活动
        public const int ERR_RoleYueKaRepeat = 300701;                          //月卡重复状态
        public const int ERR_AlreadyFinish = 300702;
        public const int ERR_HongBaoTime = 300703;
        public const int ERR_HongBaoLevel = 300704;
        public const int ERR_SoloNumMax = 300705;                               //竞技场次数达到上限
        public const int ERR_SoloExist = 300706;                               //竞技场次数达到上限
        public const int ERR_MapLimit = 300707;
        public const int ERR_TurtleSupport_1 = 300708;
        public const int ERR_HappyMove_CD = 300709;

        /// <summary>
        /// 技能CD中
        /// </summary>
        public const int ERR_UseSkillInCD1 = 300801;
        /// <summary>
        /// 技能公用CD
        /// </summary>
        public const int ERR_UseSkillInCD2 = 300802;
        /// <summary>
        /// 当前状态无法释放技能
        /// </summary>
        public const int ERR_CanNotUseSkill_1 = 300803;
        public const int ERR_UseSkillError = 300804;
        public const int ERR_NoUseItemSkill = 300805;
        public const int ERR_CanNotSkillDead = 300806;

        public const int ERR_UseSkillInCD3 = 300811;
        public const int ERR_UseSkillInCD4 = 300812;

        public const int ERR_UseSkillInCD5 = 300813;
        public const int ERR_UseSkillInCD6 = 300814;
        public const int ERR_SkillMoveTime = 300815;

        public const int ERR_CanNotUseSkill_Rigidity = 300816;
        public const int ERR_CanNotUseSkill_NetWait = 300817;
        public const int ERR_CanNotUseSkill_Dizziness = 300818;
        public const int ERR_CanNotUseSkill_JiTui = 300819;
        public const int ERR_CanNotUseSkill_Silence = 300820;
        public const int ERR_CanNotUseSkill_Sleep = 300821;
        public const int ERR_CanNotUseSkill_Hung = 300822;
        public const int ERR_CanNotUseSkill_MP = 300823;
        public const int ERR_CanNotUseSkill = 300824;

        public const int ERR_CanNotMove_1 = 300830;
        public const int ERR_CanNotMove_Dead = 300831;
        public const int ERR_CanNotMove_Rigidity = 300832;
        public const int ERR_CanNotMove_NetWait = 300833;
        public const int ERR_CanNotMove_Dizziness = 300834;
        public const int ERR_CanNotMove_JiTui = 300835;
        public const int ERR_CanNotMove_Shackle = 300836;
        public const int ERR_CanNotMove_Singing = 300837;
        public const int ERR_CanNotMove_Sleep = 300837;
        public const int ERR_CanNotMove_Fear = 300838;
        public const int ERR_CanNotMove_Speed = 300839;

        //宠物
        public const int ERR_Pet_Hint_1 = 300901;                                   //宠物星级出错
        public const int ERR_Pet_UpStar = 300902;                                   //宠物星级失败                                                              
        public const int ERR_Pet_AddSkillSame = 300903;                             //宠物打书技能ID相同
        public const int ERR_Pet_NoUseItem = 300904;                              //无法给宠物使用道具
        public const int ERR_PetIsFull = 300905;                                    //宠物已满
        public const int ERR_Pet_UpStage = 300906;                                   //宠物进阶   
        public const int ERR_Pet_NoExist = 300907;
        public const int ERR_CangKu_Already = 300908;
        public const int ERR_CangKu_NotOpen = 300910;
        public const int ERR_Pet_Hint_2 = 300911;                                   //出战宠物不能被融合
        public const int ERR_Pet_Hint_3 = 300912;                                   //出战宠物不能散步
        public const int ERR_Pet_Hint_4 = 300913;
        public const int ERR_Pet_CanNotLock = 300914;

        public const int ERR_Union_Same_Name = 301001;                              //同名的家族
        public const int ERR_MysteryItem_Max = 301002;                              //改道具每日购买次数已达上限
        public const int ERR_Union_Not_Exist = 301003;                              //家族不存在
        public const int ERR_UnionXiuLianMax = 301004;
        public const int ERR_Union_PeopleMax = 301005;                              //家族人数上限
        public const int ERR_Union_NotLeader = 301006;
        public const int ERR_Union_NotRemove = 301007;                              //族长不能解算家族
        public const int ERR_Union_NoPlayer = 301008;
        public const int ERR_Union_NoLimits = 301009;
        public const int ERR_PlayerHaveUnion = 301010;
        public const int ERR_Union_HavActive = 301011;
        public const int ERR_Union_NotActive = 301012;
        public const int ERR_Already_Guess = 301013;

        //赏金任务
        public const int ERR_ShangJinNumFull = 301101;                              //赏金任务完成上限

        //战场
        public const int ERR_BattleJoined = 301201;                                //已经参与过战场活动

        //拍卖行
        public const int Err_PaiMaiPriceLow = 301301;                               //拍卖出售价格过低
        public const int Err_StopPaiMai = 301302;                               //拍卖道具无法上架
        public const int Err_Auction_Low = 301303;
        public const int Err_Auction_Finish = 301304;

        //充值
        public const int ERR_IOSVerify = 301401;

        //坐骑
        public const int ERR_HoreseNotActive = 301501;                                //坐骑未激活
        public const int ERR_HoreseNotFight = 301502;                                 //坐骑未出战

        //珍宝
        public const int ERR_ShouJIActived = 301601;                                    //已经激活

        //称号
        public const int ERR_TitleNoActived = 301701;                                   //称号未激活

        //家园
        public const int ERR_AlreadyPlant = 301801;
        public const int ERR_CanNotGather = 301802;
        public const int ERR_WarehouseFul = 301803;
        public const int ERR_PeopleNumber = 301804;
        public const int ERR_JiaYuanLevel = 301805;
        public const int ERR_JiaYuanSteal = 301806;
        public const int ERR_PlantNotExist = 301807;
        public const int ERR_PeopleNoEnough = 301808;
        public const int ERR_LvNoHigh = 301809;

        //推广
        public const int ERR_PopularizeNot = 301901;
        public const int ERR_PopularizeThe = 301902;
        public const int ERR_SerialNoExist = 301903;
        public const int ERR_FenXiangMaxNum = 301904;                                   //今日分享次数已达上限,请明日再来
        public const int ERR_PopularizeMax = 301905;
    }
}