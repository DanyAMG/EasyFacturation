using EasyFacturation.AppServices.DTOs;
using EasyFacturation.Domain.Enums;
using EasyFacturation.Domain.Exceptions;
using EasyFacturation.Domain.Interfaces;
using EasyFacturation.Domain.Models;
using EasyFacturation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace EasyFacturation.AppServices.Services
{
    public class QuoteService
    {
        private readonly IQuoteRepository _quoteRepository;
        private readonly IClientRepository _clientRepository;
        private readonly SettingsService _settingsService;
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public QuoteService(IQuoteRepository quoteRepository, IClientRepository clientRepository, ILogger logger, SettingsService settingsService, AppDbContext context)
        {
            _quoteRepository = quoteRepository;
            _clientRepository = clientRepository;
            _settingsService = settingsService;
            _logger = logger;
            _context = context;
        }

        public async Task<List<Quote>> GetQuotesAsync(
            string? quoteNumber = null,
            Guid? clientId = null,
            string? clientLastName = null,
            string? clientCompanyName = null,
            QuoteStatus? status = null,
            DateTime? from = null,
            DateTime? to = null)
        {
            try
            {
                var quotes = await _quoteRepository.GetQuotesAsync(
                    quoteNumber,
                    clientId,
                    clientLastName,
                    clientCompanyName,
                    status,
                    from,
                    to);

                return quotes.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while attempting to get the quotes list.");
                throw new QuoteServiceException(Ressources.ValidationMessage.GettingQuoteListErrorMessage);
            }
        }

        public async Task<Quote> GetQuoteByIdAsync(Guid id)
        {
            try
            {
                var quote = await _quoteRepository.GetQuoteByIdAsync(id);
                return quote;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while attempting to get the quote.");
                throw new QuoteServiceException(Ressources.ValidationMessage.GettingQuoteErrorMessage);
            }
        }

        public async Task<Quote> CreateQuoteAsync(QuoteUpdateDTO quoteDTO)
        {
            try
            {
                var client = await _clientRepository.GetClientByIdAsync(quoteDTO.ClientId);
                if (client == null) 
                {
                    throw new ClientNotFoundException(Ressources.ValidationMessage.GettingClientErrorMessage);
                }

                var (subtotal, total) = ValidateAndCalculateAmounts(quoteDTO.QuoteLines, quoteDTO.Subtotal, quoteDTO.TaxeRate);
                if (quoteDTO.QuoteLines.Any(l => string.IsNullOrWhiteSpace(l.Description)))
                {
                    throw new QuoteServiceException(Ressources.ValidationMessage.AllQuoteLineMustHaveDescriptionErrorMessage);
                }

                var quoteNumber = await _settingsService.GenerateQuoteNumberAsync();

                var quote = new Quote
                {
                    Id = Guid.NewGuid(),
                    QuoteNumber = quoteNumber,
                    CreationDate = quoteDTO.CreationDate,
                    ExpirationDate = quoteDTO.ExpirationDate,
                    Subtotal = subtotal,
                    Total = total,
                    TaxeRate = quoteDTO.TaxeRate,
                    ClientId = quoteDTO.ClientId,
                    Client = client,
                    QuoteLines = quoteDTO.QuoteLines.Select(l => new QuoteLine
                    {
                        Id = Guid.NewGuid(),
                        Description = l.Description,
                        Quantity = l.Quantity,
                        UnitPrice = l.UnitPrice
                    }).ToList()
                };

                await _quoteRepository.CreateQuoteAsync(quote);

                return quote;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while creating the quote.");
                throw new QuoteServiceException(Ressources.ValidationMessage.CreatingQuoteErrorMessage);
            }
        }

        private (decimal Subtotal, decimal Total) ValidateAndCalculateAmounts(List<QuoteLine> quoteLines, decimal? providedSubtotal, decimal taxRate)
        {
            decimal subtotal;
            if (quoteLines != null && quoteLines.Any())
            {
                if (quoteLines.All(l => l.UnitPrice.HasValue && l.Quantity.HasValue))
                {
                    // Case 1 : All QuoteLines are detailed with price and unit.
                    // Subtotal ant total are calculated by the service.
                    subtotal = quoteLines.Sum(l => l.UnitPrice.Value * l.Quantity.Value);
                }
                else if (quoteLines.All(l => !l.UnitPrice.HasValue && !l.Quantity.HasValue))
                {
                    // Case 2 : All prices and quantity on QuoteLines are null.
                    // User must indicate global subtotal.
                    if (!providedSubtotal.HasValue)
                    {
                            throw new QuoteServiceException(Ressources.ValidationMessage.QuoteLinesMustBeFulledOrEmpty);
                    }
                    subtotal = providedSubtotal.Value;
                }
                else
                {
                    throw new QuoteServiceException(Ressources.ValidationMessage.QuoteLinesMustBeFulledOrEmpty);
                }
            }
            else
            {
                throw new QuoteServiceException(Ressources.ValidationMessage.QuoteMusteHaveLinesToBeCreated);
            }
            var total = subtotal + (subtotal * taxRate / 100);
            return (subtotal, total);
        }


        public async Task<Quote> UpdateDraftedQuoteAsync(QuoteUpdateDTO quoteDTO)
        {
            try
            {
                var existingQuote = await _quoteRepository.GetQuoteByIdAsync(quoteDTO.Id);
                if (existingQuote == null)
                {
                    throw new QuoteNotFoundException(Ressources.ValidationMessage.GettingQuoteErrorMessage);
                }

                if (existingQuote.Status == QuoteStatus.Draft)
                {
                    existingQuote.ClientId = quoteDTO.ClientId;
                    existingQuote.ExpirationDate = quoteDTO.ExpirationDate;
                    existingQuote.Subtotal = quoteDTO.Subtotal;
                    existingQuote.Total = quoteDTO.Total;
                    existingQuote.TaxeRate = quoteDTO.TaxeRate;

                    var updatedLines = quoteDTO.QuoteLines ?? new List<QuoteLine>();

                    foreach( var updatedLine in updatedLines)
                    {
                        var existingLine = existingQuote.QuoteLines.FirstOrDefault(l => l.Id == updatedLine.Id);
                        if ( existingLine != null )
                        {
                            existingLine.Description = updatedLine.Description;
                            existingLine.Quantity = updatedLine.Quantity;
                            existingLine.UnitPrice = updatedLine.UnitPrice;
                        }
                        else
                        {
                            updatedLine.Id = Guid.NewGuid();
                            existingQuote.QuoteLines.Add(existingLine);
                        }
                    }
                    var updatedQuote = await _quoteRepository.UpdateQuoteAsync(existingQuote);
                    return updatedQuote;
                }
                else
                {
                    throw new QuoteNotFoundException(Ressources.ValidationMessage.QuoteNotInDraftStatusErrorMessage);
                }
            }
            catch (Exception dbEx)
            {
                _logger.LogError(dbEx, "An error occured while updating quote");
                throw new ClientServiceException(Ressources.ValidationMessage.ClientUpdateErrorMessage, dbEx);
            }
        }

        public async Task<Quote> CorrectSentQuoteAsync(Guid originalQuoteId, QuoteUpdateDTO correctionDTO)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var originalQuote = await _quoteRepository.GetQuoteByIdAsync(originalQuoteId);

                if (originalQuote == null)
                {
                    throw new QuoteNotFoundException(Ressources.ValidationMessage.GettingQuoteErrorMessage);
                }

                if (originalQuote.Status == QuoteStatus.Sent)
                {
                    var correctionQuote = await CreateQuoteAsync(correctionDTO);

                    correctionQuote.OriginalQuoteId = originalQuoteId;

                    originalQuote.CorrectionQuoteId = correctionQuote.Id;
                    
                    originalQuote.Status = QuoteStatus.Archived;
                    await _quoteRepository.UpdateQuoteAsync(originalQuote);

                    await transaction.CommitAsync();
                    return correctionQuote;
                }
                else
                {
                    throw new QuoteNotFoundException(Ressources.ValidationMessage.QuoteNotSentStatusErrorMessage);
                }
            }
            catch (Exception dbEx)
            {
                _logger.LogError(dbEx, "An error occured while creating the quote.");
                await transaction.RollbackAsync();
                throw new QuoteServiceException(Ressources.ValidationMessage.CreatingQuoteErrorMessage);
            }
        }
    }
}
