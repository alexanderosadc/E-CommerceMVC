using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Interfaces;
using Ebay.Infrastructure.Repository;
using Ebay.Infrastructure.Business_Logic;
using Ebay.Infrastructure.Services;
using Ebay.Infrastructure.Interfaces.AdminPresentation;
using Ebay.Infrastructure.Interfaces.AdminPresentation.Services;

namespace Ebay.Presentation
{
    public static class DependencyInjectionAssignments
    {
        public static void AssignServicesToTheProject(WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IProductBL), typeof(ProductBusinessLogic));
            builder.Services.AddScoped(typeof(ICategoryBL), typeof(CategoryBusinessLogic));
            builder.Services.AddScoped(typeof(IDiscountBL), typeof(DiscountBusinessLogic));
            builder.Services.AddScoped(typeof(IUserBL), typeof(UserBusinessLogic));
            builder.Services.AddScoped(typeof(IValidationBL), typeof(ValidationBusinessLogic));
            builder.Services.AddScoped(typeof(ICategoryService), typeof(CategoryService));
            builder.Services.AddScoped(typeof(IDiscountService), typeof(DiscountService));
            builder.Services.AddScoped(typeof(IProductCategoryService), typeof(ProductCategoryService));
            builder.Services.AddScoped(typeof(IProductDiscountService), typeof(ProductDiscountService));
        }
    }
}
