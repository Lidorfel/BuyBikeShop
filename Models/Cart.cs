using System.Collections.Generic;

namespace BuyBikeShop.Models
{
    public class Cart
    {
        public string CustomerId { get; set; }  // Still useful for identifying the cart owner, if needed
        public List<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
