using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Models;

namespace EasyFacturation.Domain.Interfaces
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetInvoiceByIdAsync(Guid id);
        Task<Invoice> GetInvoiceByNumberAsync(string number);
        Task<IEnumerable<Invoice>> GetAllInvoicesAsync();
        Task<Invoice> CreateInvoiceAsync(Invoice invoice);
        Task<Invoice> UpdateInvoiceAsync(Invoice invoice);
        Task<Invoice> DeleteInvoiceAsync(Guid id);
    }
}
