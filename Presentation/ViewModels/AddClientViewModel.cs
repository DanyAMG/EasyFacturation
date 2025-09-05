using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyFacturation.AppServices.DTOs;
using EasyFacturation.AppServices.Services;
using EasyFacturation.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Presentation.ViewModels
{
    public partial class AddClientViewModel : ObservableObject
    {
        private readonly ClientService _clientService;

        public AddClientViewModel(ClientService clientService)
        {
            _clientService = clientService;
        }

        [ObservableProperty] private string firstName;
        [ObservableProperty] private string lastName;
        [ObservableProperty] private string companyName;
        [ObservableProperty] private string streetNumber;
        [ObservableProperty] private string streetName;
        [ObservableProperty] private string adressLine1;
        [ObservableProperty] private string adressLine2;
        [ObservableProperty] private string city;
        [ObservableProperty] private string zipCode;
        [ObservableProperty] private string phone;
        [ObservableProperty] private string email;

        [ObservableProperty] private Client.ClientTitle title;

        [ObservableProperty] private string errorMessage;

        public IEnumerable<Client.ClientTitle> TitleValues => Enum.GetValues(typeof(Client.ClientTitle)).Cast<Client.ClientTitle>();
        public Action OnClientCreated { get; set; }
        public Action OnCancel { get; set; }

        [RelayCommand]
        public async Task SaveAsync()
        {
            try
            {
                var dto = new ClientCreateDTO
                {
                    Title = this.title,
                    FirstName = this.firstName,
                    LastName = this.lastName,
                    CompanyName = this.companyName,
                    StreetNumber = this.streetNumber,
                    StreetName = this.streetName,
                    AdressLine1 = this.adressLine1,
                    AdressLine2 = this.adressLine2,
                    City = this.city,
                    ZipCode = this.zipCode,
                    Phone = this.phone,
                    Email = this.email
                };

                await _clientService.AddClientAsync(dto);

                OnClientCreated?.Invoke();
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

        [RelayCommand]
        public void Cancel()
        {
            OnCancel?.Invoke();
        }
    }
}
