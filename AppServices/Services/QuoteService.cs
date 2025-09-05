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
using System.Collections;
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
        private readonly QuoteStatusService _statusService;
        private readonly AppDbContext _context;
        private readonly ILogger _logger;

        public QuoteService(IQuoteRepository quoteRepository, IClientRepository clientRepository, ILogger logger, SettingsService settingsService, AppDbContext context, QuoteStatusService statusService)
        {
            _quoteRepository = quoteRepository;
            _clientRepository = clientRepository;
            _settingsService = settingsService;
            _logger = logger;
            _context = context;
            _statusService = statusService;
        }

        public async Task<List<Quote>> GetQuotesAsync(
            string? quoteNumber = null,
            string? title = null,
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
                    title,
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

        public async Task<Quote> CreateQuoteAsync(QuoteCreateDTO quoteDTO)
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
                    Title = quoteDTO.Title,
                    QuoteNumber = quoteNumber,
                    CreationDate = quoteDTO.CreationDate,
                    ExpirationDate = quoteDTO.ExpirationDate,
                    Subtotal = subtotal,
                    Total = total,
                    TaxeRate = quoteDTO.TaxeRate,
                    ClientId = quoteDTO.ClientId,
                    QuoteLines = quoteDTO.QuoteLines.Select(l => new QuoteLine
                    {
                        Id = Guid.NewGuid(),
                        Description = l.Description,
                        Quantity = l.Quantity,
                        UnitPrice = l.UnitPrice
                    }).ToList()
                };

                await _quoteRepository.CreateQuoteAsync(quote);

                await _statusService.ChangeStatusAsync(quote.Id, QuoteStatus.Draft);

                return quote;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while creating the quote.");
                throw new QuoteServiceException(Ressources.ValidationMessage.CreatingQuoteErrorMessage);
            }
        }

        private (decimal Subtotal, decimal Total) ValidateAndCalculateAmounts(List<QuoteLineCreateDTO> quoteLines, decimal? providedSubtotal, decimal taxRate)
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


        public async Task<Quote> UpdateDraftedQuoteAsync(QuoteUpdateDTO quoteDTO, Guid id)
        {
            try
            {
                var existingQuote = await _quoteRepository.GetQuoteByIdAsync(id);
                if (existingQuote == null)
                {
                    throw new QuoteNotFoundException(Ressources.ValidationMessage.GettingQuoteErrorMessage);
                }

                if (existingQuote.Status == QuoteStatus.Draft)
                {
                    existingQuote.ClientId = quoteDTO.ClientId;
                    existingQuote.Title = quoteDTO.Title;
                    existingQuote.ExpirationDate = quoteDTO.ExpirationDate;
                    existingQuote.TaxeRate = quoteDTO.TaxeRate;

                    var (subtotal, total) = ValidateAndCalculateAmounts(quoteDTO.QuoteLines, quoteDTO.Subtotal, quoteDTO.TaxeRate);
                    existingQuote.Subtotal = quoteDTO.Subtotal;
                    existingQuote.Total = quoteDTO.Total;

                    existingQuote.QuoteLines.Clear();
                    foreach ( var lineDTO in quoteDTO.QuoteLines)
                    {
                        existingQuote.QuoteLines.Add(new QuoteLine
                        {
                            Id = Guid.NewGuid(),
                            Description = lineDTO.Description,
                            Quantity = lineDTO.Quantity,
                            UnitPrice = lineDTO.UnitPrice
                        });
                    }
                    var updatedQuote = await _quoteRepository.UpdateQuoteAsync(existingQuote);

                    await _statusService.ChangeStatusAsync(updatedQuote.Id, QuoteStatus.Draft);

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
                    var createDTO = new QuoteCreateDTO
                    {
                        Title = correctionDTO.Title,
                        CreationDate = correctionDTO.CreationDate,
                        ExpirationDate = correctionDTO.ExpirationDate,
                        Subtotal = correctionDTO.Subtotal,
                        Total = correctionDTO.Total,
                        TaxeRate = correctionDTO.TaxeRate,
                        ClientId = correctionDTO.ClientId,
                        QuoteLines = correctionDTO.QuoteLines
                    };

                    var correctionQuote = await CreateQuoteAsync(createDTO);

                    correctionQuote.OriginalQuoteId = originalQuoteId;

                    await _statusService.ChangeStatusAsync(originalQuote.Id, QuoteStatus.Archived);

                    await transaction.CommitAsync();
                    return correctionQuote;
                }
                else
                {
                    throw new QuoteServiceException(Ressources.ValidationMessage.QuoteNotSentStatusErrorMessage);
                }
            }
            catch (Exception dbEx)
            {
                _logger.LogError(dbEx, "An error occured while creating the quote.");
                await transaction.RollbackAsync();
                throw new QuoteServiceException(Ressources.ValidationMessage.CreatingQuoteErrorMessage);
            }
        }

        public async Task SendQuoteAsync(Guid quoteId)
        {
            await _statusService.ChangeStatusAsync(quoteId, QuoteStatus.Sent);

            //Call here the PDF generation method
        }
    }
}
