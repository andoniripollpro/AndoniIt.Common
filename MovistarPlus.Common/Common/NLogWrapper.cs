using MovistarPlus.Common.Common;
using MovistarPlus.Common.Interface;
using NLog;
using System;
using System.Diagnostics;

namespace MovistarPlus.Common
{
	public class NLogWrapper: ILog, IDisposable
	{		
		private readonly Logger wrappedLog;
		private readonly ILog incidenceEscalator;

		public const string APP_NAME = "ServicioDaypartSaver";

		public NLogWrapper(Logger wrappedLog, ILog incidenceEscalator = null)
		{
			this.wrappedLog = wrappedLog ?? throw new ArgumentNullException("wrappedLog");			
			this.incidenceEscalator = incidenceEscalator;
		}
				
		public void Fatal(string message, Exception exception = null, StackTrace stackTrace = null)
		{
			this.incidenceEscalator?.Fatal(message, exception, stackTrace);
			if (stackTrace != null) message = $"{stackTrace.ToStringClassMethod()}: {message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Fatal(exception, message);			
		}		
		public void Error(string message, Exception exception = null, StackTrace stackTrace = null)
		{
			this.incidenceEscalator?.Error(message, exception, stackTrace);
			if (stackTrace != null) message = $"{stackTrace.ToStringClassMethod()}: {message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Error(exception, message);
		}				
		public void Warn(string message, Exception exception = null, StackTrace stackTrace = null)
		{
			if (stackTrace != null) message = $"{stackTrace.ToStringClassMethod()}: {message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Warn(exception, message);
		}		
		public void Info(string message, StackTrace stackTrace = null)
		{
			if (stackTrace != null) message = $"{stackTrace.ToStringClassMethod()}: {message}";
			this.wrappedLog.Info(message);			
		}				
		public void Debug(string message, StackTrace stackTrace = null)
		{
			if (stackTrace != null) message = $"{stackTrace.ToStringClassMethod()}: {message}";
			this.wrappedLog.Debug(message);
		}
		public void Dispose()
		{
			LogManager.Flush();
			LogManager.Shutdown();
		}
	}
}
