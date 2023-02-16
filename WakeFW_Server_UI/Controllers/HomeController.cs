using Microsoft.AspNetCore.Mvc;

namespace WakeFW_Server_UI.Controllers
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
