using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using vuapos.Presentation.Commands;
using vuapos.Presentation.Models;
using vuapos.Presentation.DAO.Interface;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows.Input;
using vuapos.Presentation.Services;
using System.Threading.Tasks;

namespace vuapos.Presentation.ViewModels
{
    public class OrderViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Order> _orders;
        private readonly OrderService _orderService;
        private Order _selectedOrder;

        public Order? SelectedOrder
        {
            get => _selectedOrder;
            set
            {
                _selectedOrder = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Order> Orders
        {
            get => _orders;
            set {
                _orders = value; 
                OnPropertyChanged(); 
            }
        }

        public ICommand RemoveOrderCommand { get;  }
        public OrderViewModel(OrderService orderService)
        {
            _orderService = orderService;  
            // Giả lập dữ liệu đơn hàng
            _orders = new ObservableCollection<Order>();
            RemoveOrderCommand = new RelayCommand<Order>(RemoveOrder);
            _ = LoadOrders();

        }

        private async Task LoadOrders()
        {
            var responseOrder = await _orderService.GetAllOrdersAsync(1);
            if (responseOrder == null || responseOrder.Data == null) return;
            Orders.Clear();
            foreach (var order in responseOrder.Data)
            {
               Orders.Add(order);
            }
        }

        private void RemoveOrder(Order order)
        {
            _orders.Remove(order);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
