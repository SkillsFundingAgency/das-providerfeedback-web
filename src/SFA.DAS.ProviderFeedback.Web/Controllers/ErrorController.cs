using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SFA.DAS.ProviderFeedback.Web.Controllers
{
    public class ErrorController : Controller
    {
        [HttpGet]
        [Route("accessdenied")]
        public IActionResult AccessDenied()
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return View();
        }
    }
}
