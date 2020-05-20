using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApolloTest.Conf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ApolloTest.Controllers
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
        private readonly IOptionsSnapshot<AppSettings> _appSetting;
        private readonly IConfiguration _configuration;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IOptionsSnapshot<AppSettings> appSetting, IConfiguration configuration)
        {
            _logger = logger;
            _appSetting = appSetting;
            _configuration = configuration;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            yield return $"{_appSetting.Value.ConsulIp}:{_appSetting.Value.ConsulPort}";
            yield return $"{_configuration["ConsulIp"]}:{_configuration["ConsulPort"]}";
        }
    }
}
