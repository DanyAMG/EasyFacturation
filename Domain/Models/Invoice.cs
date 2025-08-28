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
        public int SequenceNumber { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public decimal TaxeRate { get; set; }

        //Foreign Key
        public Guid ClientID { get; set; }
        public Client Client { get; set; }
        public Guid AppOwnerId { get; set; }
        public AppOwner AppOwner { get; set; }
        public Guid? QuoteId { get; set; }
        public Quote Quote { get; set; }
    }
}
