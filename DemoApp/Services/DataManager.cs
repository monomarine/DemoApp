using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace DemoApp.Services
{
    internal static class DataManager
    {
        //настройки сериализатора
        private static readonly JsonSerializerOptions _options = new JsonSerializerOptions()
        {
            WriteIndented = true, //поддержка перехода на новую строку
            //поддержка кириллицы
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };


        public static void SaveData<T>(IEnumerable<T> data, string path)
        {
            try
            {
                //сериализируем входную коллекцию объектов  в json строку
                string jsonString = JsonSerializer.Serialize(data, _options); 
                File.WriteAllText(path, jsonString); //записываем полученную строку в файл
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка сохранения файла " + ex.Message);
            }

        }

        public static IEnumerable<T> LoadData<T>(string path)
        {
            try
            {
                //если нет файла сохранения - то возвращаем пустой список
                if (!File.Exists(path))
                    return new List<T>();

                //чтение файла и сохранение всего содержимого в переменную jsonString
                string jsonString = File.ReadAllText(path);
                //десериализация строки к типу List<T> 
                var data = JsonSerializer.Deserialize<List<T>>(jsonString, _options);

                //если десериализация успешна - возвращается data если нет - то пустой список
                return data ?? new List<T>(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных {path}");
                return new List<T>();
            }
        }
    }
}
