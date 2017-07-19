using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ISoftSmart.API.Configuration
{
    [Serializable]
    public class Cors
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("Orgins")]
        public string Orgins { get; set; }

        [XmlAttribute("Headers")]
        public string Headers { get; set; }

        [XmlAttribute("Methods")]
        public string Methods { get; set; }

        [XmlAttribute("Credentials")]
        public string Credentials { get; set; }
    }
}