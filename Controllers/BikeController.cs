using BuyBikeShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyBikeShop.Controllers
{
    public class BikeController : Controller
    {
        private readonly BuyBikeShopContext _context;
        public BikeController(BuyBikeShopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var bikes = await _context.Products.Where(p => p.Class_Name == "Bike").ToListAsync();

            return View(bikes);
        }
    }
}
