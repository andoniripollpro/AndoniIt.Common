using System;
using System.Diagnostics;
using System.Reflection;

namespace AndoIt.Common
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
				
		public void Fatal(string message, Exception exception = null, StackTrace stackTrace = null, params object[] paramValues)
		{
			this.incidenceEscalator?.Fatal(message, exception, stackTrace);
			if (stackTrace != null) message = $"{StackTraceToString(stackTrace)}: {message}{Environment.NewLine}"
					+ $"Params: {ParamsToString(stackTrace.GetFrame(2).GetMethod(), paramValues)}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Fatal(message, exception);
		}		
		public void Error(string message, Exception exception = null, StackTrace stackTrace = null, params object[] paramValues)
		{
			this.incidenceEscalator?.Error(message, exception, stackTrace);
			if (stackTrace != null) message = $"{StackTraceToString(stackTrace)}: {message}{Environment.NewLine}"
					+ $"Params: {ParamsToString(stackTrace.GetFrame(2).GetMethod(), paramValues)}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Error(message, exception);
		}				
		public void Warn(string message, Exception exception = null, StackTrace stackTrace = null)
		{			
			if (stackTrace != null) message = $"{StackTraceToString(stackTrace)}: {message}{Environment.NewLine}{stackTrace.ToString()}";
			this.wrappedLog.Warn(message, exception);
		}		
		public void Info(string message, StackTrace stackTrace = null)
		{
			if (stackTrace != null) message = $"{StackTraceToString(stackTrace)}: {message}";
			this.wrappedLog.Info(message);			
		}				
		public void Debug(string message, StackTrace stackTrace = null)
		{
			if (stackTrace != null) message = $"{StackTraceToString(stackTrace)}: {message}";
			this.wrappedLog.Debug(message);
		}
		private string StackTraceToString(StackTrace stackTrace)
		{
			return $"{stackTrace?.GetFrame(0).GetMethod().ReflectedType.Name}.{stackTrace?.GetFrame(0).GetMethod().Name}";
		}
		private string ParamsToString(MethodBase method, params object[] values)
		{
			ParameterInfo[] parms = method.GetParameters();
			object[] namevalues = new object[2 * parms.Length];

			string msg = "(";
			for (int i = 0, j = 0; i < parms.Length; i++, j += 2)
			{
				msg += "{" + j + "}={" + (j + 1) + "}, ";
				namevalues[j] = parms[i].Name;
				if (i < values.Length) namevalues[j + 1] = values[i];
			}
			msg += ")";
			return string.Format(msg, namevalues);
		}
	}
}
