using EventFool.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.ViewModels
{
    public class EventsAttendedViewModel
    {
        public IEnumerable<Event> ActiveEvents { get; set; }
        public IEnumerable<Event> InactiveEvents { get; set; }
    }
}