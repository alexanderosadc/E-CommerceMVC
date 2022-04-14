using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Interfaces.AdminPresentation;
using Microsoft.AspNetCore.Identity;

namespace Ebay.Infrastructure.Business_Logic
{
    public class ValidationBusinessLogic : IValidationBL
    {
        private readonly UserManager<User> _userManager;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Discount> _discountRepository;

        public ValidationBusinessLogic(
            UserManager<User> userManager,
            IRepository<Product> productRepository,
            IRepository<Category> categoryRepository,
            IRepository<Discount> discountRepository
            )
        {
            _userManager = userManager;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _discountRepository = discountRepository;
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

        public async Task<IDictionary<string, string>> ValidateProduct(string productName)
        {
            IDictionary<string, string> modelErrors = new Dictionary<string, string>();
            bool isProductNameExist = await IsProductNameExist(productName);

            if(isProductNameExist == true)
            {
                modelErrors.Add("Name", "Product with the same name already exists in the Database");
            }
            return modelErrors;
        }

        public async Task<bool> IsProductNameExist(string productName)
        {
            var products = await _productRepository.GetAll();
            return products.Any(item => item.Name == productName);
        }

        public async Task<IDictionary<string, string>> ValidateCategory(string categoryName)
        {
            IDictionary<string, string> modelErrors = new Dictionary<string, string>();
            bool isCategoryNameExist = await IsCategoryNameExist(categoryName);

            if(isCategoryNameExist == true)
            {
                modelErrors.Add("Name", "Category with the same name already exists in the Database");
            }
            return modelErrors;
        }

        public async Task<bool> IsCategoryNameExist(string categoryName)
        {
            var categories = await _categoryRepository.GetAll();
            return categories.Any(item => item.Name == categoryName);
        }

        public async Task<IDictionary<string, string>> ValidateDiscount(string discountName)
        {
            IDictionary<string, string> modelErrors = new Dictionary<string, string>();
            bool isDiscountNameExist = await IsDiscountNameExist(discountName);

            if (isDiscountNameExist == true)
            {
                modelErrors.Add("Name", "Discount with the same name already exists in the Database");
            }
            return modelErrors;
        }

        public async Task<bool> IsDiscountNameExist(string discountName)
        {
            var discounts = await _discountRepository.GetAll();
            return discounts.Any(item => item.Name == discountName);
        }
    }
}
