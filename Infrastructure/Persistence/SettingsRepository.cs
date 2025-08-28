using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Interfaces;
using EasyFacturation.Domain.Models;
using EasyFacturation.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace EasyFacturation.Infrastructure.Persistence
{
    public class SettingsRepository : ISettingsRepository
    {
        private readonly AppDbContext _context;

        public SettingsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Settings> GetSettingsAsync()
        {
            return await _context.Settings.FirstOrDefaultAsync()
                ?? throw new Exception("Settings not found");
        }

        public async Task<Settings> UpdateSettingsAsync(Settings settings)
        {
            var existingSettings = await _context.Settings.FindAsync(settings.Id);

            if (existingSettings == null)
            {
                throw new Exception("Setting not found exception");
            }

            existingSettings.Language = settings.Language;
            existingSettings.DefaultTaxRate = settings.DefaultTaxRate;
            existingSettings.Currency = settings.Currency;
            existingSettings.QuoteNumberPrefix = settings.QuoteNumberPrefix;
            existingSettings.QuoteNumberFormat = settings.QuoteNumberFormat;
            existingSettings.QuoteSequence = settings.QuoteSequence;
            existingSettings.InvoiceNumberPrefix = settings.InvoiceNumberPrefix;
            existingSettings.InvoiceNumberFormat = settings.InvoiceNumberFormat;
            existingSettings.InvoiceSequence = settings.InvoiceSequence;

            await _context.SaveChangesAsync();
            return settings;
        }
    }
}
