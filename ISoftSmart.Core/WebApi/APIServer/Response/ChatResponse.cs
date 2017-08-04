using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace ISoftSmart.Core.WebApi.APIServer.Response
{
    public class ChatResponse<T> : APIResponse
    {
        [JsonProperty("result")]
        public T Result { get; set; }
    }
}
