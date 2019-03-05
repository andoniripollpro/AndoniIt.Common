﻿using Newtonsoft.Json.Linq;
using System.Xml;

namespace MovistarPlus.Common.Interface
{
	public interface IGoodConfig
	{
		XmlNode GetXmlNodeByTagAddress(string tagAddress);
		JToken GetJNodeByTagAddress(string tagAddress);
		void AddUpdateFromJToken(JToken configuration);
	}
}