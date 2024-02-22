using BuyBikeShop.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BuyBikeShop.Data
{
    public class BuyBikeShopContext :IdentityDbContext<Customer>
    {
        public BuyBikeShopContext(DbContextOptions<BuyBikeShopContext> options):base(options) { }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderProduct> OrderProducts { get; set; }
        public DbSet<Product> Products { get; set; }
    }

}
