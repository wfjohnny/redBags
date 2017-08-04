using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.WebApi
{
    public interface IAPIRequest<T> where T : APIResponse
    {
        /// <summary>
        /// 获取API名称。
        /// </summary>
        /// <returns>API名称</returns>
        String ApiRequestSample { get; }
        String ApiWXRequestSample { get; set; }

        string RequestMethod { get; set; }
        string Token { get; set; }

        bool IsJsonRequest { get; set; }

        SDictionary RequestBodyDictionary { get; }

        String GetParamJson();

        void PrepareParam();

        /// <summary>
        /// 提前验证参数。
        /// </summary>
        void Validate();
    }
}
