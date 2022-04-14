using Ebay.Domain.Entities;
using Ebay.Infrastructure.ViewModels.Admin.CreateProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation.Services
{
    public interface IDiscountService
    {
        public List<Discount> GetSelectedDiscounts(ProductCreateDTO viewModel);
    }
}
