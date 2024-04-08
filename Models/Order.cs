namespace BuyBikeShop.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }

       
        public string? CustomerId { get; set; }
        public Customer? CustomerUser { get; set; }

        public string CustomerName { get; set; }

     
        public ICollection<OrderProduct> OrderProducts { get; set; }

       
        public double TotalPrice
        {
            get
            {
                
                return OrderProducts.Sum(op => op.Quantity * op.UnitPrice);
            }
        }
        public string? Status { get; set; }

    }
}
