using AndoIt.Common.Interface;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace AndoIt.Common.Common
{
	public static class OracleConnectionExtension
	{
		public static void EnsureDatabaseConnection(this OracleConnection oracleConnection)
        {
            ILog log = GetLog();
            try
            {
                new Insister(log).Insist(new Action(() => oracleConnection.OpenIfNeeded()), 5);
            }
            catch (Exception e)
            {
                log?.Warn("EnsureDatabaseConnection. Connection to database lost.", e);
            }
        }

        private static ILog GetLog()
        {
            ILog log = null;
            try
            {
                log = IoCObjectContainer.Singleton.Get<ILog>();
            }
            catch { Console.WriteLine("WARNING: Log estándar AndoIt no asignado"); }

            return log;
        }

        public static void OpenIfNeeded(this OracleConnection oracleConnection)
		{
            var log = GetLog();
            if (oracleConnection.State != ConnectionState.Open)
			{
				oracleConnection.Open();
				log?.Debug("Connection.Open();");
			}
			else
			{
				log?.Debug("Connection already openned");				
			}
		}
	}
}
