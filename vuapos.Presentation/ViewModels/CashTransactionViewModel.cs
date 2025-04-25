using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using vuapos.Presentation.Models;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace vuapos.Presentation.ViewModels
{
    public class CashTransactionViewModel : INotifyPropertyChanged
    {
        private readonly CashTransaction _transaction;

        public CashTransactionViewModel(CashTransaction transaction)
        {
            _transaction = transaction;
        }

        public string TransactionTime => _transaction.TransactionTime.ToString("dd/MM/yyyy HH:mm");

        public string TransactionTypeText => _transaction.Type switch
        {
            TransactionType.CashIn => "Thu - " + _transaction.Description,
            TransactionType.CashOut => "Chi - " + _transaction.Description,
            TransactionType.InitialCash => "Số dư ban đầu",
            TransactionType.Adjustment => "Điều chỉnh",
            TransactionType.EndOfDay => "Kết số cuối ngày",
            _ => _transaction.Description
        };

        public string Description => string.IsNullOrEmpty(_transaction.ReferenceNumber)
            ? _transaction.Description
            : $"{_transaction.Description} #{_transaction.ReferenceNumber}";

        public string Amount
        {
            get
            {
                var prefix = (_transaction.Type == TransactionType.CashIn ||
                              _transaction.Type == TransactionType.InitialCash)
                    ? "+"
                    : (_transaction.Type == TransactionType.CashOut ? "-" : "");

                return $"{prefix}{_transaction.Amount:N0}";
            }
        }

        public Brush AmountColor
        {
            get
            {
                return (_transaction.Type == TransactionType.CashIn ||
                        _transaction.Type == TransactionType.InitialCash)
                    ? new SolidColorBrush(Colors.Green)
                    : (_transaction.Type == TransactionType.CashOut
                        ? new SolidColorBrush(Colors.Red)
                        : new SolidColorBrush(Colors.Black));
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
