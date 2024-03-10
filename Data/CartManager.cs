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

    /* public static void AddToCart(string customerId, int productId, int quantity)
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
 */

    public static void AddToCart(Cart cart, int productId, int quantity)
    {
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

        // Depending on your setup, you might need to update the cart in your data store here
    }

    /*public static void UpdateCartItemQuantity(string customerId, int productId, int newQuantity)
    {
        var cart = GetCart(customerId);
        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (cartItem != null)
        {
            cartItem.Quantity = newQuantity;
        }

        // Depending on your application's structure, you might need to persist these changes to a database
    }*/

    public static void UpdateCartItemQuantity(Cart cart, int productId, int newQuantity)
    {
        // Find the cart item by productId
        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (cartItem != null)
        {
            // Update the quantity
            cartItem.Quantity = newQuantity;

            // Optionally, if you're using a database, persist these changes
            // This might involve calling your repository or DbContext to save changes
        }
        else
        {
            // If the item doesn't exist in the cart and the new quantity is greater than 0, add it to the cart
            // This is optional and depends on your application's requirements
            if (newQuantity > 0)
            {
                cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = newQuantity });
                // Again, persist these changes if necessary
            }
        }
    }






    /* public static void RemoveFromCart(string customerId, int productId)
     {
         var cart = GetCart(customerId);
         var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
         if (cartItem != null)
         {
             cart.CartItems.Remove(cartItem);
         }

         // Depending on your application's structure, you might need to persist these changes to a database
     }*/

    public static void RemoveFromCart(HttpContext httpContext, int productId)
    {
        var cart = GetCart(httpContext); // Retrieve the cart using HttpContext

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (cartItem != null)
        {
            cart.CartItems.Remove(cartItem);
            // Optionally, update the cart in the database or persistent storage if needed
        }
    }


    /* public static Cart GetCart(HttpContext httpContext)
     {
         string cartId = GetCartId(httpContext);
         var cart = Carts.GetOrAdd(cartId, _ => new Cart());

         if (string.IsNullOrEmpty(httpContext.User.Identity.Name))
         {
             cart.SessionId = cartId; // Set SessionId for guest carts
         }
         else
         {
             cart.CustomerId = httpContext.User.Identity.Name; // Set CustomerId for authenticated users
         }

         return cart;
     }
 */

    public static Cart GetCart(HttpContext httpContext)
    {
        string cartId;

        // Check if user is authenticated and has a user-specific cart
        if (!string.IsNullOrEmpty(httpContext.User.Identity.Name))
        {
            cartId = httpContext.User.Identity.Name;
        }
        else
        {
            // Use existing guest cart ID or generate a new one for guests
            cartId = httpContext.Session.GetString("CartId") ?? Guid.NewGuid().ToString();
            httpContext.Session.SetString("CartId", cartId);
        }

        return Carts.GetOrAdd(cartId, _ => new Cart());
    }


    private static string GetCartId(HttpContext httpContext)
    {
        // Check if we already have a cart ID in the session
        if (httpContext.Session.GetString("CartId") == null)
        {
            if (!string.IsNullOrEmpty(httpContext.User.Identity.Name))
            {
                // Authenticated user, use their username or user ID
                httpContext.Session.SetString("CartId", httpContext.User.Identity.Name);
            }
            else
            {
                // Guest user, generate a new unique ID
                httpContext.Session.SetString("CartId", Guid.NewGuid().ToString());
            }
        }

        return httpContext.Session.GetString("CartId");
    }

    public static Cart GetCartByUserId(string userId)
    {
        // Check if a cart for the user ID already exists
        Cart cart;
        if (!Carts.TryGetValue(userId, out cart))
        {
            // If not, create a new cart and add it to the dictionary
            cart = new Cart { CustomerId = userId };
            Carts.TryAdd(userId, cart);
        }
        return cart;
    }

    public static Cart GetCartBySessionId(string sessionId)
    {
        // Check if a cart for the session ID already exists
        Cart cart;
        if (!Carts.TryGetValue(sessionId, out cart))
        {
            // If not, create a new cart and add it to the dictionary
            cart = new Cart { SessionId = sessionId };
            Carts.TryAdd(sessionId, cart);
        }
        return cart;
    }





}
