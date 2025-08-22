using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Models;

namespace EasyFacturation.Domain.Interfaces
{
    public interface IQuoteLineRepository
    {
        Task<QuoteLine> GetQuoteLineByIdAsync(Guid id);
        Task<IEnumerable<QuoteLine>> GetAllQuoteLinesAsync();
        Task<QuoteLine> CreateQuoteLineAsync(QuoteLine lineine);
        Task<QuoteLine> UpdateQuoteLineAsync(QuoteLine line);
        Task<QuoteLine> DeleteQuoteLineAsync(Guid id);
    }
}
