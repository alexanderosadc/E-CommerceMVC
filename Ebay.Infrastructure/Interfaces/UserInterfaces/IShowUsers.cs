using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels.Admin.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.UserInterfaces
{
    public interface IShowUsers
    {
        public Task<List<AppUserViewDTO>> GetUsers();
        public Task<AppUserViewDTO> ToUserView(User user);
    }
}
