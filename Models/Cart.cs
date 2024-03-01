using BuyBikeShop.Models;

public class Cart
{
    public string CustomerId { get; set; } // Existing property for authenticated users
    public string SessionId { get; set; } // Add this for guest users
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
}
