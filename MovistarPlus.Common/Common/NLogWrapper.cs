using MovistarPlus.Common.Interface;
using NLog;
using System;
using System.Diagnostics;

namespace MovistarPlus.Common
{
	public class NLogWrapper: ILog
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
			if (stackTrace != null) message = $"{message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Fatal(exception, message);			
		}		
		public void Error(string message, Exception exception = null, StackTrace stackTrace = null)
		{
			this.incidenceEscalator?.Error(message, exception, stackTrace);
			if (stackTrace != null) message = $"{message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Error(exception, message);
		}				
		public void Warn(string message, Exception exception = null, StackTrace stackTrace = null)
		{			
			if (stackTrace != null) message = $"{message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Warn(exception, message);
		}		
		public void Info(string message, Exception exception = null, StackTrace stackTrace = null)
		{
			if (stackTrace != null) message = $"{message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Info(exception, message);			
		}				
		public void Debug(string message, Exception exception = null, StackTrace stackTrace = null)
		{
			if (stackTrace != null) message = $"{message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Debug(exception, message);
		}
	}
}
