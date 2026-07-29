using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common_Module.src.BusinessExceptions;
using Common_Module.src.Presentation.ApiResponseModels;
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
            return StatusCode(StatusCodes.Status200OK, new ApiSuccessResponse<string>("It's OK data"));
        }

        [HttpGet("400")] 
        public async Task<ActionResult<string>> GetTestWith400()
        {
            await Task.Delay(0);
            throw new BadRequestException("Test for 400_BAD_REQUEST", new Dictionary<string, List<string>>()
            {
               {"ErrorA", new List<string>() {"ErrorA1", "ErrorA2"}},
               {"ErrorB", new List<string>() {"ErrorB1", "ErrorB2"}}, 
               {"ErrorC", new List<string>() {"ErrorC1", "ErrorC2"}} 
            });
        }

    }
}