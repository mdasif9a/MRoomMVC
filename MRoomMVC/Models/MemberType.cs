using System.ComponentModel.DataAnnotations;

namespace MRoomMVC.Models
{
    public class MemberType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TypeName { get; set; }
        public string Status { get; set; }
    }
}
