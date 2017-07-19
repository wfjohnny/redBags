using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace ISoftSmart.API.Configuration
{
    [Serializable]
    public class CorsConfiguration : IConfigurationSectionHandler
    {
        private static readonly Lazy<CorsConfiguration> _instance = new Lazy<CorsConfiguration>(GetConfiguration);
        private CorsConfiguration()
        {

        }

        private static CorsConfiguration GetConfiguration()
        {
            try
            {
                var c = (CorsConfiguration)ConfigurationManager.GetSection("CorsConfiguration");
                if (c == null) throw new ConfigurationErrorsException("configuration missing!");

                return c;
            }
            catch (Exception ex)
            {
                var log = LogManager.GetLogger("CorsConfiguration");
                log.Error(ex.Message);
                log.Error(ex.StackTrace);
                return null;
            }
        }

        public static CorsConfiguration Instance
        {
            get
            {
                var instance = _instance.Value;
                if (instance == null) return null;
                var corsAll = instance.CorsSettings?.OfType<Cors>().ToList();
                instance.DefaultCors = corsAll?.FirstOrDefault(x => x.ID.Equals(instance.DefaultAPICors.CorsUsed)); ;
                return instance;
            }
        }
        public object Create(object parent, object configContext, XmlNode section)
        {
            var srl = new XmlSerializer(this.GetType());
            return srl.Deserialize(new XmlNodeReader(section));
        }

        private static List<Cors> GetAllCors()
        {
            var ins = Instance;
            return ins?.CorsSettings?.OfType<Cors>().ToList();
        }

        private static Cors GetCors(string id)
        {
            var cors = GetAllCors();
            return cors?.FirstOrDefault(x => x.ID.Equals(id));
        }

        [XmlElement("DefaultConfiguration", typeof(DefaultConfiguration))]
        public DefaultConfiguration DefaultAPICors { get; set; }

        [XmlArray("Settings")]
        [XmlArrayItem("Cors", typeof(Cors))]
        public ArrayList CorsSettings { get; set; }

        public Cors DefaultCors { get; set; }
    }
}