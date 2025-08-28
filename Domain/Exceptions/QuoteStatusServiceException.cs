using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Domain.Exceptions
{
    public class QuoteStatusServiceException :Exception
    {
        public QuoteStatusServiceException()
        {

        }

        public QuoteStatusServiceException(string message) : base(message)
        {
        
        }

        public QuoteStatusServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
