using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Enums;
using EasyFacturation.Domain.Exceptions;
using EasyFacturation.Domain.Interfaces;
using EasyFacturation.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyFacturation.Infrastructure.Persistence
{
    public class QuoteStatusRepository : IQuoteStatusRepository
    {
        private readonly AppDbContext _context;

        public QuoteStatusRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<QuoteStatusHistory> GetQuoteStatusById(Guid id)
        {
            return await _context.QuoteStatusHistories.FindAsync(id);
        }

        public async Task<IEnumerable<QuoteStatusHistory>> GetQuoteStatusByQuoteIdAsync(Guid quoteId)
        {
            return await _context.QuoteStatusHistories.Where(qs => qs.QuoteId == quoteId).ToListAsync();
        }
        
        public async Task<QuoteStatusHistory> CreateQuoteStatusAsync(QuoteStatusHistory quoteStatus)
        {
            _context.QuoteStatusHistories.Add(quoteStatus);
            await _context.SaveChangesAsync();
            return quoteStatus;
        }

    }
}
