using DemoApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoApp
{
    /// <summary>
    /// Логика взаимодействия для EditorWindow.xaml
    /// </summary>
    public partial class EditorWindow : Window
    {
        private Dictionary<DeviceType, string> TypesToComboBox; //значения для combobox из перечисления
        public Device CurrentDevice { get; set; } //открытие свойство окна для редактирования записи
        private int SelectedStatusId = 0; //по умолчанию 1 элемент словаря в качестве выбранного типа
        public EditorWindow(Device device)
        {
            InitializeComponent();

            // Если объект пустой (нажали "Добавить"), создаем новый экземпляр
            // Если передали существующий (нажали "Изменить"), работаем с ним
            CurrentDevice = device ?? new Device();

            // Заполняем текстовые поля текущими значениями
            NameTextBox.Text = CurrentDevice.Name;
            SelectedStatusId = (int)CurrentDevice.Type;
            PriceTextBox.Text = CurrentDevice.Price.ToString();
            InUseCheckBox.IsChecked = CurrentDevice.IsInUse;
        }

      
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем без сохранения
            DialogResult = false;
            Close();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Переносим данные из полей ввода обратно в объект
            CurrentDevice.Name = NameTextBox.Text;
          //  CurrentDevice.Type = TypeTextBox.Text;

            // Пытаемся безопасно распарсить число (в следующей практике добавим валидацию ошибок)
            if (decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                CurrentDevice.Price = price;
            }

            CurrentDevice.IsInUse = InUseCheckBox.IsChecked == true;

            // Говорим окну, что результат успешный, и закрываем его
            DialogResult = true;
            Close();
        }

        //метод для конвертации значения перечислени в словарь
        private Dictionary<TEnum, string> ToDictionary<TEnum>() where TEnum : struct, Enum
        {
            return Enum.GetValues<TEnum>()
                .ToDictionary(e => e, e => e.ToString());
        }

    }
}
