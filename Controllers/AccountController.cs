using BuyBikeShop.Models;
using BuyBikeShop.ViewModels;
using BuyBikeShop.Data;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BuyBikeShop.Controllers
{
    public class AccountController : Controller
    {
        private readonly SignInManager<Customer> signInManager;
        private readonly UserManager<Customer> userManager;
		private readonly BuyBikeShopContext _context;

		public AccountController(SignInManager<Customer> signInManager, UserManager<Customer> userManager,BuyBikeShopContext context)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
			this._context = context;
		}
        public IActionResult Login()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction( "Index", "Home");
            else
                return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var guestCartId = HttpContext.Session.GetString("CartId");
                if (!string.IsNullOrEmpty(guestCartId))
                {
                    HttpContext.Session.SetString("GuestCartId", guestCartId);
                }
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if(result.Succeeded)
                {
                    var user = await userManager.FindByEmailAsync(model.Email);
                    var userCart = CartManager.LoadCartFromCookie(HttpContext, userManager); // Load the user's cart from the cookie
                    if (userCart == null || userCart.CartItems.Count == 0)
                    {
                        // If the user's cart is empty or doesn't exist, consider loading a default cart or creating a new one
                        userCart = new Cart();
                    }
                    CartManager.SaveCartInCookie(userCart, HttpContext, userManager); // Save the loaded or new cart back into a cookie
                    HttpContext.Session.SetString("CartId", user.Id.ToString()); // Set the session cart ID to the user's ID

                  
                    return RedirectToAction("Index", "Home");
                 
                }
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
            return View(model);
        }
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction( "Index", "Home");
            else
                return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                Customer cust = new Customer
                {
                    FName = Utils.CapitalizeFirstLetter(model.FName!),
                    LName = Utils.CapitalizeFirstLetter(model.LName!),
                    Phone = model.Phone!,
                    Birthdate = model.Birthdate,
                    Email = model.Email,
                    UserName = model.Email
                };
                var result =  await userManager.CreateAsync(cust,model.Password!);
                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(cust, false);
                    return RedirectToAction("Index", "Home");

                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            var userCookieName = $"Cart_User_{userManager.GetUserId(User)}";
            

            await signInManager.SignOutAsync();
            HttpContext.Session.Remove("CartId");
                                                  
            return RedirectToAction("Index", "Home");
		}
		[Authorize]
		public async Task<IActionResult> MyOrders()
		{

			Customer curUser = _context.Customers.FirstOrDefault(u => u.UserName == User.Identity.Name);
            
            List<Order> orders = await GetOrders(curUser!);
			return View(orders);
		}
		public async Task<List<Order>> GetOrders(Customer curUser=null)
		{
            if (curUser != null)
            {
                List<Order> orders = await _context.Orders.Where(o => o.CustomerId == curUser.Id)
                    .Include(o => o.CustomerUser)
                    .Include(o => o.OrderProducts)
                        .ThenInclude(op => op.Product)
                    .ToListAsync();

                return orders;
            }
            return new List<Order> ();
		}
		
       

    }
}
