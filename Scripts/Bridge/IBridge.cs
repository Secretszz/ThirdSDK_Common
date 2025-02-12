
// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IBridge.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/20 19:54:19
// *******************************************

namespace Bridge.Common
{
	/// <summary>
	/// 
	/// </summary>
	public interface IBridge
	{
		/// <summary>
		/// 初始化
		/// </summary>
		/// <param name="listener">回调事件</param>
		void Init(IBridgeListener listener);

		/// <summary>
		/// 振动
		/// </summary>
		/// <param name="effectType">振动等级</param>
		/// <param name="listener">回调事件</param>
		void Vibrator(VibratorEffectType effectType, IBridgeListener listener);

		/// <summary>
		/// 获取国家信息
		/// </summary>
		/// <param name="listener">回调事件</param>
		void GetCountryInfo(IBridgeListener listener);
	
		/// <summary>
		/// 一键拉起加QQ群
		/// </summary>
		/// <param name="qqGroupValue">加群参数</param>
		/// <param name="listener">加群回调</param>
		void JoinQQGroup(string qqGroupValue, IBridgeListener listener);
	}
}