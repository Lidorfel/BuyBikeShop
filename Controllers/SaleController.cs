using BuyBikeShop.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyBikeShop.Controllers
{
    public class SaleController : Controller
    {
        private readonly BuyBikeShopContext _context;
        public SaleController(BuyBikeShopContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var productSale = await _context.Products.Where(p => p.Sale_Perc != 0).ToListAsync();

            return View(productSale);
        }
    }
}
