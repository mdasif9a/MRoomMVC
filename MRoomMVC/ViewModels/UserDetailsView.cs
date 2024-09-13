using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MRoomMVC.ViewModels
{
    public class UserDetailsView
    {
        [Required(ErrorMessage = "Enter Your Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Enter Your Father's Name")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "Enter Your Email")]
        [EmailAddress(ErrorMessage = "Enater Valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Your Date Of Birth.")]
        public DateTime? Dob { get; set; }
        [Required(ErrorMessage = "Enter Your 10 Digit Mobile No")]
        [StringLength(10, ErrorMessage = "Only 10 Digit Number Allowed", MinimumLength = 10)]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Enter Your Address")]
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        [Required(ErrorMessage = "Enter Your Password")]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "Password must be between 6 and 8 characters long.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[\W_]).*$", ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
        public string Role { get; set; }
    }
}