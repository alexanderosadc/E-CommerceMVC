using Ebay.Domain.Entities;
using Ebay.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Ebay.Presentation.Business_Logic
{
    public class ValidationBusinessLogic : IValidationBL
    {
        private readonly UserManager<User> _userManager;
        public ValidationBusinessLogic(UserManager<User> userManager)
        {
            _userManager = userManager;
        }
        public async Task<bool> IsUserEmailAlreadyExist(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return (user != null) ? true : false;
        }

        public async Task<bool> IsUsernameAlreadyExists(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            return (user != null) ? true : false;
        }
        public async Task<IDictionary<string, string>> ValidateUser(string username, string email)
        {
            IDictionary<string, string> modelErrors = new Dictionary<string, string>();
            bool isUserNameExist = await IsUsernameAlreadyExists(username);
            bool isEmailExist = await IsUserEmailAlreadyExist(email);
            if (isUserNameExist == true)
            {
                modelErrors.Add("UserName", "User with same username already exists");
            }

            if(isEmailExist == true)
            {
                modelErrors.Add("Email", "User with same email already exists");
            }
            return modelErrors;
        }
    }
}
