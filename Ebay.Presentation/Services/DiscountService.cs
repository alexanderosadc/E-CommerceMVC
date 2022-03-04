using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ebay.Presentation.Services
{
    public class DiscountService
    {
        private readonly IRepository<Discount> _discountRepository;
        public DiscountService(IRepository<Discount> discountRepository)
        {
            _discountRepository = discountRepository;
        }
        public DiscountView ToDiscountView(Discount discount)
        {
            return new DiscountView
            {
                Id = discount.Id,
                Name = discount.Name,
                DiscountPercent = discount.DiscountPercent,
                IsActive = discount.IsActive,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
            };
        }

        public async Task<List<DiscountView>> GetAllDiscounts()
        {
            IEnumerable<Discount> discount = await _discountRepository.GetAll();
            var discountViews = discount.Select(item => new DiscountView
            {
                Id = item.Id,
                Name = item.Name,
                DiscountPercent = item.DiscountPercent,
            });

            return discountViews.ToList();
        }

        public async Task<List<SelectListItem>> CreateDropdownDiscounts()
        {
            var productCategories = await _discountRepository.GetAll();
            var categorySelectedItems = productCategories.Select(item => new SelectListItem
            {
                Text = item.Name + " = " + item.DiscountPercent.ToString() + "%",
                Value = item.Id.ToString()
            });

            return categorySelectedItems.ToList();
        }
    }
}
