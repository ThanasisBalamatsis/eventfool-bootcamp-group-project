using EventFool.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.ViewModels
{
    public class AdminEditEventViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MinTickets { get; set; }
        public int MaxTickets { get; set; }
        public float TicketPrice { get; set; }
        public string Description { get; set; }

        public Guid CategoryId { get; set; }
        public Guid LocationId { get; set; }
    }
}