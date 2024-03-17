using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using BuyBikeShop.Models;
namespace BuyBikeShop.ViewModels
{
    public class RegisterVM
    {
        [Required(ErrorMessage = "First Name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name should be between 2 - 20 letters.")]
        [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "First name must contain only letters.")]
        public string FName { get; set; }
        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Last name should be between 2 - 20 letters.")]
        [RegularExpression(@"^[A-Za-z\s]*$", ErrorMessage = "Last name must contain only letters.")]
        public string LName { get; set; }
        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone Number is required")]
        [RegularExpression(@"^05\d{8}$", ErrorMessage = "Phone Number must start with '05' and consist of another 8 digits.")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Birth Date is required")]
        [MinimumAge(16)]
        [DataType(DataType.Date),DisplayFormat(DataFormatString = "{0:dd/MM/YYYY}",ApplyFormatInEditMode = true)]
        public DateTime Birthdate { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password don't match.")]
        [DataType(DataType.Password)]

        public string ConfirmPassword { get; set; }
    }
}