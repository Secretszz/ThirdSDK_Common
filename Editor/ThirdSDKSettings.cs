// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		ThirdSDKSettings.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/02 11:16:59
// *******************************************

namespace Bridge.Common
{
	using System.IO;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// 
	/// </summary>
	internal class ThirdSDKSettings : ScriptableObject
	{
		private const string MobileAdsSettingsResDir = "Assets/GoogleMobileAds/Resources";

		private const string MobileAdsSettingsFile = "ThirdSDKSettings";

		private const string MobileAdsSettingsFileExtension = ".asset";
		
		internal static ThirdSDKSettings LoadInstance()
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
		
		[SerializeField] private string universalLink = string.Empty;
		
		[SerializeField] private string wxAppId = string.Empty;

		[SerializeField] private string xhsAppId = string.Empty;

		public string UniversalLink
		{
			get { return universalLink; }

			set { universalLink = value; }
		}

		public string WxAppId
		{
			get { return wxAppId; }

			set { wxAppId = value; }
		}
		
		public string XhsAppId
		{
			get { return xhsAppId; }

			set { xhsAppId = value; }
		}
	}
}