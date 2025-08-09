using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Domain.Models
{
    public class Invoice
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public int InvoiceNumber { get; set; }

        [Required]
        public DateTime CreationDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [Required]
        [Range (0, 100)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxeRate { get; set; }

        //Foreign Key
        [Required]
        public Guid ClientID { get; set; }
        public Client Client { get; set; }

        [Required]
        public Guid AppOwnerId { get; set; }
        public AppOwner AppOwner { get; set; }

        public Guid? QuoteId { get; set; }
        public Quote Quote { get; set; }
    }
}
