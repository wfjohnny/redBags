using log4net;
using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace ISoftSmart.API.Providers
{
    public class CustomOAuthBearerAuthenticationProvider : OAuthBearerAuthenticationProvider
    {
        #region user definitions

        private readonly ILog _log = LogManager.GetLogger("EMIS.ResourceServer");

        //private readonly IOAuthServices _authServices;

        public CustomOAuthBearerAuthenticationProvider()
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

        public Task OAuthResponseChangeEventTask(OAuthChallengeContext context)
        {
            return base.OnApplyChallenge(context);
        }

        public override async Task ValidateIdentity(OAuthValidateIdentityContext context)
        {
            try
            {
                await base.ValidateIdentity(context);
                // token validate, only one valid token, diffrent jti to diffrent ticket
                // one time one valid token
                #region Claims vlidate

                var nameClaim = context.Ticket.Identity.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
                if (nameClaim == null)
                {
                    _log.Error("subject is not in ticket");
                    context.SetError("subject is not in ticket");
                    context.OwinContext.Set("error", "subject is not in ticket.");
                    context.Rejected();
                    return;
                }
                var projectClaim = context.Ticket.Identity.Claims.FirstOrDefault(x => x.Type == "iss");
                if (projectClaim == null)
                {
                    _log.Error("iss is not in ticket");
                    context.SetError("iss is not in ticket");
                    context.OwinContext.Set("error", "iss is not in ticket.");
                    context.Rejected();
                    return;
                }
                var audClaim = context.Ticket.Identity.Claims.FirstOrDefault(x => x.Type == "aud");
                if (audClaim == null)
                {
                    _log.Error("client_id is not in ticket");
                    context.SetError("client_id is not in ticket");
                    context.OwinContext.Set("error", "client_id is not in ticket.");
                    context.Rejected();
                    return;
                }
                var jtiClaim = context.Ticket.Identity.Claims.FirstOrDefault(x => x.Type == "jti");
                if (jtiClaim == null)
                {
                    _log.Error("jti is not in ticket");
                    context.SetError("jti is not in ticket");
                    context.OwinContext.Set("error", "jti is not in ticket.");
                    context.Rejected();
                    return;
                }

                #endregion

                var projectId = projectClaim.Value;
                var registerUser = nameClaim.Value;
                var clientId = audClaim.Value;
                var jti = jtiClaim.Value;
                if (string.IsNullOrEmpty(clientId))
                {
                    _log.Error("client_id is null");
                    context.SetError("client_id is null");
                    context.OwinContext.Set("error", "client_id is null.");
                    context.Rejected();
                    return;
                }
                //var refreshTokens = await Task.Run(() => _authServices.GetAllRefreshTokens(clientId));
                //if (refreshTokens == null || refreshTokens.Count == 0)
                //{
                //    _log.Error("ticket is missing");
                //    context.SetError("ticket is missing");
                //    context.OwinContext.Set("error", "ticket is missing.");
                //    context.Rejected();
                //    return;
                //}
                //if (!refreshTokens.Any(x => (x.ProjectId == projectId && x.Subject == registerUser)))
                //{
                //    _log.Error("ticket is not valid");
                //    context.SetError("ticket is not valid");
                //    context.OwinContext.Set("error", "ticket is not valid.");
                //    context.Rejected();
                //    return;
                //}
                //var rft = refreshTokens.First(x => (x.ProjectId == projectId && x.Subject == registerUser));

                // validate ticket jti&server jti
                //var serverJti = rft.TokenId;
                //if (!serverJti.Equals(jti))
                //{
                //    _log.Error("jti is not valid");
                //    context.SetError("jti is not valid");
                //    context.OwinContext.Set("error", "Token已更新，请使用最新的Token。");
                //    context.Rejected();
                //    return;
                //}

                #region IP validate

                var ip = context.Request.RemoteIpAddress;
                _log.Info($"request ip({ip}) is validating...");
                // find allowed ips & validate ip
                //var client = _authServices.FindClient(clientId);
                //if (client != null)
                //{
                //    if (!projectId.Equals(client.ProjectId))
                //    {
                //        _log.Error("client's projectId is not valid.");
                //        context.SetError("client's projectId is not valid.");
                //        context.OwinContext.Set("error", "client's projectId is not valid.");
                //        return;
                //    }
                //    var allowedIPs = client.AllowedIPs;
                //    if (string.IsNullOrEmpty(allowedIPs))
                //    {
                //        _log.Error($"IP({ip}) is not allowed in this client.");
                //        context.SetError($"IP({ip}) is not allowed in this client.");
                //        context.OwinContext.Set("error", $"IP({ip}) is not allowed in this client.");
                //        return;
                //    }
                //    if (allowedIPs != "*")
                //    {
                //        var detailIPs = allowedIPs.Split(';');
                //        if (detailIPs.All(x => x != ip))
                //        {
                //            _log.Error($"IP({ip}) is not allowed in this client.");
                //            context.OwinContext.Set("error", $"IP({ip}) is not allowed in this client.");
                //            context.Rejected();
                //        }
                //    }
                //}

                #endregion
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                _log.Error(ex.StackTrace);
                context.Rejected();
            }
        }


        public override Task RequestToken(OAuthRequestTokenContext context)
        {
            return base.RequestToken(context);
        }
    }
}