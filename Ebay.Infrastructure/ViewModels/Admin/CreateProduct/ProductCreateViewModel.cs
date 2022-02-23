using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.ViewModels.Admin.CreateProduct
{
    public class ProductCreateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter product name")]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Please enter product description")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Please enter total quantity of product description")]
        public int TotalQuantity { get; set; }

        [Required(ErrorMessage = "Please enter price per unit")]
        public double Price { get; set; }
        public List<string> CategoriesIds { get; set; } = new List<string>();

        public List<SelectListItem> CategoryResponseItems = new List<SelectListItem>();

        public string DiscountId { get; set; }

        public List<SelectListItem> DiscountItems = new List<SelectListItem>();
    }
}
