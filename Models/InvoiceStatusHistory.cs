using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Models
{
    public class InvoiceStatusHistory
    {
        [Key]
        public Guid Id { get; set; }

        
        public enum InvoiceStatus
        {
            Waiting = 0,
            PartiallyPaid = 1,
            Paid = 2
        }

        //Foreign Key
        [Required]
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
