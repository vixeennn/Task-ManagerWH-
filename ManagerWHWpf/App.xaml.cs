using System;
using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ManagerWHWpf.ViewModels;
using ManagerWHWpf.Views;
using BusinessLogic.Interface;
using BusinessLogic.Concrete;
using Dal.Interface;
using Dal.Concrete;

namespace ManagerWHWpf
{
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;

        public App()
        {
            // Налаштування DI-контейнера
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            // Створення ServiceProvider для керування залежностями
            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services) // Змінено на IServiceCollection
        {
            // Налаштування конфігурації для зчитування з manager.json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("manager.json", optional: false, reloadOnChange: true)
                .Build();

            // Реєстрація Microsoft.Extensions.Configuration.IConfiguration для конфігурації
            services.AddSingleton<Microsoft.Extensions.Configuration.IConfiguration>(configuration);

            // Реєстрація DAL для кожної сутності з використанням рядка підключення
            services.AddTransient<IUsersDal>(provider =>
                new UsersDal(configuration.GetConnectionString("ManagerWH"))); // Параметр конструктора

            services.AddTransient<IOrdersDal, OrdersDal>();
            services.AddTransient<IProductsDal, ProductsDal>();
            services.AddTransient<ISuppliersDal, SuppliersDal>();

            // Реєстрація бізнес-логіки (BLL) для кожної сутності
            services.AddTransient<IUsersManager, UsersManager>();
            services.AddTransient<IOrdersManager, OrdersManager>();
            services.AddTransient<IProductsManager, ProductsManager>();
            services.AddTransient<ISuppliersManager, SuppliersManager>();

            // Реєстрація ViewModels
            services.AddTransient<OrdersViewModel>();
            services.AddTransient<ProductsViewModel>();
            services.AddTransient<SuppliersViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<RegisterViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<BaseViewModel>();

            // Реєстрація Views
            services.AddTransient<OrdersView>();
            services.AddTransient<ProductsView>();
            services.AddTransient<SuppliersView>();
            services.AddTransient<DashboardView>();
            services.AddTransient<LoginView>();
            services.AddTransient<RegisterView>();

            // Реєстрація головного вікна
            services.AddSingleton<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Отримуємо екземпляр головного вікна з DI-контейнера та відображаємо його
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
            base.OnExit(e);
        }
    }
}
