using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FantaxyWebApplication.Controllers
{
    public class MainController : Controller
    {
        [Authorize]
        public IActionResult Main()
        {
            return View();
        }
    }
}
