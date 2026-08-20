using Common_Module.BusinessExceptions;
using Common_Module.CustomAttributes;
using Common_Module.Mediator.Command.Abstract;
using Common_Module.Mediator.Event.Abstract;
using Common_Module.Presentation.ApiResponseModels;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Test_Module.Test.Command;

namespace Test_Module.Presentation.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : ControllerBase
    {
        private readonly ICommandExecutor _commandExecutor;
        private readonly IEventPublisher _eventPublisher;
        public TestController(ICommandExecutor commandExecutor, IEventPublisher eventPublisher)
        {
            _commandExecutor = commandExecutor;
            _eventPublisher = eventPublisher;
        }
        
        [HttpGet]
        public async Task<ActionResult<string>> GetTest()
        {
            await Task.Delay(0);
            TestEvent e = new TestEvent()
            {
                Name = "Test 1"
            };

            await _eventPublisher.Publish(e);


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