using System.Collections.Concurrent;
using BuyBikeShop.Models;
using BuyBikeShop.Data;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using BuyBikeShop.Migrations;

public static class CartManager
{
 
    private static readonly ConcurrentDictionary<string, Cart> Carts = new ConcurrentDictionary<string, Cart>();
    

    public static Cart GetCart(string customerId)
    {
        return Carts.GetOrAdd(customerId, new Cart());
    }

 

    public static void AddToCart(Cart cart, int productId, int quantity)
    {

        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (cartItem != null)
        {
         
            cartItem.Quantity += quantity;
        }
        else
        {
            
            cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity });
        }

    }

    

    public static bool UpdateCartItemQuantity(Cart cart, int productId, int newQuantity)
    {
        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (cartItem != null)
        {
            if (newQuantity > 0)
            {
               
                cartItem.Quantity = newQuantity;
                return true;
            }
            else
            {
               
                cart.CartItems.Remove(cartItem);
                return false;
            }
        }
        else if (newQuantity > 0)
        {

            cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = newQuantity });
            return true;
        }
        return true;
       
    }

   

    public static void MergeCarts(Cart mainCart, Cart cartToMerge)
    {
        foreach (var itemToMerge in cartToMerge.CartItems)
        {
            var existingItem = mainCart.CartItems.FirstOrDefault(ci => ci.ProductId == itemToMerge.ProductId);
           
            if (existingItem == null)
            {
                
                mainCart.CartItems.Add(new CartItem
                {
                    ProductId = itemToMerge.ProductId,
                    Quantity = itemToMerge.Quantity
                });
            }
        }
    }







  

    public static void RemoveFromCartP(Cart cart, int productId)
    {
        var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
        if (cartItem != null)
        {
            cart.CartItems.Remove(cartItem); 
        }
        
    }

    public static void RemoveFromCart(Cart cart, int productId, HttpContext httpContext, UserManager<Customer> userManager)
    {

        var cookieCart = CartManager.LoadCartFromCookie(httpContext, userManager);

        
        CartManager.MergeCarts(cart, cookieCart);

     
        RemoveFromCartP(cart, productId);
        
        httpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
        CartManager.SaveCartInCookie(cart, httpContext, userManager);
    }




   
    public static Cart GetCart(HttpContext httpContext)
    {
        string cartId;

    
        if (!string.IsNullOrEmpty(httpContext.User.Identity.Name))
        {
            cartId = httpContext.User.Identity.Name;
        }
        else
        {
            
            cartId = httpContext.Session.GetString("CartId") ?? Guid.NewGuid().ToString();
            httpContext.Session.SetString("CartId", cartId);
        }

        return Carts.GetOrAdd(cartId, _ => new Cart());
    }
    public static void ResetCart(Cart c, HttpContext httpContext, UserManager<Customer> userManager)
    {
        var cookieCart = CartManager.LoadCartFromCookie(httpContext, userManager);

        
        CartManager.MergeCarts(c, cookieCart);

    
        c.CartItems = new List<CartItem>();
        
        
        httpContext.Session.SetString("Cart", JsonConvert.SerializeObject(c));
        CartManager.SaveCartInCookie(c, httpContext, userManager);
        
    }


   
    public static Cart GetCartByUserId(string userId)
    {
        
        Cart cart;
        if (!Carts.TryGetValue(userId, out cart))
        {
            
            cart = new Cart { CustomerId = userId };
            Carts.TryAdd(userId, cart);
        }
        return cart;
    }

    public static Cart GetCartBySessionId(string sessionId)
    {
        
        Cart cart;
        if (!Carts.TryGetValue(sessionId, out cart))
        {
            
            cart = new Cart { SessionId = sessionId };
            Carts.TryAdd(sessionId, cart);
        }
        return cart;
    }

    public static void SaveCartInCookie(Cart cart, HttpContext httpContext, UserManager<Customer> userManager)
    {
        string cookieName;

     
        if (httpContext.User.Identity.IsAuthenticated)
        {
            
            var userId = userManager.GetUserId(httpContext.User);

            
            cookieName = $"Cart_User_{userId}";
        }
        else
        {
         
            cookieName = "Cart_Guest";
        }

     
        string serializedCart = JsonConvert.SerializeObject(cart);

      
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

        
        if (httpContext.User.Identity.IsAuthenticated)
        {
            
            var userId = userManager.GetUserId(httpContext.User);
            string userCookieName = $"Cart_User_{userId}";
            serializedCart = httpContext.Request.Cookies[userCookieName];
        }
        else
        {
            
            serializedCart = httpContext.Request.Cookies["Cart_Guest"];
        }

      
        if (!string.IsNullOrEmpty(serializedCart))
        {
            try
            {
                cart = JsonConvert.DeserializeObject<Cart>(serializedCart);
            }
            catch
            {
                cart = null;
            }
        }

        
        return cart ?? new Cart(); 
    }







}
