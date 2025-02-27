using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CRM.Client.Models
{
    public class CheckBoxModel : INotifyPropertyChanged
    {
        private bool _selected;
        public object Value { get; set; }
        public string Label { get; set; }

        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}