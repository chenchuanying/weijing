using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

namespace ET
{

    public class UICommonHuoBiSetComponent : Entity, IAwake, IDestroy
    {

        public Text WeiJing_ZiJin;
        public GameObject WeiJingSet;
        public GameObject JiaZuSet;
        public GameObject JiaZu_ZiJin;
        public GameObject ImageZuanShiIcon;
        public GameObject Lab_ZiJin;
        public GameObject ZiJinSet;
        public GameObject Lab_RongYu;
        public GameObject Img_Back_Title;
        public GameObject Lab_ZuanShi;
        public GameObject Lab_Gold;
        public GameObject ButtonClose;
        public GameObject ButtonClose2;
        public GameObject Btn_AddZuanShi;
        public GameObject Btn_AddGold;
        public Image JiaZu_ZijinIcon;
        public Image WeiJing_ZiJinIcon;


        public List<string> AssetPath = new List<string>();
    }


    public class UICommonHuoBiSetComponentAwakeSystem : AwakeSystem<UICommonHuoBiSetComponent>
    {
        public override void Awake(UICommonHuoBiSetComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.Lab_ZuanShi = rc.Get<GameObject>("Lab_ZuanShi");
            self.Lab_Gold = rc.Get<GameObject>("Lab_Gold");
            self.Img_Back_Title = rc.Get<GameObject>("Img_Back_Title");
            self.Lab_RongYu = rc.Get<GameObject>("Lab_RongYu");

            self.JiaZuSet = rc.Get<GameObject>("JiaZuSet");
            self.JiaZu_ZiJin = rc.Get<GameObject>("JiaZu_ZiJin");

            self.ImageZuanShiIcon = rc.Get<GameObject>("ImageZuanShiIcon");
            string path = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, "3");
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            self.ImageZuanShiIcon.GetComponent<Image>().sprite = sp;

            self.JiaZu_ZijinIcon = rc.Get<GameObject>("JiaZu_ZijinIcon").GetComponent<Image>();
            string path_2 = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, "16");
            Sprite sp_2 = ResourcesComponent.Instance.LoadAsset<Sprite>(path_2);
            self.JiaZu_ZijinIcon.sprite = sp_2;
            
            self.WeiJing_ZiJinIcon = rc.Get<GameObject>("WeiJing_ZiJinIcon").GetComponent<Image>();
            string path_3 = ABPathHelper.GetAtlasPath_2(ABAtlasTypes.ItemIcon, "36");
            Sprite sp_3 = ResourcesComponent.Instance.LoadAsset<Sprite>(path_3);
            self.WeiJing_ZiJinIcon.sprite = sp_3;

            self.Btn_AddZuanShi = rc.Get<GameObject>("Btn_AddZuanShi");
            ButtonHelp.AddListenerEx(self.Btn_AddZuanShi, self.OnBtn_AddZuanShi);

            self.Btn_AddGold = rc.Get<GameObject>("Btn_AddGold");
            ButtonHelp.AddListenerEx(self.Btn_AddGold, () => { self.OnBtn_AddGold().Coroutine(); });

            self.ButtonClose = rc.Get<GameObject>("ButtonClose");
            self.ButtonClose.GetComponent<Button>().onClick.AddListener(() => { self.OnButtonClose(); });

            self.ButtonClose2 = rc.Get<GameObject>("ButtonClose_2");
            self.ButtonClose2.GetComponent<Button>().onClick.AddListener(() => { self.OnButtonClose(); });

            self.Lab_ZiJin = rc.Get<GameObject>("Lab_ZiJin");
            self.ZiJinSet = rc.Get<GameObject>("ZiJinSet");

            self.WeiJing_ZiJin = rc.Get<GameObject>("WeiJing_ZiJin").GetComponent<Text>();
            self.WeiJingSet = rc.Get<GameObject>("WeiJingSet");
            self.WeiJingSet.SetActive(false);

