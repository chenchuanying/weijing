// <auto-generated />
//
// To parse this JSON data, add NuGet 'LC.Newtonsoft.Json' then do:
//
//    using Localization.AntiAddiction;
//
//    var antiAddictionLocalizationItems = AntiAddictionLocalizationItems.FromJson(jsonString);

namespace TapTap.UI.Localization.AntiAddiction
{
    using System;
    using System.Collections.Generic;

    using System.Globalization;
    using LC.Newtonsoft.Json;
    using LC.Newtonsoft.Json.Converters;
	using TapTap.UI;

    public partial class AntiAddictionLocalizationItems
    {
        [JsonProperty("items")]
        public Items Items { get; set; }
		public Item Current
		{
			get
			{
				switch (LocalizationMgr.Instance.CurrentLanguageType)
				{
					case ELanguageType.cn:
						return this.Items.Cn;
					case ELanguageType.en:
						return this.Items.En;
					default:
						return this.Items.Cn;
				}
			}
		}
		public const string PATH = "Config/AntiAddictionLocalization";
    }

    public partial class Items
    {
        [JsonProperty("cn")]
        public Item Cn { get; set; }

        [JsonProperty("en")]
        public Item En { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("NetError")]
        public string NetError { get; set; }

        [JsonProperty("NoVerification")]
        public string NoVerification { get; set; }

        [JsonProperty("EnterGame")]
        public string EnterGame { get; set; }

        [JsonProperty("ExitGame")]
        public string ExitGame { get; set; }

        [JsonProperty("Retry")]
        public string Retry { get; set; }
    }

    public partial class AntiAddictionLocalizationItems
    {
        public static AntiAddictionLocalizationItems FromJson(string json) => JsonConvert.DeserializeObject<AntiAddictionLocalizationItems>(json, Localization.AntiAddiction.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this AntiAddictionLocalizationItems self) => JsonConvert.SerializeObject(self, Localization.AntiAddiction.Converter.Settings);
    }

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }
}
