using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels;
using Ebay.Infrastructure.ViewModels.Admin.CreateCategory;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using Ebay.Infrastructure.ViewModels.Admin.Index;
using Ebay.Infrastructure.ViewModels.Admin.Users;

namespace Ebay.Presentation.Helpers
{
    public static class DTOMapper
    {
        /// <summary>
        ///  Method transfroms ProductCreateDTO to Product.
        /// </summary>
        /// <param name="dto">
        ///     <c>ProductCreateViewModel</c> represents DTO of the product.
        /// </param>
        /// <param name="ignoreId">
        ///     Helps us to detect if we Update the entity(ignoreId = false) or if we create new Product (ignoreId = true)
        /// </param>
        /// <returns>
        ///     New <c>Product</c> entity.
        /// </returns>
        public static Product ToProduct(ProductCreateDTO dto, bool ignoreId)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new Product
            {
                Id = ignoreId == false ? dto.Id : 0,
                Name = dto.Name,
                Description = dto.Description,
                Quantity = dto.TotalQuantity,
                Price = dto.Price,
            };
        }

        /// <summary>
        ///  Method transfroms Product to ProductCreateDTO.
        /// </summary>
        /// <param name="product">
        ///     EF entity.
        /// </param>
        /// <returns>
        ///     New <c>ProductCreateDTO</c> entity.
        /// </returns>
        public static ProductCreateDTO ToProductCreateDTO(Product product)
        {
            if(product == null)
                throw new ArgumentNullException(nameof(product));

            return new ProductCreateDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                TotalQuantity = product.Quantity,
                Price = product.Price,
                Photos = product.Photos.ToList(),
            };
        }

        /// <summary>
        ///  Method transfroms Product to ProductViewDTO.
        /// </summary>
        /// <param name="product">
        ///     EF entity.
        /// </param>
        /// <returns>
        ///     New <c>ProductViewDTO</c> entity.
        /// </returns>
        public static ProductViewDTO ToProductViewDTO(Product product)
        {
            if(product == null)
                throw new ArgumentNullException(nameof(product));

            return new ProductViewDTO
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                TotalQuantity = product.Quantity,
                Price = product.Price,
                CategoryNames = product.ProductCategories.Select(productCat => productCat.Category.Name).ToList(),
                DiscountViews = product.ProductDiscounts
                    .Select(productDisc => ToDiscountViewDTO(productDisc.Discount)),
                Photos = product.Photos
            };
        }

        /// <summary>
        ///  Method transfroms DiscountViewDTO to Discount.
        /// </summary>
        /// <param name="dto">
        ///     DTO of discount.
        /// </param>
        /// <returns>
        ///     New <c>Discount</c> entity.
        /// </returns>
        public static Discount ToDiscount(DiscountViewDTO dto)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new Discount
            {
                Name = dto.Name,
                DiscountPercent = dto.DiscountPercent,
                IsActive = dto.IsActive,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
        }

        /// <summary>
        ///  Method transforms <c>Discount</c> to <c>DiscountView</c>.
        /// </summary>
        /// <param name="discount">
        ///     Discount EF entity.
        /// </param>
        /// <returns>
        ///     <c>DiscountView</c> entity.
        /// </returns>
        public static DiscountViewDTO ToDiscountViewDTO(Discount discount)
        {
            if(discount == null)
                throw new ArgumentNullException(nameof(discount));

            return new DiscountViewDTO
            {
                Id = discount.Id,
                Name = discount.Name,
                DiscountPercent = discount.DiscountPercent,
                IsActive = discount.IsActive,
                StartDate = discount.StartDate,
                EndDate = discount.EndDate,
            };
        }

        public static async Task<CategoryCreateDTO> ToCategoryCreateDTO(Category category, IEnumerable<Category> childCategories)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            List<int> selectedItemsId = new List<int>();
            selectedItemsId = category.Categories.Select(category => category.Id).ToList();

            var categoryCreateView = new CategoryCreateDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                AllChildrenCategories = await DropdownHelper.CreateDropdownCategory(childCategories)
            };

            categoryCreateView.AllChildrenCategories
                .ForEach(selectedElement => selectedElement.Selected =
                selectedItemsId.Contains(int.Parse(selectedElement.Value)));

            return categoryCreateView;
        }

        public static Category ToCategory(CategoryCreateDTO dto, List<Category> childCategories, bool isNew)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto));

            return new Category
            {
                Id = (isNew == true) ? 0 : dto.Id,
                Name = dto.Name,
                Description = dto.Description,
                Categories = childCategories
            };
        }

        /*public static List<Photo> ToPhoto(List<IFormFile> files)
        {
            var photos = files.Select(item => new Photo
            {
                Name = item.FileName,
            });
        }*/
    }
}
