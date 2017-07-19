using ISoftSmart.API.Attributes;
using ISoftSmart.API.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;

namespace ISoftSmart.API.Handlers
{
    public class CustomRequestHandler : DelegatingHandler
    {
        //protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        //{
        //    //得到描述目标Action的HttpActionDescriptor
        //    HttpMethod originalMethod = request.Method;
        //    bool isPreflightRequest = request.IsPreflightRequest();
        //    if (isPreflightRequest)
        //    {
        //        string method = request.Headers.GetValues("Access-Control-Request-Method").First();
        //        request.Method = new HttpMethod(method);
        //    }

        //    //利用CorsAttribute实施授权并生成响应报头
        //    //IDictionary<string, string> headers;
        //    request.Method = originalMethod;
        //    ;
        //    var response = isPreflightRequest
        //        ? new HttpResponseMessage(HttpStatusCode.OK)
        //        : base.SendAsync(request, cancellationToken).Result;

        //    // 配置
        //    var corsSetting = CorsConfiguration.Instance;
        //    if (corsSetting != null)
        //    {
        //        var cors = corsSetting.DefaultCors;
        //        // cache enable
        //        if ((corsSetting.DefaultAPICors.CacheEnable ?? "false").ToLower().Equals("true"))
        //        {
        //            //添加响应报头
        //            response.Headers.CacheControl = new CacheControlHeaderValue
        //            {
        //                NoCache = true,
        //                NoStore = true,
        //                MustRevalidate = true
        //            };
        //            response.Headers.Pragma.Add(new NameValueHeaderValue("no-cache"));
        //            if (response.Content != null)
        //            {
        //                if ((response.Content.Headers.ContentType?.MediaType?.ToLower().Contains("image") ?? false) ==
        //                    false)
        //                    response.Content.Headers.Expires = DateTimeOffset.UtcNow;
        //            }
        //        }
        //        // cors
        //        if (cors.Orgins == "*" && request.Headers?.Referrer != null)
        //        {
        //            string cor;
        //            var absUrl = request.Headers.Referrer.AbsoluteUri;
        //            if (!string.IsNullOrEmpty(absUrl) && absUrl.ToLower().StartsWith("http"))
        //            {
        //                var url = absUrl.Replace("://", "$*$");
        //                if (url.Contains("/")) url = url.Split('/')[0];
        //                cor = url.Replace("$*$", "://");
        //            }
        //            else cor = request.Headers.Referrer.Scheme + "://" + request.Headers.Referrer.Authority;
        //            response.Headers.Add("access-control-allow-origin", cor);
        //        }
        //        else
        //        {
        //            var urls = cors.Orgins.Split(',');
        //            response.Headers.Add("access-control-allow-origin", urls);
        //        }
        //        response.Headers.Add("access-control-allow-headers", cors.Headers);
        //        response.Headers.Add("Access-Control-Allow-Methods", cors.Methods);
        //        response.Headers.Add("Access-Control-Allow-Credentials", cors.Credentials);
        //    }
        //    else
        //    {
        //        //添加响应报头
        //        response.Headers.CacheControl = new CacheControlHeaderValue
        //        {
        //            NoCache = true,
        //            NoStore = true,
        //            MustRevalidate = true
        //        };
        //        response.Headers.Pragma.Add(new NameValueHeaderValue("no-cache"));
        //        if (response.Content != null)
        //        {
        //            if ((response.Content.Headers.ContentType?.MediaType?.ToLower().Contains("image") ?? false) ==
        //                false)
        //                response.Content.Headers.Expires = DateTimeOffset.UtcNow;
        //        }

        //        string cor;
        //        var absUrl = request.Headers.Referrer.AbsoluteUri;
        //        if (!string.IsNullOrEmpty(absUrl) && absUrl.ToLower().StartsWith("http"))
        //        {
        //            var url = absUrl.Replace("://", "$*$");
        //            if (url.Contains("/")) url = url.Split('/')[0];
        //            cor = url.Replace("$*$", "://");
        //        }
        //        else cor = request.Headers.Referrer.Scheme + "://" + request.Headers.Referrer.Authority;
        //        response.Headers.Add("access-control-allow-origin", cor);
        //        //response.Headers.Add("Access-Control-Allow-Origin", "*");
        //        response.Headers.Add("access-control-allow-headers", "*");
        //        response.Headers.Add("Access-Control-Allow-Methods", "*");
        //        response.Headers.Add("Access-Control-Allow-Credentials", "true");
        //    }
        //    return Task.FromResult(response);
        //}
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            //得到描述目标Action的HttpActionDescriptor
            HttpMethod originalMethod = request.Method;
            bool isPreflightRequest = request.IsPreflightRequest();
            if (isPreflightRequest)
            {
                string method = request.Headers.GetValues("Access-Control-Request-Method").First();
                request.Method = new HttpMethod(method);
            }

