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
	using Common;
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

		private string universalLinkDomain = "domian";

		private string universalLinkPath = "project";

		public string UniversalLink => $"https://{universalLinkDomain}/{universalLinkPath}/";

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

		private static void PreferencesGUI()
		{
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.LabelField("Universal Link", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;

			Instance.universalLinkDomain = EditorGUILayout.DelayedTextField("domain", Instance.universalLinkDomain);

			Instance.universalLinkPath = EditorGUILayout.DelayedTextField("path", Instance.universalLinkPath);

			EditorGUILayout.HelpBox($"Universal Link: {Instance.UniversalLink}", MessageType.Info);

			EditorGUI.indentLevel--;
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			DrawWxConfig();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			DrawXhsConfig();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();
			DrawFacebookConfig();
			EditorGUILayout.Separator();
			EditorGUILayout.Separator();

			if (EditorGUI.EndChangeCheck())
				Instance.Save();
		}

		private static void DrawWxConfig()
		{
			bool IsOpenWxApi = ThirdSDKTool.IsOpenWxApi();
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("WeChat Open SDK Configuration", EditorStyles.boldLabel);
			if (IsOpenWxApi)
			{
				if (GUILayout.Button("Remove WeCaht Api Package"))
				{
					ThirdSDKPackageManager.RemoveWxApiPackage();
				}
			}
			else
			{
				if (GUILayout.Button("Add WeCaht Api Package"))
				{
					ThirdSDKPackageManager.AddWxApiPackage();
				}
			}
			EditorGUILayout.EndHorizontal();
			EditorGUI.indentLevel++;
			if (IsOpenWxApi)
			{
				Instance.WxAppId = EditorGUILayout.DelayedTextField("App Id", Instance.WxAppId);
			}
			EditorGUI.indentLevel--;
		}

		private static void DrawXhsConfig()
		{
			if (ThirdSDKTool.IsOpenXhsApi())
			{
				EditorGUILayout.LabelField("XiaoHongShu Open SDK Configuration", EditorStyles.boldLabel);
				EditorGUI.indentLevel++;
				Instance.XhsAppId = EditorGUILayout.DelayedTextField("App Id", Instance.XhsAppId);
				EditorGUI.indentLevel--;
			}
		}

		private static void DrawFacebookConfig()
		{
			if (ThirdSDKTool.IsOpenFBApi())
			{
				EditorGUILayout.LabelField("Facebook Open SDK Configuration", EditorStyles.boldLabel);
				EditorGUI.indentLevel++;
				Instance.FbAppId = EditorGUILayout.DelayedTextField("App Id", Instance.FbAppId);
				Instance.FbClientToken = EditorGUILayout.DelayedTextField("Client Token", Instance.FbClientToken);
				EditorGUI.indentLevel--;
			}
		}
	}
}