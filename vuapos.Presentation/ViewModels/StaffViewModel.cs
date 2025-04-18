using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.Models;
using System.Windows.Input;
using vuapos.Presentation.Commands;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
using vuapos.Presentation.Views.Staff;
using vuapos.Presentation.Services;
using Microsoft.Extensions.DependencyInjection;
using vuapos.Presentation.DAO.Interface;
using vuapos.Presentation.DAO.Implement;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using vuapos.Presentation.DTO.Staff;

namespace vuapos.Presentation.ViewModels
{
    public class StaffViewModel : INotifyPropertyChanged
    {
        private readonly StaffService _staffService;
        private ObservableCollection<Staff> _staffs;
        private Staff _selectedStaff;
        private string _newPassword;
        private string _confirmPassword;
        private string _passwordError;
        private bool _isPasswordValid;
        private bool _isAddMode;
        //private XamlRoot _xamlRoot; // Để hiển thị dialog

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Staff> Staffs
        {
            get { return _staffs; }
            set { SetProperty(ref _staffs, value); }
        }

        public Staff SelectedStaff
        {
            get { return _selectedStaff; }
            set
            {
                SetProperty(ref _selectedStaff, value);
                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
                PasswordError = string.Empty;
            }
        }

        public string NewPassword
        {
            get { return _newPassword; }
            set
            {
                SetProperty(ref _newPassword, value);
               // ValidatePassword();
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                SetProperty(ref _confirmPassword, value);
               // ValidatePassword();
            }
        }

        public string PasswordError
        {
            get { return _passwordError; }
            set { SetProperty(ref _passwordError, value); }
        }

        public bool IsPasswordValid
        {
            get { return _isPasswordValid; }
            set { SetProperty(ref _isPasswordValid, value); }
        }

        public bool IsAddMode
        {
            get { return _isAddMode; }
            set
            {
                SetProperty(ref _isAddMode, value);
                // Thông báo sự thay đổi của PasswordLabel
                OnPropertyChanged(nameof(PasswordLabel));
            }
        }

        public string PasswordLabel => IsAddMode ? "Mật khẩu" : "Mật khẩu mới (để trống nếu không đổi)";

        public ICommand AddStaffCommand { get; }
        public ICommand EditStaffCommand { get; }
        public ICommand DeleteStaffCommand { get; }
        public ICommand SaveStaffCommand { get; }
        public ICommand CancelCommand { get; }

     
        public StaffViewModel()
        {
            _staffService = App.Services.GetRequiredService<StaffService>();

            // Khởi tạo các lệnh
            AddStaffCommand = new RelayCommand<object>(param => ShowAddStaffDialog());
            //EditStaffCommand = new RelayCommand<object>(param => ShowEditStaffDialog(param as Staff));
            //DeleteStaffCommand = new RelayCommand<object>(param => ExecuteDeleteStaff(param as Staff), param => CanDeleteStaff());
            //SaveStaffCommand = new RelayCommand<object>(param => ExecuteSaveStaff(), param => CanSaveStaff());
            //CancelCommand = new RelayCommand<object>(param => { /* Dialog sẽ tự đóng */ });

            // Load dữ liệu ban đầu
            _ = LoadStaff();
        }

        //public void UpdateXamlRoot(XamlRoot xamlRoot)
        //{
        //    _xamlRoot = xamlRoot;
        //}

        private async Task LoadStaff()
        {
            var staffs = await _staffService.GetAllStaffsAsync();
            if (staffs != null)
            {
                Staffs = new ObservableCollection<Staff>(staffs);
            }
        }

