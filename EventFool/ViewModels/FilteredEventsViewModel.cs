using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.ViewModels
{
    public class FilteredEventsViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid LocationId { get; set; }
        public string LocationName { get; set; }
        public DateTime StartDate { get; set; }
        public float TicketPrice { get; set; }
        public string ProfilePhoto { get; set; }
        public string UserName { get; set; }

    }
}