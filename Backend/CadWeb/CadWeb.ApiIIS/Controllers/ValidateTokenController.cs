using OAuthServer.Api.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CadWeb.ApiIIS.Controllers
{
    public class ValidateTokenController : ApiController
    {
        public void Validar()
        {


            SimpleAuthorizationServerProvider context = new SimpleAuthorizationServerProvider();
           
            //var validationParameters = new TokenValidationParameters()
            //{
            //    ValidIssuer = _tokenOptions.Issuer,
            //    ValidAudience = _tokenOptions.Audience,
            //    IssuerSigningKey = _tokenOptions.SigningKey,
            //    RequireExpirationTime = true
            //};
        }



    }

}

