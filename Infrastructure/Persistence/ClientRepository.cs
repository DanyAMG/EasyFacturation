using EasyFacturation.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Exceptions;
using EasyFacturation.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace EasyFacturation.Infrastructure.Persistence
{
    public class ClientRepository :IClientRepository
    {
        private readonly AppDbContext _context;

        // Constructor to initialize the SQL Local DB
        public ClientRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Client> GetClientByIdAsync(Guid id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task<IEnumerable<Client>> GetClientsByLastNameAsync(string lastName)
        {
            var clientsFound = _context.Clients
                .Where(c => EF.Functions.Like(c.LastName, $"%{lastName}%"))
                .ToListAsync();
            return await clientsFound;
        }

        public async Task<IEnumerable<Client>> GetClientsByCompanyNameAsync(string companyName)
        {
            var clientsFound = _context.Clients
                .Where(c => EF.Functions.Like(c.CompanyName, $"%{companyName}"))
                .ToListAsync();
            return await clientsFound;
        }

        public async Task<IEnumerable<Client>> GetAllClientsAsync()
        {
            return await _context.Clients.ToListAsync();
        }

        public async Task<Client> CreateClientAsync(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> UpdateClientAsync(Client client)
        {
            var existingClient = await _context.Clients.FindAsync(client.Id);
            if (existingClient == null)
            {
                throw new ClientNotFoundException($"Client with ID {client.Id} not found.");
            }
            _context.Entry(client).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> DeleteClientAsync(Guid id)
        {
            var existingClient = await _context.Clients.FindAsync(id);

            if (existingClient == null)
            {
                throw new ClientNotFoundException($"Client with ID {id} not found.");
            }
            _context.Clients.Remove(existingClient);
            await _context.SaveChangesAsync();
            return existingClient;
        }
    }
}
