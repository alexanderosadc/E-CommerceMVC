using Ebay.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.ViewModels.Admin.CreateCategory
{
    public class CategoryCreateDTO
    {
        public CategoryCreateDTO()
        {
            AllChildrenCategories = new List<SelectListItem>();
            ChildIds = new List<int>();
        }
        public int Id { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Introduct name of the category.")]
        public string Name { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Introduct description of the category.")]
        public string? Description { get; set; }
        public List<int>? ChildIds { get; set;}
        public List<SelectListItem> AllChildrenCategories;
    }
}
