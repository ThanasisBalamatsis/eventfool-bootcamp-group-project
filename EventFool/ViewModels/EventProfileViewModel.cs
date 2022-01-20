using EventFool.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.ViewModels
{
    public class EventProfileViewModel
    {
        public Guid EventId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public float TicketPrice { get; set; }
        public int RemainingTickets { get; set; }
        public string ProfilePhoto { get; set; }
        public Category Category { get; set; }
        public Location Location { get; set; }
        public List<Photo> Photos { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<ApplicationUser> Attendees { get; set; }


    }
}