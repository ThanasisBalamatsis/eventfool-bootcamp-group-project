using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EventFool.Domain.Models;

namespace EventFool.ViewModels
{
    public class EventsCombinedViewModel
    {
        public List<Event> eventsOrganised { get; set; }
        public List<Event> eventsAttended { get; set; }
    }
}