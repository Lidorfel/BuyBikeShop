using BuyBikeShop.Models;
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

        public IActionResult CreateOrder(Customer customer)
        {
            /*Currently not used but when clickng the place order this should have some connection  with the cart and all its items to an order of a customer*/
            return View("Payment");
        }

        
    }
}
