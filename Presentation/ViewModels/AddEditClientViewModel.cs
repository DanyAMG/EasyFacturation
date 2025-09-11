using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyFacturation.AppServices.DTOs;
using EasyFacturation.AppServices.Services;
using EasyFacturation.Domain.Models;
using EasyFacturation.Presentation.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Presentation.ViewModels
{
    public partial class AddEditClientViewModel : ObservableObject
    {
        private readonly ClientService _clientService;

        private readonly Client _existingClient;
        private readonly MainViewModel _mainViewModel;

        public AddEditClientViewModel(ClientService clientService, MainViewModel mainViewModel, Client? existingClient = null)
        {
            _clientService = clientService;
            _existingClient = existingClient;
            _mainViewModel = mainViewModel;

            if (existingClient != null)
            {
                Title = _existingClient.Title;
                FirstName = _existingClient.FirstName;
                LastName = _existingClient.LastName;
                CompanyName = _existingClient.CompanyName;
                StreetNumber = _existingClient.StreetNumber;
                StreetName = _existingClient.StreetName;
                AdressLine1 = _existingClient.AdressLine1;
                AdressLine2 = _existingClient.AdressLine2;
                City = _existingClient.City;
                ZipCode = _existingClient.ZipCode;
                Phone = _existingClient.Phone;
                Email = _existingClient.Email;
            }
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
        public Action<Client> OnClientCreated { get; set; }
        public Action OnClientUpdated { get; set; }
        public Action OnCancel { get; set; }

        [RelayCommand]
        public async Task SaveAsync()
        {
            try
            {
                if (_existingClient == null)
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

                    var createdClient = await _clientService.AddClientAsync(dto);

                    OnClientCreated?.Invoke(createdClient);
                }
                else
                {
                    _existingClient.Title = Title;
                    _existingClient.FirstName = FirstName;
                    _existingClient.LastName = LastName;
                    _existingClient.CompanyName = CompanyName;
                    _existingClient.StreetNumber = StreetNumber;
                    _existingClient.StreetName = StreetName;
                    _existingClient.AdressLine1 = AdressLine1;
                    _existingClient.AdressLine2 = AdressLine2;
                    _existingClient.City = City;
                    _existingClient.ZipCode = ZipCode;
                    _existingClient.Phone = Phone;
                    _existingClient.Email = Email;

                    OnClientUpdated.Invoke();
                }
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
