
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.ViewModels
{
    public class FilteredLocationsViewModel
    {
        public Guid LocationId { get; set; }
        public string Price { get; set; }
        public string Day { get; set; }
        public string CategoryId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int EventNumber { get; set; }
        public List<AssociatedEventsViewModel> AssociatedEvents { get; set; }
    }
}