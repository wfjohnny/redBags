using ISoftSmart.Model;
using ISoftSmart.Model.AD.My;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.SessionState;

namespace ISoftSmart.API.App_Start
{
    public class BaseController : ApiController, IRequiresSessionState
    {

        #region datatable分页相关数据获取
        /// <summary>
        /// datatable参数数据
        /// </summary>
        public JObjectParse DataTableParams;
        /// <summary>
        /// 页面显示数据长度
        /// </summary>
        public int PageSize;
        /// <summary>
        /// 当前页号
        /// </summary>
        public int CurrentPage;
        #endregion

        #region cookie  session
        public HttpResponseMessage CookieResponse(List<CookieHeaderValue> cookies, object data)
        {

            var resp = new HttpResponseMessage();
            resp.StatusCode = HttpStatusCode.OK;
            resp.Headers.AddCookies(cookies);
            resp.Content = new StringContent(JsonConvert.SerializeObject(data));
            return resp;
        }
        public string ReadCookie(string key)
        {
            CookieHeaderValue cookie = Request.Headers.GetCookies(key).FirstOrDefault();
            if (cookie != null)
            {
                CookieState cookieState = cookie[key];
                return cookieState.Value;
            }
            return null;
        }

        #region <<登陆用户信息>>
        /// <summary>
        /// 当前用户信息
        /// </summary>
        public MyAdUser CurrentUser
        {
            get
            {
                if (!Session.Keys.Contains("adUser"))
                {
                    return null;
                }
                return Session["adUser"] as MyAdUser;
            }
        }
        #endregion

        /// <summary>
        /// session机制  暂时无用（开启需要将sessionfilter加上才能使用）
        /// </summary>
        public SessionState Session;
        #endregion

    }
}