﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace ET
{
    public class UITeamItemComponent : Entity, IAwake, IAwake<GameObject>
    {
        public GameObject RawImage;
        public GameObject TextLevel;
        public GameObject TextName;
        public GameObject Text_Wait_2;
        public GameObject TextCombat;
        public GameObject TextOcc;
        public GameObject RootShowSet;

        public TeamPlayerInfo TeamPlayerInfo;
        public UIModelShowComponent UIModelShowComponent;
    }


    public class UITeamItemComponentAwakeSystem : AwakeSystem<UITeamItemComponent, GameObject>
    {
        public override void Awake(UITeamItemComponent self,  GameObject goParent)
        {
            self.RawImage = goParent.transform.Find("RawImage").gameObject;
            self.TextLevel = goParent.transform.Find("TextLevel").gameObject;
            self.TextName = goParent.transform.Find("TextName").gameObject;
            self.Text_Wait_2 = goParent.transform.Find("Text_Wait_2").gameObject;
            self.TextCombat = goParent.transform.Find("TextCombat").gameObject;
            self.TextOcc = goParent.transform.Find("TextOcc").gameObject;
            self.RootShowSet = goParent.transform.Find("RootShowSet").gameObject;

            self.UIModelShowComponent = null;
        }
    }

    public static class UITeamItemComponentSystem
    {
        public static  void OnInitUI(this UITeamItemComponent self, int index)
        {
            //模型展示界面
            var path = ABPathHelper.GetUGUIPath("Common/UIModelShow" + (index + 1).ToString());
            GameObject bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            UICommonHelper.SetParent(gameObject, self.RawImage);
            gameObject.transform.localPosition = new Vector3(index * 1000, 0, 0);
            gameObject.transform.Find("Camera").localPosition = new Vector3(0f, 55f, 115f);

            UI ui = self.AddChild<UI, string, GameObject>("UIModelShow", gameObject);
            self.UIModelShowComponent = ui.AddComponent<UIModelShowComponent, GameObject>(self.RawImage);
            self.UIModelShowComponent.ClickHandler = () => { self.OnClickTeamItem().Coroutine(); };
            if (self.TeamPlayerInfo != null)
            {
                self.UIModelShowComponent.ShowPlayerModel(new BagInfo() { ItemID = self.TeamPlayerInfo.WeaponId }, self.TeamPlayerInfo.Occ, 0, new List<int>() { });
            }
        }

        public static async ETTask OnClickTeamItem(this UITeamItemComponent self)
        {
            UI uI = await UIHelper.Create(self.DomainScene(), UIType.UIWatchMenu);
            uI.GetComponent<UIWatchMenuComponent>().OnUpdateUI_1(MenuEnumType.Team, self.TeamPlayerInfo.UserID, string.Empty, true).Coroutine();
        }

        public static void OnUpdateItem(this UITeamItemComponent self, TeamPlayerInfo teamPlayerInfo)
        {
            self.RootShowSet.SetActive(false);
            self.TeamPlayerInfo = teamPlayerInfo;

            if (teamPlayerInfo == null)
            {
                self.Text_Wait_2.SetActive(true);
                self.RawImage.SetActive(false);
                self.TextLevel.SetActive(false);
                self.TextName.SetActive(false);
                self.TextCombat.SetActive(false);
                self.TextOcc.SetActive(false);
            }
            else
            {
                self.Text_Wait_2.SetActive(false);
                self.RawImage.SetActive(true);
                self.TextLevel.SetActive(true);
                self.TextName.SetActive(true);
                self.TextCombat.SetActive(true);
                self.TextOcc.SetActive(true);
                self.TextLevel.GetComponent<Text>().text = $"{teamPlayerInfo.PlayerLv} 级";
                self.TextName.GetComponent<Text>().text = teamPlayerInfo.PlayerName;
                self.TextCombat.GetComponent<Text>().text = $"战力: {teamPlayerInfo.Combat}";

                self.TextOcc.SetActive(teamPlayerInfo.Occ!=0 || teamPlayerInfo.OccTwo!=0);
                if (teamPlayerInfo.Occ != 0)
                {
                    self.TextOcc.GetComponent<Text>().text = OccupationConfigCategory.Instance.Get(teamPlayerInfo.Occ).OccupationName;
                }
                if (teamPlayerInfo.OccTwo != 0)
                {
                    self.TextOcc.GetComponent<Text>().text = OccupationTwoConfigCategory.Instance.Get(teamPlayerInfo.OccTwo).OccupationName;
                }

                //机器人显示
                if (teamPlayerInfo.RobotId > 0) {
                    self.RootShowSet.SetActive(true);
                }
            }

            if (teamPlayerInfo != null && self.UIModelShowComponent != null)
            {
                self.UIModelShowComponent.ShowPlayerModel(new BagInfo() { ItemID = teamPlayerInfo.WeaponId}, self.TeamPlayerInfo.Occ, 0, new List<int>() { }    );
            }
        }
    }
}
