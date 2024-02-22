using Microsoft.AspNetCore.Mvc;

namespace BuyBikeShop.Controllers
{
    public class PartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
