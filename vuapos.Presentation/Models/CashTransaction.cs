using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace vuapos.Presentation.Models
{
    public class CashTransaction
    {
        public string Id { get; set; }
        public string CashRegisterId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionTime { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; } // Ví dụ: Mã đơn hàng, mã phiếu nhập kho
        public string? CreatedByEmployeeId { get; set; }
        public string Notes { get; set; }
    }

    public enum TransactionType
    {
        CashIn, // Thu tiền
        CashOut, // Chi tiền
        InitialCash, // Số dư ban đầu
        Adjustment, // Điều chỉnh
        EndOfDay // Kết số cuối ngày
    }

    public class EndOfDayReport
    {
        public string Id { get; set; }
        public string CashRegisterId { get; set; }
        public DateTime ReportDate { get; set; }
        public decimal SystemBalance { get; set; } // Số dư theo hệ thống
        public decimal ActualBalance { get; set; } // Số dư thực tế kiểm đếm
        public decimal Difference { get; set; } // Chênh lệch
        public string Notes { get; set; }
        public string EmployeeId { get; set; }
        public bool IsApproved { get; set; }
        public string? ApprovedByEmployeeId { get; set; }
    }
}
