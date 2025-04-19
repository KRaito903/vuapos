using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using vuapos.Presentation.Commands;
using vuapos.Presentation.DTO.Order;
using vuapos.Presentation.Models;
using vuapos.Presentation.Services;
using vuapos.Presentation.Views.Product;

namespace vuapos.Presentation.ViewModels
{
    public class OrderDetailViewModel : INotifyPropertyChanged
    {
        private readonly OrderService _orderService;
        private readonly ProductService _productService;

        private Order _currentOrder;
        private string _searchQuery;
        private ObservableCollection<Product> _searchResults;
        private Product _selectedProduct;
        private int _productQuantity = 1;

        public OrderDetailViewModel(OrderService orderService, ProductService productService)
        {
            _orderService = orderService;
            _productService = productService;

            // Initialize a new order
            SearchResults = new ObservableCollection<Product>();
            OrderDetails = new ObservableCollection<OrderDetail>();

            // Commands
            SearchProductCommand = new RelayCommand(async _ => await SearchProductsAsync());
            AddProductCommand = new RelayCommand(_ => AddProductToOrder(), _ => CanAddProduct());
            RemoveOrderDetailCommand = new RelayCommand(parameter => RemoveOrderDetail(parameter as OrderDetail));
            SaveOrderCommand = new RelayCommand(async _ => await SaveOrderAsync(), _ => CanSaveOrder());
        }

        public Order CurrentOrder
        {
            get => _currentOrder;
            set
            {
                _currentOrder = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<OrderDetail> OrderDetails { get; set; }

        public decimal OrderTotal => OrderDetails.Sum(od => od.Quantity);

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
                Debug.WriteLine($"SelectedProduct: {value?.Product_Name}");
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

            // Check if product already exists in order
            var existingDetail = OrderDetails.FirstOrDefault(od => od.Product_Id == SelectedProduct.Product_Id);

            if (existingDetail != null)
            {
                // Update quantity if already exists
                existingDetail.Quantity += ProductQuantity;
                // Make sure UI refreshes with new total
                RefreshOrderDetails();
            }
            else
            {
                // Add new order detail
                var orderDetail = new OrderDetail
                {
                    Product_Id = SelectedProduct.Product_Id,
                    Product = SelectedProduct,
                    UnitPrice = SelectedProduct.Price,
                    Quantity = ProductQuantity
                };

                OrderDetails.Add(orderDetail);
            }

            // Reset selection
            ProductQuantity = 1;
            OnPropertyChanged(nameof(OrderTotal));
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
            return !string.IsNullOrWhiteSpace(CurrentOrder.CustomerName) &&
                   !string.IsNullOrWhiteSpace(CurrentOrder.CustomerPhone) &&
                   OrderDetails.Count > 0;
        }

        private async Task SaveOrderAsync()
        {
            CurrentOrder.TotalAmount = OrderTotal;
            CurrentOrder.OrderDetails = OrderDetails;

            //// Save the order
            //var orderCreateDTO = new OrderCreateDTO
            //{
            //    customer_id = CurrentOrder.
            //    staff_id = CurrentOrder.Staff_Id,
            //    total_amount = CurrentOrder.TotalAmount,
            //};
            //await _orderService.CreateOrder(orderCreateDTO);

            OrderDetails.Clear();
            OnPropertyChanged(nameof(OrderTotal));
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
