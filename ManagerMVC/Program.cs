using BusinessLogic.Interface;
using BusinessLogic.Concrete;
using Dal.Interface;
using Dal.Concrete;
using DTO;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Отримуємо рядок підключення з конфігурації
var connectionString = builder.Configuration.GetConnectionString("ManagerWH");

// Реєстрація IUsersDal та IProductsDal з їх конкретними реалізаціями
builder.Services.AddScoped<IUsersDal>(provider => new UsersDal(connectionString));
builder.Services.AddScoped<IProductsDal>(provider => new ProductsDal(connectionString));
builder.Services.AddScoped<IUsersManager, UsersManager>();
builder.Services.AddScoped<IProductsManager, ProductsManager>();

// Додавання контролерів з в'ю
builder.Services.AddControllersWithViews();

// Налаштування аутентифікації
builder.Services.AddAuthentication("Cookies") // Використовуємо стандартну схему "Cookies"
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Product/AccessDenied";
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
