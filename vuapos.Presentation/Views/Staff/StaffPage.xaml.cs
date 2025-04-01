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
    public sealed partial class StaffPage : UserControl
    {
        
        public StaffViewModel ViewModel { get; set; }
        public StaffPage()
        {
            this.InitializeComponent();
            ViewModel = new StaffViewModel();

            this.DataContext = ViewModel;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Models.Staff staff)
            {
                EditStaffDialog editDialog = new EditStaffDialog(staff, ViewModel);
                editDialog.Activate();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is Models.Staff staff)
            {
                ViewModel.DeleteStaffCommand.Execute(staff);
            }
        }


    }
}
