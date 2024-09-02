// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		ThirdSDKSettings.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/02 11:16:59
// *******************************************

namespace Bridge.Editor
{
	using System.IO;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// 
	/// </summary>
	public class ThirdSDKSettings : ScriptableObject
	{
		private const string MobileAdsSettingsResDir = "Assets/GoogleMobileAds/Resources";

		private const string MobileAdsSettingsFile = "ThirdSDKSettings";

		private const string MobileAdsSettingsFileExtension = ".asset";
		
		public static ThirdSDKSettings LoadInstance()
		{
			//Read from resources.
			var instance = Resources.Load<ThirdSDKSettings>(MobileAdsSettingsFile);

			//Create instance if null.
			if (instance == null)
			{
				Directory.CreateDirectory(MobileAdsSettingsResDir);
				instance = ScriptableObject.CreateInstance<ThirdSDKSettings>();
				string assetPath = Path.Combine(
						MobileAdsSettingsResDir,
						MobileAdsSettingsFile + MobileAdsSettingsFileExtension);
				AssetDatabase.CreateAsset(instance, assetPath);
				AssetDatabase.SaveAssets();
			}

			return instance;
		}
		
		[SerializeField] private string universalLinkDomain = "domian";
		
		[SerializeField] private string universalLinkPath = "project";

		[SerializeField] private string wxAppId = string.Empty;

		[SerializeField] private string xhsAppId = string.Empty;

		public string UniversalLink => $"https://{universalLinkDomain}/{universalLinkPath}/";

		public string WxAppId => wxAppId;

		public string XhsAppId => xhsAppId;
	}
}