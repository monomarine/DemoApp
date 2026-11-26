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
            win.Show();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}