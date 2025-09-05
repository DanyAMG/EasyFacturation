using CommunityToolkit.Mvvm.ComponentModel;
using EasyFacturation.AppServices.Services;
using EasyFacturation.Domain.Models;
using EasyFacturation.Presentation.ViewModels.Items;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Presentation.ViewModels
{
    public partial class ClientListViewModel : ObservableObject
    {
        private readonly ClientService _clientService;

        [ObservableProperty]
        private ObservableCollection<ClientListItem> clients = new();

        public ClientListViewModel(ClientService clientService)
        {
            _clientService = clientService;
        }

        public async Task InitializeAsync()
        {
            await LoadClientAsync();
        }
        public async Task LoadClientAsync()
        {
            var clientsFromDb = await GetAllClientsForListAsync();
            Clients = new ObservableCollection<ClientListItem>(clientsFromDb);
        }

        public async Task<List<ClientListItem>> GetAllClientsForListAsync()
        {
            var clients = await _clientService.GetAllClientsAsync();

            return clients.Select(c=> new ClientListItem
            {
                Id = c.Id,
                DisplayName = $"{c.FirstName} {c.LastName}",
                Email = c.Email,
                Phone = c.Phone,
                DisplayAdress = $"{(string.IsNullOrWhiteSpace(c.StreetNumber) ? "" : c.StreetNumber + " ")}" +
                                $"{c.StreetName}, " +
                                $"{(string.IsNullOrWhiteSpace(c.AdressLine1) ? "" : c.AdressLine1 + ", ")}" +
                                $"{(string.IsNullOrWhiteSpace(c.AdressLine2) ? "" : c.AdressLine2 + ", ")}" +
                                $"{c.ZipCode} {c.City}"

            }).ToList();
        }
    }
}
