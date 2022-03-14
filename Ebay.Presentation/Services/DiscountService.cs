using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
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
        /// <summary>
        ///  Method <c>ToDiscountView</c> transforms <c>Discount</c> to <c>DiscountView</c>.
        /// </summary>
        /// <param name="discount">
        ///     Discount EF entity.
        /// </param>
        /// <returns>
        ///     <c>DiscountView</c> entity.
        /// </returns>
        public DiscountViewModel ToDiscountView(Discount discount)
        {
            return new DiscountViewModel
            {
                Id = discount.Id,
                Name = discount.Name,
                DiscountPercent = discount.DiscountPercent,
                IsActive = discount.IsActive,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
            };
        }
        /// <summary>
        ///  Method <c>CreateDropdownDiscounts</c> creates dropodown list of all discounts for the UI.
        /// </summary>
        /// <returns>
        ///     <c>List<SelectedListItem></c> which is used on visualization in UI.
        /// </returns>
        public async Task<List<SelectListItem>> CreateDropdownDiscounts()
        {
            var productCategories = await _discountRepository.GetAll();

            return productCategories.Select(item => new SelectListItem
            {
                Text = item.Name + " = " + item.DiscountPercent.ToString() + "%",
                Value = item.Id.ToString()
            }).ToList(); ;
        }

        public Discount FromDtoToDiscount(DiscountViewModel discountViewModel)
        {
            return new Discount
            {
                Name = discountViewModel.Name,
                DiscountPercent = discountViewModel.DiscountPercent,
                IsActive = discountViewModel.IsActive,
                StartDate = discountViewModel.StartDate,
                EndDate = discountViewModel.EndDate
            };
        }

        public DiscountViewModel FromDiscountToDto(Discount discount)
        {
            return new DiscountViewModel
            {
                Id = discount.Id,
                Name= discount.Name,
                DiscountPercent = discount.DiscountPercent,
                StartDate = discount.StartDate,
                EndDate= discount.EndDate,
                IsActive= discount.IsActive,
            };
        }

        public async Task<int> GetNumberOfRecords()
        {
            var discounts = await _discountRepository.GetAll();
            return discounts.AsQueryable().Count();
        }

        /// <summary>
        ///  Method <c>GetSelectedDiscounts</c> gets <c>ProductCreateViewModel</c> and finds 
        ///  all <c>Discounts</c> related to this product.
        /// </summary>
        /// <param name="viewModel">
        ///     <c>ProductCreateViewModel</c> which represents the product entity.
        /// </param>
        /// <returns>
        ///     <c>List<DiscountView></c> selects all Discounts related to the product.
        /// </returns>
        public List<Discount> GetSelectedDiscounts(ProductCreateViewModel viewModel)
        {
            return viewModel.DiscountIds
                .Select(async item => await _discountRepository.Get(item))
                .Select(task => task.Result)
                .Where(discount => discount != null)
                .ToList();
        }
    }
}
