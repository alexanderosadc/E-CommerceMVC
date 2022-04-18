﻿using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Interfaces;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Infrastructure.Helpers;
using Ebay.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ebay.Infrastructure.Interfaces.AdminPresentation;

namespace Ebay.Infrastructure.Business_Logic
{
    public class DiscountBusinessLogic : IDiscountBL
    {
        private readonly IRepository<Discount> _discountRepository;
        public DiscountBusinessLogic(IRepository<Discount> discountRepository)
        {
            _discountRepository = discountRepository;
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
            var lastItem = await _discountRepository.GetLastItem();
            discountViewModel.Id = lastItem.Id + 1;
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
            return DTOMapper.FromDiscountToDto(discount);
        }

        public async Task UpdateDiscount(DiscountViewDTO discountViewModel)
        {
            var product = DTOMapper.ToDiscount(discountViewModel);
            product.Id = discountViewModel.Id;
            await _discountRepository.Update(product);
        }

        public async Task Delete(string itemId)
        {
            var discount = await _discountRepository.Get(int.Parse(itemId));
            await _discountRepository.Delete(discount);
        }

        public async Task<List<SelectListItem>> GetDropdownDiscounts()
        {
            var discounts = await _discountRepository.GetAll();
            return await DropdownHelper.CreateDropdownDiscounts(discounts);
        }
    }
}