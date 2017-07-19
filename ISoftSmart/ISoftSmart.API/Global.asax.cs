using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ISoftSmart.API
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            SessionContainer.TimeOut = 30 * 60 * 1000;
            //APIClient client = new APIClient("http://localhost/AuthorizationServer.API");
            //var request = new RegisterRequest();
            //request.Infos = new RegisterInfo
            //{
            //    ProjectId = "ChatManagement",
            //    RegisterUser = "Dean"
            //};
            //request.IsJsonRequest = true;
            //request.RequestMethod = "POST";
            //var rsp = client.Execute(request);
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (Request.HttpMethod.Equals("options", StringComparison.OrdinalIgnoreCase))
            {
                Response.Headers.Add("access-control-allow-origin", Request.Headers["Origin"]);
                Response.Headers.Add("Access-Control-Allow-Headers", "content-type,x-requested-with");
                Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE,OPTIONS");
                Response.Headers.Add("Access-Control-Allow-Credentials", "true");
                Response.StatusCode = 200;
                Response.SubStatusCode = 200;
                Response.End();
            }
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

            string errMsg = JsonConvert.SerializeObject(new
            {
                Code = "FAIL",
                ResponseMessage = "服务发生错误！"
            });
            Response.StatusCode = 500;
            Response.Write(errMsg);
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}