using System.Diagnostics;

namespace MovistarPlus.Common.Common
{
	public static class StackTraceExtender
	{
		public static string ToStringClassMethod(this StackTrace stackTrace)
		{			
			var callingStackTraceFrame = new StackTrace().GetFrame(2);
			return $"{callingStackTraceFrame.GetMethod().ReflectedType.Name}.{callingStackTraceFrame.GetMethod().Name}";
		}
	}
}