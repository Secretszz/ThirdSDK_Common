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

		private bool needAddAssociatedDomains;

		public bool NeedAddAssociatedDomains
		{
			get
			{
				if (ThirdSDKPackageManager.IsOpenApi(PackageType.WeChat) || ThirdSDKPackageManager.IsOpenApi(PackageType.XiaoHongShu) || ThirdSDKPackageManager.IsOpenApi(PackageType.Facebook))
				{
					return true;
				}

				return needAddAssociatedDomains;
			}
			private set => needAddAssociatedDomains = value;
		}

		public string UniversalLinkDomain { get; private set; } = "domian";

		public string UniversalLinkPath { get; private set; } = "project";

		public string UniversalLink => $"https://{UniversalLinkDomain}/{UniversalLinkPath}/";

		public string WxAppId { get; private set; }

		public string XhsAppId { get; private set; }

		public string FbAppId { get; private set; }

		public string FbClientToken { get; private set; }

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
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUI.indentLevel++;

			DrawApiDownload();

			EditorGUI.indentLevel--;
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUI.indentLevel++;

			EditorGUILayout.LabelField("Universal Link", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
			EditorGUI.indentLevel++;

			Instance.NeedAddAssociatedDomains = EditorGUILayout.Toggle("Open Associated Domains", Instance.NeedAddAssociatedDomains, GUILayout.ExpandWidth(false));

			if (Instance.NeedAddAssociatedDomains)
			{
				Instance.UniversalLinkDomain = EditorGUILayout.DelayedTextField("domain: ", Instance.UniversalLinkDomain, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));

				Instance.UniversalLinkPath = EditorGUILayout.DelayedTextField("path: ", Instance.UniversalLinkPath, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));

				EditorGUILayout.LabelField("Universal Link: ", Instance.UniversalLink, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
			}

			EditorGUI.indentLevel--;

			EditorGUI.indentLevel--;
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUI.indentLevel++;

			DrawWxConfig();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			DrawXhsConfig();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			DrawFacebookConfig();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			DrawInstagramConfig();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			DrawQQConfig();
			EditorGUI.indentLevel--;

			if (EditorGUI.EndChangeCheck())
				Instance.Save();
		}

		private static void DrawApiDownload()
		{
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Adapter", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
			EditorGUILayout.LabelField("Version", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
			EditorGUILayout.LabelField("Actions", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
			EditorGUILayout.EndHorizontal();

			foreach (PackageType packageType in Enum.GetValues(typeof(PackageType)))
			{
				EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.LabelField(ThirdSDKPackageManager.GetPackageName(packageType), EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
				EditorGUILayout.LabelField(ThirdSDKPackageManager.GetVersionName(packageType), EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
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
		}

		private static void DrawWxConfig()
		{
			if (ThirdSDKPackageManager.IsOpenApi(PackageType.WeChat))
			{
				EditorGUILayout.LabelField("WeChat Open SDK Configuration", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
				EditorGUILayout.Separator();
				EditorGUI.indentLevel++;
				Instance.WxAppId = EditorGUILayout.DelayedTextField("App Id: ", Instance.WxAppId, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
				EditorGUI.indentLevel--;
			}
		}

		private static void DrawXhsConfig()
		{
			if (ThirdSDKPackageManager.IsOpenApi(PackageType.XiaoHongShu))
			{
				EditorGUILayout.LabelField("XiaoHongShu Open SDK Configuration", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
				EditorGUILayout.Separator();
				EditorGUI.indentLevel++;
				Instance.XhsAppId = EditorGUILayout.DelayedTextField("App Id: ", Instance.XhsAppId, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
				EditorGUI.indentLevel--;
			}
		}

		private static void DrawFacebookConfig()
		{
			if (ThirdSDKPackageManager.IsOpenApi(PackageType.Facebook))
			{
				EditorGUILayout.LabelField("Facebook Open SDK Configuration", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
				EditorGUILayout.Separator();
				EditorGUI.indentLevel++;
				Instance.FbAppId = EditorGUILayout.DelayedTextField("App Id: ", Instance.FbAppId, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
				Instance.FbClientToken = EditorGUILayout.DelayedTextField("Client Token: ", Instance.FbClientToken, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
				EditorGUI.indentLevel--;
			}
		}

		private static void DrawInstagramConfig()
		{
			if (ThirdSDKPackageManager.IsOpenApi(PackageType.Instagram))
			{

			}
		}

		private static void DrawQQConfig()
		{
			if (ThirdSDKPackageManager.IsOpenApi(PackageType.QQ))
			{

			}
		}
	}
}