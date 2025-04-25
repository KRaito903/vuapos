using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using vuapos.Presentation.DAO.Interface;
using vuapos.Presentation.DAO.MockData;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.ViewModels;

namespace vuapos.Presentation
{
    public partial class App : Application
    {
        public static IServiceProvider? Services { get; private set; }
        public static Window? m_window { get; private set; }

        public App()
        {
            this.InitializeComponent();
            ConfigureServices();
        }

        private static void ConfigureServices()
        {
            var services = new ServiceCollection();

            //httpclient
            services.AddHttpClient<ApiService>();
            services.AddHttpClient<CustomerService>();
            services.AddHttpClient<CategoryService>();
            services.AddHttpClient<StaffService>();
            services.AddHttpClient<ProductService>();
            services.AddHttpClient<OrderService>();
            services.AddSingleton<CloudinaryService>();


            //services
            services.AddSingleton<IDialogService, DialogService>();
            services.AddSingleton<ICashRegisterService, CashRegisterService>();
            services.AddSingleton<IUserSession, UserSession>();


            // viewmodel
            services.AddTransient<ProductSearchViewModel>();
            services.AddTransient<LoginViewModel>();
            services.AddTransient<StaffViewModel>();
            services.AddTransient<CashRegisterViewModel>();


            services.AddTransient<Func<OrderViewModel, OrderDetailViewModel>>(provider => (orderViewModel) => {
                var orderService = provider.GetRequiredService<OrderService>();
                var productService = provider.GetRequiredService<ProductService>();
                var dialogService = provider.GetRequiredService<IDialogService>();
                // Create and return the OrderDetailViewModel instance
                return new OrderDetailViewModel(orderService, productService, dialogService, orderViewModel);
            });


            Services = services.BuildServiceProvider();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            
            m_window = new MainWindow();
            m_window.Activate();
        }
    }
}
