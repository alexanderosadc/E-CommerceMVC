using Ebay.Infrastructure.Interfaces.Shared;
using Ebay.Infrastructure.Interfaces.UserInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation
{
    public interface IUserBL: ICreateUser, IEditUser, IShowUsers, IDeleteEntity
    {
    }
}
