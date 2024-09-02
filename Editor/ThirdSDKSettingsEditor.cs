// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		ThirdSDKSettingsEditor.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/02 11:23:30
// *******************************************

namespace Bridge.Common
{
	using UnityEngine;
	using UnityEditor;

	/// <summary>
	/// 
	/// </summary>
	[InitializeOnLoad]
	[CustomEditor(typeof(ThirdSDKSettings))]
	public class ThirdSDKSettingsEditor : Editor
	{
		private SerializedProperty _universalLink;
		private SerializedProperty _wxAppId;
		private SerializedProperty _xhsAppId;

        [MenuItem("Assets/ThirdSDK/Settings...")]
        public static void OpenInspector()
        {
            Selection.activeObject = ThirdSDKSettings.LoadInstance();
        }

        public void OnEnable()
        {
	        _universalLink = serializedObject.FindProperty("universalLink");
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

	        EditorGUILayout.PropertyField(_universalLink, new GUIContent(""));

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