        // Thực hiện thêm mới nhân viên
        private async void ShowAddStaffDialog()
        {
            Debug.WriteLine("ShowAddStaffDialog called");
            //IsAddMode = true;
            //SelectedStaff = new Staff();
            //NewPassword = string.Empty;
            //ConfirmPassword = string.Empty;
            //PasswordError = string.Empty;

            ////ContentDialog dialog = new ContentDialog
            ////{
            ////    Title = "Thêm nhân viên mới",
            ////    PrimaryButtonText = "Lưu",
            ////    CloseButtonText = "Hủy",
            ////    DefaultButton = ContentDialogButton.Primary,
            ////    Content = new StaffEditDialog { DataContext = this },
            ////    XamlRoot = _xamlRoot
            ////};

            //ContentDialog dialog = new ContentDialog
            //{
            //    Title = "Thông báo",
            //    Content = "Bạn chắc chắn muốn thoát?",
            //    PrimaryButtonText = "Có",
            //    CloseButtonText = "Không",
            //    XamlRoot = _xamlRoot // Bắt buộc nếu dùng trong WinUI 3
            //};

            ////dialog.PrimaryButtonCommand = SaveStaffCommand;
            //ContentDialogResult result = await dialog.ShowAsync();

            //if (result == ContentDialogResult.Primary)
            //{
            //    // Thực hiện hành động nếu người dùng đồng ý
            //}
        }

        //// Hiển thị dialog sửa nhân viên
        //private async void ShowEditStaffDialog(Staff staff)
        //{
        //    if (staff == null) return;

        //    IsAddMode = false;
        //    SelectedStaff = staff;
        //    NewPassword = string.Empty;
        //    ConfirmPassword = string.Empty;
        //    PasswordError = string.Empty;

        //    ContentDialog dialog = new ContentDialog
        //    {
        //        Title = "Chỉnh sửa nhân viên",
        //        PrimaryButtonText = "Lưu",
        //        CloseButtonText = "Hủy",
        //        DefaultButton = ContentDialogButton.Primary,
        //        Content = new StaffEditDialog { DataContext = this },
        //        XamlRoot = _xamlRoot
        //    };

        //    dialog.PrimaryButtonCommand = SaveStaffCommand;
        //    var result = await dialog.ShowAsync();
        //}

        //// Xóa nhân viên
        //private async void ExecuteDeleteStaff(Staff staff)
        //{
        //    if (staff == null) staff = SelectedStaff;
        //    if (staff == null) return;

        //    ContentDialog confirmDialog = new ContentDialog
        //    {
        //        Title = "Xác nhận xóa",
        //        Content = $"Bạn có chắc muốn xóa nhân viên '{staff.Username}'?",
        //        PrimaryButtonText = "Xóa",
        //        CloseButtonText = "Hủy",
        //        DefaultButton = ContentDialogButton.Close,
        //        XamlRoot = _xamlRoot
        //    };

        //    var result = await confirmDialog.ShowAsync();
        //    if (result == ContentDialogResult.Primary)
        //    {
        //        try
        //        {
        //            await _staffService.DeleteStaffAsync(staff.Staff_Id);
        //            Staffs.Remove(staff);
        //            ShowInfoMessage("Đã xóa thành công!", "Thông báo");
        //        }
        //        catch (Exception ex)
        //        {
        //            ShowErrorMessage($"Lỗi khi xóa nhân viên: {ex.Message}", "Lỗi");
        //        }
        //    }
        //}

        //// Lưu nhân viên (thêm mới hoặc cập nhật)
        //private async void ExecuteSaveStaff()
        //{
        //    if (SelectedStaff == null) return;

        //    try
        //    {
        //        // Nếu đang ở chế độ thêm mới
        //        if (IsAddMode)
        //        {
        //            // Cập nhật mật khẩu cho nhân viên mới
        //            SelectedStaff.Password = NewPassword;

        //            // Gọi service để thêm nhân viên mới
        //            // Chuyển đổi SelectedStaff thành StaffCreateDTO
        //            var staffCreateDTO = new StaffCreateDTO
        //            {
        //                username = SelectedStaff.Username,
        //                password = SelectedStaff.Password,
        //                phone = SelectedStaff.Phone,
        //                role = SelectedStaff.Role
        //            };
        //            var newStaff = await _staffService.CreateStaffAsync(staffCreateDTO);
        //            if (newStaff != null)
        //            {
        //                Staffs.Add(SelectedStaff);
        //                ShowInfoMessage("Thêm nhân viên thành công!", "Thông báo");
        //            }
        //        }
        //        else // Chế độ chỉnh sửa
        //        {
        //            // Nếu nhập mật khẩu mới thì cập nhật, nếu để trống thì giữ nguyên
        //            if (!string.IsNullOrEmpty(NewPassword))
        //            {
        //                SelectedStaff.Password = NewPassword;
        //            }

