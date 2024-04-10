using BuyBikeShop.Data;
using BuyBikeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

public class CartController : Controller
{
    private readonly UserManager<Customer> userManager;
    private readonly BuyBikeShopContext _context;

    public CartController(UserManager<Customer> userManager, BuyBikeShopContext context)
    {
        this.userManager = userManager;
        _context = context;
    }

    [HttpPost]


    public IActionResult AddToCart(int productId, int quantity)
    {
     
        var mainCart = CartManager.GetCart(HttpContext);

        
     
            var cookieCart = CartManager.LoadCartFromCookie(HttpContext, userManager);

            if (cookieCart.CartItems.Count > 0 && !ReferenceEquals(mainCart, cookieCart))
            {
                CartManager.MergeCarts(mainCart, cookieCart);
            }
        

    
        var existingItem = mainCart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (existingItem != null)
        {
          
            existingItem.Quantity += quantity;
        }
        else
        {
            CartManager.AddToCart(mainCart, productId, quantity);
        }

        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(mainCart));
        CartManager.SaveCartInCookie(mainCart, HttpContext, userManager);

        return RedirectToAction("Index", "Product", new { id = productId });
    }



    [HttpPost]
    public async Task<IActionResult> UpdateCartItemQuantity(int productId, int currentQuantity, string change)
    {
        
        var mainCart = CartManager.GetCart(HttpContext);

        var specificProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (specificProduct != null)
        {
            int newQuantity = currentQuantity + (change == "increase" ? 1 : -1);
            newQuantity = Math.Max(newQuantity, 1); 
            newQuantity = Math.Min(newQuantity, specificProduct.Quantity); 

            bool res = CartManager.UpdateCartItemQuantity(mainCart, productId, newQuantity);

            if (!res)
            {
                TempData["CartUpdateError"] = "Failed to update cart item quantity for product # " + productId + ", Check for new stock.";
            }
        }

        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(mainCart));
        CartManager.SaveCartInCookie(mainCart, HttpContext, userManager);

        return RedirectToAction("Cart", "CheckOut");
    }


    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        
        var mainCart = CartManager.GetCart(HttpContext);
        var cookieCart = CartManager.LoadCartFromCookie(HttpContext, userManager);

        CartManager.MergeCarts(mainCart, cookieCart);


        CartManager.RemoveFromCartP(mainCart, productId);


        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(mainCart));
        CartManager.SaveCartInCookie(mainCart, HttpContext, userManager);

        return RedirectToAction("Cart", "CheckOut");
    }

    public class UpdateCartItemModel
    {
        public int ProductId { get; set; }
        public int NewQuantity { get; set; }
    }

}
