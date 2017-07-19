using ISoftSmart.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace ISoftSmart.API.Attributes
{
    public class TokenAuthorizeAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            try
            {
                base.HandleUnauthorizedRequest(actionContext);

                // get owin context error
                var ctx = actionContext.Request.GetOwinContext();
                var error = ctx.Get<string>("error") ?? string.Empty;
                if (string.IsNullOrEmpty(error))
                {
                    if (HttpContext.Current.Cache.Count > 0)
                    {
                        var ex = HttpContext.Current.Cache.Get("error.lifetime") ??
                            HttpContext.Current.Cache.Get("error.issuer") ??
                            HttpContext.Current.Cache.Get("error.signingkey") ??
                            HttpContext.Current.Cache.Get("error.createclaims") ??
                            HttpContext.Current.Cache.Get("error.audience") ??
                            HttpContext.Current.Cache.Get("error.noclient");
                        error = ex?.ToString() ?? "授权不成功！";
                    }
                }
                if (string.IsNullOrEmpty(error)) error = "授权不成功！";
                var responseContent = new APIResponse<string>
                {
                    Code = "ERROR",
                    ResponseMessage = error,
                    Result = string.Empty
                };
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, responseContent);
            }
            catch (Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, new APIResponse<string>
                {
                    Code = "ERROR",
                    ResponseMessage = ex.Message,
                    Result = string.Empty
                });
            }
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            //return true;
            return base.IsAuthorized(actionContext);
        }
    }
}