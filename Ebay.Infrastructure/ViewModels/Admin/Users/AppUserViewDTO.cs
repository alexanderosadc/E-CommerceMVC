using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.ViewModels.Admin.Users
{
    public class AppUserViewDTO
    {
        public AppUserViewDTO()
        {
            UserRoles = new List<string>();
        }
        public string Id { get; set; }

        public string UserName { get; set; }
        public int UserRolesId { get; set; }
        public IList<string> UserRoles;
        public string Email { get; set; }
    }
}
