using Microsoft.AspNetCore.Mvc;

namespace WakeFwGateway.Controllers
{
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
