using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EasyFacturation.AppServices.Services;
using EasyFacturation.Presentation.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EasyFacturation.Presentation.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private object _currentView;
        private ClientService _clientService;
        private readonly MainViewModel _mainViewModel;

        public IRelayCommand NavigateHomeCommand { get; }
        public IRelayCommand NavigateClientListCommand { get; }
        public IRelayCommand NavigateAddClientCommand { get; }
        public IRelayCommand NavigateEditClientCommand { get; }
        public IRelayCommand NavigateQuoteListCommand { get; }
        public IRelayCommand NavigateAddQuoteCommand { get; }
        public IRelayCommand NavigateEditQuoteCommand { get; }
        public IRelayCommand NavigateInvoiceListCommand { get; }
        public IRelayCommand NavigateAddInvoiceCommand { get; }
        public IRelayCommand NavigateEditInvoiceCommand { get; }
        public IRelayCommand NavigateArchiveListCommand { get; }
        public IRelayCommand NavigateUserSettingsCommand { get; }
        public IRelayCommand NavigateQuoteModelCommand { get; }
        public IRelayCommand NavigateInvoiceModelCommand { get; }

        public MainViewModel(ClientService clientService)
        {
            _clientService = clientService;

            NavigateHomeCommand = new RelayCommand(()  => CurrentView = new HomeView());

            NavigateClientListCommand = new RelayCommand(async () =>
            {
                var clientListVM = new ClientListViewModel(_clientService, this);
                await clientListVM.InitializeAsync();
                CurrentView = new ClientListView { DataContext = clientListVM };
            });

            NavigateAddClientCommand = new RelayCommand(() => CurrentView = new AddEditClientView()
            {
                DataContext = new AddEditClientViewModel(_clientService, _mainViewModel)
            });
            NavigateEditClientCommand = new RelayCommand(() => CurrentView = new AddEditClientView());

            NavigateQuoteListCommand = new RelayCommand(() => CurrentView = new QuoteListView());
            NavigateAddQuoteCommand = new RelayCommand(() => CurrentView = new AddQuoteView());
            NavigateEditQuoteCommand = new RelayCommand(() => CurrentView = new EditQuoteView());

            NavigateInvoiceListCommand = new RelayCommand(() => CurrentView = new InvoiceListView());
            NavigateAddInvoiceCommand = new RelayCommand(() => CurrentView = new AddInvoiceView());
            NavigateEditInvoiceCommand = new RelayCommand(() => CurrentView = new EditInvoiceView());

            NavigateArchiveListCommand = new RelayCommand(() => CurrentView = new ArchiveListView());

            NavigateQuoteModelCommand = new RelayCommand(() => CurrentView = new QuoteModelView());
            NavigateInvoiceModelCommand = new RelayCommand(() => CurrentView = new InvoiceModelView());
            NavigateUserSettingsCommand = new RelayCommand(() => CurrentView = new UserSettingsView());


            NavigateAddClientCommand = new RelayCommand(() =>
            {
                var addClientVM = new AddEditClientViewModel(_clientService, this);
                
                addClientVM.OnClientCreated = (client) =>
                {
                   this.CurrentView = new ClientView
                    {
                        DataContext = new ClientViewModel(client, this, _clientService)
                    };
                };

                addClientVM.OnCancel = () => CurrentView = new HomeView();

                CurrentView = new AddEditClientView { DataContext = addClientVM };
            });

            CurrentView = new HomeView();
        }
    }
}
