using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Models;

namespace EasyFacturation.Domain.Interfaces
{
    public interface IQuoteRepository
    {
        Task<Quote> GetQuoteByIdAsync(Guid id);
        Task<Quote> GetQuoteByNumberAsync(string number);
        Task<IEnumerable<Quote>> GetAllQuotesAsync();
        Task<Quote>CreateQuoteAsync(Quote quote);
        Task<Quote> UpdateQuoteAsync(Quote quote);
        Task<Quote> DeleteQuoteAsync(Guid id);
    }
}
