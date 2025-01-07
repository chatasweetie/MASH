using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hello_World
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private AISuggestions _aiSuggestions;

        public MainViewModel()
        {
            _aiSuggestions = new AISuggestions();
        }

        public AISuggestions AISuggestions
        {
            get => _aiSuggestions;
            set
            {
                _aiSuggestions = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
