
// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IInitListener.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/03 10:32:47
// *******************************************

namespace Bridge.Common
{
	/// <summary>
	/// 
	/// </summary>
	public interface IInitListener
	{
		void OnSuccess();

		void OnError(int errCode, string errMsg);
	}
}