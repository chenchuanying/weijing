﻿using UnityEngine;

namespace ET
{

    public class UIBattleEnterComponent : Entity, IAwake
    {
        public GameObject ButtonEnter;
        public GameObject ItemListNode;
    }


    public class UIBattleEnterComponentAwakeSystem : AwakeSystem<UIBattleEnterComponent>
    {
        public override void Awake(UIBattleEnterComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            self.ButtonEnter = rc.Get<GameObject>("ButtonEnter");
            self.ItemListNode = rc.Get<GameObject>("ItemListNode");

            GlobalValueConfig globalValue = GlobalValueConfigCategory.Instance.Get(56);
            UICommonHelper.ShowItemList(globalValue.Value, self.ItemListNode, self, 1f);
            ButtonHelp.AddListenerEx(self.ButtonEnter, () => { self.OnButtonEnter().Coroutine(); } );
        }
    }

    public static class UIBattleEnterComponentSystem
    {
        
        public static async ETTask OnButtonEnter(this UIBattleEnterComponent self)
        {
            int errorCode = await NetHelper.RequstBattleEnter(self.ZoneScene());
            if (errorCode == ErrorCode.ERR_Success)
            {
                UIHelper.Remove(self.ZoneScene(), UIType.UIBattle);
                return;
            }
            if (ErrorHelp.Instance.ErrorHintList.ContainsKey(errorCode))
            {
                HintHelp.GetInstance().ShowHint(ErrorHelp.Instance.ErrorHintList[errorCode]);
            }
        }
    }
}