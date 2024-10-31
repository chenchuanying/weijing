using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace ET
{

    public class UIServerShowComponent : Entity, IAwake, IDestroy
    {

        public GameObject ImageSelect;
        public GameObject UIServerShowItem_2;
        public GameObject UIServerShowItem_1;
        public GameObject ServerListNode_2;
        public GameObject ServerListNode_1;
        public GameObject ButtonClose;

        public List<GameObject> ServerItemList_1 = new List<GameObject>();
        public List<GameObject> ServerItemList_2 = new List<GameObject>();

        public Dictionary<string, GameObject> Iptogameobject = new Dictionary<string, GameObject>();    
        public Dictionary<string, List<int>> Iptoservers = new Dictionary<string, List<int>>();    
    }

    public class UIServerShowComponentAwake : AwakeSystem<UIServerShowComponent>
    {
        public override void Awake(UIServerShowComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.ServerItemList_1.Clear();
            self.ServerItemList_2.Clear();

            self.UIServerShowItem_2 = rc.Get<GameObject>("UIServerShowItem_2");
            self.UIServerShowItem_1 = rc.Get<GameObject>("UIServerShowItem_1");
            self.UIServerShowItem_2.SetActive(false);
            self.UIServerShowItem_1.SetActive(false);
            self.ImageSelect = rc.Get<GameObject>("ImageSelect");

            self.ServerListNode_2 = rc.Get<GameObject>("ServerListNode_2");
            self.ServerListNode_1 = rc.Get<GameObject>("ServerListNode_1");

            self.ButtonClose = rc.Get<GameObject>("ButtonClose");
            self.ButtonClose.GetComponent<Button>().onClick.AddListener(() =>
            {
                UIHelper.Remove( self.ZoneScene(), UIType.UIServerShow );
            });

            List <ServerItem> serverItems = ServerHelper.GetServerList(false);

            self.Iptoservers.Clear();
            self.Iptoservers = new Dictionary<string, List<int>>();

            for (int i = 0; i < serverItems.Count; i++)
            {
                if (!self.Iptoservers.ContainsKey(serverItems[i].ServerIp))
                {
                    self.Iptoservers.Add(serverItems[i].ServerIp, new List<int>());
                }

                self.Iptoservers[serverItems[i].ServerIp].Add(serverItems[i].ServerId);
            }

            foreach (var item in self.Iptoservers)
            {
                int serverId = item.Value[0];
                ServerItem serverItem = ServerHelper.GetGetServerItem(false, serverId);
                GameObject item_1 = GameObject.Instantiate(self.UIServerShowItem_1);
                UICommonHelper.SetParent(item_1, self.ServerListNode_1);
                item_1.SetActive(true);
                item_1.transform.Find("Text_ServerName").GetComponent<Text>().text = serverItem.ServerName;
                item_1.transform.Find("ImageButton").GetComponent<Button>().onClick.AddListener(() =>
                {
                    string texxx = item.Key;
                    self.OnClickServerItem(texxx);
                });
                self.Iptogameobject.Add(item.Key, item_1);
            }

        }
    }

    public class UIServerShowComponentDestroy : DestroySystem<UIServerShowComponent>
    {
        public override void Destroy(UIServerShowComponent self)
        {
            self.ServerItemList_1.Clear();
            self.ServerItemList_2.Clear();
        }
    }

    public static class UIServerShowComponentSystem
    {

        public static void OnClickServerItem(this UIServerShowComponent self, string ip)
        {
            List<int> serverids = self.Iptoservers[ip];

            UICommonHelper.DestoryChild(self.ServerListNode_2);

            for (int i = 0; i < serverids.Count; i++)
            {
                int serverId = serverids[i];
                ServerItem serverItem = ServerHelper.GetGetServerItem(false, serverId);
                string openTime = TimeInfo.Instance.ToDateTime(serverItem.ServerOpenTime).ToString();
                string serverName = $"{serverId}  {serverItem.ServerName}  {openTime}";

                GameObject item_2 = GameObject.Instantiate(self.UIServerShowItem_2);
                UICommonHelper.SetParent(item_2, self.ServerListNode_2);
                item_2.SetActive(true);
                item_2.transform.Find("Text_ServerName").GetComponent<Text>().text = serverName;
            }

            UICommonHelper.SetParent( self.ImageSelect, self.Iptogameobject[ip]);
        }

    }
}