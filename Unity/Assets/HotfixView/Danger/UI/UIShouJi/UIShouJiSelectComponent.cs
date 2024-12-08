using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ET
{

    public class UIShouJiSelectComponent : Entity, IAwake
    {
        public GameObject ButtonClose;
        public GameObject BuildingList;
        public GameObject ButtonTunShi;

        public List<UIItemComponent> UIItems = new List<UIItemComponent> ();
        public ShoujiComponent ShoujiComponent;
        public int ShouJIId;
        public Action UpdateRedDotAction;
    }


    public class UIShouJiSelectEventComponentAwake : AwakeSystem<UIShouJiSelectComponent>
    {
        public override void Awake(UIShouJiSelectComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.ButtonTunShi = rc.Get<GameObject>("ButtonTunShi");
            ButtonHelp.AddListenerEx(self.ButtonTunShi, self.OnButtonTunShi);

            self.ButtonClose = rc.Get<GameObject>("ButtonClose");
            self.ButtonClose.GetComponent<Button>().onClick.AddListener(() =>
            {
                UIHelper.Remove(self.ZoneScene(), UIType.UIShouJiSelect);
            });

            self.BuildingList = rc.Get<GameObject>("BuildingList");

            self.ShoujiComponent = self.ZoneScene().GetComponent<ShoujiComponent>();
        }
    }

    public static class UIShouJiSelectEventSystem
    {
        public static void OnInitUI(this UIShouJiSelectComponent self, int shouiId,Action updateRedDotAction)
        {
            self.UpdateRedDotAction = updateRedDotAction;
            
            self.ShouJIId = shouiId;
            ShouJiItemConfig shouJiItemConfig = ShouJiItemConfigCategory.Instance.Get(shouiId);
            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);

            BagComponent bagComponent = self.ZoneScene().GetComponent<BagComponent>();
            List<BagInfo> allInfos = (bagComponent.GetBagList());

            for (int i = 0; i < allInfos.Count; i++)
            {
                if (allInfos[i].ItemID != shouJiItemConfig.ItemID)
                {
                    continue;
                }

                GameObject go = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(go, self.BuildingList);
                go.transform.localScale = Vector3.one;

                UIItemComponent uI_1 = self.AddChild<UIItemComponent, GameObject>(go);
                uI_1.SetEventTrigger(false);
                uI_1.SetClickHandler(self.OnSelectItem);
                uI_1.UpdateItem(allInfos[i], ItemOperateEnum.None);
                uI_1.Label_ItemName.SetActive(true);
                uI_1.Image_XuanZhong.SetActive(false);
                self.UIItems.Add(uI_1);
            }
        }

        public static  void OnButtonTunShi(this UIShouJiSelectComponent self)
        {
            KeyValuePairInt keyValuePairInt = self.ShoujiComponent.GetTreasureInfo(self.ShouJIId);
            
            ShouJiItemConfig shouJiItemConfig = ShouJiItemConfigCategory.Instance.Get(self.ShouJIId);
            long number = keyValuePairInt != null ? keyValuePairInt.Value : 0;

            var returnvalue =  self.GetSelectItems();
            List<long> selects = returnvalue.Item1;
            bool havegem = returnvalue.Item2;

            if (selects.Count == 0)
            {
                FloatTipManager.Instance.ShowFloatTip("请选择道具！");
                return;
            }

            if (number + selects.Count > shouJiItemConfig.AcitveNum)
            {
                FloatTipManager.Instance.ShowFloatTip("吞噬数量超出！");
                return;
            }


            if (havegem)
            {
                PopupTipHelp.OpenPopupTip(  self.ZoneScene(), "系统提示", "装备有橙色宝石，是否继续？" , ()=>
                {
                    self.RequestShouJiTreasure(selects, self.ShouJIId).Coroutine();
                }).Coroutine();
                return;
            }

            self.RequestShouJiTreasure(selects, self.ShouJIId).Coroutine();
        }

        private static async ETTask RequestShouJiTreasure(this UIShouJiSelectComponent self, List<long> selects, int shoujiId)
        {
            long instanceId = self.InstanceId;
            C2M_ShouJiTreasureRequest request = new C2M_ShouJiTreasureRequest() { ItemIds = selects, ShouJiId = shoujiId };
            M2C_ShouJiTreasureResponse response = (M2C_ShouJiTreasureResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);

            if (instanceId != self.InstanceId)
            {
                return;
            }
            if (response.Error == ErrorCode.ERR_Success)
            {
                self.ShoujiComponent.OnShouJiTreasure(self.ShouJIId, response.ActiveNum);
            }

            UI uI = UIHelper.GetUI(self.ZoneScene(), UIType.UIShouJi);
            uI.GetComponent<UIShouJiComponent>().OnShouJiTreasure();

            // 更新被选择道具的红点
            self.UpdateRedDotAction.Invoke();

            FloatTipManager.Instance.ShowFloatTip("吞噬道具完成。");

            UIHelper.Remove(self.ZoneScene(), UIType.UIShouJiSelect);
        }

        public static (List<long>, bool) GetSelectItems(this UIShouJiSelectComponent self)
        { 
            List<long> ids =  new List<long>();
            bool havgreengem = false;
            for (int i = 0; i < self.UIItems.Count; i++)
            {
                if (self.UIItems[i].Image_XuanZhong.activeSelf)
                {
                    BagInfo bagInfo = self.UIItems[i].Baginfo;

                    string gemStr = bagInfo.GemIDNew;
                    string[] gem = gemStr.Split('_');
                    for (int j = 0; j < gem.Length; j++)
                    {
                        
                        if (ComHelp.IfNull(gem[j]) )
                        {
                            continue;
                        }

                        ItemConfig gemItemCof = ItemConfigCategory.Instance.Get(int.Parse(gem[j]));
                        if (gemItemCof.ItemSubType == 110)
                        {
                            havgreengem = true;
                        }
                    }

                    ids.Add(bagInfo.BagInfoID);
                }
            }
            return (ids, havgreengem);
        }

        public static void OnSelectItem(this UIShouJiSelectComponent self, BagInfo bagInfo)
        {
            for (int i = 0; i < self.UIItems.Count; i++)
            {
                if (self.UIItems[i].Baginfo.BagInfoID == bagInfo.BagInfoID)
                {
                    bool selected = self.UIItems[i].Image_XuanZhong.activeSelf;
                    self.UIItems[i].Image_XuanZhong.SetActive(!selected);
                }
            }
        }
    }
}