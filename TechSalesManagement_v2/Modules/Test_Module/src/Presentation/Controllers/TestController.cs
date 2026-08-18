using Common_Module.BusinessExceptions;
using Common_Module.CustomAttributes;
using Common_Module.Presentation.ApiResponseModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Test_Module.Presentation.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        public class Haha : IRequest<string>
        {
            
        }
        public TestController()
        {
        }
        [HttpGet]
        public async Task<ActionResult<string>> GetTest()
        {
            
            await Task.Delay(0);
            return StatusCode(StatusCodes.Status200OK, new ApiSuccessResponse<string>("hi"));
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


        [HttpGet("test-authZ")]
        [HasPermission("TestAuthZ")]
        public async Task<ActionResult<string>> TestAuthZ()
        {
            await Task.Delay(0);
            return StatusCode(StatusCodes.Status200OK, new ApiSuccessResponse<string>("It's OK data"));
        }

    }
}