using EasyFacturation.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.AppServices.DTOs
{
    public class InvoiceCreateDTO
    {
        [Required]
        public string Title { get; set; }

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
        [Range(0, 100)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal TaxeRate { get; set; }
    }
}
