using System;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Reserver
{
    static class Copier
    {
        private static string[] PathsGetter()
        {
            string path = File.ReadAllText(@"..\..\Settings.json");
            JObject obj = JObject.Parse(path);
            string[] sources = obj["source"].ToString().Split('|');
            string[] paths = new string[sources.Length + 1];
            paths[0] = obj["destination"].ToString();

            if (paths[0] == String.Empty)
            {
                Console.WriteLine("Отсутствует путь к папке назначения.\nВведите путь:");
                paths[0] = Console.ReadLine();
                obj["destination"] = paths[0];
            }

            if ((paths.Length >= 2) && (paths[0] != String.Empty) && (sources[0] != String.Empty))
            {
                for (int i = 0; i < sources.Length; i++)
                    paths[i + 1] = sources[i];
                Console.WriteLine($"Папка назначения:{paths[0]}");
                for (int i = 1; i < paths.Length; i++)
                    Console.WriteLine($"Папка-источник №{i}:{sources[i - 1]}");
                Console.WriteLine("Желаете переписать пути?\n1 - Да\t0 - Нет");
                byte ans = byte.Parse(Console.ReadLine());
                if (ans == 1) paths = PathsRewriter(paths, obj);
            }

            if (sources[0] != String.Empty)
                for (int i = 0; i < sources.Length; i++)
                    paths[i + 1] = sources[i];
            else
            {
                Console.WriteLine("Отсутствуют пути к source-папкам.\nВведите количество путей:");
                int s_num = int.Parse(Console.ReadLine());
                while (s_num < 1)
                {
                    Console.WriteLine("Вы ввели недопустимое количество путей.\nВведите новое значение:");
                    s_num = int.Parse(Console.ReadLine());
                }
                paths = new string[s_num + 1];
                Console.WriteLine("Введите пути папок-источников:");
                string source_to_obj = "";
                for (int i = 0; i < s_num; i++)
                {
                    Console.WriteLine($"{i + 1}:");
                    paths[i + 1] = Console.ReadLine();
                    source_to_obj += $"{paths[i + 1]}|";
                }
                obj["source"] = source_to_obj.Remove(source_to_obj.Length - 1);
            }

            FileStream fs = File.Create(@"..\..\Settings.json");
            byte[] array = System.Text.Encoding.Default.GetBytes(obj.ToString());
            fs.Write(array, 0, System.Text.Encoding.UTF8.GetByteCount(obj.ToString()));
            fs.Close();


            return paths;
        }

        private static string[] PathsRewriter(string[] paths, JObject obj)
        {
            Console.WriteLine("Желаете переписать пути к источникам?\n1 - Да\t0 - Нет");
            byte ans = byte.Parse(Console.ReadLine());
            if (ans == 1)
            {
                Console.WriteLine("Введите количество путей:");
                int s_num = int.Parse(Console.ReadLine());
                while (s_num < 1)
                {
                    Console.WriteLine("Вы ввели недопустимое количество путей.\nВведите новое значение:");
                    s_num = int.Parse(Console.ReadLine());
                }
                paths = new string[s_num + 1];
                Console.WriteLine("Введите пути папок-источников:");
                string source_to_obj = "";
                for (int i = 0; i < s_num; i++)
                {
                    Console.WriteLine($"{i + 1}:");
                    paths[i + 1] = Console.ReadLine();
                    source_to_obj += $"{paths[i + 1]}|";
                }
                obj["source"] = source_to_obj.Remove(source_to_obj.Length - 1);
            }

            Console.WriteLine("Желаете переписать путь к папке назначения?\n1 - Да\t0 - Нет");
            ans = byte.Parse(Console.ReadLine());
            if (ans == 1)
            {
                Console.WriteLine("Введите путь:");
                paths[0] = Console.ReadLine();
                obj["destination"] = paths[0];
            }

            FileStream fs = File.Create(@"..\..\Settings.json");
            byte[] array = System.Text.Encoding.Default.GetBytes(obj.ToString());
            fs.Write(array, 0, System.Text.Encoding.UTF8.GetByteCount(obj.ToString()));
            fs.Close();

            return paths;
        }

        public static void Copy()
        {
            string[] paths = PathsGetter();

            string timestamp = $"{DateTime.Now.ToShortDateString()}" +
                $"_{DateTime.Now.ToLongTimeString()}";
            string dest_path = $@"{paths[0]}\{timestamp.Replace(':', '.')}";
            Directory.CreateDirectory(dest_path);

            string fileName,
                destFile;
            for (int i=1; i<paths.Length; i++)
            {
                string[] files = Directory.GetFiles(paths[i]);
                foreach (string s in files)
                {
                    fileName = Path.GetFileName(s);
                    destFile = Path.Combine(dest_path, fileName);
                    File.Copy(s, destFile);
                }
            }

            Console.WriteLine("Копирование завершено.\nЖелаете продолжить работу?\n1 - Да\t0 - Нет");
            byte ans = byte.Parse(Console.ReadLine());
            if (ans == 1) Copy();
        }
    }
}
