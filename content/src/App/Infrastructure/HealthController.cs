using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyVendor.MyApp.Infrastructure
{
    /// <summary>
    /// Provides health checks for the service.
    /// </summary>
    [Route("health")]
    [AllowAnonymous]
    public class HealthController : Controller
    {
        /// <summary>
        /// Indicates if the service is up and running.
        /// </summary>
        [HttpGet, Route("")]
        public string Status() => "OK";
    }
}
