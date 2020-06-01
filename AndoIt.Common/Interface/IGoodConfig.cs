using Newtonsoft.Json.Linq;
using System;
using System.Xml;

namespace AndoIt.Common.Interface
{
	public interface IGoodConfig
	{
		string ConnectionString { get; }

		string GetAsString(string tagAddress);
		int GetAsInt(string tagAddress);

		XmlNode GetXmlNodeByTagAddress(string tagAddress);
		JToken GetJNodeByTagAddress(string tagAddress = null);
		void AddUpdateFromJToken(JToken configuration);
        void ReloadConfig();
    }
}