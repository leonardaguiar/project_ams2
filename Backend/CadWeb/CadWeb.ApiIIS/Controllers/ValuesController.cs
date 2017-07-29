using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CadWeb.ApiIIS.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/v1")]
    public class ValuesController : ApiController
    {
        [Authorize()]
        [Route("values")]
        public string Get()
        {
            return User.Identity.Name;
            
            
        }

    }
}
