using System.ComponentModel.DataAnnotations;

namespace BuyBikeShop.ViewModels
{
    public class RegisterVM
    {
        [Required]
        public string? FName { get; set; }
        [Required]
        public string? LName { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public DateTime Birthdate { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Compare("Password",ErrorMessage ="Password don't match.")]
        [DataType(DataType.Password)]

        public string? ConfirmPassword { get; set; }



    }
}
