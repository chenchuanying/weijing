﻿using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIJiaYuanPlanWatchComponent : Entity, IAwake, IDestroy
    {
        public GameObject BtnClose;
        public GameObject Text_Record;
        public GameObject Text_Desc_3;
        public GameObject Text_Desc_2;
        public GameObject Text_Desc_1;

        public GameObject TextStage;
        public GameObject TextName;

        public GameObject RawImage;
        public RenderTexture RenderTexture;
        public UIModelDynamicComponent UIModelShowComponent;

        public UIItemComponent UIGetItem;
    }


    public class UIJiaYuanPlanWatchComponentAwake : AwakeSystem<UIJiaYuanPlanWatchComponent>
    {
        public override void Awake(UIJiaYuanPlanWatchComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();

            self.BtnClose = rc.Get<GameObject>("BtnClose");
            self.BtnClose.GetComponent<Button>().onClick.AddListener(() => { UIHelper.Remove( self.ZoneScene(), UIType.UIJiaYuanPlanWatch ); });

            self.Text_Desc_3 = rc.Get<GameObject>("Text_Desc_3");
            self.Text_Desc_2 = rc.Get<GameObject>("Text_Desc_2");
            self.Text_Desc_1 = rc.Get<GameObject>("Text_Desc_1");

            self.TextStage = rc.Get<GameObject>("TextStage");
            self.TextName = rc.Get<GameObject>("TextName");

            self.RawImage = rc.Get<GameObject>("RawImage");

            self.Text_Record = rc.Get<GameObject>("Text_Record");

            GameObject uIGetItem = rc.Get<GameObject>("UIGetItem");
            self.UIGetItem = self.AddChild<UIItemComponent, GameObject>(uIGetItem);

            self.OnInitUI().Coroutine();
        }
    }


    public class UIJiaYuanPlanWatchComponentDestroy : DestroySystem<UIJiaYuanPlanWatchComponent>
    {
        public override void Destroy(UIJiaYuanPlanWatchComponent self)
        {
            self.UIModelShowComponent.ReleaseRenderTexture();
            self.RenderTexture.Release();
            GameObject.Destroy(self.RenderTexture);
            self.RenderTexture = null;
            //RenderTexture.ReleaseTemporary(self.RenderTexture);
        }
    }

    public static class UIJiaYuanPlanWatchComponentSystem
    {

        public static async ETTask OnInitUI(this UIJiaYuanPlanWatchComponent self)
        {
            UI uI = UIHelper.GetUI(self.ZoneScene(), UIType.UIJiaYuanMain);
            UIJiaYuanMainComponent jiaYuanViewComponent = uI.GetComponent<UIJiaYuanMainComponent>();
            Unit unit = JiaYuanHelper.GetUnitByCellIndex(self.ZoneScene().CurrentScene(), jiaYuanViewComponent.CellIndex);
            if (unit == null)
            {
                return;
            }
            NumericComponent numericComponent = unit.GetComponent<NumericComponent>();
            long StartTime = numericComponent.GetAsLong(NumericType.StartTime);
            int GatherNumber = numericComponent.GetAsInt(NumericType.GatherNumber);
            long GatherLastTime = numericComponent.GetAsLong(NumericType.GatherLastTime);

            JiaYuanFarmConfig jiaYuanFarmConfig = JiaYuanFarmConfigCategory.Instance.Get(unit.ConfigId);
            int stage = JiaYuanHelper.GetPlanStage(unit.ConfigId, StartTime, GatherNumber);
           
            self.RenderTexture = null;
            self.RenderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            self.RenderTexture.Create();
            self.RawImage.GetComponent<RawImage>().texture = self.RenderTexture;

            var path = ABPathHelper.GetUGUIPath("Common/UIModelDynamic");
            GameObject bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            self.UIModelShowComponent = self.AddChild<UIModelDynamicComponent, GameObject>(gameObject);
            self.UIModelShowComponent.OnInitUI(self.RawImage, self.RenderTexture);
            self.UIModelShowComponent.ShowModel($"JiaYuan/{jiaYuanFarmConfig.ModelID + stage}").Coroutine();
            gameObject.transform.Find("Camera").localPosition = new Vector3(1f, 11f, 150f);
            gameObject.transform.Find("Camera").GetComponent<Camera>().fieldOfView = 15;
            gameObject.transform.localPosition = new Vector2(1000, 0);
            gameObject.transform.Find("Model").localRotation = Quaternion.Euler(0f, -45f, 0f);

            self.TextName.GetComponent<Text>().text = jiaYuanFarmConfig.Name;
            self.TextStage.GetComponent<Text>().text = JiaYuanHelper.GetPlanStageName(stage);

            self.Text_Desc_1.GetComponent<Text>().text = $"当前阶段: {JiaYuanHelper.GetPlanStageName(stage)}";

            long nextTime = JiaYuanHelper.GetNextStateTime(unit.ConfigId, StartTime);
            self.Text_Desc_2.GetComponent<Text>().text = $"下一阶段: {  JiaYuanHelper.TimeToShow(TimeInfo.Instance.ToDateTime(nextTime).ToString("f"))}";

            long shouhuoTime =  JiaYuanHelper.GetPlanNextShouHuoTime(unit.ConfigId, StartTime, GatherNumber, GatherLastTime);
            self.Text_Desc_3.GetComponent<Text>().text = $"预计收获: { JiaYuanHelper.TimeToShow( TimeInfo.Instance.ToDateTime(shouhuoTime).ToString("f"))}";

            self.UIGetItem.UpdateItem(new BagInfo() { ItemID = jiaYuanFarmConfig.GetItemID, ItemNum = 1 }, ItemOperateEnum.None);

            JiaYuanComponent jiaYuanComponent = self.ZoneScene().GetComponent<JiaYuanComponent>();
            C2M_JiaYuanWatchRequest c2m_watchWatch = new C2M_JiaYuanWatchRequest() { MasterId = jiaYuanComponent.MasterId , OperateId = unit.Id};
            M2C_JiaYuanWatchResponse m2C_JiaYuanWatch = (M2C_JiaYuanWatchResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(c2m_watchWatch);
            string gatherrecode = string.Empty;
            for (int i = 0; i < m2C_JiaYuanWatch.JiaYuanRecord.Count; i++)
            {
                string gatherTime = TimeInfo.Instance.ToDateTime(m2C_JiaYuanWatch.JiaYuanRecord[i].Time).ToString();
                gatherTime = gatherTime.Substring(5,gatherTime.Length - 5);
                gatherrecode += $"{gatherTime} {m2C_JiaYuanWatch.JiaYuanRecord[i].PlayerName}收获一次 \n";
            }

            self.Text_Record.GetComponent<Text>().text = gatherrecode;
        }
    }
}
