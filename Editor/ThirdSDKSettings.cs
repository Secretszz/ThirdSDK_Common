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
	using System;
	using System.IO;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// 
	/// </summary>
	public class ThirdSDKSettings
	{
		private const string SAVE_PATH = "ProjectSettings/ThirdSDKSettings.json";

		private static ThirdSDKSettings m_instance;

		public static ThirdSDKSettings Instance
		{
			get
			{
				if (m_instance == null)
				{
					try
					{
						if (File.Exists(SAVE_PATH))
							m_instance = JsonUtility.FromJson<ThirdSDKSettings>(File.ReadAllText(SAVE_PATH));
						else
							m_instance = new ThirdSDKSettings();
					}
					catch (System.Exception e)
					{
						Debug.LogException(e);
						m_instance = new ThirdSDKSettings();
					}
				}

				return m_instance;
			}
		}

		public bool NeedAddAssociatedDomains = true;

		public string UniversalLinkDomain = "domian";

		public string UniversalLinkPath = "project";

		public string WxAppId;

		public string XhsAppId;

		public string XhsAppId_iOS;

		public string FbAppId;

		public string FbClientToken;

		public string FbDisplayName;

		public string UniversalLink => $"https://{UniversalLinkDomain}/{UniversalLinkPath}/";

		private void Save()
		{
			File.WriteAllText(SAVE_PATH, JsonUtility.ToJson(this, true));
		}

		[SettingsProvider]
		public static SettingsProvider CreatePreferencesGUI()
		{
			return new SettingsProvider("Project/ThirdSDK/Settings", SettingsScope.Project)
			{
					guiHandler = _ => PreferencesGUI(),
					keywords = new System.Collections.Generic.HashSet<string> { "Native", "ThirdSDK", "Android", "iOS" }
			};
		}

		private static float labelWidth = 200f;
		private static float inputWidth = 600f;

		private static void PreferencesGUI()
		{
			EditorGUIUtility.labelWidth = labelWidth;
			DrawApiDownload();
			EditorGUI.BeginChangeCheck();
			DrawUniversalLink();
			DrawApiConfig();
			if (EditorGUI.EndChangeCheck())
				Instance.Save();
		}

		private static void DrawApiDownload()
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUI.indentLevel++;

			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Adapter", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
			EditorGUILayout.LabelField("Version", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
			EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
			EditorGUILayout.EndHorizontal();

			foreach (PackageType packageType in Enum.GetValues(typeof(PackageType)))
			{
				EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField($"{ThirdSDKPackageManager.GetPackageName(packageType)} Open SDK", GUILayout.ExpandWidth(false));
				EditorGUILayout.LabelField(ThirdSDKPackageManager.GetVersionName(packageType), GUILayout.ExpandWidth(false));
				if (ThirdSDKPackageManager.IsOpenApi(packageType))
				{
					if (GUILayout.Button("Remove", GUILayout.Width(100f), GUILayout.ExpandWidth(false)))
					{
						ThirdSDKPackageManager.RemovePackage(packageType);
					}
				}
				else
				{
					if (GUILayout.Button("Add", GUILayout.Width(100f), GUILayout.ExpandWidth(false)))
					{
						ThirdSDKPackageManager.AddPackage(packageType);
					}
				}

				EditorGUILayout.EndHorizontal();
			}
			
			EditorGUI.indentLevel--;
		}

		private static void DrawUniversalLink()
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUI.indentLevel++;
			EditorGUILayout.LabelField("Universal Link", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));

			Instance.UniversalLinkDomain = EditorGUILayout.DelayedTextField("domain: ", Instance.UniversalLinkDomain, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));

			Instance.UniversalLinkPath = EditorGUILayout.DelayedTextField("path: ", Instance.UniversalLinkPath, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));

			EditorGUILayout.LabelField("Universal Link: ", Instance.UniversalLink, EditorStyles.boldLabel, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
			EditorGUILayout.BeginHorizontal();
			Instance.NeedAddAssociatedDomains = EditorGUILayout.Toggle("Add Associated Domains", Instance.NeedAddAssociatedDomains, GUILayout.ExpandWidth(false));
			EditorGUILayout.LabelField("If enabled, automatically add Associated Domains. Use in iOS environment", EditorStyles.boldLabel);
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel--;
		}

		private static void DrawApiConfig()
		{
			foreach (PackageType packageType in Enum.GetValues(typeof(PackageType)))
			{
				if (ThirdSDKPackageManager.IsOpenApi(packageType))
				{
					EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
					EditorGUI.indentLevel++;
					EditorGUILayout.LabelField($"{ThirdSDKPackageManager.GetPackageName(packageType)} Configuration", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
					EditorGUILayout.Separator();
					switch (packageType)
					{
						case PackageType.WeChat:
							Instance.WxAppId = EditorGUILayout.DelayedTextField("App Id: ", Instance.WxAppId, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
							break;
						case PackageType.XiaoHongShu:
							Instance.XhsAppId = EditorGUILayout.DelayedTextField("Android App Id: ", Instance.XhsAppId, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
							Instance.XhsAppId_iOS = EditorGUILayout.DelayedTextField("iOS App Id: ", Instance.XhsAppId_iOS, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
							break;
						case PackageType.Facebook:
							Instance.FbAppId = EditorGUILayout.DelayedTextField("App Id: ", Instance.FbAppId, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
							Instance.FbClientToken = EditorGUILayout.DelayedTextField("Client Token: ", Instance.FbClientToken, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
							Instance.FbDisplayName = EditorGUILayout.DelayedTextField("Display Name: ", Instance.FbDisplayName, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
							break;
						case PackageType.Instagram:
							EditorGUILayout.LabelField("future", GUILayout.ExpandWidth(false));
							break;
						case PackageType.QQ:
							EditorGUILayout.LabelField("future", GUILayout.ExpandWidth(false));
							break;
						default:
							throw new ArgumentOutOfRangeException(nameof(packageType), packageType, null);
					}
					EditorGUI.indentLevel--;
				}
			}
		}
	}
}