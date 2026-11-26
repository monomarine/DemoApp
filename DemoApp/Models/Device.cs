using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DemoApp.Models
{
    public enum DeviceType
    {
        [Description("не определен")] Unknown,
        [Description("телефон")] Phone,
        [Description("ПК")] PC,
        [Description("Ноутбук")] Laptop,
        [Description("Принтер")] Printer
    }
    public class Device : INotifyPropertyChanged
    {
        private string _name;
        private DeviceType _type;
        private decimal _price;
        private bool _isInUse;

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(); //уведомление UI
            }
        }
        public decimal Price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged();
            }
        }
        public bool IsInUse
        {
            get => _isInUse;
            set
            {
                _isInUse = value;
                OnPropertyChanged();
            }
        }
        public DeviceType Type
        {
            get => _type;
            set
            {
                _type = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
