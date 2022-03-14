using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Presentation.Services;

namespace Ebay.Presentation.Business_Logic
{
    public class DiscountBusinessLogic
    {
        private readonly IRepository<Discount> _discountRepository;
        private readonly DiscountService _discountService;
        public DiscountBusinessLogic(IRepository<Discount> discountRepository)
        {
            _discountRepository = discountRepository;
            _discountService = new DiscountService(_discountRepository);
        }

        public async Task<IEnumerable<DiscountViewModel>> GetDiscountsDTO()
        {
            var categories = await _discountRepository.GetAll();
            var categoryViews = categories.Select(item => CreateDiscountView(item));
            return categoryViews;
        }

        public DiscountViewModel CreateDiscountView(Discount discount)
        {
            var discountViewModel = new DiscountViewModel
            {
                Id = discount.Id,
                Name = discount.Name,
                DiscountPercent = discount.DiscountPercent,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
                IsActive = discount.IsActive,
            };
            return discountViewModel;
        }

        public async Task<DiscountViewModel> GetDiscountDTO()
        {
            DiscountViewModel discountViewModel = new DiscountViewModel();
            discountViewModel.Id = await _discountService.GetNumberOfRecords() + 1;
            return discountViewModel;
        }

        public async Task CreateNewDiscount(DiscountViewModel discountViewModel)
        {
            Discount discount = _discountService.FromDtoToDiscount(discountViewModel);
            await _discountRepository.Insert(discount);
        }

        public async Task<DiscountViewModel> EditDiscount(int itemId)
        {
            var discount = await _discountRepository.Get(itemId);
            return _discountService.FromDiscountToDto(discount);
        }

        public async Task UpdateDiscount(DiscountViewModel discountViewModel)
        {
            var product = _discountService.FromDtoToDiscount(discountViewModel);
            product.Id = discountViewModel.Id;
            await _discountRepository.Update(product);
        }

        public async Task DeleteDiscount(int itemId)
        {
            var discount = await _discountRepository.Get(itemId);
            await _discountRepository.Delete(discount);
        }
    }
}
