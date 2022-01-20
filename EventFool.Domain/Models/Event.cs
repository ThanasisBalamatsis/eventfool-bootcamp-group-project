using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFool.Domain.Models
{
    public class Event : Entity
    {
        public Event()
        {
            Users = new HashSet<ApplicationUser>();
            Tickets = new HashSet<Ticket>();
            Photos = new HashSet<Photo>();
        }

        [Required]
        [StringLength (50)]
        public string Name { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a value greater than or equal to {1}")]
        public int MinTickets { get; set; }
        [Required]
        [GreaterThan("MinTickets", ErrorMessage = "Please enter a value greater than the minimum number of tickets")]
        public int MaxTickets { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a value greater than or equal to {1}")]
        public float TicketPrice { get; set; }
        [Required]
        [StringLength(250, ErrorMessage = "Please enter no more than {1} characters")]
        public string Description { get; set; }
        public string ProfilePhoto { get; set; }
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
        [Required]
        public virtual Location Location { get; set; }
        [Required]
        public virtual Category Category { get; set; }

    }
}
