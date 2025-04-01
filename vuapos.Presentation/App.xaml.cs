using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using System;
using vuapos.Presentation.Services;

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


            services.AddHttpClient<ApiService>();
            services.AddHttpClient<CustomerService>();
            services.AddHttpClient<CategoryService>();
            services.AddHttpClient<ProductService>();
            services.AddSingleton<CloudinaryService>();
            Services = services.BuildServiceProvider();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }
    }
}
