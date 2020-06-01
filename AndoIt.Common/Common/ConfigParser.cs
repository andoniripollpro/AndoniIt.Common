using AndoIt.Common.Interface;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.Xml;

namespace AndoIt.Common.Common
{
    public abstract class ConfigParser : IGoodConfig
    {
        public abstract string ConnectionString { get; }

        public abstract void AddUpdateFromJToken(JToken configuration);
        public abstract JToken GetJNodeByTagAddress(string tagAddress = null);
        public abstract XmlNode GetXmlNodeByTagAddress(string tagAddress);
        public abstract void ReloadConfig();
        
        public int GetAsInt(string tagAddress)
        {            
            try
            {
                return int.Parse(this.GetJNodeByTagAddress(tagAddress).ToString());
            }
            catch(Exception ex)  
            {
                throw new ConfigurationErrorsException($"No existe, o no está bien expresado (int), el valor con tagAddress '{tagAddress}' en la configuración", ex);
            }
        }
        public string GetAsString(string tagAddress)
        {
            try
            {
                return this.GetJNodeByTagAddress(tagAddress).ToString();
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException($"No existe, o no está bien expresado (string), el valor con tagAddress '{tagAddress}' en la configuración", ex);
            }
        }

        
        
    }
}
