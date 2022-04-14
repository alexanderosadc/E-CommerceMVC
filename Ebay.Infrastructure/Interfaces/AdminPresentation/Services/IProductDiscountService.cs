using Ebay.Domain.Entities;
using Ebay.Domain.Entities.JoinTables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Infrastructure.Interfaces.AdminPresentation.Services
{
    public interface IProductDiscountService
    {
        public List<ProductDiscount> CreateProductDiscounts(List<Discount> discounts, Product product);
        public Task DeleteAll(int productId);
    }
}
