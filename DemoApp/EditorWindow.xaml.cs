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
        public Device CurrentDevice { get; set; } //открытие свойство окна для редактирования записи
        public EditorWindow(Device device)
        {
            InitializeComponent();

            // Если объект пустой (нажали "Добавить"), создаем новый экземпляр
            // Если передали существующий (нажали "Изменить"), работаем с ним
            CurrentDevice = device ?? new Device();

            // Заполняем текстовые поля текущими значениями
            NameTextBox.Text = CurrentDevice.Name;
            TypeTextBox.Text = CurrentDevice.Type.ToString();
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
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(NameTextBox.Text)) //проверка корректности названия
            {
                NameTextBox.BorderBrush = Brushes.Red; // рамка красная
                isValid = false;
            }

            else if (string.IsNullOrWhiteSpace(TypeTextBox.Text)) //проверка корректности типа устройства
            {
                TypeTextBox.BorderBrush = Brushes.Red;
                isValid = false;
            }

            //проверка корректности цены (возможно преобразование в decimal)
            else if (!Decimal.TryParse(PriceTextBox.Text, out decimal price))
            {
                PriceTextBox.BorderBrush = Brushes.Red;
                isValid = false;
            }

            //во всех остальных случаях ввод корректный - сбрасываем сцета рамок и сохраняем данные
            else
            {
                PriceTextBox.ClearValue(Border.BorderBrushProperty);
                TypeTextBox.ClearValue(Border.BorderBrushProperty);
                NameTextBox.ClearValue(Border.BorderBrushProperty);

                try
                {
                    CurrentDevice.Name = NameTextBox.Text;
                    CurrentDevice.Type = StringToEnum(TypeTextBox.Text);
                    //decimal.Parse, который сам выбросит FormatException, если текст содержит буквы
                    CurrentDevice.Price = decimal.Parse(PriceTextBox.Text);
                    CurrentDevice.IsInUse = InUseCheckBox.IsChecked == true;

                    // Говорим окну, что результат успешный, и закрываем его
                    DialogResult = true;
                    Close();
                }

                catch(FormatException) // Ловим ошибку конвертации числа
                {
                    PriceTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show("Поле 'Цена' должно содержать только числа.", 
                        "Ошибка формата", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                catch (ArgumentException ex) // Ловим нашу собственную ошибку из сеттера модели
                {
                    PriceTextBox.BorderBrush = Brushes.Red;
                    MessageBox.Show(ex.Message, "Логическая ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex) // Глобальный перехватчик любых других непредвиденных ошибок
                {
                    MessageBox.Show($"Произошла неизвестная ошибка: {ex.Message}", 
                        "Критическая ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }           
        }

        private DeviceType StringToEnum(string text)
        {
            switch(text.ToLower())
            {
                case "Персональный компьютер": return DeviceType.PC; 
                case "Ноутбук": return DeviceType.Laptop; 
                case "Телефон": return DeviceType.Phone; 
                case "Принтер": return DeviceType.Printer; 
                case "Не установлен": return DeviceType.Unknown; 
                default: return DeviceType.Unknown; 
            }
        }
    }
}
