using System;
using System.Configuration;
using System.Xml;
using MovistarPlus.Common.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MovistarPlus.Common
{
	public class GoodConfigOnBoxDb : IGoodConfig
	{
		private readonly string iBoxDbLocation;
		private ConfigOnBoxDao.ILog log = null;

		public GoodConfigOnBoxDb(string iBoxDbLocation)
		{
			this.iBoxDbLocation = iBoxDbLocation;			
		}

		public static IGoodConfig CreateConfig
		{
			get
			{				
				return new GoodConfigOnBoxDb(ConfigurationManager.AppSettings["iBoxDbLocation"]);
			}
		}

		public ConfigOnBoxDao.ILog Log { get => log; set => log = value; }
				
		public void AddUpdateFromJToken(JToken configuration)
		{
			using (ConfigOnBoxDao configOnBoxDao = new ConfigOnBoxDao(this.iBoxDbLocation, this.Log))
			{
				configOnBoxDao.AddUpdateFromString(JsonConvert.SerializeObject(configuration));				
			};
		}

		public JToken GetJNodeByTagAddress(string tagAddress)
		{
			int i = 0;
			do
			{
				try
				{
					using (ConfigOnBoxDao configOnBoxDao = new ConfigOnBoxDao(this.iBoxDbLocation, this.Log))
					{
						string content = configOnBoxDao.GetContent();
						JObject json = JObject.Parse(content);
						return (JToken)json["Config"][tagAddress];
					};
				}
				catch (Exception ex)
				{
					this.log?.Message($"GetJNodeByTagAddress: Algo me impice acceder a la configuración. Mensaje:  {ex.Message}");
					i++;
					if (i >= 100)
						throw;
				}
			} while (true);
		}

		public XmlNode GetXmlNodeByTagAddress(string tagAddress)
		{
			int i = 0;
			do
			{
				try
				{
					using (ConfigOnBoxDao configOnBoxDao = new ConfigOnBoxDao(this.iBoxDbLocation, this.Log))
					{
						string content = GetRootJString(configOnBoxDao);
						XmlDocument doc = JsonConvert.DeserializeXmlNode(content);
						return doc.ChildNodes[0][tagAddress];
					};
				}
				catch (Exception ex)
				{
					this.log?.Message($"GetXmlNodeByTagAddress: Algo me impide acceder a la configuración. Mensaje:  {ex.Message}");
					System.Threading.Thread.Sleep(100);
					if (i >= 100)
						throw;
					i++;
				}
			} while (true);			
		}

		private string GetRootJString(ConfigOnBoxDao configOnBoxDao)
		{
			string content = configOnBoxDao.GetContent();
			if (string.IsNullOrWhiteSpace(content))
				throw new ConfigurationErrorsException($"iBoxDb no contiene configuración válida en la tabla de configuración que se encuntra en {this.iBoxDbLocation}db1.box. Contenido de la configuración: '{content}'");
			return content;
		}
	}
}
