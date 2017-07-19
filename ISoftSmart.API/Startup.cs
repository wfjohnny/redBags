using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using log4net;
using ISoftSmart.API.App_Start;
using System.Web.Http;

[assembly: OwinStartup(typeof(ISoftSmart.API.Startup))]

namespace ISoftSmart.API
{
    public class Startup
    {
        #region<<用户变量>>

        private readonly ILog _log = LogManager.GetLogger("ResourceServer");

        //private readonly IOAuthServices _authServices;// = Components.BusinessServiceProxyLocator.GetService<IOAuthServices>();

        #endregion

        #region intialization

        public Startup()
        {
            try
            {
                //_authServices = BusinessServiceProxyLocator.GetService<IOAuthServices>();
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                _log.Error(ex.StackTrace);
                throw;
            }
        }

        #endregion

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();

            ConfigureOAuth(app);

            //config.EnableCors(new EnableCorsAttribute("http://localhost:27137", "*", "*") {SupportsCredentials = true});
            WebApiConfig.Register(config);
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            /*try
            {
                // 获取authorization server
                var issuer = ConfigurationManager.AppSettings["issuer"] ?? "";
                if (string.IsNullOrEmpty(issuer))
                {
                    throw new ConfigurationErrorsException("未获取配置issuer");
                }
                // 获取所有有效clients
                var clients = _authServices.FindClients(new MyAuthClients
                {
                    ProjectId = issuer,
                    IsActive = 1,
                    Status = 1
                });

                var key = new byte[32];
                RandomNumberGenerator.Create().GetBytes(key);
                var base64Secret = TextEncodings.Base64Url.Encode(key);

                //if (clients == null || clients.Count == 0)
                {
                    clients = new List<MyAuthClients>
                    {
                        new MyAuthClients
                        {
                            ClientId = new Guid().ToString("N"),
                            ClientSecret = base64Secret
                        }
                    };
                    HttpContext.Current.Cache.Insert("error.noclient", "该项目无有效的client！项目来源编号：" + issuer);
                }

                app.UseJwtBearerAuthentication(new JwtBearerAuthenticationOptions
                {
                    AuthenticationMode = AuthenticationMode.Active,
                    //AllowedAudiences = clients.Select(x => x.ClientId),
                    //IssuerSecurityTokenProviders = new IIssuerSecurityTokenProvider[]
                    //{
                    //    new SymmetricKeyIssuerSecurityTokenProvider(issuer,
                    //        clients.Select(x => TextEncodings.Base64Url.Decode(x.ClientSecret)))
                    //},
                    Provider = new CustomOAuthBearerAuthenticationProvider()
                    {
                        OnApplyChallenge = (context) =>
                        {

                            return null;
                        }
                    },
                    TokenHandler = new CustomJwtSecurityTokenHandler()
                });
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                _log.Error(ex.StackTrace);
                throw;
            }*/
        }
    }
}

