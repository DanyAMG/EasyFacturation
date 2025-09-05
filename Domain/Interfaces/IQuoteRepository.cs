using EasyFacturation.Domain.Enums;
using EasyFacturation.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Domain.Interfaces
{
    public interface IQuoteRepository
    {
        Task<Quote> GetQuoteByIdAsync(Guid id);
        Task<IEnumerable<Quote>> GetQuotesAsync(
            string? quoteNumber = null,
            string? title = null,
            Guid? clientId = null,
            string? clientLastName = null,
            string? clientCompanyName = null,
            QuoteStatus? status = null,
            DateTime? from = null,
            DateTime? to = null);
        Task<Quote>CreateQuoteAsync(Quote quote);
        Task<Quote> UpdateQuoteAsync(Quote quote);
        Task DeleteQuoteAsync(Guid id);
    }
}
