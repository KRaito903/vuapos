using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI.Xaml;
using vuapos.Presentation.Commands;
using vuapos.Presentation.DTO.Order;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;
using vuapos.Presentation.Services.Interfaces;
using vuapos.Presentation.Views.Product;

namespace vuapos.Presentation.ViewModels
{
    public class OrderDetailViewModel : INotifyPropertyChanged
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly IDialogService _dialogService;
        private Window _window;

        private readonly OrderViewModel _orderViewModel;
        private Order _currentOrder = new Order();
        private string _searchQuery;
        private ObservableCollection<Product> _searchResults;
        private Product _selectedProduct;
        private int _productQuantity = 1;
        private bool _userCustomerPoints = false;

        public string PromotionCode { get; set; } = string.Empty;
        public decimal SubTotal => OrderDetails.Sum(od => od.Subtotal);

        public decimal TotalDiscount { get; set; } = 0;

        public bool UseCustomerPoints
        {
            get => _userCustomerPoints;
            set
            {
               SetProperty(ref _userCustomerPoints, value);
                if (value)
                {
                    TotalDiscount += CustomerPointsValue;
                    OnPropertyChanged(nameof(TotalDiscount));
                    OnPropertyChanged(nameof(OrderTotal));
                }
                else
                {
                    TotalDiscount -= CustomerPointsValue;
                    OnPropertyChanged(nameof(TotalDiscount));
                    OnPropertyChanged(nameof(OrderTotal));
                }
            }
        }


        public decimal CustomerPointsValue { get; set; } = 0;
       


        public OrderDetailViewModel(OrderService orderService, ProductService productService, IDialogService dialogService , OrderViewModel orderViewModel)
        {
            _orderService = orderService;
            _productService = productService;
            _orderViewModel = orderViewModel;
            _dialogService = dialogService;

            // Initialize a new order
            SearchResults = new ObservableCollection<Product>();
            OrderDetails = new ObservableCollection<OrderDetail>();

            // Commands
            SearchProductCommand = new RelayCommand(async _ => await SearchProductsAsync());
            AddProductCommand = new RelayCommand(_ => AddProductToOrder(), _ => CanAddProduct());
            RemoveOrderDetailCommand = new RelayCommand(parameter => RemoveOrderDetail(parameter as OrderDetail));
            SaveOrderCommand = new RelayCommand(async _ => await SaveOrderAsync());
            ApplyPromotionCodeCommand = new RelayCommand (async _ => ApplyPromotionCode());

            _ = LoadOrderDetail();
        }

        public void SetWindow(Window window)
        {
            _window = window;
        }

        private async Task LoadOrderDetail()
        {
            if (_orderViewModel.SelectedOrder != null)
            {
                _currentOrder = _orderViewModel.SelectedOrder;
                CustomerName = _currentOrder.CustomerName;
                CustomerPhone = _currentOrder.CustomerPhone;
                CustomerMail = _currentOrder.CustomerMail;
                // Load order details
                foreach (var orderDetail in _currentOrder.OrderDetails)
                {
                    OrderDetails.Add(orderDetail);
                }
            }
            else
            {
                // Handle the case when no order is selected
                Debug.WriteLine("No order selected.");
            }
        }

        private async Task GetPointCustomerByPhone(string phone)
        {
            //var customer = await _orderService.GetCustomerByPhoneAsync(phone);
            //if (customer != null)
            //{
            //    CustomerPointsValue = customer.Points;
            //    OnPropertyChanged(nameof(CustomerPointsValue));
            //}

            if (phone == "123")
            {
                CustomerPointsValue = 100;
            }
            else if (phone == "456")
            {
                CustomerPointsValue = 200;
            }
            else
            {
                CustomerPointsValue = 0;
            }
            OnPropertyChanged(nameof(CustomerPointsValue));
        }
        private async void ApplyPromotionCode()
        {

            decimal discount = 0;
            if (PromotionCode == "DISCOUNT10")
            {
                discount = 0.1m; // 10% discount
            }
            else if (PromotionCode == "DISCOUNT20")
            {
                discount = 0.2m; // 20% discount
            }
            else
            {
                // Handle invalid promotion code
                discount = 0;
            }
            TotalDiscount = UseCustomerPoints ? (CustomerPointsValue + OrderTotal * discount) : (OrderTotal * discount);

            // Update the UI
            OnPropertyChanged(nameof(TotalDiscount));
            OnPropertyChanged(nameof(OrderTotal));

        }
 
        public string CustomerPhone
        {
            get => _currentOrder.CustomerPhone ?? string.Empty;
            set
            {
                if (_currentOrder.CustomerPhone != value)
                {
                    _currentOrder.CustomerPhone = value;
                    _ = GetPointCustomerByPhone(value);
                    OnPropertyChanged();
                }
            }
        }

        public string CustomerName
        {
            get => _currentOrder.CustomerName ?? string.Empty;
            set
            {
                if (_currentOrder.CustomerName != value)
                {
                    _currentOrder.CustomerName = value;
                    OnPropertyChanged();
                }
            }
        }

