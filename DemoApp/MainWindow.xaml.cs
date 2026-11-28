using DemoApp.Models;
using DemoApp.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string filePath = "devices.json"; // Файл сохранится рядом с .exe в папке bin/Debug

        //хранение всех загруженных объектов
        private List<Device> _allDevices = new(); 

        //список отображаемых объектов (для фильтрации)
        private ObservableCollection<Device> _displayedDevices = new(); 
        public MainWindow()
        {
            InitializeComponent();
            MainDataGrid.ItemsSource = _displayedDevices;
            LoadData();


        }

        private void LoadData()
        {
            _allDevices = DataManager.LoadData<Device>(filePath).ToList();
            UpdateDisplayedDevices(_allDevices);
            
        }

        //обновление отображаемого списка - вспомогательный при сбросе и загрузке
        private void UpdateDisplayedDevices(IEnumerable<Device> data)
        {
            _displayedDevices.Clear();
            foreach (Device device in data) 
                _displayedDevices.Add(device);
        }

        private void SerachTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SerachTextBox.Text.ToLower();

            // если поле поиска пусто 
            if(string.IsNullOrWhiteSpace(searchText))
            {
                // то отображаем все элементы из основного списка
                UpdateDisplayedDevices(_allDevices);
                return;
            }

            //фильтрация данных с помощью LINQ
            //ищем только те объекты у которых свойство Name содержит введенный текст

            var filterResult = _allDevices.Where(d => d.Name.ToLower().Contains(searchText));
            UpdateDisplayedDevices(filterResult);
        }


        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

            var win = new EditorWindow(null);
            if  (win.ShowDialog() == true)
            {
                _allDevices.Add(win.CurrentDevice);
                _displayedDevices.Add(win.CurrentDevice);

                DataManager.SaveData<Device>(_allDevices, filePath );
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItem is Device selectedDevice)
            {
                var win = new EditorWindow(selectedDevice);
                if (win.ShowDialog() == true)
                {
                    DataManager.SaveData<Device>(_allDevices, filePath);
                }

            }
            else
                MessageBox.Show("Выберите запись для редактирования");
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItem is Device selectedDevice)
            {
                // Спрашиваем подтверждение
                var result = MessageBox.Show($"Вы уверены, что хотите удалить '{selectedDevice.Name}'?",
                                             "Подтверждение удаления",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    _allDevices.Remove(selectedDevice);
                    _displayedDevices.Remove(selectedDevice); // Убираем из интерфейса

                    DataManager.SaveData(_allDevices, filePath); // Сохраняем в JSON
                }
            }
        }

        private void ReportButton_Click(object sender, RoutedEventArgs e)
        {
            var win = new ReportWindow(_allDevices);
            win.Show();
        }
    }
}