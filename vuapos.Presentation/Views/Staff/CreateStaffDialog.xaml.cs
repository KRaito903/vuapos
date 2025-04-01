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
using System.Windows.Input;
using vuapos.Presentation.Utils;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Staff
{

    public sealed partial class CreateStaffDialog : Window
    {
        public Models.Staff NewStaff { get; private set; }
        public ICommand AddStaffCommand { get; }

        public CreateStaffDialog(ICommand addStaffCommand)
        {
            this.InitializeComponent();
            AddStaffCommand = addStaffCommand;
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                txtPassword.Tag = passwordBox.Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                txtConfirmPassword.Tag = passwordBox.Password;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string password = txtPassword.Tag as string;
            string confirmPassword = txtConfirmPassword.Tag as string;

            if (password != confirmPassword)
            {
                txtError.Text = "Passwords do not match!";
                txtError.Visibility = Visibility.Visible;
                return;
            }

            if (!PasswordValidator.IsValidPassword(password))
            {
                txtError.Text = "Password does not meet the security requirements!";
                txtError.Visibility = Visibility.Visible;
                return;
            }

            NewStaff = new Models.Staff
            {
                Username = txtUsername.Text,
                Password = password,
                Role = txtRole.Text,
                Phone = txtPhone.Text
            };

            AddStaffCommand?.Execute(NewStaff);
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }

}
