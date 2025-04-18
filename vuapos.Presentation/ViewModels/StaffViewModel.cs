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
using vuapos.Presentation.Utils;

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
        private XamlRoot _xamlRoot; // Để hiển thị dialog
        private string _passwordLabel;
        public string PasswordLabel
        {
            get { return _passwordLabel; }
            set { SetProperty(ref _passwordLabel, value); }
        }


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
                // Khi chọn nhân viên, reset các trường mật khẩu và lỗi
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
                ValidatePassword();
            }
        }

        public string ConfirmPassword
        {
            get { return _confirmPassword; }
            set
            {
                SetProperty(ref _confirmPassword, value);
                ValidatePassword();
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
            set { 
                SetProperty(ref _isAddMode, value);
                // Cập nhật nhãn mật khẩu khi thay đổi chế độ
                PasswordLabel = value ? "Mật khẩu" : "Mật khẩu mới";
            }
        }

  

        public ICommand AddStaffCommand { get; }
        public ICommand EditStaffCommand { get; }
        public ICommand DeleteStaffCommand { get; }
        public ICommand SaveStaffCommand { get; }
        public ICommand CancelCommand { get; }

        // Cần XamlRoot để hiện dialog trong WinUI 3
        public StaffViewModel(StaffService staffService)
        {
            _staffService = staffService;
            // Khởi tạo các lệnh
            AddStaffCommand = new RelayCommand(param => ShowAddStaffDialog());
            EditStaffCommand = new RelayCommand(param => ShowEditStaffDialog(), param => CanEditStaff());
            DeleteStaffCommand = new RelayCommand(param => ExecuteDeleteStaff(), param => CanDeleteStaff());
            SaveStaffCommand = new RelayCommand(param => ExecuteSaveStaff(), param => CanSaveStaff());
            CancelCommand = new RelayCommand(param => { /* Dialog sẽ tự đóng */ });

            // Load dữ liệu ban đầu
            _ = LoadStaff();
        }

        // Phương thức để cập nhật XamlRoot khi cần
        public void UpdateXamlRoot(XamlRoot xamlRoot)
        {
            _xamlRoot = xamlRoot;
        }

        private async Task LoadStaff()
        {
            var staffs = await _staffService.GetAllStaffsAsync();
            if (staffs != null)
            {
                Staffs = new ObservableCollection<Staff>(staffs);
            }
        }

        private void ValidatePassword()
        {
            // Nếu đang ở chế độ chỉnh sửa và không nhập mật khẩu mới, thì không cần kiểm tra
            if (!IsAddMode && string.IsNullOrEmpty(NewPassword) && string.IsNullOrEmpty(ConfirmPassword))
            {
                PasswordError = string.Empty;
                IsPasswordValid = true;
                return;
            }

            // Kiểm tra mật khẩu khi ở chế độ thêm mới hoặc đã nhập mật khẩu mới
            if (string.IsNullOrEmpty(NewPassword))
            {
                PasswordError = "Vui lòng nhập mật khẩu";
                IsPasswordValid = false;
                return;
            }

            if (NewPassword != ConfirmPassword)
            {
                PasswordError = "Mật khẩu không trùng khớp";
                IsPasswordValid = false;
                return;
            }

            if (!PasswordValidator.IsValidPassword(NewPassword))
            {
                PasswordError = "Mật khẩu phải có ít nhất 6 ký tự, bao gồm chữ cái và số";
                IsPasswordValid = false;
                return;
            }

            PasswordError = string.Empty;
            IsPasswordValid = true;
        }

        private async void ShowAddStaffDialog()
        {
            if (_xamlRoot == null)
            {
                Debug.WriteLine("XamlRoot không được thiết lập, không thể hiển thị dialog");
                return;
            }

            IsAddMode = true;
            NewPassword = string.Empty;
            ConfirmPassword = string.Empty;
            PasswordError = string.Empty;
            IsPasswordValid = false;

            SelectedStaff = new Staff();

            ContentDialog dialog = new ContentDialog
            {
                Title = "Thêm nhân viên mới",
                PrimaryButtonText = "Lưu",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = _xamlRoot,
                Content = new StaffDialogContent(this)
            };

            dialog.PrimaryButtonClick += async (s, e) =>
            {
                if (CanSaveStaff())
                {
                    // Cho phép dialog đóng
                    e.Cancel = false;

                    // Lưu lại logic xử lý sau khi dialog đã đóng
                    dialog.Closed += async (_s, _e) =>
                    {
                        await SaveStaffAsync();
                    };
                }
                else
                {
                    e.Cancel = true; // Ngăn dialog đóng nếu chưa hợp lệ
                }
            };


            await dialog.ShowAsync();
        }

        private async void ShowEditStaffDialog()
        {
            if (_xamlRoot == null)
            {
                Debug.WriteLine("XamlRoot không được thiết lập, không thể hiển thị dialog");
                return;
            }

            if (SelectedStaff != null)
            {
                IsAddMode = false;
                // Tạo bản sao để tránh sửa trực tiếp vào đối tượng gốc
                SelectedStaff = new Staff
                {
                    Staff_Id = SelectedStaff.Staff_Id,
                    Username = SelectedStaff.Username,
                    Phone = SelectedStaff.Phone,
                    Role = SelectedStaff.Role,
                    // Không sao chép mật khẩu
                };

                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;
                PasswordError = string.Empty;
                IsPasswordValid = true; // Cho phép lưu nếu không đổi mật khẩu

                // Tạo và hiển thị dialog
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Chỉnh sửa nhân viên",
                    PrimaryButtonText = "Lưu",
                    CloseButtonText = "Hủy",
                    DefaultButton = ContentDialogButton.Primary,
                    XamlRoot = _xamlRoot,
                    Content = new StaffDialogContent(this)
                };

                // Gắn lệnh vào sự kiện của dialog
                dialog.PrimaryButtonClick += async (s, e) =>
                {
                    if (CanSaveStaff())
                    {
                        e.Cancel = false; 
                        await SaveStaffAsync();
                    }
                    else
                    {
                        e.Cancel = true; // Ngăn dialog đóng nếu dữ liệu không hợp lệ
                    }
                };

                await dialog.ShowAsync();
            }
        }

        private bool CanEditStaff()
        {
            return SelectedStaff != null;
        }

        private async void ExecuteDeleteStaff()
        {
            if (_xamlRoot == null)
            {
                Debug.WriteLine("XamlRoot không được thiết lập, không thể hiển thị dialog");
                return;
            }

            if (SelectedStaff != null)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Xác nhận xóa",
                    Content = $"Bạn có chắc chắn muốn xóa nhân viên {SelectedStaff.Username}?",
                    PrimaryButtonText = "Xóa",
                    CloseButtonText = "Hủy",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = _xamlRoot
                };

                var result = await dialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    try
                    {
                        await _staffService.DeleteStaffAsync(SelectedStaff.Staff_Id);
                        Staffs.Remove(SelectedStaff);
                        SelectedStaff = null;
                    }
                    catch (Exception ex)
                    {
                        // Hiển thị thông báo lỗi
                        ContentDialog errorDialog = new ContentDialog
                        {
                            Title = "Lỗi",
                            Content = $"Không thể xóa nhân viên: {ex.Message}",
                            CloseButtonText = "Đóng",
                            XamlRoot = _xamlRoot
                        };
                        await errorDialog.ShowAsync();
                    }
                }
            }
        }

        private bool CanDeleteStaff()
        {
            return SelectedStaff != null;
        }

        private bool CanSaveStaff()
        {
            if (SelectedStaff == null)
                return false;
            // Kiểm tra các trường bắt buộc
            if (string.IsNullOrWhiteSpace(SelectedStaff.Username) ||
                string.IsNullOrWhiteSpace(SelectedStaff.Phone))
                return false;

            if (IsAddMode)
            {
                return IsPasswordValid;
            }
            else
            {
                // Chế độ chỉnh sửa, nếu không nhập mật khẩu mới hoặc mật khẩu hợp lệ
                return IsPasswordValid;
            }
        }

        private void ExecuteSaveStaff()
        {
            _ = SaveStaffAsync();
        }

        private async Task SaveStaffAsync()
        {
            if (_xamlRoot == null)
            {
                Debug.WriteLine("XamlRoot không được thiết lập, không thể hiển thị dialog");
                return;
            }

            try
            {
                if (IsAddMode)
                {
                    // Thêm mật khẩu vào đối tượng nhân viên
                    SelectedStaff.Password = NewPassword;
                    // creatDto DTO
                    var staffCreateDto = new StaffCreateDTO
                    {
                        username = SelectedStaff.Username,
                        password = SelectedStaff.Password,
                        phone = SelectedStaff.Phone,
                        role = SelectedStaff.Role
                    };

                    // Gọi API thêm nhân viên
                    var newStaff = await _staffService.CreateStaffAsync(staffCreateDto);
                    if (newStaff)
                    {
                        _ = LoadStaff();
                        SelectedStaff = null;
                        ContentDialog successDialog = new ContentDialog
                        {
                            Title = "Thành công",
                            Content = "Đã thêm nhân viên mới thành công",
                            CloseButtonText = "Đóng",
                            XamlRoot = _xamlRoot
                        };
                        await successDialog.ShowAsync();
                    }
                }
                else
                {
                    // Chỉ cập nhật mật khẩu nếu đã nhập
                    if (!string.IsNullOrEmpty(NewPassword))
                    {
                        SelectedStaff.Password = NewPassword;
                    }

                    // Gọi API cập nhật nhân viên
                    var updatedStaff = await _staffService.UpdateStaffAsync(SelectedStaff.Staff_Id, new
                    {
                        username = SelectedStaff.Username,
                        password = SelectedStaff.Password,
                        phone = SelectedStaff.Phone,
                        role = SelectedStaff.Role
                    }); 
                    if (updatedStaff != null)
                    {
                        // Cập nhật lại danh sách
                        int index = Staffs.IndexOf(Staffs.FirstOrDefault(s => s.Staff_Id == SelectedStaff.Staff_Id));
                        if (index >= 0)
                        {
                            Staffs[index] = SelectedStaff;
                        }

                        // Thông báo thành công
                        ContentDialog successDialog = new ContentDialog
                        {
                            Title = "Thành công",
                            Content = "Đã cập nhật nhân viên thành công",
                            CloseButtonText = "Đóng",
                            XamlRoot = _xamlRoot
                        };
                        await successDialog.ShowAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                // Hiển thị thông báo lỗi
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Không thể lưu nhân viên: {ex.Message}",
                    CloseButtonText = "Đóng",
                    XamlRoot = _xamlRoot
                };
                await errorDialog.ShowAsync();
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