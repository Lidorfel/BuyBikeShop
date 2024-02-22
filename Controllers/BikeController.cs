using Microsoft.AspNetCore.Mvc;

namespace BuyBikeShop.Controllers
{
    public class BikeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
