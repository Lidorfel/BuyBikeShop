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
    /*public IActionResult AddToCart(int productId, int quantity)
    {


        string customerId = userManager.GetUserId(User);



        var cart = CartManager.GetCart(HttpContext);
        // Assuming AddToCart in CartManager is updated to accept a Cart object or cartId
        CartManager.AddToCart(cart.CustomerId ?? cart.SessionId, productId, quantity);

        return RedirectToAction("Index", "Product", new { id = productId });

    }*/

    [HttpPost]
    /*public IActionResult AddToCart(int productId, int quantity)
    {
        var cart = CartManager.GetCart(HttpContext);
        CartManager.AddToCart(cart, productId, quantity); // Pass the whole Cart object

        return RedirectToAction("Index", "Product", new { id = productId });
    }
*/

    public IActionResult AddToCart(int productId, int quantity)
    {
        // Load the main cart (session or user cart)
        var mainCart = CartManager.GetCart(HttpContext);

        // Only perform merging if the user is not authenticated (guest user scenario)
        if (!User.Identity.IsAuthenticated)
        {
            var cookieCart = CartManager.LoadCartFromCookie(HttpContext, userManager);

            // Merge the cookie cart into the main cart only if they are different and cookie cart has items
            if (cookieCart.CartItems.Count > 0 && !ReferenceEquals(mainCart, cookieCart))
            {
                CartManager.MergeCarts(mainCart, cookieCart);
            }
        }

        // Check if the product already exists in the cart to update quantity instead of adding a new entry
        var existingItem = mainCart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (existingItem != null)
        {
            // Product exists, update quantity
            existingItem.Quantity += quantity;
        }
        else
        {
            // Product does not exist, add new
            CartManager.AddToCart(mainCart, productId, quantity);
        }

        // Save the updated main cart into the session and cookie
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(mainCart));
        CartManager.SaveCartInCookie(mainCart, HttpContext, userManager);

        return RedirectToAction("Index", "Product", new { id = productId });
    }



    [HttpPost]
    public async Task<IActionResult> UpdateCartItemQuantity(int productId, int currentQuantity, string change)
    {
        // Load the main cart from the session or cookie
        var mainCart = CartManager.GetCart(HttpContext);

        // Perform the quantity update
        var specificProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);
        if (specificProduct != null)
        {
            int newQuantity = currentQuantity + (change == "increase" ? 1 : -1);
            newQuantity = Math.Max(newQuantity, 1); // Ensure quantity doesn't go below 1
            newQuantity = Math.Min(newQuantity, specificProduct.Quantity); // Ensure quantity doesn't exceed stock

            CartManager.UpdateCartItemQuantity(mainCart, productId, newQuantity);
        }

        // Save the updated main cart back to the session and cookie
        HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(mainCart));
        CartManager.SaveCartInCookie(mainCart, HttpContext, userManager);

        return RedirectToAction("Cart", "CheckOut");
    }


    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        // Load the main cart (session or user cart) and the cookie cart
        var mainCart = CartManager.GetCart(HttpContext);
        var cookieCart = CartManager.LoadCartFromCookie(HttpContext, userManager);

        // Merge the cookie cart into the main cart
        CartManager.MergeCarts(mainCart, cookieCart);

        // Perform the remove action on the merged cart
        CartManager.RemoveFromCartP(mainCart, productId);

        // Save the updated merged cart back to the session and cookie
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
