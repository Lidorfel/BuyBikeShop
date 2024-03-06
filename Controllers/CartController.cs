using BuyBikeShop.Data;
using BuyBikeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public IActionResult AddToCart(int productId, int quantity)
    {
        var cart = CartManager.GetCart(HttpContext);
        CartManager.AddToCart(cart, productId, quantity); // Pass the whole Cart object

        return RedirectToAction("Index", "Product", new { id = productId });
    }


    [HttpPost]
    /* public IActionResult UpdateCartItemQuantity(int productId, int newQuantity)
     {
         string customerId = userManager.GetUserId(User);
         CartManager.UpdateCartItemQuantity(customerId, productId, newQuantity);

         // Redirect back to the cart view or return a JSON response if using AJAX
         return RedirectToAction("Cart", "CheckOut");
     }*/

    [HttpPost]
    public async Task<IActionResult> UpdateCartItemQuantity(int productId, int currentQuantity, string change)
    {
        var cart = CartManager.GetCart(HttpContext);
        var specificProduct = await _context.Products.FirstOrDefaultAsync(p => p.Id == productId);

        int newQuantity = currentQuantity + (change == "increase" ? 1 : -1);
        newQuantity = Math.Max(newQuantity, 1); // Ensure quantity doesn't go below 0
     
        newQuantity = Math.Min(newQuantity, specificProduct.Quantity);

        CartManager.UpdateCartItemQuantity(cart, productId, newQuantity);

        return RedirectToAction("Cart", "CheckOut");
    }







    [HttpPost]
    /*public IActionResult RemoveFromCart(int productId)
    {
        string customerId = userManager.GetUserId(User);
        CartManager.RemoveFromCart(customerId, productId);

        // Redirect back to the cart view
        return RedirectToAction("Cart", "CheckOut");
    }*/

    [HttpPost]
    public IActionResult RemoveFromCart(int productId)
    {
        CartManager.RemoveFromCart(HttpContext, productId);

        // Redirect back to the cart view or to another appropriate view
        return RedirectToAction("Cart", "CheckOut");
    }


    public class UpdateCartItemModel
    {
        public int ProductId { get; set; }
        public int NewQuantity { get; set; }
    }

}
