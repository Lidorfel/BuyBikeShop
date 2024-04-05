using BuyBikeShop.Models;

public class Cart
{
    public string CustomerId { get; set; } 
    public string SessionId { get; set; } 
    public List<CartItem> CartItems { get; set; } = new List<CartItem>();
}
