// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		EditorBridge.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/20 20:54:37
// *******************************************

namespace Bridge.Common
{
	using Newtonsoft.Json;

	/// <summary>
	/// 
	/// </summary>
	public class EditorBridge : IBridge
	{
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="listener">回调事件</param>
		void IBridge.Init(IBridgeListener listener)
		{
			listener?.OnSuccess("editor");
		}

		/// <summary>
		/// 振动
		/// </summary>
		/// <param name="effectType">振动等级</param>
		/// <param name="listener">回调事件</param>
		void IBridge.Vibrator(VibratorEffectType effectType, IBridgeListener listener)
		{
			listener?.OnSuccess(effectType.ToString());
		}

		/// <summary>
		/// 获取国家信息
		/// </summary>
		/// <param name="listener">回调事件</param>
		void IBridge.GetCountryInfo(IBridgeListener listener)
		{
			listener?.OnSuccess(JsonConvert.SerializeObject(new CountryInfo
			{
					ip = "154.21.193.43",
					city = "Los Angeles",
					region = "California",
					country = "US",
					loc = "34.0522,-118.2437",
					org = "AS174 Cogent Communications",
					postal = "90009",
					timezone = "America/Los_Angeles",
					readme = "https://ipinfo.io/missingauth"
			}));
		}
	
		/// <summary>
		/// 一键拉起加QQ群
		/// </summary>
		/// <param name="qqGroupValue">加群参数</param>
		/// <param name="listener">加群回调</param>
		void IBridge.JoinQQGroup(string qqGroupValue, IBridgeListener listener)
		{
			listener?.OnSuccess("");
		}
	}
}
