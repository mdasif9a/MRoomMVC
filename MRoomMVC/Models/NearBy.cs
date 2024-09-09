﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MRoomMVC.Models
{
    public class NearBy
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int CountryId { get; set; }
        [Required]
        public int StateId { get; set; }
        [Required]
        public int CityId { get; set; }
        [Required]
        public string NearByName { get; set; }
        public string Status { get; set; }
        [NotMapped]
        public string CountryName { get; set; }
        [NotMapped]
        public string StateName { get; set; }
        [NotMapped]
        public string CityName { get; set; }
    }
}
