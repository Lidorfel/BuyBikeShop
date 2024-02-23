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
           
            var specificProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);


            if (specificProduct == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var products = await _context.Products
                .Where(p => p.Sub_Class == specificProduct.Sub_Class)
                .ToListAsync();

          
            products.Remove(specificProduct);
            products.Insert(0, specificProduct);

            return View(products);
        }


    }
}
