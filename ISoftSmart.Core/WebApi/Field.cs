﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ISoftSmart.Core.WebApi
{
    [Serializable]
    public class Field : SObject
    {
        [JsonProperty("key")]
        public string Key
        {
            get;
            set;
        }

        [JsonProperty("value")]
        public string Value
        {
            get;
            set;
        }
    }
}
