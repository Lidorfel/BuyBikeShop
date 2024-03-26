using System.Collections.Concurrent;
using BuyBikeShop.Models;
using BuyBikeShop.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;

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
        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (cartItem != null)
        {
            if (newQuantity > 0)
            {
                // Update the quantity of the existing item
                cartItem.Quantity = newQuantity;
            }
            else
            {
                // If the new quantity is zero or negative, remove the item from the cart
                cart.CartItems.Remove(cartItem);
            }
        }
        else if (newQuantity > 0)
        {
            // If the item doesn't exist and the new quantity is positive, add the new item
            cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = newQuantity });
        }
        // No action if the item doesn't exist and new quantity is zero or negative
    }

    /*   public static void MergeCarts(Cart mainCart, Cart cartToMerge)
       {
           foreach (var itemToMerge in cartToMerge.CartItems)
           {
               var existingItem = mainCart.CartItems.FirstOrDefault(ci => ci.ProductId == itemToMerge.ProductId);
               if (existingItem != null)
               {
                   // If the item exists in the main cart, update the quantity.
                   // If needed, adjust the logic here to handle specific merge rules (e.g., taking the max, summing quantities, etc.)
                   existingItem.Quantity = Math.Max(existingItem.Quantity, itemToMerge.Quantity);
               }
               else
               {
                   // If the item doesn't exist in the main cart, add it.
                   mainCart.CartItems.Add(new CartItem
                   {
                       ProductId = itemToMerge.ProductId,
                       Quantity = itemToMerge.Quantity
                   });
               }
           }
       }
   */

    public static void MergeCarts(Cart mainCart, Cart cartToMerge)
    {
        foreach (var itemToMerge in cartToMerge.CartItems)
        {
            var existingItem = mainCart.CartItems.FirstOrDefault(ci => ci.ProductId == itemToMerge.ProductId);
            // If the item exists in the main cart, don't do anything to avoid duplicates
            if (existingItem == null)
            {
                // If the item doesn't exist in the main cart, add it.
                mainCart.CartItems.Add(new CartItem
                {
                    ProductId = itemToMerge.ProductId,
                    Quantity = itemToMerge.Quantity
                });
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

    public static void RemoveFromCartP(Cart cart, int productId)
    {
        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (cartItem != null)
        {
            cart.CartItems.Remove(cartItem); // Remove the item from the cart
        }
        // If the item does not exist in the cart, no action is required.
    }

    public static void RemoveFromCart(Cart cart, int productId, HttpContext httpContext, UserManager<Customer> userManager)
    {

        var cookieCart = CartManager.LoadCartFromCookie(httpContext, userManager);

        // Merge the cookie cart into the main cart
        CartManager.MergeCarts(cart, cookieCart);

        // Perform the remove action on the merged cart
        RemoveFromCartP(cart, productId);
        // Save the updated merged cart back to the session and cookie
        httpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        CartManager.SaveCartInCookie(cart, httpContext, userManager);
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
    public static void ResetCart(Cart c)
    {
        if (c.CartItems.Count>0)
        {
            c.CartItems = new List<CartItem>();
        }
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

    public static void SaveCartInCookie(Cart cart, HttpContext httpContext, UserManager<Customer> userManager)
    {
        string cookieName;

        // Check if the user is authenticated
        if (httpContext.User.Identity.IsAuthenticated)
        {
            // Get the user ID
            var userId = userManager.GetUserId(httpContext.User);

            // Use the user ID for the cookie name
            cookieName = $"Cart_User_{userId}";
        }
        else
        {
            // Use a generic name for the guest cart cookie
            cookieName = "Cart_Guest";
        }

        // Serialize the cart object to a JSON string
        string serializedCart = JsonConvert.SerializeObject(cart);

        // Create the cookie with the serialized cart
        httpContext.Response.Cookies.Append(cookieName, serializedCart, new CookieOptions
        {
            Expires = DateTime.UtcNow.AddDays(7),
            HttpOnly = true
        });
    }

    public static Cart LoadCartFromCookie(HttpContext httpContext, UserManager<Customer> userManager)
    {
        Cart cart = null;
        string serializedCart;

        // Check if the user is authenticated
        if (httpContext.User.Identity.IsAuthenticated)
        {
            // For an authenticated user, try to retrieve the user-specific cart cookie
            var userId = userManager.GetUserId(httpContext.User);
            string userCookieName = $"Cart_User_{userId}";
            serializedCart = httpContext.Request.Cookies[userCookieName];
        }
        else
        {
            // For a guest, try to retrieve the guest cart cookie
            serializedCart = httpContext.Request.Cookies["Cart_Guest"];
        }

        // If a serialized cart is found in the cookie, deserialize it
        if (!string.IsNullOrEmpty(serializedCart))
        {
            try
            {
                cart = JsonConvert.DeserializeObject<Cart>(serializedCart);
            }
            catch
            {
                // Handle potential deserialization errors, log if necessary
                cart = null;
            }
        }

        // Return the deserialized cart or null if not found or deserialization failed
        return cart ?? new Cart(); // Consider returning a new Cart instead of null to avoid null reference errors elsewhere
    }







}
