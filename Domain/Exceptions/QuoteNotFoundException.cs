using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Domain.Exceptions
{
    public class QuoteNotFoundException : Exception
    {
        public QuoteNotFoundException()
        {
            
        }

        public QuoteNotFoundException(string message)
            : base(message)
        {
            
        }

        public QuoteNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
            
        }
    }
}
