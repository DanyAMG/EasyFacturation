using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Models;

namespace EasyFacturation.Domain.Interfaces
{
    public interface IInvoiceStatusRepository
    {
        Task<InvoiceStatusHistory> GetInvoiceStatusByIdAsync(Guid id);
        Task<IEnumerable<InvoiceStatusHistory>> GetAllInvoiceStatusAsync();
        Task<InvoiceStatusHistory> CreateInvoiceStatusAsync(InvoiceStatusHistory invoiceStatus);
        Task<InvoiceStatusHistory> UpdateInvoiceStatusAsync(InvoiceStatusHistory invoiceStatus);
        Task<InvoiceStatusHistory> DeleteInvoiceStatusAsync(Guid id);
    }
}
