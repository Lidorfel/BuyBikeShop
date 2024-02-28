using BuyBikeShop.Data;
using BuyBikeShop.Models;
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
        [HttpPost]
        public IActionResult GetFilteredProducts(string[] CategoriesChosen, string sortOption, string priceRange)
        {
            string[] range = null;
            double minRange = double.MinValue;
            double maxRange = double.MaxValue;

            if (priceRange != null)
            {
                range = priceRange.Split('&');
                if (double.TryParse(range[0], out double x))
                {
                    minRange = x;
                }
                if (double.TryParse(range[1], out double y))
                {
                    maxRange = y;
                }
            }
            IQueryable<Product> products = _context.Products.Where(p=>p.Sale_Perc !=0 & (Math.Floor(p.Price * (1 - (p.Sale_Perc / 100.0))) >= minRange & Math.Floor(p.Price * (1 - (p.Sale_Perc / 100.0))) <= maxRange));

            if (CategoriesChosen != null && CategoriesChosen.Length > 0)
            {
                products = products.Where(p => CategoriesChosen.Contains(p.Class_Name));
            }
            else
            {
               
            }

            // Apply sorting based on the selected option
            switch (sortOption)
            {
                case "lowToHigh":
                    products = products.OrderBy(p => p.Price * (1 - (p.Sale_Perc / 100.0)));
                    break;
                case "highToLow":
                    products = products.OrderByDescending(p => p.Price * (1 - (p.Sale_Perc / 100.0)));
                    break;
                case "mostRating":
                    products = products.OrderByDescending(p => p.Rating);
                    break;
                case "specificDate":
                    products = products.OrderBy(p => p.ReleaseDate);
                    break;
                default:
                    // Handle default case, if needed
                    break;
            }

            // Return the partial view with the filtered and sorted products
            return PartialView("_ProductsPartialView", products.ToList());
        }
    }
}
