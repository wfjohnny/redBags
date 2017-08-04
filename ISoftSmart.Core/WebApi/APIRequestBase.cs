using ISoftSmart.Core.WebApi.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISoftSmart.Core.WebApi
{
    public abstract class APIRequestBase<T> : IAPIRequest<T> where T : APIResponse
    {
        IDictionary<string, object> _addedParams = new Dictionary<string, object>();

        SDictionary _requestBodyDictionary = new SDictionary();

        /// <summary>
        /// 附加参数
        /// </summary>
        public IDictionary<String, Object> AddedParam
        {
            get
            {
                return _addedParams;
            }
        }

        public SDictionary RequestBodyDictionary
        {
            get { return _requestBodyDictionary; }
        }

        /// <summary>
        /// API方法名称
        /// </summary>
        public abstract string ApiRequestSample { get; }

        public virtual string RequestMethod { get; set; }

        public string Token { get; set; }

        public bool IsJsonRequest { get; set; }

        public string ApiWXRequestSample
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// 增加附加参数
        /// </summary>
        /// <param name="key">参数名</param>
        /// <param name="value">参数值</param>
        public void AddParam(String key, Object value)
        {
            _addedParams.Add(key, value);
        }

        /// <summary>
        /// 增加请求参数
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddRequestParam(string key, object value)
        {
            if (_requestBodyDictionary.ContainsKey(key))
            {
                _requestBodyDictionary.Remove(key);
            }
            _requestBodyDictionary.Add(key, value);
        }

        /// <summary>
        /// 移除附加参数
        /// </summary>
        /// <param name="key">参数名</param>
        /// <returns></returns>
        public Boolean RemoveParam(String key)
        {
            return _addedParams.Remove(key);
        }

        /// <summary>
        /// 移除请求参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool RemoveRequestParam(string key)
        {
            return _requestBodyDictionary.Remove(key);
        }

        protected abstract void PrepareParam(IDictionary<String, Object> paramters);

        public abstract void PrepareParam();

        public virtual String GetParamJson()
        {
            var paramters = new Dictionary<String, Object>();
            PrepareParam(paramters);
            foreach (var added in _addedParams)
            {
                if (!paramters.ContainsKey(added.Key))
                {
                    paramters.Add(added.Key, added.Value);
                }
            }

            return JsonConvert.SerializeObject(paramters, CommonUtil.GetJsonConverters());
        }

        public virtual void Validate()
        {
        }
    }
}