            self.InitShow();
            DataUpdateComponent.Instance.AddListener(DataType.UpdateUserData, self);
        }
    }

    public class UICommonHuoBiSetComponentDestroySystem : DestroySystem<UICommonHuoBiSetComponent>
    {
        public override void Destroy(UICommonHuoBiSetComponent self)
        {
            DataUpdateComponent.Instance.RemoveListener(DataType.UpdateUserData, self);

            if (TimerComponent.Instance != null)
            {
                UIHelper.PlayUIMusic("10002");
            }
        }
    }

    public static class UICommonHuoBiSetComponentSystem
    {

        public static void OnUpdateTitle(this UICommonHuoBiSetComponent self, string uiType)
        {
            string[] paths = uiType.Split('/');
            string titlePath = paths[paths.Length - 1];
            if (uiType.Contains("UITeamDungeon"))
            {
                titlePath = "UITeamDungeon";
            }

            string path =ABPathHelper.GetAtlasPath_2(ABAtlasTypes.TiTleIcon, "Img_" + titlePath);
            Sprite sp = ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            if (!self.AssetPath.Contains(path))
            {
                self.AssetPath.Add(path);
            }
            self.Img_Back_Title.GetComponent<Image>().sprite = sp;
        }

        public static void OnBtn_AddZuanShi(this UICommonHuoBiSetComponent self)
        {
            if (UIHelper.GetUI(self.DomainScene(), UIType.UIRecharge) != null)
            {
                return;
            }
            UIHelper.Create(self.DomainScene(), UIType.UIRecharge).Coroutine() ;
        }

        public static async ETTask OnBtn_AddGold(this UICommonHuoBiSetComponent self)
        {
            if (UIHelper.GetUI(self.DomainScene(), UIType.UIPaiMai) != null)
            {
                return;
            }
            UI uI = await UIHelper.Create(self.DomainScene(), UIType.UIPaiMai);
            uI.GetComponent<UIPaiMaiComponent>().UIPageButton.OnSelectIndex(3);
        }

        public static void OnButtonClose(this UICommonHuoBiSetComponent self)
        {
            UIHelper.Remove(self.ZoneScene(), UIType.UIItemTips);
            UIHelper.Remove(self.ZoneScene(), UIType.UIEquipDuiBiTips);

            if (UIHelper.OpenUIList.Count >0 )
            {
                if (UIHelper.OpenUIList[0] == UIType.UISetting)
                {
                    UIHelper.GetUI(self.DomainScene(), UIType.UISetting).GetComponent<UISettingComponent>().OnBeforeClose();
                }
                if (UIHelper.OpenUIList[0] == UIType.UIRole)
                {
                    UIHelper.Remove(self.ZoneScene(), UIType.UIRoleZodiac);
                }
                UIHelper.Remove(self.ZoneScene(), UIType.UIRoleZodiac);
                UIHelper.Remove(self.DomainScene(), UIHelper.OpenUIList[0]);
            }

        }

        public static void InitShow(this UICommonHuoBiSetComponent self)
        {
            self.OnUpdateUI();

            if (UIHelper.OpenUIList.Count > 0)
            {
                self.OnUpdateTitle(UIHelper.OpenUIList[0]);
                self.ZiJinSet.SetActive(UIHelper.OpenUIList[0].Contains("JiaYuan"));
                self.JiaZuSet.SetActive(UIHelper.OpenUIList[0].Contains("UIUnion"));
                self.WeiJingSet.SetActive(UIHelper.OpenUIList[0].Contains("UIPaiMai"));
            }
        }

        public static void OnUpdateUI(this UICommonHuoBiSetComponent self)
        {
            self.UpdataGoldShow();
            self.UpdataRmbShow();
            self.UpdateRongYu();
            self.UpdateZiJin();
            self.UpdateJiaZuZiJin();
            self.UpdateWeiJinZiJin();
        }

        public static void UpdateWeiJinZiJin(this UICommonHuoBiSetComponent self)
        {
            self.WeiJing_ZiJin.text = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.WeiJingGold.ToString();
        }

        public static void UpdateJiaZuZiJin(this UICommonHuoBiSetComponent self)
        {
            self.JiaZu_ZiJin.GetComponent<Text>().text = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.UnionZiJin.ToString();
        }

        public static void UpdateZiJin(this UICommonHuoBiSetComponent self)
        {
            self.Lab_ZiJin.GetComponent<Text>().text = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.JiaYuanFund.ToString();
        }


        public static void UpdateRongYu(this UICommonHuoBiSetComponent self)
        { 
            self.Lab_RongYu.GetComponent<Text>().text = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.RongYu.ToString();
        }

        public static void UpdataGoldShow(this UICommonHuoBiSetComponent self)
        {
            self.Lab_Gold.GetComponent<Text>().text = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Gold.ToString();
        }

        public static void UpdataRmbShow(this UICommonHuoBiSetComponent self)
        {
            self.Lab_ZuanShi.GetComponent<Text>().text = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Diamond.ToString();
        }

    }
}
