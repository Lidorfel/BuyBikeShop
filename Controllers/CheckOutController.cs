using BuyBikeShop.Data;
using BuyBikeShop.Models;
using BuyBikeShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;
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
        public PaymentVM insertProductsIntoCart(Cart cart)
        {
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
            return payVM;
        }
        [HttpGet]
        public IActionResult Payment()
        {
            TempData["DecryptedCreditNumber"] = "";
            TempData["DecryptedCVV"] = "";
            TempData["MonthOfCard"] = "";
            TempData["YearOfCard"] = "";
            if (User.Identity.IsAuthenticated)
            {
                var userId = userManager.GetUserId(User);
                var user = _context.Customers.FirstOrDefault(x => x.Id == userId);
                if (user.CreditCard != null)
                {
                    TempData["DecryptedCreditNumber"] = ManageAES.Decrypt(user.CreditCard.ToString(), KeyManager.LoadKey(), KeyManager.LoadIV());
                    TempData["DecryptedCVV"] = ManageAES.Decrypt(user.CVV.ToString(), KeyManager.LoadKey(), KeyManager.LoadIV());
                    string exp = ManageAES.Decrypt(user.ExpDate.ToString(), KeyManager.LoadKey(), KeyManager.LoadIV());
                    string[] values = exp.Split("/");
                    TempData["MonthOfCard"] = int.Parse(values[0]) >= 10 ? values[0] : "0" + values[0];
                    TempData["YearOfCard"] = values[1];
                }
            }
            var cart = CartManager.GetCart(HttpContext);
            if (cart.CartItems.Count==0)
            {
                return NotFound();
            }
            var payVM = insertProductsIntoCart(cart);
            return View("Payment", payVM);
        }

        [HttpPost]
        public IActionResult Payment(int productId, int quantity) // CustomerPaymentDetailsVM cp
        {
            TempData["DecryptedCreditNumber"] = "";
            TempData["DecryptedCVV"] = "";
            TempData["MonthOfCard"] = "";
            TempData["YearOfCard"] = "";
            if (User.Identity.IsAuthenticated)
            {
                var userId = userManager.GetUserId(User);
                var user = _context.Customers.FirstOrDefault(x => x.Id == userId);
                if (user.CreditCard != null)
                {
                    TempData["DecryptedCreditNumber"] = ManageAES.Decrypt(user.CreditCard.ToString(), KeyManager.LoadKey(), KeyManager.LoadIV());
                    TempData["DecryptedCVV"] = ManageAES.Decrypt(user.CVV.ToString(), KeyManager.LoadKey(), KeyManager.LoadIV());
                    string exp = ManageAES.Decrypt(user.ExpDate.ToString(), KeyManager.LoadKey(), KeyManager.LoadIV());
                    string[] values = exp.Split("/");
                    TempData["MonthOfCard"] = int.Parse(values[0])>=10 ? values[0] : "0"+values[0] ;
                    TempData["YearOfCard"] = values[1];
                }
            }
            var cart = CartManager.GetCart(HttpContext);
            CartManager.AddToCart(cart,productId, quantity);
            var payVM = insertProductsIntoCart(cart);
            return View("Payment",payVM); 
        }
        [HttpGet]

        public IActionResult CartPayment()
        {
            return Payment();
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

        public IActionResult ConfirmPurchase()
        {
            var orderNum = TempData["OrderNumber"] as string;
            return View("ConfirmPurchase",orderNum);
        }



        [HttpPost]
        public async Task <IActionResult> CreateOrder(PaymentVM pay)
        {
            var cart = CartManager.GetCart(HttpContext);
            try
            { 
                Customer cust = null;
                string customerName = pay.cp.first_name;
                List<OrderProduct> OrderProductsList = new List<OrderProduct>();
                
                foreach (var item in cart.CartItems)
                {
                    var pr = _context.Products.FirstOrDefault(i=>i.Id==item.ProductId);
                    if (pr!.Quantity < item.Quantity)
                    {
                        throw new Exception(pr!.Title +"&" + pr!.Id);
                    }
                    OrderProductsList.Add(new OrderProduct
                    {
                        Product = pr,
                        Quantity = item.Quantity,
                        UnitPrice = Math.Floor(item.Product.Price * (1 - (item.Product.Sale_Perc / 100.0)))
                    });
                    pr.Quantity -= item.Quantity;
                    _context.Products.Update(pr);

                }

                if (User.Identity.IsAuthenticated)
                {
                    cust = await userManager.GetUserAsync(User);
                    if (cust != null)
                    {
                        if (pay.cp.saveDetails)
                        {
                            cust.Street = pay.cp.address.ToString();
                            cust.City = pay.cp.city.ToString();
                            cust.Country = pay.cp.country.ToString();
                            cust.Zip = pay.cp.zip_code.ToString();
                            cust.CreditCard = ManageAES.Encrypt(pay.cp.car_number.ToString(), KeyManager.LoadKey(), KeyManager.LoadIV());//must be encrypt
                            cust.CVV = ManageAES.Encrypt(pay.cp.car_code.ToString(), KeyManager.LoadKey(), KeyManager.LoadIV());//must be encrypt
                            cust.ExpDate = ManageAES.Encrypt((pay.cp.ExpirationMonth + "/" + pay.cp.ExpirationYear), KeyManager.LoadKey(), KeyManager.LoadIV());
                            //must be encrypt
                            _context.Customers.Update(cust);
                        }
                        else
                        {
                            cust.Street = null;
                            cust.City = null;
                            cust.Country = null;
                            cust.Zip = null;
                            cust.CreditCard = null;//must be encrypt
                            cust.CVV = null;//must be encrypt
                            cust.ExpDate = null;//must be encrypt
                            _context.Customers.Update(cust);
                        }

                    }
                    else
                    {
                        NotFound();
                    }
                }
                Order order = new Order
                {
                    OrderDate = DateTime.Now,
                    CustomerId = cust != null ? cust.Id : null,
                    CustomerName = customerName,
                    OrderProducts = OrderProductsList,
                    Status="Order Placed"
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                if (User.Identity.IsAuthenticated)
                {
                    TempData["OrderNumber"] = order.OrderId.ToString();
                }
                CartManager.ResetCart(CartManager.GetCart(HttpContext));
                return ConfirmPurchase();
            }
            catch (Exception ex)
            {
                string[] values = ex.Message.Split("&");
                TempData["OrderNumber"] = "Sorry, there is not enough stock in this Product : " + values[0];
                CartManager.RemoveFromCart(cart, int.Parse(values[1]),HttpContext,userManager);
                return ConfirmPurchase();
            }
        }
    }
}
