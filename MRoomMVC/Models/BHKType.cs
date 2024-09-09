using System.ComponentModel.DataAnnotations;

namespace MRoomMVC.Models
{
    public class BHKType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string BHKName { get; set; }
        public string Status { get; set; }
    }
}
