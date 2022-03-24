using Ebay.Infrastructure.CustomAttributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.ViewModels.Admin.CreateProduct
{
    public class ProductCreateDTO
    {
        public int Id { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter product name")]
        public string? Name { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please enter product description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please enter total quantity of product description")]
        public int TotalQuantity { get; set; }

        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "Please enter price per unit")]
        public double Price { get; set; }
        public List<int> CategoriesIds { get; set; } = new List<int>();

        public List<SelectListItem> CategoryResponseItems = new List<SelectListItem>();

        public List<int> DiscountIds { get; set; } = new List<int>();

        public List<SelectListItem> DiscountItems = new List<SelectListItem>();

        [AllowedExtensions(Extensions = new string[] { ".jpg", ".png", ".jpeg" }, FileListName = "Photos")]
        [MaxFileSize(MaxFileSize = 200 * 200 * 3, FileListName = "Photos")]
        public List<IFormFile> Photos { get; set; }
    }
}
