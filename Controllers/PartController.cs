using BuyBikeShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyBikeShop.Controllers
{
    public class PartController : Controller
    {
        private readonly BuyBikeShopContext _context;
        public PartController(BuyBikeShopContext context) { 
            _context = context;

        }
        public async Task<IActionResult> Index()
        {
            var Parts = await _context.Products.Where(p => p.Class_Name == "Parts").ToListAsync();
            return View(Parts);
        }
    }
}
