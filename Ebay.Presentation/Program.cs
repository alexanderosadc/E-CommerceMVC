using Ebay.Domain.Entities;
using Ebay.Domain.Interfaces;
using Ebay.Infrastructure.Interfaces;
using Ebay.Infrastructure.Persistance;
using Ebay.Infrastructure.Repository;
using Ebay.Presentation.Business_Logic;
using Ebay.Presentation.Seeders;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//Add builder class
// Add region in builder class

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(config => config
.UseLazyLoadingProxies()
.UseSqlServer(
    builder.Configuration.GetConnectionString("SQLServer")
    ));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>().
    AddDefaultTokenProviders();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped(typeof(IProductBL), typeof(ProductBusinessLogic));
builder.Services.AddScoped(typeof(ICategoryBL), typeof(CategoryBusinessLogic));
builder.Services.AddScoped(typeof(IDiscountBL), typeof(DiscountBusinessLogic));
builder.Services.AddScoped(typeof(IUserBL), typeof(UserBusinessLogic));

builder.Services.ConfigureApplicationCookie(config =>
{
    config.Cookie.Name = "Identity.Cookie";
    config.LoginPath = "/Autthentification/Login";
});

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Index}/{id?}");

// Seed the default data into the database
IdentitySeedData.EnsurePopulatedRoles(app);
IdentitySeedData.EnsurePopulatedUsers(app);
DbSeedData.EnsurePopulated(app);

app.Run();