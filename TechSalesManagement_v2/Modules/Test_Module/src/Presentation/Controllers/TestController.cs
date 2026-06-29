using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Test_Module.src.Presentation.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<string>> GetTest()
        {
            await Task.Delay(0);
            return StatusCode(StatusCodes.Status200OK, "Hello from TechSales");
        }
    }
}