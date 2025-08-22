using EasyFacturation.AppServices.DTOs;
using EasyFacturation.Domain.Models;
using EasyFacturation.Domain.Interfaces;
using EasyFacturation.Infrastructure.Persistence;
using EasyFacturation.Domain.Exceptions;
using EasyFacturation.Ressources;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace EasyFacturation.AppServices.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;
        private readonly ILogger<ClientService> _logger;

        public ClientService (IClientRepository clientRepository, ILogger<ClientService> logger)
        {
            _clientRepository = clientRepository;
            _logger = logger;
        }

        public async Task<List<Client>> GetAllClientsAsync()
        {
            try
            {
                var clients = await _clientRepository.GetAllClientsAsync();
                return clients.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while attempting to get the client list.");
                throw new ClientServiceException(Ressources.ValidationMessage.GettingClientListErrorMessage, ex);
            }
        }

        public async Task<List<Client>> GetClientsByLastNameAsync(string lastName)
        {
            try
            {
                var clients = await _clientRepository.GetClientsByLastNameAsync(lastName);
                return clients.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while attempting to get the client list.");
                throw new ClientServiceException(Ressources.ValidationMessage.GettingClientListErrorMessage, ex);
            }
        }

        public async Task<List<Client>> GetClientsByCompanyNameAsync(string companyName)
        {
            try
            {
                var clients = await _clientRepository.GetClientsByCompanyNameAsync(companyName);
                return clients.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while attempting to get the client list.");
                throw new ClientServiceException(Ressources.ValidationMessage.GettingClientListErrorMessage, ex);
            }
        }

        public async Task<Client> GetClientByIdAsync(Guid id)
        {
            try
            {
                var client = await _clientRepository.GetClientByIdAsync(id);
                return client;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while attempting to get the client.");
                throw new ClientServiceException(Ressources.ValidationMessage.GettingClientErrorMessage, ex);
            }
        }

        public async Task AddClientAsync(ClientCreateDTO clientDTO)
        {
            try
            {
                var client = new Client
                {
                    Title = clientDTO.Title,
                    LastName = clientDTO.LastName,
                    FirstName = clientDTO.FirstName,
                    CompanyName = clientDTO.CompanyName,
                    StreetNumber = clientDTO.StreetNumber,
                    StreetName = clientDTO.StreetName,
                    AdressLine1 = clientDTO.AdressLine1,
                    AdressLine2 = clientDTO.AdressLine2,
                    City = clientDTO.City,
                    ZipCode = clientDTO.ZipCode,
                    Phone = clientDTO.Phone,
                    Email = clientDTO.Email,
                };

                var createdClient = await _clientRepository.CreateClientAsync(client);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occured while creating client");
                throw new ClientServiceException(Ressources.ValidationMessage.ClientCreateErrorMessage, dbEx);
            }
        }

        public async Task UpdateClientAsync(ClientUpdateDTO clientDTO)
        {
            try
            {
                var client = await _clientRepository.GetClientByIdAsync(clientDTO.Id);

                if (client == null)
                {
                    throw new ClientServiceException(Ressources.ValidationMessage.GettingClientErrorMessage);
                }

                if (clientDTO.Title.HasValue) client.Title = clientDTO.Title.Value;
                if (clientDTO.LastName != null) client.LastName = clientDTO.LastName;
                if (clientDTO.FirstName != null) client.FirstName = clientDTO.FirstName;
                if (clientDTO.CompanyName != null) client.CompanyName = clientDTO.CompanyName;
                if (clientDTO.StreetNumber != null) client.StreetNumber = clientDTO.StreetNumber;
                if (clientDTO.StreetName != null) client.StreetName = clientDTO.StreetName;
                if (clientDTO.AdressLine1 != null) client.AdressLine1 = clientDTO.AdressLine1;
                if (clientDTO.AdressLine2 != null) client.AdressLine2 = clientDTO.AdressLine2;
                if (clientDTO.City != null) client.City = clientDTO.City;
                if (clientDTO.ZipCode != null) client.ZipCode = clientDTO.ZipCode;
                if (clientDTO.Phone != null) client.Phone = clientDTO.Phone;
                if (clientDTO.Email != null) client.Email = clientDTO.Email;

                var updatedClient = await _clientRepository.UpdateClientAsync(client);
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "An error occured while updating client");
                throw new ClientServiceException(Ressources.ValidationMessage.ClientUpdateErrorMessage, dbEx);
            }
        }

    }
}
