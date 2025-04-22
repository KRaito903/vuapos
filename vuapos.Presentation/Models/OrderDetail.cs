using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using vuapos.Presentation.Views.Product;

namespace vuapos.Presentation.Models
{
    public class OrderDetail : INotifyPropertyChanged
    {
        private int _quantity;
        private decimal _unitPrice;
        private decimal _subtotal;

        public string OrderDetail_Id { get; set; }
        public string Order_Id { get; set; } // Khóa ngoại liên kết đến Order
        public string Product_Id { get; set; } // Khóa ngoại liên kết đến Product

        public int Quantity
        {
            get => _quantity;
            set
            {
                if (_quantity != value)
                {
                    _quantity = value;
                    Subtotal = _quantity * UnitPrice; // Cập nhật luôn Subtotal
                    OnPropertyChanged();
                }
            }
        }

        public decimal UnitPrice
        {
            get => _unitPrice;
            set
            {
                if (_unitPrice != value)
                {
                    _unitPrice = value;
                    Subtotal = Quantity * _unitPrice; // Cập nhật luôn Subtotal
                    OnPropertyChanged();
                }
            }
        }

        public decimal Subtotal
        {
            get => _subtotal;
            private set
            {
                if (_subtotal != value)
                {
                    _subtotal = value;
                    OnPropertyChanged();
                }
            }
        }

        public Product Product { get; set; }

        public OrderDetail() { }

        public OrderDetail(Product product, int quantity)
        {
            Product = product;
            Product_Id = product.Product_Id;
            Quantity = quantity;
            UnitPrice = product.Price;
            Subtotal = Quantity * UnitPrice;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
