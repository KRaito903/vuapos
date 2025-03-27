using Microsoft.UI.Xaml;

namespace vuapos.Presentation.Views.Customer
{
    public sealed partial class CustomerDetailDialog : Window
    {
        public Customer Customer { get; }

        public CustomerDetailDialog(Customer customer)
        {
            this.InitializeComponent();
            Customer = customer;
        }

        private void OnCloseClicked(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
