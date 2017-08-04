using ISoftSmart.Core.WebApi.Parser;
using ISoftSmart.Core.WebApi.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
//using _800Tele.APIServices.Parser;
//using _800Tele.APIServices.Util;

namespace ISoftSmart.Core.WebApi
{
    public class APIClient : IAPIClient
    {
        //private readonly ILog _log = LogManager.GetLogger("Token");

        private string apiUrl = "http://localhost/resource/";
        //private string clientId = "1f6fbc67ec8f441c9fce28f86e4fb026";
        //private string clientSecret = "XNowo2mR85cOOmj1rVpDvGABe2oD68QBbwSyEnzox3c";
        private WebUtil webUtil = null;

        public bool DisableParser { get; set; }


        public APIClient(string serverUrl)
        {
            DisableParser = false;
            this.apiUrl = serverUrl.TrimEnd('/');

            webUtil = new WebUtil();
        }

        /// <summary>
        /// API调用接口通用方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="request"></param>
        /// <returns></returns>
        public T Execute<T>(IAPIRequest<T> request) where T : APIResponse
        {
            try
            {
                var tryTimes = 0;
                while (tryTimes < 3)
                {
                    T t = null;
                    try
                    {
                        t = DoExcute(request);
                    }
                    catch { }
                    if (t != null) return t;
                    tryTimes++;
                    //_log.Info("error, request is retrying " + tryTimes);
                    Thread.Sleep(1000);
                }
                throw new Exception("have tried 3 times, failure request!");
            }
            catch (Exception ex)
            {
                //_log.Error(ex.Message);
                //_log.Error(ex.StackTrace);
                throw new Exception("Request Excuete error.");
            }
        }

        private T DoExcute<T>(IAPIRequest<T> request) where T : APIResponse
        {
            try
            {
                // 构建请求地址
                string requestUrl = string.Empty;
                if (!string.IsNullOrEmpty(request.ApiWXRequestSample))
                {
                    requestUrl = request.ApiWXRequestSample;
                }
                else
                {
                    requestUrl = request.ApiRequestSample.Replace("%BASE_REST_URL%", apiUrl);
                }
                // request
                request.PrepareParam();
                var body = string.Empty;
                var method = request.RequestMethod.ToLower();
                if (!request.IsJsonRequest)
                {
                    if (method == "post")
                    {
                        body = webUtil.DoPost(requestUrl, request.RequestBodyDictionary);
                    }
                    else if (method == "get")
                    {
                        body = webUtil.DoGet(requestUrl, request.RequestBodyDictionary);
                    }
                }
                else
                {
                    var json = request.GetParamJson();
                    body = webUtil.DoJsonRequest(requestUrl, json, request.RequestMethod, request.Token);
                }
                // process response
                T rsp;
                if (DisableParser)
                {
                    rsp = Activator.CreateInstance<T>();
                    rsp.Body = body;
                }
                else
                {
                    // json convert
                    IParser<T> parser = new JsonParser<T>();
                    rsp = parser.Parse(body);
                }
                return rsp;
            }
            catch (Exception ex)
            {
                //_log.Error(ex.Message);
                //_log.Error(ex.StackTrace);
                return null;
            }
        }

        /// <summary>
        /// 签名sign为通过接口双方约定的密钥appsecret生成的一个MD5字符串。生成方式为：
        /// 1. 先取字符串timestamp + "&" + appsecret；（统一取小写值）
        /// 2. 再取上个字符串的utf8字符集的URL编码；（统一取小写值）
        /// 3. 最后再取字符串的16进制md5值；（统一取小写值）
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="appsecret"></param>
        /// <returns></returns>
        private string CreateSign(string timestamp, string appsecret)
        {
            var str = timestamp + "&" + appsecret;
            // ReSharper disable once PossibleNullReferenceException
            var urlEncode =HttpUtility.UrlEncode(str.ToLower(), Encoding.UTF8).ToLower();
            var md5Str = CommonUtil.Md5MSDN(urlEncode, Encoding.UTF8);
            return md5Str.ToLower();
        }
    }
}
