using IdentityIOC.Server;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.Swagger.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityIOC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IGetUserInfo _getUserInfo;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IGetUserInfo getUserInfo)
        {
            _logger = logger;
            _getUserInfo = getUserInfo;
        }


        [Route("GetUser")]
        [HttpPost]
        [SwaggerResponse(200)]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                _getUserInfo.OnGet("测试日志");
                return Ok(_getUserInfo.GetName());
            }
            catch (Exception ex)
            {

                throw ex;
            }
            
        }
    }
}
