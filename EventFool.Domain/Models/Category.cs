using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFool.Domain.Models
{
    public class Category : Entity
    {
        public Category()
        {
            Events = new HashSet<Event>();
        }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Event> Events { get; set; }
    }
}
