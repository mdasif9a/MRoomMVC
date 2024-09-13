using System;
using System.ComponentModel.DataAnnotations;

namespace MRoomMVC.Models
{
    public class UserDetails
    {
        [Key]
        public int Id { get; set; }
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
        public int LoginId { get; set; }
    }
}