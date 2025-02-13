namespace ET
{
    /// <summary>
    /// AB实用函数集，主要是路径拼接
    /// </summary>
    public class ABPathHelper
    {
        public static string GetMaterialPath(string fileName)
        {
            return $"Assets/Bundles/Material/{fileName}.mat";
        }

        public static string GetTexturePath(string fileName)
        {
            return $"Assets/Bundles/Altas/{fileName}.prefab";
        }

        public static string GetUGUIPath(string name)
        {
            return $"Assets/Bundles/UI/{name}.prefab";
        }

        public static string GetConfigPath(string fileName)
        {
            return $"Assets/Bundles/Config/{fileName}.bytes";
        }
        public static string GetNormalConfigPath(string fileName)
        {
            return $"Assets/Bundles/Independent/{fileName}.prefab";
        }

        public static string GetAudioPath(string fileName)
        {
            return $"Assets/Bundles/Audio/{fileName}.mp3";
        }

        public static string GetAudioOggPath(string fileName)
        {
            return $"Assets/Bundles/Audio/{fileName}.ogg";
        }

        public static string GetSoundPath(string fileName)
        {
            return $"Assets/Bundles/Sound/{fileName}.prefab";
        }

        public static string GetUnitPath(string fileName)
        {
            return $"Assets/Bundles/Unit/{fileName}.prefab";
        }

        public static string GetItemPath(string fileName)
        {
            return $"Assets/Bundles/Unit/ItemModel/{fileName}.prefab";
        }

        public static string GetScenePath(string fileName)
        {
            return $"Assets/Scenes/{fileName}.unity";
        }

        public static string GetEffetPath(string fileName)
        {
            return $"Assets/Bundles/Effect/{fileName}.prefab";
        }

        //技能特效
        public static string GetSkillEffetPath(string fileName)
        {
            return $"Assets/Bundles/Effect/SkillEffect/{fileName}.prefab";
        }

        //技能受击特效
        public static string GetSkillHitEffetPath(string fileName)
        {
            return $"Assets/Bundles/Effect/SkillHitEffect/{fileName}.prefab";
        }

        //图集2
        public static string GetAtlasPath_2( string path, string name)
        {
            return $"Assets/Bundles/Icon/{path}/{name}.png";
        }

        //图集
        public static string GetAtlasPath(string path)
        {
            return $"Assets/Bundles/Atlas/{path}.prefab";
        }

        /* 加载Loding图
         var path = ABPathHelper.GetJpgPath(loadResName);
         Sprite atlas =ResourcesComponent.Instance.LoadAsset<Sprite>(path);
         self.Img_BackIcon.GetComponent<Image>().sprite = atlas;
         */
        //Png
        public static string GetJpgPath(string path)
        {
            return $"Assets/Bundles/Jpg/{path}.jpg";
        }

        //文本
        public static string GetTextPath(string text)
        {
            return $"Assets/Bundles/Text/{text}.txt";
        }

        public static string GetRecastPath(string text)
        {
            return $"Assets/Bundles/Recast/{text}.bytes";
        }
    }
}