﻿using System.ComponentModel.DataAnnotations;

namespace MRoomMVC.Models
{
    public class ElevatorType
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Status { get; set; }
    }
}
