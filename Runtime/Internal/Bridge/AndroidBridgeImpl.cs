// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		AndroidBridgeImpl.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/20 19:58:39
// *******************************************

#if UNITY_ANDROID
namespace SDSGDK.Common
{
	using System;
	using UnityEngine;

	/// <summary>
	/// 
	/// </summary>
	internal class AndroidBridgeImpl : IBridge
	{
		private const string UnityPlayerClassName = "com.unity3d.player.UnityPlayer";
		private const string ManagerClassName = "com.platform.tools.MobilePlatformTools";
		private static AndroidJavaObject api;
		private static AndroidJavaObject currentActivity;

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="listener">回调事件</param>
		void IBridge.Init(IBridgeListener listener)
		{
			AndroidJavaClass unityPlayer = new AndroidJavaClass(UnityPlayerClassName);
			currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

			AndroidJavaClass jc = new AndroidJavaClass(ManagerClassName);
			api = jc.CallStatic<AndroidJavaObject>("getInstance");
			api.Call("init", currentActivity, new BridgeCallback(listener));
		}

		/// <summary>
		/// 振动
		/// </summary>
		/// <param name="effectType">振动等级</param>
		/// <param name="listener">回调事件</param>
		void IBridge.Vibrator(VibratorEffectType effectType, IBridgeListener listener)
		{
			api.Call("vibratorByEffectType", GetVibratorEffectName(effectType), new BridgeCallback(listener));
		}

		/// <summary>
		/// 获取国家信息
		/// </summary>
		/// <param name="listener">回调事件</param>
		void IBridge.GetCountryInfo(IBridgeListener listener)
		{
			api.Call("getCountryInfo", new BridgeCallback(listener));
		}

		/// <summary>
		/// 转换到java的VibratorEffectType对象
		/// </summary>
		/// <param name="effectType"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		private string GetVibratorEffectName(VibratorEffectType effectType)
		{
			switch (effectType)
			{
				case VibratorEffectType.Low:
					return "LOW";
				case VibratorEffectType.Middle:
					return "MIDDLE";
				case VibratorEffectType.High:
					return "HIGH";
				default:
					throw new ArgumentOutOfRangeException(nameof(effectType), effectType, null);
			}
		}
	}
}
#endif