using BuyBikeShop.Data;
using BuyBikeShop.Models;
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

        [HttpPost]
        public IActionResult GetFilteredProducts(string[] CategoriesChosen, string sortOption,string priceRange)
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
            IQueryable<Product> products = _context.Products.Where(p => Math.Floor(p.Price * (1 - (p.Sale_Perc / 100.0))) >= minRange & Math.Floor(p.Price * (1 - (p.Sale_Perc / 100.0))) <= maxRange);

            if (CategoriesChosen != null && CategoriesChosen.Length > 0)
            {
                products = products.Where(p => CategoriesChosen.Contains(p.Sub_Class));
            }
            else
            {
                // If no categories are selected, apply the default filter for bikes
                products = products.Where(p => p.Class_Name == "Bike");
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
