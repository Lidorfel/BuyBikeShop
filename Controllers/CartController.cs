using BuyBikeShop.Data;
using BuyBikeShop.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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

        string customerId = userManager.GetUserId(User);

        CartManager.AddToCart(customerId, productId, quantity);

        return RedirectToAction("Index", "Product" ,new { id = productId });
    }

    [HttpPost]
    public IActionResult UpdateCartItemQuantity(int productId, int newQuantity)
    {
        string customerId = userManager.GetUserId(User);
        CartManager.UpdateCartItemQuantity(customerId, productId, newQuantity);

        // Redirect back to the cart view or return a JSON response if using AJAX
        return RedirectToAction("Cart", "CheckOut");
    }



}
