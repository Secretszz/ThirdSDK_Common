
// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IBridgeListener.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/11 10:35:39
// *******************************************

namespace Bridge.Common
{
	/// <summary>
	/// 通信回调
	/// </summary>
	public interface IBridgeListener
	{
		/// <summary>
		/// 成功事件
		/// </summary>
		/// <param name="result"></param>
		void OnSuccess(string result);

		/// <summary>
		/// 用户取消事件
		/// </summary>
		void OnCancel();

		/// <summary>
		/// 错误事件
		/// </summary>
		/// <param name="errCode"></param>
		/// <param name="errMsg"></param>
		void OnError(int errCode, string errMsg);
	}
}