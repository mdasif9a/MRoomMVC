using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MRoomMVC.Models
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string PropertyId { get; set; }
    }
}