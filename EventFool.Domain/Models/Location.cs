using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Spatial;
using System.Text;
using System.Threading.Tasks;

namespace EventFool.Domain.Models
{
    public class Location : Entity
    {
        public Location()
        {
            Events = new HashSet<Event>();
        }
        [Required(ErrorMessage = "Price is required")]
        public decimal Longitude { get; set; }
        [Required]
        public decimal Latitude { get; set; }
        [Required]
        [StringLength(200)]
        public string Address { get; set; }
        [Required]
        [StringLength(50)]
        public string Name { get; set; }


        public virtual ICollection<Event> Events { get; set; } 
    }
}
