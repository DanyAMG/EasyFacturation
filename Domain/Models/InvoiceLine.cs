using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyFacturation.Domain.Models
{
    public class InvoiceLine
    {
        [Key]
        public Guid ID { get; set; }
        public string Description { get; set; }
        public decimal? UnitPrice { get; set; }
        public int? Quantity { get; set; }

        //Foreign Key
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
