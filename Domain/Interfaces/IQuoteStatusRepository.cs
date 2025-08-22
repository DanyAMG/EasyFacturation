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
        Task<QuoteStatusHistory> GetQuoteStatusByIdAsync(Guid id);
        Task<IEnumerable<QuoteStatusHistory>> GetAllQuoteStatusAsync();
        Task<QuoteStatusHistory> CreateQuoteStatusAsync(QuoteStatusHistory quoteStatus);
        Task<QuoteStatusHistory> UpdateQuoteStatusAsync(QuoteStatusHistory quoteStatus);
        Task<QuoteStatusHistory> DeleteQuoteStatusAsync(Guid id);
    }
}
