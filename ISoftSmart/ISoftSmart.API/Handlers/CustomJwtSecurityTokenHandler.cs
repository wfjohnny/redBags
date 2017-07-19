using log4net;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using System.Web;

namespace ISoftSmart.API.Handlers
{
    public class CustomJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        private readonly ILog _log = LogManager.GetLogger("EMIS.ResourceServer");
        protected override SecurityKey ResolveIssuerSigningKey(string token, SecurityToken securityToken, SecurityKeyIdentifier keyIdentifier,
            TokenValidationParameters validationParameters)
        {
            try
            {
                return base.ResolveIssuerSigningKey(token, securityToken, keyIdentifier, validationParameters);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                _log.Error(ex.StackTrace);
                HttpContext.Current.Cache.Insert("error.signingkey", ex.Message);
                throw;
            }
        }

        protected override ClaimsIdentity CreateClaimsIdentity(JwtSecurityToken jwt, string issuer, TokenValidationParameters validationParameters)
        {
            try
            {
                return base.CreateClaimsIdentity(jwt, issuer, validationParameters);
            }
            catch (Exception ex)
            {
                _log.Error(ex.Message);
                _log.Error(ex.StackTrace);
                HttpContext.Current.Cache.Insert("error.createclaims", ex.Message);
                throw;
            }
        }

        protected override void ValidateLifetime(DateTime? notBefore, DateTime? expires, SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            try
            {
                base.ValidateLifetime(notBefore, expires, securityToken, validationParameters);
            }
            catch (SecurityTokenExpiredException ex)
            {
                _log.Error(ex.Message);
                _log.Error(ex.StackTrace);
                HttpContext.Current.Cache.Insert("error.lifetime", ex.Message);
                throw;
            }
        }

        protected override string ValidateIssuer(string issuer, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            try
            {
                return base.ValidateIssuer(issuer, securityToken, validationParameters);
            }
            catch (SecurityTokenInvalidIssuerException ex)
            {
                _log.Error(ex.Message);
                _log.Error(ex.StackTrace);
                HttpContext.Current.Cache.Insert("error.issuer", ex.Message);
                throw;
            }
        }

        protected override void ValidateAudience(IEnumerable<string> audiences, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            try
            {
                base.ValidateAudience(audiences, securityToken, validationParameters);
            }
            catch (SecurityTokenInvalidAudienceException ex)
            {
                _log.Error(ex.Message);
                _log.Error(ex.StackTrace);
                HttpContext.Current.Cache.Insert("error.audience", ex.Message);
                throw;
            }
        }
    }
}