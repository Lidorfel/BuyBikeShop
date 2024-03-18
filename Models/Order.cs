namespace BuyBikeShop.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

        // Nullable foreign key for CustomerUser (if order is placed by a registered user)
        public string CustomerId { get; set; }
        public Customer CustomerUser { get; set; }

        // Customer information for non-registered users
        public string CustomerName { get; set; }

        // Navigation property for the products in the order
        public ICollection<OrderProduct> OrderProducts { get; set; }

        // Calculated property for the total price of the order
        public double TotalPrice
        {
            get
            {
                // Calculate total price based on the products in the order
                return OrderProducts.Sum(op => op.Quantity * op.UnitPrice);
            }
        }
        public string? Status { get; set; }

    }
}
