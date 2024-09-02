// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IOSProcessorTool.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/02 18:45:11
// *******************************************

#if UNITY_IOS

namespace Bridge.Editor
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using UnityEditor.iOS.Xcode;

	/// <summary>
	/// 
	/// </summary>
	public static class IOSProcessorTool
	{
		public static void AddApplicationQueriesSchemes(this PlistElementDict rootDic, string[] items)
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

		public static void AddCFBundleURLTypes(this PlistElementArray array, string CFBundleTypeRole, string CFBundleURLName, string[] CFBundleURLSchemes)
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

		public static void AddFrameworkToProjectEx(this PBXProject proj, string targetGuid, string framework, bool weak)
		{
			if (!proj.ContainsFramework(targetGuid, framework))
			{
				proj.AddFrameworkToProject(targetGuid, framework, weak);
			}
		}

		public static PlistElementArray GetElementArray(this PlistElementDict rootDict, string key)
		{
			return rootDict.values.TryGetValue(key, out var element) ? element.AsArray() : rootDict.CreateArray(key);
		}

		public static List<T> ToList<TSource, T>(this IEnumerable<TSource> source, Func<TSource, T> predicate, Func<TSource, bool> selector = null)
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

#endif