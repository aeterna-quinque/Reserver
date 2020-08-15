using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Reserver
{
    static class Copier
    {
        public static void Copy()
        {
            #region Получение данных из файла настроек
            string path = File.ReadAllText(@"..\..\Settings.json");
            JObject obj = JObject.Parse(path);
            #endregion

            #region Определение путей и создание папки в директории назначения
            string dest = obj["destination"].ToString();
            string timestamp = $"{DateTime.Now.ToShortDateString()}" +
                $"_{DateTime.Now.ToLongTimeString()}";
            string dest_path = $@"{dest}\{timestamp.Replace(':', '.')}";
            Directory.CreateDirectory(dest_path);
            #endregion

            #region Обход исходных папок и копирование документов
            string fileName,
                destFile,
                temp;
            foreach (var item in obj["source"])
            {
                temp = $@"{item.Value<string>("folder").ToString()}";
                string[] files = Directory.GetFiles(temp);
                foreach (string s in files)
                {
                    fileName = Path.GetFileName(s);
                    destFile = Path.Combine(dest_path, fileName);
                    File.Copy(s, destFile);
                }
            }
            #endregion
        }
    }
}
