using Microsoft.AspNetCore.Identity;

namespace BuyBikeShop.Models
{
    public class Customer : IdentityUser
    {
        public string FName { get; set; }
        public string LName { get; set; }
        public string Phone { get; set; }
        public DateTime Birthdate { get; set; }
        public string? CreditCard { get; set; }
        public DateTime? ExpDate { get; set; }
        public int? CVV { get; set; }
        public string? Street { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Zip { get; set; }
    }
}
