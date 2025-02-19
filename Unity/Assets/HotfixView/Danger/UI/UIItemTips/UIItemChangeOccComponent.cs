using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{

    public class UIItemChangeOccComponent : Entity, IAwake
    {
        public GameObject BtnClose;
        public GameObject GridList;
        public GameObject GridList_2;
        public GameObject ButtonChage;
        public GameObject[] OccTwoSelectList = new GameObject[3];
        public long BagInfoID;
        public int SelectOcc;
        public int SelectOccTwo;
    }

    public class UIItemChangeOccComponentAwake : AwakeSystem<UIItemChangeOccComponent>
    {
        public override void Awake(UIItemChangeOccComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.GridList = rc.Get<GameObject>("GridList");
            self.GridList_2 = rc.Get<GameObject>("GridList_2");
            self.GridList_2.SetActive(false);   
            int occnumber = self.GridList.transform.childCount;
            List<OccupationConfig> occlist = OccupationConfigCategory.Instance.GetAll().Values.ToList();
            for (int i = 0; i < occlist.Count; i++)
            {
                Transform occitem = self.GridList.transform.GetChild(i);
                Transform Image_ItemIcon = occitem.Find("Image_ItemIcon");
                UICommonHelper.ShowOccIcon(Image_ItemIcon.gameObject, i+1);

                int ii = i;
                Image_ItemIcon.GetComponent<Button>().onClick.AddListener(() =>
                {
                    self.OnClickOccItem(ii);
                });  
            }

            self.ButtonChage = rc.Get<GameObject>("ButtonChage");
            ButtonHelp.AddListenerEx(self.ButtonChage, () => { self.OnButtonChange().Coroutine(); });

            self.BtnClose = rc.Get<GameObject>("BtnClose");
            ButtonHelp.AddListenerEx(self.BtnClose, () => { UIHelper.Remove(self.ZoneScene(), UIType.UIItemChangeOcc);  });
        }
    }

    public static class UIItemChangeOccComponentSystem
    {

        public static void OnClickOccItem(this UIItemChangeOccComponent self, int index)
        {
            self.SelectOcc = index + 1;
            self.SelectOccTwo = 0;
            int occnumber = self.GridList.transform.childCount;
          
            for (int i = 0; i < occnumber; i++)
            {
                Transform occitem = self.GridList.transform.GetChild(i);
                occitem.Find("Image_XuanZhong").gameObject.SetActive(i == index);
            }

            OccupationConfig occupationConfig = OccupationConfigCategory.Instance.Get(self.SelectOcc);
            for (int i = 0;i < occupationConfig.OccTwoID.Length; i++)
            {
                OccupationTwoConfig occupationTwoConfig = OccupationTwoConfigCategory.Instance.Get(occupationConfig.OccTwoID[i]);
                Transform occitem = self.GridList_2.transform.GetChild(i);
                occitem.Find("Text").GetComponent<Text>().text = occupationTwoConfig.OccupationName;
                self.OccTwoSelectList[i] = occitem.Find("Image_XuanZhong").gameObject;

                int ii = i;
                occitem.GetComponent<Button>().onClick.RemoveAllListeners();
                occitem.GetComponent<Button>().onClick.AddListener(() =>
                {
                    self.SelectOccTwo = occupationTwoConfig.Id;

                    foreach (var occitem in self.OccTwoSelectList)
                    {
                        occitem.gameObject.SetActive(false);
                    }
                    self.OccTwoSelectList[ii].SetActive(true);  
                    Log.Debug($"self.SelectOccTwo:  {self.SelectOccTwo}");
                });
            }

            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            self.GridList_2.SetActive(userInfoComponent.UserInfo.OccTwo > 0);
        }

        public static async ETTask OnButtonChange(this UIItemChangeOccComponent self)
        {
            if (self.SelectOcc == 0)
            {
                FloatTipManager.Instance.ShowFloatTip("请选择新职业！");
                return;
            }

            UserInfoComponent userInfoComponent = self.ZoneScene().GetComponent<UserInfoComponent>();
            if (self.SelectOcc == userInfoComponent.UserInfo.Occ)
            {
                FloatTipManager.Instance.ShowFloatTip("请选择新职业！");
                return;
            }

            long instanceid = self.InstanceId;
            C2M_ChangeOccRequest c2M_ChangeOcc = new C2M_ChangeOccRequest() {  Occ = self.SelectOcc,  BagInfoID = self.BagInfoID, OccTwo = self.SelectOccTwo };
            M2C_ChangeOccResponse response = (M2C_ChangeOccResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(c2M_ChangeOcc);

            if (instanceid != self.InstanceId)
            {
                return;
            }

            if (response.Error!= ErrorCode.ERR_Success)
            {
                return;
            }

            PopupTipHelp.OpenPopupTip_2(self.ZoneScene(), "重新登录", "请重新登录！", () => {
                EventType.ReturnLogin.Instance.ZoneScene = self.DomainScene();
                EventType.ReturnLogin.Instance.ErrorCode = ErrorCode.ERR_Success;
                Game.EventSystem.PublishClass(EventType.ReturnLogin.Instance);
            }).Coroutine();
        }

    }
}