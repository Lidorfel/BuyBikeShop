using BuyBikeShop.Data;
using BuyBikeShop.Models;
using BuyBikeShop.ViewModels;
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
            var cart = CartManager.GetCart(HttpContext);
            CartManager.AddToCart(cart,productId, quantity);
            foreach (var item in cart.CartItems)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    item.Product = product; // Assuming CartItem has a Product property
                }
            }
            var payVM = new PaymentVM();
            payVM.Cart = cart;
            payVM.cp = new CustomerPaymentDetailsVM();


            // Perform any additional logic you need

            return View("Payment",payVM); 
        }
        [HttpGet]

        public IActionResult CartPayment()
        {
            var cart = CartManager.GetCart(HttpContext);
            var payVM = new PaymentVM();
            payVM.Cart = cart;
            payVM.cp = new CustomerPaymentDetailsVM();

            return View("Payment", payVM);

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


        [HttpPost]
        public async Task <IActionResult> CreateOrder(PaymentVM pay)
        {
                Customer cust = null;
                string customerName = pay.cp.first_name;
                if (User.Identity.IsAuthenticated)
                {
                    cust = await userManager.GetUserAsync(User); 
                    if (cust != null)
                    {
                        cust.Street = pay.cp.address.ToString();
                        cust.City = pay.cp.city.ToString();
                        cust.Country = pay.cp.country.ToString();
                        cust.Zip = pay.cp.zip_code.ToString();
                        cust.CreditCard = pay.cp.car_number.ToString();//must be encrypt
                        cust.CVV = int.Parse(pay.cp.car_code);//must be encrypt
                        cust.ExpDate = new DateTime(pay.cp.ExpirationYear, pay.cp.ExpirationMonth, 1);//must be encrypt
                        _context.Customers.Update(cust);

                }
                else
                    {
                        NotFound();
                    }
                }

                List<OrderProduct> OrderProductsList = new List<OrderProduct>();
                var cart = CartManager.GetCart(HttpContext);
                foreach(var item in cart.CartItems)
                {
                    OrderProductsList.Add(new OrderProduct
                    {
                        Product = item.Product,
                        Quantity = item.Quantity,
                        UnitPrice = Math.Floor(item.Product.Price * (1 - (item.Product.Sale_Perc / 100.0)))
                    });
                }
                Order order = new Order
                {
                    OrderDate = DateTime.Now,
                    CustomerId = cust != null ? cust.Id : null,
                    CustomerName = customerName,
                    OrderProducts = OrderProductsList,
                };
                _context.Orders.Add(order);
                foreach (var item in cart.CartItems)
                    {
                        var pr = _context.Products.FirstOrDefault(i => i.Id == item.ProductId);
                        if (pr != null)
                        {
                            pr.Quantity -= item.Quantity;
                            _context.Products.Update(pr);

                        }
                    }
                await _context.SaveChangesAsync();

                return RedirectToAction("Index","Home");

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
