using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Domain.Enums
{
    public enum QuoteStatus
    {
            Draft = 0,
            Sent = 1,
            Accepted = 2,
            Rejected = 3,
            Archived = 4
    }
}
