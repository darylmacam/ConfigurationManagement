using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationManagement.Json.Web.DotNetCore
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly PaymentsConfig  _paymentsConfig;

        public HomeController(IOptions<PaymentsConfig> options)
        {
            _paymentsConfig = options.Value;
        }

        [HttpGet]
        public PaymentsConfig Get()
            => _paymentsConfig;
    }
}
