using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Domain.Exceptions
{
    public class ClientServiceException :Exception
    {
        public ClientServiceException()
        {

        }

        public ClientServiceException(string message) : base(message)
        {
        
        }

        public ClientServiceException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
