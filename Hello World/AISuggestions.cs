using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Hello_World
{
    public class AISuggestions : INotifyPropertyChanged
    {
        private string _spouse3;
        private string _kids3;
        private string _car3;
        private string _career3;

        public string Spouse3
        {
            get => _spouse3;
            set
            {
                _spouse3 = value;
                Debug.WriteLine("Spouse3: " + _spouse3);
                OnPropertyChanged();
            }
        }

        public string Kids3
        {
            get => _kids3;
            set
            {
                _kids3 = value;
                Debug.WriteLine("Kids3: " + _kids3);
                OnPropertyChanged();

            }
        }

        public string Car3
        {
            get => _car3;
            set
            {
                _car3 = value;
                Debug.WriteLine("Car3: " + _car3);
                OnPropertyChanged();
            }
        }

        public string Career3
        {
            get => _career3;
            set
            {
                _career3 = value;
                Debug.WriteLine("Career3: " + _career3);
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            Debug.WriteLine("Property Changed: " + propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
