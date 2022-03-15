using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.ViewModels.Admin
{
    public class CategoryViewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? ParentName { get; set; }
        public List<string>? ChildCategories { get; set; }
    }
}
