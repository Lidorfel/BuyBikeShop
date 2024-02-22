using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BuyBikeShop.Models
{
    public class OrderProduct
    {
        public int OrderProductId { get; set; }

        // Foreign key for Order
        [ForeignKey("Order")]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        // Foreign key for Product
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public Product Product { get; set; }

        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
    }
}
