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

		public bool NeedAddAssociatedDomains;

		public string UniversalLinkDomain = "domian";

		public string UniversalLinkPath = "project";

		public string WxAppId;

		public string XhsAppId;

		public string FbAppId;

		public string FbClientToken;

		public string FbDisplayName;

		public string UniversalLink => $"https://{UniversalLinkDomain}/{UniversalLinkPath}/";

		private void Save()
		{
			string value = JsonUtility.ToJson(this, true);
			Debug.Log("value===" + value);
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
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUI.indentLevel++;

			DrawApiDownload();

			EditorGUI.indentLevel--;
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUI.indentLevel++;

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.LabelField("Universal Link", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
			EditorGUI.indentLevel++;

			bool forceOpen = ThirdSDKPackageManager.IsOpenApi(PackageType.WeChat) || ThirdSDKPackageManager.IsOpenApi(PackageType.XiaoHongShu) || ThirdSDKPackageManager.IsOpenApi(PackageType.Facebook);
			if (forceOpen)
			{
				Instance.NeedAddAssociatedDomains = true;
				EditorGUILayout.Toggle("Open Associated Domains", Instance.NeedAddAssociatedDomains, GUILayout.ExpandWidth(false));
			}
			else
			{
				Instance.NeedAddAssociatedDomains = EditorGUILayout.Toggle("Open Associated Domains", Instance.NeedAddAssociatedDomains, GUILayout.ExpandWidth(false));
			}

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
			EditorGUI.indentLevel--;
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUI.indentLevel++;
			DrawXhsConfig();
			EditorGUI.indentLevel--;
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUI.indentLevel++;
			DrawFacebookConfig();
			EditorGUI.indentLevel--;
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUI.indentLevel++;
			DrawInstagramConfig();
			EditorGUI.indentLevel--;
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
			EditorGUI.indentLevel++;
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
				EditorGUILayout.LabelField(ThirdSDKPackageManager.GetPackageName(packageType), GUILayout.ExpandWidth(false));
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
		}

		private static void DrawWxConfig()
		{
			if (ThirdSDKPackageManager.IsOpenApi(PackageType.WeChat))
			{
				EditorGUILayout.LabelField("WeChat Configuration", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
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
				EditorGUILayout.LabelField("XiaoHongShu Configuration", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
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
				EditorGUILayout.LabelField("Facebook Configuration", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
				EditorGUILayout.Separator();
				EditorGUI.indentLevel++;
				Instance.FbAppId = EditorGUILayout.DelayedTextField("App Id: ", Instance.FbAppId, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
				Instance.FbClientToken = EditorGUILayout.DelayedTextField("Client Token: ", Instance.FbClientToken, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
				Instance.FbDisplayName = EditorGUILayout.DelayedTextField("Display Name: ", Instance.FbDisplayName, GUILayout.Width(inputWidth), GUILayout.ExpandWidth(false));
				EditorGUI.indentLevel--;
			}
		}

		private static void DrawInstagramConfig()
		{
			if (ThirdSDKPackageManager.IsOpenApi(PackageType.Instagram))
			{
				EditorGUILayout.LabelField("Instagram Configuration", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
				EditorGUILayout.Separator();
				EditorGUI.indentLevel++;
				EditorGUILayout.LabelField("future", GUILayout.ExpandWidth(false));
				EditorGUI.indentLevel--;
			}
		}

		private static void DrawQQConfig()
		{
			if (ThirdSDKPackageManager.IsOpenApi(PackageType.QQ))
			{
				EditorGUILayout.LabelField("QQ Configuration", EditorStyles.boldLabel, GUILayout.ExpandWidth(false));
				EditorGUILayout.Separator();
				EditorGUI.indentLevel++;
				EditorGUILayout.LabelField("future", GUILayout.ExpandWidth(false));
				EditorGUI.indentLevel--;
			}
		}
	}
}