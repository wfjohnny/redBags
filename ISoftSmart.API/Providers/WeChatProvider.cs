using ISoftSmart.Common;
using log4net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;

namespace ISoftSmart.API.Providers
{
    public class WeChatProvider
    {
        private static readonly ILog _log = LogManager.GetLogger("WeChatServer");
        ///微信配置信息
        public static readonly string AppId = System.Configuration.ConfigurationManager.AppSettings["AppId"].ToString();
        public static readonly string AppSecret = System.Configuration.ConfigurationManager.AppSettings["AppSercret"].ToString();
        ///微信接口 
        public static readonly string ApiAccessTokenUrl = ConfigurationManager.AppSettings["TokenUrl"];
        public static readonly string ApiJsapi_tiketUrl = ConfigurationManager.AppSettings["JsTiketUrl"];

        public static bool RemoteCertificateValidationCallback(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;
            return false;
        }
        /// <summary>
        /// 获取access_token （已添加缓存）
        /// </summary>
        /// <returns></returns>
        public static string GetAccessTokenStr()
        {
            if (CacheHelper.GetCache("wxToken") == null)
            {
                string reqponesJson = HttpHelper.SendHttpGet(string.Format(ApiAccessTokenUrl + "&appid={0}&secret={1}&type=1", AppId, AppSecret));
                var ja = JsonConvert.DeserializeObject<JObject>(reqponesJson);
                if (ja != null)
                {
                    CacheHelper.SetCache("wxToken", ja["access_token"], 7200);
                    return ja["access_token"].ToString();
                }
                _log.Error(reqponesJson);
                return null;
            }
            else
                return CacheHelper.GetCache("wxToken").ToString();
        }
        /// <summary>
        /// 获取微信JS sdk   Jsticket (已添加缓存)
        /// </summary>
        /// <returns></returns>
        public static string GetJsapiTicketStr()
        {
            if (CacheHelper.GetCache("wxTicket") == null)
            {
                var tiketJson = HttpHelper.SendHttpGet(string.Format(ApiJsapi_tiketUrl + "?access_token={0}&type=jsapi", GetAccessTokenStr()));
                var retJson = JsonConvert.DeserializeObject<JObject>(tiketJson);
                if (retJson != null && retJson["ticket"] != null)
                {
                    CacheHelper.SetCache("wxTicket", retJson["ticket"]);
                    return retJson["ticket"].ToString();
                }
                _log.Error(tiketJson);
                return null;
            }
            return CacheHelper.GetCache("wxTicket").ToString();
        }
    }
}