using EventFool.Domain.Models;
using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using EventFool.Validations;

namespace EventFool.ViewModels
{
    public class EventCreateViewModel
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        [Required]
        [StringLength(250)]
        public string Description { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        public float TicketPrice { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value greater than or equal to {1}")]
        public int MinTickets { get; set; }
        [Required]
        [GreaterThan("MinTickets")]
        public int MaxTickets { get; set; }
        public Location Location { get; set; }
        public List<PhotoViewModel> Photos { get; set; }
        [Required]
        [Display(Name = "Category")]
        public Guid CategoryId { get; set; }
        public IEnumerable<Category> Categories { get; set; }

        public string ProfilePhoto { get; set; }
        [Required]
        [DateGreaterThanAttribute(ErrorMessage = "Please enter a valid start date.")]
        public DateTime StartDate { get; set; }
        [Required]
        [GreaterThan("StartDate", ErrorMessage = "Please enter a valid end date.")]
        public DateTime EndDate { get; set; }

        public string ErrorMessage { get; set; }
        
    }
}