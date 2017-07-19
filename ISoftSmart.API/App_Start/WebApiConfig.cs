using ISoftSmart.API.Attributes;
using ISoftSmart.API.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;

namespace ISoftSmart.API.App_Start
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // route
            config.MessageHandlers.Add(new CustomRequestHandler());
            config.MapHttpAttributeRoutes();
            config.Filters.Add(new SessionFilter());
            //config.Routes.MapHttpRoute(
            //    name: "DefaultApi",
            //    routeTemplate: "api/{controller}/{id}",
            //    defaults: new { id = RouteParameter.Optional }
            //    );

            // jsonformatter
            config.Formatters.Clear();
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
            if (jsonFormatter != null)
            {
                jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                jsonFormatter.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                jsonFormatter.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
            }
            else
            {
                jsonFormatter = new JsonMediaTypeFormatter
                {
                    SerializerSettings = { DateFormatHandling = DateFormatHandling.MicrosoftDateFormat, DateFormatString = "yyyy-MM-dd HH:mm:ss", ContractResolver = new CamelCasePropertyNamesContractResolver() }
                };
                config.Formatters.Add(jsonFormatter);
            }
        }
    }
}