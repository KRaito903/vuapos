using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using vuapos.Presentation.DAO;
using vuapos.Presentation.Models;

namespace vuapos.Presentation.ViewModels
{
    public class StaffViewModel : INotifyPropertyChanged
    {
        private readonly IDAO<Staff> _staffDAO;
        private ObservableCollection<Staff> _staffList;

        public ObservableCollection<Staff> StaffList
        {
            get { return _staffList; }
            set { _staffList = value; OnPropertyChanged(nameof(StaffList)); }
        }

        public StaffViewModel()
        {
            _staffDAO = new StaffDAO();
            LoadStaff();
        }

        private void LoadStaff()
        {
            StaffList = new ObservableCollection<Staff>(_staffDAO.GetAll());
        }

        public void AddStaff(Staff staff)
        {
            _staffDAO.Add(staff);
            LoadStaff();
        }

        public void UpdateStaff(Staff staff)
        {
            _staffDAO.Update(staff);
            LoadStaff();
        }

        public void DeleteStaff(string id)
        {
            _staffDAO.Delete(id);
            LoadStaff();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
