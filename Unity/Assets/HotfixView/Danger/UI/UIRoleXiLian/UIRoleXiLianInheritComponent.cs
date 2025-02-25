﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public class UIRoleXiLianInheritComponent : Entity, IAwake
	{
		public GameObject ProgressBarImg;
		public GameObject InheritTimesText;
		public GameObject Obj_EquipPropertyText;
		public GameObject EquipBaseSetList;
		public GameObject UIXiLianItemNode;
		public GameObject XiLianButton;
		public GameObject Text_CostValue;
		public GameObject Text_CostName;
		public GameObject CostItem;
		public GameObject EquipListNode;
		
		public UIItemComponent CostItemUI;
		public UIItemComponent XiLianItemUI;
		public List<UIItemComponent> EquipUIList = new List<UIItemComponent>();

		public BagInfo XilianBagInfo;
		public ETCancellationToken ETCancellationToken;
		public BagComponent BagComponent;
		public List<int> InheritSkills = new List<int> {  };
	}

	public class UIRoleXiLianInheritComponentAwake : AwakeSystem<UIRoleXiLianInheritComponent>
	{
		public override void Awake(UIRoleXiLianInheritComponent self)
		{
			self.EquipUIList.Clear();
			self.XilianBagInfo = null;
			ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			self.XiLianButton = rc.Get<GameObject>("XiLianButton");
			ButtonHelp.AddListenerEx(self.XiLianButton, () => { self.OnXiLianButton(1).Coroutine(); });

			self.ProgressBarImg = rc.Get<GameObject>("ProgressBarImg");
			self.InheritTimesText = rc.Get<GameObject>("InheritTimesText");
			self.Text_CostValue = rc.Get<GameObject>("Text_CostValue");
			self.Text_CostName = rc.Get<GameObject>("Text_CostName");
			self.CostItem = rc.Get<GameObject>("CostItem");
			self.EquipListNode = rc.Get<GameObject>("EquipListNode");
			self.UIXiLianItemNode = rc.Get<GameObject>("UIXiLianItemNode");
			
			self.Obj_EquipPropertyText = rc.Get<GameObject>("Obj_EquipPropertyText");
			self.EquipBaseSetList = rc.Get<GameObject>("EquipBaseSetList");
			self.BagComponent = self.ZoneScene().GetComponent<BagComponent>();

			self.GetParent<UI>().OnUpdateUI = () => { self.OnUpdateUI(); };

			self.XiLianItemUI = null;
			self.CostItem.SetActive(false);
			self.InitSubItemUI().Coroutine();
		}
	}

	public static class UIRoleXiLianInheritComponentSystem
	{
		//显示的时候刷新
		public static void OnUpdateUI(this UIRoleXiLianInheritComponent self)
		{
			self.XilianBagInfo = null;
			self.OnEquiListUpdate().Coroutine();
		}

		public static void UpdateAttribute(this UIRoleXiLianInheritComponent self, BagInfo bagInfo)
		{
			UICommonHelper.DestoryChild(self.EquipBaseSetList);
			if (bagInfo == null)
			{
				return;
			}
            BagComponent bagComponent = self.ZoneScene().GetComponent<BagComponent>();
			ItemViewHelp.ShowBaseAttribute(bagComponent.GetEquipList(), bagInfo, self.Obj_EquipPropertyText, self.EquipBaseSetList); ;
		}

		public static void OnUpdateXinLian(this UIRoleXiLianInheritComponent self)
		{
			BagInfo bagInfo = self.XilianBagInfo;
			self.CostItem.SetActive(bagInfo != null);
			self.UpdateAttribute(bagInfo);
			if (bagInfo == null)
			{
				return;
			}
			
			if (self.XiLianItemUI != null)
			{
				self.XiLianItemUI.UpdateItem(bagInfo, ItemOperateEnum.None);
			}


			//洗炼消耗
			string[] costitem = ItemHelper.GetInheritCost(bagInfo.InheritTimes).Split(';');
			int costitemid		= int.Parse(costitem[0]);
			int constitemnumber = int.Parse(costitem[1]);
			BagInfo bagInfoNeed = new BagInfo() { ItemID = costitemid, ItemNum = constitemnumber };

			self.CostItemUI.UpdateItem(bagInfoNeed, ItemOperateEnum.None);
			self.CostItemUI.Label_ItemNum.SetActive(false);

			self.Text_CostValue.GetComponent<Text>().text = string.Format("{0}/{1}", self.BagComponent.GetItemNumber(costitemid), constitemnumber);
			self.Text_CostValue.GetComponent<Text>().color = self.BagComponent.GetItemNumber(int.Parse(costitem[0])) >= int.Parse(costitem[1]) ? Color.green : Color.red;

			self.Text_CostName.GetComponent<Text>().text = ItemConfigCategory.Instance.Get(bagInfoNeed.ItemID).ItemName;
			self.Text_CostName.GetComponent<Text>().color = FunctionUI.GetInstance().QualityReturnColorDi((int)ItemConfigCategory.Instance.Get(bagInfoNeed.ItemID).ItemQuality);
			if (self.BagComponent.GetItemNumber(int.Parse(costitem[0])) >= int.Parse(costitem[1]) )
			{
				self.Text_CostValue.GetComponent<Text>().color = Color.green;
			}

			int maxTimes = GlobalValueConfigCategory.Instance.Get(117).Value2;
			self.ProgressBarImg.GetComponent<Image>().fillAmount = bagInfo.InheritTimes * 1f / maxTimes;
			self.InheritTimesText.GetComponent<Text>().text = $"{bagInfo.InheritTimes}/{maxTimes}次";
		}

		public static void OnXiLianReturn(this UIRoleXiLianInheritComponent self)
		{
			self.XilianBagInfo = self.BagComponent.GetBagInfo(self.XilianBagInfo.BagInfoID);
			self.OnUpdateXinLian();
			self.OnEquiListUpdate().Coroutine();
		}

		public static async ETTask OnEquiListUpdate(this UIRoleXiLianInheritComponent self)
		{
			int number = 0;
			var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
			var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);
			List<BagInfo> equipInfos = self.BagComponent.GetItemsByType(ItemTypeEnum.Equipment);

			for (int i = 0; i < equipInfos.Count; i++)
			{
				if (equipInfos[i].IfJianDing)
				{
					continue;
				}
				ItemConfig itemConfig = ItemConfigCategory.Instance.Get(equipInfos[i].ItemID);
				if (itemConfig.EquipType == 101)
				{
					continue;
				}

				if (itemConfig.UseLv < 60 && itemConfig.ItemQuality <= 5 )
				{
					continue;
				}

				//饰品不显示
				if (itemConfig.ItemSubType == 5) {
					continue;
				}

				UIItemComponent uI = null;
				if (number < self.EquipUIList.Count)
				{
					uI = self.EquipUIList[number];
					uI.GameObject.SetActive(true);
				}
				else
				{
					GameObject go = GameObject.Instantiate(bundleGameObject);
					UICommonHelper.SetParent(go, self.EquipListNode);
					uI = self.AddChild<UIItemComponent, GameObject>(go);
					uI.SetClickHandler((BagInfo bagInfo) => { self.OnSelectItem(bagInfo); });
					self.EquipUIList.Add(uI);
				}
				number++;
				uI.UpdateItem(equipInfos[i], ItemOperateEnum.ItemXiLian);
			}

			for (int i = number; i < self.EquipUIList.Count; i++)
			{
				self.EquipUIList[i].GameObject.SetActive(false);
			}

			if (self.XilianBagInfo != null)
			{
				self.OnSelectItem(self.XilianBagInfo);
			}
			else if (number > 0)
			{
				self.EquipUIList[0].OnClickUIItem();
			}
		}

		public static void OnSelectItem(this UIRoleXiLianInheritComponent self, BagInfo bagInfo)
		{
			self.XilianBagInfo = bagInfo;
			for (int i = 0; i < self.EquipUIList.Count; i++)
			{
				self.EquipUIList[i].SetSelected(bagInfo);
			}
			self.OnUpdateXinLian();
		}

		public static async ETTask InitSubItemUI(this UIRoleXiLianInheritComponent self)
		{
			var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
			var bundleGameObject = await ResourcesComponent.Instance.LoadAssetAsync<GameObject>(path);

			GameObject go_1 = GameObject.Instantiate(bundleGameObject);
			UICommonHelper.SetParent(go_1, self.UIXiLianItemNode);
			self.XiLianItemUI = self.AddChild<UIItemComponent, GameObject>(go_1);
			self.XiLianItemUI.Label_ItemName.SetActive(true);

			GameObject go_2 = GameObject.Instantiate(bundleGameObject);
			UICommonHelper.SetParent(go_2, self.CostItem);
			self.CostItemUI = self.AddChild<UIItemComponent, GameObject>(go_2);
			self.CostItemUI.Label_ItemNum.SetActive(false);
			self.CostItemUI.Label_ItemName.SetActive(false);

			BagInfo bagInfo = self.XilianBagInfo;
			self.CostItem.SetActive(bagInfo != null);
			if (bagInfo != null)
			{
				self.XiLianItemUI.UpdateItem(bagInfo, ItemOperateEnum.None);
			}
		}

		public static async ETTask OnXiLianButton(this UIRoleXiLianInheritComponent self, int times)
		{
			BagInfo bagInfo = self.XilianBagInfo;
			if (bagInfo == null)
			{
				return;
			}

			int maxInheritTimes = GlobalValueConfigCategory.Instance.Get(117).Value2;
			if (bagInfo.InheritTimes >= maxInheritTimes)
			{
				FloatTipManager.Instance.ShowFloatTip("该装备不可再进行传承鉴定！");
				return;
			}

			ItemConfig itemConfig = ItemConfigCategory.Instance.Get(bagInfo.ItemID);
			string costitem = ItemHelper.GetInheritCost(bagInfo.InheritTimes);
            if (!self.BagComponent.CheckNeedItem(costitem))
			{
				FloatTipManager.Instance.ShowFloatTip("材料不足！");
				return;
			}

			C2M_ItemInheritRequest c2M_ItemHuiShouRequest = new C2M_ItemInheritRequest() { OperateBagID = bagInfo.BagInfoID  };
			M2C_ItemInheritResponse r2c_roleEquip = (M2C_ItemInheritResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_ItemHuiShouRequest);
			if (r2c_roleEquip.Error != 0)
			{
				return;
			}
			
			self.InheritSkills = r2c_roleEquip.InheritSkills;
			int skillid = r2c_roleEquip.InheritSkills[0];
			SkillConfig skillConfig = SkillConfigCategory.Instance.Get(skillid);
			// 二次确认框
			PopupTipHelp.OpenPopupTip( self.DomainScene(), "传承鉴定", $"传承鉴定效果：{skillConfig.SkillDescribe}\n传承装备只有{maxInheritTimes}次重新鉴定传承的机会\n请问是否覆盖原始传承鉴定效果?", ()=>
			{
				self.RequestInheritSelect().Coroutine();
			}, () => { self.OnXiLianReturn();}).Coroutine();
			
			//self.RequestInheritSelect().Coroutine();

			//提示框
			/*
			PopupTipHelp.OpenPopupTip_2( self.DomainScene(), "传承鉴定", $"传承鉴定效果：{skillConfig.SkillDescribe}\n恭喜你装备传承效果鉴定成功!", ()=>
			{

			}).Coroutine();
			*/
		}


		public static async ETTask RequestInheritSelect(this UIRoleXiLianInheritComponent self)
		{
			BagInfo bagInfo = self.XilianBagInfo;
			if (bagInfo == null)
			{
				return;
			}

			C2M_ItemInheritSelectRequest  request = new C2M_ItemInheritSelectRequest() { OperateBagID = bagInfo.BagInfoID, InheritSkills = self.InheritSkills };
			M2C_ItemInheritSelectResponse response	= (M2C_ItemInheritSelectResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);

			self.OnXiLianReturn();
		}
	}
}
