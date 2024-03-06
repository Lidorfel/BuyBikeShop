namespace BuyBikeShop.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }  // Assuming a navigation property to Product
        public int Quantity { get; set; }
    }
}
