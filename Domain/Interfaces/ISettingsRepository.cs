using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Models;

namespace EasyFacturation.Domain.Interfaces
{
    public interface ISettingsRepository
    {
        Task<Settings> GetSettingsAsync();
        Task<Settings> UpdateSettingsAsync(Settings settings);
    }
}
