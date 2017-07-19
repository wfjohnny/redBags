using ISoftSmart.API.App_Start;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.SessionState;

namespace ISoftSmart.API.Attributes
{
    public class SessionFilter : AuthorizeAttribute
    {
        #region 变量
        private object locker = new object();
        private RandomNumberGenerator _randgen;
        private static SessionContainer SessionContainer = new SessionContainer();
        #endregion
        /// <summary>
        /// 包含获取创建session的机制
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //actionContext.Request.Headers.Add("Access-Control-Allow-Origin", "*");
            var controller = (BaseController)actionContext.ControllerContext.Controller;
            if (controller is IRequiresSessionState)
            {    //有session需求
                var sessionId = controller.ReadCookie("Customer-Session");
                if (sessionId == null || sessionId.Count() == 0)
                {
                    sessionId = SessionId.Create(ref this._randgen);
                    var response = HttpContext.Current.Response;  //有线程问题
                    var cookie = new HttpCookie("Customer-Session", sessionId);
                    cookie.Expires = DateTime.MaxValue;
                    response.Cookies.Add(cookie);
                    lock (locker)
                    {
                        //无sessionid
                        if (!SessionContainer.ContainsKey(sessionId))
                        {
                            controller.Session = new SessionState(sessionId, SessionContainer.TimeOut);

                            SessionContainer.Add(sessionId, controller.Session);
                        }
                        else
                        {
                            //session生成重合的情况    暂时复用   有风险
                            controller.Session = SessionContainer[sessionId];
                        }
                    }
                }
                else
                {
                    lock (locker)
                    {
                        if (SessionContainer.ContainsKey(sessionId))
                        {  //有sessionid 有存储
                            controller.Session = SessionContainer[sessionId];
                        }
                        else
                        {
                            //有sessionid 无存储 (session丢失或者过期 重新生成) 
                            SessionContainer.Remove(sessionId);
                            sessionId = SessionId.Create(ref this._randgen);
                            var response = HttpContext.Current.Response;  //有线程问题
                            var cookie = new HttpCookie("Customer-Session", sessionId);
                            cookie.Expires = DateTime.MaxValue;
                            response.Cookies.Add(cookie);

                            controller.Session = new SessionState(sessionId, SessionContainer.TimeOut);
                            SessionContainer.Add(sessionId, controller.Session);
                        }
                    }
                }

            }
        }

        #region sessionI的生成
        private static class SessionId
        {
            internal const int NUM_CHARS_IN_ENCODING = 32;
            internal const int ENCODING_BITS_PER_CHAR = 5;
            internal const int ID_LENGTH_BITS = 120;
            internal const int ID_LENGTH_BYTES = 15;
            internal const int ID_LENGTH_CHARS = 24;
            private static char[] s_encoding;
            private static bool[] s_legalchars;
            internal static bool IsLegit(string s)
            {
                if (s == null || s.Length != 24)
                {
                    return false;
                }
                bool result;
                try
                {
                    int num = 24;
                    while (--num >= 0)
                    {
                        char c = s[num];
                        if (!SessionId.s_legalchars[(int)c])
                        {
                            result = false;
                            return result;
                        }
                    }
                    result = true;
                }
                catch (IndexOutOfRangeException)
                {
                    result = false;
                }
                return result;
            }
            internal static string Create(ref RandomNumberGenerator randgen)
            {
                if (randgen == null)
                {
                    randgen = new RNGCryptoServiceProvider();
                }
                byte[] array = new byte[15];
                randgen.GetBytes(array);
                return SessionId.Encode(array);
            }
            static SessionId()
            {
                SessionId.s_encoding = new char[]
                {
                'a',
                'b',
                'c',
                'd',
                'e',
                'f',
                'g',
                'h',
                'i',
                'j',
                'k',
                'l',
                'm',
                'n',
                'o',
                'p',
                'q',
                'r',
                's',
                't',
                'u',
                'v',
                'w',
                'x',
                'y',
                'z',
                '0',
                '1',
                '2',
                '3',
                '4',
                '5'
                };
                SessionId.s_legalchars = new bool[128];
                for (int i = SessionId.s_encoding.Length - 1; i >= 0; i--)
                {
                    char c = SessionId.s_encoding[i];
                    SessionId.s_legalchars[(int)c] = true;
                }
            }
            private static string Encode(byte[] buffer)
            {
                char[] array = new char[24];
                int num = 0;
                for (int i = 0; i < 15; i += 5)
                {
                    int num2 = (int)buffer[i] | (int)buffer[i + 1] << 8 | (int)buffer[i + 2] << 16 | (int)buffer[i + 3] << 24;
                    int num3 = num2 & 31;
                    array[num++] = SessionId.s_encoding[num3];
                    num3 = (num2 >> 5 & 31);
                    array[num++] = SessionId.s_encoding[num3];
                    num3 = (num2 >> 10 & 31);
                    array[num++] = SessionId.s_encoding[num3];
                    num3 = (num2 >> 15 & 31);
                    array[num++] = SessionId.s_encoding[num3];
                    num3 = (num2 >> 20 & 31);
                    array[num++] = SessionId.s_encoding[num3];
                    num3 = (num2 >> 25 & 31);
                    array[num++] = SessionId.s_encoding[num3];
                    num2 = ((num2 >> 30 & 3) | (int)buffer[i + 4] << 2);
                    num3 = (num2 & 31);
                    array[num++] = SessionId.s_encoding[num3];
                    num3 = (num2 >> 5 & 31);
                    array[num++] = SessionId.s_encoding[num3];
                }
                return new string(array);
            }
        }
        #endregion
    }
}