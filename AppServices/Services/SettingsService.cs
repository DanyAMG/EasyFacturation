using EasyFacturation.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.AppServices.Services
{
    public class SettingsService
    {
        private readonly ISettingsRepository _settingsRepository;

        public SettingsService(ISettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<string> GenerateQuoteNumberAsync()
        {
            var settings = await _settingsRepository.GetSettingsAsync();

            var number = settings.QuoteNumberFormat
                .Replace("{prefix}", settings.QuoteNumberPrefix)
                .Replace("{year}", DateTime.UtcNow.Year.ToString())
                .Replace("{sequence}", settings.QuoteSequence.ToString("D5"));

            settings.QuoteSequence++;
            await _settingsRepository.UpdateSettingsAsync(settings);
            return number;
        }

        public async Task<string> GenerateInvoiceNumberAsync()
        {
            var settings = await _settingsRepository.GetSettingsAsync();

            var number = settings.InvoiceNumberFormat
                .Replace("{prefix}", settings.InvoiceNumberPrefix)
                .Replace("{year}", DateTime.UtcNow.Year.ToString())
                .Replace("{sequence}", settings.InvoiceSequence.ToString("D5"));

            settings.InvoiceSequence++;
            await _settingsRepository.UpdateSettingsAsync(settings);
            return number;
        }
    }
}
