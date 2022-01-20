using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFool.Domain.Models
{
    public class Photo : Entity
    {
        public Photo()
        {
            Events = new HashSet<Event>();
        }
        
        [Required]
        public string Image { get; set; }

        public virtual ICollection<Event> Events { get; set; }
        [Required]
        public virtual ApplicationUser User { get; set; }
    }
}
