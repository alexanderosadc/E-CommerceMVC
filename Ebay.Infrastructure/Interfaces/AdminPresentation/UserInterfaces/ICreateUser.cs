using Ebay.Infrastructure.ViewModels.Admin.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.UserInterfaces
{
    public interface ICreateUser
    {
        public Task CreateUser(AppUserCreateDTO dto);
        public Task<AppUserCreateDTO> GetUserDTO(string itemId);
    }
}
