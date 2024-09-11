// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		BridgeCallback.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/11 10:37:31
// *******************************************

namespace Bridge.Common
{
	using UnityEngine;

	/// <summary>
	/// 成功回调
	/// </summary>
	public delegate void U3DBridgeCallback_Success(string result);

	/// <summary>
	/// 取消回调
	/// </summary>
	public delegate void U3DBridgeCallback_Cancel();

	/// <summary>
	/// 错误回调
	/// </summary>
	public delegate void U3DBridgeCallback_Error(int errCode, string errMsg);

	/// <summary>
	/// 
	/// </summary>
	public class BridgeCallback : AndroidJavaProxy
	{
		public BridgeCallback(IBridgeListener listener) : base("com.bridge.common.listener.IBridgeListener")
		{
			this.listener = listener;
		}

		private IBridgeListener listener;

		/// <summary>
		/// 成功回调
		/// </summary>
		/// <param name="result"></param>
		public void onSuccess(string result)
		{
			listener?.OnSuccess(result);
		}

		/// <summary>
		/// 取消回调
		/// </summary>
		public void onCancel()
		{
			listener.OnCancel();
		}

		/// <summary>
		/// 错误回调
		/// </summary>
		/// <param name="errCode"></param>
		/// <param name="errStr"></param>
		public void onError(int errCode, string errStr)
		{
			listener.OnError(errCode, errStr);
		}
	}
}