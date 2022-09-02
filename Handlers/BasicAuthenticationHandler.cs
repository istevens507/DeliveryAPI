﻿using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DeliveryAPI.Handlers
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler( 
              IOptionsMonitor<AuthenticationSchemeOptions> options,
              ILoggerFactory logger,
              UrlEncoder encoder,
              ISystemClock clock)
            : base (options,logger, encoder, clock)
        {

        }

        //abtract class from BasicAuthenticationHandler
        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        { // Before a request is executed this is going to be executed first

           AuthenticationTicket authTicket;

            if (!Request.Headers.ContainsKey("Authorization")) {
                return AuthenticateResult.Fail("Authorization header was not found");
            }

         var taskAuthTicket = await Task.Run(() =>
            {
                var authenticationHeaderValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var bytes = Convert.FromBase64String(authenticationHeaderValue.Parameter);
                string[] credentials = Encoding.UTF8.GetString(bytes).Split(".");
                string username = credentials[0];
                string password = credentials[1];

                //Verify against db to validate them
                var mokeDbUsername = "user12345";
                var mokeDbPassword = "pass12345";

                if (username == mokeDbUsername && password == mokeDbPassword)
                {
                    //valid user
                    var claims = new[] { new Claim(ClaimTypes.Name, mokeDbUsername) };
                    var identity = new ClaimsIdentity(claims, Scheme.Name);
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    authTicket = ticket;

                    return Task.FromResult(ticket);
                }
                return Task.FromResult<AuthenticationTicket>(null);
            });

            if (taskAuthTicket != null)
            {
                return AuthenticateResult.Success(taskAuthTicket);
            }
          

            return AuthenticateResult.Fail("Authentication failed");
        }
    }
}
