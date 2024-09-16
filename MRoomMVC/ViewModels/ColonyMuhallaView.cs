using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRoomMVC.ViewModels
{
    public class ColonyMuhallaView
    {
        public int Id { get; set; }
        public int CountryId { get; set; }
        public int StateId { get; set; }
        public int CityId { get; set; }
        public string ColonyName { get; set; }
        public string PinCode { get; set; }
        public string Zone { get; set; }
        public string Status { get; set; }
        public string CountryName { get; set; }
        public string StateName { get; set; }
        public string CityName { get; set; }
    }
}
