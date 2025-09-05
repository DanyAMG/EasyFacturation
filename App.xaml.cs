using EasyFacturation.AppServices.Services;
using EasyFacturation.Domain.Interfaces;
using EasyFacturation.Infrastructure.Persistence;
using EasyFacturation.Presentation.ViewModels;
using EasyFacturation.Presentation.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Configuration;
using System.Data;
using System.Windows;

namespace EasyFacturation
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var factory = new AppDbContextFactory();
            using (var context = factory.CreateDbContext(Array.Empty<string>()))
            {
                context.Database.Migrate();
            }

            var dbContext = factory.CreateDbContext(Array.Empty<string>());
            IClientRepository clientRepository = new ClientRepository(dbContext);

            ILogger<ClientService> logger = new LoggerFactory().CreateLogger<ClientService>();

            ClientService clientService = new ClientService(clientRepository, logger);
            var mainVM = new MainViewModel(clientService);

            var mainWindow = new MainWindow { DataContext = mainVM };
            mainWindow.Show();
        }
    }

}
