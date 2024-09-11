using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MRoomMVC.ViewModels
{
    public class ApprovePro
    {
        public string PropertyId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public string ApprovedBy { get; set; }
        [Required]
        public string UniqueName { get; set; }
        [Required]
        public string VerifiedBy { get; set; }
    }
}