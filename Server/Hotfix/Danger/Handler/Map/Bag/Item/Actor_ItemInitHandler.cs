using System;
using System.Collections.Generic;

namespace ET
{
	//游戏背包
	[ActorMessageHandler]
	public class Actor_ItemInitHandler : AMActorLocationRpcHandler<Unit, Actor_ItemInitRequest, Actor_ItemInitResponse>
	{
		protected override async ETTask Run(Unit unit, Actor_ItemInitRequest request, Actor_ItemInitResponse response, Action reply)
		{
			BagComponent bagComponent = unit.GetComponent<BagComponent>();

			//测试 送一个转职道具
			//if (ComHelp.IsInnerNet() && bagComponent.GetItemNumber(90000014) < 10)
			//{
   //             bagComponent.OnAddItemData($"90000014;10", $"{ItemGetWay.GM}_{TimeHelper.ServerNow()}", false);
   //         }

            //读取数据库
            int occ = unit.GetComponent<UserInfoComponent>().UserInfo.Occ;
            int occTwo = unit.GetComponent<UserInfoComponent>().UserInfo.OccTwo;
            List<BagInfo> bagInfos = bagComponent.GetAllItems(occ, occTwo);

			/*
			for (int i = 0; i < bagInfos.Count; i++) {
				Log.Info("道具ID：" + bagInfos[i]  + bagInfos[i].GetWay);
			}
			*/

			
			//初始化
			for (int i = 0; i < bagInfos.Count; i++)
			{
				if (bagInfos[i].FumoProLists.Count > 0
					&& bagInfos[i].FumoProLists[0].HideValue > 10000)
				{
					bagInfos[i].FumoProLists.Clear();
				}
				if (string.IsNullOrEmpty(bagInfos[i].GemIDNew))
				{
					bagInfos[i].GemIDNew = ItemHelper.DefaultGem;
                    bagInfos[i].GemHole = ItemHelper.DefaultGem;
                }

				//鉴定符错误
				//ItemConfig itemConfig = ItemConfigCategory.Instance.Get(bagInfos[i].ItemID);
				//if(itemConfig.ItemSubType == 121)
				//{
				//	try
				//	{
				//		int quality = int.Parse(bagInfos[i].ItemPar);
				//	}
				//	catch (Exception ex)
				//	{
				//		Log.Debug(ex.ToString()+ "_____" + bagInfos[i].ItemPar);
				//	}
				//	bagInfos[i].ItemPar = "99";
				//}
			}

			if (bagComponent.FashionEquipList.Count == 0)
			{
                OccupationConfig occupationConfig = OccupationConfigCategory.Instance.Get(occ);
				for (int i = 0; i < occupationConfig.FashionBase.Length; i++)
				{
					bagComponent.FashionEquipList.Add(occupationConfig.FashionBase[i]);
                }
			}
            if (bagComponent.FashionActiveIds.Count == 0)
            {
                OccupationConfig occupationConfig = OccupationConfigCategory.Instance.Get(occ);
                for (int i = 0; i < occupationConfig.FashionBase.Length; i++)
                {
                    bagComponent.FashionActiveIds.Add(occupationConfig.FashionBase[i]);
                }
            }

            List<int> fashionTypes = new List<int>();
			for (int i = bagComponent.FashionEquipList.Count - 1; i >= 0; i--)
			{
				if(!FashionConfigCategory.Instance.Contain(bagComponent.FashionEquipList[i]))
				{
                    bagComponent.FashionEquipList.RemoveAt(i);	
                    continue;
                }

				FashionConfig fashionConfig = FashionConfigCategory.Instance.Get(bagComponent.FashionEquipList[i]);
				if (fashionTypes.Contains(fashionConfig.SubType))
				{
                    fashionTypes.RemoveAt(i);	
                    continue;
				}

				fashionTypes.Add(fashionConfig.SubType);
            }
			for (int i = bagComponent.FashionActiveIds.Count - 1; i >= 0; i--)
			{
				if (!FashionConfigCategory.Instance.Contain(bagComponent.FashionActiveIds[i]))
				{
					bagComponent.FashionActiveIds.RemoveAt(i);
					continue;
				}
			}

            response.BagInfos = bagInfos;
			response.QiangHuaLevel = bagComponent.QiangHuaLevel;
			response.QiangHuaFails = bagComponent.QiangHuaFails;
			//response.BagAddedCell = bagComponent.BagAddedCell;
			response.WarehouseAddedCell = bagComponent.WarehouseAddedCell;
			response.FashionActiveIds = bagComponent.FashionActiveIds;	
			response.FashionEquipList = bagComponent.FashionEquipList;
            response.SeasonJingHePlan = bagComponent.SeasonJingHePlan;
			response.AdditionalCellNum = bagComponent.AdditionalCellNum;	
            reply();
			await ETTask.CompletedTask;
		}
	}
}