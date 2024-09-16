using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRoomMVC.ViewModels
{
    public class CityMasterView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StateId { get; set; }
        public int CountryId { get; set; }
        public string Status { get; set; }
        public string StateName { get; set; }
        public string CountryName { get; set; }
    }
}
