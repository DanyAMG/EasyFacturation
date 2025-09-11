using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyFacturation.Domain.Models;
using System.Windows.Input;
using EasyFacturation.Presentation.ViewModels.Items;
using EasyFacturation.Presentation.Views;
using EasyFacturation.AppServices.Services;

namespace EasyFacturation.Presentation.ViewModels
{
    public partial class ClientViewModel : ObservableObject
    {
        [ObservableProperty]
        private Client selectedClient;

        private readonly ClientService _clientService;
        private readonly MainViewModel _mainViewModel;

        public Client Client { get; }

        public IRelayCommand CreateQuoteCommand { get; }
        public IRelayCommand CreateInvoiceCommand { get; }
        public IRelayCommand EditClientCommand { get; }
        public Action OnClientUpdated { get; set; }

        public ClientViewModel(Client client, MainViewModel mainViewModel, ClientService clientService)
        {
            Client = client;
            _mainViewModel = mainViewModel;
            _clientService = clientService;
            EditClientCommand = new RelayCommand(OnEditClient);
        }

        public string FullName => $"{Client.FirstName} {Client.LastName}";
        public string CompanyName => Client.CompanyName;
        public string DisplayAddress =>
            $"{(string.IsNullOrWhiteSpace(Client.StreetNumber) ? "" : Client.StreetNumber + " ")}" +
            $"{Client.StreetName}, " +
            $"{(string.IsNullOrWhiteSpace(Client.AdressLine1) ? "" : Client.AdressLine1 + ", ")}" +
            $"{(string.IsNullOrWhiteSpace(Client.AdressLine2) ? "" : Client.AdressLine2 + ", ")}" +
            $"{Client.ZipCode} {Client.City}";
        public string Email => Client.Email;
        public string Phone => Client.Phone;

        private void OnCreateQuote()
        {
            // TODO: Navigate to AddQuoteView with this client prefilled
        }

        private void OnCreateInvoice()
        {
            // TODO: Navigate to AddInvoiceView with this client prefilled
        }

        private void OnEditClient()
        {
            if (Client == null)
            {
                return;
            }

            var addEditVM = new AddEditClientViewModel(_clientService, _mainViewModel ,Client)
            {
                OnClientUpdated = () =>
                {
                    _mainViewModel.CurrentView = new ClientView
                    {
                        DataContext = new ClientViewModel(Client, _mainViewModel, _clientService)
                    };
                }
            };
        
            var addEditView = new AddEditClientView
            {
                DataContext = addEditVM,
            };

            _mainViewModel.CurrentView = addEditView;
        }
    }
}