        //            // Gọi service để cập nhật nhân viên
        //            var updatedStaff = await _staffService.UpdateStaffAsync(SelectedStaff.Staff_Id, new
        //            {
        //                username = SelectedStaff.Username,
        //                password = SelectedStaff.Password,
        //                phone = SelectedStaff.Phone,
        //                role = SelectedStaff.Role
        //            });
        //            if (updatedStaff != null)
        //            {
        //                // Tìm và cập nhật nhân viên trong danh sách
        //                int index = Staffs.IndexOf(Staffs.FirstOrDefault(s => s.Staff_Id == SelectedStaff.Staff_Id));
        //                if (index >= 0)
        //                {
        //                    Staffs[index] = SelectedStaff;
        //                }
        //                ShowInfoMessage("Cập nhật nhân viên thành công!", "Thông báo");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ShowErrorMessage($"Lỗi khi lưu nhân viên: {ex.Message}", "Lỗi");
        //    }
        //}

        //// Kiểm tra có thể chỉnh sửa không
        //private bool CanEditStaff()
        //{
        //    return SelectedStaff != null;
        //}

        //// Kiểm tra có thể xóa không
        //private bool CanDeleteStaff()
        //{
        //    return SelectedStaff != null;
        //}

        //// Kiểm tra có thể lưu không
        //private bool CanSaveStaff()
        //{
        //    if (SelectedStaff == null) return false;

        //    // Trong chế độ thêm mới, mật khẩu là bắt buộc
        //    if (IsAddMode && string.IsNullOrEmpty(NewPassword))
        //        return false;

        //    // Kiểm tra các trường bắt buộc khác
        //    if (string.IsNullOrEmpty(SelectedStaff.Username) ||
        //        string.IsNullOrEmpty(SelectedStaff.Role))
        //        return false;

        //    // Nếu có nhập mật khẩu mới, phải hợp lệ
        //    if (!string.IsNullOrEmpty(NewPassword) && !IsPasswordValid)
        //        return false;

        //    return true;
        //}

        // Xác thực mật khẩu
        //private void ValidatePassword()
        //{
        //    // Chỉ xác thực nếu có nhập mật khẩu
        //    if (string.IsNullOrEmpty(NewPassword))
        //    {
        //        // Trong chế độ thêm mới, mật khẩu là bắt buộc
        //        if (IsAddMode)
        //        {
        //            PasswordError = "Mật khẩu là bắt buộc";
        //            IsPasswordValid = false;
        //        }
        //        else // Trong chế độ sửa, mật khẩu có thể để trống
        //        {
        //            PasswordError = string.Empty;
        //            IsPasswordValid = true;
        //        }
        //        return;
        //    }

        //    // Kiểm tra độ dài mật khẩu
        //    if (NewPassword.Length < 6)
        //    {
        //        PasswordError = "Mật khẩu phải có ít nhất 6 ký tự";
        //        IsPasswordValid = false;
        //        return;
        //    }

        //    // Kiểm tra xác nhận mật khẩu
        //    if (NewPassword != ConfirmPassword)
        //    {
        //        PasswordError = "Mật khẩu xác nhận không khớp";
        //        IsPasswordValid = false;
        //        return;
        //    }

        //    // Mật khẩu hợp lệ
        //    PasswordError = string.Empty;
        //    IsPasswordValid = true;
        //}

        //// Hiển thị thông báo lỗi
        //private async void ShowErrorMessage(string message, string title)
        //{
        //    ContentDialog dialog = new ContentDialog
        //    {
        //        Title = title,
        //        Content = message,
        //        CloseButtonText = "Đóng",
        //        XamlRoot = _xamlRoot
        //    };
        //    await dialog.ShowAsync();
        //}

        //// Hiển thị thông báo thông tin
        //private async void ShowInfoMessage(string message, string title)
        //{
        //    ContentDialog dialog = new ContentDialog
        //    {
        //        Title = title,
        //        Content = message,
        //        CloseButtonText = "Đóng",
        //        XamlRoot = _xamlRoot
        //    };
        //    await dialog.ShowAsync();
        //}

        // Helper method to notify property changed
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Helper method to set property and notify if changed
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}