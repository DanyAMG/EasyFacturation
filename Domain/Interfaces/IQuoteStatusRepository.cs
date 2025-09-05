using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Models;

namespace EasyFacturation.Domain.Interfaces
{
    public interface IQuoteStatusRepository
    {
        Task<QuoteStatusHistory> GetQuoteStatusById(Guid id);
        Task<IEnumerable<QuoteStatusHistory>> GetQuoteStatusByQuoteIdAsync(Guid quoteId);
        Task<QuoteStatusHistory> CreateQuoteStatusAsync(QuoteStatusHistory quoteStatus);
    }
}
