using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.ViewModels
{
    public class AssociatedEventsViewModel
    {
        public Guid EventId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MinTickets { get; set; }
        public int MaxTickets { get; set; }
        public float TicketPrice { get; set; }
        public string Description { get; set; }
        public string ProfilePhoto { get; set; }
        public string UserName { get; set; }

    }

    public class LocationData
    {
        public Guid ID { get; set; }
        public string Price { get; set; }
        public string Day { get; set; }
        public string CategoryId { get; set; }
        public List<AssociatedEventsViewModel> AssociatedEvents { get; set; }

    }
}
