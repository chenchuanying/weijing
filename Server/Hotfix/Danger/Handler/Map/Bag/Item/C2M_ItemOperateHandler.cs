using Alipay.AopSdk.Core.Domain;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{

    [ActorMessageHandler]
    public class C2M_ItemOperateHandler : AMActorLocationRpcHandler<Unit, C2M_ItemOperateRequest, M2C_ItemOperateResponse>
    {
        protected override async ETTask Run(Unit unit, C2M_ItemOperateRequest request, M2C_ItemOperateResponse response, Action reply)
        {
            try
            {
                //获取UserID及User数据
                UserInfoComponent userInfoComponent = unit.GetComponent<UserInfoComponent>();
                UserInfo useInfo = userInfoComponent.UserInfo;
                long bagInfoID = request.OperateBagID;

                ItemLocType locType = ItemLocType.ItemLocBag;
                if (request.OperateType == 2)
                {
                    ItemConfig config = ItemConfigCategory.Instance.Get(int.Parse(request.OperatePar.Split('_')[0]));
                    locType = config.ItemType == (int)ItemTypeEnum.PetHeXin ? ItemLocType.ItemPetHeXinBag : locType;
                }
                if (request.OperateType == 4)
                {
                    locType = ItemLocType.ItemLocEquip;
                }
                if (request.OperateType == 7)
                {
                    locType = (ItemLocType)(int.Parse(request.OperatePar));
                }

                int weizhi = -1;
                ItemConfig itemConfig = null;
                BagInfo useBagInfo = unit.GetComponent<BagComponent>().GetItemByLoc(locType, bagInfoID);
                if (useBagInfo == null && request.OperateType != 8)
                {
                    reply();
                    return;
                }

                if (useBagInfo != null)
                {
                    itemConfig = ItemConfigCategory.Instance.Get(useBagInfo.ItemID);
                    weizhi = itemConfig.ItemSubType;
                }

                //通知客户端背包刷新
                M2C_RoleBagUpdate m2c_bagUpdate = new M2C_RoleBagUpdate();
                //使用道具
                if (request.OperateType == 1 && itemConfig != null)
                {
                    if (itemConfig.Id == 10000156)
                    {
                        reply();
                        return;
                    }
                    if (itemConfig.DayUseNum > 0 && userInfoComponent.GetDayItemUse(itemConfig.Id) >= itemConfig.DayUseNum)
                    {
                        response.Error = ErrorCode.ERR_ItemNoUseTime;
                        reply();
                        return;
                    }
                    if (itemConfig.SumUseNum > 0 && userInfoComponent.GetTotalUseTimes(itemConfig.Id) >= itemConfig.SumUseNum)
                    {
                        response.Error = ErrorCode.ERR_ItemNoUseTime;
                        reply();
                        return;
                    }

                    //获取背包数据
                    int costNumber = 1;
                    bool bagIsFull = false;
                    List<RewardItem> droplist = new List<RewardItem>();
                    if (itemConfig.ItemSubType == 8)   //碎片兑换
                    {
                        string[] duihuanparams = itemConfig.ItemUsePar.Split(';');
                        int neednum = int.Parse(duihuanparams[0]);
                        if (unit.GetComponent<BagComponent>().GetItemNumber(itemConfig.Id) < neednum)
                        {
                            response.Error = ErrorCode.ERR_ItemNotEnoughError;
                            reply();
                            return;
                        }
                    }
                    if (itemConfig.ItemSubType == 9)   //充值达到一定额度开启宝箱获得道具
                    {
                        string[] itemPar = itemConfig.ItemUsePar.Split(';');
                        if (unit.GetComponent<NumericComponent>().GetAsLong(NumericType.RechargeNumber) < long.Parse(itemPar[0]))
                        {
                            response.Error = ErrorCode.ERR_NoPayValueError;
                            reply();
                            return;
                        }
                        string[] rewardInfos = itemConfig.ItemUsePar.Split(';');
                        DropHelper.DropIDToDropItem(int.Parse(rewardInfos[1]), droplist);

                        if (unit.GetComponent<BagComponent>().GetBagLeftCell() < ItemHelper.GetNeedCell(droplist))
                        {
                            bagIsFull = true;
                        }
                    }
                    if (itemConfig.ItemSubType == 102 || (itemConfig.ItemSubType == 103))  //宠物蛋(点击使用直接获得1个宠物)
                    {
                        if (unit.GetComponent<BagComponent>().GetBagLeftCell() < 1)
                        {
                            bagIsFull = true;
                        }
                    }
                    if (itemConfig.ItemSubType == 104)  //随机道具盒子
                    {
                        int dropid = int.Parse(itemConfig.ItemUsePar);
                        droplist = new List<RewardItem>();
                        DropHelper.DropIDToDropItem(dropid, droplist);
                        if (unit.GetComponent<BagComponent>().GetBagLeftCell() < droplist.Count)
                        {
                            bagIsFull = true;
                        }
                    }

                    if (itemConfig.ItemSubType == 110 && unit.DomainScene().GetComponent<MapComponent>().SceneId != 2000001) // 领主怪物召唤
                    {
                        response.Error = ErrorCode.ERR_ItemOnlyUseMiJing;
                        reply();
                        return;
                    }

                    if ((itemConfig.ItemSubType == 111 || itemConfig.ItemSubType == 112)
                        && ConfigHelper.BatchUseItemList.Contains(itemConfig.Id))
                    {
                        //目前只有111类型支持批量使用
                        if (!string.IsNullOrEmpty(request.OperatePar))
                        {
                            if (itemConfig.ItemSubType == 112)
                            {
                                // 经验盒子特殊处理，有免费开启和钻石开启
                                costNumber = int.Parse(request.OperatePar.Split(';')[1]);

                                string[] expInfos = itemConfig.ItemUsePar.Split('@');
                                string[] operatePar = request.OperatePar.Split(';'); //使用类型;数量
                                int needZuanshi = operatePar[0] == "1" ? int.Parse(expInfos[0]) * costNumber : 0;
                                string[] paramInfo = expInfos[int.Parse(operatePar[0])].Split(';');
                             
                                //如果当前钻石不足返回错误
                                if (unit.GetComponent<UserInfoComponent>().UserInfo.Diamond < needZuanshi)
                                {
                                    response.Error = ErrorCode.ERR_DiamondNotEnoughError;
                                    reply();
                                    return;
                                }
                            }
                            else
                            {
                                costNumber = int.Parse(request.OperatePar);
                            }
                        }

                    }

                    if (itemConfig.ItemSubType == 14      //召唤卷轴
                        || itemConfig.ItemSubType == 114) //宝石
                    {
                        costNumber = 0;
                    }
                    if (itemConfig.ItemSubType == 112)   //经验木桩
                    {
                        int openDay = DBHelper.GetOpenServerDay(unit.DomainZone());
                        if (openDay <= 1)
                        {
                            response.Error = ErrorCode.ERR_ItemNoUseTime;
                            reply();
                            return;
                        }
                    }
                    if (itemConfig.ItemSubType == 127)
                    {
                        if (unit.GetComponent<BagComponent>().GetBagLeftCell() < 1)
                        {
                            bagIsFull = true;
                        }
                    }
                    if (itemConfig.ItemSubType == 137)
                    {
                        //检测要附灵的宠物蛋是否存在
                        long chongwudanId = long.Parse(request.OperatePar);      
                        BagInfo chongwudan = unit.GetComponent<BagComponent>().GetItemByLoc(ItemLocType.ItemLocBag, chongwudanId);
                        if (chongwudan == null)
                        {
                            response.Error = ErrorCode.ERR_ItemNotExist;
                            reply();
                            return;
                        }
                    }
                    if (itemConfig.ItemSubType == 138)
                    {
                        // int totalTimes = int.Parse(GlobalValueConfigCategory.Instance.Get(19).Value);
                        if (unit.GetComponent<NumericComponent>().GetAsInt(NumericType.TeamDungeonTimes) <= 0)
                        {
                            response.Error = ErrorCode.ERR_TeamDungeonTimesMax;
                            reply();
                            return;
                        }
                    }
                    

                    if (bagIsFull)
                    {
                        response.Error = ErrorCode.ERR_BagIsFull;
                        reply();
                        return;
                    }

                    if (itemConfig.ItemType != 1
                       && itemConfig.ItemType != 2)
                    {
                        reply();
                        return;
                    }
                    if (unit.GetComponent<BagComponent>().OnCostItemData(useBagInfo, ItemLocType.ItemLocBag, costNumber))
                    {
                        bool costItemStatus = true;
                        //根据道具子类分发不同的功能
                        switch (itemConfig.ItemSubType)
                        {
                            //增加金币
                            case 1:
                                unit.GetComponent<UserInfoComponent>().UpdateRoleMoneyAdd(UserDataType.Gold, itemConfig.ItemUsePar, true, ItemGetWay.ItemBox_6);
                                break;
                            //增加经验
                            case 2:
                                unit.GetComponent<UserInfoComponent>().UpdateRoleMoneyAdd(UserDataType.Exp, itemConfig.ItemUsePar, true, ItemGetWay.ItemBox_6);
                                break;
                            //回城卷轴[返回另外一个副本场景]
                            case 4:
                                if (unit.DomainScene().GetComponent<MapComponent>().SceneTypeEnum == (int)SceneTypeEnum.LocalDungeon)
                                {
                                    LocalDungeonComponent localDungeon = unit.DomainScene().GetComponent<LocalDungeonComponent>();
                                    TransferHelper.LocalDungeonTransfer(unit, 0, int.Parse(itemConfig.ItemUsePar), localDungeon.FubenDifficulty).Coroutine();
                                }
                                break;
                            //图纸制造
                            case 5:
                                break;
                            //随机宝箱
                            case 6:
                                int dropId = 0;
                                try
                                {
                                    dropId = int.Parse(itemConfig.ItemUsePar);
                                }
                                catch (Exception ex)
                                { 
                                    Log.Error(ex.ToString() + $"{itemConfig.Id}   dropId ==0");

                                }
                                if (dropId > 0)
                                {
                                    DropHelper.DropIDToDropItem_2(dropId, droplist);
                                    unit.GetComponent<BagComponent>().OnAddItemData(droplist, string.Empty, $"{ItemGetWay.ItemBox_6}_{TimeHelper.ServerNow()}_{itemConfig.Id}");
                                }
                                break;
                            //兑换：
                            case 8:
                                string[] duihuanparams = itemConfig.ItemUsePar.Split(';');
                                int neednum = int.Parse(duihuanparams[0]);
                                int newItem = int.Parse(duihuanparams[1]);

                                unit.GetComponent<BagComponent>().OnCostItemData($"{itemConfig.Id};{neednum - 1}", ItemLocType.ItemLocBag, ItemGetWay.DuiHuan);
                                unit.GetComponent<BagComponent>().OnAddItemData($"{newItem};1", $"{ItemGetWay.ItemBox_8}_{TimeHelper.ServerNow()}");
                                break;
                            case 9:
                                unit.GetComponent<BagComponent>().OnAddItemData(droplist, string.Empty, $"{ItemGetWay.ActivityHongBao}_{TimeHelper.ServerNow()}");
                                break;
                            //冷却时间清空卷轴"
                            case 12:
                                unit.GetComponent<UserInfoComponent>().OnCleanBossCD();
                                if (unit.DomainScene().GetComponent<MapComponent>().SceneTypeEnum == (int)SceneTypeEnum.LocalDungeon)
                                {
                                    unit.DomainScene().GetComponent<LocalDungeonComponent>().OnCleanBossCD();
                                }
                                break;
                            //召唤卷轴
                            case 14:
                                if (unit.DomainScene().GetComponent<MapComponent>().SceneTypeEnum == (int)SceneTypeEnum.LocalDungeon)
                                {
                                    UnitFactory.CreateTempFollower(unit, int.Parse(itemConfig.ItemUsePar));
                                }
                                break;
                            case 15:    //附魔道具
                                List<HideProList> hideProLists = XiLianHelper.GetItemFumoPro(itemConfig.Id);
                                unit.GetComponent<BagComponent>().OnEquipFuMo(itemConfig.Id, hideProLists, int.Parse(request.OperatePar));
                                unit.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.FoMoNumber_213, 0, 1);
                                break;
                            case 16: //附魔技能
                                unit.GetComponent<UserInfoComponent>().UserInfo.MakeList.Add(int.Parse(itemConfig.ItemUsePar));
                                break;
                            //使用技能
                            case 101:
                                break;
                            //宠物蛋
                            case 102:
                                string[] getway = useBagInfo.GetWay.Split('_');
                                unit.GetComponent<PetComponent>().OnAddPet(int.Parse(getway[0]), int.Parse(itemConfig.ItemUsePar), 0, useBagInfo.FuLing);
                                break;
                            //随机宠物蛋
                            case 103:
                                getway = useBagInfo.GetWay.Split('_');
                                int petId = int.Parse(itemConfig.ItemUsePar);
                                int skinId = 0;
                                if(!string.IsNullOrEmpty(useBagInfo.ItemPar))
                                {
                                    skinId = int.Parse(useBagInfo.ItemPar);
                                }
                                unit.GetComponent<PetComponent>().OnAddPet(int.Parse(getway[0]), petId, skinId, useBagInfo.FuLing);
                                break;
                            //随机盒子
                            case 104:
                                unit.GetComponent<BagComponent>().OnAddItemData(droplist, string.Empty, $"{ItemGetWay.ItemBox_104}_{TimeHelper.ServerNow()}");
                                break;
                            //指定道具
                            case 106:
                                unit.GetComponent<BagComponent>().OnAddItemData(itemConfig.ItemUsePar, $"{ItemGetWay.ItemBox_106}_{TimeHelper.ServerNow()}");
                                break;
                            //永久技能
                            case 107:
                                //判定职业是否符合
                                if (itemConfig.ItemUsePar != "0")
                                {
                                    if (itemConfig.ItemUsePar == unit.GetComponent<UserInfoComponent>().UserInfo.Occ.ToString() || itemConfig.ItemUsePar == unit.GetComponent<UserInfoComponent>().UserInfo.OccTwo.ToString())
                                    {
                                        //符合条件
                                    }
                                    else
                                    {
                                        //不符合条件
                                        costItemStatus = false;
                                        response.Error = ErrorCode.ERR_ItemUseError;
                                        break;
                                    }
                                }
                                unit.GetComponent<SkillSetComponent>().OnAddSkillBook(SkillSourceEnum.Book, int.Parse(itemConfig.SkillID));
                                break;
                            case 108:   //宠物经验骨头
                            case 109:   //宠物经验牛奶
                                break;
                            case 110:
                                //1;20;70010101,70010102@21;70;70020101,70020102
                                int createMonsterID = 0;
                                int lv = unit.GetComponent<UserInfoComponent>().UserInfo.Lv;
                                string[] monsters = itemConfig.ItemUsePar.Split('@');

                                if (monsters.Length > 100)
                                {
                                    Log.Error($"monsters.Length > 100:  {itemConfig.ItemUsePar}");
                                    reply();
                                    return;
                                }
                                for (int c = 0; c < monsters.Length; c++)
                                {
                                    //1;20;70010101,70010102
                                    string[] lelveparams = monsters[c].Split(";");
                                    int level_1 = int.Parse(lelveparams[0]);
                                    int level_2 = int.Parse(lelveparams[1]);
                                    if (lv < level_1 || lv > level_2)
                                    {
                                        continue;
                                    }
                                    string[] ids = lelveparams[2].Split(',');
                                    int r_number = RandomHelper.RandomNumber(0, ids.Length);
                                    Vector3 vector3 = new Vector3(unit.Position.x + RandomHelper.RandFloat01() * 1, unit.Position.y, unit.Position.z + RandomHelper.RandFloat01() * 1);
                                    Unit monster = UnitFactory.CreateMonster(unit.DomainScene(), int.Parse(ids[r_number]), vector3, new CreateMonsterInfo()
                                    {
                                        Camp = CampEnum.CampMonster1
                                    });

                                    createMonsterID = int.Parse(ids[r_number]);
                                }
                                //发送广播信息
                                if (createMonsterID != 0)
                                {
                                    MonsterConfig monsterCof = MonsterConfigCategory.Instance.Get(createMonsterID);
                                    ServerMessageHelper.SendServerMessage(DBHelper.GetChatServerId(unit.DomainZone()),
                                        NoticeType.Notice, "玩家" + unit.GetComponent<UserInfoComponent>().UserInfo.Name + "在宝藏之地召唤出领主怪物:<color=#FF75F0>" + monsterCof.MonsterName + "</color>").Coroutine();
                                }
                                break;
                            //金币袋子
                            case 111:
                                string[] jinbiInfos = itemConfig.ItemUsePar.Split(';');
                                int userLv = unit.GetComponent<UserInfoComponent>().UserInfo.Lv;
                                ExpConfig expConfig = ExpConfigCategory.Instance.Get(userLv);
                                long addCoin = (int)RandomHelper.RandomNumberFloat(float.Parse(jinbiInfos[0]) * expConfig.RoseGoldPro, float.Parse(jinbiInfos[1]) * expConfig.RoseGoldPro);
                                addCoin *= costNumber;
                                unit.GetComponent<UserInfoComponent>().UpdateRoleMoneyAdd(UserDataType.Gold, addCoin.ToString(), true, ItemGetWay.ItemBox_6);
                                break;
                            //经验木桩
                            case 112:
                                string[] expInfos = itemConfig.ItemUsePar.Split('@');
                                string[] operatePar = request.OperatePar.Split(';'); //使用类型;数量
                                int needZuanshi = operatePar[0] == "1"? int.Parse(expInfos[0]) * costNumber : 0;
                                string[] paramInfo = expInfos[int.Parse(operatePar[0])].Split(';');
                                userLv = unit.GetComponent<UserInfoComponent>().UserInfo.Lv;

                                expConfig = ExpConfigCategory.Instance.Get(userLv);
                                int addExp = (int)RandomHelper.RandomNumberFloat(float.Parse(paramInfo[0]) * expConfig.RoseExpPro, float.Parse(paramInfo[1]) * expConfig.RoseExpPro);
                                addExp *= costNumber;   
                                unit.GetComponent<UserInfoComponent>().UpdateRoleMoneyAdd(UserDataType.Exp, addExp.ToString(), true, ItemGetWay.DuiHuan);
                                if (needZuanshi > 0)
                                {
                                    unit.GetComponent<UserInfoComponent>().UpdateRoleMoneySub(UserDataType.Diamond, (needZuanshi * -1).ToString(), true, ItemGetWay.DuiHuan);
                                }

                                response.OperatePar = addExp.ToString();
                                break;
                            //藏宝图
                            case 113:
                                int dropid = int.Parse(useBagInfo.ItemPar.Split('@')[2]);
                                UnitFactory.CreateDropItems(unit, unit, 0, dropid, request.OperatePar);
                                break;
                            case 114: //宝石
                                break;
                            case 115://宠物皮肤激活道具
                                unit.GetComponent<PetComponent>().OnUnlockSkin(itemConfig.ItemUsePar);
                                break;
                            case 116:   //角色洗点
                                unit.GetComponent<HeroDataComponent>().OnResetPoint();
                                break;
                            case 117:   //宠物洗点
                            case 118:   //宠物资质
                            case 119:   //宠物成长
                                break;
                            case 120://120 冒险积分
                                unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.MaoXianExp, int.Parse(itemConfig.ItemUsePar), 0);
                                break;
                            case 121: //鉴定符
                                break;
                            case 122:   //宠物技能书
                                break;
                            case 123:   //宠物扩展工具
                                unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.PetExtendNumber, 1, 0);
                                break;
                            case 124: //仓库扩展工具
                                int cangkuNumber = unit.GetComponent<NumericComponent>().GetAsInt(NumericType.CangKuNumber);
                                unit.GetComponent<NumericComponent>().ApplyValue(NumericType.CangKuNumber, cangkuNumber + 1);
                                break;
                            case 125://坐骑获取
                                userInfoComponent.OnHorseActive(int.Parse(itemConfig.ItemUsePar), true);
                                int hourseId = int.Parse(itemConfig.ItemUsePar);
                                bool canhorse = hourseId == 10001 ? userInfoComponent.UserInfo.Lv >= 25 : true;
                                if (canhorse && userInfoComponent.UserInfo.HorseIds.Count == 1)
                                {
                                    unit.GetComponent<NumericComponent>().ApplyValue(NumericType.HorseFightID, hourseId);
                                }
                                Function_Fight.GetInstance().UnitUpdateProperty_Base( unit, true, true );
                                break;
                            case 126: //集字
                                break;
                            case 127: //藏宝图
                                string rewardItem = useBagInfo.ItemPar.Split('@')[2];
                                unit.GetComponent<BagComponent>().OnAddItemData(rewardItem, $"{ItemGetWay.TreasureMap}_{TimeHelper.ServerNow()}");
                                unit.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.TreasureMapNumber_210, 0, 1);

                                //普通
                                if (itemConfig.ItemQuality == 4)
                                {
                                    unit.GetComponent<TaskComponent>().TriggerTaskEvent(TaskTargetType.TreasureMapNormal_26, 0, 1);
                                    unit.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.TreasureMapNormal_26, 0, 1);
                                }
                                if (itemConfig.ItemQuality == 5)
                                {
                                    unit.GetComponent<TaskComponent>().TriggerTaskEvent(TaskTargetType.TreasureMapHigh_27, 0, 1);
                                    unit.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.TreasureMapHigh_27, 0, 1);
                                }

                                break;
                            case 128://激活称号
                                unit.GetComponent<TitleComponent>().OnActiveTile(int.Parse(itemConfig.ItemUsePar));
                                break;
                            case 129://激活精灵
                                unit.GetComponent<ChengJiuComponent>().OnActiveJingLing(int.Parse(itemConfig.ItemUsePar));
                                Function_Fight.GetInstance().UnitUpdateProperty_Base(unit, true, true);
                                break;
                            case 131://增加饱食度
                                string[] baoshipas = itemConfig.ItemUsePar.Split(';')[0].Split(',');
                                int baoshiadd = RandomHelper.RandomNumber(int.Parse(baoshipas[0]), int.Parse(baoshipas[1]) + 1);
                                unit.GetComponent<UserInfoComponent>().UpdateRoleData(UserDataType.BaoShiDu, baoshiadd.ToString());
                                break;
                            case 132:
                                long reduceTime = long.Parse(itemConfig.ItemUsePar);
                                unit.GetComponent<NumericComponent>().ApplyChange(null, NumericType.SeasonBossRefreshTime, -1 * reduceTime, 0);
                                break;
                            case 133:
                            case 134:
                                break;
                            case 135:
                                C2M_SkillCmd cmd = new C2M_SkillCmd();
                                cmd.SkillID = int.Parse(itemConfig.ItemUsePar);
                                cmd.TargetID = unit.Id;
                                cmd.TargetAngle = (int)Quaternion.QuaternionToEuler(unit.Rotation).y;
                                cmd.TargetDistance = 0f;
                                unit.GetComponent<SkillManagerComponent>().OnUseSkill(cmd);
                                break;
                            case 136:
                                break;
                            case 137:

                                //宠物蛋附灵
                                long chongwudanId = long.Parse(request.OperatePar);
                                BagInfo chongwudan = unit.GetComponent<BagComponent>().GetItemByLoc(ItemLocType.ItemLocBag, chongwudanId);
                                chongwudan.FuLing = 1;
                                m2c_bagUpdate.BagInfoUpdate.Add(chongwudan);
                                break;
                            case 138:
                                // 增加副本次数
                                unit.GetComponent<NumericComponent>().ApplyValue(NumericType.TeamDungeonTimes, unit.GetTeamDungeonTimes() - 1);
                                break;
                            case 139:
                                //增加背包格子
                                unit.GetComponent<BagComponent>().AdditionalCellNum[0]++;
                                break;
                            case 140:
                                unit.GetComponent<BagComponent>().AdditionalCellNum[5]++;
                                unit.GetComponent<BagComponent>().AdditionalCellNum[6]++;
                                unit.GetComponent<BagComponent>().AdditionalCellNum[7]++;
                                unit.GetComponent<BagComponent>().AdditionalCellNum[8]++;
                                //增加仓库格子
                                break;
                        }

                        //扣除道具
                        if (costItemStatus)
                        {
                            if (useBagInfo.ItemNum <= 0)
                            {
                                m2c_bagUpdate.BagInfoDelete.Add(useBagInfo);
                            }
                            else
                            {
                                m2c_bagUpdate.BagInfoUpdate.Add(useBagInfo);
                            }
                        }
                        if (itemConfig.DayUseNum > 0)
                        {
                            userInfoComponent.OnDayItemUse(itemConfig.Id);
                        }
                        if (itemConfig.SumUseNum > 0)
                        {
                            userInfoComponent.OnTotalUseTimes(itemConfig.Id);
                        }
                    }
                }

                //出售道具
                if (request.OperateType == 2 && locType == ItemLocType.ItemLocBag)
                {
                    //默认出售全部
                    //给与对应金币或货币奖励
                    string[] sellinfo = request.OperatePar.Split('_');
                    if (sellinfo.Length < 2)
                    {
                        response.Error = ErrorCode.ERR_VersionNoMatch;
                        reply();
                        return;
                    }
                    if (ComHelp.IfNull(sellinfo[1]))
                    {
                        response.Error = ErrorCode.ERR_VersionNoMatch;
                        reply();
                        return;
                    }

                    int sellNum = int.Parse(sellinfo[1]);
                    if (sellNum <= 0 || sellNum > useBagInfo.ItemNum)
                    {
                        Log.Error($"C2M_ItemOperateHandler 3");
                        response.Error = ErrorCode.ERR_ModifyData;
                        reply();
                        return;
                    }

                    string[] gemids = useBagInfo.GemIDNew.Split('_');
                    ItemConfig itemConf = null;
                    for (int i = 0; i < gemids.Length; i++)
                    {
                        if (gemids[i] == "0")
                        {
                            continue;
                        }
                        itemConf = ItemConfigCategory.Instance.Get(int.Parse(gemids[i]));
                        unit.GetComponent<UserInfoComponent>().UpdateRoleData((int)itemConf.SellMoneyType, (itemConf.SellMoneyValue).ToString());
                    }

                    //珍宝属性价格提升
                    int sellValue = itemConfig.SellMoneyValue;
                    if (useBagInfo.HideSkillLists.Contains(68000102))
                    {
                        sellValue = itemConfig.SellMoneyValue * 20;
                    }

                    itemConf = ItemConfigCategory.Instance.Get(useBagInfo.ItemID);
                    unit.GetComponent<UserInfoComponent>().UpdateRoleMoneyAdd((int)itemConf.SellMoneyType, (sellNum * sellValue).ToString(), true, ItemGetWay.Sell);
                    unit.GetComponent<BagComponent>().OnCostItemData(useBagInfo, locType, sellNum);
                    if (useBagInfo.ItemNum <= 0)
                    {
                        m2c_bagUpdate.BagInfoDelete.Add(useBagInfo);
                    }
                    else
                    {
                        m2c_bagUpdate.BagInfoUpdate.Add(useBagInfo);
                    }
                }

                if (request.OperateType == 2 && locType == ItemLocType.ItemPetHeXinBag)
                {
                    //默认出售全部
                    //给与对应金币或货币奖励
                    int sellNum = int.Parse(request.OperatePar.Split('_')[1]);
                    if (sellNum <= 0 || sellNum > useBagInfo.ItemNum)
                    {
                        Log.Error($"C2M_ItemOperateHandler 4");
                        response.Error = ErrorCode.ERR_ModifyData;
                        reply();
                        return;
                    }

                    unit.GetComponent<UserInfoComponent>().UpdateRoleData(itemConfig.SellMoneyType, (sellNum * itemConfig.SellMoneyValue).ToString());
                    unit.GetComponent<BagComponent>().OnCostItemData(useBagInfo, locType, sellNum);
                    if (useBagInfo.ItemNum == 0)
                    {
                        m2c_bagUpdate.BagInfoDelete.Add(useBagInfo);
                    }
                    else
                    {
                        m2c_bagUpdate.BagInfoUpdate.Add(useBagInfo);
                    }
                }

                //穿戴装备
                if (request.OperateType == 3)
                {
                    //判断等级
                    int roleLv = useInfo.Lv;
                    int equipLv = itemConfig.UseLv;
                    //宝石
                    if (itemConfig.ItemType == 4)
                    {
                        response.Error = ErrorCode.ERR_EquipLvLimit;
                        reply();
                        return;
                    }

                    //简易
                    if (useBagInfo.HideSkillLists.Contains(68000103))
                    {
                        equipLv = equipLv - 5;
                    }

                    //无级别
                    if (useBagInfo.HideSkillLists.Contains(68000106))
                    {
                        equipLv = 1;
                    }

                    if (roleLv < equipLv)
                    {
                        response.Error = ErrorCode.ERR_EquipLvLimit;
                        reply();
                        return;
                    }

                    //对应部位是否符合
                    if (itemConfig.ItemType == 3 && itemConfig.EquipType != 0)
                    {
                        //查看自身是否是二转
                        if (useInfo.OccTwo > 0)
                        {
                            OccupationTwoConfig occtwoCof = OccupationTwoConfigCategory.Instance.Get(useInfo.OccTwo);
                            if (occtwoCof.ArmorMastery == itemConfig.EquipType || itemConfig.EquipType == 99 || itemConfig.EquipType == 101 || itemConfig.EquipType == 201)
                            {
                                //可以穿戴
                            }
                            else
                            {
                                bool ifWear = ConfigHelper.OccWeaponList[useInfo.Occ].Contains(itemConfig.EquipType);
                                //if (useInfo.Occ == 1 && (itemConfig.EquipType == 1 || itemConfig.EquipType == 2))
                                //{
                                //    ifWear = true;
                                //}

                                //if (useInfo.Occ == 2 && (itemConfig.EquipType == 3 || itemConfig.EquipType == 4))
                                //{
                                //    ifWear = true;
                                //}

                                //if (useInfo.Occ == 3 && (itemConfig.EquipType == 1 || itemConfig.EquipType == 5))
                                //{
                                //    ifWear = true;
                                //}

                                //if (useInfo.Occ == 4 && (itemConfig.EquipType == 3 || itemConfig.EquipType == 4))
                                //{
                                //    ifWear = true;
                                //}

                                //佩戴部位不符
                                if (ifWear == false)
                                {
                                    response.Error = ErrorCode.ERR_EquipType;     //错误码:穿戴类型不符
                                    reply();
                                    return;
                                }
                            }
                        }
                    }

                    //获取之前的位置是否有装备
                    BagInfo beforeequip = null;
                    if (weizhi == (int)ItemSubTypeEnum.Shiping && !ComHelp.IsBanHaoZone(unit.DomainZone()))
                    {
                        List<BagInfo> equipList = unit.GetComponent<BagComponent>().GetEquipListByWeizhi(ItemLocType.ItemLocEquip, weizhi);
                        beforeequip = equipList.Count < 3 ? null : equipList[0];
                    }
                    else
                    {
                        beforeequip = unit.GetComponent<BagComponent>().GetEquipBySubType(ItemLocType.ItemLocEquip, weizhi);
                    }
                    if (beforeequip != null)
                    {
                        unit.GetComponent<BagComponent>().OnChangeItemLoc(beforeequip, ItemLocType.ItemLocBag, ItemLocType.ItemLocEquip);
                        unit.GetComponent<BagComponent>().OnChangeItemLoc(useBagInfo, ItemLocType.ItemLocEquip, ItemLocType.ItemLocBag);

                        unit.GetComponent<SkillSetComponent>().OnTakeOffEquip(ItemLocType.ItemLocEquip, beforeequip);
                        unit.GetComponent<SkillSetComponent>().OnWearEquip(useBagInfo);
                        m2c_bagUpdate.BagInfoUpdate.Add(beforeequip);
                    }
                    else
                    {
                        unit.GetComponent<BagComponent>().OnChangeItemLoc(useBagInfo, ItemLocType.ItemLocEquip, ItemLocType.ItemLocBag);
                        unit.GetComponent<SkillSetComponent>().OnWearEquip(useBagInfo);
                    }
                    int zodiacnumber = unit.GetComponent<BagComponent>().GetZodiacnumber();
                    unit.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.ZodiacEquipNumber_215, 0, zodiacnumber);

                    Function_Fight.GetInstance().UnitUpdateProperty_Base(unit, true, true);
                    useBagInfo.isBinging = true;
                    m2c_bagUpdate.BagInfoUpdate.Add(useBagInfo);

                    if (weizhi == (int)ItemSubTypeEnum.Wuqi)
                    {
                        unit.GetComponent<SkillPassiveComponent>().OnTrigegerPassiveSkill(SkillPassiveTypeEnum.WandBuff_8, useBagInfo.ItemID);
                        unit.GetComponent<NumericComponent>().ApplyValue(NumericType.Now_Weapon, useBagInfo.ItemID);
                        unit.GetComponent<NumericComponent>().ApplyValue(NumericType.WearWeaponFisrt, 1, true, true);
                    }
                }

                //卸下装备
                if (request.OperateType == 4)
                {
                    //判断背包格子是否足够
                    bool full = unit.GetComponent<BagComponent>().IsBagFull();
                    if (full)
                    {
                        response.Error = ErrorCode.ERR_BagIsFull;
                        reply();
                        return;
                    }
                    if (itemConfig.ItemType == ItemTypeEnum.Equipment && itemConfig.ItemSubType == 201)
                    {
                        reply();
                        return;
                    }

                    unit.GetComponent<BagComponent>().OnChangeItemLoc(useBagInfo, ItemLocType.ItemLocBag, ItemLocType.ItemLocEquip);
                    unit.GetComponent<SkillSetComponent>().OnTakeOffEquip(ItemLocType.ItemLocEquip, useBagInfo);
                    Function_Fight.GetInstance().UnitUpdateProperty_Base(unit, true, true);
                    m2c_bagUpdate.BagInfoUpdate.Add(useBagInfo);
                    if (weizhi == (int)ItemSubTypeEnum.Wuqi)
                    {
                        unit.GetComponent<SkillPassiveComponent>().OnTrigegerPassiveSkill(SkillPassiveTypeEnum.WandBuff_8, 0);
                        unit.GetComponent<NumericComponent>().ApplyValue(NumericType.Now_Weapon, 0);
                    }
                }

                //鉴定装备
                if (request.OperateType == 5)
                {
                    //判定材料消耗
                    bool ifSell = false;     //默认出售全部
                    long baginfoId = long.Parse(request.OperatePar);
                    int rolelv = useInfo.Lv;
                    string qulitylv = "";
                    bool ifItem = false;
                    if (baginfoId == 0 && itemConfig != null)
                    {
                        //金币鉴定，扣除金币
                        qulitylv = itemConfig.UseLv.ToString();
                        ifSell = unit.GetComponent<BagComponent>().OnCostItemData($"1;{ItemHelper.GetJianDingCoin(itemConfig.UseLv)}", ItemLocType.ItemLocBag, ItemGetWay.CostItem);
                    }
                    else
                    {
                        BagInfo baginfoCost = unit.GetComponent<BagComponent>().GetItemByLoc(ItemLocType.ItemLocBag, baginfoId);
                        if (baginfoCost != null)
                        {
                            //道具鉴定，扣除道具
                            qulitylv = baginfoCost.ItemPar;
                            qulitylv = string.IsNullOrEmpty(qulitylv) ? "0" : qulitylv;
                            ItemConfig costitemconfig = ItemConfigCategory.Instance.Get(baginfoCost.ItemID);

                            unit.GetComponent<TaskComponent>().TriggerTaskEvent(TaskTargetType.JianDingQulity_42, int.Parse(qulitylv), 1);
                            unit.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.JianDingQulity_42, int.Parse(qulitylv), 1);

                            unit.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.JianDing_1017, 0, 1);
                           
                            ifSell = unit.GetComponent<BagComponent>().OnCostItemData(baginfoId, 1);
                            ifItem = true;
                        }
                        else
                        {
                            ifSell = false;
                        }
                    }
                    if (ifSell)
                    {
                        //未鉴定才可以
                        useBagInfo.IfJianDing = false;

                        if (itemConfig.EquipType != 101)
                        {
                            useBagInfo.HideProLists = ItemAddHelper.GetEquipZhuanJingHidePro(itemConfig.ItemEquipID, itemConfig.Id, int.Parse(qulitylv), unit, ifItem);
                        }
                        m2c_bagUpdate.BagInfoUpdate.Add(useBagInfo);
                        //如果当前有隐藏技能一起飘出
                        if (useBagInfo.HideSkillLists.Count > 0)
                        {
                            string skillName = "";
                            for (int i = 0; i < useBagInfo.HideSkillLists.Count; i++)
                            {
                                skillName = skillName + $" {SkillConfigCategory.Instance.Get(useBagInfo.HideSkillLists[0]).SkillName}";
                                unit.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.EquipActiveSkillId_222, useBagInfo.HideSkillLists[i], 1);
                            }

                            string noticeContent = $"恭喜玩家<color=#B6FF00>{unit.GetComponent<UserInfoComponent>().UserName}</color>在拾取装备时,意外在装备上发现了隐藏技能:<color=#FFA313>{skillName}</color>";
                            ServerMessageHelper.SendBroadMessage(unit.DomainZone(), NoticeType.Notice, noticeContent);
                        }

                        long totalValue = 0;
                        if (useBagInfo.HideProLists != null && useBagInfo.HideProLists.Count > 0)
                        {
                            unit.GetComponent<TaskComponent>().TriggerTaskEvent(TaskTargetType.JianDingAttrNumber_43, useBagInfo.HideProLists.Count, 1);
                            unit.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.JianDingAttrNumber_43, useBagInfo.HideProLists.Count, 1);

                            for (int pro = 0; pro < useBagInfo.HideProLists.Count; pro++)
                            {
                                totalValue += useBagInfo.HideProLists[pro].HideValue;
                            }
                        }

                        unit.GetComponent<TaskComponent>().TriggerTaskEvent(TaskTargetType.JianDingValue_140, (int)totalValue, 1);
                        unit.GetComponent<TaskComponent>().TriggerTaskCountryEvent(TaskTargetType.JianDingValue_140, (int)totalValue, 1);

                        unit.GetComponent<ChengJiuComponent>().TriggerEvent(ChengJiuTargetEnum.JianDingEqipNumber_212, int.Parse(qulitylv), 1);
                    }
                    else
                    {
                        response.Error = ErrorCode.ERR_ItemNotEnoughError;
                        reply();
                        return;
                    }
                }

                //放入仓库
                if (request.OperateType == 6)
                {
                    int hourseId = int.Parse(request.OperatePar);
                    if (unit.GetComponent<BagComponent>().IsHourseFullByLoc(hourseId))
                    {
                        response.Error = ErrorCode.ERR_BagIsFull;     //错误码:仓库已满
                        reply();
                        return;
                    }
                    if (useBagInfo.Loc != (int)ItemLocType.ItemLocBag)
                    {
                        Log.Error($"C2M_ItemOperateHandler 5");
                        response.Error = ErrorCode.ERR_ModifyData;
                        reply();
                        return;
                    }

                    unit.GetComponent<BagComponent>().OnChangeItemLoc(useBagInfo, (ItemLocType)hourseId, ItemLocType.ItemLocBag);

                    m2c_bagUpdate.BagInfoUpdate.Add(useBagInfo);
                }

                //放回背包
                if (request.OperateType == 7)
                {
                    int hourseId = useBagInfo.Loc;
                    if (unit.GetComponent<BagComponent>().IsBagFull())
                    {
                        response.Error = ErrorCode.ERR_BagIsFull;     //错误码:仓库已满
                        reply();
                        return;
                    }
                    if (useBagInfo.Loc != hourseId)
                    {
                        Log.Error($"C2M_ItemOperateHandler 6");
                        response.Error = ErrorCode.ERR_ModifyData;
                        reply();
                        return;
                    }

                    unit.GetComponent<BagComponent>().OnChangeItemLoc(useBagInfo, ItemLocType.ItemLocBag, (ItemLocType)hourseId);
                    unit.GetComponent<TaskComponent>().OnGetItemForWarehouse(useBagInfo.ItemID);
                    m2c_bagUpdate.BagInfoUpdate.Add(useBagInfo);
                }

                //整理背包
                if (request.OperateType == 8)
                {
                    unit.GetComponent<BagComponent>().OnRecvItemSort((ItemLocType)(int.Parse(request.OperatePar)));
                }

                if (unit.IsRobot())
                {
                    DBHelper.SaveComponentCache(unit.DomainZone(), unit.Id, unit.GetComponent<BagComponent>()).Coroutine();
                }

                MessageHelper.SendToClient(unit, m2c_bagUpdate);
                //通知客户端属性刷新
                reply();
                await ETTask.CompletedTask;
            }
            catch (Exception ex)
            {
                Log.Debug(ex.ToString());
            }
        }
    }
}
