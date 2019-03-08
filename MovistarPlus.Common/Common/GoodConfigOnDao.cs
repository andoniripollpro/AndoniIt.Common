﻿using System;
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
		private readonly string connectionString;
		private readonly string appId;
		private readonly ILog log;
		private readonly string configurationInJson;

		public GoodConfigOnDao(string connectionString, string appId)
		{
			if (string.IsNullOrWhiteSpace(connectionString))
				throw new ArgumentNullException("connectionString");
			this.connectionString = connectionString;
			if (string.IsNullOrWhiteSpace(appId))
				throw new ArgumentNullException("appId");
			this.appId = appId;
			this.log = IoCObjectContainer.Singleton.Get<ILog>();
			this.configurationInJson = GetRootJStringFromDB();
		}

		public ILog Log => log;

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
				this.log.Info($"Esta es la configuración leída de la BD: {Environment.NewLine}{configurationInJson}");
				return configurationInJson;
			}
		}
	}
}