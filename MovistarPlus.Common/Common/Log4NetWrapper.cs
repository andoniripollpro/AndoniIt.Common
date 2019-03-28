using System;
using System.Diagnostics;

namespace MovistarPlus.Common
{
	public class Log4NetWrapper : Interface.ILog
	{		
		private readonly log4net.ILog wrappedLog;
		private readonly Interface.ILog incidenceEscalator;

		public const string APP_NAME = "ServicioDaypartSaver";

		public Log4NetWrapper(log4net.ILog wrappedLog, Interface.ILog incidenceEscalator = null)
		{
			this.wrappedLog = wrappedLog ?? throw new ArgumentNullException("wrappedLog");
			this.incidenceEscalator = incidenceEscalator;
		}
				
		public void Fatal(string message, Exception exception = null, StackTrace stackTrace = null)
		{
			this.incidenceEscalator?.Fatal(message, exception, stackTrace);
			if (stackTrace != null) message = $"{StackTraceToString(stackTrace)}: {message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Fatal(message, exception);
		}		
		public void Error(string message, Exception exception = null, StackTrace stackTrace = null)
		{
			this.incidenceEscalator?.Error(message, exception, stackTrace);
			if (stackTrace != null) message = $"{StackTraceToString(stackTrace)}: {message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Error(message, exception);
		}				
		public void Warn(string message, Exception exception = null, StackTrace stackTrace = null)
		{			
			if (stackTrace != null) message = $"{StackTraceToString(stackTrace)}: {message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Warn(message, exception);
		}		
		public void Info(string message, Exception exception = null, StackTrace stackTrace = null)
		{
			if (stackTrace != null) message = $"{StackTraceToString(stackTrace)}: {message}{Environment.NewLine}";
			this.wrappedLog.Info(message, exception);			
		}				
		public void Debug(string message, Exception exception = null, StackTrace stackTrace = null)
		{
			if (stackTrace != null) message = $"{StackTraceToString(stackTrace)}: {message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Debug(message, exception);
		}
		private string StackTraceToString(StackTrace stackTrace)
		{
			return $"{stackTrace?.GetFrame(0).GetMethod().ReflectedType.Name}.{stackTrace?.GetFrame(0).GetMethod().Name}";
		}
	}
}
