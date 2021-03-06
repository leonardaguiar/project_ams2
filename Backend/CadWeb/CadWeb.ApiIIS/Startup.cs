﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using Newtonsoft.Json.Serialization;
using Microsoft.Owin.Security.OAuth;
using OAuthServer.Api.Security;
using Newtonsoft.Json;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;

[assembly: OwinStartup(typeof(CadWeb.ApiIIS.Startup))]

namespace CadWeb.ApiIIS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            HttpConfiguration config = new HttpConfiguration();
            ConfigureWeApi(config);
            ConfigureOAuth(app);

            
            app.UseWebApi(config);
        }

        public static void ConfigureWeApi(HttpConfiguration config) {


            
            //settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            //config.Formatters.Add(formatters);
            foreach (var jsonFormatter2 in config.Formatters.OfType<JsonMediaTypeFormatter>())
            {
                jsonFormatter2.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;

            jsonFormatter.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            settings.Formatting = Newtonsoft.Json.Formatting.Indented;
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
            var singlejsonFormatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            singlejsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.EnableCors();
            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/v1/security/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),
                Provider = new SimpleAuthorizationServerProvider()
            };

            // Token Generation
            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}
