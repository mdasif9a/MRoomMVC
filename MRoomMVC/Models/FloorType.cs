using System.ComponentModel.DataAnnotations;

namespace MRoomMVC.Models
{
    public class FloorType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string FloorTypeName { get; set; }
        public string Status { get; set; }
    }
}
