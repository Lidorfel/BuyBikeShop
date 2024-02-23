using BuyBikeShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyBikeShop.Controllers
{
    public class AccessoryController : Controller
    {
        private readonly BuyBikeShopContext _context;

        public AccessoryController(BuyBikeShopContext context) 
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var Accessory = await _context.Products.Where(p => p.Class_Name == "Accessories").ToListAsync();
            return View(Accessory);
        }
    }
}
