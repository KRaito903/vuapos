using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using vuapos.Presentation.Views.Customer;
using vuapos.Presentation.Views.Module1;

namespace vuapos.Presentation
{
    public sealed partial class MainWindow : Window
    {
        private Page1Content page1;
        private Page2Content page2;
        private CustomerPage customerPage;

        public MainWindow()
        {
            this.InitializeComponent();

            // Initialize page instances
            page1 = new Page1Content();
            page2 = new Page2Content();
            customerPage = new CustomerPage();

            // Set default selected item
            MainNavigationView.SelectedItem = MainNavigationView.MenuItems[0];
        }

        private void MainNavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer is NavigationViewItem selectedItem)
            {
                switch (selectedItem.Tag.ToString())
                {
                    case "page1":
                        MainLayout.Title = "Page 1";
                        MainLayout.PageContent = page1;
                        break;

                    case "page2":
                        MainLayout.Title = "Page 2";
                        MainLayout.PageContent = page2;
                        break;

                    case "customers":
                        MainLayout.Title = "Customers";
                        MainLayout.PageContent = customerPage;
                        break;
                }
            }
        }
    }
}
