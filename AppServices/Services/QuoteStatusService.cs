using EasyFacturation.Domain.Enums;
using EasyFacturation.Domain.Exceptions;
using EasyFacturation.Domain.Interfaces;
using EasyFacturation.Domain.Models;
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

        public QuoteStatusService(IQuoteStatusRepository quoteStatusRepository, IQuoteRepository quoteRepository)
        {
            _quoteStatusRepository = quoteStatusRepository;
            _quoteRepository = quoteRepository;
        }

        public async Task ChangeStatusAsync(Guid quoteId, QuoteStatus status)
        {
            var quote = await _quoteRepository.GetQuoteByIdAsync(quoteId);

            if (quote == null)
            {
                throw new QuoteServiceException(Ressources.ValidationMessage.GettingQuoteErrorMessage);
            }

            if(quote.Status == QuoteStatus.Archived)
            {
                throw new QuoteStatusServiceException(Ressources.ValidationMessage.QuoteAlreadyArchivedErrorMessage);
            }

            quote.StatusHistory ??= new List<QuoteStatusHistory>();
            quote.StatusHistory.Add(new QuoteStatusHistory
            {
                Id = Guid.NewGuid(),
                Status = status,
                ChangedAt = DateTime.UtcNow,
                QuoteId = quoteId
            });

            quote.Status = status;

            await _quoteRepository.UpdateQuoteAsync(quote);
        }
    }
}
