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

        [RegularExpression(@"^[\w'\-,.][^0-9_!¡?÷?¿/\\+=@#$%ˆ&*(){}|~<>;:[\]]{2,}$", ErrorMessage = "Invalid symbols. Feel free to contact us if we're wrong.")] //any unicode that counts as a letter + spaces
        [Display(Name = "Name")]
        [Required(ErrorMessage = "We need your name!")]
        public String Name { get; set; }

        [Display(Name = "Email")]
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

        [Phone]
        [Display(Name = "Phone number")]
        [RegularExpression(@"^([\+]?(?:00)?[0-9]{1,3}[\s.-]?[0-9]{1,12})([\s.-]?[0-9]{1,4}?)$", ErrorMessage = "That's not your phone number!")] //some phone number regex
        public String Phone { get; set; }
    }
}
