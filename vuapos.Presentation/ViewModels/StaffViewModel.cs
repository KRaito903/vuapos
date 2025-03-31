using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DAO;
using vuapos.Presentation.Models;
using System.Windows.Input;
using vuapos.Presentation.Commands;

namespace vuapos.Presentation.ViewModels
{
    public class StaffViewModel : INotifyPropertyChanged
    {
        private readonly IDAO<Staff> _staffDAO;
        private ObservableCollection<Staff> _staffList;
        private Staff _selectedStaff; // Nhân viên được chọn để sửa hoặc xóa

        public ObservableCollection<Staff> StaffList
        {
            get { return _staffList; }
            set { _staffList = value; OnPropertyChanged(nameof(StaffList)); }
        }

        public Staff SelectedStaff
        {
            get { return _selectedStaff; }
            set { _selectedStaff = value; OnPropertyChanged(nameof(SelectedStaff)); }
        }

        public ICommand AddStaffCommand { get; }
        public ICommand EditStaffCommand { get; }
        public ICommand DeleteStaffCommand { get; }

        public StaffViewModel()
        {
            _staffDAO = new StaffDAO();
            LoadStaff();

            AddStaffCommand = new RelayCommand(AddStaff);
            EditStaffCommand = new RelayCommand(EditStaff, CanEditOrDelete);
            DeleteStaffCommand = new RelayCommand(DeleteStaff, CanEditOrDelete);
        }

        private void LoadStaff()
        {
            StaffList = new ObservableCollection<Staff>(_staffDAO.GetAll());
        }

        private void AddStaff(object parameter)
        {
            var newStaff = new Staff
            {
                StaffId = System.Guid.NewGuid().ToString(), // Tạo ID mới
                Username = "newuser",
                Password = "password",
                Role = "Staff",
                Phone = "123456789"
            };

            _staffDAO.Add(newStaff);
            LoadStaff();
        }

        private void EditStaff(object parameter)
        {
            if (SelectedStaff == null) return;

            _staffDAO.Update(SelectedStaff);
            LoadStaff();
        }

        private void DeleteStaff(object parameter)
        {
            if (SelectedStaff == null) return;

            _staffDAO.Delete(SelectedStaff.StaffId);
            LoadStaff();
        }

        private bool CanEditOrDelete(object parameter) => SelectedStaff != null;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
