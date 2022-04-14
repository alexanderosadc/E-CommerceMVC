using Ebay.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ebay.Infrastructure.Helpers
{
    public static class DropdownHelper
    {
        /// <summary>
        ///  Method <c>CreateDropdownCategory</c> creates List of SelectedListItems for DropdownMenu
        ///  dropdown menu in UI.
        /// </summary>
        /// <returns>
        ///     <c>List of SelectedListItems.</c> entity.
        /// </returns>
        public static async Task<List<SelectListItem>> CreateDropdownCategory(IEnumerable<Category> productCategories)
        {

            return productCategories.Select(item => new SelectListItem
            {
                Text = item.Name,
                Value = item.Id.ToString()
            }).ToList(); ;
        }

        /// <summary>
        ///  Method <c>CreateDropdownDiscounts</c> creates dropodown list of all discounts for the UI.
        /// </summary>
        /// <returns>
        ///     <c>List<SelectedListItem></c> which is used on visualization in UI.
        /// </returns>
        public static async Task<List<SelectListItem>> CreateDropdownDiscounts(IEnumerable<Discount> productDiscounts)
        {
            return productDiscounts.Select(item => new SelectListItem
            {
                Text = item.Name + " = " + item.DiscountPercent.ToString() + "%",
                Value = item.Id.ToString()
            }).ToList();
        }
    }
}
