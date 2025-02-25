﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class UIChengJiuJingLingItemComponent : Entity, IAwake<GameObject>, IDestroy
    {
        public GameObject RawImage;
        public GameObject Text_value;
        public GameObject ButtonActivite;
        public GameObject ButtonShouHui;
        public GameObject ObjGetText;
        public GameObject UseSet;
        public GameObject ChengHaoName;
        public GameObject GameObject;
        public GameObject JingLingDes;
        public int JingLingId;

        public RenderTexture RenderTexture;
        public UIModelDynamicComponent UIModelShowComponent;
    }


    public class UIChengJiuJingLingItemComponentAwake : AwakeSystem<UIChengJiuJingLingItemComponent, GameObject>
    {
        public override void Awake(UIChengJiuJingLingItemComponent self, GameObject gameObject)
        {
            self.GameObject = gameObject;

            ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
            self.Text_value = rc.Get<GameObject>("Text_value");

            self.ButtonActivite = rc.Get<GameObject>("ButtonActivite");
            ButtonHelp.AddListenerEx(self.ButtonActivite, () => { self.OnButtonActivite().Coroutine();  });

            self.ButtonShouHui = rc.Get<GameObject>("ButtonShouHui");
            ButtonHelp.AddListenerEx(self.ButtonShouHui, () => { self.OnButtonActivite().Coroutine(); });

            self.RawImage = rc.Get<GameObject>("RawImage");
            self.ObjGetText = rc.Get<GameObject>("ObjGetText");
            self.UseSet = rc.Get<GameObject>("UseSet");
            self.ChengHaoName = rc.Get<GameObject>("ChengHaoName");
            self.JingLingDes = rc.Get<GameObject>("JingLingDes");
        }
    }


    public class UIChengJiuJingLingItemComponentDestroy : DestroySystem<UIChengJiuJingLingItemComponent>
    {
        public override void Destroy(UIChengJiuJingLingItemComponent self)
        {
            self.UIModelShowComponent.ReleaseRenderTexture();
            self.RenderTexture.Release();
            GameObject.Destroy(self.RenderTexture);
            self.RenderTexture = null;
        }
    }

    public static class UIChengJiuJingLingItemComponentSystem
    {

        public static async ETTask OnButtonActivite(this UIChengJiuJingLingItemComponent self)
        {
            ChengJiuComponent chengJiuComponent = self.ZoneScene().GetComponent<ChengJiuComponent>();
            if (!chengJiuComponent.JingLingList.Contains(self.JingLingId))
            {
                FloatTipManager.Instance.ShowFloatTip("请先激活此精灵！");
                return;
            }

            C2M_JingLingUseRequest request = new C2M_JingLingUseRequest() { JingLingId = self.JingLingId, OperateType = 1 };
            M2C_JingLingUseResponse response = (M2C_JingLingUseResponse)await self.ZoneScene().GetComponent<SessionComponent>().Session.Call(request);
            if (response.Error != 0 || self.IsDisposed)
            {
                return;
            }

            chengJiuComponent.JingLingId =  ( response.JingLingId );
            bool current = chengJiuComponent.JingLingId == self.JingLingId;
            self.ButtonShouHui.SetActive(current);
            self.ButtonActivite.SetActive(!current);

            EventType.DataUpdate.Instance.DataType = DataType.JingLingButton;
            EventSystem.Instance.PublishClass(EventType.DataUpdate.Instance);
        }

        public static void OnInitUI(this UIChengJiuJingLingItemComponent self, int jid, bool active)
        {
            JingLingConfig jingLingConfig = JingLingConfigCategory.Instance.Get(jid);
            self.JingLingId = jid;
            self.RenderTexture = null;
            self.RenderTexture = new RenderTexture(256, 256, 16, RenderTextureFormat.ARGB32);
            self.RenderTexture.Create();
            self.RawImage.GetComponent<RawImage>().texture = self.RenderTexture;
            self.ChengHaoName.GetComponent<Text>().text = jingLingConfig.Name;

            var path = ABPathHelper.GetUGUIPath("Common/UIModelDynamic");
            GameObject bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            self.UIModelShowComponent = self.AddChild<UIModelDynamicComponent, GameObject>(gameObject);
            self.UIModelShowComponent.OnInitUI(self.RawImage, self.RenderTexture);
            self.UIModelShowComponent.ShowModel("JingLing/" + jingLingConfig.Assets).Coroutine();
            gameObject.transform.Find("Camera").localPosition = new Vector3(0f, 40f, 200f);
            gameObject.transform.localPosition = new Vector2(jingLingConfig.Id % 10 * 1000, 0);
            gameObject.transform.Find("Model").localRotation = Quaternion.Euler(0f, -45f, 0f);

            self.Text_value.GetComponent<Text>().text = jingLingConfig.Des;
            self.ObjGetText.GetComponent<Text>().text = jingLingConfig.GetDes;
            self.JingLingDes.GetComponent<Text>().text = jingLingConfig.ProDes;
            UICommonHelper.SetRawImageGray(self.RawImage, !active);

            ChengJiuComponent chengJiuComponent = self.ZoneScene().GetComponent<ChengJiuComponent>();
            bool current = chengJiuComponent.JingLingId == jid;
            self.ButtonShouHui.SetActive(current);
            self.ButtonActivite.SetActive(!current);
        }

        public static void OnUpdateUI(this UIChengJiuJingLingItemComponent self)
        { 
            
        }
    }
}
