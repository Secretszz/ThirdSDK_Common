// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		iOSBridgeImpl.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/20 20:57:03
// *******************************************

#if UNITY_IOS
namespace Bridge.Common
{
	using AOT;
	using System.Runtime.InteropServices;

	/// <summary>
	/// 
	/// </summary>
	internal class iOSBridgeImpl : IBridge
	{
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="listener">回调事件</param>
		void IBridge.Init(IBridgeListener listener)
		{
			Callback._listener = listener;
			c_platform_tools_init(Callback.OnSuccess, Callback.OnError);
		}

		/// <summary>
		/// 振动
		/// </summary>
		/// <param name="effectType">振动等级</param>
		/// <param name="listener">回调事件</param>
		void IBridge.Vibrator(VibratorEffectType effectType, IBridgeListener listener)
		{
			Callback._listener = listener;
			c_platform_tools_vibrator((int)effectType, Callback.OnSuccess, Callback.OnError);
		}

		/// <summary>
		/// 获取国家信息
		/// </summary>
		/// <param name="listener">回调事件</param>
		void IBridge.GetCountryInfo(IBridgeListener listener)
		{
			Callback._listener = listener;
			c_platform_tools_getCountryInfo(Callback.OnSuccess);
		}

		[DllImport("__Internal")]
		private static extern void c_platform_tools_init(U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Error onError);

		/// <summary>
		/// 振动
		/// </summary>
		/// <param name="effectType">振动类型</param>
		/// <param name="onSuccess">调用回调</param>
		/// <param name="onError">调用回调</param>
		[DllImport("__Internal")]
		private static extern void c_platform_tools_vibrator(int effectType, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Error onError);

		/// <summary>
		/// 获取国家信息
		/// </summary>
		/// <param name="onSuccess">调用回调</param>
		[DllImport("__Internal")]
		private static extern void c_platform_tools_getCountryInfo(U3DBridgeCallback_Success onSuccess);

		private static class Callback
		{
			/// <summary>
			/// 支付回调监听
			/// </summary>
			public static IBridgeListener _listener;

			/// <summary>
			/// 支付成功回调桥接函数
			/// </summary>
			/// <param name="result"></param>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Success))]
			public static void OnSuccess(string result)
			{
				_listener?.OnSuccess(result);
			}

			/// <summary>
			/// 支付用户取消回调桥接函数
			/// </summary>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Cancel))]
			public static void OnCancel()
			{
				_listener?.OnCancel();
			}

			/// <summary>
			/// 支付错误回调桥接函数
			/// </summary>
			/// <param name="errCode"></param>
			/// <param name="errMsg"></param>
			[MonoPInvokeCallback(typeof(U3DBridgeCallback_Error))]
			public static void OnError(int errCode, string errMsg)
			{
				_listener?.OnError(errCode, errMsg);
			}
		}
	}
}
#endif