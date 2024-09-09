using System.ComponentModel.DataAnnotations;

namespace MRoomMVC.Models
{
    public class PropertyType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PropertyTypeName { get; set; }
        public string Status { get; set; }
    }
}
