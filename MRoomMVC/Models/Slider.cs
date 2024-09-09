using System.ComponentModel.DataAnnotations;

namespace MRoomMVC.Models
{
    public class Slider
    {
        [Key]
        public int Id { get; set; }
        public string FilePath { get; set; }
    }
}
