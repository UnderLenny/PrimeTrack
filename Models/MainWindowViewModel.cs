using System.ComponentModel;

namespace PrimeTrack.Models
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private bool _isAdmin;
        public bool IsAdmin
        {
            get { return _isAdmin; }
            set
            {
                _isAdmin = value;
                OnPropertyChanged(nameof(IsAdmin));
            }
        }

        public MainWindowViewModel(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
