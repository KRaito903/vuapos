using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using vuapos.Presentation.DAO.Interface;
using vuapos.Presentation.DAO.MockData;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;
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
      

            //dao
            services.AddSingleton<IProductDao, MockProductDao>();

            // viewmodel
            services.AddTransient<ProductSearchViewModel>();
            services.AddTransient<StaffViewModel>();
            services.AddTransient<OrderDetailViewModel>();

            Services = services.BuildServiceProvider();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }
    }
}