        public string CustomerMail
        {
            get => _currentOrder.CustomerMail ?? string.Empty;
            set
            {
                if (_currentOrder.CustomerMail != value)
                {
                    _currentOrder.CustomerMail = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<OrderDetail> OrderDetails { get; set; }

        public XamlRoot XamlRoot { get; set; }


        public decimal OrderTotal => SubTotal - TotalDiscount;

        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                _searchQuery = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Product> SearchResults
        {
            get => _searchResults;
            set
            {
               SetProperty(ref _searchResults, value);
            }
        }

        public Product SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                SetProperty(ref _selectedProduct, value);
                // When selected product changes, notify that CanAddProduct might have changed
                (AddProductCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        public int ProductQuantity
        {
            get => _productQuantity;
            set
            {
                if (value < 1) value = 1;
                _productQuantity = value;
                OnPropertyChanged();
            }
        }

        // Commands
        public ICommand SearchProductCommand { get; }
        public ICommand AddProductCommand { get; }
        public ICommand RemoveOrderDetailCommand { get; }
        public ICommand SaveOrderCommand { get; }
        public ICommand ApplyPromotionCodeCommand { get; }

        private async Task SearchProductsAsync()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                return;

            var products = await _productService.SearchProductsAsync(SearchQuery);
            SearchResults.Clear();

            foreach (var product in products)
            {
                SearchResults.Add(product);
            }
        }

        private bool CanAddProduct()
        {
            return SelectedProduct != null && ProductQuantity > 0;
        }

        private void AddProductToOrder()
        {
            if (SelectedProduct == null)
                return;


            // Nếu đã có sản phẩm trong danh sách OrderDetails, chỉ cần cập nhật số lượng
            var existingDetail = OrderDetails.FirstOrDefault(od => od.Product_Id == SelectedProduct.Product_Id);

            if (existingDetail != null)
            {
                // Cập nhật số lượng
                existingDetail.Quantity += ProductQuantity;
                // UI cập nhật tự động vì OrderDetail implements INotifyPropertyChanged
                RefreshOrderDetails();
            }
            else
            {
                // Thêm sản phẩm mới vào danh sách OrderDetails
                var orderDetail = new OrderDetail
                {
                    Product_Id = SelectedProduct.Product_Id,
                    Product = SelectedProduct,
                    UnitPrice = SelectedProduct.Price,
                    Quantity = ProductQuantity,
                };

                OrderDetails.Add(orderDetail);
            }

            // Reset
            ProductQuantity = 1;
            OnPropertyChanged(nameof(OrderTotal));
            OnPropertyChanged(nameof(SubTotal));
            (SaveOrderCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }

        private void RemoveOrderDetail(OrderDetail orderDetail)
        {
            if (orderDetail != null)
            {
                OrderDetails.Remove(orderDetail);
                OnPropertyChanged(nameof(OrderTotal));
                (SaveOrderCommand as RelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        private void RefreshOrderDetails()
        {
            OnPropertyChanged(nameof(OrderDetails));
            OnPropertyChanged(nameof(OrderTotal));
        }


        private bool CanSaveOrder()
        {
            return !string.IsNullOrWhiteSpace(CustomerName) &&
                   !string.IsNullOrWhiteSpace(CustomerPhone) &&
                   !string.IsNullOrWhiteSpace(CustomerMail) &&
                   OrderDetails.Count > 0;
        }

        private async Task SaveOrderAsync()
        {
            //CurrentOrder.TotalAmount = OrderTotal;
            //CurrentOrder.OrderDetails = OrderDetails;

            //// Save the order
            //var orderCreateDTO = new OrderCreateDTO
            //{
            //    customer_id = CurrentOrder.
            //    staff_id = CurrentOrder.Staff_Id,
            //    total_amount = CurrentOrder.TotalAmount,
            //};
            //await _orderService.CreateOrder(orderCreateDTO);

      
            if (!CanSaveOrder())
            {
                await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Lỗi", "Vui lòng điền đầy đủ thông tin khách hàng và sản phẩm");
                return;
            }


            var order = new Order
            {
                Order_Id = Guid.NewGuid().ToString(),
                CustomerName = CustomerName,
                CustomerPhone = CustomerPhone,
                CustomerMail = CustomerMail,
                OrderDetails = OrderDetails,
                TotalAmount = OrderTotal,
                OrderDate = "",
                OrderStatus = "Đang xử lí",
            };
         

            if (_orderViewModel.SelectedOrder != null)
            {
                var index = _orderViewModel.Orders.IndexOf(_orderViewModel.SelectedOrder);
                if (index >= 0)
                {
                    _orderViewModel.Orders[index] = order;
                     await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Thông báo", "Cập nhật đơn hàng thành công");
                    _window.Close();
                }
            }
            else
            {
                _orderViewModel.Orders.Add(order);
                await _dialogService.ShowMessageAsync(_window.Content.XamlRoot, "Thông báo", "Thêm đơn hàng thành công");
                _window.Close();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }

    }
}
