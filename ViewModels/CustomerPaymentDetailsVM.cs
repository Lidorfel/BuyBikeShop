using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace BuyBikeShop.ViewModels
{

    public class CustomerPaymentDetailsVM
    {
        public CustomerPaymentDetailsVM()
        {
            

        }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "First name should be between 2 - 20 letters.")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = "First name must contain only letters.")]
        public string first_name { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Last name should be between 2 - 20 letters.")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = "Last name must contain only letters")]
        public string last_name { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Address should be between 2 - 20 letters.")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = "Address must contain only letters")]
        public string address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = "City must contain only letters")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "City name should be between 2 - 20 letters.")]
        public string city { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [RegularExpression(@"^[A-Za-z]*$", ErrorMessage = "Country must contain only letters")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Country should be between 4  - 30 letters.")]
        public string country { get; set; }


        [Required(ErrorMessage = "Zip/Postal Code is required")]
        [RegularExpression(@"^\d{5}$", ErrorMessage = "Zip Code must be exactly 5 digits.")]
        public string zip_code { get; set; }

        [Required(ErrorMessage =    "   Phone Number is required")]
        [RegularExpression(@"^05\d{8}$", ErrorMessage = "Phone Number must start with '05' and consist of another 8 digits.")]
        public string phone_number { get; set; }

        [Required(ErrorMessage = "Email Address is required")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email_address { get; set; }

        //Payment section

        [Required(ErrorMessage = "Credit Card Type is required")]
        public int CreditCardType { get; set; }

        [Required(ErrorMessage = "Credit Card Number is required")]
        [RegularExpression(@"^\d{16}$", ErrorMessage = "Invalid Credit Card Number")]
        public string car_number { get; set; }

        [Required(ErrorMessage = "CVV is required")]
        [RegularExpression(@"^\d{3}$", ErrorMessage = "Invalid CVV, should contain exactly 3 digits")]
        public string car_code { get; set; }

        [Required(ErrorMessage = "Expiration Month is required")]
        [FutureMonth(ErrorMessage = "Expiration Month must be in the future")]
        public string ExpirationMonth { get; set; }

        
        [Required(ErrorMessage = "Expiration Year is required")]
        [FutureYear(ErrorMessage = "Expiration Year must be in the future")]
        public int ExpirationYear { get; set; }


        //public string successMessage = "";
        //public string errorMessage = "";
        //public void OnGet() { }
        //public void OnPost()
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        errorMessage = "Data Validation failed";
        //        return;
        //    }

        //    successMessage = "Your Order has been placed.";
        //}

    }

    public class FutureMonthAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string month = value?.ToString();
            int currentYear = DateTime.Now.Year;
            int currentMonth = DateTime.Now.Month;

            // Cast validationContext.ObjectInstance to CustomerPaymentDetailsVM
            var paymentDetails = (CustomerPaymentDetailsVM)validationContext.ObjectInstance;

            if (!string.IsNullOrEmpty(month))
            {
                int selectedMonth;
                if (int.TryParse(month, out selectedMonth))
                {
                    // Access ExpirationYear property from paymentDetails
                    if (currentYear <= paymentDetails.ExpirationYear && selectedMonth < currentMonth)
                    {
                        return new ValidationResult("Expiration Month must be in the future.");
                    }
                }
            }

            return ValidationResult.Success;
        }
    }

    public class FutureYearAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int year = (int)value;
            int currentYear = DateTime.Now.Year;

            if (year < currentYear)
            {
                return new ValidationResult("Expiration Year must be in the future.");
            }

            return ValidationResult.Success;
        }
    }


}
