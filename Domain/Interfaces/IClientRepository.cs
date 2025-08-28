using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Models;

namespace EasyFacturation.Domain.Interfaces
{
    public interface IClientRepository
    {
        Task<Client> GetClientByIdAsync(Guid id);
        Task<IEnumerable<Client>> GetClientsByLastNameAsync(string name);
        Task<IEnumerable<Client>> GetClientsByCompanyNameAsync(string companyName);
        Task<IEnumerable<Client>> GetAllClientsAsync();
        Task<Client> CreateClientAsync(Client client);
        Task<Client> UpdateClientAsync(Client client);
        Task ArchiveClientAsync(Guid id);
    }

}
