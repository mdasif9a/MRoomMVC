using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MRoomMVC.ViewModels
{
    public class BookingView
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Remarks { get; set; }
        public string PropertyId { get; set; }
        public DateTime? BookingTime { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsActive { get; set; }
    }
}