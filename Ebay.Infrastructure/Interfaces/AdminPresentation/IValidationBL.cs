using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation
{
    public interface IValidationBL
    {
        public Task<bool> IsUsernameAlreadyExists(string username);
        public Task<bool> IsUserEmailAlreadyExist(string email);
        public Task<IDictionary<string, string>> ValidateUser(string username, string email);
        public Task<IDictionary<string, string>> ValidateProduct(string name);
        public Task<bool> IsProductNameExist(string productName);
        public Task<IDictionary<string, string>> ValidateCategory(string categoryName);
        public Task<bool> IsCategoryNameExist(string categoryName);
        public Task<IDictionary<string, string>> ValidateDiscount(string discountName);
        public Task<bool> IsDiscountNameExist(string discountName);
    }
}
