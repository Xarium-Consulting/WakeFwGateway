using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WakeFwGateway.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        [HttpGet]
        [HttpPost]
        public IActionResult Index()
        {
            return Redirect("/TargetNetworkDevices/");
        }
    }
}
