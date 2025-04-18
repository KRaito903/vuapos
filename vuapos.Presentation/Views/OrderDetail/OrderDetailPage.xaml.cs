using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using vuapos.Presentation.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace vuapos.Presentation.Views.OrderDetail
{
 
    public sealed partial class OrderDetailPage : Window
    {
        public ProductSearchViewModel ViewModel { get; }

        public OrderDetailPage()
        {
            this.InitializeComponent();
            ViewModel = App.Services!.GetRequiredService<ProductSearchViewModel>();
        }
    }
}
