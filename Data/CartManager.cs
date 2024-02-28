using System.Collections.Concurrent;
using BuyBikeShop.Models;
using BuyBikeShop.Data; 

public static class CartManager
{
    // ConcurrentDictionary to handle concurrent access in a web environment
    private static readonly ConcurrentDictionary<string, Cart> Carts = new ConcurrentDictionary<string, Cart>();

    public static Cart GetCart(string customerId)
    {
        return Carts.GetOrAdd(customerId, new Cart());
    }

    public static void AddToCart(string customerId, int productId, int quantity)
    {
        var cart = GetCart(customerId);

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (cartItem != null)
        {
            // If the product is already in the cart, just update the quantity
            cartItem.Quantity += quantity;
        }
        else
        {
            // Add the new item to the cart
            cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity });
        }
    }

    public static void UpdateCartItemQuantity(string customerId, int productId, int newQuantity)
    {
        var cart = GetCart(customerId);
        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (cartItem != null)
        {
            cartItem.Quantity = newQuantity;
        }

        // Depending on your application's structure, you might need to persist these changes to a database
    }



    // Optionally, implement methods to remove items from the cart, clear the cart, etc.
}
