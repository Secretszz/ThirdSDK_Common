// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IOSProcessor.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/04 18:48:32
// *******************************************

#if UNITY_IOS
namespace Bridge.Common
{
	using UnityEditor;
	using UnityEditor.Callbacks;
	using UnityEditor.iOS.Xcode;

	/// <summary>
	/// 
	/// </summary>
	internal static class IOSProcessor
	{
		[PostProcessBuild(10000)]
		public static void OnPostProcessBuild(BuildTarget target, string pathToBuildProject)
		{
			if (target == BuildTarget.iOS)
			{
				ThirdSDKSettings instance = ThirdSDKSettings.Instance;
				var projPath = pathToBuildProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
				var proj = new PBXProject();
				proj.ReadFromFile(projPath);
				var frameworkTargetGuid = proj.GetUnityFrameworkTargetGuid();
				var mainTargetGuid = proj.GetUnityMainTargetGuid();
				var entitlementFilePath = proj.GetEntitlementFilePathForTarget(mainTargetGuid);
				var manager = new ProjectCapabilityManager(projPath, entitlementFilePath, null, mainTargetGuid);
				if (instance.NeedAddAssociatedDomains)
				{
					manager.AddAssociatedDomains(new[] { $"applinks:{instance.UniversalLinkDomain}" });
				}
				manager.WriteToFile();
			}
		}
	}
}
#endif
