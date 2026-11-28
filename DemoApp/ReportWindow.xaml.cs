using DemoApp.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DemoApp
{
    /// <summary>
    /// Логика взаимодействия для ReportWindow.xaml
    /// </summary>
    public partial class ReportWindow : Window
    {
        private readonly List<Device> _data;
        public ReportWindow(List<Device> data)
        {
            InitializeComponent();
            _data = data;
        }

        private void SaveReportButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog()
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                FileName = "Report.txt" ,
                DefaultExt = "txt",
                Filter = "Текстовые файлы (*.txt)|*.txt|Все файлы (*.*)|*.*",
                AddExtension = true,            
                OverwritePrompt = true         
            };

            if(sfd.ShowDialog() == true)
            {
                 File.WriteAllText(sfd.FileName, ReportTextBlock.Text);
            }
        }

        private void CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(_data is null)
            {
                MessageBox.Show("Нет данных для формирования отчета");
                return;
            }
            TimeStampTextBlock.Text = DateTime.Now.ToString();
            StringBuilder report = new StringBuilder();
            report.AppendLine($"Всего устройств на балансе - {_data.Count()}");
            report.AppendLine($"В пользовании - {_data.Where(s => s.IsInUse).Count()}");
            report.AppendLine();
            report.AppendLine($"Общая стоимость устройв в пользовании - {_data.Where(s => s.IsInUse).Sum(s => s.Price)}");
            report.AppendLine($"Общая стоимость устройв на хранении - {_data.Where(s => !s.IsInUse).Sum(s => s.Price)}");


            ReportTextBlock.Text = report.ToString();

        }
    }
}
