using Microsoft.AspNetCore.Mvc;

namespace RunGroupWebApp.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
