using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventFool.Domain.Models
{
    public class Ticket : Entity
    {

        public Guid EventId { get; set; }
        public string UserId { get; set; }
        [Required]
        public string QRCode { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Range(0, float.MaxValue, ErrorMessage = "Please enter a value greater than or equal to {1}")]
        public float Price { get; set; }
        [DefaultValue("true")]
        public bool Active { get; set; }
        [StringLength(100)]
        public string PayPalReference { get; set; }
        public virtual Event Event { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
