using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Collections;
using CommunityToolkit.Mvvm.Input;
using EasyFacturation.AppServices.DTOs;
using EasyFacturation.Domain.Models;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EasyFacturation.Presentation.ViewModels
{
    public partial class AddEditQuoteViewModel : ObservableObject
    {
        private readonly MainViewModel _mainViewModel;

        public AddEditQuoteViewModel(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public ObservableCollection<Client> Clients { get; set; } = new();
        public Client CurrentQuoteClient { get; set; }
        public ICommand AddClientCommand { get; set; }

    }
}
