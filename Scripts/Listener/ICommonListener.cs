// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		ICommonListener.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/03 18:56:54
// *******************************************

namespace Bridge.Common
{
	/// <summary>
	/// 
	/// </summary>
	public interface ICommonListener
	{
		void OnResult(int errCode, string errMsg);
	}
}