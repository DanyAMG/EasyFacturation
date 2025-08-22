using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Models;

namespace EasyFacturation.Domain.Interfaces
{
    public interface IInvoiceLineRepository
    {
        Task<InvoiceLine> GetInvoiceLineByIdAsync(Guid id);
        Task<IEnumerable<InvoiceLine>> GetAllnvoiceLinesAsync();
        Task<InvoiceLine> CreateInvoiceLineAsync(InvoiceLine line);
        Task<InvoiceLine> UpdateInvoiceLineAsync(InvoiceLine line);
        Task<InvoiceLine> DeleteInvoiceLineAsync(Guid id);
    }
}
