using System.Diagnostics;

namespace MovistarPlus.Common
{
	public static class ProcessExtension
	{
		public static bool IsAsp(this Process process)
		{
			if (process.ProcessName == "w3wp"
				|| process.ProcessName == "aspnet_wp"
				|| process.ProcessName == "iisexpress")
				return true;
			else
				return false;
		}
	}
}
