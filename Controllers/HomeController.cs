using BuyBikeShop.Data;
using BuyBikeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BuyBikeShop.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BuyBikeShopContext _context;

        public HomeController(ILogger<HomeController> logger, BuyBikeShopContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ProductContext = await _context.Products.ToListAsync();
            return View(ProductContext);
        }
        [HttpGet]   
        public async Task<IActionResult> FilteredBySearch(string searchBar)
        {
            List<Product> products;
            if (string.IsNullOrEmpty(searchBar))
            {
                products = await _context.Products.ToListAsync();
            }
            else
            {
                products = await _context.Products.Where(p=>p.Class_Name.ToString().ToLower().Contains(searchBar.ToLower()) 
                || p.Sub_Class.ToString().ToLower().Contains(searchBar.ToLower()) 
                || p.Title.ToString().ToLower().Contains(searchBar.ToLower()) 
                || p.Color.ToString().ToLower().Contains(searchBar.ToLower())
                ).ToListAsync();
            }
            return View("Index",products);
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
            IQueryable<Product> products = _context.Products.Where(p => Math.Floor(p.Price * (1 - (p.Sale_Perc / 100.0))) >= minRange & Math.Floor(p.Price * (1 - (p.Sale_Perc / 100.0))) <= maxRange);

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
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
