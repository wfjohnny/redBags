using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Model
{
    [Serializable]
    public class APIResponse<T>
    {
        /// <summary>
        /// SUCCESS, FAIL
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string ResponseMessage { get; set; }

        /// <summary>
        /// 响应结果是否错误
        /// </summary>
        [JsonProperty("isError")]
        public virtual bool IsError => string.IsNullOrEmpty(this.Code) ||
                                       (!string.IsNullOrEmpty(this.Code) &&
                                        !string.Equals(this.Code, "SUCCESS", StringComparison.InvariantCultureIgnoreCase))
            ;

        [JsonProperty("result")]
        public T Result { get; set; }
    }
}
