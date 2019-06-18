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
		private readonly IIoCObjectContainer ioCObjectContainer;
		private readonly string connectionString;
		private readonly string appId;
		private readonly int secondsToRefreshConfig;
		private readonly ILog log;
		private string configurationInJson;
		private DateTime dataBaseLastRead;

		public GoodConfigOnDao(IIoCObjectContainer ioCObjectContainer, string connectionString, string appId, int secondsToRefreshConfig = 0)
		{
			this.ioCObjectContainer = ioCObjectContainer ?? throw new ArgumentNullException("ioCObjectContainer");
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentNullException("connectionString");
			this.connectionString = connectionString;
			if (string.IsNullOrWhiteSpace(appId))
				throw new ArgumentNullException("appId");
			this.appId = appId ?? throw new ArgumentNullException("appId");
			this.secondsToRefreshConfig = secondsToRefreshConfig;
			this.log = this.ioCObjectContainer.Get<ILog>();
			this.configurationInJson = new Insister(this.log).Insist<string>(() => GetRootJStringFromDB() , 2);
			this.dataBaseLastRead = DateTime.Now;
			this.log.Debug($"Ended: connectionString: {this.connectionString}", new System.Diagnostics.StackTrace());
		}

		public ILog Log => log;

		public void AddUpdateFromJToken(JToken configuration)
		{
			throw new NotImplementedException();
		}

		public JToken GetJNodeByTagAddress(string tagAddress = null)
		{
			string json = GetRootJString();
			JToken jTokenParsed = JToken.Parse(json);
			JToken jTokenResult = GetJNodeByTagAddressOnJNode(jTokenParsed, tagAddress);
			return jTokenResult ?? throw new ConfigurationErrorsException($"El tagAddress '{tagAddress}' me da nulo en la configuración");
		}

		public JToken GetJNodeByTagAddressOnJNode(JToken jToken, string tagAddress)
		{
			if (tagAddress != null)
			{
				if (!tagAddress.Contains("."))
					return jToken[tagAddress];
				else
				{
					int indexOfDot = tagAddress.IndexOf('.');
					string prefix = tagAddress.Substring(0, indexOfDot);
					string sufix = tagAddress.Substring(indexOfDot + 1, tagAddress.Length - indexOfDot - 1);
					return GetJNodeByTagAddressOnJNode(jToken[prefix], sufix);
				}
			}
			else
				return jToken;
		}

		public XmlNode GetXmlNodeByTagAddress(string tagAddress)
		{
			string content = GetRootJString();
			XmlDocument doc = JsonConvert.DeserializeXmlNode(content);
			return doc.ChildNodes[0][tagAddress];
		}
		private string GetRootJString()
		{
			//	Refresh config each this.secondsToRefreshConfig OR each hour by default 
			if ((this.secondsToRefreshConfig != 0 && DateTime.Now >= this.dataBaseLastRead.AddSeconds(this.secondsToRefreshConfig))
				|| (this.secondsToRefreshConfig == 0 && DateTime.Now >= this.dataBaseLastRead.AddHours(1)))
			{
				this.configurationInJson = new Insister(this.log).Insist<string>(() => GetRootJStringFromDB(), 2);
				this.dataBaseLastRead = DateTime.Now;
			}
			return this.configurationInJson;
		}
		private const string SELECT_CONFIG = "Select CONFIGURACION from VOD_APP_CONFIG " +
			"where APP_ID = '{appId}' " +
			"and (F_I_VIGENCIA is null or SYSDATE >= F_I_VIGENCIA) " +
			"and (F_F_VIGENCIA is null or SYSDATE < F_F_VIGENCIA) ";

		private string GetRootJStringFromDB()
		{
			var connection = new OracleConnection(this.connectionString);
			connection.EnsureDatabaseConnection();
			string query = SELECT_CONFIG.Replace("{appId}", this.appId);
			this.log.Debug(query);
			var configurationList = connection.Query<dynamic>(query).ToList();

			if (configurationList.Count() == 0)
				throw new ConfigurationErrorsException($"Existen {configurationList.Count()} registros de configuración en la base de datos para este sistema y rango de fechas, y se espera sólo 1");
			else
			{
				string configurationInJson = configurationList[0].CONFIGURACION.ToString();
				if (configurationList.Count() > 1) this.log.Error($"Existen {configurationList.Count()} registros de configuración en la base de datos para este sistema y rango de fechas, y se espera sólo 1");
				//this.log.Info($"Esta es la configuración leída de la BD: {Environment.NewLine}{configurationInJson}");
				return configurationInJson;
			}
		}
	}
}