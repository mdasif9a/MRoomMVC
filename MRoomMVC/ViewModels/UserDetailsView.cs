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
        [Required(ErrorMessage = "Enter Your Email")]
        [EmailAddress(ErrorMessage = "Enater Valid Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Enter Your 10 Digit Mobile No")]
        [StringLength(10, ErrorMessage = "Only 10 Digit Number Allowed", MinimumLength = 10)]
        public string Mobile { get; set; }
        [Required(ErrorMessage = "Enter Your Address")]
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}