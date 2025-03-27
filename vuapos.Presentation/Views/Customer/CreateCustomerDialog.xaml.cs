using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.Views.Customer
{
    public sealed partial class CreateCustomerDialog : Window
    {
        private readonly CustomerViewModel _viewModel;

        public CreateCustomerDialog(CustomerViewModel viewModel)
        {
            this.InitializeComponent();
            _viewModel = viewModel;
        }

        private async void OnCreateClicked(object sender, RoutedEventArgs e)
        {
            await _viewModel.AddCustomerAsync(NameInput.Text, PhoneInput.Text, EmailInput.Text);
            this.Close();
        }
    }
}
