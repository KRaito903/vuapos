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
using vuapos.Presentation.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace vuapos.Presentation.Views.Staff
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary
        public sealed partial class EditStaffDialog : Window
        {
            public Models.Staff CurrentStaff { get; private set; }
            public StaffViewModel ViewModel { get; set; }

        public EditStaffDialog(Models.Staff staff, StaffViewModel viewModel)
            {  
                this.InitializeComponent();
                ViewModel = viewModel;
                CurrentStaff = staff;
                LoadStaffData();
            }

            private void LoadStaffData()
            {
                txtStaffId.Text = CurrentStaff.StaffId;
                txtUsername.Text = CurrentStaff.Username;
                txtRole.Text = CurrentStaff.Role;
                txtPhone.Text = CurrentStaff.Phone;
            }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Update staff data
            CurrentStaff.Username = txtUsername.Text;
            CurrentStaff.Role = txtRole.Text;
            CurrentStaff.Phone = txtPhone.Text;

            // Handle password change if applicable
            string newPassword = txtNewPassword.Password;
            string confirmPassword = txtConfirmPassword.Password;

            if (!string.IsNullOrEmpty(newPassword))
            {
                if (newPassword == confirmPassword)
                {
                    CurrentStaff.Password = newPassword;  // Update the password
                }
                else
                {
                    ContentDialog errorDialog = new ContentDialog()
                    {
                        Title = "Error",
                        Content = "Passwords do not match!",
                        CloseButtonText = "OK",
                        XamlRoot = this.Content.XamlRoot
                    };
                    return;
                }
             
            }
            ViewModel.EditStaffCommand.Execute(CurrentStaff);
            this.Close(); 
        }


        private void CancelButton_Click(object sender, RoutedEventArgs e)
            {
                this.Close();
            }
        }
}
