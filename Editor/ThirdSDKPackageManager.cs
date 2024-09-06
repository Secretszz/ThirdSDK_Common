// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		ThirdSDKPackageManager.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/04 11:54:43
// *******************************************

using System;
using System.Linq;
using System.Threading.Tasks;

namespace Bridge.Editor
{
	using UnityEditor;
	using UnityEditor.PackageManager;
	using UnityEditor.PackageManager.Requests;
	using UnityEngine;

	/// <summary>
	/// 
	/// </summary>
	public static class ThirdSDKPackageManager
	{
		#region private

		private static AddRequest addRequest;
		private static RemoveRequest removeRequest;

		private static void AddPackage(string gitUrl)
		{
			if (addRequest != null)
			{
				Debug.LogWarning("a package is downloading");
				return;
			}

			EditorApplication.update += AddProgress;
			addRequest = Client.Add(gitUrl);
		}

		private static void RemovePackage(string packageName)
		{
			if (removeRequest != null)
			{
				Debug.LogWarning("a package is downloading");
				return;
			}

			EditorApplication.update += RemoveProgress;
			removeRequest = Client.Remove(packageName);
		}

		private static void AddProgress()
		{
			if (addRequest != null && addRequest.IsCompleted)
			{
				if (addRequest.Status == StatusCode.Success)
					Debug.Log("Installed: " + addRequest.Result.packageId);
				else if (addRequest.Status >= StatusCode.Failure)
					Debug.Log(addRequest.Error.message);

				addRequest = null;
				EditorApplication.update -= AddProgress;
			}
		}

		private static void RemoveProgress()
		{
			if (addRequest != null && addRequest.IsCompleted)
			{
				if (addRequest.Status == StatusCode.Success)
					Debug.Log("Installed: " + addRequest.Result.packageId);
				else if (addRequest.Status >= StatusCode.Failure)
					Debug.Log(addRequest.Error.message);

				addRequest = null;
				EditorApplication.update -= RemoveProgress;
			}
		}

		private static async Task<bool> ContainsPackage(string packageName)
		{
			var request = Client.List();
			var task = new TaskCompletionSource<object>();
			EditorApplication.update += ListProgress;
			await task.Task;
			EditorApplication.update -= ListProgress;
			if (request.Status == StatusCode.Failure)
			{
				throw new Exception("request package list failure: " + request.Error);
			}

			return request.Result.Any(x => x.name == packageName);

			void ListProgress()
			{
				if (request.IsCompleted) task.SetResult(null);
			}
		}

		private static bool IsOpenApi(string startWith)
		{
			return AppDomain.CurrentDomain.GetAssemblies().Any(asssembly => asssembly.GetName().FullName.StartsWith(startWith));
		}

		#endregion

		public const string WxApiGitUrl = "https://github.com/Secretszz/ThirdSDK_WxApi.git";
		public const string InstagramApiGitUrl = "https://github.com/Secretszz/ThirdSDK_InstagramApi.git";
		public const string FacebookApiGitUrl = "https://github.com/Secretszz/ThirdSDK_FacebookApi.git";
		public const string XhsApiGitUrl = "https://github.com/Secretszz/ThirdSDK_XhsApi.git";
		public const string QQApiGitUrl = "https://github.com/Secretszz/ThirdSDK_QQApi.git";

		public const string WxApiPackageName = "com.bridge.wxapi";
		public const string InstagramApiPackageName = "com.bridge.ins";
		public const string FacebookApiPackageName = "com.bridge.facebook";
		public const string XhsApiPackageName = "com.bridge.xhsapi";
		public const string QQApiPackageName = "com.bridge.qqapi";

		public const string WxApiVersion = "1.0.1";
		public const string InstagramApiVersion = "1.0.1";
		public const string FacebookApiVersion = "1.0.1";
		public const string XhsApiVersion = "1.0.1";
		public const string QQApiVersion = "1.0.1";

		public const string WxApiAsssemblyName = "Bridge.WxApi";
		public const string XhsApiAsssemblyName = "Bridge.XhsApi";
		public const string FBApiAsssemblyName = "Bridge.FacebookApi";
		public const string InsApiAsssemblyName = "Bridge.InstagramApi";
		public const string QQApiAsssemblyName = "Bridge.QQApi";

		public static void AddPackage(PackageType packageType)
		{
			AddPackage(packageType switch
			{
					PackageType.WeChat => WxApiGitUrl,
					PackageType.XiaoHongShu => XhsApiGitUrl,
					PackageType.Facebook => FacebookApiGitUrl,
					PackageType.Instagram => InstagramApiGitUrl,
					PackageType.QQ => QQApiGitUrl,
					_ => throw new ArgumentOutOfRangeException(nameof(packageType), packageType, null)
			});
		}

		public static void RemovePackage(PackageType packageType)
		{
			RemovePackage(packageType switch
			{
					PackageType.WeChat => WxApiPackageName,
					PackageType.XiaoHongShu => XhsApiPackageName,
					PackageType.Facebook => FacebookApiPackageName,
					PackageType.Instagram => InstagramApiPackageName,
					PackageType.QQ => QQApiPackageName,
					_ => throw new ArgumentOutOfRangeException(nameof(packageType), packageType, null)
			});
		}

		public static string GetVersionName(PackageType packageType)
		{
			return packageType switch
			{
					PackageType.WeChat => WxApiVersion,
					PackageType.XiaoHongShu => XhsApiVersion,
					PackageType.Facebook => FacebookApiVersion,
					PackageType.Instagram => InstagramApiVersion,
					PackageType.QQ => QQApiVersion,
					_ => throw new ArgumentOutOfRangeException(nameof(packageType), packageType, null)
			};
		}
		
		public static string GetPackageName(PackageType packageType)
		{
			return packageType switch
			{
					PackageType.WeChat => "WeChat",
					PackageType.XiaoHongShu => "XiaoHongShu",
					PackageType.Facebook => "Facebook",
					PackageType.Instagram => "Instagram",
					PackageType.QQ => "QQ",
					_ => throw new ArgumentOutOfRangeException(nameof(packageType), packageType, null)
			};
		}

		public static bool IsOpenApi(PackageType packageType)
		{
			return IsOpenApi(packageType switch
			{
					PackageType.WeChat => WxApiAsssemblyName,
					PackageType.XiaoHongShu => XhsApiAsssemblyName,
					PackageType.Facebook => FBApiAsssemblyName,
					PackageType.Instagram => InsApiAsssemblyName,
					PackageType.QQ => QQApiAsssemblyName,
					_ => throw new ArgumentOutOfRangeException(nameof(packageType), packageType, null)
			});
		}

		public static string GetUnityPackagePath(string packageName)
		{
			var request = Client.List(true);
			while (request.IsCompleted == false)
			{
				System.Threading.Thread.Sleep(100);
			}

			var pkgs = request.Result;
			if (pkgs == null)
				return "";
			foreach (var pkg in pkgs)
			{
				if (pkg.name == packageName)
				{
					return pkg.source switch
					{
							PackageSource.Local => pkg.resolvedPath,
							PackageSource.Embedded => pkg.resolvedPath,
							_ => pkg.resolvedPath
					};
				}
			}

			return "";
		}
	}

	public enum PackageType
	{
		WeChat,
		XiaoHongShu,
		Facebook,
		Instagram,
		QQ
	}
}
