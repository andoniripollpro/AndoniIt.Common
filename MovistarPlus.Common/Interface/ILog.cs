using System;
using System.Diagnostics;

namespace MovistarPlus.Common.Interface
{
	public interface ILog
	{		
		void Debug(string message, Exception exception = null, StackTrace stackTrace = null);
		void Error(string message, Exception exception = null, StackTrace stackTrace = null);	
		void Fatal(string message, Exception exception = null, StackTrace stackTrace = null);	
		void Info(string message, Exception exception = null, StackTrace stackTrace = null);	
		void Warn(string message, Exception exception = null, StackTrace stackTrace = null);
	}
}