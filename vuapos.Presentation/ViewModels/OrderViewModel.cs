using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using vuapos.Presentation.Commands;
using vuapos.Presentation.Models;
using vuapos.Presentation.DAO.Interface;

namespace vuapos.Presentation.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Order> _orders;
        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set { _orders = value; OnPropertyChanged(); }
        }

        public RelayCommand AddOrderCommand { get; }
        public RelayCommand<Order> EditOrderCommand { get; }
        public RelayCommand<Order> DeleteOrderCommand { get; }

        public OrderViewModel()
        {
            Orders = new ObservableCollection<Order>
            {
                new Order { Order_Id = "ORD001", CustomerName = "Nguyễn Văn A", CustomerMail = "Example@gmail.com", CustomerPhone = "123", Staff_Id = "123", OrderDate = "2025-04-17", TotalAmount = 250000, OrderStatus = "Đang xử lí" },
                new Order { Order_Id = "ORD002", CustomerName = "Trần Thị B",CustomerMail = "Example@gmail.com", CustomerPhone = "123", Staff_Id = "124", OrderDate = "2025-04-16", TotalAmount = 340000, OrderStatus = "Thành công" }
            };


            AddOrderCommand = new RelayCommand(_ => AddOrder());
            EditOrderCommand = new RelayCommand<Order>(order => EditOrder(order));
            DeleteOrderCommand = new RelayCommand<Order>(order => DeleteOrder(order));
        }

        private void AddOrder()
        {
            Orders.Add(new Order
            {

            });
        }

        private void EditOrder(Order order)
        {
            if (order != null)
            {
                order.CustomerName += " (Đã sửa)";
                OnPropertyChanged(nameof(Orders));
            }
        }

        private void DeleteOrder(Order order)
        {
            if (order != null && Orders.Contains(order))
            {
                Orders.Remove(order);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
