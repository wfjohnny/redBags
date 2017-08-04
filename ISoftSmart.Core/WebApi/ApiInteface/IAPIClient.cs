using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.WebApi
{
    public interface IAPIClient
    {
        /// <summary>
        /// 执行公开API请求。
        /// </summary>
        /// <typeparam name="T">领域对象</typeparam>
        /// <param name="request">具体的Jd API请求</param>
        /// <returns>领域对象</returns>
        T Execute<T>(IAPIRequest<T> request) where T : APIResponse;
    }
}