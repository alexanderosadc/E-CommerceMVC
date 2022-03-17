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

        public DiscountViewDTO FromDiscountToDto(Discount discount)
        {
            return new DiscountViewDTO
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
        public List<Discount> GetSelectedDiscounts(ProductCreateDTO viewModel)
        {
            return viewModel.DiscountIds
                .Select(async item => await _discountRepository.Get(item))
                .Select(task => task.Result)
                .Where(discount => discount != null)
                .ToList();
        }
    }
}
