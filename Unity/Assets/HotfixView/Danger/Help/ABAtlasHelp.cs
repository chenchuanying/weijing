﻿using UnityEngine;
using UnityEngine.U2D;

namespace ET
{

    public  static class ABAtlasTypes
    {
        public const string ItemIcon = "ItemIcon";
        public const string ItemQualityIcon = "ItemQualityIcon";
        public const string PetHeadIcon = "PetHeadIcon";
        public const string PetSkillIcon = "PetSkillIcon";
        public const string RoleSkillIcon = "RoleSkillIcon";
        public const string TianFuIcon = "TianFuIcon";
        public const string ChapterIcon = "ChapterIcon";
        public const string PropertyIcon = "PropertyIcon";
        public const string ChengJiuIcon = "ChengJiuIcon";
        public const string RechageIcon = "RechageIcon";
        public const string MonsterIcon = "MonsterIcon";
        public const string TiTleIcon = "TiTleIcon";
        public const string TaskIcon = "TaskIcon";
        public const string OtherIcon = "OtherIcon";
        public const string PlayerIcon = "PlayerIcon";
        public const string ChengHaoIcon = "ChengHaoIcon";
        public const string MulLanguageIcon = "MulLanguageIcon";
    }

    public static class ABAtlasHelp
    {

        /// <summary>
        /// 全部修改完之后移除此方法
        /// </summary>
        /// <param name="types"></param>
        /// <param name="icon"></param>
        /// <returns></returns>
        public static Sprite GetIconSprite(string types, string icon)
        {
            var path = ABPathHelper.GetAtlasPath_2(types, icon);
            Sprite prefab =  ResourcesComponent.Instance.LoadAsset<Sprite>(path);
            return prefab;
        }
    }
}
