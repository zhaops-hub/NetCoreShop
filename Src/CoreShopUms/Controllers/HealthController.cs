using Microsoft.AspNetCore.Mvc;

namespace CoreShopUms.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HealthController : ControllerBase
    {
        [HttpGet(nameof(Health))]
        public string Health()
        {
            return "ok";
        }
    }
}