using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Presentation.Helpers;
using Ebay.Presentation.Services;

namespace Ebay.Presentation.Business_Logic
{
    public class DiscountBusinessLogic : IDiscountBL
    {
        private readonly IRepository<Discount> _discountRepository;
        private readonly DiscountService _discountService;
        public DiscountBusinessLogic(IRepository<Discount> discountRepository)
        {
            _discountRepository = discountRepository;
            _discountService = new DiscountService(_discountRepository);
        }

        public async Task<IEnumerable<DiscountViewDTO>> GetDiscountsDTO()
        {
            var categories = await _discountRepository.GetAll();
            var categoryViews = categories.Select(item => DTOMapper.ToDiscountViewDTO(item));
            return categoryViews;
        }
        public async Task<DiscountViewDTO> GetDiscountDTO()
        {
            DiscountViewDTO discountViewModel = new DiscountViewDTO();
            discountViewModel.Id = await _discountService.GetNumberOfRecords() + 1;
            return discountViewModel;
        }

        public async Task CreateNewDiscount(DiscountViewDTO discountViewModel)
        {
            Discount discount = DTOMapper.ToDiscount(discountViewModel);
            await _discountRepository.Insert(discount);
        }

        public async Task<DiscountViewDTO> EditDiscount(int itemId)
        {
            var discount = await _discountRepository.Get(itemId);
            return _discountService.FromDiscountToDto(discount);
        }

        public async Task UpdateDiscount(DiscountViewDTO discountViewModel)
        {
            var product = DTOMapper.ToDiscount(discountViewModel);
            product.Id = discountViewModel.Id;
            await _discountRepository.Update(product);
        }

        public async Task Delete(int itemId)
        {
            var discount = await _discountRepository.Get(itemId);
            await _discountRepository.Delete(discount);
        }
    }
}
