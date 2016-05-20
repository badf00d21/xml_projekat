using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Parliament.Api.SGNS.Endpoints
{
    [Authorize]
    public class AuthTestController : ApiController
    {
        [HttpGet]
        [Route("api/test/token")]
        public IHttpActionResult TokenTest()
        {
            return Ok("Radi");
        }
    }
}