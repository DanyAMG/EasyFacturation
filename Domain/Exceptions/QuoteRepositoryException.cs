using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Domain.Exceptions
{
    public class QuoteRepositoryException :Exception
    {
        public QuoteRepositoryException()
        {

        }

        public QuoteRepositoryException(string message) : base(message)
        {
        
        }

        public QuoteRepositoryException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
