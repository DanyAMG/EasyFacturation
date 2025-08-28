using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Enums;

namespace EasyFacturation.Domain.Models
{
    public class QuoteStatusHistory
    {
        [Key]
        public Guid Id { get; set; }

        public QuoteStatus Status { get; set; }

        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        //Foreign Key
        public Guid QuoteId { get; set; }
        public Quote Quote { get; set; }
    }
}
