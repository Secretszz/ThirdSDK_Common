
// *******************************************
// Company Name:	深圳市晴天互娱科技有限公司
//
// File Name:		IShareListener.cs
//
// Author Name:		Bridge
//
// Create Time:		2024/09/03 10:42:31
// *******************************************

namespace Bridge.Common
{
	/// <summary>
	/// 
	/// </summary>
	public interface IShareListener
	{
		public void OnSuccess();

		public void OnCancel();

		public void OnError(int errCode, string errMsg);
	}
}