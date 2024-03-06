using BuyBikeShop.Data;
using BuyBikeShop.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyBikeShop.Controllers
{
    public class CheckOutController : Controller
    {
        private readonly UserManager<Customer> userManager;
        private readonly BuyBikeShopContext _context;

        public CheckOutController(UserManager<Customer> userManager, BuyBikeShopContext context)
        {
            this.userManager = userManager;
            _context = context;
        }

        [HttpPost]
        public IActionResult Payment(int productId, int quantity) // CustomerPaymentDetailsVM cp
        {
            // Here you can access productId and quantity directly
            ViewBag.Quantity = quantity;
            ViewBag.Id = productId;

            // Perform any additional logic you need

            return View("Payment"); // quantity ?
        }




       

        /*public IActionResult Cart()
        {
            if (!User.Identity.IsAuthenticated)
            {
               *//*Create a cart for the user find some logic to it because he is not connected*//*
            }
            


            var userId = userManager.GetUserId(User);
            var cart = CartManager.GetCart(userId);

            // Fetch product details for each cart item
            foreach (var item in cart.CartItems)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    item.Product = product; // Assuming CartItem has a Product property
                }
            }

            return View("Cart",cart);
        }*/
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Cart()
        {
            Cart cart;
            if (User.Identity.IsAuthenticated)
            {
                // For authenticated users, get the cart based on the user's ID
                var userId = userManager.GetUserId(User);
                cart = CartManager.GetCart(HttpContext);
            }
            else
            {
                // For guest users, get the cart based on the session ID
                cart = CartManager.GetCart(HttpContext);
            }

            // Fetch product details for each cart item
            foreach (var item in cart.CartItems)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    item.Product = product; // Assuming CartItem has a Product property
                }
            }

            return View("Cart", cart);
        }



        public IActionResult CreateOrder(Customer customer)
        {
            /*Currently not used but when clickng the place order this should have some connection  with the cart and all its items to an order of a customer*/
            return View("Payment");
        }


        //Validation
        //public async Task<IActionResult> PlaceOrderPress(RegisterVM model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //Customer cust = new Customer
        //        //{
        //        //    FName = Utils.CapitalizeFirstLetter(model.FName!),
        //        //    LName = Utils.CapitalizeFirstLetter(model.LName!),
        //        //    Phone = model.Phone!,
        //        //    Birthdate = model.Birthdate,
        //        //    Email = model.Email,
        //        //    UserName = model.Email
        //        //};

        //        var result = await userManager.CreateAsync(cust, model.Password!);
        //        if (result.Succeeded)
        //        {
        //            await signInManager.SignInAsync(cust, false);
        //            return RedirectToAction("Index", "Home");

        //        }
        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError("", error.Description);
        //        }
        //    }
        //    return View(model);
        //}


    }
}
