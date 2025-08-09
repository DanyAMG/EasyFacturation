using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Domain.Models
{
    public class QuoteStatusHistory
    {
        [Key]
        public Guid Id { get; set; }

        public enum QuoteStatus
        {
            Waiting = 0,
            Late = 1,
            Reminded = 2,
            Accepted = 3,
            Refused = 4
        }

        //Foreign Key
        [Required]
        public Guid QuoteId { get; set; }
        public Quote Quote { get; set; }
    }
}
