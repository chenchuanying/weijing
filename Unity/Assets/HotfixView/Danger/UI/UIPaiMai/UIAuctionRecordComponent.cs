﻿using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIAuctionRecordComponent : Entity, IAwake
    {
        public GameObject BuildingList;
        public GameObject UIAuctionRecordItem;
        public GameObject ButtonClose;
    }

    public class UIAuctionRecodeComponentAwake : AwakeSystem<UIAuctionRecordComponent>
    {
        public override void Awake(UIAuctionRecordComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.BuildingList = rc.Get<GameObject>("BuildingList");
            self.UIAuctionRecordItem = rc.Get<GameObject>("UIAuctionRecordItem");
            self.UIAuctionRecordItem.SetActive(false);

            self.ButtonClose = rc.Get<GameObject>("ButtonClose");
            self.ButtonClose.GetComponent<Button>().onClick.AddListener(() => { UIHelper.Remove( self.ZoneScene(), UIType.UIAuctionRecode );  });

            self.OnInitUI().Coroutine();
        }
    }

    public static class UIAuctionRecodeComponentSystem
    {
        public static async ETTask OnInitUI(this UIAuctionRecordComponent self)
        {
            C2P_PaiMaiAuctionRecordRequest request = new C2P_PaiMaiAuctionRecordRequest();
            P2C_PaiMaiAuctionRecordResponse response = (P2C_PaiMaiAuctionRecordResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);

            long instanceid = self.InstanceId;
            if (instanceid != self.InstanceId)
            {
                return;
            }
            for ( int i = 0; i < response.RecordList.Count; i++)
            {
                GameObject gameObject = GameObject.Instantiate(self.UIAuctionRecordItem);
                gameObject.SetActive(true);
                UICommonHelper.SetParent( gameObject, self.BuildingList );
                UIAuctionRecodeItemComponent recodeItemComponent = self.AddChild<UIAuctionRecodeItemComponent, GameObject>(gameObject);
                recodeItemComponent.OnInitUI(response.RecordList[i]);
            }
        }
    }
}
