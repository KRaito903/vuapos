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
using vuapos.Presentation.Views.OrderDetail;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Order
{
    public sealed partial class OrderPage : UserControl
    {
        public OrderViewModel ViewModel { get; set; }
        public OrderPage()
        {
            this.InitializeComponent();

            ViewModel = App.Services!.GetRequiredService<OrderViewModel>();
            this.DataContext = ViewModel;

        }

        private void ViewOrder_Click(object sender, RoutedEventArgs e)
        {  
            if (sender is Button button && button.Tag is Models.Order order)
            {

                ViewModel.SelectedOrder = order;
                OrderDetailPage orderDetailPage = new OrderDetailPage(ViewModel);
                orderDetailPage.Activate();
            }
        }

        private void PrintOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Models.Order order)
            {
               
            }
        }

        private void CancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Models.Order order)
            {
               
            }
        }
        private void CreateOrder_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SelectedOrder = null;
            OrderDetailPage orderDetailPage = new OrderDetailPage(ViewModel);
            orderDetailPage.Activate();
        }

    }
}
