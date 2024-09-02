// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		ThirdSDKSettingsEditor.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/02 11:23:30
// *******************************************

namespace Bridge.Editor
{
	using UnityEngine;
	using UnityEditor;
	using Common;

	/// <summary>
	/// 
	/// </summary>
	[InitializeOnLoad]
	[CustomEditor(typeof(ThirdSDKSettings))]
	internal class ThirdSDKSettingsEditor : Editor
	{
		private SerializedProperty _universalLinkDomain;
		private SerializedProperty _universalLinkPath;
		private SerializedProperty _wxAppId;
		private SerializedProperty _xhsAppId;

        [MenuItem("Assets/ThirdSDK/Settings...")]
        public static void OpenInspector()
        {
            Selection.activeObject = ThirdSDKSettings.LoadInstance();
        }

        public void OnEnable()
        {
	        _universalLinkDomain = serializedObject.FindProperty("universalLinkDomain");
	        _universalLinkPath = serializedObject.FindProperty("universalLinkPath");
	        _wxAppId = serializedObject.FindProperty("wxAppId");
	        _xhsAppId = serializedObject.FindProperty("xhsAppId");
        }

        public override void OnInspectorGUI()
        {
	        // Make sure the Settings object has all recent changes.
	        serializedObject.Update();

	        var settings = (ThirdSDKSettings)target;

	        if (settings == null)
	        {
		        Debug.LogError("GoogleMobileAdsSettings is null.");
		        return;
	        }

	        EditorGUILayout.LabelField("Universal Link", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;

	        EditorGUILayout.PropertyField(_universalLinkDomain, new GUIContent("domain"));

	        EditorGUILayout.PropertyField(_universalLinkPath, new GUIContent("path"));
	        
	        EditorGUILayout.HelpBox($"Universal Link: {settings.UniversalLink}", MessageType.Info);

	        EditorGUI.indentLevel--;
	        EditorGUILayout.Separator();

	        EditorGUILayout.LabelField("App ID", EditorStyles.boldLabel);
	        EditorGUI.indentLevel++;

	        if (ThirdSDKTool.IsOpenWxApi())
	        {
		        EditorGUILayout.PropertyField(_wxAppId, new GUIContent("微信"));
	        }
	        
	        if (ThirdSDKTool.IsOpenXhsApi())
	        {
		        EditorGUILayout.PropertyField(_xhsAppId, new GUIContent("小红书"));
	        }

	        EditorGUI.indentLevel--;
	        EditorGUILayout.Separator();

	        serializedObject.ApplyModifiedProperties();
        }
	}
}