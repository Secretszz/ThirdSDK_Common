// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IOSProcessor.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/02 12:01:02
// *******************************************

namespace Bridge.Common
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using UnityEditor.iOS.Xcode;
	using UnityEditor;
	using UnityEditor.Callbacks;

	/// <summary>
	/// 
	/// </summary>
	internal static class IOSProcessor
	{
		[PostProcessBuild(10002)]
		public static void OnPostProcessBuild(BuildTarget target, string pathToBuildProject)
		{
			if (target == BuildTarget.iOS)
			{
				ThirdSDKSettings instance = ThirdSDKSettings.LoadInstance();
				var projPath = pathToBuildProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
				var proj = new PBXProject();
				proj.ReadFromFile(projPath);
#if UNITY_2019_3_OR_NEWER
				var targetGUID = proj.GetUnityFrameworkTargetGuid();
#else
				var targetGUID = proj.TargetGuidByName("Unity-iPhone");
#endif
				var plistPath = Path.Combine(pathToBuildProject, "Info.plist");
				var plist = new PlistDocument();
				plist.ReadFromFile(plistPath);
				var rootDic = plist.root;

				if (ThirdSDKTool.IsOpenWxApi())
				{
					rootDic.AddApplicationQueriesSchemes(new[] { "xhsdiscover" });

					var array = rootDic.GetElementArray("CFBundleURLTypes");
					array.AddCFBundleURLTypes("Editor", "xiaohongshu", new[] { $"xhs{instance.XhsAppId}" });
				}

				if (ThirdSDKTool.IsOpenXhsApi())
				{
					proj.AddBuildProperty(targetGUID, "OTHER_LDFLAGS", "-ObjC -all_load");

					proj.AddFrameworkToProjectEx(targetGUID, "Security.framework", false);
					proj.AddFrameworkToProjectEx(targetGUID, "CoreGraphics.framework", false);
					proj.AddFrameworkToProjectEx(targetGUID, "WebKit.framework", false);

					rootDic.AddApplicationQueriesSchemes(new[] { "weixin", "weixinULAPI", "weixinURLParamsAPI" });

					var array = rootDic.GetElementArray("CFBundleURLTypes");
					array.AddCFBundleURLTypes("Editor", "weixin", new[] { instance.WxAppId });
				}

				proj.WriteToFile(projPath);
				plist.WriteToFile(plistPath);
			}
		}
	}

	internal static class IOSProcessorTool
	{
		internal static void AddApplicationQueriesSchemes(this PlistElementDict rootDic, string[] items)
		{
			PlistElementArray plistElementList = rootDic.GetElementArray("LSApplicationQueriesSchemes");
			string[] list = plistElementList.values.Select(x => x.AsString()).ToArray();
			foreach (var t in items)
			{
				if (!list.Contains(t))
				{
					plistElementList.AddString(t);
				}
			}
		}

		internal static void AddCFBundleURLTypes(this PlistElementArray array, string CFBundleTypeRole, string CFBundleURLName, string[] CFBundleURLSchemes)
		{
			PlistElementDict wxURLType = array.AddDict();
			wxURLType.SetString("CFBundleTypeRole", CFBundleTypeRole);
			wxURLType.SetString("CFBundleURLName", CFBundleURLName);
			var schemes = wxURLType.CreateArray("CFBundleURLSchemes");
			foreach (var scheme in CFBundleURLSchemes)
			{
				schemes.AddString(scheme);
			}
		}

		internal static void AddFrameworkToProjectEx(this PBXProject proj, string targetGuid, string framework, bool weak)
		{
			if (!proj.ContainsFramework(targetGuid, framework))
			{
				proj.AddFrameworkToProject(targetGuid, framework, weak);
			}
		}

		internal static PlistElementArray GetElementArray(this PlistElementDict rootDict, string key)
		{
			return rootDict.values.TryGetValue(key, out var element) ? element.AsArray() : rootDict.CreateArray(key);
		}

		internal static List<T> ToList<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T> predicate, Func<TSource, bool> selector = null)
		{
			List<T> result = new List<T>();
			foreach (var item in source)
			{
				if (selector == null || selector(item))
					result.Add(predicate(item));
			}

			return result;
		}
	}
}