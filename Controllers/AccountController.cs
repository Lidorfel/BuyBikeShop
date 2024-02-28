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
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "Invalid login attempt");
                return View(model);
            }
            return View(model);
        }
        public IActionResult Register()
        {
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
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
		}
		[Authorize]
		public async Task<IActionResult> MyOrders()
		{
            //can't be null because [Authorize]
			Customer curUser = _context.Customers.FirstOrDefault(u => u.UserName == User.Identity.Name);
            /*await CreateOrderForTest(curUser!);*/
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
		public async Task CreateOrderForTest(Customer curUser = null)
		{

			// Assume you have retrieved products from the database
			List<Models.Product> products = await _context.Products.Take(2).ToListAsync(); // Taking two products for the order

			// Create the order
			Order order = new Order
			{
				OrderDate = DateTime.Now,
				CustomerId = curUser != null ? curUser.Id : null,
                CustomerName = curUser != null ? curUser.FName.ToString() +curUser.LName.ToString() : null , //instead of null, can take name from payment page
				OrderProducts = new List<OrderProduct>
                    {
	                    new OrderProduct
	                    {
		                    Product = products[0],
		                    Quantity = 3, // Assume the quantity ordered is 2
                            UnitPrice = products[0].Price // Assume the unit price is the product's price
                        },
	                    new OrderProduct
	                    {
		                    Product = products[1],
		                    Quantity = 2, // Assume the quantity ordered is 1
                            UnitPrice = products[1].Price // Assume the unit price is the product's price
	                    }
                    }
			};

			// Add order to the database
			_context.Orders.Add(order);
			await _context.SaveChangesAsync();
		}
	}
}
