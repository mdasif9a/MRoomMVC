using System.ComponentModel.DataAnnotations;

namespace MRoomMVC.Models
{
    public class UserLogin
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Your Username")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Enter Your Password")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Select Role")]
        public string Role { get; set; }
        public bool IsRemember { get; set; }
    }
}
