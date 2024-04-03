using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIWeiJingShopComponent : Entity, IAwake
    {

        public int SellId;
        public GameObject ButtonBuy;
        public GameObject Btn_Close;
        public GameObject ItemListNode;
        public List<UIStoreItemComponent> SellList = new List<UIStoreItemComponent>();

        public GameObject Btn_BuyNum_jian10;
        public GameObject Btn_BuyNum_jian1;
        public GameObject Btn_BuyNum_jia10;
        public GameObject Btn_BuyNum_jia1;

        public GameObject Lab_RmbNum;
        public GameObject Lab_Num;
        public GameObject UITowerShopItem;
    }


    public class UIWeiJingShopComponentAwake : AwakeSystem<UIWeiJingShopComponent>
    {

        public override void Awake(UIWeiJingShopComponent self)
        {
            self.SellId = 0;
            self.SellList.Clear();
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.ItemListNode = rc.Get<GameObject>("ItemListNode");

            self.ButtonBuy = rc.Get<GameObject>("ButtonBuy");
            ButtonHelp.AddListenerEx(self.ButtonBuy, self.OnButtonBuy);

            self.Lab_RmbNum = rc.Get<GameObject>("Lab_RmbNum");
            self.Lab_Num = rc.Get<GameObject>("Lab_Num");
            self.UITowerShopItem = rc.Get<GameObject>("UITowerShopItem");
            self.UITowerShopItem.SetActive(false);
            self.Btn_BuyNum_jian10 = rc.Get<GameObject>("Btn_BuyNum_jian10");
            self.Btn_BuyNum_jian10.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_BuyNum_jia(-10); });
            self.Btn_BuyNum_jian1 = rc.Get<GameObject>("Btn_BuyNum_jian1");
            self.Btn_BuyNum_jian1.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_BuyNum_jia(-1); });
            self.Btn_BuyNum_jia10 = rc.Get<GameObject>("Btn_BuyNum_jia10");
            self.Btn_BuyNum_jia10.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_BuyNum_jia(10); });
            self.Btn_BuyNum_jia1 = rc.Get<GameObject>("Btn_BuyNum_jia1");
            self.Btn_BuyNum_jia1.GetComponent<Button>().onClick.AddListener(() => { self.OnBtn_BuyNum_jia(1); });

            self.Btn_Close = rc.Get<GameObject>("Btn_Close");
            self.Btn_Close.GetComponent<Button>().onClick.AddListener(() => { UIHelper.Remove( self.ZoneScene(), UIType.UIWeiJingShop ); });

            self.GetParent<UI>().OnUpdateUI = self.OnUpdateUI;
            self.OnInitUI();

            //默认购买数量为1
            self.Lab_RmbNum.GetComponent<InputField>().text = "1";
        }
    }

    public static class UIWeiJingShopComponentSystem
    {

        public static async void OnButtonBuy(this UIWeiJingShopComponent self)
        {
            if (self.SellId == 0)
            {
                FloatTipManager.Instance.ShowFloatTip("请选择道具！");
                return;
            }
            int buyNum = int.Parse(self.Lab_RmbNum.GetComponent<InputField>().text);
            await self.ZoneScene().GetComponent<BagComponent>().SendBuyItem(self.SellId, buyNum);
            self.OnUpdateNumShow();
            for (int i = 0; i < self.SellList.Count; i++)
            {
                self.SellList[i].UpdateLeftNumber();
            }
        }

        public static void OnUpdateUI(this UIWeiJingShopComponent self)
        {
            self.OnClickHandler(0);
        }

        public static void OnClickHandler(this UIWeiJingShopComponent self, int sellId)
        {
            self.SellId = sellId;
            for (int i = 0; i < self.SellList.Count; i++)
            {
                self.SellList[i].SetSelected(sellId);
            }
        }

        public static void OnInitUI(this UIWeiJingShopComponent self)
        {
            int shopSellid = 91000001;
            int playLv = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Lv;
            while (shopSellid != 0)
            {
                StoreSellConfig storeSellConfig = StoreSellConfigCategory.Instance.Get(shopSellid);
                shopSellid = storeSellConfig.NextID;
                if (playLv < storeSellConfig.ShowRoleLvMin || playLv > storeSellConfig.ShowRoleLvMax)
                {
                    continue;
                }

                GameObject storeItem = GameObject.Instantiate(self.UITowerShopItem);
                storeItem.SetActive(true);
                UICommonHelper.SetParent(storeItem, self.ItemListNode);
                UIStoreItemComponent uIItemComponent = self.AddChild<UIStoreItemComponent, GameObject>(storeItem);
                uIItemComponent.OnUpdateData(storeSellConfig);
                uIItemComponent.SetClickHandler(self.OnClickHandler);
                self.SellList.Add(uIItemComponent);
            }

            //获取道具数量进行显示
            self.OnUpdateNumShow();
        }

        public static void OnUpdateNumShow(this UIWeiJingShopComponent self)
        {
            //获取道具数量进行显示
            self.Lab_Num.GetComponent<Text>().text = "当前拥有数量:" + self.ZoneScene().GetComponent<BagComponent>().GetItemNumber(36);
        }

        public static void OnBtn_BuyNum_jia(this UIWeiJingShopComponent self, int num)
        {

            long diamondsNumber = long.Parse(self.Lab_RmbNum.GetComponent<InputField>().text);

            if (num > 0 && diamondsNumber >= 100)
            {
                FloatTipManager.Instance.ShowFloatTip("购买最多100个！");
                return;
            }

            diamondsNumber += num;
            if (diamondsNumber < 1)
                diamondsNumber = 1;
            //单次兑换最多100
            if (diamondsNumber > 100)
            {
                diamondsNumber = 100;
            }

            self.Lab_RmbNum.GetComponent<InputField>().text = diamondsNumber.ToString();
        }
    }
}
