using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyFacturation.Domain.Models;

namespace EasyFacturation.Domain.Interfaces
{
    public interface IAppOwner
    {
        Task<AppOwner> GetAppOwnerbyIdAsync(Guid id);
        Task<AppOwner> CreateAppOwnerAsync(AppOwner appOwner);
        Task<AppOwner> UpdateAppOwnerAsyncAssync(AppOwner appOwner);
        Task<AppOwner> DeleteAppOwnerAsync(Guid id);
    }
}
