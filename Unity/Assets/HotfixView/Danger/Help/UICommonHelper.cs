﻿using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public static class UICommonHelper
    {
        //"Assets/Bundles/Effect/SkillEffect/Eff_Skill_GongJianAct_1.prefab"
        public static List<string> NoUsePool = new List<string>()
        {
            
        };
       
        /// <summary>
        /// 保留两位小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ShowFloatValue_2(float value)
        {
            string svalue = value.ToString("0.##");
            return svalue;
        }

        /// <summary>
        /// 保留两位小数
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ShowFloatValue_1(float value)
        {
            string svalue = value.ToString("0.#");
            return svalue;
        }

        public static void TargetFrameRate(int frame)
        { 
            Application.targetFrameRate = frame;    
        }

        public static string GetNeedItemDesc(string needitems)
        {
            string itemDesc = "";
            string[] needList = needitems.Split('@');

            for (int i = 0; i < needList.Length; i++)
            {
                string[] itemInfo = needList[i].Split(';');
                int itemId = int.Parse(itemInfo[0]);
                int itemNum = int.Parse(itemInfo[1]);
                ItemConfig itemConfig= ItemConfigCategory.Instance.Get(itemId);
                itemDesc += $"{itemConfig.ItemName} x {itemNum} ";
            }
            return itemDesc;
        }

        /// <summary>
        /// PlayerIcon 可以不用释放
        /// </summary>
        /// <param name="go"></param>
        /// <param name="occ"></param>
        public static void ShowOccIcon(GameObject go, int occ)
        {
            occ = occ == 0 ? 1 : occ;
            /// PlayerIcon 可以不用释放
            Sprite sp = ABAtlasHelp.GetIconSprite(ABAtlasTypes.PlayerIcon, occ.ToString());
            go.GetComponent<Image>().sprite = sp;
        }

        public static void ShowItemList(List<RewardItem> rewardItems, GameObject itemNodeList, Entity entity, float scale = 1f, bool showNumber = true, bool showName = false, int getWay = 0)
        {
            rewardItems.Sort(delegate (RewardItem a, RewardItem b)
            {
                int itemIda = a.ItemID;
                int itemIdb = b.ItemID;
                int quliatya = ItemConfigCategory.Instance.Get(itemIda).ItemQuality;
                int quliatyb = ItemConfigCategory.Instance.Get(itemIdb).ItemQuality;
                if (quliatya == quliatyb)
                {
                    if (itemIda == itemIdb)
                        return b.ItemNum - a.ItemNum;
                    else
                        return itemIda - itemIdb;
                }
                else
                {
                    return quliatyb - quliatya;
                }
            });

            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            for (int i = 0; i < rewardItems.Count; i++)
            {
                GameObject itemSpace = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(itemSpace, itemNodeList);
                UIItemComponent uIItemComponent = entity.AddChild<UIItemComponent, GameObject>(itemSpace);
                uIItemComponent.UpdateItem(new BagInfo() { ItemID = rewardItems[i].ItemID, ItemNum = rewardItems[i].ItemNum }, ItemOperateEnum.None);
                uIItemComponent.Label_ItemName.SetActive(showName);
                uIItemComponent.Label_ItemNum.SetActive(showNumber);
                uIItemComponent.Image_Binding.SetActive(getWay == ItemGetWay.Activity_DayTeHui);
                itemSpace.transform.localScale = Vector3.one * scale;
            }
        }

        public static void ShowItemList(string itemList, GameObject itemNodeList, Entity entity, float scale = 1f, bool showNumber = true, int getWay =0)
        {
            if (string.IsNullOrEmpty(itemList))
            {
                return;
            }
            ShowItemList(ItemHelper.GetRewardItems(itemList), itemNodeList,entity, scale,showNumber,  false, getWay);
        }

        public static  void ShowCostItemList(string itemList, GameObject itemNodeList, Entity entity, float scale = 1f)
        {
            if (string.IsNullOrEmpty(itemList))
            {
                return;
            }
            long instanceid = entity.InstanceId;
            var path = ABPathHelper.GetUGUIPath("Main/Common/UICommonCostItem");
            var bundleGameObject = ResourcesComponent.Instance.LoadAsset<GameObject>(path);
            if (instanceid != entity.InstanceId)
            {
                return;
            }
            string[] rewardItems = itemList.Split('@');
            for (int i = 0; i < rewardItems.Length; i++)
            {
                string[] itemInfo = rewardItems[i].Split(';');
                GameObject itemSpace = GameObject.Instantiate(bundleGameObject);
                UICommonHelper.SetParent(itemSpace, itemNodeList);
                UICommonCostItemComponent uIItemComponent = entity.AddChild<UICommonCostItemComponent, GameObject>(itemSpace);
                uIItemComponent.UpdateItem(int.Parse(itemInfo[0]), int.Parse(itemInfo[1]));
                itemSpace.transform.localScale = Vector3.one * scale;
            }
        }

        public static void ShowSkillItem(GameObject itemObj, GameObject itemParent, Entity entity, int[] skills, string aBAtlas)
        {
            for (int i = 0; i < skills.Length; i++)
            {
                GameObject skillItem = GameObject.Instantiate(itemObj);
                UICommonHelper.SetParent(skillItem, itemParent);
                UICommonSkillItemComponent ui_item = entity.AddChild<UICommonSkillItemComponent, GameObject>(skillItem);
                ui_item.OnUpdateUI(skills[i], aBAtlas);
            }
        }

        public static void SetParent( GameObject son, GameObject parent )
        {
            if (son == null || parent == null)
                return;
            son.transform.SetParent(parent.transform);
            son.transform.localPosition = Vector3.zero;
            son.transform.localScale = Vector3.one;
        }

        public static void DestoryChild(GameObject go)
        {
            if (go == null)
                return;

            for (int i = go.transform.childCount - 1; i >= 0; i--)
            {
                GameObject.Destroy(go.transform.GetChild(i).gameObject);
            }
        }

        public static void SetImageGray(GameObject obj, bool val)
        {
            if (val)
            {
                Material mat = ResourcesComponent.Instance.LoadAsset<Material>(ABPathHelper.GetMaterialPath("UI_Hui"));
                obj.GetComponent<Image>().material = mat;
            }
            else
            {
                obj.GetComponent<Image>().material = null;
            }
        }

        public static void SetRawImageGray(GameObject obj, bool val)
        {
            if (val)
            {
                Material mat = ResourcesComponent.Instance.LoadAsset<Material>(ABPathHelper.GetMaterialPath("UI_Hui"));
                obj.GetComponent<RawImage>().material = mat;
            }
            else
            {
                obj.GetComponent<RawImage>().material = null;
            }
        }

        //传入值显示名称
        public static string GetProName(int proID) {

            if (proID >= 10000) {
                proID = (int)(proID / 100);
            }

            string returnName = "";

            switch (proID) {

                case 1002:
                    returnName = "血量";
                    break;
                case 1003:
                    returnName = "最低攻击";
                    break;
                case 1004:
                    returnName = "最高攻击";
                    break;
                case 1005:
                    returnName = "最低防御";
                    break;
                case 1006:
                    returnName = "最高防御";
                    break;
                case 1007:
                    returnName = "最低魔防";
                    break;
                case 1008:
                    returnName = "最高魔防";
                    break;

                case 1051:
                    returnName = "力量";
                    break;

                case 1052:
                    returnName = "敏捷";
                    break;

                case 1053:
                    returnName = "智力";
                    break;

                case 1054:
                    returnName = "耐力";
                    break;

                case 1055:
                    returnName = "体质";
                    break;
            }
            return returnName;

        }

        //传入宠物品质显示文字
        public static string GetPetQualityName(int quality) {

            switch (quality) {

                case 1:
                    return "大众";
                    //break;
                case 2:
                    return "优秀";
                    //break;
                case 3:
                    return "百里挑一";
                    //break;
                case 4:
                    return "千载难逢";
                    //break;
                case 5:
                    return "万众瞩目";
                    //break;
            }

            return "";

        }

        //根据品质返回一个Color
        public static Color QualityReturnColor(int ItenQuality)
        {
            Color color = new Color(1, 1, 1);
            switch (ItenQuality)
            {
                case 1:
                    color = new Color(1, 1, 1);
                    break;

                case 2:
                    color = new Color(0, 1, 0);
                    break;
                case 3:
                    color = new Color(0.047f, 0.76f, 0.847f);
                    break;

                case 4:
                    color = new Color(0.937f, 0.5f, 1.0f);
                    break;
                case 5:
                    color = new Color(1, 0.49f, 0);
                    break;
                case 6:
                    color = new Color(0.80f, 0.49f, 0.19f);
                    break;
            }
            return color;
        }

        public static void DOScale(Transform transform, Vector3 vector3, float time)
        {
            //transform.DOScale(new Vector3(1f, 1f, 1f), time / 2).SetEase(Ease.OutCubic).SetDelay(time / 2);
            transform.DOScale(vector3, time).SetEase(Ease.OutCubic);  //.SetDelay(time / 2);
        }

        public static  void DOLocalMove2(Transform transform, Vector3 vector3, float totalTime)
        {
            transform.DOLocalMove(vector3, totalTime).SetEase(Ease.OutCubic);
        }

        public static async ETTask DOLocalMove(Transform transform, Vector3 vector3, float totalTime)
        {
            Vector3 oldPostition = transform.localPosition;
            float passTime = 0;
            float starTime = Time.time;
            while(passTime < totalTime)
            {
                passTime = Time.time - starTime;
                float rate = passTime / totalTime ;
                Vector3 curPostion = rate * (vector3 - oldPostition) + oldPostition;
                transform.transform.localPosition = curPostion;
                await TimerComponent.Instance.WaitFrameAsync();
                if (transform == null)
                {
                    break;
                }
            }
        }

        public static void CrossFadeAlpha(Transform transform, float alpha, float duration)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.GetComponent<Text>()!=null)
                {
                    child.GetComponent<Text>().CrossFadeAlpha(alpha, duration, false);
                }
                if (transform.GetComponent<Image>() != null)
                {
                    child.GetComponent<Image>().CrossFadeAlpha(alpha, duration, false);
                }
            }
        }

        //数字转换万
        public static string NumToWString(long num) {

            //超过10万才显示
            if (num >= 100000)
            {
                if (num % 10000 == 0)
                {
                    return (num / 10000).ToString() + "万";
                }
                else {
                    return ((float)num / 10000f).ToString("F2") + "万";
                }
            }
            else {
                return num.ToString();
            }

        }


    }
}
