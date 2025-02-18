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
        public GameObject ButtonChage;
        public int SelectOcc;
    }

    public class UIItemChangeOccComponentAwake : AwakeSystem<UIItemChangeOccComponent>
    {
        public override void Awake(UIItemChangeOccComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.GridList = rc.Get<GameObject>("GridList");

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
            int occnumber = self.GridList.transform.childCount;
          
            for (int i = 0; i < occnumber; i++)
            {
                Transform occitem = self.GridList.transform.GetChild(i);
                occitem.Find("Image_XuanZhong").gameObject.SetActive(i == index);
            }
        }

        public static async ETTask OnButtonChange(this UIItemChangeOccComponent self)
        {
            await ETTask.CompletedTask;
        }

    }
}