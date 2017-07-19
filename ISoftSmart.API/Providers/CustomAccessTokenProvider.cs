using Microsoft.Owin.Security.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ISoftSmart.API.Providers
{
    public class CustomAccessTokenProvider : AuthenticationTokenProvider
    {
        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
            var expired = context.Ticket.Properties.ExpiresUtc < DateTime.UtcNow;
            if (expired)
            {
                context.OwinContext.Set("error", "jwt is expired.");
                //If current token is expired, set a custom response header
                context.Response.Headers.Add("X-AccessTokenExpired", new string[] { "1" });
            }

            base.Receive(context);
        }
    }
}