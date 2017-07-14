using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Api
{
    class Startup
    {
        public void Configuration(IAppBuilder appBuilder) {

           
            HttpConfiguration config = new HttpConfiguration();
            config.EnableCors();
            config.Formatters.Clear();
            config.Formatters.Add(new JsonMediaTypeFormatter());
            config.Formatters.JsonFormatter.SerializerSettings =
            new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            //config.Formatters.JsonFormatter.SerializerSettings.Converters.Add(new StringEnumConverter());
            config.Routes.MapHttpRoute(
                  name: "CadWebApi",
                  routeTemplate: "api/{controller}/{id}",
                  defaults:new { id = RouteParameter.Optional }
                );
            
            appBuilder.UseWebApi(config);
            
        }
    }
}
