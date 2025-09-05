using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using EasyFacturation.Domain.Models;

namespace EasyFacturation.Infrastructure.Persistence
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Clients { get; set; }
        public DbSet<AppOwner> AppOwner { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteLine> QuoteLines { get; set; }
        public DbSet<QuoteStatusHistory> QuoteStatusHistories { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
        public DbSet<InvoiceStatusHistory> InvoiceStatusHistories { get; set; }
        public DbSet<Settings> Settings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>()
                .HasOne(i => i.Quote)
                .WithOne(q => q.Invoice)
                .HasForeignKey<Invoice>(i => i.QuoteId)
                .IsRequired(false);

            modelBuilder.Entity<Quote>()
                .HasMany(q => q.Corrections)
                .WithOne(q => q.OriginalQuote)
                .HasForeignKey(q => q.OriginalQuoteId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
