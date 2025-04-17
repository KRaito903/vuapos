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


namespace vuapos.Presentation.ViewModels
{
    public class StaffViewModel : INotifyPropertyChanged
    {
        private readonly IDAO<Staff> _staffDAO;
        private readonly StaffService _StaffService;

        private ObservableCollection<Staff> _staffs;
        private Staff _selectedStaff;

        public ObservableCollection<Staff> Staffs
        {
            get { return _staffs; }
            set { SetProperty(ref _staffs, value); }
        }

        public Staff SelectedStaff
        {
            get { return _selectedStaff; }
            set { SetProperty(ref _selectedStaff, value); }
        }

        public ICommand AddStaffCommand { get; }
        public ICommand EditStaffCommand { get; }
        public ICommand DeleteStaffCommand { get; }

        public StaffViewModel()
        {
            _staffDAO = new StaffDAO();
            _StaffService = App.Services.GetRequiredService<StaffService>();
            _ = LoadStaff();

            AddStaffCommand = new RelayCommand(AddStaff);
            EditStaffCommand = new RelayCommand<Staff>(EditStaff);
            DeleteStaffCommand = new RelayCommand<Staff>(DeleteStaff);
        }

        private async Task LoadStaff()
        {
            var staffs = await _StaffService.GetAllStaffsAsync();
            Debug.WriteLine($"Staffs: {staffs}");
            if (staffs != null)
            {
                Staffs = new ObservableCollection<Staff>(staffs);
            }
            else
            {
                Staffs = new ObservableCollection<Staff>();
            }
        }

        private void AddStaff(object parameter)
        {
            var newStaff = new Staff
            {
                StaffId = Guid.NewGuid().ToString(),
                Username = "newuser",
                Password = "password",
                Role = "Staff",
                Phone = "123456789"
            };

            _staffDAO.Add(newStaff);
            _ = LoadStaff();
        }

        private void EditStaff(Staff staff)
        {
            if (staff == null) return;
            _staffDAO.Update(staff);

            LoadStaff();
        }

        private void DeleteStaff(Staff staff)
        {
            if (staff == null) return;

            _staffDAO.Delete(staff.StaffId);
            LoadStaff();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(storage, value))
                return false;

            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}