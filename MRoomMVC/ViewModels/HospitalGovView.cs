using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRoomMVC.ViewModels
{
    public class HospitalGovView
    {
        
        public int Id { get; set; }
        
        public int CountryId { get; set; }
        
        public int StateId { get; set; }
        
        public int CityId { get; set; }

        
        public string Name { get; set; }
        public string Status { get; set; }

        
        public string CountryName { get; set; }
        
        public string StateName { get; set; }
        
        public string CityName { get; set; }

    }
}
