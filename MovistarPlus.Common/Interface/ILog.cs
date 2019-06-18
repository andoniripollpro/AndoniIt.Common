using System;
using System.Diagnostics;

namespace MovistarPlus.Common.Interface
{
	public interface ILog
	{
		void Fatal(string message, Exception exception = null, StackTrace stackTrace = null);
		void Warn(string message, Exception exception = null, StackTrace stackTrace = null);
		void Error(string message, Exception exception = null, StackTrace stackTrace = null);		
		void Info(string message, StackTrace stackTrace = null);	
		void Debug(string message, StackTrace stackTrace = null);
	}
}