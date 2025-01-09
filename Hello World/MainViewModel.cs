using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Hello_World
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private AISuggestions _aiSuggestions;
        private bool _modelIsReady;
        private string _buttonText;
        private string _spouse1;
        private string _spouse2;
        private string _kids1;
        private string _kids2;
        private string _car1;
        private string _car2;
        private string _career1;
        private string _career2;

        public MainViewModel()
        {
            _aiSuggestions = new AISuggestions();
            _buttonText = "Warming up the Magic";
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

        public bool ModelIsReady
        {
            get => _modelIsReady;
            set
            {
                _modelIsReady = value;
                OnPropertyChanged();
            }
        }

        public string ButtonText
        {
            get => _buttonText;
            set
            {
                _buttonText = value;
                OnPropertyChanged();
            }
        }

        public string Spouse1
        {
            get => _spouse1;
            set
            {
                _spouse1 = value;
                OnPropertyChanged();
            }
        }

        public string Spouse2
        {
            get => _spouse2;
            set
            {
                _spouse2 = value;
                OnPropertyChanged();
            }
        }

        public string Kids1
        {
            get => _kids1;
            set
            {
                _kids1 = value;
                OnPropertyChanged();
            }
        }

        public string Kids2
        {
            get => _kids2;
            set
            {
                _kids2 = value;
                OnPropertyChanged();
            }
        }

        public string Car1
        {
            get => _car1;
            set
            {
                _car1 = value;
                OnPropertyChanged();
            }
        }

        public string Car2
        {
            get => _car2;
            set
            {
                _car2 = value;
                OnPropertyChanged();
            }
        }

        public string Career1
        {
            get => _career1;
            set
            {
                _career1 = value;
                OnPropertyChanged();
            }
        }

        public string Career2
        {
            get => _career2;
            set
            {
                _career2 = value;
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
