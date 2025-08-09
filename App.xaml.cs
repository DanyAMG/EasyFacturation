using EasyFacturation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
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
        }
    }

}
