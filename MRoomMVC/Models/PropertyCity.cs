using System.ComponentModel.DataAnnotations;

namespace MRoomMVC.Models
{
    public class PropertyCity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string PropertyCityName { get; set; }
        public string Status { get; set; }
    }
}
