using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using Ebay.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ebay.Presentation.Services
{
    public class CategoryService
    {
        private readonly IRepository<Category> _categoryRepository;

        public CategoryService(IRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<ICollection<Category>> ExtractCategories(List<string> listOfId, IRepository<Category> context)
        {
            ICollection<Category> categories = new List<Category>();
            if (listOfId != null)
            {
                foreach (var id in listOfId)
                {
                    var matchedCategory = await context.Get(int.Parse(id));
                    if (matchedCategory != null)
                    {
                        categories.Add(matchedCategory);
                    }
                }
            }
            return categories;
        }

        public async Task<List<string>> GetCategoryNames()
        {
            IEnumerable<Category> category = await _categoryRepository.GetAll();
            List<string> categoryNames = category.Select(item => item.Name).ToList();
            return categoryNames;
        }

        public async Task<List<SelectListItem>> CreateDropdownCategory()
        {

            var productCategories = await _categoryRepository.GetAll();
            var categorySelectedItems = productCategories.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            });

            return categorySelectedItems.ToList();
        }
    }
}
