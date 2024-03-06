using BuyBikeShop.Data;
using BuyBikeShop.Models;
using BuyBikeShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BuyBikeShop.Controllers
{
    public class CheckOutController : Controller
    {
        [HttpPost]
        public IActionResult Payment(int productId, int quantity) // CustomerPaymentDetailsVM cp
        {
            // Here you can access productId and quantity directly
            ViewBag.Quantity = quantity;
            ViewBag.Id = productId;

            // Perform any additional logic you need

            return View("Payment"); // quantity ?
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
