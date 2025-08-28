using EasyFacturation.Domain.Enums;
using EasyFacturation.Domain.Exceptions;
using EasyFacturation.Domain.Interfaces;
using EasyFacturation.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Infrastructure.Persistence
{
    public class QuoteRepository : IQuoteRepository
    {
        private readonly AppDbContext _context;

        public QuoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Quote> GetQuoteByIdAsync(Guid id)
        {
            return await _context.Quotes.FindAsync(id);
        }

        public async Task<IEnumerable<Quote>> GetQuotesAsync(
            string? quoteNumber = null,
            Guid? clientId = null,
            string? clientLastName = null,
            string? clientCompanyName = null,
            QuoteStatus? status = null,
            DateTime? from = null,
            DateTime? to = null)
        {
            var query = _context.Quotes
                .Include(q => q.Client)
                .Include(q => q.QuoteLines)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(quoteNumber))
            {
                query = query.Where(q => q.QuoteNumber == quoteNumber);
            }

            if (clientId.HasValue)
            {
                query = query.Where(q => q.ClientId == clientId.Value);
            }

            if (!string.IsNullOrWhiteSpace(clientLastName))
            {
                query = query.Where(q => q.Client.LastName.Contains(clientLastName));
            }

            if (!string.IsNullOrWhiteSpace(clientCompanyName))
            {
                query = query.Where(q => q.Client.CompanyName.Contains(clientCompanyName));
            }

            if (status.HasValue)
            {
                query = query.Where(q => q.Status == status.Value);
            }

            if (from.HasValue)
            {
                query = query.Where(q => q.CreationDate >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(q => q.CreationDate <= to.Value);
            }

            return await query.OrderByDescending(q => q.CreationDate).ToListAsync();
        }

        public async Task<Quote> CreateQuoteAsync(Quote quote)
        {
            _context.Quotes.Add(quote);
            await _context.SaveChangesAsync();
            return quote;
            
        }

        public async Task<Quote> UpdateQuoteAsync(Quote quote)
        {
            var existingQuote = await _context.Quotes.FindAsync(quote.Id);
            if (existingQuote == null)
            {
                throw new QuoteNotFoundException($"Quote with ID {quote.Id} not found.");
            }
           
                existingQuote.ClientId = quote.ClientId;
                existingQuote.ExpirationDate = quote.ExpirationDate;
                existingQuote.QuoteLines = quote.QuoteLines;
                existingQuote.Subtotal = quote.Subtotal;
                existingQuote.Total = quote.Total;
                existingQuote.TaxeRate = quote.TaxeRate;

                await _context.SaveChangesAsync();
                return existingQuote;
        }

        public async Task DeleteQuoteAsync(Guid id)
        {
            var existingQuote = await _context.Quotes.FindAsync(id);

            if (existingQuote == null)
            {
                throw new QuoteNotFoundException($"Quote with ID {id} not found.");
            }

            _context.Quotes.Remove(existingQuote);
            await _context.SaveChangesAsync();
        }
    }
}
