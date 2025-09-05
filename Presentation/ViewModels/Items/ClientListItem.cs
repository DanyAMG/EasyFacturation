using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyFacturation.Presentation.ViewModels.Items
{
    public class ClientListItem
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; } = string.Empty;
        public string CompanyName {  get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty ;
        public string Phone { get; set; } = string.Empty;
        public string DisplayAdress {  get; set; } = string.Empty;
    }
}
