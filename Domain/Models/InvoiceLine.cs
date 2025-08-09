using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyFacturation.Domain.Models
{
    public class InvoiceLine
    {
        [Key]
        public Guid ID { get; set; }

        [Required]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }
        public int? Quantity { get; set; }

        //Foreign Key
        [Required]
        public Guid InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
