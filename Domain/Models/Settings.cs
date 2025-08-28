using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Domain.Models
{
    public class Settings
    {
        [Key]
        public Guid Id { get; set; }

        // - General -
        public string Language { get; set; }
        public decimal DefaultTaxRate { get; set; }
        public string Currency {  get; set; }

        // - Quote Numerotation -
        public string QuoteNumberPrefix { get; set; } = "DEV";
        public string QuoteNumberFormat { get; set; } = "{prefix}-{year}-{sequence}";
        public int QuoteSequence { get; set; } = 1;


        // - Invoice Numerotation -
        public string InvoiceNumberPrefix { get; set; } = "FACT";
        public string InvoiceNumberFormat { get; set; }= "{prefix}-{year}-{sequence}";
        public int InvoiceSequence { get; set; } = 1;
    }
}
