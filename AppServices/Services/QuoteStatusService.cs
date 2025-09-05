using EasyFacturation.Domain.Enums;
using EasyFacturation.Domain.Exceptions;
using EasyFacturation.Domain.Interfaces;
using EasyFacturation.Domain.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.AppServices.Services
{
    public class QuoteStatusService
    {
        private readonly IQuoteStatusRepository _quoteStatusRepository;
        private readonly IQuoteRepository _quoteRepository;
        private readonly ILogger _logger;

        public QuoteStatusService(IQuoteStatusRepository quoteStatusRepository, IQuoteRepository quoteRepository, ILogger logger)
        {
            _quoteStatusRepository = quoteStatusRepository;
            _quoteRepository = quoteRepository;
            _logger = logger;
        }

        public async Task ChangeStatusAsync(Guid quoteId, QuoteStatus status)
        {
            try
            {
                var quote = await _quoteRepository.GetQuoteByIdAsync(quoteId);

                if (quote == null)
                {
                    throw new QuoteServiceException(Ressources.ValidationMessage.GettingQuoteErrorMessage);
                }

                if (quote.Status == QuoteStatus.Archived)
                {
                    throw new QuoteStatusServiceException(Ressources.ValidationMessage.QuoteAlreadyArchivedErrorMessage);
                }

                var statusHistory = new QuoteStatusHistory
                {
                    Id = Guid.NewGuid(),
                    Status = status,
                    ChangedAt = DateTime.UtcNow,
                    QuoteId = quoteId
                };

                quote.Status = status;
                await _quoteStatusRepository.CreateQuoteStatusAsync(statusHistory);
                await _quoteRepository.UpdateQuoteAsync(quote);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while attempting to change the quote's status.");
                throw new QuoteStatusServiceException(Ressources.ValidationMessage.ChangingQuoteStatusErrorMessage);
            }
        }

        public async Task<List<QuoteStatusHistory>> GetStatusHistoryByQuoteId(Guid quoteId)
        {
            try
            {
                var quoteStatusHistory = await _quoteStatusRepository.GetQuoteStatusByQuoteIdAsync(quoteId);
                return quoteStatusHistory.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while attempting to get the quote's status history.");
                throw new QuoteStatusServiceException(Ressources.ValidationMessage.GettingQuoteStatusHistoryErrorMessage);
            }
        }
    }
}
