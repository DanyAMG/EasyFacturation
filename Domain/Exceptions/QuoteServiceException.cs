using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Domain.Exceptions
{
    public class QuoteServiceException :Exception
    {
        public QuoteServiceException()
        {

        }

        public QuoteServiceException(string message) : base(message)
        {
        
        }

        public QuoteServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
