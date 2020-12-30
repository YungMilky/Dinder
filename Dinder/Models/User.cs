using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Dinder.Models
{
    public class User
    {
        [Display(Name = "User ID")]
        public int UserID { get; set; }

        [Display(Name = "First name")]
        [Required(ErrorMessage = "We need your name!")]
        public String Firstname { get; set; }

        [Display(Name = "Last name")]
        [Required(ErrorMessage = "We need your last name!")]
        public String Lastname { get; set; }

        [Display(Name = "Email adress")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "We need your email.")]
        public String Email { get; set; }

        [Display(Name = "Confirm email adress")]
        [Compare("Email", ErrorMessage = "The emails don't match.")]
        public String ConfirmEmail { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [StringLength(50, MinimumLength = 12, ErrorMessage = "Your password has to be 12-50 characters.")]
        [Required(ErrorMessage = "We need a password!")]
        public String Password { get; set; }

        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The passwords don't match.")]
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }
    }
}
