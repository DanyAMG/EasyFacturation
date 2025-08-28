using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using EasyFacturation.Domain.Enums;

namespace EasyFacturation.Domain.Models
{
    public class Quote
    {
        [Key]
        public Guid Id { get; set; }
        public string QuoteNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public decimal TaxeRate { get; set; }
        public QuoteStatus Status { get; set; }

        //If the quote is a correction of another quote
        public Guid? OriginalQuoteId { get; set; } //null if the quote is an original
        public Quote OriginalQuote { get; set; }

        //If the quote is an original and is corrected by a correction quote
        public Guid? CorrectionQuoteId { get; set; } //null if the quote is a correction
        public Quote CorrectionQuote { get; set; }

        //Foreign Key
        public Guid ClientId { get; set; }
        public Client Client { get; set; }

        public Guid AppOwnerId { get; set; }
        public AppOwner AppOwner { get; set; }

        public Invoice Invoice { get; set; }
        public List<QuoteLine> QuoteLines { get; set; }
        public List<QuoteStatusHistory> StatusHistory { get; set; }
    }
}
