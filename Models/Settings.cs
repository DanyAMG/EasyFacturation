using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Models
{
    public class Settings
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Language { get; set; }

        [Required]
        [Range(0, 100)]
        [Column(TypeName = "decimal(5,2)")]
        public decimal DefaultTaxRate { get; set; }

        [Required]
        public string Currency {  get; set; }
    }
}
