using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CourseScheduler.Web.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchedulerController : ControllerBase
    {
        private readonly ILogger<SchedulerController> _logger;

        public SchedulerController(ILogger<SchedulerController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult SubmitSettings([FromBody] Dictionary<string, Object> data)
        {
            _logger.LogInformation(JsonSerializer.Serialize(data));
            return Ok(new { Message = "Success!!!!" });
        }

        [HttpGet]
        public IActionResult TestEndpoint()
        {
            return Ok(new { Message = "Scheduler API is working!" });
        }
    }
}
