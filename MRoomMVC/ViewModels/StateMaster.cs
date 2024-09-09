using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRoomMVC.ViewModels
{
    public class StateMasterView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public string Status { get; set; }
        public string CountryName { get; set; }
    }
}
