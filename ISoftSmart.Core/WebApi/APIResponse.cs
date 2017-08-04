using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ISoftSmart.Core.WebApi
{
    [Serializable]
    public class APIResponse
    {
        [JsonProperty("code")]
        public String Code { get; set; }

        [JsonProperty("message")]
        public String ResponseMessage { get; set; }

        /// <summary>
        /// 响应原始内容
        /// </summary>
        public String Body { get; set; }


        /// <summary>
        /// 响应的JSON object
        /// </summary>
        public JObject Json { get; set; }


        /// <summary>
        /// HTTP GET请求的URL
        /// </summary>
        public string ReqUrl { get; set; }

        /// <summary>
        /// 响应结果是否错误
        /// </summary>
        [JsonProperty("isError")]
        public virtual bool IsError => string.IsNullOrEmpty(this.Code) || (!string.IsNullOrEmpty(this.Code)
                                                                           &&
                                                                           !string.Equals(this.Code, "SUCCESS",
                                                                               StringComparison
                                                                                   .InvariantCultureIgnoreCase)
            );
    }
}
