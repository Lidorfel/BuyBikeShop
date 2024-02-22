using BuyBikeShop.Data;
using BuyBikeShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BuyBikeShop.Controllers
{
    public class ProductController : Controller
    {
        private readonly BuyBikeShopContext _context;
        public ProductController(BuyBikeShopContext context)
        {
            _context = context;
        }
        /*public async Task<IActionResult> Index(int id)
        {


            var specificProduct = await _context.Products
                .Where(p => p.Id == id).FirstOrDefaultAsync();
            if (specificProduct == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var ProductContext = await _context.Products
             .Where(p => p.Id != id && p.Sub_Class == specificProduct.Sub_Class)
             .ToListAsync()
             .ConfigureAwait(false);

            if (specificProduct != null && !ProductContext.Any(p => p.Id == specificProduct.Id))
            {
                ProductContext.Insert(0, specificProduct);
            }

            return View(ProductContext);
        }*/

        public async Task<IActionResult> Index(int id)
        {
            // Retrieve the specific product along with related products in one query
            var products = await _context.Products
                .Where(p => p.Id == id || (p.Sub_Class == _context.Products.FirstOrDefault(p => p.Id == id).Sub_Class && p.Id != id))
                .ToListAsync();

            // Check if the specific product exists
            var specificProduct = products.FirstOrDefault(p => p.Id == id);
            if (specificProduct == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // If needed, ensure the specific product is the first item in the list
            // This step may be redundant if you're already handling the specific product separately in your view
            if (!products.Any(p => p.Id == specificProduct.Id))
            {
                products.Insert(0, specificProduct);
            }

            return View(products);
        }

    }
}
