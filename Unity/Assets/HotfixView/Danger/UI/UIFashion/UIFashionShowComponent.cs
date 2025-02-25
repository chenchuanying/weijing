using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace ET
{

    public class UIFashionShowComponent : Entity, IAwake
    {
        public GameObject RawImage;
        public GameObject Image_Select;
        public GameObject BuildingList;
        public GameObject UIFashionShowItem;
      
        public Dictionary<int, GameObject> ButtonList = new Dictionary<int, GameObject>();
        public List<UIFashionShowItemComponent> FashionItemList = new List<UIFashionShowItemComponent>();

        public UIModelShowComponent UIModelShowComponent;
    }

    public class UIFashionShowComponentAwake : AwakeSystem<UIFashionShowComponent>
    {
        public override void Awake(UIFashionShowComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.Image_Select = rc.Get<GameObject>("Image_Select");

            self.BuildingList = rc.Get<GameObject>("BuildingList");
            self.UIFashionShowItem = rc.Get<GameObject>("UIFashionShowItem");
            self.UIFashionShowItem.SetActive(false);
            self.RawImage = rc.Get<GameObject>("RawImage");
            self.FashionItemList.Clear();
            self.RawImage.gameObject.SetActive(true);

            self.OnInitModelShow();

            int occ = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Occ;
            List<int> keys= new List<int> { 1001, 2001, 3001 };
            for (int i = 0; i < keys.Count; i++)
            {
                int keyid = keys[i];
                GameObject Button_key = rc.Get<GameObject>($"Button_{keyid}");
                Button_key.GetComponent<Button>().onClick.AddListener(() => { self.OnClickSubButton(keyid); });
                self.ButtonList.Add(keyid, Button_key);
            }

            self.OnClickSubButton(keys[0]);
        }
    }

    public static class UIFashionShowComponentSystem
    {

        public static void OnInitModelShow(this UIFashionShowComponent self)
        {
            //模型展示界面
            var path = ABPathHelper.GetUGUIPath("Common/UIModelShow1");
            GameObject bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UICommonHelper.SetParent(gameObject, self.RawImage);
            UI ui = self.AddChild<UI, string, GameObject>("UIModelShow", gameObject);
            self.UIModelShowComponent = ui.AddComponent<UIModelShowComponent, GameObject>(self.RawImage);
            gameObject.transform.localPosition  = new Vector2(2000, 4000);
            //配置摄像机位置[0,115,257]
            gameObject.transform.Find("Camera").localPosition = new Vector3(-20f, 80f, 250f);
            gameObject.transform.Find("Camera").GetComponent<Camera>().fieldOfView = 35;
            gameObject.transform.Find("Camera").GetComponent<Camera>().cullingMask = 1 << 0;
            gameObject.transform.Find("Camera").GetComponent<Camera>().cullingMask = 1 <<11;
        }


        public static void OnFashionWear(this UIFashionShowComponent self)
        {
            int occ = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Occ;
            List<int> fashionids = self.ZoneScene().GetComponent<BagComponent>().FashionEquipList;

            ////////把拼装后的模型显示在RawImages
            BagInfo bagInfo = new BagInfo()
            {
                ItemID = UnitHelper.GetWuqiItemID(self.ZoneScene())
            };
            self.UIModelShowComponent.ShowPlayerPreviewModel(bagInfo, fashionids, occ);

            for (int i = 0; i < self.FashionItemList.Count; i++)
            {
                self.FashionItemList[i].Position = i + 2;
                self.FashionItemList[i].OnUpdateUI(self.FashionItemList[i].FashionId);
            }
        }

        public static void OnFashionPreview(this UIFashionShowComponent self, int fashionid)
        {
            int occ = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Occ;
            List<int> equipids = self.ZoneScene().GetComponent<BagComponent>().FashionEquipList;

            List<int> fashionids = new List<int>() {  };
            fashionids.AddRange(equipids);  

            bool have = false;
            FashionConfig fashionConfig = FashionConfigCategory.Instance.Get(fashionid);
            for(int i = 0; i < fashionids.Count; i++)
            {

                FashionConfig fashionConfig_2 = FashionConfigCategory.Instance.Get(fashionids[i]);
                if (fashionConfig_2.SubType == fashionConfig.SubType)
                {
                    have = true;    
                    fashionids[i] = fashionid;
                    break;
                }
            }
            if (!have)
            {
                fashionids.Add(fashionid);
            }

            ////////把拼装后的模型显示在RawImages
            BagInfo bagInfo = new BagInfo()
            {
                ItemID = UnitHelper.GetWuqiItemID(self.ZoneScene())
            };
            self.UIModelShowComponent.ShowPlayerPreviewModel(bagInfo, fashionids, occ);
        }

        public static void OnClickSubButton(this UIFashionShowComponent self, int subType)
        {
            UICommonHelper.SetParent(self.Image_Select, self.ButtonList[subType]);
            self.Image_Select.transform.SetAsFirstSibling();

            self.OnUpdateFashionList(  subType);

            self.OnFashionWear();
        }

        public static void OnUpdateFashionList(this UIFashionShowComponent self, int subType)
        {
            int occ = self.ZoneScene().GetComponent<UserInfoComponent>().UserInfo.Occ;
            List<int> occfashionids = FashionConfigCategory.Instance.GetOccFashionList( occ, subType );

            for (int i = 0; i < occfashionids.Count; i++)
            {
                UIFashionShowItemComponent uiitem = null;
                if (i < self.FashionItemList.Count)
                {
                    uiitem = self.FashionItemList[i];
                    uiitem.GameObject.SetActive(true);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(self.UIFashionShowItem);
                    go.SetActive(true);
                    UICommonHelper.SetParent(go, self.BuildingList);
                    uiitem = self.AddChild<UIFashionShowItemComponent, GameObject>(go);
                    self.FashionItemList.Add(uiitem);
                }
                self.FashionItemList[i].Position = i + 2;
                uiitem.OnUpdateUI(occfashionids[i]);
                uiitem.FashionWearHandler = self.OnFashionWear;
                uiitem.PreviewHandler = self.OnFashionPreview;
            }

            for (int i = occfashionids.Count; i < self.FashionItemList.Count; i++)
            {
                self.FashionItemList[i].GameObject.SetActive(false);
            }
        }


    }
}