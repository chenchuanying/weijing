﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

namespace ET
{
    public class UIFriendBlackComponent : Entity, IAwake
    {
        public GameObject FriendNodeList;
        public GameObject UIFriendBlackItem;
        public FriendComponent FriendComponent;
    }


    public class UIFriendBlackComponentAwakeSystem : AwakeSystem<UIFriendBlackComponent>
    {
        public override void Awake(UIFriendBlackComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.FriendNodeList = rc.Get<GameObject>("FriendNodeList");
            self.UIFriendBlackItem = rc.Get<GameObject>("UIFriendBlackItem");
            self.UIFriendBlackItem.SetActive(false);
            self.FriendComponent = self.ZoneScene().GetComponent<FriendComponent>();

            self.OnUpdateFriendList();
        }
    }

    public static class UIFriendBlackComponentSystem
    {
        public static  void OnUpdateFriendList(this UIFriendBlackComponent self)
        {
            self.FriendNodeList.GetComponent<RectTransform>().sizeDelta = new Vector2(0, self.FriendComponent.FriendList.Count * 210 + 20);


            List<Entity> childs = self.Children.Values.ToList();
            for (int i = 0; i < self.FriendComponent.Blacklist.Count; i++)
            {
                UIFriendBlackItemComponent uI_1;
                if (i < childs.Count)
                {
                    uI_1 = childs[i] as UIFriendBlackItemComponent;
                    uI_1.GameObject.SetActive(true);
                }
                else
                {
                    GameObject go = GameObject.Instantiate(self.UIFriendBlackItem);
                    go.SetActive(true);
                    UICommonHelper.SetParent(go, self.FriendNodeList);
                    uI_1 = self.AddChild<UIFriendBlackItemComponent, GameObject>(go);
                }
                uI_1.OnUpdateUI(self.FriendComponent.Blacklist[i]);
            }
            for (int i = self.FriendComponent.Blacklist.Count; i < childs.Count; i++)
            {
                (childs[i] as UIFriendBlackItemComponent).GameObject.SetActive(false);
            }
        }
    }
}
