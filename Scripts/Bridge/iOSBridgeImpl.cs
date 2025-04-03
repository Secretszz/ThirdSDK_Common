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
	public class iOSBridgeImpl : IBridge
	{
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="listener">回调事件</param>
		void IBridge.Init(IBridgeListener listener)
		{
			Callback._listener = listener;
			c_common_init(Callback.OnSuccess, Callback.OnError);
		}

		/// <summary>
		/// 振动
		/// </summary>
		/// <param name="effectType">振动等级</param>
		/// <param name="listener">回调事件</param>
		void IBridge.Vibrator(VibratorEffectType effectType, IBridgeListener listener)
		{
			Callback._listener = listener;
			c_vibrator((int)effectType, Callback.OnSuccess, Callback.OnError);
		}

		/// <summary>
		/// 获取国家信息
		/// </summary>
		/// <param name="listener">回调事件</param>
		void IBridge.GetCountryInfo(IBridgeListener listener)
		{
			Callback._listener = listener;
			c_getCountryInfo(Callback.OnSuccess);
		}

		/// <summary>
		/// 一键拉起加QQ群
		/// </summary>
		/// <param name="qqGroupValue">加群参数</param>
		/// <param name="listener">加群回调</param>
		void IBridge.JoinQQGroup(string qqGroupValue, IBridgeListener listener)
		{
			string[] keys = qqGroupValue.Split(',');
			if (c_join_qq_group(keys[0], keys[1]))
			{
				listener?.OnSuccess("");
			}
			else
			{
				listener?.OnError(-1, "打开QQ失败，请检查设备内是否安装了QQ");
			}
		}

		[DllImport("__Internal")]
		private static extern bool c_join_qq_group(string groupUin, string key);

		[DllImport("__Internal")]
		private static extern void c_common_init(U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Error onError);

		/// <summary>
		/// 振动
		/// </summary>
		/// <param name="effectType">振动类型</param>
		/// <param name="onSuccess">调用回调</param>
		/// <param name="onError">调用回调</param>
		[DllImport("__Internal")]
		private static extern void c_vibrator(int effectType, U3DBridgeCallback_Success onSuccess, U3DBridgeCallback_Error onError);

		/// <summary>
		/// 获取国家信息
		/// </summary>
		/// <param name="onSuccess">调用回调</param>
		[DllImport("__Internal")]
		private static extern void c_getCountryInfo(U3DBridgeCallback_Success onSuccess);

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