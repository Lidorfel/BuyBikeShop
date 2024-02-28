using BuyBikeShop.Data;
using BuyBikeShop.Models;
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
        [HttpPost]
        public IActionResult GetFilteredProducts(string[] CategoriesChosen, string sortOption)
        {
            IQueryable<Product> products = _context.Products;

            if (CategoriesChosen != null && CategoriesChosen.Length > 0)
            {
                products = products.Where(p => CategoriesChosen.Contains(p.Sub_Class));
            }
            else
            {
                // If no categories are selected, apply the default filter for bikes
                products = products.Where(p => p.Class_Name == "Accessories");
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
                default:
                    // Handle default case, if needed
                    break;
            }

            // Return the partial view with the filtered and sorted products
            return PartialView("_ProductsPartialView", products.ToList());
        }
    }
}
