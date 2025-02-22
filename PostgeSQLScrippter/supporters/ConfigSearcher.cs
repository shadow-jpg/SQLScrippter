using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SqlScrippter
{
    internal class ConfigSearcher
    {
        string configFile = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); // Current location of library
        public ConfigSearcher() { }

        public List<string> FindConfigFiles(string directoryPath, string searchPattern = "config.*")
        {
            var configFiles = new List<string>();

            // Проверяем, существует ли директория
            if (!Directory.Exists(directoryPath))
            {
                throw new DirectoryNotFoundException($"Директория не найдена: {directoryPath}");
            }

            // Рекурсивно ищем файлы по шаблону
            SearchDirectory(directoryPath, searchPattern, configFiles);

            return configFiles;
        }

        // Вспомогательный метод для рекурсивного поиска
        private void SearchDirectory(string directoryPath, string searchPattern, List<string> configFiles)
        {
            try
            {
                // Ищем файлы по шаблону в текущей директории
                foreach (var file in Directory.GetFiles(directoryPath, searchPattern))
                {
                    configFiles.Add(file);
                }

                // Рекурсивно ищем в поддиректориях
                foreach (var subDirectory in Directory.GetDirectories(directoryPath))
                {
                    SearchDirectory(subDirectory, searchPattern, configFiles);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Обработка ошибок доступа (например, если нет прав на чтение директории)
                Console.WriteLine($"Нет доступа к директории: {directoryPath}");
            }
            catch (Exception ex)
            {
                // Обработка других исключений
                Console.WriteLine($"Ошибка при поиске в директории {directoryPath}: {ex.Message}");
            }
        }
    }
}
