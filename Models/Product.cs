using System.ComponentModel.DataAnnotations;

namespace BuyBikeShop.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        public string Class_Name { get; set; }
        public string Sub_Class { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public int Sale_Perc { get; set; }
        public string Photo { get; set; }
        public string Color { get; set; }
        public DateTime ReleaseDate { get; set; }
        public int? Age_Limit { get; set; }
        public double Rating { get; set; }
    }
}
