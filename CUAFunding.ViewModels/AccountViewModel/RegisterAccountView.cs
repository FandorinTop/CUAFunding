using System;
using System.ComponentModel.DataAnnotations;

namespace CUAFunding.ViewModels.AccountViewModel
{
    public class RegisterAccountView
    {
        [Required]
        [EmailAddress]
        public string Email { get; set;}

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password are different")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public string Name { get; set; }

        public Uri CurrentUrl { get; set; }
    }
}
