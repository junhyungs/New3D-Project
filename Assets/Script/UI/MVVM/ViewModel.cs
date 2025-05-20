using System.ComponentModel;

namespace ModelViewViewModel
{
    public class ViewModel
    {
        public PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public virtual void RegisterEvent() { }
        public virtual void UnRegisterEvent() { }
        public virtual void RegisterEvent<TParameter>(TParameter parameter) { }
        public virtual void UnRegisterEvent<TParameter>(TParameter parameter) { }
    }
}

