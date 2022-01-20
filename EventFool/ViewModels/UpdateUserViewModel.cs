using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EventFool.Domain.Models;

namespace EventFool.ViewModels
{
    public class UpdateUserViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }
        
        public DateTime Birthdate { get; set; }

        [Required]
        [StringLength(80)]
        public string Gender { get; set; }

        public string ProfilePhoto { get; set; }

        public List<PhotoViewModel> Photos { get; set; }

    }
}