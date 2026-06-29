using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Auth_Module.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpGet("test")]
        public async Task<ActionResult<string>> GetTest()
        {
            return StatusCode(StatusCodes.Status200OK, "Ok from Auth");
        }
    }
}