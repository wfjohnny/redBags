using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ISoftSmart.API.Configuration
{
    [Serializable]
    public class DefaultConfiguration
    {
        [XmlAttribute("CacheEnable")]
        public string CacheEnable { get; set; }

        [XmlAttribute("CorsUsed")]
        public string CorsUsed { get; set; }
    }
}