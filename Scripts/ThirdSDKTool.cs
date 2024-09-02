namespace Bridge.Common
{
	using System;
	using System.Linq;

	public static class ThirdSDKTool
	{
		public const string WxApiAsssemblyName = "Bridge.WxApi";
		public const string XhsApiAsssemblyName = "Bridge.XhsApi";
		
		public static bool IsOpenWxApi()
		{
			return IsOpenApi(WxApiAsssemblyName);
		}
		
		public static bool IsOpenXhsApi()
		{
			return IsOpenApi(XhsApiAsssemblyName);
		}

		public static bool IsOpenApi(string startWith)
		{
			return AppDomain.CurrentDomain.GetAssemblies().Any(asssembly => asssembly.GetName().FullName.StartsWith(startWith));
		}
	}
}