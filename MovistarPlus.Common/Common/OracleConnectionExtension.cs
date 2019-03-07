using MovistarPlus.Common.Interface;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace MovistarPlus.Common.Common
{
	static class OracleConnectionExtension
	{
		public static void EnsureDatabaseConnection(this OracleConnection oracleConnection)
		{
			var log = IoCObjectContainer.Singleton.Get<ILog>();
			for (int i = 0; i < 100; i++)
			{
				try
				{
					if (oracleConnection.State != ConnectionState.Open)
					{
						oracleConnection.Open();
						log.Debug("Connection.Open();");
					}
					else
					{
						log.Debug("Connection already openned");
						break;
					}
				}
				catch (Exception e)
				{
					log.Warn("EnsureDatabaseConnection. Connection to database lost.", e);
				}
			}
		}

	}
}
