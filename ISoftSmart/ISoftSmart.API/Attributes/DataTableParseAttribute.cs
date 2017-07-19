using ISoftSmart.API.App_Start;
using ISoftSmart.Model;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http.Filters;

namespace ISoftSmart.API.Attributes
{
    public class DataTableParseAttribute : ActionFilterAttribute
    {
        #region 变量
        private readonly ILog _log = LogManager.GetLogger("DataTable插件参数获取过滤器");
        #endregion
        public override void OnActionExecuting(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var controlller = actionContext.ControllerContext.Controller;
            Stream stream = actionContext.Request.Content.ReadAsStreamAsync().Result;
            Encoding encoding = Encoding.UTF8;
            //  stream.Position = 0;
            string responseData = "";
            using (StreamReader reader = new StreamReader(stream, encoding))
            {
                responseData = reader.ReadToEnd().ToString();
            }
            //反序列化进行处理
            var data = JsonConvert.DeserializeObject(responseData);
            var queryData = new JObjectParse(JsonConvert.DeserializeObject(responseData));
            try
            {
                ((BaseController)controlller).PageSize = int.Parse(queryData.GetDicValue("iDisplayLength"));
                if (((BaseController)controlller).PageSize < 0) { ((BaseController)controlller).PageSize = int.MaxValue; }
                var num = (float)int.Parse(queryData.GetDicValue("iDisplayStart")) / int.Parse(queryData.GetDicValue("iDisplayLength"));
                ((BaseController)controlller).CurrentPage = (int)Math.Ceiling(num) + 1;
                ((BaseController)controlller).DataTableParams = queryData;
            }
            catch (KeyNotFoundException ex)
            {
                _log.Error("iDisplayLength或者iDisplayStart在querryData中不存在" + Environment.NewLine +
                    ex.Message + Environment.NewLine + ex.StackTrace);
                actionContext.Response.StatusCode = HttpStatusCode.InternalServerError;
                actionContext.Response.Content = new StringContent("{Code=\"FAIL\",ResponseMessage=\"datatable分页参数解析发生错误！\"}");
            }
        }
    }
}