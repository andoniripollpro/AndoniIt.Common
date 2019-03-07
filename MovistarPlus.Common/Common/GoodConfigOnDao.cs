using System;
using System.Configuration;
using System.Linq;
using System.Xml;
using Dapper;
using MovistarPlus.Common.Common;
using MovistarPlus.Common.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Oracle.ManagedDataAccess.Client;

namespace MovistarPlus.Common
{
	public class GoodConfigOnDao : IGoodConfig
	{
		private readonly ConnectionStringSettings connectionStringSettings;
		private readonly ILog log;
		private readonly string configurationInJson;

		public GoodConfigOnDao(ConnectionStringSettings connectionStringSettings)
		{
			this.connectionStringSettings = connectionStringSettings ?? throw new ArgumentNullException("connectionStringSettings");
			this.log = IoCObjectContainer.Singleton.Get<ILog>();
			this.configurationInJson = GetRootJStringFromDB();
		}

		public void AddUpdateFromJToken(JToken configuration)
		{
			throw new NotImplementedException();
		}

		public JToken GetJNodeByTagAddress(string tagAddress)
		{
			string json = GetRootJString();
			JToken jToken = JToken.Parse(json);
			return jToken[tagAddress];
		}

		public XmlNode GetXmlNodeByTagAddress(string tagAddress)
		{
			string content = GetRootJString();
			XmlDocument doc = JsonConvert.DeserializeXmlNode(content);
			return doc.ChildNodes[0][tagAddress];
		}
		private string GetRootJString()
		{
			return configurationInJson;
		}
		private const string SELECT_CONFIG = "Select CONFIGDATA from CONFIG " +
			"where APPID = 'CodificacionSubida' " +
			"and (F_INI is null or SYSDATE >= F_INI) " +
			"and (F_FIN is null or SYSDATE < F_FIN) ";

		private string GetRootJStringFromDB()
		{
			var connection = new OracleConnection(this.connectionStringSettings.ConnectionString);
			connection.EnsureDatabaseConnection();
			this.log.Debug(SELECT_CONFIG);
			var configurationList = connection.Query<dynamic>(SELECT_CONFIG).ToList();

			if (configurationList.Count() == 0)
				throw new ConfigurationErrorsException($"Existen {configurationList.Count()} registros de configuración en la base de datos para este sistema y rango de fechas, y se espera sólo 1");			
			else
			{
				string configurationInJson = configurationList[0].CONFIGDATA.ToString();
				if (configurationList.Count() > 1) this.log.Error($"Existen {configurationList.Count()} registros de configuración en la base de datos para este sistema y rango de fechas, y se espera sólo 1");
				this.log.Info($"Esta es la configuración leída de la BD: {Environment.NewLine}{configurationInJson}");
				return configurationInJson;
			}
		}
	}
}