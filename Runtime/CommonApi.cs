// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		PlatformTools.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/20 19:54:01
// *******************************************

namespace Bridge.Common
{
	/// <summary>
	/// 
	/// </summary>
	public static class CommonApi
	{
		private static IBridge _bridge;

		private static IBridge Bridge
		{
			get
			{
				if (_bridge == null)
				{
#if UNITY_IOS && !UNITY_EDITOR
					_bridge = new iOSBridgeImpl();
#elif UNITY_ANDROID && !UNITY_EDITOR
					_bridge = new AndroidBridgeImpl();
#elif UNITY_OPENHARMONY && !UNITY_EDITOR
					_bridge = new OpenHarmonyImpl();
#else
					_bridge = new EditorBridge();
#endif
				}

				return _bridge;
			}
		}

		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="listener">回调事件</param>
		public static void Init(IBridgeListener listener)
		{
			Bridge.Init(listener);
		}

		/// <summary>
		/// 振动
		/// </summary>
		/// <param name="effectType">振动等级</param>
		/// <param name="listener">回调事件</param>
		public static void Vibrator(VibratorEffectType effectType, IBridgeListener listener)
		{
			Bridge.Vibrator(effectType, listener);
		}

		/// <summary>
		/// 获取国家信息
		/// </summary>
		/// <param name="listener">回调事件</param>
		public static void GetCountryInfo(IBridgeListener listener)
		{
			Bridge.GetCountryInfo(listener);
		}
	}
}