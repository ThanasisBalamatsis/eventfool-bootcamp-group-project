using EventFool.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.ViewModels
{
    public class EventSearchViewModel
    {
        //MapBoundsViewModel
        
        public decimal swLat { get; set; }
        public decimal swLng { get; set; }
        public decimal neLat { get; set; }
        public decimal neLng { get; set; }
        public string Price { get; set; }
        public string Day { get; set; }
        public string CategoryId { get; set; }
    }
}