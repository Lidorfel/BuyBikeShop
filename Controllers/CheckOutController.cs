using BuyBikeShop.Data;
using BuyBikeShop.Models;
using BuyBikeShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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
        /* public IActionResult Cart()
         {
             // Attempt to load the cart from a cookie
             Cart cookieCart = CartManager.LoadCartFromCookie(HttpContext, userManager);

             Cart sessionOrUserCart;
             if (User.Identity.IsAuthenticated)
             {
                 // For authenticated users, get the cart based on the user's ID
                 var userId = userManager.GetUserId(User);
                 sessionOrUserCart = CartManager.GetCartByUserId(userId);
             }
             else
             {
                 // For guest users, get the cart based on the session ID
                 sessionOrUserCart = CartManager.GetCart(HttpContext);
             }

             // Merge the carts from the cookie and the session/user-specific cart
             Cart mergedCart = MergeCarts(cookieCart, sessionOrUserCart);

             // Fetch product details for each cart item
             foreach (var item in mergedCart.CartItems)
             {
                 var product = _context.Products.Find(item.ProductId);
                 if (product != null)
                 {
                     item.Product = product; // Ensure each cart item has the latest product details
                 }
             }

             // Save the merged cart back into the session and cookie
             HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(mergedCart));
             CartManager.SaveCartInCookie(mergedCart, HttpContext, userManager);

             return View("Cart", mergedCart);
         }*/


        public IActionResult Cart()
        {
            // Load the main cart from the session or create a new one
            var mainCart = CartManager.GetCart(HttpContext);

            // Load the cart from the cookie
            var cookieCart = CartManager.LoadCartFromCookie(HttpContext, userManager);

            // Merge the carts, avoiding duplicates
            CartManager.MergeCarts(mainCart, cookieCart);

            // Save the merged cart back to the session and cookie for consistency
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(mainCart));
            CartManager.SaveCartInCookie(mainCart, HttpContext, userManager);

            // Fetch product details for each cart item, if necessary
            foreach (var item in mainCart.CartItems)
            {
                var product = _context.Products.Find(item.ProductId);
                if (product != null)
                {
                    item.Product = product; // Ensure each cart item has the latest product details
                }
            }

            return View("Cart", mainCart);
        }


        private Cart MergeCarts(Cart primaryCart, Cart secondaryCart)
        {
            var mergedCart = new Cart();
            mergedCart.CartItems = new List<CartItem>();

            // Add all items from the primary cart to the merged cart
            foreach (var item in primaryCart.CartItems)
            {
                mergedCart.CartItems.Add(new CartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity
                });
            }

            // Add or update items from the secondary cart in the merged cart
            foreach (var item in secondaryCart.CartItems)
            {
                var existingItem = mergedCart.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId);
                if (existingItem != null)
                {
                    existingItem.Quantity += item.Quantity; // Update quantity if the item exists
                }
                else
                {
                    mergedCart.CartItems.Add(new CartItem // Add new item if it doesn't exist
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity
                    });
                }
            }

            return mergedCart;
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
