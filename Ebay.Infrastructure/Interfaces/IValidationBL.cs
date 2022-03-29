using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces
{
    public interface IValidationBL
    {
        public Task<bool> IsUsernameAlreadyExists(string username);
        public Task<bool> IsUserEmailAlreadyExist(string email);
        public Task<IDictionary<string, string>> ValidateUser(string username, string email);
    }
}