            //利用CorsAttribute实施授权并生成响应报头
            IDictionary<string, string> headers;
            request.Method = originalMethod;
            ;
            var response = isPreflightRequest
                ? new HttpResponseMessage(HttpStatusCode.OK)
                : base.SendAsync(request, cancellationToken).Result;

            // 配置
            var corsSetting = CorsConfiguration.Instance;
            if (corsSetting != null)
            {
                var cors = corsSetting.DefaultCors;
                // cache enable
                if ((corsSetting.DefaultAPICors.CacheEnable ?? "false").ToLower().Equals("true"))
                {
                    //添加响应报头
                    response.Headers.CacheControl = new CacheControlHeaderValue
                    {
                        NoCache = true,
                        NoStore = true,
                        MustRevalidate = true
                    };
                    response.Headers.Pragma.Add(new NameValueHeaderValue("no-cache"));
                    if (response.Content != null)
                    {
                        if ((response.Content.Headers.ContentType?.MediaType?.ToLower().Contains("image") ?? false) ==
                            false)
                            response.Content.Headers.Expires = DateTimeOffset.UtcNow;
                    }
                }
                // cors
                if (cors.Orgins == "*" && request.Headers?.Referrer != null)
                {
                    string cor;
                    var absUrl = request.Headers.Referrer.AbsoluteUri;
                    if (!string.IsNullOrEmpty(absUrl) && absUrl.ToLower().StartsWith("http"))
                    {
                        var url = absUrl.Replace("://", "$*$");
                        if (url.Contains("/")) url = url.Split('/')[0];
                        cor = url.Replace("$*$", "://");
                    }
                    else cor = request.Headers.Referrer.Scheme + "://" + request.Headers.Referrer.Authority;
                    response.Headers.Add("access-control-allow-origin", cor);
                }
                else
                {
                    var urls = cors.Orgins.Split(',');
                    response.Headers.Add("access-control-allow-origin", urls);
                }
                response.Headers.Add("access-control-allow-headers", cors.Headers);
                response.Headers.Add("Access-Control-Allow-Methods", cors.Methods);
                response.Headers.Add("Access-Control-Allow-Credentials", cors.Credentials);
            }
            else
            {
                //添加响应报头
                response.Headers.CacheControl = new CacheControlHeaderValue
                {
                    NoCache = true,
                    NoStore = true,
                    MustRevalidate = true
                };
                response.Headers.Pragma.Add(new NameValueHeaderValue("no-cache"));
                if (response.Content != null)
                {
                    if ((response.Content.Headers.ContentType?.MediaType?.ToLower().Contains("image") ?? false) ==
                        false)
                        response.Content.Headers.Expires = DateTimeOffset.UtcNow;
                }

                string cor;
                if (request.Headers.Referrer != null)
                {
                    var absUrl = request.Headers.Referrer.AbsoluteUri;
                    if (!string.IsNullOrEmpty(absUrl) && absUrl.ToLower().StartsWith("http"))
                    {
                        var url = absUrl.Replace("://", "$*$");
                        if (url.Contains("/")) url = url.Split('/')[0];
                        cor = url.Replace("$*$", "://");
                    }
                    else cor = request.Headers.Referrer.Scheme + "://" + request.Headers.Referrer.Authority;
                    response.Headers.Add("access-control-allow-origin", cor);
                    //response.Headers.Add("Access-Control-Allow-Origin", "*");
                    response.Headers.Add("access-control-allow-headers", "*");
                    response.Headers.Add("Access-Control-Allow-Methods", "*");
                    response.Headers.Add("Access-Control-Allow-Credentials", "true");
                }
               
            }
            return Task.FromResult(response);
        }
    }
}