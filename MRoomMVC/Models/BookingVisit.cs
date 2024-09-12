using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MRoomMVC.Models
{
    public class BookingVisit
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PropertyId { get; set; }
        [Required(ErrorMessage = "Enter Your Booking Visit Date.")]
        public DateTime? BookingTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }

    }
}