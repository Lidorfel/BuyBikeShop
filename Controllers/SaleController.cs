using Microsoft.AspNetCore.Mvc;

namespace BuyBikeShop.Controllers
{
    public class SaleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
