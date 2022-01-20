using EventFool.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EventFool.ViewModels
{
    public class CreateTicketViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Event Name")]
        public string Name { get; set; }
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        [Display(Name = "Ticket Price")]
        public float TicketPrice { get; set; }
        [Display(Name = "Location")]
        public string LocationName { get; set; }

    }
}