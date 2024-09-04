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

		public static void AddWxApiPackage()
		{
			AddPackage(WxApiGitUrl);
		}
		
		public static void RemoveWxApiPackage()
		{
			RemovePackage(WxApiPackageName);
		}
		
		public static void AddInstagramApiPackage()
		{
			AddPackage(InstagramApiGitUrl);
		}
		
		public static void RemoveInstagramApiPackage()
		{
			RemovePackage(InstagramApiPackageName);
		}
		
		public static void AddFacebookApiPackage()
		{
			AddPackage(FacebookApiGitUrl);
		}
		
		public static void RemoveFacebookApiPackage()
		{
			RemovePackage(FacebookApiPackageName);
		}
		
		public static void AddXhsApiPackage()
		{
			AddPackage(XhsApiGitUrl);
		}
		
		public static void RemoveXhsApiPackage()
		{
			RemovePackage(XhsApiPackageName);
		}
		
		public static void AddQQApiPackage()
		{
			AddPackage(QQApiGitUrl);
		}
		
		public static void RemoveQQApiPackage()
		{
			RemovePackage(QQApiPackageName);
		}
	}
}