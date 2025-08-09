using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Models
{
    public class QuoteLine
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Description { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? UnitPrice { get; set; }

        public decimal? Quantity { get; set; }

        //Foreign Key
        [Required]
        public Guid QuoteId { get; set; }
        public Quote Quote { get; set; }
    }
}
