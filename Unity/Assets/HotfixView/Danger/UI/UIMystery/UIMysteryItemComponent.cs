﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIMysteryItemComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject UIItemNode;
        public GameObject Text_Number;
        public GameObject Text_value;
        public GameObject ButtonBuy;
        public GameObject Image_gold;
        public GameObject GameObject;

        public MysteryItemInfo MysteryItemInfo;
        public UIItemComponent UICommonItem;
        public int NpcId;
        
        public List<string> AssetPath = new List<string>();
    }


    public class UIMysteryItemComponentAwakeSystem : AwakeSystem<UIMysteryItemComponent, GameObject>
    {
        public override void Awake(UIMysteryItemComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;
            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();

            self.UIItemNode = rc.Get<GameObject>("UIItemNode");
            self.Text_Number = rc.Get<GameObject>("Text_Number");
            self.Text_value = rc.Get<GameObject>("Text_value");
            self.Image_gold = rc.Get<GameObject>("Image_gold");

            self.ButtonBuy = rc.Get<GameObject>("ButtonBuy");
            ButtonHelp.AddListenerEx(self.ButtonBuy, () => { self.OnButtonBuy().Coroutine(); });

            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject go = GameObject.Instantiate(bundleGameObject);
            UICommonHelper.SetParent(go, self.UIItemNode);
            self.UICommonItem = self.AddChild<UIItemComponent, GameObject>(go);
            self.UICommonItem.Label_ItemName.SetActive(true);
        }
    }
    public class UIMysteryItemComponentDestroy : DestroySystem<UIMysteryItemComponent>
    {
        public override void Destroy(UIMysteryItemComponent self)
        {
            for(int i = 0; i < self.AssetPath.Count; i++)
            {
                if (!string.IsNullOrEmpty(self.AssetPath[i]))
                {
                    ResourcesComponent.Instance.UnLoadAsset(self.AssetPath[i]); 
                }
            }
            self.AssetPath = null;
        }
    }
    public static class UIMysteryItemComponentSystem
    {

        public static async ETTask OnButtonBuy(this UIMysteryItemComponent self)
        {
            int leftSpace = self.ZoneScene().GetComponent<BagComponent>().GetBagLeftCell();
            if (leftSpace < 1)
            {
                ErrorHelp.Instance.ErrorHint(ErrorCode.ERR_BagIsFull);
                return;
            }

            MysteryConfig mysteryConfig = MysteryConfigCategory.Instance.Get(self.MysteryItemInfo.MysteryId);
            int sellValue = mysteryConfig.SellValue;

            if (mysteryConfig.SellType == 1 && self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Gold < sellValue)
            {
                ErrorHelp.Instance.ErrorHint(ErrorCode.ERR_GoldNotEnoughError);
                return;
            }
            if (mysteryConfig.SellType == 3 && self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Diamond < sellValue)
            {
                ErrorHelp.Instance.ErrorHint(ErrorCode.ERR_DiamondNotEnoughError);
                return;
            }
            if(!self.ZoneScene().GetComponent<BagComponent>().CheckNeedItem($"{mysteryConfig.SellType};{mysteryConfig.SellValue}"))
            {
                ErrorHelp.Instance.ErrorHint(ErrorCode.ERR_ItemNotEnoughError);
                return;
            }

            MysteryItemInfo mysteryItemInfo = new MysteryItemInfo() {  MysteryId = self.MysteryItemInfo.MysteryId };
            C2M_MysteryBuyRequest c2M_MysteryBuyRequest = new C2M_MysteryBuyRequest() { MysteryItemInfo = mysteryItemInfo,  NpcId =  self.NpcId };
            M2C_MysteryBuyResponse r2c_roleEquip = (M2C_MysteryBuyResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(c2M_MysteryBuyRequest);

            UI uI = UIHelper.GetUI( self.DomainScene(), UIType.UIMystery );
            uI.GetComponent<UIMysteryComponent>().RequestMystery().Coroutine();
        }

        public static void OnUpdateUI(this UIMysteryItemComponent self, MysteryItemInfo mysteryItemInfo, int npcId)
        {
            self.NpcId = npcId;

            MysteryConfig mysteryConfig = MysteryConfigCategory.Instance.Get(mysteryItemInfo.MysteryId);
            self.MysteryItemInfo = mysteryItemInfo;
            self.Text_Number.GetComponent<Text>().text = $"剩余 {mysteryItemInfo.ItemNumber}件";
            self.Text_value.GetComponent<Text>().text = mysteryConfig.SellValue.ToString();

            self.UICommonItem.UpdateItem(new BagInfo() { ItemID = self.MysteryItemInfo.ItemID }, ItemOperateEnum.None);
            self.UICommonItem.Label_ItemNum.SetActive(false);

            ItemConfig itemConfig = ItemConfigCategory.Instance.Get(mysteryConfig.SellType);
            string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, itemConfig.Icon);
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.Image_gold.GetComponent<Image>().sprite = sp;
        }

    }
}
