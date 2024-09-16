using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRoomMVC.ViewModels
{
    public class PropertyVariantView
    {
        public int Id { get; set; }
        public int PropertyTypeId { get; set; }
        public string PropertyVariantName { get; set; }
        public string Status { get; set; }

        public string PropertyTypeName { get; set; }
    }
}
