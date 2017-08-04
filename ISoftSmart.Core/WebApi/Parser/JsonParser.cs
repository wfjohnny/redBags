using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ISoftSmart.Core.WebApi.Util;

namespace ISoftSmart.Core.WebApi.Parser
{
    public class JsonParser<T> : IParser<T> where T : APIResponse
    {
        public T Parse(string body)
        {
            T rsp = null;
            JObject json = JObject.Parse(body);
            if (json != null && json.First != null)
            {
                //var data = (JObject)json.First.First;
                //if (data != null)
                //{
                //    rsp = data.ToObject<T>(GetJsonSerializer());
                //}
                rsp = json.ToObject<T>(GetJsonSerializer());
            }

            if (rsp == null)
            {
                rsp = Activator.CreateInstance<T>();
            }

            if (rsp != null)
            {
                rsp.Body = body;
                rsp.Json = json;
            }

            return rsp;
        }

        private static JsonSerializer _jsonSerializer = null;
        public static JsonSerializer GetJsonSerializer()
        {
            if (_jsonSerializer == null)
            {
                var settings = new JsonSerializerSettings
                {
                    Converters = new List<JsonConverter>(CommonUtil.GetJsonConverters())
                };
                _jsonSerializer = JsonSerializer.Create(settings);
            }

            return _jsonSerializer;
        }

    }
}

