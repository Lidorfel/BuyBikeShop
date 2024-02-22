using Microsoft.AspNetCore.Mvc;

namespace BuyBikeShop.Controllers
{
    public class AccessoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
