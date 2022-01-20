using EventFool.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventFool.ViewModels
{
    public class UpdateEventViewModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string Description { get; set; }
        [Required]
        public float TicketPrice { get; set; }
        [Required]
        public int MinTickets { get; set; }
        [Required]
        public int MaxTickets { get; set; }
        
        [Required]
        public Guid LocationId { get; set; }
        [Required]
        [StringLength(50)]
        public string LocationName { get; set; }
        [Required]
        [StringLength(200)]
        public string Address { get; set; }
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        public decimal Longitude { get; set; }
        public string ProfilePhoto { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }
      
        public IEnumerable<Category> Categories { get; set; }


    }
}