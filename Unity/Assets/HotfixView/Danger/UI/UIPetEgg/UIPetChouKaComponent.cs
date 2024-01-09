﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIPetChouKaComponent : Entity, IAwake, IDestroy
    {

        public GameObject Text_ChouKaNumber;
        public GameObject CostItem;
        public GameObject Text_DiamondNumber;
        public GameObject Btn_ChouKaDiamonds;
        public GameObject Btn_ChouKaCoin;
        public GameObject Btn_ChouKaPro;
        public GameObject ItemImageIcon;
        public GameObject Text_CoinNumber;
        public PetComponent PetComponent;
        
        public List<string> AssetPath = new List<string>();
    }


    public class UIPetChouKaComponentAwakeSystem : AwakeSystem<UIPetChouKaComponent>
    {
        public override void Awake(UIPetChouKaComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.ItemImageIcon = rc.Get<GameObject>("ItemImageIcon");
            self.Text_CoinNumber = rc.Get<GameObject>("Text_CoinNumber");
            self.Text_ChouKaNumber = rc.Get<GameObject>("Text_ChouKaNumber");
            //self.CostItem.SetActive(false);

            self.Text_DiamondNumber = rc.Get<GameObject>("Text_DiamondNumber");
            self.Btn_ChouKaDiamonds = rc.Get<GameObject>("Btn_ChouKaDiamonds");
            ButtonHelp.AddListenerEx( self.Btn_ChouKaDiamonds, ()=> { self.OnBtn_ChouKa(2).Coroutine(); }  );
            self.Btn_ChouKaCoin = rc.Get<GameObject>("Btn_ChouKaCoin");
            ButtonHelp.AddListenerEx(self.Btn_ChouKaCoin, () => { self.OnBtn_ChouKa(1).Coroutine(); } );
            self.Btn_ChouKaPro = rc.Get<GameObject>("Btn_ChouKaPro");
            ButtonHelp.AddListenerEx(self.Btn_ChouKaPro, () => { self.BtnChouKaProHint(); });

            self.PetComponent = self.ZoneScene().GetComponent<PetComponent>();

            self.UpdateMoney();
            self.UpdateChouKaTime();
        }
    }
    public class UIPetChouKaComponentDestroy : DestroySystem<UIPetChouKaComponent>
    {
        public override void Destroy(UIPetChouKaComponent self)
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
    public static class UIPetChouKaComponentSystem
    {
        public static void UpdateChouKaTime(this UIPetChouKaComponent self)
        {
            Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            int leftTime = 20 - unit.GetComponent<NumericComponent>().GetAsInt(NumericType.PetChouKa);
            self.Text_ChouKaNumber.GetComponent<Text>().text = $"(兑换次数: {leftTime}/20)";
        }

        public static void UpdateMoney(this UIPetChouKaComponent self)
        {
            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;

            int needDimanond = int.Parse(GlobalValueConfigCategory.Instance.Get(17).Value);
            self.Text_DiamondNumber.GetComponent<Text>().text = needDimanond.ToString(); //  $"{needDimanond}/{userInfo.Diamond}";
   
            string[] itemInfo = GlobalValueConfigCategory.Instance.Get(16).Value.Split(';');
            string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, ItemConfigCategory.Instance.Get(int.Parse(itemInfo[0])).Icon);
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.ItemImageIcon.GetComponent<Image>().sprite = sp;
            self.Text_CoinNumber.GetComponent<Text>().text = self.ZoneScene().GetComponent<BagComponent>().GetItemNumber(int.Parse(itemInfo[0])) + "/" + itemInfo[1];
            //self.ItemNodeList.SetActive(false);
            //self.ItemNodeList.SetActive(true);
        }

        public static async ETTask OnBtn_ChouKa(this UIPetChouKaComponent self, int choukaType)
        {
            string needItems = GlobalValueConfigCategory.Instance.Get(16).Value;
            if (choukaType == 1 && !self.ZoneScene().GetComponent<BagComponent>().CheckNeedItem(needItems) )
            {
                ErrorHelp.Instance.ErrorHint(ErrorCode.ERR_ItemNotEnoughError);
                return;
            }

            int needDimanond = int.Parse(GlobalValueConfigCategory.Instance.Get(17).Value);
            UserInfo userInfo = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo;
            if (choukaType == 2 && userInfo.Diamond < needDimanond)
            {
                ErrorHelp.Instance.ErrorHint(ErrorCode.ERR_DiamondNotEnoughError);
                return;
            }

            PetComponent petComponent = self.ZoneScene().GetComponent<PetComponent>();
            if (petComponent.RolePetBag.Count >= GlobalValueConfigCategory.Instance.Get(119).Value2)
            {
                FloatTipManager.Instance.ShowFloatTip("请及时清理探索宠物仓库！");
                return;
            }
            // Unit unit = UnitHelper.GetMyUnitFromZoneScene(self.ZoneScene());
            // int maxNum = PetHelper.GetPetMaxNumber(unit, userInfo.Lv);
            // if (PetHelper.GetBagPetNum(self.PetComponent.RolePetInfos) >= maxNum)
            // {
            //     FloatTipManager.Instance.ShowFloatTip("已达到宠物最大数量");
            //     return;
            // }
            /*
            int leftTime = 20 - unit.GetComponent<NumericComponent>().GetAsInt(NumericType.PetChouKa);
            if (choukaType == 2 && leftTime<=0)
            {
                FloatTipManager.Instance.ShowFloatTip("已达到钻石抽卡最大次数");
                return;
            }
            */
            C2M_RolePetChouKaRequest m_ItemOperateWear = new C2M_RolePetChouKaRequest() {  ChouKaType = choukaType };
            M2C_RolePetChouKaResponse r2c_roleEquip = (M2C_RolePetChouKaResponse)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(m_ItemOperateWear);
            if (r2c_roleEquip.Error != 0)
            {
                return;
            }
            self.UpdateMoney();

            //记录tap数据
            AccountInfoComponent accountInfoComponent = self.ZoneScene().GetComponent<AccountInfoComponent>();
            string serverName = ServerHelper.GetGetServerItem(!GlobalHelp.IsOutNetMode, accountInfoComponent.ServerId).ServerName;
#if UNITY_ANDROID
            if (choukaType == 1)
            {
                TapSDKHelper.UpLoadPlayEvent(userInfo.Name, serverName, userInfo.Lv, 3, 1);
            }
            if (choukaType == 2)
            {
                TapSDKHelper.UpLoadPlayEvent(userInfo.Name, serverName, userInfo.Lv, 3, 10);
            }
#endif
        }

        public static void BtnChouKaProHint(this UIPetChouKaComponent self)
        {
            PopupTipHelp.OpenPopupTip_2(self.DomainScene(), "概率提示", "大众 55% 优秀 25 % 百里挑一 14 % 千载难逢 5 % 万里挑一 1 % ", null).Coroutine();
        }
    }



}