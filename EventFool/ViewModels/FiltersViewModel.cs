using EventFool.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EventFool.ViewModels
{
    public class FiltersViewModel
    {
        public string Price { get; set; }
        public string Day { get; set; }
        public string CategoryId { get; set; }
        public List<Category> Categories { get; set; }

        

    }
}