using Microsoft.AspNetCore.Mvc;

namespace BuyBikeShop.Controllers
{
    public class CheckOutController : Controller
    {
        [HttpPost]
        public IActionResult Payment(int quantity,int productId)
        {
            ViewBag.Quantity = quantity;
            ViewBag.Id = productId;
           
            return View("Payment",quantity);
        }

        public IActionResult Cart()
        {
            

            return View("Cart");
        }
    }
}
