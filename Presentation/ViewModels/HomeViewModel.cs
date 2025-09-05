using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using EasyFacturation.AppServices.Services;

namespace EasyFacturation.Presentation.ViewModels
{
    public class HomeViewModel
    {
        private readonly ClientService _clientService;
        public ICommand SearchClientCommand { get; }
        public ICommand AddClient { get; }

        public HomeViewModel(ICommand searchClientCommand, ICommand addClient, ClientService clientService)
        {
            SearchClientCommand = searchClientCommand;
            AddClient = addClient;
            _clientService = clientService;
        }

        private void ExecuteSearchClient(object parameter)
        {

        }

        private void ExecuteAddClient(object parameter)
        {

        }
    }
}
