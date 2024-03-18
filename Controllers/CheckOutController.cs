using BuyBikeShop.Data;
using BuyBikeShop.Models;
using BuyBikeShop.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Cryptography;

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

        public IActionResult ConfirmPurchase()
        {
            var orderNum = TempData["OrderNumber"] as string;
            return View("ConfirmPurchase",orderNum);
        }

        [HttpPost]
        public async Task <IActionResult> CreateOrder(PaymentVM pay)
        {
            try
            { 
                Customer cust = null;
                string customerName = pay.cp.first_name;
                List<OrderProduct> OrderProductsList = new List<OrderProduct>();
                var cart = CartManager.GetCart(HttpContext);
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
                CartManager.RemoveFromCart(HttpContext, int.Parse(values[1]));
                return ConfirmPurchase();
            }
        }
    }
}
