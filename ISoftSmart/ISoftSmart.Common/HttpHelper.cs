using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ISoftSmart.Common
{
    /// <summary>
    /// create:xulong.ji
    /// date:20170512
    /// des:简单请求封装
    /// </summary>
    internal class AgentInfo
    {
        /// <summary>
        /// 代理信息
        /// </summary>
        public string UserAgent { get; set; }
        /// <summary>
        /// 代理对象名称
        /// </summary>
        public string AgentName { get; set; }

        public AgentInfo(string userAgent, string agentName)
        {
            this.UserAgent = userAgent;
            this.AgentName = agentName;
        }
        // Fields
        public static readonly AgentInfo Chrome = new AgentInfo("Chrome", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.13 (KHTML, like Gecko) Chrome/9.0.597.98 Safari/534.13");
        //public static readonly AgentInfo Firefox11 = new AgentInfo("Firefox", "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:11.0) Gecko/20100101 Firefox/11.0");
        // public static readonly AgentInfo InternetExplorer8 = new AgentInfo("Internet Explorer 8", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; WOW64; Trident/4.0; SLCC2; )");
    }
    public class HttpHelper
    {
        /// <summary>
        /// 创建默认的 
        /// </summary>
        /// <param name="url">请求的地址</param>
        /// <returns></returns>
        public static HttpWebRequest CreateHttpWebRequest(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = AgentInfo.Chrome.UserAgent;
            request.AllowAutoRedirect = true;
            //request.Method = "GET";
            request.KeepAlive = true;
            request.Referer = null;
            request.Timeout = 100000; // 默认 100 秒
            request.Headers["Accept-Language"] = CultureInfo.CreateSpecificCulture("zh-CN").Name;

            return request;
        }


        /// <summary>
        /// 向 HttpWebRequest 请求流写入 POST 数据
        /// </summary>
        /// <param name="request">HttpWebRequest 对象</param>
        /// <param name="postData">提交的数据。比如：UserName=bruce&Password=123456 。注意：如果 IsNullOrEmpty 则不会向 HttpWebRequest 请求流中写入数据</param>
        /// <param name="encoding">数据编码，可为 null。如果为 null ，则默认 UTF8 编码 </param>
        public static void WritePostData(HttpWebRequest request, string postData, Encoding encoding)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (string.IsNullOrEmpty(postData))
            {
                return;
            }
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            if (request.Method != "POST")
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded; charset=" + encoding.WebName;
            }
            using (BinaryWriter bw = new BinaryWriter(request.GetRequestStream()))
            {
                bw.Write(encoding.GetBytes(postData));
                bw.Flush();
            }
        }

        /// <summary>
        /// 从 NameValueCollection 中得到 PostData
        /// </summary>
        /// <param name="variables">集合</param>
        /// <returns>PostData</returns>
        public static string GetHttpPostVars(NameValueCollection variables)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < variables.Count; i++)
            {
                string key = variables.GetKey(i);
                string[] values = variables.GetValues(i);
                if (values != null)
                {
                    foreach (string value in values)
                    {
                        builder.AppendFormat("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value));
                    }
                }
                if (i < variables.Count - 1)
                {
                    builder.Append("&");
                }
            }
            return builder.ToString();
        }

        /// <summary>
        /// 根据 HttpWebRequest 请求对象得到响应的 Html 代码
        /// </summary>
        /// <param name="request">HttpWebRequest 对象</param>
        /// <returns></returns>
        public static string GetResponseText(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wex)
            {
                if (wex.Response == null)
                {
                    return wex.Message;
                }
                response = (HttpWebResponse)wex.Response;
            }
            using (response)
            {
                Stream strem = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(strem, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }
        }
        /// <summary>
        /// 根据 HttpWebRequest 请求对象得到响应的流
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static MemoryStream GetResponseStream(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wex)
            {
                response = (HttpWebResponse)wex.Response;
            }
            using (response)
            {
                Stream stream = response.GetResponseStream();
                MemoryStream ms = new MemoryStream();
                byte[] buffer = new byte[1024];

                while (true)
                {
                    int sz = stream.Read(buffer, 0, 1024);
                    if (sz == 0) break;
                    ms.Write(buffer, 0, sz);
                }
                ms.Position = 0;
                stream.Close();
                return ms;
            }
        }
        /// <summary>
        /// 发送 Get 请求，得到 Html 代码
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="cookieContainer">Cookie 容器，可为 null。响应时远程服务器返回的 Cookie 也自动包含其中</param>
        /// <returns>Html 代码</returns>
        public static string SendHttpGet(string url, CookieContainer cookieContainer = null)
        {
            HttpWebRequest request = CreateHttpWebRequest(url);

            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }
            return GetResponseText(request);
        }
        /// <summary>
        /// 发送 Get 请求，得到 Stream
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="cookieContainer">Cookie 容器，可为 null。响应时远程服务器返回的 Cookie 也自动包含其中</param>
        /// <returns>Stream</returns>
        public static MemoryStream SendHttpGetStream(string url, CookieContainer cookieContainer = null)
        {
            HttpWebRequest request = CreateHttpWebRequest(url);

            request.Method = "GET";
            request.ContentType = "application/x-www-form-urlencoded";
            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }
            return GetResponseStream(request);
        }
        /// <summary>
        /// 发送 POST 请求，得到 Html 代码
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postData">提交的数据。</param>
        /// <param name="cookieContainer">Cookie 容器，可为 null。响应时远程服务器返回的 Cookie 也自动包含其中</param>
        /// <returns>Html 代码</returns>
        public static string SendHttpPost(string url, string postData, CookieContainer cookieContainer)
        {
            HttpWebRequest request = CreateHttpWebRequest(url);

            if (cookieContainer != null)
            {
                request.CookieContainer = cookieContainer;
            }
            WritePostData(request, postData, Encoding.UTF8);

            return GetResponseText(request);
        }

        /// <summary>
        /// 发送 POST 请求，得到 Html 代码
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="postVariables">提交的数据。</param>
        /// <param name="cookieContainer">Cookie 容器，可为 null。响应时远程服务器返回的 Cookie 也自动包含其中</param>
        /// <returns>Html 代码</returns>
        public static string SendHttpPost(string url, NameValueCollection postVariables, CookieContainer cookieContainer)
        {
            string postData = GetHttpPostVars(postVariables);
            return SendHttpPost(url, postData, cookieContainer);
        }

    }
}
