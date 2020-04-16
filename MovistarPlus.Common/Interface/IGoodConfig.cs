using Newtonsoft.Json.Linq;
using System.Xml;

namespace MovistarPlus.Common.Interface
{
	public interface IGoodConfig
	{
		string ConnectionString { get; }
		XmlNode GetXmlNodeByTagAddress(string tagAddress);
		JToken GetJNodeByTagAddress(string tagAddress = null);
		void AddUpdateFromJToken(JToken configuration);
	}
}